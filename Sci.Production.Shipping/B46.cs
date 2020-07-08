using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B46
    /// </summary>
    public partial class B46 : Win.Tems.Input1
    {
        /// <summary>
        /// B46
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B46(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("Code can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("Name can't empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
