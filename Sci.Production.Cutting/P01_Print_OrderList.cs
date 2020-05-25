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
using System.Collections;
using Sci.Production.PublicPrg;

namespace Sci.Production.Cutting
{
    public partial class P01_Print_OrderList : Sci.Win.Tems.QueryForm
    {
        string _id;
        int _finished;
        private DataTable dtColor;

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
                #region 處理排序(動態), 因合併裁只以小大的 seq 之 Sizecode 為排序
                DataColumnCollection columns = dts[2].Columns;
                int size1 = 4;
                int size2 = 4;
                if (columns.Contains("Cutno"))
                {
                    size1 = columns.IndexOf("Cutno") + 1;
                }
                if (columns.Contains("SP#"))
                {
                    size2 = columns.IndexOf("SP#") - 1;
                }

                DataTable dt2 = dts[2].Clone();
                for (int i = size2; i >= size1; i--)
                {
                    string sizeName = dts[2].Columns[i].ColumnName;
                    string filter = $"[{sizeName}] is not null";
                    DataRow[] dr2s = dts[2].Select(filter, "Color,Fabric Combo,Marker name");
                    
                    if (dr2s.Length > 0)
                    {
                        DataTable dt2f = dr2s.CopyToDataTable();
                        dt2.Merge(dt2f);
                        dts[2].Select(filter).Delete();
                    }
                }

                #endregion
                sxrc.XltRptTable dt = new sxrc.XltRptTable(dt2);

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
                DualResult result;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01_QtyBreakdown_PoCombbySPList", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);


                if (!res) { MyUtility.Msg.ErrorBox(res.ToString(), "error"); return false; }
                if (dts.Length < 1) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }              
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_QtyBreakdown_PoCombbySPList.xltx");
                // keepApp=true 產生excel後才可修改編輯                
                sxrc sxr = new sxrc(xltPath, keepApp: true);
                sxr.BoOpenFile = true;
                DataTable dtQtyList;
                DataTable dtOrder;
                result = DBProxy.Current.Select("", string.Format(@"select dbo.getPOComboList('{0}','{0}') as qtylist ", _id),out dtQtyList);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString(), "error");
                    return false;
                }
                result = DBProxy.Current.Select("", string.Format(@"select id,StyleID,SeasonID,CdCodeID,FactoryID,BrandID,ProgramID from orders where id='{0}'", _id), out dtOrder);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString(), "error");
                    return false;
                }


                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");
                sxr.DicDatas.Add(sxr.VPrefix + "Title", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));
                sxrc.XltRptTable dt = new sxrc.XltRptTable(dts[0]);  

                #region Sheet1
                //合併儲存格
                dt.Columns["TTL"].SetOrdinal(7);
                dt.LisTitleMerge.Add(new Dictionary<string, string> { { "SIZE", string.Format("{0},{1}", 9, dt.Columns.Count) } });

                //凍結窗格
                dt.Borders.AllCellsBorders = true;
                dt.BoFreezePanes = true;
                dt.BoAutoFitColumn = true;
                dt.BoAddFilter = true;
                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);
                

                Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.Sheets[1];               
                string sc = MyExcelPrg.GetExcelColumnName(dt.Columns.Count);               
                wks.get_Range(string.Format("A1:{0}1", sc)).Merge();
                wks.get_Range(string.Format("A2:{0}2", sc)).Merge();
                #endregion

                #region Sheet2
                DataTable[] dt_Sheet2;
                result = DBProxy.Current.SelectSP("", "Cutting_P01_QtyBreakdown_PoCombbySPList_TableOfGoods", new List<SqlParameter> { new SqlParameter("@PoID", _id) }, out dt_Sheet2);
                if (!result) { MyUtility.Msg.ErrorBox(result.ToString(), "error"); return false; }
                if (dt_Sheet2.Length < 1) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }
                if (dt_Sheet2[0].Rows.Count < 1) { MyUtility.Msg.ErrorBox("no data.", ""); return false; }

                // 計算orders.Qty
                DataTable dtQty;
                result = DBProxy.Current.Select("", string.Format(@"select sum(qty) as totalQty from orders	where poid='{0}' ", _id), out dtQty);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString(), "error");
                    return false;
                }
               
                // 需要先刪除dt_Sheet2[0]的 Article欄位
                DataTable dtSheet2 = dt_Sheet2[0].Copy();
                dtSheet2.Columns.Remove("Article");
                sxrc.XltRptTable dt2 = new sxrc.XltRptTable(dtSheet2);
                sxr.DicDatas.Add(sxr.VPrefix + "Title2", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));
                sxr.DicDatas.Add(sxr.VPrefix + "QTYSP", "QTY_SP_NO:" + dtQtyList.Rows[0]["qtylist"].ToString() + "= " + dtQty.Rows[0]["totalQty"] + "PCS");
                sxr.DicDatas.Add(sxr.VPrefix + "Style", "STYLE: " + dtOrder.Rows[0]["StyleID"].ToString() + " - " + dtOrder.Rows[0]["SeasonID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "CDCode", "CD CCODE: " + dtOrder.Rows[0]["CdCodeID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "Factory", "FACTORY: " + dtOrder.Rows[0]["FactoryID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "Customer", "CUSTOMER: " + dtOrder.Rows[0]["BrandID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "Program", "PROGRAM: " + dtOrder.Rows[0]["ProgramID"].ToString());
                DataTable dtMarkerNo;
                result = DBProxy.Current.Select("", string.Format(@"select top 1 MarkerNo from order_eachcons where id='{0}'", _id), out dtMarkerNo);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString(), "error");
                    return false;
                }
                sxr.DicDatas.Add(sxr.VPrefix + "MarkerNO", "Marker NO: " + MyUtility.GetValue.Lookup(string.Format(@"select top 1 MarkerNo from order_eachcons where id='{0}'", _id), null));
                sxr.DicDatas.Add(sxr.VPrefix + "MCHandle", "MC Handle: " + MyUtility.GetValue.Lookup(string.Format(@"select b.Name+'-'+b.ExtNo from orders a inner join pass1 b on a.mchandle=b.id where a.id='{0}'", _id), null));
                sxr.DicDatas.Add(sxr.VPrefix + "LocalMR", "Local MR: " + MyUtility.GetValue.Lookup(string.Format(@"select b.Name+'-'+b.ExtNo from orders a inner join pass1 b on a.LocalMR =b.id where a.id='{0}'", _id), null));
                sxr.DicDatas.Add(sxr.VPrefix + "Date", "Date: " + DateTime.Now.ToString("MM/dd/yyyy"));
                sxr.DicDatas.Add(sxr.VPrefix + "Planner", "Planner: " + MyUtility.GetValue.Lookup(string.Format(@"select b.ID+'-'+b.ExtNo from cutting a inner join pass1 b on a.EditName =b.id where a.id='{0}'", _id), null));

                // 設定Sheet2 格式
                Microsoft.Office.Interop.Excel.Worksheet wkcolor = sxr.ExcelApp.Sheets[2];
                string sc2 = MyExcelPrg.GetExcelColumnName(dt2.Columns.Count);
                int rowCnt = dt_Sheet2[0].Rows.Count + 8;
                wkcolor.get_Range(string.Format("A2:{0}2", sc2)).Merge();
                wkcolor.get_Range(string.Format("A3:{0}3", sc2)).Merge();   
                sxr.FontName = "Times New Roman";
                sxr.DicDatas.Add(sxr.VPrefix + "tbl2", dtSheet2);
                #endregion

                // sxr.BoOpenFile = false;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("Cutting_P01_QtyBreakdown_PoCombbySPList"));
                
                // 不同的Article 需要合併以及變色次數
                DataTable dtArticle;
                MyUtility.Tool.ProcessWithDatatable(dt_Sheet2[0], "article", "select distinct article from #tmp where article <>'ZZ'", out dtArticle);              

                // 依照Article數量取得隨機的顏色
                RandomColor(dtArticle);

                #region 合併Sheet2表身Columns
                DataTable dtSewLing;
                MyUtility.Tool.ProcessWithDatatable(dt_Sheet2[0], "article,Sewing Line", "select distinct article,[Sewing Line] from #tmp where [Sewing Line] is not null order by article,[Sewing Line]", out dtSewLing);

                DataTable dtMerge;
                MyUtility.Tool.ProcessWithDatatable(dt_Sheet2[0], "SHELL A/ SIZE,Article", "select distinct [SHELL A/ SIZE],Article from #tmp order by Article,[SHELL A/ SIZE] ", out dtMerge);

                int rangeMerge1 = 0;
                int rangeMerge2 = 0;
                int artChg_Count = 0;
                int lineChg_Count = 0;
                for (int ii = 0; ii < dtSheet2.Rows.Count; ii++)
                {
                    string currentArticle = MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)wkcolor.Cells[9 + ii, 1]).Text);
                    string currentLine = MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)wkcolor.Cells[9 + ii, 2]).Text);
                    string mergeArticle = dtMerge.Rows[artChg_Count]["SHELL A/ SIZE"].ToString();
                    string mergeSewLine = string.Empty;
                    if (dtSewLing.Rows.Count > 0)
                    {
                        mergeSewLine = dtSewLing.Rows[lineChg_Count]["Sewing Line"].ToString();
                    }

                    if (mergeArticle == currentArticle)
                    {
                        rangeMerge1++;

                        // 拿當前資料比較下一筆如果Aricle不同,就合併相同的範圍
                        if (mergeArticle != MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)wkcolor.Cells[9 + ii + 1, 1]).Text))
                        {
                            // 合併
                            wkcolor.Range[string.Format("A{0}", 9 + ii - rangeMerge1 + 1), string.Format("A{0}", 9 + ii)].Merge();
                            // 水平置中
                            wkcolor.Range[string.Format("A{0}", 9 + ii - rangeMerge1 + 1), string.Format("A{0}", 9 + ii)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            // 垂直置中
                            wkcolor.Range[string.Format("A{0}", 9 + ii - rangeMerge1 + 1), string.Format("A{0}", 9 + ii)].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                            // 拿來比較的值不可超出範圍,更新後的值須要回頭比對上一筆資料
                            if (artChg_Count < dtMerge.Rows.Count - 1)
                            {
                                artChg_Count++;
                            }

                            // 歸0
                            rangeMerge1 = 0;
                        }

                        // 合併 Sewing Line
                        if (mergeSewLine== currentLine && !MyUtility.Check.Empty(mergeSewLine))
                        {
                            rangeMerge2++;

                            // 下一筆sewingline不同,代表當前欄位為合併的終點
                            if (mergeSewLine != MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)wkcolor.Cells[9 + ii + 1, 2]).Text))
                            {
                                wkcolor.Range[string.Format("B{0}", 9 + ii - rangeMerge2 + 1), string.Format("B{0}", 9 + ii)].Merge();
                                // 水平置中
                                wkcolor.Range[string.Format("B{0}", 9 + ii - rangeMerge2 + 1), string.Format("B{0}", 9 + ii)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                // 垂直置中
                                wkcolor.Range[string.Format("B{0}", 9 + ii - rangeMerge2 + 1), string.Format("B{0}", 9 + ii)].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                                if (lineChg_Count < dtSewLing.Rows.Count - 1)
                                {
                                    lineChg_Count++;
                                }
                                rangeMerge2 = 0;
                            }
                        }                      
                    }                   
                }
                #endregion

                #region 填入Sheet2 底部的Line
                // 依Line分類
                // 計算SewingLine 底部清單放置位置 9=表身資料第一筆 + 總共表身資料筆數 + 1(一行Date+一行空白)
                int countLine = 9 + dt_Sheet2[0].Rows.Count + 2;
                DataTable dtBottom = dt_Sheet2[1].Copy();
                DataTable distLine;
                int countTotal = 0;

                MyUtility.Tool.ProcessWithDatatable(dt_Sheet2[1], "SewingLineID", "select distinct SewingLineID from #tmp order by SewingLineID", out distLine);

                for (int li = 0; li < distLine.Rows.Count; li++)
                {
                    wkcolor.Cells[countLine + countTotal + li, 1] = "LINE " + distLine.Rows[li]["SewingLineID"];
                    wkcolor.Cells[countLine + countTotal + li, 1].Font.Bold = true;

                    DataTable dtline2;
                    MyUtility.Tool.ProcessWithDatatable(dtBottom, "SewingLineID,colorid,colorName,Article,Category,SizeList,No,Qty,buyerdelivery", string.Format("select * from #tmp where SewingLineID='{0}' order by SewingLineID,Category desc,buyerdelivery,article", distLine.Rows[li]["SewingLineID"]), out dtline2);

                    // 組合SewingLine 資料
                    for (int lii = 0; lii < dtline2.Rows.Count; lii++)
                    {
                        wkcolor.Cells[countLine + li + countTotal + 1, 1] = dtline2.Rows[lii]["colorid"] + "-" + dtline2.Rows[lii]["ColorName"] + "<" + dtline2.Rows[lii]["Article"] + "> " + "SIZE:" + dtline2.Rows[lii]["SizeList"] + "(" + dtline2.Rows[lii]["No"] + ")" + "," + "Qty=" + dtline2.Rows[lii]["qty"] + "PCS";
             
                        // 取得相同Article 紅綠藍顏色
                        int red = (int)dtColor.Select(string.Format("ArticleNo='{0}'", dtline2.Rows[lii]["Article"]))[0]["RedNo"];
                        int green = (int)dtColor.Select(string.Format("ArticleNo='{0}'", dtline2.Rows[lii]["Article"]))[0]["greenNo"];
                        int blue = (int)dtColor.Select(string.Format("ArticleNo='{0}'", dtline2.Rows[lii]["Article"]))[0]["blueNo"];
                        // 變色
                        wkcolor.Range[string.Format("A{0}", countLine + li + countTotal + 1), string.Format("A{0}", countLine + li + countTotal + 1)].Font.Color = Color.FromArgb(red, green, blue);

                        countTotal++;
                    }

                }
                #endregion

                #region Sheet2 表身變色
                // 顏色群組,以原始table計算,差別在原始有article,產生的Excel沒有
                string range = MyExcelPrg.GetExcelColumnName(dtSheet2.Columns.Count - 7);
                string wholRange = MyExcelPrg.GetExcelColumnName(dtSheet2.Columns.Count);
                wkcolor.Range["A8", string.Format("{0}8", wholRange)].Interior.Color = Color.FromArgb(146, 208, 80);
                wkcolor.Range["A8", string.Format("{0}8", wholRange)].Font.Color = Color.FromArgb(255, 0, 0);

                int ArticleID = 0;
                for (int i = 0; i < dt_Sheet2[0].Rows.Count; i++)
                {

                    string currentArticle = dt_Sheet2[0].Rows[i]["Article"].ToString();
                    string distArticle = dtArticle.Rows[ArticleID]["Article"].ToString();
                    string currentLine = dt_Sheet2[0].Rows[i]["Sewing Line"].ToString();

                    int red = MyUtility.Convert.GetInt(dtColor.Rows[ArticleID]["redNo"]);
                    int green = MyUtility.Convert.GetInt(dtColor.Rows[ArticleID]["greenNo"]);
                    int blue = MyUtility.Convert.GetInt(dtColor.Rows[ArticleID]["blueNo"]);     

                    if (currentArticle==distArticle)
                    {
                        wkcolor.Range[string.Format("A{0}", 9 + i), string.Format("{0}{1}", range, 9 + i)].Font.Color = Color.FromArgb(red, green, blue);
                    }
                    else
                    {
                        wkcolor.Range[string.Format("A{0}", 9 + i - 1), string.Format("{0}{1}", wholRange, 9 + i - 1)].Interior.Color = Color.FromArgb(146, 208, 80);
                        if (ArticleID < dtArticle.Rows.Count - 1)
                        {
                            ArticleID++;
                            i--;
                        }
                    }
                }
                // 改變最後Total 顏色
                wkcolor.Range[string.Format("A{0}", 8 + dt_Sheet2[0].Rows.Count), string.Format("{0}{1}", wholRange, 8 + dt_Sheet2[0].Rows.Count)].Interior.Color = Color.FromArgb(146, 208, 80);
                wkcolor.Range[string.Format("{0}8", range), string.Format("{0}{1}", range,dt2.Rows.Count+8)].Interior.Color = Color.FromArgb(146, 208, 80);
                wkcolor.Range[string.Format("A{0}", 8 + dt_Sheet2[0].Rows.Count), string.Format("{0}{1}", wholRange, 8 + dt_Sheet2[0].Rows.Count)].Font.Color = Color.FromArgb(255, 0, 0);
                #endregion

                // 產生報表後的額外處理
                // 畫框線 必須要Save完後處理,不然框線範圍會是Sheet1+Sheet2的總和
                #region 畫框線
                Microsoft.Office.Interop.Excel.Range rg1;
                rg1 = wkcolor.Range["A8", string.Format("{0}{1}", sc2, rowCnt)];                
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;
                #endregion

                int dt2ColsCnt = dt2.Columns.Count;
                int dt2RowsCnt = dt2.Rows.Count;
                #region 加總

                // 取得所有Size
                DataTable dtSize;
                DBProxy.Current.Select("", string.Format(@"
select distinct sizecode,Seq
	from Order_SizeCode 
	where sizecode in (
	SELECT oq.sizecode	
         FROM   orders o WITH (nolock) 
                INNER JOIN order_qty oq WITH (nolock) 
                        ON o.id = oq.id 
                LEFT JOIN order_article oa WITH (nolock) 
                       ON oa.id = oq.id 
                          AND oa.article = oq.article 
				left join order_colorcombo oc
				 ON oa.article = oc.article 
                 AND oc.patternpanel = 'FA' 
                 AND oc.id = o.poid 
         WHERE  o.poid = '{0}'
	)
	and id = '{0}'
	order by Seq
",_id), out dtSize);

                DataTable dtTotalNub = new DataTable();
                dtTotalNub.Columns.Add("NubPosition", typeof(int));
                for (int i = 0; i < dt2RowsCnt; i++)
                {
                    // 直向Total
                    wkcolor.Cells[9 + i,dt2ColsCnt - 7 ].Value = string.Format("=SUM({0}{1}:{2}{1})", MyExcelPrg.GetExcelColumnName(3), 9 + i, MyExcelPrg.GetExcelColumnName(dt2ColsCnt -8));

                    // 取得相同的Article 數量
                    DataRow[] drArtCnt = dt_Sheet2[0].Select(string.Format("Article='{0}' and SPNO <>'' ", dt_Sheet2[0].Rows[i]["Article"]));

                    // 取得報表為Total columns
                    string TotalCol =MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)wkcolor.Cells[9 + i, 1]).Text);

                    // 字串為空值代表被合併存格所以跳下一筆資料
                    if (TotalCol.Length==0)
                    {
                        continue;
                    }
                    // 橫向Total
                    if (TotalCol.ToString().Substring(0,5).ToUpper() == "TOTAL")
                    {
                        for (int ii  = 0; ii < dtSize.Rows.Count  ; ii++)
                        {
                            // if: 非最後一行, else: 最後一行加總
                            if (dt2RowsCnt != i + 1)
                            {
                                wkcolor.Cells[9 + i, 3 + ii].Value = string.Format("=SUM({0}{1}:{0}{2})", MyExcelPrg.GetExcelColumnName(3 + ii), 9 + i - drArtCnt.Length, 9 + i - 1);  
                            }
                            else
                            {
                                string sumValue = "";
                                for (int iii = 0; iii < dtTotalNub.Rows.Count; iii++)
                                {
                                    sumValue = sumValue + string.Format("{0}{1}", MyExcelPrg.GetExcelColumnName(3 + ii), dtTotalNub.Rows[iii]["NubPosition"]) + ",";
                                }
                                wkcolor.Cells[9 + i, 3 + ii].Value = string.Format("=SUM({0})", sumValue.ToString().Substring(0, sumValue.Length - 1));
                            }
                        }
                        DataRow dr = dtTotalNub.NewRow();
                        dr["NubPosition"] = 9 + i;
                        dtTotalNub.Rows.Add(dr);

                        // 合併 Total
                        wkcolor.Range[string.Format("A{0}", 9 + i), string.Format("B{0}", 9 + i)].Merge();
                    }

                }
               
                

                #endregion

                #region 後續Excel格式設定

                // 自動篩選
                rg1.AutoFilter(1);
                // 最適欄寬
                wkcolor.Cells.EntireColumn.AutoFit();
                // 自動換列                
                wkcolor.Range["A8", string.Format("A{0}", rowCnt)].WrapText = true;
                wkcolor.Range["A8", string.Format("{0}8", sc2)].WrapText = true;

                // 調整欄位 SHELL A/ SIZE 寬度
                wkcolor.Columns[1].ColumnWidth = 12;
                // 調整欄位 Sewing Line 寬度
                wkcolor.Columns[2].ColumnWidth = 7;             

                int cntTotal = dt2ColsCnt - 6;
                // 計算size欄位位置
                for (int i = 3; i < cntTotal; i++)
                {
                    // 將所有Size欄位寬度調整為5.5
                    wkcolor.Columns[i].ColumnWidth = 5.5;
                }

                // 調整欄位 EX-FTY Date 寬度
                wkcolor.Columns[dt2ColsCnt - 1].ColumnWidth = 11;
                // 加入註解                
                wkcolor.Cells.ClearComments();
                wkcolor.Cells[8, dt2ColsCnt - 1].AddComment("One day in advance and Exclude weekend");
                // 調整欄位 TOTAL 寬度
                wkcolor.Columns[dt2ColsCnt - 7].ColumnWidth = 10;
                // 調整欄位 SPNO 寬度
                wkcolor.Columns[dt2ColsCnt - 6].ColumnWidth = 13;
                // 調整欄位 KIT 寬度
                wkcolor.Columns[dt2ColsCnt - 5].ColumnWidth = 10;
                // 調整欄位 OrderNo 寬度
                wkcolor.Columns[dt2ColsCnt - 4].ColumnWidth = 10;
                // 調整欄位 P.O.No 寬度
                wkcolor.Columns[dt2ColsCnt - 3].ColumnWidth = 10;
                // 調整欄位CUST CD 寬度
                wkcolor.Columns[dt2ColsCnt - 2].ColumnWidth = 13;

                // 調整Sheet2表身Table header
                wkcolor.Rows[8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                wkcolor.Rows[8].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                wkcolor.Rows[8].RowHeight = 48;

                // 將表身數字欄位都置中
                Microsoft.Office.Interop.Excel.Range rgBody = wkcolor.Range["A9", string.Format("{0}{1}", MyExcelPrg.GetExcelColumnName(dt2ColsCnt - 7), rowCnt)];
                rgBody.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                rgBody.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // 將表身欄位 :SPNO,KIT,OrderNo,P.O.No,CUST CD 都自動換列置中
                Microsoft.Office.Interop.Excel.Range rgBody2 = wkcolor.Range[string.Format("{0}9", MyExcelPrg.GetExcelColumnName(dt2ColsCnt - 6)), string.Format("{0}{1}", MyExcelPrg.GetExcelColumnName(dt2ColsCnt - 2), rowCnt)];
                rgBody2.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter; 
                rgBody2.WrapText = true;

                // 合併
                string spstring = MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)wkcolor.Cells[4, 1]).Text);
                var c = (char)10;
                int cntLine = spstring.Length / 110;
                string spValue = "";

                // 超過110字元,加上換行符號
                if (cntLine > 0)
                {
                    for (int i = 1; i <= cntLine + 1; i++)
                    {
                        spValue = spValue + spstring.Substring(110 * (i - 1),
                            (spstring.Length - (110 * (i - 1))) > 110 ? 110 : spstring.Length - 110 * (i - 1));
                        if ((spstring.Length - (110 * (i - 1))) > 110)
                        {
                            spValue= spValue + c;
                        }                        
                    }                   
                    wkcolor.Cells[4, 1].Value = spValue;                    
                    wkcolor.Rows[4].RowHeight = 16 * (cntLine + 1);
                }
               
                Microsoft.Office.Interop.Excel.Range rgsp = wkcolor.Range["A4", string.Format("{0}4", MyExcelPrg.GetExcelColumnName(dt2ColsCnt))];
                rgsp.Merge();              
                rgBody.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                rgsp.WrapText = true;

                // 設定從第幾行開始凍結                
                wkcolor.Activate();
                wkcolor.Range["A9"].Select();
                // 進行凍結視窗
                wkcolor.Application.ActiveWindow.FreezePanes = true;
                #endregion

                sxr.FinishSave();               
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
                SaveXltReportCls sxr = new SaveXltReportCls(xltPath, true);
                sxr.AllowRangeTransferToString = false;
                string Cuttingfactory = MyUtility.GetValue.Lookup("FactoryID", _id, "Cutting", "ID");
                sxr.DicDatas.Add(sxr.VPrefix + "Title", MyUtility.GetValue.Lookup("NameEN", Cuttingfactory, "Factory", "ID"));
                sxr.DicDatas.Add(sxr.VPrefix + "ORDERNO", dr["ORDERNO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "STYLENO", dr["STYLENO"]);
                sxr.DicDatas.Add(sxr.VPrefix + "QTY", MyUtility.Convert.GetString(dr["QTY"]));
                sxr.DicDatas.Add(sxr.VPrefix + "FTY", dr["FACTORY"]);
                SaveXltReportCls.XltRptTable dt = new SaveXltReportCls.XltRptTable(dts[1]);
                dt.ShowHeader = false;

                //欄位水平對齊
                for (int i = 1; i <= dt.Columns.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.XlHAlign xha = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    if (i <= 3) // <= 2
                    {
                        xha = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    }

                    SaveXltReportCls.XlsColumnInfo citbl = new SaveXltReportCls.XlsColumnInfo(i, true, 0, xha);
                    if (i == 6 | i == 9 | i == 11 | i == 13)
                    {
                        citbl.PointCnt = 2; // 小數點兩位
                        citbl.NumberFormate = "##,##0.0#";
                    }
                    else if (i == 5)
                    {
                        citbl.NumberFormate = "##,##0.0000";
                    }
                    else if (i == 4)
                    {
                        citbl.NumberFormate = "@";
                    }

                    dt.LisColumnInfo.Add(citbl);
                }

                dt.Borders.DependOnColumn.Add(1, 2);

                //合併儲存格
                // dt.LisTitleMerge.Add(new Dictionary<string, string> { { "Usage", "7,8" }, { "Purchase", "9,10" } });
                sxr.DicDatas.Add(sxr.VPrefix + "tbl1", dt);
                sxr.DicDatas.Add(sxr.VPrefix + "Now", DateTime.Now);
                sxr.ActionAfterFillData = this.SetPageAutoFit;

                sxr.BoOpenFile = true;
                sxr.Save();
                #endregion
            }
            
            return true;
        }

        private void RandomColor(DataTable dtArticle)
        {
            // dtColor 增加三個欄位 NO=用來隨機取樣的編號, ID=Color識別ID, Name=color顏色
            dtColor = new DataTable();
            dtColor.Columns.Add("CountNo", typeof(int));
            dtColor.Columns.Add("ArticleNo", typeof(string));
            dtColor.Columns.Add("redNo", typeof(int));
            dtColor.Columns.Add("greenNo", typeof(int));
            dtColor.Columns.Add("blueNo", typeof(int));

            Hashtable ht = new Hashtable();
            ht.Add(0, "188,16,127");        ht.Add(1, "34,4,252");
            ht.Add(2, "102,0,204");         ht.Add(3, "204,51,0");
            ht.Add(4, "255,51,153");        ht.Add(5, "0,102,102");
            ht.Add(6, "102,51,0");          ht.Add(7, "255,51,0");
            ht.Add(8, "204,0,255");         ht.Add(9, "51,102,204");
            ht.Add(10, "204,0,255");        ht.Add(11, "239,79,67");
            ht.Add(12, "255,0,102");        ht.Add(13, "79,98,40");
            ht.Add(14, "0,102,153");        ht.Add(15, "153,51,255");
            ht.Add(16, "165,0,33");         ht.Add(17, "0,51,0");
            ht.Add(18, "255,0,0");          ht.Add(19, "0,0,153");
            ht.Add(20, "255,0,255");        ht.Add(21, "0,128,128");
            ht.Add(22, "255,102,0");        ht.Add(23, "0,51,204");
            ht.Add(24, "153,0,153");        ht.Add(25, "51,102,153");
            ht.Add(26, "153,0,153");        ht.Add(27, "188,16,127");
            ht.Add(28, "153,0,153");        ht.Add(29, "34,4,252");
            ht.Add(30, "255,0,102");        ht.Add(31, "79,98,40");

            for (int i = 0; i < dtArticle.Rows.Count; i++)
            {
                string[] cols = ht[(i > 31) ? i - 32 : i].ToString().Split(',');

                DataRow dr = dtColor.NewRow();
                dr["CountNo"] = i + 1;
                dr["ArticleNo"] = dtArticle.Rows[i]["article"].ToString();
                dr["redNo"] = cols[0];
                dr["greenNo"] = cols[1];
                dr["blueNo"] = cols[2];
                dtColor.Rows.Add(dr);

            }            

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
            this.ChangeColumnDataType(dt, "Q'ty/PCS", typeof(string));
            this.ChangeColumnDataType(dt, "CONSUMPTION.", typeof(string));
            this.ChangeColumnDataType(dt, "CONSUMPTION", typeof(string));

            AddTotal(dt, new int[] { 0, 1 });
            this.RemoveRepeat(dt, new int[] { 0, 1, 2 });
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

        void AddTotal(DataTable dt, int[] cidx)
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

        private void RemoveRepeat(DataTable dt, int[] cidx)
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
                    ////
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
            ToExcel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
