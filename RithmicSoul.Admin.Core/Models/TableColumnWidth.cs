namespace RithmicSoul.Admin.Core.Models;

public class TableColumnWidth
{
    public string TableName { get; set; }
    public Dictionary<int, string> ColumnWidths { get; set; } = new();
}