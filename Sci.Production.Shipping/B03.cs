using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Sci.Win.Tems.Input1
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

            // 按鈕Canvass Record變色
            if (MyUtility.Check.Seek(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "ShipExpense_CanVass", "ID"))
            {
                this.btnCanvassRecord.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCanvassRecord.ForeColor = Color.Black;
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
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.txtsubconSupplier.TextBox1.ReadOnly = true;
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
        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.B03_PrintReviseList callNextForm = new Sci.Production.Shipping.B03_PrintReviseList();
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
            Sci.Production.Shipping.B03_CanvassRecord callNextForm = new Sci.Production.Shipping.B03_CanvassRecord(Prgs.GetAuthority(Sci.Env.User.UserID, "B03. Shipping Expense", "CanEdit"), this.CurrentMaintain);
            callNextForm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnPaymentHistory_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.B03_PaymentHistory callNextForm = new Sci.Production.Shipping.B03_PaymentHistory(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
