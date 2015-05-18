using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["Type"] = "CP";
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
                MessageBox.Show("< ID > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MessageBox.Show("< Problem > can not be empty!");
                this.textBox2.Focus();
                return false;
            }
            return base.OnSaveBefore();
        }
    }
}
