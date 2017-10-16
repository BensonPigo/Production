using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B44 : Sci.Win.Tems.Input1
    {
        public B44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtNLCode.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["NLCode"]))
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty");
                return false;
            }
            if (MyUtility.Convert.GetDecimal(CurrentMaintain["WasteLower"]) > MyUtility.Convert.GetDecimal(CurrentMaintain["WasteUpper"]))
            {
                MyUtility.Msg.WarningBox("WasteLower can't bigger than wasteUpper");
                return false;
            }
            return base.ClickSaveBefore();
        }

        //NL Code
        private void txtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode", "5,11,8", this.Text, false, ",", headercaptions: "Customs Code, HSCode, Unit");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            CurrentMaintain["NLCode"] = item.GetSelectedString();
        }

        //NL Code
        private void txtNLCode_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtNLCode.OldValue != txtNLCode.Text && !MyUtility.Check.Empty(txtNLCode.Text))
            {
                if (!MyUtility.Check.Seek(string.Format(@"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", txtNLCode.Text)))
                    {
                        CurrentMaintain["NLCode"] = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                        return;
                    }
            }
        }
    }
}
