using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Cutting_B01
    /// </summary>
    public partial class Cutting_B01 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// Cutting_B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Cutting_B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtID.Text.Trim()) || MyUtility.Check.Empty(this.txtShowSeq.Text.Trim()))
            {
                MyUtility.Msg.InfoBox("'ID' and 'Show Seq' can not empty");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }
    }
}
