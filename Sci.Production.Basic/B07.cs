using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                this.txtID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Term > can not be empty!");
                this.txtTerm.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
