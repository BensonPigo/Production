using Sci.Data;
using Sci.Win.Tools;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class QA_B12 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public QA_B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            if (!DBProxy.Current.DefaultModuleName.Contains("testing"))
            {
                this.ConnectionName = "ProductionTPE";
            }
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtSubProcess.ReadOnly = true;
            this.txtDefectCode.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["SubProcessID"]))
            {
                this.txtSubProcess.Focus();
                MyUtility.Msg.WarningBox("< SubProcess > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["DefectCode"]))
            {
                this.txtDefectCode.Focus();
                MyUtility.Msg.WarningBox("< DefectCode > can not be empty!");
                return false;
            }

            if (this.IsDetailInserting &&
                MyUtility.Check.Seek($"select 1 from SubProDefectCode where SubProcessID='{this.CurrentMaintain["SubProcessID"]}' and DefectCode = '{this.CurrentMaintain["DefectCode"]}'"))
            {
                MyUtility.Msg.WarningBox("SubProcess, DefectCode  can not duplicate!");
            }

            return base.ClickSaveBefore();
        }

        private void TxtSubProcess_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"
select s.ID, a.Remark
from SubProcess s with(nolock)
left join ArtworkType a on a.Id = s.ArtworkTypeId
where s.IsSubprocessInspection = 1
and s.Junk = 0
";
            SelectItem item = new SelectItem(sqlcmd, "20,4,20", this.txtSubProcess.Text, false, ",")
            {
                Size = new System.Drawing.Size(635, 510),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSubProcess.Text = item.GetSelectedString();
        }
    }
}
