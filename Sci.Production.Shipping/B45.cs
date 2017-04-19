using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B45 : Sci.Win.Tems.Input1
    {
        public B45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Type"] = 1;
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
                MyUtility.Msg.WarningBox("NL Code can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Qty"]))
            {
                MyUtility.Msg.WarningBox("Qty can't empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        //NL Code
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode", "5,11,8", this.Text, false, ",", headercaptions: "NL Code, HSCode, Unit");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> selectedData = item.GetSelecteds();
            CurrentMaintain["NLCode"] = item.GetSelectedString();
            CurrentMaintain["HSCode"] = selectedData[0]["HSCode"];
            CurrentMaintain["UnitID"] = selectedData[0]["UnitID"];

        }

        //NL Code
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtNLCode.OldValue != txtNLCode.Text)
            {
                if (MyUtility.Check.Empty(txtNLCode.Text))
                {
                    CurrentMaintain["NLCode"] = "";
                    CurrentMaintain["HSCode"] = "";
                    CurrentMaintain["UnitID"] = "";
                }
                else
                {
                    DataRow NLCodeDate;
                    if (MyUtility.Check.Seek(string.Format(@"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", txtNLCode.Text), out NLCodeDate))
                    {
                        CurrentMaintain["NLCode"] = txtNLCode.Text;
                        CurrentMaintain["HSCode"] = NLCodeDate["HSCode"];
                        CurrentMaintain["UnitID"] = NLCodeDate["UnitID"];
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("The NL Code is not in the Contract!!");
                        CurrentMaintain["NLCode"] = "";
                        CurrentMaintain["HSCode"] = "";
                        CurrentMaintain["UnitID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }
    }
}
