using Sci.Win.Tems;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class B41 : Input1
    {
        public B41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtLocation.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("<Location> can not be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
