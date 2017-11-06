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
using System.Runtime.InteropServices;

namespace Sci.Production.Cutting
{
    public partial class P01_Print_OrderList : Sci.Win.Tems.QueryForm
    {
        string _id;
        int _finished;
        public P01_Print_OrderList(string args,int f = 0)
        {
            this._id = args;
            this._finished = f;
            InitializeComponent();
            EditMode = true;
        }
        
        private bool ToExcel()
        {
            if (radioCuttingWorkOrder.Checked)
            {
                #region rdCheck_CuttingWorkOrder
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_print_CuttingWorkOrder", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 3) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];
                DataRow dr2 = dts[1].Rows[0];
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_CuttingWorkOrder.xltx");
                sxrc sxr = new sxrc(xltPath);
                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");

                sxr.DicDatas.Add(sxr.VPrefix + "Title", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));
                sxr.DicDatas.Add(sxr.VPrefix + "PoList", dr["PoList"]);
                sxr.DicDatas.Add(sxr.VPrefix + "StyleID", dr["StyleID"]);
                sxr.DicDatas.Add(sxr.VPrefix + "CutLine", dr["CutLine"]);
                sxr.DicDatas.Add(sxr.VPrefix + "OrderQty", MyUtility.Convert.GetString(dr2["OrderQty"]));

                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[2]);

                dt.Borders.AllCellsBorders = true;
                
                //合併儲存格
                dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 11) } });

                //自動欄位寬度
                dt.BoAutoFitColumn = true;

                for (int i = 5; i <= dt.Columns.Count - 11; i++)
                {
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(i) { ColumnWidth = (decimal)5.38 });
                }

                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(2) { ColumnWidth = (decimal)7.38 });  //Marker Name調窄
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(3) { ColumnWidth = (decimal)7.38 });  //Fabric Combo調窄
                                
                //凍結窗格
                dt.BoFreezePanes = true;

                dt.BoAddFilter = true;
                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);

                Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                string sc = MyExcelPrg.GetExcelColumnName(dt.Columns.Count);
                wks.get_Range(string.Format("A1:{0}1", sc)).Merge();
                wks.get_Range(string.Format("A2:{0}2", sc)).Merge();
                wks.get_Range(string.Format("B3:{0}3", sc)).Merge();

                if (dts.Length == 4)
                {
                    wks.get_Range(string.Format("B{0}:{1}{2}", 6, sc, 6 + dts[3].Rows.Count)).Merge();
                    wks.get_Range(string.Format("B{0}:{1}{2}", 6, sc, 6 + dts[3].Rows.Count)).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                    // Fabric Desc
                    string txtFabricDesc = "";
                    for (int i = 0; i < dts[3].Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            txtFabricDesc += "\n\r";
                        }
                        txtFabricDesc += dts[3].Rows[i][0].ToString();
                    }
                    wks.Cells[6, 2] = txtFabricDesc;
                }

                wks.get_Range("A5").RowHeight = 16.5;
                wks.get_Range("A6").RowHeight = 16.5;
                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_CuttingWorkOrder"));
                #endregion
            }
            if (radioCuttingschedule.Checked)
            {
                #region rdCheck_CuttingSchedule
                System.Data.DataTable[] dts;
                List<SqlParameter> lsp = new List<SqlParameter>();
                lsp.Add(new SqlParameter("@M", Sci.Env.User.Keyword));
                lsp.Add(new SqlParameter("@Finished", _finished));
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_print_cuttingschedule", lsp, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 1) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_CuttingSchedule.xltx");
                sxrc sxr = new sxrc(xltPath);

                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[0]);

                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");

                sxr.DicDatas.Add(sxr.VPrefix + "Title", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));

                Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                string sc = MyExcelPrg.GetExcelColumnName(dt.Columns.Count);
                wks.get_Range(string.Format("A1:{0}1", sc)).Merge();
                wks.get_Range(string.Format("A2:{0}2", sc)).Merge();
                
                dt.Borders.AllCellsBorders = true;
                
                //自動欄位寬度
                dt.BoAutoFitColumn = true;
                dt.HeaderColor = Color.LawnGreen;
                dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(10) { ColumnWidth = (decimal)10 });
                
                //凍結窗格
                dt.BoFreezePanes = true;

                //篩選
                dt.BoAddFilter = true;

                

                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_CuttingSchedule"));
                #endregion
            }
            if (radioEachConsumption.Checked)
            {
                #region Each Consumption (Cutting Combo)
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_EachConsumption"
                    , new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_EachConsumptionCuttingCombo.xltx");
                                                                           //Cutting_P01_EachConsumptionCuttingCombo.xltx
                sxrc sxr = new sxrc(xltPath);
                sxr.CopySheet.Add(1, dts.Length - 2);

                for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
                {
                    string idxStr = (sgIdx - 1).ToString();
                    string SizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();
                    string MarkerDownloadID = dts[sgIdx].Compute("MAX(MarkerDownloadID)", "").ToString();

                    dts[sgIdx].Columns.RemoveAt(1);
                    dts[sgIdx].Columns.RemoveAt(0);

                    extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

                    sxr.DicDatas.Add(sxr.VPrefix + "APPLYNO" + idxStr, dr["APPLYNO"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "MARKERNO" + idxStr, dr["MARKERNO"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "CUTTINGSP" + idxStr, dr["CUTTINGSP"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO" + idxStr, dr["ORDERNO"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, dr["STYLENO"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, MyUtility.Convert.GetString(dr["QTY"]));
                    sxr.DicDatas.Add(sxr.VPrefix + "FACTORY" + idxStr, dr["FACTORY"]);
                    sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[sgIdx]);

                    //欄位水平對齊
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
                    //合併儲存格
                    dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 8) } });

                    dt.Borders.AllCellsBorders = true;

                    //凍結窗格
                    dt.BoFreezePanes = true;
                    dt.IntFreezeColumn = 3;

                    sxr.DicDatas.Add(sxr.VPrefix + "tbl1" + idxStr, dt);
                    sxr.DicDatas.Add(sxr.VPrefix + "SizeGroup" + idxStr, SizeGroup);
                    sxr.DicDatas.Add(sxr.VPrefix + "MarkerDownloadID" + idxStr, MarkerDownloadID);

                    sxrc.ReplaceAction a = exMethod;
                    sxr.DicDatas.Add(sxr.VPrefix + "exAction" + idxStr, a);
                }

                sxr.VarToSheetName = sxr.VPrefix + "SizeGroup";

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_EachConsumptionCuttingCombo"));
                #endregion
            }
            if (radioTTLConsumption.Checked)
            {
                #region TTL consumption (PO Combo)
                System.Data.DataTable[] dts;
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
                this.Extra_P01_Report_TTLconsumptionPOCombo(dts[1], Convert.ToInt32(dr["QTY"]));

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_TTLconsumptionPOCombo.xltx");
                sxrc sxr = new sxrc(xltPath, true);
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
                // dt.LisTitleMerge.Add(new Dictionary<string, string> { { "Usage", string.Format("{0},{1}", 3, 4) }, { "Purchase", string.Format("{0},{1}", 5, 6) } });
                dt.Borders.DependOnColumn.Add(1, 2);

                // 不顯示標題列
                dt.ShowHeader = false;

                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);
                sxr.ActionAfterFillData = this.SetPageAutoFit;
                
                sxr.BoOpenFile = true;
                sxr.Save();
                //sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_TTLconsumptionPOCombo"));                
                #endregion
            }
            if (radioColorQtyBDown.Checked)
            {
                #region Color & Q'ty B'Down (PO Combo)
                System.Data.DataTable rpt3;
                DualResult res = DBProxy.Current.Select("", "select b.POComboList,Style=StyleID+'-'+SeasonID from dbo.Orders a WITH (NOLOCK) inner join Order_POComboList b WITH (NOLOCK) on a.id = b.ID where a.ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) }, out rpt3);
                if (rpt3.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("Data not find!");
                    return false;
                }
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_ColorCombo_SizeBreakdown.xltx");

                sxrc sxr = new sxrc(xltPath);
                string POComboList = rpt3.Rows[0]["POComboList"].ToString();
                string sty = rpt3.Rows[0]["Style"].ToString();

                sxr.DicDatas.Add(sxr.VPrefix + "SP", POComboList);
                sxr.DicDatas.Add(sxr.VPrefix + "Style", sty);
                sxr.DicDatas.Add(sxr.VPrefix + "Now", DateTime.Now);

                System.Data.DataTable[] dts;
                res = DBProxy.Current.SelectSP("", "Cutting_Color_P01_OrderQtyDown_POCombo", new List<SqlParameter> { new SqlParameter("@OrderID", _id), new SqlParameter("@ByType", "2") }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 3 || (dts[0].Rows.Count <= 0 && dts[1].Rows.Count <= 0 && dts[2].Rows.Count <= 0)) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[0], 1, 2, true);
                sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[1], 1, 3);
                sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[2], 1, 0);
                SetColumn(tbl1, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                SetColumn(tbl2, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                SetColumn(tbl3, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);

                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", tbl1);
                sxr.DicDatas.Add(sxr.VPrefix + "tbl2", tbl2);
                sxr.DicDatas.Add(sxr.VPrefix + "tbl3", tbl3);

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_ColorCombo_SizeBreakdown"));
                #endregion
            }
            if (radioQtyBreakdown_PoCombbySPList.Checked)
            {
                #region rdcheck_QtyBreakdown_PoCombbySPList
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_QtyBreakdown_PoCombbySPList", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 1) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_QtyBreakdown_PoCombbySPList.xltx");
                sxrc sxr = new sxrc(xltPath);

                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");
                sxr.DicDatas.Add(sxr.VPrefix + "Title", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));
                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[0]);

                dt.Borders.AllCellsBorders = true;

                //合併儲存格
                dt.Columns["TTL"].SetOrdinal(7);
                dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE", string.Format("{0},{1}", 9, dt.Columns.Count) } });

                //凍結窗格
                dt.BoFreezePanes = true;
                dt.BoAutoFitColumn = true;
                dt.BoAddFilter = true;
                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);

                Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                string sc = MyExcelPrg.GetExcelColumnName(dt.Columns.Count);
                wks.get_Range(string.Format("A1:{0}1", sc)).Merge();
                wks.get_Range(string.Format("A2:{0}2", sc)).Merge();
                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_QtyBreakdown_PoCombbySPList"));
                #endregion
            }
            if (radioEachConsVSOrderQtyBDown.Checked)
            {
                #region Each cons. vs Order Q'ty B'Down (PO Combo)
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_Eachcons_vs_OrderQtyDown_POCombo", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2 || dts[1].Rows.Count <= 0) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];
                extra_P01_EachconsVSOrderQTYBDownPOCombo(dts[1]);

                string xltPath = Sci.Env.Cfg.XltPathDir + "\\Cutting_P01_EachconsVSOrderQTYBDownPOCombo.xltx";
                    
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(xltPath);
                excel.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");
                worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID");
                worksheet.Cells[2, 2] = dr["ORDERNO"];
                worksheet.Cells[3, 2] = dr["StyleID"];

                Microsoft.Office.Interop.Excel.Range rg1, rg2;
                if (dts[1].Rows.Count > 0)
                {                    
                    int count_header = 0;
                    int count_left = 0;
                    int count_right = 0;
                    int countRow_left = 1;
                    int countRow_Right = 1;

                    for (int i = 0; i < dts[1].Rows.Count; i++)
                    {
                        #region 塞第一頁的值與Header
                        /*
                         * 第一頁左右只能各塞44行資料(一頁共49行 = 4行Header+44資料+一行空白)
                         */
                        if (i <= 43)
                        {
                            //左側資料Header
                            worksheet.Cells[4, 1] = "#";
                            worksheet.Cells[4, 2] = "Article";
                            worksheet.Cells[4, 3] = "Size";
                            worksheet.Cells[4, 4] = "CutQty";
                            worksheet.Cells[4, 5] = "OrderQty";
                            worksheet.Cells[4, 6] = "Balance";
                            rg1 = worksheet.Range["A4", "F4"];
                            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 3;

                            /*塞第一頁左側資料*/
                            //當FabricPanelCode有東西,就對上一行加上下底框線
                            if (!MyUtility.Check.Empty(dts[1].Rows[i]["#"].ToString()))
                            {
                                worksheet.Range["A" + (4 + i), "F" + (4 + i)].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                worksheet.Range["A" + (4 + i), "F" + (4 + i)].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                            }
                            //5+i 是因為從第五行開始才塞值
                            worksheet.Cells[5 + i, 1] = dts[1].Rows[i]["#"].ToString();
                            worksheet.Cells[5 + i, 2] = dts[1].Rows[i]["Article"].ToString();
                            worksheet.Cells[5 + i, 3] = dts[1].Rows[i]["Size"].ToString();
                            worksheet.Cells[5 + i, 4] = dts[1].Rows[i]["CutQty"].ToString();
                            worksheet.Cells[5 + i, 5] = dts[1].Rows[i]["orderQty"].ToString();
                            worksheet.Cells[5 + i, 6] = dts[1].Rows[i]["Balance"].ToString();

                        }
                        //塞第一頁右側資料
                        else if (i >= 44 && i < 88)
                        {
                            worksheet.Cells[4, 8] = "#";
                            worksheet.Cells[4, 9] = "Article";
                            worksheet.Cells[4, 10] = "Size";
                            worksheet.Cells[4, 11] = "CutQty";
                            worksheet.Cells[4, 12] = "OrderQty";
                            worksheet.Cells[4, 13] = "Balance";
                            rg2 = worksheet.Range["H" + (4), "M" + (4)];
                            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                            rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 3;

                            if (!MyUtility.Check.Empty(dts[1].Rows[i]["#"].ToString()))
                            {
                                worksheet.Range["H" + (4 + i - 44), "M" + (4 + i - 44)].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                worksheet.Range["H" + (4 + i - 44), "M" + (4 + i - 44)].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                            }
                            /*
                             * 5 + i - 44 
                             * 從第五行開始塞資料
                             * 需要跟左側資料平行,需要減掉左側i=44的資料筆數
                             */
                            worksheet.Cells[5 + i - (44), 8] = dts[1].Rows[i]["#"].ToString();
                            worksheet.Cells[5 + i - (44), 9] = dts[1].Rows[i]["Article"].ToString();
                            worksheet.Cells[5 + i - (44), 10] = dts[1].Rows[i]["Size"].ToString();
                            worksheet.Cells[5 + i - (44), 11] = dts[1].Rows[i]["CutQty"].ToString();
                            worksheet.Cells[5 + i - (44), 12] = dts[1].Rows[i]["orderQty"].ToString();
                            worksheet.Cells[5 + i - (44), 13] = dts[1].Rows[i]["Balance"].ToString();
                        }
                        #endregion
                        #region 塞第二頁以後的值與Header
                        else
                        {
                            /*
                             * 塞第二頁以後的資料
                             * 塞進Excel的資料 以及DataTable的資料必須分開計算
                             * Excel行數計算:
                             *      必須從第50行算起,每一頁只能存在46行,必須以46的倍數放進Excel
                             * DataTable筆數計算:
                             *      每一頁只能放46行資料,每頁第一行放Header + 44行的資料 + 1行的空白分隔
                             *      所以44的倍數計算                             
                             * */
                            //左側資料Header
                            if ((i % 44 == 0) && ((i / 44) % 2 == 0))
                            {
                                worksheet.Cells[50 + (count_header * 46), 1] = "#";
                                worksheet.Cells[50 + (count_header * 46), 2] = "Article";
                                worksheet.Cells[50 + (count_header * 46), 3] = "Size";
                                worksheet.Cells[50 + (count_header * 46), 4] = "CutQty";
                                worksheet.Cells[50 + (count_header * 46), 5] = "OrderQty";
                                worksheet.Cells[50 + (count_header * 46), 6] = "Balance";
                                rg1 = worksheet.Range["A" + (50 + (count_header * 46)), "F" + (50 + (count_header * 46))];
                                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 3;
                                count_left++;
                                countRow_left = 1;
                            }
                            //右側資料Header
                            else if (i % 44 == 0 && (i / 44) % 2 != 0)
                            {
                                worksheet.Cells[50 + (count_header * 46), 8] = "#";
                                worksheet.Cells[50 + (count_header * 46), 9] = "Article";
                                worksheet.Cells[50 + (count_header * 46), 10] = "Size";
                                worksheet.Cells[50 + (count_header * 46), 11] = "CutQty";
                                worksheet.Cells[50 + (count_header * 46), 12] = "OrderQty";
                                worksheet.Cells[50 + (count_header * 46), 13] = "Balance";
                                rg2 = worksheet.Range["H" + (50 + (count_header * 46)), "M" + (50 + (count_header * 46))];
                                rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                                rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                rg2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 3;
                                count_right++;
                                count_header++;
                                countRow_Right = 1;

                            }

                            //左側的資料
                            if (i >= count_left * 2 * 44 && i < (count_left * 2 + 1) * 44)
                            {
                                int BordeCnt = 50 + (46 * (count_left - 1)) + (countRow_left - 1);
                                if (!MyUtility.Check.Empty(dts[1].Rows[i]["#"].ToString()))
                                {
                                    worksheet.Range["A" + BordeCnt, "F" + BordeCnt].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                    worksheet.Range["A" + BordeCnt, "F" + BordeCnt].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                                }
                                worksheet.Cells[50 + (46 * (count_left - 1)) + countRow_left, 1] = dts[1].Rows[i]["#"].ToString();
                                worksheet.Cells[50 + (46 * (count_left - 1)) + countRow_left, 2] = dts[1].Rows[i]["Article"].ToString();
                                worksheet.Cells[50 + (46 * (count_left - 1)) + countRow_left, 3] = dts[1].Rows[i]["Size"].ToString();
                                worksheet.Cells[50 + (46 * (count_left - 1)) + countRow_left, 4] = dts[1].Rows[i]["CutQty"].ToString();
                                worksheet.Cells[50 + (46 * (count_left - 1)) + countRow_left, 5] = dts[1].Rows[i]["orderQty"].ToString();
                                worksheet.Cells[50 + (46 * (count_left - 1)) + countRow_left, 6] = dts[1].Rows[i]["Balance"].ToString();
                                countRow_left++;

                            }
                            //右側的資料                          
                            else
                            {
                                int BorderCnt = 50 + (46 * (count_right - 1)) + (countRow_Right - 1);
                                if (!MyUtility.Check.Empty(dts[1].Rows[i]["#"].ToString()))
                                {
                                    worksheet.Range["H" + BorderCnt, "M" + BorderCnt].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                    worksheet.Range["H" + BorderCnt, "M" + BorderCnt].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 3;
                                }
                                worksheet.Cells[50 + (46 * (count_right - 1)) + countRow_Right, 8] = dts[1].Rows[i]["#"].ToString();
                                worksheet.Cells[50 + (46 * (count_right - 1)) + countRow_Right, 9] = dts[1].Rows[i]["Article"].ToString();
                                worksheet.Cells[50 + (46 * (count_right - 1)) + countRow_Right, 10] = dts[1].Rows[i]["Size"].ToString();
                                worksheet.Cells[50 + (46 * (count_right - 1)) + countRow_Right, 11] = dts[1].Rows[i]["CutQty"].ToString();
                                worksheet.Cells[50 + (46 * (count_right - 1)) + countRow_Right, 12] = dts[1].Rows[i]["orderQty"].ToString();
                                worksheet.Cells[50 + (46 * (count_right - 1)) + countRow_Right, 13] = dts[1].Rows[i]["Balance"].ToString();
                                countRow_Right++;
                            }
                        #endregion
                        }
                    }
                }

                #region Save & Show Excel
                string strExcelName = MyUtility.Excel.GetRandomFileName("Cutting_P01_EachconsVSOrderQTYBDownPOCombo");
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks[1];
                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion 
                this.HideWaitMessage();
                return true;

                #endregion
            }
            if (radioMarkerList.Checked)
            {
                #region Marker List
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "cutting_P01_MarkerList", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 2) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                DataRow dr = dts[0].Rows[0];

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "cutting_P01_MarkerList.xltx");
                sxrc sxr = new sxrc(xltPath, true);
                sxr.CopySheet.Add(1, dts.Length - 2);

                for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
                {
                    string idxStr = (sgIdx - 1).ToString();
                    string SizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();

                    dts[sgIdx].Columns.RemoveAt(0);

                    extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

                    sxr.DicDatas.Add(sxr.VPrefix + "REPORTNAME" + idxStr, dr["REPORTNAME"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO" + idxStr, dr["ORDERNO"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, dr["STYLENO"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, dr["QTY"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "FACTORY" + idxStr, dr["FACTORY"]);
                    sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[sgIdx]);

                    #region 補上空白的SizeCode
                    int SizeCodeCnt = dt.Columns.Count - 2 - 3;
                    int addEmptySizecode = 8 - SizeCodeCnt;
                    for (int i = 0; i < addEmptySizecode; i++)
                    {
                        dt.Columns.Add(new string(' ', i + 1));
                        dt.Columns[dt.Columns.Count - 1].SetOrdinal(dt.Columns.Count - 3);
                    }
                    #endregion

                    //欄位水平對齊
                    for (int i = 4; i <= dt.Columns.Count - 2; i++)
                    {
                        sxrc.XlsColumnInfo citbl = new sxrc.XlsColumnInfo(i, false, 6, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                        dt.LisColumnInfo.Add(citbl);
                    }
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(3, false, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count - 1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(dt.Columns.Count, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter));
                    dt.Borders.AllCellsBorders = true;

                    //合併儲存格
                    dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 4, dt.Columns.Count - 2) } });
                    sxr.DicDatas.Add(sxr.VPrefix + "tbl1" + idxStr, dt);
                    //凍結窗格
                    dt.BoFreezePanes = true;
                    dt.IntFreezeColumn = 3;
                    dt.LisColumnInfo.Add(new sxrc.XlsColumnInfo(2) { ColumnWidth = (decimal)5.88 });
                    sxr.DicDatas.Add(sxr.VPrefix + "Now", DateTime.Now);
                    sxr.DicDatas.Add(sxr.VPrefix + "SizeGroup" + idxStr, SizeGroup);

                    Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
                    wks.Range["B3", "B3"].WrapText = 1;
                    wks.get_Range("A3").RowHeight = 16.5;
                    wks.get_Range("A4").RowHeight = 16.5;
                    sxrc.ReplaceAction a = exMethod;
                    sxr.DicDatas.Add(sxr.VPrefix + "exAction" + idxStr, a);
                    
                }
                sxr.VarToSheetName = sxr.VPrefix + "SizeGroup";

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("cutting_P01_MarkerList"));
                #endregion
            }
            if (radioConsumptionCalculateByMarkerListConsPerPC.Checked)
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
                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");
                sxr.DicDatas.Add(sxr.VPrefix + "Title", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));
                sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO", dr["ORDERNO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "STYLENO", dr["STYLENO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "QTY", MyUtility.Convert.GetString(dr["QTY"]));
                sxr.DicDatas.Add(sxr.VPrefix + "FTY", dr["FACTORY"]);
                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[1]);
                dt.ShowHeader = false;

                //欄位水平對齊
                for (int i = 1; i <= dt.Columns.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.XlHAlign xha = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    if (i <= 2)
                        xha = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;


                    sxrc.XlsColumnInfo citbl = new sxrc.XlsColumnInfo(i, true, 0, xha);
                    if (i == 5 | i == 8 | i == 10 | i == 12)
                    {
                        citbl.PointCnt = 2; //小數點兩位
                    }
                    else if (i == 4)
                    {
                        citbl.PointCnt = 4;
                    }
                    dt.LisColumnInfo.Add(citbl);
                }

                dt.Borders.DependOnColumn.Add(1, 2);

                //合併儲存格
                dt.LisTitleMerge.Add(new Dictionary<string, string> { { "Usage", "7,8" }, { "Purchase", "9,10" } });
                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);
                sxr.DicDatas.Add(sxr.VPrefix + "Now", DateTime.Now);

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_ConsumptionCalculatebyMarkerListConsPerpc"));
                #endregion
            }
            
            return true;
        }

        private void SetPageAutoFit(Microsoft.Office.Interop.Excel.Worksheet wks)
        {
            wks.UsedRange.EntireColumn.AutoFit();
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

        private void Extra_P01_Report_TTLconsumptionPOCombo(DataTable dt, int qty)
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
                        this.AddSubTotalRow(dt, totaltmp, i, qty);

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

            this.AddSubTotalRow(dt, totaltmp, dt.Rows.Count, qty);
        }
        private void AddSubTotalRow(DataTable dt, decimal tot, int idx, int qty)
        {
            DataRow dr = dt.NewRow();
            dr["TOTAL(Inclcut. use)"] = tot;
            dr["M/WEIGHT"] = "SubTotal";
            dr["CONS/PC"] = qty == 0 ? 0 : Math.Round(tot / qty, 3);
            dt.Rows.InsertAt(dr, idx);
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
                        {                            
                            //預防不同FabricPanelCode,相同Article計算Total會把上一筆資料給刪除
                            if (MyUtility.Check.Empty(dt.Rows[i - 1]["Size"].ToString()))
                            {
                                dt.Rows.RemoveAt(i - 1);
                            }
                                                     
                        }
                            

                        sCutQty = 0;
                        sOrderQty = 0;
                        sBalance = 0;
                        if (exRow) i += 2;
                    }
                }
                else
                {

                }
                //防止加總上一個Total,避免Total重複相加
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
