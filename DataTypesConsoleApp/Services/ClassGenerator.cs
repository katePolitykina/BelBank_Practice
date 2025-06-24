using System.Text;
using DataTypesConsoleApp.Utils;

namespace DataTypesConsoleApp.Services;

public class ClassGenerator
{
    private readonly SqlDatabaseService _dbService;

    public ClassGenerator(SqlDatabaseService dbService)
    {
        _dbService = dbService;
    }

    public string GenerateClassCode(string tableName)
    {
        var columns = _dbService.GetTableColumns(tableName);
        var sb = new StringBuilder();

        sb.AppendLine($"public class {tableName}");
        sb.AppendLine("{");

        foreach (var column in columns)
        {
            string csharpType = SqlTypeMapper.MapSqlToCSharp(column.SqlType);
            sb.AppendLine($"    public {csharpType} {column.Name} {{ get; set; }}");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

}