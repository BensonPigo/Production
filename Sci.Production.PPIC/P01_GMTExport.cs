using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_GMTExport
    /// </summary>
    public partial class P01_GMTExport : Win.Subs.Base
    {
        private string orderID;

        /// <summary>
        /// P01_GMTExport
        /// </summary>
        /// <param name="orderID">string orderID</param>
        public P01_GMTExport(string orderID)
        {
            this.InitializeComponent();
            this.orderID = orderID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable qtyBDown, packingList;
            string sqlCmd = string.Format(
                @"
select oq.Article,oq.SizeCode,oq.Qty as OrderQty, 
isnull((select sum(ShipQty) from PackingList_Detail WITH (NOLOCK) where OrderID = oq.ID and Article = oq.Article and SizeCode = oq.SizeCode),0) as PacklistQty, 
isnull((select sum(pd.ShipQty) from PackingList p WITH (NOLOCK) ,PackingList_Detail pd WITH (NOLOCK) where pd.OrderID = oq.ID and pd.Article = oq.Article and pd.SizeCode = oq.SizeCode and p.ID = pd.ID and p.INVNo <> ''),0) as BookingQty, 
isnull((select sum(pdd.ShipQty) from Pullout_Detail_Detail pdd WITH (NOLOCK) where pdd.OrderID = oq.ID and pdd.Article = oq.Article and pdd.SizeCode = oq.SizeCode),0) as PulloutQty, 
isnull((select sum(iq.DiffQty) from InvAdjust i WITH (NOLOCK) ,InvAdjust_Qty iq WITH (NOLOCK) where i.ID = iq.ID and i.OrderID = oq.ID and iq.Article = oq.Article and iq.SizeCode = oq.SizeCode),0) as AdjQty 
from Order_Qty oq WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = oq.ID 
left join Order_Article oa WITH (NOLOCK) on oa.id = oq.ID and oa.Article = oq.Article
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = oq.SizeCode
where oq.ID = '{0}'
order by oa.Seq,os.Seq", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out qtyBDown);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query q'ty b'down fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = qtyBDown;
            object orderQty = qtyBDown.Compute("sum(OrderQty)", string.Empty);
            object packlistQty = qtyBDown.Compute("sum(PacklistQty)", string.Empty);
            object bookingQty = qtyBDown.Compute("sum(BookingQty)", string.Empty);
            object pulloutQty = qtyBDown.Compute("sum(PulloutQty)", string.Empty);
            object adjQty = qtyBDown.Compute("sum(AdjQty)", string.Empty);
            this.numOrderQty.Value = orderQty.Empty() ? 0 : Convert.ToInt32(orderQty);
            this.numPackingQty.Value = packlistQty.Empty() ? 0 : Convert.ToInt32(packlistQty);
            this.numBookingQty.Value = bookingQty.Empty() ? 0 : Convert.ToInt32(bookingQty);
            this.numPulloutQty.Value = pulloutQty.Empty() ? 0 : Convert.ToInt32(pulloutQty);
            this.numAdjQty.Value = adjQty.Empty() ? 0 : Convert.ToInt32(adjQty);

            sqlCmd = string.Format(
                @"
with tmpPackingList as (
    select *
           , adjQty = (select isnull(sum(iq.DiffQty),0) 
                       from Pullout_Detail pd WITH (NOLOCK) 
                            , InvAdjust i WITH (NOLOCK) 
                            , InvAdjust_Qty iq WITH (NOLOCK) 
                       where pd.ID = a.PulloutID 
                             and pd.OrderID = '{0}' 
                             and pd.UKey = i.Ukey_Pullout 
                             and i.ID = iq.ID) 
    from (
        select pl.ShipModeID
               , pl.FactoryID
               , pl.ID
               , pl.INVNo
               , pl.PulloutID
               , pl.PulloutDate
               , pl.CTNQty
               , ShipQty = sum(pd.ShipQty)
        from PackingList pl WITH (NOLOCK) 
        inner join PackingList_Detail pd WITH (NOLOCK) on pl.ID = pd.ID
        left join Pullout p WITH (NOLOCK) on pl.PulloutID = p.ID
        where pd.OrderID = '{0}' 
        group by pl.ShipModeID,pl.FactoryID,pl.ID,pl.INVNo,pl.PulloutID,pl.PulloutDate,pl.CTNQty
    ) a
),
tmpPullout as(
    select *
           , adjQty = (select isnull(sum(iq.DiffQty),0) 
                       from Pullout_Detail pd WITH (NOLOCK) 
                            , InvAdjust i WITH (NOLOCK) 
                            , InvAdjust_Qty iq WITH (NOLOCK) 
                       where pd.ID = a.PulloutID 
                             and pd.OrderID = '{0}' 
                             and pd.UKey = i.Ukey_Pullout 
                             and i.ID = iq.ID)
    from (
        select ShipModeID = ''
               , p.FactoryID
               , ID = ''
               , INVNo = '' 
               , PulloutID = p.ID
               , p.PulloutDate
               , CTNQty = 0
               , ShipQty = sum(pd.ShipQty)
        from Pullout_Detail pd WITH (NOLOCK) 
             , Pullout p WITH (NOLOCK) 
        where pd.OrderID = '{0}'
              and pd.ShipQty > 0
              and p.ID = pd.ID
              and not exists (
                    select 1 
                    from PackingList pl WITH (NOLOCK) 
                         , PackingList_Detail pld WITH (NOLOCK) 
                    where pl.ID = pld.ID 
                          and pd.OrderID = pld.OrderID 
                          and pl.PulloutID = p.ID)
        group by p.FactoryID, p.ID, p.PulloutDate
    ) a
),
tmpInvAdj as (
    select ShipModeID = ''
           , FactoryID = ''
           , ID = ''
           , INVNo = i.GarmentInvoiceID
           , PulloutID = ''
           , PulloutDate = null
           , CTNQty = 0
           , ShipQty = 0
           , adjQty = sum(iq.DiffQty)
    from InvAdjust i WITH (NOLOCK) 
         , InvAdjust_Qty iq WITH (NOLOCK) 
    where i.ID = iq.ID
          and i.OrderID = '{0}'
          and not exists (
                select 1 
                from tmpPackingList 
                where INVNo = i.GarmentInvoiceID)
    group by i.GarmentInvoiceID
)
select * from tmpPackingList
union all
select * from tmpPullout
union all
select * from tmpInvAdj", this.orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out packingList);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query packing list fail!!" + result.ToString());
            }

            this.listControlBindingSource2.DataSource = packingList;

            DataGridViewGeneratorNumericColumnSettings orderqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings packlistqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings bookingqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings pulloutqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings adjqty1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings shipqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings adjqty2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings ctnqty = new DataGridViewGeneratorNumericColumnSettings();
            orderqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            packlistqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            bookingqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            pulloutqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            adjqty1.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            shipqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            adjqty2.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            ctnqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            // 設定Grid1的顯示欄位
            this.gridQtyBDownByGarmentExport.IsEditingReadOnly = true;
            this.gridQtyBDownByGarmentExport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridQtyBDownByGarmentExport)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(6), settings: orderqty)
                .Numeric("PacklistQty", header: "Packing List Q'ty", width: Widths.AnsiChars(6), settings: packlistqty)
                .Numeric("BookingQty", header: "Booking Q'ty", width: Widths.AnsiChars(6), settings: bookingqty)
                .Numeric("PulloutQty", header: "Pull-out Q'ty", width: Widths.AnsiChars(6), settings: pulloutqty)
                .Numeric("AdjQty", header: "Adj Q'ty", width: Widths.AnsiChars(6), settings: adjqty1);

            // 設定Grid2的顯示欄位
            this.gridPackingListBookingPulloutDetail.IsEditingReadOnly = true;
            this.gridPackingListBookingPulloutDetail.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridPackingListBookingPulloutDetail)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(6))
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5))
                .Text("ID", header: "Packing#", width: Widths.AnsiChars(15))
                .Numeric("CTNQty", header: "#CTN", width: Widths.AnsiChars(5), settings: ctnqty)
                .Text("INVNo", header: "Invoice#", width: Widths.AnsiChars(20))
                .Text("PulloutID", header: "Pull-out#", width: Widths.AnsiChars(15))
                .Date("PulloutDate", header: "Pull-out date", width: Widths.AnsiChars(10))
                .Numeric("ShipQty", header: "Ship Q'ty", width: Widths.AnsiChars(6), settings: shipqty)
                .Numeric("AdjQty", header: "Adj Q'ty", width: Widths.AnsiChars(6), settings: adjqty2);
        }
    }
}
