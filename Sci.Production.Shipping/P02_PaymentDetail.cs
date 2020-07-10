using System;
using System.Data;

namespace Sci.Production.Shipping
{
    public partial class P02_PaymentDetail : Win.Tems.QueryForm
    {
        private DataRow row;

        public P02_PaymentDetail(DataRow master)
        {
            this.InitializeComponent();
            this.row = master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DateTime? payDate = MyUtility.Convert.GetDate(this.row["PayDate"]);
            this.datePayDate.Text = payDate.HasValue ? payDate.Value.ToString("yyyy/MM/dd") : string.Empty;
            this.displayAmount.Text = this.row["CurrencyID"].ToString();
            this.numAmount.Text = this.row["Amount"].ToString();
            this.displayInvNo.Text = this.row["InvNo"].ToString();
        }
    }
}
