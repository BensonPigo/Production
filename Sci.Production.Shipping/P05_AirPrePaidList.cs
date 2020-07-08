using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_AirPrePaidList
    /// </summary>
    public partial class P05_AirPrePaidList : Sci.Win.Subs.Base
    {
        private string gmtBookingID;
        private DualResult result;
        private DataTable gridData;

        /// <summary>
        /// P05_AirPrePaidList
        /// </summary>
        /// <param name="gMTBookingID">gMTBookingID</param>
        public P05_AirPrePaidList(string gMTBookingID)
        {
            this.InitializeComponent();
            this.gmtBookingID = gMTBookingID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlCmd = string.Format(
                @"select p.ID as PackID,p.OrderID,p.OrderShipmodeSeq,isnull(a.ID,'') as AirPPID
from (select distinct pd.ID,pd.OrderID,pd.OrderShipmodeSeq 
      from PackingList p WITH (NOLOCK) , PackingList_Detail pd WITH (NOLOCK) 
	  where p.INVNo = '{0}' and p.ID = pd.ID) p
left join AirPP a WITH (NOLOCK) on a.OrderID = p.OrderID and a.OrderShipmodeSeq = p.OrderShipmodeSeq", this.gmtBookingID);

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
                .Text("PackID", header: "Packing No.", width: Widths.AnsiChars(13))
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("AirPPID", header: "Air Pre-Paid No.", width: Widths.AnsiChars(13));
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
