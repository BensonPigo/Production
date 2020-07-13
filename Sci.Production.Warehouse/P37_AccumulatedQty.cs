using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P37_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P37;
        protected DataRow dr;

        public P37_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @";with group_by 
as
(select b.PoId,b.Seq1,b.Seq2
,sum(b.Qty) as Qty
from dbo.ReturnReceipt a WITH (NOLOCK) inner join dbo.ReturnReceipt_Detail b WITH (NOLOCK) on a.ID = b.ID
where a.Id = '{0}'
GROUP BY b.PoId,b.Seq1,b.Seq2
)
,
cte as
(
	select d.POID,d.Seq1,d.Seq2,sum(d.qty) accu_qty
	from dbo.ReturnReceipt c WITH (NOLOCK) inner join dbo.ReturnReceipt_Detail d WITH (NOLOCK) on c.Id= d.Id
	inner join group_by f WITH (NOLOCK) on f.POID = d.POID and f.Seq1= d.Seq1 and f.seq2 = d.Seq2
	where c.id != '{0}' and c.Status = 'Confirmed'
	group by d.POID,d.Seq1,d.Seq2
)
select group_by.POID,group_by.seq1,group_by.seq2
,(select InQty from MDivisionPoDetail m WITH (NOLOCK) where m.POID = group_by.POID and m.seq1 =group_by.seq1 and m.seq2= group_by.Seq2) inqty
,isnull(cte.accu_qty,0.00) accu_qty
,group_by.Qty
,dbo.getMtlDesc(group_by.poid,group_by.seq1,group_by.seq2,2,0) [description]
from group_by left join cte on cte.POID = group_by.POID and cte.Seq1= group_by.Seq1 and cte.seq2 = group_by.Seq2", this.dr["id"]));

            DataTable selectDataTable1;
            this.P37.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P37.HideWaitMessage();
            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("inqty", header: "Rcv. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("accu_qty", header: "Accu. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("qty", header: "Return Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
