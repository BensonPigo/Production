using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_StandardQtyPlannedCuttingWIP : Win.Tems.QueryForm
    {
        private string ID;
        private string finalmsg;
        private DataTable DistqtyTb;
        private DataTable detailData;
        private DataTable summaryData;
        private DataTable dtG;
        private DataTable dtF;
        private DateTime MinInLine;
        private DateTime MaxOffLine;
        private List<string> FtyFroup = new List<string>();
        private List<InOffLineList> AllDataTmp = new List<InOffLineList>();
        private List<InOffLineList> AllData = new List<InOffLineList>();
        private List<InOffLineList_byFabricPanelCode> AllDataTmp2 = new List<InOffLineList_byFabricPanelCode>();
        private List<InOffLineList_byFabricPanelCode> AllData2 = new List<InOffLineList_byFabricPanelCode>();
        private List<PublicPrg.Prgs.Day> Days = new List<PublicPrg.Prgs.Day>();
        private List<PublicPrg.Prgs.Day> Days2 = new List<PublicPrg.Prgs.Day>();
        private List<LeadTime> LeadTimeList = new List<LeadTime>();

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_StandardQtyPlannedCuttingWIP"/> class.
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="distqtyTb">Distqty Tb</param>
        public P02_StandardQtyPlannedCuttingWIP(string id, DataTable distqtyTb)
        {
            this.InitializeComponent();
            this.ID = id;
            this.DistqtyTb = distqtyTb;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.bgWorkerUpdateInfo.RunWorkerAsync();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Query1()
        {
            List<string> orderIDs = this.DistqtyTb.AsEnumerable()
                .Select(s => new { ID = MyUtility.Convert.GetString(s["OrderID"]) })
                .Where(w => w.ID != "EXCESS")
                .Distinct()
                .Select(s => s.ID)
                .ToList();

            if (!this.Check_Subprocess_LeadTime(orderIDs))
            {
                return false;
            }

            #region 起手資料
            string cmd = $@"SELECT s.* FROM SewingSchedule s INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID WHERE s.OrderID in ('{string.Join("','", orderIDs)}')";
            DualResult result = DBProxy.Current.Select(null, cmd, out DataTable dt_Schedule);
            if (!result)
            {
                this.finalmsg = result.ToString();
                return false;
            }

            if (dt_Schedule.Rows.Count == 0)
            {
                this.finalmsg = "Data not Found";
                return false;
            }
            #endregion

            #region 時間軸
            this.Days.Clear();

            // 取出最早InLine / 最晚OffLine
            this.MinInLine = dt_Schedule.AsEnumerable().Min(o => Convert.ToDateTime(o["Inline"]));
            this.MaxOffLine = dt_Schedule.AsEnumerable().Max(o => Convert.ToDateTime(o["offline"]));

            int maxLeadTime = this.LeadTimeList.Max(o => o.LeadTimeDay);
            int minLeadTime = this.LeadTimeList.Min(o => o.LeadTimeDay);

            // 起點 = (最早Inline - 最大Lead Time)、終點 = (最晚Offline - 最小Lead Time)
            DateTime start_where = this.MinInLine;
            DateTime end_where = this.MaxOffLine;

            List<PublicPrg.Prgs.Day> daylist1 = GetDays(maxLeadTime, start_where.Date, this.FtyFroup);
            List<PublicPrg.Prgs.Day> daylist2 = GetDays(minLeadTime, end_where.Date, this.FtyFroup);
            foreach (var item in daylist1)
            {
                this.Days.Add(item);
            }

            foreach (var item in daylist2)
            {
                this.Days.Add(item);
            }

            DateTime d2 = daylist2.Select(s => s.Date).Min(m => m.Date);

            // 若 daylist1 是1/1~1/3, daylist2 是1/10~1/12, 中間也要補上
            if (start_where < d2)
            {
                List<PublicPrg.Prgs.Day> daylist3 = GetRangeHoliday(start_where, d2, this.FtyFroup);
                foreach (var item in daylist3)
                {
                    this.Days.Add(item);
                }
            }

            this.Days = this.Days
                .Select(s => new { s.Date.Date, s.IsHoliday }).Distinct()
                .Select(s => new PublicPrg.Prgs.Day { Date = s.Date.Date, IsHoliday = s.IsHoliday })
                .OrderBy(o => o.Date).ToList();
            #endregion

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(5);

            this.AllData = GetInOffLineList(dt_Schedule, this.Days, bw: this.bgWorkerUpdateInfo);

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(90);

            List<DataTable> leadTimeList = GetCutting_WIP_DataTable(this.Days, this.AllData.OrderBy(o => o.OrderID).ToList());

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(95);

            this.summaryData = leadTimeList[0];
            this.detailData = leadTimeList[1];

            this.dtG = this.summaryData.Clone();
            foreach (DataRow item in this.summaryData.Rows)
            {
                DataRow drdStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Desc./Sewing Date] = 'Std. Qty'")[0];
                this.dtG.ImportRow(drdStdQty);
                DataRow drdAccStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Desc./Sewing Date] = 'Accu. Std. Qty'")[0];
                DataRow drdAccCutPlan = this.detailData.Select($"SP='{item["SP"]}' and [Desc./Sewing Date] = 'Accu. Cut Plan Qty'")[0];

                // 2 是日期欄位開始
                for (int i = 2; i < this.detailData.Columns.Count; i++)
                {
                    string bal = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(drdAccCutPlan[i]) - MyUtility.Convert.GetDecimal(drdAccStdQty[i]), 0));
                    if (MyUtility.Convert.GetString(drdAccCutPlan[i]) == string.Empty && MyUtility.Convert.GetString(drdAccStdQty[i]) == string.Empty)
                    {
                        bal = string.Empty;
                    }

                    string wip = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(item[i - 1]), 2));
                    if (MyUtility.Convert.GetString(item[i - 1]) == string.Empty)
                    {
                        wip = string.Empty;
                    }

                    string srow2 = string.Empty;
                    if (!(MyUtility.Convert.GetString(bal) == string.Empty && MyUtility.Convert.GetString(wip) == string.Empty))
                    {
                        srow2 = wip + " / " + bal;
                    }

                    drdAccCutPlan[i] = srow2;
                }

                drdAccCutPlan["SP"] = string.Empty;
                this.dtG.ImportRow(drdAccCutPlan);
            }

            return true;
        }

        private bool Query2()
        {
            List<string> orderIDs = this.DistqtyTb.AsEnumerable()
                .Select(s => new { ID = MyUtility.Convert.GetString(s["OrderID"]) })
                .Where(w => w.ID != "EXCESS")
                .Distinct()
                .Select(s => s.ID)
                .ToList();

            if (!this.Check_Subprocess_LeadTime(orderIDs))
            {
                return false;
            }

            #region 起手資料
            string cmd = $@"SELECT s.* FROM SewingSchedule s INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID WHERE s.OrderID in ('{string.Join("','", orderIDs)}')";
            DualResult result = DBProxy.Current.Select(null, cmd, out DataTable dt_Schedule);
            if (!result)
            {
                this.finalmsg = result.ToString();
                return false;
            }

            if (dt_Schedule.Rows.Count == 0)
            {
                return false;
            }
            #endregion

            #region 時間軸
            this.Days2.Clear();

            // 取出最早InLine / 最晚OffLine
            this.MinInLine = dt_Schedule.AsEnumerable().Min(o => Convert.ToDateTime(o["Inline"]));
            this.MaxOffLine = dt_Schedule.AsEnumerable().Max(o => Convert.ToDateTime(o["offline"]));

            int maxLeadTime = this.LeadTimeList.Max(o => o.LeadTimeDay);
            int minLeadTime = this.LeadTimeList.Min(o => o.LeadTimeDay);

            // 起點 = (最早Inline - 最大Lead Time)、終點 = (最晚Offline - 最小Lead Time)
            DateTime start_where = this.MinInLine;
            DateTime end_where = this.MaxOffLine;

            List<PublicPrg.Prgs.Day> daylist1 = GetDays(maxLeadTime, start_where, this.FtyFroup);
            List<PublicPrg.Prgs.Day> daylist2 = GetDays(minLeadTime, end_where, this.FtyFroup);
            foreach (var item in daylist1)
            {
                this.Days2.Add(item);
            }

            foreach (var item in daylist2)
            {
                this.Days2.Add(item);
            }

            DateTime d2 = daylist2.Select(s => s.Date).Min(m => m.Date);

            // 若 daylist1 是1/1~1/3, daylist2 是1/10~1/12, 中間也要補上
            if (start_where < d2)
            {
                List<PublicPrg.Prgs.Day> daylist3 = GetRangeHoliday(start_where, d2, this.FtyFroup);
                foreach (var item in daylist3)
                {
                    this.Days2.Add(item);
                }
            }

            this.Days2 = this.Days2
                .Select(s => new { s.Date, s.IsHoliday }).Distinct() // start和end加入日期有重複
                .Select(s => new PublicPrg.Prgs.Day { Date = s.Date, IsHoliday = s.IsHoliday })
                .OrderBy(o => o.Date).ToList();
            #endregion

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(5);

            this.AllData2 = GetInOffLineList_byFabricPanelCode(dt_Schedule, this.Days2, bw: this.bgWorkerUpdateInfo);

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(90);

            List<DataTable> leadTimeList = GetCutting_WIP_DataTable(this.Days2, this.AllData2.OrderBy(o => o.OrderID).ToList());

            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                return false;
            }

            this.bgWorkerUpdateInfo.ReportProgress(95);

            this.summaryData = leadTimeList[0];
            this.detailData = leadTimeList[1];

            this.dtF = this.summaryData.Clone();
            foreach (DataRow item in this.summaryData.Rows)
            {
                DataRow drdStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Fab. Panel Code] = '{item["Fab. Panel Code"]}' and [Desc./Sewing Date] = 'Std. Qty'")[0];
                this.dtF.ImportRow(drdStdQty);
                DataRow drdAccStdQty = this.detailData.Select($"SP='{item["SP"]}' and [Fab. Panel Code] = '{item["Fab. Panel Code"]}' and [Desc./Sewing Date] = 'Accu. Std. Qty'")[0];
                DataRow drdAccCutPlan = this.detailData.Select($"SP='{item["SP"]}' and [Fab. Panel Code] = '{item["Fab. Panel Code"]}' and [Desc./Sewing Date] = 'Accu. Cut Plan Qty'")[0];

                // 2 是日期欄位開始
                for (int i = 3; i < this.detailData.Columns.Count; i++)
                {
                    string bal = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(drdAccCutPlan[i]) - MyUtility.Convert.GetDecimal(drdAccStdQty[i]), 0));
                    if (MyUtility.Convert.GetString(drdAccCutPlan[i]) == string.Empty && MyUtility.Convert.GetString(drdAccStdQty[i]) == string.Empty)
                    {
                        bal = string.Empty;
                    }

                    string wip = MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(item[i - 1]), 2));
                    if (MyUtility.Convert.GetString(item[i - 1]) == string.Empty)
                    {
                        wip = string.Empty;
                    }

                    string srow2 = string.Empty;
                    if (!(MyUtility.Convert.GetString(bal) == string.Empty && MyUtility.Convert.GetString(wip) == string.Empty))
                    {
                        srow2 = wip + " / " + bal;
                    }

                    drdAccCutPlan[i] = srow2;
                }

                drdAccCutPlan["SP"] = string.Empty;
                this.dtF.ImportRow(drdAccCutPlan);
            }

            return true;
        }

        private void AfterQuery1()
        {
            this.listControlBindingSource1.DataSource = this.dtG;
            foreach (DataColumn item in this.dtG.Columns)
            {
                this.Helper.Controls.Grid.Generator(this.gridGarment)
                .Text(item.ColumnName, header: item.ColumnName, width: Widths.Auto(), iseditingreadonly: true)
                ;
            }

            this.gridGarment.AutoResizeColumns();

            int columnIndex = 1;
            foreach (var day in this.Days)
            {
                // string dateStr = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                // gridGarment.Columns[ColumnIndex].Name = dateStr;
                // 假日的話粉紅色
                if (day.IsHoliday)
                {
                    this.gridGarment.Columns[columnIndex].HeaderCell.Style.BackColor = Color.FromArgb(255, 199, 206);
                }

                columnIndex++;
            }

            // 扣除無產出的日期
            List<PublicPrg.Prgs.Day> removeDays = new List<PublicPrg.Prgs.Day>();

            foreach (var day in this.Days)
            {
                // 如果該日期，不是「有資料」，則刪掉
                if (!this.AllData.Where(x => x.InOffLines.Where(y => y.DateWithLeadTime == day.Date
                                                                && (MyUtility.Convert.GetInt(y.CutQty) > 0 || MyUtility.Convert.GetInt(y.StdQty) > 0)) // 不同於R01,是因此只有顯示CutQty,StdQty資料
                                                               .Any())
                                       .Any())
                {
                    removeDays.Add(day);
                }
                else if (!this.AllData.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                    && MyUtility.Convert.GetDecimal(y.CutQty) > 0)
                                                                .Any())
                                       .Any()
                    && day.IsHoliday)
                {
                    removeDays.Add(day);
                }
            }

            columnIndex = 1;
            foreach (var day in this.Days)
            {
                if (removeDays.Where(o => o.Date == day.Date).Any())
                {
                    this.gridGarment.Columns[columnIndex].Visible = false; // 隱藏,但還在 ColumnIndex不會變
                }

                columnIndex++;
            }

            #region 關閉排序功能
            for (int i = 0; i < this.gridGarment.ColumnCount; i++)
            {
                this.gridGarment.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion

            // 凍結欄位
            this.gridGarment.Columns[0].Frozen = true;
            this.gridGarment.Columns[0].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
        }

        private void AfterQuery2()
        {
            this.listControlBindingSource2.DataSource = this.dtF;
            foreach (DataColumn item in this.dtF.Columns)
            {
                this.Helper.Controls.Grid.Generator(this.gridFabric_Panel_Code)
                .Text(item.ColumnName, header: item.ColumnName, width: Widths.Auto(), iseditingreadonly: true)
                ;
            }

            int columnIndex = 2;
            foreach (var day in this.Days2)
            {
                // string dateStr = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                // gridFabric_Panel_Code.Columns[ColumnIndex].Name = dateStr;
                // 假日的話粉紅色
                if (day.IsHoliday)
                {
                    this.gridFabric_Panel_Code.Columns[columnIndex].HeaderCell.Style.BackColor = Color.FromArgb(255, 199, 206);
                }

                columnIndex++;
            }

            // 扣除無產出的日期
            List<PublicPrg.Prgs.Day> removeDays = new List<PublicPrg.Prgs.Day>();

            foreach (var day in this.Days2)
            {
                // 如果該日期，不是「有資料」，則刪掉
                if (!this.AllData2.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                    && (MyUtility.Convert.GetInt(y.CutQty) > 0 || MyUtility.Convert.GetInt(y.StdQty) > 0)) // 不同於R01,是因此只有顯示CutQty,StdQty資料
                                                                .Any())
                                       .Any())
                {
                    removeDays.Add(day);
                }
                else if (!this.AllData.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                    && MyUtility.Convert.GetDecimal(y.WIP) > 0)
                                                                .Any())
                                       .Any()
                    && day.IsHoliday)
                {
                    removeDays.Add(day);
                }
            }

            columnIndex = 2;
            foreach (var day in this.Days2)
            {
                if (removeDays.Where(o => o.Date == day.Date).Any())
                {
                    this.gridFabric_Panel_Code.Columns[columnIndex].Visible = false; // 隱藏,但還在 ColumnIndex不會變
                }

                columnIndex++;
            }

            this.gridFabric_Panel_Code.Columns[0].Width = 115;

            #region 關閉排序功能
            for (int i = 0; i < this.gridFabric_Panel_Code.ColumnCount; i++)
            {
                this.gridFabric_Panel_Code.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion

            // 凍結欄位
            this.gridFabric_Panel_Code.Columns[0].Frozen = true;
            this.gridFabric_Panel_Code.Columns[1].Frozen = true;
            this.gridFabric_Panel_Code.Columns[0].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.gridFabric_Panel_Code.Columns[1].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            return;
        }

        /// <summary>
        /// Check Subprocess LeadTime
        /// </summary>
        /// <param name="orderIDs">List Order ID</param>
        /// <returns>bool</returns>
        public bool Check_Subprocess_LeadTime(List<string> orderIDs)
        {
            DualResult result;
            string cmd = $@"
SELECT  DISTINCT OrderID, s.MDivisionID, s.FactoryID
INTO #OrderList
FROM SewingSchedule s WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID
WHERE o.LocalOrder = 0
AND OrderID in ('{string.Join("','", orderIDs)}')
";

            cmd += $@"
SELECT DIStINCT  b.POID ,a.OrderID ,b.FtyGroup, a.MDivisionID, a.FactoryID
FROM #OrderList a
INNER JOIN Orders b ON a.OrderID= b.ID 

drop table #OrderList
";
            result = DBProxy.Current.Select(null, cmd, out DataTable poID_dt);

            if (!result)
            {
                this.finalmsg = result.ToString();
                return false;
            }

            this.FtyFroup = poID_dt.AsEnumerable().Select(o => o["FtyGroup"].ToString()).Distinct().ToList();
            List<string> msg = new List<string>();

            foreach (DataRow dr in poID_dt.Rows)
            {
                string pOID = dr["POID"].ToString();
                string orderID = dr["OrderID"].ToString();
                string mDivisionID = dr["MDivisionID"].ToString();
                string factoryID = dr["FactoryID"].ToString();
                GetGarmentListTable(string.Empty, pOID, string.Empty, out DataTable garmentTb);
                List<string> annotationList = garmentTb.AsEnumerable().Where(o => !MyUtility.Check.Empty(o["Annotation"].ToString())).Select(o => o["Annotation"].ToString()).Distinct().ToList();
                List<string> annotationList_Final = new List<string>();

                foreach (var annotation in annotationList)
                {
                    foreach (var item in annotation.Split('+'))
                    {
                        string input = string.Empty;
                        for (int i = 0; i <= item.Length - 1; i++)
                        {
                            // 排除掉數字
                            if (!int.TryParse(item[i].ToString(), out int x))
                            {
                                input += item[i].ToString();
                            }
                        }

                        if (!annotationList_Final.Contains(input) && MyUtility.Check.Seek($"SELECT 1 FROM Subprocess WHERE ID='{input}' "))
                        {
                            annotationList_Final.Add(input);
                        }
                    }
                }

                string annotationStr = annotationList_Final.OrderBy(o => o.ToString()).JoinToString("+");
                string chk_LeadTime = $@"
SELECT DISTINCT SD.ID
                ,Subprocess.IDs
                ,LeadTime= s.LeadTime
FROM SubprocessLeadTime s WITH(NOLOCK)
INNER JOIN SubprocessLeadTime_Detail SD WITH(NOLOCK) on s.ID = sd.ID
OUTER APPLY(
	SELECT IDs=STUFF(
	 (
		SELECT '+'+SubprocessID
		FROM SubprocessLeadTime_Detail WITH(NOLOCK)
		WHERE ID = SD.ID
		FOR XML PATH('')
	)
	,1,1,'')
)Subprocess
WHERE Subprocess.IDs = '{annotationStr}'
and s.MDivisionID = '{mDivisionID}'
and s.FactoryID = '{factoryID}'
";
                result = DBProxy.Current.Select(null, chk_LeadTime, out DataTable leadTime_dt);
                if (!result)
                {
                    this.finalmsg = result.ToString();
                    return false;
                }

                // 收集需要顯示訊息的Subprocess ID
                if (leadTime_dt.Rows.Count == 0 && annotationStr != string.Empty)
                {
                    msg.Add(mDivisionID + ";" + factoryID + ";" + annotationStr);
                }
                else
                {
                    // 記錄下加工段的Lead Time
                    LeadTime o = new LeadTime()
                    {
                        OrderID = orderID,
                        LeadTimeDay = MyUtility.Check.Empty(annotationStr) ? 0 : Convert.ToInt32(leadTime_dt.Rows[0]["LeadTime"]), // 加工段為空，LeadTimeDay = 0
                    };
                    this.LeadTimeList.Add(o);
                }
            }

            if (msg.Count > 0)
            {
                string message = "<" + msg.Distinct().OrderBy(o => o).JoinToString(">" + Environment.NewLine + "<") + ">";
                message = message.Replace(";", "><");
                message += Environment.NewLine + @"Please set cutting lead time in [Cutting_B09. Subprocess Lead Time].
When the settings are complete, can be use this function!!
";
                this.finalmsg = message;

                // MyUtility.Msg.InfoBox(this.finalmsg);
                return false;
            }

            return true;
        }

        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex > 0)
            {
                return;
            }

            if (e.RowIndex > 0)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }

            if (!this.IsEmptyCellValue(e.RowIndex, (Win.UI.Grid)sender))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = ((Win.UI.Grid)sender).AdvancedCellBorderStyle.Bottom;
            }
        }

        private bool IsEmptyCellValue(int row, Win.UI.Grid grid)
        {
            if (row == grid.Rows.Count - 1)
            {
                return true;
            }

            DataGridViewCell cell1 = grid["SP", row];

            return MyUtility.Check.Empty(cell1.Value);
        }

        private int Qend = 0;
        private int Qrun = 1;

        private void BgWorkerUpdateInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {
                this.Qend = 0;
                if (!this.Query1())
                {
                    this.Qend = 3;
                    this.bgWorkerUpdateInfo.CancelAsync();
                    this.bgWorkerUpdateInfo.ReportProgress(0);
                    return;
                }

                this.Qend = 1;

                if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
                {
                    return;
                }

                this.bgWorkerUpdateInfo.ReportProgress(100);

                this.Qrun = 2;
                if (!this.Query2())
                {
                    this.Qend = 3;
                    this.bgWorkerUpdateInfo.CancelAsync();
                    this.bgWorkerUpdateInfo.ReportProgress(0);
                    return;
                }

                this.Qend = 2;
                this.bgWorkerUpdateInfo.ReportProgress(100);
            }
        }

        private void BgWorkerUpdateInfo_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.bgWorkerUpdateInfo == null || this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                if (!MyUtility.Check.Empty(this.finalmsg))
                {
                    MyUtility.Msg.WarningBox(this.finalmsg);
                }

                return;
            }

            if (this.Qrun == 1)
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
            else
            {
                this.progressBar2.Value = e.ProgressPercentage;
            }

            if (this.Qend == 1)
            {
                this.progressBar1.Visible = false;
                this.AfterQuery1();
                this.Qend = 0;
            }
            else if (this.Qend == 2)
            {
                this.progressBar2.Visible = false;
                this.AfterQuery2();
                this.Qend = 0;
            }
            else if (this.Qend == 3)
            {
                this.progressBar1.Visible = false;
                this.progressBar2.Visible = false;
            }
        }

        private void P02_StandardQtyPlannedCuttingWIP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bgWorkerUpdateInfo.IsBusy)
            {
                this.bgWorkerUpdateInfo.CancelAsync();
            }
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex > -1)
            {
                if (e.RowIndex % 4 > 1)
                {
                    e.CellStyle.BackColor = Color.FromArgb(128, 255, 255);
                }
            }
        }
    }
}
