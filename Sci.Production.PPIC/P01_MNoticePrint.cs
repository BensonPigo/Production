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
        private string _id;
        private sxrc sxr;

        // private string _username;
        // private string _userid;
        // DualResult result;
        // DataRow CurrentDataRow ;

        /// <summary>
        /// P01_MNoticePrint
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        /// <param name="args">string args</param>
        public P01_MNoticePrint(ToolStripMenuItem menuitem, string args)
            : base(menuitem)
        {
            this.Constructor(args);
        }

        /// <summary>
        /// P01_MNoticePrint
        /// </summary>
        /// <param name="args">string args</param>
        public P01_MNoticePrint(string args)
        {
            this.Constructor(args);
        }

        private void Constructor(string args)
        {
            this._id = args;
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
            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;

            if (this.radioMNotice.Checked == true)
            {
                strFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_M_Notice");
                strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_M_Notice", Sci.Production.Class.PDFFileNameExtension.PDF);
                this.sxr.AllowRangeTransferToString = false;
                this.sxr.Save(strFileName);
                this.sxr.FinishSave();
            }
            else
            {
                // for CustCD
                // #if DEBUG
                //                sxr.ExcelApp.Visible = true;
                // #endif
                strFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_Report04");
                strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_Report04", Sci.Production.Class.PDFFileNameExtension.PDF);

                this.sxr.Save(strFileName);
                this.sxr.FinishSave();
            }

            switch (this.Print_type)
            {
                case Excel:
                    strFileName.OpenFile();
                    break;
                case PDF:
                    if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
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
                string ordercomboid = MyUtility.GetValue.Lookup("select ordercomboid FROM dbo.MNOrder WITH (NOLOCK) where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", this._id) });

                DataRow drvar = this.GetTitleDataByCustCD(ordercomboid, this._id);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                if (drvar == null)
                {
                    DualResult failResult = new DualResult(false, "data not found!!" );
                    return failResult;
                }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice.xltx");
                this.sxr = new sxrc(xltPath, true);
                this.sxr.BoOpenFile = false;
                this.sxr.AddPrintRange = true;
                this.sxr.FontName = "Times New Roman";
                this.sxr.FontSize = 14;
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_NOW", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_MAKER", drvar["MAKER"].ToString());
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_STYLENO", drvar["sty"].ToString());
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_QTY", drvar["QTY"].ToString());
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "POID", ordercomboid);
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_CustCD", drvar["CustCD"].ToString());
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_pono", drvar["pono"].ToString());
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_delDate", drvar["delDate"]);

                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report_SizeSpec", new List<SqlParameter> { new SqlParameter("@ID", ordercomboid), new SqlParameter("@WithZ", this.checkAdditionally.Checked), new SqlParameter("@fullsize", 1) }, out dts);

                sxrc.XltRptTable xltTbl = new sxrc.XltRptTable(dts[0], 1, 0, false, 18, 2);
                for (int i = 3; i <= 18; i++)
                {
                    sxrc.XlsColumnInfo xcinfo = new sxrc.XlsColumnInfo(i, false, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                    xcinfo.NumberFormate = "@";
                    xltTbl.LisColumnInfo.Add(xcinfo);
                }

                this.sxr.DicDatas.Add(this.sxr.VPrefix + "S1_Tbl1", xltTbl);
                this.intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; // 起始位置加一、格線加二
                sxrc.ReplaceAction ra = this.ForSizeSpec;
                this.sxr.DicDatas.Add(this.sxr.VPrefix + "ExtraAction", ra);

                System.Data.DataTable dt;
                DualResult getIds = DBProxy.Current.Select(string.Empty, "select ID, FactoryID as MAKER, StyleID+'-'+SeasonID as sty, QTY , CustCdID as CustCD, CustPONo as pono, BuyerDelivery as delDate,Customize1 from MNOrder WITH (NOLOCK) where ordercomboid = @ordercomboid order by ID", new List<SqlParameter> { new SqlParameter("ordercomboid", ordercomboid) }, out dt);
                if (!getIds && dt.Rows.Count <= 0)
                {
                    DualResult failResult = new DualResult(false, "Error:" + getIds.ToString());
                    return failResult;
                }

                this.sxr.CopySheet.Add(2, dt.Rows.Count - 1);
                this.sxr.VarToSheetName = this.sxr.VPrefix + "SP";

                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(dt.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = dt.Rows[i]["ID"].ToString();
                    string idxStr = i.ToString();

                    res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report02", new List<SqlParameter> { new SqlParameter("@ID", id), new SqlParameter("@WithZ", this.checkAdditionally.Checked) }, out dts);

                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "NOW" + idxStr, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "SP" + idxStr, id);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "MAKER" + idxStr, dt.Rows[i]["MAKER"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "STYLENO" + idxStr, dt.Rows[i]["sty"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "QTY" + idxStr, dt.Rows[i]["QTY"].ToString());

                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "CustCD" + idxStr, dt.Rows[i]["CustCD"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "pono" + idxStr, dt.Rows[i]["pono"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "delDate" + idxStr, dt.Rows[i]["delDate"]);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "coms1" + idxStr, dt.Rows[i]["Customize1"].ToString());

                    sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[0], 1, 2, true);
                    sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[1], 1, 3);
                    sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[2], 1, 0);
                    this.SetColumn1toText(tbl1);
                    this.SetColumn1toText(tbl2);
                    this.SetColumn1toText(tbl3);

                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl1" + idxStr, tbl1);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl2" + idxStr, tbl2);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl3" + idxStr, tbl3);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl4" + idxStr, dts[3]); // COLOR list
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl5" + idxStr, dts[4]); // Fabric list
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl6" + idxStr, dts[5]); // Accessories list
                    if (dts[6].Rows.Count > 0)
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2SHIPINGMARK" + idxStr, new sxrc.XltLongString(dts[6].Rows[0]["shipingMark"].ToString()));
                    }

                    if (dts[7].Rows.Count > 0)
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2PACKING" + idxStr, new sxrc.XltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    }

                    if (dts[8].Rows.Count > 0)
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2LH" + idxStr, new sxrc.XltLongString(dts[8].Rows[0]["Label"].ToString()));
                    }

                    if (dts[9].Rows[0].Field<bool?>("VasShas").GetValueOrDefault(false) == true)
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2VS" + idxStr, new sxrc.XltLongString(dts[9].Rows[0]["Packing2"].ToString()));
                    }
                    else
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2VS" + idxStr, new sxrc.XlsPrivateCommand() { UpDelete = 2 });
                    }
                }
            }

            // M/Notict (Combo by ComboID)
            else
            {
                string ordercomboid = MyUtility.GetValue.Lookup("select ordercomboid FROM dbo.MNOrder WITH (NOLOCK) where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", this._id) });

                System.Data.DataTable dtOrderCombo = this.GetDtByComboID(ordercomboid);

                if (dtOrderCombo == null)
                {
                    DualResult failResult = new DualResult(false, "data not found!!");
                    return failResult;
                }

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice_Combo.xltx");
                this.sxr = new sxrc(xltPath);
                this.sxr.BoOpenFile = false;
                this.sxr.AddPrintRange = true;
                this.sxr.FontName = "Times New Roman";
                this.sxr.FontSize = 14;
                this.sxr.CopySheets.Add("1,2,3", dtOrderCombo.Rows.Count - 1);
                this.sxr.VarToSheetName = this.sxr.VPrefix + "sname";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                int ii = 0;
                foreach (DataRow row in dtOrderCombo.Rows)
                {
                    string idxStr = ii.ToString();
                    ii += 1;

                    string orderComboID = row["OrderComboID"].ToString();

                    DataRow drvar = this.GetTitleDataByCustCD(ordercomboid, orderComboID);

                    System.Data.DataTable[] dts;
                    DualResult res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report04", new List<SqlParameter> { new SqlParameter("@ID", orderComboID), new SqlParameter("@WithZ", this.checkAdditionally.Checked), new SqlParameter("@ByType", 1) }, out dts);

                    // 顯示筆數於PrintForm上Count欄位
                    this.SetCount(dts[10].Rows.Count);

                    if (drvar == null | !res)
                    {
                        DualResult failResult = new DualResult(false, "data not found!!");
                        return failResult;
                    }

                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_NOW" + idxStr, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_MAKER" + idxStr, drvar["MAKER"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_STYLENO" + idxStr, drvar["sty"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_QTY" + idxStr, drvar["QTY"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "POID" + idxStr, drvar["SPNO"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_CustCD" + idxStr, drvar["CustCD"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_pono" + idxStr, drvar["pono"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "PO_delDate" + idxStr, drvar["delDate"]);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "sname1" + idxStr, orderComboID + "-1");
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "sname2" + idxStr, orderComboID + "-2");
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "sname3" + idxStr, orderComboID + "-3");

                    // For SizeSpec
                    sxrc.XltRptTable xltTbl = new sxrc.XltRptTable(dts[0], 1, 0, false, 18, 2);
                    for (int i = 3; i <= 18; i++)
                    {
                        sxrc.XlsColumnInfo xcinfo = new sxrc.XlsColumnInfo(i, false, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                        xcinfo.NumberFormate = "@";
                        xltTbl.LisColumnInfo.Add(xcinfo);
                    }

                    xltTbl.Separator1 = "'- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
                    xltTbl.Separator2 = "'= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =";
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S1_Tbl1" + idxStr, xltTbl);
                    this.intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; // 起始位置加一、格線加二
                    sxrc.ReplaceAction ra = this.ForSizeSpec;
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "ExtraAction" + idxStr, ra);

                    sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[1], 1, 2, true);
                    sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[2], 1, 3);
                    sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[3], 1, 0);
                    this.SetColumn1toText(tbl1);
                    this.SetColumn1toText(tbl2);
                    this.SetColumn1toText(tbl3);

                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "NOW" + idxStr, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "SP" + idxStr, drvar["SPNO"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "MAKER" + idxStr, drvar["MAKER"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "STYLENO" + idxStr, drvar["sty"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "QTY" + idxStr, drvar["QTY"].ToString());

                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "CustCD" + idxStr, drvar["CustCD"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "pono" + idxStr, drvar["pono"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "delDate" + idxStr, drvar["delDate"]);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "coms1" + idxStr, drvar["Customize1"].ToString());
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl1" + idxStr, tbl1);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl2" + idxStr, tbl2);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl3" + idxStr, tbl3); // COLOR list
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl4" + idxStr, dts[4]); // Fabric list
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl5" + idxStr, dts[5]); // Accessories list
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2_Tbl6" + idxStr, dts[6]);
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2PACKING" + idxStr, new sxrc.XltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2LH" + idxStr, new sxrc.XltLongString(dts[8].Rows[0]["Label"].ToString()));
                    if (dts[9].Rows[0].Field<bool?>("VasShas").GetValueOrDefault(false) == true)
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2VS" + idxStr, new sxrc.XltLongString(dts[9].Rows[0]["Packing2"].ToString()));
                    }
                    else
                    {
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S2VS" + idxStr, new sxrc.XlsPrivateCommand() { UpDelete = 2 });
                    }

                    // 新增Range Repeat數
                    this.sxr.DicDatas.Add(this.sxr.VPrefix + "CR" + idxStr, dts[10].Rows.Count);

                    int idx = 0;
                    foreach (DataRow dr in dts[10].Rows)
                    {
                        string sIdx = idx.ToString();
                        idx += 1;

                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_SP" + idxStr + this.sxr.CRPrefix + sIdx, dr["ID"].ToString());
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_Style" + idxStr + this.sxr.CRPrefix + sIdx, dr["sty"].ToString());
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_QTY" + idxStr + this.sxr.CRPrefix + sIdx, dr["QTY"].ToString());
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_CUSTCD" + idxStr + this.sxr.CRPrefix + sIdx, dr["CustCDID"].ToString());
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_PoNo" + idxStr + this.sxr.CRPrefix + sIdx, dr["CustPONO"].ToString());
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_Order" + idxStr + this.sxr.CRPrefix + sIdx, dr["Customize1"].ToString());
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_DELIVERY" + idxStr + this.sxr.CRPrefix + sIdx, dr["BuyerDelivery"]);
                        this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_Mark" + idxStr + this.sxr.CRPrefix + sIdx, new sxrc.XltLongString(dr["Mark"].ToString()));

                        System.Data.DataTable[] dts2;
                        List<SqlParameter> lis2 = new List<SqlParameter>();
                        lis2.Add(new SqlParameter("@OrderID", dr["ID"]));
                        lis2.Add(new SqlParameter("@ByType", "0"));

                        res = DBProxy.Current.SelectSP(string.Empty, "Order_Report_QtyBreakdown", lis2, out dts2);

                        if (res)
                        {
                            sxrc.XltRptTable stbl = new sxrc.XltRptTable(dts2[0], 1, 2, true);
                            this.SetColumn1toText(stbl);
                            this.sxr.DicDatas.Add(this.sxr.VPrefix + "S3_Tbl" + idxStr + this.sxr.CRPrefix + sIdx, stbl);
                        }
                    }
                }
            }

            return new DualResult(true);
        }

        private System.Data.DataTable GetDtByComboID(string poid)
        {
            System.Data.DataTable dt;
            string strSqlSelect = @"
SELECT distinct OrderComboID from MNOrder
outer apply (select 1 as cnt from MNOrder tmp where tmp.OrderComboID = MNOrder.ID) cnt
WHERE MNOrder.ordercomboid = @POID";

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

        private DataRow GetTitleDataByCustCD(string ordercomboid, string id, bool byCustCD = true)
        {
            DataRow drvar;
            string cmd = string.Empty;
            if (byCustCD)
            {
                cmd = @"
SELECT MAKER=max(FactoryID)
,sty=max(StyleID)+'-'+max(SeasonID)
,QTY=sum(QTY)
--,'SPNO'=RTRIM(POID)+b.spno 
,'SPNO'=b.OrderComboList
,(select CustCDID from MnOrder o where o.ID = @ID) as CustCD
,(select CustPONo from MnOrder o where o.ID = @ID) as pono
,(select BuyerDelivery from MnOrder o where o.ID = @ID) as delDate
,(select Customize1 from MnOrder o where o.ID = @ID) as Customize1
FROM MNOrder a WITH (NOLOCK) 
--OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@ordercomboid,'') FROM MNOrder WITH (NOLOCK) WHERE OrderComboID = @ID
OUTER APPLY(Select Top 1 OrderComboList from dbo.Order_OrderComboList with(nolock) where ID  = @ID) b
where OrderComboID = @ordercomboid group by POID,b.OrderComboList";
            }
            else
            {
                cmd = @"
SELECT 
MAKER=max(FactoryID)
,sty=max(StyleID)+'-'+max(SeasonID)
,QTY=sum(QTY)
--,'SPNO'=RTRIM(POID)+b.spno
,'SPNO'=b.spno
,(select CustCDID from MnOrder o where o.ID = @ID) as CustCD
,(select CustPONo from MnOrder o where o.ID = @ID) as pono
,(select BuyerDelivery from MnOrder o where o.ID = @ID) as delDate
,(select Customize1 from MnOrder o where o.ID = @ID) as Customize1 
FROM MNOrder a WITH (NOLOCK) 
--OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@ordercomboid,'') FROM MNOrder WITH (NOLOCK) WHERE OrderComboID = @ordercomboid
OUTER APPLY(SELECT STUFF((SELECT '/'+ID FROM MNOrder WITH (NOLOCK) WHERE OrderComboID = @ordercomboid
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where OrderComboID = @ordercomboid group by POID,b.spno";
            }

            bool res = MyUtility.Check.Seek(cmd, new List<SqlParameter> { new SqlParameter("@ordercomboid", ordercomboid), new SqlParameter("@ID", id) }, out drvar, null);
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


    }
}
