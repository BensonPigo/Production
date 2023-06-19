using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <summary>
    /// B31
    /// </summary>
    public partial class B31 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B31
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtEmployeeID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["EmployeeID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SubprocessID"]))
            {
                MyUtility.Msg.WarningBox("< Employee ID > and < Subprocess >cannot be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
