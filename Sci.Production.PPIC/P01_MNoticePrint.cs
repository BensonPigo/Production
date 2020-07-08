using Ict;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Diagnostics;
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_MNoticePrint
    /// </summary>
    public partial class P01_MNoticePrint : Sci.Win.Tems.PrintForm
    {
        private const bool Excel = true;
        private const bool PDF = false;
        private bool Print_type = true;
        private string strFileName = string.Empty;
        private string strPDFFileName = string.Empty;
        private string _id;

        // private string _username;
        // private string _userid;
        // DualResult result;
        // DataRow CurrentDataRow ;

        /// <summary>
        /// P01_MNoticePrint
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        /// <param name="orderID">string orderID</param>
        public P01_MNoticePrint(ToolStripMenuItem menuitem, string orderID)
            : base(menuitem)
        {
            this.Constructor(orderID);
        }

        /// <summary>
        /// P01_MNoticePrint
        /// </summary>
        /// <param name="orderID">string args</param>
        public P01_MNoticePrint(string orderID)
        {
            this.Constructor(orderID);
        }

        private void Constructor(string orderID)
        {
            this._id = orderID;
            this.InitializeComponent();
            this.EditMode = true;

            foreach (var control in this.groupBox1.Controls)
            {
                if (control is Sci.Win.UI.RadioButton)
                {
                    Sci.Win.UI.RadioButton rdb = (Sci.Win.UI.RadioButton)control;
                    rdb.CheckedChanged += this.Rd_CheckedChanged;
                }
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return this.ExcelProcess(this.Print_type);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Excel processing, please wait ...");

            switch (this.Print_type)
            {
                case Excel:
                    this.strFileName.OpenFile();
                    break;
                case PDF:
                    if (ConvertToPDF.ExcelToPDF(this.strFileName, this.strPDFFileName))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(this.strPDFFileName);
                        Process.Start(startInfo);
                    }

                    break;
            }

            this.HideWaitMessage();
            return true;
        }

        private void BtnToPDF_Click(object sender, EventArgs e)
        {
            this.Print_type = PDF;
            this.OnExcelClick();

            // this.ExcelProcess(PDF);
        }

        private void Btn_toExccel_Click(object sender, EventArgs e)
        {
            this.Print_type = Excel;
            this.OnExcelClick();
        }

        private DualResult ExcelProcess(bool process)
        {
            if (this.radioMNotice.Checked == true)
            {
                string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.Orders WITH (NOLOCK) where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", this._id) });

                DataRow drvar = this.GetTitleDataByCustCD(poid, this._id);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                if (drvar == null)
                {
                    DualResult failResult = new DualResult(false, "data not found!!");
                    return failResult;
                }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice.xltx");
                sxrc sxr = new sxrc(xltPath, true);
                sxr.BoOpenFile = false;
                sxr.AddPrintRange = true;
                sxr.SetPrinterAtLocal = true;
                sxr.FontName = "Times New Roman";
                sxr.FontSize = 14;
                sxr.DicDatas.Add(sxr.VPrefix + "PO_NOW", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                sxr.DicDatas.Add(sxr.VPrefix + "PO_MAKER", drvar["MAKER"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "PO_STYLENO", drvar["sty"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "PO_QTY", drvar["QTY"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "POID", poid);
                sxr.DicDatas.Add(sxr.VPrefix + "PO_CustCD", drvar["CustCD"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "PO_SI", drvar["SI"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "Siname", string.IsNullOrEmpty(drvar["Customize2"].ToString()) ? string.Empty : drvar["Customize2"].ToString() + ":");
                sxr.DicDatas.Add(sxr.VPrefix + "PO_pono", drvar["pono"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "PO_delDate", drvar["delDate"]);
                sxr.DicDatas.Add(sxr.VPrefix + "PO_ChangeMemoDate", drvar["ChangeMemoDate"] == DBNull.Value ? string.Empty : drvar["ChangeMemoDate"]);

                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report_SizeSpec", new List<SqlParameter> { new SqlParameter("@ID", poid), new SqlParameter("@WithZ", this.checkAdditionally.Checked), new SqlParameter("@fullsize", 1) }, out dts);

                sxrc.XltRptTable xltTbl = new sxrc.XltRptTable(dts[0], 1, 0, false, 18, 2);
                for (int i = 3; i <= 18; i++)
                {
                    sxrc.XlsColumnInfo xcinfo = new sxrc.XlsColumnInfo(i, false, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                    xcinfo.NumberFormate = "@";
                    xltTbl.LisColumnInfo.Add(xcinfo);
                }

                sxr.DicDatas.Add(sxr.VPrefix + "S1_Tbl1", xltTbl);
                this.intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; // 起始位置加一、格線加二
                sxrc.ReplaceAction ra = this.ForSizeSpec;
                sxr.DicDatas.Add(sxr.VPrefix + "ExtraAction", ra);

                System.Data.DataTable dt;
                DualResult getIds = DBProxy.Current.Select(
string.Empty,
@"select a.ID, a.FactoryID as MAKER, a.StyleID+'-'+ a.SeasonID as sty, a.QTY , a.CustCdID as CustCD, a.CustPONo as pono, a.BuyerDelivery as delDate, a.Customize1 , a.Customize2 as SI,Br.Customize2
,ChangeMemoDate
from Orders a 
LEFT JOIN Brand Br ON a.BrandID = Br.ID 
where poid = @poid
and (SMnorderApv is not null or MnorderApv is not null)
order by ID",
new List<SqlParameter> { new SqlParameter("poid", poid) },
out dt);
                if (!getIds && dt.Rows.Count <= 0)
                {
                    DualResult failResult = new DualResult(false, "Error:" + getIds.ToString());
                    return failResult;
                }

                sxr.CopySheet.Add(2, dt.Rows.Count - 1);

                // 注意！其他##標籤的名字不能跟Sheet Name有任何文字排列組合相同，否則Sheet Name會被取代掉
                sxr.VarToSheetName = sxr.VPrefix + "SP";

                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(dt.Rows.Count);

                int maxRowheight = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = dt.Rows[i]["ID"].ToString();
                    string idxStr = i.ToString();

                    res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report02", new List<SqlParameter> { new SqlParameter("@ID", id), new SqlParameter("@WithZ", this.checkAdditionally.Checked) }, out dts);

                    sxr.DicDatas.Add(sxr.VPrefix + "NOW" + idxStr, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    sxr.DicDatas.Add(sxr.VPrefix + "SP" + idxStr, id);
                    sxr.DicDatas.Add(sxr.VPrefix + "MAKER" + idxStr, dt.Rows[i]["MAKER"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, dt.Rows[i]["sty"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, dt.Rows[i]["QTY"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "CustCD" + idxStr, dt.Rows[i]["CustCD"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "SI" + idxStr, dt.Rows[i]["SI"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "Siname" + idxStr, string.IsNullOrEmpty(dt.Rows[i]["Customize2"].ToString()) ? string.Empty : dt.Rows[i]["Customize2"].ToString() + ":");
                    sxr.DicDatas.Add(sxr.VPrefix + "pono" + idxStr, dt.Rows[i]["pono"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "delDate" + idxStr, dt.Rows[i]["delDate"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "ChangeMemoDate" + idxStr, dt.Rows[i]["ChangeMemoDate"] == DBNull.Value ? string.Empty : dt.Rows[i]["ChangeMemoDate"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "coms1" + idxStr, dt.Rows[i]["Customize1"].ToString());

                    string msg = this.GetSPNote(id);
                    string[] stringSeparators = new string[] { "\r\n" };
                    string[] msgs = msg.Split(stringSeparators, StringSplitOptions.None);
                    int sPNoteRowHeight = msgs.Length == 0 ? 20 : msgs.Length * 20;

                    maxRowheight = maxRowheight < sPNoteRowHeight ? sPNoteRowHeight : maxRowheight;
                    Microsoft.Office.Interop.Excel.Worksheet wks = wks = sxr.ExcelApp.Worksheets[2];
                    wks.get_Range($"A3:A3").RowHeight = maxRowheight;

                    sxr.DicDatas.Add(sxr.VPrefix + "Note" + idxStr, msg);

                    sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[0], 1, 2, true);
                    sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[1], 1, 3);
                    sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[2], 1, 0);
                    this.SetColumn1toText(tbl1);
                    this.SetColumn1toText(tbl2);
                    this.SetColumn1toText(tbl3);

                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl1" + idxStr, tbl1);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl2" + idxStr, tbl2);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl3" + idxStr, tbl3);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl4" + idxStr, dts[3]); // COLOR list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl5" + idxStr, dts[4]); // Fabric list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl6" + idxStr, dts[5]); // Accessories list

                    if (dts[6].Rows.Count > 0)
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2SHIPINGMARK" + idxStr, new sxrc.XltLongString(dts[6].Rows[0]["shipingMark"].ToString()));
                    }

                    if (dts[7].Rows.Count > 0)
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2PACKING" + idxStr, new sxrc.XltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    }

                    if (dts[8].Rows.Count > 0)
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2LH" + idxStr, new sxrc.XltLongString(dts[8].Rows[0]["Label"].ToString()));
                    }

                    if (dts[9].Rows[0].Field<bool?>("VasShas").GetValueOrDefault(false) == true)
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2VS" + idxStr, new sxrc.XltLongString(dts[9].Rows[0]["Packing2"].ToString()));
                    }
                    else
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2VS" + idxStr, new sxrc.XlsPrivateCommand() { UpDelete = 2 });
                    }
                }

                this.strFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_M_Notice");
                this.strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_M_Notice", Sci.Production.Class.PDFFileNameExtension.PDF);
                sxr.AllowRangeTransferToString = false;
                sxr.Save(this.strFileName);
                sxr.FinishSave();
            }

            // M/Notict (Combo by ComboID)
            else
            {
                string poid = MyUtility.GetValue.Lookup("select poid FROM dbo.Orders WITH (NOLOCK) where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", this._id) });

                System.Data.DataTable dtOrderCombo = this.GetDtByComboID(poid);

                if (dtOrderCombo == null)
                {
                    DualResult failResult = new DualResult(false, "data not found!!");
                    return failResult;
                }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice_Combo.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.BoOpenFile = false;
                sxr.AddPrintRange = true;
                sxr.SetPrinterAtLocal = true;
                sxr.FontName = "Times New Roman";
                sxr.FontSize = 14;
                sxr.CopySheets.Add("1,2,3", dtOrderCombo.Rows.Count - 1);
                sxr.VarToSheetName = sxr.VPrefix + "sname";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                int ii = 0;
                int maxRowheight = 0;
                int maxRowheight2 = 0;
                foreach (DataRow row in dtOrderCombo.Rows)
                {
                    string idxStr = ii.ToString();
                    ii += 1;

                    string orderComboID = row["OrderComboID"].ToString();

                    DataRow drvar = this.GetTitleDataByCustCD(poid, orderComboID);

                    System.Data.DataTable[] dts;
                    DualResult res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report04", new List<SqlParameter> { new SqlParameter("@ID", orderComboID), new SqlParameter("@WithZ", this.checkAdditionally.Checked), new SqlParameter("@ByType", 1) }, out dts);

                    // 顯示筆數於PrintForm上Count欄位
                    this.SetCount(dts[10].Rows.Count);

                    if (drvar == null | !res)
                    {
                        DualResult failResult = new DualResult(false, "data not found!!");
                        return failResult;
                    }

                    string sPs = drvar["SPNO"].ToString();
                    string msg = this.GetSPNotes(sPs);
                    string[] stringSeparators = new string[] { "\r\n" };
                    string[] msgs = msg.Split(stringSeparators, StringSplitOptions.None);
                    int sPNoteRowHeight = msgs.Length == 0 ? 20 : msgs.Length * 20;

                    // 這功能是先把每個sheet都處理完成再複製，因此每個複製出來的sheet列高都會一樣
                    maxRowheight = maxRowheight < sPNoteRowHeight ? sPNoteRowHeight : maxRowheight;

                    sxr.DicDatas.Add(sxr.VPrefix + "SheetOneNote" + idxStr, msg);
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_NOW" + idxStr, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_MAKER" + idxStr, drvar["MAKER"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_STYLENO" + idxStr, drvar["sty"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_QTY" + idxStr, drvar["QTY"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "POID" + idxStr, drvar["SPNO"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_CustCD" + idxStr, drvar["CustCD"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_SI" + idxStr, drvar["SI"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "Siname" + idxStr, string.IsNullOrEmpty(drvar["Customize2"].ToString()) ? string.Empty : drvar["Customize2"].ToString() + ":");
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_pono" + idxStr, drvar["pono"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_delDate" + idxStr, drvar["delDate"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "sname1" + idxStr, orderComboID + "-1");
                    sxr.DicDatas.Add(sxr.VPrefix + "sname2" + idxStr, orderComboID + "-2");
                    sxr.DicDatas.Add(sxr.VPrefix + "sname3" + idxStr, orderComboID + "-3");
                    sxr.DicDatas.Add(sxr.VPrefix + "PO_ChangeMemoDate" + idxStr, drvar["ChangeMemoDate"] == DBNull.Value ? string.Empty : drvar["ChangeMemoDate"]);

                    // For SizeSpec
                    sxrc.XltRptTable xltTbl = new sxrc.XltRptTable(dts[0], 1, 0, false, 18, 2);
                    for (int i = 3; i <= 18; i++)
                    {
                        sxrc.XlsColumnInfo xcinfo = new sxrc.XlsColumnInfo(i, false, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                        xcinfo.NumberFormate = "@";
                        xltTbl.LisColumnInfo.Add(xcinfo);
                    }

                    // 調整列高
                    Microsoft.Office.Interop.Excel.Worksheet wks = wks = sxr.ExcelApp.Worksheets[1];
                    wks.get_Range($"A4:A4").RowHeight = maxRowheight;

                    xltTbl.Separator1 = "'- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
                    xltTbl.Separator2 = "'= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =";
                    sxr.DicDatas.Add(sxr.VPrefix + "S1_Tbl1" + idxStr, xltTbl);
                    this.intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; // 起始位置加一、格線加二
                    sxrc.ReplaceAction ra = this.ForSizeSpec;
                    sxr.DicDatas.Add(sxr.VPrefix + "ExtraAction" + idxStr, ra);

                    sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[1], 1, 2, true);
                    sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[2], 1, 3);
                    sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[3], 1, 0);
                    this.SetColumn1toText(tbl1);
                    this.SetColumn1toText(tbl2);
                    this.SetColumn1toText(tbl3);

                    sxr.DicDatas.Add(sxr.VPrefix + "SheetTwoNote" + idxStr, msg);
                    sxr.DicDatas.Add(sxr.VPrefix + "NOW" + idxStr, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    sxr.DicDatas.Add(sxr.VPrefix + "SP" + idxStr, drvar["SPNO"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "MAKER" + idxStr, drvar["MAKER"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, drvar["sty"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, drvar["QTY"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "CustCD" + idxStr, drvar["CustCD"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "SIno" + idxStr, drvar["SI"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "pono" + idxStr, drvar["pono"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "CustPONo" + idxStr, drvar["pono"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "ChangeMemoDate" + idxStr, drvar["ChangeMemoDate"] == DBNull.Value ? string.Empty : drvar["ChangeMemoDate"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "delDate" + idxStr, drvar["delDate"]);
                    sxr.DicDatas.Add(sxr.VPrefix + "coms1" + idxStr, drvar["Customize1"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl1" + idxStr, tbl1);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl2" + idxStr, tbl2);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl3" + idxStr, tbl3); // COLOR list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl4" + idxStr, dts[4]); // Fabric list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl5" + idxStr, dts[5]); // Accessories list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl6" + idxStr, dts[6]);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2PACKING" + idxStr, new sxrc.XltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    sxr.DicDatas.Add(sxr.VPrefix + "S2LH" + idxStr, new sxrc.XltLongString(dts[8].Rows[0]["Label"].ToString()));

                    // 調整列高
                    wks = sxr.ExcelApp.Worksheets[2];
                    wks.get_Range($"A3:A3").RowHeight = maxRowheight;

                    if (dts[9].Rows[0].Field<bool?>("VasShas").GetValueOrDefault(false) == true)
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2VS" + idxStr, new sxrc.XltLongString(dts[9].Rows[0]["Packing2"].ToString()));
                    }
                    else
                    {
                        sxr.DicDatas.Add(sxr.VPrefix + "S2VS" + idxStr, new sxrc.XlsPrivateCommand() { UpDelete = 2 });
                    }

                    // 新增Range Repeat數
                    sxr.DicDatas.Add(sxr.VPrefix + "CR" + idxStr, dts[10].Rows.Count);

                    int idx = 0;

                    // Sheet3 每張SP的分隔線，只需要畫好一個，後面都是複製的
                    wks = sxr.ExcelApp.Worksheets[3];
                    wks.get_Range($"A3:S3").Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 3;

                    foreach (DataRow dr in dts[10].Rows)
                    {
                        string sIdx = idx.ToString();
                        idx += 1;

                        sxr.DicDatas.Add(sxr.VPrefix + "S3_SP" + idxStr + sxr.CRPrefix + sIdx, dr["ID"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_Style" + idxStr + sxr.CRPrefix + sIdx, dr["sty"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_QTY" + idxStr + sxr.CRPrefix + sIdx, dr["QTY"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_CUSTCD" + idxStr + sxr.CRPrefix + sIdx, dr["CustCDID"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_PoNo" + idxStr + sxr.CRPrefix + sIdx, dr["CustPONO"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_Order" + idxStr + sxr.CRPrefix + sIdx, dr["Customize1"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_DELIVERY" + idxStr + sxr.CRPrefix + sIdx, dr["BuyerDelivery"]);
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_ChangeMemoDate" + idxStr + sxr.CRPrefix + sIdx, dr["ChangeMemoDate"] == DBNull.Value ? string.Empty : dr["ChangeMemoDate"]);
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_Mark" + idxStr + sxr.CRPrefix + sIdx, new sxrc.XltLongString(dr["Mark"].ToString()));

                        string noteMsg = this.GetSPNote(dr["ID"].ToString());
                        stringSeparators = new string[] { "\r\n" };
                        msgs = noteMsg.Split(stringSeparators, StringSplitOptions.None);
                        int sPNoteRowHeight2 = msgs.Length == 0 ? 20 : msgs.Length * 20;

                        maxRowheight2 = maxRowheight2 < sPNoteRowHeight2 ? sPNoteRowHeight2 : maxRowheight2;
                        wks.get_Range($"A4:A4").RowHeight = maxRowheight2;

                        sxr.DicDatas.Add(sxr.VPrefix + "SheetThreeNote" + idxStr + sxr.CRPrefix + sIdx, noteMsg);

                        System.Data.DataTable[] dts2;
                        List<SqlParameter> lis2 = new List<SqlParameter>();
                        lis2.Add(new SqlParameter("@OrderID", dr["ID"]));
                        lis2.Add(new SqlParameter("@ByType", "0"));

                        res = DBProxy.Current.SelectSP(string.Empty, "Order_Report_QtyBreakdown", lis2, out dts2);

                        if (res)
                        {
                            sxrc.XltRptTable stbl = new sxrc.XltRptTable(dts2[0], 1, 2, true);
                            this.SetColumn1toText(stbl);
                            sxr.DicDatas.Add(sxr.VPrefix + "S3_Tbl" + idxStr + sxr.CRPrefix + sIdx, stbl);
                        }
                    }
                }

                this.strFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_Report04");
                this.strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_Report04", Sci.Production.Class.PDFFileNameExtension.PDF);

                sxr.Save(this.strFileName);
                sxr.FinishSave();
            }

            return new DualResult(true);
        }

        private System.Data.DataTable GetDtByComboID(string poid)
        {
            System.Data.DataTable dt;
            string strSqlSelect = @"
SELECT distinct OrderComboID from Orders
outer apply (select 1 as cnt from Orders tmp where tmp.OrderComboID = Orders.ID) cnt
WHERE Orders.POID = @POID
and (SMnorderApv is not null or MnorderApv is not null)
";

            DualResult res = DBProxy.Current.Select(string.Empty, strSqlSelect, new List<SqlParameter> { new SqlParameter("@POID", poid) }, out dt);

            if (res && dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        private void SetColumn1toText(sxrc.XltRptTable tbl)
        {
            sxrc.XlsColumnInfo c1 = new sxrc.XlsColumnInfo(1);
            c1.NumberFormate = "@";
            tbl.LisColumnInfo.Add(c1);
        }

        private int intSizeSpecRowCnt = 0;
        private int intSizeSpecColumnCnt = 18;

        private void ForSizeSpec(Worksheet oSheet, int rowNo, int columnNo)
        {
            for (int colIdx = 3; colIdx <= this.intSizeSpecColumnCnt; colIdx++)
            {
                // oSheet.Cells[4, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                oSheet.Cells[4, colIdx].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            }

            for (int colIdx = 3; colIdx <= this.intSizeSpecColumnCnt; colIdx++)
            {
                // oSheet.Cells[4 + intSizeSpecRowCnt, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                oSheet.Cells[4, colIdx].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            }
        }

        private DataRow GetTitleDataByCustCD(string poid, string id, bool byCustCD = true)
        {
            DataRow drvar;
            string cmd = string.Empty;
            if (byCustCD)
            {
                cmd = @"
SELECT MAKER=max(FactoryID)
,sty=max(StyleID)+'-'+max(SeasonID)
,QTY=sum(QTY)
,'SPNO'=RTRIM(POID)+b.spno 
,(select CustCDID from orders o where o.ID = @ID) as CustCD
,(select CustPONo from orders o where o.ID = @ID) as pono
,(select BuyerDelivery from orders o where o.ID = @ID) as delDate
,(select Customize1 from orders o where o.ID = @ID) as Customize1
,(select Customize2 from Orders o where o.ID = @ID) AS SI
,br.Customize2 
,(select ChangeMemoDate from orders o where o.ID = @ID) as ChangeMemoDate
FROM orders a WITH (NOLOCK) 
LEFT JOIN Brand Br ON a.BrandID = Br.ID
OUTER APPLY(
    SELECT spno = 
    substring(
        (SELECT '/'+REPLACE(ID,@poid,'') 
        FROM dbo.Orders 
        WHERE POID = @poid AND OrderComboID = 
        (select top 1 OrderComboID from Orders where id = @ID)
	        order by ID FOR XML PATH(''))
     ,2,999) 
) b
where POID = @poid 
and (SMnorderApv is not null or MnorderApv is not null)
and OrderComboID = (select top 1 OrderComboID from Orders where id = @ID) 
group by POID,b.spno,br.Customize2";
            }
            else
            {
                cmd = @"
SELECT 
    MAKER=max(FactoryID)
,sty=max(StyleID)+'-'+max(SeasonID)
,QTY=sum(QTY)
,'SPNO'=RTRIM(POID)+b.spno
,(select CustCDID from Orders o where o.ID = @ID) as CustCD
,(select CustPONo from Orders o where o.ID = @ID) as pono
,(select BuyerDelivery from Orders o where o.ID = @ID) as delDate
,(select Customize1 from Orders o where o.ID = @ID) as Customize1 
,(select Customize2 from Orders o where o.ID = @ID) AS SI
,br.Customize2 
,(select ChangeMemoDate from orders o where o.ID = @ID) as ChangeMemoDate
FROM Orders a WITH (NOLOCK) 
LEFT JOIN Brand Br ON a.BrandID = Br.ID
OUTER APPLY(
    SELECT substring((SELECT '/'+REPLACE(ID,@poid,'') 
    FROM Orders 
    WHERE POID = @poid
	order by ID FOR XML PATH('')),2,999) as spno
) b
where POID = @poid 
and (SMnorderApv is not null or MnorderApv is not null)
group by POID,b.spno,br.Customize2";
            }

            bool res = MyUtility.Check.Seek(cmd, new List<SqlParameter> { new SqlParameter("@poid", poid), new SqlParameter("@ID", id) }, out drvar, null);
            if (res)
            {
                return drvar;
            }
            else
            {
                return null;
            }
        }

        private void Rd_CheckedChanged(object sender, EventArgs e)
        {
            this.checkAdditionally.Visible = false;
            this.checkAdditionally.Visible = this.radioMNotice.Checked || this.radioByOrderCombo.Checked;
        }

        private string GetSPNote(string orderID)
        {
            string msg = string.Empty;

            List<string> msgList = new List<string>();

            System.Data.DataTable sP_note;
            DBProxy.Current.Select(null, $"SELECT  Junk,NeedProduction,KeepPanels,IsBuyBack  FROM Orders WITH(NOLOCK) WHERE ID='{orderID}'", out sP_note);

            if (MyUtility.Convert.GetBool(sP_note.Rows[0]["Junk"]) && MyUtility.Convert.GetBool(sP_note.Rows[0]["NeedProduction"]))
            {
                msgList.Add("Cancel still need to continue production");
            }
            else if (MyUtility.Convert.GetBool(sP_note.Rows[0]["Junk"]) && MyUtility.Convert.GetBool(sP_note.Rows[0]["KeepPanels"]))
            {
                msgList.Add("Keep Panel without production");
            }
            else if (MyUtility.Convert.GetBool(sP_note.Rows[0]["Junk"]))
            {
                msgList.Add("Cancel");
            }

            if (MyUtility.Convert.GetBool(sP_note.Rows[0]["IsBuyBack"]))
            {
                msgList.Add("Buy Back");
            }

            msg = msgList.JoinToString(Environment.NewLine);

            return msg;
        }

        private string GetSPNotes(string orderIDs)
        {
            string msg = string.Empty;

            string firstSP = string.Empty;
            string poid = string.Empty;

            List<string> spList = new List<string>();

            // Cancel still need to continue Production
            List<string> status1 = new List<string>();

            // Keep Panel without production
            List<string> status2 = new List<string>();

            // Cancel
            List<string> status3 = new List<string>();

            // Buy Back
            List<string> status4 = new List<string>();

            int q = 0;

            // 以20021103GG/20021103GG001舉例
            foreach (var sp in orderIDs.Split('/'))
            {
                if (q == 0)
                {
                    // 第一個SP(20021103GG)直接塞進List即可
                    firstSP = sp;
                    poid = sp.Substring(0, 10);
                    spList.Add(sp);
                }
                else
                {
                    // 第一個之後的，會是001、002...等等，因此需要抓POID，加上001、002...組成正確SP#
                    spList.Add(poid + sp);
                }

                q++;
            }

            System.Data.DataTable sP_note;
            System.Data.DataTable isBuyBackDt;
            DBProxy.Current.Select(null, $"SELECT ID,Junk,NeedProduction,KeepPanels,IsBuyBack FROM Orders WITH(NOLOCK) WHERE ID IN ('{spList.JoinToString("','")}')", out sP_note);
            DBProxy.Current.Select(null, $"SELECT ID,Junk,NeedProduction,KeepPanels,IsBuyBack FROM Orders WITH(NOLOCK) WHERE POID ='{poid}' AND IsBuyBack = 1", out isBuyBackDt);

            foreach (DataRow dr in sP_note.Rows)
            {
                if (MyUtility.Convert.GetBool(dr["Junk"]) && MyUtility.Convert.GetBool(dr["NeedProduction"]))
                {
                    status1.Add(dr["ID"].ToString());
                }
                else if (MyUtility.Convert.GetBool(dr["Junk"]) && MyUtility.Convert.GetBool(dr["KeepPanels"]))
                {
                    status2.Add(dr["ID"].ToString());
                }
                else if (MyUtility.Convert.GetBool(dr["Junk"]))
                {
                    status3.Add(dr["ID"].ToString());
                }

                if (isBuyBackDt.Rows.Count > 0)
                {
                    status4.Add(poid);
                }
            }

            List<string> mesgs = new List<string>();

            string tmp1Msg = status1.Any() ? "Cancel still need to continue Production : "
                + status1.FirstOrDefault()
                + (status1.Where(o => o != status1.FirstOrDefault()).Any() ?
                   ("/" + status1.Where(o => o != status1.FirstOrDefault()).JoinToString("/").Replace(firstSP, string.Empty)) : string.Empty)
                    : string.Empty;

            if (!MyUtility.Check.Empty(tmp1Msg))
            {
                mesgs.Add(tmp1Msg);
            }

            string tmp2Msg = status2.Any() ? "Keep Panel without production : "
                + status2.FirstOrDefault()
                + (status2.Where(o => o != status2.FirstOrDefault()).Any() ?
                   ("/" + status2.Where(o => o != status2.FirstOrDefault()).JoinToString("/").Replace(firstSP, string.Empty)) : string.Empty)
                    : string.Empty;

            if (!MyUtility.Check.Empty(tmp2Msg))
            {
                mesgs.Add(tmp2Msg);
            }

            string tmp3Msg = status3.Any() ? "Cancel : "
                + status3.FirstOrDefault()
                + (status3.Where(o => o != status3.FirstOrDefault()).Any() ?
                   ("/" + status3.Where(o => o != status3.FirstOrDefault()).JoinToString("/").Replace(firstSP, string.Empty)) : string.Empty)
                    : string.Empty;

            if (!MyUtility.Check.Empty(tmp3Msg))
            {
                mesgs.Add(tmp3Msg);
            }

            string tmp4Msg = status4.Any() ? "Buy Back" : string.Empty;

            if (!MyUtility.Check.Empty(tmp4Msg))
            {
                mesgs.Add(tmp4Msg);
            }

            msg = mesgs.JoinToString(Environment.NewLine);

            return msg;
        }
    }
}
