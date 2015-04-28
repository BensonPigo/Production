using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B03 : Sci.Win.Tems.Input1
    {
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool OnSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MessageBox.Show("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Name"].ToString()))
            {
                MessageBox.Show("< Name > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            return base.OnSaveBefore();
        }
    }
}
