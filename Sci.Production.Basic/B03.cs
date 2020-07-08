using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Win.Tems.Input1
    {
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtM.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< M > can not be empty!");
                this.txtM.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
