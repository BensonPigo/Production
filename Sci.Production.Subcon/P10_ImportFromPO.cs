using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
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
                string strSQLCmd = $@"
SELECT bdo.QTY 
	,bdo.Orderid 
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
INNER JOIN Bundle_Detail_Order bdo with(nolock) on bdo.BundleNo = BD.BundleNo
INNER JOIN BundleInOut bio WITH (NOLOCK)  ON bio.BundleNo = bd.BundleNo
INNER JOIN SubProcess s WITH (NOLOCK)  ON s.id= bio.SubProcessId
WHERE s.ArtworkTypeId='{this.dr_artworkAp["artworktypeid"]}' AND bio.RFIDProcessLocationID=''
";
                if (!MyUtility.Check.Empty(sp_b))
                {
                    strSQLCmd += $@" AND bdo.Orderid >= @sp1 ";
                }

                if (!MyUtility.Check.Empty(sp_e))
                {
                    strSQLCmd += $@" AND bdo.Orderid <= @sp2";
                }

                if (!MyUtility.Check.Empty(poid_b) && !MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@"
AND bdo.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID BETWEEN @artworkpoid1 AND  @artworkpoid2
)
";
                }
                else if (!MyUtility.Check.Empty(poid_b))
                {
                    strSQLCmd += $@"
AND bdo.Orderid IN (
    SELECT DISTINCT OrderID
    FROM ArtworkPO_Detail
    WHERE ID >= @artworkpoid1 
)
";
                }
                else if (!MyUtility.Check.Empty(poid_e))
                {
                    strSQLCmd += $@"
AND bdo.Orderid IN (
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
        ,[AccumulatedQty] = b.ApQty
        ,[Balance] = b.PoQty - b.ApQty
        ,b.ukey artworkpo_detailukey
        ,'' id
        ,[LocalSuppCtn]=LocalSuppCtn.Val
        ,b.Article
        ,b.SizeCode
        ,o.FactoryID
        ,f.IsProduceFty
        ,oa.Remark
        ,at.SubconFarmInOutfromSewOutput
        ,a.ArtworkTypeID
into #quoteDetailBase
from ArtworkPO a WITH (NOLOCK) 
INNER JOIN ArtworkPO_Detail b WITH (NOLOCK)  ON  a.id = b.id 
inner join dbo.Orders o with (nolock) on b.OrderID = o.id
inner join ArtworkType at with (nolock) on at.ID = a.ArtworkTypeID
left join Factory f with (nolock) on f.ID = o.FactoryID
left join ArtworkReq_Detail ard with (nolock) on b.ArtworkReq_DetailUkey = ard.Ukey
left join Order_Artwork oa with (nolock) on ard.OrderArtworkUkey = oa.Ukey
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

select v.*
into #tmp_SewingOutput_FarmInOutDate
from View_SewingOutput_FarmInOutDate v with (nolock) 
where exists (select 1 from #quoteDetailBase t where v.OrderId = t.OrderID)

select	q.*
		, [FarmOut] = iif(q.SubconFarmInOutfromSewOutput = 1, ISNULL(AccuFarmOut.Qty,0), ISNULL(FarmOut.Value,0))
		, [FarmIn] = iif(q.SubconFarmInOutfromSewOutput = 1, ISNULL(AccuFarmIn.Qty,0), ISNULL(FarmIn.Value,0))	
		, [AccuFarmOut] = iif(q.SubconFarmInOutfromSewOutput = 1, ISNULL(AccuFarmOut.Qty,0), ISNULL(FarmOut.Value,0))
		, [AccuFarmIn] = iif(q.SubconFarmInOutfromSewOutput = 1, ISNULL(AccuFarmIn.Qty,0), ISNULL(FarmIn.Value,0))		
        , [ApQty]= IIF(MinQty.Val - q.AccumulatedQty < 0 , 0 ,MinQty.Val - q.AccumulatedQty )
        , [Amount] = 1.0 * IIF(MinQty.Val - q.AccumulatedQty < 0 , 0 ,MinQty.Val - q.AccumulatedQty ) * q.price --0.0 amount
from #quoteDetailBase q
OUTER APPLY(
	SELECT [Value] = SUM(bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid= q.OrderID
    AND (bd.Article = q.Article or q.Article = '')
    AND (bd.SizeCode = q.SizeCode or q.SizeCode = '')
	AND bd.ArtworkTypeId = q.ArtworkTypeId
	AND bd.Patterncode = q.PatternCode 
	AND bd.PatternDesc = q.PatternDesc
	AND bd.OutGoing IS NOT NULL 
) FarmOut
OUTER APPLY(	
	SELECT [Value] = SUM(bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid= q.OrderID 
    AND (bd.Article = q.Article or q.Article = '')
    AND (bd.SizeCode = q.SizeCode or q.SizeCode = '')
	AND bd.ArtworkTypeId = q.ArtworkTypeId
	AND bd.Patterncode = q.PatternCode 
	AND bd.PatternDesc = q.PatternDesc
	AND bd.InComing IS NOT NULL
) FarmIn
OUTER APPLY(
	select [Qty] = SUM(v.Qty)
	from #tmp_SewingOutput_FarmInOutDate v with (nolock) 
	where q.OrderID = v.OrderId
	and (q.Article = v.Article or q.Article = '')
	and (q.SizeCode = v.SizeCode or q.SizeCode = '')
    and v.FarmOutDate < GETDATE()
) AccuFarmOut
OUTER APPLY(
	select [Qty] = SUM(v.Qty)
	from #tmp_SewingOutput_FarmInOutDate v with (nolock) 
	where q.OrderID = v.OrderId
	and (q.Article = v.Article or q.Article = '')
	and (q.SizeCode = v.SizeCode or q.SizeCode = '')
	and v.FarmInDate < GETDATE()
) AccuFarmIn
OUTER APPLY(
	SELECT [Val]=MIN(Qty)
	FROM (
		SELECT [Qty]=ISNULL(q.PoQty,0)
		UNION 
		SELECT [Qty]=ISNULL(FarmOut.Value,0)
		UNION 
		SELECT [Qty]=ISNULL(FarmIn.Value,0)
	)tmp
)MinQty

DROP TABLE #Bundle, #quoteDetailBase, #tmp_SewingOutput_FarmInOutDate
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

            #region Accu Farm Out 開窗
            DataGridViewGeneratorNumericColumnSettings dgsAccuFarmOut = new DataGridViewGeneratorNumericColumnSettings();
            dgsAccuFarmOut.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridImportFromPO.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null || int.Parse(dr["AccuFarmOut"].ToString()) == 0)
                {
                    return;
                }

                bool subconFarmInOutfromSewOutput = Prgs.IsSubconFarmInOutfromSewOutput(this.dr_artworkAp["artworktypeid"].ToString());
                string sqlCmd;
                if (subconFarmInOutfromSewOutput)
                {
                    sqlCmd = $@"
                select [Farm Out Date] = FORMAT(v.FarmOutDate, 'yyyy/MM/dd'), Qty = sum(v.Qty)
                from View_SewingOutput_FarmInOutDate v with(nolock)
                where v.OrderId = '{dr["Orderid"]}'" +
                    (!string.IsNullOrEmpty(dr["Article"].ToString()) ? $" and v.Article = '{dr["Article"]}'" : string.Empty) +
                    (!string.IsNullOrEmpty(dr["SizeCode"].ToString()) ? $" and v.SizeCode = '{dr["SizeCode"]}'" : string.Empty) +
                    @" 
                and v.FarmOutDate < GETDATE() 
                group by v.FarmOutDate";
                }
                else
                {
                    sqlCmd = $@"
                SELECT bdo.QTY, bdo.Orderid, bdl.Article, bd.SizeCode, s.ArtworkTypeId, OutGoing = FORMAT(bio.OutGoing, 'yyyy/MM/dd'), InComing = FORMAT(bio.InComing, 'yyyy/MM/dd'), bd.Patterncode, bd.PatternDesc
                INTO #Bundle
                FROM Bundle_Detail bd WITH (NOLOCK)
                INNER JOIN Bundle bdl WITH (NOLOCK) ON bdl.id=bd.id
                INNER JOIN Bundle_Detail_Order bdo WITH (NOLOCK) on bdo.BundleNo = BD.BundleNo
                INNER JOIN BundleInOut bio WITH (NOLOCK) ON bio.BundleNo = bd.BundleNo
                INNER JOIN SubProcess s WITH (NOLOCK) ON s.id= bio.SubProcessId
                WHERE bio.RFIDProcessLocationID = '' AND s.ArtworkTypeId = '{this.dr_artworkAp["artworktypeid"].ToString()}' 

                SELECT [Farm Out Date] = bd.OutGoing, Qty = SUM(bd.Qty)
                FROM #Bundle bd
                WHERE bd.Orderid = '{dr["Orderid"]}'" +
                    (!string.IsNullOrEmpty(dr["Article"].ToString()) ? $" and bd.Article = '{dr["Article"]}'" : string.Empty) +
                    (!string.IsNullOrEmpty(dr["SizeCode"].ToString()) ? $" and bd.SizeCode = '{dr["SizeCode"]}'" : string.Empty) +
                    $@" 
                AND bd.ArtworkTypeId = '{this.dr_artworkAp["artworktypeid"].ToString()}' 
                AND bd.Patterncode = '{dr["PatternCode"]}' 
                AND bd.PatternDesc = '{dr["PatternDesc"]}' 
                AND bd.OutGoing IS NOT NULL 
                group by bd.OutGoing";
                }

                DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: "Accu Farm Out");
            };
            #endregion

            #region Accu Farm In 開窗
            DataGridViewGeneratorNumericColumnSettings dgsAccuFarmIn = new DataGridViewGeneratorNumericColumnSettings();
            dgsAccuFarmIn.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridImportFromPO.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null || int.Parse(dr["AccuFarmIn"].ToString()) == 0)
                {
                    return;
                }

                bool subconFarmInOutfromSewOutput = Prgs.IsSubconFarmInOutfromSewOutput(this.dr_artworkAp["artworktypeid"].ToString());
                string sqlCmd;
                if (subconFarmInOutfromSewOutput)
                {
                    sqlCmd = $@"
            select [Farm In Date] = FORMAT(v.FarmInDate, 'yyyy/MM/dd'), Qty = sum(v.Qty)
            from View_SewingOutput_FarmInOutDate v with(nolock)
            where v.OrderId = '{dr["Orderid"]}'" +
                    (!string.IsNullOrEmpty(dr["Article"].ToString()) ? $" and v.Article = '{dr["Article"]}'" : string.Empty) +
                    (!string.IsNullOrEmpty(dr["SizeCode"].ToString()) ? $" and v.SizeCode = '{dr["SizeCode"]}'" : string.Empty) +
                    @" 
            and v.FarmInDate < GETDATE() 
            group by v.FarmInDate";
                }
                else
                {
                    sqlCmd = $@"
            SELECT bdo.QTY, bdo.Orderid, bdl.Article, bd.SizeCode, s.ArtworkTypeId, OutGoing = FORMAT(bio.OutGoing, 'yyyy/MM/dd'), InComing = FORMAT(bio.InComing, 'yyyy/MM/dd'), bd.Patterncode, bd.PatternDesc
            INTO #Bundle
            FROM Bundle_Detail bd WITH (NOLOCK)
            INNER JOIN Bundle bdl WITH (NOLOCK) ON bdl.id=bd.id
            INNER JOIN Bundle_Detail_Order bdo WITH (NOLOCK) on bdo.BundleNo = BD.BundleNo
            INNER JOIN BundleInOut bio WITH (NOLOCK) ON bio.BundleNo = bd.BundleNo
            INNER JOIN SubProcess s WITH (NOLOCK) ON s.id= bio.SubProcessId
            WHERE bio.RFIDProcessLocationID = '' AND s.ArtworkTypeId = '{this.dr_artworkAp["artworktypeid"].ToString()}' 

            SELECT [Farm In Date] = bd.InComing, Qty = SUM(bd.Qty)
            FROM #Bundle bd
            WHERE bd.Orderid = '{dr["Orderid"]}'" +
                    (!string.IsNullOrEmpty(dr["Article"].ToString()) ? $" and bd.Article = '{dr["Article"]}'" : string.Empty) +
                    (!string.IsNullOrEmpty(dr["SizeCode"].ToString()) ? $" and bd.SizeCode = '{dr["SizeCode"]}'" : string.Empty) +
                    $@" 
            AND bd.ArtworkTypeId = '{this.dr_artworkAp["artworktypeid"].ToString()}' 
            AND bd.Patterncode = '{dr["PatternCode"]}' 
            AND bd.PatternDesc = '{dr["PatternDesc"]}' 
            AND bd.InComing IS NOT NULL 
            group by bd.InComing";
                }

                DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: "Accu Farm In");
            };
            #endregion

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
                .Numeric("AccuFarmOut", header: "Farm Out", iseditingreadonly: true, settings: dgsAccuFarmOut)
                .Numeric("AccuFarmIn", header: "Farm In", iseditingreadonly: true, settings: dgsAccuFarmIn)
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
                        findrow[0]["AccuFarmIn"] = tmp["AccuFarmIn"];
                        findrow[0]["AccuFarmOut"] = tmp["AccuFarmOut"];
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
