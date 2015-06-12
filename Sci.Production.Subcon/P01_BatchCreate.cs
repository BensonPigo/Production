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

namespace Sci.Production.Subcon
{
    public partial class P01_BatchCreate : Sci.Win.Subs.Base
    {
        DataRow dr_artworkpo;
        DataTable dt_artworkpoDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag ;
        string poType ,isArtwork;
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

            dateBox1.Value = DateTime.Today;
            dateBox2.Value = DateTime.Today;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, artworktype;
            apvdate_b = null;
            apvdate_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            Inline_b = null;
            Inline_e = null;
            artworktype = null;

            if (dateRange1.Value1 != null) apvdate_b = this.dateRange1.Text1;
            if (dateRange1.Value2 != null) { apvdate_e = this.dateRange1.Text2; }
            if (dateRange2.Value1 != null) sciDelivery_b = this.dateRange2.Text1;
            if (dateRange2.Value2 != null) { sciDelivery_e = this.dateRange2.Text2; }
            if (dateRange3.Value1 != null) Inline_b = this.dateRange3.Text1;
            if (dateRange3.Value2 != null) { Inline_e = this.dateRange3.Text2; }

            String sp_b = this.textBox1.Text;
            String sp_e = this.textBox2.Text;
            artworktype = txtartworktype_fty1.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                myUtility.WarningBox("< Approve Date > or < SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                dateRange1.Focus1();
                return;
            }

            string SqlCmd;
            #region 組query sqlcmd
            if (string.IsNullOrWhiteSpace(artworktype))
            {
                myUtility.WarningBox("< Artwork Type > can't be empty!!");
                txtartworktype_fty1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                if (isArtwork.ToUpper() == "TRUE")
                {
                    SqlCmd = @"SELECT 0 Selected, 									
                                orders.FTYGroup,															
                                Order_TmsCost.ID as orderid,                  
                                rtrim(v.article) article,                     
                                rtrim(Orders.Styleid) Styleid,                
                                Orders.SeasonID,                              
                                Orders.SciDelivery,                           
                                Order_TmsCost.ArtworkTypeID,                  
                                rtrim(v.ArtworkID) ArtworkID,                 
                                v.PatternCode,                                
                                v.PatternDesc,                                
                                rtrim(order_tmscost.LocalSuppID) LocalSuppID, 
                                v.Cost,                                       
                                v.stitch costStitch,                          
                                v.stitch,                                     
                                isnull((select b.Price from                   
	                                style_artwork a,Style_Artwork_Quot b        
	                                where a.StyleUkey = orders.StyleUkey        
	                                and a.Ukey = b.Ukey                         
	                                and a.Article =v.article                    
	                                and a.ArtworkTypeID=v.ArtworkTypeID         
	                                and a.ArtworkID = v.ArtworkID               
	                                and a.PatternCode = v.PatternCode 
	                                and b.LocalSuppId = Order_TmsCost.LocalSuppID and b.PriceApv = 'Y'),0) as unitprice,

                                order_tmscost.Qty qtygarment,                                                         
                                sum(v.poqty) poqty ,                                                                  
                                Order_TmsCost.ArtworkInLine,                                                          
                                Order_TmsCost.artworkoffline, rtrim(orders.factoryid) factoryid,'' message            
                                ,Order_TmsCost.apvdate                                                                
                                ,Orders.Factoryid                                                                     
                               
                                FROM Order_TmsCost, Orders,view_order_artworks v 
	                                ,(	select c.ID,c.ArtworkTypeID,d.ArtworkID,d.PatternCode
		                                from Order_TmsCost c, Order_Artwork d
		                                where c.ID=d.id 
		                                and c.ArtworkTypeID = d.ArtworkTypeID 
		                                and c.apvdate is not null 
		                                and c.localsuppid !='' ";
                    SqlCmd += string.Format(" AND c.InhouseOSP = '{0}'", poType);
                    if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and c.ArtworkTypeID = '{0}'", artworktype); }
                    if (!(string.IsNullOrWhiteSpace(apvdate_b))) { SqlCmd += string.Format(" and c.ApvDate between '{0}' and '{1}'", apvdate_b, apvdate_e); }
                    if (!(string.IsNullOrWhiteSpace(Inline_b)))
                    {
                        SqlCmd += string.Format(" and not (c.ArtworkInLine > '{0}' or c.ArtworkOffLine < '{1}') ", Inline_b, Inline_e);
                    }
                    SqlCmd += string.Format(@"
                                        except
		                                select b1.orderid
		                                    ,a1.ArtworkTypeID
		                                    ,b1.ArtworkId,b1.PatternCode
		                                from artworkpo a1, ArtworkPO_Detail b1
		                                where a1.id = b1.id
		                                and a1.POType = '{0}'
                                    ) awk 
                                WHERE  awk.ID = Order_TmsCost.ID                                                   
                                and awk.ArtworkTypeID = Order_TmsCost.ArtworkTypeID
                                and awk.ID = Orders.ID                                                   
                                and awk.ArtworkTypeID = v.ArtworkTypeID
                                and awk.ArtworkID = v.ArtworkID
                                and awk.PatternCode = v.PatternCode                                  
                                and awk.id = v.id                                                 
                                and orders.Finished=0                                                                 
                                AND orders.IsForecast = 0                                                             
                                AND orders.Junk = 0 ", poType);

                    SqlCmd += string.Format(" AND orders.factoryid = '{0}'", Env.User.Factory);
                    SqlCmd += string.Format(" AND Order_TmsCost.InhouseOSP = '{0}'", poType);


                    if (!(string.IsNullOrWhiteSpace(sciDelivery_b))) { SqlCmd += string.Format(" and Orders.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
                    if (!(string.IsNullOrWhiteSpace(sp_b))) { SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", sp_b, sp_e); }

                    SqlCmd += @" group by
                                orders.FTYGroup,
                                Order_TmsCost.ID,           
                                v.article,                  
                                Orders.Styleid,             
                                Orders.SeasonID,            
                                Orders.SciDelivery,         
                                Order_TmsCost.ArtworkTypeID,
                                order_tmscost.LocalSuppID,  
                                order_tmscost.Qty ,         
                                Order_TmsCost.ArtworkInLine,
                                Order_TmsCost.artworkoffline,
                                Orders.SewInLine,           
                                Order_TmsCost.ApvDate       
                                ,orders.StyleUkey           
                                ,V.ArtworkID                
                                ,V.PatternCode              
                                ,V.PatternDesc              
                                ,V.Cost                     
                                ,V.ArtworkTypeID,orders.factoryid,v.stitch";
                }
                else
                {
                    SqlCmd = string.Format(@" SELECT 0 Selected
                                    , orders.FTYGroup,														
                                    Order_TmsCost.ID as orderid,                 
                                    rtrim(v.article) article,                    
                                    rtrim(Orders.Styleid) Styleid,               
                                    rtrim(Orders.SeasonID) SeasonID,             
                                    Orders.SciDelivery,                          
                                    Order_TmsCost.ArtworkTypeID,                 
                                    Order_TmsCost.ArtworkTypeID ArtworkID,       
                                    Order_TmsCost.ArtworkTypeID PatternCode,     
                                    Order_TmsCost.ArtworkTypeID PatternDesc,     
                                    rtrim(order_tmscost.LocalSuppID) LocalSuppID,
                                    0.0 Cost,                                    
                                    1.0 costStitch,                              
                                    1.0 stitch,                                  
                                isnull((select b.Price 
                                        from style_artwork a,Style_Artwork_Quot b         
                                        where a.StyleUkey = orders.StyleUkey         
                                        and a.Ukey = b.Ukey                          
                                        and a.Article =v.article                     
                                        and b.LocalSuppId = Order_TmsCost.LocalSuppID and b.PriceApv = 'Y'),0) as unitprice,
                                isnull(order_tmscost.Qty,1) qtygarment,                           
                                sum(v.Qty) poqty ,                                                
                                Order_TmsCost.ArtworkInLine,                                      
                                Order_TmsCost.artworkoffline,                                     
                                Orders.SewInLine,
                                Order_TmsCost.ApvDate,rtrim(orders.factoryid) factoryid,'' message
                                FROM Order_TmsCost, Orders, order_qty v
                                     ,(	select c.ID,c.ArtworkTypeID
		                                from Order_TmsCost c
		                                where c.InhouseOSP = '{0}' 
                                        AND LocalSuppID is not null 
                                        and LocalSuppID !=''
                                        and c.apvdate is not null", poType);
                    if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and c.ArtworkTypeID = '{0}'", artworktype); }
                    SqlCmd += string.Format(@"
                                        except
		                                select b1.orderid
		                                    ,a1.ArtworkTypeID
		                                from artworkpo a1, ArtworkPO_Detail b1
		                                where a1.id = b1.id
		                                and a1.POType = '{0}'
                                    ) awk     
                                WHERE  Order_TmsCost.ID = Orders.ID
                                and awk.id = orders.id                               
                                AND LocalSuppID is not null and LocalSuppID !=''                                      
                                and orders.Finished=0                                             
                                AND orders.IsForecast = 0                                         
                                AND orders.Junk = 0                                               
                                and Orders.id = v.id", poType);
                    SqlCmd += string.Format(" AND orders.factoryid = '{0}'", Env.User.Factory);
                    SqlCmd += string.Format(" AND Order_TmsCost.InhouseOSP = '{0}'", poType);
                    if (!(string.IsNullOrWhiteSpace(artworktype))) { SqlCmd += string.Format(" and Order_TmsCost.ArtworkTypeID = '{0}'", artworktype); }
                    if (!(string.IsNullOrWhiteSpace(sciDelivery_b))) { SqlCmd += string.Format(" and Orders.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
                    if (!(string.IsNullOrWhiteSpace(sp_b))) { SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", sp_b, sp_e); }
                    SqlCmd += @" group by                      
                                orders.FTYGroup,							
                                Order_TmsCost.ID,             
                                v.article,                    
                                Orders.Styleid,               
                                Orders.SeasonID,              
                                Orders.SciDelivery,           
                                Order_TmsCost.ArtworkTypeID,  
                                order_tmscost.LocalSuppID,    
                                order_tmscost.Qty ,           
                                Order_TmsCost.ArtworkInLine,  
                                Order_TmsCost.artworkoffline, 
                                Orders.SewInLine,             
                                Order_TmsCost.ApvDate         
                                ,orders.StyleUkey,orders.factoryid";
                }
            }
        


            #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, SqlCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { myUtility.WarningBox("Data not found!!"); }
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

            //Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            //ns2.CellValidating += (s, e) =>
            //{
            //    DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
            //    ddr["Price"] = (decimal)e.FormattedValue * (decimal)ddr["UnitPrice"];
            //    ddr["Amount"] = (decimal)e.FormattedValue * (int)ddr["poqty"] * (decimal)ddr["UnitPrice"];
            //};

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   
                .Text("Factoryid", header: "Fty", iseditingreadonly: true)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("Styleid", header: "Style", iseditingreadonly: true)
                .Text("SeasonID", header: "Season", iseditingreadonly: true)
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

            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Create
        private void button2_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            string issuedate, delivery;
            issuedate = dateBox1.Text;
            delivery = dateBox2.Text;

            if (dateBox1.Value ==null)
            {
                myUtility.WarningBox("< Issue Date > can't be empty!!");
                dateBox1.Focus();
                return;
            }
            if (dateBox2.Value == null)
            {
                myUtility.WarningBox("< Delivery > can't be empty!!");
                dateBox2.Focus();
                return;
            }

            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;

            DataRow[] find ;

            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                myUtility.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            //find = dt.Select("(unitprice = 0 or apvdate is null) and Selected = 1");
            //if (find.Length > 0)
            //{
            //    foreach (DataRow dr in find)
            //    {
            //        dr["message"] = "Unit price = 0 or Approve Date is null";
            //    }
            //    myUtility.WarningBox("Unit Price or Approve Date can't be zero or empty", "Warning");
            //    grid1.Sort(grid1.Columns[17], ListSortDirection.Descending);
            //    return;
            //}

            #region 表頭資料group by --- LINQ
            var query = (from row in dt.AsEnumerable()
                        where row.Field<int>("Selected").ToString()=="1"
                        group row by new { t1 = row.Field<string>("ftygroup"), 
                                           t2 = row.Field<string>("localsuppid") ,
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
                TransactionScope _transactionscope = new TransactionScope();
                DualResult result;
                string sqlcmd;

                foreach (var q  in  query.ToList())
                {
                    using (_transactionscope)
                    {
                        try
                        {
                            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
                            ITableSchema tableSchema = null;
                            result = DBProxy.Current.GetTableSchema(null, "artworkpo", out tableSchema);
                            string id = Sci.myUtility.GetID(ProductionEnv.Keyword+"OS", "artworkpo",DateTime.Parse(dateBox1.Text));
                            decimal ttlamt = 0;
                            string currency = "";
                            string str = "";
                            int exact = 0;

                            #region 加總明細金額至表頭
                            if (poType == "O")
                            {
                                currency = myUtility.Lookup("CurrencyID", q.localsuppid, "LocalSupp", "ID");
                                str = myUtility.Lookup(string.Format("Select exact from Currency where id = '{0}'", currency), null);
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

                            #region 表身資料 group by -- LINQ
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
                                             t6 = row.Field<decimal>("coststitch"),
                                             t7 = row.Field<decimal>("stitch"),
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
                                                    ,[AddDate])       
                                                    VALUES            
                                                    ('{0}'    
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
                                                    ,getdate())",
                                        id, Env.User.Factory, q.localsuppid, issuedate, delivery, q.artworktypeid,
                                        currency,myUtility.Round(ttlamt, exact),
                                        "'by batch create!'",Env.User.UserID,poType,Env.User.UserID);
                           
                            #endregion

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
                            {
                                myUtility.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }

                            #region 新增明細
                            foreach (var q2 in query2.ToList())
                            {
                                sqlcmd = string.Format(@"INSERT INTO [dbo].[ArtworkPO_Detail]
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
                                , q2.ArtworkTypeID);

                                if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
                                {
                                    myUtility.WarningBox("Create failed, Pleaes re-try");
                                    break;
                                }
                            }
                            #endregion
                            _transactionscope.Complete();
                            myUtility.WarningBox("Complete!");
                        }

                        catch (Exception ex)
                        {
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                    _transactionscope.Dispose();
                    _transactionscope = null;
                }
            } 

            
        }

        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            isArtwork = myUtility.Lookup(string.Format("select isartwork from artworktype where id = '{0}'", ((Sci.Production.Class.txtartworktype_fty)sender).Text), null);
            this.dateRange1.Enabled = (isArtwork.ToUpper() == "TRUE");
            this.dateRange3.Enabled = (isArtwork.ToUpper() == "TRUE");
        }


    }
}
