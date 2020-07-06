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
    public partial class P33_IssueBreakDown : Sci.Win.Subs.Base
    {
        DataRow Master;
        DataTable dtQtyBreakDown, DtIssueBreakDown, DtSizeCode, DtModifyIssueBDown;
        StringBuilder sbSizecode;
        public P33_IssueBreakDown()
        {
            InitializeComponent();
            this.EditMode = true;
        }

        public P33_IssueBreakDown(DataRow _master, DataTable _dtIssueBreakDown, DataTable _dtSizeCode)
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

            this.displayColor.BackColor = Color.LightGray;
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
                //sbSizecode.Append(string.Format(@"[{0}],", DtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
                sbSizecode.Append($@"[{DtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()}],");
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

            DtModifyIssueBDown.ColumnsBooleanAdd("Selected", defaultValue: true);
            //設定Columns 位置
            DtModifyIssueBDown.Columns["Selected"].SetOrdinal(0);

            DataTable ReadonlyDT;
            result = DBProxy.Current.Select(null, "SELECT ID FROM Orders WITH(NOLOCK) WHERE (Junk=1 AND NeedProduction = 0) OR (IsBuyBack=1 AND BuyBackReason='Garment') ", out ReadonlyDT);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            
            if (ReadonlyDT.Rows.Count > 0)
            {
                List<string> ReadonlyList = ReadonlyDT.AsEnumerable().Select(o => o["ID"].ToString()).Distinct().ToList();

                foreach (DataRow item in DtModifyIssueBDown.Rows)
                {
                    if (ReadonlyList.Where(o => o == item["OrderID"].ToString()).Any())
                    {
                        item["Selected"] = false;
                    }
                }
            }

            gridIssueBreakDownBS.DataSource = DtModifyIssueBDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = false;
            if (gridIssueBreakDown.ColumnCount > 0)
            {
                gridIssueBreakDown.Columns[1].ReadOnly = true;
            }

            if (gridIssueBreakDown.ColumnCount > 1)
            {
                gridIssueBreakDown.Columns[1].ReadOnly = true;
                gridIssueBreakDown.Columns[2].ReadOnly = true;
                gridIssueBreakDown.Columns[2].Frozen = true;
            }

            gridIssueBreakDown.Columns["Selected"].HeaderText = "";
            gridIssueBreakDown.Columns["Selected"].Width = 25;

            this.HideNullColumn(gridQtyBreakDown);
            this.HideNullColumn(gridIssueBreakDown);

            this.Pink();
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

        /// <summary>
        /// Header自動勾選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridIssueBreakDown_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DtModifyIssueBDown == null) return;

            DataTable readonlyDt;
            DBProxy.Current.Select(null, "SELECT ID FROM Orders WITH(NOLOCK) WHERE (Junk=1 AND NeedProduction = 0) OR (IsBuyBack=1 AND BuyBackReason='Garment') ", out readonlyDt);

            // 自動勾選Select
            if (MouseButtons.Left == e.Button && 0 == e.ColumnIndex)
            {
                DataTable dt = (DataTable)gridIssueBreakDownBS.DataSource;
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

        private void gridIssueBreakDown_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            var dataRow = this.gridIssueBreakDown.GetDataRow(this.gridIssueBreakDownBS.Position);
            if (e.ColumnIndex > 1 && null != dataRow) 
            { 
                string col_name = this.gridIssueBreakDown.Columns[e.ColumnIndex].DataPropertyName;
                if (dataRow[col_name].Empty())
                {
                    if (!(dataRow[col_name] == DBNull.Value))
                    {
                        dataRow[col_name] = 0;
                    }
                }
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

        private void gridIssueBreakDown_Sorted(object sender, EventArgs e)
        {
            this.Pink();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (dtQtyBreakDown == null || DtModifyIssueBDown == null) return;
            
            DataTable dtCopy = ((DataTable)gridIssueBreakDownBS.DataSource).Copy();
            DataTable dt = (DataTable)gridIssueBreakDownBS.DataSource;

            dt.Rows.Clear();
            if (!MyUtility.Check.Empty(dtCopy.Select("Selected = 0")))
            {
                // 將Select未勾選的結構匯入
                DataTable dtUnSelect = dtCopy.Select("Selected = 0").CopyToDataTable();
                dt.Merge(dtUnSelect);
            }
            
            foreach (DataRow tmprow in dtQtyBreakDown.Rows)
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

        private void HideNullColumn(Win.UI.Grid grid)
        {

            List<string> nullCol = new List<string>();
            foreach (DataGridViewColumn Column in grid.Columns)
            {
                int rowCount = 0;
                int nullCount = 0;
                string ColumnName = Column.Name;
                if (ColumnName != "Selected" && ColumnName != "Article" && ColumnName != "OrderID")
                {
                    foreach (DataGridViewRow Row in grid.Rows)
                    {
                        string val = Row.Cells[ColumnName].Value.ToString();
                        if (MyUtility.Check.Empty(val))
                        {
                            nullCount++;
                        }
                        rowCount++;
                    }
                    if (rowCount == nullCount)
                    {
                        nullCol.Add(ColumnName);
                    }
                }
            }

            foreach (var col in nullCol)
            {
                grid.Columns[col].Visible = false;
            }
        }

        private void Pink()
        {
            DataTable dt;
            DBProxy.Current.Select(null, "SELECT ID FROM Orders WITH(NOLOCK) WHERE (Junk=1 AND NeedProduction = 0) OR (IsBuyBack=1 AND BuyBackReason='Garment')", out dt);

            foreach (DataGridViewColumn Column in gridIssueBreakDown.Columns)
            {
                string ColumnName = Column.Name;

                if (ColumnName != "Selected" && ColumnName != "Article" && ColumnName != "OrderID")
                {
                    foreach (DataGridViewRow Row in gridIssueBreakDown.Rows)
                    {
                        string val = Row.Cells[ColumnName].Value.ToString();
                        if (!MyUtility.Check.Empty(val))
                        {
                            Row.Cells[ColumnName].Style.BackColor = Color.Pink;
                        }
                        else
                        {
                            Row.Cells[ColumnName].ReadOnly = true;
                        }
                    }
                }
                if (ColumnName == "Selected")
                {

                    foreach (DataGridViewRow Row in gridIssueBreakDown.Rows)
                    {
                        Row.Cells[ColumnName].Style.BackColor = Color.Pink;
                    }
                }
            }

            foreach (DataGridViewRow Row in gridIssueBreakDown.Rows)
            {
                string orderID = Row.Cells["OrderID"].Value.ToString();

                DataRow[] s = dt.Select($"ID = '{orderID}'");

                if (s.Length > 0)
                {
                    foreach (DataGridViewColumn Column in gridIssueBreakDown.Columns)
                    {
                        Row.Cells[Column.Name].Style.BackColor = Color.LightGray;
                        Row.ReadOnly = true;
                    }
                    Row.Cells["Selected"].Value = false;
                }

            }

            foreach (DataGridViewRow Row in gridQtyBreakDown.Rows)
            {
                string orderID = Row.Cells["OrderID"].Value.ToString();

                DataRow[] s = dt.Select($"ID = '{orderID}'");

                if (s.Length > 0)
                {
                    foreach (DataGridViewColumn Column in gridQtyBreakDown.Columns)
                    {
                        Row.Cells[Column.Name].Style.BackColor = Color.LightGray;
                        Row.ReadOnly = true;
                    }
                }

            }
        }
    }
}
