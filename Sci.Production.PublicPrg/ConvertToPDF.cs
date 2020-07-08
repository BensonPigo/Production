using System;
using System.Runtime.InteropServices;

namespace Sci.Production.PublicPrg
{
    public static partial class ConvertToPDF
    {
        /// <summary>
        /// ExcelToPDF
        /// </summary>
        /// <param name="excelPath">ExcelPath</param>
        /// <param name="pdfPath">PDFPath</param>
        /// <returns>bool</returns>
        public static bool ExcelToPDF(string excelPath, string pdfPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Application application = null;
            Microsoft.Office.Interop.Excel.Workbook workBook = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.Application();
                application.Visible = false;
                workBook = application.Workbooks.Open(excelPath);
                workBook.ExportAsFixedFormat(targetType, pdfPath);
                result = true;
            }
            catch (Exception e)
            {
                MyUtility.Msg.WarningBox(e.Message, "Convert to  PDF failed.");
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                }

                if (application != null)
                {
                    application.Quit();
                }

                Marshal.ReleaseComObject(workBook);
                Marshal.ReleaseComObject(application);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return result;
        }

        /// <summary>
        /// WordToPDF
        /// </summary>
        /// <param name="wordPath">WordPath</param>
        /// <param name="pdfPath">PDFPath</param>
        /// <returns>bool</returns>
        public static bool WordToPDF(string wordPath, string pdfPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Word.WdExportFormat exportFormat = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
            Microsoft.Office.Interop.Word.Application application = null;
            Microsoft.Office.Interop.Word.Document document = null;

            try
            {
                application = new Microsoft.Office.Interop.Word.Application();
                application.Visible = false;
                document = application.Documents.Open(wordPath);
                document.ExportAsFixedFormat(pdfPath, exportFormat);
                result = true;
            }
            catch (Exception e)
            {
                MyUtility.Msg.WarningBox(e.Message, "Convert to  PDF failed.");
                result = false;
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                }

                if (application != null)
                {
                    application.Quit();
                }

                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(application);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return result;
        }
    }
}
