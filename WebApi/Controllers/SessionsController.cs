using Application.Interfaces;
using Domain.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Text.Json;
using System.Text;
using System.IO.Compression;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
//using WebSocketSharp;
using System.Net.WebSockets;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Student")]
    public class SessionsController : ControllerBase
    {
        private readonly IConnectionService _service;
        private readonly RfContext _dbContext;
        private readonly ILogger<SessionsController> _logger;

        private string[] permittedExtensions = { ".sof" };
        public SessionsController(IConnectionService service, RfContext dbContext, ILogger<SessionsController> logger)
        {
            _service = service;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult StartConnection(StartConnectionDto dto)
        {
            _logger.LogInformation($"Starting connection... User: {dto.UserId}, Stand: {dto.StandId}");
            var result = _service.StartConnection(dto);
            if (result == null)
            {
                _logger.LogError($"Connection: User: {dto.UserId}, Stand: {dto.StandId} was not created");
                return BadRequest("Connection was not created");
            }
            _logger.LogInformation($"Starting connection... User: {dto.UserId}, Stand: {dto.StandId} SUCCESS");
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public IActionResult CloseConnection(Guid id)
        {
            _logger.LogInformation($"Closing connection({id})...");
            var result = _service.CloseConnection(id);
            if (result == null)
            {
                _logger.LogError($"Connections with id {id} not found.");
                return NotFound("No connections found");
            }
            _logger.LogInformation($"Closing connection({id}): Success");
            return Ok(result);
        }

        [HttpPost("{sessionId:guid}/sendData")]
        public IActionResult SendData(Guid sessionId, BoardDataDto dto)
        {
            _logger.LogInformation($"Sending Data to board... " +
                $"SessionId: {sessionId}, User: {dto.UserId}, Stand: {dto.StandId}, Data: {dto.Data}");
            var session = _dbContext.Sessions?.Where(s => (s.Id == sessionId &&
                                                        s.State == SessionState.Active &&
                                                        s.UserId == dto.UserId &&
                                                        s.StandId == dto.StandId)).FirstOrDefault();
            if (session is null)
            {
                _logger.LogError($"Send Data ({sessionId}): Session not exist");
                return BadRequest("Session not exist");

            }

            if (session.DesignFile is null)
            {
                _logger.LogError($"SendData ({sessionId}): Design file not loaded");
                return BadRequest("Load design file first");
            }

            //for (var i = 0; i < dto.Data.Count; i++)
            //{
            //    Console.WriteLine($"{i}: {dto.Data[i]}");
            //}

            try
            {
                HttpClient client = new HttpClient();
                var connUrl = _dbContext.Stands?
                    .Where(s => s.Id == dto.StandId)
                    .Select(s => s.ConnectionUrl)
                    .FirstOrDefault();

                if (connUrl is null)
                {
                    _logger.LogError($"SendData ({sessionId}): Incorrect stand Url");
                    return StatusCode(500);
                }

                _logger.LogInformation($"SendData ({sessionId}): Making request to stand");
                client.BaseAddress = new Uri(connUrl);
                client.DefaultRequestHeaders.Accept
                    .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("api/data/sendData/", content).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SendData ({sessionId}): Stand response error");
                return BadRequest(ex.Message);
            }
            _logger.LogInformation($"SendData ({sessionId}): Success");
            return Ok(dto.Data);
        }

        [HttpPost("{sessionId:guid}/designFile")]
        public IActionResult UploadFile(Guid sessionId, IFormFile formFile)
        {
            _logger.LogInformation($"UploadFile ({sessionId}): Starting uploading...");

            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                // The extension is invalid ...
                _logger.LogError($"UploadFile ({sessionId}): Incorrect file extension");
                return BadRequest("The file extension must be .sof");
            }
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length > 2097151)
                {
                    _logger.LogError($"UploadFile ({sessionId}): File size is too large");
                    return BadRequest("The file size is too large");
                }


                var fileContent = memoryStream.ToArray();

                var session = _dbContext.Sessions?.Find(sessionId);
                if (session is null)
                {
                    _logger.LogError($"UploadFile ({sessionId}): Session not exist");
                    return StatusCode(500, "No session available");
                }

                session.DesignFile = fileContent;
                _dbContext.SaveChanges();
                _logger.LogInformation($"UploadFile ({sessionId}): File saved to database");
                // Process file on StandHostService

                try
                {
                    _logger.LogInformation($"UploadFile ({sessionId}): Making request to stand");
                    HttpClient client = new HttpClient();
                    var connUrl = _dbContext.Stands?
                        .Where(s => s.Id == session.StandId)
                        .Select(s => s.ConnectionUrl)
                        .FirstOrDefault();

                    if (connUrl is null)
                    {
                        _logger.LogError($"UploadFile ({sessionId}): Incorrect stand Url");
                        return StatusCode(500, "Incorrect stand Url");
                    }

                    var dto = new ProcessFileDto(fileContent);

                    client.BaseAddress = new Uri(connUrl);
                    client.DefaultRequestHeaders.Accept
                        .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync("api/data/processFile/", content).Result;
                }
                catch (Exception)
                {
                    _logger.LogError($"UploadFile ({sessionId}): Error during processing file on server");
                    return StatusCode(500, "Error during processing file on server");
                }
            }
            _logger.LogInformation($"UploadFile ({sessionId}): Success");
            return Ok("File was successfully uploaded");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConnectToWebSocket()
        {
            //using (var ws = new WebSocket("ws://localhost:5002/Camera"))
            //{
            //    ws.OnOpen += (sender, e) =>
            //                      _logger.LogInformation("Opening web socket");

            //    ws.OnMessage += (sender, e) =>
            //                      _logger.LogInformation("Camera says: " + e.Data);

            //    ws.OnError += (sender, e) => {
            //        _logger.LogError("WebSocket Error: " + e.Message);;
            //    };

            //    ws.OnClose += (sender, e) => {
            //        _logger.LogInformation("Closing web socket");
            //    };

            //    ws.Connect();
            //    ws.Send("BALUS");
            //    Task.Delay(100);
            //}

            using (ClientWebSocket clientWebSocket = new ClientWebSocket())
            {
                Uri serviceUri = new Uri("ws://localhost:5001/send");
                var cTs = new CancellationTokenSource();
                cTs.CancelAfter(TimeSpan.FromSeconds(600));
                try
                {
                    await clientWebSocket.ConnectAsync(serviceUri, cTs.Token);
                    var n = 0;
                    while (clientWebSocket.State == WebSocketState.Open)
                    {
                        _logger.LogInformation("Enter message to send");
                        string message = Console.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> byteTosend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await clientWebSocket.SendAsync(byteTosend, WebSocketMessageType.Text, true, cTs.Token);
                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;
                            while (true)
                            {
                                ArraySegment<byte> byteReceived = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult response = await clientWebSocket.ReceiveAsync(byteReceived, cTs.Token);
                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                if (response.EndOfMessage)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (WebSocketException exception)
                {
                    _logger.LogError(exception.Message);
                    return BadRequest();
                }
            }

            return Ok();
        }
    }
}
