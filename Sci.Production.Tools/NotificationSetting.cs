using Sci.Data;
using Sci.Production.Class;
using System.Data;
using System.Windows.Forms;
using Sci.Production.Class.Command;

namespace Sci.Production.Tools
{
    /// <inheritdoc />
    internal partial class NotificationSetting : Sci.Win.Tems.Input1
    {
        /// <inheritdoc />
        public NotificationSetting(ToolStripMenuItem menuitem)
           : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc />
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            var sqlCmd = $"Select distinct MenuName From Menu with(nolock) Where IsSubMenu = 0 and ForMISOnly = 0 ";

            DataTable dtddl;
            if (SQL.Select(SQL.queryConn, sqlCmd, out dtddl))
            {
                this.cmbMenu.DataSource = dtddl;
                this.cmbMenu.ValueMember = "MenuName";
                this.cmbMenu.DisplayMember = "MenuName";
                this.cmbMenu.SelectedIndex = 0;
            }
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.cmbMenu.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["MenuName"]))
            {
                MyUtility.Msg.WarningBox("Menu cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FormName"]))
            {
                MyUtility.Msg.WarningBox("Form Name cannot be empty.");
                return false;
            }

            if (this.IsDetailInserting)
            {
                var maxID = DBProxy.Current.LookupEx<int>($"Select cast(Max(ID) as INT) From NotificationList with(nolock) Where MenuName = '{this.CurrentMaintain["MenuName"]}'").ExtendedData;
                this.CurrentMaintain["ID"] = (maxID + 1).ToString().PadLeft(2, '0');
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Description"] = string.Empty;
            this.CurrentMaintain["FormName"] = string.Empty;
            this.CurrentMaintain["Name"] = string.Empty;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            this.CurrentMaintain["ID"] = string.Empty;

            base.ClickCopyAfter();
        }

        private void TxtFormName_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string cmd = $@"
declare @MenuName varchar(30) = '{this.CurrentMaintain["MenuName"]}'
declare @MenuUkey int = (select pkey from menu where MenuName = @MenuName)
declare @CodeName varchar(30) = 
(
	select case @MenuName when 'QA' then 'Quality'
	when 'Clog' then 'Logistic'
	else @MenuName end
)

select * 
into #tmpMenu
from Menu m
where m.PKey=@MenuUkey
or exists(
	select * 
	from MenuDetail md
	where UKey=@MenuUkey and ObjectCode = 1
	and md.BarPrompt = m.MenuName
)

SELECT distinct FormName FROM MenuDetail where Ukey in (select pkey from #tmpMenu) 
and FormName!=''
and FormName LIKE '%' + @CodeName +'.P'+ '%'　-- 只取P開頭程式
ORDER BY FormName

drop table #tmpMenu
";
            DBProxy.Current.Select("Production", cmd, out DataTable dt);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "FormName", "30", this.txtFormName.Text);

            DialogResult result = item.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (result == DialogResult.OK)
            {
                this.CurrentMaintain["FormName"] = item.GetSelectedString();
            }
            else
            {
                this.txtFormName.Text = string.Empty;
            }
        }
    }
}
