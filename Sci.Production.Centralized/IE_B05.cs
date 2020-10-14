using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B05
    /// </summary>
    public partial class IE_B05 : Win.Tems.Input1
    {
        /// <summary>
        /// IE_B05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public IE_B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = string.Format("select * from [MachineType_ThreadRatio] where ID='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sql, this.ConnectionName))
            {
                this.btnThreadRatio.ForeColor = Color.Blue;
            }
            else
            {
                this.btnThreadRatio.ForeColor = DefaultForeColor;
            }

            this.Init();
        }

        private void Init()
        {
            string sqlCmd = string.Format("select IsDesignatedArea from [ProductionTPE].[dbo].[MachineTypeTPE] where id = '{0}'", this.CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(this.ConnectionName, sqlCmd, out DataTable dt);
            if (result && dt.Rows.Count > 0)
            {
                this.chkIsDesignatedArea.Checked = MyUtility.Convert.GetBool(dt.Rows[0]["IsDesignatedArea"]);
            }
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DualResult result = new DualResult(false);
            string sqlCmd = string.Format(
                @"
if exists (select 1 from [ProductionTPE].[dbo].[MachineTypeTPE] where id = '{0}')
begin
	update [ProductionTPE].[dbo].[MachineTypeTPE] set IsDesignatedArea = '{1}' where id = '{0}'
end
else
begin
	insert into [ProductionTPE].[dbo].[MachineTypeTPE]([ID], [IsDesignatedArea])
	values('{0}', '{1}')
end",
                this.CurrentMaintain["ID"].ToString(),
                this.chkIsDesignatedArea.Checked);
            result = DBProxy.Current.Execute(this.ConnectionName, sqlCmd);

            return result;
        }

        private void BtnThreadRatio_Click(object sender, EventArgs e)
        {
            IE_B05_ThreadRatio callNextForm = new IE_B05_ThreadRatio(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
