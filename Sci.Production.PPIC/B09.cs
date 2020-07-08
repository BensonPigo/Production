using Ict;
using Sci.Data;
using System;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B09 : Sci.Win.Tems.Input1
    {
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
                this.toolbar.cmdJunk.Enabled = !junk && this.Perm.Junk;
                this.toolbar.cmdUnJunk.Enabled = junk && this.Perm.Junk;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = false;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["DM300"] = DBNull.Value;
        }

        protected override bool ClickSaveBefore()
        {
            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.Lookup(@"
select MAXID = case when max(DM300) >= 0 then max(DM300) +1 else 0 end
from FinishingProcess");
                this.CurrentMaintain["DM300"] = id;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickJunk()
        {
            base.ClickJunk();
            string sqlcmd = $@"update FinishingProcess set junk = 1 where DM300 = '{this.CurrentMaintain["DM300"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            string sqlcmd = $@"update FinishingProcess set junk = 0 where DM300 = '{this.CurrentMaintain["DM300"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }
    }
}
