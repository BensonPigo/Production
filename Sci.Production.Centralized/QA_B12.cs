using Sci.Data;
using Sci.Win.Tools;
using System.Data;
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
            this.txtArtworkType.ReadOnly = true;
            this.txtDefectCode.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ArtworkTypeID"]))
            {
                this.txtArtworkType.Focus();
                MyUtility.Msg.WarningBox("< ArtworkType > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["DefectCode"]))
            {
                this.txtDefectCode.Focus();
                MyUtility.Msg.WarningBox("< DefectCode > can not be empty!");
                return false;
            }

            if (this.IsDetailInserting &&
                MyUtility.Check.Seek($"select 1 from SubProDefectCode where ArtworkTypeID='{this.CurrentMaintain["ArtworkTypeID"]}' and DefectCode = '{this.CurrentMaintain["DefectCode"]}'"))
            {
                MyUtility.Msg.WarningBox("ArtworkType, DefectCode  can not duplicate!");
            }

            return base.ClickSaveBefore();
        }

        private void TxtArtworkType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"
select a.ID, a.Remark
from ArtworkType a with(nolock)
where a.IsSubprocessInspection = 1
and a.Junk = 0
";

            SelectItem item = new SelectItem(sqlcmd, "20,4,20", this.txtArtworkType.Text, false, ",")
            {
                Size = new System.Drawing.Size(635, 510),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtArtworkType.Text = item.GetSelectedString();
        }
    }
}
