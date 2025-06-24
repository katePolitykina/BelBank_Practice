namespace DataTypesConsoleApp.Models;

public class ProcedureParameter
{
    public string Name { get; set; }
    public string SqlType { get; set; }
    public bool IsOutput { get; set; }
    public bool IsNullable { get; set; }
}