using Ict;
using Ict.Win;
using Microsoft.SqlServer.Management.Smo.Agent;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05
    /// </summary>
    public partial class P05 : Sci.Win.Tems.Input6
    {
        private DataTable dtAutomatedLineMapping_DetailTemp = new DataTable();
        private DataTable dtAutomatedLineMapping_DetailAuto = new DataTable();
        private DataTable dtGridDetailRightSummary = new DataTable();

        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboPhase, 2, 1, ",,Initial,Initial,Prelim,Prelim");

            this.splitLineMapping.Panel1.Controls.Add(this.detailgrid);
            this.gridCentralizedPPALeft.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.detailgridbs.DataSourceChanged += this.Detailgridbs_DataSourceChanged;
            this.gridCentralizedPPALeft.DataSource = this.gridCentralizedPPALeftBS;

            this.detailgrid.CellPainting += this.Detailgrid_CellPainting;

            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("No", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("NoCnt", typeof(int)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("TotalGSDTime", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("OperatorLoading", typeof(decimal)));

            this.gridLineMappingRight.DataSource = this.dtGridDetailRightSummary;
            this.gridCentralizedPPARight.DataSource = this.dtGridDetailRightSummary;
            this.detailgrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridCentralizedPPALeft.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        }

        private void Detailgridbs_DataSourceChanged(object sender, EventArgs e)
        {
            this.gridCentralizedPPALeftBS.DataSource = this.detailgridbs.DataSource;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.FilterGrid();

            this.detailgrid.ColumnHeadersHeight = this.gridLineMappingRight.ColumnHeadersHeight;
            this.gridCentralizedPPALeft.ColumnHeadersHeight = this.gridCentralizedPPARight.ColumnHeadersHeight;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            P05_CreateNewLineMapping p05_CreateNewLineMapping = new P05_CreateNewLineMapping(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.dtAutomatedLineMapping_DetailTemp, this.dtAutomatedLineMapping_DetailAuto);
            p05_CreateNewLineMapping.ShowDialog();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.detailgridbs.RemoveFilter();
            this.gridCentralizedPPALeftBS.RemoveFilter();
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("No", header: "No", width: Widths.AnsiChars(4))
               .CheckBox("select", string.Empty)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13))
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23));

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPALeft)
               .Text("No", header: "No", width: Widths.AnsiChars(4))
               .Text("Location", header: "Location", width: Widths.AnsiChars(13))
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23));

            this.Helper.Controls.Grid.Generator(this.gridLineMappingRight)
               .Text("No", header: "PPA" + Environment.NewLine + "No.", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridCentralizedPPARight)
               .Text("No", header: "PPA" + Environment.NewLine + "No.", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));
        }

        private void TabDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FilterGrid();
        }

        private void FilterGrid()
        {
            if (this.tabDetail.SelectedIndex == 0)
            {
                this.detailgridbs.Filter = "PPA <> 'C' and IsNonSewingLine = 0";
            }
            else
            {
                this.gridCentralizedPPALeftBS.Filter = "PPA = 'C' and IsNonSewingLine = 0";
            }

            this.RefreshRightSummaryGrid();
        }

        private void Detailgrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 0 || e.ColumnIndex == 1))
            {
                DataGridView grid = (DataGridView)sender;
                string curNo = grid[0, e.RowIndex].Value.ToString();
                string nextNo = e.RowIndex == this.DetailDatas.Count - 1 ? string.Empty : grid[0, e.RowIndex + 1].Value.ToString();
                string preNo = e.RowIndex == 0 ? null : grid[0, e.RowIndex - 1].Value.ToString();

                if (curNo == preNo)
                {
                    // 在儲存格內繪製空白矩形
                    using (SolidBrush brush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }

                    // 繪製儲存格的右邊框線
                    using (Pen pen = new Pen(grid.GridColor))
                    {
                        e.Graphics.DrawLine(pen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                        if (curNo != nextNo)
                        {
                            e.Graphics.DrawLine(pen, e.CellBounds.Left + 1, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                        }
                    }

                    e.Handled = true;
                }
                else if (curNo == nextNo)
                {
                    // 在儲存格內繪製空白矩形
                    using (SolidBrush brush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }

                    using (Pen pen = new Pen(grid.GridColor))
                    {
                        e.Graphics.DrawLine(pen, e.CellBounds.Left + 1, e.CellBounds.Top - 1, e.CellBounds.Right - 1, e.CellBounds.Top - 1);
                        e.Graphics.DrawLine(pen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    }

                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void RefreshRightSummaryGrid()
        {
            this.dtGridDetailRightSummary.Clear();
            List<DataRow> resultRows;
            if (this.tabDetail.SelectedIndex == 0)
            {
                resultRows = this.DetailDatas.Where(s => s["PPA"].ToString() != "C" && !MyUtility.Convert.GetBool(s["IsNonSewingLine"]))
                                .GroupBy(s => new
                                {
                                    No = s["No"].ToString(),
                                })
                                .Select(groupItem =>
                                {
                                    DataRow newRow = this.dtGridDetailRightSummary.NewRow();
                                    newRow["No"] = groupItem.Key.No;
                                    newRow["NoCnt"] = groupItem.Count();
                                    newRow["TotalGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"])), 2);
                                    newRow["OperatorLoading"] = MyUtility.Check.Empty(this.CurrentMaintain["AvgGSDTime"]) ? 0 : MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"])) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["AvgGSDTime"]) * 100, 0);
                                    return newRow;
                                }).ToList();
            }
            else
            {
                var dataPPA = this.DetailDatas.Where(s => s["PPA"].ToString() == "C" && !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) && !MyUtility.Check.Empty(s["No"]));
                if (!dataPPA.Any())
                {
                    this.gridCentralizedPPARight.DataSource = null;
                    return;
                }

                decimal avgGSDTimePPA = dataPPA.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])) / dataPPA.Select(s => s["No"].ToString()).Distinct().Count();

                resultRows = dataPPA.GroupBy(s => new
                {
                    No = s["No"].ToString(),
                }).Select(groupItem =>
                {
                    DataRow newRow = this.dtGridDetailRightSummary.NewRow();
                    newRow["No"] = groupItem.Key.No;
                    newRow["NoCnt"] = groupItem.Count();
                    newRow["TotalGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])), 2);
                    newRow["OperatorLoading"] = avgGSDTimePPA == 0 ? 0 : MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])) / avgGSDTimePPA * 100, 0);
                    return newRow;
                }).ToList();
            }

            foreach (DataRow dr in resultRows)
            {
                this.dtGridDetailRightSummary.Rows.Add(dr);
            }

            foreach (DataGridViewRow gridRow in this.gridLineMappingRight.Rows)
            {
                DataRow dr = this.gridLineMappingRight.GetDataRow(gridRow.Index);

                gridRow.Height = gridRow.Height * MyUtility.Convert.GetInt(dr["NoCnt"]);
            }

            foreach (DataGridViewRow gridRow in this.gridCentralizedPPARight.Rows)
            {
                DataRow dr = this.gridCentralizedPPARight.GetDataRow(gridRow.Index);

                gridRow.Height = gridRow.Height * MyUtility.Convert.GetInt(dr["NoCnt"]);
            }
        }
    }
}
