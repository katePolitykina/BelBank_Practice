using System.Data;
using DataTypesConsoleApp.Models;
using Microsoft.Data.SqlClient;

namespace DataTypesConsoleApp.Services;

public class SqlDatabaseService(string connectionString)
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public IEnumerable<ColumnInfo> GetTableColumns(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

        var columns = new List<ColumnInfo>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        using var cmd = new SqlCommand("sys.sp_columns", conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        cmd.Parameters.AddWithValue("@table_name", tableName);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            columns.Add(new ColumnInfo(reader["COLUMN_NAME"].ToString(),reader["TYPE_NAME"].ToString()));
        }

        return columns;
    }

     public IEnumerable<ProcedureParameter> GetProcedureParameters(string procedureName)
     {
         if (string.IsNullOrWhiteSpace(procedureName))
             throw new ArgumentException("Procedure name cannot be null or empty.", nameof(procedureName));

         var parameters = new List<ProcedureParameter>();

         using var conn = new SqlConnection(_connectionString);
         conn.Open();

         string sql = "SELECT name, system_type_id FROM sys.parameters WHERE object_id = OBJECT_ID(@procName)";
         using var cmd = new SqlCommand(sql, conn);
         cmd.Parameters.AddWithValue("@procName", procedureName);

         using var reader = cmd.ExecuteReader();
         while (reader.Read())
         {
             parameters.Add(new ProcedureParameter
             {
                 Name = reader["name"].ToString().TrimStart('@'),
                 SqlTypeId = (byte)reader["system_type_id"]
             });
         }

         return parameters;
     }
    
}