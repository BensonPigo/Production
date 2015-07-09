using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "ShipExpense_CanVass", "ID"))
            {
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }

            //按鈕Payment History變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "ShippingAP_Detail", "ShipExpenseID"))
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
            this.textBox1.ReadOnly = true;
            this.txtsubcon1.TextBox1.ReadOnly = true;
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = DBNull.Value;
            CurrentMaintain["LocalSuppID"] = DBNull.Value;
            CurrentMaintain["CurrencyID"] = DBNull.Value;
            CurrentMaintain["Price"] = DBNull.Value;
            CurrentMaintain["CanvassDate"] = DBNull.Value;
            this.txtsubcon1.TextBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MessageBox.Show("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MessageBox.Show("< Description > can not be empty!");
                this.editBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentMaintain["AccountNo"].ToString()))
            {
                MessageBox.Show("< Account No > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text) && this.textBox1.Text != this.textBox1.OldValue)
            {
                string selectCommand = string.Format("select ID from ShipExpense where ID = '{0}'", this.textBox1.Text.ToString());
                if (MyUtility.Check.Seek(selectCommand, null))
                {
                    MessageBox.Show(string.Format("Code: '{0}' is duplicate!", this.textBox1.Text.ToString().Trim()));
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.B03_CanvassRecord callNextForm = new Sci.Production.Shipping.B03_CanvassRecord(this.IsSupportEdit, CurrentMaintain);
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
