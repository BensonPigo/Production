using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P11_TransferInOutInformation : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P11_TransferInOutInformation()
        {
            this.InitializeComponent();
        }

        private DataSet ds;

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.grid1)
                 .Text("ID", header: "SP#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("Qty", header: "Order Ttl Qty", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("IsSet", header: "Is Set", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("IsCancelOrder", header: "Is Cancel Order", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("IsBuyback", header: "Is Buyback", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 ;
            this.Helper.Controls.Grid.Generator(this.grid2)
                 .Text("ComboType", header: "Product\r\nType", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("BuybackQty", header: "Buyback\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("CancelQty", header: "Cancel\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("QAQty", header: "Ori Sewing Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("TransferInQty", header: "Transfer-In\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("TransferOutQty", header: "Transfer-Out\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("SewingQty", header: "Sewing\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;
            this.Helper.Controls.Grid.Generator(this.grid3)
                 .Text("Transfer_Type", header: "Transfer Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("FromToOrderID", header: "From/To SP#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Numeric("BuybackQty", header: "Buyback\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("CancelQty", header: "Cancel\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("TransferQty", header: "Transfer\r\nQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;
            this.listControlBindingSource3.DataSource = null;

            if (MyUtility.Check.Empty(this.txtsp_from.Text))
            {
                MyUtility.Msg.WarningBox("Please input < SP#>!");
                return;
            }

            List<SqlParameter> sqls = new List<SqlParameter>() { new SqlParameter("@OrderID", this.txtsp_from.Text) };
            string sqlcmd = $@"
Declare @SP varchar(13) = @OrderID

select
	o.ID
	, Qty = isnull(O.Qty, 0)
	, IsSet = iif(o.StyleUnit = 'PCS', 'N', 'Y')
	, IsCancelOrder = iif(o.Junk = 0, 'N', 'Y')
	, IsBuyback = iif(o.IsBuyBack = 0, 'N', 'Y')
from orders o with(nolock)
where o.POID in (select o2.POID from orders o2 where o2.id=@SP)
and o.FtyGroup = '{Env.User.Factory}'
order by o.ID

select
	Transfer_Type = 'In' 
	, FromToOrderID = sotd.FromOrderID
	, OrderID = sotd.ToOrderID
	, buybackQty = isnull(buybackQty_in.buybackQty, 0)
	, CancelQty = isnull(CancelQty_in.CancelQty, 0)
	, TransferQty = isnull(sotd.TransferQty, 0)
	, TransferInQty = isnull(sotd.TransferQty, 0)
	, TransferOutQty = 0
	, ComboType = sotd.ToComboType
	, Article = sotd.ToArticle
	, SizeCode = sotd.ToSizeCode
into #tmp
from SewingOutputTransfer sot
inner join SewingOutputTransfer_Detail sotd on sot.id=sotd.id
outer apply (
	select buybackQty = sum(obbq.Qty)
	from Order_BuyBack_Qty obbq
	where obbq.OrderIDFrom = sotd.FromOrderID
	and obbq.id = sotd.ToOrderID
	and obbq.Article = sotd.Article
	and obbq.SizeCode =sotd.SizeCode
) buybackQty_in
outer apply (
	select CancelQty = sum(oq.Qty)
	from orders ord
	inner join order_qty oq on ord.id = oq.ID
	where ord.ID = sotd.FromOrderID
	and ord.Junk =1
	and oq.Article = sotd.Article
	and oq.SizeCode = sotd.SizeCode
) CancelQty_in
where 1=1
and sot.Status ='Confirmed'
and sot.FactoryID = '{Env.User.Factory}'
and sotd.ToOrderID in (select distinct o3.id from Orders o2 inner join Orders o3 on o3.POID = o2.POID where o2.id=@SP)

union all

select
	Transfer_Type = 'Out'
	, sotd.ToOrderID
	, sotd.FromOrderID
	, buybackQty = isnull(buybackQty_Out.buybackQty, 0)
	, CancelQty = isnull(CancelQty_Out.CancelQty, 0)
	, TransferQty = isnull(sotd.TransferQty, 0)
	, TransferInQty = 0
	, TransferOutQty = isnull(sotd.TransferQty, 0)
	, sotd.FromComboType
	, sotd.Article
	, sotd.SizeCode
from SewingOutputTransfer sot
inner join SewingOutputTransfer_Detail sotd on sot.id=sotd.id
outer apply (
	select buybackQty = sum(obbq.Qty)
	from Order_BuyBack_Qty obbq
	where obbq.OrderIDFrom = sotd.ToOrderID
	and obbq.id = sotd.FromOrderID
	and obbq.Article = sotd.Article
	and obbq.SizeCode = sotd.SizeCode
) buybackQty_Out
outer apply (
	select CancelQty = sum(oq.Qty)
	from orders ord
	inner join order_qty oq on ord.id=oq.ID
	where 1=1
	and ord.ID =sotd.ToOrderID
	and ord.Junk =1
	and oq.Article = sotd.Article
	and oq.SizeCode =sotd.SizeCode
) CancelQty_Out
where 1=1
and sot.Status ='Confirmed'
and sot.FactoryID = '{Env.User.Factory}'
and sotd.FromOrderID in (select distinct o3.id from Orders o2 inner join Orders o3 on o3.POID = o2.POID where o2.id=@SP)
order by Transfer_Type, FromOrderID

select d.*, QAQty = isnull(g.QAQty, 0) + d.TransferOutQty - d.TransferInQty , SewingQty = isnull(g.QAQty, 0)
into #tmp2
from(
	select t.OrderID, t.ComboType, t.Article, t.SizeCode, TransferInQty = SUM(TransferInQty) , TransferOutQty = SUM(TransferOutQty)
	from #tmp t
	group by  t.OrderID, t.ComboType, t.Article, t.SizeCode
)d
left join(
	select sodd.OrderId, sodd.ComboType, sodd.Article, sodd.SizeCode, QAQty = SUM(sodd.QAQty)
	from orders o with(nolock)
	inner join SewingOutput_Detail_Detail sodd with(nolock) on o.id = sodd.OrderId
	where o.POID in (select o2.POID from orders o2 where o2.id=@SP)
    --and o.FtyGroup = 'MAI'
	group by sodd.OrderId, sodd.ComboType, sodd.Article, sodd.SizeCode, o.Junk
)g on d.OrderID = g.OrderId and d.ComboType = g.ComboType and d.Article = g.Article and d.SizeCode = g.SizeCode

union
select x.OrderId, x.ComboType, x.Article, x.SizeCode, TransferInQty = isnull(TransferInQty.TransferInQty, 0), TransferOutQty = isnull(TransferOutQty.TransferOutQty, 0),
	QAQty = x.QAQty + isnull(TransferOutQty.TransferOutQty, 0) - isnull(TransferInQty.TransferInQty, 0), x.SewingQty
from(
	select
		sodd.OrderId
		, sodd.ComboType
		, sodd.Article
		, sodd.SizeCode
		, QAQty = isnull(sum(sodd.QAQty), 0)
		, SewingQty = isnull(sum(sodd.QAQty), 0)
	from SewingOutput_Detail_Detail sodd
	where sodd.OrderId in (select distinct o3.id from Orders o2 inner join Orders o3 on o3.POID = o2.POID where o2.id=@SP)	group by sodd.OrderId, sodd.ComboType, sodd.Article, sodd.SizeCode)x
outer apply (
	select TransferInQty = sum(sotd.TransferQty)
	from SewingOutputTransfer_Detail sotd
	inner join SewingOutputTransfer sot on sotd.id=sot.ID
	where sot.Status ='Confirmed'
	and sotd.ToOrderID = x.OrderId
	and sotd.ToComboType  = x.ComboType
	and sotd.ToArticle  = x.Article
	and sotd.ToSizeCode  = x.SizeCode
) TransferInQty
outer apply (
	select TransferOutQty = sum(sotd.TransferQty)
	from SewingOutputTransfer_Detail sotd
	inner join SewingOutputTransfer sot on sotd.id=sot.ID
	where sot.Status ='Confirmed'
	and sotd.FromOrderID = x.OrderId
	and sotd.FromComboType = x.ComboType
	and sotd.Article = x.Article
	and sotd.SizeCode  = x.SizeCode
) TransferOutQty

select t.*, buybackQty = ISNULL(buybackQty, 0), CancelQty = ISNULL(CancelQty, 0)
from #tmp2 t
outer apply (
	select buybackQty = sum(obbq.Qty)
	from Order_BuyBack_Qty obbq
	where obbq.id = t.OrderId
	and obbq.Article = t.Article
	and obbq.SizeCode =t.SizeCode
) buybackQty
outer apply (
	select CancelQty = sum(oq.Qty)
	from orders o
	inner join order_qty oq on o.id=oq.ID
	where 1=1
	and o.ID =t.OrderId
	and o.Junk =1
	and oq.Article = t.Article
	and oq.SizeCode =t.SizeCode
) CancelQty

select* from #tmp

drop table #tmp,#tmp2
";
            if (!SQL.Selects(string.Empty, sqlcmd, out this.ds, sqls))
            {
                MyUtility.Msg.WarningBox(sqlcmd, "DB error!!");
                return;
            }

            if (this.ds.Tables[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = this.ds.Tables[0];
            this.listControlBindingSource2.DataSource = this.ds.Tables[1];
            this.listControlBindingSource3.DataSource = this.ds.Tables[2];

            this.grid1.ColumnsAutoSize();
            this.Grid1_SelectionChanged();
            this.Grid2_SelectionChanged();
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            this.Grid1_SelectionChanged();
            this.Grid2_SelectionChanged();
        }

        private void Grid2_SelectionChanged(object sender, EventArgs e)
        {
            this.Grid2_SelectionChanged();
        }

        private void Grid1_SelectionChanged()
        {
            if (this.grid1.CurrentDataRow == null || this.ds == null)
            {
                this.ds.Tables[1].DefaultView.RowFilter = "1=0";
                return;
            }

            string filter = $"OrderId ='{this.grid1.CurrentDataRow["ID"]}'";
            this.ds.Tables[1].DefaultView.RowFilter = filter;

            this.GridViewColumnsAutoSize();
        }

        private void Grid2_SelectionChanged()
        {
            if (this.grid1.CurrentDataRow == null || this.grid2.CurrentDataRow == null || this.ds == null)
            {
                this.ds.Tables[2].DefaultView.RowFilter = "1=0";
                return;
            }

            DataRow dr = this.grid2.CurrentDataRow;
            string filter = $"OrderID ='{dr["OrderId"]}' and ComboType ='{dr["ComboType"]}' and Article ='{dr["Article"]}' and SizeCode ='{dr["SizeCode"]}'";
            this.ds.Tables[2].DefaultView.RowFilter = filter;

            this.GridViewColumnsAutoSize();
        }

        private void GridViewColumnsAutoSize()
        {
            this.grid2.ColumnsAutoSize();
            this.grid3.ColumnsAutoSize();
        }
    }
}
