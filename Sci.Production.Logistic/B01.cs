using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
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
                this.textBox2.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
