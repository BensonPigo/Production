using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class B53 : Sci.Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;
        public B53(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            label14.Text = string.Format("Weight\r\n(kgs/{0})", MyUtility.Convert.GetString(CurrentMaintain["UnitID"]));
            textBox1.Text = MyUtility.GetValue.Lookup(string.Format("select GoodsDescription from KHGoodsHSCode where NLCode = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["NLCode"])));
        }

        protected override bool ClickEditBefore()
        {
            editName = MyUtility.Convert.GetString(CurrentMaintain["EditName"]);
            editDate = MyUtility.Convert.GetDate(CurrentMaintain["EditDate"]);
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            editBox1.ReadOnly = true;
            txtsubcon1.TextBox1.ReadOnly = true;
            checkBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            CurrentMaintain["NLCodeEditName"] = Sci.Env.User.UserID;
            CurrentMaintain["NLCodeEditDate"] = DateTime.Now;

            return base.ClickSaveBefore();
        }

        protected override Ict.DualResult ClickSavePost()
        {
            string updateCmd;
            if (MyUtility.Check.Empty(editDate))
            {
                updateCmd = string.Format("update LocalItem Set EditName = '{0}', EditDate = null where RefNo = '{1}';", editName, MyUtility.Convert.GetString(CurrentMaintain["RefNo"]));
            }
            else
            {
                updateCmd = string.Format("update LocalItem Set EditName = '{0}', EditDate = '{1}' where RefNo = '{2}';", editName, Convert.ToDateTime(editDate).ToString("yyyy/MM/dd HH:mm:ss"), MyUtility.Convert.GetString(CurrentMaintain["RefNo"]));
            }

            return DBProxy.Current.Execute(null, updateCmd);
        }

        //Good's Description
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"select g.GoodsDescription, g.Category, isnull(cd.UnitID,'') as UnitID, g.NLCode
from KHGoodsHSCode g
left join KHContract_Detail cd on g.NLCode = cd.NLCode
where g.Junk = 0
and cd.ID in (select ID from (select ID,MAX(StartDate) as MaxDate from KHContract where Status = 'Confirmed' group by ID) a)
and Category <> 'MACHINERY'
order by GoodsDescription", "50,10,8,0", this.Text, false, ",", headercaptions: "Good's Description,Category,Unit,");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                IList<DataRow> selectedData = item.GetSelecteds();
                textBox1.Text = item.GetSelectedString();
                CurrentMaintain["NLCode"] = selectedData[0]["NLCode"];
                CurrentMaintain["CustomsUnit"] = selectedData[0]["UnitID"];
            }
        }

        //Good's Description
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox1.OldValue != textBox1.Text)
            {
                if (MyUtility.Check.Empty(textBox1.Text))
                {
                    textBox1.Text = "";
                    CurrentMaintain["NLCode"] = "";
                    CurrentMaintain["CustomsUnit"] = "";
                }
                else
                {
                    DataRow NLCodeDate;
                    if (MyUtility.Check.Seek(string.Format(@"select GoodsDescription,NLCode from KHGoodsHSCode where GoodsDescription = '{0}'", textBox1.Text), out NLCodeDate))
                    {
                        textBox1.Text = textBox1.Text;
                        CurrentMaintain["NLCode"] = NLCodeDate["NLCode"];
                        CurrentMaintain["CustomsUnit"] = MyUtility.GetValue.Lookup(string.Format("select TOP(1) UnitID from KHContract_Detail where NLCode = '{0}' and ID in (select ID from (select ID,MAX(StartDate) as MaxDate from KHContract where Status = 'Confirmed' group by ID) a)", MyUtility.Convert.GetString(NLCodeDate["NLCode"])));
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("The Good's Description is not in the Contract!!");
                        textBox1.Text = "";
                        CurrentMaintain["NLCode"] = "";
                        CurrentMaintain["CustomsUnit"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

    }
}
