using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sci.Production.PublicPrg;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Win.Tems.Input1
    {
        /// <summary>
        /// B03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 按鈕Quotation Record變色
            if (MyUtility.Check.Seek(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "ShipExpense_CanVass", "ID"))
            {
                this.btnQuotationRecord.ForeColor = Color.Blue;
            }
            else
            {
                this.btnQuotationRecord.ForeColor = Color.Black;
            }

            // 按鈕Payment History變色
            if (MyUtility.Check.Seek(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "ShippingAP_Detail", "ShipExpenseID"))
            {
                this.btnPaymentHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnPaymentHistory.ForeColor = Color.Black;
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.txtsubconSupplier.TextBox1.ReadOnly = true;
            this.CurrentMaintain["Status"] = "New";
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.txtsubconSupplier.TextBox1.ReadOnly = true;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Locked"))
            {
                this.editDescription.ReadOnly = true;
                this.txtUnit.ReadOnly = true;
                this.txtAccountNo.TextBox1.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["ID"] = string.Empty;
            this.CurrentMaintain["LocalSuppID"] = string.Empty;
            this.CurrentMaintain["CurrencyID"] = string.Empty;
            this.CurrentMaintain["Price"] = 0;
            this.CurrentMaintain["CanvassDate"] = DBNull.Value;
            this.CurrentMaintain["Status"] = "New";
            this.txtsubconSupplier.TextBox1.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                this.txtCode.Focus();
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(MyUtility.Convert.GetString(this.CurrentMaintain["Description"])))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(MyUtility.Convert.GetString(this.CurrentMaintain["AccountID"])))
            {
                this.txtAccountNo.Focus();
                MyUtility.Msg.WarningBox("< Account No > can not be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickClose()
        {
            string updatesql = $@"update  ShipExpense set Status = 'Locked' where id = '{this.CurrentMaintain["id"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickClose();
        }

        /// <inheritdoc/>
        protected override void ClickUnclose()
        {
            string updatesql = $@"update  ShipExpense set Status = 'New' where id = '{this.CurrentMaintain["id"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickUnclose();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            B03_PrintReviseList callNextForm = new B03_PrintReviseList();
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        private void TxtCode_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtCode.Text) && this.txtCode.Text != this.txtCode.OldValue)
            {
                if (this.txtCode.Text.IndexOf("'") != -1)
                {
                    this.txtCode.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                    return;
                }

                string selectCommand = string.Format("select ID from ShipExpense WITH (NOLOCK) where ID = '{0}'", this.txtCode.Text);
                if (MyUtility.Check.Seek(selectCommand, null))
                {
                    this.txtCode.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("Code: '{0}' is duplicate!", this.txtCode.Text.Trim()));
                    return;
                }
            }
        }

        private void BtnCanvassRecord_Click(object sender, EventArgs e)
        {
            B03_Quotation callNextForm = new B03_Quotation(Prgs.GetAuthority(Sci.Env.User.UserID, "B03. Shipping Expense", "CanEdit"), this.CurrentMaintain, this.Perm.Confirm);
            callNextForm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnPaymentHistory_Click(object sender, EventArgs e)
        {
            B03_PaymentHistory callNextForm = new B03_PaymentHistory(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        Form batchapprove;

        private void BtnBatchApprove_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Confirm)
            {
                MyUtility.Msg.WarningBox("You don't have permission to confirm.");
                return;
            }

            if (this.batchapprove == null || this.batchapprove.IsDisposed)
            {
                this.batchapprove = new B03_BatchApprove(this.reload);
                this.batchapprove.Show();
            }
            else
            {
                this.batchapprove.Activate();
            }
        }

        public void reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }

        private void B03_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.batchapprove != null)
            {
                this.batchapprove.Dispose();
            }
        }

        private void B03_FormLoaded(object sender, EventArgs e)
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = "JUNK = 0";
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            DBProxy.Current.Execute(null, $"UPDATE ShipExpense SET Junk=1,Status='Junked',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            DBProxy.Current.Execute(null, $"UPDATE ShipExpense SET Junk=0,Status='New',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }
    }
}
