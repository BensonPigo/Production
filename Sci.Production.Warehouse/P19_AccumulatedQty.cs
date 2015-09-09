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
            selectCommand1.Append(string.Format(@";with cte as 
(
select A.PoId,A.Seq1,A.Seq2,isnull(sum(a1.qty),0 ) requestqty
	,A1.UnitID
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail A LEFT JOIN DBO.Invtrans A1 
	ON a1.InventoryPOID = a.PoId AND A1.InventorySeq1 =a.seq1 AND A1.InventorySeq2 = a.seq2
	where a1.type = 2 AND a.Id = '{0}' 
AND A1.TransferFactory ='{1}' 
and a1.FactoryID='{2}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.QTY,A1.FactoryID,A1.TransferFactory,A1.UnitID
union all
select A.PoId,A.Seq1,A.Seq2,isnull(sum(0 - a1.Qty),0) requestqty
	,A1.UnitID
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail a LEFT JOIN DBO.Invtrans A1 
	ON a1.InventoryPOID = a.PoId AND A1.InventorySeq1 =a.seq1 AND A1.InventorySeq2 = a.seq2
	where a1.type = 6 AND a.Id = '{0}'  
AND A1.TransferFactory ='{1}' 
and a1.FactoryID='{2}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
union all
select A.PoId,A.Seq1,A.Seq2,isnull(sum(a1.qty),0) requestqty
	,A1.UnitID
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail a LEFT JOIN DBO.Invtrans A1 
	ON  a1.InventoryPOID = a.PoId and a1.InventorySeq1 = a.Seq1 and a1.InventorySeq2 = a.Seq2
					WHERE TransferFactory = '{1}' 
					and a1.FactoryID='{2}'
					and a1.Type = 3  
                    AND a.Id = '{0}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
)
select cte.Poid,seq1,seq2
,sum(cte.requestqty) * (select v.Rate from View_unitrate v where v.FROM_U = cte.UnitID and v.TO_U = cte.stockunit) requestqty
,cte.qty
,dbo.getmtldesc(cte.poid,cte.seq1,cte.seq2,2,0) as [Description]
,cte.UnitID
,cte.stockunit from cte
group by cte.Poid,seq1,seq2
,cte.qty
,dbo.getmtldesc(cte.poid,cte.seq1,cte.seq2,2,0)
,cte.UnitID
,cte.stockunit ;
", dr["id"].ToString(), dr["toftyid"].ToString(), dr["factoryid"].ToString()));

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
                 .Text("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
