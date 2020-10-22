using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P15_AccumulatedQty : Win.Subs.Base
    {
        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Win.Tems.Base P15;
        private DataRow dr;

        /// <inheritdoc/>
        public P15_AccumulatedQty(DataRow data)
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
                @"select A.PoId,b.Seq1,b.Seq2
,b.RequestQty
,sum(c.Qty) as Qty
,dbo.getMtlDesc(a.poid,b.seq1,b.seq2,2,0) as [Description]
from dbo.lack a WITH (NOLOCK) inner join 
dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
left join dbo.IssueLack_Detail c WITH (NOLOCK) on a.POID = c.Poid and b.Seq1 = c.seq1 and b.seq2 = c.Seq2
where a.Id = '{0}' and c.id = '{1}'
GROUP BY A.PoId,b.Seq1,b.Seq2,b.RequestQty", this.dr["requestid"], this.dr["id"]));

            DataTable selectDataTable1;
            this.P15.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1.ToString(), out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1.ToString(), selectResult1);
            }

            this.P15.HideWaitMessage();
            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Numeric("requestqty", header: "Req. Qty", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 2)
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
