using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B46 : Sci.Win.Tems.Input1
    {
        public B46(ToolStripMenuItem menuitem)
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
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("Code can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("Name can't empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
