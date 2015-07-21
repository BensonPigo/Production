using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = DBNull.Value;
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["Forwarder"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Forwarder > can not be empty!");
                this.txtsubcon1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["WhseNo"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Warehouse# > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
