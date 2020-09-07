using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P23_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P23;
        protected DataRow dr;

        public P23_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"
;with cte
as (
select pd.id as poid, pd.seq1,pd.seq2,st.ToStockType,pd.Qty,pd.ShipQty,pd.StockQty,pd.InputQty,pd.OutputQty
	,x.taipei_issue_date,x.taipei_qty,pd.POUnit,pd.StockUnit,st.trans_qty
from dbo.PO_Supp_Detail pd WITH (NOLOCK) 
inner join (select ToPOID,ToSeq1,ToSeq2,ToStockType,sum(qty) trans_qty from dbo.SubTransfer_Detail WITH (NOLOCK) where ID='{0}' 
	group by ToPOID,ToSeq1,ToSeq2,ToStockType) st 
on st.ToPOID = pd.ID and st.ToSeq1 = pd.SEQ1 and st.ToSeq2 = pd.SEQ2 
--inner join dbo.orders o on o.id = pd.id  --bug fix:352:WAREHOUSE_P23_AccumulatedQty_Accumulate qty
cross apply
	(select max(i.ConfirmDate) taipei_issue_date,sum(iif(i.type='2',i.Qty,0-i.qty)) taipei_qty
		from dbo.Invtrans i WITH (NOLOCK) inner join dbo.Factory f WITH (NOLOCK) on f.ID = i.FactoryID and f.MDivisionID = '{1}'
		where (i.type='2' OR I.TYPE='6') and i.InventoryPOID = pd.StockPOID and i.InventorySeq1 = pd.Stockseq1 and i.InventorySeq2 = pd.StockSEQ2 and i.PoID = pd.ID
                and i.seq70seq1 = pd.seq1
                and i.seq70seq2 = pd.seq2
	) x
)

select  m.poid
        , m.seq1
        , m.seq2
        , m.StockUnit
        , dbo.GetUnitQty(POUnit, StockUnit, m.Qty) as poqty
        , dbo.GetUnitQty(POUnit, StockUnit, m.InputQty) as inputQty
        , dbo.getMtlDesc(poid,seq1,seq2,2,0) as [description]
        , m.taipei_issue_date
        , dbo.GetUnitQty(POUnit, StockUnit, m.taipei_qty) as taipei_qty
        , m.POUnit
        , accu_qty
        ,  m.trans_qty	
        , [balanceqty] = m.trans_qty + isnull(accu_qty,0) - dbo.GetUnitQty(POUnit, StockUnit, isnull(m.taipei_qty,0))
from cte m 
cross apply
(
select sum(qty) accu_qty from (
	select sum(r2.Qty) as qty from dbo.TransferIn r1 WITH (NOLOCK) inner join dbo.TransferIn_Detail r2 WITH (NOLOCK) on r2.Id= r1.Id 
				where r1.Status ='Confirmed' and r2.StockType = 'B' 
					and r2.PoId = m.poid and r2.seq1 = m.seq1 and r2.seq2 = m.seq2
			union all
	select sum(s2.Qty) qty from dbo.SubTransfer s1 WITH (NOLOCK) inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
				where s1.type ='B' and s1.Status ='Confirmed' and s2.ToStockType = m.ToStockType
					and s2.ToPOID = m.poid and s2.ToSeq1 = m.seq1 and s2.ToSeq2 = m.seq2 and s1.Id !='{0}'
	)xx
) xxx
", this.dr["id"], this.dr["mdivisionid"]));

            DataTable selectDataTable1;
            this.P23.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P23.HideWaitMessage();

            // selectDataTable1.Columns.Add("balanceqty", typeof(decimal), "Taipei_qty - accu_qty - trans_qty");
            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("poqty", header: "PO Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)

                 // .Numeric("inputqty", header: "Input Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("taipei_qty", header: "Taipei Output", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("accu_qty", header: "Accu. Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("trans_qty", header: "Transfer Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("balanceqty", header: "Variance", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(10))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(30))
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
