using System.Text;

namespace DataTypesConsoleApp.Services;

public class InsertMethodGenerator
{
    private readonly SqlDatabaseService _dbService;

    public InsertMethodGenerator(SqlDatabaseService dbService)
    {
        _dbService = dbService;
    }

    public string GenerateInsertMethod(string procedureName)
    {
        var parameters = _dbService.GetProcedureParameters(procedureName);
        var sb = new StringBuilder();

        sb.AppendLine("public void InsertData(");
        sb.AppendLine(string.Join(",\n", parameters.Select(p =>
            $"    object {p.Name} // TODO: Map system_type_id to C# type"
        )));
        sb.AppendLine(")");
        sb.AppendLine("{");
        sb.AppendLine("    using var conn = new SqlConnection(_connectionString);");
        sb.AppendLine("    conn.Open();");
        sb.AppendLine($"    using var cmd = new SqlCommand(\"{procedureName}\", conn);");
        sb.AppendLine("    cmd.CommandType = CommandType.StoredProcedure;");
        sb.AppendLine();

        foreach (var p in parameters)
        {
            sb.AppendLine($"    cmd.Parameters.Add(new SqlParameter(\"@{p.Name}\", {p.Name}));");
        }

        sb.AppendLine();
        sb.AppendLine("    cmd.ExecuteNonQuery();");
        sb.AppendLine("}");

        return sb.ToString();
    }
}