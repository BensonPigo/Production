using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class B08 : Sci.Win.Tems.Input1
    {         
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["Refno"]))
                txtReson.Text = "Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension";
            else txtReson.Text = "";
            base.OnDetailEntered();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtRefno.ReadOnly = true;         
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["Refno"]))
            {
                ShowErr("<RefNo> cannot be empty!");
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
                result = Result.F(string.Format("<RefNo:{0}>existed,change other one please!", txtRefno.Text), result.GetException());
            }
            return result;
        }

        private void txtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"SELECT DISTINCT RefNo
FROM Fabric WHERE junk=0 AND TYPE='F' ORDER BY RefNo", "25","Refno");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) return; 
            txtRefno.Text = item.GetSelectedString();
            txtReson.Text = "Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension";
        }

        private void txtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (txtRefno.Text.Trim() == "") { txtReson.Text = ""; return; }
            if (MyUtility.Check.Seek(string.Format("SELECT DISTINCT RefNo FROM Fabric WHERE junk=0 AND TYPE='F' AND RefNo = '{0}'",txtRefno.Text)))
            {
                txtReson.Text = "Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension";
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("<RefNo:{0}> not found!!!", txtRefno.Text));
                txtRefno.Text = "";
                txtReson.Text = "";
                e.Cancel = true;
                return;
            }
        }
    }
}
