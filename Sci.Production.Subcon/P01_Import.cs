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
    public partial class P01_Import : Sci.Win.Subs.Base
    {
        DataRow dr_artworkpo;
        DataTable dt_artworkpoDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtArtwork;
        bool isNeedPlanningB03Quote = false;
        bool IsSintexSubcon = false;
        string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, sp_b, sp_e;
        string titleStitch = "";

        public P01_Import(DataRow master, DataTable detail, string fuc, bool isNeedPlanningB03Quote = false)
        {
            InitializeComponent();
            dr_artworkpo = master;
            dt_artworkpoDetail = detail;
            flag = fuc == "P01";
            if (flag)
            {
                poType = "O";
                this.Text += " (Sub-con Purchase Order)";
            }
            else
            {
                poType = "I";
                this.Text += " (In-House Requisition)";
            }

            this.Text += string.Format(" : {0}", dr_artworkpo["LocalSuppID"].ToString());

            this.isNeedPlanningB03Quote = isNeedPlanningB03Quote;
            this.IsSintexSubcon = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select IsSintexSubcon from LocalSupp with (nolock) where ID = '{master["localsuppid"]}'"));

            this.titleStitch = MyUtility.GetValue.Lookup($"select iif(artworkunit='','PCS',artworkunit) from artworktype WITH (NOLOCK) where id='{master["ArtworkTypeID"]}'");
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {

            this.apvdate_b = null;
            this.apvdate_e = null;
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }


            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < SP# > can't be empty!!");
                dateApproveDate.Focus1();
                return;
            }

            if (!MyUtility.Check.Empty(apvdate_e))
            {
                apvdate_e = Convert.ToDateTime(apvdate_e).AddDays(1).ToString("yyyyMMdd");
            }

            string strSQLCmd = string.Empty;
            if (isNeedPlanningB03Quote)
            {
                if (this.IsSintexSubcon && Prgs.CheckIsArtworkorUseArtwork(MyUtility.Convert.GetString(this.dr_artworkpo["artworktypeid"])))
                {
                    strSQLCmd = this.QuoteIsSintexSubcon();
                }
                else
                {
                    strSQLCmd = this.QuoteFromPlanningB03();
                }
            }
            else
            {
                strSQLCmd = this.QuoteFromTmsCost();
            }

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
            {
                DualResult resultGetSpecialRecordData = GetSpecialRecordData();
                if (!resultGetSpecialRecordData)
                {
                    this.ShowErr(resultGetSpecialRecordData);
                }

                if (dtArtwork.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
               
                listControlBindingSource1.DataSource = dtArtwork;
                this.gridBatchImport.AutoResizeColumns();

                foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
                {
                    this.DetalGridCellEditChange(dr.Index);
                }
                this.gridBatchImport.ClearSelection();
            }
            else { ShowErr(strSQLCmd, result); }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow ddr = gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["UnitPrice"] = Convert.ToDecimal(e.FormattedValue);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["qtygarment"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["PoQty"]) * Convert.ToInt32(ddr["qtygarment"]);
                ddr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                DataRow ddr = gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["PoQty"]) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings col_ReqQtyIssueQty = new DataGridViewGeneratorNumericColumnSettings();

            col_ReqQtyIssueQty.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Subcon.P01_AccuPoQtyList(dr);
                frm.ShowDialog();
            };


            this.gridBatchImport.Font = new Font("Arial", 9);
            this.gridBatchImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridBatchImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("IssueQty", header: "Accu. PO Qty", iseditingreadonly: true, settings: col_ReqQtyIssueQty)
                .Numeric("PoQty", header: "Req. Qty", iseditingreadonly: true)
                .Date("sewinline", header: "Sewinline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SciDelivery", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true)      //5
                .Numeric("coststitch", header: "Cost" + Environment.NewLine + "(Pcs/Stitch)", iseditingreadonly: true)
                .Numeric("Stitch", header: this.titleStitch, iseditable: true, iseditingreadonly: true)    //7
                .Text("PatternCode", header: "Cut Part", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cut Part Name", iseditingreadonly: true)
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, settings: ns2, iseditingreadonly: true) //10
                .Numeric("Cost", header: "Cost(USD)", settings: ns, iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //11
                .Numeric("UnitPrice", header: "Unit Price", settings: ns, iseditable: true, decimal_places: 4, integer_places: 4)  //12
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)  //13
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14)
                .Numeric("FarmOut", header: "Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("FarmIn", header: "Farm In", iseditingreadonly: true)
                .Text("IrregularQtyReason", header: "Irregular Qty Reason", iseditingreadonly: true);  //14


            //this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            //this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty/GMT
            this.gridBatchImport.Columns["UnitPrice"].DefaultCellStyle.BackColor = Color.Pink;  //UnitPrice
            this.gridBatchImport.Columns["Cost"].Visible = flag;
            this.gridBatchImport.Columns["UnitPrice"].Visible = flag;
            this.gridBatchImport.Columns["Price"].Visible = flag;
            this.gridBatchImport.Columns["Amount"].Visible = flag;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;
            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");

            if (dr2.Length > 0 && flag)
            {
                MyUtility.Msg.WarningBox("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length > 0)
            {
                bool yns = false;
                StringBuilder ids = new StringBuilder();
                
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = dt_artworkpoDetail.Select($@"orderid = '{tmp["orderid"].ToString()}' and 
ArtworkId = '{tmp["ArtworkId"].ToString()}' and 
patterncode = '{tmp["patterncode"].ToString()}' and 
cost='{tmp["Cost"]}' and
ArtworkReqID = '{tmp["ArtworkReqID"]}'
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
                        tmp["id"] = dr_artworkpo["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_artworkpoDetail.ImportRow(tmp);
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
Declare @sp1 varchar(16)= '{sp_b}'
Declare @sp2 varchar(16)= '{sp_e}'

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
WHERE bio.RFIDProcessLocationID=''
";
            if (!MyUtility.Check.Empty(sp_b))
            {
                strSQLCmd += $@" AND bdl.Orderid >= @sp1 ";
            }
            if (!MyUtility.Check.Empty(sp_e))
            {
                strSQLCmd += $@" AND bdl.Orderid <= @sp2";
            }

            strSQLCmd += string.Format(@"
select distinct Selected = 0
        , sao.LocalSuppId
        , id = ''
        , orderid = ard.OrderID
        , OrderQty = o.qty
        , IssueQty.IssueQty 
        , [PoQty] = ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = oa.qty
        , ard.Stitch 
        , ard.PatternDesc
        , ard.QtyGarment
        , Cost = iif(at.isArtwork = 1,vsa.Cost,sao.Price)
        , unitprice = isnull(sao.Price,0)
        , price = sao.Price
        , amount = ard.ReqQty * sao.Price
        , Style = o.StyleID
		, o.POID
        , [ArtworkReqID] = ar.ID
        , oa.Article
        , o.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
		,[Farmout] = ISNULL(FarmOut.Value,0)
		,[FarmIn] = ISNULL(FarmIn.Value,0)
into #quoteFromPlanningB03
from  orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID and
														vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID and vsa.PatternCode = oa.PatternCode and
														vsa.PatternDesc = oa.PatternDesc 
inner join ArtworkReq_Detail ard with (nolock) on   ard.OrderId = o.ID and 
                                                    ard.ArtworkID = oa.ArtworkID and 
                                                    ard.PatternCode = oa.PatternCode and 
                                                    ard.PatternDesc = oa.PatternDesc and
                                                    ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = oa.ArtworkTypeID and ar.Status = 'Approved' and ar.LocalSuppId = '{2}'
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
left join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.PriceApv = 'Y' and sao.Price > 0 and sao.LocalSuppId = ar.LocalSuppId
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= oa.PatternCode
) IssueQty
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=o.ID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID
	AND bd.Patterncode = oa.PatternCode 
	AND bd.PatternDesc = oa.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=o.ID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID
	AND bd.Patterncode = oa.PatternCode 
	AND bd.PatternDesc = oa.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
where f.IsProduceFty=1
--and o.PulloutComplete = 0
and o.category  in ('B','S')
and o.MDivisionID='{0}' and oa.ArtworkTypeID = '{1}' and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1)
", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"], dr_artworkpo["localsuppid"]);

            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }
            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                strSQLCmd += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            strSQLCmd += @"
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
        , [Article] = (SELECT Stuff((select concat( ',',Article)   
                            from #quoteFromPlanningB03 
                            where   orderid = main.orderid and 
                                    ArtworkID = main.ArtworkID and
                                    PatternCode = main.PatternCode and 
                                    PatternDesc = main.PatternDesc and
                                    unitprice = main.unitprice FOR XML PATH('')),1,1,'') )
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
Declare @sp1 varchar(16)= '{sp_b}'
Declare @sp2 varchar(16)= '{sp_e}'

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
WHERE bio.RFIDProcessLocationID=''
";
            if (!MyUtility.Check.Empty(sp_b))
            {
                strSQLCmd += $@" AND bdl.Orderid >= @sp1 ";
            }
            if (!MyUtility.Check.Empty(sp_e))
            {
                strSQLCmd += $@" AND bdl.Orderid <= @sp2";
            }

            strSQLCmd += $@"
select  Selected = 0
        , ot.LocalSuppID
        , id = ''
        , orderid = ard.OrderID
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
        , Cost = ot.Price
        , unitprice = isnull(ot.Price,0)
        , price = ot.Price * isnull (ot.Qty, 1)
        , amount = ard.ReqQty * ot.Price
        , Style = o.StyleID
        , [ArtworkReqID] = ar.ID
        , [Article] = (SELECT Stuff((select concat( ',',Article)   from Order_Article with (nolock) where ID = o.ID FOR XML PATH('')),1,1,'') )
        , o.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
from ArtworkReq ar WITH (NOLOCK) 
inner join ArtworkReq_Detail ard with (nolock) on ar.ID = ard.ID and ard.ArtworkPOID = ''
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
inner join orders o WITH (NOLOCK) on ard.OrderID = o.ID
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID and ot.ArtworkTypeID = ar.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= ard.PatternCode
) IssueQty
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=ard.OrderID
	AND bd.ArtworkTypeId = ar.ArtworkTypeID
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc = ard.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=ard.OrderID
	AND bd.ArtworkTypeId = ar.ArtworkTypeID
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc = ard.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
where   
f.IsProduceFty=1
and o.category  in ('B','S')
and ar.Status = 'Approved'
and ar.LocalSuppID = '{dr_artworkpo["localsuppid"]}'
";

            strSQLCmd += string.Format(" and o.MDivisionID='{0}' and ar.ArtworkTypeID = '{1}' and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1) ", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"]);
            if (poType == "O")
            {
                strSQLCmd += @"  and ((o.Category = 'B' and ot.InhouseOSP='O' and ot.price > 0) or o.category !='B')";
            }
            else
            {
                strSQLCmd += $" and ot.InhouseOSP = 'I'";
            }
            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }
            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                strSQLCmd += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            return strSQLCmd;
        }

        private string QuoteIsSintexSubcon()
        {
            string strSQLCmd = string.Empty;

            strSQLCmd += $@"
Declare @sp1 varchar(16)= '{sp_b}'
Declare @sp2 varchar(16)= '{sp_e}'

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
WHERE bio.RFIDProcessLocationID=''
";
            if (!MyUtility.Check.Empty(sp_b))
            {
                strSQLCmd += $@" AND bdl.Orderid >= @sp1 ";
            }
            if (!MyUtility.Check.Empty(sp_e))
            {
                strSQLCmd += $@" AND bdl.Orderid <= @sp2";
            }

            strSQLCmd += string.Format(@"
select  Selected = 0
        , id = ''
        , orderid = ard.OrderID
        , OrderQty = o.qty 
        , IssueQty.IssueQty 
        , [PoQty] = ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = sum(isnull(oa.Qty, 0)) / count(1)
        , ard.Stitch
        , ard.PatternDesc
        , ard.QtyGarment
        , Cost = sum(isnull(oa.Cost, 0)) / count(1)
        , unitprice = sum(isnull(oa.Price, 0)) / count(1)
        , price = sum(isnull(oa.Price, 0)) / count(1)
        , amount = ard.ReqQty *  sum(isnull(oa.Price, 0)) / count(1)
        , Style = o.StyleID
        , [ArtworkReqID] = ar.ID
        , [Article] = oat.Article
        , o.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
		,[Farmout] = ISNULL(FarmOut.Value,0)
		,[FarmIn] = ISNULL(FarmIn.Value,0)
		, COUNT(1),sum(isnull(oa.Price, 0))
from  orders o WITH (NOLOCK)
inner join Order_Article oat with (nolock) on o.ID = oat.ID
inner join ArtworkReq ar WITH (NOLOCK) on ar.Status = 'Approved'
inner join ArtworkReq_Detail ard with (nolock) on   ard.ID = ar.ID  and
                                                    ard.OrderId = o.ID and 
                                                    ard.ArtworkPOID = ''
inner join dbo.Order_Artwork oa WITH (NOLOCK) on oa.ID = o.ID and 
                                                 oa.ArtworkTypeID = ar.ArtworkTypeID and
                                                 oa.ArtworkID = ard.ArtworkID      and
                                                 oa.PatternCode = ard.PatternCode  and
                                                 oa.PatternDesc = ard.PatternDesc  
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= ard.PatternCode
) IssueQty
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=o.ID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc = ard.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=o.ID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc = ard.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
where f.IsProduceFty=1
and o.category  in ('B','S')
and o.MDivisionID='{0}' and ar.ArtworkTypeID = '{1}' and ar.LocalSuppId = '{2}' and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1)
and ((o.Category = 'B' and  oa.price > 0) or (o.category !='B'))
", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"], dr_artworkpo["localsuppid"]);

            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }
            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                strSQLCmd += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            strSQLCmd += @"
group by  ard.OrderID
        , o.qty 
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
        , o.StyleID
        , ar.ID
        , oat.Article
        , o.Category
        , sr.ID 
		, sr.Reason
		, FarmOut.Value
		, FarmIn.Value
";

            return strSQLCmd;
        }

        private DualResult GetSpecialRecordData()
        {
            string sqlWhere = string.Empty;
            if (!(dateSCIDelivery.Value1 == null)) { sqlWhere += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { sqlWhere += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { sqlWhere += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { sqlWhere += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { sqlWhere += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }
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
where  ar.ArtworkTypeID = '{dr_artworkpo["artworktypeid"]}' and ar.Status = 'Approved' and ar.LocalSuppId = '{dr_artworkpo["LocalSuppId"]}' and  ard.ArtworkPOID = '' and
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


            if (orderCategory != "S" && isNeedPlanningB03Quote)
            {
                this.gridBatchImport.Rows[index].Cells["unitprice"].ReadOnly = true;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Black;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.White; //Unit Price

                decimal unitPrice = (decimal)this.gridBatchImport.GetDataRow(index)["unitprice"];
                if (unitPrice == 0)
                {
                    this.gridBatchImport.Rows[index].Cells["Selected"].ReadOnly = true;
                    this.gridBatchImport.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(229, 108, 126);
                    this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.FromArgb(229, 108, 126); //Unit Price
                }
            }
            else
            {
                this.gridBatchImport.Rows[index].Cells["unitprice"].ReadOnly = false;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Red;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.Pink; //Unit Price
            }
        }
    }
}
