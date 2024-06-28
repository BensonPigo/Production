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
        private readonly string id;

        /// <inheritdoc/>
        public P09_History(string id)
        {
            this.InitializeComponent();
            this.id = id;
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
            this.Helper.Controls.Grid.Generator(this.gridOriCutRef)
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
            string sqlcmd = $@"
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutputHistory WITH (NOLOCK) WHERE ID = '{this.id}' AND GroupID <> '' ORDER BY GroupID, CutRef
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutput WITH (NOLOCK) WHERE ID = '{this.id}' AND GroupID <> '' ORDER BY GroupID, CutRef
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutputDelete WITH (NOLOCK) WHERE ID = '{this.id}' AND GroupID <> '' ORDER BY GroupID, CutRef
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dts);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.oriCutRefbs.DataSource = dts[0];
            this.currentCutRefbs.DataSource = dts[1];
            this.removeListbs.DataSource = dts[2];

            DataTable oridt = dts[0].DefaultView.ToTable(true, "CutRef");
            DataTable curdt = dts[1].DefaultView.ToTable(true, "CutRef");

            DataTable oridt2 = this.AddEmptyItem(oridt);
            DataTable curdt2 = this.AddEmptyItem(curdt);

            MyUtility.Tool.SetupCombox(this.cmbOriCutRef, 1, oridt2);
            MyUtility.Tool.SetupCombox(this.cmbCurrentCutRef, 1, curdt2);

            this.displayBox1.Text = dts[0].AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])).ToString();
            this.displayBox2.Text = dts[1].AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])).ToString();
            this.displayBox3.Text = dts[2].AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])).ToString();

            // 綁定 DataSource 後加事件
            this.gridOriCutRef.RowEnter += this.Grid_RowEnter;
            this.gridCurrentCutRef.RowEnter += this.Grid_RowEnter;
            this.gridRemoveList.RowEnter += this.Grid_RowEnter;
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

        private void BtnClean_Click(object sender, EventArgs e)
        {
            this.cmbOriCutRef.SelectedIndex = 0;
            this.cmbCurrentCutRef.SelectedIndex = 0;
            this.ResetRowColors(this.gridOriCutRef);
            this.ResetRowColors(this.gridCurrentCutRef);
            this.ResetRowColors(this.gridRemoveList);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            this.HighlightAndScrollToRows(this.gridOriCutRef, clickedGrid, groupID);
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
            string filter = $"CutRef = '{this.cmbOriCutRef.SelectedValue}'";
            if (MyUtility.Check.Empty(this.cmbOriCutRef.SelectedValue))
            {
                filter = string.Empty;
            }

            this.oriCutRefbs.Filter = filter;
        }

        private void CmbCurrentCutRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = $"CutRef = '{this.cmbCurrentCutRef.SelectedValue}'";
            if (MyUtility.Check.Empty(this.cmbCurrentCutRef.SelectedValue))
            {
                filter = string.Empty;
            }

            this.currentCutRefbs.Filter = filter;
        }
    }
}
