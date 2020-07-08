using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Ict;
using Microsoft.Office.Interop.Excel;
using EXCEL = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Prg
{
    public static class PrivUtils
    {
        const string CELLs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static DualResult F_ReportNoData()
        {
            return new DualResult(false, "Data not found.");
        }

        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool DeleteFile(string name);

        private const string ZoneIdentifierStreamName = "Zone.Identifier";
        private static string strMsg = string.Empty;

        public static class Conds
        {
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

        public static class Excels
        {
            public delegate object CellFormatter(DataRow data);

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

                // 某些excel會被Office偵測為受保護, 不關掉的話Open 的時候會掛掉
                if (exc.Version.ToString().StrStartsWith("14"))
                {
                    exc.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
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

                            // case ".xlt":
                            //    exc.Workbooks.Add(templatefile+"x");
                            //    break;
                            default:
                                Unblock(templatefile);
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
                    return new DualResult(false, "Prepare excel workbook error." + strMsg + templatefile, ex);
                }

                excel = exc;
                return Result.True;
            }

            public static void Unblock(string file)
            {
                strMsg = string.Empty;
                FileInfo oFI = new FileInfo(file);

                // 刪除方式1 -可刪除保護檢視 , 但 exc.Workbooks.Add(templatefile); 有此Error : 未發送 AddJob呼叫
                if (oFI.Exists)
                {
                    if (DeleteFile(file + ":Zone.Identifier"))
                    {
                        // System.Diagnostics.Debug.WriteLine("移除 Zone.Identifier 完成。");
                        strMsg = " : \r\n刪除保護檢視失敗 !! \r\n請自行開啟檔案解除保護檢視\r\n";
                    }
                    else
                    {
                        strMsg = " : \r\n這檔案偵測不出具備 Zone.Identifier標記。\r\n請自行開啟檔案解除保護檢視";
                    }
                }

                // 刪除方式2 -可刪除保護檢視(我的電腦),無法刪除保護檢視(Serena電腦-不具備 Zone.Identifier 標記) , 但exc.Workbooks.Add(templatefile); 跟未刪除前Error相同: 偵測到此綁案有問題,為保護您的電腦,無法開啟
                /*if (oFI.Exists && oFI.AlternateDataStreamExists(ZoneIdentifierStreamName))
                {
                    //AlternateDataStreamInfo s = file.GetAlternateDataStream("Zone.Identifier", FileMode.Open);
                    // Delete the stream:
                    oFI.DeleteAlternateDataStream(ZoneIdentifierStreamName);
                    oFI.Refresh();
                    strMsg = " : \r\n刪除保護檢視失敗 !! \r\n請自行開啟檔案解除保護檢視\r\n";
                }
                else
                {
                    strMsg = " : \r\n這個檔案不具備 Zone.Identifier 標記。\r\n";
                }
                */
            }

            // Excel存檔畫面
            public static DualResult SaveExcel(string templatefile, Application excel, string mode = "N")
            {
                string file_name = string.Empty;
                try
                {
                    // 存檔至Server特定路徑(抓設定檔)
                    // excel.ActiveWorkbook.SaveAs(templatefile + ".xls");

                    // excel.ActiveWorkbook.SaveAs(templatefile+".xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    // EXCEL.XlSaveAsAccessMode.xlShared, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    if (mode == "N")
                    {
                        System.Windows.Forms.SaveFileDialog openDlg = new System.Windows.Forms.SaveFileDialog();
                        openDlg.Filter = "Excel workbook (*.xls;*.xlsx)|*.xls;*.xlsx";
                        openDlg.InitialDirectory = Directory.Exists(@"\\TsClient\D") ? @"\\TsClient\D" : @"D:\";
                        openDlg.FileName = Path.GetFileName(templatefile);
                        if (openDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            excel.DisplayAlerts = false;
                            string path = openDlg.FileName;
                            excel.ActiveWorkbook.SaveAs(path);

                            excel.Visible = true;

                            // workBook.SaveAs(/*path to save it to*/);  // NOTE: You can use 'Save()' or 'SaveAs()'
                            // workBook.Close();
                        }
                    }
                    else
                    {
                        // 若取消會回傳false(bool)--則跳掉 exception 不處理
                        var rtn = excel.GetSaveAsFilename(templatefile, "Excel workbook (*.xls;*.xlsx),*.xls;*.xlsx"); // Style:S1606LHSW510
                        try
                        {
                            file_name = rtn;
                        }
                        catch
                        {
                        }

                        if (file_name != string.Empty)
                        {
                            excel.ActiveWorkbook.SaveAs(file_name);
                            excel.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 已存在檔案開啟中無法存檔
                    string[] arr = file_name.Split('\\');
                    if (ex.ToString().Contains(arr[arr.Length - 1]))
                    {
                        if (excel != null)
                        {
                            excel.DisplayAlerts = false;
                            excel.Quit();
                        }

                        return new DualResult(false, "save fail : file opened, please close first.", ex);
                    }
                    else
                    {
                        // return new DualResult(false, "Svae excel workbook error.", ex);
                    }
                }

                return Result.True;
            }

            public static void WriteDatas(Worksheet sheet, System.Data.DataTable datas, object[] cols_or_formatters, int rowix_begin)
            {
                int ix = rowix_begin;
                var ecell = ParseCell(cols_or_formatters.Length);

                // 欄位Format 主要針對儲存格格式設定過日期,數值,文字,但未設定到位置之範例檔
                PrivUtils.Excels.setFormat(sheet, rowix_begin);
                foreach (DataRow it in datas.Rows)
                {
                    var array = ToArray(it, cols_or_formatters);

                    var range = "A{0}:{1}{0}".InvariantFormat(ix, ecell);
                    try
                    {
                        sheet.Range[range].Value2 = array;
                    }
                    catch
                    {
                        string ss = string.Format("Export excel error.{0}", it[0].ToString());
                    }

                    ++ix;
                }

                // 欄寬調整
                sheet.Range[string.Format("A:{0}", ecell)].WrapText = false;
                sheet.get_Range(string.Format("A:{0}", ecell)).EntireColumn.AutoFit();
                setPosition_Focus(sheet, ix);
            }

            public static void WriteValue(Worksheet sheet, string cell, object value)
            {
                sheet.Range[cell].Value = ParseValue(value);
            }

            public static void setPosition_Focus(Worksheet sheet, int rowix_begin)
            {
                // Excel欄位Focus
                sheet.Select();
                EXCEL.Range formatRange = sheet.get_Range(string.Format("A{0}", rowix_begin));
                formatRange.Select();
            }

            public static void setFormat(Worksheet sheet, int rowix_begin, string type = "", int intRowsCount = 65536)
            {
                sheet.Select();
                if (type == string.Empty)
                {
                    // 文字靠左,數字,日期欄位靠右
                    int intColumns = 999;
                    for (int k = 0; k < intColumns - 1; k++)
                    {
                        // wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).NumberFormatLocal = "yyyy/MM/dd";
                        string ss = sheet.get_Range(string.Format("{1}{0}:{1}{0}", rowix_begin, PrivUtils.getPosition(k + 1))).NumberFormatLocal;
                        object val = sheet.get_Range(string.Format("{1}{0}:{1}{0}", rowix_begin - 1, PrivUtils.getPosition(k + 1))).Value;
                        if (val == null || val.ToString().Trim() == string.Empty)
                        {
                            break; // 遇欄位名稱空白-結束
                        }

                        /*if (k == 81)
{
   System.Diagnostics.Debug.WriteLine(ss);
}*/

                        // int rowCount = (intRowsCount + rowix_begin - 1) > 65536 ? 65536 : intRowsCount + rowix_begin - 1;
                        if (ss == "yyyy/m/d")
                        {
                            sheet.get_Range(string.Format("{0}:{0}", PrivUtils.getPosition(k + 1))).HorizontalAlignment = EXCEL.Constants.xlRight;
                        }
                        else if (ss.Contains("#,##") || ss.Contains("0"))
                        {
                            sheet.get_Range(string.Format("{0}:{0}", PrivUtils.getPosition(k + 1))).HorizontalAlignment = EXCEL.Constants.xlRight;
                        }
                        else if (ss == "@")
                        {
                            sheet.get_Range(string.Format("{0}:{0}", PrivUtils.getPosition(k + 1))).HorizontalAlignment = EXCEL.Constants.xlLeft;
                        }
                        else
                        {
                            sheet.get_Range(string.Format("{0}:{0}", PrivUtils.getPosition(k + 1))).HorizontalAlignment = EXCEL.Constants.xlLeft;
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine(type);
            }

            public static DualResult writeExcelErr(EXCEL.Worksheet wsSheet, object[,] objArray, int rownum, int mm, int intColumns, Exception ex)
            {
                try
                {
                    if (mm < 100)
                    {
                        string tt = mm.ToString();
                    }

                    if (mm <= 1)
                    {
                        string scol = string.Empty;
                        for (int k = 0; k < intColumns; k++)
                        {
                            if (objArray[0, k].ToString().IndexOf("=") == 0)
                            {
                                scol += "err msg : 值有=號開頭, 寫入數值格式欄位,誤判為公式錯誤-->"; // Ex. =46=TW=9D
                            }

                            scol += PrivUtils.getPosition(k + 1) + " :: " + objArray[0, k].ToString() + "\r\n ";
                        }

                        return new DualResult(false, "Export excel error.  -> 所有欄位(排序)值 : \r\n" + scol, ex);
                    }

                    int kk = mm;
                    mm = int.Parse(Math.Truncate((mm / 2) + 0.5).ToString());
                    object[,] objArray1 = new object[mm, intColumns]; // 每列匯入欄位區間
                    try
                    {
                        for (int z = 0; z < mm; z++)
                        {
                            for (int k = 0; k < intColumns; k++)
                            {
                                objArray1[z, k] = objArray[z, k]; // 處理特殊內容 Ex. "=46=TW=9D";//
                            }
                        }

                        wsSheet.Range[string.Format("A{0}:{2}{1}", rownum, rownum + mm - 1, PrivUtils.getPosition(intColumns))].Value2 = objArray1;
                        try
                        {
                            rownum += mm;
                            objArray1 = new object[kk - mm, intColumns]; // 每列匯入欄位區間
                            for (int z = mm; z < kk; z++)
                            {
                                for (int k = 0; k < intColumns; k++)
                                {
                                    objArray1[z - mm, k] = objArray[z, k]; // 處理特殊內容 Ex. "=46=TW=9D";//
                                }
                            }

                            wsSheet.Range[string.Format("A{0}:{2}{1}", rownum, rownum + kk - 1, PrivUtils.getPosition(intColumns))].Value2 = objArray1;
                        }
                        catch (Exception ex1)
                        {
                            return writeExcelErr(wsSheet, objArray1, rownum, kk - mm, intColumns, ex1);
                        }
                    }
                    catch (Exception ex2)
                    {
                        return writeExcelErr(wsSheet, objArray1, rownum, mm, intColumns, ex2);
                    }
                }
                catch (Exception e)
                {
                    return new DualResult(false, "Export excel error.", e);
                }

                return Result.True;
            }

            const string CELLs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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

        public static TABLE CreateTable<TABLE>()
            where TABLE : System.Data.DataTable, new()
        {
            var t = new TABLE();
            ClearConstrants(t);
            return t;
        }

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

        public static string GetString(this DataRow data, DataColumn col)
        {
            if (data == null)
            {
                return null;
            }

            var obj = data[col];
            return obj != DBNull.Value ? (string)obj : null;
        }

        public static DateTime? GetDateTime(this DataRow data, DataColumn col)
        {
            if (data == null)
            {
                return null;
            }

            var obj = data[col];
            return obj != DBNull.Value ? (DateTime)obj : (DateTime?)null;
        }

        // 取小數位數
        public static string getValueFormat(string value, string type = "S", int exact = 2)
        {
            var rtn = value;
            switch (type)
            {
                case "D": // 取小數兩位(超過兩位時)
                    decimal dvalue;
                    if (decimal.TryParse(value, out dvalue))
                    {
                        // dvalue = Math.Round(dvalue, 2);
                        string ss = dvalue.ToString();
                        string[] arr = ss.Split('.');
                        int ilen = 0;
                        if (arr.Length > 1)
                        {
                            ilen = arr[1].Length;
                            if (ilen > exact)
                            {
                                ilen = exact;
                            }

                            ss = Math.Round(dvalue, ilen).ToString(); // string.Format("{0}.{1}", arr[0], arr[1].Substring(0, ilen));
                        }

                        if (ilen == 0)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0");
                        }

                        if (ilen == 1)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0.0");
                        }

                        if (ilen == 2)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0.00");
                        }
                    }
                    else
                    {
                        rtn = "0";
                    }

                    break;
                case "S":
                    if (value.ToString().IndexOf("=") == 0) // 值有=號開頭, 若寫入數值格式欄位,誤判為公式錯誤-->";// Ex. =46=TW=9D
                    {
                        rtn = " " + value; // 在 = 號前埔空白
                    }

                    break;
                default:
                    break;
            }

            return rtn;
        }

        // 取小數位數--取到為非 0 的位數
        public static string getValueFormat_0(string value)
        {
            var rtn = value;
            decimal dvalue;
            if (decimal.TryParse(value, out dvalue))
            {
                dvalue = Math.Round(dvalue, 6);
                string ss = dvalue.ToString();
                string[] arr = ss.Split('.');
                string s1 = string.Empty, s2 = string.Empty;
                if (arr.Length > 1)
                {
                    s1 = arr[0];
                    int iLen = 0;
                    for (int k = arr[1].Length - 1; k >= 0; k--)
                    {
                        if (int.Parse(arr[1].Substring(k, 1)) > 0)
                        {
                            iLen = k + 1;
                            break;
                        }
                    }

                    for (int k = 0; k < iLen; k++)
                    {
                        s2 += arr[1].Substring(k, 1);
                    }

                    if (s2 != string.Empty)
                    {
                        ss = string.Format("{0}.{1}", s1, s2);
                        if (iLen == 1)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0.0");
                        }

                        if (iLen == 2)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0.00");
                        }

                        if (iLen == 3)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0.000");
                        }

                        if (iLen == 4)
                        {
                            rtn = decimal.Parse(ss).ToString("#,##0.0000");
                        }
                    }
                    else
                    {
                        rtn = s1;
                    }
                }
            }
            else
            {
                rtn = "0";
            }

            return rtn;
        }

        public static string getPath_XLT(string sPath)
        {
            string rtn = string.Empty;
            if (Sci.Env.Cfg.XltPathDir != string.Empty)
            {
                rtn = Sci.Env.Cfg.XltPathDir;
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

        public static string getPosition(int num)
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

        public static string getDayOfWeek(string value)
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

        public static string getDayOfMonth(string value)
        {
            var cd = string.Empty;
            switch (value)
            {
                case "01":
                    cd = "JAN";
                    break;
                case "02":
                    cd = "FAB";
                    break;
                case "03":
                    cd = "MAR";
                    break;
                case "04":
                    cd = "APR";
                    break;
                case "05":
                    cd = "MAY";
                    break;
                case "06":
                    cd = "JUN";
                    break;
                case "07":
                    cd = "JUL";
                    break;
                case "08":
                    cd = "AUG";
                    break;
                case "09":
                    cd = "SEP";
                    break;
                case "10":
                    cd = "OCT";
                    break;
                case "11":
                    cd = "NOV";
                    break;
                case "12":
                    cd = "DEC";
                    break;
                default:
                    cd = value;
                    break;
            }

            return cd;
        }

        public static int getPageNum()
        {
            int cd = 65537;

            return cd;
        }

        public static void SetRangeNotMerageCell(Range rngCell)
        {
            rngCell.MergeCells = false;

            // rngCell.HorizontalAlignment = HorizontalAlignment.Center;
            // rngCell.VerticalAlignment = HorizontalAlignment.Center;
            rngCell.VerticalAlignment = EXCEL.Constants.xlCenter;
            rngCell.HorizontalAlignment = EXCEL.Constants.xlCenter;
        }

        public static void SetRangeMerageCell(Range rngCell)
        {
            // rngCell.WrapText = false;
            // rngCell.Orientation = 0;
            // rngCell.AddIndent = false;
            // rngCell.IndentLevel = 0;
            // rngCell.ShrinkToFit = false;
            rngCell.MergeCells = true;

            // rngCell.HorizontalAlignment = HorizontalAlignment.Center;
            // rngCell.VerticalAlignment = HorizontalAlignment.Center;
            rngCell.VerticalAlignment = EXCEL.Constants.xlCenter;
            rngCell.HorizontalAlignment = EXCEL.Constants.xlCenter;
        }

        /// <summary>
        /// 範圍內含合併儲存格 使用
        /// </summary>
        /// <param name="rngCell"></param>
        /// <param name="sLineType"></param>
        public static void SetRangeLineStyle_Clear(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlLineStyleNone;
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = xLineStyle;

            // 已下設定:範圍內含合併儲存格會出錯
            // rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = xLineStyle;
            // rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = xLineStyle;
        }

        public static void SetRangeLineStyle_Merge(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlContinuous;
            if (sLineType == "A")
            {
                xLineStyle = XlLineStyle.xlDouble;
            }

            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = xLineStyle;

            // 已下設定:範圍內含合併儲存格會出錯
            // rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = xLineStyle;
            // rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = xLineStyle;
        }

        public static void SetRangeLineStyle(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlContinuous;
            if (sLineType == "A")
            {
                xLineStyle = XlLineStyle.xlDouble;
            }

            // 無法設定種類 Border 的 LineStyle 屬性--vu0 先取消再設定
            /*rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlLineStyleNone;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlLineStyleNone;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlLineStyleNone;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlLineStyleNone;
            rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlLineStyleNone;
            rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlLineStyleNone;
            */
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = xLineStyle;
        }

        public static void SetRangeLineStyle_Top(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlContinuous;
            XlBorderWeight xWeight = XlBorderWeight.xlThin;
            if (sLineType == "A")
            {
                xLineStyle = XlLineStyle.xlDouble;
                xWeight = XlBorderWeight.xlThick;
            }

            if (sLineType == "B")
            {
                xWeight = XlBorderWeight.xlThick;
            }

            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeTop].Weight = xWeight;
        }

        public static void SetRangeLineStyle_Bottom(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlContinuous;
            XlBorderWeight xWeight = XlBorderWeight.xlThin;
            if (sLineType == "A")
            {
                xLineStyle = XlLineStyle.xlDouble;
                xWeight = XlBorderWeight.xlThick;
            }

            if (sLineType == "B")
            {
                xWeight = XlBorderWeight.xlThick;
            }

            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].Weight = xWeight;
        }

        public static void SetRangeLineStyle_Left(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlContinuous;
            XlBorderWeight xWeight = XlBorderWeight.xlThin;
            if (sLineType == "A")
            {
                xLineStyle = XlLineStyle.xlDouble;
                xWeight = XlBorderWeight.xlThick;
            }

            if (sLineType == "B")
            {
                xWeight = XlBorderWeight.xlThick;
            }

            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].Weight = xWeight;
        }

        public static void SetRangeLineStyle_Right(Range rngCell, string sLineType = "")
        {
            XlLineStyle xLineStyle = XlLineStyle.xlContinuous;
            XlBorderWeight xWeight = XlBorderWeight.xlThin;
            if (sLineType == "A")
            {
                xLineStyle = XlLineStyle.xlDouble;
                xWeight = XlBorderWeight.xlThick;
            }

            if (sLineType == "B")
            {
                xWeight = XlBorderWeight.xlThick;
            }

            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = xLineStyle;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].Weight = xWeight;
        }

        public static string getVersion(string value)
        {
            var rtn = value; // String.Format("{0}          {1}", value, " version.2016111101 ");

            return rtn;
        }
    }
}
