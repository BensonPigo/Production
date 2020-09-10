using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using System.Linq;
using System.Transactions;
using MsExcel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Sci.Production.PublicPrg;
using Sci.Win.UI;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P05_BatchCreate
    /// </summary>
    public partial class P05_BatchCreate : Sci.Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        private string isArtwork;
        private DataTable dtArtwork;
        private string sciDelivery_b;
        private string sciDelivery_e;
        private string Inline_b;
        private string Inline_e;
        private string sp_b;
        private string sp_e;
        private string artworktype;
        private P05 p05;

        /// <summary>
        /// P05_BatchCreate
        /// </summary>
        public P05_BatchCreate()
        {
            this.InitializeComponent();
            this.dateReqDate.Value = DateTime.Today;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.FindData(true);
        }

        private void FindData(bool showNoDataMsg)
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
            this.artworktype = this.txtartworktype_ftyArtworkType.Text;

            if (this.sciDelivery_b == null && this.sciDelivery_e == null &&
                this.Inline_b == null && this.Inline_e == null &&
                string.IsNullOrWhiteSpace(this.sp_b) && string.IsNullOrWhiteSpace(this.sp_e))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Sew. Inline > or < SP# > can't be empty!!");
                this.txtSPNoStart.Focus();
                return;
            }

            string sqlCmd;
            #region 組query sqlcmd
            if (string.IsNullOrWhiteSpace(this.artworktype))
            {
                MyUtility.Msg.WarningBox("< Artwork Type > can't be empty!!");
                this.txtartworktype_ftyArtworkType.Focus();
                return;
            }

            sqlCmd = this.QuoteFromTmsCost();

            #endregion

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, out this.dtArtwork))
            {
                if (this.dtArtwork.Rows.Count == 0 && showNoDataMsg)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.FilterResult();
            }
            else
            {
                this.ShowErr(sqlCmd, result);
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.p05 = (P05)this.ParentIForm;
            #region -- Grid 設定 --
            DataGridViewGeneratorNumericColumnSettings tsReqQty = new DataGridViewGeneratorNumericColumnSettings();

            tsReqQty.CellMouseDoubleClick += (s, e) =>
            {
                DataTable dtMsg = ((DataTable)this.listControlBindingSource1.DataSource).Clone();
                dtMsg.ImportRow(this.gridBatchCreateFromSubProcessData.GetDataRow(e.RowIndex));
                MsgGridForm msgGridForm = new MsgGridForm(dtMsg, "Buy Back Qty", "Buy Back Qty", "orderID,OrderQty,BuyBackArtworkReq");
                msgGridForm.grid1.Columns[0].HeaderText = "SP";
                msgGridForm.grid1.Columns[1].HeaderText = "Order\r\nQty";
                msgGridForm.grid1.Columns[2].HeaderText = "Buy Back\r\nQty";
                msgGridForm.grid1.AutoResizeColumns();
                msgGridForm.grid1.Columns[0].Width = 120;
                msgGridForm.ShowDialog();
            };

            this.gridBatchCreateFromSubProcessData.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchCreateFromSubProcessData.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchCreateFromSubProcessData)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("LocalSuppId", header: "Local Supp", iseditingreadonly: true)
                .Text("orderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("AccReqQty", header: "Accu. Req. Qty", iseditingreadonly: true)
                .Numeric("ReqQty", header: "Req. Qty", iseditable: true, settings: tsReqQty)
                .Date("sewinline", header: "Sew. Inline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ArtworkID", header: "Artwork", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 5
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("stitch", header: "PCS/Stitch", iseditingreadonly: true)
                .Text("PatternCode", header: "Cut. Part", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cut. Part Name", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, iseditingreadonly: true)
                ;
            #endregion
            this.gridBatchCreateFromSubProcessData.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < this.gridBatchCreateFromSubProcessData.Columns.Count; i++)
            {
                this.gridBatchCreateFromSubProcessData.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void CheckBoxReqQtyHasValue_CheckedChanged(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = this.FilterResult();
        }

        // Cancel
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Create
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            string reqdate;
            reqdate = this.dateReqDate.Text;

            if (this.dateReqDate.Value == null)
            {
                MyUtility.Msg.WarningBox("< Req Date > can't be empty!!");
                this.dateReqDate.Focus();
                return;
            }

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] find;
            find = dt.Select("Selected = 1");

            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first", "Warnning");
                return;
            }

            find = dt.Select("Selected = 1 and ReqQty <= 0");
            if (find.Length > 0)
            {
                MyUtility.Msg.WarningBox("<Req. Qty> cannot be zero", "Warnning");
                return;
            }

            #region -- 表頭資料 query --- LINQ
            var query = from row in dt.AsEnumerable()
                         where row.Field<int>("Selected").ToString() == "1"
                         group row by new
                         {
                             t1 = row.Field<string>("ftygroup"),
                             t2 = row.Field<string>("localsuppid"),
                             t3 = row.Field<string>("artworktypeid"),
                         }
                        into m
                         select new
                         {
                             ftygroup = m.Key.t1,
                             localsuppid = m.Key.t2,
                             artworktypeid = m.Key.t3,
                         };
            #endregion

            if (query.ToList().Count > 0)
            {
                ITableSchema tableSchema = null;
                DualResult result;
                result = DBProxy.Current.GetTableSchema(null, "ArtworkReq", out tableSchema);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Get Schema(dbo.ArtworkReq) Faild!!, Please re-try it later!!");
                    return;
                }

                string sqlcmd;

                foreach (var q in query.ToList())
                {
                    TransactionScope transactionscope = new TransactionScope();
                    using (transactionscope)
                    {
                        try
                        {
                            string ftyKeyWord = MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where id='{0}'", q.ftygroup));

                            string id = Sci.MyUtility.GetValue.GetID(ftyKeyWord + "OR", "artworkReq", DateTime.Parse(this.dateReqDate.Text));
                            if (MyUtility.Check.Empty(id))
                            {
                                transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Get Id Faild!!, Please re-try it later!!");
                                return;
                            }

                            #region 表身資料 query2 -- LINQ
                            var query2 = from row in dt.AsEnumerable()
                                         where row.Field<int>("Selected").ToString() == "1"
                                               && row.Field<string>("ftygroup").ToString() == q.ftygroup
                                               && row.Field<string>("localsuppid").ToString() == q.localsuppid
                                group row by new
                                         {
                                             t1 = row.Field<string>("orderid"),
                                             t2 = row.Field<string>("ArtworkTypeID"),
                                             t3 = row.Field<string>("artworkid"),
                                             t4 = row.Field<string>("PatternCode"),
                                             t5 = row.Field<string>("PatternDesc"),
                                             t7 = row.Field<int>("stitch"),
                                             t8 = row.Field<string>("Article"),
                                             t9 = row.Field<string>("SizeCode"),
                                             t10 = row.Field<decimal>("QtyGarment") == 0 ? 1 : row.Field<decimal>("QtyGarment"),
                                         }
                                        into m
                                         select new
                                         {
                                             orderid = m.Key.t1,
                                             ArtworkTypeID = m.Key.t2,
                                             artworkid = m.Key.t3,
                                             PatternCode = m.Key.t4,
                                             PatternDesc = m.Key.t5,
                                             stitch = m.Key.t7,
                                             Article = m.Key.t8,
                                             SizeCode = m.Key.t9,
                                             QtyGarment = m.Key.t10,
                                             ExceedQty =
                                             MyUtility.Convert.GetDecimal(
                                              (
                                              MyUtility.Convert.GetDecimal(dt.Compute(
                                                  "sum(ReqQty)",
                                             $@"OrderID = '{m.Key.t1}' and ArtworkTypeID = '{m.Key.t2}' and artworkid = '{m.Key.t3}'
                                                    and PatternCode = '{m.Key.t4}' and PatternDesc = '{m.Key.t5}' and Selected = 1"))
                                             +
                                             MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<decimal>("AccReqQty")))
                                             - MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<int>("OrderQty")))) < 0 ? 0 :
                                              MyUtility.Convert.GetDecimal(dt.Compute(
                                                  "sum(ReqQty)",
                                             $@"OrderID = '{m.Key.t1}' and ArtworkTypeID = '{m.Key.t2}' and artworkid = '{m.Key.t3}'
                                                    and PatternCode = '{m.Key.t4}' and PatternDesc = '{m.Key.t5}' and Selected = 1"))
                                             +
                                             MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<decimal>("AccReqQty")))
                                             - MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<int>("OrderQty")))),
                                             ReqQty = m.Sum(n => n.Field<decimal>("ReqQty")),
                                         };

                            #endregion

                            // 若是EMB則QTYGarment=1,View_order_artworks.Qty->Coststich&Stich
                            StringBuilder sqlcmd2 = new StringBuilder();
                            if (this.txtartworktype_ftyArtworkType.Text == "EMBROIDERY")
                            {
                                // 明細資料
                                foreach (var q2 in query2.ToList())
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append($@"
                                    INSERT INTO [dbo].[ArtworkReq_Detail]
                                    ([ID]
                                    ,[OrderID]   
                                    ,[Article]    
                                    ,[SizeCode]    
                                    ,[ArtworkId]  
                                    ,[PatternCode]
                                    ,[PatternDesc]
                                    ,[Stitch]       
                                    ,[QtyGarment]   
                                    ,[ReqQty]
                                    ,[ExceedQty]
                                    )
                                    VALUES    
                                    ('{id}'  
                                    ,'{q2.orderid}'  
                                    ,'{q2.Article}'  
                                    ,'{q2.SizeCode}'  
                                    ,'{q2.artworkid}'  
                                    ,'{q2.PatternCode}'  
                                    ,'{q2.PatternDesc}'  
                                    ,'{q2.stitch}'
                                    ,1      
                                    ,{q2.ReqQty}
                                    ,{q2.ExceedQty}
                                     )");
                                    #endregion
                                }
                            }
                            else
                            {
                                // 明細資料
                                foreach (var q2 in query2.ToList())
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append($@"
                                    INSERT INTO [dbo].[ArtworkReq_Detail]
                                    ([ID]
                                    ,[OrderID]    
                                    ,[Article]    
                                    ,[SizeCode]    
                                    ,[ArtworkId]  
                                    ,[PatternCode]
                                    ,[PatternDesc]
                                    ,[Stitch]  
                                    ,[QtyGarment] 
                                    ,[ReqQty]
                                    ,[ExceedQty]
                                    )
                                    VALUES    
                                    ('{id}'  
                                    ,'{q2.orderid}'  
                                    ,'{q2.Article}'  
                                    ,'{q2.SizeCode}'  
                                    ,'{q2.artworkid}'  
                                    ,'{q2.PatternCode}'  
                                    ,'{q2.PatternDesc}'  
                                    ,{q2.stitch}    
                                    ,{q2.QtyGarment}     
                                    ,{q2.ReqQty}
                                    ,{q2.ExceedQty}     
                                    )");
                                    #endregion
                                }
                            }

                            #region 表頭sql
                            int exceed = 0;
                            exceed = query2.Sum(x => x.ExceedQty) > 0 ? 1 : 0;

                            sqlcmd = string.Format($@"INSERT INTO [dbo].[ArtworkReq]
                                                    ([ID]
                                                    ,[MdivisionId]
                                                    ,[FactoryId]      
                                                    ,[LocalSuppID]    
                                                    ,[ReqDate]      
                                                    ,[ArtworkTypeID]  
                                                    ,[Handle]    
                                                    ,[AddName]        
                                                    ,[AddDate]
                                                    ,[Status]
                                                    ,[Exceed])       
                                                    VALUES            
                                                    ('{id}'    
                                                    ,'{Env.User.Keyword}'
                                                    ,'{q.ftygroup}'   
                                                    ,'{q.localsuppid}'
                                                    ,'{reqdate}'
                                                    ,'{q.artworktypeid}'
                                                    ,'{Env.User.UserID}'
                                                    ,'{Env.User.UserID}'
                                                    ,getdate()
                                                    ,'New'
                                                    ,{exceed})");

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
                            {
                                transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }

                            #endregion

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd2.ToString())))
                            {
                                transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }
                            else
                            {
                                transactionscope.Complete();
                                transactionscope.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            transactionscope.Dispose();
                            result = new DualResult(false, "Commit transaction error.", ex);
                            this.ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }

                    MyUtility.Msg.InfoBox("Complete!");
                    transactionscope.Dispose();
                    transactionscope = null;

                    // Import 成功重新Query
                    this.FindData(false);
                }
            }
        }

        // excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(dt);
            sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P05_BatchCreate"));
        }

        private void Txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            this.isArtwork = MyUtility.GetValue.Lookup(
                string.Format(
                "select isartwork from artworktype WITH (NOLOCK) where id = '{0}'",
                ((Sci.Production.Class.Txtartworktype_fty)sender).Text), null);
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

            if (filterResult.Any())
            {
                return filterResult.CopyToDataTable();
            }
            else
            {
                return null;
            }
        }

        private string QuoteFromTmsCost()
        {
            string sqlCmd;
            string sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.Inline_b))
            {
                sqlWhere += string.Format(" and o.SewInLIne >= '{0}' ", this.Inline_b);
            }

            if (!string.IsNullOrWhiteSpace(this.Inline_e))
            {
                sqlWhere += string.Format(" and o.SewInLIne <= '{0}' ", this.Inline_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sciDelivery_b))
            {
                sqlWhere += string.Format("and  o.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!string.IsNullOrWhiteSpace(this.sciDelivery_e))
            {
                sqlWhere += string.Format("and  o.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                sqlWhere += string.Format(" and o.ID >= '{0}'", this.sp_b);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_e))
            {
                sqlWhere += string.Format(" and o.ID <= '{0}'", this.sp_e);
            }

            sqlCmd = $@"
select Selected = 0
        , LocalSuppID = isnull(rtrim(sao.LocalSuppId),'')
        , o.FTYGroup
		, [orderID] = o.ID
        , oa.Article
        , oa.SizeCode
        , OrderQty = oa.poqty
		, o.SewInLIne
		, o.SciDelivery
		, [ArtworkID] = oa.ArtworkID
		, stitch = iif(isnull(vsa.ActStitch,0) > 0, vsa.ActStitch, 1)
		, [PatternCode] = isnull(oa.PatternCode,'')
		, [PatternDesc] = isnull(oa.PatternDesc,'')
		, [qtygarment] = CONVERT(decimal, 1)
        , o.StyleID
		, o.POID
        , [ExceedQty] = 0
        , [ArtworkTypeID] = oa.ArtworkTypeID
into #baseArtworkReq
from  orders o WITH (NOLOCK) 
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID 
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID and
														vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID and vsa.PatternCode = oa.PatternCode and
														vsa.PatternDesc = oa.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on   sao.Ukey = vsa.StyleArtworkUkey and 
                                                    sao.PriceApv = 'Y' and 
                                                    sao.Price > 0 and
                                                    sao.SizeCode = ''
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
where f.IsProduceFty=1
and oa.ArtworkTypeID = '{this.txtartworktype_ftyArtworkType.Text}'
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1 or o.KeepPanels=1)
and sao.LocalSuppId is not null
{sqlWhere}

select  * into #FinalArtworkReq
from (
select  LocalSuppID
        , FTYGroup
		, orderID
        , Article
        , SizeCode
        , OrderQty
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
from #baseArtworkReq
union all
select    LocalSuppID
        , FTYGroup
		, orderID
        , [Article] = ''
        , SizeCode
        , [OrderQty] = OrderQty.val
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
from #baseArtworkReq t
outer apply(select val = isnull(sum(oq.Qty),0)
            from Order_Qty oq with (nolock)
            where   oq.ID = t.orderID and
                    oq.Article = t.Article
            )   OrderQty
group by  LocalSuppID
        , FTYGroup
		, orderID
        , SizeCode
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
        , OrderQty.val
union all
select    LocalSuppID
        , FTYGroup
		, orderID
        , Article
        , [SizeCode] = ''
        , [OrderQty] = OrderQty.val
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
from #baseArtworkReq t
outer apply(select val = isnull(sum(oq.Qty),0)
            from Order_Qty oq with (nolock)
            where   oq.ID = t.orderID and
                    oq.SizeCode = t.SizeCode
            )   OrderQty
group by  LocalSuppID
        , FTYGroup
		, orderID
        , Article
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
        , OrderQty.val
union all
select    LocalSuppID
        , FTYGroup
		, orderID
        , [Article] = ''
        , [SizeCode] = ''
        , [OrderQty] = OrderQty.val
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
from #baseArtworkReq t
outer apply(select val = isnull(sum(oq.Qty),0)
            from Order_Qty oq with (nolock)
            where   oq.ID = t.orderID
            )   OrderQty
group by  LocalSuppID
        , FTYGroup
		, orderID
		, SewInLIne
		, SciDelivery
		, ArtworkID
		, stitch
		, PatternCode
		, PatternDesc
		, qtygarment
        , StyleID
		, POID
        , ExceedQty
        , ArtworkTypeID
        , OrderQty.val

) a

{this.p05.SqlGetBuyBackDeduction(this.txtartworktype_ftyArtworkType.Text)}

select  [Selected] = 0
        , fr.LocalSuppID
        , fr.FTYGroup
		, fr.orderID
        , fr.Article
        , fr.SizeCode
        , fr.OrderQty
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) 
        , ReqQty = iif(fr.OrderQty  - (ReqQty.value + PoQty.value + isnull(tbbd.BuyBackArtworkReq, 0)) < 0, 0, fr.OrderQty  - (ReqQty.value + PoQty.value + isnull(tbbd.BuyBackArtworkReq, 0)))
		, fr.SewInLIne
		, fr.SciDelivery
		, fr.ArtworkID
		, fr.stitch
		, fr.PatternCode
		, fr.PatternDesc
		, fr.qtygarment
        , fr.StyleID
		, fr.POID
        , fr.ExceedQty
        , fr.ArtworkTypeID
        , [BuyBackArtworkReq] = isnull(tbbd.BuyBackArtworkReq, 0)
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
        from ArtworkReq_Detail AD
        inner join ArtworkReq a with (nolock) on ad.ID = a.ID
        where a.ArtworkTypeID = fr.ArtworkTypeID
		and ad.OrderID = fr.OrderID
        and ad.Article = fr.Article
        and ad.SizeCode = fr.SizeCode
        and ad.PatternCode = fr.PatternCode
        and ad.PatternDesc = fr.PatternDesc
        and ad.ArtworkID = fr.ArtworkID
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD with (nolock)
        inner join ArtworkPO A with (nolock) on a.ID = ad.ID
        where a.ArtworkTypeID = fr.ArtworkTypeID
		and ad.OrderID = fr.OrderID
        and ad.Article = fr.Article
        and ad.SizeCode = fr.SizeCode
        and ad.PatternCode = fr.PatternCode
        and ad.PatternDesc = fr.PatternDesc
        and ad.ArtworkID = fr.ArtworkID
		and ad.ArtworkReqID = ''
) PoQty

		";

            return sqlCmd;
        }
    }
}
