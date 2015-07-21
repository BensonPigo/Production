using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //按鈕Shipping Mark變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Factory_TMS", "ID") ||
                MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Factory_WorkHour", "ID"))
            {
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["NameEN"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Name > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B01_CapacityWorkDay callNextForm = new Sci.Production.Basic.B01_CapacityWorkDay(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
