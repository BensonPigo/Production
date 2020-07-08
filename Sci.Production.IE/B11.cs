using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B11
    /// </summary>
    public partial class B11 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B11
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B11(ToolStripMenuItem menuitem)
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
            this.txtID.ReadOnly = true;
        }

        /// <summary>
        /// txtID_Validating
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TxtID_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtID.Text.Contains(","))
            {
                MyUtility.Msg.WarningBox("<ID> can not have ',' !");

                this.txtID.Text = string.Empty;
            }
        }
    }
}
