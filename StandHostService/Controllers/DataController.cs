using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StandHostService.ApplicationServices;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace StandHostService.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly SerialPortService _serialPortService;
        private readonly ProcessFileService _processFileService;
        private readonly ILogger<DataController> _logger;
        public DataController(
            SerialPortService serialPortService, 
            ProcessFileService processFileSerive,
            ILogger<DataController> logger)
        {
            _serialPortService = serialPortService;
            _processFileService = processFileSerive;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public IActionResult SendData(BoardDataDto dto)
        {
            
            if (dto.Data is null || dto.Data.Length != 8)
            {
                _logger.LogError("Incorrect SendData dto data format");
                return BadRequest("Error data format");
            }
            _logger.LogInformation("Sending data to board...");
            var result = _serialPortService.SendData(dto.Data);

            if (!result)
            {
                _logger.LogError("Data was not sended to board");
                return BadRequest("Data was not sent to board");
            }

            _logger.LogInformation("Sending data: Success");
            return Ok(dto);
        }

        [HttpPost("[action]")]
        public IActionResult ProcessFile(ProcessFileDto dto)
        {
            _logger.LogInformation("Processing file...");
            var result = _processFileService.ProcessFile(dto.Content);

            if (!result)
            {
                _logger.LogError("File was not uploaded to board");
                return BadRequest("File was not uploaded to board");
            }
            _logger.LogInformation("Processing file: Success");
            return Ok("File was successfully uploaded");

        }
    }
}
