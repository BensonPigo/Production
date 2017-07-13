using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    public partial class PPIC_B02 : Sci.Win.Tems.Input1
    {
        public PPIC_B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboType, 2, 1, "L,Lacking,R,Replacement");
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtID.ReadOnly = true;
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Type"] = "AL";
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                this.txtID.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
