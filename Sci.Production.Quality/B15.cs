using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win.Tools;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B15 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
            this.txtSubProcessID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("Response Team can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SubProcessID"]))
            {
                MyUtility.Msg.WarningBox("SubProcess can't empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            var result = base.ClickSave();
            string msg = result.ToString().ToUpper();
            if (msg.Contains("PK") && msg.Contains("DUPLICAT"))
            {
                result = Ict.Result.F("Response Team, SubProcess duplicated", result.GetException());
            }

            return result;
        }

        private void TxtSubProcessID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CheckSubProcessID();
        }

        private void TxtSubProcessID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CheckSubProcessID(true);
        }

        private void CheckSubProcessID(bool validating = false)
        {
            string sqlcmd = $@"select ID,ArtworkTypeId from SubProcess where IsSubprocessInspection = 1 and junk = 0";
            if (validating)
            {
                sqlcmd += $" and id = '{this.txtSubProcessID.Text}'";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (validating)
            {
                if (dt.Rows.Count == 0 && !this.txtSubProcessID.Text.Empty())
                {
                    MyUtility.Msg.WarningBox($"{this.txtSubProcessID.Text} not found!");
                    this.txtSubProcessID.Text = string.Empty;
                    return;
                }
            }
            else
            {
                SelectItem item = new SelectItem(dt, "ID,ArtworkTypeId", string.Empty, this.txtSubProcessID.Text, false, ",");
                if (item.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["SubProcessID"] = item.GetSelectedString();
            }
        }
    }
}
