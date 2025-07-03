namespace Employee_Self_Service_BAL.Interface;

public interface IExcelExportService
{
    // byte[] ExportToExcel<T>(List<T> data, string sheetName, Dictionary<string, string> columnMappings = null);
    public byte[] ExportToExcel<T>(List<T> data, string sheetName, Dictionary<string, string> columnMappings = null);
}
