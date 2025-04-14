﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P10_ImportFromPO
    /// </summary>
    public partial class P10_ImportFromPO : Sci.Win.Subs.Base
    {
        private DataRow dr_artworkAp;
        private DataTable dt_artworkApDetail;

        private DataTable dtArtwork;

        /// <summary>
        /// P10_ImportFromPO
        /// </summary>
        /// <param name="master">master</param>
        /// <param name="detail">detail</param>
        public P10_ImportFromPO(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_artworkAp = master;
            this.dt_artworkApDetail = detail;
            this.Text += string.Format(" : {0} - {1}", this.dr_artworkAp["artworktypeid"].ToString(), this.dr_artworkAp["localsuppid"].ToString());
            this.displayBox1.BackColor = Color.Yellow;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            string sp_b = this.txtSPNoStart.Text;
            string sp_e = this.txtSPNoEnd.Text;
            string poid_b = this.txtArtworkPOIDStart.Text;
            string poid_e = this.txtArtworkPOIDEnd.Text;

            if (MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) && MyUtility.Check.Empty(poid_b) && MyUtility.Check.Empty(poid_e))
            {
                MyUtility.Msg.WarningBox("SP# and Artwork POID can't be emtpqy");
                this.txtSPNoStart.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                string strSQLCmd = string.Empty;

                strSQLCmd += $@"
SELECT  BDO.QTY 
	,BDO.Orderid 
    ,bdl.Article
    ,bd.SizeCode
	,s.ArtworkTypeId
	,bio.OutGoing 
	,bio.InComing
	,bd.Patterncode
	,bd.PatternDesc
INTO #Bundle
FROM Bundle_Detail bd WITH (NOLOCK) 
INNER JOIN Bundle bdl WITH (NOLOCK)  ON bdl.id=bd.id
INNER JOIN Bundle_Detail_Order BDO with(nolock) on BDO.BundleNo = BD.BundleNo
INNER JOIN BundleInOut bio WITH (NOLOCK)  ON bio.BundleNo = bd.BundleNo
INNER JOIN SubProcess s WITH (NOLOCK)  ON s.id= bio.SubProcessId
WHERE s.ArtworkTypeId='{this.dr_artworkAp["artworktypeid"]}' AND bio.RFIDProcessLocationID=''
";
                if (!MyUtility.Check.Empty(sp_b))
                {
                    strSQLCmd += $@" AND BDO.Orderid >= @sp1 ";
                }

                if (!MyUtility.Check.Empty(sp_e))
                {
                    strSQLCmd += $@" AND BDO.Orderid <= @sp2";
                }

                if (!MyUtility.Check.Empty(poid_b) && !MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@"
AND BDO.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID BETWEEN @artworkpoid1 AND  @artworkpoid2
)
";
                }
                else if (!MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@"
AND BDO.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID >= @artworkpoid1 
)
";
                }
                else if (!MyUtility.Check.Empty(poid_e))
                {
                    strSQLCmd += $@"
AND BDO.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID <= @artworkpoid2
)
";
                }

                strSQLCmd += $@"

Select 1 as Selected
        ,b.id as artworkpoid
        ,b.orderid
        ,b.artworkid
        ,b.stitch
        ,b.patterncode
        ,b.patterndesc
        ,b.unitprice
        ,b.qtygarment
        ,b.price
        ,b.poqty
		, [Farmout] = iif(at.SubconFarmInOutNotFromRFID = 1, ISNULL(FarmOutNotFromRFID.Value,0), ISNULL(FarmOut.Value,0))
		, [FarmIn] = iif(at.SubconFarmInOutNotFromRFID = 1, ISNULL(FarmInNotFromRFID.Value,0), ISNULL(FarmIn.Value,0))
        ,[AccumulatedQty] = b.ApQty
        ,[Balance] = b.PoQty - b.ApQty
		,[ApQty]= IIF(MinQty.Val - b.ApQty < 0 , 0 ,MinQty.Val - b.ApQty )
        ,b.ukey artworkpo_detailukey
        ,'' id
        ,[Amount] = 1.0 * IIF(MinQty.Val - b.ApQty < 0 , 0 ,MinQty.Val - b.ApQty ) * b.price --0.0 amount
        ,[LocalSuppCtn]=LocalSuppCtn.Val
        ,b.Article
        ,b.SizeCode
        ,o.FactoryID
        ,f.IsProduceFty
        ,oa.Remark
from ArtworkPO a WITH (NOLOCK) 
INNER JOIN ArtworkPO_Detail b WITH (NOLOCK)  ON  a.id = b.id 
inner join dbo.Orders o with (nolock) on b.OrderID = o.id
inner join ArtworkType at with (nolock) on at.ID = a.ArtworkTypeID
left join Factory f with (nolock) on f.ID = o.FactoryID
left join ArtworkReq_Detail ard with (nolock) on b.ArtworkReq_DetailUkey = ard.Ukey
left join Order_Artwork oa with (nolock) on ard.OrderArtworkUkey = oa.Ukey
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=b.OrderID 
    AND (bd.SizeCode = b.SizeCode or b.SizeCode = '')
    AND (bd.Article = b.Article or b.Article = '')
	AND bd.ArtworkTypeId=a.ArtworkTypeID 
	AND bd.Patterncode=b.PatternCode 
	AND bd.PatternDesc =b.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=b.OrderID 
    AND (bd.SizeCode = b.SizeCode or b.SizeCode = '')
    AND (bd.Article = b.Article or b.Article = '')
	AND bd.ArtworkTypeId=a.ArtworkTypeID 
	AND bd.Patterncode=b.PatternCode 
	AND bd.PatternDesc =b.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
OUTER APPLY(
	select [Value]= SUM(appd.APQty)
    from    ArtworkAP_Detail appd with (nolock)
    inner join  ArtworkPO_Detail apod with (nolock) on appd.ArtworkPo_DetailUkey = apod.Ukey
    inner join	ArtworkAP app with (nolock) on appd.id = app.id
    where app.Status != 'New' and
    exists (
        SELECT 1
        from ArtworkReq arr with (nolock)
        inner join ArtworkReq_Detail arrd with (nolock) on arrd.ID = arr.ID
        where	arrd.uKey = apod.ArtworkReq_DetailUkey and 
        		arrd.Orderid = ard.Orderid and
        		arrd.Article = ard.Article and
        		arrd.SizeCode = ard.SizeCode and
        		arr.ArtworkTypeID = a.ArtworkTypeID and
        		arrd.Patterncode = ard.PatternCode and
        		arrd.PatternDesc = ard.PatternDesc)
) FarmInNotFromRFID
OUTER APPLY(	
	select [Value]= SUM(apod.PoQty)
    from    ArtworkPO apo with (nolock)
    inner join  ArtworkPO_Detail apod with (nolock) on apod.ID = apo.ID
    where apo.Status != 'New' and
    exists (
        SELECT 1
        from ArtworkReq arr with (nolock)
        inner join ArtworkReq_Detail arrd with (nolock) on arrd.ID = arr.ID
        where	arrd.uKey = apod.ArtworkReq_DetailUkey and 
        		arrd.Orderid = ard.Orderid and
        		arrd.Article = ard.Article and
        		arrd.SizeCode = ard.SizeCode and
        		arr.ArtworkTypeID = a.ArtworkTypeID and
        		arrd.Patterncode = ard.PatternCode and
        		arrd.PatternDesc = ard.PatternDesc)
) FarmOutNotFromRFID
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
OUTER APPLY(
	SELECT [Val]= COUNT(LocalSuppID)
	FROM (
		SELECT DISTINCT apo.LocalSuppID 
		from ArtworkPO_Detail ad
		inner join ArtworkPO apo on apo.id = ad.id
		where ad.OrderID= b.OrderID
		and ad.PatternCode=b.PatternCode
		and ad.PatternDesc =b.PatternDesc
		and apo.ArtworkTypeID = '{this.dr_artworkAp["artworktypeid"]}' 
	)tmp
)LocalSuppCtn
where a.status='Approved' 
--and b.apqty < b.farmin
and a.artworktypeid = '{this.dr_artworkAp["artworktypeid"]}' 
and a.localsuppid = '{this.dr_artworkAp["localsuppid"]}' 
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
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@sp1",
                    Value = sp_b,
                };

                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@sp2",
                    Value = sp_e,
                };

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@artworkpoid1",
                    Value = poid_b,
                };

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@artworkpoid2",
                    Value = poid_e,
                };
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
                {
                    sp1,
                    sp2,
                    sp3,
                    sp4,
                };
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, cmds, out this.dtArtwork))
                {
                    if (this.dtArtwork.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }

                    this.listControlBindingSource1.DataSource = this.dtArtwork;

                    for (int i = 0; i < this.gridImportFromPO.Rows.Count; i++)
                    {
                        if ((int)this.gridImportFromPO.Rows[i].Cells["LocalSuppCtn"].Value >= 2)
                        {
                            this.gridImportFromPO.Rows[i].Cells["FarmOut"].Style.BackColor = Color.Yellow;
                            this.gridImportFromPO.Rows[i].Cells["farmin"].Style.BackColor = Color.Yellow;
                        }
                    }
                }
                else
                {
                    this.ShowErr(strSQLCmd, result);
                }
            }

            this.gridImportFromPO.AutoResizeColumns();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow ddr = this.gridImportFromPO.GetDataRow<DataRow>(e.RowIndex);
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

                    ddr["Amount"] = (decimal)e.FormattedValue * (decimal)ddr["Price"];
                    ddr["ApQty"] = e.FormattedValue;
                }
            };

            this.gridImportFromPO.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImportFromPO.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImportFromPO)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("artworkpoid", header: "Artwork PO", iseditingreadonly: true)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true)
                .Text("SizeCode", header: "Size", iseditingreadonly: true)
                .Numeric("Stitch", header: "Stitch", iseditable: true)
                .Text("PatternCode", header: "Cutpart Id", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)
                .Text("Remark", header: "Remark", iseditingreadonly: true)
                .Numeric("UnitPrice", header: "Unit Price", iseditingreadonly: true, decimal_places: 4, integer_places: 4)
                .Numeric("qtygarment", header: "Qty/GMT", iseditingreadonly: true, integer_places: 2)
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)
                .Numeric("poqty", header: "PO Qty", iseditingreadonly: true)
                .Numeric("FarmOut", header: "Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("FarmIn", header: "Farm In", iseditingreadonly: true)
                .Numeric("AccumulatedQty", header: "Accu. Paid Qty", iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", iseditingreadonly: true)
                .Numeric("ApQty", header: "Qty", settings: ns)
                .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14)
                .Numeric("LocalSuppCtn", header: "LocalSuppCtn", width: Widths.AnsiChars(0));

            this.gridImportFromPO.Columns["apqty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImportFromPO.Columns["LocalSuppCtn"].Visible = false;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridImportFromPO.ValidateControl();

            DataTable dtImport = (DataTable)this.listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtImport.Select("Selected = 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = this.dt_artworkApDetail.Select($" orderid = '{tmp["orderid"].ToString()}' and ArtworkId = '{tmp["ArtworkId"].ToString()}' and patterncode = '{tmp["patterncode"].ToString()}' and artworkpoid='{tmp["artworkpoid"].ToString()}' AND ArtworkPo_DetailUkey = {tmp["ArtworkPO_Detailukey"]} ");
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
                        tmp["id"] = this.dr_artworkAp["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_artworkApDetail.ImportRow(tmp);
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
