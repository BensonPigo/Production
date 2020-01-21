﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Subcon
{
    public partial class P05_Import : Sci.Win.Subs.Base
    {
        DataRow dr_artworkReq;
        DataTable dt_artworkReqDetail;
        protected DataTable dtArtwork;
        string sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, sp_b, sp_e;
        bool isNeedPlanningB03Quote = false;
        bool IsSintexSubcon = false;

        public P05_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_artworkReq = master;
            dt_artworkReqDetail = detail;
            this.Text += string.Format(" : {0}", dr_artworkReq["artworktypeid"].ToString());
            this.labelNoSuppHint.Text = $"No assign supplier({dr_artworkReq["LocalSuppID"]}). Please check with planning team";
            this.checkBoxAssignedSupp.Text = $"Already assigned supplier({dr_artworkReq["LocalSuppID"]}) by planning team";
            this.isNeedPlanningB03Quote = Prgs.CheckNeedPlanningP03Quote(master["artworktypeid"].ToString());
            this.IsSintexSubcon = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select IsSintexSubcon from LocalSupp with (nolock) where ID = '{master["localsuppid"]}'"));
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;
            this.Inline_b = null;
            this.Inline_e = null;

            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }
            if (dateInlineDate.Value1 != null) Inline_b = this.dateInlineDate.Text1;
            if (dateInlineDate.Value2 != null) { Inline_e = this.dateInlineDate.Text2; }

            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;

            if ((sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                txtSPNoStart.Focus();
                return;
            }
            
            string sqlwhere = string.Empty;

            if (!(dateSCIDelivery.Value1 == null))
            {
                sqlwhere += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b);
            }
            if (!(dateSCIDelivery.Value2 == null))
            {
                sqlwhere += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e);
            }
            if (!(dateInlineDate.Value1 == null))
            {
                sqlwhere += string.Format(" and o.SewInLIne >= '{0}' ", Inline_b);
            }
            if (!(dateInlineDate.Value2 == null))
            {
                sqlwhere += string.Format(" and o.SewInLIne <= '{0}' ", Inline_e);
            }
            if (!(string.IsNullOrWhiteSpace(sp_b)))
            {
                sqlwhere += string.Format(" and o.ID >= '{0}'", sp_b);
            }
            if (!(string.IsNullOrWhiteSpace(sp_e)))
            {
                sqlwhere += string.Format(" and o.ID <= '{0}'", sp_e);
            }

            string strSQLCmd = string.Empty;
            if (isNeedPlanningB03Quote)
            {
                if (this.IsSintexSubcon && Prgs.CheckIsArtworkorUseArtwork(MyUtility.Convert.GetString(this.dr_artworkReq["artworktypeid"])))
                {
                    strSQLCmd = this.QuoteIsSintexSubcon(sqlwhere);
                }
                else
                {
                    strSQLCmd = this.QuoteFromPlanningB03(sqlwhere);
                }
            }
            else
            {
                strSQLCmd = this.QuoteFromTmsCost(sqlwhere);
            }

            Ict.DualResult result;
            //if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
            if (result = MyUtility.Tool.ProcessWithDatatable(dt_artworkReqDetail, "", strSQLCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = this.FilterResult();

                foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
                {
                    this.DetalGridCellEditChange(dr.Index);
                }
                this.gridBatchImport.ClearSelection();
            }
            else { ShowErr(strSQLCmd, result); }
        }

        private void checkBoxReqQtyHasValue_CheckedChanged(object sender, EventArgs e)
        {
            listControlBindingSource1.DataSource = this.FilterResult();
            this.gridBatchImport.ClearSelection();
            foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
            {
                this.DetalGridCellEditChange(dr.Index);
            }
        }

        private void gridBatchImport_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
            {
                this.DetalGridCellEditChange(dr.Index);
            }
        }

        private DataTable FilterResult()
        {
            if (this.dtArtwork == null)
            {
                return null;
            }

            var filterResult = this.dtArtwork.AsEnumerable();

            if (this.checkBoxReqQtyHasValue.Checked)
            {
                filterResult = filterResult.Where(s => (decimal)s["ReqQty"] > 0);
                
            }

            if (this.checkBoxAssignedSupp.Checked)
            {
                filterResult = filterResult.Where(s => !MyUtility.Check.Empty(s["LocalSuppId"]));
            }

            if (filterResult.Any())
            {
                return filterResult.CopyToDataTable();
            }
            else
            {
                return null;
            }

            
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings col_ReqQty = new DataGridViewGeneratorNumericColumnSettings();

            col_ReqQty.CellMouseDoubleClick += (s, e) =>
             {
                 DataRow dr = this.gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                 if (null == dr) return;
                 var frm = new Sci.Production.Subcon.P05_ReqQtyList(dr);
                 frm.ShowDialog();
             };


            string Caption_stitch = "PCS/Stitch";
            if (dr_artworkReq != null)
            {
                Caption_stitch = MyUtility.GetValue.Lookup($@"
select Caption = iif(artworkunit='','PCS',artworkunit) 
from artworktype WITH (NOLOCK)
where id ='{dr_artworkReq["artworktypeid"]}'
");
            }

            this.gridBatchImport.Font = new Font("Arial", 9);
            this.gridBatchImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridBatchImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)   //0
                .Text("orderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("AccReqQty", header: "Accu. Req. Qty", iseditingreadonly: true, settings: col_ReqQty)
                .Numeric("ReqQty", header: "Req. Qty", iseditable: true)
                .Date("sewinline", header: "Sew. Inline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Text("ArtworkID", header: "Artwork", iseditingreadonly: true, width: Widths.AnsiChars(13))      //5
                .Numeric("stitch", header: Caption_stitch, iseditingreadonly: true)
                .Text("PatternCode", header: "Cut. Part", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cut. Part Name", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, iseditingreadonly: true)
                ;
            this.gridBatchImport.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;
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
            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");

            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("please select data first.", "Warning");
                return;
            }
            else
            {
                foreach (DataRow tmp in dr2)
                {
                    //DataRow[] findrow = dt_artworkReqDetail.Select($@"orderid = '{tmp["orderID"].ToString()}' and ArtworkId = '{tmp["ArtworkId"].ToString()}' and patterncode = '{tmp["patterncode"].ToString()}' and PatternDesc = '{tmp["PatternDesc"].ToString()}'");
                    DataRow[] findrow = dt_artworkReqDetail.Select($@"orderid = '{tmp["orderID"]}' and ArtworkId = '{tmp["ArtworkId"]}' and patterncode = '{tmp["patterncode"]}' and PatternDesc = '{tmp["PatternDesc"]}'");
                    decimal exceedQty = MyUtility.Convert.GetDecimal(tmp["AccReqQty"]) + MyUtility.Convert.GetDecimal(tmp["ReqQty"]) - MyUtility.Convert.GetDecimal(tmp["OrderQty"]);

                    decimal finalExceedQty = exceedQty < 0 ? 0 : exceedQty;

                    if (findrow.Length > 0)
                    {
                        findrow[0]["ExceedQty"] = finalExceedQty;//(exceedQty - (decimal)findrow[0]["ReqQty"]) < 0 ? 0 : (exceedQty - (decimal)findrow[0]["ReqQty"]);
                        findrow[0]["ReqQty"] = tmp["ReqQty"];
                        findrow[0]["qtygarment"] = 1;
                    }
                    else
                    {
                        tmp["id"] = dr_artworkReq["id"];
                        tmp["ExceedQty"] = exceedQty < 0 ? 0 : exceedQty;
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_artworkReqDetail.ImportRow(tmp);
                    }
                }
            }

            this.Close();
        }

        private void DetalGridCellEditChange(int index)
        {
            string localSuppId = (string)this.gridBatchImport.GetDataRow(index)["LocalSuppId"];
            if (MyUtility.Check.Empty(localSuppId))
            {
                this.gridBatchImport.Rows[index].Cells["Selected"].ReadOnly = true;
                this.gridBatchImport.Rows[index].Cells["ReqQty"].ReadOnly = true;
                this.gridBatchImport.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(229, 108, 126);
            }
        }

        private string QuoteIsSintexSubcon(string sqlWhere)
        {
            string strSQLCmd;
            string tmpTable = string.Empty;
            string tmpcurrentReq = string.Empty;

            // 上一層表身有資料, 就加上表身當前ReqQty 
            if (dt_artworkReqDetail.Rows.Count > 0)
            {
                tmpTable = $@"
outer apply(
    select value = sum(ReqQty) 
    from #tmp
    where OrderID = o.id and ArtworkID = iif(oa.ArtworkID is null,'{dr_artworkReq["artworktypeid"]}',oa.ArtworkID)
    and PatternDesc = isnull(oa.PatternDesc,'') and PatternCode = isnull(oa.PatternCode,'')
) CurrentReq";
                tmpcurrentReq = "+ isnull(CurrentReq.value,0)";
            }

            strSQLCmd = $@"
select distinct Selected = 0
        , [LocalSuppId] = isnull(ot.LocalSuppId,'')
		, [orderID] = o.ID
        , OrderQty = o.Qty
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) {tmpcurrentReq}
        , ReqQty = iif(o.Qty-(ReqQty.value + PoQty.value {tmpcurrentReq}) < 0, 0, o.Qty - (ReqQty.value + PoQty.value {tmpcurrentReq}))
		, o.SewInLIne
		, o.SciDelivery
		, [ArtworkID] = oa.ArtworkID
		, stitch = oa.qty 
		, PatternCode = oa.PatternCode
		, PatternDesc = oa.PatternDesc
		, [qtygarment] = 1
        , o.StyleID
		, o.POID
        , id = ''
        , ExceedQty = 0
from  orders o WITH (NOLOCK)
inner join dbo.Order_Artwork oa on oa.ID = o.ID and oa.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
left join Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = oa.ArtworkTypeID
		and OrderID = o.ID 
        and ad.PatternCode = oa.PatternCode
        and ad.PatternDesc = oa.PatternDesc
        and ad.ArtworkID = oa.ArtworkID
        and a.id != '{dr_artworkReq["id"]}'
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = oa.ArtworkTypeID
		and OrderID = o.ID 
        and ad.PatternCode = oa.PatternCode
        and ad.PatternDesc = oa.PatternDesc
        and ad.ArtworkID = oa.ArtworkID
		and ad.ArtworkReqID=''
) PoQty
{tmpTable}
where f.IsProduceFty=1
and o.category in ('B','S')
and ot.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and o.Junk=0
and ((o.Category = 'B' and  ot.InhouseOSP = 'O') or (o.category = 'S'))
{sqlWhere}

";

            return strSQLCmd;
        }
        private string QuoteFromPlanningB03(string sqlWhere)
        {
            string strSQLCmd;
            string tmpTable = string.Empty;
            string tmpGroupby = string.Empty;
            string tmpcurrentReq = string.Empty;

            // 上一層表身有資料, 就加上表身當前ReqQty 
            if (dt_artworkReqDetail.Rows.Count > 0)
            {
                tmpTable = $@"
outer apply(
    select value = sum(ReqQty) 
    from #tmp
    where OrderID = o.id and ArtworkID = iif(oa.ArtworkID is null,'{dr_artworkReq["artworktypeid"]}',oa.ArtworkID)
    and PatternDesc = isnull(oa.PatternDesc,'') and PatternCode = isnull(oa.PatternCode,'')
) CurrentReq";
                tmpcurrentReq = "+ isnull(CurrentReq.value,0)";
                tmpGroupby = ",CurrentReq.value";
            }

            strSQLCmd = $@"
select  Selected = 0
        , [LocalSuppId] = isnull(sao.LocalSuppId,'')
		, [orderID] = o.ID
        , OrderQty = sum(oa.poqty)
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) {tmpcurrentReq}
        , ReqQty = iif(sum(oa.poqty)-(ReqQty.value + PoQty.value {tmpcurrentReq}) < 0, 0, sum(oa.poqty)- (ReqQty.value + PoQty.value {tmpcurrentReq}))
		, o.SewInLIne
		, o.SciDelivery
		, [ArtworkID] = oa.ArtworkID
		, stitch = 1
		, PatternCode = isnull(oa.PatternCode,'') 
		, PatternDesc = isnull(oa.PatternDesc,'')
		, [qtygarment] = 1
        , o.StyleID
		, o.POID
        , id = ''
        , ExceedQty = 0
from  orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID and
														vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID and vsa.PatternCode = oa.PatternCode and
														vsa.PatternDesc = oa.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on   sao.Ukey = vsa.StyleArtworkUkey and 
                                                    sao.LocalSuppId = '{dr_artworkReq["LocalSuppId"]}'
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = oa.ArtworkTypeID
		and OrderID = o.ID 
        and ad.PatternCode = oa.PatternCode
        and ad.PatternDesc = oa.PatternDesc
        and ad.ArtworkID = oa.ArtworkID
        and a.id != '{dr_artworkReq["id"]}'
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = oa.ArtworkTypeID
		and OrderID = o.ID 
        and ad.PatternCode = oa.PatternCode
        and ad.PatternDesc = oa.PatternDesc
        and ad.ArtworkID = oa.ArtworkID
		and ad.ArtworkReqID=''
) PoQty
{tmpTable}
where f.IsProduceFty=1
and oa.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and o.Junk=0
{sqlWhere}
group by o.ID,sao.LocalSuppID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,o.SewInLIne,o.SciDelivery
            ,oa.PatternDesc, o.StyleID, o.StyleID, o.POID,PoQty.value,ReqQty.value  {tmpGroupby}
";

            return strSQLCmd;

        }
        private string QuoteFromTmsCost(string sqlWhere)
        {
            string strSQLCmd;
            string tmpTable = string.Empty;
            string tmpcurrentReq = string.Empty;
            
            // 上一層表身有資料, 就加上表身當前ReqQty 
            if (dt_artworkReqDetail.Rows.Count > 0)
            {
                tmpTable = $@"
outer apply(
    select value = sum(ReqQty) 
    from #tmp
    where OrderID = o.id and ArtworkID = '{dr_artworkReq["artworktypeid"]}'
    and PatternDesc = '' and PatternCode = ''
) CurrentReq";
                tmpcurrentReq = "+ isnull(CurrentReq.value,0)";
            }

            strSQLCmd = $@"
select  Selected = 0
        , [LocalSuppId] = '{dr_artworkReq["LocalSuppId"]}'
		, [orderID] = o.ID
        , OrderQty = o.Qty
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) {tmpcurrentReq}
        , ReqQty = iif(o.Qty-(ReqQty.value + PoQty.value {tmpcurrentReq}) < 0, 0, o.Qty - (ReqQty.value + PoQty.value {tmpcurrentReq}))
		, o.SewInLIne
		, o.SciDelivery
		, [ArtworkID] = at.id
		, stitch = 1
		, PatternCode = ''
		, PatternDesc = ''
		, [qtygarment] = 1
        , o.StyleID
		, o.POID
        , id = ''
        , ExceedQty = 0
from  Order_TmsCost ot
inner join orders o WITH (NOLOCK) on ot.ID = o.ID
inner join ArtworkType at WITH (NOLOCK) on at.id = '{dr_artworkReq["artworktypeid"]}'
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = ot.ArtworkTypeID
		and OrderID = o.ID 
        and ad.PatternCode = ''
        and ad.PatternDesc = ''
        and ad.ArtworkID = at.id
        and a.id != '{dr_artworkReq["id"]}'
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = ot.ArtworkTypeID
		and OrderID = o.ID 
        and ad.PatternCode= ''
        and ad.PatternDesc = ''
        and ad.ArtworkID = at.id
		and ad.ArtworkReqID=''
) PoQty
{tmpTable}
where f.IsProduceFty = 1
and ot.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
and ot.Price > 0
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and o.Junk=0
and ((o.Category = 'B' and  ot.InhouseOSP = 'O') or (o.category = 'S'))
{sqlWhere}

";

            return strSQLCmd;
        }

    }
}
