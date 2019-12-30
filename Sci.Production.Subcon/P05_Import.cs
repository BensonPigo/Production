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
    public partial class P05_Import : Sci.Win.Subs.Base
    {
        DataRow dr_artworkReq;
        DataTable dt_artworkReqDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtArtwork;
        string sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, sp_b, sp_e;

        public P05_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_artworkReq = master;
            dt_artworkReqDetail = detail;
            this.Text += string.Format(" : {0}", dr_artworkReq["artworktypeid"].ToString());
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

            string strSQLCmd = string.Empty;
            string tmpTable = string.Empty;
            string tmpcurrentReq = string.Empty;
            string tmpGroupby = string.Empty;
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
        , sao.LocalSuppId
		, [orderID] = oa.ID
        , OrderQty = sum(oa.qty)  
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) {tmpcurrentReq}
        , ReqQty = iif(sum(oa.qty)-(ReqQty.value + PoQty.value {tmpcurrentReq}) < 0, 0, sum(oa.qty)- (ReqQty.value + PoQty.value {tmpcurrentReq}))
		, o.SewInLIne
		, o.SciDelivery
		, [ArtworkID] = case when oa.ArtworkID is null then '{dr_artworkReq["artworktypeid"]}' 
                        else oa.ArtworkID end 
		, stitch = 1
		, PatternCode = isnull(oa.PatternCode,'')
		, PatternDesc = isnull(oa.PatternDesc,'')
		, [qtygarment] = 1
        , o.StyleID
		, o.POID
        , id = ''
        , ExceedQty = 0
from  Order_TmsCost ot
inner join orders o WITH (NOLOCK) on ot.ID = o.ID
cross apply(
	select * 
	from (		
		select a.id,a.ArtworkTypeID,q.Article,q.Qty,q.SizeCode,a.PatternCode,a.PatternDesc,a.ArtworkID,a.ArtworkName
		,rowNo = ROW_NUMBER() over (
			partition by a.id,a.ArtworkTypeID,q.Article,a.PatternCode,a.PatternDesc
				,a.ArtworkID,q.sizecode order by a.AddDate desc)
		from Order_Artwork a WITH (NOLOCK)
		inner join order_qty q WITH (NOLOCK) on q.id = a.ID and a.Article = q.Article
		where a.id = o.id
		and a.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
		) s
	where rowNo = 1 
)oa
outer apply(
	select * 
	from (		
		select *
		,rowNo = ROW_NUMBER() over (
			partition by a.StyleUkey,a.ArtworkTypeID,a.Article,a.PatternCode,a.PatternDesc
				,a.ArtworkID,a.ArtworkName order by a.StyleArtworkUkey desc)
		from View_Style_Artwork a WITH (NOLOCK)
		where a.StyleUkey = o.StyleUkey
		and a.Article = oa.Article and a.ArtworkID = oa.ArtworkID 
		and a.ArtworkName = oa.ArtworkName and a.ArtworkTypeID = oa.ArtworkTypeID 
		and a.PatternCode = oa.PatternCode and a.PatternDesc = oa.PatternDesc 
		) s
	where rowNo = 1 
)vsa
left join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.PriceApv = 'Y' and sao.Price > 0
and sao.LocalSuppId = '{dr_artworkReq["localsuppid"]}' 
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
		and OrderID = o.ID 
        and ad.PatternCode= isnull(oa.PatternCode,'')
        and ad.PatternDesc = isnull(oa.PatternDesc,'') 
        and ad.ArtworkID = iif(oa.ArtworkID is null,'{dr_artworkReq["artworktypeid"]}' ,oa.ArtworkID)
        and a.id != '{dr_artworkReq["id"]}'
        and a.status != 'Closed' and ad.ArtworkPOID =''
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = '{dr_artworkReq["artworktypeid"]}'
		and OrderID = o.ID and ad.PatternCode= isnull(oa.PatternCode,'')
        and ad.PatternDesc = isnull(oa.PatternDesc,'') 
        and ad.ArtworkID = iif(oa.ArtworkID is null,'{dr_artworkReq["artworktypeid"]}' ,oa.ArtworkID)
		and ad.ArtworkReqID=''
) PoQty
{tmpTable}
where f.IsProduceFty=1
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and ot.ArtworkTypeID like '{dr_artworkReq["artworktypeid"]}%' 
and o.Junk=0
and ((o.Category = 'B' and  ot.InhouseOSP = 'O') or (o.category = 'S'))
";

            if (!(dateSCIDelivery.Value1 == null))
            {
                strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b);
            }
            if (!(dateSCIDelivery.Value2 == null))
            {
                strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e);
            }
            if (!(dateInlineDate.Value1 == null))
            {
                strSQLCmd += string.Format(" and o.SewInLIne >= '{0}' ", Inline_b);
            }
            if (!(dateInlineDate.Value2 == null))
            {
                strSQLCmd += string.Format(" and o.SewInLIne <= '{0}' ", Inline_e);
            }
            if (!(string.IsNullOrWhiteSpace(sp_b)))
            {
                strSQLCmd += string.Format(" and o.ID >= '{0}'", sp_b);
            }
            if (!(string.IsNullOrWhiteSpace(sp_e)))
            {
                strSQLCmd += string.Format(" and o.ID <= '{0}'",  sp_e);
            }

            strSQLCmd += $@" 
            group by oa.id,sao.LocalSuppID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,o.SewInLIne,o.SciDelivery
            ,oa.PatternDesc, o.StyleID, o.StyleID, o.POID,ot.qty,PoQty.value,ot.ArtworkTypeID,ReqQty.value  {tmpGroupby}";

            Ict.DualResult result;
            //if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
            if(result = MyUtility.Tool.ProcessWithDatatable(dt_artworkReqDetail,"", strSQLCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtArtwork;
            }
            else { ShowErr(strSQLCmd, result); }
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
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
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
    }
}
