using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //按鈕Accounting chart no變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "LocalSupp_AccountNo", "ID"))
            {
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }

            //按鈕Bank detail變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "LocalSupp_Bank", "ID"))
            {
                this.button2.ForeColor = Color.Blue;
            }
            else
            {
                this.button2.ForeColor = Color.Black;
            }
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textCode.ReadOnly = true;
            this.textBox2.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.textCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Abb"]))
            {
                MyUtility.Msg.WarningBox("< Abbreviation > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["CountryID"]))
            {
                MyUtility.Msg.WarningBox("< Nationality > can not be empty!");
                this.txtcountry1.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Company > can not be empty!");
                this.textBox3.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B04_AccountNo callNextForm = new Sci.Production.Basic.B04_AccountNo(this.IsSupportEdit,CurrentMaintain["ID"].ToString(),null,null);
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B04_BankData callNextForm = new Sci.Production.Basic.B04_BankData(this.IsSupportEdit, CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }
    }
}
