using Ict;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;

namespace Sci.Production.PPIC
{
    public partial class P01_MNoticePrint : Sci.Win.Tems.PrintForm
    {
        private string _id;
        //private string _username;
        //private string _userid;
        //DualResult result;
        //DataRow CurrentDataRow ;

        public P01_MNoticePrint(ToolStripMenuItem menuitem, string args)
            : base(menuitem)
        {
            Constructor(args);
        }
        public P01_MNoticePrint(string args)
        {
            Constructor(args);
        }
        private void Constructor(string args)
        {
            this._id = args;
            InitializeComponent();
            EditMode = true;

            foreach (var control in groupBox1.Controls)
            {
                if (control is Sci.Win.UI.RadioButton)
                {
                    Sci.Win.UI.RadioButton rdb = (Sci.Win.UI.RadioButton)control;
                    rdb.CheckedChanged += rd_CheckedChanged;
                }
            }
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return new DualResult(true);
        }


        protected override bool ToExcel()
        {
            this.ShowWaitMessage("Data processing, please wait ...");
            if (radioMNotice.Checked == true)
            {
                string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.MNOrder WITH (NOLOCK) where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });

                DataRow drvar = GetTitleDataByCustCD(poid, _id);

                if (drvar == null)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("data not found!!");
                    return true;
                }
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice.xltx");
                sxrc sxr = new sxrc(xltPath, true);

                sxr.DicDatas.Add(sxr.VPrefix + "NOW", DateTime.Now);
                sxr.DicDatas.Add(sxr.VPrefix + "PO_MAKER", drvar["MAKER"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "PO_STYLENO", drvar["sty"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "PO_QTY", drvar["QTY"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "POID", poid);

                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "PPIC_Report_SizeSpec", new List<SqlParameter> { new SqlParameter("@ID", poid), new SqlParameter("@WithZ", checkAdditionally.Checked), new SqlParameter("@fullsize", 1) }, out dts);

                sxrc.XltRptTable xltTbl = new sxrc.XltRptTable(dts[0], 1, 0, false, 18, 2);
                for (int i = 3; i <= 18; i++)
                {
                    sxrc.XlsColumnInfo xcinfo = new sxrc.XlsColumnInfo(i, false, 0, XlHAlign.xlHAlignLeft);
                    xcinfo.NumberFormate = "@";
                    xltTbl.LisColumnInfo.Add(xcinfo);
                }

                sxr.DicDatas.Add(sxr.VPrefix + "S1_Tbl1", xltTbl);
                intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; //起始位置加一、格線加二
                sxrc.ReplaceAction ra = ForSizeSpec;
                sxr.DicDatas.Add(sxr.VPrefix + "ExtraAction", ra);

                System.Data.DataTable dt;
                DualResult getIds = DBProxy.Current.Select("", "select ID, FactoryID as MAKER, StyleID+'-'+SeasonID as sty, QTY from MNOrder WITH (NOLOCK) where poid = @poid", new List<SqlParameter> { new SqlParameter("poid", poid) }, out dt);
                if (!getIds && dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.ErrorBox(getIds.ToString(), "error");
                    this.HideWaitMessage();
                    return true;
                }

                sxr.CopySheet.Add(2, dt.Rows.Count - 1);
                sxr.VarToSheetName = sxr.VPrefix + "SP";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string ID = dt.Rows[i]["ID"].ToString();
                    string idxStr = i.ToString();

                    res = DBProxy.Current.SelectSP("", "PPIC_Report02", new List<SqlParameter> { new SqlParameter("@ID", ID), new SqlParameter("@WithZ", checkAdditionally.Checked) }, out dts);

                    sxr.DicDatas.Add(sxr.VPrefix + "Now" + idxStr, DateTime.Now);
                    sxr.DicDatas.Add(sxr.VPrefix + "SP" + idxStr, ID);
                    sxr.DicDatas.Add(sxr.VPrefix + "MAKER" + idxStr, dt.Rows[i]["MAKER"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, dt.Rows[i]["sty"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, dt.Rows[i]["QTY"].ToString());

                    sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[0], 1, 2, true);
                    sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[1], 1, 3);
                    sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[2], 1, 0);
                    SetColumn1toText(tbl1);
                    SetColumn1toText(tbl2);
                    SetColumn1toText(tbl3);

                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl1" + idxStr, tbl1);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl2" + idxStr, tbl2);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl3" + idxStr, tbl3);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl4" + idxStr, dts[3]); //COLOR list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl5" + idxStr, dts[4]); //Fabric list
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl6" + idxStr, dts[5]); //Accessories list
                    if (dts[6].Rows.Count>0)
                        sxr.DicDatas.Add(sxr.VPrefix + "S2SHIPINGMARK" + idxStr, new sxrc.XltLongString(dts[6].Rows[0]["shipingMark"].ToString()));
                    if (dts[7].Rows.Count > 0)
                        sxr.DicDatas.Add(sxr.VPrefix + "S2PACKING" + idxStr, new sxrc.XltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    if (dts[8].Rows.Count > 0)
                        sxr.DicDatas.Add(sxr.VPrefix + "S2LH" + idxStr, new sxrc.XltLongString(dts[8].Rows[0]["Label"].ToString()));

                }

                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_M_Notice"));
            }

             //M/Notict (Combo by ComboID)
            else
            {
                string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.MNOrder WITH (NOLOCK) where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });

                System.Data.DataTable dtOrderCombo = GetDtByComboID(poid);

                if (dtOrderCombo == null) { MyUtility.Msg.WarningBox("data not found!!"); this.HideWaitMessage(); return true; }
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice_Combo.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.CopySheets.Add("1,2,3", dtOrderCombo.Rows.Count - 1);
                sxr.VarToSheetName = sxr.VPrefix + "sname";

                int ii = 0;
                foreach (DataRow row in dtOrderCombo.Rows)
                {
                    string idxStr = ii.ToString();
                    ii += 1;

                    string OrderComboID = row["OrderComboID"].ToString();

                    DataRow drvar = GetTitleDataByCustCD(poid, OrderComboID);

                    System.Data.DataTable[] dts;
                    DualResult res = DBProxy.Current.SelectSP("", "PPIC_Report04", new List<SqlParameter> { new SqlParameter("@ID", OrderComboID), new SqlParameter("@WithZ", checkAdditionally.Checked), new SqlParameter("@ByType", 1) }, out dts);

                    if (drvar == null | !res)
                    {
                        this.HideWaitMessage();
                        MyUtility.Msg.WarningBox("data not found!!");
                        return true;
                    }

                    sxr.DicDatas.Add(sxr.VPrefix + "Now" + idxStr, DateTime.Now);
                    sxr.DicDatas.Add(sxr.VPrefix + "MAKER" + idxStr, drvar["MAKER"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "STYLENO" + idxStr, drvar["sty"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "QTY" + idxStr, drvar["QTY"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "SP" + idxStr, drvar["SPNO"].ToString());
                    sxr.DicDatas.Add(sxr.VPrefix + "sname1" + idxStr, OrderComboID + "-1");
                    sxr.DicDatas.Add(sxr.VPrefix + "sname2" + idxStr, OrderComboID + "-2");
                    sxr.DicDatas.Add(sxr.VPrefix + "sname3" + idxStr, OrderComboID + "-3");

                    //For SizeSpec
                    sxrc.XltRptTable xltTbl = new sxrc.XltRptTable(dts[0], 1, 0, false, 18, 2);
                    for (int i = 3; i <= 18; i++)
                    {
                        sxrc.XlsColumnInfo xcinfo = new sxrc.XlsColumnInfo(i, false, 0, XlHAlign.xlHAlignLeft);
                        xcinfo.NumberFormate = "@";
                        xltTbl.LisColumnInfo.Add(xcinfo);
                    }
                    xltTbl.Separator1 = "'- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
                    xltTbl.Separator2 = "'= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =";
                    sxr.DicDatas.Add(sxr.VPrefix + "S1_Tbl1" + idxStr, xltTbl);
                    intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; //起始位置加一、格線加二
                    sxrc.ReplaceAction ra = ForSizeSpec;
                    sxr.DicDatas.Add(sxr.VPrefix + "ExtraAction" + idxStr, ra);

                    sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[1], 1, 2, true);
                    sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[2], 1, 3);
                    sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[3], 1, 0);
                    SetColumn1toText(tbl1);
                    SetColumn1toText(tbl2);
                    SetColumn1toText(tbl3);

                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl1" + idxStr, tbl1);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl2" + idxStr, tbl2);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl3" + idxStr, tbl3); //COLOR list                
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl4" + idxStr, dts[4]); //Fabric list                
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl5" + idxStr, dts[5]); //Accessories list                
                    sxr.DicDatas.Add(sxr.VPrefix + "S2_Tbl6" + idxStr, dts[6]);
                    sxr.DicDatas.Add(sxr.VPrefix + "S2PACKING" + idxStr, new sxrc.XltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    sxr.DicDatas.Add(sxr.VPrefix + "S2LH" + idxStr, new sxrc.XltLongString(dts[8].Rows[0]["Label"].ToString()));

                    //新增Range Repeat數
                    sxr.DicDatas.Add(sxr.VPrefix + "CR" + idxStr, dts[9].Rows.Count);

                    int idx = 0;
                    foreach (DataRow dr in dts[9].Rows)
                    {
                        string sIdx = idx.ToString();
                        idx += 1;

                        sxr.DicDatas.Add(sxr.VPrefix + "S3_SP" + idxStr + sIdx, dr["ID"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_Style" + idxStr + sIdx, dr["sty"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_QTY" + idxStr + sIdx, dr["QTY"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_CUSTCD" + idxStr + sIdx, dr["OrderComboID"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_PoNo" + idxStr + sIdx, dr["CustPONO"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_Order" + idxStr + sIdx, dr["Customize1"].ToString());
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_DELIVERY" + idxStr + sIdx, dr["BuyerDelivery"]);
                        sxr.DicDatas.Add(sxr.VPrefix + "S3_Mark" + idxStr + sIdx, new sxrc.XltLongString(dr["Mark"].ToString()));

                        System.Data.DataTable[] dts2;
                        List<SqlParameter> lis2 = new List<SqlParameter>();
                        lis2.Add(new SqlParameter("@OrderID", dr["ID"]));
                        lis2.Add(new SqlParameter("@ByType", "0"));

                        res = DBProxy.Current.SelectSP("", "Order_Report_QtyBreakdown", lis2, out dts2);

                        if (res)
                        {
                            sxrc.XltRptTable stbl = new sxrc.XltRptTable(dts2[0], 1, 2, true);
                            SetColumn1toText(stbl);
                            sxr.DicDatas.Add(sxr.VPrefix + "S3_Tbl" + idxStr + sIdx, stbl);
                        }
                    }

                } //for CustCD 


                //#if DEBUG
                //                sxr.ExcelApp.Visible = true;
                //#endif

                sxr.BoOpenFile = true;
                sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("PPIC_Report04"));
            }
            this.HideWaitMessage();
            return true;
        }

        private System.Data.DataTable GetDtByComboID(string POID)
        {
            System.Data.DataTable dt;
            DualResult res = DBProxy.Current.Select("", @"
SELECT distinct OrderComboID from MNOrder
outer apply (select 1 as cnt from MNOrder tmp where tmp.OrderComboID = MNOrder.ID) cnt
WHERE MNOrder.POID = @POID
", new List<SqlParameter> { new SqlParameter("@POID", POID) }, out dt);

            if (res && dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        void SetColumn1toText(sxrc.XltRptTable tbl)
        {
            sxrc.XlsColumnInfo c1 = new sxrc.XlsColumnInfo(1);
            c1.NumberFormate = "@";
            tbl.LisColumnInfo.Add(c1);
        }

        private int intSizeSpecRowCnt = 0;
        private int intSizeSpecColumnCnt = 18;
        void ForSizeSpec(Worksheet oSheet, int rowNo, int columnNo)
        {
            for (int colIdx = 3; colIdx <= intSizeSpecColumnCnt; colIdx++)
            {
                //oSheet.Cells[4, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);    
                oSheet.Cells[4, colIdx].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }
            for (int colIdx = 3; colIdx <= intSizeSpecColumnCnt; colIdx++)
            {
                //oSheet.Cells[4 + intSizeSpecRowCnt, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                oSheet.Cells[4, colIdx].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }
        }

        private DataRow GetTitleDataByCustCD(string poid, string id, bool ByCustCD = true)
        {
            DataRow drvar;
            string cmd = "";
            if (ByCustCD)
            {
                cmd = @"
SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a WITH (NOLOCK) 
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WITH (NOLOCK) WHERE POID = @poid AND OrderComboID = @ID
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where POID = @poid and OrderComboID = @ID group by POID,b.spno";
            }
            else
            {
                cmd = @"
SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a WITH (NOLOCK) 
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WITH (NOLOCK) WHERE POID = @poid
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where POID = @poid group by POID,b.spno";
            }

            bool res = MyUtility.Check.Seek(cmd, new List<SqlParameter> { new SqlParameter("@poid", poid), new SqlParameter("@ID", id) }, out drvar, null);
            if (res)
                return drvar;
            else
                return null;
        }

        void rd_CheckedChanged(object sender, EventArgs e)
        {
            checkAdditionally.Visible = false;
            checkAdditionally.Visible = radioMNotice.Checked || radioByOrderCombo.Checked;
        }
    }
}
