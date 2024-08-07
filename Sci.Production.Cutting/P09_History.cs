using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09_History : Sci.Win.Tems.QueryForm
    {
        private DataTable[] dtsHistory;
        private DataTable dtAlloriginalCutref;
        private DataTable dtAllcurrentCutref;

        /// <inheritdoc/>
        public P09_History(DataTable[] dtsHistory)
        {
            this.InitializeComponent();
            this.dtsHistory = dtsHistory;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridOriginalCutRef)
                .Text("CutRef", header: "CutRef", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("GroupID", header: "Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridCurrentCutRef)
                .Text("CutRef", header: "CutRef", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("GroupID", header: "Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridRemoveList)
                .Text("CutRef", header: "CutRef", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("GroupID", header: "Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            this.originalCutRefbs.DataSource = this.dtsHistory[0].Copy();
            this.currentCutRefbs.DataSource = this.dtsHistory[1].Copy();
            this.removeListbs.DataSource = this.dtsHistory[2].Copy();

            DataTable oridt = this.dtsHistory[0].DefaultView.ToTable(true, "CutRef");
            DataTable curdt = this.dtsHistory[1].DefaultView.ToTable(true, "CutRef");

            this.dtAlloriginalCutref = this.AddEmptyItem(oridt);
            this.dtAllcurrentCutref = this.AddEmptyItem(curdt);

            this.SetupCombobox();

            // 綁定 DataSource 後加事件
            this.gridOriginalCutRef.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridView_CellFormatting);
            this.gridCurrentCutRef.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridView_CellFormatting);
            this.gridRemoveList.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridView_CellFormatting);

            this.AddRowEnterEvent();
            this.Cal3TTLQty();
        }

        private DataTable AddEmptyItem(DataTable dt)
        {
            DataTable dt2 = dt.Clone();
            DataRow newRow = dt2.NewRow();
            newRow[0] = string.Empty;
            dt2.Rows.Add(newRow);
            dt2.Merge(dt);
            return dt2;
        }

        private void SetupCombobox()
        {
            MyUtility.Tool.SetupCombox(this.cmbOriginalCutRef, 1, this.dtAlloriginalCutref);
            MyUtility.Tool.SetupCombox(this.cmbCurrentCutRef, 1, this.dtAllcurrentCutref);
        }

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)
            {
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
        }

        private void AddRowEnterEvent()
        {
            this.gridOriginalCutRef.RowEnter += this.Grid_RowEnter;
            this.gridCurrentCutRef.RowEnter += this.Grid_RowEnter;
            this.gridRemoveList.RowEnter += this.Grid_RowEnter;
        }

        private void Cal3TTLQty()
        {
            this.displayBox1.Text = ((DataView)this.originalCutRefbs.List).ToTable().AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])).ToString();
            this.displayBox2.Text = ((DataView)this.currentCutRefbs.List).ToTable().AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])).ToString();
            this.displayBox3.Text = ((DataView)this.removeListbs.List).ToTable().AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])).ToString();
        }

        private void BtnClean_Click(object sender, EventArgs e)
        {
            this.SetupCombobox();
            this.cmbOriginalCutRef.SelectedIndex = 0;
            this.cmbCurrentCutRef.SelectedIndex = 0;
            this.ResetRowColors(this.gridOriginalCutRef);
            this.ResetRowColors(this.gridCurrentCutRef);
            this.ResetRowColors(this.gridRemoveList);
        }

        private void Grid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridView clickedGrid = (DataGridView)sender;
            string groupID = MyUtility.Convert.GetString(clickedGrid.Rows[e.RowIndex].Cells["GroupID"].Value);

            // Highlight and scroll to matching rows in all grids
            this.HighlightAndScrollToRows(this.gridOriginalCutRef, clickedGrid, groupID);
            this.HighlightAndScrollToRows(this.gridCurrentCutRef, clickedGrid, groupID);
            this.HighlightAndScrollToRows(this.gridRemoveList, clickedGrid, groupID);
        }

        private void HighlightAndScrollToRows(DataGridView currentGrid, DataGridView clickedGrid, string groupID)
        {
            bool firstMatchFound = false;

            foreach (DataGridViewRow row in currentGrid.Rows)
            {
                if (MyUtility.Convert.GetString(row.Cells["GroupID"].Value) == groupID)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 153); // 黄色

                    if (!firstMatchFound && currentGrid != clickedGrid)
                    {
                        currentGrid.FirstDisplayedScrollingRowIndex = row.Index; // 定位到第一筆
                        firstMatchFound = true;
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White; // 恢复背景色
                }
            }
        }

        private void ResetRowColors(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void CmbOriCutRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = $"CutRef = '{this.cmbOriginalCutRef.SelectedValue}'";
            if (MyUtility.Check.Empty(this.cmbOriginalCutRef.SelectedValue))
            {
                filter = string.Empty;
            }

            this.originalCutRefbs.Filter = filter;

            this.FilterBasedOnGroupID(this.originalCutRefbs, this.currentCutRefbs, this.removeListbs);
        }

        private void CmbCurrentCutRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = $"CutRef = '{this.cmbCurrentCutRef.SelectedValue}'";
            if (MyUtility.Check.Empty(this.cmbCurrentCutRef.SelectedValue))
            {
                filter = string.Empty;
            }

            this.currentCutRefbs.Filter = filter;

            this.FilterBasedOnGroupID(this.currentCutRefbs, this.originalCutRefbs, this.removeListbs);
        }

        private void FilterBasedOnGroupID(BindingSource sourceBs, BindingSource targetBs1, BindingSource targetBs2)
        {
            DataTable sourceTable = ((DataView)sourceBs.List).ToTable();
            string groupIDFilter = string.Join(",", sourceTable.AsEnumerable().Select(row => $"'{row["GroupID"]}'").Distinct());

            if (MyUtility.Check.Empty(groupIDFilter))
            {
                targetBs1.Filter = string.Empty;
                targetBs2.Filter = string.Empty;
            }
            else
            {
                targetBs1.Filter = $"GroupID IN ({groupIDFilter})";
                targetBs2.Filter = $"GroupID IN ({groupIDFilter})";
            }

            this.Cal3TTLQty();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
