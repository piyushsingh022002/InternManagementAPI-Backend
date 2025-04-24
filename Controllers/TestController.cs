using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementAPI.Controllers;

[ApiController]
[Route("api/v1/test")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public() => Ok("✅ This is a public endpoint.");

    [Authorize]
    [HttpGet("auth")]
    public IActionResult Authenticated() => Ok("✅ Authenticated access.");

    [Authorize(Roles = "Intern")]
    [HttpGet("intern")]
    public IActionResult InternOnly() => Ok("✅ Hello Intern.");

    [Authorize(Roles = "HR")]
    [HttpGet("hr")]
    public IActionResult HrOnly() => Ok("✅ Hello HR.");
}
