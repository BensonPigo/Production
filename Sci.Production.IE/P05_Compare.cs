using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_Compare
    /// </summary>
    public partial class P05_Compare : Sci.Win.Tems.QueryForm
    {
        private List<DataTable> listManulData = new List<DataTable>();
        private List<DataTable> listAutoData = new List<DataTable>();
        private List<DataTable> listManulDataForDetail = new List<DataTable>();
        private List<DataTable> listAutoDataForDetail = new List<DataTable>();
        private DataRow drMain;
        private int firstDiaplaySewerManpower;
        private int minSewermanpower;
        private List<SummaryInfo> listAutoSummaryInfo = new List<SummaryInfo>();
        private List<SummaryInfo> listManualSummaryInfo = new List<SummaryInfo>();

        private class SummaryInfo
        {
            public int SewerManpower { get; set; }

            public int LBR { get; set; }

            public int HighestOPLoading { get; set; }
        }

        /// <summary>
        /// P05_Compare
        /// </summary>
        /// <param name="firstDiaplaySewerManpower">firstDiaplaySewerManpower</param>
        /// <param name="drMain">drMain</param>
        /// <param name="dtAutomatedLineMapping_Detail">dtAutomatedLineMapping_Detail</param>
        /// <param name="dtAutomatedLineMapping_DetailTemp">dtAutomatedLineMapping_DetailTemp</param>
        /// <param name="dtAutomatedLineMapping_DetailAuto">dtAutomatedLineMapping_DetailAuto</param>
        public P05_Compare(int firstDiaplaySewerManpower, DataRow drMain, DataTable dtAutomatedLineMapping_Detail, DataTable dtAutomatedLineMapping_DetailTemp, DataTable dtAutomatedLineMapping_DetailAuto)
        {
            this.InitializeComponent();

            this.drMain = drMain;
            this.firstDiaplaySewerManpower = firstDiaplaySewerManpower;
            this.minSewermanpower = dtAutomatedLineMapping_DetailAuto.AsEnumerable().Select(s => MyUtility.Convert.GetInt(s["SewerManpower"])).Min();

            this.GenerateCompareDatas(dtAutomatedLineMapping_Detail, dtAutomatedLineMapping_DetailTemp, dtAutomatedLineMapping_DetailAuto);
            this.tabCompare.SelectedIndexChanged += this.TabCompare_SelectedIndexChanged;

            this.gridAutoMain.Scroll += this.GridSync_Scroll;
            this.gridManualMain.Scroll += this.GridSync_Scroll;

            this.gridAutoMain.CellPainting += this.Grid_CellPainting;
            this.gridManualMain.CellPainting += this.Grid_CellPainting;
            this.gridAutoNoDetail.CellPainting += this.Grid_CellPainting;
            this.gridManualNoDetail.CellPainting += this.Grid_CellPainting;

            this.gridAutoMain.RowPrePaint += this.Grid_RowPrePaint;
            this.gridManualMain.RowPrePaint += this.Grid_RowPrePaint;

            this.gridAutoMain.SelectionChanged += this.Grid_SelectionChanged;
            this.gridManualMain.SelectionChanged += this.Grid_SelectionChanged;

            this.gridAutoMain.CellFormatting += this.Grid_CellFormatting;
            this.gridManualMain.CellFormatting += this.Grid_CellFormatting;

            this.gridAutoNoDetail.DataSource = this.gridAutoDetailBs;
            this.gridManualNoDetail.DataSource = this.gridManualDetailBs;

            this.gridAutoNoDetail.Scroll += this.GridAutoNoDetail_Scroll;
            this.gridManualNoDetail.Scroll += this.GridManualNoDetail_Scroll;
        }

        private void GridManualNoDetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                this.gridAutoNoDetail.HorizontalScrollingOffset = e.NewValue;
            }

            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll &&
                this.gridAutoNoDetail.Rows.Count > this.gridManualNoDetail.FirstDisplayedScrollingRowIndex)
            {
                this.gridAutoNoDetail.FirstDisplayedScrollingRowIndex = this.gridManualNoDetail.FirstDisplayedScrollingRowIndex;
            }
        }

        private void GridAutoNoDetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                this.gridManualNoDetail.HorizontalScrollingOffset = e.NewValue;
            }

            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll &&
                this.gridManualNoDetail.Rows.Count > this.gridAutoNoDetail.FirstDisplayedScrollingRowIndex)
            {
                this.gridManualNoDetail.FirstDisplayedScrollingRowIndex = this.gridAutoNoDetail.FirstDisplayedScrollingRowIndex;
            }
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;
            DataRow dr = sourceGrid.GetDataRow(e.RowIndex);
            if (e.ColumnIndex > 2)
            {
                e.CellStyle.BackColor = MyUtility.Convert.GetInt(dr["TimeStudyDetailUkeyCnt"]) > 1 ? Color.FromArgb(255, 255, 153) : sourceGrid.DefaultCellStyle.BackColor;
            }
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;
            if (sourceGrid.SelectedRows.Count == 0)
            {
                return;
            }

            string selectedNo = sourceGrid.SelectedRows[0].Cells["No"].Value.ToString();

            this.gridAutoDetailBs.Filter = $" No = '{selectedNo}'";
            this.gridManualDetailBs.Filter = $" No = '{selectedNo}'";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridAutoMain)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridManualMain)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridAutoNoDetail)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(50), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .Text("Attachment", header: "Attachment", width: Widths.AnsiChars(10))
               .Text("SewingMachineAttachmentID", header: "Part ID", width: Widths.AnsiChars(25))
               .Text("Template", header: "Template", width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("ThreadComboID", header: "Thread" + Environment.NewLine + "Combination", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridManualNoDetail)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(50), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .Text("Attachment", header: "Attachment", width: Widths.AnsiChars(10))
               .Text("SewingMachineAttachmentID", header: "Part ID", width: Widths.AnsiChars(25))
               .Text("Template", header: "Template", width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("ThreadComboID", header: "Thread" + Environment.NewLine + "Combination", width: Widths.AnsiChars(10));

            int displayTabIndex = this.firstDiaplaySewerManpower - this.minSewermanpower;
            if (displayTabIndex == this.tabCompare.SelectedIndex)
            {
                this.gridAutoMain.DataSource = this.listAutoData[this.tabCompare.SelectedIndex];
                this.gridManualMain.DataSource = this.listManulData[this.tabCompare.SelectedIndex];
                this.gridAutoDetailBs.DataSource = this.listAutoDataForDetail[this.tabCompare.SelectedIndex];
                this.gridManualDetailBs.DataSource = this.listManulDataForDetail[this.tabCompare.SelectedIndex];
                this.UpdateSummaryInfo();
            }
            else
            {
                this.tabCompare.SelectTab(displayTabIndex);
            }
        }

        private void GridSync_Scroll(object sender, ScrollEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;

            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll)
            {
                return;
            }

            bool isScrollDown = e.NewValue > e.OldValue;

            string scrollNo = sourceGrid.Rows[sourceGrid.FirstDisplayedScrollingRowIndex].Cells["No"].Value.ToString();
            string oldScrollNo = sourceGrid.Rows[e.OldValue].Cells["No"].Value.ToString();

            if (isScrollDown && scrollNo == oldScrollNo)
            {
                scrollNo = (MyUtility.Convert.GetInt(scrollNo) + 1).ToString().PadLeft(2, '0');
            }

            this.ScrollLineMapping(scrollNo);
        }

        private void Grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow gridRow = grid.Rows[e.RowIndex];
            DataRow dr = (gridRow.DataBoundItem as DataRowView).Row;

            int operatorLoading = MyUtility.Convert.GetInt(dr["OperatorLoading"]);
            if (operatorLoading > 115)
            {
                gridRow.Cells["OperatorLoading"].Style.BackColor = Color.LightPink;
                gridRow.Cells["OperatorLoading"].Style.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            }
            else if (operatorLoading < 85)
            {
                gridRow.Cells["OperatorLoading"].Style.BackColor = Color.LightSkyBlue;
                gridRow.Cells["OperatorLoading"].Style.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            }
            else
            {
                gridRow.Cells["OperatorLoading"].Style = gridRow.DefaultCellStyle;
            }
        }

        private void UpdateSummaryInfo()
        {
            int displayIndex = this.tabCompare.SelectedIndex;
            this.numAutoLBR.Value = this.listAutoSummaryInfo[displayIndex].LBR;
            this.numAutoOPLoading.Value = this.listAutoSummaryInfo[displayIndex].HighestOPLoading;
            this.numAutoSewerManpower.Value = this.listAutoSummaryInfo[displayIndex].SewerManpower;

            this.numManualLBR.Value = this.listManualSummaryInfo[displayIndex].LBR;
            this.numManualOPLoading.Value = this.listManualSummaryInfo[displayIndex].HighestOPLoading;
            this.numManualSewerManpower.Value = this.listManualSummaryInfo[displayIndex].SewerManpower;
        }

        private void ScrollLineMapping(string scrollToNo)
        {
            DataRow drAutoFirst = this.listAutoData[this.tabCompare.SelectedIndex].AsEnumerable().Where(s => s["No"].ToString() == scrollToNo).First();
            DataRow drManualFirst = this.listManulData[this.tabCompare.SelectedIndex].AsEnumerable().Where(s => s["No"].ToString() == scrollToNo).First();

            if (this.gridAutoMain.GetRowIndexByDataRow(drAutoFirst) == -1)
            {
                return;
            }

            this.gridAutoMain.FirstDisplayedScrollingRowIndex = this.gridAutoMain.GetRowIndexByDataRow(drAutoFirst);
            this.gridManualMain.FirstDisplayedScrollingRowIndex = this.gridManualMain.GetRowIndexByDataRow(drManualFirst);
        }

        private void TabCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabCompare.SelectedTab.Controls.Add(this.panelDisplay);
            this.gridAutoMain.DataSource = this.listAutoData[this.tabCompare.SelectedIndex];
            this.gridManualMain.DataSource = this.listManulData[this.tabCompare.SelectedIndex];
            this.gridAutoDetailBs.DataSource = this.listAutoDataForDetail[this.tabCompare.SelectedIndex];
            this.gridManualDetailBs.DataSource = this.listManulDataForDetail[this.tabCompare.SelectedIndex];
            this.UpdateSummaryInfo();
        }

        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            DataGridView grid = (DataGridView)sender;

            if (e.RowIndex >= 0 &&
                (grid.Columns[e.ColumnIndex].DataPropertyName == "No" ||
                 grid.Columns[e.ColumnIndex].DataPropertyName == "TotalGSDTime" ||
                 grid.Columns[e.ColumnIndex].DataPropertyName == "OperatorLoading"))
            {
                string curNo = grid[0, e.RowIndex].Value.ToString();
                string nextNo = e.RowIndex == grid.Rows.Count - 1 ? string.Empty : grid[0, e.RowIndex + 1].Value.ToString();
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

        private void GenerateCompareDatas(DataTable dtAutomatedLineMapping_Detail, DataTable dtAutomatedLineMapping_DetailTemp, DataTable dtAutomatedLineMapping_DetailAuto)
        {
            var listSewerManpower = dtAutomatedLineMapping_DetailAuto.AsEnumerable().Select(s => MyUtility.Convert.GetInt(s["SewerManpower"])).Distinct();

            foreach (int sewerManpower in listSewerManpower)
            {
                // 產生Auto的資料
                DataTable dtAuto = dtAutomatedLineMapping_DetailAuto.AsEnumerable().Where(s => MyUtility.Convert.GetInt(s["SewerManpower"]) == sewerManpower).CopyToDataTable();
                this.listAutoData.Add(this.GenerateAutoLineMappingData(dtAuto, sewerManpower, ref this.listAutoSummaryInfo));

                // 產生Manual資料
                if (MyUtility.Convert.GetInt(this.drMain["SewerManpower"]) == sewerManpower)
                {
                    this.listManulData.Add(this.GenerateAutoLineMappingData(dtAutomatedLineMapping_Detail, sewerManpower, ref this.listManualSummaryInfo));
                }
                else
                {
                    DataTable dtManual = dtAutomatedLineMapping_DetailTemp.AsEnumerable().Where(s => MyUtility.Convert.GetInt(s["SewerManpower"]) == sewerManpower).CopyToDataTable();
                    this.listManulData.Add(this.GenerateAutoLineMappingData(dtManual, sewerManpower, ref this.listManualSummaryInfo));
                }
            }

            foreach (DataTable dt in this.listManulData)
            {
                this.listManulDataForDetail.Add(dt.AsEnumerable().CopyToDataTable());
            }

            foreach (DataTable dt in this.listAutoData)
            {
                this.listAutoDataForDetail.Add(dt.AsEnumerable().CopyToDataTable());
            }
        }

        private DataTable GenerateAutoLineMappingData(DataTable dtAutomatedLineMapping, int sewermanpower, ref List<SummaryInfo> listSummaryInfo)
        {
            if (dtAutomatedLineMapping.Rows.Count == 0)
            {
                return new DataTable();
            }

            SummaryInfo summaryInfo = new SummaryInfo();
            summaryInfo.SewerManpower = sewermanpower;

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(new DataColumn("No", typeof(string)));
            dtResult.Columns.Add(new DataColumn("TotalGSDTime", typeof(decimal)));
            dtResult.Columns.Add(new DataColumn("OperatorLoading", typeof(decimal)));
            dtResult.Columns.Add(new DataColumn("Location", typeof(string)));
            dtResult.Columns.Add(new DataColumn("PPADesc", typeof(string)));
            dtResult.Columns.Add(new DataColumn("MachineTypeID", typeof(string)));
            dtResult.Columns.Add(new DataColumn("MasterPlusGroup", typeof(string)));
            dtResult.Columns.Add(new DataColumn("OperationDesc", typeof(string)));
            dtResult.Columns.Add(new DataColumn("Annotation", typeof(string)));
            dtResult.Columns.Add(new DataColumn("Attachment", typeof(string)));
            dtResult.Columns.Add(new DataColumn("SewingMachineAttachmentID", typeof(string)));
            dtResult.Columns.Add(new DataColumn("Template", typeof(string)));
            dtResult.Columns.Add(new DataColumn("GSD", typeof(decimal)));
            dtResult.Columns.Add(new DataColumn("SewerDiffPercentageDesc", typeof(string)));
            dtResult.Columns.Add(new DataColumn("DivSewer", typeof(decimal)));
            dtResult.Columns.Add(new DataColumn("OriSewer", typeof(decimal)));
            dtResult.Columns.Add(new DataColumn("ThreadComboID", typeof(string)));
            dtResult.Columns.Add(new DataColumn("TimeStudyDetailUkeyCnt", typeof(int)));

            decimal totalGSD = dtAutomatedLineMapping.AsEnumerable()
                                .Where(s => s["PPA"].ToString() != "C" &&
                                            !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) &&
                                            s["OperationID"].ToString() != "PROCIPF00003" &&
                                            s["OperationID"].ToString() != "PROCIPF00004")
                                .Sum(s => MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"]), 2));

            decimal avgGSD = sewermanpower == 0 ? 0 : MyUtility.Math.Round(totalGSD / sewermanpower, 2);

            var resultRows = dtAutomatedLineMapping.AsEnumerable()
                            .Where(s => s["PPA"].ToString() != "C" &&
                                        !MyUtility.Convert.GetBool(s["IsNonSewingLine"]))
                            .GroupBy(s => new
                            {
                                No = s["No"].ToString(),
                            })
                            .Select(groupItem =>
                                new
                                {
                                    groupItem.Key.No,
                                    TotalGSDTime = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"])), 2),
                                    OperatorLoading = MyUtility.Check.Empty(avgGSD) ? 0 : MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"])) / avgGSD * 100, 0),
                                    ExcludeCalSummary = groupItem.Any(s => s["OperationID"].ToString() == "PROCIPF00003" || s["OperationID"].ToString() == "PROCIPF00004"),
                                })
                            .ToList();

            foreach (DataRow dr in dtAutomatedLineMapping.Rows)
            {
                if (MyUtility.Check.Empty(dr["No"]))
                {
                    continue;
                }

                DataRow newRowResult = dtResult.NewRow();

                var summaryResult = resultRows.Where(s => s.No == dr["No"].ToString()).First();
                newRowResult["No"] = summaryResult.No;
                newRowResult["TotalGSDTime"] = summaryResult.TotalGSDTime;
                newRowResult["OperatorLoading"] = summaryResult.OperatorLoading;
                newRowResult["Location"] = dr["Location"];
                newRowResult["PPADesc"] = dr["PPADesc"];
                newRowResult["MachineTypeID"] = dr["MachineTypeID"];
                newRowResult["MasterPlusGroup"] = dr["MasterPlusGroup"];
                newRowResult["OperationDesc"] = dr["OperationDesc"];
                newRowResult["Annotation"] = dr["Annotation"];
                newRowResult["Attachment"] = dr["Attachment"];
                newRowResult["SewingMachineAttachmentID"] = dr["SewingMachineAttachmentID"];
                newRowResult["Template"] = dr["Template"];
                newRowResult["GSD"] = dr["GSD"];
                newRowResult["SewerDiffPercentageDesc"] = dr["SewerDiffPercentageDesc"];
                newRowResult["DivSewer"] = dr["DivSewer"];
                newRowResult["OriSewer"] = dr["OriSewer"];
                newRowResult["ThreadComboID"] = dr["ThreadComboID"];
                newRowResult["TimeStudyDetailUkeyCnt"] = dr["TimeStudyDetailUkeyCnt"];

                dtResult.Rows.Add(newRowResult);
            }

            decimal highestGSD = resultRows.Where(s => !s.ExcludeCalSummary).Max(s => s.TotalGSDTime);
            summaryInfo.LBR = MyUtility.Convert.GetInt(MyUtility.Math.Round((totalGSD / highestGSD) / sewermanpower * 100, 0));
            summaryInfo.HighestOPLoading = MyUtility.Convert.GetInt(MyUtility.Math.Round(resultRows.Max(s => s.OperatorLoading), 0));
            listSummaryInfo.Add(summaryInfo);

            return dtResult;
        }
    }
}
