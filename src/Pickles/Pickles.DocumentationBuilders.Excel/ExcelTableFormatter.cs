//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExcelTableFormatter.cs" company="PicklesDoc">
//  Copyright 2011 Jeffrey Cameron
//  Copyright 2012-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using ClosedXML.Excel;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel
{
    public class ExcelTableFormatter
    {
        private const int TableStartColumn = 4;

        public void Format(IXLWorksheet worksheet, Table table, ref int row)
        {
            var startRow = row;
            var headerColumn = TableStartColumn;
            foreach (var cell in table.HeaderRow.Cells)
            {
                worksheet.Cell(row, headerColumn).Style.Font.SetBold();
                worksheet.Cell(row, headerColumn).Style.Font.SetItalic();
                worksheet.Cell(row, headerColumn).Style.Fill.SetBackgroundColor(XLColor.AliceBlue);
                worksheet.Cell(row, headerColumn++).Value = cell;
            }

            row++;

            foreach (var dataRow in table.DataRows)
            {
                var dataColumn = TableStartColumn;
                foreach (var cell in dataRow.Cells)
                {
                    worksheet.Cell(row, dataColumn++).Value = cell;
                }

                row++;
            }

            var lastRow = row - 1;
            var lastColumn = headerColumn - 1;

            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.TopBorder =
                XLBorderStyleValues.Thin;
            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.LeftBorder =
                XLBorderStyleValues.Thin;
            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.BottomBorder =
                XLBorderStyleValues.Thin;
            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.RightBorder =
                XLBorderStyleValues.Thin;
        }


        public void Format(IXLWorksheet worksheet, ExampleTable table, ref int row)
        {
            var startRow = row;
            var headerColumn = TableStartColumn;
            foreach (var cell in table.HeaderRow.Cells)
            {
                worksheet.Cell(row, headerColumn).Style.Font.SetBold();
                worksheet.Cell(row, headerColumn).Style.Font.SetItalic();
                worksheet.Cell(row, headerColumn).Style.Fill.SetBackgroundColor(XLColor.AliceBlue);
                worksheet.Cell(row, headerColumn++).Value = cell;
            }

            row++;

            foreach (var dataRow in table.DataRows)
            {
                var dataColumn = TableStartColumn;
                foreach (var cell in dataRow.Cells)
                {
                    worksheet.Cell(row, dataColumn++).Value = cell;
                }

                row++;
            }

            var lastRow = row - 1;
            var lastColumn = headerColumn - 1;

            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.TopBorder =
                XLBorderStyleValues.Thin;
            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.LeftBorder =
                XLBorderStyleValues.Thin;
            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.BottomBorder =
                XLBorderStyleValues.Thin;
            worksheet.Range(startRow, TableStartColumn, lastRow, lastColumn).Style.Border.RightBorder =
                XLBorderStyleValues.Thin;
        }
    }

}
