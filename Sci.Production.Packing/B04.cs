using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class B04 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B04(ToolStripMenuItem menuitem)
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

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            string cmd = $@"

select   b.ShippingMarkPictureUkey
	    ,b.ShippingMarkTypeUkey
	    ,[Horizontal]=b.FromBottom + c.Width 
	    ,[NotHorizontal]=b.FromBottom + c.Length 
	    ,[IsOverCtnHt]= CASE WHEN b.IsHorizontal = 1 THEN IIF( b.FromBottom + c.Width > a.CtnHeight , 1, 0)
					 	     WHEN b.IsHorizontal = 0 THEN IIF( b.FromBottom + c.Length > a.CtnHeight , 1, 0)
					    ELSE 0
					    END
INTO #tmp
from ShippingMarkPicture a
inner join ShippingMarkPicture_Detail b on a.Ukey = b.ShippingMarkPictureUkey
inner join StickerSize c on b.StickerSizeID = c.ID
WHERE c.ID = {this.CurrentMaintain["ID"]} 

UPDATE t
SET t.IsOverCtnHt = s.IsOverCtnHt ,t.NotAutomate = s.IsOverCtnHt
FROM ShippingMarkPicture_Detail t
INNER JOIN #tmp s ON t.ShippingMarkPictureUkey = s.ShippingMarkPictureUkey AND t.ShippingMarkTypeUkey = s.ShippingMarkTypeUkey

DROP TABLE #tmp
";

            DBProxy.Current.Execute(null, cmd);
            base.ClickSaveAfter();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
