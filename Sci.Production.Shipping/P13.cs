using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P13 : Sci.Win.Tems.QueryForm
    {
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeableQty", header: "Chargeable Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCQty", header: "FOC Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeablePulloutQty", header: "Chargeable Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCPulloutQty", header: "FOC Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FinishedFOCStockinQty", header: "Finished FOC Stock-in Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Stock-in Date", iseditingreadonly: true)
                .Numeric("CurrentFOCStock", header: "Current FOC Stock", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;
        }

        private void Find()
        {
            #region 必輸入條件
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Please input <Buyer Delivery>!");
                return;
            }
            #endregion
            this.listControlBindingSource1.DataSource = null;
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                where += $@" and o.id >= '{this.txtSP1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $@" and o.id <= '{this.txtSP2.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                where += $@" and o.brandid = '{this.txtbrand1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $@" and o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("d")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("d")}' ";
            }

            #endregion
            string sqlcmd = $@"
select
	o.FactoryID,
	o.BrandID,
	o.BuyerDelivery,
	OrderID=o.ID,
	o.CustPONo,
	o.StyleID,
	o.SeasonID,
	o.Qty,
	ChargeableQty =o.Qty-o.FOCQty,
	o.FOCQty,
	ChargeablePulloutQty = isnull(c.value,0),
	FOCPulloutQty = isnull(c.value2,0),
	FinishedFOCStockinQty =isnull(oxx.FOCQty,0),
    [StockInDate] = convert(date, oxx.addDate),
	CurrentFOCStock= isnull(oxx.FOCQty,0) - isnull(c2.value,0)
	
from orders o with(nolock)
outer apply(
	select sum(value) as value , sum(value2) as value2 
	from
	(	
		select 
		value = iif(pl.Type <> 'F',sum(pod.ShipQty),0),
		value2=iif( pl.Type='F',sum(pod.ShipQty),0)
		from Pullout_Detail pod with(nolock)
		inner join PackingList pl with(nolock) on pl.ID = pod.PackingListID
		where pod.OrderID = o.ID
		group by pl.Type
	) a
)c
outer apply(
	select FOCQty=sum(ox.FOCQty),addDate=min(addDate) from Order_Finish ox where ox.id = o.ID
)oxx
outer apply(
	select 
		value=iif( pl.Type='F',sum(pod.ShipQty),0)
	from Pullout_Detail pod with(nolock)
	inner join PackingList pl with(nolock) on pl.ID = pod.PackingListID
	where pod.OrderID = o.id
    and pl.pulloutdate > oxx.addDate
	group by pl.Type
)c2


where o.Junk = 0
and exists(select 1 from Order_Finish ox where ox.id = o.ID)
and exists (
	select 1
	from Order_QtyShip_Detail oqd WITH (NOLOCK) 
	left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = oqd.Id and ou1.Article = '----' and ou1.SizeCode = '----' 
	left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = oqd.Id and ou2.Article = oqd.Article and ou2.SizeCode = oqd.SizeCode 
	where oqd.Id = o.id
	and isnull(ou2.POPrice,isnull(ou1.POPrice,-1)) = 0
)--有一筆Price為0表示此Orderid有Foc
{where}
order by o.ID
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }
    }
}
