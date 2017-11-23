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
    /// B04
    /// </summary>
    public partial class B04 : Sci.Win.Tems.Input1
    {
        private string keyword = Sci.Env.User.Keyword;

        /// <summary>
        /// B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string defaultfilter = string.Format("MDivisionid = '{0}' ", this.keyword);
            this.DefaultFilter = defaultfilter;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtThreadLocation.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["mDivisionid"] = this.keyword;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtThreadLocation.Focus();
                MyUtility.Msg.WarningBox("<Thread Location> can not be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
