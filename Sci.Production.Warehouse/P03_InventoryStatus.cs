using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_InventoryStatus : Win.Subs.Base
    {
        DataRow dr;

        public P03_InventoryStatus(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += " (" + this.dr["id"].ToString() + "-" + this.dr["seq1"].ToString() + "-" + this.dr["seq2"].ToString() + ")";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1
                = string.Format(
                    @"with tmp
as
(select b.Roll,b.Dyelot,sum(b.StockQty) Receiving,0 A2B,0 C2B,0 TransIn,0 BackIn,0 ReturnIn,0 B2A,0 B2C,0 TransOut,0 BorrowOut,0 Adjust
from Receiving a WITH (NOLOCK) inner join Receiving_Detail b WITH (NOLOCK) on a.Id= b.id
where a.Status = 'Confirmed' and b.StockType='I'
and b.PoId ='{0}'
and b.Seq1 = '{1}'
and b.Seq2 = '{2}'
group by b.Roll,b.Dyelot
union all
select b.ToRoll,b.ToDyelot,0,sum(b.Qty) as a2b,0,0,0,0,0,0,0,0,0
from SubTransfer a WITH (NOLOCK) inner join SubTransfer_Detail b WITH (NOLOCK) on a.Id = b.id
where a.Status = 'Confirmed' and a.type='A' and b.ToStockType = 'I'
and b.ToPoId ='{0}'
and b.ToSeq1 = '{1}'
and b.ToSeq2 = '{2}'
group by b.ToRoll,b.ToDyelot
union all
select b.ToRoll,b.ToDyelot,0,0,sum(b.Qty) C2B,0,0,0,0,0,0,0,0
from SubTransfer a WITH (NOLOCK) inner join SubTransfer_Detail b WITH (NOLOCK) on a.Id=b.id
where a.Status = 'Confirmed' and a.Type='C' and b.ToStockType = 'I'
and b.ToPoId ='{0}'
and b.ToSeq1 = '{1}'
and b.ToSeq2 = '{2}'
group by b.ToRoll,b.ToDyelot
union all
select b.ToRoll,b.ToDyelot,0,0,0,0,0,0,0,SUM(b.Qty) B2C,0,0,0
from SubTransfer a WITH (NOLOCK) inner join SubTransfer_Detail b WITH (NOLOCK) on a.Id=b.id
where a.Status = 'Confirmed' and a.Type='E' and b.FromStockType = 'I'
and b.FromPoId ='{0}'
and b.FromSeq1 = '{1}'
and b.FromSeq2 = '{2}'
group by b.ToRoll,b.ToDyelot
union all
select b.Roll,b.Dyelot,0,0,0,sum(b.Qty) TransIn,0,0,0,0,0,0,0
from TransferIn a WITH (NOLOCK) inner join TransferIn_Detail b WITH (NOLOCK) on a.Id=b.id
where a.Status = 'Confirmed' 
and b.PoId ='{0}'
and b.Seq1 = '{1}'
and b.Seq2 = '{2}'
and b.Stocktype ='I'
group by b.Roll,b.Dyelot
union all
select B.ToRoll,B.ToDyelot,0,0,0,0,SUM(B.QTY) backin,0,0,0,0,0,0
from BorrowBack a WITH (NOLOCK) inner join BorrowBack_Detail b WITH (NOLOCK) on a.id =b.ID
where a.Status = 'Confirmed' and B.ToStockType = 'B'
and b.ToPoId ='{0}'
and b.ToSeq1 = '{1}'
and b.ToSeq2 = '{2}'
group by b.ToRoll,b.ToDyelot
union all
select b.Roll,b.Dyelot,0,0,0,0,0,sum(b.Qty) returnin,0,0,0,0,0
from ReturnReceipt a WITH (NOLOCK) inner join ReturnReceipt_Detail b WITH (NOLOCK) on a.id=b.Id
Where a.Status = 'Confirmed'
and b.PoId ='{0}'
and b.Seq1 = '{1}'
and b.Seq2 = '{2}'
group by b.Roll,b.Dyelot
union all
select b.FromRoll,b.FromDyelot,0,0,0,0,0,0,sum(b.Qty) as B2A,0,0,0,0
from SubTransfer a WITH (NOLOCK) inner join SubTransfer_Detail b WITH (NOLOCK) on a.Id = b.id
where a.Status = 'Confirmed' and a.type='B' and b.FromStockType = 'I'
and b.FromPoId ='{0}'
and b.FromSeq1 = '{1}'
and b.FromSeq2 = '{2}'
group by b.FromRoll,b.FromDyelot
union all
select b.Roll,b.Dyelot,0,0,0,0,0,0,0,0,sum(b.Qty) as transout,0,0
from TransferOut a WITH (NOLOCK) inner join TransferOut_Detail b WITH (NOLOCK) on a.Id=b.id
where a.Status = 'Confirmed' and stocktype='I'
and b.PoId ='{0}'
and b.Seq1 = '{1}'
and b.Seq2 = '{2}'
group by b.Roll,b.Dyelot
union all
select b.FromRoll,b.FromDyelot,0,0,0,0,0,0,0,0,0,sum(b.Qty) as borrowout,0
from BorrowBack a WITH (NOLOCK) inner join BorrowBack_Detail b WITH (NOLOCK) on a.Id=b.id
where a.Status = 'Confirmed' and FromStockType ='I'
and b.FromPoId ='{0}'
and b.FromSeq1 = '{1}'
and b.FromSeq2 = '{2}'
group by b.FromRoll,b.FromDyelot
union all
select b.Roll,b.Dyelot,0,0,0,0,0,0,0,0,0,0,sum(b.QtyAfter - b.QtyBefore) as adjust
from Adjust a WITH (NOLOCK) inner join Adjust_Detail b WITH (NOLOCK) on a.Id=b.id
where a.Status = 'Confirmed' 
and b.PoId ='{0}'
and b.Seq1 = '{1}'
and b.Seq2 = '{2}'
group by b.Roll,b.Dyelot)
select tmp.Roll,tmp.Dyelot,sum(tmp.Receiving) Receiving,sum(tmp.A2B) A2B,sum(tmp.C2B) C2B,sum(tmp.TransIn) TransIn,sum(tmp.BackIn) BackIn
,sum(tmp.ReturnIn) ReturnIn,sum(tmp.B2A) B2A,sum(tmp.B2C) B2C,sum(tmp.TransOut) TransOut,sum(tmp.BorrowOut) BorrowOut,sum(tmp.Adjust) Adjust
,stuff((select ',' + cast(t.MtlLocationID as nvarchar)
	from (select b1.MtlLocationID from FtyInventory a1 WITH (NOLOCK) inner join FtyInventory_Detail b1 WITH (NOLOCK) on a1.Ukey = b1.Ukey 
	where a1.Poid = '{0}' and a1.seq1 = '{1}' and a1.Seq2 = '{2}' and a1.StockType = 'I' and a1.Roll = tmp.Roll and a1.Dyelot =tmp.Dyelot 
	group by b1.MtlLocationID) t for xml path('')), 1, 1, '') as location
from  tmp 
group by tmp.roll,tmp.Dyelot
union all
select '','Total',sum(tmp.Receiving) Receiving,sum(tmp.A2B) A2B,sum(tmp.C2B) C2B,sum(tmp.TransIn) TransIn,sum(tmp.BackIn) BackIn
,sum(tmp.ReturnIn) ReturnIn,sum(tmp.B2A) B2A,sum(tmp.B2C) B2C,sum(tmp.TransOut) TransOut,sum(tmp.BorrowOut) BorrowOut,sum(tmp.Adjust) Adjust,''
from TMP",
                    this.dr["id"].ToString(), this.dr["seq1"].ToString(), this.dr["seq2"].ToString());
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }
            else
            {
                this.bindingSource1.DataSource = selectDataTable1;
            }

            // 設定Grid1的顯示欄位
            this.gridInventoryStatus.IsEditingReadOnly = true;
            this.gridInventoryStatus.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridInventoryStatus)
                 .Text("roll", header: "Roll#", width: Widths.AnsiChars(8))
                 .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                 .Numeric("receiving", header: "Rcv.", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("A2B", header: "A to B", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("C2B", header: "C to B", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("TransIn", header: "Trans. In", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("BackIn", header: "Give Back", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("ReturnIn", header: "Rtn. Rcv.", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("B2A", header: "B to A", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("B2C", header: "B to C", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("TransOut", header: "Trans. Out", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("BorrowOut", header: "Borrow Out", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjust", header: "Adj. Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("Location", header: "Location", width: Widths.AnsiChars(8))
                 ;

            this.gridInventoryStatus.Columns[1].Frozen = true;  // Fabric Type
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
