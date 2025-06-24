using System.Data;
using DataTypesConsoleApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace DataTypesConsoleApp.Services;

public class DatabaseService
{
    private readonly MyDbContext _context;

    public DatabaseService(string connectionString)
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        _context = new MyDbContext(options);
        
    }
     public async Task<List<ColumnInfo>> GetTableColumnsAsync(string tableName)
    {
        // Получаем инфу из sp_columns + identity через sys.columns
        var sql = @$"
        SELECT 
            c.name AS COLUMN_NAME,
            t.name AS TYPE_NAME,
            c.is_nullable,
            c.is_identity
        FROM sys.columns c
        JOIN sys.types t ON c.user_type_id = t.user_type_id
        INNER JOIN sys.tables tb ON c.object_id = tb.object_id
        WHERE tb.name = @tableName";

        var param = new SqlParameter("@tableName", tableName);
        var result = new List<ColumnInfo>();

        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.Parameters.Add(param);
        await _context.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new ColumnInfo(
                reader["COLUMN_NAME"].ToString(),
                reader["TYPE_NAME"].ToString(),
                Convert.ToBoolean(reader["is_nullable"]),
                Convert.ToBoolean(reader["is_identity"])
            ));
        }
        await _context.Database.CloseConnectionAsync();
        return result;
    }

    public async Task<List<ProcedureParameter>> GetProcedureParametersAsync(string procName)
    {
        var sql = @$"
        SELECT 
            p.name, 
            t.name AS TYPE_NAME, 
            p.is_output,
            p.is_nullable
        FROM sys.parameters p
        JOIN sys.types t ON p.user_type_id = t.user_type_id
        WHERE p.object_id = OBJECT_ID(@procName)";

        var param = new SqlParameter("@procName", procName);
        var result = new List<ProcedureParameter>();

        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.Parameters.Add(param);
        await _context.Database.OpenConnectionAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new ProcedureParameter
            {
                Name = reader["name"].ToString().TrimStart('@'),
                SqlType = reader["TYPE_NAME"].ToString(),
                IsOutput = Convert.ToBoolean(reader["is_output"]),
                IsNullable = Convert.ToBoolean(reader["is_nullable"])
            });
        }
        await _context.Database.CloseConnectionAsync();
        return result;
    }
    
}