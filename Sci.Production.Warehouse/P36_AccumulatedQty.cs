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
    public partial class P36_AccumulatedQty : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P36;
        protected DataRow dr;
        public P36_AccumulatedQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(@"select a.FromPoId, a.FromSeq1,a.FromSeq2
,sum(a.Qty) as qty
,dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0) as [Description]
from dbo.SubTransfer_Detail a 
inner join PO_Supp_Detail b on a.FromPoId = b.id and a.FromSeq1 = b.seq1 and a.FromSeq2 = b.SEQ2
where a.Id = '{0}'
group by a.FromPoId,a.FromSeq1,a.FromSeq2", dr["id"].ToString()));

            DataTable selectDataTable1;
            P36.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            
            if (selectResult1 == false)
            { ShowErr(selectCommand1.ToString(), selectResult1); }

            P36.HideWaitMessage();

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("frompoid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("fromseq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("fromseq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("qty", header: "Accu. Qty", width: Widths.AnsiChars(12), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
