using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Shipping_B08 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public Shipping_B08(ToolStripMenuItem menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.txtPort.ReadOnly = !this.EditMode;
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.txtPort.ReadOnly = true;
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.txtPort.ReadOnly = false;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtPort.Text))
            {
                MyUtility.Msg.WarningBox("Port cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtName.Text))
            {
                MyUtility.Msg.WarningBox("Name cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtcountry.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("Country cannot be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            this.txtPort.ReadOnly = true;
            base.ClickSaveAfter();
        }
    }
}
