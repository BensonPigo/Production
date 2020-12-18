using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B40
    /// </summary>
    public partial class B40 : Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;

        /// <summary>
        /// B40
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "F,Fabric,A,Accessory");
            this.labelDescription2.Text = "Description\r\n(Detail)";
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
            this.editDescription2.ReadOnly = true;
            this.comboType.ReadOnly = true;
            this.txtUnitUsageUnit.TextBox1.ReadOnly = true;
            this.numUsableWidth.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.CurrentMaintain["NLCodeEditName"] = Env.User.UserID;
            this.CurrentMaintain["NLCodeEditDate"] = DateTime.Now;

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSavePost()
        {
            string updateCmd;
            if (MyUtility.Check.Empty(this.editDate))
            {
                updateCmd = string.Format("update Fabric Set EditName = '{0}', EditDate = null where SCIRefNo = '{1}';", this.editName, MyUtility.Convert.GetString(this.CurrentMaintain["SCIRefNo"]));
            }
            else
            {
                updateCmd = string.Format("update Fabric Set EditName = '{0}', EditDate = '{1}' where SCIRefNo = '{2}';", this.editName, Convert.ToDateTime(this.editDate).ToString("yyyy/MM/dd HH:mm:ss"), MyUtility.Convert.GetString(this.CurrentMaintain["SCIRefNo"]));
            }

            return DBProxy.Current.Execute(null, updateCmd);
        }

        // NL Code
        private void TxtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CurrentMaintain["NLCode"] = this.txtNLCode.Text;
            this.CurrentMaintain["HSCode"] = this.txtNLCode.HSCode;
            this.CurrentMaintain["CustomsUnit"] = this.txtNLCode.CustomsUnit;
        }

        // NL Code
        private void TxtNLCode_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["NLCode"] = this.txtNLCode.Text;
            this.CurrentMaintain["HSCode"] = this.txtNLCode.HSCode;
            this.CurrentMaintain["CustomsUnit"] = this.txtNLCode.CustomsUnit;
            this.CheckHSCode();
        }

        private void TxtNLCode2_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["NLCode2"] = this.txtNLCode2.Text;
            this.CurrentMaintain["HSCode"] = this.txtNLCode2.HSCode;
            this.CurrentMaintain["CustomsUnit"] = this.txtNLCode2.CustomsUnit;
            this.CheckHSCode();
        }

        private void TxtNLCode2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CurrentMaintain["NLCode2"] = this.txtNLCode2.Text;
            this.CurrentMaintain["HSCode"] = this.txtNLCode2.HSCode;
            this.CurrentMaintain["CustomsUnit"] = this.txtNLCode2.CustomsUnit;
        }

        private void CheckHSCode()
        {
            if (MyUtility.Check.Empty(this.txtNLCode.HSCode) ||
               MyUtility.Check.Empty(this.txtNLCode2.HSCode))
            {
                return;
            }

            if (this.txtNLCode.HSCode != this.txtNLCode2.HSCode ||
                this.txtNLCode.CustomsUnit != this.txtNLCode2.CustomsUnit)
            {
                MyUtility.Msg.InfoBox($@"[Customs Code]({this.txtNLCode.HSCode}, {this.txtNLCode.CustomsUnit}) and
[Customs Code(2021)]({this.txtNLCode2.HSCode}, {this.txtNLCode2.CustomsUnit})'s <HS Code> or <Customs Unit> are different");
            }
        }
    }
}
