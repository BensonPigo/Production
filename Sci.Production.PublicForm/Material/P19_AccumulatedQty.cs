﻿using System;
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
        public Sci.Win.Tems.Base P19;
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
	,(select StockUnit from dbo.PO_Supp_Detail t WITH (NOLOCK) where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail A WITH (NOLOCK) 
	outer apply(
		select q = (
			Select sum(qty) 
			from Invtrans B WITH (NOLOCK) 
			where (B.type='2' or B.type='3')
			 and B.InventoryPoid=a.POID 
			 and B.InventorySeq1=a.Seq1
			 and B.InventorySeq2=a.Seq2
			 and B.FactoryId in (Select Id from Factory WITH (NOLOCK) where MDivisionId='{1}') 
			 and B.TransferFactory not in (Select Id from Factory WITH (NOLOCK))
		 )
	)x
	where  a.Id = '{0}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,x.q

	Union all
	select A.PoId,A.Seq1,A.Seq2
	,requestqty = -isnull(x.q,0)
	,sum(a.Qty) as Qty 
	,(select StockUnit from dbo.PO_Supp_Detail t WITH (NOLOCK) where t.id = a.Poid and t.seq1=a.seq1 and t.seq2 = a.Seq2) stockunit
	from dbo.TransferOut_Detail a WITH (NOLOCK) 
	outer apply(
		select q = (
			Select sum(qty) 
			from Invtrans B WITH (NOLOCK) 
			where B.type='6' 
			and B.InventoryPoid = a.POID 
			and B.InventorySeq1 = a.Seq1
			and B.InventorySeq2 = a.Seq2
			and B.FactoryId in (Select Id from Factory WITH (NOLOCK))
		 )
	)x
	where  a.Id = '{0}'
	GROUP BY A.PoId,A.Seq1,A.Seq2,x.q
)
select  PoId
        , Seq1
        , Seq2
        , requestqty = Round(dbo.GetUnitQty((select distinct unitID
			                                 from Invtrans B WITH (NOLOCK) 
			                                 where   B.InventoryPoId = z.PoId 
			                                         and B.InventorySeq1 = z.Seq1 
			                                         and B.InventorySeq2 = z.Seq2 
			                                         and B.FactoryId = '{2}') 
		                                    , z.stockunit
                                            , sum(requestqty))
                            , 2)
        ,Qty,stockunit
        ,[Description] = dbo.getmtldesc(Z.poid,Z.seq1,Z.seq2,2,0)
from Z
group by PoId,Seq1,Seq2,Qty,stockunit;
", dr["id"].ToString(), Sci.Env.User.Keyword, Sci.Env.User.Factory));

            DataTable selectDataTable1;
            P19.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            
            if (selectResult1 == false)
            { ShowErr(selectCommand1.ToString(), selectResult1); }

            P19.HideWaitMessage();

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("requestqty", header: "Request Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("qty", header: "Accu. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
