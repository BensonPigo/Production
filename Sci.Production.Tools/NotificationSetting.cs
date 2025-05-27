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
            if (this.IsDetailInserting)
            {
                var maxID = DBProxy.Current.LookupEx<int>($"Select cast(Max(ID) as INT) From NotificationList with(nolock) Where MenuName = '{this.CurrentMaintain["MenuName"]}'").ExtendedData;
                this.CurrentMaintain["ID"] = (maxID + 1).ToString().PadLeft(2, '0');
            }

            return base.ClickSaveBefore();
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

            string cmd = $@"SELECT FormName FROM MenuDetail where Ukey = (select pkey from menu where MenuName = '{this.CurrentMaintain["MenuName"]}') ORDER BY ID";
            DBProxy.Current.Select("ProductionTPE", cmd, out DataTable dt);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "FormName", "30", this.txtFormName.Text);

            DialogResult result = item.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }
        }
    }
}
