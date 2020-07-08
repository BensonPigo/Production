using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B44
    /// </summary>
    public partial class B44 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B44
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtNLCode.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["NLCode"]))
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty");
                return false;
            }

            if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["WasteLower"]) > MyUtility.Convert.GetDecimal(this.CurrentMaintain["WasteUpper"]))
            {
                MyUtility.Msg.WarningBox("WasteLower can't bigger than wasteUpper");
                return false;
            }

            return base.ClickSaveBefore();
        }

        // NL Code
        private void TxtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode", "5,11,8",
                this.Text,
                false,
                ",",
                headercaptions: "Customs Code, HSCode, Unit");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["NLCode"] = item.GetSelectedString();
        }

        // NL Code
        private void TxtNLCode_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtNLCode.OldValue != this.txtNLCode.Text && !MyUtility.Check.Empty(this.txtNLCode.Text))
            {
                if (!MyUtility.Check.Seek(string.Format(
                    @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'",
                    this.txtNLCode.Text)))
                {
                        this.CurrentMaintain["NLCode"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                        return;
                }
            }
        }
    }
}
