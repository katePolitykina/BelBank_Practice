using System;
using System.Collections.Generic;
using System.Data;
using DataTypesConsoleApp.Services;
using Microsoft.Data.SqlClient;

class Program
{
    static readonly string connectionString = 
        "Server=192.168.1.105,1433;Database=belbank;User Id=SA;Password=MyStrongPass123;TrustServerCertificate=True;";
    
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        
        const string tableName = "SampleTable";
        const string insertProcName = "sp_InsertSampleTable";
        
        DatabaseService dbService = new DatabaseService(connectionString);
        ClassGenerator classGenerator = new ClassGenerator(dbService);
        InsertMethodGenerator insertGenerator = new InsertMethodGenerator(dbService);
        

        Console.WriteLine($"// Class for table: {tableName}");
        var classCode = await classGenerator.GenerateClassCodeAsync(tableName);
        Console.WriteLine(classCode);

        Console.WriteLine();
        Console.WriteLine($"// Insert method for procedure: {insertProcName}");
        var insertCode = await insertGenerator.GenerateInsertMethodAsync(insertProcName);
        Console.WriteLine(insertCode);

    }
    
}


