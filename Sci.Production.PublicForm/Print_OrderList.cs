using System;
using System.Collections.Generic;
using System.Data;
using Sci.Data;
using Ict;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Data.SqlClient;
using Sci.Utility.Excel;

namespace Sci.Production.PublicForm
{
    public partial class Print_OrderList : Win.Tems.QueryForm
    {
        string _id;
        int _finished;

        public Print_OrderList(string args, int f = 0)
        {
            this._id = args;
            this._finished = f;
            this.InitializeComponent();
            this.EditMode = true;
        }

        private bool ToExcel()
        {
            if (this.radioEachConsumption.Checked)
            {
                #region Each Consumption (Cutting Combo)
                DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP(string.Empty, "Cutting_P01print_EachConsumption",
                    new List<SqlParameter> { new SqlParameter("@OrderID", this._id) }, out dts);

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

                                                                           // Cutting_P01_EachConsumptionCuttingCombo.xltx
                sxrc sxr = new sxrc(xltPath);
                sxr.CopySheet.Add(1, dts.Length - 2);

                for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
                {
                    string idxStr = (sgIdx - 1).ToString();
                    string SizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();
                    string MarkerDownloadID = dts[sgIdx].Compute("MAX(MarkerDownloadID)", string.Empty).ToString();

                    dts[sgIdx].Columns.RemoveAt(1);
                    dts[sgIdx].Columns.RemoveAt(0);

                    this.extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

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

                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 7, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 6, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 5, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 4, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 3, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));

                    // 合併儲存格
                    dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 8) } });

                    dt.Borders.AllCellsBorders = true;

                    // 凍結窗格
                    dt.BoFreezePanes = true;
                    dt.IntFreezeColumn = 3;

                    sxr.DicDatas.Add(sxr.VPrefix + "tbl1" + idxStr, dt);
                    sxr.DicDatas.Add(sxr.VPrefix + "SizeGroup" + idxStr, SizeGroup);
                    sxr.DicDatas.Add(sxr.VPrefix + "MarkerDownloadID" + idxStr, MarkerDownloadID);

                    sxrc.ReplaceAction a = this.exMethod;
                    sxr.DicDatas.Add(sxr.VPrefix + "exAction" + idxStr, a);
                }

                sxr.VarToSheetName = sxr.VPrefix + "SizeGroup";

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_EachConsumptionCuttingCombo"));
                #endregion
            }

            if (this.radioTTLConsumption.Checked)
            {
                #region TTL consumption (PO Combo)
                DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP(string.Empty, "Cutting_P01print_TTLconsumption", new List<SqlParameter> { new SqlParameter("@OrderID", this._id) }, out dts);

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
                this.extra_P01_Report_TTLconsumptionPOCombo(dts[1], Convert.ToInt32(dr["QTY"]));

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_TTLconsumptionPOCombo.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO", dr["ORDERNO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "STYLENO", dr["STYLENO"]);

                sxr.DicDatas.Add(sxr.VPrefix + "QTY", MyUtility.Convert.GetString(dr["QTY"]));
                sxr.DicDatas.Add(sxr.VPrefix + "FTY", dr["FACTORY"]);

                sxr.DicDatas.Add(sxr.VPrefix + "FABTYPE", dr["FABTYPE"]);
                sxr.DicDatas.Add(sxr.VPrefix + "FLP", dr["FLP"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "MarkerDownloadID", dr["MarkerDownloadID"]);

                sxr.DicDatas.Add(sxr.VPrefix + "Now", DateTime.Now);

                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[1]);

                // 欄位水平對齊
                for (int i = 3; i <= dt.Columns.Count; i++)
                {
                    sxrc.XlsColumnInfo citbl = new sxrc.XlsColumnInfo(i, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);

                    if (i == 4 | i == 6 | i == 8 | i == 7)
                    {
                        citbl.PointCnt = 2; // 小數點兩位
                    }
                    else if (i == 9)
                    {
                        citbl.PointCnt = 0;
                    }
                    else if (i == 13)
                    {
                        citbl.PointCnt = 3;
                    }

                    dt.LisColumnInfo.Add(citbl);
                }

                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));

                // 合併儲存格
                // dt.lisTitleMerge.Add(new Dictionary<string, string> { { "Usage", string.Format("{0},{1}", 3, 4) }, { "Purchase", string.Format("{0},{1}", 5, 6) } });
                dt.Borders.DependOnColumn.Add(1, 2);

                // 不顯示標題列
                dt.ShowHeader = false;

                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_TTLconsumptionPOCombo"));
                #endregion
            }

            return true;
        }

        void SetColumn(sxrc.XltRptTable tbl, Microsoft.Office.Interop.Excel.XlHAlign Alignment)
        {
            sxrc.XlsColumnInfo xlc1 = new sxrc.XlsColumnInfo(tbl.Columns[0].ColumnName);
            xlc1.NumberFormate = "@";
            tbl.LisColumnInfo.Add(xlc1);

            for (int i = 1; i < tbl.Columns.Count; i++)
            {
                sxrc.XlsColumnInfo xlc = new sxrc.XlsColumnInfo(tbl.Columns[i].ColumnName);
                xlc.Alignment = Alignment;
                tbl.LisColumnInfo.Add(xlc);
            }
        }

        void extra_P01_EachConsumptionCuttingCombo(DataTable dt)
        {
            string COMB = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (COMB != dr["COMB"].ToString().Trim())
                {
                    DataRow ndr = dt.NewRow();
                    ndr["COMB"] = dr["COMB"];
                    ndr["REMARK"] = dr["COMBdes"];

                    dt.Rows.InsertAt(ndr, i);

                    i += 1;
                    COMB = dr["COMB"].ToString().Trim();
                }

                dr["COMB"] = string.Empty;
            }

            dt.Columns.Remove("COMBdes");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["REMARK"].ToString().Trim() != string.Empty & dr["COMB"].ToString().Trim() == string.Empty)
                {
                    DataRow ndr = dt.NewRow();
                    ndr["REMARK"] = dr["REMARK"];
                    dr["REMARK"] = string.Empty;
                    dt.Rows.InsertAt(ndr, i);
                    i += 1;
                }
            }
        }

        void exMethod(Microsoft.Office.Interop.Excel.Worksheet oSheet, int rowNo, int columnNo)
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
        }

        void extra_P01_Report_TTLconsumptionPOCombo(DataTable dt, int Qty)
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
                        this.addSubTotalRow(dt, totaltmp, i, Qty);

                        totaltmp = 0;
                        i += 1;
                    }
                }
                else
                {
                    dt.Rows[i][0] = DBNull.Value;
                    dt.Rows[i]["M/WIDTH"] = DBNull.Value;
                    dt.Rows[i]["M/WEIGHT"] = DBNull.Value;
                    dt.Rows[i]["STYLE DATA CONS/PC"] = DBNull.Value;
                }

                totaltmp += decimal.Parse(dt.Rows[i]["TOTAL(Inclcut. use)"].ToString());
            }

            this.addSubTotalRow(dt, totaltmp, dt.Rows.Count, Qty);
        }

        void addSubTotalRow(DataTable dt, decimal tot, int idx, int Qty)
        {
            DataRow dr = dt.NewRow();
            dr["TOTAL(Inclcut. use)"] = tot;
            dr["M/WEIGHT"] = "SubTotal";
            dr["CONS/PC"] = Qty == 0 ? 0 : Math.Round(tot / Qty, 3);
            dt.Rows.InsertAt(dr, idx);
        }

        void extra_P01_EachconsVSOrderQTYBDownPOCombo(DataTable dt)
        {
            this.addTotal(dt, 1, "Sub.TTL:", true);
            this.addTotal(dt, 0, "Total:", false);
            this.removeRepeat(dt, 0, true);
            this.removeRepeat(dt, 1, false);
        }

        void addTotal(DataTable dt, int cidx, string txt, bool exRow)
        {
            string col2tmp = string.Empty;
            decimal sCutQty = 0;
            decimal sOrderQty = 0;
            decimal sBalance = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col2 = dt.Rows[i][cidx].ToString();
                if (col2 == string.Empty)
                {
                    continue;
                }

                if (col2tmp != col2)
                {
                    if (col2tmp == string.Empty)
                    {
                        col2tmp = col2;
                    }
                    else
                    {
                        col2tmp = col2;
                        this.addSubTotalRow(txt, dt, sCutQty, sOrderQty, sBalance, i);

                        if (exRow)
                        {
                            dt.Rows.InsertAt(dt.NewRow(), i + 1);
                        }
                        else
                        {
                            // 預防不同FabricPanelCode,相同Article計算Total會把上一筆資料給刪除
                            if (MyUtility.Check.Empty(dt.Rows[i - 1]["Size"].ToString()))
                            {
                                dt.Rows.RemoveAt(i - 1);
                            }
                        }

                        sCutQty = 0;
                        sOrderQty = 0;
                        sBalance = 0;
                        if (exRow)
                        {
                            i += 2;
                        }
                    }
                }
                else
                {
                }

                // 防止加總上一個Total,避免Total重複相加
                if (i > 1 && dt.Rows[i - 1]["Size"].ToString() == "Total:")
                {
                    sCutQty = 0;
                    sOrderQty = 0;
                    sBalance = 0;
                }

                sCutQty += decimal.Parse(dt.Rows[i]["CutQty"].ToString());
                sOrderQty += decimal.Parse(dt.Rows[i]["OrderQty"].ToString());
                sBalance += decimal.Parse(dt.Rows[i]["Balance"].ToString());
            }

            this.addSubTotalRow(txt, dt, sCutQty, sOrderQty, sBalance, dt.Rows.Count);
        }

        void addSubTotalRow(string txt, DataTable dt, decimal sCutQty, decimal sOrderQty, decimal sBalance, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["Size"] = txt;
            dr["CutQty"] = sCutQty;
            dr["OrderQty"] = sOrderQty;
            dr["Balance"] = sBalance;
            dt.Rows.InsertAt(dr, idx);
        }

        void removeRepeat(DataTable dt, int cidx, bool addNewRow)
        {
            string col2tmp = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col2 = dt.Rows[i][cidx].ToString();
                if (col2 == string.Empty)
                {
                    continue;
                }

                if (col2tmp != col2)
                {
                    if (col2tmp == string.Empty)
                    {
                        col2tmp = col2;
                        if (addNewRow)
                        {
                            DataRow dr = dt.NewRow();
                            dr[cidx] = dt.Rows[i][cidx].ToString();
                            dt.Rows.InsertAt(dr, i);
                            i += 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        col2tmp = col2;
                        if (addNewRow)
                        {
                            DataRow dr = dt.NewRow();
                            dr[cidx] = dt.Rows[i][cidx].ToString();
                            dt.Rows.InsertAt(dr, i);
                            i += 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                }

                dt.Rows[i][cidx] = string.Empty;
            }
        }

        void extra_P01_ConsumptionCalculatebyMarkerListConsPerpc(DataTable dt)
        {
            // removeRepeat(dt, new int[] {0,1} );
            this.ChangeColumnDataType(dt, "Q'ty/PCS", typeof(string));
            this.ChangeColumnDataType(dt, "CONSUMPTION.", typeof(string));
            this.ChangeColumnDataType(dt, "CONSUMPTION", typeof(string));

            this.addTotal(dt, new int[] { 0, 1 });
            this.removeRepeat(dt, new int[] { 0, 1 });
        }

        private bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
            {
                return false;
            }

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
            {
                return true;
            }

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newtype);
                table.Columns.Add(newcolumn);

                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnname], newtype);
                    }
                    catch
                    {
                    }
                }

                newcolumn.SetOrdinal(column.Ordinal);
                table.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        void addTotal(DataTable dt, int[] cidx)
        {
            string col2tmp = string.Empty;

            decimal sOrderQty = 0;
            decimal sUsageCon = 0;
            string Unit = string.Empty;

            string PUnit = string.Empty;
            decimal PCon = 0;
            string PLUSName = string.Empty;
            decimal Total = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col2 = string.Empty; // dt.Rows[i][cidx].ToString();
                for (int k = 0; k < cidx.Length; k++)
                {
                    col2 += dt.Rows[i][cidx[k]].ToString();
                }

                if (col2 == string.Empty)
                {
                    continue;
                }

                if (col2tmp != col2)
                {
                    if (col2tmp == string.Empty)
                    {
                        col2tmp = col2;
                    }
                    else
                    {
                        col2tmp = col2;
                        this.addSubTotalRow_06(Unit, dt, sOrderQty, sUsageCon, i);
                        this.addSubTotalRow_06_2(PUnit, PLUSName, dt, PCon, Total, i + 1);

                        // if (exRow)
                        //    dt.Rows.InsertAt(dt.NewRow(), i + 1);
                        // else
                        //    dt.Rows.RemoveAt(i - 1);
                        sOrderQty = 0;
                        sUsageCon = 0;

                        i += 3;

                        // if (exRow) i += 2;
                    }
                }
                else
                {
                }

                Unit = dt.Rows[i]["Unit"].ToString();
                dt.Rows[i]["Unit"] = DBNull.Value;
                sOrderQty += decimal.Parse(dt.Rows[i]["Order Qty"].ToString());
                sUsageCon += decimal.Parse(dt.Rows[i]["CONSUMPTION"].ToString());

                PUnit = dt.Rows[i]["Unit."].ToString();
                dt.Rows[i]["Unit."] = DBNull.Value;
                PCon = decimal.Parse(dt.Rows[i]["CONSUMPTION."].ToString());
                dt.Rows[i]["CONSUMPTION."] = DBNull.Value;
                PLUSName = dt.Rows[i]["PLUS(YDS/%)"].ToString();
                dt.Rows[i]["PLUS(YDS/%)"] = DBNull.Value;
                Total = decimal.Parse(dt.Rows[i]["TOTAL"].ToString());
                dt.Rows[i]["TOTAL"] = DBNull.Value;
            }

            this.addSubTotalRow_06(Unit, dt, sOrderQty, sUsageCon, dt.Rows.Count);
            this.addSubTotalRow_06_2(PUnit, PLUSName, dt, PCon, Total, dt.Rows.Count);
        }

        void removeRepeat(DataTable dt, int[] cidx)
        {
            string col2tmp = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col2 = string.Empty; // dt.Rows[i][cidx].ToString();
                for (int k = 0; k < cidx.Length; k++)
                {
                    col2 += dt.Rows[i][cidx[k]].ToString();
                }

                if (col2 == string.Empty)
                {
                    continue;
                }

                if (col2tmp != col2)
                {
                    if (col2tmp == string.Empty)
                    {
                        col2tmp = col2;
                        continue;
                    }
                    else
                    {
                        col2tmp = col2;
                        continue;
                    }
                }
                else
                {
                }

                for (int k = 0; k < cidx.Length; k++)
                {
                    dt.Rows[i][cidx[k]] = string.Empty;
                }
            }
        }

        void addSubTotalRow_06(string Unit, DataTable dt, decimal sOrderQty, decimal sUsageCon, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["Q'ty/PCS"] = "TTL";
            dr["Unit"] = Unit;
            dr["Order Qty"] = sOrderQty;
            dr["CONSUMPTION"] = sUsageCon;
            dt.Rows.InsertAt(dr, idx);
        }

        void addSubTotalRow_06_2(string PUnit, string PLUSName, DataTable dt, decimal PCon, decimal Total, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["CONSUMPTION"] = "SubTotal";
            dr["Unit."] = PUnit;
            dr["CONSUMPTION."] = PCon;
            dr["PLUS(YDS/%)"] = PLUSName;
            dr["TOTAL"] = Total;

            dt.Rows.InsertAt(dr, idx);
            dt.Rows.InsertAt(dt.NewRow(), idx + 1);
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            this.ToExcel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
