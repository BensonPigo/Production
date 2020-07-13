using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_QtyBreakDown
    /// </summary>
    public partial class P05_QtyBreakDown : Win.Subs.Input6A
    {
        /// <summary>
        /// P05_QtyBreakDown
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P05_QtyBreakDown(DataRow masterData)
        {
            this.InitializeComponent();
            this.displayInvoice.Text = MyUtility.Convert.GetString(masterData["ID"]);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridQtyBreakDown.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDown)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5))
                .Numeric("OrderQty", header: "Order Q'ty")
                .Numeric("ShipQty", header: "Q'ty")
                .Numeric("POPrice", header: "FOB", decimal_places: 2)
                .Numeric("Surcharge", header: "Surcharge", decimal_places: 4);
        }

        /// <inheritdoc/>
        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);
            string sqlCmd = string.Format(
                @"select a.OrderID,a.OrderShipmodeSeq,a.Article,a.SizeCode,oqd.Qty as OrderQty,a.ShipQty,isnull(ou2.POPrice,ou1.POPrice) as POPrice,
(select round(sum(iif(os.PriceType = '1',(os.Price/o.Qty),os.Price)),4)
from Order_Surcharge os WITH (NOLOCK) , Orders o WITH (NOLOCK) 
where os.Id = o.ID
and o.ID = a.OrderID) as Surcharge
from (select pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode,sum(pd.ShipQty) as ShipQty
      from PackingList_Detail pd WITH (NOLOCK) 
      where pd.ID = '{0}'
      group by pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode) a
left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = a.OrderID and ou1.Article = '----' and ou1.SizeCode = '----'
left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = a.OrderID and ou2.Article = a.Article and ou2.SizeCode = a.SizeCode
left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = a.OrderID and oqd.Seq = a.OrderShipmodeSeq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode
left join Orders o WITH (NOLOCK) on o.ID = a.OrderID
left join Order_Article oa WITH (NOLOCK) on oa.ID = o.POID and oa.Article = a.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = o.POID and os.SizeCode = a.SizeCode
order by a.OrderID,a.OrderShipmodeSeq,oa.Seq,os.Seq", this.displayPacking.Text);
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
