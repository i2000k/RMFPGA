using Application.Interfaces;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StandsController : ControllerBase
{
    private readonly IStandsService _service;
    private readonly ILogger<StandsController> _logger;

    public StandsController(IStandsService service, ILogger<StandsController> logger)
    {
        _service = service;
        _logger = logger;
    }


    //Get All Stands
    [HttpGet, Authorize(Roles = "Admin")]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Getting all stands...");
        var result = _service.GetStands();
        _logger.LogInformation("Getting all stands: Success");
        if (result == null)
        {
            return NoContent();
        }
        
        return Ok(result);
    }

    // GET Available Stands
    [HttpGet("available"), Authorize(Roles = "Admin, Student")]
    public IActionResult GetAvailable()
    {
        _logger.LogInformation("Getting available stands");
        var result = _service.GetAvailable();
        _logger.LogInformation("Getting available stands: Success");
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    // GET Stand
    [HttpGet("{id:guid}"), Authorize(Roles = "Admin")]
    public IActionResult Get(Guid id)
    {
        _logger.LogInformation($"Getting stand ({id})");
        var result = _service.GetStand(id);
        _logger.LogInformation($"Getting stand ({id}): Success");
        if (result == null) 
            return NoContent();
        return Ok(result);
    }

    // Create Stand
    [HttpPost, Authorize(Roles = "Admin")]
    public IActionResult Create(StandDto dto)
    {
        _logger.LogInformation($"Creating stand...");
        var result = _service.CreateStand(dto);
        _logger.LogInformation($"Creating stand: Success");
        return Ok(result);
    }

    // Update Stand
    [HttpPut("{id:guid}"), Authorize(Roles = "Admin")]
    public IActionResult Update(StandDto dto, Guid id)
    {
        _logger.LogInformation($"Updating stand ({id})");

        var result = _service.UpdateStand(dto, id);
        if (result == null)
        {
            _logger.LogError($"Updating stand ({id}): Stand not found");
            return NotFound();
        }

        _logger.LogInformation($"Updating stand ({id}): Success");
        return Ok(result);
    }

    [HttpDelete("{id:guid}"), Authorize(Roles = "Admin")]
    public IActionResult Delete(Guid id)
    {
        _logger.LogInformation($"Deleting stand ({id})...");
        var result = _service.RemoveStand(id);
        if (result == null)
        {
            _logger.LogError($"Deleting stand ({id}): Stand not found");
            return NotFound();

        }
        _logger.LogInformation($"Deleting stand ({id}): Success");
        return Ok(result);
    }

    [HttpGet("enable/{id:guid}"), Authorize(Roles = "Admin")]
    public IActionResult EnableStand(Guid id)
    {
        _logger.LogInformation($"Enabling stand ({id})...");
        var result = _service.EnableStand(id);

        if (result is null) {
            _logger.LogError($"Stand {id} not found");
            return BadRequest();
        }

        if (result == false)
        {
            _logger.LogError($"Stand {id} is active");
            return BadRequest();
        }
        _logger.LogInformation($"Enabling stand ({id}): Success");
        return Ok();
    }

    [HttpGet("disable/{id:guid}"), Authorize(Roles = "Admin")]
    public IActionResult DisableStand(Guid id)
    {
        _logger.LogInformation($"Disabling stand ({id})...");
        var result = _service.DisableStand(id);

        if (result is null)
        {
            _logger.LogError($"Stand {id} not found");
            return BadRequest();
        }

        if (result == false)
        {
            _logger.LogError($"Stand {id} is active");
            return BadRequest();
        }
        _logger.LogInformation($"Disabling stand ({id}): Success");
        return Ok();
    }
}
    