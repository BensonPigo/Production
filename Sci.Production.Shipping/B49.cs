using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B49 : Win.Tems.Input1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B49"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B49(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "NeedDeclare = 1";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.txtSubconSupplier.TextBox1.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.IsSupportEditMode = false;
            base.OnFormLoaded();
        }

        private void TxtCustomerCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CurrentMaintain["NLCode"] = this.txtCustomerCode.Text;
            this.CurrentMaintain["HSCode"] = this.txtCustomerCode.HSCode;
            this.CurrentMaintain["CustomsUnit"] = this.txtCustomerCode.CustomsUnit;
        }

        private void TxtCustomerCode_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["NLCode"] = this.txtCustomerCode.Text;
            this.CurrentMaintain["HSCode"] = this.txtCustomerCode.HSCode;
            this.CurrentMaintain["CustomsUnit"] = this.txtCustomerCode.CustomsUnit;
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
            if (MyUtility.Check.Empty(this.txtCustomerCode.HSCode) ||
               MyUtility.Check.Empty(this.txtNLCode2.HSCode))
            {
                return;
            }

            if (this.txtCustomerCode.HSCode != this.txtNLCode2.HSCode ||
                this.txtCustomerCode.CustomsUnit != this.txtNLCode2.CustomsUnit)
            {
                MyUtility.Msg.InfoBox($@"[Customs Code]({this.txtCustomerCode.HSCode}, {this.txtCustomerCode.CustomsUnit}) and
[Customs Code(2021)]({this.txtNLCode2.HSCode}, {this.txtNLCode2.CustomsUnit})'s <HS Code> or <Customs Unit> are different");
            }
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema pass1Schema;
            var ok = DBProxy.Current.GetTableSchema("Machine", "Misc", out pass1Schema);
            pass1Schema.IsSupportEditDate = false;
            pass1Schema.IsSupportEditName = false;

            decimal pcslength = MyUtility.Check.Empty(this.CurrentMaintain["PcsLength"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsLength"]);
            decimal pcsWidth = MyUtility.Check.Empty(this.CurrentMaintain["PcsWidth"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsWidth"]);
            decimal pcsKG = MyUtility.Check.Empty(this.CurrentMaintain["PcsKg"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsKg"]);
            decimal miscRate = MyUtility.Check.Empty(this.CurrentMaintain["MiscRate"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["MiscRate"]);

            string strUpdate = $@"
update Misc
set UsageUnit = '{this.CurrentMaintain["UsageUnit"]}'
,NLCode  = '{this.CurrentMaintain["NLCode"]}'
,NLCode2  = '{this.CurrentMaintain["NLCode2"]}'
,CustomsUnit = '{this.CurrentMaintain["CustomsUnit"]}'
,HSCode = '{this.CurrentMaintain["HSCode"]}'
,PcsLength = '{pcslength}'
,PcsWidth = '{pcsWidth}'
,PcsKg = '{pcsKG}'
,MiscRate = {miscRate}
,NLCodeEditDate = '{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}'
,NLCodeEditName = '{Env.User.UserID}'
where id='{this.CurrentMaintain["ID"]}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute("Machine", strUpdate)))
            {
                this.ShowErr(result);
            }

            return Ict.Result.True;
        }
    }
}
