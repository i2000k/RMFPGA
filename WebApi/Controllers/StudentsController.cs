using Application.Interfaces;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _service;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentsService service, ILogger<StudentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //Get All students
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Getting all students...");
            var result = _service.GetStudents();
            _logger.LogInformation($"Getting all students: Success");

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);

        }

        // GET student
        [HttpGet("{id:guid}"), Authorize(Roles = "Admin, Student")]
        public IActionResult Get(Guid id)
        {
            _logger.LogInformation($"Getting student ({id})...");
            var result = _service.GetStudent(id);
            if (result == null)
            {
                _logger.LogError($"Getting student ({id}): Not Found");
                return NotFound("No students with this id");
            }
            _logger.LogInformation($"Getting student ({id}): Success");

            return Ok(result);
        }

        // Create Student
        [HttpPost, Authorize(Roles = "Admin")]
        public IActionResult Create(StudentDto dto)
        {
            _logger.LogInformation($"Creating student...");
            var result = _service.CreateStudent(dto);
            _logger.LogInformation($"Creating student: Success");

            return Ok(result);
        }

        // Update Student
        [HttpPut("{id:guid}"), Authorize(Roles = "Admin")]
        public IActionResult Update(StudentDto dto, Guid id)
        {
            _logger.LogInformation($"Updating student ({id})...");
            var result = _service.UpdateStudent(dto, id);
            if (result == null)
            {
                _logger.LogError($"Updating student ({id}): Not Found");
                return NotFound("No students with this id");
            }
            _logger.LogInformation($"Updating student ({id}): Success");
            return Ok(result);
        }

        [HttpDelete("{id:guid}"), Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation($"Deleting student ({id})...");
            var result = _service.RemoveStudent(id);
            if (result == null)
            {
                _logger.LogError($"Deleting student ({id}): Not Found");
                return NotFound("No students with this id");
            }
            _logger.LogInformation($"Deleting student ({id}): Success");
            return Ok(result);
        }
    }
}
