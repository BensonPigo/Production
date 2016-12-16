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


namespace Sci.Production.Warehouse
{
    public partial class P19_AccumulatedQty : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P19_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@"
;with Z as(
	select A.PoId,A.Seq1,A.Seq2
	,requestqty =isnull(x.q,0 )
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail A 
	outer apply(
		select q = (
			Select sum(qty) 
			from Invtrans B 
			where (B.type='2' or B.type='3')
			 and B.InventoryPoid=a.POID 
			 and B.InventorySeq1=a.Seq1
			 and B.InventorySeq2=a.Seq2
			 and B.FactoryId in (Select Id from Factory where MDivisionId='{1}') 
			 and B.TransferFactory not in (Select Id from Factory where MDivisionId='{1}')
		 )
	)x
	where  a.Id = '{0}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,x.q

	Union all
	select A.PoId,A.Seq1,A.Seq2
	,requestqty = -isnull(x.q,0)
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail a 
	outer apply(
		select q = (
			Select sum(qty) 
			from Invtrans B
			where B.type='6' 
			and B.InventoryPoid = a.POID 
			and B.InventorySeq1 = a.Seq1
			and B.InventorySeq2 = a.Seq2
			and B.FactoryId in (Select Id from Factory where MDivisionId='{1}')
		 )
	)x
	where  a.Id = '{0}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,x.q
)
select PoId,Seq1,Seq2,requestqty=sum(requestqty)
* isnull((select v.Ratevalue from dbo.View_Unitrate v where v.FROM_U = 
		(
			select distinct unitID
			from Invtrans B
			where B.InventoryPoId = z.PoId 
			and B.InventorySeq1 = z.Seq1 
			and B.InventorySeq2 = z.Seq2 
			and B.FactoryId = 'SPS'
		) 
		and v.TO_U = z.stockunit),1)
,Qty,stockunit
,[Description] = dbo.getmtldesc(Z.poid,Z.seq1,Z.seq2,2,0)
from Z
group by PoId,Seq1,Seq2,Qty,stockunit
                ;
", dr["id"].ToString(),Sci.Env.User.Keyword));

            DataTable selectDataTable1;
            MyUtility.Msg.WaitWindows("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            
            if (selectResult1 == false)
            { ShowErr(selectCommand1.ToString(), selectResult1); }

            MyUtility.Msg.WaitClear();

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("requestqty", header: "Request Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("qty", header: "Accu. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
