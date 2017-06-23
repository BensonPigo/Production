using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P11_IssueBreakDown : Sci.Win.Subs.Base
    {
        DataRow Master;
        DataTable dtQtyBreakDown, DtIssueBreakDown, DtSizeCode, DtModifyIssueBDown;
        StringBuilder sbSizecode;
        public P11_IssueBreakDown()
        {
            InitializeComponent();
            this.EditMode = true;
        }

        public P11_IssueBreakDown(DataRow _master, DataTable _dtIssueBreakDown, DataTable _dtSizeCode)
            : this()
        {
            DtSizeCode = _dtSizeCode;
            DtIssueBreakDown = _dtIssueBreakDown;
            Master = _master;
            DtModifyIssueBDown = _dtIssueBreakDown.Clone();
            foreach (DataRow dr in DtIssueBreakDown.Rows){
                DtModifyIssueBDown.ImportRow(dr);
            }
            DtModifyIssueBDown.DefaultView.RowFilter = DtIssueBreakDown.DefaultView.RowFilter;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder sbQtyBreakDown;
            DualResult result;            
            if (DtSizeCode==null || DtSizeCode.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", Master["orderid"]));
                return;
            }

            sbSizecode = new StringBuilder();
            sbSizecode.Clear();
            for (int i = 0; i < DtSizeCode.Rows.Count; i++)
            {
                sbSizecode.Append(string.Format(@"[{0}],", DtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }

            sbQtyBreakDown = new StringBuilder();
            sbQtyBreakDown.Append(string.Format(@"
;with Bdown as (
    select  a.ID [OrderID]
            , a.Article
            , a.SizeCode
            , a.Qty 
    from dbo.order_qty a WITH (NOLOCK) 
    inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
    where b.POID = (select poid 
                    from dbo.orders WITH (NOLOCK) 
                    where id = '{0}')
)
select * from Bdown
pivot
(
	sum(qty)
	for sizecode in ({1})
)as pvt
order by [OrderID], [Article]", Master["orderid"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));//.Replace("[", "[_")
            if (!(result = DBProxy.Current.Select(null, sbQtyBreakDown.ToString(), out dtQtyBreakDown)))
            {
                ShowErr(sbQtyBreakDown.ToString(), result);
                return;
            }
            gridQtyBreakDown.AutoGenerateColumns = true;
            gridQtyBreakDownBS.DataSource = dtQtyBreakDown;
            gridQtyBreakDown.DataSource = gridQtyBreakDownBS;
            gridQtyBreakDown.IsEditingReadOnly = true;
            gridQtyBreakDown.ReadOnly = true;
            if (gridQtyBreakDown.ColumnCount > 1) gridQtyBreakDown.Columns[1].Frozen = true;

            gridIssueBreakDown.AutoGenerateColumns = true;
            gridIssueBreakDownBS.DataSource = DtModifyIssueBDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = false;
            if (gridIssueBreakDown.ColumnCount > 0) gridIssueBreakDown.Columns[0].ReadOnly = true;
            if (gridIssueBreakDown.ColumnCount > 1) gridIssueBreakDown.Columns[1].ReadOnly = true;
            if (gridIssueBreakDown.ColumnCount > 1) gridIssueBreakDown.Columns[1].Frozen = true;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {           
            if (!this.gridIssueBreakDown.ValidateControl()) { return; }
            DtIssueBreakDown.Clear();
            foreach (DataRow dr in DtModifyIssueBDown.Rows)
            {
                DtIssueBreakDown.ImportRow(dr);
            }
            this.Dispose();
            return;
        }

        private void gridIssueBreakDown_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && null != dataRow) 
            { 
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty()) dataRow[col_name] = 0;
            }
        }

        private void gridIssueBreakDown_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && null != dataRow)
            {
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty()) dataRow[col_name] = 0;
            }
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (dtQtyBreakDown == null || DtModifyIssueBDown == null) return;
            ((DataTable)gridIssueBreakDownBS.DataSource).Rows.Clear();
            foreach (DataRow tmprow in dtQtyBreakDown.Rows)
            {
                ((DataTable)gridIssueBreakDownBS.DataSource).ImportRow(tmprow);
            }
        }

    }
}
