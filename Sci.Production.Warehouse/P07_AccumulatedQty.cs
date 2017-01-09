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
            selectCommand1.Append(string.Format(@"select poid,seq1,seq2,sum(shipqty) shipqty,sum(accu_rcv) received
, sum(rcv) receiving ,description 
from (
select a.PoId,a.Seq1,a.Seq2,0 as shipqty,0 as accu_rcv,sum(a.StockQty) as rcv
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]  
from dbo.Receiving_Detail a where id='{0}' group by a.PoId,a.Seq1,a.Seq2
union all
select a.PoId,a.Seq1,a.Seq2,0 as shipqty,sum(a.StockQty) as accu_rcv ,0 as rcv
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
from dbo.Receiving_Detail a
,dbo.Receiving b
,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail where id='{0}') c 
where b.id!='{0}' and b.Status='Confirmed' and a.id=b.id 
and a.PoId=c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2 
group by a.PoId,a.Seq1,a.Seq2
union all" + Environment.NewLine,dr["id"].ToString(),dr["exportid"].ToString()));
            if (MyUtility.Check.Empty(dr["exportid"].ToString()))
            {
                selectCommand1.Append(string.Format(@"select a.id poid,a.Seq1,a.seq2,(a.Qty+a.Foc) as shipqty,0 as accu_rcv,0 as rcv
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [description]
from dbo.PO_Supp_Detail a,(select distinct PoId,Seq1,Seq2 from dbo.Receiving_Detail where id='{0}') c 
 where a.id = c.poid and a.seq1 = c.seq1 and a.seq2 = c.seq2) tmp
group by poid,seq1,seq2,description", dr["id"].ToString()));
            }
            else
            {
                selectCommand1.Append(string.Format(@"select a.PoID,a.Seq1,a.seq2,(a.Qty+a.Foc) as shipqty,0 as accu_rcv,0 as rcv
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
from dbo.Export_Detail a where id='{0}') tmp
group by poid,seq1,seq2,description"
                                , dr["exportid"].ToString()));
            }
            DataTable selectDataTable1;
            P07.ShowWaitMessage("Data loading...");
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            if (selectResult1 == false) { ShowErr(selectCommand1.ToString(), selectResult1); }
            DBProxy.Current.DefaultTimeout = 0;
            P07.HideWaitMessage();
            selectDataTable1.ColumnsDecimalAdd("variance", 0m, "received+receiving-shipqty");
            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("shipQty", header: "Ship Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("received", header: "Accu. Rcvd.", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("receiving", header: "Rcvd. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("variance", header: "Variance", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
