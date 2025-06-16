namespace DataTypesConsoleApp.Utils;

public class SqlTypeMapper
{
    public static string MapSqlToCSharp(string sqlType)
    {
        return sqlType.ToLower() switch
        {
            "tinyint" => "byte",
            "smallint" => "short",
            "int" => "int",
            "bigint" => "long",
            "bit" => "bool",
            "float" => "double",
            "real" => "float",
            "date" => "DateTime",
            "time" => "TimeSpan",
            "datetime" => "DateTime",
            "char" => "string",
            "varchar" => "string",
            "text" => "string",
            "nchar" => "string",
            "nvarchar" => "string",
            "ntext" => "string",
            "binary" => "byte[]",
            "varbinary" => "byte[]",
            _ => "object"
        };
    }
}