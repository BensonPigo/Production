using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P11_IssueBreakDown : Win.Subs.Base
    {
        private DataRow Master;
        private DataTable dtQtyBreakDown;
        private DataTable DtIssueBreakDown;
        private DataTable DtSizeCode;
        private DataTable DtModifyIssueBDown;
        private StringBuilder sbSizecode;

        public P11_IssueBreakDown()
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        public P11_IssueBreakDown(DataRow master, DataTable dtIssueBreakDown, DataTable dtSizeCode)
            : this()
        {
            this.DtSizeCode = dtSizeCode;
            this.DtIssueBreakDown = dtIssueBreakDown;
            this.Master = master;
            this.DtModifyIssueBDown = dtIssueBreakDown.Clone();
            foreach (DataRow dr in this.DtIssueBreakDown.Rows)
            {
                this.DtModifyIssueBDown.ImportRow(dr);
            }

            this.DtModifyIssueBDown.DefaultView.RowFilter = this.DtIssueBreakDown.DefaultView.RowFilter;

            this.displayColor.BackColor = Color.LightGray;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder sbQtyBreakDown;
            DualResult result;
            if (this.DtSizeCode == null || this.DtSizeCode.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", this.Master["orderid"]));
                return;
            }

            this.sbSizecode = new StringBuilder();
            this.sbSizecode.Clear();
            for (int i = 0; i < this.DtSizeCode.Rows.Count; i++)
            {
                this.sbSizecode.Append(string.Format(@"[{0}],", this.DtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }

            sbQtyBreakDown = new StringBuilder();
            sbQtyBreakDown.Append(string.Format(
                @"
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
order by [OrderID], [Article]", this.Master["orderid"], this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1))); // .Replace("[", "[_")
            if (!(result = DBProxy.Current.Select(null, sbQtyBreakDown.ToString(), out this.dtQtyBreakDown)))
            {
                this.ShowErr(sbQtyBreakDown.ToString(), result);
                return;
            }

            this.gridQtyBreakDown.AutoGenerateColumns = true;
            this.gridQtyBreakDownBS.DataSource = this.dtQtyBreakDown;
            this.gridQtyBreakDown.DataSource = this.gridQtyBreakDownBS;
            this.gridQtyBreakDown.IsEditingReadOnly = true;
            this.gridQtyBreakDown.ReadOnly = true;
            if (this.gridQtyBreakDown.ColumnCount > 1)
            {
                this.gridQtyBreakDown.Columns[1].Frozen = true;
            }

            this.gridIssueBreakDown.AutoGenerateColumns = true;

            this.DtModifyIssueBDown.ColumnsBooleanAdd("Selected", defaultValue: true);

            // 設定Columns 位置
            this.DtModifyIssueBDown.Columns["Selected"].SetOrdinal(0);

            DataTable readonlyDT;
            DBProxy.Current.Select(null, "SELECT ID FROM Orders WITH(NOLOCK) WHERE (Junk=1 AND NeedProduction = 0) OR (IsBuyBack=1 AND BuyBackReason='Garment') ", out readonlyDT);
            List<string> readonlyList = readonlyDT.AsEnumerable().Select(o => o["ID"].ToString()).Distinct().ToList();

            foreach (DataRow item in this.DtModifyIssueBDown.Rows)
            {
                if (readonlyList.Where(o => o == item["OrderID"].ToString()).Any())
                {
                    item["Selected"] = false;
                }
            }

            this.gridIssueBreakDownBS.DataSource = this.DtModifyIssueBDown;
            this.gridIssueBreakDown.DataSource = this.gridIssueBreakDownBS;
            this.gridIssueBreakDown.IsEditingReadOnly = false;
            if (this.gridIssueBreakDown.ColumnCount > 0)
            {
                this.gridIssueBreakDown.Columns[1].ReadOnly = true;
            }

            if (this.gridIssueBreakDown.ColumnCount > 1)
            {
                this.gridIssueBreakDown.Columns[1].ReadOnly = true;
                this.gridIssueBreakDown.Columns[2].ReadOnly = true;
                this.gridIssueBreakDown.Columns[2].Frozen = true;
            }

            this.gridIssueBreakDown.Columns["Selected"].HeaderText = string.Empty;
            this.gridIssueBreakDown.Columns["Selected"].Width = 25;

            this.Pink();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!this.gridIssueBreakDown.ValidateControl())
            {
                return;
            }

            this.DtIssueBreakDown.Clear();
            foreach (DataRow dr in this.DtModifyIssueBDown.Rows)
            {
                this.DtIssueBreakDown.ImportRow(dr);
            }

            this.Dispose();
            return;
        }

        /// <summary>
        /// Header自動勾選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridIssueBreakDown_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.DtModifyIssueBDown == null)
            {
                return;
            }

            DataTable readonlyDt;
            DBProxy.Current.Select(null, "SELECT ID FROM Orders WITH(NOLOCK) WHERE (Junk=1 AND NeedProduction = 0) OR (IsBuyBack=1 AND BuyBackReason='Garment') ", out readonlyDt);

            // 自動勾選Select
            if (e.Button == MouseButtons.Left && e.ColumnIndex == 0)
            {
                DataTable dt = (DataTable)this.gridIssueBreakDownBS.DataSource;
                DataRow[] drCheck = dt.Select("Selected = 1 ");
                if (drCheck.Length == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (readonlyDt.Select($"ID = '{dr["OrderId"]}'").Length > 0)
                        {
                            dr["Selected"] = 0;
                        }
                        else
                        {
                            dr["Selected"] = 1;
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Selected"] = 0;
                    }
                }
            }
        }

        private void GridIssueBreakDown_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && dataRow != null)
            {
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty())
                {
                    dataRow[col_name] = 0;
                }
            }
        }

        private void GridIssueBreakDown_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && dataRow != null)
            {
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty())
                {
                    dataRow[col_name] = 0;
                }
            }
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            if (this.dtQtyBreakDown == null || this.DtModifyIssueBDown == null)
            {
                return;
            }

            DataTable dtCopy = ((DataTable)this.gridIssueBreakDownBS.DataSource).Copy();
            DataTable dt = (DataTable)this.gridIssueBreakDownBS.DataSource;
            dt.Rows.Clear();
            if (!MyUtility.Check.Empty(dtCopy.Select("Selected = 0")))
            {
                // 將Select未勾選的結構匯入
                DataTable dtUnSelect = dtCopy.Select("Selected = 0").CopyToDataTable();
                dt.Merge(dtUnSelect);
            }

            foreach (DataRow tmprow in this.dtQtyBreakDown.Rows)
            {
                // 新增Select欄位, 有勾選才需要匯入Qty
                DataRow[] findrow = dtCopy.Select($@"Selected = 1 and OrderID = '{tmprow["OrderID"]}' and Article = '{tmprow["Article"]}'");
                if (findrow.Length > 0)
                {
                    dt.ImportRow(tmprow);
                }
            }

            dt.DefaultView.Sort = "OrderID,Article";

            this.Pink();
        }

        private void GridIssueBreakDown_Sorted(object sender, EventArgs e)
        {
            this.Pink();
        }

        private void Pink()
        {
            DataTable dt;
            DBProxy.Current.Select(null, "SELECT ID FROM Orders WITH(NOLOCK) WHERE (Junk=1 AND NeedProduction = 0) OR (IsBuyBack=1 AND BuyBackReason='Garment')", out dt);

            foreach (DataGridViewRow row in this.gridIssueBreakDown.Rows)
            {
                string orderID = row.Cells["OrderID"].Value.ToString();

                DataRow[] s = dt.Select($"ID = '{orderID}'");

                if (s.Length > 0)
                {
                    foreach (DataGridViewColumn column in this.gridIssueBreakDown.Columns)
                    {
                        row.Cells[column.Name].Style.BackColor = Color.LightGray;
                        row.ReadOnly = true;
                    }

                    row.Cells["Selected"].Value = false;
                }
            }

            foreach (DataGridViewRow row in this.gridQtyBreakDown.Rows)
            {
                string orderID = row.Cells["OrderID"].Value.ToString();

                DataRow[] s = dt.Select($"ID = '{orderID}'");

                if (s.Length > 0)
                {
                    foreach (DataGridViewColumn column in this.gridQtyBreakDown.Columns)
                    {
                        row.Cells[column.Name].Style.BackColor = Color.LightGray;
                        row.ReadOnly = true;
                    }
                }
            }
        }
    }
}
