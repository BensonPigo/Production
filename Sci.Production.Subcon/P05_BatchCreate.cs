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
        string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, sp_b, sp_e, artworktype;
        bool isNeedPlanningP03Quote = false;

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
            this.apvdate_b = null;
            this.apvdate_e = null;
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;
            this.Inline_b = null;
            this.Inline_e = null;

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }
            if (dateInlineDate.Value1 != null) Inline_b = this.dateInlineDate.Text1;
            if (dateInlineDate.Value2 != null) { Inline_e = this.dateInlineDate.Text2; }


            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;
            this.artworktype = txtartworktype_ftyArtworkType.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                dateApproveDate.Focus1();
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

            this.isNeedPlanningP03Quote = Prgs.CheckNeedPlanningP03Quote(this.txtartworktype_ftyArtworkType.Text);
            
            if (this.isNeedPlanningP03Quote)
            {
                SqlCmd = this.QuoteFromPlanningP03();
            }
            else
            {
                SqlCmd = this.QuoteFromTmsCost();
            }

            #endregion

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, SqlCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0 && showNoDataMsg)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtArtwork;
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
                .Text("FTYGroup", header: "Fty", iseditingreadonly: true)   //1
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))   //2
                .Text("Styleid", header: "Style", iseditingreadonly: true)  //3
                .Text("SeasonID", header: "Season", iseditingreadonly: true)    //4
                .Text("orderTypeId", header: "Order Type", iseditingreadonly: true)    //5
                .Date("SciDelivery", header: "Sci Delivery", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true)
                .Text("PatternCode", header: "Cutpart Id", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)
                .Text("LocalSuppID", header: "Supplier", iseditingreadonly: true)
                .Numeric("ReqQty", header: "Po QTY", iseditingreadonly: true)
                .Date("artworkinline", header: "inline", iseditingreadonly: true)
                .Date("artworkoffline", header: "offline", iseditingreadonly: true)
                .Text("message", header: "Message", iseditingreadonly: true, width: Widths.AnsiChars(30))
                ;
            #endregion
            
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
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
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
                                             t10 = (row.Field<decimal>("QtyGarment") == 0 ? 1 : row.Field<decimal>("QtyGarment")),
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
                                             ReqQty = m.Sum(n => n.Field<int>("ReqQty"))

                                         };

                            #endregion
                            #region 表頭sql
                            sqlcmd = string.Format($@"INSERT INTO [dbo].[ArtworkReq]
                                                    ([ID]
                                                    ,[MdivisionId]
                                                    ,[FactoryId]      
                                                    ,[LocalSuppID]    
                                                    ,[ReqDate]      
                                                    ,[ArtworkTypeID]  
                                                    ,[Handle]         
                                                    ,[AddName]        
                                                    ,[AddDate],[Status])       
                                                    VALUES            
                                                    ('{id}'    
                                                    ,'{Env.User.Keyword}'
                                                    ,'{q.ftygroup}'   
                                                    ,'{q.localsuppid}'
                                                    ,'{Reqdate}'
                                                    ,'{q.artworktypeid}'
                                                    ,'{Env.User.UserID}'
                                                    ,'{Env.User.UserID}'
                                                    ,getdate(),'New')"
                                        );

                            #endregion

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }

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
                                    ,[ReqQty])
                                    VALUES    
                                    ('{id}'  
                                    ,'{q2.orderid}'  
                                    ,'{q2.artworkid}'  
                                    ,'{q2.PatternCode}'  
                                    ,'{q2.PatternDesc}'  
                                    ,'{q2.stitch}'
                                    ,1      
                                    ,{q2.ReqQty})");
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
                                    ,[ReqQty])
                                    VALUES    
                                    ('{id}'  
                                    ,'{q2.orderid}'  
                                    ,'{q2.artworkid}'  
                                    ,'{q2.PatternCode}'  
                                    ,'{q2.PatternDesc}'  
                                    ,{q2.stitch}    
                                    ,{q2.QtyGarment}     
                                    ,{q2.ReqQty})");
                                    #endregion
                                }
                            }

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
                                MyUtility.Msg.WarningBox("Complete!");
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

        private string QuoteFromPlanningP03()
        {
            string SqlCmd;
            SqlCmd = $@"
SELECT 	Selected = 0 
		, orders.FTYGroup
		, orderid = Order_TmsCost.ID
		, article = rtrim(v.article) 
		, Styleid = rtrim(Orders.Styleid) 
		, orders.ordertypeid
		, Orders.SeasonID
		, Orders.SciDelivery
		, Order_TmsCost.ArtworkTypeID
		, ArtworkID = rtrim(v.ArtworkID) 
		, v.PatternCode
		, v.PatternDesc
		, LocalSuppID = rtrim(order_tmscost.LocalSuppID) 
        , [Cost] = isnull(cost.value,0)
		, costStitch = v.qty
		, stitch = v.qty
		, qtygarment = 1.0
		, Reqqty = sum(v.poqty) 
		, Order_TmsCost.ArtworkInLine
		, Order_TmsCost.artworkoffline
		, Order_TmsCost.apvdate 
		, message = '' 
        , IsArtwork = 1
FROM Order_TmsCost WITH (NOLOCK) 
inner join Orders WITH (NOLOCK) on Order_TmsCost.id = Orders.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join ArtworkType awt WITH (NOLOCK) on Order_TmsCost.ArtworkTypeID=awt.ID
inner join view_order_artworks v on v.id = Order_TmsCost.id 
									and v.artworktypeid = Order_TmsCost.artworktypeid
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = orders.StyleUkey and vsa.Article = v.Article and vsa.ArtworkID = v.ArtworkID and
														vsa.ArtworkName = v.ArtworkName and vsa.ArtworkTypeID = v.ArtworkTypeID and vsa.PatternCode = v.PatternCode and
														vsa.PatternDesc = v.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.LocalSuppID = order_tmscost.LocalSuppID  and sao.Price > 0  and sao.PriceApv = 'Y'
left join LocalSupp ls with (nolock) on ls.id = order_tmscost.LocalSuppID
outer apply (select value = iif(ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1), v.Cost,sao.Price))unitprice
outer apply (
    select value = 
        case when ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1) then v.Cost
             when awt.isArtwork = 1 then vsa.Cost
             else sao.Price
             end
)cost
WHERE 	not exists(
			select 1
			from artworkReq a WITH (NOLOCK) 
			inner join artworkReq_detail ap WITH (NOLOCK) on ap.id = a.id 
			where a.localsuppid = Order_TmsCost.localsuppid 
				  and a.artworktypeid = Order_TmsCost.artworktypeid and 
				  ap.OrderID = orders.ID) 
	  	AND orders.Finished=0                                                                 
		AND orders.IsForecast = 0                                                             
		AND orders.Junk = 0 
		AND factory.mdivisionid = '{Sci.Env.User.Keyword}'
		AND factory.IsProduceFty = 1
		AND Order_TmsCost.localsuppid !=''
        --AND Orders.PulloutComplete = 0
        AND (orders.Category ='s' or (orders.Category='B' AND Order_TmsCost.Price > 0) AND Order_TmsCost.InhouseOSP = 'O')
		--↓(ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1)), ISP20190803增加IsSintexSubcon狀況
		--↓或是sao.Ukey is not null, 原本存在 View_Style_Artwork, Style_Artwork_Quot
		and ((ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1)) or sao.Ukey is not null)
		";

            if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkTypeID = '{0}'", artworktype); }
            if (!(string.IsNullOrWhiteSpace(apvdate_b))) { SqlCmd += string.Format(" and Order_TmsCost.ApvDate >= '{0}' ", apvdate_b); }
            if (!(string.IsNullOrWhiteSpace(apvdate_e))) { SqlCmd += string.Format(" and Order_TmsCost.ApvDate <= '{0}' ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(Inline_b))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkInLine <= '{0}' ", Inline_b); }
            if (!(string.IsNullOrWhiteSpace(Inline_e))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkOffLine >= '{0}' ", Inline_e); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_b))) { SqlCmd += string.Format("and  Orders.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_e))) { SqlCmd += string.Format("and  Orders.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", sp_b, sp_e); }
            SqlCmd += @"
group by 	orders.FTYGroup, Order_TmsCost.ID, v.article, Orders.Styleid, Orders.SeasonID
			, Orders.OrderTypeId, Orders.SciDelivery, Order_TmsCost.ArtworkTypeID
			, order_tmscost.LocalSuppID, order_tmscost.Qty, Order_TmsCost.ArtworkInLine
			, Order_TmsCost.artworkoffline, Orders.SewInLine, Order_TmsCost.ApvDate
			, V.ArtworkID, V.PatternCode, V.PatternDesc, cost.value, V.ArtworkTypeID, v.qty,unitprice.value";

            return SqlCmd;
        }

        private string QuoteFromTmsCost()
        {
            string SqlCmd;
            // 建立可以符合回傳的Cursor

            #region -- 非ArtworK類 的sql command --
            SqlCmd = $@"
SELECT 	Selected = 0 
		, Orders.FTYGroup
		, orderid = Order_TmsCost.ID 
		, article = rtrim(v.article) 
		, Styleid = rtrim(Orders.Styleid) 
		, SeasonID = rtrim(Orders.SeasonID) 
		, orders.ordertypeid
		, Orders.SciDelivery
		, Order_TmsCost.ArtworkTypeID
		, ArtworkID = Order_TmsCost.ArtworkTypeID 
		, PatternCode = voa.PatternCode 
		, PatternDesc = voa.PatternDesc 
		, LocalSuppID = rtrim(order_tmscost.LocalSuppID) 
		, costStitch = 1 
		, stitch = 1 
		, qtygarment = IIF(order_tmscost.Qty IS NULL OR order_tmscost.Qty=0 ,1 ,order_tmscost.Qty)--  isnull(order_tmscost.Qty,1)
		, Reqqty = sum(v.Qty) 
		, Order_TmsCost.ArtworkInLine
		, Order_TmsCost.artworkoffline
		, Orders.SewInLine
		, Order_TmsCost.ApvDate
		, message = '' 
        , IsArtwork = 0
FROM Order_TmsCost WITH (NOLOCK) 
inner join Orders WITH (NOLOCK) on orders.id = order_tmscost.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join order_qty v WITH (NOLOCK) on v.id = order_tmscost.id
inner join ArtworkType awt WITH (NOLOCK) on Order_TmsCost.ArtworkTypeID=awt.ID
left join View_Order_Artworks voa WITH (NOLOCK) on voa.id = Order_TmsCost.od
    and voa.artworktypeid = = Order_TmsCost.artworktypeid
WHERE 	not exists(
			select * 
			from artworkReq a WITH (NOLOCK) 
			inner join artworkReq_detail ap WITH (NOLOCK) on ap.id = a.id 
			where a.localsuppid = Order_TmsCost.localsuppid 
				  and a.artworktypeid = Order_TmsCost.artworktypeid 
				  and ap.OrderID = orders.ID ) 
		and factory.mdivisionid = '{Sci.Env.User.Keyword}' 
		and factory.IsProduceFty = 1
		and orders.Finished=0
		and orders.IsForecast = 0
		and orders.Junk = 0
		and Order_TmsCost.localsuppid !=''		
        --and Orders.PulloutComplete = 0
		" ;
  
            if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkTypeID = '{0}'", artworktype); }
            if (!(string.IsNullOrWhiteSpace(apvdate_b))) { SqlCmd += string.Format(" and Order_TmsCost.ApvDate >= '{0}' ", apvdate_b); }
            if (!(string.IsNullOrWhiteSpace(apvdate_e))) { SqlCmd += string.Format(" and Order_TmsCost.ApvDate <= '{0}' ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(Inline_b))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkInLine <= '{0}' ", Inline_b); }
            if (!(string.IsNullOrWhiteSpace(Inline_e))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkOffLine >= '{0}' ", Inline_e); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_b))) { SqlCmd += string.Format("and  Orders.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_e))) { SqlCmd += string.Format("and  Orders.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", sp_b, sp_e); }
            SqlCmd += @" 
group by	orders.FTYGroup, Order_TmsCost.ID, v.article, Orders.Styleid, Orders.SeasonID
			, Orders.OrderTypeId, Orders.SciDelivery, Order_TmsCost.ArtworkTypeID
			, order_tmscost.LocalSuppID, order_tmscost.Qty, Order_TmsCost.ArtworkInLine
			, Order_TmsCost.artworkoffline, Orders.SewInLine, Order_TmsCost.ApvDate
			";
            #endregion

            return SqlCmd;
        }
    }
}
