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
            if (radioButton_MNotice.Checked == true)
            {

                string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.Orders where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });

                DataRow drvar = GetTitleDataByCustCD(poid, _id);

                if (drvar == null) return true;

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice.xltx");
                sxrc sxr = new sxrc(xltPath, true);
                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);
                sxr.dicDatas.Add(sxr._v + "PO_MAKER", drvar["MAKER"].ToString());
                sxr.dicDatas.Add(sxr._v + "PO_STYLENO", drvar["sty"].ToString());
                sxr.dicDatas.Add(sxr._v + "PO_QTY", drvar["QTY"].ToString());
                sxr.dicDatas.Add(sxr._v + "POID", poid);

                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "PPIC_Report_SizeSpec", new List<SqlParameter> { new SqlParameter("@POID", poid), new SqlParameter("@WithZ", chkAdditional.Checked), new SqlParameter("@fullsize", 1) }, out dts);

                sxrc.xltRptTable xltTbl = new sxrc.xltRptTable(dts[0], 1, 0, false, 18, 2);
                for (int i = 3; i <= 18; i++)
                {
                    sxrc.xlsColumnInfo xcinfo = new sxrc.xlsColumnInfo(i, false, 0, XlHAlign.xlHAlignLeft);
                    xcinfo.NumberFormate = "@";
                    xltTbl.lisColumnInfo.Add(xcinfo);
                }

                sxr.dicDatas.Add(sxr._v + "S1_Tbl1", xltTbl);
                intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; //起始位置加一、格線加二
                sxrc.ReplaceAction ra = ForSizeSpec;
                sxr.dicDatas.Add(sxr._v + "ExtraAction", ra);

                System.Data.DataTable dt;
                DualResult getIds = DBProxy.Current.Select("", "select ID, FactoryID as MAKER, StyleID+'-'+SeasonID as sty, QTY from Orders where poid = @poid", new List<SqlParameter> { new SqlParameter("poid", poid) }, out dt);
                if (!getIds && dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.ErrorBox(getIds.ToString(), "error");
                    return true;
                }

                sxr.CopySheet.Add(2, dt.Rows.Count - 1);
                sxr.VarToSheetName = sxr._v + "SP";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string ID = dt.Rows[i]["ID"].ToString();
                    string idxStr = (i == 0) ? "" : i.ToString();

                    res = DBProxy.Current.SelectSP("", "PPIC_Report02", new List<SqlParameter> { new SqlParameter("@ID", ID), new SqlParameter("@WithZ", chkAdditional.Checked) }, out dts);

                    sxr.dicDatas.Add(sxr._v + "SP" + idxStr, ID);
                    sxr.dicDatas.Add(sxr._v + "MAKER" + idxStr, dt.Rows[i]["MAKER"].ToString());
                    sxr.dicDatas.Add(sxr._v + "STYLENO" + idxStr, dt.Rows[i]["sty"].ToString());
                    sxr.dicDatas.Add(sxr._v + "QTY" + idxStr, dt.Rows[i]["QTY"].ToString());

                    sxrc.xltRptTable tbl1 = new sxrc.xltRptTable(dts[0], 1, 2, true);
                    sxrc.xltRptTable tbl2 = new sxrc.xltRptTable(dts[1], 1, 3);
                    sxrc.xltRptTable tbl3 = new sxrc.xltRptTable(dts[2], 1, 0);
                    SetColumn1toText(tbl1);
                    SetColumn1toText(tbl2);
                    SetColumn1toText(tbl3);

                    sxr.dicDatas.Add(sxr._v + "S2_Tbl1" + idxStr, tbl1);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl2" + idxStr, tbl2);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl3" + idxStr, tbl3);
                    if (dts[3].Rows.Count > 0)
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl4" + idxStr, dts[3]); //COLOR list
                    if (dts[4].Rows.Count > 0)
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl5" + idxStr, dts[4]); //Fabric list
                    if (dts[5].Rows.Count > 0)
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl6" + idxStr, dts[5]); //Accessories list
                    if (dts[6].Rows.Count > 0)
                        sxr.dicDatas.Add(sxr._v + "S2SHIPINGMARK" + idxStr, new sxrc.xltLongString(dts[6].Rows[0]["shipingMark"].ToString()));
                    if (dts[7].Rows.Count > 0)
                        sxr.dicDatas.Add(sxr._v + "S2PACKING" + idxStr, new sxrc.xltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    if (dts[8].Rows.Count > 0)
                        sxr.dicDatas.Add(sxr._v + "S2LH" + idxStr, new sxrc.xltLongString(dts[8].Rows[0]["Label"].ToString()));

                }
                sxr.isProtect = true;
                sxr.boOpenFile = true;
                sxr.Save();
            }

             //M/Notict (Combo by CustCD)
            else
            {
                string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.Orders where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });

                System.Data.DataTable dtCustCD = GetDtByCustCD(poid);

                if (dtCustCD == null) return true;
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice_Combo.xltx");
                sxrc sxr = new sxrc(xltPath, true);
                sxr.CopySheets.Add("1,2,3", dtCustCD.Rows.Count - 1);
                sxr.VarToSheetName = sxr._v + "sname";

                int ii = 0;
                foreach (DataRow row in dtCustCD.Rows)
                {
                    string idxStr = ii == 0 ? "" : ii.ToString();
                    ii += 1;

                    string CustCDID = row["CustCDID"].ToString();
                    string ID = row["ID"].ToString();

                    DataRow drvar = GetTitleDataByCustCD(poid, ID);

                    System.Data.DataTable[] dts;
                    DualResult res = DBProxy.Current.SelectSP("", "PPIC_Report04", new List<SqlParameter> { new SqlParameter("@ID", ID), new SqlParameter("@WithZ", chkAdditional.Checked) }, out dts);

                    if (drvar == null | !res) return true;

                    sxr.dicDatas.Add(sxr._v + "Now" + idxStr, DateTime.Now);
                    sxr.dicDatas.Add(sxr._v + "MAKER" + idxStr, drvar["MAKER"].ToString());
                    sxr.dicDatas.Add(sxr._v + "STYLENO" + idxStr, drvar["sty"].ToString());
                    sxr.dicDatas.Add(sxr._v + "QTY" + idxStr, drvar["QTY"].ToString());
                    sxr.dicDatas.Add(sxr._v + "SP" + idxStr, drvar["SPNO"].ToString());
                    sxr.dicDatas.Add(sxr._v + "sname1" + idxStr, CustCDID + "-1");
                    sxr.dicDatas.Add(sxr._v + "sname2" + idxStr, CustCDID + "-2");
                    sxr.dicDatas.Add(sxr._v + "sname3" + idxStr, CustCDID + "-3");

                    //For SizeSpec
                    sxrc.xltRptTable xltTbl = new sxrc.xltRptTable(dts[0], 1, 0, false, 18, 2);
                    for (int i = 3; i <= 18; i++)
                    {
                        sxrc.xlsColumnInfo xcinfo = new sxrc.xlsColumnInfo(i, false, 0, XlHAlign.xlHAlignLeft);
                        xcinfo.NumberFormate = "@";
                        xltTbl.lisColumnInfo.Add(xcinfo);
                    }
                    xltTbl.Separator1 = "'- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
                    xltTbl.Separator2 = "'= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =";
                    sxr.dicDatas.Add(sxr._v + "S1_Tbl1" + idxStr, xltTbl);
                    intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; //起始位置加一、格線加二
                    sxrc.ReplaceAction ra = ForSizeSpec;
                    sxr.dicDatas.Add(sxr._v + "ExtraAction" + idxStr, ra);

                    sxrc.xltRptTable tbl1 = new sxrc.xltRptTable(dts[1], 1, 2, true);
                    sxrc.xltRptTable tbl2 = new sxrc.xltRptTable(dts[2], 1, 3);
                    sxrc.xltRptTable tbl3 = new sxrc.xltRptTable(dts[3], 1, 0);
                    SetColumn1toText(tbl1);
                    SetColumn1toText(tbl2);
                    SetColumn1toText(tbl3);

                    sxr.dicDatas.Add(sxr._v + "S2_Tbl1" + idxStr, tbl1);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl2" + idxStr, tbl2);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl3" + idxStr, tbl3); //COLOR list                
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl4" + idxStr, dts[4]); //Fabric list                
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl5" + idxStr, dts[5]); //Accessories list                
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl6" + idxStr, dts[6]);
                    sxr.dicDatas.Add(sxr._v + "S2PACKING" + idxStr, new sxrc.xltLongString(dts[7].Rows[0]["Packing"].ToString()));
                    sxr.dicDatas.Add(sxr._v + "S2LH" + idxStr, new sxrc.xltLongString(dts[8].Rows[0]["Label"].ToString()));

                    //新增Range Repeat數
                    sxr.dicDatas.Add(sxr._v + "CR" + idxStr, dts[9].Rows.Count);

                    int idx = 0;
                    foreach (DataRow dr in dts[9].Rows)
                    {
                        string sIdx = idx.ToString();
                        idx += 1;

                        sxr.dicDatas.Add(sxr._v + "S3_SP" + idxStr + sIdx, dr["ID"].ToString());
                        sxr.dicDatas.Add(sxr._v + "S3_Style" + idxStr + sIdx, dr["sty"].ToString());
                        sxr.dicDatas.Add(sxr._v + "S3_QTY" + idxStr + sIdx, dr["QTY"]);
                        sxr.dicDatas.Add(sxr._v + "S3_CUSTCD" + idxStr + sIdx, dr["CustCDID"].ToString());
                        sxr.dicDatas.Add(sxr._v + "S3_PoNo" + idxStr + sIdx, dr["CustPONO"].ToString());
                        sxr.dicDatas.Add(sxr._v + "S3_Order" + idxStr + sIdx, dr["Customize1"].ToString());
                        sxr.dicDatas.Add(sxr._v + "S3_DELIVERY" + idxStr + sIdx, dr["BuyerDelivery"]);
                        sxr.dicDatas.Add(sxr._v + "S3_Mark" + idxStr + sIdx, new sxrc.xltLongString(dr["Mark"].ToString()));

                        System.Data.DataTable[] dts2;
                        List<SqlParameter> lis2 = new List<SqlParameter>();
                        lis2.Add(new SqlParameter("@OrderID", dr["ID"]));
                        lis2.Add(new SqlParameter("@ByType", "0"));

                        res = DBProxy.Current.SelectSP("", "PPIC_Report_QtyBreakdown", lis2, out dts2);

                        if (res)
                        {
                            sxrc.xltRptTable stbl = new sxrc.xltRptTable(dts2[0], 1, 2, true);
                            SetColumn1toText(stbl);
                            sxr.dicDatas.Add(sxr._v + "S3_Tbl" + idxStr + sIdx, stbl);
                        }
                    }

                } //for CustCD 


                //#if DEBUG
                //                sxr.ExcelApp.Visible = true;
                //#endif
                sxr.isProtect = true;
                sxr.boOpenFile = true;
                sxr.Save();
            }
            return true;
        }

        private System.Data.DataTable GetDtByCustCD(string POID)
        {
            System.Data.DataTable dt;
            DualResult res = DBProxy.Current.Select("", @"
SELECT CustCDID,ID FROM (
	SELECT CustCDID,ID,ROW_NUMBER()OVER(partition by CustCDID ORDER BY ID) as idx FROM Orders WHERE POID = @POID
) C WHERE idx = 1
order by ID
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
        void SetColumn1toText(sxrc.xltRptTable tbl)
        {
            sxrc.xlsColumnInfo c1 = new sxrc.xlsColumnInfo(1);
            c1.NumberFormate = "@";
            tbl.lisColumnInfo.Add(c1);
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

SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WHERE POID = @poid AND CustCDID = (select CustCDID from MNOrder where ID = @ID) 
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where POID = @poid and CustCDID = (select CustCDID from MNOrder where ID = @ID) group by POID,b.spno";
            }
            else
            {
                cmd = @"SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WHERE POID = @poid
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
            chkAdditional.Visible = false;
            chkAdditional.Visible = radioButton_MNotice.Checked || radioButton_ByCustCD.Checked;
        }
    }
}
