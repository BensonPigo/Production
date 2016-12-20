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
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["NLCode"]))
            {
                MyUtility.Msg.WarningBox("NL Code can't empty");
                return false;
            }
            return base.ClickSaveBefore();
        }

        //NL Code
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"select NLCode,HSCode,UnitID
from VNContract_Detail
where ID in (select ID from VNContract WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract where Status = 'Confirmed') )
order by NLCode", "5,11,8", this.Text, false, ",", headercaptions: "NL Code, HSCode, Unit");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            CurrentMaintain["NLCode"] = item.GetSelectedString();
        }

        //NL Code
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox1.OldValue != textBox1.Text && !MyUtility.Check.Empty(textBox1.Text))
            {
                if (!MyUtility.Check.Seek(string.Format(@"select NLCode,HSCode,UnitID
from VNContract_Detail
where ID in (select ID from VNContract WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract where Status = 'Confirmed') )
and NLCode = '{0}'", textBox1.Text)))
                    {
                        MyUtility.Msg.WarningBox("The NL Code is not in the Contract!!");
                        CurrentMaintain["NLCode"] = "";
                        e.Cancel = true;
                        return;
                    }
            }
        }
    }
}
