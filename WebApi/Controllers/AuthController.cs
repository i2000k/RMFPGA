using Application.Interfaces;
using Application.Utils;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RfContext _dbContext;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(RfContext dbContext, IAuthService authService, ILogger<AuthController> logger)
        {
            _dbContext = dbContext;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register(StudentDto dto)
        {
            var exist = _dbContext.Students.Any(x => x.Email == dto.Email);

            if (exist)
            {
                _logger.LogError("Registering user: user with email already exist");
                return BadRequest($"User with email: {dto.Email} already exist");
            }

            AuthUtils.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new Student(
                dto.Email,
                passwordHash,
                passwordSalt,
                dto.Group,
                dto.FirstName,
                dto.SecondName,
                dto.Grade,
                dto.GradeYear
            );
            _dbContext.Students.Add(user);

            _dbContext.SaveChanges();
            _logger.LogInformation("Registering user: Success");

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto dto)
        {
            _logger.LogInformation("Login user");
            var student = _dbContext.Students.FirstOrDefault(x => x.Email == dto.Email);
            // var admin = _dbContext.Administrators.FirstOrDefault(x => x.Email == dto.Username);
            //
            // if (admin == null && student == null)
            // {
            //     return BadRequest("User not found.");
            // }

            // var user = student as User ?? admin;

            if (student is null)
            {
                _logger.LogError("Login user: user not found");
                return BadRequest("User not found.");
            }

            if (!AuthUtils.VerifyPasswordHash(dto.Password, student.PasswordHash, student.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }
            string token = _authService.CreateToken(student);

            var refreshToken = AuthUtils.GenerateRefreshToken();
            SetRefreshToken(refreshToken, student);
            var result = new AuthDto(student.Id, token, "Student");
            _logger.LogInformation($"Login user: {student.Id}");
            return Ok(result);
        }

        [HttpPost("refresh-token/{id:guid}")]
        public IActionResult RefreshToken(Guid id)
        {
            var user = _dbContext.Students.FirstOrDefault(x => x.Id == id);
            var refreshToken = Request.Cookies["refreshToken"];

            if (user is null)
            {
                return BadRequest("User was not found.");
            }

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }

            if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = _authService.CreateToken(user);
            var newRefreshToken = AuthUtils.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);
            _dbContext.SaveChanges();
            return Ok(token);
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            _dbContext.SaveChanges();
        }
    }
}
