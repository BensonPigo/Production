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

namespace Sci.Production.Subcon
{
    public partial class P05_BatchCreate : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        string isArtwork;
        protected DataTable dtArtwork;
        string sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, sp_b, sp_e, artworktype;

        public P05_BatchCreate()
        {
            InitializeComponent();
            dateReqDate.Value = DateTime.Today;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            this.FindData(true);
        }

        private void FindData(bool showNoDataMsg)
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
            this.artworktype = txtartworktype_ftyArtworkType.Text;

            if ((sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Sew. Inline > or < SP# > can't be empty!!");
                txtSPNoStart.Focus();
                return;
            }

            string SqlCmd;
            #region 組query sqlcmd
            if (string.IsNullOrWhiteSpace(artworktype))
            {
                MyUtility.Msg.WarningBox("< Artwork Type > can't be empty!!");
                txtartworktype_ftyArtworkType.Focus();
                return;
            }
            SqlCmd = this.QuoteFromTmsCost();

            #endregion

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, SqlCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0 && showNoDataMsg)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = this.FilterResult();
            }
            else
            {
                ShowErr(SqlCmd, result);
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Grid 設定 --
            this.gridBatchCreateFromSubProcessData.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridBatchCreateFromSubProcessData.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchCreateFromSubProcessData)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("LocalSuppId", header: "Local Supp", iseditingreadonly: true)
                .Text("orderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("AccReqQty", header: "Accu. Req. Qty", iseditingreadonly: true)
                .Numeric("ReqQty", header: "Req. Qty", iseditable: true)
                .Date("sewinline", header: "Sew. Inline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Text("ArtworkID", header: "Artwork", iseditingreadonly: true, width: Widths.AnsiChars(13))      //5
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

        private void checkBoxReqQtyHasValue_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterResult();
        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Create
        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            string Reqdate;
            Reqdate = dateReqDate.Text;

            if (dateReqDate.Value == null)
            {
                MyUtility.Msg.WarningBox("< Req Date > can't be empty!!");
                dateReqDate.Focus();
                return;
            }

            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;

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
            var query = (from row in dt.AsEnumerable()
                         where row.Field<int>("Selected").ToString() == "1"
                         group row by new
                         {
                             t1 = row.Field<string>("ftygroup"),
                             t2 = row.Field<string>("localsuppid"),
                             t3 = row.Field<string>("artworktypeid")
                         } into m
                         select new
                         {
                             ftygroup = m.Key.t1,
                             localsuppid = m.Key.t2,
                             artworktypeid = m.Key.t3
                         });
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
                    TransactionScope _transactionscope = new TransactionScope();
                    using (_transactionscope)
                    {
                        try
                        {
                            string ftyKeyWord = MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where id='{0}'", q.ftygroup));

                            string id = Sci.MyUtility.GetValue.GetID(ftyKeyWord + "OR", "artworkReq", DateTime.Parse(dateReqDate.Text));
                            if (MyUtility.Check.Empty(id))
                            {
                                _transactionscope.Dispose();
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
                                             t10 = (row.Field<decimal>("QtyGarment") == 0 ? 1 : row.Field<decimal>("QtyGarment"))
                                             
                                         } into m
                                         select new
                                         {
                                             orderid = m.Key.t1,
                                             ArtworkTypeID = m.Key.t2,
                                             artworkid = m.Key.t3,
                                             PatternCode = m.Key.t4,
                                             PatternDesc = m.Key.t5,
                                             stitch = m.Key.t7,
                                             QtyGarment = m.Key.t10,
                                             ExceedQty =
                                             MyUtility.Convert.GetDecimal(
                                              (
                                              MyUtility.Convert.GetDecimal(dt.Compute("sum(ReqQty)"
                                             , $@"OrderID = '{m.Key.t1}' and ArtworkTypeID = '{m.Key.t2}' and artworkid = '{m.Key.t3}'
                                                    and PatternCode = '{m.Key.t4}' and PatternDesc = '{m.Key.t5}' and Selected = 1"))
                                             +
                                             MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<decimal>("AccReqQty")))
                                             - MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<int>("OrderQty")))
                                             ) < 0 ? 0 :
                                              MyUtility.Convert.GetDecimal(dt.Compute("sum(ReqQty)"
                                             , $@"OrderID = '{m.Key.t1}' and ArtworkTypeID = '{m.Key.t2}' and artworkid = '{m.Key.t3}'
                                                    and PatternCode = '{m.Key.t4}' and PatternDesc = '{m.Key.t5}' and Selected = 1"))
                                             +
                                             MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<decimal>("AccReqQty")))
                                             - MyUtility.Convert.GetDecimal(m.Sum(n => n.Field<int>("OrderQty")))
                                             ),
                                             ReqQty = m.Sum(n => n.Field<decimal>("ReqQty"))
                                            
                                         };

                            #endregion

                            //若是EMB則QTYGarment=1,View_order_artworks.Qty->Coststich&Stich
                            StringBuilder sqlcmd2 = new StringBuilder();
                            if (txtartworktype_ftyArtworkType.Text == "EMBROIDERY")
                            {
                                foreach (var q2 in query2.ToList()) // 明細資料
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append($@"
                                    INSERT INTO [dbo].[ArtworkReq_Detail]
                                    ([ID]
                                    ,[OrderID]    
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
                                foreach (var q2 in query2.ToList()) // 明細資料
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append($@"
                                    INSERT INTO [dbo].[ArtworkReq_Detail]
                                    ([ID]
                                    ,[OrderID]    
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
                            int Exceed = 0;
                            Exceed = (query2.Sum(x => x.ExceedQty)) > 0 ? 1 : 0;

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
                                                    ,'{Reqdate}'
                                                    ,'{q.artworktypeid}'
                                                    ,'{Env.User.UserID}'
                                                    ,'{Env.User.UserID}'
                                                    ,getdate()
                                                    ,'New'
                                                    ,{Exceed})"
                                        );

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }

                            #endregion

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd2.ToString())))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }
                            else
                            {
                                _transactionscope.Complete();
                                _transactionscope.Dispose();
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            _transactionscope.Dispose();
                            result = new DualResult(false, "Commit transaction error.", ex);
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                    MyUtility.Msg.InfoBox("Complete!");
                    _transactionscope.Dispose();
                    _transactionscope = null;
                    // Import 成功重新Query
                    this.FindData(false);

                }
            }
        }

        //excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(dt);
            sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P05_BatchCreate"));
        }

        private void txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            isArtwork = MyUtility.GetValue.Lookup(string.Format("select isartwork from artworktype WITH (NOLOCK) where id = '{0}'"
                , ((Sci.Production.Class.txtartworktype_fty)sender).Text), null);
        }

        private DataTable FilterResult()
        {
            if (this.dtArtwork == null)
            {
                return null;
            }

            if (this.checkBoxReqQtyHasValue.Checked)
            {
                var filterResult = this.dtArtwork.AsEnumerable().Where(s => (decimal)s["ReqQty"] > 0);
                return filterResult.Any() ? filterResult.CopyToDataTable() : null;
            }
            else
            {
                return this.dtArtwork;
            }
        }
        private string QuoteFromTmsCost()
        {
            string SqlCmd;
            string sqlWhere = string.Empty;
            if (!(string.IsNullOrWhiteSpace(Inline_b)))
            {
                sqlWhere += string.Format(" and o.SewInLIne >= '{0}' ", Inline_b);
            }
            if (!(string.IsNullOrWhiteSpace(Inline_e)))
            {
                sqlWhere += string.Format(" and o.SewInLIne <= '{0}' ", Inline_e);
            }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_b)))
            {
                sqlWhere += string.Format("and  o.SciDelivery >= '{0}' ", sciDelivery_b);
            }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_e)))
            {
                sqlWhere += string.Format("and  o.SciDelivery <= '{0}' ", sciDelivery_e);
            }
            if (!(string.IsNullOrWhiteSpace(sp_b)))
            {
                sqlWhere += string.Format(" and o.ID >= '{0}'", sp_b);
            }
            if (!(string.IsNullOrWhiteSpace(sp_e)))
            {
                sqlWhere += string.Format(" and o.ID <= '{0}'", sp_e);
            }

            SqlCmd = $@"
select Selected = 0
        , LocalSuppID = isnull(rtrim(sao.LocalSuppId),'')
        , o.FTYGroup
		, [orderID] = o.ID
        , OrderQty = sum(oa.poqty)  
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0) 
        , ReqQty = iif(sum(oa.poqty)  -(ReqQty.value + PoQty.value) < 0, 0, sum(oa.poqty)  - (ReqQty.value + PoQty.value))
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
from  orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID and
														vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID and vsa.PatternCode = oa.PatternCode and
														vsa.PatternDesc = oa.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on   sao.Ukey = vsa.StyleArtworkUkey and 
                                                    sao.PriceApv = 'Y' and 
                                                    sao.Price > 0 
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
where f.IsProduceFty=1
and oa.ArtworkTypeID = '{this.txtartworktype_ftyArtworkType.Text}'
and o.category in ('B','S')
and o.MDivisionID='{Sci.Env.User.Keyword}' 
and (o.Junk=0 or o.Junk=1 and o.NeedProduction=1)
and sao.LocalSuppId is not null
{sqlWhere}
group by o.ID,sao.LocalSuppID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,o.SewInLIne,o.SciDelivery
            ,oa.PatternDesc, o.StyleID, o.StyleID, o.POID,PoQty.value,ReqQty.value,o.FTYGroup,vsa.ActStitch
		" ;


            return SqlCmd;
        }
    }
}
