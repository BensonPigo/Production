using Ict;
using Sci.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Clog_B02 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public Clog_B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]);
                this.toolbar.cmdJunk.Enabled = !junk && this.Perm.Junk;
                this.toolbar.cmdUnJunk.Enabled = junk && this.Perm.Junk;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = false;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        /// /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]) || MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
            {
                MyUtility.Msg.WarningBox("< ID > and < Reason > can not be empty!");
                return false;
            }

            this.CurrentMaintain["Type"] = "ID";

            return base.ClickSaveBefore();
        }

        /// /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            DBProxy.Current.Execute("ProductionTPE", $"UPDATE ClogReason SET Junk=1 ,EditDate=GETDATE() ,EditName='{Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            DBProxy.Current.Execute("ProductionTPE", $"UPDATE ClogReason SET Junk=0 ,EditDate=GETDATE() ,EditName='{Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }
    }
}
