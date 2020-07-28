using System;
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
using Sci.Production.PublicPrg;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P01_Import
    /// </summary>
    public partial class P01_Import : Sci.Win.Subs.Base
    {
        private DataRow dr_artworkpo;
        private DataTable dt_artworkpoDetail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private bool flag;
        private string poType;
        private DataTable dtArtwork;
        private bool isNeedPlanningB03Quote = false;
        private bool IsSintexSubcon = false;
        private string apvdate_b;
        private string apvdate_e;
        private string sciDelivery_b;
        private string sciDelivery_e;
        private string sp_b;
        private string sp_e;
        private string titleStitch = string.Empty;
        private string sqlFarmOutApply = @"OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid = bar.Orderid 
    AND (bd.Article = bar.Article or bar.Article = '')
    AND (bd.SizeCode = bar.SizeCode or bar.SizeCode = '')
	AND bd.ArtworkTypeId = bar.ArtworkTypeID
	AND bd.Patterncode = bar.PatternCode 
	AND bd.PatternDesc = bar.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid = bar.Orderid 
    AND (bd.Article = bar.Article or bar.Article = '')
    AND (bd.SizeCode = bar.SizeCode or bar.SizeCode = '')
	AND bd.ArtworkTypeId = bar.ArtworkTypeID
	AND bd.Patterncode = bar.PatternCode 
	AND bd.PatternDesc = bar.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn";

        /// <summary>
        /// P01_Import
        /// </summary>
        /// <param name="master">master</param>
        /// <param name="detail">detail</param>
        /// <param name="fuc">fuc</param>
        /// <param name="isNeedPlanningB03Quote">isNeedPlanningB03Quote</param>
        public P01_Import(DataRow master, DataTable detail, string fuc, bool isNeedPlanningB03Quote = false)
        {
            this.InitializeComponent();
            this.dr_artworkpo = master;
            this.dt_artworkpoDetail = detail;
            this.flag = fuc == "P01";
            if (this.flag)
            {
                this.poType = "O";
                this.Text += " (Sub-con Purchase Order)";
            }
            else
            {
                this.poType = "I";
                this.Text += " (In-House Requisition)";
            }

            this.Text += string.Format(" : {0}", this.dr_artworkpo["LocalSuppID"].ToString());

            this.isNeedPlanningB03Quote = isNeedPlanningB03Quote;
            this.IsSintexSubcon = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select IsSintexSubcon from LocalSupp with (nolock) where ID = '{master["localsuppid"]}'"));

            this.titleStitch = MyUtility.GetValue.Lookup($"select iif(artworkunit='','PCS',artworkunit) from artworktype WITH (NOLOCK) where id='{master["ArtworkTypeID"]}'");
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.apvdate_b = null;
            this.apvdate_e = null;
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;

            if (this.dateApproveDate.Value1 != null)
            {
                this.apvdate_b = this.dateApproveDate.Text1;
            }

            if (this.dateApproveDate.Value2 != null)
            {
                this.apvdate_e = this.dateApproveDate.Text2;
            }

            if (this.dateSCIDelivery.Value1 != null)
            {
                this.sciDelivery_b = this.dateSCIDelivery.Text1;
            }

            if (this.dateSCIDelivery.Value2 != null)
            {
                this.sciDelivery_e = this.dateSCIDelivery.Text2;
            }

            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;

            if (this.apvdate_b == null && this.apvdate_e == null &&
                this.sciDelivery_b == null && this.sciDelivery_e == null &&
                string.IsNullOrWhiteSpace(this.sp_b) && string.IsNullOrWhiteSpace(this.sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < SP# > can't be empty!!");
                this.dateApproveDate.Focus1();
                return;
            }

            if (!MyUtility.Check.Empty(this.apvdate_e))
            {
                this.apvdate_e = Convert.ToDateTime(this.apvdate_e).AddDays(1).ToString("yyyyMMdd");
            }

            string strSQLCmd = string.Empty;
            string sqlWhere = string.Empty;
            if (!(this.dateSCIDelivery.Value1 == null))
            {
                sqlWhere += string.Format(" and o.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!(this.dateSCIDelivery.Value2 == null))
            {
                sqlWhere += string.Format(" and o.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!(this.dateApproveDate.Value1 == null))
            {
                sqlWhere += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", this.apvdate_b);
            }

            if (!(this.dateApproveDate.Value2 == null))
            {
                sqlWhere += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", this.apvdate_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                sqlWhere += string.Format("     and o.ID between '{0}' and '{1}'", this.sp_b, this.sp_e);
            }

            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                sqlWhere += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            strSQLCmd += $@"
select  orderid = ard.OrderID
        , OrderQty = OrderQty.val
        , IssueQty.IssueQty 
        , ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , ard.Stitch 
        , ard.PatternDesc
        , ard.QtyGarment
        , Style = o.StyleID
		, o.POID
        , [ArtworkReqID] = ar.ID
        , o.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
        , ard.Article
        , ard.SizeCode
        , o.StyleUkey
        , ar.LocalSuppId
into #baseArtworkReq
from orders o WITH (NOLOCK) 
inner join ArtworkReq_Detail ard with (nolock) on   ard.OrderId = o.ID and 
                                                    ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.Status = 'Approved' 
outer apply (select [val] = sum(oq.Qty) from Order_Qty oq with (nolock) where  oq.ID = o.ID and 
                                                                    (oq.Article = ard.Article or ard.Article = '') and
                                                                    (oq.SizeCode = ard.SizeCode or ard.SizeCode = '')
            ) OrderQty  
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail ad with (nolock)
        inner join ArtworkPO a with (nolock) on AD.ID = A.ID  
        where   a.ArtworkTypeID = ar.ArtworkTypeID    and
		        ad.OrderID = o.ID                                         and
                ad.Article = ard.Article                                 and
                ad.SizeCode = ard.SizeCode                               and
                ad.PatternCode = ard.PatternCode                         and
                ad.PatternDesc = ard.PatternDesc                         and
                ad.ArtworkID = ard.ArtworkID                             and
		        a.Status = 'Approved'
) IssueQty
where   f.IsProduceFty=1 and 
        o.category  in ('B','S') and
        o.MDivisionID='{Env.User.Keyword}' and 
        ar.ArtworkTypeID = '{this.dr_artworkpo["artworktypeid"]}' and 
        ar.LocalSuppId = '{this.dr_artworkpo["localsuppid"]}' and
        (o.Junk=0 or o.Junk=1 and o.NeedProduction=1)
        {sqlWhere} 


SELECT  bd.QTY 
	,bdl.Orderid 
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
INNER JOIN BundleInOut bio WITH (NOLOCK)  ON bio.BundleNo = bd.BundleNo
INNER JOIN SubProcess s WITH (NOLOCK)  ON s.id= bio.SubProcessId
WHERE   bio.RFIDProcessLocationID='' and
        exists (select 1 from #baseArtworkReq bar where bar.orderid = bdl.Orderid)
";

            if (this.isNeedPlanningB03Quote)
            {
                if (this.IsSintexSubcon && Prgs.CheckIsArtworkorUseArtwork(MyUtility.Convert.GetString(this.dr_artworkpo["artworktypeid"])))
                {
                    strSQLCmd += this.QuoteIsSintexSubcon();
                }
                else
                {
                    strSQLCmd += this.QuoteFromPlanningB03();
                }
            }
            else
            {
                strSQLCmd += this.QuoteFromTmsCost();
            }

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd, out this.dtArtwork))
            {
                DualResult resultGetSpecialRecordData = this.GetSpecialRecordData();
                if (!resultGetSpecialRecordData)
                {
                    this.ShowErr(resultGetSpecialRecordData);
                }

                if (this.dtArtwork.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtArtwork;
                this.gridBatchImport.AutoResizeColumns();

                foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
                {
                    this.DetalGridCellEditChange(dr.Index);
                }

                this.gridBatchImport.ClearSelection();
            }
            else
            {
                this.ShowErr(strSQLCmd, result);
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow ddr = this.gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["UnitPrice"] = Convert.ToDecimal(e.FormattedValue);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["qtygarment"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["PoQty"]) * Convert.ToInt32(ddr["qtygarment"]);
                ddr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                DataRow ddr = this.gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["PoQty"]) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings col_ReqQtyIssueQty = new DataGridViewGeneratorNumericColumnSettings();

            col_ReqQtyIssueQty.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Subcon.P01_AccuPoQtyList(dr);
                frm.ShowDialog();
            };

            this.gridBatchImport.Font = new Font("Arial", 9);
            this.gridBatchImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("IssueQty", header: "Accu. PO Qty", iseditingreadonly: true, settings: col_ReqQtyIssueQty)
                .Numeric("PoQty", header: "Req. Qty", iseditingreadonly: true)
                .Date("sewinline", header: "Sewinline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SciDelivery", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true) // 5
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("coststitch", header: "Cost" + Environment.NewLine + "(Pcs/Stitch)", iseditingreadonly: true)
                .Numeric("Stitch", header: this.titleStitch, iseditable: true, iseditingreadonly: true) // 7
                .Text("PatternCode", header: "Cut Part", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cut Part Name", iseditingreadonly: true)
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, settings: ns2, iseditingreadonly: true) // 10
                .Numeric("Cost", header: "Cost(USD)", settings: ns, iseditingreadonly: true, decimal_places: 4, integer_places: 4) // 11
                .Numeric("UnitPrice", header: "Unit Price", settings: ns, iseditable: true, decimal_places: 4, integer_places: 4) // 12
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5) // 13
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14)
                .Numeric("FarmOut", header: "Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("FarmIn", header: "Farm In", iseditingreadonly: true)
                .Text("IrregularQtyReason", header: "Irregular Qty Reason", iseditingreadonly: true);  // 14

            // this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            // this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty/GMT
            this.gridBatchImport.Columns["UnitPrice"].DefaultCellStyle.BackColor = Color.Pink;  // UnitPrice
            this.gridBatchImport.Columns["Cost"].Visible = this.flag;
            this.gridBatchImport.Columns["UnitPrice"].Visible = this.flag;
            this.gridBatchImport.Columns["Price"].Visible = this.flag;
            this.gridBatchImport.Columns["Amount"].Visible = this.flag;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");

            if (dr2.Length > 0 && this.flag)
            {
                MyUtility.Msg.WarningBox("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length > 0)
            {
                StringBuilder ids = new StringBuilder();

                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = this.dt_artworkpoDetail.Select($@"orderid = '{tmp["orderid"].ToString()}' and 
ArtworkId = '{tmp["ArtworkId"].ToString()}' and 
patterncode = '{tmp["patterncode"].ToString()}' and 
cost='{tmp["Cost"]}' and
ArtworkReqID = '{tmp["ArtworkReqID"]}' and
Article =  '{tmp["Article"]}' and
SizeCode = '{tmp["SizeCode"]}'
");

                    if (findrow.Length > 0)
                    {
                        findrow[0]["unitprice"] = tmp["unitprice"];
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["amount"] = tmp["amount"];
                        findrow[0]["poqty"] = tmp["PoQty"];
                        findrow[0]["qtygarment"] = 1;
                        findrow[0]["FarmOut"] = tmp["FarmOut"];
                        findrow[0]["FarmIn"] = tmp["FarmIn"];
                    }
                    else
                    {
                        tmp["id"] = this.dr_artworkpo["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_artworkpoDetail.ImportRow(tmp);
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

        private string QuoteFromPlanningB03()
        {
            string strSQLCmd = string.Empty;

            strSQLCmd += $@"
select  distinct
        Selected = 0
        , sao.LocalSuppId
        , id = ''
        , bar.OrderID
        , bar.OrderQty
        , bar.IssueQty 
        , [PoQty] = bar.ReqQty
        , bar.ArtworkTypeID
        , bar.ArtworkID
        , bar.PatternCode
        , bar.SewInLIne
        , bar.SciDelivery
        , coststitch = oa.qty
        , bar.Stitch 
        , bar.PatternDesc
        , bar.QtyGarment
        , Cost = iif(at.isArtwork = 1,vsa.Cost,sao.Price)
        , unitprice = isnull(sao.Price,0)
        , price = sao.Price
        , amount = bar.ReqQty * sao.Price
        , bar.Style
		, bar.POID
        , bar.ArtworkReqID
        , bar.Category
        , bar.IrregularQtyReason
		, [Farmout] = ISNULL(FarmOut.Value,0)
		, [FarmIn] = ISNULL(FarmIn.Value,0)
        , bar.Article
        , bar.SizeCode
        , [QuotArticle] = isnull(oa.Article, '')
        , [QuotSizeCode] = isnull(sao.SizeCode, '')
into #quoteFromPlanningB03Base
from  #baseArtworkReq bar
left join dbo.View_Order_Artworks oa on   oa.ID = bar.OrderID and
										  (bar.Article = oa.Article or bar.Article = '') and
                                          bar.ArtworkTypeID = oa.ArtworkTypeID and
                                          bar.ArtworkID = oa.ArtworkID and 
                                          bar.PatternCode = oa.PatternCode and 
                                          bar.PatternDesc = oa.PatternDesc 
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = bar.StyleUkey and 
                                        vsa.Article = oa.Article and 
                                        vsa.ArtworkID = oa.ArtworkID and
										vsa.ArtworkName = oa.ArtworkName and 
                                        vsa.ArtworkTypeID = oa.ArtworkTypeID and 
                                        vsa.PatternCode = oa.PatternCode and
										vsa.PatternDesc = oa.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on   sao.Ukey = vsa.StyleArtworkUkey and 
                                                    sao.PriceApv = 'Y' and 
                                                    sao.Price > 0 and 
                                                    sao.LocalSuppId = bar.LocalSuppId and
												    sao.SizeCode = bar.SizeCode
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
{this.sqlFarmOutApply}
";

            strSQLCmd += @"
select	* ,
		[QuotSeq] = ROW_NUMBER() OVER (PARTITION BY orderid,Article,SizeCode,ArtworkID,PatternCode,PatternDesc ORDER BY QuotArticle,QuotSizeCode desc)
into #quoteFromPlanningB03
from #quoteFromPlanningB03Base 

delete #quoteFromPlanningB03 where Article = '' and SizeCode <> QuotSizeCode and QuotSeq > 1

--將報價相同的Article資料合併
select distinct
        Selected
        , LocalSuppId
        , id
        , orderid
        , OrderQty
        , IssueQty 
        , PoQty
        , ArtworkTypeID
        , ArtworkID
        , PatternCode
        , SewInLIne
        , SciDelivery
        , coststitch
        , Stitch 
        , PatternDesc
        , QtyGarment
        , Cost
        , unitprice
        , price
        , amount
        , Style 
		, POID
        , ArtworkReqID
        , Article
        , SizeCode
        , Category
        , IrregularQtyReason
		,[Farmout]
		,[FarmIn]
from #quoteFromPlanningB03 main
";

            return strSQLCmd;
        }

        private string QuoteFromTmsCost()
        {
            string strSQLCmd = string.Empty;

            strSQLCmd += $@"
select  Selected = 0
        , ot.LocalSuppID
        , id = ''
        , bar.OrderID
        , bar.OrderQty
        , bar.IssueQty 
        , [PoQty] = bar.ReqQty
        , bar.ArtworkTypeID
        , bar.ArtworkID
        , bar.PatternCode
        , bar.SewInLIne
        , bar.SciDelivery
        , coststitch = 1
        , bar.Stitch
        , bar.PatternDesc
        , bar.QtyGarment
        , Cost = ot.Price
        , unitprice = isnull(ot.Price,0)
        , price = ot.Price * isnull (ot.Qty, 1)
        , amount = bar.ReqQty * ot.Price
        , Style = o.StyleID
        , bar.ArtworkReqID
        , bar.Article
        , bar.SizeCode
        , o.Category
        , bar.IrregularQtyReason
from #baseArtworkReq bar
inner join dbo.Orders o with (nolock) on o.ID = bar.OrderID
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = bar.OrderID and ot.ArtworkTypeID = bar.ArtworkTypeID
{this.sqlFarmOutApply}
where 1 = 1
";

            if (this.poType == "O")
            {
                strSQLCmd += @"  and ((o.Category = 'B' and ot.InhouseOSP='O' and ot.price > 0) or o.category !='B')";
            }
            else
            {
                strSQLCmd += $" and ot.InhouseOSP = 'I'";
            }

            return strSQLCmd;
        }

        private string QuoteIsSintexSubcon()
        {
            string strSQLCmd = string.Empty;

            strSQLCmd += $@"
select  Selected = 0
        , id = ''
        , bar.OrderID
        , bar.OrderQty
        , bar.IssueQty 
        , [PoQty] = bar.ReqQty
        , bar.ArtworkTypeID
        , bar.ArtworkID
        , bar.PatternCode
        , bar.SewInLIne
        , bar.SciDelivery
        , coststitch = sum(isnull(oa.Qty, 0)) / count(1)
        , bar.Stitch
        , bar.PatternDesc
        , bar.QtyGarment
        , Cost = sum(isnull(oa.Cost, 0)) / count(1)
        , unitprice = sum(isnull(oa.Cost, 0)) / count(1)
        , price = sum(isnull(oa.Cost, 0)) / count(1)
        , amount = bar.ReqQty *  sum(isnull(oa.Cost, 0)) / count(1)
        , Style = o.StyleID
        , bar.ArtworkReqID
        , bar.Article
        , bar.SizeCode
        , bar.Category
        , bar.IrregularQtyReason
		, [Farmout] = ISNULL(FarmOut.Value,0)
		, [FarmIn] = ISNULL(FarmIn.Value,0)
from  #baseArtworkReq bar
inner join Orders o with (nolock) on o.ID = bar.OrderID
inner join dbo.Order_Artwork oa WITH (NOLOCK) on oa.ID = bar.OrderID and 
                                                 (bar.Article = oa.Article or bar.Article = '') and
                                                 oa.ArtworkTypeID = bar.ArtworkTypeID and
                                                 oa.ArtworkID = bar.ArtworkID      and
                                                 oa.PatternCode = bar.PatternCode  and
                                                 oa.PatternDesc = bar.PatternDesc 
{this.sqlFarmOutApply}
where  ((o.Category = 'B' and  oa.price > 0) or (o.category !='B'))
group by  bar.OrderID
        , bar.OrderQty
        , bar.IssueQty 
        , bar.ReqQty
        , bar.ArtworkTypeID
        , bar.ArtworkID
        , bar.PatternCode
        , bar.SewInLIne
        , bar.SciDelivery
        , bar.Stitch
        , bar.PatternDesc
        , bar.QtyGarment
        , o.StyleID
        , bar.Article
        , bar.SizeCode
        , bar.Category
        , bar.IrregularQtyReason 
		, FarmOut.Value
		, FarmIn.Value
        , bar.ArtworkReqID
";

            return strSQLCmd;
        }

        private DualResult GetSpecialRecordData()
        {
            string sqlWhere = string.Empty;
            if (!(this.dateSCIDelivery.Value1 == null))
            {
                sqlWhere += string.Format(" and o.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!(this.dateSCIDelivery.Value2 == null))
            {
                sqlWhere += string.Format(" and o.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!(this.dateApproveDate.Value1 == null))
            {
                sqlWhere += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", this.apvdate_b);
            }

            if (!(this.dateApproveDate.Value2 == null))
            {
                sqlWhere += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", this.apvdate_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                sqlWhere += string.Format("     and o.ID between '{0}' and '{1}'", this.sp_b, this.sp_e);
            }

            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                sqlWhere += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            string sqlGetSpecialRecordData = $@"
select	ar.LocalSuppId
        , id = ''
        , ard.orderid
        , OrderQty = o.qty
        , IssueQty.IssueQty 
        , [PoQty] = ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = 1
        , ard.Stitch 
        , ard.PatternDesc
        , ard.QtyGarment
        , Cost = 0.0
        , unitprice = 0.0
        , price = 0.0
        , amount = 0.0
        , Style = o.StyleID
		, o.POID
        , [ArtworkReqID] = ar.ID
        , [Article] = (SELECT Stuff((select concat( ',',Article)   from Order_Article with (nolock) where ID = o.ID FOR XML PATH('')),1,1,'') )
        , o.Category
from ArtworkReq ar  with (nolock)
inner join ArtworkReq_Detail ard with (nolock) on ar.ID = ard.ID 
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
inner join orders o WITH (NOLOCK) on ard.OrderId = o.ID  
inner join ArtworkType at with (nolock) on ar.ArtworkTypeID = at.ID
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= ard.PatternCode
) IssueQty
where  ar.ArtworkTypeID = '{this.dr_artworkpo["artworktypeid"]}' and ar.Status = 'Approved' and ar.LocalSuppId = '{this.dr_artworkpo["LocalSuppId"]}' and  ard.ArtworkPOID = '' and
    (
	(o.Category = 'B' and at.IsSubprocess = 1 and at.isArtwork = 0 and at.Classify = 'O') 
	or 
	(o.category = 'S')
	) and
    not exists( select 1 from #tmpArtwork t where t.OrderID = ard.orderid)
    {sqlWhere}
";
            DataTable dtResult = new DataTable();
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtArtwork, "OrderID", sqlGetSpecialRecordData, out dtResult, temptablename: "#tmpArtwork");

            if (!result)
            {
                return result;
            }

            // 若有special資料舊merge回主table中
            if (dtResult.Rows.Count > 0)
            {
                this.dtArtwork.Merge(dtResult);
            }

            return new DualResult(true);
        }

        private void DetalGridCellEditChange(int index)
        {
            string orderCategory = this.gridBatchImport.GetDataRow(index)["Category"].ToString();

            if (orderCategory != "S" && this.isNeedPlanningB03Quote)
            {
                this.gridBatchImport.Rows[index].Cells["unitprice"].ReadOnly = true;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Black;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.White; // Unit Price

                decimal unitPrice = (decimal)this.gridBatchImport.GetDataRow(index)["unitprice"];
                if (unitPrice == 0)
                {
                    this.gridBatchImport.Rows[index].Cells["Selected"].ReadOnly = true;
                    this.gridBatchImport.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(229, 108, 126);
                    this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.FromArgb(229, 108, 126); // Unit Price
                }
            }
            else
            {
                this.gridBatchImport.Rows[index].Cells["unitprice"].ReadOnly = false;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Red;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.Pink; // Unit Price
            }
        }
    }
}
