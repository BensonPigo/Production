using System;
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
using Sci.Win.UI;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P05_Import
    /// </summary>
    public partial class P05_Import : Sci.Win.Subs.Base
    {
        private DataRow dr_artworkReq;
        private DataTable dt_artworkReqDetail;
        private DataTable dtArtwork;
        private string sciDelivery_b;
        private string sciDelivery_e;
        private string Inline_b;
        private string Inline_e;
        private string sp_b;
        private string sp_e;
        private string MasterSP;
        private bool isNeedPlanningB03Quote = false;
        private bool IsSintexSubcon = false;
        private P05 p05;

        /// <summary>
        /// P05_Import
        /// </summary>
        /// <param name="master">master</param>
        /// <param name="detail">detail</param>
        public P05_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_artworkReq = master;
            this.dt_artworkReqDetail = detail;
            this.Text += string.Format(" : {0}", this.dr_artworkReq["artworktypeid"].ToString());
            this.labelNoSuppHint.Text = $"No assign supplier({this.dr_artworkReq["LocalSuppID"]}). Please check with planning team";
            this.checkBoxAssignedSupp.Text = $"Already assigned supplier({this.dr_artworkReq["LocalSuppID"]}) by planning team";
            this.isNeedPlanningB03Quote = Prgs.CheckNeedPlanningP03Quote(master["artworktypeid"].ToString());
            this.IsSintexSubcon = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select IsSintexSubcon from LocalSupp with (nolock) where ID = '{master["localsuppid"]}'"));
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;
            this.Inline_b = null;
            this.Inline_e = null;

            if (this.dateSCIDelivery.Value1 != null)
            {
                this.sciDelivery_b = this.dateSCIDelivery.Text1;
            }

            if (this.dateSCIDelivery.Value2 != null)
            {
                this.sciDelivery_e = this.dateSCIDelivery.Text2;
            }

            if (this.dateInlineDate.Value1 != null)
            {
                this.Inline_b = this.dateInlineDate.Text1;
            }

            if (this.dateInlineDate.Value2 != null)
            {
                this.Inline_e = this.dateInlineDate.Text2;
            }

            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;
            this.MasterSP = this.txtMasterSP.Text;

            if (this.sciDelivery_b == null && this.sciDelivery_e == null &&
                this.Inline_b == null && this.Inline_e == null &&
                string.IsNullOrWhiteSpace(this.sp_b) && string.IsNullOrWhiteSpace(this.sp_e) &&
                MyUtility.Check.Empty(this.MasterSP))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Inline Date > or < SP# > or <Master SP#> can't be empty!!");
                this.txtSPNoStart.Focus();
                return;
            }

            string sqlwhere = string.Empty;

            if (!(this.dateSCIDelivery.Value1 == null))
            {
                sqlwhere += string.Format(" and o.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!(this.dateSCIDelivery.Value2 == null))
            {
                sqlwhere += string.Format(" and o.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!(this.dateInlineDate.Value1 == null))
            {
                sqlwhere += string.Format(" and o.SewInLIne >= '{0}' ", this.Inline_b);
            }

            if (!(this.dateInlineDate.Value2 == null))
            {
                sqlwhere += string.Format(" and o.SewInLIne <= '{0}' ", this.Inline_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                sqlwhere += string.Format(" and o.ID >= '{0}'", this.sp_b);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_e))
            {
                sqlwhere += string.Format(" and o.ID <= '{0}'", this.sp_e);
            }

            if (!MyUtility.Check.Empty(this.MasterSP))
            {
                sqlwhere += string.Format(" and o.poid = '{0}'", this.MasterSP);
            }

            string strSQLCmd = string.Empty;
            if (this.isNeedPlanningB03Quote)
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

            strSQLCmd = this.FinalGetDataSql(strSQLCmd);

            Ict.DualResult result;
            if (result = MyUtility.Tool.ProcessWithDatatable(this.dt_artworkReqDetail, string.Empty, strSQLCmd, out this.dtArtwork))
            {
                if (this.dtArtwork.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.FilterResult();

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

        private void CheckBoxReqQtyHasValue_CheckedChanged(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = this.FilterResult();
            this.gridBatchImport.ClearSelection();
            foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
            {
                this.DetalGridCellEditChange(dr.Index);
            }
        }

        private void GridBatchImport_Sorted(object sender, EventArgs e)
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

            if (this.chkArticle.Checked && this.chkSize.Checked)
            {
                filterResult = filterResult.Where(s => !MyUtility.Check.Empty(s["Article"]) && !MyUtility.Check.Empty(s["SizeCode"]));
            }
            else if (this.chkArticle.Checked)
            {
                filterResult = filterResult.Where(s => !MyUtility.Check.Empty(s["Article"]) && MyUtility.Check.Empty(s["SizeCode"]));
            }
            else if (this.chkSize.Checked)
            {
                filterResult = filterResult.Where(s => MyUtility.Check.Empty(s["Article"]) && !MyUtility.Check.Empty(s["SizeCode"]));
            }
            else
            {
                filterResult = filterResult.Where(s => MyUtility.Check.Empty(s["Article"]) && MyUtility.Check.Empty(s["SizeCode"]));
            }

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

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.p05 = (P05)this.ParentIForm;
            Ict.Win.DataGridViewGeneratorNumericColumnSettings col_ReqQty = new DataGridViewGeneratorNumericColumnSettings();

            col_ReqQty.CellMouseDoubleClick += (s, e) =>
             {
                 DataRow dr = this.gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                 if (dr == null)
                 {
                     return;
                 }

                 var frm = new Sci.Production.Subcon.P05_ReqQtyList(dr);
                 frm.ShowDialog();
             };

            string caption_stitch = "PCS/Stitch";
            if (this.dr_artworkReq != null)
            {
                caption_stitch = MyUtility.GetValue.Lookup($@"
select Caption = iif(artworkunit='','PCS',artworkunit) 
from artworktype WITH (NOLOCK)
where id ='{this.dr_artworkReq["artworktypeid"]}'
");
            }

            DataGridViewGeneratorNumericColumnSettings tsReqQty = new DataGridViewGeneratorNumericColumnSettings();

            tsReqQty.CellMouseDoubleClick += (s, e) =>
            {
                DataTable dtMsg = ((DataTable)this.listControlBindingSource1.DataSource).Clone();
                dtMsg.ImportRow(this.gridBatchImport.GetDataRow(e.RowIndex));
                MsgGridForm msgGridForm = new MsgGridForm(dtMsg, "Buy Back Qty", "Buy Back Qty", "orderID,OrderQty,BuyBackArtworkReq");
                msgGridForm.grid1.Columns[0].HeaderText = "SP";
                msgGridForm.grid1.Columns[1].HeaderText = "Order\r\nQty";
                msgGridForm.grid1.Columns[2].HeaderText = "Buy Back\r\nQty";
                msgGridForm.grid1.AutoResizeColumns();
                msgGridForm.grid1.Columns[0].Width = 120;
                msgGridForm.ShowDialog();
            };

            this.gridBatchImport.Font = new Font("Arial", 9);
            this.gridBatchImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0) // 0
                .Text("orderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("AccReqQty", header: "Accu. Req. Qty", iseditingreadonly: true, settings: col_ReqQty)
                .Numeric("ReqQty", header: "Req. Qty", iseditable: true, settings: tsReqQty)
                .Date("sewinline", header: "Sew. Inline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ArtworkID", header: "Artwork", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 5
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("stitch", header: caption_stitch, iseditingreadonly: true)
                .Text("PatternCode", header: "Cut. Part", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cut. Part Name", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, iseditingreadonly: true)
                ;
            this.gridBatchImport.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;
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
                    // DataRow[] findrow = dt_artworkReqDetail.Select($@"orderid = '{tmp["orderID"].ToString()}' and ArtworkId = '{tmp["ArtworkId"].ToString()}' and patterncode = '{tmp["patterncode"].ToString()}' and PatternDesc = '{tmp["PatternDesc"].ToString()}'");
                    DataRow[] findrow = this.dt_artworkReqDetail.Select($@"orderid = '{tmp["orderID"]}' and Article = '{tmp["Article"]}' and SizeCode = '{tmp["SizeCode"]}' and ArtworkId = '{tmp["ArtworkId"]}' and patterncode = '{tmp["patterncode"]}' and PatternDesc = '{tmp["PatternDesc"]}'");
                    decimal exceedQty = MyUtility.Convert.GetDecimal(tmp["AccReqQty"]) + MyUtility.Convert.GetDecimal(tmp["ReqQty"]) - MyUtility.Convert.GetDecimal(tmp["OrderQty"]);

                    decimal finalExceedQty = exceedQty < 0 ? 0 : exceedQty;

                    if (findrow.Length > 0)
                    {
                        findrow[0]["ExceedQty"] = finalExceedQty; // (exceedQty - (decimal)findrow[0]["ReqQty"]) < 0 ? 0 : (exceedQty - (decimal)findrow[0]["ReqQty"]);
                        findrow[0]["ReqQty"] = tmp["ReqQty"];
                        findrow[0]["qtygarment"] = 1;
                    }
                    else
                    {
                        tmp["id"] = this.dr_artworkReq["id"];
                        tmp["ExceedQty"] = exceedQty < 0 ? 0 : exceedQty;
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_artworkReqDetail.ImportRow(tmp);
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

            strSQLCmd = $@"
--使用distinct是因為 Order_Artwork若同時有正常Article與----資料時，會抓出重複資料
select  distinct
        [LocalSuppId] = '{this.dr_artworkReq["LocalSuppId"]}'
		, [orderID] = o.ID
        , oq.Article
        , oq.SizeCode
        , OrderQty = oq.Qty
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
into #baseArtworkReq
from  orders o WITH (NOLOCK)
inner join Order_Qty oq with (nolock) on o.ID = oq.ID
inner join dbo.Order_Artwork oa on  oa.ID = o.ID and 
                                    (oq.Article = oa.Article or oa.Article = '----') and 
                                    oa.ArtworkTypeID = '{this.dr_artworkReq["artworktypeid"]}'
left join Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
where f.IsProduceFty=1
and o.category in ('B','S')
and ot.ArtworkTypeID = '{this.dr_artworkReq["artworktypeid"]}'
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1 or o.KeepPanels=1) 
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

            strSQLCmd = $@"
--因為Planning B03(Style_Artwork)會有同一個Artwork多筆報價關係，所以這邊distinct避免資料發散
select  distinct
        [LocalSuppId] = iif(o.category = 'S', '{this.dr_artworkReq["LocalSuppId"]}', isnull(sao.LocalSuppId, ''))
		, [orderID] = o.ID
        , oq.Article
        , oq.SizeCode
        , OrderQty = oq.Qty
		, o.SewInLIne
		, o.SciDelivery
		, [ArtworkID] = oa.ArtworkID
		, [Stitch] = iif(isnull(vsa.ActStitch,0) > 0, vsa.ActStitch, 1)
		, PatternCode = isnull(oa.PatternCode,'') 
		, PatternDesc = isnull(oa.PatternDesc,'')
		, [qtygarment] = 1
        , o.StyleID
		, o.POID
        , id = ''
        , ExceedQty = 0
into #baseArtworkReq
from  orders o WITH (NOLOCK) 
inner join Order_Qty oq with (nolock) on o.ID = oq.ID
left join dbo.Order_Artwork oa on oa.ID = o.ID and oa.ArtworkTypeID = '{this.dr_artworkReq["artworktypeid"]}'
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and 
                                        vsa.Article = oq.Article and 
                                        vsa.ArtworkID = oa.ArtworkID and
										vsa.ArtworkName = oa.ArtworkName and 
                                        vsa.ArtworkTypeID = oa.ArtworkTypeID and 
                                        vsa.PatternCode = oa.PatternCode and
										vsa.PatternDesc = oa.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on   sao.Ukey = vsa.StyleArtworkUkey and 
                                                    sao.LocalSuppId = '{this.dr_artworkReq["LocalSuppId"]}' and
                                                    (sao.SizeCode = oq.SizeCode or sao.SizeCode = '') 
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
where f.IsProduceFty=1
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1 or o.KeepPanels=1)
{sqlWhere}
";

            return strSQLCmd;
        }

        private string QuoteFromTmsCost(string sqlWhere)
        {
            string strSQLCmd;
            string tmpTable = string.Empty;
            string tmpcurrentReq = string.Empty;

            strSQLCmd = $@"
select  [LocalSuppId] = '{this.dr_artworkReq["LocalSuppId"]}'
		, [orderID] = o.ID
        , oq.Article
        , oq.SizeCode
        , OrderQty = oq.Qty
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
into #baseArtworkReq
from  Order_TmsCost ot
inner join orders o WITH (NOLOCK) on ot.ID = o.ID
inner join Order_Qty oq with (nolock) on o.ID = oq.ID
inner join ArtworkType at WITH (NOLOCK) on at.id = '{this.dr_artworkReq["artworktypeid"]}'
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
where f.IsProduceFty = 1
and ot.ArtworkTypeID = '{this.dr_artworkReq["artworktypeid"]}'
and ot.Price > 0
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1 or o.KeepPanels=1)
and ((o.Category = 'B' and  ot.InhouseOSP = 'O') or (o.category = 'S'))
{sqlWhere}
";

            return strSQLCmd;
        }

        private string FinalGetDataSql(string sql)
        {
            string strSQLCmd = sql + $@"
select  * into #FinalArtworkReq
from (
select  LocalSuppId
		,orderID
        ,Article
        ,SizeCode
        ,OrderQty
		,SewInLIne
		,SciDelivery
		,ArtworkID
		,stitch
		,PatternCode
		,PatternDesc
		,qtygarment
        ,StyleID
		,POID
        ,id
        ,ExceedQty
from #baseArtworkReq
union all
select  [LocalSuppId] = isnull(LocalSuppId.val,'')
		,orderID
        ,Article
        ,[SizeCode] = ''
        ,[OrderQty] = OrderQty.val
		,SewInLIne
		,SciDelivery
		,ArtworkID
		,stitch
		,PatternCode
		,PatternDesc
		,qtygarment
        ,StyleID
		,POID
        ,id
        ,ExceedQty
from #baseArtworkReq t
outer apply (select top 1 val = LocalSuppId
			 from #baseArtworkReq t1 
			 where  t1.orderID = t.orderID and
					t1.Article = t.Article and
					t1.PatternCode = t.PatternCode and
					t1.PatternDesc = t.PatternDesc and
					t1.ArtworkID = t.ArtworkID and
					t1.LocalSuppId <> ''
			 ) LocalSuppId
outer apply(select val = isnull(sum(oq.Qty),0)
            from Order_Qty oq with (nolock)
            where   oq.ID = t.orderID and
                    oq.Article = t.Article
            )   OrderQty
group by     LocalSuppId.val
		    ,orderID
            ,Article
		    ,SewInLIne
		    ,SciDelivery
		    ,ArtworkID
		    ,stitch
		    ,PatternCode
		    ,PatternDesc
		    ,qtygarment
            ,StyleID
		    ,POID
            ,id
            ,ExceedQty
            ,OrderQty.val
union all
select  [LocalSuppId] = isnull(LocalSuppId.val,'')
		,orderID
        ,[Article] = ''
        ,SizeCode
        ,[OrderQty] = OrderQty.val
		,SewInLIne
		,SciDelivery
		,ArtworkID
		,stitch
		,PatternCode
		,PatternDesc
		,qtygarment
        ,StyleID
		,POID
        ,id
        ,ExceedQty
from #baseArtworkReq t
outer apply (select top 1 val = LocalSuppId
			 from #baseArtworkReq t1 
			 where  t1.orderID = t.orderID and
					t1.SizeCode = t.SizeCode and
					t1.PatternCode = t.PatternCode and
					t1.PatternDesc = t.PatternDesc and
					t1.ArtworkID = t.ArtworkID and
					t1.LocalSuppId <> ''
			 ) LocalSuppId
outer apply(select val = isnull(sum(oq.Qty),0)
            from Order_Qty oq with (nolock)
            where   oq.ID = t.orderID and
                    oq.SizeCode = t.SizeCode
            )   OrderQty
group by     LocalSuppId.val
		    ,orderID
            ,SizeCode
		    ,SewInLIne
		    ,SciDelivery
		    ,ArtworkID
		    ,stitch
		    ,PatternCode
		    ,PatternDesc
		    ,qtygarment
            ,StyleID
		    ,POID
            ,id
            ,ExceedQty
            ,OrderQty.val
union all
select  [LocalSuppId] = isnull(LocalSuppId.val,'')
		,orderID
        ,[Article] = ''
        ,[SizeCode] = ''
        ,[OrderQty] = OrderQty.val
		,SewInLIne
		,SciDelivery
		,ArtworkID
		,stitch
		,PatternCode
		,PatternDesc
		,qtygarment
        ,StyleID
		,POID
        ,id
        ,ExceedQty
from #baseArtworkReq t
outer apply (select top 1 val = LocalSuppId
			 from #baseArtworkReq t1 
			 where  t1.orderID = t.orderID and
					t1.PatternCode = t.PatternCode and
					t1.PatternDesc = t.PatternDesc and
					t1.ArtworkID = t.ArtworkID and
					t1.LocalSuppId <> ''
			 ) LocalSuppId
outer apply(select val = isnull(sum(oq.Qty),0)
            from Order_Qty oq with (nolock)
            where   oq.ID = t.orderID
            )   OrderQty
group by     LocalSuppId.val
		    ,orderID
		    ,SewInLIne
		    ,SciDelivery
		    ,ArtworkID
		    ,stitch
		    ,PatternCode
		    ,PatternDesc
		    ,qtygarment
            ,StyleID
		    ,POID
            ,id
            ,ExceedQty
            ,OrderQty.val
) a

{this.p05.SqlGetBuyBackDeduction(this.dr_artworkReq["artworktypeid"].ToString())}

select  [Selected] = 0
        ,fr.orderID
		,fr.LocalSuppId
        ,fr.Article
        ,fr.SizeCode
        ,fr.OrderQty
        ,[AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) + isnull(CurrentReq.value,0)
        ,ReqQty = iif(  fr.OrderQty - (ReqQty.value + PoQty.value + isnull(CurrentReq.value,0) + isnull(tbbd.BuyBackArtworkReq, 0)) < 0, 0, 
                        fr.OrderQty - (ReqQty.value + PoQty.value + isnull(CurrentReq.value,0) + isnull(tbbd.BuyBackArtworkReq, 0)))
		,fr.SewInLIne
		,fr.SciDelivery
		,fr.ArtworkID
		,fr.stitch
		,fr.PatternCode
		,fr.PatternDesc
		,fr.qtygarment
        ,fr.StyleID
		,fr.POID
        ,fr.id
        ,fr.ExceedQty
        ,[BuyBackArtworkReq] = isnull(tbbd.BuyBackArtworkReq, 0)
from #FinalArtworkReq fr
left join #tmpBuyBackDeduction tbbd on  tbbd.OrderID = fr.OrderID       and
                                        tbbd.Article = fr.Article       and
                                        tbbd.SizeCode = fr.SizeCode     and
                                        tbbd.PatternCode = fr.PatternCode   and
                                        tbbd.PatternDesc = fr.PatternDesc   and
                                        tbbd.ArtworkID = fr.ArtworkID and
										tbbd.LocalSuppID = fr.LocalSuppID
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD with (nolock)
        inner join ArtworkReq a with (nolock) on ad.ID = a.ID
        where a.ArtworkTypeID = '{this.dr_artworkReq["artworktypeid"]}'
		and ad.OrderID = fr.OrderID
        and ad.Article = fr.Article
        and ad.SizeCode = fr.SizeCode
        and ad.PatternCode = fr.PatternCode
        and ad.PatternDesc = fr.PatternDesc
        and ad.ArtworkID = fr.ArtworkID
        and a.id != '{this.dr_artworkReq["id"]}'
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD with (nolock)
        inner join ArtworkPO A with (nolock) on a.ID = ad.ID
        where a.ArtworkTypeID = '{this.dr_artworkReq["artworktypeid"]}'
		and ad.OrderID = fr.OrderID 
        and ad.Article = fr.Article
        and ad.SizeCode = fr.SizeCode
        and ad.PatternCode = fr.PatternCode
        and ad.PatternDesc = fr.PatternDesc
        and ad.ArtworkID = fr.ArtworkID
		and ad.ArtworkReqID = ''
) PoQty
outer apply(
    select value = sum(ReqQty) 
    from #tmp
    where   OrderID = fr.OrderID and
            Article = fr.Article and
            SizeCode = fr.SizeCode and
            ArtworkID = fr.ArtworkID and
            PatternCode = isnull(fr.PatternCode,'') and
            PatternDesc = isnull(fr.PatternDesc,'')  
) CurrentReq
order by    fr.orderID, fr.Article, fr.SizeCode
";

            return strSQLCmd;
        }
    }
}
