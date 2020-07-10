using System;
using System.IO;

namespace Sci.Production.Class
{
    /// <summary>
    /// Microsoft File
    /// </summary>
    public static class MicrosoftFile
    {
        /// <summary>
        /// Get Microsoft File Name
        /// </summary>
        /// <param name="processName">主檔名</param>
        /// <param name="nameExtension">副檔名，預設 xlsx</param>
        /// <returns>路徑+檔名+副檔名</returns>
        public static string GetName(string processName, string nameExtension = ExcelFileNameExtension.Xlsx)
        {
            string fileName = processName.Trim()
                                + DateTime.Now.ToString("_yyMMdd_HHmmssfff")
                                + nameExtension;
            return Path.Combine(Env.Cfg.ReportTempDir, fileName);
        }
    }
}