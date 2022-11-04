﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P13 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P13"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeableQty", header: "Chargeable Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCQty", header: "FOC Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeablePulloutQty", header: "Chargeable Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCPulloutQty", header: "FOC Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FinishedFOCStockinQty", header: "Finished FOC Stock-in Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("StockInDate", header: "Stock-in Date", iseditingreadonly: true)
                .Numeric("CurrentFOCStock", header: "Current FOC Stock", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;
        }

        private void Find()
        {
            #region 必輸入條件
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.txtSP1.Text) &&
                MyUtility.Check.Empty(this.txtSP2.Text))
            {
                MyUtility.Msg.WarningBox("Please input <SP#> or <Buyer Delivery> !");
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
                where += $@" and o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd")}' ";
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
	ChargeablePulloutQty = isnull(ShipQty_ByType.TotalNotFocShipQty,0),
	FOCPulloutQty = isnull(ShipQty_ByType.TotalFocShipQty,0),
	FinishedFOCStockinQty =isnull(oxx.FOCQty,0),
    [StockInDate] = convert(date, oxx.addDate),
	CurrentFOCStock= dbo.GetFocStockByOrder(o.ID)
    ,o.OrderTypeID
from orders o with(nolock)
outer apply(
	select sum(TotalNotFocShipQty) as TotalNotFocShipQty , sum(TotalFocShipQty) as TotalFocShipQty 
	from
	(	
		select 
        [TotalNotFocShipQty] = iif(p.Type <> 'F', sum(pd.ShipQty),0),
        [TotalFocShipQty] = iif(p.Type = 'F',sum(pd.ShipQty),0)
        from PackingList_Detail pd WITH (NOLOCK)
        inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
        where pd.OrderId = o.ID
        and p.PulloutID <> ''
        group by p.Type
	) a
)ShipQty_ByType
outer apply(
	select FOCQty=sum(ox.FOCQty),addDate=min(addDate) from Order_Finish ox where ox.id = o.ID
)oxx

where 1=1
and exists(select 1 from Order_Finish ox where ox.id = o.ID)
{where}
order by o.ID
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 『系統紀錄 P12 完成入庫的庫存』小於『當下應有的庫存』
            if (this.chkOnlyShow.Checked)
            {
                var query = dt.AsEnumerable().Where(x => x.Field<int>("FinishedFOCStockinQty") < x.Field<int>("CurrentFOCStock"));
                if (query.Any())
                {
                    dt = query.CopyToDataTable();
                }
                else
                {
                    dt = dt.Clone();
                }
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }
    }
}
