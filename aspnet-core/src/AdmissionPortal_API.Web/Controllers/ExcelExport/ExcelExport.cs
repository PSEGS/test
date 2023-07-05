using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    public class ExportController : Controller
    {
        public FileContentResult ExcelExport(dynamic records, List<ExportExcelModel> exportedColumn, string fileName = "Export", string sheetName = "Sheet1")
        {
            XLWorkbook workbook = new();

            // name of the sheet
            var workSheet = workbook.Worksheets.Add(sheetName);

            // Setting the properties of the first row
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            // Header of the Excel sheet
            int columnIndex = 1;
            foreach(ExportExcelModel item in exportedColumn)
            {
                workSheet.Cell(1, columnIndex).Value = item.ColumnLabel;
                columnIndex++;
            }

            // Inserting the data into excel
            int recordIndex = 2;
            foreach (var record in records)
            {
                int rowColumnIndex = 1;
                foreach (ExportExcelModel item in exportedColumn)
                {
                    var propertyInfo = record.GetType().GetProperty(item.Column);
                    var value = propertyInfo.GetValue(record, null);
                    workSheet.Cell(recordIndex, rowColumnIndex).Value = value;
                    rowColumnIndex++;
                }
                recordIndex++;
            }

            // By default, the column width is not set to auto fit for the content of the range, so we are using AutoFit() method here. 
            int adjustColumnIndex = 1;
            foreach (ExportExcelModel item in exportedColumn)
            {
                workSheet.Column(adjustColumnIndex).AdjustToContents();
                adjustColumnIndex++;
            }

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, contentType, fileName);
        }
    }
    public class ExportExcelModel
    {
        public string ColumnLabel { get; set; }
        public string Column { get; set; }
    }
}
