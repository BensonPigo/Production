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
    public partial class P01_BatchCreate : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private string poType, isArtwork;
        protected DataTable dtArtwork;
        string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, sp_b, sp_e, artworktype;
        bool isNeedPlanningB03Quote = false;

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

            this.gridBatchCreateFromSubProcessData.RowPostPaint += GridBatchCreateFromSubProcessData_RowPostPaint;
        }

        private void GridBatchCreateFromSubProcessData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.DetalGridCellEditChange(e.RowIndex);
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

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }


            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;
            this.artworktype = txtartworktype_ftyArtworkType.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
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

            if (poType == "O")
            {
                this.isNeedPlanningB03Quote = Prgs.CheckNeedPlanningP03Quote(this.txtartworktype_ftyArtworkType.Text);
            }

            if (this.isNeedPlanningB03Quote)
            {
                SqlCmd = this.QuoteFromPlanningB03();
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
                find = dt.Select("(unitprice = 0 or (apvdate is null and IsArtwork = 1)) and Selected = 1");
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
                                                  amount = m.Sum(n => n.Field<decimal?>("poqty") * n.Field<decimal?>("unitprice") *
                                                                            (n.Field<decimal?>("qtygarment") == 0 || n.Field<decimal?>("qtygarment") == null ? 1 : n.Field<decimal?>("qtygarment"))
                                                                )
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
                                             t7 = row.Field<decimal>("stitch"),
                                             t8 = row.Field<decimal>("cost"),
                                             t9 = row.Field<decimal>("unitprice"),
                                             t10 = (row.Field<decimal>("QtyGarment") == 0 ? 1 : row.Field<decimal>("QtyGarment")),
                                             t11 = row.Field<string>("ArtworkReqID")
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
                                             poqty = m.Sum(n => n.Field<decimal>("poqty")),
                                             ArtworkReqID = m.Key.t11
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
                                    ,[ArtworkTypeID]
                                    ,[ArtworkReqID])
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
                                    ,'{13}'
                                    ,'{14}')", id, q2.orderid, q2.artworkid, q2.PatternCode, q2.PatternDesc
                                    , q2.coststitch, q2.stitch, q2.unitprice, q2.cost
                                    , 1
                                    , q2.unitprice * q2.QtyGarment
                                    , q2.poqty * q2.unitprice * q2.QtyGarment
                                    , q2.poqty
                                    , q2.ArtworkTypeID, q2.ArtworkReqID));
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
                                    ,[ArtworkTypeID]
                                    ,[ArtworkReqID])
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
                                    ,'{13}'
                                    ,'{14}')", id, q2.orderid, q2.artworkid, q2.PatternCode, q2.PatternDesc
                                    , q2.coststitch, q2.stitch, q2.unitprice, q2.cost, q2.QtyGarment
                                    , q2.unitprice * q2.QtyGarment
                                    , q2.poqty * q2.unitprice * q2.QtyGarment
                                    , q2.poqty, q2.ArtworkTypeID, q2.ArtworkReqID));
                                    #endregion
                                }
                            }

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd2.ToString())))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }

                            result = Prgs.UpdateArtworkReq_DetailArtworkPOID(id);
                            if (!result)
                            {
                                _transactionscope.Dispose();
                                this.ShowErr(result);
                                break;
                            }

                            _transactionscope.Complete();
                            _transactionscope.Dispose();
                            MyUtility.Msg.WarningBox("Complete!");
                        }
                        catch (Exception ex)
                        {
                            _transactionscope.Dispose();
                            result = new DualResult(false, "Commit transaction error.", ex);
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }

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
            sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P01_BatchCreate"));
        }

        private void txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            isArtwork = MyUtility.GetValue.Lookup(string.Format("select isartwork from artworktype WITH (NOLOCK) where id = '{0}'"
                , ((Sci.Production.Class.txtartworktype_fty)sender).Text), null);
        }

        private string QuoteFromPlanningB03()
        {
            string SqlCmd;
            SqlCmd = string.Format(@"
SELECT 	Selected = 0 
		, orders.FTYGroup
		, orderid = Order_TmsCost.ID
		, article = rtrim(v.article) 
		, Styleid = rtrim(Orders.Styleid) 
		, orders.ordertypeid
		, Orders.SeasonID
		, Orders.SciDelivery
		, ar.ArtworkTypeID
		, ArtworkID = rtrim(ard.ArtworkID) 
		, ard.PatternCode
		, ard.PatternDesc
		, LocalSuppID = rtrim(order_tmscost.LocalSuppID) 
        , [Cost] = isnull(cost.value,0)
		, costStitch = v.qty
		, stitch = ard.Stitch
		, unitprice = isnull(unitprice.value,0)
		, qtygarment = ard.QtyGarment
		, poqty = ard.ReqQty
		, Order_TmsCost.ArtworkInLine
		, Order_TmsCost.artworkoffline
		, Order_TmsCost.apvdate 
		, message = '' 
        , IsArtwork = 1
		, [ArtworkReq_DetailUkey] = ard.Ukey
		, [ArtworkReqID] = ar.ID
FROM Order_TmsCost WITH (NOLOCK) 
inner join Orders WITH (NOLOCK) on Order_TmsCost.id = Orders.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join ArtworkType awt WITH (NOLOCK) on Order_TmsCost.ArtworkTypeID=awt.ID
inner join view_order_artworks v on v.id = Order_TmsCost.id 
									and v.artworktypeid = Order_TmsCost.artworktypeid
inner join ArtworkReq_Detail ard with (nolock) on   ard.OrderId = Orders.ID and 
                                                    ard.ArtworkID = v.ArtworkID and 
                                                    ard.PatternCode = v.PatternCode and 
                                                    ard.PatternDesc = v.PatternDesc and
                                                    ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = Order_TmsCost.artworktypeid  and ar.Status = 'Approved'
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
WHERE 	orders.Finished=0                                                                 
		AND orders.IsForecast = 0                                                             
		AND orders.Junk = 0 
		AND factory.mdivisionid = '{0}'
		AND factory.IsProduceFty = 1
		AND Order_TmsCost.localsuppid !=''
        AND (orders.Category ='s' or (orders.Category='B' AND Order_TmsCost.Price > 0) AND Order_TmsCost.InhouseOSP = 'O')
		--↓(ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1)), ISP20190803增加IsSintexSubcon狀況
		--↓或是sao.Ukey is not null, 原本存在 View_Style_Artwork, Style_Artwork_Quot
		and ((ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1)) or sao.Ukey is not null)
		", Sci.Env.User.Keyword);

            if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkTypeID = '{0}'", artworktype); }
            if (!(string.IsNullOrWhiteSpace(apvdate_b))) { SqlCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(string.IsNullOrWhiteSpace(apvdate_e))) { SqlCmd += string.Format(" and ((ar.DeptApvDate <= '{0}' and ar.Exceed = 0) or (ar.MgApvDate <= '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_b))) { SqlCmd += string.Format("and  Orders.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_e))) { SqlCmd += string.Format("and  Orders.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", sp_b, sp_e); }
            SqlCmd += @"
group by 	orders.FTYGroup, Order_TmsCost.ID, v.article, Orders.Styleid, Orders.SeasonID
			, Orders.OrderTypeId, Orders.SciDelivery
			, order_tmscost.LocalSuppID, order_tmscost.Qty, Order_TmsCost.ArtworkInLine
			, Order_TmsCost.artworkoffline, Orders.SewInLine, Order_TmsCost.ApvDate
			, ard.ArtworkID, ard.PatternCode, ard.PatternDesc, cost.value, ar.ArtworkTypeID, v.qty,unitprice.value, ard.Ukey, ar.ID,ard.Stitch,ard.QtyGarment,ard.ReqQty";

            return SqlCmd;
        }

        private string QuoteFromTmsCost()
        {
            string SqlCmd;
            // 建立可以符合回傳的Cursor

            #region -- 非ArtworK類 的sql command --
            SqlCmd = string.Format(@"
SELECT 	Selected = 0 
		, Orders.FTYGroup
		, orderid = Order_TmsCost.ID 
		, Styleid = rtrim(Orders.Styleid) 
		, SeasonID = rtrim(Orders.SeasonID) 
        , [Article] = (SELECT Stuff((select concat( ',',Article)   from Order_Article with (nolock) where ID = Order_TmsCost.ID FOR XML PATH('')),1,1,'') )
		, orders.ordertypeid
		, Orders.SciDelivery
		, ar.ArtworkTypeID
		, ArtworkID = ard.ArtworkID 
		, PatternCode = ard.PatternCode 
		, PatternDesc = ard.PatternDesc 
		, LocalSuppID = rtrim(order_tmscost.LocalSuppID) 
		, Cost = Order_TmsCost.Price
		, costStitch = 1 
		, stitch = ard.stitch
		, unitprice = Order_TmsCost.Price
		, qtygarment = ard.QtyGarment
		, poqty = ard.ReqQty
		, Order_TmsCost.ArtworkInLine
		, Order_TmsCost.artworkoffline
		, Orders.SewInLine
		, Order_TmsCost.ApvDate
		, message = '' 
        , IsArtwork = 0
		, [ArtworkReq_DetailUkey] = ard.Ukey
		, [ArtworkReqID] = ar.ID
FROM Order_TmsCost WITH (NOLOCK) 
inner join Orders WITH (NOLOCK) on orders.id = order_tmscost.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join ArtworkType awt WITH (NOLOCK) on Order_TmsCost.ArtworkTypeID=awt.ID
inner join ArtworkReq_Detail ard with (nolock) on ard.OrderId = Orders.ID and ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = Order_TmsCost.ArtworkTypeID  and ar.Status = 'Approved'
WHERE 	factory.mdivisionid = '{1}' 
		and factory.IsProduceFty = 1
		and orders.Finished=0
		and orders.IsForecast = 0
		and orders.Junk = 0
		and Order_TmsCost.localsuppid !=''		
        --and Orders.PulloutComplete = 0
		", poType, Sci.Env.User.Keyword);
            SqlCmd += string.Format(" and Order_TmsCost.InhouseOSP = '{0}'", poType);
            switch (poType)
            {
                case "O":
                    SqlCmd += $@" 
        and (
                orders.Category ='s' 
                or (Order_TmsCost.Price > 0 and orders.Category = 'B')
        )";
                    break;
                case "I":
                    SqlCmd += $@" 
        and orders.Category in ('S','B') ";
                    break;
            }
            if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkTypeID = '{0}'", artworktype); }
            if (!(string.IsNullOrWhiteSpace(apvdate_b))) { SqlCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(string.IsNullOrWhiteSpace(apvdate_e))) { SqlCmd += string.Format(" and ((ar.DeptApvDate <= '{0}' and ar.Exceed = 0) or (ar.MgApvDate <= '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_b))) { SqlCmd += string.Format("and  Orders.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(string.IsNullOrWhiteSpace(sciDelivery_e))) { SqlCmd += string.Format("and  Orders.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", sp_b, sp_e); }
            SqlCmd += @" 
group by	orders.FTYGroup, Order_TmsCost.ID, Orders.Styleid, Orders.SeasonID
			, Orders.OrderTypeId, Orders.SciDelivery, ar.ArtworkTypeID
			, order_tmscost.LocalSuppID, Order_TmsCost.ArtworkInLine
			, Order_TmsCost.artworkoffline, Orders.SewInLine, Order_TmsCost.ApvDate
			, Order_TmsCost.Price,ard.Ukey,ar.ID,ard.PatternCode ,ard.PatternDesc ,ard.stitch, ard.QtyGarment,ard.ArtworkID,ard.ReqQty ";
            #endregion

            return SqlCmd;
        }

        private void DetalGridCellEditChange(int index)
        {

            #region 檢查Qty欄位是否可編輯
            string spNo = this.gridBatchCreateFromSubProcessData.GetDataRow(index)["orderid"].ToString();

            string sqlCheckSampleOrder = $@"
select 1
from orders with (nolock)
where id = '{spNo}' and Category = 'S'
";
            bool isSampleOrder = MyUtility.Check.Seek(sqlCheckSampleOrder);

            if (!isSampleOrder && this.isNeedPlanningB03Quote)
            {
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].ReadOnly = true;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Black;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.BackColor = Color.White; //Unit Price
            }
            else
            {
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].ReadOnly = false;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Red;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.BackColor = Color.Pink; //Unit Price
            }

            #endregion
        }
    }
}
