using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class B04 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("Sewing Team can't empty!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                if (MyUtility.Check.Seek($"select 1 from SewingTeam where id = '{this.CurrentMaintain["ID"]}'"))
                {
                    MyUtility.Msg.WarningBox("Sewing Team already exists!");
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["ID"] = string.Empty;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }
    }
}
