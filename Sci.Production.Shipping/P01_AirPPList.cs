using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P01_AirPPList
    /// </summary>
    public partial class P01_AirPPList : Sci.Win.Subs.Base
    {
        private string orderID;

        /// <summary>
        /// P01_AirPPList
        /// </summary>
        /// <param name="orderID">orderID</param>
        public P01_AirPPList(string orderID)
        {
            this.InitializeComponent();
            this.Text = this.Text + orderID;
            this.orderID = orderID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string selectCommand = string.Format(
                @"select iif(a.Status = 'Junked','Y','') as Cancel,a.TaskApvDate,a.ID,a.CDate,a.OrderShipmodeSeq,
oq.BuyerDelivery,oq.ShipmodeID,a.ShipQty,a.EstAmount,a.ActualAmount
from AirPP a WITH (NOLOCK) 
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = a.OrderID and oq.Seq = a.OrderShipmodeSeq
where OrderID = '{0}'", this.orderID);
            DataTable selectDataTable;
            DualResult selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
            this.listControlBindingSource1.DataSource = selectDataTable;

            // 設定Grid1的顯示欄位
            this.gridAirPrePaidList.IsEditingReadOnly = true;
            this.gridAirPrePaidList.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAirPrePaidList)
                 .Text("Cancel", header: "Cancel", width: Widths.AnsiChars(2))
                 .Date("TaskApvDate", header: "Lock Date", width: Widths.AnsiChars(10))
                 .Text("ID", header: "Air No.", width: Widths.AnsiChars(13))
                 .Date("CDate", header: "Create Date", width: Widths.AnsiChars(10))
                 .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2))
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10))
                 .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
                 .Numeric("ShipQty", header: "Air Qty")
                 .Numeric("EstAmount", header: "EstAmt", decimal_places: 4)
                 .Numeric("ActualAmount", header: "ActAmt", decimal_places: 2);
        }
    }
}
