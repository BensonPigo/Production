using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Win;
using System.Transactions;
namespace Sci.Production.Miscellaneous
{
    public partial class P40 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        private int exact = 2;
        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            string defaultfilter = string.Format("factoryid = '{0}'", factory);
            this.DefaultFilter = defaultfilter;
            this.InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
            @"Select a.* ,b.description,b.Unitid,e.projectid,c.Purchasetype,b.Inspect,
            iif(b.Inspect=1,a.InspQty -a.ApQty,a.InQty-a.ApQty) as balance,
            a.Qty*a.Price as amount
            From MiscAp_Detail a 
            Left join Misc b on a.miscid = b.id 
            Left join Miscpo_Detail e on e.id = a.Miscpoid and e.seq1 = a.seq1 and e.seq2 = a.seq2 
            Left join MiscReq c on c.id = a.Miscreqid
            Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings QtyCell = new DataGridViewGeneratorNumericColumnSettings();
            QtyCell.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1 || !this.EditMode) return;
                int index = e.RowIndex;
                if (e.FormattedValue == null) return;
                CurrentDetailData["Amount"] = (decimal)e.FormattedValue * (decimal)CurrentDetailData["Price"];
                CurrentDetailData["Qty"] = (decimal)e.FormattedValue;
                CurrentDetailData.EndEdit();
            };
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("MiscPOID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("MiscReqid", header: "Miscellaneous Req#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("PurchaseType", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Projectid", header: "Project Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("InQty", header: "Accu. In Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("ApQty", header: "Accu. Paid Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .CheckBox("Inspect", header: "Need to Inspect", iseditable: false)
            .Numeric("InspQty", header: "Accu. Insp. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, settings: QtyCell)
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("Price", header: "Price", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 4, iseditingreadonly: true);
            detailgrid.Columns[13].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.button2.Enabled = !this.EditMode && CurrentMaintain["Status"].ToString() == "New";

            this.label10.Text = CurrentMaintain["Status"].ToString();

            if (!string.IsNullOrWhiteSpace(CurrentMaintain["Amount"].ToString()))
            {
                this.numericBox4.Value = (decimal)CurrentMaintain["Amount"] + (decimal)CurrentMaintain["Vat"];
            }
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string reApQtyStr;
            DataRow apDr;
            List<String> reApStrList = new List<String>();
            DataTable errTable = new DataTable();
            errTable.Columns.Add("MiscPOID", typeof(string));
            errTable.Columns.Add("SEQ1", typeof(string));
            errTable.Columns.Add("SEQ2", typeof(string));
            errTable.Columns.Add("ApQty", typeof(decimal));
            errTable.Columns.Add("InQty", typeof(decimal));
            errTable.Columns.Add("InspQty", typeof(decimal));
            errTable.Columns.Add("Inspect", typeof(bool));
             bool showmsg1 = false;

            foreach(DataRow dr in this.DetailDatas)
            {
                reApQtyStr = string.Format("Select ApQty,InQty,InspQty,Inspect from Miscpo_Detail a Left join Misc b on b.id = a.miscid where a.id ='{0}' and seq1 = '{1}' and seq2 = '{2}' ", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                if (MyUtility.Check.Seek(reApQtyStr, out apDr))
                {
                    if ((apDr["Inspect"].ToString() == "False" && (decimal)apDr["ApQty"] + (decimal)dr["Qty"] > (decimal)apDr["InQty"]) || (apDr["Inspect"].ToString() == "True" && (decimal)apDr["ApQty"] + (decimal)dr["Qty"] > (decimal)apDr["InspQty"]))
                    {
                        showmsg1 = true;
                        DataRow edr = errTable.NewRow();
                        edr["MiscPOID"] = dr["MiscPOID"];
                        edr["SEQ1"] = dr["SEQ1"];
                        edr["SEQ2"] = dr["SEQ2"];
                        edr["ApQty"] = apDr["ApQty"];
                        edr["InQty"] = apDr["InQty"];
                        edr["InspQty"] = apDr["InspQty"];
                        edr["Inspect"] = apDr["Inspect"];
                        errTable.Rows.Add(edr);
                    }
                    else
                    {
                        reApQtyStr = string.Format("upDate MiscAp_Detail set apQty = {4}, inQty={5},inspQty = {7} where id ='{0}' and Miscpoid = '{1}' and seq1 = '{2}' and seq2 = '{3}';update MiscPO_Detail set apQty = apQty+{6} where id = '{1}' and seq1 = '{2}' and seq2 = '{3}'", CurrentMaintain["ID"].ToString(), dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString(), apDr["ApQty"], apDr["inQty"],dr["Qty"],apDr["InspQty"]);
                        reApStrList.Add(reApQtyStr);
                    }
                }
            }
            if (showmsg1)
            {
                MyUtility.Msg.ShowMsgGrid(errTable, "The <Acc. Ap Qty> can not exceed <In Qty> or <Insp Qty>.");
                return;
            }
            string updSql = string.Format("update MiscAp set Approve = '{0}',ApproveDate = GETDATE(),Status = 'Approved',editname='{0}',editDate = GETDATE()", loginID);

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    for (int i = 0; i < reApStrList.Count; i++)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, reApStrList[i])))
                        {
                            ShowErr(reApStrList[i], upResult);
                            return;
                        }
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        ShowErr(updSql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
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
            EnsureToolbarExt();
        }
        protected override void ClickUnconfirm()
        {

            if (!MyUtility.Check.Empty(CurrentMaintain["TransID"]))
            {
                MyUtility.Msg.WarningBox("The Ap already transfered to A/C, you can't unapprove.");
                return;
            }

            base.ClickUnconfirm();
            string reApQtyStr;
            DataRow apDr;
            List<String> reApStrList = new List<String>();

            foreach (DataRow dr in this.DetailDatas)
            {
                reApQtyStr = string.Format("Select ApQty,InQty,InspQty,Inspect from Miscpo_Detail a Left join Misc b on b.id = a.miscid where a.id ='{0}' and seq1 = '{1}' and seq2 = '{2}' ", dr["MiscpoID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                if (MyUtility.Check.Seek(reApQtyStr, out apDr))
                {

                    reApQtyStr = string.Format("upDate MiscAp_Detail set apQty = {4}, inQty={5},inspQty = {7} where id ='{0}' and Miscpoid = '{1}' and seq1 = '{2}' and seq2 = '{3}';update MiscPO_Detail set apQty = apQty-{6} where id = '{1}' and seq1 = '{2}' and seq2 = '{3}'", CurrentMaintain["ID"].ToString(), dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString(), (decimal)apDr["ApQty"] - (decimal)dr["Qty"], apDr["inQty"], dr["Qty"],apDr["inspQty"]);
                    reApStrList.Add(reApQtyStr);
                }
            }
            string updSql = string.Format("update MiscAp set Approve = '',ApproveDate = null ,Status = 'New',editname='{0}',editDate = GETDATE()", loginID);

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    for (int i = 0; i < reApStrList.Count; i++)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, reApStrList[i])))
                        {
                            ShowErr(reApStrList[i], upResult);
                            return;
                        }
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        ShowErr(updSql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
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
            EnsureToolbarExt();
            

        }
        //新增之後給初始值
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Factoryid"] = factory;
            CurrentMaintain["cdate"] = DateTime.Today;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Handle"] = loginID;
            CurrentMaintain["Vat"] = 0;
            CurrentMaintain["Vatrate"] = 0;
            CurrentMaintain["Amount"] = 0;
            this.numericBox4.Value = 0;
        }

        protected override void OnDetailUIConvertToMaintain()
        {
            base.OnDetailUIConvertToMaintain();
            this.txtuser1.TextBox1.ReadOnly = true;
            this.txtuser2.TextBox1.ReadOnly = true;

        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Junked")
            {
                MyUtility.Msg.WarningBox("The record status was junked, you can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (CurrentMaintain["Status"].ToString() == "Approved")
            {
                this.detailgrid.IsEditingReadOnly = true;
                this.dateBox1.ReadOnly = true;
                this.txtsubcon1.TextBox1.ReadOnly = true;
                this.txtuser1.TextBox1.ReadOnly = true;
                this.txtuser2.TextBox1.ReadOnly = true;
                this.numericBox2.ReadOnly = true;
                this.txtpayterm_fty1.TextBox1.ReadOnly = true;
                this.button1.Enabled = false;
            }
            else
            {
                string reApQtyStr;
                DataRow apDr;
                foreach (DataRow dr in this.DetailDatas)
                {
                    reApQtyStr = string.Format("Select ApQty,InQty,InspQty from Miscpo_Detail a where a.id ='{0}' and seq1 = '{1}' and seq2 = '{2}' ", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                    if (MyUtility.Check.Seek(reApQtyStr, out apDr))
                    {
                        dr["ApQty"] = apDr["ApQty"];
                        dr["InQty"] = apDr["InQty"];
                        dr["InspQty"] = apDr["InspQty"];
                    }
                }
            }
            this.gridicon.Append.Enabled = false;
            this.gridicon.Insert.Enabled = false;

        }
        protected override bool ClickSaveBefore()
        {
            //確認不可空白欄位
            decimal amount = 0;
            if (MyUtility.Check.Empty(CurrentMaintain["Cdate"]))
            {
                MyUtility.Msg.WarningBox("<A/P Date> can not be empty.");
                this.dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppid"]))
            {
                MyUtility.Msg.WarningBox("<Supplier> can not be empty.");
                this.txtsubcon1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("<Handle> can not be empty.");
                this.txtuser1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PaytermId"]))
            {
                MyUtility.Msg.WarningBox("<Term> can not be empty.");
                this.txtpayterm_fty1.TextBox1.Focus();
                return false;
            }


            #region 填入表頭Amount, Vat 
            foreach (DataRow dr in this.DetailDatas)
            {

                if (MyUtility.Check.Empty(dr["Price"]) || MyUtility.Check.Empty(dr["Qty"]))
                {
                    this.CurrentDetailData.Delete();
                }
                else
                {
                    amount += (decimal)dr["Price"] * (decimal)dr["Qty"];
                }

            }
            exact = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", CurrentMaintain["Currencyid"].ToString(), "Currency", "ID", "Production"));
            CurrentMaintain["Amount"] = MyUtility.Math.Round(amount, exact);
            getMasterTotal();

            #endregion 

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("<Detail> can not be Empty!");
                return false;
            }


            #region 填入ID
            if (this.IsDetailInserting)
            {
                string keyWordID = keyWord + "MA";
                string id = MyUtility.GetValue.GetID(keyWordID, "MiscAp");
                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion
            return base.ClickSaveBefore();

        }
        private void txtsubcon1_Validated(object sender, EventArgs e)
        {
            base.OnValidated(e);
            DataRow suppDr;
            string currencySql = string.Format("Select * from LocalSupp where id = '{0}' ",CurrentMaintain["LocalSuppid"].ToString());
            if(MyUtility.Check.Seek(currencySql,out suppDr ,"Production"))
            {
                CurrentMaintain["currencyId"] = suppDr["Currencyid"];
                CurrentMaintain["PayTermID"] = suppDr["PayTermID"];
            }                  
        }

        private void numericBox2_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                getMasterTotal();
            }
        }
        //算表頭Amount與Vat
        private void getMasterTotal()
        {
            exact = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", CurrentMaintain["Currencyid"].ToString(), "Currency", "ID", "Production"));

            CurrentMaintain["Vat"] = MyUtility.Math.Round((decimal)CurrentMaintain["Amount"] * (decimal)CurrentMaintain["VatRate"] / 100, exact);
            this.numericBox4.Value = (decimal)CurrentMaintain["Amount"] + (decimal)CurrentMaintain["Vat"];
        }

       

        
        //按下Junk 鈕
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MyUtility.Msg.WarningBox("Are you sure junk this data?", "Warning", buttons);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                string updSql = string.Format("update MiscAp set Status = 'Junked',editname='{0}', editdate = GETDATE() where id='{1}'", loginID, CurrentMaintain["ID"].ToString());

                DualResult upResult;
                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
                {
                    try
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                        {
                            ShowErr(updSql, upResult);
                            return;
                        }
                        _transactionscope.Complete();
                        MyUtility.Msg.WarningBox("Successfully");
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
                EnsureToolbarExt();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppid"]))
            {
                MyUtility.Msg.WarningBox("Please entry Supplier first.");
                return;
            }
            DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
            Form P40_import = new Sci.Production.Miscellaneous.P40_Import(detTable,CurrentMaintain["LocalSuppid"].ToString());
            P40_import.ShowDialog();
        }
    }
}
