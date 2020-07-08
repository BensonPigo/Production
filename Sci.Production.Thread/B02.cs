using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B02
    /// </summary>
    public partial class B02 : Win.Tems.Input1
    {
        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtThreadColor.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtThreadColor.Focus();
                MyUtility.Msg.WarningBox("<Thread Color> can not be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtThreadColorGroupID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        @"Select ID,description  from ThreadColorGroup WITH (NOLOCK) where JUNK=0 order by ID", "10,45", null);
            item.Size = new System.Drawing.Size(630, 535);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ThreadColorGroupID"] = item.GetSelectedString();
        }

        private void TxtThreadColorGroupID_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtThreadColorGroupID.Text))
            {
                return;
            }

            if (!MyUtility.Check.Seek($@"select 1 from ThreadColorGroup WITH (NOLOCK) where junk=0 and id='{this.txtThreadColorGroupID.Text}'"))
            {
                MyUtility.Msg.WarningBox("<Thread Color Group>data not found!");
                e.Cancel = true;
            }
        }
    }
}
