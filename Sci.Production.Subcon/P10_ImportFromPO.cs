﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P10_ImportFromPO : Sci.Win.Subs.Base
    {
        DataRow dr_artworkAp;
        DataTable dt_artworkApDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        protected DataTable dtArtwork;

        public P10_ImportFromPO(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_artworkAp = master;
            dt_artworkApDetail = detail;
            this.Text += string.Format(" : {0} - {1}", dr_artworkAp["artworktypeid"].ToString(), dr_artworkAp["localsuppid"].ToString());
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            String sp_b = this.txtSPNoStart.Text;
            String sp_e = this.txtSPNoEnd.Text;
            String poid_b = this.txtArtworkPOIDStart.Text;
            String poid_e = this.txtArtworkPOIDEnd.Text;

            if (MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) && MyUtility.Check.Empty(poid_b) && MyUtility.Check.Empty(poid_e))
            {
                MyUtility.Msg.WarningBox("SP# and Artwork POID can't be emtpqy");
                txtSPNoStart.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Empty;


                strSQLCmd += $@"
SELECT  bd.QTY 
	,bdl.Orderid 
	,s.ArtworkTypeId
	,bio.OutGoing 
	,bio.InComing
	,bd.Patterncode
	,bd.PatternDesc
INTO #Bundle
FROM Bundle_Detail bd WITH (NOLOCK) 
INNER JOIN Bundle bdl WITH (NOLOCK)  ON bdl.id=bd.id
INNER JOIN BundleInOut bio WITH (NOLOCK)  ON bio.BundleNo = bd.BundleNo
INNER JOIN SubProcess s WITH (NOLOCK)  ON s.id= bio.SubProcessId
WHERE s.ArtworkTypeId='{dr_artworkAp["artworktypeid"]}' 
";
                if (!MyUtility.Check.Empty(sp_b))
                {
                    strSQLCmd += $@" AND bdl.Orderid >= @sp1 ";
                }
                if (!MyUtility.Check.Empty(sp_e))
                {
                    strSQLCmd += $@" AND bdl.Orderid <= @sp2";
                }

                if (!MyUtility.Check.Empty(poid_b) && !MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@"
AND bdl.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID BETWEEN @artworkpoid1 AND  @artworkpoid2
)
";
                }
                else if (!MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@"
AND bdl.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID >= @artworkpoid1 
)
";
                }
                else if (!MyUtility.Check.Empty(poid_e))
                {
                    strSQLCmd += $@"
AND bdl.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID <= @artworkpoid2
)
";
                }

                strSQLCmd  += $@"

Select 1 as Selected
        ,b.id as artworkpoid
        , b.orderid
        ,b.artworkid
        ,b.stitch
        ,b.patterncode
        ,b.patterndesc
        ,b.unitprice
        ,b.qtygarment
        ,b.price
        ,b.poqty
		,[Farmout] = ISNULL(FarmOut.Value,0)
		,[FarmIn] = ISNULL(FarmIn.Value,0)
        ,[AccumulatedQty] = b.ApQty
        ,[Balance] = b.PoQty - b.ApQty
		,[ApQty]= IIF(MinQty.Val - b.ApQty < 0 , 0 ,MinQty.Val - b.ApQty )
        ,b.ukey artworkpo_detailukey
        ,'' id
        ,[Amount] = 1.0 * IIF(MinQty.Val - b.ApQty < 0 , 0 ,MinQty.Val - b.ApQty ) * b.price --0.0 amount
from ArtworkPO a WITH (NOLOCK) 
INNER JOIN ArtworkPO_Detail b WITH (NOLOCK)  ON  a.id = b.id 
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=b.OrderID 
	AND bd.ArtworkTypeId=a.ArtworkTypeID 
	AND bd.Patterncode=b.PatternCode 
	AND bd.PatternDesc =b.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=b.OrderID 
	AND bd.ArtworkTypeId=a.ArtworkTypeID 
	AND bd.Patterncode=b.PatternCode 
	AND bd.PatternDesc =b.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
OUTER APPLY(
	SELECT [Val]=MIN(Qty)
	FROM (
		SELECT [Qty]=ISNULL(b.PoQty,0)
		UNION 
		SELECT [Qty]=ISNULL(FarmOut.Value,0)
		UNION 
		SELECT [Qty]=ISNULL(FarmIn.Value,0)
	)tmp
)MinQty
where a.status='Approved' 
--and b.apqty < b.farmin
and a.artworktypeid = '{dr_artworkAp["artworktypeid"]}' 
and a.localsuppid = '{dr_artworkAp["localsuppid"]}' 
and a.mdivisionid='{Sci.Env.User.Keyword}'

";
                if (!MyUtility.Check.Empty(sp_b))
                {
                    strSQLCmd += $@" AND b.Orderid >= @sp1 ";
                }
                if (!MyUtility.Check.Empty(sp_e))
                {
                    strSQLCmd += $@" AND b.Orderid <= @sp2";
                }

                if (!MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@" AND b.id >= @artworkpoid1 ";
                }
                if (!MyUtility.Check.Empty(poid_e))
                {
                    strSQLCmd += $@" AND b.id <= @artworkpoid2";
                }


                strSQLCmd += @" 
ORDER BY b.id,b.ukey   
DROP TABLE #Bundle
";

                #region 準備sql參數資料
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";
                sp1.Value = sp_b;

                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp2.ParameterName = "@sp2";
                sp2.Value = sp_e;

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp3.ParameterName = "@artworkpoid1";
                sp3.Value = poid_b;

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                sp4.ParameterName = "@artworkpoid2";
                sp4.Value = poid_e;
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                cmds.Add(sp3);
                cmds.Add(sp4);
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd,cmds, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd, result); }
            }
            this.gridImportFromPO.AutoResizeColumns();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow ddr = gridImportFromPO.GetDataRow<DataRow>(e.RowIndex);
                    if ((decimal)e.FormattedValue > (decimal)ddr["balance"])
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Qty can't be more than balance");
                        return;
                    }

                    if ((decimal)e.FormattedValue > ((decimal)ddr["poqty"] - (decimal)ddr["accumulatedqty"]))
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Total Qty can't be more than PO Qty");
                        return;
                    }

                    //if ((decimal)e.FormattedValue > (decimal)ddr["PoQty"])
                    //{
                    //    e.Cancel = true;
                    //    MyUtility.Msg.WarningBox("<Qty> can not over PO qty!!");
                    //    return;
                    //}
                    ddr["Amount"] = (decimal)e.FormattedValue * (decimal)ddr["Price"];
                    ddr["ApQty"] = e.FormattedValue;
                }

            };


            this.gridImportFromPO.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImportFromPO.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImportFromPO)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("artworkpoid", header: "Artwork PO", iseditingreadonly: true)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                 .Text("artworkid", header: "Artwork", iseditingreadonly: true)      //3
                 .Numeric("Stitch", header: "Stitch", iseditable: true)    //4
                 .Text("PatternCode", header: "Cutpart Id", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)
                .Numeric("UnitPrice", header: "Unit Price", iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //7
                .Numeric("qtygarment", header: "Qty/GMT", iseditingreadonly: true, integer_places: 2) //8
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)  //9
                .Numeric("poqty", header: "PO Qty", iseditingreadonly: true)
                .Numeric("FarmOut", header: "Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("FarmIn", header: "Farm In", iseditingreadonly: true)
                .Numeric("AccumulatedQty", header: "Accu. Paid Qty", iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", iseditingreadonly: true)
                .Numeric("ApQty", header: "Qty",settings:ns)//14
                .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14);

            this.gridImportFromPO.Columns["apqty"].DefaultCellStyle.BackColor = Color.Pink;  //Qty

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            gridImportFromPO.ValidateControl();
            
            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;
            
            if (MyUtility.Check.Empty(dtImport)|| dtImport.Rows.Count == 0) return;
            
            DataRow[] dr2 = dtImport.Select("apqty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtImport.Select("Selected = 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = dt_artworkApDetail.Select(string.Format(" orderid = '{0}' and ArtworkId = '{1}' and patterncode = '{2}' and artworkpoid='{3}'", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString(), tmp["patterncode"].ToString(),tmp["artworkpoid"].ToString()));

                    if (findrow.Length > 0)
                    {
                       
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["poqty"] = tmp["poqty"];
                        findrow[0]["farmin"] = tmp["farmin"];
                        findrow[0]["FarmOut"] = tmp["FarmOut"];
                        findrow[0]["accumulatedqty"] = tmp["accumulatedqty"];
                        findrow[0]["balance"] = tmp["balance"];
                        findrow[0]["apqty"] = tmp["apqty"];
                        findrow[0]["amount"] = tmp["amount"];
                    }
                    else
                    {
                        tmp["id"] = dr_artworkAp["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_artworkApDetail.ImportRow(tmp);
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }
            this.Close();
        }
    }
}
