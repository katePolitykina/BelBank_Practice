using System.Data;

namespace Belbank.CodeGenLib.Utils;

public class SqlTypeMapper
{
    public static string MapSqlToCSharp(string sqlType,  bool isNullable)
    {
        string result = sqlType.ToLower() switch
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
        if (result != "string" && result != "byte[]" && isNullable)
            result += "?";
        return result;
    }
    public static SqlDbType MapSqlToSqlDbType(string sqlType)
    {
        return sqlType.ToLower() switch
        {
            "tinyint" => SqlDbType.TinyInt,
            "smallint" => SqlDbType.SmallInt,
            "int" => SqlDbType.Int,
            "bigint" => SqlDbType.BigInt,
            "bit" => SqlDbType.Bit,
            "float" => SqlDbType.Float,
            "real" => SqlDbType.Real,
            "date" => SqlDbType.Date,
            "time" => SqlDbType.Time,
            "datetime" => SqlDbType.DateTime,
            "char" => SqlDbType.Char,
            "varchar" => SqlDbType.VarChar,
            "text" => SqlDbType.Text,
            "nchar" => SqlDbType.NChar,
            "nvarchar" => SqlDbType.NVarChar,
            "ntext" => SqlDbType.NText,
            "binary" => SqlDbType.Binary,
            "varbinary" => SqlDbType.VarBinary,
            _ => SqlDbType.Variant
        };
    }
}