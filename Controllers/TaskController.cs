using InternManagementAPI.DTOs;
using InternManagementAPI.Models;
using InternManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternManagementAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [Authorize(Roles = "HR")]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "HR")]
    public async Task<ActionResult<TaskModel>> Get(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    [Authorize(Roles = "HR")] 
    public async Task<ActionResult> Create(CreateTaskDto dto)
    {
        var id = await _taskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "HR")]
    public async Task<ActionResult> Update(int id, UpdateTaskDto dto)
    {
        var updated = await _taskService.UpdateTaskAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "HR")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
