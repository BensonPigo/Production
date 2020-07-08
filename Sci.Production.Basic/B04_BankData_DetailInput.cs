using System;
using static Sci.Production.Basic.B04_BankDetail;

namespace Sci.Production.Basic
{
    public partial class B04_BankData_DetailInput : Sci.Win.Subs.Base
    {
        public LocalSupp_Bank_Detail _detail;

        public B04_BankData_DetailInput(LocalSupp_Bank_Detail detail)
        {
            this.InitializeComponent();
            this._detail = detail;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.checkDefault.Checked = this._detail.IsDefault;
            this.txtAccountNo.Text = this._detail.AccountNo;
            this.txtAccountName.Text = this._detail.AccountName;
            this.txtBankName.Text = this._detail.BankName;
            this.txtBranchCode.Text = this._detail.BranchCode;
            this.txtBranchName.Text = this._detail.BranchName;
            this.txtCountry.TextBox1.Text = this._detail.CountryID;

            // this.txtAlias.Text = this._detail.Alias;
            this.txtCity.Text = this._detail.City;
            this.txtSWIFTCode.Text = this._detail.SWIFTCode;
            this.txtMidSWIFTCode.Text = this._detail.MidSWIFTCode;
            this.txtMidBankName.Text = this._detail.MidBankName;
            this.txtRemark.Text = this._detail.Remark;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            this._detail.IsDefault = this.checkDefault.Checked;
            this._detail.AccountNo = this.txtAccountNo.Text;
            this._detail.AccountName = this.txtAccountName.Text;
            this._detail.BankName = this.txtBankName.Text;
            this._detail.BranchCode = this.txtBranchCode.Text;
            this._detail.BranchName = this.txtBranchName.Text;
            this._detail.CountryID = this.txtCountry.TextBox1.Text;
            this._detail.Alias = MyUtility.GetValue.Lookup($"SELECT Alias FROM Country WHERE ID='{this.txtCountry.TextBox1.Text}'");
            this._detail.City = this.txtCity.Text;
            this._detail.SWIFTCode = this.txtSWIFTCode.Text;
            this._detail.MidSWIFTCode = this.txtMidSWIFTCode.Text;
            this._detail.MidBankName = this.txtMidBankName.Text;
            this._detail.Remark = this.txtRemark.Text;

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this._detail = null;
            this.Close();
        }
    }
}
