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
using System.Reflection;


namespace Sci.Production.Subcon
{
    public partial class P35 : Sci.Win.Tems.Input6
    {
        public P35(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            
            this.DefaultFilter = "mdivisionid = '" + Sci.Env.User.Keyword + "'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier.TextBox1.Text != this.txtsubconSupplier.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    CurrentMaintain["Paytermid"] = MyUtility.GetValue.Lookup("paytermid", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
                }
            };

        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"
select lapd.Localpoid	
	   , lapd.orderid	
	   , lapd.Refno	
	   , lapd.ThreadColorID	
	   , lapd.Qty	
	   , lapd.Unitid	
	   , lapd.price	
	   , inqty	
	   , apqty
	   , amount = lapd.price*lapd.Qty
	   , balance = inqty - apqty,inqty,apqty
	   , lapd.LocalPo_DetailUkey
	   , dbo.getItemDesc(localap.Category,lapd.Refno) as description 
	   , lapd.Ukey
from localap_detail lapd WITH (NOLOCK) 
left join localap WITH (NOLOCK) on localap.ID = lapd.ID
left join localpo_detail lpod on lpod.Ukey = localpo_detailukey
where lapd.id = '{0}'"
                , masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void txtartworktype_ftyCategory_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
            }
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["Status"] = "New";
            //((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["status"].ToString() == "Approved")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("Localap", "remark", CurrentMaintain);
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
                txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            if (CurrentMaintain["Category"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Category >  can't be empty!", "Warning");
                txtartworktype_ftyCategory.Focus();
                return false;
            }

            if (CurrentMaintain["CurrencyID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["CurrencyID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory.Focus();
                return false;
            }
            if (CurrentMaintain["PayTermid"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["PayTermid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Terms >  can't be empty!", "Warning");
                txtpayterm_ftyTerms.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty = 0"))
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
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LA", "LocalAP", (DateTime)CurrentMaintain["issuedate"]);
                if (MyUtility.Check.Empty(CurrentMaintain["id"]))
                {
                    MyUtility.Msg.WarningBox("Server is busy, Please re-try it again", "GetID() Failed");
                    return false;
                }
            }

            #region 加總明細金額至表頭
            //string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", CurrentMaintain["currencyId"]), null);
            //if (str == null || string.IsNullOrWhiteSpace(str))
            //{
            //    MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", CurrentMaintain["currencyID"]), "Warning");
            //    return false;
            //}
            //int exact = int.Parse(str);
            if (!get_exact()) {
                MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", CurrentMaintain["currencyID"]), "Warning");
                return false;
            }
            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(amount)", "");
            CurrentMaintain["amount"] = MyUtility.Math.Round((decimal)detail_a, exact);
            CurrentMaintain["vat"] = MyUtility.Math.Round((decimal)detail_a * (decimal)CurrentMaintain["vatrate"] / 100, exact);
            #endregion

            return base.ClickSaveBefore();
        }


        protected override void ClickSaveAfter()
        {
            //檢查localap ,localap_detail amount與vat在存檔之後是否有差異
            string chk_sql = string.Format(@"select la.amount,ld.detail_amount
                                from LocalAP la
                                inner join (select '{0}' as id,round(sum(a.price * a.qty),{1}) as detail_amount from localap_detail a 
                                 where a.id = '{0}') ld on la.id = ld.id
                                where la.id = '{0}' and la.amount <> ld.detail_amount", CurrentMaintain["ID"], exact);

            DataRow dr;
            if (MyUtility.Check.Seek(chk_sql, out dr, "")) {
                MyUtility.Msg.WarningBox(string.Format("Header Amount<{0}> and Detail Amount<{1}> are different,please check with MIS",dr["amount"],dr["detail_amount"]));
            }

            base.ClickSaveAfter();
        }


        string old_currencyID = "";
        int exact = 2;
        private bool get_exact() {
            if (!old_currencyID.Equals(CurrentMaintain["currencyID"].ToString())) {
                string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", CurrentMaintain["currencyId"]), null);
                if (str == null || string.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                exact = int.Parse(str);
                old_currencyID = CurrentMaintain["currencyID"].ToString();
            }
            return true;
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                if (e.Details.Columns.Contains("Amount"))
                    e.Details.Columns.Remove("Amount");
                e.Details.ColumnsDecimalAdd("Amount", 0, "Qty*Price");
                foreach (DataRow dr in e.Details.Rows)
                {
                    dr["description"] = Prgs.GetItemDesc(e.Master["category"].ToString(), dr["refno"].ToString());                    
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            #region -- 加總明細金額，顯示於表頭 --
            if (!(CurrentMaintain == null))
            {
                if (!(CurrentMaintain["amount"] == DBNull.Value) && !(CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
                    numTotal.Text = amount.ToString();
                }
                get_exact();
                decimal x = 0; decimal x1 = 0; decimal x2 = 0; decimal totalqty = 0;
                foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    x += (decimal)drr["amount"];
                    if (!MyUtility.Check.Empty(drr["Qty"].ToString()))
                        totalqty += (decimal)drr["Qty"];                  
                }
                x = MyUtility.Math.Round(x, 2);
                x2 = MyUtility.Math.Round(x * (decimal)CurrentMaintain["VatRate"] / 100, exact);
                x1 += x + x2;
                Console.WriteLine("get {0}", x);
                numAmount.Text = x.ToString();
                numTotal.Text = x1.ToString();
                numVat.Text = x2.ToString();
                numTotalqty.Text = totalqty.ToString();
            }
            #endregion

            txtsubconSupplier.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_ftyCategory.Enabled = !this.EditMode || IsDetailInserting;
            txtpayterm_ftyTerms.Enabled = !this.EditMode || IsDetailInserting;
            txtmfactory.Enabled = !this.EditMode || IsDetailInserting;

            #region Status Label
            label25.Text = CurrentMaintain["status"].ToString();
            #endregion

            #region Batch Import, Special record button
            btnBatchImport.Enabled = this.EditMode;
            #endregion

            this.detailgrid.AutoResizeColumn(0);
            this.detailgrid.AutoResizeColumn(1);
            this.detailgrid.AutoResizeColumn(2);
            this.detailgrid.AutoResizeColumn(3);
            this.detailgrid.AutoResizeColumn(5);
            this.detailgrid.AutoResizeColumn(6);
            this.detailgrid.AutoResizeColumn(7);
            this.detailgrid.AutoResizeColumn(8);
            this.detailgrid.AutoResizeColumn(9);
            this.detailgrid.AutoResizeColumn(10);
            this.detailgrid.AutoResizeColumn(11);
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["bundleno"] = "";
            CurrentDetailData["qty"] = 0;
        }

        // Detail Grid 設定 & Detail Vaild
        protected override void OnDetailGridSetup()
        {
            #region qtygarment Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {                   
                    decimal b = MyUtility.Check.Empty(CurrentDetailData["balance"]) ? 0 : MyUtility.Convert.GetDecimal(CurrentDetailData["balance"]);
                    if ((decimal)e.FormattedValue > b)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("can't over balance qty", "Warning");
                        return;
                    }
                    CurrentDetailData["Qty"] = e.FormattedValue;
                    CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Localpoid", header: "Local PO", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)   //1
            .Text("Refno", header: "Ref#", iseditingreadonly: true)   //2
            .Text("ThreadColorID", header: "Color Shade", width: Widths.AnsiChars(8), iseditingreadonly: true)    //3
            .Text("Description", header: "Description", iseditingreadonly: true)    //4
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6),settings:ns2)    //5
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true) //6
            .Numeric("price", header: "Price", width: Widths.AnsiChars(5), decimal_places: 4, integer_places: 4, iseditingreadonly: true)     //7
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 15, integer_places: 14)  //8
            .Numeric("inqty", header: "Accumulated" + Environment.NewLine + "Received Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //9
            .Numeric("apqty", header: "Accumulated" + Environment.NewLine + "Paid Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //10
            .Numeric("balance", header: "Balance Qty", width: Widths.AnsiChars(6), iseditingreadonly: true);    //11
            #endregion

            #region 可編輯欄位變色
            detailgrid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink; //qty
            #endregion
        }

        //Approve
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            //mantis 9749 confirm前先檢查此單狀態為NEW才可以confirm
            bool chk_status = MyUtility.Check.Seek(string.Format("select status from dbo.Localap  where id = '{0}' and status = 'New' ", CurrentMaintain["id"]));
            if (chk_status == false)
            {
                MyUtility.Msg.WarningBox(string.Format("This ID<{0}> is already Approved", CurrentMaintain["id"]));
                return;
            }

            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            
            #region 檢查apqty是否超過poqty
            ids = "";
            foreach (var dr1 in DetailDatas)
            {
                sqlcmd = string.Format("select * from Localpo_detail WITH (NOLOCK) where ukey = '{0}'", dr1["Localpo_detailukey"]);
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }
                if (datacheck.Rows.Count > 0)
                {
                    if ((decimal)dr1["qty"] + (decimal)datacheck.Rows[0]["apqty"] > (decimal)datacheck.Rows[0]["qty"]
                        || (decimal)dr1["qty"] + (decimal)datacheck.Rows[0]["apqty"] > (decimal)datacheck.Rows[0]["inqty"])
                    {
                        ids += string.Format("{0}-{1}-{2}-{3} is over Balance or PO qty"
                            , datacheck.Rows[0]["id"]
                            , datacheck.Rows[0]["orderid"]
                            , datacheck.Rows[0]["refno"]
                            , datacheck.Rows[0]["threadcolorid"]
                            ) + Environment.NewLine;
                    }
                }
            }
            if (!MyUtility.Check.Empty(ids))
            {
                MyUtility.Msg.WarningBox(ids);
                return;
            }
            #endregion
            #region 開始更新相關table資料
            sqlupd3 = string.Format("update Localap set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DataTable dtUpdateLocalPoData = new DataTable();
            dtUpdateLocalPoData.Columns.Add("ApQty");
            dtUpdateLocalPoData.Columns.Add("Ukey");

            foreach (DataRow drchk in DetailDatas)
            {
                DataRow drNewPoData = dtUpdateLocalPoData.NewRow();

                sqlcmd = string.Format(@"select b.Localpo_detailukey, sum(b.qty) qty
                                from Localap a , Localap_detail b 
                                where a.id = b.id  and a.status = 'Approved' and b.Localpo_detailukey ='{0}'
                                group by b.Localpo_detailukey ", drchk["Localpo_detailukey"]);

                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }

                if (datacheck.Rows.Count > 0)
                {
                    drNewPoData["ApQty"] = (decimal)datacheck.Rows[0]["qty"] + (decimal)drchk["qty"];
                    drNewPoData["Ukey"] = drchk["Localpo_detailukey"];
                    //sqlupd2 += string.Format("update Localpo_detail set apqty = {0}  where ukey = '{1}';"
                    //    + Environment.NewLine,(decimal)datacheck.Rows[0]["qty"] + (decimal)drchk["qty"], drchk["Localpo_detailukey"]);
                }
                else
                {
                    drNewPoData["ApQty"] = (decimal)drchk["qty"];
                    drNewPoData["Ukey"] = drchk["Localpo_detailukey"];
                    //sqlupd2 += string.Format("update Localpo_detail set APQty  = {0} where ukey = '{1}';"
                    //    + Environment.NewLine, (decimal)drchk["qty"], drchk["Localpo_detailukey"]);
                }
                dtUpdateLocalPoData.Rows.Add(drNewPoData);
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    DataTable dtResult;
                    string strUpdate = @"
update a
	set a.ApQty = b.ApQty
from Localpo_detail a 
inner join #tmp b on a.ukey = b.ukey";
                    if (!(result2 = MyUtility.Tool.ProcessWithDatatable(dtUpdateLocalPoData, null, strUpdate, out dtResult)))//DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
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
            
            #endregion
        }

        //unApprove
        protected override void ClickUnconfirm()
        {
            //mantis 9749 confirm前先檢查此單狀態為Approved才可以UNconfirm
            bool chk_status = MyUtility.Check.Seek(string.Format("select status from dbo.Localap  where id = '{0}' and status = 'Approved' ", CurrentMaintain["id"]));
            if (chk_status == false)
            {
                MyUtility.Msg.WarningBox(string.Format("This ID<{0}> is already Unapproved", CurrentMaintain["id"]));
                return;
            }

            //若VoucherID有值，則不能UNCONFIRM
            if (!MyUtility.Check.Empty(CurrentMaintain["VoucherID"]))
            {
                MyUtility.Msg.WarningBox("Voucher# already has value, can not unconfirm !!");
                return;
            }

            base.ClickUnconfirm();
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
   
            #region 開始更新相關table資料
            sqlupd3 = string.Format(@"update Localap set status='New',apvname='', apvdate = null , editname = '{0}' 
                                                , editdate = GETDATE()"+ "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            foreach (DataRow drchk in DetailDatas)
            {
                sqlcmd = string.Format(@"select b.Localpo_detailukey,sum(b.qty) qty
                                from Localap a  , Localap_detail b
                                where a.id = b.id  and a.status ='Approved' and b.Localpo_detailukey ='{0}'
                                group by b.Localpo_detailukey ", drchk["Localpo_detailukey"]);

                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }
                if (datacheck.Rows.Count > 0)
                {
                 
                     sqlupd2 += string.Format("update Localpo_detail set apqty = {0} where ukey = '{1}';"
                       + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] - (decimal)drchk["qty"], drchk["Localpo_detailukey"]);
                }
                else
                {
                    sqlupd2 += string.Format("update Localpo_detail set apqty = {0} where ukey = '{1}';"
                            + Environment.NewLine, 0m, drchk["Localpo_detailukey"]);
                }
            }
        
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnApprove successful");
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
            

            #endregion
        }

        // P35_Import
        private void btnBatchImport_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            
            if (MyUtility.Check.Empty(dr["Category"]))
            {
                MyUtility.Msg.WarningBox("Please fill Category first!");
                txtartworktype_ftyCategory.Focus();
                return;
            }

            if (MyUtility.Check.Empty(dr["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubconSupplier.TextBox1.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P35_Import(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);

            //foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
            //{
            //    DataRow tmp;
            //    if (MyUtility.Check.Seek(string.Format("select inqty,apqty from localpo_detail WITH (NOLOCK) where ukey = '{0}'", drr["localpo_detailukey"]), out tmp))
            //    {
            //        drr["inqty"] = tmp["inqty"];
            //        drr["apqty"] = tmp["apqty"];
            //        drr["balance"] = MyUtility.Convert.GetInt(tmp["inqty"]) - MyUtility.Convert.GetInt(tmp["apqty"]);
            //    }
            //}
        }
        
        protected override bool ClickNewBefore()
        {
            this.DetailSelectCommand = string.Format("select *,0.0 as amount,0.0 as balance,0 as inqty,0 as apqty,'' as description from localap_detail WITH (NOLOCK) where 1=0");
            return base.ClickNewBefore();
        }

        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",@"
select b.nameEn 
		,b.AddressEN
		,b.Tel
		,a.Id
        ,c.name
		,c.Address
		,c.Tel
		,a.PaytermID+d.Name [Terms]
		,a.InvNo [Invoice]
		,a.Remark [Remark]
		,e.AccountNo [AC_No]
        ,e.AccountName [AC_Name]
        ,e.BankName [Bank_Name]
        ,e.CountryID [Country]
        ,e.city [city] 
        ,e.swiftcode [SwiftCode]
        ,a.Handle+f.Name [Prepared_by]
        ,a.CurrencyID[CurrencyID]
		,a.VatRate[VatRate]
from dbo.LocalAP a WITH (NOLOCK) 
left join dbo.factory  b WITH (NOLOCK) on b.id = a.factoryid
inner join dbo.LocalSupp c WITH (NOLOCK) on c.id=a.LocalSuppID
left join dbo.PayTerm d WITH (NOLOCK) on d.id=a.PaytermID
left join dbo.LocalSupp_Bank e WITH (NOLOCK) on e.IsDefault=1 and e.id=a.LocalSuppID
left join dbo.Pass1 f WITH (NOLOCK) on f.id=a.Handle
left join dbo.Currency cr WITH (NOLOCK) on cr.ID = a.CurrencyID
where a.id = @ID"
                , pars, out dt);
            if (!result) { this.ShowErr(result); }
            string RptTitle = dt.Rows[0]["nameEn"].ToString();
            string address = dt.Rows[0]["AddressEN"].ToString();
            string Tel = dt.Rows[0]["Tel"].ToString();
            string Barcode = dt.Rows[0]["Id"].ToString();
            string Supplier = dt.Rows[0]["name"].ToString();
            string Address1 = dt.Rows[0]["Address"].ToString();
            string TEL1 = dt.Rows[0]["Tel"].ToString();
            string Terms = dt.Rows[0]["Terms"].ToString();
            string Invoice = dt.Rows[0]["Invoice"].ToString();
            string Remark = dt.Rows[0]["Remark"].ToString();
            string AC_No = dt.Rows[0]["AC_No"].ToString();
            string AC_Name = dt.Rows[0]["AC_Name"].ToString();
            string Bank_Name = dt.Rows[0]["Bank_Name"].ToString();
            string Country = dt.Rows[0]["Country"].ToString();
            string city = dt.Rows[0]["city"].ToString();
            string SwiftCode = dt.Rows[0]["SwiftCode"].ToString();
            string Total = numAmount.Text;
            string Vat = numVat.Text;
            string Grand_Total = (decimal.Parse(numAmount.Text) + decimal.Parse(numVat.Text)).ToString();
            string Prepared_by = dt.Rows[0]["Prepared_by"].ToString();
            string CurrencyID = dt.Rows[0]["CurrencyID"].ToString();
            string VatRate = dt.Rows[0]["VatRate"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("address", address));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", Barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address1", Address1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL1", TEL1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("id", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Terms", Terms));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", numTotalqty.Text.ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AC_No", AC_No));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AC_Name", AC_Name));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Bank_Name", Bank_Name));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Country", Country));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("city", city));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SwiftCode", SwiftCode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total", Total));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Vat", Vat));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Grand_Total", Grand_Total));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Prepared_by", Prepared_by));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyID", CurrencyID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VatRate", VatRate));
            #endregion


            #region  抓表身資料
            //pars = new List<SqlParameter>();
            //pars.Add(new SqlParameter("@ID", id));
            //DataTable dd;
            //result = DBProxy.Current.Select("",
            //@"select [SP] = a.OrderId 
            //         , [Description] = dbo.getItemDesc(b.Category,a.Refno)
            //         , ThreadColorID = isnull (a.ThreadColorID, 0)
            //         , [Price] = a.price
            //         , [Qty] = a.qty
            //         , [Unit] = a.unitid
            //         , [Amount] = format(CONVERT(decimal(10,2),a.price*a.Qty),'#,###,###,##0.00')
            //from dbo.LocalAP_Detail a WITH (NOLOCK) 
            //left join dbo.LocalAP b WITH (NOLOCK) on a.id=b.Id
            //where a.id= @ID", pars, out dd);
            //if (!result) { this.ShowErr(result); }

            // 傳 list 資料            
            List<P35_PrintData> data = DetailDatas.AsEnumerable()
                .Select(row1 => new P35_PrintData()
                {
                    LocalPO = row1["Localpoid"].ToString(),
                    SP = row1["orderid"].ToString(),
                    Description = row1["Description"].ToString(),
                    ThreadColorID = row1["ThreadColorID"].ToString(),
                    Price = row1["Price"].ToString(),
                    Qty = row1["Qty"].ToString(),
                    Unit = row1["Unitid"].ToString(),
                    Amount = row1["Amount"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P35_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P35_Print.rdlc";

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
