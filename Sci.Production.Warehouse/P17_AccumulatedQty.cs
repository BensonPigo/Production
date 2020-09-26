using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P17_AccumulatedQty : Win.Subs.Base
    {
        public Win.Tems.Base P17;
        protected DataRow dr;

        public P17_AccumulatedQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"select A.PoId,A.Seq1,A.Seq2
,sum(a.Qty) as Qty
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
from dbo.IssueReturn_Detail a WITH (NOLOCK) 
where a.Id = '{0}'
GROUP BY A.PoId,A.Seq1,A.Seq2", this.dr["id"].ToString()));

            DataTable selectDataTable1;
            this.P17.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P17.HideWaitMessage();

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
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
