using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Clog_B03 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public Clog_B03(ToolStripMenuItem menuitem)
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

            var dataTable = (DataTable)this.gridbs.DataSource;

            if(dataTable.Rows.Count > 0)
            {
                var listcount = dataTable.Select($"Description = '{this.CurrentMaintain["Description"]}'").ToList();
                if ( listcount.Count > 0)
                {
                    MyUtility.Msg.WarningBox("This <Reason> already exists.");
                }
                return false;
            }


            this.CurrentMaintain["Type"] = "CL";

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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            var dataTable = (DataTable)this.gridbs.DataSource;
            var sortedTableCount = dataTable.Rows.Count + 1;
            this.CurrentMaintain["ID"] = sortedTableCount.ToString("D5");
            base.ClickNewAfter();
        }
    }
}
