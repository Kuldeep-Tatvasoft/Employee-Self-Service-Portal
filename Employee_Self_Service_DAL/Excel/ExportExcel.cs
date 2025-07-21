using System.Reflection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml.Style;
using System.Drawing;
using Employee_Self_Service.DAL.Attributes;

namespace Employee_Self_Service_DAL.Excel;

public class ExportExcel
{
    public byte[] ExportToExcel<T>(List<T> data, string sheetName, string status, string searchQuery, Dictionary<string, string> columnMappings = null)
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
            worksheet.Cells[currentRow, currentCol].Value = "Status: ";
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
            var properties = typeof(T).GetProperties()
            .Where(p => p.IsDefined(typeof(DisplayColumnAttribute), false))
            .Where(p => p.GetCustomAttribute<DisplayColumnAttribute>()?.IsVisible == true )
            .ToList();

            // Set headers
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<DisplayColumnAttribute>();
                worksheet.Cells[headingRow, headingCol, headingRow, headingCol + 1].Merge = true;
                worksheet.Cells[headingRow, headingCol].Value = attr.Name;
                headingCol += 2;
            }
            FormatHeaderCells(worksheet.Cells[headingRow, 1, headingRow, headingCol - 1]);

            // Populate data
            int row = headingRow + 1;
            foreach (var item in data)
            {
                int startCol = 1;
                foreach (var prop in properties)
                {
                    // if(prop.Name != "StatusId"){
                    worksheet.Cells[row, startCol, row, startCol + 1].Merge = true;
                    var value = prop.GetValue(item);
                    if (value is IEnumerable<string> list && !(value is string))
                    {
                        worksheet.Cells[row, startCol].Value = string.Join(", ", list);
                    }
                    else
                    {
                        worksheet.Cells[row, startCol].Value = value?.ToString();
                    }
                    startCol += 2;
                    // }
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
        cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Color.SetColor(Color.Black);
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
        if (row % 2 == 0)
        {
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Right.Color.SetColor(Color.Black);
        }

        cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Color.SetColor(Color.LightGray);
        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    }

    
}



