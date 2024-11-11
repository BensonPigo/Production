using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Sci.Data;
using Ict;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Data.SqlClient;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;
using System.Collections;
using Sci.Production.PublicPrg;

namespace Sci.Production.PublicPrg
{
    /// <inheritdoc/>
    public static partial class Prgs
    {
        private static string pTitleRow = "1:8";

        /// <summary>
        /// Trade Purchase_P01_01
        /// </summary>
        /// <inheritdoc/>
        public static bool EachConsumption(string id)
        {
            #region Each Consumption (Cutting Combo)
            DualResult res = DBProxy.Current.SelectSP(
                string.Empty,
                "Cutting_P01print_EachConsumption",
                new List<SqlParameter> { new SqlParameter("@OrderID", id) },
                out DataTable[] dts);

            if (!res)
            {
                MyUtility.Msg.ErrorBox(res.ToString(), "error");
                return false;
            }

            if (dts.Length < 2)
            {
                MyUtility.Msg.ErrorBox("no data.", string.Empty);
                return false;
            }

            DataRow dr = dts[0].Rows[0];

            string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_EachConsumptionCuttingCombo.xltx");
            sxrc sxr = new sxrc(xltPath);
            sxr.CopySheet.Add(1, dts.Length - 2);

            for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
            {
                string idxStr = (sgIdx - 1).ToString();
                string sizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();
                string markerDownloadID = dts[sgIdx].Compute("MAX(MarkerDownloadID)", string.Empty).ToString();

                dts[sgIdx].Columns.RemoveAt(1);
                dts[sgIdx].Columns.RemoveAt(0);

                Extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

                sxr.DicDatas.Add(sxr.VPrefix + "APPLYNO" + idxStr, dr["APPLYNO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "MARKERNO" + idxStr, dr["MARKERNO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "CUTTINGSP" + idxStr, dr["CUTTINGSP"]);
                sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO" + idxStr, dr["ORDERNO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, dr["STYLENO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, MyUtility.Convert.GetString(dr["QTY"]));
                sxr.DicDatas.Add(sxr.VPrefix + "FACTORY" + idxStr, dr["FACTORY"]);
                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[sgIdx]);

                // 欄位水平對齊
                for (int i = 5; i <= dt.Columns.Count - 8; i++)
                {
                    sxrc.XlsColumnInfo citbl = new sxrc.XlsColumnInfo(i, false, 6, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                    dt.LisColumnInfo.Add(citbl);
                }

                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 7, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft) { ColumnWidth = 17 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 6) { IsAutoFit = true, Alignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight, NumberFormate = "##,##0.0000 \"Y/pc\"", ColumnWidth = 11 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 5, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft) { ColumnWidth = 30 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 4, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight) { ColumnWidth = 8 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 3, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight) { ColumnWidth = 6 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight) { ColumnWidth = 8 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight) { ColumnWidth = 4 });
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight) { ColumnWidth = 10 });

                // 合併儲存格
                dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 8) } });

                dt.Borders.AllCellsBorders = true;

                sxr.DicDatas.Add(sxr.VPrefix + "tbl1" + idxStr, dt);
                sxr.DicDatas.Add(sxr.VPrefix + "SizeGroup" + idxStr, sizeGroup);
                sxr.DicDatas.Add(sxr.VPrefix + "MarkerDownloadID" + idxStr, markerDownloadID);
                pTitleRow = "1:8";
                sxrc.ReplaceAction a = ExMethod;
                sxr.DicDatas.Add(sxr.VPrefix + "exAction" + idxStr, a);
            }

            sxr.VarToSheetName = sxr.VPrefix + "SizeGroup";

            sxr.BoOpenFile = true;
            sxr.Save(Prgs.GetName("Cutting_P01_EachConsumptionCuttingCombo", ".xlsx"));
            #endregion

            return true;
        }

        /// <summary>
        /// Trade Purchase_P01_02
        /// </summary>
        /// <inheritdoc/>
        public static bool TTLConsumption(string id, string reportFormat = "T")
        {
            #region TTL consumption (PO Combo)
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@OrderID", id),
                new SqlParameter("@ReportFormat", reportFormat),
            };
            DualResult res = DBProxy.Current.SelectSP(string.Empty, "Cutting_P01print_TTLconsumption", parameters, out DataTable[] dts);

            if (!res)
            {
                MyUtility.Msg.ErrorBox(res.ToString(), "error");
                return false;
            }

            if (dts.Length < 2 || dts[1].Rows.Count <= 0)
            {
                MyUtility.Msg.ErrorBox("no data.", string.Empty);
                return false;
            }

            DataRow dr = dts[0].Rows[0];
            decimal orderQty = MyUtility.Convert.GetDecimal(dr["Qty"]);
            Extra_P01_Report_TTLconsumptionPOCombo(dts[1], orderQty, reportFormat);

            string fileName = reportFormat == "S" ? "Cutting_P01_TTLconsumptionPOCombo_Segmentation" : "Cutting_P01_TTLconsumptionPOCombo";
            int col_Segmentation = reportFormat == "S" ? 3 : 0;

            string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, $"{fileName}.xltx");
            sxrc sxr = new sxrc(xltPath, true);
            sxr.DicDatas.Add(sxr.VPrefix + "APPLYNO", dr["APPLYNO"]);
            sxr.DicDatas.Add(sxr.VPrefix + "MARKERNO", dr["MARKERNO"]);

            sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO", dr["ORDERNO"]);
            sxr.DicDatas.Add(sxr.VPrefix + "STYLENO", dr["STYLENO"]);

            sxr.DicDatas.Add(sxr.VPrefix + "QTY", MyUtility.Convert.GetString(dr["QTY"]));
            sxr.DicDatas.Add(sxr.VPrefix + "FTY", dr["FACTORY"]);

            sxr.DicDatas.Add(sxr.VPrefix + "FABTYPE", dr["FABTYPE"]);
            sxr.DicDatas.Add(sxr.VPrefix + "FLP", dr["FLP"].ToString());
            sxr.DicDatas.Add(sxr.VPrefix + "MarkerDownloadID", dr["MarkerDownloadID"]);

            sxr.DicDatas.Add(sxr.VPrefix + "Now", DateTime.Now);

            SaveXltReportCls.XltRptTable dt = new SaveXltReportCls.XltRptTable(dts[1]);

            // 欄位水平對齊
            for (int i = 3 + col_Segmentation; i <= dt.Columns.Count; i++)
            {
                SaveXltReportCls.XlsColumnInfo citbl = new SaveXltReportCls.XlsColumnInfo(i, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);

                if (i == 4 + col_Segmentation | i == 6 + col_Segmentation | i == 8 + col_Segmentation | i == 7 + col_Segmentation)
                {
                    citbl.PointCnt = 2; // 小數點兩位
                }
                else if (i == 9 + col_Segmentation)
                {
                    citbl.PointCnt = 1;
                }
                else if (i == 13 + col_Segmentation)
                {
                    citbl.PointCnt = 3;
                }

                dt.LisColumnInfo.Add(citbl);
            }

            dt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
            dt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));

            // 合併儲存格
            // dt.LisTitleMerge.Add(new Dictionary<string, string> { { "Usage", string.Format("{0},{1}", 3, 4) }, { "Purchase", string.Format("{0},{1}", 5, 6) } });
            dt.Borders.DependOnColumn.Add(1, 2);

            // 不顯示標題列
            dt.ShowHeader = false;

            sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);
            sxr.ActionAfterFillData = SetPageAutoFit;

            sxr.BoOpenFile = true;
            sxr.Save();

            // sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_TTLconsumptionPOCombo"));
            #endregion

            return true;
        }

        private static void Extra_P01_EachConsumptionCuttingCombo(DataTable dt)
        {
            string comb = string.Empty;
            string remark2 = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (comb != dr["COMB"].ToString().Trim())
                {
                    DataRow ndr = dt.NewRow();
                    ndr["COMB"] = dr["COMB"];
                    ndr["REMARK"] = dr["COMBdes"];

                    dt.Rows.InsertAt(ndr, i);
                    i += 1;

                    comb = dr["COMB"].ToString().Trim();

                    if (dr.Table.Columns.Contains("REMARK2") && !MyUtility.Check.Empty(dr["REMARK2"]))
                    {
                        ndr = dt.NewRow();
                        ndr["REMARK"] = dr["REMARK2"];
                        remark2 = dr["REMARK2"].ToString();

                        dt.Rows.InsertAt(ndr, i);
                        i += 1;
                    }
                }
                else
                {
                    // 若Remakr2不等於之前的，則插入一行新的Row for remark
                    if (dr.Table.Columns.Contains("REMARK2") && !MyUtility.Check.Empty(dr["REMARK2"])
                        && remark2 != dr["REMARK2"].ToString())
                    {
                        DataRow ndr = dt.NewRow();
                        ndr["REMARK"] = dr["REMARK2"];
                        remark2 = dr["REMARK2"].ToString();

                        dt.Rows.InsertAt(ndr, i);
                        i += 1;
                    }
                }

                dr["COMB"] = string.Empty;
            }

            dt.Columns.Remove("COMBdes");
            if (dt.Columns.Contains("REMARK2"))
            {
                dt.Columns.Remove("REMARK2");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["MarkerName"].ToString() != string.Empty && dr["REMARK"].ToString().Trim() != string.Empty & dr["COMB"].ToString().Trim() == string.Empty)
                {
                    DataRow ndr = dt.NewRow();
                    ndr["REMARK"] = dr["REMARK"];
                    dr["REMARK"] = string.Empty;
                    dt.Rows.InsertAt(ndr, i);
                    i += 1;
                }
            }
        }

        private static void ExMethod(Microsoft.Office.Interop.Excel.Worksheet oSheet, int rowNo, int columnNo)
        {
            Microsoft.Office.Interop.Excel.Range rg = oSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            int rows = rg.Row;
            bool title = false;
            for (int i = 1; i < rows; i++)
            {
                if (title == false && oSheet.Cells[i, 1].Value == "COMB")
                {
                    title = true;
                }

                if (title && oSheet.Cells[i, 2].Value != null && oSheet.Cells[i, 2].Value == string.Empty)
                {
                    string rgStr = string.Format("C{0}:{1}{0}", i, MyExcelPrg.GetExcelColumnName(rg.Column));
                    Microsoft.Office.Interop.Excel.Range rgMerge = oSheet.get_Range(rgStr);
                    rgMerge.Merge();
                    rgMerge.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }

                if (oSheet.Cells[i, 5].Value != null && oSheet.Cells[i, 5].Value.ToString() == "SIZE RATIO OF MARKER")
                {
                    Microsoft.Office.Interop.Excel.Range r = oSheet.Cells[7, 5];
                    r.Columns.AutoFit();
                }
            }

            oSheet.PageSetup.PrintTitleRows = pTitleRow;
        }

        private static void Extra_P01_Report_TTLconsumptionPOCombo(DataTable dt, decimal orderQty, string reportFormat)
        {
            string coltmp = string.Empty;
            decimal totaltmp = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col1content = dt.Rows[i][0].ToString();

                if (coltmp != col1content)
                {
                    if (coltmp == string.Empty)
                    {
                        coltmp = col1content;
                    }
                    else
                    {
                        coltmp = col1content;
                        AddSubTotalRow(dt, totaltmp, i, orderQty);

                        totaltmp = 0;
                        i += 1;
                    }
                }
                else
                {
                    dt.Rows[i][0] = DBNull.Value;
                    if (reportFormat != "T")
                    {
                        dt.Rows[i][1] = DBNull.Value;
                        dt.Rows[i][2] = DBNull.Value;
                    }

                    dt.Rows[i]["M/WIDTH"] = DBNull.Value;
                    dt.Rows[i]["M/WEIGHT"] = DBNull.Value;
                    dt.Rows[i]["STYLE DATA CONS/PC"] = DBNull.Value;
                }

                totaltmp += decimal.Parse(dt.Rows[i]["TOTAL(Inclcut. use)"].ToString());
            }

            AddSubTotalRow(dt, totaltmp, dt.Rows.Count, orderQty);
        }

        private static void SetPageAutoFit(Microsoft.Office.Interop.Excel.Worksheet wks)
        {
            wks.UsedRange.EntireColumn.AutoFit();
        }

        private static void AddSubTotalRow(DataTable dt, decimal tot, int idx, decimal orderQty)
        {
            DataRow dr = dt.NewRow();
            dr["TOTAL(Inclcut. use)"] = tot;
            dr["M/WEIGHT"] = "SubTotal";
            dr["CONS/PC"] = orderQty == 0 ? 0 : Math.Round(tot / orderQty, 3);
            dt.Rows.InsertAt(dr, idx);
        }
    }
}
