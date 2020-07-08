using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class B04 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B04(ToolStripMenuItem menuitem)
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

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Size", this.CurrentMaintain["Size"].ToString()));

            bool isSizeExists = MyUtility.Check.Seek($"SELECT 1 FROM StickerSize WHERE Size=@Size AND ID <> {this.CurrentMaintain["ID"]} ", paras);

            if (isSizeExists)
            {
                MyUtility.Msg.InfoBox("The Sticker Size exists already. Please check again.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickJunk()
        {
            base.ClickJunk();
            string sqlcmd = $@"update StickerSize set junk = 1 where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            string sqlcmd = $@"update StickerSize set junk = 0 where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }
    }
}
