using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P04_ScrapQty : Sci.Win.Subs.Base
    {
        private string Poid;
        private string Refno;
        private string Color;

        public P04_ScrapQty(string Poid, string Refno, string Color)
        {
            this.InitializeComponent();
            this.Poid = Poid;
            this.Refno = Refno;
            this.Color = Color;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Grid Setting
            this.Helper.Controls.Grid.Generator(this.grid)
                .Date("IssueDate", header: "Date", iseditingreadonly: true)
                .Text("TransactionID", header: "Transaction ID", iseditingreadonly: true)
                .Text("Name", header: "Name", iseditingreadonly: true)
                .Numeric("InQty", header: "In Qty", iseditingreadonly: true)
                .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true)
                .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", iseditingreadonly: true);
            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion
            #region Sql Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@Poid", this.Poid));
            listSqlParameter.Add(new SqlParameter("@Refno", this.Refno));
            listSqlParameter.Add(new SqlParameter("@Color", this.Color));
            #endregion
            #region Sql Command
            string strSqlCmd = @"
select	a.IssueDate
		, a.TransactionID
		, a.Name
		, InQty = iif (a.InQty = 0, '', Convert (varchar, a.InQty))
		, OutQty = iif (a.OutQty = 0, '', Convert (varchar, a.OutQty))
		, AdjustQty = iif (a.AdjustQty = 0, '', Convert (varchar, a.AdjustQty))
		, Balance = SUM (Balance) over (order by a.IssueDate, a.Sequence)
from (
	select	IssueDate = a.IssueDate
			, TransactionID = a.Id
			, Name = 'P47 Transfer Bulk to Scrap (A2C)(Local)'
			, InQty = isnull (sum (b.Qty), 0)
			, OutQty = 0
			, AdjustQty = 0
			, Balance = isnull (sum (b.Qty), 0)
			, Sequence = 1
	from SubTransferLocal a
    inner join SubTransferLocal_Detail b on a.Id = b.ID
				                            and b.Poid = @Poid
				                            and b.Refno = @Refno
				                            and b.Color = @Color
	where	a.Status = 'Confirmed'
			and a.Type = 'D'
    group by a.IssueDate, a.ID

	union all
	select	IssueDate = a.IssueDate
			, TransactionID = a.Id
			, Name = 'P46 Remove from Scrap Whse (Local)'
			, InQty = 0
			, OutQty = isnull (sum (b.QtyAfter - b.QtyBefore), 0)
			, AdjustQty = 0
			, Balance = - isnull (sum (b.QtyAfter - b.QtyBefore), 0)
			, Sequence = 3
	from AdjustLocal a
    inner join AdjustLocal_Detail b on a.Id = b.ID
				                       and b.POID = @Poid
				                       and b.Refno = @Refno
				                       and b.Color = @Color
	where	a.Status = 'Confirmed'
			and a.Type = 'R'
    group by a.IssueDate, a.ID

	union all
	select	IssueDate = a.IssueDate
			, TransactionID = a.Id
			, Name = 'P44 Adjust Scrap Qty (Local)'
			, InQty = 0
			, OutQty = 0
			, AdjustQty = isnull (sum (b.QtyAfter - b.QtyBefore), 0)
			, Balance = isnull (sum (b.QtyAfter - b.QtyBefore), 0)
			, Sequence = 2
	from AdjustLocal a
    inner join AdjustLocal_Detail b on a.Id = b.ID
				                       and b.POID = @Poid
				                       and b.Refno = @Refno
				                       and b.Color = @Color
	where	a.Status = 'Confirmed'
			and a.Type = 'C'
            group by a.IssueDate, a.ID
) a";
            #endregion
            #region Data Setting
            DataTable dtSetData;
            DualResult result = DBProxy.Current.Select(null, strSqlCmd, listSqlParameter, out dtSetData);
            if (result)
            {
                this.bindingSource.DataSource = dtSetData;
            }
            else
            {
                this.bindingSource.DataSource = null;
                MyUtility.Msg.WarningBox(result.Description);
            }
            #endregion
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
