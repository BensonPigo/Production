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
    public partial class P08_AccumulatedQty : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P08_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@"select a.poid, a.seq1,a.seq2
,sum(b.Qty * isnull(c.Rate,1)) as useqty 
,sum(a.StockQty) as stockqty
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
from dbo.Receiving_Detail a 
inner join PO_Artwork b on a.PoId = b.id and a.seq1 = b.seq1 and a.seq2 = b.SEQ2
left join View_Unitrate c on c.FROM_U = b.POUnit and c.TO_U = b.StockUnit
where a.Id = '{0}'
group by a.PoId,a.seq1,a.seq2", dr["id"].ToString()));

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
                 .Numeric("useqty", header: "Use Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("stockqty", header: "Accu. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
