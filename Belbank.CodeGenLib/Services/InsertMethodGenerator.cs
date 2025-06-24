using System.Text;
using Belbank.CodeGenLib.Utils;

namespace Belbank.CodeGenLib.Services;

public class InsertMethodGenerator
{
    private readonly DatabaseService _dbService;

    public InsertMethodGenerator(DatabaseService dbService)
    {
        _dbService = dbService;
    }
    public async Task<string> GenerateInsertMethodAsync(string procedureName)
    {
        var parameters = await _dbService.GetProcedureParametersAsync(procedureName);
        var sb = new StringBuilder();

        sb.AppendLine("public void InsertData(");
        sb.AppendLine(string.Join(",\n", parameters.Select(p =>
            p.IsOutput
                ? $"    out {SqlTypeMapper.MapSqlToCSharp(p.SqlType, p.IsNullable)} {p.Name}"
                : $"    {SqlTypeMapper.MapSqlToCSharp(p.SqlType, p.IsNullable)} {p.Name}"
        )));
        sb.AppendLine(")");
        sb.AppendLine("{");
        sb.AppendLine("    using var context = new MyDbContext(...);");
        sb.AppendLine("    var sqlParams = new List<SqlParameter>();");

        foreach (var p in parameters)
        {
            string sqlDbType = $"SqlDbType.{SqlTypeMapper.MapSqlToSqlDbType(p.SqlType)}";
            string dir = p.IsOutput ? "ParameterDirection.Output" : "ParameterDirection.Input";
            string value = p.IsOutput ? "DBNull.Value" : p.Name;
            sb.AppendLine($@"    var param_{p.Name} = new SqlParameter(""@{p.Name}"", {value})");
            sb.AppendLine("    {");
            sb.AppendLine($"        SqlDbType = {sqlDbType},");
            sb.AppendLine($"        Direction = {dir}");
            sb.AppendLine("    };");
            sb.AppendLine($"    sqlParams.Add(param_{p.Name});");
        }

        sb.AppendLine();
        sb.AppendLine($@"    context.Database.ExecuteSqlRaw(""EXEC {procedureName} {string.Join(", ", parameters.Select(p => "@" + p.Name + (p.IsOutput ? " OUTPUT" : "")))}"", sqlParams.ToArray());");

        // Для output-параметров — возврат значения после ExecuteSqlRaw
        foreach (var p in parameters.Where(x => x.IsOutput))
        {
            sb.AppendLine($"    {p.Name} = ({SqlTypeMapper.MapSqlToCSharp(p.SqlType, p.IsNullable)})param_{p.Name}.Value;");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}