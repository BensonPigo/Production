using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B03
        /// </summary>
        /// <param name="menuitem">menuitemS</param>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtThreadType.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtThreadType.Focus();
                MyUtility.Msg.WarningBox("<Thread Type> can not be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
