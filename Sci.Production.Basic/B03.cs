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

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textM.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< M > can not be empty!");
                this.textM.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }


    }
}
