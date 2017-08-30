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
        public Sci.Win.Tems.Base P08;
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
            selectCommand1.Append(string.Format(@"
select  a.poid
        , a.seq1
        , a.seq2
        , Round(sum(dbo.GetUnitQty(b.POUnit, b.StockUnit, b.Qty)), 2) as useqty 
        , sum(a.StockQty) as stockqty
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
from dbo.Receiving_Detail a WITH (NOLOCK) 
left join po_supp_detail b WITH (NOLOCK) on a.PoId = b.id and a.seq1 = b.seq1 and a.seq2 = b.SEQ2
where a.Id = '{0}'
group by a.PoId,a.seq1,a.seq2", dr["id"].ToString()));

            DataTable selectDataTable1;
            P08.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);
            
            if (selectResult1 == false)
            { ShowErr(selectCommand1.ToString(), selectResult1); }

            P08.HideWaitMessage();

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("useqty", header: "Use Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .Numeric("stockqty", header: "Accu. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(40))
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
