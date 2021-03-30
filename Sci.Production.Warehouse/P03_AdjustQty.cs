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
    public partial class P03_AdjustQty : Sci.Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P03_AdjustQty(DataRow data)
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
[Date] = a1.IssueDate
,[ID] = a1.Id
,[Name] = case when a1.Type = 'A' then 'P35. Adjust Bulk Qty' 
		       when a1.Type = 'B' then 'P34. Adjust Stock Qty' else '' end
,[AdjustQty] = sum(a2.QtyAfter-a2.QtyBefore)
,a1.Remark
,[Reason] = Reason.value
from Adjust a1
inner join Adjust_Detail a2 on a1.Id = a2.Id
outer apply(
	select value = stuff((
	select CONCAT(',', Name )
	from (
			select distinct r.Name 
			from Reason r
			where r.ReasonTypeID = 'Stock_Adjust'
			and r.ID = a2.ReasonId
		) s
	FOR XML PATH(''))
	,1,1,'')
) Reason
where 1=1
and a1.Status='Confirmed'
and a1.Type in ('A','B')
and a2.POID = '{this.dr["ID"]}'
and a2.Seq1 = '{this.dr["Seq1"]}' 
and a2.Seq2 = '{this.dr["Seq2"]}'
group by a1.IssueDate,a1.Id,a1.Remark,Reason.value,a1.Type
order by a1.IssueDate, a1.Id

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
                    object ttlQty = dt.Compute("sum(AdjustQty)", null);
                    this.numTotal.Value = !MyUtility.Check.Empty(ttlQty) ? decimal.Parse(ttlQty.ToString()) : 0m;
                }
            }

            this.bindingSource1.DataSource = dt;

            #region 開窗
            DataGridViewGeneratorTextColumnSettings col_ID = new DataGridViewGeneratorTextColumnSettings();
            col_ID.CellMouseDoubleClick += (s, e) =>
            {
                var dr2 = this.gridAdjust.GetDataRow<DataRow>(e.RowIndex);
                if (dr2 == null)
                {
                    return;
                }

                var frm = new Win.Tems.Input6(null);
                switch (dr2["Name"].ToString())
                {
                    case "P35. Adjust Bulk Qty":
                        frm = new P35(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                    case "P34. Adjust Stock Qty":
                        frm = new P34(null, dr2["id"].ToString());
                        frm.ShowDialog(this);
                        break;
                }
            };
            #endregion

            this.gridAdjust.IsEditingReadOnly = true;
            this.gridAdjust.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAdjust)
            .Date("Date", header: "Date", width: Widths.AnsiChars(12))
            .Text("ID", header: "ID", width: Widths.AnsiChars(15), settings: col_ID)
            .Text("Name", header: "Name", width: Widths.AnsiChars(18))
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            .Text("Reason", header: "Reason", width: Widths.AnsiChars(30))
            ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
