using System;
using static Sci.Production.Basic.B04_BankDetail;

namespace Sci.Production.Basic
{
    /// <inheritdoc/>
    public partial class B04_BankData_DetailInput : Win.Subs.Base
    {
        /// <inheritdoc/>
        public LocalSupp_Bank_Detail Detail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="B04_BankData_DetailInput"/> class.
        /// </summary>
        /// <param name="detail">LocalSupp_Bank_Detail</param>
        public B04_BankData_DetailInput(LocalSupp_Bank_Detail detail)
        {
            this.InitializeComponent();
            this.Detail = detail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.checkDefault.Checked = this.Detail.IsDefault;
            this.txtAccountNo.Text = this.Detail.AccountNo;
            this.txtAccountName.Text = this.Detail.AccountName;
            this.txtBankName.Text = this.Detail.BankName;
            this.txtBranchCode.Text = this.Detail.BranchCode;
            this.txtBranchName.Text = this.Detail.BranchName;
            this.txtCountry.TextBox1.Text = this.Detail.CountryID;

            // this.txtAlias.Text = this._detail.Alias;
            this.txtCity.Text = this.Detail.City;
            this.txtSWIFTCode.Text = this.Detail.SWIFTCode;
            this.txtMidSWIFTCode.Text = this.Detail.MidSWIFTCode;
            this.txtMidBankName.Text = this.Detail.MidBankName;
            this.txtRemark.Text = this.Detail.Remark;
        }

        /// <inheritdoc/>
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            this.Detail.IsDefault = this.checkDefault.Checked;
            this.Detail.AccountNo = this.txtAccountNo.Text;
            this.Detail.AccountName = this.txtAccountName.Text;
            this.Detail.BankName = this.txtBankName.Text;
            this.Detail.BranchCode = this.txtBranchCode.Text;
            this.Detail.BranchName = this.txtBranchName.Text;
            this.Detail.CountryID = this.txtCountry.TextBox1.Text;
            this.Detail.Alias = MyUtility.GetValue.Lookup($"SELECT Alias FROM Country WHERE ID='{this.txtCountry.TextBox1.Text}'");
            this.Detail.City = this.txtCity.Text;
            this.Detail.SWIFTCode = this.txtSWIFTCode.Text;
            this.Detail.MidSWIFTCode = this.txtMidSWIFTCode.Text;
            this.Detail.MidBankName = this.txtMidBankName.Text;
            this.Detail.Remark = this.txtRemark.Text;

            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Detail = null;
            this.Close();
        }
    }
}
