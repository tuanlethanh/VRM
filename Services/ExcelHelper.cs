using Aspose.Cells;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Onboarding.Common.Excel.Emums;

namespace VRM.Services
{
    public class ExcelHelper
    {

        public static Workbook GetWorkbook(string templateFilePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var aWorkbook = new Workbook(templateFilePath);
            return aWorkbook;
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <typeparam name="T">Objecttype</typeparam>
        /// <param name="pDatas"></param>
        /// <param name="aWorkbook"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static Workbook ExportExcel<T>(List<T> listDatas, Workbook aWorkbook, int sheetIndex, List<ColumnValue> listDataHeaders = null, List<ColumnValue> listDataFooters = null) where T : class
        {

            // TODO: DOC TEMPLATE
            List<string> listAttributes = new List<string>();

            try
            {
                //Style chung
                Aspose.Cells.Style style = null;
                //style.Font.Name = "Arial";
                //style.Font.Size = 10;
                //style.Font.Color = Color.Black;
                //style.ForegroundColor = Color.White;
                //style.Pattern = BackgroundType.Solid;
                //style.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
                //style.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
                //style.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
                //style.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);

                var styleHeader = aWorkbook.CreateStyle();
                styleHeader.Font.Name = "Arial";
                styleHeader.Font.Size = 10;
                styleHeader.Font.Color = Color.Black;
                styleHeader.ForegroundColor = Color.White;
                styleHeader.Pattern = BackgroundType.Solid;
                styleHeader.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.LightGray);
                styleHeader.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.LightGray);
                styleHeader.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.LightGray);
                styleHeader.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.LightGray);

                var styleFlag = new StyleFlag();
                styleFlag.CellShading = true;
                styleFlag.Font = true;
                styleFlag.Borders = true;
                styleFlag.HorizontalAlignment = true;
                styleFlag.VerticalAlignment = true;

                var sheet = aWorkbook.Worksheets[sheetIndex];
                int maxColumn = sheet.Cells.MaxDataColumn;
                int maxDataRow = sheet.Cells.MaxDataRow;

                //TODO: LAY TAT CA CAC COT CAN FILL DU LIEU
                int indexFillData = 0;

                for (int i = maxDataRow; i >= 0; i--)
                {
                    if (sheet.Cells[i, 0].StringValue.StartsWith("$"))
                    {
                        indexFillData = i;
                        for (int j = 0; j <= maxColumn; j++)
                        {
                            /*if (sheet.Cells[indexFillData, j].IsFormula)
                            {
                                listAttributes.Add(sheet.Cells[indexFillData, j].Formula);
                            }
                            else
                            {
                                if (sheet.Cells[indexFillData, j].StringValue.StartsWith("$"))
                                {
                                    listAttributes.Add(sheet.Cells[indexFillData, j].StringValue.Replace("$", ""));
                                }
                            }*/

                            if (sheet.Cells[indexFillData, j].StringValue.StartsWith("$"))
                            {
                                style = sheet.Cells[indexFillData, j].GetStyle();
                                listAttributes.Add(sheet.Cells[indexFillData, j].StringValue.Replace("$", ""));
                                sheet.Cells[indexFillData, j].Value = string.Empty;
                            }
                        }
                        break;
                    }
                }

                //TODO: LAY TAT CA CAC COT CAN FILL DU LIEU FOOTER/HEADER
                List<ColumnValue> columnHeaderValues = new List<ColumnValue>();
                List<ColumnValue> columnFooterValues = new List<ColumnValue>();
                for (int i = maxDataRow; i >= 0; i--)
                {
                    if (indexFillData != i)
                    {
                        for (int j = 0; j <= maxColumn; j++)
                        {
                            if (sheet.Cells[i, j].StringValue.StartsWith("$"))
                            {
                                var valueCanFill = new ColumnValue();
                                valueCanFill.FieldName = sheet.Cells[i, j].StringValue.Replace("$", "").ToUpper().Trim();
                                valueCanFill.ColIndex = j;
                                valueCanFill.RowIndex = i;
                                valueCanFill.IsValue = true;
                                valueCanFill.IsMerged = sheet.Cells[i, j].IsMerged;
                                valueCanFill.Range = sheet.Cells[i, j].GetMergedRange();
                                valueCanFill.Style = sheet.Cells[i, j].GetStyle();
                                if (i < indexFillData) // header
                                    columnHeaderValues.Add(valueCanFill);
                                else
                                {
                                    columnFooterValues.Add(valueCanFill);
                                }

                            }
                            else if (!string.IsNullOrEmpty(sheet.Cells[i, j].StringValue))
                            {
                                if (i > indexFillData)
                                {
                                    var valueCanFill = new ColumnValue();
                                    valueCanFill.FieldValue = sheet.Cells[i, j].StringValue.Trim();
                                    valueCanFill.ColIndex = j;
                                    valueCanFill.RowIndex = i;
                                    valueCanFill.IsValue = false;
                                    valueCanFill.IsMerged = sheet.Cells[i, j].IsMerged;
                                    valueCanFill.Range = sheet.Cells[i, j].GetMergedRange();
                                    valueCanFill.Style = sheet.Cells[i, j].GetStyle();
                                    columnFooterValues.Add(valueCanFill);
                                }
                            }
                        }
                    }
                }

                foreach (var col in columnFooterValues)
                {
                    sheet.Cells.DeleteRow(col.RowIndex);
                }
                //var numberStyle = aWorkbook.CreateStyle();
                //var cells = sheet.Cells;
                //TODO: FILL HEADER DATA
                if (listDataHeaders != null)
                {
                    styleFlag.Borders = true;
                    foreach (var columnVal in listDataHeaders)
                    {
                        var col = columnHeaderValues.FirstOrDefault(x => !string.IsNullOrEmpty(x.FieldName) && x.FieldName.Equals(columnVal.FieldName.ToUpper()));
                        if (col != null)
                        {
                           
                            if (columnVal.FieldValue != null)
                            {
                                switch (columnVal.ValueType)
                                {
                                    case ValueDataType.Number:
                                        Write2ExcelCellDecimal(aWorkbook, col, styleFlag, Convert.ToDecimal(columnVal.FieldValue ?? 0), TextAlignmentType.Right, sheetIndex);
                                        break;
                                    case ValueDataType.DateTime:
                                        Write2ExcelCellDate(aWorkbook, col, styleFlag, Convert.ToDateTime(columnVal.FieldValue), TextAlignmentType.Right, sheetIndex);
                                        break;
                                    case ValueDataType.Boolean:
                                        Write2ExcelCellString(aWorkbook, col, styleFlag, columnVal.FieldValue.ToString(), TextAlignmentType.Right, sheetIndex);
                                        break;
                                    case ValueDataType.String:
                                        Write2ExcelCellString(aWorkbook, col, styleFlag, columnVal.FieldValue.ToString(), TextAlignmentType.Left, sheetIndex);
                                        break;
                                }
                            }
                            else
                            {
                                Write2ExcelCellString(aWorkbook, col, styleFlag, columnVal.FieldValue.ToString(), TextAlignmentType.Left, sheetIndex);
                            }
                        }

                    }
                }
                // TODO: FILL DATA
                int rowData = indexFillData;
                int stt = 0;
                styleFlag.Borders = true;
                foreach (var dataItem in listDatas)
                {
                    stt++;
                    for (int i = 0; i < listAttributes.Count; i++)
                    {
                        // var cell = cells[0, i];
                        var colTemplate = new ColumnValue();
                        colTemplate.FieldValue = sheet.Cells[rowData, i].StringValue.Trim();
                        colTemplate.ColIndex = i;
                        colTemplate.RowIndex = rowData;
                        colTemplate.IsValue = false;
                        //colTemplate.IsMerged = sheet.Cells[rowData, i].IsMerged;
                        //colTemplate.Range = sheet.Cells[rowData, i].GetMergedRange();
                        colTemplate.Style = style;
                        if (!string.IsNullOrEmpty(listAttributes[i]))
                        {
                            

                            if (listAttributes[i].ToUpper().Equals("STT"))
                            {
                                Write2ExcelCellString(aWorkbook, colTemplate, styleFlag, stt.ToString(), TextAlignmentType.Center, sheetIndex);
                            }
                            else if (listAttributes[i].ToUpper().StartsWith("=")) // Formular
                            {
                                Write2ExcelCellFomula(aWorkbook, colTemplate, styleFlag, listAttributes[i].ToUpper(), TextAlignmentType.Left, sheetIndex);
                            }
                            else
                            {
                                var memberInfo = dataItem.GetType().GetProperty(listAttributes[i]);
                                if (memberInfo != null)
                                {
                                    var propertyInfo = memberInfo;
                                    {
                                        var type = propertyInfo.GetMethod;
                                        if (type != null)
                                        {
                                            var value = memberInfo.GetValue(dataItem, null);
                                            if (value == null)
                                            {
                                                Write2ExcelCellString(aWorkbook, colTemplate, styleFlag, "-", TextAlignmentType.Left, sheetIndex);
                                            }
                                            else
                                            {
                                                if (type.ReturnType == typeof(decimal) || type.ReturnType == typeof(int) ||
                                                type.ReturnType == typeof(double) || type.ReturnType == typeof(float))
                                                {
                                                    Write2ExcelCellDecimal(aWorkbook, colTemplate, styleFlag, Convert.ToDecimal(value ?? 0), TextAlignmentType.Right, sheetIndex);
                                                    // cells.Columns[cell.Column].ApplyStyle(numberStyle, new StyleFlag() { NumberFormat = true });

                                                }
                                                else if (type.ReturnType == typeof(DateTime))
                                                {
                                                    Write2ExcelCellDate(aWorkbook, colTemplate, styleFlag, Convert.ToDateTime(value), TextAlignmentType.Right, sheetIndex);
                                                }
                                                else
                                                {
                                                    Write2ExcelCellString(aWorkbook, colTemplate, styleFlag, value.ToString(), TextAlignmentType.Left, sheetIndex);
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            Write2ExcelCellString(aWorkbook, colTemplate, styleFlag, "-", TextAlignmentType.Left, sheetIndex);
                        }

                    }
                    rowData++;
                }

                //TODO: FILL FOOTER DATA

                styleFlag.Borders = true;
                if (listDataFooters != null)
                {

                    // write data
                    foreach (var columnVal in listDataFooters)
                    {
                        var col =
                            columnFooterValues.FirstOrDefault(
                                x => !string.IsNullOrEmpty(x.FieldName) && x.FieldName.Equals(columnVal.FieldName.ToUpper()) && x.IsValue);

                        // var cell = cells[0, col.ColIndex];
                        if (col != null)
                        {
                            int rowIndexCanWrite = col.RowIndex - indexFillData + rowData;
                            if (columnVal.FieldValue != null)
                            {
                                switch (columnVal.ValueType)
                                {
                                    case ValueDataType.Number:
                                        Write2ExcelCellDecimal(aWorkbook, col
                                                   , styleFlag, Convert.ToDecimal(columnVal.FieldValue ?? 0), TextAlignmentType.Center, sheetIndex);
                                        break;
                                    case ValueDataType.DateTime:
                                        Write2ExcelCellDate(aWorkbook, col, styleFlag, Convert.ToDateTime(columnVal.FieldValue), TextAlignmentType.Center, sheetIndex);
                                        break;
                                    case ValueDataType.Boolean:
                                        Write2ExcelCellString(aWorkbook, col, styleFlag, columnVal.FieldValue.ToString(), TextAlignmentType.Center, sheetIndex);
                                        break;
                                    case ValueDataType.String:
                                        Write2ExcelCellString(aWorkbook, col
                                                           , styleFlag, columnVal.FieldValue.ToString(), TextAlignmentType.Center, sheetIndex);
                                        break;
                                }
                            }
                            else
                            {
                                Write2ExcelCellString(aWorkbook, col, styleFlag, columnVal.FieldValue.ToString(), TextAlignmentType.Center, sheetIndex);
                            }
                        }

                    }


                }
                //write title
                var listTitles = columnFooterValues.Where(x => !x.IsValue).ToList();
                foreach (var columnValue in listTitles)
                {
                    columnValue.RowIndex = columnValue.RowIndex - indexFillData + rowData;
                    Write2ExcelCellFooterString(aWorkbook, columnValue, styleFlag, columnValue.FieldValue.ToString(), TextAlignmentType.Center, sheetIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}\n{1}", ex.Message, ex.StackTrace));
            }
            aWorkbook.CalculateFormula();
            return aWorkbook;
        }

        private static void Write2ExcelRangeText(Workbook aWorkbook, String pathfile, int y1, int x1, int range_y, int range_x,
            Style style, StyleFlag styleflag, string text)
        {
            Aspose.Cells.Range range = aWorkbook.Worksheets[0].Cells.CreateRange(y1, x1, range_y, range_x);
            range.Value = text;
            range.ApplyStyle(style, styleflag);
        }

        private static void Write2ExcelRange(Workbook aWorkbook, String pathfile, int y1, int x1, int range_y, int range_x,
            Style style, StyleFlag styleflag)
        {
            //Create a range of cells
            //bat dau tu diem 20,3 -> keo dai 22 keo rong 4 cells
            Aspose.Cells.Range range = aWorkbook.Worksheets[0].Cells.CreateRange(y1, x1, range_y, range_x);
            //range.Name = "MyRange";      
            //Apply the style to the range.
            range.Value = "1";
            range.ApplyStyle(style, styleflag);
        }

        private static void Write2ExcelCellString(Workbook aWorkbook, ColumnValue template, StyleFlag styleFlag,
            string value,
            TextAlignmentType align = TextAlignmentType.Left,
            int sheetIndex = 0)
        {
            //[row, column] -> set value  
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].Value = value;
            template.Style.VerticalAlignment = align;
            template.Style.HorizontalAlignment = align;
            template.Style.ShrinkToFit = true;
            styleFlag.WrapText = true;
            template.Style.IsTextWrapped = true;
            if (template.IsMerged)
            {
                aWorkbook.Worksheets[sheetIndex].Cells.Merge(template.Range.FirstRow, template.Range.FirstColumn, template.Range.RowCount, template.Range.ColumnCount);
                aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].PutValue(value);
            }
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].SetStyle(template.Style, styleFlag);
        }

        private static void Write2ExcelCellFooterString(Workbook aWorkbook, ColumnValue template, StyleFlag styleFlag,
            string value,
            TextAlignmentType align = TextAlignmentType.Left,
            int sheetIndex = 0)
        {
            //[row, column] -> set value  
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].Value = value;
            template.Style.VerticalAlignment = align;
            template.Style.HorizontalAlignment = align;
            template.Style.ShrinkToFit = true;
            styleFlag.WrapText = true;
            template.Style.IsTextWrapped = true;
            if (template.IsMerged)
            {
                aWorkbook.Worksheets[sheetIndex].Cells.Merge(template.RowIndex, template.Range.FirstColumn, template.Range.RowCount, template.Range.ColumnCount);
                aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].PutValue(value);
            }
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].SetStyle(template.Style, styleFlag);
        }
        private static void Write2ExcelCellDate(Workbook aWorkbook,ColumnValue template, StyleFlag styleFlag,
            DateTime value, TextAlignmentType align = TextAlignmentType.Right, int sheetIndex = 0)
        {
            //[row, column] -> set value  

            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].Value = value.ToShortDateString();
            template.Style.VerticalAlignment = align;
            template.Style.HorizontalAlignment = align;
            template.Style.ShrinkToFit = true;
            styleFlag.WrapText = true;
            template.Style.IsTextWrapped = true;
            template.Style.Custom = "dd/mmm/yyyy hh:mm:ss";
            if (template.IsMerged)
            {
                aWorkbook.Worksheets[sheetIndex].Cells.Merge(template.Range.FirstRow, template.Range.FirstColumn, template.Range.RowCount, template.Range.ColumnCount);
                aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].PutValue(value.ToShortDateString());
            }
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].SetStyle(template.Style, styleFlag);
        }

        private static void Write2ExcelCellDecimal(Workbook aWorkbook, ColumnValue template, StyleFlag styleFlag, 
            decimal value, TextAlignmentType align = TextAlignmentType.Right, int sheetIndex = 0)
        {
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].Value = value;

            template.Style.VerticalAlignment = align;
            template.Style.HorizontalAlignment = align;
            template.Style.ShrinkToFit = true;
            template.Style.Custom = "#,##0";
            styleFlag.WrapText = true;
            template.Style.IsTextWrapped = true;
            if (template.IsMerged)
            {
                aWorkbook.Worksheets[sheetIndex].Cells.Merge(template.Range.FirstRow, template.Range.FirstColumn, template.Range.RowCount, template.Range.ColumnCount);
                aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].PutValue(value);
            }
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].SetStyle(template.Style, styleFlag);
        }

        private static void Write2ExcelCellFomula(Workbook aWorkbook, ColumnValue template, StyleFlag styleFlag, 
            string value, TextAlignmentType align = TextAlignmentType.Right, int sheetIndex = 0)
        {
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].Value = value;
            template.Style.VerticalAlignment = align;
            template.Style.HorizontalAlignment = align;
            template.Style.ShrinkToFit = true;
            styleFlag.WrapText = true;
            template.Style.IsTextWrapped = true;
            //style.Custom = "#,##0";
            if (template.IsMerged)
            {
                aWorkbook.Worksheets[sheetIndex].Cells.Merge(template.Range.FirstRow, template.Range.FirstColumn, template.Range.RowCount, template.Range.ColumnCount);
                aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].PutValue(value);
            }
            aWorkbook.Worksheets[sheetIndex].Cells[template.RowIndex, template.ColIndex].SetStyle(template.Style, styleFlag);

        }

        private static void Write2ExcelCellHtml(Workbook aWorkbook, int y1, int x1, string htlm, Style style, StyleFlag styleFlag)
        {
            //set HTML       
            // "<ul><li>one</li><li>Two</li><li>Three</li><li>Four</li></ul>";
            aWorkbook.Worksheets[0].Cells[y1, x1].HtmlString = htlm;
            aWorkbook.Worksheets[0].Cells[y1, x1].SetStyle(style, styleFlag);
        }
    }
}
