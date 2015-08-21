using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P01_AirPPList : Sci.Win.Subs.Base
    {
        string orderID;
        public P01_AirPPList(string OrderID)
        {
            InitializeComponent();
            this.Text = this.Text + OrderID;
            orderID = OrderID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string selectCommand = string.Format(@"select iif(a.Status = 'Junked','Y','') as Cancel,a.TaskApvDate,a.ID,a.CDate,a.OrderShipmodeSeq,
oq.BuyerDelivery,oq.ShipmodeID,a.ShipQty,a.EstAmount,a.ActualAmount
from AirPP a
left join Order_QtyShip oq on oq.Id = a.OrderID and oq.Seq = a.OrderShipmodeSeq
where OrderID = '{0}'", orderID);
            DataTable selectDataTable;
            DualResult selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
            listControlBindingSource1.DataSource = selectDataTable;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
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
