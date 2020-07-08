using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B07
    /// </summary>
    public partial class B07 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                this.txtID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Term > can not be empty!");
                this.txtTerm.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
