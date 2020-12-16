using Ict;
using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B07
    /// </summary>
    public partial class IE_B07 : Win.Tems.Input1
    {
        /// <summary>
        /// IE_B07
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public IE_B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.labelDescriptionEnglish.Text = "Description\r\n(English)";
            this.labelDescriptionChinese.Text = "Description\r\n(Chinese)";
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "A,Accoessory,W,Workmanship");
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.Init();
        }

        private void Init()
        {
            string sqlCmd = string.Format("select IsAttachment, IsTemplate from [ProductionTPE].[dbo].[MoldTPE] where id = '{0}'", this.CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out DataTable dt);
            if (result && dt.Rows.Count > 0)
            {
                this.chkIsAttachment.Checked = MyUtility.Convert.GetBool(dt.Rows[0]["IsAttachment"]);
                this.chkIsTemplate.Checked = MyUtility.Convert.GetBool(dt.Rows[0]["IsTemplate"]);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DualResult result = new DualResult(false);
            string sqlCmd = string.Format(
                @"
if exists (select 1 from [ProductionTPE].[dbo].[MoldTPE] where id = '{0}')
begin
	update [ProductionTPE].[dbo].[MoldTPE] set IsAttachment = '{1}', IsTemplate = '{2}' where id = '{0}'
end
else
begin
	insert into [ProductionTPE].[dbo].[MoldTPE]([ID], [IsAttachment], [IsTemplate])
	values('{0}', '{1}', '{2}')
end",
                this.CurrentMaintain["ID"].ToString(),
                this.chkIsAttachment.Checked,
                this.chkIsTemplate.Checked);
            result = DBProxy.Current.Execute(this.ConnectionName, sqlCmd);

            return result;
        }
    }
}
