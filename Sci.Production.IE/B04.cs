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

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Type"] = "CP";
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
            return base.ClickSaveBefore();
        }
    }
}
