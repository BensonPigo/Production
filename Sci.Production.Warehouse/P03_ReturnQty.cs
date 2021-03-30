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
    public partial class P03_ReturnQty : Sci.Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P03_ReturnQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += " (" + this.dr["id"].ToString() + "-" + this.dr["seq1"].ToString() + "-" + this.dr["seq2"].ToString() + ")";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlcmd = $@"
select
[Date] = r1.IssueDate
,[ID] = r1.Id
,[Name] = 'P37. Return Receiving Material'
,[ReturnQty] = sum(r2.Qty)
,[Refund Reason] = w.Description
,[Action] = wa.Description
,r1.Remark
from ReturnReceipt r1
inner join ReturnReceipt_Detail r2 on r1.Id = r2.Id
left join WhseReason w on w.ID = r1.WhseReasonId and w.Type ='RR'
left join WhseReason wa on wa.ID = r1.WhseReasonId and wa.Type ='RA'
where 1=1
and r1.Status='Confirmed'
and r2.POID = '{this.dr["ID"]}'
and r2.Seq1 = '{this.dr["Seq1"]}' 
and r2.Seq2 = '{this.dr["Seq2"]}'
group by r1.IssueDate,r1.Id,w.Description,r1.Remark,wa.Description
order by r1.IssueDate, r1.Id
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (result == false)
            {
                this.ShowErr(sqlcmd, result);
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    object ttlQty = dt.Compute("sum(ReturnQty)", null);
                    this.numTotal.Value = !MyUtility.Check.Empty(ttlQty) ? decimal.Parse(ttlQty.ToString()) : 0m;
                }
            }

            this.bindingSource1.DataSource = dt;

            #region 開窗
            DataGridViewGeneratorTextColumnSettings col_ID = new DataGridViewGeneratorTextColumnSettings();
            col_ID.CellMouseDoubleClick += (s, e) =>
            {
                var dr2 = this.gridReturn.GetDataRow<DataRow>(e.RowIndex);
                if (dr2 == null)
                {
                    return;
                }

                var frm = new P37(null, dr2["id"].ToString());
                frm.ShowDialog(this);
            };
            #endregion

            this.gridReturn.IsEditingReadOnly = true;
            this.gridReturn.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReturn)
            .Date("Date", header: "Date", width: Widths.AnsiChars(12))
            .Text("ID", header: "ID", width: Widths.AnsiChars(15), settings: col_ID)
            .Text("Name", header: "Name", width: Widths.AnsiChars(23))
            .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
            .Text("Refund Reason", header: "Refund Reason", width: Widths.AnsiChars(25))
            .Text("Action", header: "Action", width: Widths.AnsiChars(25))
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            ;
}

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
