using InternManagementAPI.DTOs;
using InternManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InternController : ControllerBase
{
    private readonly InternService _service;

    public InternController(InternService service)
    {
        _service = service;
    }

    // GET: api/v1/intern
    [HttpGet]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> GetAll()
    {
        var interns = await _service.GetAllAsync();
        return Ok(interns);
    }

    // GET: api/v1/intern/5
    [HttpGet("{id}")]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> GetById(int id)
    {
        var intern = await _service.GetByIdAsync(id);
        if (intern == null) return NotFound();
        return Ok(intern);
    }

    // POST: api/v1/intern
    [HttpPost]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Create([FromBody] CreateInternDto dto)
    {
        var newId = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }

    // PUT: api/v1/intern/5
    [HttpPut("{id}")]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInternDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success) return NotFound();
        return NoContent();
    }

    // DELETE: api/v1/intern/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
