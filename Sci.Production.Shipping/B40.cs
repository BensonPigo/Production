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
    public partial class B40 : Sci.Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;
        public B40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "F,Fabric,A,Accessory");
            label3.Text = "Description\r\n(Detail)";
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
            comboBox1.ReadOnly = true;
            txtunit1.TextBox1.ReadOnly = true;
            numericBox1.ReadOnly = true;
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
                updateCmd = string.Format("update Fabric Set EditName = '{0}', EditDate = null where SCIRefNo = '{1}';", editName, MyUtility.Convert.GetString(CurrentMaintain["SCIRefNo"]));
            }
            else
            {
                updateCmd = string.Format("update Fabric Set EditName = '{0}', EditDate = '{1}' where SCIRefNo = '{2}';", editName, Convert.ToDateTime(editDate).ToString("yyyy/MM/dd HH:mm:ss"), MyUtility.Convert.GetString(CurrentMaintain["SCIRefNo"]));
            }

            return DBProxy.Current.Execute(null,updateCmd);
        }

        //NL Code
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"select NLCode,HSCode,UnitID
from VNContract_Detail
where ID in (select ID from (select ID,MAX(StartDate) as MaxDate from VNContract where Status = 'Confirmed' group by ID) a)
order by NLCode", "5,11,8", this.Text, false, ",",headercaptions:"NL Code, HSCode, Unit");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> selectedData = item.GetSelecteds();
            CurrentMaintain["NLCode"] = item.GetSelectedString();
            CurrentMaintain["HSCode"] = selectedData[0]["HSCode"];
            CurrentMaintain["CustomsUnit"] = selectedData[0]["UnitID"];
        }

        //NL Code
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox1.OldValue != textBox1.Text)
            {
                if (MyUtility.Check.Empty(textBox1.Text))
                {
                    CurrentMaintain["NLCode"] = "";
                    CurrentMaintain["HSCode"] = "";
                    CurrentMaintain["CustomsUnit"] = "";
                }
                else
                {
                    DataRow NLCodeDate;
                    if (MyUtility.Check.Seek(string.Format(@"select NLCode,HSCode,UnitID
from VNContract_Detail
where ID in (select ID from (select ID,MAX(StartDate) as MaxDate from VNContract where Status = 'Confirmed' group by ID) a)
and NLCode = '{0}'", textBox1.Text), out NLCodeDate))
                    {
                        CurrentMaintain["NLCode"] = textBox1.Text;
                        CurrentMaintain["HSCode"] = NLCodeDate["HSCode"];
                        CurrentMaintain["CustomsUnit"] = NLCodeDate["UnitID"];
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("The NL Code is not in the Contract!!");
                        CurrentMaintain["NLCode"] = "";
                        CurrentMaintain["HSCode"] = "";
                        CurrentMaintain["CustomsUnit"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }
    }
}
