using System.Reflection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Employee_Self_Service_DAL.Excel;

public class ExportExcel
{
    public byte[] ExportToExcel<T>(List<T> data, string sheetName, Dictionary<string, string> columnMappings = null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Get properties of the type T
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && !p.GetGetMethod().IsVirtual) // Exclude navigation properties
                .ToList();

            // Set headers
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var columnName = columnMappings != null && columnMappings.ContainsKey(property.Name)
                    ? columnMappings[property.Name]
                    : property.Name;
                worksheet.Cells[1, i + 1].Value = columnName;
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }

            // Populate data
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    var value = properties[col].GetValue(data[row]);
                    worksheet.Cells[row + 2, col + 1].Value = value?.ToString();
                }
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
}


