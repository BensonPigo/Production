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
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Win;


namespace Sci.Production.Subcon
{
    public partial class P30 : Sci.Win.Tems.Input6
    {

        public P30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "mdivisionid = '" + Sci.Env.User.Keyword + "'";
            
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            this.txtsubcon1.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon1.TextBox1.Text != this.txtsubcon1.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                    {
                        dr.Delete();
                    }
                }
            };

        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab) && !MyUtility.Check.Empty(CurrentMaintain))
            {
                this.toolbar.cmdConfirm.Enabled = !this.EditMode && Sci.Production.PublicPrg.Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["status"].ToString() == "New";
                this.toolbar.cmdUnconfirm.Enabled = !this.EditMode && Sci.Production.PublicPrg.Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["status"].ToString() == "Approved";
            }
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["Status"] = "New";
            //((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }

            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = dr["id"].ToString();

            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            paras.Add(sp1);

            string sqlcmd;
            sqlcmd = "select fd.ID from LocalPO_Detail ad, LocalAP_Detail fd where ad.Ukey = fd.LocalPo_DetailUkey and ad.id = @id";

            DataTable dt;
            DBProxy.Current.Select(null, sqlcmd, paras, out dt);
            if (dt.Rows.Count > 0)
            {
                string ids = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids += dt.Rows[i][0].ToString() + ";";
                }
                MyUtility.Msg.WarningBox(string.Format("Below AP {0} refer to details data, can't delete.", ids), "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "APPROVED")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("Localpo", "remark", dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {

            #region 必輸檢查
            if (CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["LocalSuppID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                txtsubcon1.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            if (CurrentMaintain["Category"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Category >  can't be empty!", "Warning");
                txtartworktype_fty1.Focus();
                return false;
            }

            if (CurrentMaintain["CurrencyID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["CurrencyID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory1.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty = 0 or qty = null"))
            {
                row.Delete();
            }
            

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'LocalPO1'), 'LocalPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(factorykeyword + "LP", "Localpo", (DateTime)CurrentMaintain["issuedate"]);
            }

            #region 加總明細金額至表頭
            string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency where id = '{0}'", CurrentMaintain["currencyId"]), null);
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", CurrentMaintain["currencyID"]), "Warning");
                return false;
            }
            int exact = int.Parse(str);
            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(amount)", "");
            CurrentMaintain["amount"] = MyUtility.Math.Round((decimal)detail_a, exact);
            CurrentMaintain["vat"] = MyUtility.Math.Round((decimal)detail_a * (decimal)CurrentMaintain["vatrate"] / 100, exact);
            #endregion

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                //(e.Details).Columns.Add("factoryid", typeof(String));
                //(e.Details).Columns.Add("sewinline", typeof(DateTime));
                //(e.Details).Columns.Add("description", typeof(String));
                //(e.Details).Columns.Add("Amount", typeof(decimal));
                (e.Details).Columns["amount"].Expression = "price * qty";

            //    foreach (DataRow dr in e.Details.Rows)
            //    {
            //        //dr["Price"] = (Decimal)dr["unitprice"] * (Decimal)dr["qtygarment"];
            //        //DataTable order_dt;
            //        //DBProxy.Current.Select(null, string.Format("select factoryid, sewinline, scidelivery from orders where id='{0}'", dr["orderid"].ToString()), out order_dt);
            //        //if (order_dt.Rows.Count > 0)
            //        //{
            //        //    dr["factoryid"] = order_dt.Rows[0]["factoryid"].ToString();
            //        //    dr["sewinline"] = order_dt.Rows[0]["sewinline"];
            //        //}
            //        dr["description"] = Prgs.GetItemDesc(e.Master["category"].ToString(), dr["refno"].ToString());
            //    }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            
            if (!(CurrentMaintain == null))
            {
                if (!(CurrentMaintain["amount"] == DBNull.Value) && !(CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
                    numericBox4.Text = amount.ToString();
                }
            }
            txtsubcon1.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_fty1.Enabled = !this.EditMode || IsDetailInserting;
            txtmfactory1.Enabled = !this.EditMode || IsDetailInserting;
            #region Status Label
            label25.Text = CurrentMaintain["status"].ToString();
            #endregion
            
            #region Batch Import, Special record button
            button4.Enabled = this.EditMode;
            #endregion

        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["qty"] = 0;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP# Vaild 判斷此sp#的cateogry存在 order_tmscost
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            DataRow dr;
            ts4.CellValidating += (s, e) =>
            {
                if (!this.EditMode && (CurrentMaintain["status"].ToString().ToUpper() == "Approved"))
                {
                    if (MyUtility.Check.Seek(string.Format("select price from order_tmscost where id = '{0}' and artworktypeid = '{1}'", e.FormattedValue, CurrentMaintain["categor"]), out dr, null))
                    {
                        if ((decimal)dr["price"] == 0m)
                        {
                            MyUtility.Msg.WarningBox("TmsCost price is Zero","Warning");
                            e.Cancel=true;
                            return;
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("SP# is not in Order_TmsCost","Data not found");
                        e.Cancel = true;
                        return;
                    }
                }
                if (MyUtility.Check.Seek(string.Format("select factoryid, sewinline from orders where id = '{0}'", e.FormattedValue), out dr, null))
                {
                    //CurrentDetailData["factoryid"] = dr["factoryid"];
                    //CurrentDetailData["sewinline"] = dr["sewinline"];
                    CurrentDetailData["orderid"] = e.FormattedValue;
                }
            };
            #endregion

            #region Refno 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                        (string.Format(@"Select refno,description, localsuppid,unitid,price 
                                                    from localItem where category ='{0}' and  localsuppid = '{1}' order by refno",CurrentMaintain["category"],CurrentMaintain["localsuppid"])
                                                                                                                                 , "20,30,10,10,10",null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    IList<DataRow> x = item.GetSelecteds();
                    CurrentDetailData["refno"] = x[0][0];
                    CurrentDetailData["unitid"] = x[0][3];
                    CurrentDetailData["price"] = decimal.Parse(x[0][4].ToString());
                }
            };
            ts.CellValidating += (s, e) =>
                {
                    if (MyUtility.Check.Empty(e.FormattedValue) || !this.EditMode) return;
                    if (!MyUtility.Check.Seek(string.Format(@"select refno,unitid,price from localitem 
                                                                      where refno = '{0}' and category ='{1}'and localsuppid = '{2}'"
                                                                    ,e.FormattedValue.ToString(),CurrentMaintain["category"],CurrentMaintain["localsuppid"])
                                                                    ,out dr,null))
                    {
                        MyUtility.Msg.WarningBox("Data not found!","Ref#");
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        CurrentDetailData["refno"] = dr[0];
                        CurrentDetailData["unitid"] = dr[1];
                        CurrentDetailData["price"] = dr[2];
                    }
                };
            #endregion

            #region Color shase 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
               
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                     if (!(CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "SP_THREAD" 
                        || CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "EMB_THREAD"))
                        return;
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                        (@"Select ID,description  from threadcolor where JUNK=0 order by ID" , "20,60", null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["Threadcolorid"] = item.GetSelectedString();
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue) || !(CurrentMaintain["category"].ToString().ToUpper() == "SP_THREAD" || CurrentMaintain["category"].ToString().ToUpper() == "EMB_THREAD"))
                {
                    //e.Cancel = true;
                    return;
                }
                if (!MyUtility.Check.Seek(string.Format(@"select junk from threadcolor 
                                                                      where id = '{0}' and junk=0 "
                                                                , e.FormattedValue.ToString())
                                                                , out dr, null))
                {
                    MyUtility.Msg.WarningBox("Data not found!","Color Shade");
                    e.Cancel = true;
                    return;
                }
                
            };
            #endregion

            #region Qty Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    //CurrentDetailData["amount"] = (decimal)CurrentDetailData["price"] * (decimal)e.FormattedValue;
                    CurrentDetailData["qty"] = e.FormattedValue;
                }
            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("factoryid", header: "Order Factory", iseditingreadonly: true)  //0
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), settings: ts4)  //1
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10), iseditingreadonly: true)   //2
            .Text("refno", header: "Ref#", width: Widths.AnsiChars(20),settings:ts)    //3
            .Text("threadColorid", header: "Color Shade",settings:ts2)    //4
             .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)   //5
             .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 0, integer_places: 6, settings: ns)    //6
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)   //7
            .Numeric("price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 4, iseditingreadonly: true)     //8
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(9), iseditingreadonly: true, decimal_places: 4, integer_places: 14)   //9
            .Date("delivery", header: "Delivery", width: Widths.AnsiChars(10))   //10
            .Text("Requestid", header: "Request ID", width: Widths.AnsiChars(13), iseditingreadonly: true) //11
            .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(6), decimal_places: 0, integer_places: 6, iseditingreadonly: true) //12
            .Numeric("apqty", header: "AP Qty", width: Widths.AnsiChars(6), decimal_places: 0, integer_places: 6, iseditingreadonly: true) //13
            .Text("remark", header: "Remark", width: Widths.AnsiChars(25)) 
            ;     
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;  
            detailgrid.Columns[3].DefaultCellStyle.BackColor = Color.Pink;  
            detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink; 
            detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink; 
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink; 
            #endregion
        }

        // import thread or carton request
        private void button4_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (MyUtility.Check.Empty(dr["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubcon1.TextBox1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(dr["category"]))
            {
                MyUtility.Msg.WarningBox("Please fill Localtype first!");
                txtartworktype_fty1.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P30_Import(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    dr.Delete();
                }
            }
        }

        //Approve
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain; 
            if (null == dr) return;

            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查明細requestid是否已有回寫poid
            sqlcmd = string.Format(@"select requestid,LocalPOID from LocalPO_Detail, packinglist 
                                                    where localpo_detail.requestid = packinglist.id 
                                                            and LocalPOID!='' 
                                                            and localpo_detail.id ='{0}' group by requestid,LocalPOID ", CurrentMaintain["id"]);
            if(!(result2 = DBProxy.Current.Select(null, sqlcmd,out datacheck)))
            {
                ShowErr(sqlcmd,result2);
                return;
            }
            if (MyUtility.Check.Empty(datacheck) && datacheck.Rows.Count > 0)
            {
                foreach (DataRow tmp in datacheck.Rows)
                {
                    ids += string.Format("Request ID: {0} is already in LocalPO : {1}" + Environment.NewLine, tmp[0], tmp[1]);
                }
                MyUtility.Msg.WarningBox("Below request id already be created in Local PO, can't approve it!!" + Environment.NewLine + ids, "Warning");
                return;
            }
            #endregion

            #region 開始更新相關table資料
            sqlupd2 = string.Format(@"with MyCTE 
                                                    as
                                                    (
                                                        select requestid 
                                                        from localpo_detail
                                                        where localpo_detail.id = '{0}' group by requestid
                                                    )
                                                    update dbo.PackingList
                                                    set LocalPOID = '{0}' 
                                                    where id in (select requestid from mycte)", dr["id"]);

            sqlupd3 = string.Format("update Localpo set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    } 
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Approve successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
            #endregion
        }

        //unApprove
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable dt = (DataTable)detailgridbs.DataSource;
            DataRow[] drs = dt.Select("apqty > 0");
            if (drs.Length != 0)
            {
                MyUtility.Msg.WarningBox("Detail data has AP Qty, can't unApprove!", "Warning");
                return;
            }
            
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlupd2 = "", sqlupd3 = "";
            DualResult result, result2;
            

            #region 開始更新相關table資料
            sqlupd2 = string.Format(@"with MyCTE 
                                                    as
                                                    (
                                                        select requestid 
                                                        from localpo_detail
                                                        where localpo_detail.id = '{0}' group by requestid
                                                    )
                                                    update dbo.PackingList
                                                    set LocalPOID = '' 
                                                    where id in (select requestid from mycte)", dr["id"]);

            sqlupd3 = string.Format("update Localpo set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            sqlupd3 = string.Format(@"update Localpo set status='New',apvname='', apvdate = null , editname = '{0}' 
                                                    , editdate = GETDATE() where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    } 

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnApprove successful");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();

            #endregion
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select * ,0.0 as amount,orders.factoryid,orders.sewinline,localitem.description
                                                        from localpo_detail 
                                                            inner join orders on localpo_detail.orderid = orders.id
                                                            inner join localitem on localitem.refno = localpo_detail.refno 
                                                        Where localpo_detail.id = '{0}' order by orderid,localpo_detail.refno,threadcolorid ", masterID);

            return base.OnDetailSelectCommandPrepare(e);

        }

        protected override bool ClickNewBefore()
        {
            this.DetailSelectCommand = string.Format(@"select * ,0.0 as amount,orders.factoryid,orders.sewinline,localitem.description
                                                        from localpo_detail 
                                                            inner join orders on localpo_detail.orderid = orders.id
                                                            inner join localitem on localitem.refno = localpo_detail.refno 
                                                        where 1=2 order by orderid,localpo_detail.refno,threadcolorid ");
            return base.ClickNewBefore();
        }

        protected override bool ClickPrint()
        {

            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select    
             b.NameEN[RptTitle]
	        ,a.LocalSuppID+'-'+c.Name[Supplier]
	        ,c.Tel[Tel]
	        ,c.Address[Address]
            from dbo.localpo a 
            inner join dbo.factory  b 
            on b.id = a.factoryid
	        left join dbo.LocalSupp c
	        on c.id=a.LocalSuppID
            where b.id = a.factoryid
            and a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string RptTitle = dt.Rows[0]["RptTitle"].ToString();
            string Supplier = dt.Rows[0]["Supplier"].ToString();
            string Tel = dt.Rows[0]["Tel"].ToString();
            string Address = dt.Rows[0]["Address"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address", Address));           

            #endregion
            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select("",
            @"select a.POID,a.Seq1+'-'+a.seq2 as SEQ
	         ,a.Roll,a.Dyelot
	         ,dbo.getMtlDesc(a.poid,a.seq1,a.Seq2,2,0) [DESC]
			 ,CASE stocktype
			  WHEN 'B' THEN 'Bulk'
			  WHEN 'I' THEN 'Inventory'
			  WHEN 'O' THEN 'Scrap'
			  ELSE stocktype
			  END
			  stocktype
		     ,unit = b.StockUnit
		     ,a.Qty
		     ,dbo.Getlocation(a.FtyInventoryUkey)[Location]
             ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 ) 	        
             from dbo.TransferOut_Detail a 
             LEFT join dbo.PO_Supp_Detail b
             on 
             b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
             where a.id= @ID", pars, out dd);
            if (!result) { this.ShowErr(result); }

            // 傳 list 資料            
            List<P19_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P19_PrintData()
                {
                    POID = row1["POID"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    Roll = row1["Roll"].ToString(),
                    Dyelot = row1["Dyelot"].ToString(),
                    DESC = row1["DESC"].ToString(),
                    stocktype = row1["stocktype"].ToString(),
                    unit = row1["unit"].ToString(),
                    QTY = row1["QTY"].ToString(),
                    Location = row1["Location"].ToString(),
                    Total = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P19_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P19_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion
            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;

        }
    }
}
