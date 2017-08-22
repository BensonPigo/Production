using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class
{
    public class ExcelNameExtension
    {
        public static string Xlsm = ".xlsm", Xlsx = ".xlsx";
    }

    public class GetExcelName
    {
        public string GetName(string ProcessName, string NameExtension = "")
        {
            string fileName = ProcessName.Trim()
                                + DateTime.Now.ToString("_yyMMdd_HHmmssfff")
                                + (NameExtension.Empty() ? ExcelNameExtension.Xlsx : NameExtension);
            return Path.Combine(Sci.Env.Cfg.ReportTempDir, fileName);
        }
    }
}
