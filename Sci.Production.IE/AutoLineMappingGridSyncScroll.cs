using Ict;
using Sci.Production.Class.Command;
using Sci.Win.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// AutoLineMappingGridSyncScroll
    /// </summary>
    public class AutoLineMappingGridSyncScroll
    {
        /// <summary>
        /// SubGridType
        /// </summary>
        public enum SubGridType
        {
            /// <summary>
            /// LineMapping
            /// </summary>
            LineMapping,

            /// <summary>
            /// CentrailizedPPA
            /// </summary>
            CentrailizedPPA,

            /// <summary>
            /// LineMappingBalancing
            /// </summary>
            LineMappingBalancing,

            /// <summary>
            /// BalancingCentrailizedPPA
            /// </summary>
            BalancingCentrailizedPPA,
        }

        private Grid gridMain;
        private Grid gridSub;
        private string syncColName;
        private int _sewerManpower;
        private DataTable dtGridDetailRightSummary = new DataTable();
        private SubGridType subGridType;

        /// <summary>
        /// SewerManpower
        /// </summary>
        public int SewerManpower
        {
            get
            {
                this._sewerManpower = MyUtility.Convert.GetInt(MyUtility.Math.Round(
                        this.MainData
                            .Where(s => s["PPA"].ToString() != "C" &&
                                !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) &&
                                s["OperationID"].ToString() != "PROCIPF00004" &&
                                s["OperationID"].ToString() != "PROCIPF00003")
                            .Sum(s => MyUtility.Convert.GetDecimal(s["DivSewer"])),
                        0));

                return this._sewerManpower;
            }
        }

        private IEnumerable<DataRow> MainData
        {
            get
            {
                DataTable dtResult = this.gridMain.DataSource.GetType() == typeof(ListControlBindingSource) ? (DataTable)((ListControlBindingSource)this.gridMain.DataSource).DataSource : (DataTable)this.gridMain.DataSource;

                if (dtResult == null)
                {
                    return null;
                }

                return dtResult.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            }
        }

        public DataTable SubData
        {
            get
            {
                return this.gridSub.DataSource.GetType() == typeof(ListControlBindingSource) ? (DataTable)((ListControlBindingSource)this.gridSub.DataSource).DataSource : (DataTable)this.gridSub.DataSource;
            }
        }

        /// <summary>
        /// TotalGSD
        /// </summary>
        public decimal TotalGSD
        {
            get
            {
                return this.MainData
                    .Where(s => s["PPA"].ToString() != "C" &&
                                !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) &&
                                s["OperationID"].ToString() != "PROCIPF00004" &&
                                s["OperationID"].ToString() != "PROCIPF00003")
                    .Sum(s => MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"]), 2));
            }
        }

        /// <summary>
        /// TotalCycle
        /// </summary>
        public decimal TotalCycle
        {
            get
            {
                return this.MainData
                    .Where(s => s["PPA"].ToString() != "C" &&
                                !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) &&
                                s["OperationID"].ToString() != "PROCIPF00004" &&
                                s["OperationID"].ToString() != "PROCIPF00003")
                    .Sum(s => MyUtility.Math.Round(MyUtility.Convert.GetDecimal(s["Cycle"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"]), 2));
            }
        }

        /// <summary>
        /// TotalGSD
        /// </summary>
        public decimal HighestGSD
        {
            get
            {
                if (this.SubData.Rows.Count == 0)
                {
                    return 0;
                }

                return this.SubData.AsEnumerable().Where(s => !MyUtility.Convert.GetBool(s["NeedExclude"]))
                    .Select(s => MyUtility.Convert.GetDecimal(s["TotalGSDTime"])).Max();
            }
        }

        /// <summary>
        /// TotalGSD
        /// </summary>
        public decimal HighestCycle
        {
            get
            {
                if (this.SubData.Rows.Count == 0)
                {
                    return 0;
                }

                return this.SubData.AsEnumerable().Where(s => !MyUtility.Convert.GetBool(s["NeedExclude"]))
                    .Select(s => MyUtility.Convert.GetDecimal(s["TotalCycleTime"])).Max();
            }
        }

        /// <summary>
        /// AvgGSDTime
        /// </summary>
        public decimal AvgGSDTime
        {
            get
            {
                if (MyUtility.Check.Empty(this.SewerManpower))
                {
                    return 0;
                }

                return MyUtility.Math.Round(this.TotalGSD / this.SewerManpower, 2);
            }
        }

        /// <summary>
        /// AvgCycleTime
        /// </summary>
        public decimal AvgCycleTime
        {
            get
            {
                if (MyUtility.Check.Empty(this.SewerManpower))
                {
                    return 0;
                }

                return MyUtility.Math.Round(this.TotalCycle / this.SewerManpower, 2);
            }
        }

        /// <summary>
        /// HighestLoading
        /// </summary>
        public decimal HighestLoading
        {
            get
            {
                if (this.SubData.Rows.Count == 0)
                {
                    return 0;
                }

                return this.SubData.AsEnumerable().Select(s => MyUtility.Convert.GetDecimal(s["OperatorLoading"])).Max();
            }
        }

        /// <summary>
        /// LBR
        /// </summary>
        public int LBR
        {
            get
            {
                if (this.SubData.Rows.Count == 0)
                {
                    return 0;
                }

                return MyUtility.Convert.GetInt(MyUtility.Math.Round((this.TotalGSD / this.HighestGSD) / this.SewerManpower * 100, 0));
            }
        }

        /// <summary>
        /// IE AutoLineMapping grid同步滑動
        /// </summary>
        /// <param name="gridMain">gridMain</param>
        /// <param name="gridSub">gridSub</param>
        /// <param name="syncColName">syncColName</param>
        /// <param name="subGridType">subGridType</param>
        public AutoLineMappingGridSyncScroll(Grid gridMain, Grid gridSub, string syncColName, SubGridType subGridType)
        {
            this.gridMain = gridMain;
            this.gridSub = gridSub;
            this.syncColName = syncColName;
            this.subGridType = subGridType;

            this.gridMain.CellPainting += this.GridMain_CellPainting;
            this.gridMain.Scroll += this.GridMain_Scroll;

            this.gridSub.Scroll += this.GridSub_Scroll;
            this.gridSub.RowPrePaint += this.GridSub_RowPrePaint;
            if (subGridType == SubGridType.CentrailizedPPA)
            {
                this.gridMain.SortCompare += new DataGridViewSortCompareEventHandler(this.GridPPA_SortCompare);
            }

            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("No", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("NoCnt", typeof(int)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("sumGSDTime", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("sumCycleTime", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("TotalGSDTime", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("TotalCycleTime", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("OperatorLoading", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("NeedExclude", typeof(bool)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("EmployeeID", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("EmployeeName", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("EmployeeSkill", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("Effi", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("OperatorEffi", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("EstTotalCycleTime", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("EstOutputHr", typeof(decimal)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("IsNotShownInP05", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("IsNotShownInP06", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("IsNotShownInP06Cnt", typeof(string)));
            this.dtGridDetailRightSummary.Columns.Add(new DataColumn("IsResignationDate", typeof(string)));
            this.dtGridDetailRightSummary.PrimaryKey = new DataColumn[] { this.dtGridDetailRightSummary.Columns["No"] };
            this.gridSub.DataSource = this.dtGridDetailRightSummary;
        }

        private void GridPPA_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == "No")
            {
                string value1 = e.CellValue1.ToString();
                string value2 = e.CellValue2.ToString();

                // 將空白值移至最後
                if (string.IsNullOrEmpty(value1))
                {
                    e.SortResult = 1;
                }
                else if (string.IsNullOrEmpty(value2))
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = string.Compare(value1, value2);
                }

                e.Handled = true;
            }
        }

        private void GridSub_Scroll(object sender, ScrollEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;

            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll)
            {
                return;
            }

            string scrollNo = sourceGrid.Rows[sourceGrid.FirstDisplayedScrollingRowIndex].Cells[this.syncColName].Value.ToString();
            this.ScrollLineMapping(scrollNo);
        }

        private void GridMain_Scroll(object sender, ScrollEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;

            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll)
            {
                return;
            }

            bool isScrollDown = e.NewValue > e.OldValue;

            string scrollNo = sourceGrid.Rows[sourceGrid.FirstDisplayedScrollingRowIndex].Cells[this.syncColName].Value.ToString();
            string oldScrollNo = sourceGrid.Rows[e.OldValue].Cells[this.syncColName].Value.ToString();

            if (isScrollDown && scrollNo == oldScrollNo)
            {
                scrollNo = (MyUtility.Convert.GetInt(scrollNo) + 1).ToString().PadLeft(2, '0');
            }

            this.ScrollLineMapping(scrollNo);
        }

        private void ScrollRowByNo(Grid sourceGrid, string no)
        {
            foreach (DataGridViewRow gridRow in sourceGrid.Rows)
            {
                if (gridRow.Cells["No"].Value.ToString() == no)
                {
                    sourceGrid.FirstDisplayedScrollingRowIndex = gridRow.Index;
                    break;
                }
            }
        }

        /// <summary>
        /// ScrollLineMapping
        /// </summary>
        /// <param name="scrollToNo">scrollToNo</param>
        public void ScrollLineMapping(string scrollToNo)
        {
            DataTable dtSub = this.SubData;

            if (dtSub.Rows.Count == 0)
            {
                return;
            }

            this.ScrollRowByNo(this.gridMain, scrollToNo);
            this.ScrollRowByNo(this.gridSub, scrollToNo);
        }

        private void GridMain_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            DataGridView grid = (DataGridView)sender;

            if (e.RowIndex >= 0 && (grid.Columns[e.ColumnIndex].DataPropertyName == "No" || grid.Columns[e.ColumnIndex].DataPropertyName == "Selected"))
            {
                string curNo = grid[0, e.RowIndex].Value.ToString();
                string nextNo = e.RowIndex == grid.Rows.Count - 1 ? string.Empty : grid[0, e.RowIndex + 1].Value.ToString();
                string preNo = e.RowIndex == 0 ? null : grid[0, e.RowIndex - 1].Value.ToString();

                if (MyUtility.Check.Empty(curNo))
                {
                    return;
                }

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

                if (grid.Columns[e.ColumnIndex].DataPropertyName == "Selected" &&
                    grid.Rows[e.RowIndex].Cells["Selected"].ReadOnly &&
                    MyUtility.Convert.GetBool(grid.Rows[e.RowIndex].Cells["Selected"].Value) == false)
                {
                    using (SolidBrush brush = new SolidBrush(Color.LightGray))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }

                    e.Handled = true;
                }
            }
        }

        private void GridSub_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow gridRow = grid.Rows[e.RowIndex];
            DataRow dr = (gridRow.DataBoundItem as DataRowView).Row;

            if (MyUtility.Convert.GetInt(dr["NoCnt"]) > 1 && gridRow.Height <= 24)
            {
                gridRow.Height = gridRow.Height * MyUtility.Convert.GetInt(dr["NoCnt"]);
            }

            int operatorLoading = MyUtility.Convert.GetInt(dr["OperatorLoading"]);
            if (operatorLoading > 115)
            {
                gridRow.Cells["OperatorLoading"].Style.BackColor = Color.LightPink;
                gridRow.Cells["OperatorLoading"].Style.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            }
            else if (operatorLoading > 0 && operatorLoading < 85)
            {
                gridRow.Cells["OperatorLoading"].Style.BackColor = Color.LightSkyBlue;
                gridRow.Cells["OperatorLoading"].Style.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            }
            else
            {
                gridRow.Cells["OperatorLoading"].Style = gridRow.DefaultCellStyle;
            }
        }

        /// <summary>
        /// RefreshSubData
        /// </summary>
        /// <param name="doReload">doReload</param>
        public void RefreshSubData(bool doReload = true, bool isSort = true)
        {
            DataTable dtSub = this.SubData;

            if (doReload)
            {
                dtSub.Clear();
            }

            IEnumerable<DataRow> mainData = this.MainData;

            if (mainData == null)
            {
                return;
            }

            List<DataRow> resultRows = new List<DataRow>();

            switch (this.subGridType)
            {
                case SubGridType.LineMapping:
                    this.gridMain.Sort(this.gridMain.Columns["No"], System.ComponentModel.ListSortDirection.Ascending);
                    this.gridMain.Columns.DisableSortable();
                    this.gridSub.Sort(this.gridSub.Columns["No"], System.ComponentModel.ListSortDirection.Ascending);
                    this.gridSub.Columns.DisableSortable();
                    decimal avgGSD = this.AvgGSDTime;
                    resultRows = mainData.Where(s => s["PPA"].ToString() != "C" &&
                                                          !MyUtility.Convert.GetBool(s["IsNonSewingLine"]))
                                    .GroupBy(s => new
                                    {
                                        No = s["No"].ToString(),
                                    })
                                    .Select(groupItem =>
                                    {
                                        DataRow newRow = dtSub.NewRow();
                                        newRow["No"] = groupItem.Key.No;
                                        newRow["NoCnt"] = groupItem.Count();
                                        // newRow["sumCycleTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["Cycle"])), 2);
                                        newRow["sumGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])), 2);
                                        newRow["TotalGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"])), 2);
                                        newRow["NeedExclude"] = groupItem.Any(s => s["OperationID"].ToString() == "PROCIPF00004" ||
                                                                                   s["OperationID"].ToString() == "PROCIPF00003");
                                        newRow["OperatorLoading"] = MyUtility.Check.Empty(avgGSD) || (bool)newRow["NeedExclude"] ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(newRow["TotalGSDTime"]) / avgGSD * 100, 0);
                                        newRow["IsNotShownInP05"] = groupItem.Select(s => s["IsNotShownInP05"].ToString()).First();
                                        return newRow;
                                    }).ToList();
                    break;
                case SubGridType.LineMappingBalancing:
                    this.gridMain.BeginInvoke(new Action(() =>
                    {
                        this.gridMain.Sort(this.gridMain.Columns["No"], System.ComponentModel.ListSortDirection.Ascending);
                    }));
                    this.gridMain.Columns.DisableSortable();
                    this.gridSub.BeginInvoke(new Action(() =>
                    {
                        this.gridSub.Sort(this.gridSub.Columns["No"], System.ComponentModel.ListSortDirection.Ascending);
                    }));
                    this.gridSub.Columns.DisableSortable();
                    decimal avgCycle = this.AvgCycleTime;
                    resultRows = mainData.Where(s => s["PPA"].ToString() != "C" &&
                                                          !MyUtility.Convert.GetBool(s["IsNonSewingLine"]))
                                    .GroupBy(s => new
                                    {
                                        No = s["No"].ToString(),
                                    })
                                    .Select(groupItem =>
                                    {
                                        DataRow newRow = dtSub.NewRow();

                                        decimal decEffi = groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["Effi"])) > 0 ? groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["Effi"])): 0;

                                        newRow["No"] = groupItem.Key.No;
                                        newRow["NoCnt"] = groupItem.Count();
                                        newRow["sumGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])), 2);
                                        newRow["sumCycleTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["Cycle"])), 2);
                                        newRow["TotalGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"]) * MyUtility.Convert.GetDecimal(s["SewerDiffPercentageDesc"])), 2);
                                        newRow["TotalCycleTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["TotalCycleTime"])), 2);
                                        newRow["NeedExclude"] = groupItem.Any(s => s["OperationID"].ToString() == "PROCIPF00004" ||
                                                                                   s["OperationID"].ToString() == "PROCIPF00003");
                                        newRow["OperatorLoading"] = MyUtility.Check.Empty(avgCycle) || (bool)newRow["NeedExclude"] ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(newRow["TotalCycleTime"]) / avgCycle * 100, 0);
                                        newRow["EmployeeID"] = groupItem.Select(s => s["EmployeeID"].ToString()).First();
                                        newRow["EmployeeName"] = groupItem.Select(s => s["EmployeeName"].ToString()).First();
                                        newRow["EmployeeSkill"] = groupItem.Select(s => s["EmployeeSkill"].ToString()).First();
                                        newRow["OperatorEffi"] = groupItem.Select(s => MyUtility.Convert.GetDecimal(s["OperatorEffi"])).First();
                                        newRow["Effi"] = groupItem.Select(s => MyUtility.Convert.GetDecimal(s["Effi"])).First();
                                        newRow["EstOutputHr"] = groupItem.Select(s => MyUtility.Convert.GetDecimal(s["EstOutputHr"])).First();
                                        newRow["EstTotalCycleTime"] = groupItem.Select(s => s["EstTotalCycleTime"].ToString()).First();
                                        newRow["IsNotShownInP06"] = groupItem.Select(s => s["IsNotShownInP06"].ToString()).First();
                                        newRow["IsNotShownInP06Cnt"] = groupItem.Where(x => x["IsNotShownInP06"].ToString() == "True").Select(s => s["IsNotShownInP06"].ToString()).Count();
                                        newRow["IsResignationDate"] = groupItem.Select(s => s["IsResignationDate"].ToString()).First();

                                        return newRow;
                                    }).ToList();
                    break;

                case SubGridType.CentrailizedPPA:
                    this.gridMain.Columns.DisableSortable();
                    this.gridSub.Columns.DisableSortable();

                    var dataPPA = mainData.Where(s => s["PPA"].ToString() == "C" && !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) && !MyUtility.Check.Empty(s["No"]));
                    if (!dataPPA.Any())
                    {
                        return;
                    }

                    decimal avgGSDTimePPA = dataPPA.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])) / dataPPA.Select(s => s["No"].ToString()).Distinct().Count();

                    resultRows = dataPPA.GroupBy(s => new
                    {
                        No = s["No"].ToString(),
                    }).Select(groupItem =>
                    {
                        DataRow newRow = dtSub.NewRow();
                        newRow["No"] = groupItem.Key.No;
                        newRow["NoCnt"] = groupItem.Count();
                        newRow["TotalGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])), 2);
                        newRow["OperatorLoading"] = avgGSDTimePPA == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(newRow["TotalGSDTime"]) / avgGSDTimePPA * 100, 0);
                        newRow["NeedExclude"] = true;
                        return newRow;
                    }).ToList();
                    break;
                case SubGridType.BalancingCentrailizedPPA:
                    this.gridMain.Columns.DisableSortable();
                    this.gridSub.Columns.DisableSortable();

                    var dataBalancingPPA = mainData.Where(s => s["PPA"].ToString() == "C" && !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) && !MyUtility.Check.Empty(s["No"]));
                    if (!dataBalancingPPA.Any())
                    {
                        return;
                    }

                    decimal avgCycleTimePPA = dataBalancingPPA.Sum(s => MyUtility.Convert.GetDecimal(s["Cycle"])) / dataBalancingPPA.Select(s => s["No"].ToString()).Distinct().Count();

                    resultRows = dataBalancingPPA.GroupBy(s => new
                    {
                        No = s["No"].ToString(),
                    }).Select(groupItem =>
                    {
                        DataRow newRow = dtSub.NewRow();
                        newRow["No"] = groupItem.Key.No;
                        newRow["NoCnt"] = groupItem.Count();
                        newRow["TotalGSDTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["GSD"])), 2);
                        newRow["TotalCycleTime"] = MyUtility.Math.Round(groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["Cycle"])), 2);
                        newRow["OperatorLoading"] = avgCycleTimePPA == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(newRow["TotalCycleTime"]) / avgCycleTimePPA * 100, 0);
                        newRow["NeedExclude"] = true;
                        newRow["EmployeeID"] = groupItem.Select(s => s["EmployeeID"].ToString()).First();
                        newRow["EmployeeName"] = groupItem.Select(s => s["EmployeeName"].ToString()).First();
                        newRow["EmployeeSkill"] = groupItem.Select(s => s["EmployeeSkill"].ToString()).First();
                        newRow["OperatorEffi"] = MyUtility.Convert.GetDecimal(newRow["TotalCycleTime"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(newRow["TotalGSDTime"]) / MyUtility.Convert.GetDecimal(newRow["TotalCycleTime"]) * 100, 2);
                        return newRow;
                    }).ToList();
                    break;
                default:
                    break;
            }

            // 設定主鍵
            dtSub.PrimaryKey = new DataColumn[] { dtSub.Columns["No"] };

            foreach (DataRow dr in resultRows)
            {
                DataRow drUpdTarget = dtSub.Rows.Find(dr["No"]);
                if (drUpdTarget != null)
                {
                    drUpdTarget.ItemArray = dr.ItemArray;
                }
                else
                {
                    dtSub.Rows.Add(dr);
                }
            }
        }
    }
}
