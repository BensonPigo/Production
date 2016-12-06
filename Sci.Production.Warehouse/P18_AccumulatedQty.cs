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
    public partial class P18_AccumulatedQty : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P18_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            #region 20161118 willy backup
            //            selectCommand1.Append(string.Format(@";with cte as 
//(
//select A.PoId,A.Seq1,A.Seq2,isnull(sum(a1.qty),0 ) requestqty
//	,A1.UnitID
//	,sum(a.Qty) as Qty 
//	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
//	from dbo.TransferIn_Detail A LEFT JOIN DBO.Invtrans A1 
//	ON a1.Seq70poid = a.PoId and a1.seq70seq1 = a.seq1 and a1.seq70seq2 = a.seq2
//	where a1.type = 2 AND a.Id = '{0}' AND A1.FactoryID ='{2}' and a1.TransferFactory='{1}'
//	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.QTY,A1.FactoryID,A1.TransferFactory,A1.UnitID
//union all
//select A.PoId,A.Seq1,A.Seq2,isnull(sum(0 - a1.Qty),0) requestqty
//	,A1.UnitID
//	,sum(a.Qty) as Qty 
//	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
//	from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1 
//	ON a1.Seq70poid = a.PoId and a1.seq70seq1 = a.seq1 and a1.seq70seq2 = a.seq2
//	where a1.type = 6 AND a.Id = '{0}'  AND A1.FactoryID ='{2}' and a1.TransferFactory='{1}'
//	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
//union all
//select A.PoId,A.Seq1,A.Seq2,isnull(sum(a1.qty),0) requestqty
//	,A1.UnitID
//	,sum(a.Qty) as Qty 
//	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
//	from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1 
//	ON  a1.InventoryPOID = a.PoId and a1.InventorySeq1 = a.Seq1 and a1.InventorySeq2 = a.Seq2
//					WHERE  a1.Type =3 -- 20161118 WILLY 依照舊系統邏輯 抓TYPE=2,3
//					AND TransferFactory = '{1}' 
//					and a1.FactoryID='{2}'
//					and a.Id = '{0}'
//	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
//)
//select cte.Poid,seq1,seq2
//,sum(cte.requestqty) * (select v.Rate from dbo.View_Unitrate v where v.FROM_U = cte.UnitID and v.TO_U = cte.stockunit) requestqty
//,cte.qty
//,dbo.getmtldesc(cte.poid,cte.seq1,cte.seq2,2,0) as [Description]
//,cte.UnitID
//,cte.stockunit from cte
//group by cte.Poid,seq1,seq2
//,cte.qty
//,dbo.getmtldesc(cte.poid,cte.seq1,cte.seq2,2,0)
//,cte.UnitID
//,cte.stockunit ;
            //", dr["id"].ToString(), dr["mdivisionid"].ToString(), dr["fromftyid"].ToString()));
            #endregion
            selectCommand1.Append(string.Format(@";with cte as 
            (
            select A.PoId,A.Seq1,A.Seq2,isnull(a1.qty,0 ) requestqty
            	,A1.UnitID
            	,sum(a.Qty) as Qty 
            	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
            	from dbo.TransferIn_Detail A LEFT JOIN DBO.Invtrans A1 
            	ON a1.Seq70poid = a.PoId and a1.seq70seq1 = a.seq1 and a1.seq70seq2 = a.seq2 
                and a1.type = 2 AND A1.FactoryID ='{2}' and a1.TransferMDivisionID='{1}'
            	where a.Id = '{0}'
            	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.QTY,A1.FactoryID,A1.TransferMDivisionID,A1.UnitID
            union 
            select A.PoId,A.Seq1,A.Seq2,isnull(0 - a1.Qty,0) requestqty
            	,A1.UnitID
            	,sum(a.Qty) as Qty 
            	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
            	from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1 
            	ON a1.Seq70poid = a.PoId and a1.seq70seq1 = a.seq1 and a1.seq70seq2 = a.seq2
                and a1.type = 6 AND A1.FactoryID ='{2}' and a1.TransferMDivisionID='{1}'
            	where a.Id = '{0}'
            	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
            --union all
            --select A.PoId,A.Seq1,A.Seq2,isnull(a1.qty,0) requestqty
            --	,A1.UnitID
            --	,sum(a.Qty) as Qty 
            --	,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
            --	from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1 
            --	ON  a1.InventoryPOID = a.PoId and a1.InventorySeq1 = a.Seq1 and a1.InventorySeq2 = a.Seq2
            --					WHERE  a1.Type =3  -- 20161118 WILLY 依照舊系統邏輯 抓TYPE=2,3
            --					AND TransferMDivisionID = '{1}' 
            --					and a1.FactoryID='{2}'
            --					and a.Id = '{0}'
            --	GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
            --union all 
            --select distinct A.PoId,A.Seq1,A.Seq2,0 as requestqty
	        --    ,A1.UnitID
	        --    ,sum(a.Qty) as Qty 
	        --    ,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	        --    from dbo.TransferIn_Detail a LEFT JOIN DBO.Invtrans A1 
	        --    ON  a1.InventoryPOID = a.PoId and a1.InventorySeq1 = a.Seq1 and a1.InventorySeq2 = a.Seq2
            --    and a1.FactoryID='{2}'
			--		            WHERE  1=1
			--		            and a.Id = '{0}'                                
			--		            and a.Seq1 not like '7_'
	        --    GROUP BY A.PoId,A.Seq1,A.Seq2,A1.QTY,A1.UnitID
            ),t1 as(
	            select A.PoId,A.Seq1,A.Seq2
	            ,sum(a.Qty) as Qty 
	            ,(select StockUnit from dbo.PO_Supp_Detail t where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit	
	            from dbo.TransferIn_Detail a 
	            WHERE a.Id = '{0}'
	            GROUP BY A.PoId,A.Seq1,A.Seq2
            )
            , i1 as(
	            select distinct isnull(b1.qty,0) as requestqty,b1.UnitID,InventoryPOID,InventorySeq1,InventorySeq2
	            from invtrans b1,t1
	            where b1.InventoryPOID=t1.POID and b1.InventorySeq1=t1.Seq1 and b1.InventorySeq2 =t1.Seq2	
            ), u4 as (
	            select t1.POID,t1.Seq1,t1.Seq2,
	            i1.requestqty,i1.UnitID,t1.Qty
	            ,(select StockUnit from dbo.PO_Supp_Detail t where t.id = t1.Poid and t.seq1=t1.seq1 and t.seq2 = t1.Seq2) stockunit
	            from t1 left join i1 on i1.InventoryPOID=t1.POID and i1.InventorySeq1=t1.Seq1 and i1.InventorySeq2 =t1.Seq2	
	            where seq1 not like '7_'
	            union all 
	            select *
	            from cte
            ), la as(
				select u4.Poid,seq1,seq2
				,isnull(sum(u4.requestqty) 
					* (select v.Ratevalue from dbo.View_Unitrate v where v.FROM_U = u4.UnitID and v.TO_U = u4.stockunit),0)requestqty
				,u4.qty
				,dbo.getmtldesc(u4.poid,u4.seq1,u4.seq2,2,0) as [Description]
				--,u4.UnitID
				,u4.stockunit 
				from u4
				group by u4.Poid,seq1,seq2
				,u4.qty
				,dbo.getmtldesc(u4.poid,u4.seq1,u4.seq2,2,0)
				,u4.UnitID
				,u4.stockunit
			)
			select Poid,seq1,seq2,sum(requestqty) requestqty,qty,Description,stockunit
            from la
			group by Poid,seq1,seq2,qty,Description,stockunit
            ", dr["id"].ToString(), dr["mdivisionid"].ToString(), dr["fromftyid"].ToString()));

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
