using System.Text;
using DataTypesConsoleApp.Models;
using DataTypesConsoleApp.Utils;

namespace DataTypesConsoleApp.Services;

public class ClassGenerator
{
    private readonly DatabaseService _dbService;
    public ClassGenerator(DatabaseService dbService)
    {
        _dbService = dbService;
    }
    public async Task<string> GenerateClassCodeAsync(string tableName)
    {
        var columns = await _dbService.GetTableColumnsAsync(tableName);
        var sb = new StringBuilder();
        
        sb.AppendLine("using System;");

        if (columns.Any(c => c.IsIdentity) )
        {
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
        }
        
        sb.AppendLine(); 
        sb.AppendLine($"public class {tableName}");
        sb.AppendLine("{");

        foreach (var column in columns)
        {
            string csharpType = SqlTypeMapper.MapSqlToCSharp(column.SqlType, column.IsNullable);
            if (column.IsIdentity)
            {
                sb.AppendLine( "[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
            }
            
            sb.AppendLine($"    public {csharpType} {column.Name} {{ get; set; }}");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

}