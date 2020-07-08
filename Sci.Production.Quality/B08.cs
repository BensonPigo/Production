using System.ComponentModel;
using System.Windows.Forms;

using Ict;

namespace Sci.Production.Quality
{
    public partial class B08 : Win.Tems.Input1
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Refno"]))
            {
                this.txtReson.Text = "Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension";
            }
            else
            {
                this.txtReson.Text = string.Empty;
            }

            base.OnDetailEntered();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtRefno.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Refno"]))
            {
                this.ShowErr("<RefNo> cannot be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSave()
        {
            var result = base.ClickSave();
            string msg = result.ToString().ToUpper();
            if (msg.Contains("PK") && msg.Contains("DUPLICAT"))
            {
                result = Result.F(string.Format("<RefNo:{0}>existed,change other one please!", this.txtRefno.Text), result.GetException());
            }

            return result;
        }

        private void txtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                @"SELECT DISTINCT RefNo
FROM Fabric WHERE junk=0 AND TYPE='F' ORDER BY RefNo", "25", "Refno");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtRefno.Text = item.GetSelectedString();
            this.txtReson.Text = "Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension";
        }

        private void txtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRefno.Text.Trim() == string.Empty)
            {
                this.txtReson.Text = string.Empty;
                return;
            }

            if (MyUtility.Check.Seek(string.Format("SELECT DISTINCT RefNo FROM Fabric WHERE junk=0 AND TYPE='F' AND RefNo = '{0}'", this.txtRefno.Text)))
            {
                this.txtReson.Text = "Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension";
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("<RefNo:{0}> not found!!!", this.txtRefno.Text));
                this.txtRefno.Text = string.Empty;
                this.txtReson.Text = string.Empty;
                e.Cancel = true;
                return;
            }
        }
    }
}
