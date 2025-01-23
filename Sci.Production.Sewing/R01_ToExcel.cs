using Ict;
using Sci;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Sci.Production.Prg;

namespace Sci.Production.Sewing
{
    /// <summary>
    ///R01_ToExcel
    /// </summary>
    internal class R01_ToExcel
    {
        /// <summary>
        /// 製作R01 報表
        /// </summary>
        /// <param name="factoryName">factoryName</param>
        /// <param name="factory">factory</param>
        /// <param name="date">date</param>
        /// <param name="dateDate">dateDate</param>
        /// <param name="printData">printData</param>
        /// <param name="ttlData">ttlData</param>
        /// <param name="subprocessData">subprocessData</param>
        /// <param name="dataMode">dataMode</param>
        /// <param name="saveFileName">saveFileName</param>
        /// <param name="excelName">excelName</param>
        /// <returns>true</returns>
        public static bool ToExcel(string factoryName, string factory, DateTime? date, DateTime? dateDate, DataTable printData, DataTable ttlData, DataTable subprocessData, List<APIData> dataMode, string saveFileName, ref string excelName)
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Sewing_R01_DailyCMPReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) { return false; }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            #if DEBUG
            excel.Visible = false;
            #endif
            worksheet.Cells[1, 1] = factoryName;
            worksheet.Cells[2, 1] = string.Format("{0} Daily CMP Report, DD.{1} {2}", factory, Convert.ToDateTime(date).ToString("MM/dd"), "(Included Subcon-IN)");

            object[,] objArray = new object[1, 26];
            string[] subTtlRowInOut = new string[9];
            string[] subTtlRowExOut = new string[9];
            string[] subTtlRowExInOut = new string[9];

            if (printData.Rows.Count > 0 && !MyUtility.Check.Empty(printData))
            {
                string shift = MyUtility.Convert.GetString(printData.Rows[0]["Shift"]);
                string team = MyUtility.Convert.GetString(printData.Rows[0]["Team"]);
                int insertRow = 5, startRow = 5, ttlShift = 1, subRows = 0;
                worksheet.Cells[3, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
                DataRow[] selectRow;
                foreach (DataRow dr in printData.Rows)
                {
                    if (shift != MyUtility.Convert.GetString(dr["Shift"]) || team != MyUtility.Convert.GetString(dr["Team"]))
                    {
                        // 將多出來的Record刪除
                        for (int i = 1; i <= 2; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                            Marshal.ReleaseComObject(rng);
                        }

                        // 填入Sub Total資料
                        if (ttlData != null)
                        {
                            selectRow = ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                            if (selectRow.Length > 0)
                            {
                                worksheet.Cells[insertRow, 11] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                                worksheet.Cells[insertRow, 15] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                                worksheet.Cells[insertRow, 22] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                            }
                        }

                        worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 14] = string.Format("=SUM(N{0}:N{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 17] = string.Format("=SUM(Q{0}:Q{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 20] = string.Format("=S{0}/M{0}", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 21] = string.Format("=ROUND((S{0}/(M{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 24] = string.Format("=SUM(X{0}:X{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 25] = string.Format("=SUM(Y{0}:Y{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));

                        subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                        if (shift != "Subcon-Out")
                        {
                            subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
                        }

                        if (shift != "Subcon-Out" && shift != "Subcon-In(Non Sister)")
                        {
                            subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                        }

                        // 重置參數資料
                        shift = MyUtility.Convert.GetString(dr["Shift"]);
                        team = MyUtility.Convert.GetString(dr["Team"]);
                        worksheet.Cells[insertRow + 2, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
                        insertRow += 4;
                        startRow = insertRow;
                        ttlShift++;
                        subRows++;
                    }

                    objArray[0, 0] = dr["SewingLineID"];
                    objArray[0, 1] = dr["OrderId"];
                    objArray[0, 2] = dr["Style"];
                    objArray[0, 3] = dr["ComboType"];
                    objArray[0, 4] = dr["CDCodeNew"];
                    objArray[0, 5] = dr["ProductType"];
                    objArray[0, 6] = dr["FabricType"];
                    objArray[0, 7] = dr["Lining"];
                    objArray[0, 8] = dr["Gender"];
                    objArray[0, 9] = dr["Construction"];
                    objArray[0, 10] = dr["ActManPower"];
                    objArray[0, 11] = dr["WorkHour"];
                    objArray[0, 12] = dr["ManHour"];
                    objArray[0, 13] = dr["TargetCPU"];
                    objArray[0, 14] = dr["TMS"];
                    objArray[0, 15] = dr["CPUPrice"];
                    objArray[0, 16] = dr["TargetQty"];
                    objArray[0, 17] = dr["QAQty"];
                    objArray[0, 18] = dr["TotalCPU"];
                    objArray[0, 19] = dr["CPUSewer"];
                    objArray[0, 20] = string.Format("=ROUND((S{0}/(M{0}*3600/1400))*100,1)", insertRow);
                    objArray[0, 21] = dr["RFT"];
                    objArray[0, 22] = dr["CumulateDate"];
                    objArray[0, 23] = dr["InlineQty"];
                    objArray[0, 24] = dr["Diff"];
                    objArray[0, 25] = dr["FactoryID"];
                    worksheet.Range[string.Format("A{0}:Z{0}", insertRow)].Value2 = objArray;
                    insertRow++;

                    // 插入一筆Record
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }

                // 最後一個Shift資料
                // 將多出來的Record刪除
                for (int i = 1; i <= 2; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                // 填入Sub Total資料
                if (ttlData != null)
                {
                    selectRow = ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                    if (selectRow.Length > 0)
                    {
                        worksheet.Cells[insertRow, 11] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                        worksheet.Cells[insertRow, 15] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                        worksheet.Cells[insertRow, 22] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                    }
                }

                worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 14] = string.Format("=SUM(N{0}:N{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 17] = string.Format("=SUM(Q{0}:Q{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 20] = string.Format("=S{0}/M{0}", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 21] = string.Format("=ROUND((S{0}/(M{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 24] = string.Format("=SUM(X{0}:X{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 25] = string.Format("=SUM(Y{0}:Y{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));

                subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                if (shift != "Subcon-Out")
                {
                    subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
                }

                if (shift != "Subcon-Out" && shift != "Subcon-In(Non Sister)")
                {
                    subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                }

                // 刪除多出來的Shift Record
                for (int i = 1; i <= (9 - ttlShift) * 6; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow + 1, Type.Missing];
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                insertRow += 2;

                // 填Grand Total資料
                string ttlManhour, targetCPU, targetQty, qaQty, ttlCPU, prodOutput, diff, factoryID;
                if (ttlData != null)
                {
                    selectRow = ttlData.Select("Type = 'Grand'");
                    if (selectRow.Length > 0)
                    {
                        for (int i = 0; i < selectRow.Length; i++)
                        {
                            worksheet.Cells[insertRow, 11] = MyUtility.Convert.GetDecimal(selectRow[i]["ActManPower"]);
                            worksheet.Cells[insertRow, 15] = MyUtility.Convert.GetDecimal(selectRow[i]["TMS"]);
                            worksheet.Cells[insertRow, 22] = MyUtility.Convert.GetDecimal(selectRow[i]["RFT"]);
                            ttlManhour = "=";
                            targetCPU = "=";
                            targetQty = "=";
                            qaQty = "=";
                            ttlCPU = "=";
                            prodOutput = "=";
                            diff = "=";
                            factoryID = "=";
                            #region 組公式
                            if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "2")
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    if (!MyUtility.Check.Empty(subTtlRowInOut[j]))
                                    {
                                        ttlManhour += string.Format("M{0}+", subTtlRowInOut[j]);
                                        targetCPU += string.Format("N{0}+", subTtlRowInOut[j]);
                                        targetQty += string.Format("Q{0}+", subTtlRowInOut[j]);
                                        qaQty += string.Format("R{0}+", subTtlRowInOut[j]);
                                        ttlCPU += string.Format("S{0}+", subTtlRowInOut[j]);
                                        prodOutput += string.Format("X{0}+", subTtlRowInOut[j]);
                                        diff += string.Format("Y{0}+", subTtlRowInOut[j]);
                                    }
                                }
                            }
                            else if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "3")
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    if (!MyUtility.Check.Empty(subTtlRowExOut[j]))
                                    {
                                        ttlManhour += string.Format("M{0}+", subTtlRowExOut[j]);
                                        targetCPU += string.Format("N{0}+", subTtlRowExOut[j]);
                                        targetQty += string.Format("Q{0}+", subTtlRowExOut[j]);
                                        qaQty += string.Format("R{0}+", subTtlRowExOut[j]);
                                        ttlCPU += string.Format("S{0}+", subTtlRowExOut[j]);
                                        prodOutput += string.Format("X{0}+", subTtlRowExOut[j]);
                                        diff += string.Format("Y{0}+", subTtlRowExOut[j]);
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    if (!MyUtility.Check.Empty(subTtlRowExInOut[j]))
                                    {
                                        ttlManhour += string.Format("M{0}+", subTtlRowExInOut[j]);
                                        targetCPU += string.Format("N{0}+", subTtlRowExInOut[j]);
                                        targetQty += string.Format("Q{0}+", subTtlRowExInOut[j]);
                                        qaQty += string.Format("R{0}+", subTtlRowExInOut[j]);
                                        ttlCPU += string.Format("S{0}+", subTtlRowExInOut[j]);
                                        prodOutput += string.Format("X{0}+", subTtlRowExInOut[j]);
                                        diff += string.Format("Y{0}+", subTtlRowExInOut[j]);
                                    }
                                }
                            }
                            #endregion

                            worksheet.Cells[insertRow, 13] = ttlManhour.Substring(0, ttlManhour.Length - 1);
                            worksheet.Cells[insertRow, 14] = targetCPU.Substring(0, targetCPU.Length - 1);
                            worksheet.Cells[insertRow, 17] = targetQty.Substring(0, targetQty.Length - 1);
                            worksheet.Cells[insertRow, 18] = qaQty.Substring(0, qaQty.Length - 1);
                            worksheet.Cells[insertRow, 19] = ttlCPU.Substring(0, ttlCPU.Length - 1);
                            worksheet.Cells[insertRow, 20] = string.Format("=S{0}/M{0}", MyUtility.Convert.GetString(insertRow));
                            worksheet.Cells[insertRow, 21] = string.Format("=ROUND((S{0}/(M{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                            worksheet.Cells[insertRow, 24] = prodOutput.Substring(0, prodOutput.Length - 1);
                            worksheet.Cells[insertRow, 25] = diff.Substring(0, diff.Length - 1);
                            worksheet.Cells[insertRow, 26] = factoryID.Substring(0, factoryID.Length - 1);
                            insertRow++;
                        }
                    }
                }
                #region Direct Manpower(From PAMS)
                if (Env.User.Keyword.EqualString("CM1") ||
                    Env.User.Keyword.EqualString("CM2"))
                {
                    worksheet.Cells[insertRow, 11] = 0;
                    worksheet.Cells[insertRow, 13] = 0;
                }
                else
                {
                    dataMode = new List<APIData>();
                    GetApiData.GetAPIData(string.Empty, factory, (DateTime)dateDate.Value, (DateTime)dateDate.Value, out dataMode);
                    if (dataMode != null)
                    {
                        worksheet.Cells[insertRow, 11] = dataMode[0].SewTtlManpower;
                        worksheet.Cells[insertRow, 13] = dataMode[0].SewTtlManhours;
                    }

                    insertRow++;
                }
                #endregion

                insertRow += 2;
                foreach (DataRow dr in subprocessData.Rows)
                {
                    worksheet.Cells[insertRow, 3] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                    worksheet.Cells[insertRow, 12] = MyUtility.Convert.GetString(dr["Price"]);
                    insertRow++;

                    // 插入一筆Record
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }
            #region Save & Show Excel
            excelName = saveFileName == "Sewing_R01_DailyCMPReport" ? Class.MicrosoftFile.GetName("Sewing_R01_DailyCMPReport") :
                Path.Combine(
                Env.Cfg.ReportTempDir,
                "Daily CMP Report" + ((DateTime)date).ToString("_yyyyMMdd") + DateTime.Now.ToString("_HHmmssfff") + "(" + Env.User.Factory + ").xlsx");

            excel.ActiveWorkbook.SaveAs(excelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            if (saveFileName != string.Empty)
            {
                excelName.OpenFile();
            }
            #endregion

            return true;
        }
    }
}