using InternManagementAPI.DTOs;
using InternManagementAPI.Models;
using InternManagementAPI.Repositories;

namespace InternManagementAPI.Services;

public class TaskService
{
    private readonly TaskRepository _taskRepo;

    public TaskService(TaskRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
    {
        return await _taskRepo.GetAllAsync();
    }

    public async Task<TaskModel?> GetTaskByIdAsync(int id)
    {
        return await _taskRepo.GetByIdAsync(id);
    }

    public async Task<int> CreateTaskAsync(CreateTaskDto dto)
    {
        var task = new TaskModel
        {
            Title = dto.Title,
            Description = dto.Description,
            InternId = dto.InternId,
            AssignedDate = DateTime.UtcNow,
            IsCompleted = false
        };

        return await _taskRepo.CreateAsync(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var existing = await _taskRepo.GetByIdAsync(id);
        if (existing == null) return false;

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.IsCompleted = dto.IsCompleted;

        return await _taskRepo.UpdateAsync(existing);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _taskRepo.DeleteAsync(id);
    }
}
