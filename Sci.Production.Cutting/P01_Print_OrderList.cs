using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Data.SqlClient;
using System.Linq;
using Sci.Utility.Excel;

namespace Sci.Production.Cutting
{
    public partial class P01_Print_OrderList : Sci.Win.Tems.QueryForm
    {
        string _id;

        public P01_Print_OrderList(string args)
        {
            this._id = args;
            InitializeComponent();
            EditMode = true;
        }

        private bool ToExcel()
        {
            if (rdCheck1.Checked)
            {
                #region Each Consumption (Cutting Combo)
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_EachConsumption"
                    , new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_EachConsumptionCuttingCombo.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.CopySheet.Add(1, dts.Length - 2);

                for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
                {
                    string idxStr = (sgIdx == 1) ? "" : (sgIdx - 1).ToString();
                    string SizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();
                    string MarkerDownloadID = dts[sgIdx].Compute("MAX(MarkerDownloadID)", "").ToString();

                    dts[sgIdx].Columns.RemoveAt(1);
                    dts[sgIdx].Columns.RemoveAt(0);

                    extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

                    sxr.dicDatas.Add(sxr._v + "APPLYNO" + idxStr, dr["APPLYNO"]);
                    sxr.dicDatas.Add(sxr._v + "MARKERNO" + idxStr, dr["MARKERNO"]);
                    sxr.dicDatas.Add(sxr._v + "CUTTINGSP" + idxStr, dr["CUTTINGSP"]);
                    sxr.dicDatas.Add(sxr._v + "ORDERNO" + idxStr, dr["ORDERNO"]);
                    sxr.dicDatas.Add(sxr._v + "STYLENO" + idxStr, dr["STYLENO"]);
                    sxr.dicDatas.Add(sxr._v + "QTY" + idxStr, MyUtility.Convert.GetString(dr["QTY"]));
                    sxr.dicDatas.Add(sxr._v + "FACTORY" + idxStr, dr["FACTORY"]);
                    sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[sgIdx]);

                    //欄位水平對齊
                    for (int i = 5; i <= dt.Columns.Count - 8; i++)
                    {
                        sxrc.xlsColumnInfo citbl = new sxrc.xlsColumnInfo(i, false, 6, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                        dt.lisColumnInfo.Add(citbl);
                    }
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 7, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 6, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 5, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 4, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 3, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    //合併儲存格
                    dt.lisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 8) } });

                    dt.Borders.AllCellsBorders = true;

                    //凍結窗格
                    dt.boFreezePanes = true;
                    dt.intFreezeColumn = 3;

                    sxr.dicDatas.Add(sxr._v + "tbl1" + idxStr, dt);
                    sxr.dicDatas.Add(sxr._v + "SizeGroup" + idxStr, SizeGroup);
                    //sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);
                    sxr.dicDatas.Add(sxr._v + "MarkerDownloadID" + idxStr, MarkerDownloadID);

                    sxrc.ReplaceAction a = exMethod;
                    sxr.dicDatas.Add(sxr._v + "exAction" + idxStr, a);
                }

                sxr.VarToSheetName = sxr._v + "SizeGroup";

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck2.Checked)
            {
                #region TTL consumption (PO Combo)
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_TTLconsumption", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2 || dts[1].Rows.Count <= 0) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];
                extra_P01_Report_TTLconsumptionPOCombo(dts[1], Convert.ToInt32(dr["QTY"]));

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_TTLconsumptionPOCombo.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.dicDatas.Add(sxr._v + "ORDERNO", dr["ORDERNO"]);
                sxr.dicDatas.Add(sxr._v + "STYLENO", dr["STYLENO"]);

                sxr.dicDatas.Add(sxr._v + "QTY", MyUtility.Convert.GetString(dr["QTY"]));
                sxr.dicDatas.Add(sxr._v + "FTY", dr["FACTORY"]);

                sxr.dicDatas.Add(sxr._v + "FABTYPE", dr["FABTYPE"]);
                sxr.dicDatas.Add(sxr._v + "FLP", dr["FLP"].ToString());
                sxr.dicDatas.Add(sxr._v + "MarkerDownloadID", dr["MarkerDownloadID"]);

                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);

                sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[1]);

                //欄位水平對齊
                for (int i = 3; i <= dt.Columns.Count; i++)
                {
                    sxrc.xlsColumnInfo citbl = new sxrc.xlsColumnInfo(i, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);

                    if (i == 4 | i == 6 | i == 8 | i == 7)
                    {
                        citbl.PointCnt = 2; //小數點兩位
                    }
                    else if (i == 9)
                    {
                        citbl.PointCnt = 0;
                    }
                    else if (i == 13)
                    {
                        citbl.PointCnt = 3;
                    }
                    dt.lisColumnInfo.Add(citbl);
                }
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                //合併儲存格
                //dt.lisTitleMerge.Add(new Dictionary<string, string> { { "Usage", string.Format("{0},{1}", 3, 4) }, { "Purchase", string.Format("{0},{1}", 5, 6) } });
                dt.Borders.DependOnColumn.Add(1, 2);

                //不顯示標題列
                dt.ShowHeader = false;

                sxr.dicDatas.Add(sxr._v + "tbl1", dt);

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck3.Checked)
            {
                #region Color & Q'ty B'Down (PO Combo)
                System.Data.DataTable rpt3;
                DualResult res = DBProxy.Current.Select("", "select b.POComboList,Style=StyleID+'-'+SeasonID from dbo.Orders a WITH (NOLOCK) inner join Order_POComboList b WITH (NOLOCK) on a.id = b.ID where a.ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) }, out rpt3);
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_ColorCombo_SizeBreakdown.xltx");

                sxrc sxr = new sxrc(xltPath);
                string POComboList = rpt3.Rows[0]["POComboList"].ToString();
                string sty = rpt3.Rows[0]["Style"].ToString();

                sxr.dicDatas.Add(sxr._v + "SP", POComboList);
                sxr.dicDatas.Add(sxr._v + "Style", sty);
                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);

                System.Data.DataTable[] dts;
                res = DBProxy.Current.SelectSP("", "Cutting_Color_P01_OrderQtyDown_POCombo", new List<SqlParameter> { new SqlParameter("@OrderID", _id), new SqlParameter("@ByType", "2") }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 3 || (dts[0].Rows.Count <= 0 && dts[1].Rows.Count <= 0 && dts[2].Rows.Count <= 0)) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                sxrc.xltRptTable tbl1 = new sxrc.xltRptTable(dts[0], 1, 2, true);
                sxrc.xltRptTable tbl2 = new sxrc.xltRptTable(dts[1], 1, 3);
                sxrc.xltRptTable tbl3 = new sxrc.xltRptTable(dts[2], 1, 0);
                SetColumn(tbl1, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                SetColumn(tbl2, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                SetColumn(tbl3, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);

                sxr.dicDatas.Add(sxr._v + "tbl1", tbl1);
                sxr.dicDatas.Add(sxr._v + "tbl2", tbl2);
                sxr.dicDatas.Add(sxr._v + "tbl3", tbl3);

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck4.Checked)
            {
                #region Each cons. vs Order Q'ty B'Down (PO Combo)
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_Eachcons_vs_OrderQtyDown_POCombo", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2 || dts[1].Rows.Count <= 0) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];
                extra_P01_EachconsVSOrderQTYBDownPOCombo(dts[1]);

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_EachconsVSOrderQTYBDownPOCombo.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.dicDatas.Add(sxr._v + "SPNO", dr["ORDERNO"]);
                sxr.dicDatas.Add(sxr._v + "Style", dr["StyleID"]);
                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);

                sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[1]);

                //欄位水平對齊
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(3, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(4, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(5, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(6, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));

                //第二欄(Color)改為文字
                sxrc.xlsColumnInfo xlc1 = new sxrc.xlsColumnInfo(dt.Columns[1].ColumnName);
                xlc1.NumberFormate = "@";
                dt.lisColumnInfo.Add(xlc1);

                dt.Borders.InsideVertical = false;
                dt.Borders.OutsideVertical = false;
                dt.Borders.DependOnColumn.Add(1, 4);
                dt.TotalBorders.DependOnColumn.Add(3, "Sub.TTL:");

                sxr.dicDatas.Add(sxr._v + "tbl1", dt);

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck5.Checked)
            {
                #region Marker List
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "cutting_P01_MarkerList", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "cutting_P01_MarkerList.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.CopySheet.Add(1, dts.Length - 2);

                for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
                {
                    string idxStr = sgIdx == 1 ? "" : (sgIdx - 1).ToString();
                    string SizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();

                    dts[sgIdx].Columns.RemoveAt(0);

                    extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

                    sxr.dicDatas.Add(sxr._v + "REPORTNAME" + idxStr, dr["REPORTNAME"]);
                    sxr.dicDatas.Add(sxr._v + "ORDERNO" + idxStr, dr["ORDERNO"]);
                    sxr.dicDatas.Add(sxr._v + "STYLENO" + idxStr, dr["STYLENO"]);
                    sxr.dicDatas.Add(sxr._v + "QTY" + idxStr, MyUtility.Convert.GetString(dr["QTY"]));
                    sxr.dicDatas.Add(sxr._v + "FACTORY" + idxStr, dr["FACTORY"]);
                    sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[sgIdx]);

                    //欄位水平對齊
                    for (int i = 4; i <= dt.Columns.Count - 2; i++)
                    {
                        sxrc.xlsColumnInfo citbl = new sxrc.xlsColumnInfo(i, false, 6, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                        dt.lisColumnInfo.Add(citbl);
                    }
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(3, false, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter));
                    dt.Borders.AllCellsBorders = true;

                    //合併儲存格
                    dt.lisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 4, dt.Columns.Count - 2) } });

                    //凍結窗格
                    dt.boFreezePanes = true;
                    dt.intFreezeColumn = 3;

                    sxr.dicDatas.Add(sxr._v + "tbl1" + idxStr, dt);
                    //sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);
                    sxr.dicDatas.Add(sxr._v + "SizeGroup" + idxStr, SizeGroup);

                    sxrc.ReplaceAction a = exMethod;
                    sxr.dicDatas.Add(sxr._v + "exAction" + idxStr, a);

                }
                sxr.VarToSheetName = sxr._v + "SizeGroup";

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck6.Checked)
            {
                #region Consumption Calculate by Marker List Cons/Per pc
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_ConsumptionCalculatebyMarkerListConsPerpc", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2 || dts[1].Rows.Count <= 0) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];
                extra_P01_ConsumptionCalculatebyMarkerListConsPerpc(dts[1]);

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_ConsumptionCalculatebyMarkerListConsPerpc.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.dicDatas.Add(sxr._v + "ORDERNO", dr["ORDERNO"]);
                sxr.dicDatas.Add(sxr._v + "STYLENO", dr["STYLENO"]);
                sxr.dicDatas.Add(sxr._v + "QTY", MyUtility.Convert.GetString(dr["QTY"]));
                sxr.dicDatas.Add(sxr._v + "FTY", dr["FACTORY"]);
                sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[1]);
                dt.ShowHeader = false;

                //欄位水平對齊
                for (int i = 1; i <= dt.Columns.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.XlHAlign xha = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    if (i <= 2)
                        xha = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;


                    sxrc.xlsColumnInfo citbl = new sxrc.xlsColumnInfo(i, true, 0, xha);
                    if (i == 5 | i == 8 | i == 10 | i == 12)
                    {
                        citbl.PointCnt = 2; //小數點兩位
                    }
                    else if (i == 4)
                    {
                        citbl.PointCnt = 4;
                    }
                    dt.lisColumnInfo.Add(citbl);
                }

                dt.Borders.DependOnColumn.Add(1, 2);

                //合併儲存格
                dt.lisTitleMerge.Add(new Dictionary<string, string> { { "Usage", "7,8" }, { "Purchase", "9,10" } });
                sxr.dicDatas.Add(sxr._v + "tbl1", dt);
                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck_CuttingWorkOrder.Checked)
            {
                #region rdCheck_CuttingWorkOrder
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_print_CuttingWorkOrder", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];
                DataRow dr2 = dts[1].Rows[0];
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_CuttingWorkOrder.xltx");
                sxrc sxr = new sxrc(xltPath);

                sxr.dicDatas.Add(sxr._v + "PoList", dr["PoList"]);
                sxr.dicDatas.Add(sxr._v + "StyleID", dr["StyleID"]);
                sxr.dicDatas.Add(sxr._v + "CutLine", dr["CutLine"]);
                sxr.dicDatas.Add(sxr._v + "OrderQty", MyUtility.Convert.GetString(dr2["OrderQty"]));
                
                sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[2]);
                
                dt.Borders.AllCellsBorders = true;

                //合併儲存格
                dt.lisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 11) } });

                //凍結窗格
                dt.boFreezePanes = true;
                dt.boAutoFitColumn = true;
                dt.boAddFilter = true;
                sxr.dicDatas.Add(sxr._v + "tbl1", dt);

                Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                string sc = MyExcelPrg.GetExcelColumnName(dt.Columns.Count);
                wks.get_Range(string.Format("A1:{0}1",sc)).Merge();
                wks.get_Range(string.Format("A2:{0}2", sc)).Merge();
                wks.get_Range(string.Format("B3:{0}3", sc)).Merge();
                sxr.boOpenFile = true;
                sxr.Save();
                #endregion
            }
            if (rdcheck_QtyBreakdown_PoCombbySPList.Checked)
            {
                #region rdcheck_QtyBreakdown_PoCombbySPList
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_QtyBreakdown_PoCombbySPList", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 1) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_QtyBreakdown_PoCombbySPList.xltx");
                sxrc sxr = new sxrc(xltPath);
                
                sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[0]);

                dt.Borders.AllCellsBorders = true;

                //合併儲存格
                dt.lisTitleMerge.Add(new Dictionary<string, string> { { "SIZE", string.Format("{0},{1}", 9, dt.Columns.Count) } });

                //凍結窗格
                dt.boFreezePanes = true;
                dt.boAutoFitColumn = true;
                dt.boAddFilter = true;
                sxr.dicDatas.Add(sxr._v + "tbl1", dt);

                Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                string sc = MyExcelPrg.GetExcelColumnName(dt.Columns.Count);
                wks.get_Range(string.Format("A1:{0}1", sc)).Merge();
                wks.get_Range(string.Format("A2:{0}2", sc)).Merge();
                sxr.boOpenFile = true;
                sxr.Save();
                #endregion
            }

            return true;
        }

        void SetColumn(sxrc.xltRptTable tbl, Microsoft.Office.Interop.Excel.XlHAlign Alignment)
        {
            sxrc.xlsColumnInfo xlc1 = new sxrc.xlsColumnInfo(tbl.Columns[0].ColumnName);
            xlc1.NumberFormate = "@";
            tbl.lisColumnInfo.Add(xlc1);

            for (int i = 1; i < tbl.Columns.Count; i++)
            {
                sxrc.xlsColumnInfo xlc = new sxrc.xlsColumnInfo(tbl.Columns[i].ColumnName);
                xlc.Alignment = Alignment;
                tbl.lisColumnInfo.Add(xlc);
            }
        }
        
        void extra_P01_EachConsumptionCuttingCombo(DataTable dt)
        {
            string COMB = "";
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
                dr["COMB"] = "";
            }

            dt.Columns.Remove("COMBdes");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["REMARK"].ToString().Trim() != "" & dr["COMB"].ToString().Trim() == "")
                {
                    DataRow ndr = dt.NewRow();
                    ndr["REMARK"] = dr["REMARK"];
                    dr["REMARK"] = "";
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
                    title = true;

                if (title && oSheet.Cells[i, 2].Value != null && oSheet.Cells[i, 2].Value == "")
                {
                    string rgStr = string.Format("C{0}:{1}{0}", i, MyExcelPrg.GetExcelColumnName(rg.Column));
                    Microsoft.Office.Interop.Excel.Range rgMerge = oSheet.get_Range(rgStr);
                    rgMerge.Merge();
                    rgMerge.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter; ;
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
            string coltmp = "";
            decimal totaltmp = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col1content = dt.Rows[i][0].ToString();

                if (coltmp != col1content)
                {

                    if (coltmp == "")
                    {
                        coltmp = col1content;
                    }
                    else
                    {
                        coltmp = col1content;
                        addSubTotalRow(dt, totaltmp, i, Qty);

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

            addSubTotalRow(dt, totaltmp, dt.Rows.Count, Qty);
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
            addTotal(dt, 1, "Sub.TTL:", true);
            addTotal(dt, 0, "Total:", false);
            removeRepeat(dt, 0, true);
            removeRepeat(dt, 1, false);
        }

        void addTotal(DataTable dt, int cidx, string txt, bool exRow)
        {
            string col2tmp = "";
            decimal sCutQty = 0;
            decimal sOrderQty = 0;
            decimal sBalance = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string col2 = dt.Rows[i][cidx].ToString();
                if (col2 == "") continue;
                if (col2tmp != col2)
                {
                    if (col2tmp == "")
                    {
                        col2tmp = col2;
                    }
                    else
                    {
                        col2tmp = col2;
                        addSubTotalRow(txt, dt, sCutQty, sOrderQty, sBalance, i);

                        if (exRow)
                            dt.Rows.InsertAt(dt.NewRow(), i + 1);
                        else
                            dt.Rows.RemoveAt(i - 1);

                        sCutQty = 0;
                        sOrderQty = 0;
                        sBalance = 0;
                        if (exRow) i += 2;
                    }
                }
                else
                {

                }

                sCutQty += decimal.Parse(dt.Rows[i]["CutQty"].ToString());
                sOrderQty += decimal.Parse(dt.Rows[i]["OrderQty"].ToString());
                sBalance += decimal.Parse(dt.Rows[i]["Balance"].ToString());
            }

            addSubTotalRow(txt, dt, sCutQty, sOrderQty, sBalance, dt.Rows.Count);
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
            string col2tmp = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string col2 = dt.Rows[i][cidx].ToString();
                if (col2 == "") continue;
                if (col2tmp != col2)
                {
                    if (col2tmp == "")
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
                dt.Rows[i][cidx] = "";
            }
        }

        void extra_P01_ConsumptionCalculatebyMarkerListConsPerpc(DataTable dt)
        {
            //removeRepeat(dt, new int[] {0,1} );
            ChangeColumnDataType(dt, "Q'ty/PCS", typeof(string));
            ChangeColumnDataType(dt, "CONSUMPTION.", typeof(string));
            ChangeColumnDataType(dt, "CONSUMPTION", typeof(string));

            addTotal(dt, new int[] { 0, 1 });
            removeRepeat(dt, new int[] { 0, 1 });
        }

        private bool ChangeColumnDataType(System.Data.DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
                return false;

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
                return true;

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
                    catch { }
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
            string col2tmp = "";

            decimal sOrderQty = 0;
            decimal sUsageCon = 0;
            string Unit = "";

            string PUnit = "";
            decimal PCon = 0;
            string PLUSName = "";
            decimal Total = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string col2 = ""; //dt.Rows[i][cidx].ToString();
                for (int k = 0; k < cidx.Length; k++)
                {
                    col2 += dt.Rows[i][cidx[k]].ToString();
                }

                if (col2 == "") continue;
                if (col2tmp != col2)
                {
                    if (col2tmp == "")
                    {
                        col2tmp = col2;
                    }
                    else
                    {
                        col2tmp = col2;
                        addSubTotalRow_06(Unit, dt, sOrderQty, sUsageCon, i);
                        addSubTotalRow_06_2(PUnit, PLUSName, dt, PCon, Total, i + 1);
                        //if (exRow)
                        //    dt.Rows.InsertAt(dt.NewRow(), i + 1);
                        //else
                        //    dt.Rows.RemoveAt(i - 1);

                        sOrderQty = 0;
                        sUsageCon = 0;

                        i += 3;
                        //if (exRow) i += 2;
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

            addSubTotalRow_06(Unit, dt, sOrderQty, sUsageCon, dt.Rows.Count);
            addSubTotalRow_06_2(PUnit, PLUSName, dt, PCon, Total, dt.Rows.Count);
        }

        void removeRepeat(DataTable dt, int[] cidx)
        {
            string col2tmp = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string col2 = "";// dt.Rows[i][cidx].ToString();
                for (int k = 0; k < cidx.Length; k++)
                {
                    col2 += dt.Rows[i][cidx[k]].ToString();
                }
                if (col2 == "") continue;
                if (col2tmp != col2)
                {
                    if (col2tmp == "")
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
                    dt.Rows[i][cidx[k]] = "";
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
            ToExcel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
