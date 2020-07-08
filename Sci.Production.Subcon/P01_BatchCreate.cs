using System;
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
using Sci.Production.PublicPrg;

namespace Sci.Production.Subcon
{
    public partial class P01_BatchCreate : Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private string poType;
        private string isArtwork;
        protected DataTable dtArtwork;
        string apvdate_b;
        string apvdate_e;
        string sciDelivery_b;
        string sciDelivery_e;
        string sp_b;
        string sp_e;
        string artworktype;
        bool isNeedPlanningB03Quote = false;

        public P01_BatchCreate()
        {
            this.InitializeComponent();
        }

        public P01_BatchCreate(string fuc)
        {
            this.InitializeComponent();

            if (fuc == "P01")
            {
                this.poType = "O";
                this.Text += " (Sub-con Purchase Order)";
            }
            else
            {
                this.poType = "I";
                this.Text += " (In-House Requisition)";
            }

            this.dateIssueDate.Value = DateTime.Today;
            this.dateDelivery.Value = DateTime.Today;
        }

        // Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            this.FindData(true);
            this.gridBatchCreateFromSubProcessData.AutoResizeColumns();

            foreach (DataGridViewRow dr in this.gridBatchCreateFromSubProcessData.Rows)
            {
                this.DetalGridCellEditChange(dr.Index);
            }
        }

        private void FindData(bool showNoDataMsg)
        {
            this.apvdate_b = null;
            this.apvdate_e = null;
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;

            if (this.dateApproveDate.Value1 != null)
            {
                this.apvdate_b = this.dateApproveDate.Text1;
            }

            if (this.dateApproveDate.Value2 != null)
            {
                this.apvdate_e = this.dateApproveDate.Text2;
            }

            if (this.dateSCIDelivery.Value1 != null)
            {
                this.sciDelivery_b = this.dateSCIDelivery.Text1;
            }

            if (this.dateSCIDelivery.Value2 != null)
            {
                this.sciDelivery_e = this.dateSCIDelivery.Text2;
            }

            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;
            this.artworktype = this.txtartworktype_ftyArtworkType.Text;

            if ((this.apvdate_b == null && this.apvdate_e == null) &&
                (this.sciDelivery_b == null && this.sciDelivery_e == null) &&
                string.IsNullOrWhiteSpace(this.sp_b) && string.IsNullOrWhiteSpace(this.sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < SP# > can't be empty!!");
                this.dateApproveDate.Focus1();
                return;
            }

            if (!MyUtility.Check.Empty(this.apvdate_e))
            {
                this.apvdate_e = Convert.ToDateTime(this.apvdate_e).AddDays(1).ToString("yyyyMMdd");
            }

            string SqlCmd;
            #region 組query sqlcmd
            if (string.IsNullOrWhiteSpace(this.artworktype))
            {
                MyUtility.Msg.WarningBox("< Artwork Type > can't be empty!!");
                this.txtartworktype_ftyArtworkType.Focus();
                return;
            }

            if (this.poType == "O")
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

            DualResult result;
            if (result = DBProxy.Current.Select(null, SqlCmd, out this.dtArtwork))
            {
                DualResult resultGetSpecialRecordData = this.GetSpecialRecordData();
                if (!resultGetSpecialRecordData)
                {
                    this.ShowErr(resultGetSpecialRecordData);
                }

                if (this.dtArtwork.Rows.Count == 0 && showNoDataMsg)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtArtwork;
            }
            else
            {
                this.ShowErr(SqlCmd, result);
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Grid 設定 --
            this.gridBatchCreateFromSubProcessData.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchCreateFromSubProcessData.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchCreateFromSubProcessData)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("FTYGroup", header: "Fty", iseditingreadonly: true) // 1
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 2
                .Text("Styleid", header: "Style", iseditingreadonly: true) // 3
                .Text("SeasonID", header: "Season", iseditingreadonly: true) // 4
                .Text("orderTypeId", header: "Order Type", iseditingreadonly: true) // 5
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
                .Numeric("FarmOut", header: "Farm Out", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("FarmIn", header: "Farm In", iseditingreadonly: true)
                .Text("IrregularQtyReason", header: "Irregular Qty Reason", iseditingreadonly: true)
                .Text("message", header: "Message", iseditingreadonly: true, width: Widths.AnsiChars(30))
                ;
            #endregion
            this.gridBatchCreateFromSubProcessData.Columns[12].Visible = this.poType == "O";
            this.gridBatchCreateFromSubProcessData.Columns[13].Visible = this.poType == "O";
        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Create
        private void btnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            string issuedate, delivery;
            issuedate = this.dateIssueDate.Text;
            delivery = this.dateDelivery.Text;

            if (this.dateIssueDate.Value == null)
            {
                MyUtility.Msg.WarningBox("< Issue Date > can't be empty!!");
                this.dateIssueDate.Focus();
                return;
            }

            if (this.dateDelivery.Value == null)
            {
                MyUtility.Msg.WarningBox("< Delivery > can't be empty!!");
                this.dateDelivery.Focus();
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
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            if (this.poType == "O") // 外發加工需核可且外發單價 > 0
            {
                find = dt.Select("(unitprice = 0 or (PriceApv <> 'Y' and IsArtwork = 1)) and Selected = 1");
                if (find.Length > 0)
                {
                    foreach (DataRow dr in find)
                    {
                        dr["message"] = "Unit price = 0 or Price not approved";
                    }

                    MyUtility.Msg.WarningBox("Unit Price can't be zero or empty or Price still not approved", "Warning");
                    this.gridBatchCreateFromSubProcessData.Sort(this.gridBatchCreateFromSubProcessData.Columns[16], ListSortDirection.Descending);
                    return;
                }
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

                // result = new DualResult(false, "Get Schema(dbo.Artworkpo) Faild!!, Please re-try it later!!",);
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
                            // 取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
                            string ftyKeyWord = MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where id='{0}'", q.ftygroup));

                            string id = Sci.MyUtility.GetValue.GetID(ftyKeyWord + "OS", "artworkpo", DateTime.Parse(this.dateIssueDate.Text));
                            if (MyUtility.Check.Empty(id))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Get Id Faild!!, Please re-try it later!!");
                                return;
                            }

                            decimal ttlamt = 0;
                            string currency = string.Empty;
                            string str = string.Empty;
                            int exact = 0;

                            #region -- 加總明細金額至表頭 --
                            if (this.poType == "O")
                            {
                                currency = MyUtility.GetValue.Lookup("CurrencyID", q.localsuppid, "LocalSupp", "ID");
                                str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", currency), null);
                                if (str == null || string.IsNullOrWhiteSpace(str))
                                {
                                    continue;
                                }

                                exact = int.Parse(str);

                                var query3 = from row in dt.AsEnumerable()
                                              where row.Field<int>("Selected").ToString() == "1"
                                                     && row.Field<string>("ftygroup").ToString() == q.ftygroup
                                                    && row.Field<string>("localsuppid").ToString() == q.localsuppid
                                              group row by new
                                              {
                                                  t1 = row.Field<string>("ftygroup"),
                                                  t2 = row.Field<string>("localsuppid"),
                                                  t3 = row.Field<string>("artworktypeid"),
                                              }
                                                into m
                                              select new
                                              {
                                                  amount = m.Sum(n => n.Field<decimal?>("poqty") * n.Field<decimal?>("unitprice") *
                                                                            (n.Field<decimal?>("qtygarment") == 0 || n.Field<decimal?>("qtygarment") == null ? 1 : n.Field<decimal?>("qtygarment"))),
                                              };
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
                                             t10 = row.Field<decimal>("QtyGarment") == 0 ? 1 : row.Field<decimal>("QtyGarment"),
                                             t11 = row.Field<string>("ArtworkReqID"),
                                             t12 = MyUtility.Convert.GetDecimal(row["FarmOut"]),
                                             t13 = MyUtility.Convert.GetDecimal(row["FarmIn"]),
                                         }
                                        into m
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
                                             ArtworkReqID = m.Key.t11,
                                             FarmOut = m.Key.t12,
                                             FarmIn = m.Key.t13,
                                         };

                            #endregion
                            #region 表頭sql
                            sqlcmd = string.Format(
                                @"INSERT INTO [dbo].[ArtworkPO]
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
                                "'by batch create!'", Env.User.UserID, this.poType, Env.User.UserID, Env.User.Keyword);

                            #endregion

                            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd)))
                            {
                                _transactionscope.Dispose();
                                MyUtility.Msg.WarningBox("Create failed, Pleaes re-try");
                                break;
                            }

                            // 若是EMB則QTYGarment=1,View_order_artworks.Qty->Coststich&Stich
                            StringBuilder sqlcmd2 = new StringBuilder();
                            if (this.txtartworktype_ftyArtworkType.Text == "EMBROIDERY")
                            {
                                foreach (var q2 in query2.ToList()) // 明細資料
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append(string.Format(
                                        @"INSERT INTO [dbo].[ArtworkPO_Detail]
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
                                    ,[ArtworkReqID],[Farmout],[FarmIn])
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
                                    ,'{14}'
                                    ,'{15}'
                                    ,'{16}'
)", id, q2.orderid, q2.artworkid, q2.PatternCode, q2.PatternDesc,
                                        q2.coststitch, q2.stitch, q2.unitprice, q2.cost,
                                        1,
                                        q2.unitprice * q2.QtyGarment,
                                        q2.poqty * q2.unitprice * q2.QtyGarment,
                                        q2.poqty,
                                        q2.ArtworkTypeID, q2.ArtworkReqID, q2.FarmOut, q2.FarmIn));
                                    #endregion
                                }
                            }
                            else
                            {
                                foreach (var q2 in query2.ToList()) // 明細資料
                                {
                                    #region 新增明細 Sql Command
                                    sqlcmd2.Append(string.Format(
                                        @"INSERT INTO [dbo].[ArtworkPO_Detail]
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
                                    ,[ArtworkReqID],[Farmout],[FarmIn])
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
                                    ,'{14}'
                                    ,'{15}'
                                    ,'{16}')", id, q2.orderid, q2.artworkid, q2.PatternCode, q2.PatternDesc,
                                        q2.coststitch, q2.stitch, q2.unitprice, q2.cost, q2.QtyGarment,
                                        q2.unitprice * q2.QtyGarment,
                                        q2.poqty * q2.unitprice * q2.QtyGarment,
                                        q2.poqty, q2.ArtworkTypeID, q2.ArtworkReqID, q2.FarmOut, q2.FarmIn));
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
                            this.ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }

                    // Import 成功重新Query
                    this.FindData(false);
                }
            }
        }

        // excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(dt);
            sdExcel.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P01_BatchCreate"));
        }

        private void txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            this.isArtwork = MyUtility.GetValue.Lookup(
                string.Format(
                "select isartwork from artworktype WITH (NOLOCK) where id = '{0}'",
                ((Class.txtartworktype_fty)sender).Text), null);
        }

        private string QuoteFromPlanningB03()
        {
            string SqlCmd = string.Empty;
            SqlCmd += $@"
Declare @sp1 varchar(16)= '{this.sp_b}'
Declare @sp2 varchar(16)= '{this.sp_e}'

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
            if (!MyUtility.Check.Empty(this.artworktype))
            {
                SqlCmd += $@" AND s.ArtworkTypeId='{this.artworktype}'";
            }

            if (!MyUtility.Check.Empty(this.sp_b))
            {
                SqlCmd += $@" AND bdl.Orderid >= @sp1 ";
            }

            if (!MyUtility.Check.Empty(this.sp_e))
            {
                SqlCmd += $@" AND bdl.Orderid <= @sp2";
            }

            SqlCmd += string.Format(
                @"
SELECT distinct	Selected = 0 
		, orders.FTYGroup
		, orderid = Orders.ID
		, article = rtrim(v.article) 
		, Styleid = rtrim(Orders.Styleid) 
		, orders.ordertypeid
		, Orders.SeasonID
		, Orders.SciDelivery
		, ar.ArtworkTypeID
		, ArtworkID = rtrim(ard.ArtworkID) 
		, ard.PatternCode
		, ard.PatternDesc
		, LocalSuppID = rtrim(ar.LocalSuppID) 
        , [Cost] = isnull(cost.value,0)
		, costStitch = v.qty
		, stitch = ard.Stitch
		, unitprice = isnull(unitprice.value,0)
		, qtygarment = ard.QtyGarment
		, poqty = ard.ReqQty
		, sao.PriceApv 
		, message = '' 
        , IsArtwork = 1
		, [ArtworkReq_DetailUkey] = ard.Ukey
		, [ArtworkReqID] = ar.ID
        , Orders.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
		,[Farmout] = ISNULL(FarmOut.Value,0)
		,[FarmIn] = ISNULL(FarmIn.Value,0)
into    #quoteDetail
FROM Orders WITH (NOLOCK) 
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
inner join ArtworkType awt WITH (NOLOCK) on awt.ID = '{1}'
inner join view_order_artworks v on v.id = Orders.id 
									and v.artworktypeid = awt.ID
inner join ArtworkReq_Detail ard with (nolock) on   ard.OrderId = Orders.ID and 
                                                    ard.ArtworkID = v.ArtworkID and 
                                                    ard.PatternCode = v.PatternCode and 
                                                    ard.PatternDesc = v.PatternDesc and
                                                    ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = awt.ID  and ar.Status = 'Approved'
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
left join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = orders.StyleUkey and vsa.Article = v.Article and vsa.ArtworkID = v.ArtworkID and
														vsa.ArtworkName = v.ArtworkName and vsa.ArtworkTypeID = v.ArtworkTypeID and vsa.PatternCode = v.PatternCode and
														vsa.PatternDesc = v.PatternDesc 
left join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.LocalSuppID = ar.LocalSuppID  and sao.Price > 0  and sao.PriceApv = 'Y'
left join LocalSupp ls with (nolock) on ls.id = ar.LocalSuppID
outer apply (select value = iif(ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1), v.Cost,sao.Price)) unitprice
outer apply (
    select value = 
        case when ls.IsSintexSubcon = 1 and (awt.isArtwork = 1 or awt.useArtwork = 1) then v.Cost
             when awt.isArtwork = 1 then vsa.Cost
             else sao.Price
             end
)cost
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=Orders.ID 
	AND bd.ArtworkTypeId = awt.ID
	AND bd.Patterncode = v.PatternCode 
	AND bd.PatternDesc = v.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid=Orders.ID 
	AND bd.ArtworkTypeId = awt.ID
	AND bd.Patterncode = v.PatternCode 
	AND bd.PatternDesc = v.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
WHERE 	 orders.IsForecast = 0      
        --and Orders.PulloutComplete = 0
		AND (Orders.Junk=0 or Orders.Junk=1 and Orders.NeedProduction=1)
		AND factory.mdivisionid = '{0}'
		AND factory.IsProduceFty = 1
        AND orders.Category in ('S','B')
		", Sci.Env.User.Keyword, this.artworktype);

            if (!string.IsNullOrWhiteSpace(this.apvdate_b))
            {
                SqlCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", this.apvdate_b);
            }

            if (!string.IsNullOrWhiteSpace(this.apvdate_e))
            {
                SqlCmd += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", this.apvdate_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sciDelivery_b))
            {
                SqlCmd += string.Format("and  Orders.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!string.IsNullOrWhiteSpace(this.sciDelivery_e))
            {
                SqlCmd += string.Format("and  Orders.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", this.sp_b, this.sp_e);
            }

            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                SqlCmd += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            SqlCmd += @"
--將報價相同的Article資料合併
select distinct
        Selected = 0 
		, FTYGroup
		, orderid
		, Styleid
		, ordertypeid
		, SeasonID
		, SciDelivery
		, ArtworkTypeID
		, ArtworkID  
		, PatternCode
		, PatternDesc
		, LocalSuppID 
        , Cost
		, costStitch
		, stitch
		, unitprice
		, qtygarment
		, poqty
		, PriceApv 
		, message
        , IsArtwork
		, ArtworkReq_DetailUkey
		, ArtworkReqID
        , [Article] = (SELECT Stuff((select concat( ',',Article)   
                            from #quoteDetail 
                            where   orderid = main.orderid and 
                                    ArtworkID = main.ArtworkID and
                                    PatternCode = main.PatternCode and 
                                    PatternDesc = main.PatternDesc and
                                    unitprice = main.unitprice FOR XML PATH('')),1,1,'') )
        , Category
        , [IrregularQtyReason]
		,[Farmout]
		,[FarmIn]
from #quoteDetail main

";

            return SqlCmd;
        }

        private string QuoteFromTmsCost()
        {
            string SqlCmd = string.Empty;
            SqlCmd += $@"
Declare @sp1 varchar(16)= '{this.sp_b}'
Declare @sp2 varchar(16)= '{this.sp_e}'

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
            if (!MyUtility.Check.Empty(this.artworktype))
            {
                SqlCmd += $@" AND s.ArtworkTypeId='{this.artworktype}'";
            }

            if (!MyUtility.Check.Empty(this.sp_b))
            {
                SqlCmd += $@" AND bdl.Orderid >= @sp1 ";
            }

            if (!MyUtility.Check.Empty(this.sp_e))
            {
                SqlCmd += $@" AND bdl.Orderid <= @sp2";
            }

            // 建立可以符合回傳的Cursor
            #region -- 非ArtworK類 的sql command --
            SqlCmd += string.Format(
                @"
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
		, LocalSuppID = rtrim(Order_TmsCost.LocalSuppID) 
		, Cost = Order_TmsCost.Price
		, costStitch = 1 
		, stitch = ard.stitch
		, unitprice = Order_TmsCost.Price
		, qtygarment = ard.QtyGarment
		, poqty = ard.ReqQty
		, Orders.SewInLine
		, [PriceApv] = iif(Order_TmsCost.ApvDate is null,'N','Y')
		, message = '' 
        , IsArtwork = 0
		, [ArtworkReq_DetailUkey] = ard.Ukey
		, [ArtworkReqID] = ar.ID
        , Orders.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
		,[Farmout] = ISNULL(FarmOut.Value,0)
		,[FarmIn] = ISNULL(FarmIn.Value,0)
FROM ArtworkReq_Detail ard WITH (NOLOCK) 
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = '{2}'  and ar.Status = 'Approved'
left join ArtworkReq_IrregularQty ai with (nolock) on ai.OrderID = ard.OrderID and ai.ArtworkTypeID = ar.ArtworkTypeID and ard.ExceedQty > 0
left join SubconReason sr with (nolock) on sr.Type = 'SQ' and sr.ID = ai.SubconReasonID
inner join Order_TmsCost WITH (NOLOCK) on ard.OrderID = Order_TmsCost.ID and Order_TmsCost.ArtworkTypeID = ar.ArtworkTypeID
inner join Orders WITH (NOLOCK) on orders.id = order_tmscost.id
inner join factory WITH (NOLOCK) on orders.factoryid = factory.id
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid = ard.OrderID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID 
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc =ard.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid = ard.OrderID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID 
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc =ard.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
WHERE 	ard.ArtworkPOID = '' and
        factory.mdivisionid = '{1}' 
		and factory.IsProduceFty = 1
		and orders.IsForecast = 0
		and (Orders.Junk=0 or Orders.Junk=1 and Orders.NeedProduction=1)
		and Order_TmsCost.localsuppid !=''		
        --and Orders.PulloutComplete = 0
		", this.poType, Sci.Env.User.Keyword, this.artworktype);
            SqlCmd += string.Format(" and Order_TmsCost.InhouseOSP = '{0}'", this.poType);
            switch (this.poType)
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

            if (!string.IsNullOrWhiteSpace(this.artworktype))
            {
                SqlCmd += string.Format(" and ar.ArtworkTypeID = '{0}'", this.artworktype);
            }

            if (!string.IsNullOrWhiteSpace(this.apvdate_b))
            {
                SqlCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", this.apvdate_b);
            }

            if (!string.IsNullOrWhiteSpace(this.apvdate_e))
            {
                SqlCmd += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", this.apvdate_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sciDelivery_b))
            {
                SqlCmd += string.Format("and  Orders.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!string.IsNullOrWhiteSpace(this.sciDelivery_e))
            {
                SqlCmd += string.Format("and  Orders.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                SqlCmd += string.Format(" and orders.ID between '{0}' and '{1}'", this.sp_b, this.sp_e);
            }

            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                SqlCmd += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            #endregion

            return SqlCmd;
        }

        private DualResult GetSpecialRecordData()
        {
            string sqlWhere = string.Empty;
            if (!(this.dateSCIDelivery.Value1 == null))
            {
                sqlWhere += string.Format(" and o.SciDelivery >= '{0}' ", this.sciDelivery_b);
            }

            if (!(this.dateSCIDelivery.Value2 == null))
            {
                sqlWhere += string.Format(" and o.SciDelivery <= '{0}' ", this.sciDelivery_e);
            }

            if (!(this.dateApproveDate.Value1 == null))
            {
                sqlWhere += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", this.apvdate_b);
            }

            if (!(this.dateApproveDate.Value2 == null))
            {
                sqlWhere += string.Format(" and ((ar.DeptApvDate < '{0}' and ar.Exceed = 0) or (ar.MgApvDate < '{0}' and ar.Exceed = 1)) ", this.apvdate_e);
            }

            if (!string.IsNullOrWhiteSpace(this.sp_b))
            {
                sqlWhere += string.Format("     and o.ID between '{0}' and '{1}'", this.sp_b, this.sp_e);
            }

            if (!MyUtility.Check.Empty(this.txtIrregularQtyReason.TextBox1.Text))
            {
                string whereReasonID = this.txtIrregularQtyReason.WhereString();
                sqlWhere += $@" and ai.SubconReasonID in ({whereReasonID})";
            }

            string sqlGetSpecialRecordData = $@"
Declare @sp1 varchar(16)= '{this.sp_b}'
Declare @sp2 varchar(16)= '{this.sp_e}'

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
            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlGetSpecialRecordData += $@" AND s.ArtworkTypeId='{this.artworktype}'";
            }

            if (!MyUtility.Check.Empty(this.sp_b))
            {
                sqlGetSpecialRecordData += $@" AND bdl.Orderid >= @sp1 ";
            }

            if (!MyUtility.Check.Empty(this.sp_e))
            {
                sqlGetSpecialRecordData += $@" AND bdl.Orderid <= @sp2";
            }

            sqlGetSpecialRecordData += $@"
select	Selected = 0 
		, o.FTYGroup
		, orderid = o.ID 
		, Styleid = rtrim(o.Styleid) 
		, SeasonID = rtrim(o.SeasonID) 
        , [Article] = (SELECT Stuff((select concat( ',',Article)   from Order_Article with (nolock) where ID = o.ID FOR XML PATH('')),1,1,'') )
		, o.ordertypeid
		, o.SciDelivery
		, ar.ArtworkTypeID
		, ArtworkID = ard.ArtworkID 
		, PatternCode = ard.PatternCode 
		, PatternDesc = ard.PatternDesc 
		, LocalSuppID = rtrim(ar.LocalSuppID) 
		, Cost = 0.0
		, costStitch = 1 
		, stitch = ard.stitch
		, unitprice = 0.0
		, qtygarment = ard.QtyGarment
		, poqty = ard.ReqQty
		, o.SewInLine
		, [PriceApv] = 'Y'
		, message = '' 
        , IsArtwork = 0
		, [ArtworkReq_DetailUkey] = ard.Ukey
		, [ArtworkReqID] = ar.ID
        , o.Category
        , [IrregularQtyReason] = sr.ID +'-'+sr.Reason
		,[Farmout] = ISNULL(FarmOut.Value,0)
		,[FarmIn] = ISNULL(FarmIn.Value,0)
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
OUTER APPLY(
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid = ard.OrderID 
	AND bd.ArtworkTypeId=ar.ArtworkTypeID 
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc =ard.PatternDesc
	AND bd.OutGoing IS NOT NULL 
)FarmOut
OUTER APPLY(	
	SELECT  [Value]= SUM( bd.QTY)
	FROM #Bundle bd
	WHERE bd.Orderid = ard.OrderID 
	AND bd.ArtworkTypeId = ar.ArtworkTypeID 
	AND bd.Patterncode = ard.PatternCode 
	AND bd.PatternDesc =ard.PatternDesc
	AND bd.InComing IS NOT NULL
)FarmIn
where  ar.ArtworkTypeID = '{this.artworktype}' and ar.Status = 'Approved' and  ard.ArtworkPOID = '' and
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
            #region 檢查Qty欄位是否可編輯
            string orderCategory = this.gridBatchCreateFromSubProcessData.GetDataRow(index)["Category"].ToString();

            if (orderCategory != "S" && this.isNeedPlanningB03Quote)
            {
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].ReadOnly = true;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Black;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.BackColor = Color.White; // Unit Price

                decimal unitPrice = (decimal)this.gridBatchCreateFromSubProcessData.GetDataRow(index)["unitprice"];
                if (unitPrice == 0)
                {
                    this.gridBatchCreateFromSubProcessData.Rows[index].Cells["Selected"].ReadOnly = true;
                    this.gridBatchCreateFromSubProcessData.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(229, 108, 126);
                    this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.BackColor = Color.FromArgb(229, 108, 126); // Unit Price
                }
            }
            else
            {
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].ReadOnly = false;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Red;
                this.gridBatchCreateFromSubProcessData.Rows[index].Cells["unitprice"].Style.BackColor = Color.Pink; // Unit Price
            }

            #endregion
        }
    }
}
