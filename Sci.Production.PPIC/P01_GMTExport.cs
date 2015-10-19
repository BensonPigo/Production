using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P01_GMTExport : Sci.Win.Subs.Base
    {
        string orderID;
        public P01_GMTExport(string OrderID)
        {
            InitializeComponent();
            orderID = OrderID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable QtyBDown, PackingList;
            string sqlCmd = string.Format(@"select oq.Article,oq.SizeCode,oq.Qty as OrderQty, 
isnull((select sum(ShipQty) from PackingList_Detail where OrderID = oq.ID and Article = oq.Article and SizeCode = oq.SizeCode),0) as PacklistQty, 
isnull((select sum(pd.ShipQty) from PackingList p,PackingList_Detail pd where pd.OrderID = oq.ID and pd.Article = oq.Article and pd.SizeCode = oq.SizeCode and p.ID = pd.ID and p.INVNo <> ''),0) as BookingQty, 
isnull((select sum(pdd.ShipQty) from Pullout_Detail_Detail pdd where pdd.OrderID = oq.ID and pdd.Article = oq.Article and pdd.SizeCode = oq.SizeCode),0) as PulloutQty, 
isnull((select sum(iq.DiffQty) from InvAdjust i,InvAdjust_Qty iq where i.ID = iq.ID and i.OrderID = oq.ID and iq.Article = oq.Article and iq.SizeCode = oq.SizeCode),0) as AdjQty 
from Order_Qty oq 
left join Order_Article oa on oa.id = oq.ID and oa.Article = oq.Article
left join Order_SizeCode os on os.Id = oq.ID and os.SizeCode = oq.SizeCode
where oq.ID = '{0}'
order by oa.Seq,os.Seq",orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out QtyBDown);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query q'ty b'down fail!!");
            }
            listControlBindingSource1.DataSource = QtyBDown;
            object orderQty = QtyBDown.Compute("sum(OrderQty)","");
            object packlistQty = QtyBDown.Compute("sum(PacklistQty)", "");
            object bookingQty = QtyBDown.Compute("sum(BookingQty)", "");
            object pulloutQty = QtyBDown.Compute("sum(PulloutQty)", "");
            object adjQty = QtyBDown.Compute("sum(AdjQty)", "");
            numericBox1.Value = Convert.ToInt32(orderQty);
            numericBox2.Value = Convert.ToInt32(packlistQty);
            numericBox3.Value = Convert.ToInt32(bookingQty);
            numericBox4.Value = Convert.ToInt32(pulloutQty);
            numericBox5.Value = Convert.ToInt32(adjQty);

            sqlCmd = string.Format(@"with tmpPackingList
as (
select *,
(select isnull(sum(iq.DiffQty),0) from Pullout_Detail pd, InvAdjust i, InvAdjust_Qty iq where pd.ID = a.PulloutID and pd.OrderID = '{0}' and pd.UKey = i.Pullout3Ukey and i.ID = iq.ID) as adjQty
from (select pl.ShipModeID,pl.FactoryID,pl.ID,pl.INVNo,pl.PulloutID,p.PulloutDate,pl.CTNQty,sum(pd.ShipQty) as ShipQty
from PackingList pl
inner join PackingList_Detail pd on pl.ID = pd.ID
left join Pullout p on pl.PulloutID = p.ID
where pd.OrderID = '{0}' 
group by pl.ShipModeID,pl.FactoryID,pl.ID,pl.INVNo,pl.PulloutID,p.PulloutDate,pl.CTNQty) a
),
tmpPullout
as(
select *,
(select isnull(sum(iq.DiffQty),0) from Pullout_Detail pd, InvAdjust i, InvAdjust_Qty iq where pd.ID = a.PulloutID and pd.OrderID = '{0}' and pd.UKey = i.Pullout3Ukey and i.ID = iq.ID) as adjQty
from (select '' as ShipModeID,p.FactoryID,'' as ID,'' as INVNo,p.ID as PulloutID, p.PulloutDate,0 as CTNQty, sum(pd.ShipQty) as ShipQty
from Pullout_Detail pd, Pullout p
where pd.OrderID = '{0}'
and pd.ShipQty > 0
and p.ID = pd.ID
and not exists (select 1 from PackingList pl, PackingList_Detail pld where pl.ID = pld.ID and pd.OrderID = pld.OrderID and pl.PulloutID = p.ID)
group by p.FactoryID,p.ID,p.PulloutDate) a
),
tmpInvAdj
as
(
select '' as ShipModeID,''  as FactoryID,'' as ID,i.NegoinvID as INVNo,'' as PulloutID, null as PulloutDate,0 as CTNQty, 0 as ShipQty,sum(iq.DiffQty) as adjQty
from InvAdjust i, InvAdjust_Qty iq
where i.ID = iq.ID
and i.OrderID = '{0}'
and not exists (select 1 from tmpPackingList where INVNo = i.NegoinvID)
group by i.NegoinvID
)
select * from tmpPackingList
union all
select * from tmpPullout
union all
select * from tmpInvAdj", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out PackingList);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query packing list fail!!");
            }
            listControlBindingSource2.DataSource = PackingList;


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
            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(6), settings: orderqty)
                .Numeric("PacklistQty", header: "Packing List Q'ty", width: Widths.AnsiChars(6), settings: packlistqty)
                .Numeric("BookingQty", header: "Booking Q'ty", width: Widths.AnsiChars(6), settings: bookingqty)
                .Numeric("PulloutQty", header: "Pull-out Q'ty", width: Widths.AnsiChars(6), settings: pulloutqty)
                .Numeric("AdjQty", header: "Adj Q'ty", width: Widths.AnsiChars(6), settings: adjqty1);

            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
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
