using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Class.Command
{
    /// 這檔案 MES Basic B02, B07 有在用, Class DLL 要更新到 MES
    /// <inheritdoc/>
    public class MachineCalendar
    {
#pragma warning disable SA1503
        /// <inheritdoc/>
        public DataTable GetWeekDayDataTable(long machineIoT_CalendareUkey)
        {
            DataTable dtQuery = this.QueryMachineIoT_Calendar_Detail(machineIoT_CalendareUkey);
            if (dtQuery == null)
            {
                return this.SetWeekDayDataTable();
            }
            else
            {
                return this.ProcessMachineToWeekDayDataTable(dtQuery);
            }
        }

        /// <inheritdoc/>
        public DataTable SetWeekDayDataTable()
        {
            DataTable dt = new DataTable();

            // 建立 7 組 [Start] 和 [End] 欄位 (代表 WeekDay 1 到 WeekDay 7)
            for (int i = 1; i <= 7; i++)
            {
                dt.Columns.Add($"Start{i}");
                dt.Columns.Add($"End{i}");
                dt.Columns.Add($"IsCrossDate{i}", typeof(bool));
            }

            return dt;
        }

        /// <summary>
        /// DataTable 欄位 WeekDay (1~7), IsCrossDate (bit), StartTime (00:00), EndTime (00:00)
        /// </summary>
        /// <inheritdoc/>
        public DataTable ProcessMachineToWeekDayDataTable(DataTable dt)
        {
            DataTable dtCalendar = this.SetWeekDayDataTable();

            // 找出最多資料筆數，這將決定需要多少 DataRow
            int maxRowCount = Enumerable.Range(1, 7).Select(i => dt.Select($"WeekDay = {i}").Length).Max();

            // 初始化 DataRow，根據每個 WeekDay 的資料填入相對應的位置
            for (int rowIndex = 0; rowIndex < maxRowCount; rowIndex++)
            {
                DataRow newRow = dtCalendar.NewRow();

                for (int i = 1; i <= 7; i++)
                {
                    DataRow[] rowsForDay = dt.Select($"WeekDay = {i}");
                    if (rowIndex < rowsForDay.Length)
                    {
                        newRow[$"Start{i}"] = rowsForDay[rowIndex]["StartTime"];
                        newRow[$"End{i}"] = rowsForDay[rowIndex]["EndTime"];
                        newRow[$"IsCrossDate{i}"] = rowsForDay[rowIndex]["IsCrossDate"];
                    }
                }

                dtCalendar.Rows.Add(newRow);
            }

            return dtCalendar;
        }

        /// <inheritdoc/>
        public DataTable GetMachineIoT_Calendar_DetailSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add($"WeekDay", typeof(int));
            dt.Columns.Add($"IsCrossDate", typeof(bool)).DefaultValue = false;
            dt.Columns.Add($"StartTime");
            dt.Columns.Add($"EndTime");
            return dt;
        }

        /// <inheritdoc/>
        public DataTable QueryMachineIoT_Calendar_Detail(long machineIoT_CalendareUkey)
        {
            string sqlcmd = $@"
SELECT WeekDay
      ,IsCrossDate
      ,StartTime = CONVERT(VARCHAR(5), StartTime, 108)
      ,EndTime = CONVERT(VARCHAR(5), EndTime, 108) 
FROM MachineIoT_Calendar_Detail
WHERE MachineIoT_CalendareUkey = {machineIoT_CalendareUkey}
ORDER BY WeekDay,IsCrossDate,StartTime
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        /// <summary>
        /// Save 之前檢查 DataTable 爛位 StartTime 不可大於 EndTime
        /// Cutting B05 EditCalendar
        /// MES B07 Set
        /// MES B07 Batch assign special time
        /// </summary>
        /// <inheritdoc/>
        public bool ValidateTime(DataTable dt, bool showMsg = true)
        {
            if (!this.ValidateStartEndTime(dt, showMsg))
            {
                return false;
            }

            if (!this.ValidateTimeIntervals(dt, showMsg))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查[單筆] 爛位 StartTime 不可大於等於 EndTime
        /// Cutting B05 EditCalendar
        /// MES B07 Set
        /// MES B07 Batch assign special time
        /// </summary>
        /// <inheritdoc/>
        public bool ValidateStartEndTime(DataTable dt, bool showMsg = true)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (TimeSpan.TryParse(row["StartTime"].ToString(), out TimeSpan startTime) &&
                    TimeSpan.TryParse(row["EndTime"].ToString(), out TimeSpan endTime))
                {
                    if (startTime >= endTime && row["EndTime"].ToString() != "00:00")
                    {
                        if (showMsg) MyUtility.Msg.WarningBox("The start time cannot be greater than the end time.");
                        return false;
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Time format error");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查[同一天]不可時間交集: 排序後連續資料 EndTime 不可大於下一筆的 StartTime
        /// Cutting B05 EditCalendar
        /// MES B07 Set
        /// MES B07 Batch assign special time
        /// </summary>
        /// <inheritdoc/>
        public bool ValidateTimeIntervals(DataTable dt, bool showMsg = true)
        {
            // 把 isCrossDate 為 true 和 false 的資料分開
            var rowsByIsCrossDate = dt.AsEnumerable().GroupBy(row => MyUtility.Convert.GetBool(row["isCrossDate"]));

            foreach (var group in rowsByIsCrossDate)
            {
                // 根據 StartTime 進行排序
                var sortedRows = group.OrderBy(row => TimeSpan.Parse(row["StartTime"].ToString())).ToList();

                // 驗證每一列，確保時間段沒有重疊或直接連續
                for (int i = 0; i < sortedRows.Count - 1; i++)
                {
                    TimeSpan currentEndTime = TimeSpan.Parse(sortedRows[i]["EndTime"].ToString());
                    TimeSpan nextStartTime = TimeSpan.Parse(sortedRows[i + 1]["StartTime"].ToString());

                    if (currentEndTime == TimeSpan.Zero) currentEndTime = TimeSpan.FromHours(24); // 將其視為 24:00

                    // 檢查時間重疊的情況
                    if (currentEndTime > nextStartTime)
                    {
                        if (showMsg) MyUtility.Msg.WarningBox("Start time must be greater than the End time of the previous period.");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 新增時 預防相同 machineIoT 之下的 MachineIoT_Calendar 不能出現兩筆相同 StartDate
        /// </summary>
        /// <inheritdoc/>
        public bool CheckDuplicateStartDate(long machineIoTUkey, DateTime? startDate, long machineIoT_CalendarUkey, string machineID, bool showMsg = true)
        {
            string sqlcmd = $@"
SELECT 1 FROM MachineIoT_Calendar WHERE MachineIoTUkey = {machineIoTUkey} AND StartDate = '{startDate:yyyy-MM-dd}' AND Ukey <> {machineIoT_CalendarUkey}
";
            if (MyUtility.Check.Seek(sqlcmd, "ManufacturingExecution"))
            {
                if (showMsg) MyUtility.Msg.WarningBox($"Machine ID <{machineID}> Start Date <{startDate:yyyy-MM-dd}> already existed.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 確認【一週班表】不同天的時間是否有重疊
        /// </summary>
        /// <inheritdoc/>
        public bool ValidateWeekdayOverlap(DataTable dt, bool showMsg = true)
        {
            List<string> overlapDays = new List<string>();
            string[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            // 按 WeekDay 排序資料
            DataView dataView = dt.DefaultView;
            dataView.Sort = "WeekDay ASC, IsCrossDate ASC, StartTime ASC";
            DataTable sortedDt = dataView.ToTable();

            // 檢查每一天的時間與下一天的時間是否有重疊
            for (int i = 0; i < sortedDt.Rows.Count; i++)
            {
                DataRow currentRow = sortedDt.Rows[i];
                int currentWeekDay = Convert.ToInt32(currentRow["WeekDay"]);
                bool currentIsCrossDate = Convert.ToBoolean(currentRow["IsCrossDate"]);
                TimeSpan currentEndTime = TimeSpan.Parse(currentRow["EndTime"].ToString());
                if (currentEndTime == TimeSpan.Zero) currentEndTime = TimeSpan.FromHours(24); // 將其視為 24:00

                // 找出下一天的 WeekDay (若是 7 則下個是 1)
                int nextWeekDay = currentWeekDay == 7 ? 1 : currentWeekDay + 1;

                // 找下一天且 IsCrossDate 為 false 的資料列
                DataRow[] nextDayRows = sortedDt.Select($"WeekDay = {nextWeekDay} AND IsCrossDate = 0");

                foreach (DataRow nextDayRow in nextDayRows)
                {
                    TimeSpan nextDayStartTime = TimeSpan.Parse(nextDayRow["StartTime"].ToString());

                    // 若當前天的結束時間與下一天的開始時間有重疊，則紀錄問題
                    if (currentIsCrossDate && currentEndTime > nextDayStartTime)
                    {
                        overlapDays.Add(dayNames[currentWeekDay - 1]); // 將重疊的星期名稱加入清單
                        break; // 如果找到重疊就跳出，繼續檢查下一天
                    }
                }
            }

            // 如果有重疊情況，顯示錯誤訊息
            if (overlapDays.Count > 0)
            {
                string errorMessage = "Working time overlap on " + string.Join(" and ", overlapDays) + ".";
                if (showMsg) MyUtility.Msg.WarningBox(errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查班表
        /// </summary>
        /// <inheritdoc/>
        public DualResult ValidateOverlap(long machineIoTUkey, DateTime? startDate, DataTable dtMachineIoT_Calendar_Detail)
        {
            DualResult result;

            // 檢查這個班表的 [第一天] 是否有時間重疊
            if (!(result = this.ValidateStartOverlap(startDate, machineIoTUkey, dtMachineIoT_Calendar_Detail))) return result;

            // 找此班表結束日
            if (!(result = this.GetEndDate(startDate, machineIoTUkey, out DateTime? endDate))) return result;

            // 檢查這個班表的 [最後一天] 是否有時間重疊
            if (endDate != null && !(result = this.ValidateEndOverlap(endDate, machineIoTUkey, dtMachineIoT_Calendar_Detail))) return result;

            // 檢查在這個區間裡如果有設定【特殊班表】是否會有重疊
            if (!(result = this.ValidateRegionOverlap(startDate, endDate, machineIoTUkey, dtMachineIoT_Calendar_Detail))) return result;

            return new DualResult(true);
        }

        /// <summary>
        /// 檢查這個班表的 [第一天] 是否有時間重疊
        /// </summary>
        /// <inheritdoc/>
        public DualResult ValidateStartOverlap(DateTime? startDate, long machineIoTUkey, DataTable dtMachineIoT_Calendar_Detail)
        {
            DateTime date = (DateTime)startDate;
            DualResult result;
            if (!(result = this.GetMachineIoTScheduleNonTemp(date.AddDays(-1), machineIoTUkey, out DataTable dt1))) return result;
            if (!(result = this.GetMachineIoTSchedule(date, machineIoTUkey, dtMachineIoT_Calendar_Detail, out DataTable dt2))) return result;
            if (!this.ValidateDataTableOverlap(dt1, dt2, date.AddDays(-1), machineIoTUkey)) return new DualResult(false, new Exception(string.Empty));
            return Ict.Result.True;
        }

        /// <summary>
        /// 檢查這個班表的 [最後一天] 是否有時間重疊
        /// </summary>
        /// <inheritdoc/>
        public DualResult ValidateEndOverlap(DateTime? endDate, long machineIoTUkey, DataTable dtMachineIoT_Calendar_Detail)
        {
            DateTime date = (DateTime)endDate;
            DualResult result;
            if (!(result = this.GetMachineIoTSchedule(date, machineIoTUkey, dtMachineIoT_Calendar_Detail, out DataTable dt1))) return result;
            if (!(result = this.GetMachineIoTScheduleNonTemp(date.AddDays(+1), machineIoTUkey, out DataTable dt2))) return result;
            if (!this.ValidateDataTableOverlap(dt1, dt2, date, machineIoTUkey)) return new DualResult(false, new Exception(string.Empty));
            return Ict.Result.True;
        }

        /// <summary>
        /// 檢查在這個區間裡如果有設定【特殊班表】是否會有重疊
        /// </summary>
        /// <inheritdoc/>
        public DualResult ValidateRegionOverlap(DateTime? startDate, DateTime? endDate, long machineIoTUkey, DataTable dtMachineIoT_Calendar_Detail, bool showMsg = true)
        {
            DateTime date = (DateTime)startDate;
            DualResult result;
            if (!(result = this.GetMachineIoT_SpecialDate(machineIoTUkey, date, endDate, out DataTable dt))) return result;
            List<DateTime> specialDateList = this.GetSpecialDateList(dt);
            foreach (var specialDate in specialDateList.OrderBy(d => d))
            {
                if (!(result = this.GetMachineIoTSchedule(specialDate, machineIoTUkey, dtMachineIoT_Calendar_Detail, out DataTable dt1))) return result;
                if (!(result = this.GetMachineIoTSchedule(specialDate.AddDays(1), machineIoTUkey, dtMachineIoT_Calendar_Detail, out DataTable dt2))) return result;
                if (!this.ValidateDataTableOverlap(dt1, dt2, specialDate, machineIoTUkey, showMsg)) return new DualResult(false, new Exception(string.Empty));
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// dt1 的 IsCrossDate 最大 EndTime > dt2 !IsCrossDate 最小 StartTime 則有交集 跳訊息
        /// </summary>
        /// <inheritdoc/>
        public bool ValidateDataTableOverlap(DataTable dt1, DataTable dt2, DateTime date, long machineIoTUkey, bool showMsg = true)
        {
            var maxEndTimeInDt1 = this.GetMaxEndTime(dt1);
            var minStartTimeInDt2 = this.GetMinStartTime(dt2);

            // 如果其中一個時間為 null，直接回傳 true
            if (maxEndTimeInDt1 == null || minStartTimeInDt2 == null) return true;

            if (maxEndTimeInDt1 > minStartTimeInDt2)
            {
                DataRow dr = this.GetMachineIoT(machineIoTUkey);
                string errorMessage = $@"
Machine Type : {dr["MachineIoTType"]}
Machine ID : {dr["MachineID"]}
Working time overlap on [{date:yyyy/MM/dd}].";
                if (showMsg) MyUtility.Msg.WarningBox(errorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 找到此班表最後一天, 非實體欄位, 找到下一個周期性班表開始日 -1 一天
        /// </summary>
        /// <inheritdoc/>
        public DualResult GetEndDate(DateTime? startDate, long machineIoTUkey, out DateTime? endDate)
        {
            endDate = null;
            string sqlcmd = $@"SELECT MIN(StartDate) FROM MachineIoT_Calendar WITH(NOLOCK) WHERE MachineIoTUkey = {machineIoTUkey} AND startDate > '{startDate:yyyy/MM/dd}'";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
            if (!result) return result;
            if (!MyUtility.Check.Empty(dt.Rows[0][0]))
            {
                endDate = ((DateTime)dt.Rows[0][0]).AddDays(-1);
            }

            return Ict.Result.True;
        }

        /// <summary>
        ///  找出 IsCrossDate = true 的資料中最大 EndTime
        /// </summary>
        /// <inheritdoc/>
        public TimeSpan? GetMaxEndTime(DataTable dt)
        {
            var endTimeList = dt.AsEnumerable()
                .Where(row => MyUtility.Convert.GetBool(row["IsCrossDate"]))
                .Select(row =>
                {
                    TimeSpan endTime = TimeSpan.Parse(row["EndTime"].ToString());
                    return endTime == TimeSpan.Zero ? TimeSpan.FromHours(24) : endTime; // 如果是 00:00，視為 24:00
                })
                .ToList();

            return endTimeList.Count > 0 ? (TimeSpan?)endTimeList.Max() : null;
        }

        /// <summary>
        /// 找出 IsCrossDate = false 的資料中最小 StartTime
        /// </summary>
        /// <inheritdoc/>
        public TimeSpan? GetMinStartTime(DataTable dt)
        {
            var startTimeList = dt.AsEnumerable()
                .Where(row => !MyUtility.Convert.GetBool(row["IsCrossDate"]))
                .Select(row => TimeSpan.Parse(row["StartTime"].ToString()))
                .ToList();

            return startTimeList.Count > 0 ? (TimeSpan?)startTimeList.Min() : null;
        }

        /// <summary>
        /// 舉例:週期性班表區間 12/30~01~10
        /// 特殊班表清單中有 01/02, 01/03, 01/06
        /// 額需要再補充 01/01 與 01/05
        /// </summary>
        /// <inheritdoc/>
        public List<DateTime> GetSpecialDateList(DataTable dt)
        {
            List<DateTime> specialDateList = dt.AsEnumerable().Select(row => (DateTime)row["SpecialDate"]).ToList();

            // 建立一個新的 HashSet 來儲存補充後的日期，確保日期不重複
            HashSet<DateTime> completeDateSet = new HashSet<DateTime>(specialDateList);

            // 將每一筆日期減去一天，並加入到 HashSet 中
            foreach (var specialDate in specialDateList)
            {
                completeDateSet.Add(specialDate.AddDays(-1));
            }

            List<DateTime> finalDateList = completeDateSet.ToList();
            return finalDateList;
        }

        /// <inheritdoc/>
        public DualResult GetMachineIoTScheduleNonTemp(DateTime date, long machineIoTUkey, out DataTable dt)
        {
            string sqlcmd = $@"
DECLARE @Date DATE = '{date:yyyy/MM/dd}'
DECLARE @MachineIoTUkey BIGINT = {machineIoTUkey}
SELECT * FROM dbo.GetMachineIoTScheduleNonTemp(@Date, @MachineIoTUkey)
ORDER BY IsCrossDate,StartTime,EndTime
";
            return DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out dt);
        }

        /// <summary>
        /// 特殊班表優先於週期班表,舉例:當10/17已經有特殊班表在DB,週期性班表10/17開始,10/17 這天以特殊班表為主
        /// 取出的班表優先度 1.特殊 > 2.假日 > 3.丟進去的班表 dtMachineIoT_Calendar_Detail
        /// 所以需要去 DB 取資料來比較
        /// </summary>
        /// <inheritdoc/>
        public DualResult GetMachineIoTSchedule(DateTime date, long machineIoTUkey, DataTable dtMachineIoT_Calendar_Detail, out DataTable dt)
        {
            dt = null;
            string sqlcmd = $@"
DECLARE @Date DATE = '{date:yyyy/MM/dd}'
DECLARE @MachineIoTUkey BIGINT = {machineIoTUkey}
DECLARE @MachineIoTCalendar as dbo.MachineIoTCalendar
INSERT INTO @MachineIoTCalendar SELECT WeekDay,StartTime,EndTime,IsCrossDate FROM #tmp
SELECT * FROM dbo.GetMachineIoTSchedule(@Date, @MachineIoTUkey, @MachineIoTCalendar)
ORDER BY IsCrossDate,StartTime,EndTime
";
            DualResult result;
            if (!(result = DBProxy.Current.OpenConnection("ManufacturingExecution", out SqlConnection conn))) return result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMachineIoT_Calendar_Detail, "WeekDay,StartTime,EndTime,IsCrossDate", sqlcmd, out dt, conn: conn))) return result;
            return Ict.Result.True;
        }

        /// <summary>
        /// 找出時間範圍內的特殊班表 日期使用
        /// </summary>
        /// <inheritdoc/>
        public DualResult GetMachineIoT_SpecialDate(long machineIoTUkey, DateTime startTime, DateTime? endTime, out DataTable dt)
        {
            string where = string.Empty;
            if (endTime != null)
            {
                where = $" AND SpecialDate <= '{endTime:yyyy/MM/dd}'";
            }

            string sqlcmd = $@"
SELECT *
FROM MachineIoT_SpecialDate
WHERE MachineIoTUkey = {machineIoTUkey}
AND SpecialDate >= '{startTime:yyyy/MM/dd}'
{where}
";
            return DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out dt);
        }

        /// <summary>
        /// 刪除 MachineIoT_Calendar
        /// </summary>
        /// <inheritdoc/>
        public void DeleteMachineIoT_Calendar(long machineIoTUkey, long machineIoT_CalendarUkey, DateTime startDate)
        {
            // 找出前一個週期表班表為Table參數
            DataTable dtMachineIoT_Calendar_Detail = this.GetBeforeMachineIoT_Calendar_Detail(machineIoTUkey, machineIoT_CalendarUkey, startDate);

            // 檢查班表
            DualResult result;
            try
            {
                if (!(result = this.ValidateOverlap(machineIoTUkey, startDate, dtMachineIoT_Calendar_Detail))) throw result.GetException();

                // 刪除
                string sqlcmd = $@"
DELETE MachineIoT_Calendar WHERE Ukey = {machineIoT_CalendarUkey}
DELETE MachineIoT_Calendar_Detail WHERE MachineIoT_CalendareUkey = {machineIoT_CalendarUkey}
";
                if (!(result = DBProxy.Current.Execute("ManufacturingExecution", sqlcmd))) throw result.GetException();
            }
            catch (Exception ex)
            {
                if (ex.Message != string.Empty) MyUtility.Msg.ErrorBox(ex.ToString());
            }
        }

        /// <summary>
        /// 找到前一個周期性班表
        /// </summary>
        /// <inheritdoc/>
        public DataTable GetBeforeMachineIoT_Calendar_Detail(long machineIoTUkey, long machineIoT_CalendarUkey, DateTime? startDate)
        {
            string sqlcmd = $@"
DECLARE @MachineIoTUkey BIGINT = {machineIoTUkey}
DECLARE @MachineIoT_CalendarUkey BIGINT = {machineIoT_CalendarUkey}
DECLARE @Date Date = '{startDate:yyyy/MM/dd}'

SELECT md.WeekDay
      ,StartTime = CONVERT(VARCHAR(5), StartTime, 108)
      ,EndTime = CONVERT(VARCHAR(5), EndTime, 108)
      ,md.IsCrossDate
FROM MachineIoT_Calendar_Detail md WITH (NOLOCK)
WHERE md.MachineIoT_CalendareUkey = (
    SELECT m.Ukey
    FROM MachineIoT_Calendar m WITH (NOLOCK)
    WHERE m.MachineIoTUkey = @MachineIoTUkey
    AND m.StartDate = (
        SELECT MAX(StartDate)
        FROM MachineIoT_Calendar m WITH (NOLOCK)
        WHERE m.MachineIoTUkey = @MachineIoTUkey
        AND m.Ukey <> @MachineIoT_CalendarUkey
        AND m.StartDate <= @Date))
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            return dt;
        }

        /// <summary>
        /// Save 之前檢查[同一天] DataTable 爛位 StartTime 不可大於 EndTime
        /// Cutting B05 EditCalendar
        /// MES B07 Set
        /// MES B07 Batch assign special time
        /// </summary>
        /// <param name="dt"> grid 綁定的 DataTable</param>
        /// <param name="dt2">Time 欄位值格式化成 hh:mm 以便後續檢查和存檔 </param>
        /// <inheritdoc/>
        public bool ValidateDataTableTime(DataTable dt, out DataTable dt2)
        {
            dt2 = null;

            // 移除全空的資料列
            dt.AsEnumerable()
                .Where(row => MyUtility.Check.Empty(row["StartTime"]) && MyUtility.Check.Empty(row["EndTime"]) && !MyUtility.Convert.GetBool(row["IsCrossDate"]))
                .ToList().ForEach(row => row.Delete());
            dt.AcceptChanges();

            // 檢查 Time 欄位不可空
            if (dt.AsEnumerable().Any(row => MyUtility.Check.Empty(row["StartTime"]) || MyUtility.Check.Empty(row["EndTime"])))
            {
                MyUtility.Msg.WarningBox("StartTime and EndTime can not empty!");
                return false;
            }

            // 複製一份,格式化值後檢查
            dt2 = dt.Copy();

            // 實際值轉為 hh:mm
            foreach (DataRow dr in dt2.Rows)
            {
                dr["StartTime"] = dr["StartTime"].ToTimeFormat();
                dr["EndTime"] = dr["EndTime"].ToTimeFormat();
            }

            // 排序資料
            DataView dataView = dt2.DefaultView;
            dataView.Sort = "isCrossDate ASC, StartTime ASC";
            dt2 = dataView.ToTable();

            // 檢查 DataTable 時間不能有交集
            if (!this.ValidateTime(dt2))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public DataRow GetMachineIoT(long ukey)
        {
            MyUtility.Check.Seek($"SELECT * FROM MachineIoT WHERE Ukey = {ukey}", out DataRow dr, "ManufacturingExecution");
            return dr;
        }

        /// <inheritdoc/>
        public bool ValidSpecialDate(DataTable dt, DateTime date, long machineIoTUkey)
        {
            // 檢查特殊班表前一天是否有時間重疊
            this.GetMachineIoTScheduleNonTemp(date.AddDays(-1), machineIoTUkey, out DataTable dt1);
            if (!this.ValidateDataTableOverlap(dt1, dt, date.AddDays(-1), machineIoTUkey))
            {
                return false;
            }

            // 檢查特殊班表後一天是否有時間重疊
            this.GetMachineIoTScheduleNonTemp(date.AddDays(1), machineIoTUkey, out DataTable dt3);
            if (!this.ValidateDataTableOverlap(dt, dt3, date, machineIoTUkey))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得 MachineIoT_SpecialDate.Ukey
        /// </summary>
        /// <inheritdoc/>
        public long GetMachineIoT_SpecialUkey(long machineIoTUkey, DateTime specialDate)
        {
            string sqlcmd = $"SELECT Ukey FROM MachineIoT_SpecialDate WITH(NOLOCK) WHERE MachineIoTUkey = @MachineIoTUkey AND SpecialDate = @SpecialDate";
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@MachineIoTUkey", SqlDbType.BigInt) { Value = machineIoTUkey },
                new SqlParameter("@SpecialDate", SqlDbType.Date) { Value = specialDate },
            };
            if (MyUtility.Check.Seek(sqlcmd, sqlParameters, out DataRow dr, "ManufacturingExecution"))
            {
                return MyUtility.Convert.GetLong(dr["Ukey"]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 寫入或更新 MachineIoT_SpecialDate
        /// </summary>
        /// <inheritdoc/>
        public DualResult SaveMachineIoT_SpecialDate(long machineIoTUkey, DateTime specialDate, out long machineIoT_SpecialUkey)
        {
            machineIoT_SpecialUkey = this.GetMachineIoT_SpecialUkey(machineIoTUkey, specialDate);
            if (machineIoT_SpecialUkey == 0)
            {
                return this.InsertMachineIoT_SpecialDate(machineIoTUkey, specialDate, out machineIoT_SpecialUkey);
            }
            else
            {
                return this.UpdateMachineIoT_SpecialDate(machineIoT_SpecialUkey);
            }
        }

        /// <summary>
        /// 新增 MachineIoT_SpecialDate
        /// </summary>
        /// <inheritdoc/>
        public DualResult InsertMachineIoT_SpecialDate(long machineIoTUkey, DateTime specialDate, out long machineIoT_SpecialUkey)
        {
            string sqlcmd = $@"
INSERT INTO MachineIoT_SpecialDate (MachineIoTUkey, SpecialDate, AddName, AddDate)
OUTPUT INSERTED.Ukey
    VALUES (@MachineIoTUkey, @SpecialDate, @AddName, GETDATE())
";
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@MachineIoTUkey", SqlDbType.BigInt) { Value = machineIoTUkey },
                new SqlParameter("@SpecialDate", SqlDbType.Date) { Value = specialDate },
                new SqlParameter("@AddName", SqlDbType.VarChar, 10) { Value = Sci.Env.User.UserID },
            };
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, sqlParameters, out DataTable dt);
            if (!result)
            {
                machineIoT_SpecialUkey = 0;
            }
            else
            {
                machineIoT_SpecialUkey = MyUtility.Convert.GetLong(dt.Rows[0][0]);
            }

            return result;
        }

        /// <summary>
        /// 更新 MachineIoT_SpecialDate
        /// </summary>
        /// <inheritdoc/>
        public DualResult UpdateMachineIoT_SpecialDate(long machineIoT_SpecialUkey)
        {
            string sqlcmd = $@"
UPDATE MachineIoT_SpecialDate
SET EditName = @EditName
   ,EditDate = GETDATE()
WHERE Ukey = @machineIoT_SpecialUkey
";
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@machineIoT_SpecialUkey", SqlDbType.BigInt) { Value = machineIoT_SpecialUkey },
                new SqlParameter("@EditName", SqlDbType.VarChar, 10) { Value = Sci.Env.User.UserID },
            };
            return DBProxy.Current.Execute("ManufacturingExecution", sqlcmd, sqlParameters);
        }

        /// <summary>
        /// 全覆蓋 MachineIoT_SpecialDate_Detail
        /// </summary>
        /// <inheritdoc/>
        public DualResult SaveMachineIoT_SpecialDate_Detail(DataTable dt, long machineIoT_SpecialUkey)
        {
            string sqlcmd = $@"
DELETE MachineIoT_SpecialDate_Detail WHERE MachineIoT_SpecialUkey = {machineIoT_SpecialUkey}

INSERT INTO MachineIoT_SpecialDate_Detail(MachineIoT_SpecialUkey, StartTime, EndTime, IsCrossDate,  AddName, AddDate)
SELECT {machineIoT_SpecialUkey}, StartTime, EndTime, IsCrossDate, '{Sci.Env.User.UserID}', GETDATE()
FROM #tmp
";
            DualResult result;
            if (!(result = DBProxy.Current.OpenConnection("ManufacturingExecution", out SqlConnection conn))) return result;
            return MyUtility.Tool.ProcessWithDatatable(dt, "StartTime,EndTime,IsCrossDate", sqlcmd, out DataTable _, conn: conn);
        }

        /// <summary>
        /// 刪除特殊班表前檢查, 沒有假日才要檢查
        /// </summary>
        /// <inheritdoc/>
        public bool DeleteMachineIoT_SpecialDate_Before(long machineIoTUkey, DateTime date, string machineIoTType)
        {
            if (!this.IsHoliday(date, machineIoTType))
            {
                return this.ValidateBeforeDelete(machineIoTUkey, date);
            }

            return true;
        }

        /// <summary>
        /// 刪除假日前檢查, 沒有特殊班表才要檢查
        /// </summary>
        /// <inheritdoc/>
        public bool DeleteHoliday_Before(long machineIoTUkey, DateTime date)
        {
            if (!this.IsSpecialDate(machineIoTUkey, date))
            {
                return this.ValidateBeforeDelete(machineIoTUkey, date);
            }

            return true;
        }

        /// <inheritdoc/>
        public bool ValidateBeforeDelete(long machineIoTUkey, DateTime date)
        {
            // 檢查週期班表前一天是否有時間重疊
            this.GetMachineIoTScheduleNonTemp(date.AddDays(-1), machineIoTUkey, out DataTable dt1);
            DataTable dt2 = this.QueryCalendarByDate(date, machineIoTUkey); // 只須找出 @Date 的週期班表
            if (!this.ValidateDataTableOverlap(dt1, dt2, date.AddDays(-1), machineIoTUkey))
            {
                return false;
            }

            // 檢查週期班表後一天是否有時間重疊
            this.GetMachineIoTScheduleNonTemp(date.AddDays(1), machineIoTUkey, out DataTable dt3);
            if (!this.ValidateDataTableOverlap(dt2, dt3, date, machineIoTUkey))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 刪除特殊班表
        /// </summary>
        /// <inheritdoc/>
        public DualResult DeleteMachineIoT_SpecialDate(long machineIoTUkey, DateTime date)
        {
            long machineIoT_SpecialUkey = this.GetMachineIoT_SpecialUkey(machineIoTUkey, date);
            string sqlcmd = $@"
DELETE MachineIoT_SpecialDate WHERE Ukey = @MachineIoT_SpecialUkey
DELETE MachineIoT_SpecialDate_Detail WHERE MachineIoT_SpecialUkey = @MachineIoT_SpecialUkey
";
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@MachineIoT_SpecialUkey", SqlDbType.BigInt) { Value = machineIoT_SpecialUkey },
            };

            return DBProxy.Current.Execute("ManufacturingExecution", sqlcmd, sqlParameters);
        }

        /// <summary>
        /// 只找出 @Date 的週期班表
        /// </summary>
        /// <inheritdoc/>
        public DataTable QueryCalendarByDate(DateTime date, long machineIoTUkey)
        {
            string sqlcmd = $@"
DECLARE @Date as date = '{date:yyyy/MM/dd}'
DECLARE @MachineIoTUkey as int = {machineIoTUkey}
select [Date] = @Date
	, md.StartTime
	, md.EndTime
	, md.IsCrossDate
from MachineIoT_Calendar_Detail md with(nolock)
where md.MachineIoT_CalendareUkey = (
	select m.Ukey
	from MachineIoT_Calendar m with(nolock)
	where m.MachineIoTUkey = @MachineIoTUkey
	and m.StartDate = (
		select [StartDate] = MAX(StartDate)
		from MachineIoT_Calendar m with(nolock)
		where m.MachineIoTUkey = @MachineIoTUkey
		and m.StartDate <= @Date)
)
and md.WeekDay = (select DATEPART(WEEKDAY, @Date))
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        /// <summary>
        /// DB 確認假日
        /// </summary>
        /// <inheritdoc/>
        public bool IsHoliday(DateTime date, string machineIoTType)
        {
            string sqlcmd = $"SELECT 1 FROM MachineIoTHoliday WHERE HolidayDate = '{date:yyyy/MM/dd}' AND MachineIoTType = '{machineIoTType}'";
            return MyUtility.Check.Seek(sqlcmd, "ManufacturingExecution");
        }

        /// <summary>
        /// DB 特殊班表
        /// </summary>
        /// <inheritdoc/>
        public bool IsSpecialDate(long machineIoTUkey, DateTime date)
        {
            string sqlcmd = $"SELECT 1 FROM MachineIoT_SpecialDate WHERE MachineIoTUkey = {machineIoTUkey} AND SpecialDate = '{date:yyyy/MM/dd}'";
            return MyUtility.Check.Seek(sqlcmd, "ManufacturingExecution");
        }

        /// <summary>
        /// 計算[同一天] DataTable 中的工作時數
        /// </summary>
        /// <inheritdoc/>
        public double CalWorkingHour(DataTable dtDetail)
        {
            double totalHours = dtDetail.AsEnumerable().Sum(row =>
            {
                TimeSpan startTime = (TimeSpan)row["StartTime"];
                TimeSpan endTime = (TimeSpan)row["EndTime"];

                // 如果 EndTime 是 00:00，視為跨到隔天的 24:00
                if (endTime == TimeSpan.Zero)
                {
                    endTime = TimeSpan.FromHours(24); // 將 00:00 視為 24:00
                }

                // 計算該時間段的時數
                double hours = (endTime - startTime).TotalHours;
                return hours;
            });

            return Math.Round(totalHours, 1);
        }

        /// <summary>
        /// PMS Cutting B13, MES Basic B07 右側 Grid 資料
        /// </summary>
        /// <inheritdoc/>
        public DataTable QueryCalendarDetail(long machineIoTUkey, DateTime date)
        {
            DualResult result = this.GetMachineIoTScheduleNonTemp(date, machineIoTUkey, out DataTable dtDetail);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            dtDetail.Columns.Add("StartTimeDisplay");
            dtDetail.Columns.Add("EndTimeDisplay");

            foreach (DataRow dr in dtDetail.Rows)
            {
                // 假設 StartTime 和 EndTime 是儲存 TimeSpan 類型資料
                if (dr["StartTime"] is TimeSpan startTime)
                {
                    dr["StartTimeDisplay"] = startTime.ToString(@"hh\:mm");
                }

                if (dr["EndTime"] is TimeSpan endTime)
                {
                    dr["EndTimeDisplay"] = endTime.ToString(@"hh\:mm");
                }
            }

            return dtDetail;
        }

        /// <summary>
        /// PMS Cutting B13, MES Basic B07 一個 Cell 時數
        /// </summary>
        /// <inheritdoc/>
        public void QueryDetailAndSetMainWorkingHour(long machineIoTUkey, DateTime date, DataTable dtDisplayMain)
        {
            DataTable dtDetail = this.QueryCalendarDetail(machineIoTUkey, date);
            var totalHours = this.CalWorkingHour(dtDetail);
            dtDisplayMain.Select($"Ukey = {machineIoTUkey}").FirstOrDefault()[date.ToString("yyyy/MM/dd")] = totalHours;
        }
#pragma warning restore SA1503
    }
}
