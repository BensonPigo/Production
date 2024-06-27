using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
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
                .Text("CutRef", header: "CutRef", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("GroupID", header: "Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridCurrentCutRef)
                .Text("CutRef", header: "CutRef", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("GroupID", header: "Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridRemoveList)
                .Text("CutRef", header: "CutRef", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("GroupID", header: "Group", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutputHistory WITH (NOLOCK) WHERE ID = '{this.id}' ORDER BY GroupID, CutRef
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutput WITH (NOLOCK) WHERE ID = '{this.id}' ORDER BY GroupID, CutRef
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutputDelete WITH (NOLOCK) WHERE ID = '{this.id}' ORDER BY GroupID, CutRef
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
            MyUtility.Tool.SetupCombox(this.cmbOriCutRef, 1, oridt);
            MyUtility.Tool.SetupCombox(this.cmbCurrentCutRef, 1, curdt);

            this.displayBox1.Text = dts[0].Rows.Count.ToString();
            this.displayBox2.Text = dts[1].Rows.Count.ToString();
            this.displayBox3.Text = dts[2].Rows.Count.ToString();
        }

        private void BtnClean_Click(object sender, EventArgs e)
        {
            this.cmbOriCutRef.SelectedIndex = 0;
            this.cmbCurrentCutRef.SelectedIndex = 0;
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

            DataGridView currentGrid = sender as DataGridView;

            // 行的 GroupID
            DataGridViewRow selectedRow = currentGrid.Rows[e.RowIndex];
            string selectedGroupID = selectedRow.Cells["GroupID"].Value.ToString();

            // 高亮和定位的方法
            this.HighlightAndScrollToRows(this.gridOriCutRef, selectedGroupID);
            this.HighlightAndScrollToRows(this.gridCurrentCutRef, selectedGroupID);
            this.HighlightAndScrollToRows(this.gridRemoveList, selectedGroupID);
        }

        private void HighlightAndScrollToRows(DataGridView grid, string groupID)
        {
            bool firstMatchFound = false;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells["GroupID"].Value.ToString() == groupID)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 153); // 黄色

                    if (!firstMatchFound)
                    {
                        grid.FirstDisplayedScrollingRowIndex = row.Index; // 定位到第一筆
                        firstMatchFound = true;
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White; // 恢复背景色
                }
            }
        }
    }
}
