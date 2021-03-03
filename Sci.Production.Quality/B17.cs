using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <summary>
    /// B17
    /// </summary>
    public partial class B17 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B17
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B17(ToolStripMenuItem menuitem)
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SubProcessID"]))
            {
                MyUtility.Msg.WarningBox("<ID>, <SubProcess> can not be empty");
                return false;
            }

            bool isDupKey = MyUtility.Check.Seek($"select 1 from SubProLocation with (nolock) where ID = '{this.CurrentMaintain["ID"]}' and SubProcessID = '{this.CurrentMaintain["SubProcessID"]}'");
            if (this.CurrentMaintain.RowState == DataRowState.Added &&
                isDupKey)
            {
                MyUtility.Msg.WarningBox("<ID>, <SubProcess> are repeated");
                return false;
            }

            return base.ClickSaveBefore();
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
