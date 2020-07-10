using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P22_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P22;
        protected DataRow dr;

        public P22_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"with cte
as (
select pd.id as poid, pd.seq1,pd.seq2,pd.Qty,pd.ShipQty,pd.StockQty,pd.InputQty,pd.OutputQty
	,x.taipei_issue_date,x.taipei_qty,pd.POUnit,pd.StockUnit,st.trans_qty
from dbo.PO_Supp_Detail pd WITH (NOLOCK) 
inner join (select frompoid,fromseq1,fromseq2,sum(qty) trans_qty from dbo.SubTransfer_Detail WITH (NOLOCK) where ID='{0}' 
	group by frompoid,fromseq1,fromseq2) st 
	on st.FromPOID = pd.ID and st.FromSeq1 = pd.SEQ1 and st.FromSeq2 = pd.SEQ2 
inner join dbo.orders o WITH (NOLOCK) on o.id = pd.id
inner join dbo.Factory f WITH (NOLOCK) on f.id = o.FtyGroup
cross apply
	(select max(i.ConfirmDate) taipei_issue_date,sum(i.Qty) taipei_qty
		from dbo.Invtrans i WITH (NOLOCK) 
		where (i.type='1' OR I.TYPE='4') and i.InventoryPOID = pd.ID and i.InventorySeq1 = pd.seq1 and i.InventorySeq2 = pd.SEQ2
	) x
where f.MDivisionID ='{1}' AND X.taipei_qty > 0
)

select  m.poid
        , m.seq1
        , m.seq2
        , m.StockUnit
        , dbo.GetUnitQty(POUnit, StockUnit, m.Qty) as poqty
        , dbo.GetUnitQty(POUnit, StockUnit, m.InputQty) as inputQty
        , dbo.getMtlDesc(poid,seq1,seq2,2,0) as [description]
        , m.taipei_issue_date
        , m.taipei_qty
        , m.POUnit
        , accu_qty
        , m.trans_qty	
from cte m 
cross apply
(select isnull(sum(qty) ,0) as accu_qty
	from (
		select sum(r2.StockQty) as qty from dbo.Receiving r1 WITH (NOLOCK) inner join dbo.Receiving_Detail r2 WITH (NOLOCK) on r2.Id= r1.Id 
			where r1.Status ='Confirmed' and r2.StockType = 'I' 
				and r2.PoId = m.poid and r2.seq1 = m.seq1 and r2.seq2 = m.seq2
		union 
		select sum(s2.Qty) as qty from dbo.SubTransfer s1 WITH (NOLOCK) inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
			where s1.type ='A' and s1.Status ='Confirmed' and s2.ToStockType = 'I' 
				and s2.ToPOID = m.poid and s2.ToSeq1 = m.seq1 and s2.ToSeq2 = m.seq2 and s1.Id !='{0}'
		) xx
  ) xxx

", this.dr["id"], this.dr["mdivisionid"]));

            DataTable selectDataTable1;
            this.P22.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P22.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("poqty", header: "PO Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("inputqty", header: "Input Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("taipei_qty", header: "Taipei Input", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("accu_qty", header: "Accu. Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Numeric("trans_qty", header: "Transfer Qty", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 2)
                 .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(10))
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(30))
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
