using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B02
    /// </summary>
    public partial class B02 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtThreadColor.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtThreadColor.Focus();
                MyUtility.Msg.WarningBox("<Thread Color> can not be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
