using System.Data;
using Dapper;
using InternManagementAPI.DTOs;
using InternManagementAPI.Models;

namespace InternManagementAPI.Repositories;

public class InternRepository
{
    private readonly IDbConnection _db;

    public InternRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Intern>> GetAllAsync()
    {
        var sql = "SELECT * FROM Interns";
        return await _db.QueryAsync<Intern>(sql);
    }

    public async Task<Intern?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Interns WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Intern>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(CreateInternDto dto)
    {
        var sql = @"INSERT INTO Interns (Name, Email, Phone, Department, JoinDate)
                    VALUES (@Name, @Email, @Phone, @Department, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() as int);";

        return await _db.ExecuteScalarAsync<int>(sql, dto);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInternDto dto)
    {
        var sql = @"UPDATE Interns 
                    SET Name = @Name, Phone = @Phone, Department = @Department
                    WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, new { dto.Name, dto.Phone, dto.Department, Id = id });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Interns WHERE Id = @Id";
        var rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
