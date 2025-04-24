using Dapper;
using InternManagementAPI.Models;
using System.Data;

namespace InternManagementAPI.Repositories;

public class TaskRepository
{
    private readonly IDbConnection _db;

    public TaskRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TaskModel>> GetAllAsync()
    {
        var sql = "SELECT * FROM Tasks";
        return await _db.QueryAsync<TaskModel>(sql);
    }

    public async Task<TaskModel?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Tasks WHERE Id = @Id";
        return await _db.QuerySingleOrDefaultAsync<TaskModel>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(TaskModel task)
    {
        var sql = @"
            INSERT INTO Tasks (Title, Description, InternId, AssignedDate, IsCompleted)
            VALUES (@Title, @Description, @InternId, @AssignedDate, @IsCompleted);
            SELECT CAST(SCOPE_IDENTITY() as int)";
        return await _db.ExecuteScalarAsync<int>(sql, task);
    }

    public async Task<bool> UpdateAsync(TaskModel task)
    {
        var sql = @"
            UPDATE Tasks 
            SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted 
            WHERE Id = @Id";
        var rows = await _db.ExecuteAsync(sql, task);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Tasks WHERE Id = @Id";
        var rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
