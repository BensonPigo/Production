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
    public partial class P07_AccumulatedQty : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P07;
        protected DataRow dr;
        public P07_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@"
select poid,seq1,seq2,dbo.GetUnitQty(PoUnit,dbo.GetStockUnitBySPSeq(poid,seq1,seq2),sum(shipqty)) shipqty,sum(accu_rcv) received
    , sum(rcv) receiving ,description,Foc = sum(Foc)
from (
    select a.PoId,a.Seq1,a.Seq2,0 as shipqty,0 as accu_rcv,sum(a.StockQty) as rcv
    ,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] ,a.POUnit ,Foc = 0
    from dbo.Receiving_Detail a WITH (NOLOCK) where id='{0}' group by a.PoId,a.Seq1,a.Seq2,a.POUnit
    union all
",
                dr["id"].ToString(),dr["exportid"].ToString()));
            if (MyUtility.Check.Empty(dr["exportid"].ToString()))
            {
                selectCommand1.Append(string.Format(@"
    select a.id poid,a.Seq1,a.seq2,a.Qty as shipqty,0 as accu_rcv,0 as rcv
        ,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [description],a.POUnit,a.Foc
    from dbo.PO_Supp_Detail a WITH (NOLOCK) 
    ,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail WITH (NOLOCK) where id='{0}') c 
    where a.id = c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2
) tmp
group by poid,seq1,seq2,description,POUnit", dr["id"].ToString()));
            }
            else
            {
                selectCommand1.Append(string.Format(@"
    select a.PoId,a.Seq1,a.Seq2,0 as shipqty,sum(a.StockQty) as accu_rcv ,0 as rcv
        ,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description],a.POUnit,Foc = 0
    from dbo.Receiving_Detail a WITH (NOLOCK) 
    ,dbo.Receiving b WITH (NOLOCK) 
    ,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail WITH (NOLOCK) where id='{0}') c 
    where b.id!='{0}' and b.Status='Confirmed' and a.id=b.id and b.ExportId = '{1}'
    and a.PoId=c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2 
    group by a.PoId,a.Seq1,a.Seq2,a.POUnit
    union all
",
                    dr["id"].ToString(), dr["exportid"].ToString()));

                selectCommand1.Append(string.Format(@"
    select 
	    final.poid, 
	    final.SEQ1, 
	    final.SEQ2,
	    shipqty = isnull(sf.shipqty,sf2.shipqty), 
	    0 as accu_rcv,
	    0 as rcv
	    ,dbo.getmtldesc(final.poid, final.SEQ1, final.SEQ2,2,0) as [description] ,final.PoUnit,
	    foc = isnull(sf.foc,sf2.foc)
    from( 	
	    select distinct zz.poid , zz.seq1, zz.seq2 ,zz.PoUnit
	    from (
		    select e.PoID poid, e.seq1, e.seq2,c.PoUnit 
            from (select distinct PoId,Seq1,Seq2,PoUnit from dbo.Receiving_Detail WITH (NOLOCK) where id='{0}') c
            , (select distinct Poid, seq1, seq2 from dbo.Export_Detail d WITH (NOLOCK) where d.id = '{1}') e
			where  (c.PoId = e.poid and c.seq1 = e.seq1 and c.seq2 = e.seq2)

		    union all

		    select a.id poid, a.seq1, a.seq2,a.POUnit 
            from dbo.PO_Supp_Detail a WITH (NOLOCK) 
            ,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail WITH (NOLOCK) where id='{0}') c
			where (a.id = c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2)
	    ) zz
    )final
    outer apply
	(
		select not3RD.shipqty ,Foc
		from 
		(
			select e.PoID poid,e.Seq1,e.seq2,e.Qty as shipqty,e.Foc
			from (select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail WITH (NOLOCK) where id='{0}') c
			, (select distinct Poid, seq1, seq2, Qty, Foc from dbo.Export_Detail d WITH (NOLOCK) where d.id = '{1}') e
			where  (c.PoId = e.poid and c.seq1 = e.seq1 and c.seq2 = e.seq2)
		)not3RD 
		where not3RD.poid = final.poid and not3RD.SEQ1 = final.SEQ1 and not3RD.SEQ2 = final.SEQ2
	)sf
	outer apply
	(
		select is3RD.shipqty ,Foc
		from (
				select a.id poid,a.Seq1,a.seq2,a.Qty as shipqty,a.Foc
				from dbo.PO_Supp_Detail a WITH (NOLOCK) 
				,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail WITH (NOLOCK) where id='{0}') c
				where (a.id = c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2)
		)is3RD where is3RD.poid = final.poid and is3RD.SEQ1 = final.SEQ1 and is3RD.SEQ2 = final.SEQ2
	)sf2
) tmp
group by poid,seq1,seq2,description,PoUnit", dr["id"].ToString(), dr["exportid"].ToString()));

            }
            DataTable selectDataTable1;
            P07.ShowWaitMessage("Data loading...");
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            if (selectResult1 == false) { ShowErr(selectCommand1.ToString(), selectResult1); }
            DBProxy.Current.DefaultTimeout = 0;
            P07.HideWaitMessage();
            selectDataTable1.ColumnsDecimalAdd("variance", 0m, "received+receiving-shipqty-foc");
            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("shipQty", header: "Ship Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("Foc", header: "F.O.C", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("received", header: "Accu. Rcvd.", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("receiving", header: "Rcvd. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("variance", header: "Variance", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
