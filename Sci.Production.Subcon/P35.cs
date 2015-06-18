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


namespace Sci.Production.Subcon
{
    public partial class P35 : Sci.Win.Tems.Input6
    {

        public P35(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            this.txtsubcon1.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon1.TextBox1.Text != this.txtsubcon1.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = myUtility.Lookup("CurrencyID", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    CurrentMaintain["Paytermid"] = myUtility.Lookup("paytermid", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();
                }
            };

        }

        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
            }
        }

        // 新增時預設資料
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["Status"] = "New";
            ((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查
        protected override bool OnDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString().ToUpper() == "APPROVED")
            {
                myUtility.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }
            return base.OnDeleteBefore();
        }

        // edit前檢查
        protected override bool OnEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString() == "Approved")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("Localap", "remark", dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.OnEditBefore();
        }

        // save前檢查 & 取id
        protected override bool OnSaveBefore()
        {
            #region 必輸檢查
            if (CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["LocalSuppID"].ToString()))
            {
                myUtility.WarningBox("< Suppiler >  can't be empty!", "Warning");
                txtsubcon1.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                myUtility.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }



            if (CurrentMaintain["Category"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                myUtility.WarningBox("< Category >  can't be empty!", "Warning");
                txtartworktype_fty1.Focus();
                return false;
            }

            if (CurrentMaintain["CurrencyID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["CurrencyID"].ToString()))
            {
                myUtility.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                myUtility.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuser1.TextBox1.Focus();
                return false;
            }
            #endregion

            if (DetailDatas.Count == 0)
            {
                myUtility.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'LocalPO1'), 'LocalPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                CurrentMaintain["id"] = Sci.myUtility.GetID(ProductionEnv.Keyword + "LA", "LocalAP", (DateTime)CurrentMaintain["issuedate"]);
                if (myUtility.Empty(CurrentMaintain["id"]))
                {
                    myUtility.WarningBox("Server is busy, Please re-try it again", "GetID() Failed");
                    return false;
                }
            }

            #region 加總明細金額至表頭
            string str = myUtility.Lookup(string.Format("Select exact from Currency where id = '{0}'", CurrentMaintain["currencyId"]), null);
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                myUtility.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", CurrentMaintain["currencyID"]), "Warning");
                return false;
            }
            int exact = int.Parse(str);
            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(amount)", "");
            CurrentMaintain["amount"] = myUtility.Round((decimal)detail_a, exact);
            CurrentMaintain["vat"] = myUtility.Round((decimal)detail_a * (decimal)CurrentMaintain["vatrate"] / 100, exact);
            #endregion

            return base.OnSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                (e.Details).Columns.Add("amount", typeof(decimal));
                (e.Details).Columns["amount"].Expression = "price * qty";
                (e.Details).Columns.Add("balance", typeof(decimal));
                (e.Details).Columns.Add("inqty", typeof(decimal));
                (e.Details).Columns.Add("apqty", typeof(decimal));
                (e.Details).Columns.Add("description", typeof(string));

                foreach (DataRow dr in e.Details.Rows)
                {
                    DataRow tmp;
                    if (myUtility.Seek(string.Format("select inqty,apqty from localpo_detail where ukey = '{0}'", dr["localpo_detailukey"]), out tmp))
                    {
                        dr["inqty"] = tmp["inqty"];
                        dr["apqty"] = tmp["apqty"];
                        dr["balance"] = (decimal)dr["inqty"] - (decimal)dr["apqty"];
                    }
                    
                    dr["description"] = Prgs.GetItemDesc(e.Master["category"].ToString(), dr["refno"].ToString());
                    
                }
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
            txtpayterm_fty1.Enabled = !this.EditMode || IsDetailInserting;

            #region Status Label
            label25.Text = CurrentMaintain["status"].ToString();
            #endregion

            #region Batch Import, Special record button
            button4.Enabled = this.EditMode;

            #endregion

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
                    if ((decimal)e.FormattedValue > (decimal)CurrentDetailData["balance"] )
                    {
                        myUtility.WarningBox("can't over balance qty", "Warning");
                        e.Cancel = true;
                        return;
                    }
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
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14)  //8
            .Numeric("inqty", header: "Accumulated" + Environment.NewLine + "Received Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //9
            .Numeric("apqty", header: "Accumulated" + Environment.NewLine + "Paid Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //10
            .Numeric("balance", header: "Balance Qty", width: Widths.AnsiChars(6), iseditingreadonly: true);    //11
            #endregion

            #region 可編輯欄位變色
            detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink; //qty
            #endregion
        }

        //Approve
        protected override void OnConfirm()
        {
            base.OnConfirm();
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            #region 檢查po是否close了。
            sqlcmd = string.Format(@"select a.id from Localpo a, Localap_detail b 
                            where a.id = b.Localpoid and a.status = 'Closed' and b.id = '{0}'", CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow drchk in datacheck.Rows)
                {
                    ids += drchk[0].ToString() + ",";
                }
                myUtility.WarningBox(String.Format("These POID <{0}> already closed, can't Approve it", ids));
                return;
            }
            #endregion
            #region 檢查apqty是否超過poqty
            ids = "";
            foreach (var dr1 in DetailDatas)
            {
                sqlcmd = string.Format("select * from Localpo_detail where ukey = '{0}'", dr1["Localpo_detailukey"]);
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
            if (!myUtility.Empty(ids))
            {
                myUtility.WarningBox(ids);
                return;
            }
            #endregion
            #region 開始更新相關table資料
            sqlupd3 = string.Format("update Localap set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


            foreach (DataRow drchk in DetailDatas)
            {
                sqlcmd = string.Format(@"select b.Localpo_detailukey, sum(b.qty) qty
                                from Localap a, Localap_detail b
                                where a.id = b.id  and a.status = 'Approved' and b.Localpo_detailukey ='{0}'
                                group by b.Localpo_detailukey ", drchk["Localpo_detailukey"]);

                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }

                if (datacheck.Rows.Count > 0)
                {
                    sqlupd2 += string.Format("update Localpo_detail set apqty = {0} where ukey = '{1}';"
                        + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] + (decimal)drchk["qty"], drchk["Localpo_detailukey"]);
                }
                else
                {
                    sqlupd2 += string.Format("update Localpo_detail set apqty = {0} where ukey = '{1}';"
                        + Environment.NewLine, (decimal)drchk["qty"], drchk["Localpo_detailukey"]);
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    myUtility.InfoBox("Approve successful");
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
            this.EnsureToolbarCUSR();
            #endregion
        }

        //unApprove
        protected override void OnUnconfirm()
        {
            base.OnUnconfirm();
            DialogResult dResult = myUtility.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            #region 檢查po是否close了。
            sqlcmd = string.Format(@"select a.id from Localpo a, Localap_detail b 
                            where a.id = b.Localpoid and a.status = 'Closed' and b.id = '{0}'", CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow drchk in datacheck.Rows)
                {
                    ids += drchk[0].ToString() + ",";
                }
                myUtility.WarningBox(String.Format("These POID <{0}> already closed, can't UnApprove it", ids));
                return;
            }
            #endregion


            #region 開始更新相關table資料

            sqlupd3 = string.Format("update Localap set status='New',apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE() " +
                               "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


            foreach (DataRow drchk in DetailDatas)
            {
                sqlcmd = string.Format(@"select b.Localpo_detailukey, sum(b.qty) qty
                                from Localap a, Localap_detail b
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
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        ShowErr(sqlupd2, result);
                        return;
                    }

                    _transactionscope.Complete();
                    myUtility.InfoBox("UnApprove successful");
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
            this.EnsureToolbarCUSR();

            #endregion
        }

        // P35_Import
        private void button4_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            
            if (myUtility.Empty(dr["Category"]))
            {
                myUtility.WarningBox("Please fill Category first!");
                txtartworktype_fty1.Focus();
                return;
            }

            if (myUtility.Empty(dr["localsuppid"]))
            {
                myUtility.WarningBox("Please fill Supplier first!");
                txtsubcon1.TextBox1.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P35_Import(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (this.EditMode) return;
            var frm = new Sci.Production.Subcon.P01_BatchCreate("P01");
            frm.ShowDialog(this);
            ReloadDatas();
        }

        private void txtartworktype_fty1_Validating(object sender, CancelEventArgs e)
        {

        }




    }
}
