using System;
using System.IO;

namespace Sci.Production.Class
{
    public static class ExcelFileNameExtension
    {
        public const string Xlsm = ".xlsm";
        public const string Xlsx = ".xlsx";
    }

    public static class WordFileeNameExtension
    {
        public const string Docx = ".docx";
    }

    public static class PDFFileNameExtension
    {
        public const string PDF = ".pdf";
    }

    public static class MicrosoftFile
    {
        /// <summary>
        /// Get Microsoft File Name
        /// </summary>
        /// <param name="ProcessName">主檔名</param>
        /// <param name="NameExtension">副檔名，預設 xlsx</param>
        /// <returns>路徑+檔名+副檔名</returns>
        public static string GetName(string ProcessName, string NameExtension = ExcelFileNameExtension.Xlsx)
        {
            string fileName = ProcessName.Trim()
                                + DateTime.Now.ToString("_yyMMdd_HHmmssfff")
                                + NameExtension;
            return Path.Combine(Sci.Env.Cfg.ReportTempDir, fileName);
        }
    }
}
