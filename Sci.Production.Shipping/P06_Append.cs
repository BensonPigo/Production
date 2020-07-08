using System;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06_Append
    /// </summary>
    public partial class P06_Append : Sci.Win.Subs.Base
    {
        private DateTime pulloutDate;

        /// <summary>
        /// PulloutDate
        /// </summary>
        public DateTime PulloutDate
        {
            get
            {
                return this.pulloutDate;
            }

            set
            {
                this.pulloutDate = value;
            }
        }

        /// <summary>
        /// P06_Append
        /// </summary>
        public P06_Append()
        {
            this.InitializeComponent();
        }

        // OK
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.datePulloutDate.Value))
            {
                MyUtility.Msg.WarningBox("Pull-out Date can't empty!!");
                return;
            }

            this.PulloutDate = (DateTime)this.datePulloutDate.Value;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
