using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B04
    /// </summary>
    public partial class B04 : Win.Tems.Input1
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
            this.CurrentMaintain["AllowAutoAllocate"] = true;
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
