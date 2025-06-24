using Microsoft.EntityFrameworkCore;

namespace DataTypesConsoleApp.Models
{
    [Keyless]
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string SqlType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }

        public ColumnInfo(string name, string sqlType, bool isNullable, bool isIdentity)
        {
            Name = name;
            SqlType = sqlType;
            IsNullable = isNullable;
            IsIdentity = isIdentity;
            
        }
    }
}