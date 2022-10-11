using org.apache.pdfbox.io;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class B12 : Sci.Win.Tems.Input1
    {
        public B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.txtMachineID.ReadOnly = false;
        }

        protected override bool ClickEditBefore()
        {
            this.txtMachineID.ReadOnly = true;
            return base.ClickEditBefore();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtMachineID.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtMachineID.Text))
            {
                MyUtility.Msg.WarningBox("Machine ID cannot be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
