using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B41
    /// </summary>
    public partial class B41 : Sci.Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;

        /// <summary>
        /// B41
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.editName = MyUtility.Convert.GetString(this.CurrentMaintain["EditName"]);
            this.editDate = MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]);
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.editDescription.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.CurrentMaintain["NLCodeEditName"] = Sci.Env.User.UserID;
            this.CurrentMaintain["NLCodeEditDate"] = DateTime.Now;
            if (MyUtility.Check.Empty(this.txtNLCode.Text))
            {
                this.txtNLCode.Focus();
                MyUtility.Msg.InfoBox("<Customs Code> can't be empty!!");
                return false;
            }

            if (this.CurrentMaintain["HSCode"].Empty() || this.CurrentMaintain["CustomsUnit"].Empty())
            {
                MyUtility.Msg.InfoBox("HS Code cannot be empty!!" + Environment.NewLine + "Customs Unit cannot be empty!!");
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSavePost()
        {
            string updateCmd;
            if (MyUtility.Check.Empty(this.editDate))
            {
                updateCmd = string.Format("update LocalItem Set EditName = '{0}', EditDate = null where RefNo = '{1}';", this.editName, MyUtility.Convert.GetString(this.CurrentMaintain["RefNo"]));
            }
            else
            {
                updateCmd = string.Format("update LocalItem Set EditName = '{0}', EditDate = '{1}' where RefNo = '{2}';", this.editName, Convert.ToDateTime(this.editDate).ToString("yyyy/MM/dd HH:mm:ss"), MyUtility.Convert.GetString(this.CurrentMaintain["RefNo"]));
            }

            return DBProxy.Current.Execute(null, updateCmd);
        }

        // NL Code
        private void TxtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode",
                "5,11,8",
                this.Text,
                false,
                ",",
                headercaptions: "Customs Code, HSCode, Unit");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.CurrentMaintain["NLCode"] = item.GetSelectedString();
            this.CurrentMaintain["HSCode"] = selectedData[0]["HSCode"];
            this.CurrentMaintain["CustomsUnit"] = selectedData[0]["UnitID"];
            this.CurrentMaintain.EndEdit();
        }

        // NL Code
        private void TxtNLCode_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtNLCode.Text))
            {
                this.CurrentMaintain["NLCode"] = string.Empty;
                this.CurrentMaintain["HSCode"] = string.Empty;
                this.CurrentMaintain["CustomsUnit"] = string.Empty;
            }
            else
            {
                DataRow nLCodeDate;
                if (MyUtility.Check.Seek(
                    string.Format(
                        @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", this.txtNLCode.Text), out nLCodeDate))
                {
                    this.CurrentMaintain["NLCode"] = this.txtNLCode.Text;
                    this.CurrentMaintain["HSCode"] = nLCodeDate["HSCode"];
                    this.CurrentMaintain["CustomsUnit"] = nLCodeDate["UnitID"];
                }
                else
                {
                    this.CurrentMaintain["NLCode"] = string.Empty;
                    this.CurrentMaintain["HSCode"] = string.Empty;
                    this.CurrentMaintain["CustomsUnit"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                    return;
                }
            }

            this.CurrentMaintain.EndEdit();
        }
    }
}
