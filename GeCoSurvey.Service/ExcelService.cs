﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using GeCoSurvey.Domain;
using GeCoSurvey.Data.Infrastructure;

namespace GeCoSurvey.Service
{
    public class ExcelService
    {
        private readonly IRepository<Survey> reposSurvey;
        private readonly IRepository<Competenza> reposCompetenze;
        private readonly IUnitOfWork unitOfWork;

        private Worksheet worksheet;


        public ExcelService(IRepository<Survey> reposSurvey,
            IRepository<Competenza> reposCompetenze,
            IUnitOfWork unitOfWork)
        {
            this.reposSurvey = reposSurvey;
            this.reposCompetenze = reposCompetenze;
            this.unitOfWork = unitOfWork;
        }


        public bool CaricaSurvey()
        {
            string filepath = "D:\\survey1.xlsx";
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filepath, false))
            {
                IEnumerable<Sheet> sheets = spreadsheetDocument.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Autovalutazione");
                if (sheets.Count() == 0)
                {
                    throw new Exception("File non valido");
                }

                WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(sheets.First().Id);
                Worksheet worksheet = worksheetPart.Worksheet;
                WorkbookPart wbPart = spreadsheetDocument.WorkbookPart;

                //Creazione di una nuova survey
                Survey survey = new Survey();

                uint curRow = 4;
                bool eof = false;
                do
                {


                    //Prendo la domanda
                    string questionStr = GetCellValue(worksheet, spreadsheetDocument.WorkbookPart, "B" + curRow);

                    if (!string.IsNullOrEmpty(questionStr))
                    {
                        //Creo la question e gli associo il testo della domanda
                        Question question = new Question();
                        question.Testo = questionStr;

                        //Ricerco la competenza associata
                        Competenza competenza = reposCompetenze.Get(c => c.Titolo.ToUpper() == questionStr.ToUpper());
                        if (competenza != null)
                            question.CompetenzaId = competenza.Id;
                        else
                            throw new Exception("Non trovata competenza " + questionStr);

                        //Aggiungo quella nulla
                        question.Children.Add(new SubQuestion
                            {
                                Testo = "La padronanza della competenza non è valutabile",
                                LivelloConoscenzaId = 1
                            });


                        AddSubQuestion(worksheet, wbPart, "E" + curRow, 2, question);
                        AddSubQuestion(worksheet, wbPart, "G" + curRow, 3, question);
                        AddSubQuestion(worksheet, wbPart, "I" + curRow, 4, question);
                        AddSubQuestion(worksheet, wbPart, "K" + curRow, 5, question);

                        survey.Questions.Add(question);

                        curRow++;
                    }
                    else
                        eof = true;
                }
                while (!eof);

                survey.Name = "Prova";
                survey.Active = true;
                reposSurvey.Add(survey);
                unitOfWork.Commit();
            }

            return true;
        }

        private void AddSubQuestion(Worksheet worksheet, WorkbookPart wbPart, string cell, int livelloConoscenzaId, Question question)
        {
            string a1 = GetCellValue(worksheet, wbPart, cell);
            question.Children.Add(new SubQuestion
            {
                Testo = a1,
                LivelloConoscenzaId = livelloConoscenzaId
            });
        }

        

        private string GetCellValue(Worksheet worksheet, WorkbookPart wbPart, string targetCell)
        {
            uint targetRow = GetRowIndex(targetCell);

            string result = null;
            SharedStringTablePart sstPart = wbPart.GetPartsOfType<SharedStringTablePart>().First();
            SharedStringTable ssTable = sstPart.SharedStringTable;

            Row row = worksheet.Descendants<Row>().Single(r => r.RowIndex == targetRow);

            try
            {
                Cell cell = row.Descendants<Cell>().Single(c => targetCell.Equals(c.CellReference));

                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                {
                    result = ssTable.ChildElements[Convert.ToInt32(cell.CellValue.Text)].InnerText;
                }
                else
                {
                    if (cell.CellValue != null)
                    {
                        result = cell.CellValue.Text;
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }
        // Given a cell name, parses the specified cell to get the row index.
        private static uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }



        // Given a cell name, parses the specified cell to get the column name.
        private static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
    }
}