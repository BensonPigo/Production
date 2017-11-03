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
    public partial class B41 : Sci.Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;
        public B41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
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
            editDescription.ReadOnly = true;
            txtSubconSupplier.TextBox1.ReadOnly = true;
            checkJunk.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            CurrentMaintain["NLCodeEditName"] = Sci.Env.User.UserID;
            CurrentMaintain["NLCodeEditDate"] = DateTime.Now;
            if (MyUtility.Check.Empty(this.txtNLCode.Text))
            {
                this.txtNLCode.Focus();
                MyUtility.Msg.InfoBox("<Customs Code> can't be empty!!");
                return false;
            }
            
            if (CurrentMaintain["HSCode"].Empty() || CurrentMaintain["CustomsUnit"].Empty())
            {
                MyUtility.Msg.InfoBox("HS Code cannot be empty!!" + Environment.NewLine + "Customs Unit cannot be empty!!");
            }

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

        //NL Code
        private void txtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(@"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode", "5,11,8", this.Text, false, ",", headercaptions: "Customs Code, HSCode, Unit");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            IList<DataRow> selectedData = item.GetSelecteds();
            CurrentMaintain["NLCode"] = item.GetSelectedString();
            CurrentMaintain["HSCode"] = selectedData[0]["HSCode"];
            CurrentMaintain["CustomsUnit"] = selectedData[0]["UnitID"];
            CurrentMaintain.EndEdit();
        }

        //NL Code
        private void txtNLCode_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtNLCode.OldValue != txtNLCode.Text)
            {
                if (MyUtility.Check.Empty(txtNLCode.Text))
                {
                    CurrentMaintain["NLCode"] = "";
                    CurrentMaintain["HSCode"] = "";
                    CurrentMaintain["CustomsUnit"] = "";
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
                        CurrentMaintain["CustomsUnit"] = NLCodeDate["UnitID"];
                    }
                    else
                    {
                        CurrentMaintain["NLCode"] = "";
                        CurrentMaintain["HSCode"] = "";
                        CurrentMaintain["CustomsUnit"] = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                        return;
                    }
                }
                CurrentMaintain.EndEdit();
            }
        }
    }
}
