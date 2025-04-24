using InternManagementAPI.DTOs;
using InternManagementAPI.Models;
using InternManagementAPI.Repositories;

namespace InternManagementAPI.Services;

public class InternService
{
    private readonly InternRepository _repository;

    public InternService(InternRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Intern>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Intern?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<int> CreateAsync(CreateInternDto dto)
    {
        return _repository.CreateAsync(dto);
    }

    public Task<bool> UpdateAsync(int id, UpdateInternDto dto)
    {
        return _repository.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }
}
