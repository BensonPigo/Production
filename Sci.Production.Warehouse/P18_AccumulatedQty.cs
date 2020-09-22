using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P18_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P18;
        protected DataRow dr;

        public P18_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            #region 20161118 willy backup

            // selectCommand1.Append(string.Format(@";with cte as
// (
// select A.PoId,A.Seq1,A.Seq2,isnull(sum(a1.qty),0 ) requestqty
// ,A1.UnitID
// ,sum(a.Qty) as Qty
// ,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
// from dbo.TransferIn_Detail A LEFT JOIN DBO.Invtrans A1
// ON a1.Seq70poid = a.PoId and a1.seq70seq1 = a.seq1 and a1.seq70seq2 = a.seq2
// where a1.type = 2 AND a.Id = '{0}' AND A1.FactoryID ='{2}' and a1.TransferFactory='{1}'
// GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.QTY,A1.FactoryID,A1.TransferFactory,A1.UnitID
// union all
// select A.PoId,A.Seq1,A.Seq2,isnull(sum(0 - a1.Qty),0) requestqty
// ,A1.UnitID
// ,sum(a.Qty) as Qty
// ,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
// from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1
// ON a1.Seq70poid = a.PoId and a1.seq70seq1 = a.seq1 and a1.seq70seq2 = a.seq2
// where a1.type = 6 AND a.Id = '{0}'  AND A1.FactoryID ='{2}' and a1.TransferFactory='{1}'
// GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
// union all
// select A.PoId,A.Seq1,A.Seq2,isnull(sum(a1.qty),0) requestqty
// ,A1.UnitID
// ,sum(a.Qty) as Qty
// ,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
// from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1
// ON  a1.InventoryPOID = a.PoId and a1.InventorySeq1 = a.Seq1 and a1.InventorySeq2 = a.Seq2
// WHERE  a1.Type =3 -- 20161118 WILLY 依照舊系統邏輯 抓TYPE=2,3
// AND TransferFactory = '{1}'
// and a1.FactoryID='{2}'
// and a.Id = '{0}'
// GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
// )
// select cte.Poid,seq1,seq2
// ,sum(cte.requestqty) * (select v.Rate from dbo.View_Unitrate v where v.FROM_U = cte.UnitID and v.TO_U = cte.stockunit) requestqty
// ,cte.qty
// ,dbo.getmtldesc(cte.poid,cte.seq1,cte.seq2,2,0) as [Description]
// ,cte.UnitID
// ,cte.stockunit from cte
// group by cte.Poid,seq1,seq2
// ,cte.qty
// ,dbo.getmtldesc(cte.poid,cte.seq1,cte.seq2,2,0)
// ,cte.UnitID
// ,cte.stockunit ;
            // ", dr["id"].ToString(), dr["mdivisionid"].ToString(), dr["fromftyid"].ToString()));
            #endregion
            selectCommand1.Append(string.Format(
                @"
;with Z as(
	select A.PoId,A.Seq1,A.Seq2
	,requestqty = isnull(X.Q,0)
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t WITH (NOLOCK) where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit	
	from dbo.TransferIn_Detail a WITH (NOLOCK) 
	outer apply(
		select Q = (
			Select Sum(Qty) qty
			from Invtrans B WITH (NOLOCK) 
			where (B.Type='2' or B.Type='3')
			and B.InventoryPoId = a.Poid
			and B.InventorySeq1 = a.seq1
			and B.InventorySeq2 = a.Seq2
			and B.FactoryId = '{2}'
			and B.TransferFactory in (select Id from Factory WITH (NOLOCK))
		)
	)X
	WHERE a.Id = '{0}'
	group by  A.PoId,A.Seq1,A.Seq2,X.Q

	union all 
	select A.PoId,A.Seq1,A.Seq2
	,requestqty = isnull(X.Q,0)
	,sum(a.Qty) as Qty 
	,stockunit = (select StockUnit from dbo.PO_Supp_Detail t WITH (NOLOCK) where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) 
	from dbo.TransferIn_Detail A WITH (NOLOCK) 
	outer apply(	
		select Q = (
			select sum(qty) 
			from DBO.Invtrans A1 WITH (NOLOCK) 
			where a1.Seq70poid = a.PoId 
			and a1.seq70seq1 = a.seq1 
			and a1.seq70seq2 = a.seq2 
			and a1.type = '2' 
			AND A1.FactoryID ='{2}' 
			and A1.TransferFactory in (select Id from Factory WITH (NOLOCK))
		)
	) X
	where a.Id = '{0}'
	group by  A.PoId,A.Seq1,A.Seq2,X.Q

	union all 
	select A.PoId,A.Seq1,A.Seq2
	,requestqty = -isnull(X.Q,0)
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t WITH (NOLOCK) where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferIn_Detail a WITH (NOLOCK) 
	outer apply(	
		select Q = (
			Select Sum(Qty)
			from Invtrans B WITH (NOLOCK) 
			where B.Type = '6' 
			and B.seq70PoId = a.PoId 
			and B.seq70Seq1 = a.Seq1 
			and B.seq70Seq2 = a.Seq2 
			and B.FactoryId = '{2}'
		)
	) X
	where a.Id = '{0}'
	group by  A.PoId,A.Seq1,A.Seq2,X.Q
)

select  z.POID
        , z.Seq1
        , z.Seq2
        , requestqty = Round(dbo.GetUnitQty((select distinct unitID
			                                 from Invtrans B WITH (NOLOCK) 
			                                 where   B.InventoryPoId = z.PoId 
			                                         and B.InventorySeq1 = z.Seq1 
			                                         and B.InventorySeq2 = z.Seq2 
			                                         and B.FactoryId = '{2}') 
                                             , z.stockunit
                                             , sum(z.requestqty))
                            , 2)
        ,qty
        ,z.stockunit
        ,[Description] = dbo.getmtldesc(Z.poid,Z.seq1,Z.seq2,2,0)
from Z 
group by z.POID,z.Seq1,z.Seq2,qty,z.stockunit
            ", this.dr["id"].ToString(), this.dr["mdivisionid"].ToString(), this.dr["fromftyid"].ToString()));

            DataTable selectDataTable1;
            this.P18.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P18.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("requestqty", header: "Request Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("qty", header: "Accu. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
