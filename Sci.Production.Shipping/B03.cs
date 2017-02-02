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
    public partial class B03 : Sci.Win.Tems.Input1
    {
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //按鈕Canvass Record變色
            if (MyUtility.Check.Seek(MyUtility.Convert.GetString(CurrentMaintain["ID"]), "ShipExpense_CanVass", "ID"))
            {
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }

            //按鈕Payment History變色
            if (MyUtility.Check.Seek(MyUtility.Convert.GetString(CurrentMaintain["ID"]), "ShippingAP_Detail", "ShipExpenseID"))
            {
                this.button2.ForeColor = Color.Blue;
            }
            else
            {
                this.button2.ForeColor = Color.Black;
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.txtsubcon1.TextBox1.ReadOnly = true;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.txtsubcon1.TextBox1.ReadOnly = true;
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = "";
            CurrentMaintain["LocalSuppID"] = "";
            CurrentMaintain["CurrencyID"] = "";
            CurrentMaintain["Price"] = 0;
            CurrentMaintain["CanvassDate"] = DBNull.Value;
            this.txtsubcon1.TextBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(MyUtility.Convert.GetString(CurrentMaintain["ID"])))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(MyUtility.Convert.GetString(CurrentMaintain["Description"])))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.editBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(MyUtility.Convert.GetString(CurrentMaintain["AccountID"])))
            {
                MyUtility.Msg.WarningBox("< Account No > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {

            Sci.Production.Shipping.B03_PrintReviseList callNextForm = new Sci.Production.Shipping.B03_PrintReviseList();
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(txtCode.Text) && txtCode.Text != txtCode.OldValue)
            {
                if (txtCode.Text.IndexOf("'") != -1)
                {
                    MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                    txtCode.Text = "";
                    e.Cancel = true;
                    return;
                }

                string selectCommand = string.Format("select ID from ShipExpense where ID = '{0}'", txtCode.Text);
                if (MyUtility.Check.Seek(selectCommand, null))
                {
                    MyUtility.Msg.WarningBox(string.Format("Code: '{0}' is duplicate!", txtCode.Text.Trim()));
                    txtCode.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.B03_CanvassRecord callNextForm = new Sci.Production.Shipping.B03_CanvassRecord(Prgs.GetAuthority(Sci.Env.User.UserID, "B03. Shipping Expense", "CanEdit"), CurrentMaintain);
            callNextForm.ShowDialog(this);
            this.RenewData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.B03_PaymentHistory callNextForm = new Sci.Production.Shipping.B03_PaymentHistory(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
