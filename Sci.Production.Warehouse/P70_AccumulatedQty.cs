using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P70_AccumulatedQty : Sci.Win.Subs.Base
    {
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P70_AccumulatedQty(DataTable dt)
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
            SELECT 
            [POID] = lord.POID,
            [Seq] = Concat (lord.Seq1, ' ', lord.Seq2),
            [AccQty] = SUM(lord.Qty),
            [Desc] = lom.[Desc]
            FROM LocalOrderReceiving_Detail lord
            INNER JOIN LocalOrderReceiving lor ON lord.ID = lor.ID
            LEFT JOIN LocalOrderMaterial lom ON lord.POID = lom.POID AND lord.Seq1 = lom.Seq1 AND lord.Seq2 = lom.Seq2
            WHERE 
            lor.Status = 'Confirmed' AND
            exists(select 1 from #tmp t where t.POID =  lord.POID AND t.Seq1 = lord.Seq1 AND t.Seq2 = lord.Seq2)
            group by lord.POID,lord.Seq1,lord.Seq2,lom.[Desc]
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
