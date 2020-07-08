using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B01
    /// </summary>
    public partial class B01 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtThreadCombination.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtThreadCombination.Focus();
                MyUtility.Msg.WarningBox("<Thread Combination> can not be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
