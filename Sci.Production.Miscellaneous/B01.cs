using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Miscellaneous
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Local"].ToString() == "False")
            {
                MyUtility.Msg.WarningBox("Only Local Supplier can modify.");
                return false;
            }
            
            return base.ClickEditBefore();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Local"] = 1;
        }
    }
}
