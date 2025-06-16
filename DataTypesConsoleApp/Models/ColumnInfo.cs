namespace DataTypesConsoleApp.Models
{
    public class ColumnInfo(string name, string sqlType)
    {
        public string Name { get; set; } = name;
        public string SqlType { get; set; } = sqlType;
    }
}