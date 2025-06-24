using Belbank.CodeGenLib.Services;

class Program
{
    static readonly string connectionString = 
        "Server=192.168.1.100,1433;Database=belbank;User Id=SA;Password=MyStrongPass123;TrustServerCertificate=True;";
    
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        using var conn = new SqlConnection(connectionString);
        conn.Open();
       // Console.WriteLine("Введите имя таблицы:"); 
        
        const string tableName = "SampleTable";
        const string insertProcName = "sp_InsertSampleTable";
        
        SqlDatabaseService dbService = new SqlDatabaseService(connectionString);
        ClassGenerator classGenerator = new ClassGenerator(dbService);
        InsertMethodGenerator insertGenerator = new InsertMethodGenerator(dbService);
        
        try
        {
            Console.WriteLine($"// Class for table: {tableName}");
            string classCode = classGenerator.GenerateClassCode(tableName);
            Console.WriteLine(classCode);
        
            Console.WriteLine();
            Console.WriteLine($"// Insert method for procedure: {insertProcName}");
            string insertCode = insertGenerator.GenerateInsertMethod(insertProcName);
            Console.WriteLine(insertCode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR] {ex.Message}");
        }
    }
    
}


