using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ict;
using Microsoft.Office.Interop.Excel;

namespace Sci.Production.Report
{
    /// <summary>
    /// PrivUtils
    /// </summary>
    internal static class PrivUtils
    {
        /// <summary>
        /// F_ReportNoData
        /// </summary>
        /// <returns>DualResult</returns>
        public static DualResult F_ReportNoData()
        {
            return new DualResult(false, "Data not found.");
        }

        /// <summary>
        /// Conds
        /// </summary>
        public static class Conds
        {
            /// <summary>
            /// Between
            /// </summary>
            /// <param name="colname">colname</param>
            /// <param name="value1">value1</param>
            /// <param name="value2">value2</param>
            /// <param name="conds">conds</param>
            /// <param name="paras">paras</param>
            public static void Between(string colname, string value1, string value2, IList<string> conds, IList<SqlParameter> paras)
            {
                if (value1 != null && value1.Length > 0 && value2 != null && value2.Length > 0)
                {
                    string pname1, pname2;
                    paras.Add(new SqlParameter(pname1 = "sp_" + paras.Count, value1));
                    paras.Add(new SqlParameter(pname2 = "sp_" + paras.Count, value2));
                    conds.Add("{0} BETWEEN @{1} AND @{2}".InvariantFormat(colname, pname1, pname2));
                }
                else if (value1 != null && value1.Length > 0)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = "sp_" + paras.Count, value1));
                    conds.Add("{0}>=@{1}".InvariantFormat(colname, pname));
                }
                else if (value2 != null && value2.Length > 0)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = "sp_" + paras.Count, value2));
                    conds.Add("{0}<=@{1}".InvariantFormat(colname, pname));
                }
            }

            /// <summary>
            /// Between
            /// </summary>
            /// <param name="colname">colname</param>
            /// <param name="value1">value1</param>
            /// <param name="value2">value2</param>
            /// <param name="conds">conds</param>
            /// <param name="paras">paras</param>
            public static void Between(string colname, DateTime? value1, DateTime? value2, IList<string> conds, IList<SqlParameter> paras)
            {
                if (value1.HasValue && value2.HasValue)
                {
                    string pname1, pname2;
                    paras.Add(new SqlParameter(pname1 = "sp_" + paras.Count, value1));
                    paras.Add(new SqlParameter(pname2 = "sp_" + paras.Count, value2));
                    conds.Add("{0} BETWEEN @{1} AND @{2}".InvariantFormat(colname, pname1, pname2));
                }
                else if (value1.HasValue)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = "sp_" + paras.Count, value1));
                    conds.Add("{0}>=@{1}".InvariantFormat(colname, pname));
                }
                else if (value2.HasValue)
                {
                    string pname;
                    paras.Add(new SqlParameter(pname = "sp_" + paras.Count, value2));
                    conds.Add("{0}<=@{1}".InvariantFormat(colname, pname));
                }
            }

            /// <summary>
            /// Eq
            /// </summary>
            /// <param name="colname">colname</param>
            /// <param name="value">value</param>
            /// <param name="conds">conds</param>
            /// <param name="paras">paras</param>
            /// <param name="ignoreNullAndEmpty">ignoreNullAndEmpty</param>
            public static void Eq(string colname, string value, IList<string> conds, IList<SqlParameter> paras, bool ignoreNullAndEmpty = true)
            {
                if (ignoreNullAndEmpty)
                {
                    if (value == null || value.Length == 0)
                    {
                        return;
                    }
                }

                string pname;
                paras.Add(new SqlParameter(pname = "sp_" + paras.Count, value));
                conds.Add("{0}=@{1}".InvariantFormat(colname, pname));
            }
        }

        /// <summary>
        /// Excels
        /// </summary>
        public static class Excels
        {
            /// <summary>
            /// CellFormatter
            /// </summary>
            /// <param name="data">data</param>
            /// <returns>delegate</returns>
            public delegate object CellFormatter(DataRow data);

            /// <summary>
            /// CreateExcel
            /// </summary>
            /// <param name="templatefile">templatefile</param>
            /// <param name="excel">excel</param>
            /// <returns>DualResult</returns>
            public static DualResult CreateExcel(string templatefile, out Application excel)
            {
                excel = null;

                if (!System.IO.File.Exists(templatefile))
                {
                    return new DualResult(false, "'{0}' excel template file not exists.".InvariantFormat(templatefile));
                }

                Application exc;
                try
                {
                    exc = new Application();
                }
                catch (Exception ex)
                {
                    return new DualResult(false, "Create excel application error.", ex);
                }

                try
                {
                    if (templatefile != null && templatefile.Length > 0)
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
                return Ict.Result.True;
            }

            /// <summary>
            /// SaveExcel
            /// </summary>
            /// <param name="templatefile">templatefile</param>
            /// <param name="excel">excel</param>
            /// <returns>DualResult</returns>
            public static DualResult SaveExcel(string templatefile, Application excel)
            {
                string file_name = string.Empty;
                try
                {
                    var rtn = excel.GetSaveAsFilename(templatefile, "Excel workbook (*.xls;*.xlsx),*.xls;*.xlsx");
                    try
                    {
                        file_name = rtn;
                    }
                    catch (Exception)
                    {
                    }

                    if (file_name != string.Empty)
                    {
                        excel.ActiveWorkbook.SaveAs(file_name);
                        excel.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    string[] arr = file_name.Split('\\');
                    if (ex.ToString().Contains(arr[arr.Length - 1]))
                    {
                        return new DualResult(false, "save fail : file opened, please close first.", ex);
                    }
                }

                return Ict.Result.True;
            }

            /// <summary>
            /// WriteDatas
            /// </summary>
            /// <param name="sheet">sheet</param>
            /// <param name="datas">datas</param>
            /// <param name="cols_or_formatters">cols_or_formatters</param>
            /// <param name="rowix_begin">rowix_begin</param>
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
                    catch (Exception)
                    {
                        string ss = string.Format("Export excel error.{0}", it[0].ToString());
                    }

                    ++ix;
                }
            }

            /// <summary>
            /// WriteValue
            /// </summary>
            /// <param name="sheet">sheet</param>
            /// <param name="cell">cell</param>
            /// <param name="value">value</param>
            public static void WriteValue(Worksheet sheet, string cell, object value)
            {
                sheet.Range[cell].Value = ParseValue(value);
            }

            private const string CELLs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            private static string ParseCell(int num)
            {
                if (num < 0)
                {
                    return null;
                }

                var ix = num - 1;
                var cd = string.Empty;
                while (true)
                {
                    int m = ix % 26;
                    cd = CELLs[m] + cd;

                    ix = ix / 26;
                    if (ix == 0)
                    {
                        break;
                    }
                }

                return cd;
            }

            private static object[,] ToArray(DataRow data, object[] cols_or_formatters)
            {
                var array = new object[1, cols_or_formatters.Length];
                for (int i = 0; i < cols_or_formatters.Length; ++i)
                {
                    var o = cols_or_formatters[i];
                    if (o == null)
                    {
                        continue;
                    }

                    if (o is DataColumn)
                    {
                        var v = data[(DataColumn)o];
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
                if (value == null || value == DBNull.Value)
                {
                    return null;
                }

                if (value is DateTime)
                {
                    var v = (DateTime)value;
                    if (v.TimeOfDay == TimeSpan.Zero)
                    {
                        return v.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        return v.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                }

                return value;
            }
        }

        /// <summary>
        /// CreateTable
        /// </summary>
        /// <typeparam name="TABLE">TABLE</typeparam>
        /// <returns>TABLEx</returns>
        public static TABLE CreateTable<TABLE>()
            where TABLE : System.Data.DataTable, new()
        {
            var t = new TABLE();
            ClearConstrants(t);
            return t;
        }

        /// <summary>
        /// ClearConstrants
        /// </summary>
        /// <param name="table">table</param>
        public static void ClearConstrants(System.Data.DataTable table)
        {
            if (table == null)
            {
                table.Clear();
            }

            table.Constraints.Clear();
            foreach (DataColumn it in table.Columns)
            {
                it.AutoIncrement = false;

                it.AllowDBNull = true;
                it.ReadOnly = false;
                it.Unique = false;
                it.MaxLength = -1;
                if (it.DefaultValue != null)
                {
                    it.DefaultValue = null;
                }
            }
        }

        /// <summary>
        /// GetString
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="col">col</param>
        /// <returns>string</returns>
        public static string GetString(this DataRow data, DataColumn col)
        {
            if (data == null)
            {
                return null;
            }

            var obj = data[col];
            return obj != DBNull.Value ? (string)obj : null;
        }

        /// <summary>
        /// GetDateTime
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="col">col</param>
        /// <returns>DateTime</returns>
        public static DateTime? GetDateTime(this DataRow data, DataColumn col)
        {
            if (data == null)
            {
                return null;
            }

            var obj = data[col];
            return obj != DBNull.Value ? (DateTime)obj : (DateTime?)null;
        }

        /// <summary>
        /// GetPath_XLT
        /// </summary>
        /// <param name="sPath">sPath</param>
        /// <returns>string</returns>
        public static string GetPath_XLT(string sPath)
        {
            string rtn = string.Empty;
            if (Env.Cfg.XltPathDir != string.Empty)
            {
                rtn = Env.Cfg.XltPathDir;
            }
            else
            {
                string[] strPath_arr = sPath.Split('\\'); // Excel範例檔路徑
                for (int i = 0; i < strPath_arr.Length - 1; i++)
                {
                    rtn += strPath_arr[i] + "\\"; // Excel範例檔路徑
                }

                rtn += "XLT";
            }

            return rtn;
        }

        /// <summary>
        /// CELLs
        /// </summary>
        private const string CELLs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// GetPosition
        /// </summary>
        /// <param name="num">num</param>
        /// <returns>string</returns>
        public static string GetPosition(int num)
        {
            if (num < 0)
            {
                return null;
            }

            var ix = num - 1;
            var cd = string.Empty;
            while (true)
            {
                int m = ix / 26;
                ix = ix % 26;
                if (m > 0)
                {
                    cd = cd + CELLs[m - 1];
                }
                else
                {
                    cd = cd + CELLs[ix];
                }

                if (m == 0)
                {
                    break;
                }
            }

            return cd;
        }

        /// <summary>
        /// GetDayOfWeek
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>string</returns>
        public static string GetDayOfWeek(string value)
        {
            var cd = string.Empty;
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

        /// <summary>
        /// SetRangeMerageCell
        /// </summary>
        /// <param name="rngCell">rngCell</param>
        public static void SetRangeMerageCell(Range rngCell)
        {
            rngCell.MergeCells = true;
            rngCell.VerticalAlignment = Constants.xlCenter;
            rngCell.HorizontalAlignment = Constants.xlCenter;
        }

        /// <summary>
        /// SetRangeLineStyle
        /// </summary>
        /// <param name="rngCell">rngCell</param>
        public static void SetRangeLineStyle(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
        }

        /// <summary>
        /// SetRangeLineStyle_Top
        /// </summary>
        /// <param name="rngCell">rngCell</param>
        public static void SetRangeLineStyle_Top(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
        }

        /// <summary>
        /// SetRangeLineStyle_Bottom
        /// </summary>
        /// <param name="rngCell">rngCell</param>
        public static void SetRangeLineStyle_Bottom(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
        }

        /// <summary>
        /// SetRangeLineStyle_Left
        /// </summary>
        /// <param name="rngCell">rngCell</param>
        public static void SetRangeLineStyle_Left(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
        }

        /// <summary>
        /// SetRangeLineStyle_Right
        /// </summary>
        /// <param name="rngCell">rngCell</param>
        public static void SetRangeLineStyle_Right(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
        }

        /// <summary>
        /// GetVersion
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>string</returns>
        public static string GetVersion(string value)
        {
            var rtn = value;

            return rtn;
        }
    }
}
