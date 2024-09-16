using Application.Interfaces;
using Application.Utils;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly RfContext _dbContext;
    private readonly IAuthService _authService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(RfContext dbContext, IAuthService authService, ILogger<AdminController> logger)
    {
        _dbContext = dbContext;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public IActionResult Register(UserDto dto)
    {
        var exist = _dbContext.Administrators.Any(x => x.Email == dto.Email);

        if (exist)
        {
            _logger.LogError("Registering admin: admin with email already exist");
            return BadRequest($"Admin with email: {dto.Email} already exist");
        }

        AuthUtils.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new Administrator(dto.Email, passwordHash, passwordSalt);

        _dbContext.Administrators.Add(user);

        _dbContext.SaveChanges();
        _logger.LogInformation("Admin registration: Success");

        return Ok(user);
    }
    
    [HttpPost("login")]
    public IActionResult Login(UserDto dto)
    {
        var admin = _dbContext.Administrators.FirstOrDefault(x => x.Email == dto.Email);

        if (admin is null)
        {
            _logger.LogError("Admin not found");
            return BadRequest("User not found.");
        }

        if (!AuthUtils.VerifyPasswordHash(dto.Password, admin.PasswordHash, admin.PasswordSalt))
        {
            _logger.LogError("Wrong password");
            return BadRequest("Wrong password.");
        }
        string token = _authService.CreateToken(admin);

        var refreshToken = AuthUtils.GenerateRefreshToken();
        SetRefreshToken(refreshToken, admin);
        var result = new AuthDto(admin.Id, token, "Administrator");
        _logger.LogInformation("Admin login: Success");
        return Ok(result);
    }
    
    [HttpPost("refresh-token/{id:guid}")]
    public IActionResult RefreshToken(Guid id)
    {
        var user = _dbContext.Administrators.FirstOrDefault(x => x.Id == id);
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