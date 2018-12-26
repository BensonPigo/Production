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

namespace Sci.Production.Subcon
{
    public partial class P01_BatchCreate : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        string poType, isArtwork;
        protected DataTable dtArtwork;

        public P01_BatchCreate()
        {
            InitializeComponent();
        }

        public P01_BatchCreate(string fuc)
        {
            InitializeComponent();

            if (fuc == "P01")
            {
                poType = "O";
                this.Text += " (Sub-con Purchase Order)";
            }
            else
            {
                poType = "I";
                this.Text += " (In-House Requisition)";
            }

            dateIssueDate.Value = DateTime.Today;
            dateDelivery.Value = DateTime.Today;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, artworktype;
            apvdate_b = null;
            apvdate_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            Inline_b = null;
            Inline_e = null;
            artworktype = null;

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }
            if (dateInlineDate.Value1 != null) Inline_b = this.dateInlineDate.Text1;
            if (dateInlineDate.Value2 != null) { Inline_e = this.dateInlineDate.Text2; }

            String sp_b = this.txtSPNoStart.Text;
            String sp_e = this.txtSPNoEnd.Text;
            artworktype = txtartworktype_ftyArtworkType.Text;

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

            // 建立可以符合回傳的Cursor
            if (isArtwork.ToUpper() == "TRUE")
            {
                #region -- Artwork類 的 sql command --
                SqlCmd = string.Format(@"
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
		, v.Cost
		, costStitch = v.qty
		, stitch = v.qty
		, unitprice = v.Cost
		, qtygarment = order_tmscost.Qty 
		, poqty = sum(v.poqty) 
		, Order_TmsCost.ArtworkInLine
		, Order_TmsCost.artworkoffline
		, Order_TmsCost.apvdate 
		, message = '' 
FROM Order_TmsCost WITH (NOLOCK) 
inner join Orders WITH (NOLOCK) on Order_TmsCost.id = Orders.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join view_order_artworks v on v.id = Order_TmsCost.id 
									and v.artworktypeid = Order_TmsCost.artworktypeid
WHERE 	not exists(
			select * 
			from artworkpo a WITH (NOLOCK) 
			inner join artworkpo_detail ap WITH (NOLOCK) on ap.id = a.id 
			where a.potype = '{0}' 
				  and a.localsuppid = Order_TmsCost.localsuppid 
				  and a.artworktypeid = Order_TmsCost.artworktypeid and 
				  ap.OrderID = orders.ID) 
	  	and orders.Finished=0                                                                 
		AND orders.IsForecast = 0                                                             
		AND orders.Junk = 0 
		and factory.mdivisionid = '{1}'
		and factory.IsProduceFty = 1
		and Order_TmsCost.localsuppid !=''
		and Orders.category  in ('B','S')
		", poType, Sci.Env.User.Keyword);

                SqlCmd += string.Format(" AND Order_TmsCost.InhouseOSP = '{0}'", poType);

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
			, V.ArtworkID, V.PatternCode, V.PatternDesc, V.Cost, V.ArtworkTypeID, v.qty";
                #endregion
            }
            else
            {
                #region -- 非ArtworK類 的sql command --
                SqlCmd = string.Format(@"
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
		, PatternCode = Order_TmsCost.ArtworkTypeID 
		, PatternDesc = Order_TmsCost.ArtworkTypeID 
		, LocalSuppID = rtrim(order_tmscost.LocalSuppID) 
		, Cost = Order_TmsCost.Price
		, costStitch = 1 
		, stitch = 1 
		, unitprice = Order_TmsCost.Price
		, qtygarment = isnull(order_tmscost.Qty,1)
		, poqty = sum(v.Qty) 
		, Order_TmsCost.ArtworkInLine
		, Order_TmsCost.artworkoffline
		, Orders.SewInLine
		, Order_TmsCost.ApvDate
		, message = '' 
FROM Order_TmsCost WITH (NOLOCK) 
inner join Orders WITH (NOLOCK) on orders.id = order_tmscost.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join order_qty v WITH (NOLOCK) on v.id = order_tmscost.id
WHERE 	not exists(
			select * 
			from artworkpo a WITH (NOLOCK) 
			inner join artworkpo_detail ap WITH (NOLOCK) on ap.id = a.id 
			where a.potype = '{0}' 
				  and a.localsuppid = Order_TmsCost.localsuppid 
				  and a.artworktypeid = Order_TmsCost.artworktypeid 
				  and ap.OrderID = orders.ID ) 
		and factory.mdivisionid = '{1}' 
		and factory.IsProduceFty = 1
		and orders.Finished=0
		and orders.IsForecast = 0
		and orders.Junk = 0
		and Order_TmsCost.localsuppid !=''
		and Orders.category  in ('B','S')
		", poType, Sci.Env.User.Keyword);
                SqlCmd += string.Format(" and Order_TmsCost.InhouseOSP = '{0}'", poType);
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
			, Order_TmsCost.Price";
                #endregion
            }



            #endregion

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, SqlCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0)
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
                .Numeric("Cost", header: "Cost(USD)", iseditingreadonly: true, decimal_places: 4, integer_places: 4)
                .Numeric("UnitPrice", header: "Unit Price", iseditable: true, decimal_places: 4, integer_places: 4)
                .Numeric("poqty", header: "Po QTY", iseditingreadonly: true)
                .Date("artworkinline", header: "inline", iseditingreadonly: true)
                .Date("artworkoffline", header: "offline", iseditingreadonly: true)
                .Text("message", header: "Message", iseditingreadonly: true, width: Widths.AnsiChars(30))
                ;
            #endregion
            this.gridBatchCreateFromSubProcessData.Columns[12].Visible = poType == "O";
            this.gridBatchCreateFromSubProcessData.Columns[13].Visible = poType == "O";

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
            string issuedate, delivery;
            issuedate = dateIssueDate.Text;
            delivery = dateDelivery.Text;

            if (dateIssueDate.Value == null)
            {
                MyUtility.Msg.WarningBox("< Issue Date > can't be empty!!");
                dateIssueDate.Focus();
                return;
            }
            if (dateDelivery.Value == null)
            {
                MyUtility.Msg.WarningBox("< Delivery > can't be empty!!");
                dateDelivery.Focus();
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

            if (poType == "O")  // 外發加工需核可且外發單價 > 0
            {
                find = dt.Select("(unitprice = 0 or apvdate is null) and Selected = 1");
                if (find.Length > 0)
                {
                    foreach (DataRow dr in find)
                    {
                        dr["message"] = "Unit price = 0 or Approve Date is null";
                    }
                    MyUtility.Msg.WarningBox("Unit Price and Approve Date of out sourcing can't be zero or empty", "Warning");
                    gridBatchCreateFromSubProcessData.Sort(gridBatchCreateFromSubProcessData.Columns[17], ListSortDirection.Descending);
                    return;
                }
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
                //result = new DualResult(false, "Get Schema(dbo.Artworkpo) Faild!!, Please re-try it later!!",);
                result = DBProxy.Current.GetTableSchema(null, "Artworkpo", out tableSchema); 
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Get Schema(dbo.Artworkpo) Faild!!, Please re-try it later!!");
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
                            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
                            string ftyKeyWord = MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where id='{0}'", q.ftygroup));
                            
                            string id = Sci.MyUtility.GetValue.GetID(ftyKeyWord + "OS", "artworkpo", DateTime.Parse(dateIssueDate.Text));
                            if (MyUtility.Check.Empty(id))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Get Id Faild!!, Please re-try it later!!");
                                return;
                            }
                            decimal ttlamt = 0;
                            string currency = "";
                            string str = "";
                            int exact = 0;

                            #region -- 加總明細金額至表頭 --
                            if (poType == "O")
                            {
                                currency = MyUtility.GetValue.Lookup("CurrencyID", q.localsuppid, "LocalSupp", "ID");
                                str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", currency), null);
                                if (str == null || string.IsNullOrWhiteSpace(str))
                                {
                                    continue;
                                }
                                exact = int.Parse(str);

                                var query3 = (from row in dt.AsEnumerable()
                                              where row.Field<int>("Selected").ToString() == "1"
                                                     && row.Field<string>("ftygroup").ToString() == q.ftygroup
                                                    && row.Field<string>("localsuppid").ToString() == q.localsuppid
                                              group row by new
                                              {
                                                  t1 = row.Field<string>("ftygroup"),
                                                  t2 = row.Field<string>("localsuppid"),
                                                  t3 = row.Field<string>("artworktypeid")
                                              } into m
                                              select new
                                              {
                                                  amount = m.Sum(n => n.Field<int?>("poqty") * n.Field<decimal?>("unitprice") * n.Field<decimal?>("qtygarment"))
                                              });
                                foreach (var q3 in query3)
                                {
                                    ttlamt = (decimal)q3.amount;
                                }
                            }

                            #endregion

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
                                             t6 = row.Field<int>("coststitch"),
                                             t7 = row.Field<int>("stitch"),
                                             t8 = row.Field<decimal>("cost"),
                                             t9 = row.Field<decimal>("unitprice"),
                                             t10 = row.Field<decimal>("QtyGarment"),
                                         } into m
                                         select new
                                         {
                                             orderid = m.Key.t1,
                                             ArtworkTypeID = m.Key.t2,
                                             artworkid = m.Key.t3,
                                             PatternCode = m.Key.t4,
                                             PatternDesc = m.Key.t5,
                                             coststitch = m.Key.t6,
                                             stitch = m.Key.t7,
                                             cost = m.Key.t8,
                                             unitprice = m.Key.t9,
                                             QtyGarment = m.Key.t10,
                                             poqty = m.Sum(n => n.Field<int>("poqty"))

                                         };

                            #endregion
                            #region 表頭sql
                            sqlcmd = string.Format(@"INSERT INTO [dbo].[ArtworkPO]
                                                    ([ID]
                                                    ,[MdivisionId]
                                                    ,[FactoryId]      
                                                    ,[LocalSuppID]    
                                                    ,[IssueDate]      
                                                    ,[Delivery]       
                                                    ,[ArtworkTypeID]  

                                                    ,[CurrencyId]     
                                                    ,[Amount]         

                                                    ,[InternalRemark] 
                                                    ,[Handle]         
                                                    ,[POType]         
                                                    ,[AddName]        
                                                    ,[AddDate],[Status])       
                                                    VALUES            
                                                    ('{0}'    
                                                    ,'{12}'
                                                    ,'{1}'   
                                                    ,'{2}'
                                                    ,'{3}'
                                                    ,'{4}'
                                                    ,'{5}'
                                                    ,'{6}'
                                                    ,{7}
                                                    ,{8}
                                                    ,'{9}'
                                                    ,'{10}'  
                                                    ,'{11}'
                                                    ,getdate(),'New')",
                                        id, q.ftygroup, q.localsuppid, issuedate, delivery, q.artworktypeid,
                                        currency, MyUtility.Math.Round(ttlamt, exact),
                                        "'by batch create!'", Env.User.UserID, poType, Env.User.UserID, Env.User.Keyword);

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
                                    sqlcmd2.Append(string.Format(@"INSERT INTO [dbo].[ArtworkPO_Detail]
                                    ([ID]
                                    ,[OrderID]    
                                    ,[ArtworkId]  
                                    ,[PatternCode]
                                    ,[PatternDesc]
                                    ,[CostStitch] 
                                    ,[Stitch]     
                                    ,[UnitPrice]  
                                    ,[Cost]       
                                    ,[QtyGarment] 
                                    ,[Price]      
                                    ,[Amount]     
                                    ,[PoQty]      
                                    ,[ArtworkTypeID])
                                    VALUES    
                                    ('{0}'  
                                    ,'{1}'  
                                    ,'{2}'  
                                    ,'{3}'  
                                    ,'{4}'  
                                    ,{5}    
                                    ,{6}    
                                    ,{7}    
                                    ,{8}    
                                    ,{9}    
                                    ,{10}   
                                    ,{11}   
                                    ,{12}   
                                    ,'{13}')", id, q2.orderid, q2.artworkid, q2.PatternCode, q2.PatternDesc
                                    , q2.coststitch, q2.stitch, q2.unitprice, q2.cost
                                    , 1
                                    , q2.poqty
                                    , q2.unitprice * q2.QtyGarment
                                    , q2.poqty * q2.unitprice * q2.QtyGarment
                                    , q2.ArtworkTypeID));
                                    #endregion
                                }
                            }
                            else
                            {
                                foreach (var q2 in query2.ToList()) // 明細資料
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append(string.Format(@"INSERT INTO [dbo].[ArtworkPO_Detail]
                                    ([ID]
                                    ,[OrderID]    
                                    ,[ArtworkId]  
                                    ,[PatternCode]
                                    ,[PatternDesc]
                                    ,[CostStitch] 
                                    ,[Stitch]     
                                    ,[UnitPrice]  
                                    ,[Cost]       
                                    ,[QtyGarment] 
                                    ,[Price]      
                                    ,[Amount]     
                                    ,[PoQty]      
                                    ,[ArtworkTypeID])
                                    VALUES    
                                    ('{0}'  
                                    ,'{1}'  
                                    ,'{2}'  
                                    ,'{3}'  
                                    ,'{4}'  
                                    ,{5}    
                                    ,{6}    
                                    ,{7}    
                                    ,{8}    
                                    ,{9}    
                                    ,{10}   
                                    ,{11}   
                                    ,{12}   
                                    ,'{13}')", id, q2.orderid, q2.artworkid, q2.PatternCode, q2.PatternDesc
                                    , q2.coststitch, q2.stitch, q2.unitprice, q2.cost, q2.QtyGarment, q2.poqty
                                    , q2.unitprice * q2.QtyGarment, q2.poqty * q2.unitprice * q2.QtyGarment
                                    , q2.ArtworkTypeID));
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
                            result = new DualResult(false,"Commit transaction error.",ex);
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                    _transactionscope.Dispose();
                    _transactionscope = null;
                }
            }
        }

        //excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(dt);
            sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P01_BatchCreate"));
        }

        private void txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            isArtwork = MyUtility.GetValue.Lookup(string.Format("select isartwork from artworktype WITH (NOLOCK) where id = '{0}'"
                , ((Sci.Production.Class.txtartworktype_fty)sender).Text), null);
        }
    }
}
