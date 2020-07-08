using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_AirPrePaidStatus
    /// </summary>
    public partial class P05_AirPrePaidStatus : Win.Subs.Base
    {
        private DualResult result;
        private DataTable gridData;

        /// <summary>
        /// P05_AirPrePaidStatus
        /// </summary>
        public P05_AirPrePaidStatus()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlCmd = @"select status,followup from AirPPStatus WITH (NOLOCK)";

            if (this.result = DBProxy.Current.Select(null, sqlCmd, out this.gridData))
            {
                this.listControlBindingSource1.DataSource = this.gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
            }

            // Grid設定
            this.gridAirPrePaidList.IsEditingReadOnly = true;
            this.gridAirPrePaidList.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAirPrePaidList)
                .Text("status", header: "Air-Prepaid Status", width: Widths.AnsiChars(13))
                .Text("followup", header: "Follow up", width: Widths.AnsiChars(65));
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
