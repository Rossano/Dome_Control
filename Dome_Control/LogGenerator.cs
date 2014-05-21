﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace Dome_Control
{
    class LogGenerator
    {
        private string path;                        //  spreadsheet path
        private string name;                        //  spreadsheet filename
        private WorkbookPart wbPart = null;         //  WorkBookPart
        public SpreadsheetDocument doc = null;      //  Spreadsheet document
        private WorksheetPart wsSheet = null;       //  WorkSheetPart
        private Sheets sheets = null;               //  Spreasheet sheets
        private Sheet sheet = null;                 //  Spreadsheet active sheet

        public LogGenerator(string fn)
        {
            ////  get spreadsheet path from constructor
            //path = folder;                  
            ////  File name is based on date and time
            //DateTime now = DateTime.Now;
            ////  Construct the spreadsheet filename
            //string fn = string.Format("{0}\\report_{1}-{2}-{3}_{4}{5}{6}.xlsx", 
            //    path, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            if (File.Exists(fn))
            {
                doc = SpreadsheetDocument.Open(fn, true);
            }
            else
            {
                //
                //  Create the Spreadsheet document
                //
                doc = SpreadsheetDocument.Create(fn, SpreadsheetDocumentType.Workbook);
                //
                //  Add WoorkBookPart to the document
                //
                wbPart = doc.AddWorkbookPart();
                wbPart.Workbook = new Workbook();
                wbPart.Workbook.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                //
                //  Add WorkSheetPart to the WorkBookPart
                //
                wsSheet = wbPart.AddNewPart<WorksheetPart>();
                wsSheet.Worksheet = new Worksheet(new SheetData());
                wsSheet.Worksheet.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                //
                //  Add sheets to the WorkBook
                //
                sheets = doc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                //
                //  Append new sheet and associate it with the workbook
                //
                sheet = new Sheet() { Id = doc.WorkbookPart.GetIdOfPart(wsSheet), SheetId = 1, Name = "oi_th" };
                sheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                sheets.Append(sheet);            
                
                //CreatePackage(fn);

                //if (File.Exists(fn))
                //{                    
                //    TestXls();
                //}
            }
            
        }

        // Creates a SpreadsheetDocument.
        public void CreatePackage(string filePath)
        {
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(package);
                doc = package;
            }
        }

        // Adds child parts and generates content of the specified part.
        private void CreateParts(SpreadsheetDocument document)
        {
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            wbPart = workbookPart1;
            GenerateWorkbookPart1Content(workbookPart1);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("R3c8458136e1d4b6c");
            GenerateWorksheetPart1Content(worksheetPart1);
            wsSheet = worksheetPart1;

            SetPackageProperties(document);
        }

        // Generates content of workbookPart1.
        private void GenerateWorkbookPart1Content(WorkbookPart workbookPart1)
        {
            Workbook workbook1 = new Workbook();
            workbook1.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

            Sheets sheets1 = new Sheets();
            sheets = sheets1;

            Sheet sheet1 = new Sheet() { Name = "oi_th", SheetId = (UInt32Value)1U, Id = "R3c8458136e1d4b6c" };
            sheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            sheet = sheet1;
            sheets1.Append(sheet1);

            workbook1.Append(sheets1);

            workbookPart1.Workbook = workbook1;
        }

        // Generates content of worksheetPart1.
        private void GenerateWorksheetPart1Content(WorksheetPart worksheetPart1)
        {
            Worksheet worksheet1 = new Worksheet();
            worksheet1.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

            SheetData sheetData1 = new SheetData();
            
            worksheet1.Append(sheetData1);

            worksheetPart1.Worksheet = worksheet1;
        }

        private void SetPackageProperties(OpenXmlPackage document)
        {
        }


        //
        //  Simple Method to test the xlsx generation
        //
        private void TestXls()
        {                        
            string col = string.Empty;
            for (int row = 3; row < 13; row++)
            {
                col = "A" + row.ToString();
                int val = row - 2;
                upDateValue("oi_th", col, val.ToString(), 0, false);
                col = "B" + row.ToString();
                val = row * 2;
                upDateValue("oi_th", col, val.ToString(), 0, false);
            }
            closeDoc();            
        }

        //
        //  Close the spreadsheet
        //
        public void closeDoc()
        {
            doc.Close();
        }

        //
        //  Get the row of a given row index
        //
        private Row getRow (SheetData wsData, UInt32 rowIndex)
        {
            //
            //  Look for the rowIndex, if it exists return it otherwise create a new row and retourn it
            //
            var row = wsData.Elements<Row>().Where(r => r.RowIndex.Value == rowIndex).FirstOrDefault();
            if (row == null)
            {
                row = new Row();
                row.RowIndex = rowIndex;
                wsData.Append(row);
            }
            return row;
        }

        //
        //  Return the rowIndex from an Excel address
        //
        private UInt32 getRowIndex (string addr)
        {
            string rowPart;
            UInt32 l;
            UInt32 result = 0;

            for (int i = 0; i < addr.Length; i++)
            {
                if (UInt32.TryParse(addr.Substring(i, 1), out l))
                {
                    rowPart = addr.Substring(i, addr.Length - i);
                    if (UInt32.TryParse(rowPart, out l))
                    {
                        result = l;
                        break;
                    }
                }
            }
            return result;
        }

        //
        //  Add a new cell at the specified address
        //
        private Cell createCell (Row row, string addr)
        {
            Cell cellResult;
            Cell refCell = null;
            //
            //  Parse the selected row until found the wanted one
            //  or return a new cell
            //
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, addr, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }
            //
            //  Create a new Cell and insert it 
            //
            cellResult = new Cell();
            cellResult.CellReference = addr;
            row.InsertBefore(cellResult, refCell);
            return cellResult;
        }

        //
        //  Insert a new cell into the Worksheet at the given address
        //
        private Cell insertCellInWorkSheet (Worksheet ws, string addr)
        {
            //  Select the sheet data
            SheetData sheetData = ws.GetFirstChild<SheetData>();
            Cell cell = null;
            //  Get the row
            UInt32 rowNum = getRowIndex(addr);
            Row row = getRow(sheetData, rowNum);
            //
            //  If the selected cell already exists return it else create a new one
            //
            Cell refCell = row.Elements<Cell>().Where(c => c.CellReference.Value == addr).FirstOrDefault();
            if (refCell != null)
            {
                cell = refCell;
            }
            else
            {
                cell = createCell(row, addr);
            }
            return cell;
        }

        //
        //  Insert the string into the shared string table of the WorkBookPart
        //
        private int insertSharedStringItem (WorkbookPart wbPart, string Value)
        {
            //
            //  Look if the string already exists into the SharedStringTable, if it does return it
            //  else return a new one
            //
            int index = 0;
            bool found = false;            
            //
            // If the shared string table is missing, something's wrong.
            // Just return the index that you found in the cell.
            // Otherwise, look up the correct text in the table.
            //
            var stringTablePart = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            if (stringTablePart == null)
            {
                //  Create a new SharedString
                stringTablePart = wbPart.AddNewPart<SharedStringTablePart>();
            }
            
            var stringTable = stringTablePart.SharedStringTable;
            if (stringTable == null)
            {
                stringTable = new SharedStringTable();
            }
            //
            // Iterate through all the items in the SharedStringTable. 
            // If the text already exists, return its index.
            //
            foreach (SharedStringItem item in stringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == Value)
                {
                    found = true;
                    break;
                }
                index++;
            }
            //
            //  if not found append a new SharedString to the table
            //
            if (!found)
            {
                stringTable.AppendChild(new SharedStringItem(new Text(Value)));
                try
                {
                    stringTable.Save();
                }
                catch (Exception) { }                
            }

            return index;
        }

        //
        //  Write or Update a cell value of the given worksheet
        //
        public bool upDateValue (string sheetName, string addr, string Value, UInt32Value styleIndex, bool isString)
        {
            bool update = false;
            //
            //  Seek the worksheet into the workbook, if found return true else return false
            //
            Sheet sheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();
            if (sheet != null)
            {
                //
                //  If found select the worksheet and insert the cell at the given address
                //
                Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(sheet.Id))).Worksheet;
                Cell cell = insertCellInWorkSheet(ws, addr);
                //
                //  If content is a string inser the cell value and update ShareStringTable
                //  else insert it as number
                //
                if (isString)
                {
                    int stringIndex = insertSharedStringItem(wbPart, Value);
                    cell.CellValue = new CellValue(stringIndex.ToString());
                    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                }
                else
                {
                    cell.CellValue = new CellValue(Value);
                    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                }
                //  Update style
                if (styleIndex > 0) cell.StyleIndex = styleIndex;
                //
                //  Save the WorkSheet
                //
                ws.Save();
                update = true;
            }
            return update;
        }

    }
}
