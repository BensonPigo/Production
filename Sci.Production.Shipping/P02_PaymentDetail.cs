using System;
using System.Data;

namespace Sci.Production.Shipping
{
    public partial class P02_PaymentDetail : Win.Tems.QueryForm
    {
        private DataRow row;

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_PaymentDetail"/> class.
        /// </summary>
        /// <param name="master"></param>
        public P02_PaymentDetail(DataRow master)
        {
            this.InitializeComponent();
            this.row = master;
        }

        /// <inheritdoc/>
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
