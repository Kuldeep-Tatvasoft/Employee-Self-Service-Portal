using System.Reflection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Employee_Self_Service_DAL.Excel;

public class ExportExcel
{
    public byte[] ExportToExcel<T>(List<T> data, string sheetName,string status,string searchQuery, Dictionary<string, string> columnMappings = null)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        {
            var worksheet = package.Workbook.Worksheets.Add(sheetName);
            var currentRow = 3;
            var currentCol = 1;

            currentCol += 2;
            // First row 
            worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 1].Merge = true;
            worksheet.Cells[currentRow, currentCol].Value =  "Status: ";
            FormatHeaderCells(worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 1]);

            currentCol += 2;
            worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 2].Merge = true;
            worksheet.Cells[currentRow, currentCol].Value = status;
            FormatValueCells(worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 2]);

            currentCol += 4;
            worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 1].Merge = true;
            worksheet.Cells[currentRow, currentCol].Value = "Search Text: ";
            FormatHeaderCells(worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 1]);

            currentCol += 2;
            worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 2].Merge = true;
            worksheet.Cells[currentRow, currentCol].Value = searchQuery;
            FormatValueCells(worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 2]);

            currentCol += 4;
            worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 1].Merge = true;
            worksheet.Cells[currentRow, currentCol].Value = "No. of Records: ";
            FormatHeaderCells(worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 1]);

            currentCol += 2;
            worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 2].Merge = true;
            worksheet.Cells[currentRow, currentCol].Value = data.Count;
            FormatValueCells(worksheet.Cells[currentRow, currentCol, currentRow + 1, currentCol + 2]);

            currentRow += 3;
            currentCol = 1;

            // Table headers
            int headingRow = currentRow + 2;
            int headingCol = 1;
            var properties = typeof(T).GetProperties();

            // Set headers
            foreach (var prop in properties)
            {
                var property = prop;
                var columnName = columnMappings != null && columnMappings.ContainsKey(property.Name)
                ? columnMappings[property.Name]
                : property.Name;
                if(property.Name != "StatusId"){
                    worksheet.Cells[headingRow, headingCol, headingRow, headingCol + 1].Merge = true;
                    worksheet.Cells[headingRow, headingCol].Value = columnName;
                    headingCol += 2;
                }
            }
            FormatHeaderCells(worksheet.Cells[headingRow, 1, headingRow, headingCol - 1]);

            // Populate data
            int row = headingRow + 1;
            foreach (var item in data)
            {
                int startCol = 1;
                foreach (var prop in properties)
                {   
                    if(prop.Name != "StatusId"){
                        worksheet.Cells[row, startCol, row, startCol + 1].Merge = true;
                        worksheet.Cells[row, startCol].Value = prop.GetValue(item)?.ToString();
                        startCol += 2;
                    }
                }
                FormatDataRow(worksheet.Cells[row, 1, row, startCol - 1], row);
                row++;
            }
            return package.GetAsByteArray();
        }
    }

    private static void FormatHeaderCells(ExcelRange cells)
    {
        cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cells.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#0F67B1 "));
        cells.Style.Font.Bold = true;
        cells.Style.Font.Color.SetColor(Color.White);
        cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        // cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        // cells.Style.Border.Right.Color.SetColor(Color.Black);
        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    }
    private static void FormatValueCells(ExcelRange cells)
    {
        cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cells.Style.Fill.BackgroundColor.SetColor(Color.White);
        cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    }
    private static void FormatDataRow(ExcelRange cells, int row)
    {   
        if(row%2 == 0){
        cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Color.SetColor(Color.Black);
        }
        // else{
        //     cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //     cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //     cells.Style.Border.Right.Color.SetColor(Color.LightGray);
        // }
        cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Color.SetColor(Color.LightGray);
        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    }
    
    //     var worksheet = package.Workbook.Worksheets.Add(sheetName);


    //     // Get properties of the type T
    //     var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
    //         .Where(p => p.CanRead && !p.GetGetMethod().IsVirtual) // Exclude navigation properties
    //         .ToList();

    //     // Set headers
    //     for (int i = 0; i < properties.Count; i++)
    //     {
    //         var property = properties[i];
    //         var columnName = columnMappings != null && columnMappings.ContainsKey(property.Name)
    //             ? columnMappings[property.Name]
    //             : property.Name;
    //         worksheet.Cells[1, i + 1].Value = columnName;
    //         worksheet.Cells[1, i + 1].Style.Font.Bold = true;
    //     }

    //     // Populate data
    //     for (int row = 0; row < data.Count; row++)
    //     {
    //         for (int col = 0; col < properties.Count; col++)
    //         {
    //             var value = properties[col].GetValue(data[row]);
    //             worksheet.Cells[row + 2, col + 1].Value = value?.ToString();
    //         }
    //     }

    //     // Auto-fit columns
    //     worksheet.Cells.AutoFitColumns();

    //     return package.GetAsByteArray();
}



