using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Ict;
using Microsoft.Office.Interop.Excel;
using EXCEL = Microsoft.Office.Interop.Excel;
using Sci;

namespace Sci.Production.Report
{
    internal static class PrivUtils
    {
        public static DualResult F_ReportNoData() { return new DualResult(false, "Data not found."); }

        public static class Conds
        {
            public static void Between(string colname, string value1, string value2, IList<string> conds, IList<SqlParameter> paras)
            {
                if (null != value1 && 0 < value1.Length && null != value2 && 0 < value2.Length)
                {
                    string pname1, pname2;
                    paras.Add(new SqlParameter(pname1 = ("sp_" + paras.Count), value1));
                    paras.Add(new SqlParameter(pname2 = ("sp_" + paras.Count), value2));
                    conds.Add("{0} BETWEEN @{1} AND @{2}".InvariantFormat(colname, pname1, pname2));
                }
                else if (null != value1 && 0 < value1.Length)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = ("sp_" + paras.Count), value1));
                    conds.Add("{0}>=@{1}".InvariantFormat(colname, pname));
                }
                else if (null != value2 && 0 < value2.Length)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = ("sp_" + paras.Count), value2));
                    conds.Add("{0}<=@{1}".InvariantFormat(colname, pname));
                }
            }
            public static void Between(string colname, DateTime? value1, DateTime? value2, IList<string> conds, IList<SqlParameter> paras)
            {
                if (value1.HasValue && value2.HasValue)
                {
                    string pname1, pname2;
                    paras.Add(new SqlParameter(pname1 = ("sp_" + paras.Count), value1));
                    paras.Add(new SqlParameter(pname2 = ("sp_" + paras.Count), value2));
                    conds.Add("{0} BETWEEN @{1} AND @{2}".InvariantFormat(colname, pname1, pname2));
                }
                else if (value1.HasValue)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = ("sp_" + paras.Count), value1));
                    conds.Add("{0}>=@{1}".InvariantFormat(colname, pname));
                }
                else if (value2.HasValue)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = ("sp_" + paras.Count), value2));
                    conds.Add("{0}<=@{1}".InvariantFormat(colname, pname));
                }
            }
            public static void Eq(string colname, string value, IList<string> conds, IList<SqlParameter> paras, bool ignoreNullAndEmpty = true)
            {
                if (ignoreNullAndEmpty)
                {
                    if (null == value || 0 == value.Length) return;
                }

                string pname;
                paras.Add(new SqlParameter(pname = ("sp_" + paras.Count), value));
                conds.Add("{0}=@{1}".InvariantFormat(colname, pname));
            }
        }
        public static class Excels
        {
            public delegate object CellFormatter(DataRow data);
            public static DualResult CreateExcel(string templatefile, out EXCEL.Application excel)
            {
                excel = null;

                if (!System.IO.File.Exists(templatefile)) return new DualResult(false, "'{0}' excel template file not exists.".InvariantFormat(templatefile));

                EXCEL.Application exc;
                try
                {
                    exc = new EXCEL.Application();
                }
                catch (Exception ex) { return new DualResult(false, "Create excel application error.", ex); }

                try
                {
                    if (null != templatefile && 0 < templatefile.Length)
                    {
                        switch (System.IO.Path.GetExtension(templatefile).ToLowerInvariant())
                        {
                            case ".xls":
                                exc.Workbooks.Open(templatefile);
                                break;
                            default:
                                exc.Workbooks.Add(templatefile);
                                break;
                        }
                    }
                    else
                    {
                        exc.Workbooks.Add();
                    }
                }
                catch (Exception ex)
                {
                    exc.Quit();
                    return new DualResult(false, "Prepare excel workbook error.", ex);
                }

                excel = exc;
                return Result.True;
            }
            //自動開啟Excel存檔畫面
            public static DualResult SaveExcel(string templatefile, EXCEL.Application excel)
            {
                string file_name = "";
                try
                {
                    //存檔至Server特定路徑(抓設定檔)
                    //excel.ActiveWorkbook.SaveAs(templatefile + ".xls");

                    //excel.ActiveWorkbook.SaveAs(templatefile+".xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    //EXCEL.XlSaveAsAccessMode.xlShared, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    //若取消會回傳false(bool)--則跳掉 exception 不處理
                    var rtn = excel.GetSaveAsFilename(templatefile, "Excel workbook (*.xls;*.xlsx),*.xls;*.xlsx"); //Style:S1606LHSW510
                    try
                    {
                        file_name = rtn;
                    }
                    catch (Exception ex)
                    {
                    }
                    if (file_name != "")
                    {

                        excel.ActiveWorkbook.SaveAs(file_name);
                        excel.Visible = true;
                    }
                    //excel.ActiveWorkbook.OpenLinks(file_name);
                }
                catch (Exception ex)
                {
                    //已存在檔案開啟中無法存檔
                    string[] arr = file_name.Split('\\');
                    if (ex.ToString().Contains(arr[arr.Length - 1]))
                    {
                        return new DualResult(false, "save fail : file opened, please close first.", ex);
                    }
                    else
                    {
                        //return new DualResult(false, "Svae excel workbook error.", ex);
                    }
                }

                return Result.True;
            }

            public static void WriteDatas(Worksheet sheet, System.Data.DataTable datas, object[] cols_or_formatters, int rowix_begin)
            {
                int ix = rowix_begin;
                var ecell = ParseCell(cols_or_formatters.Length);
                foreach (DataRow it in datas.Rows)
                {
                    var array = ToArray(it, cols_or_formatters);

                    var range = "A{0}:{1}{0}".InvariantFormat(ix, ecell);
                    try
                    {
                        sheet.Range[range].Value2 = array;
                    }
                    catch (Exception ex)
                    {

                        string ss = string.Format("Export excel error.{0}", it[0].ToString());

                    }

                    ++ix;
                }
            }
            public static void WriteValue(Worksheet sheet, string cell, object value)
            {
                sheet.Range[cell].Value = ParseValue(value);
            }

            const string CELLs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private static string ParseCell(int num)
            {
                if (0 > num) return null;
                var ix = num - 1;
                var cd = "";
                while (true)
                {
                    int m = ix % 26;
                    cd = CELLs[m] + cd;

                    ix = ix / 26;
                    if (0 == ix) break;
                }
                return cd;
            }
            private static object[,] ToArray(System.Data.DataRow data, object[] cols_or_formatters)
            {
                var array = new object[1, cols_or_formatters.Length];
                for (int i = 0; i < cols_or_formatters.Length; ++i)
                {
                    var o = cols_or_formatters[i];
                    if (null == o) continue;
                    if (o is System.Data.DataColumn)
                    {
                        var v = data[(System.Data.DataColumn)o];
                        array[0, i] = ParseValue(v);
                    }
                    else if (o is string)
                    {
                        var v = data[(string)o];
                        array[0, i] = ParseValue(v);
                    }
                    else
                    {
                        var f = (CellFormatter)o;
                        var v = f(data);
                        array[0, i] = ParseValue(v);
                    }
                }
                return array;
            }

            private static object ParseValue(object value)
            {
                if (null == value || DBNull.Value == value) return null;
                if (value is DateTime)
                {
                    var v = (DateTime)value;
                    if (v.TimeOfDay == TimeSpan.Zero)
                    {
                        return v.ToString("yyyy/MM/dd");
                    }
                    else return v.ToString("yyyy/MM/dd HH:mm:ss");
                }
                return value;
            }
        }

        public static TABLE CreateTable<TABLE>()
            where TABLE : System.Data.DataTable, new()
        {
            var t = new TABLE();
            ClearConstrants(t);
            return t;
        }
        public static void ClearConstrants(System.Data.DataTable table)
        {
            if (null == table) table.Clear();

            table.Constraints.Clear();
            foreach (DataColumn it in table.Columns)
            {
                it.AutoIncrement = false;

                it.AllowDBNull = true;
                it.ReadOnly = false;
                it.Unique = false;
                it.MaxLength = -1;
                if (null != it.DefaultValue) it.DefaultValue = null;
            }
        }
        public static string GetString(this DataRow data, DataColumn col)
        {
            if (null == data) return null;
            var obj = data[col];
            return DBNull.Value != obj ? (string)obj : null;
        }
        public static DateTime? GetDateTime(this DataRow data, DataColumn col)
        {
            if (null == data) return null;
            var obj = data[col];
            return DBNull.Value != obj ? (DateTime)obj : (DateTime?)null;
        }
        public static string getPath_XLT(string sPath)
        {
            //string strPath = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("Sci.Trade")) + "Sci.Trade.Report\\XLT"; //Excel範例檔路徑
            //string temfile = Sci.Env.Cfg.XltPathDir + @"\Style-R13.SimilarStyleCalucateOrderqtyAndForecastqty.xlt";
            string rtn = "";
            if (Sci.Env.Cfg.XltPathDir != "")
            {
                rtn = Sci.Production.Class.Commons.TradeSystem.Env.XltPathDir;
            }
            else
            {

                string[] strPath_arr = sPath.Split('\\'); //Excel範例檔路徑
                for (int i = 0; i < strPath_arr.Length - 1; i++)
                {
                    rtn += strPath_arr[i] + "\\"; //Excel範例檔路徑
                }
                rtn += "XLT";
            }
            return rtn;
        }
        const string CELLs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string getPosition(int num)
        {
            if (0 > num) return null;
            var ix = num - 1;
            var cd = "";
            while (true)
            {
                int m = ix / 26;
                ix = ix % 26;
                if (m > 0) cd = cd + CELLs[m - 1];
                else cd = cd + CELLs[ix];

                if (0 == m) break;
            }
            return cd;
        }
        public static string getDayOfWeek(string value)
        {
            var cd = "";
            switch (value)
            {
                case "Monday":
                    cd = "星期一";
                    break;
                case "Tuesday":
                    cd = "星期二";
                    break;
                case "Wednesday":
                    cd = "星期三";
                    break;
                case "Thursday":
                    cd = "星期四";
                    break;
                case "Friday":
                    cd = "星期五";
                    break;
                case "Saturday":
                    cd = "星期六";
                    break;
                case "Sunday":
                    cd = "星期日";
                    break;
                default:
                    cd = value;
                    break;

            }
            return cd;
        }
        public static void SetRangeMerageCell(Range rngCell)
        {
            //rngCell.WrapText = false;
            //rngCell.Orientation = 0;
            //rngCell.AddIndent = false;
            //rngCell.IndentLevel = 0;
            //rngCell.ShrinkToFit = false;
            rngCell.MergeCells = true;
            //rngCell.HorizontalAlignment = HorizontalAlignment.Center;
            //rngCell.VerticalAlignment = HorizontalAlignment.Center;
            rngCell.VerticalAlignment = EXCEL.Constants.xlCenter;
            rngCell.HorizontalAlignment = EXCEL.Constants.xlCenter;

        }

        public static void SetRangeLineStyle(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
        }
        public static void SetRangeLineStyle_Top(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
        }
        public static void SetRangeLineStyle_Bottom(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
        }
        public static void SetRangeLineStyle_Left(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
        }
        public static void SetRangeLineStyle_Right(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
        }
        public static string getVersion(string value)
        {
            var rtn = value; // String.Format("{0}          {1}", value, " version.2016111101 ");

            return rtn;
        }

    }
}
