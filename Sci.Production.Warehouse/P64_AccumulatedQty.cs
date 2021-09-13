using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P64_AccumulatedQty : Sci.Win.Subs.Base
    {
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P64_AccumulatedQty(DataTable dt)
        {
            this.InitializeComponent();
            this.dtDetail = dt;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定Grid1的顯示欄位
            this.grid.IsEditingReadOnly = true;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(6))
                .Numeric("AccQty", header: "Acc Qty", width: Widths.AnsiChars(11), integer_places: 9, decimal_places: 2)
                .Text("Desc", header: "Description", width: Widths.AnsiChars(35))
                ;

            string sqlCmd = $@"
select  sfd2.POID,sfd2.Seq
,[AccQty] = sum(sfd2.Qty)
,sf.[Desc]
from SemiFinishedReceiving_Detail sfd2
inner join SemiFinishedReceiving sfd with (nolock) on sfd.ID = sfd2.ID
left join SemiFinished sf on sf.POID = sfd2.POID and sf.Seq = sfd2.Seq
where sfd.Status='Confirmed'
and exists(select 1 from #tmp t where t.poid =  sfd2.POID and t.Seq = sfd2.Seq)
group by sfd2.POID,sfd2.Seq,sf.[Desc]
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtDetail, string.Empty, sqlCmd, out DataTable gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = gridData;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
