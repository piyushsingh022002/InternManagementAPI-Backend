using InternManagementAPI.DTOs;
using InternManagementAPI.Models;
using InternManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementAPI.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDTO dto)
    {
        // Hardcoded for now
        var users = new List<User>
        {
            new User { Id = 1, Username = "intern1", Password = "pass123", Role = "Intern" },
            new User { Id = 2, Username = "hr1", Password = "hrpass", Role = "HR" }
        };

        var user = users.SingleOrDefault(u => u.Username == dto.Username && u.Password == dto.Password);

        if (user == null) return Unauthorized("Invalid credentials");

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpGet("public")]
    public IActionResult PublicEndpoint()
    {
        return Ok("This is a public endpoint.");
    }

    [Authorize]
    [HttpGet("auth")]
    public IActionResult AuthenticatedEndpoint()
    {
        return Ok($"Hello {User.Identity?.Name}, you are authenticated.");
    }

     [Authorize(Roles = "Intern")]
    [HttpGet("intern")]
    public IActionResult InternOnlyEndpoint()
    {
        return Ok("Welcome, Intern! You can access intern-only resources.");
    }

    [Authorize(Roles = "HR")]
    [HttpGet("hr")]
    public IActionResult HrOnlyEndpoint()
    {
        return Ok("Welcome, HR! You can manage everything.");
    }

}
