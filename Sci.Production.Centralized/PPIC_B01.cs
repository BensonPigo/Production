using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// PPIC_B01
    /// </summary>
    public partial class PPIC_B01 : Win.Tems.Input1
    {
        /// <summary>
        /// PPIC_B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public PPIC_B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "L,Lacking,R,Replacement");
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = "FL";
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                this.txtID.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
