using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReportGeneration_Kazakov.Classes.Common
{
    public class Report
    {
        public static void Group(int IdGroup, ReportGeneration_Kazakov.Pages.Main Main)
        {
            SaveFileDialog SFD = new SaveFileDialog()
            {
                InitialDirectory = @"C:\",
                Filter = "Excel (*.xlsx)|*.xlsx"
            };
            SFD.ShowDialog();

            if (SFD.FileName != "")
            {
                var Group = Main.GrpCont.Find(x => x.Id == IdGroup);
                Excel.Application ExcelApp = null;
                Excel.Workbook Workbook = null;
                Excel.Worksheet Worksheet = null;

                try
                {
                    ExcelApp = new Excel.Application();
                    ExcelApp.Visible = false;
                    ExcelApp.DisplayAlerts = false; // отключаем диалоги Excel

                    Workbook = ExcelApp.Workbooks.Add(System.Type.Missing);
                    Worksheet = Workbook.ActiveSheet;

                    (Worksheet.Cells[1, 1] as Excel.Range).Value = $"Отчет о группе {Group.Name}";
                    Worksheet.Range[Worksheet.Cells[1, 1], Worksheet.Cells[1, 5]].Merge();
                    Styles(Worksheet.Cells[1, 1], 18);

                    (Worksheet.Cells[3, 1] as Excel.Range).Value = "Список группы:";
                    Worksheet.Range[Worksheet.Cells[3, 1], Worksheet.Cells[3, 5]].Merge();
                    Styles(Worksheet.Cells[3, 1], 12, Excel.XlHAlign.xlHAlignLeft);

                    (Worksheet.Cells[4, 1] as Excel.Range).Value = "№№";
                    Styles(Worksheet.Cells[4, 1], 12, Excel.XlHAlign.xlHAlignCenter, true);
                    (Worksheet.Cells[4, 1] as Excel.Range).ColumnWidth = 35.0f;

                    (Worksheet.Cells[4, 2] as Excel.Range).Value = "Кол-во не сданных практик";
                    Styles(Worksheet.Cells[4, 2], 12, Excel.XlHAlign.xlHAlignCenter, true);

                    (Worksheet.Cells[4, 3] as Excel.Range).Value = "Кол-во не сданных теоретических";
                    Styles(Worksheet.Cells[4, 3], 12, Excel.XlHAlign.xlHAlignCenter, true);

                    (Worksheet.Cells[4, 4] as Excel.Range).Value = "Отсутствовал на паре";
                    Styles(Worksheet.Cells[4, 4], 12, Excel.XlHAlign.xlHAlignCenter, true);

                    (Worksheet.Cells[4, 5] as Excel.Range).Value = "Опоздал";
                    Styles(Worksheet.Cells[4, 5], 12, Excel.XlHAlign.xlHAlignCenter, true);

                    int Height = 5;
                    List<StudentContext> Students = Main.StdCont.FindAll(x => x.IdGroup == IdGroup);

                    foreach (StudentContext Student in Students)
                    {
                        List<DisciplineContext> StudentDisciplines = Main.DisCont.FindAll(
                            x => x.IdGroup == Student.IdGroup);

                        int PracticeCount = 0;
                        int TheoryCount = 0;
                        int AbsenteeismCount = 0;
                        int LateCount = 0;

                        foreach (DisciplineContext StudentDiscipline in StudentDisciplines)
                        {
                            List<WorkContext> StudentWorks = Main.WrkCont.FindAll(
                                x => x.IdDiscipline == StudentDiscipline.Id);

                            foreach (WorkContext StudentWork in StudentWorks)
                            {
                                EvaluationContext Evaluation = Main.EvaCont.Find(x =>
                                    x.IdWork == StudentWork.Id &&
                                    x.IdStudent == Student.Id);

                                if ((Evaluation != null && (Evaluation.Value.Trim() == "" || Evaluation.Value.Trim() == "2"))
                                    || Evaluation == null)
                                {
                                    if (StudentWork.IdType == 1)
                                        PracticeCount++;
                                    else if (StudentWork.IdType == 2)
                                        TheoryCount++;
                                }

                                if (Evaluation != null && Evaluation.Lateness.Trim() != "")
                                {
                                    if (Convert.ToInt32(Evaluation.Lateness) == 90)
                                        AbsenteeismCount++;
                                    else
                                        LateCount++;
                                }
                            }
                        }

                        (Worksheet.Cells[Height, 1] as Excel.Range).Value = $"{Student.Lastname} {Student.Firstname}";
                        Styles(Worksheet.Cells[Height, 1], 12, Excel.XlHAlign.xlHAlignLeft, true);

                        (Worksheet.Cells[Height, 2] as Excel.Range).Value = PracticeCount.ToString();
                        Styles(Worksheet.Cells[Height, 2], 12, Excel.XlHAlign.xlHAlignCenter, true);

                        (Worksheet.Cells[Height, 3] as Excel.Range).Value = TheoryCount.ToString();
                        Styles(Worksheet.Cells[Height, 3], 12, Excel.XlHAlign.xlHAlignCenter, true);

                        (Worksheet.Cells[Height, 4] as Excel.Range).Value = AbsenteeismCount.ToString();
                        Styles(Worksheet.Cells[Height, 4], 12, Excel.XlHAlign.xlHAlignCenter, true);

                        (Worksheet.Cells[Height, 5] as Excel.Range).Value = LateCount.ToString();
                        Styles(Worksheet.Cells[Height, 5], 12, Excel.XlHAlign.xlHAlignCenter, true);

                        Height++;
                    }

                    Workbook.SaveAs(SFD.FileName, Excel.XlFileFormat.xlOpenXMLWorkbook);
                    System.Windows.MessageBox.Show("Файл сохранён успешно!", "Готово",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
                catch (Exception exp)
                {
                    System.Windows.MessageBox.Show($"Ошибка: {exp.Message}");
                }
                finally
                {
                    try
                    {
                        if (Workbook != null)
                        {
                            Workbook.Close(false);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(Workbook);
                            Workbook = null;
                        }
                        if (Worksheet != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(Worksheet);
                            Worksheet = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (ExcelApp != null)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            System.Threading.Thread.Sleep(1000);
                            ExcelApp.Quit();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);
                            ExcelApp = null;
                        }
                    }
                    catch { } // подавляем 0x800AC472 — файл уже сохранён, это не критично

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        public static void Styles(Excel.Range Cell,
            int FontSize,
            Excel.XlHAlign Position = Excel.XlHAlign.xlHAlignCenter,
            bool Border = false)
        {
            Cell.Font.Name = "Bahnschrift Light Condensed";
            Cell.Font.Size = FontSize;
            Cell.HorizontalAlignment = Position;
            Cell.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            if (Border)
            {
                var Borders = Cell.Borders;
                Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Cell.WrapText = true;
            }
        }
    }
}