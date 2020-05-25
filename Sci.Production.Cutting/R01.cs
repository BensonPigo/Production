using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Cutting
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }


        string MDivisionID, FactoryID;
        DataTable detailData;
        DataTable summaryData;
        DateTime? SewingDate_s, SewingDate_e;
        DateTime MinInLine, MaxOffLine;
        List<string> FtyFroup = new List<string>();
        List<InOffLineList> AllDataTmp = new List<InOffLineList>();
        List<InOffLineList> AllData = new List<InOffLineList>();
        List<PublicPrg.Prgs.Day> Days = new List<PublicPrg.Prgs.Day>();
        List<LeadTime> LeadTimeList = new List<LeadTime>();

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        protected override bool ValidateInput()
        {
            this.LeadTimeList.Clear();
            this.AllDataTmp.Clear();
            this.AllData.Clear();
            this.FtyFroup.Clear();
            this.Days.Clear();
            this.summaryData = new DataTable();
            this.detailData = new DataTable();

            if (!(SewingDate.Value1.HasValue || SewingDate.Value2.HasValue))
            {
                MyUtility.Msg.InfoBox("<Sewing Date> can’t be empty!!");
                return false;
            }


            this.MDivisionID = this.txtMdivision.Text;
            this.FactoryID = this.txtfactory.Text;
            this.SewingDate_s = SewingDate.Value1.Value;
            this.SewingDate_e = SewingDate.Value2.Value.AddDays(1).AddSeconds(-1);
            this.ShowWaitMessage("Checking...");
            if (!Check_Subprocess_LeadTime())
            {
                this.HideWaitMessage();
                return false;
            }
            this.HideWaitMessage();
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DataTable dt;
            string cmd = string.Empty;
            DualResult result;

            #region 起手資料
            cmd = $@"
SELECT *
FROM SewingSchedule s WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID
WHERE o.LocalOrder = 0
AND ( 
	(Cast(s.Inline as Date) >= '{SewingDate_s.Value.ToString("yyyy/MM/dd HH:mm:ss")}' AND Cast( s.Inline as Date) <= '{SewingDate_e.Value.ToString("yyyy/MM/dd HH:mm:ss")}' )
	OR
	(Cast(s.Offline as Date) >= '{SewingDate_s.Value.ToString("yyyy/MM/dd HH:mm:ss")}' AND Cast( s.Offline as Date) <= '{SewingDate_e.Value.ToString("yyyy/MM/dd HH:mm:ss")}' )
)
";
            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                cmd += Environment.NewLine + $@"AND s.MDivisionID='{this.MDivisionID}'";
            }
            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                cmd += Environment.NewLine + $@"AND s.FactoryID='{this.FactoryID}'";
            }

            result = DBProxy.Current.Select(null, cmd, out dt);

            if (!result)
            {
                return result;
            }

            if (dt.Rows.Count == 0)
            {
                return result;
            }

            #endregion

            //取出整份報表最早InLine / 最晚OffLine，先存下來待會用
            this.MinInLine = dt.AsEnumerable().Min(o => Convert.ToDateTime(o["Inline"]));
            this.MaxOffLine = dt.AsEnumerable().Max(o => Convert.ToDateTime(o["offline"]));

            #region 處理報表上橫向日期的時間軸 (扣除Lead Time)

            // 取得時間軸 ： (最早Inline - 最大Lead Time) ~ (最晚Offline - 最小Lead Time)
            int maxLeadTime = this.LeadTimeList.Max(o => o.LeadTimeDay);
            int minLeadTime = this.LeadTimeList.Min(o => o.LeadTimeDay);

            int maxLeadTime_u = maxLeadTime;
            int minLeadTime_u = minLeadTime;
            // 起點 = (最早Inline - 最大Lead Time)、終點 = (最晚Offline - 最小Lead Time)
            DateTime start_where = this.SewingDate_s.Value.Date;
            DateTime end_where = this.SewingDate_e.Value.Date;
            DateTime start = start_where.AddDays(-maxLeadTime).Date;
            DateTime end = end_where.AddDays(-minLeadTime).Date;

            this.Days.Clear();
            #region 找出 (start) ~ (start_where) 時間軸內，所有的假日
            DataTable dt2;
            string cmd2 = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{start_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{this.FtyFroup.JoinToString("','")}')
";
            result = DBProxy.Current.Select(null, cmd2, out dt2);
            // 開始組合時間軸
            for (int Day = 0; Day <= maxLeadTime_u; Day++)
            {
                PublicPrg.Prgs.Day day = new PublicPrg.Prgs.Day();
                day.Date = start_where.AddDays(-Day);

                // 是否行事曆設定假日
                bool IsHoliday = dt2.AsEnumerable().Where(o => MyUtility.Convert.GetDate(o["HolidayDate"]) == day.Date).Any();
                // 是行事曆設定假日 or 星期天
                if (IsHoliday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    IsHoliday = true;

                    // 為避免假日推移的影響，讓時間軸不夠長，因此每遇到一次假日，就要加長一次時間軸
                    maxLeadTime_u++;
                    start = start.AddDays(-1);

                    cmd2 = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{start_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{this.FtyFroup.JoinToString("','")}')
";
                    DBProxy.Current.Select(null, cmd2, out dt2);
                }

                day.IsHoliday = IsHoliday;
                this.Days.Add(day);
            }
            #endregion

            #region 找出 (end) ~ (end_where) 時間軸內，所有的假日
            cmd2 = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{end.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{end_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{this.FtyFroup.JoinToString("','")}')
";
            result = DBProxy.Current.Select(null, cmd2, out dt2);
            DateTime? lastprocessday = null;
            for (int Day = 0; Day <= minLeadTime_u; Day++)
            {
                PublicPrg.Prgs.Day day = new PublicPrg.Prgs.Day();
                day.Date = end_where.AddDays(-Day);

                // 是否行事曆設定假日
                bool IsHoliday = dt2.AsEnumerable().Where(o => MyUtility.Convert.GetDate(o["HolidayDate"]) == day.Date).Any();
                // 是行事曆設定假日 or 星期天
                if (IsHoliday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    IsHoliday = true;

                    // 為避免假日推移的影響，讓時間軸不夠長，因此每遇到一次假日，就要加長一次時間軸
                    minLeadTime_u++;
                    end = end.AddDays(-1);

                    cmd2 = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{end.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{end_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{this.FtyFroup.JoinToString("','")}')
";
                    DBProxy.Current.Select(null, cmd2, out dt2);
                }

                day.IsHoliday = IsHoliday;
                this.Days.Add(day);
                lastprocessday = day.Date;
            }

            this.Days = this.Days.Where(w => w.Date <= lastprocessday)
                .Select(s => new { s.Date, s.IsHoliday }).Distinct()
                .Select(s => new PublicPrg.Prgs.Day { Date = s.Date, IsHoliday = s.IsHoliday })
                .OrderBy(o => o.Date).ToList();
            #endregion
            #endregion

            List<string> allOrder = dt.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            this.AllData = GetInOffLineList(dt, this.Days);

            List<DataTable> LeadTimeList = PublicPrg.Prgs.GetCutting_WIP_DataTable(this.Days, this.AllData);

            this.summaryData = LeadTimeList[0];
            this.detailData = LeadTimeList[1];
            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.summaryData == null || this.detailData == null || this.summaryData.Rows.Count == 0 || this.detailData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R01.xltx"); //預先開啟excel app
            objApp.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet Summary_Sheet = objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet Detail_Sheet = objApp.ActiveWorkbook.Worksheets[2];

            #region 產生橫向日期表格

            //扣除無產出的日期
            List<PublicPrg.Prgs.Day> removeDays = new List<PublicPrg.Prgs.Day>();

            foreach (var day in this.Days)
            {
                //如果該日期，不是「有資料」，則刪掉
                if (!this.AllData.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                ).Any()
                                       ).Any() 
                                       //&& day.IsHoliday
                    )
                {
                    removeDays.Add(day);
                }
            }
            //this.Days.RemoveAll(o => removeDays.Where(x => x.Date == o.Date).Any());


            int dateCount = this.Days.Count;
            int orderCount = (this.summaryData.Rows.Count);
            string lastColumnName_detail = "";
            int dataCount = this.detailData.Rows.Count / 4;

            //Detail Sheet
            string columnName = "D";   // 預設值為D開始
            lastColumnName_detail = columnName;
            for (int i = 4; i <= (dateCount + 3) - 1; i++)  //注意 dateCount + i-1
            {
                // 從D開始貼
                columnName = MyExcelPrg.GetExcelColumnName(i);
                lastColumnName_detail = columnName;
            }
            // 選擇要被貼上的位置
            Microsoft.Office.Interop.Excel.Range PasteRange = Detail_Sheet.get_Range($"D1:{columnName}1", Type.Missing);
            // 選取要被複製的資料，然後貼上
            PasteRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, Detail_Sheet.get_Range("C1", "C5").Copy(Type.Missing));

            // Summary Sheet
            string lastColumnName_summary = "";
            columnName = "C";  // 預設值為C開始
            lastColumnName_summary = columnName;
            for (int i = 3; i <= (dateCount + 2) - 1; i++) //注意 dateCount + i-1
            {
                // 從C開始貼
                columnName = MyExcelPrg.GetExcelColumnName(i);
                lastColumnName_summary = columnName;
            }
            // 選擇要被貼上的位置
            Microsoft.Office.Interop.Excel.Range PasteRange2 = Summary_Sheet.get_Range($"C1:{columnName}1", Type.Missing);

            // 選取要被複製的資料，然後貼上
            PasteRange2.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, Summary_Sheet.get_Range("B1", "B3").Copy(Type.Missing));

            #endregion

            #region 產生縱向SP#表格


            // Detail Sheet
            int rowIndex = (orderCount * 4) + 1;
            Microsoft.Office.Interop.Excel.Range PasteRange3 = Detail_Sheet.get_Range($"A6:A{rowIndex}", Type.Missing);
            // 貼上
            PasteRange3.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, Detail_Sheet.get_Range("A2", $"{lastColumnName_detail}5").Copy(Type.Missing));


            // Summary Sheet
            rowIndex = orderCount + 2;
            Microsoft.Office.Interop.Excel.Range PasteRange4 = Summary_Sheet.get_Range($"A4:A{rowIndex}", Type.Missing);
            // 貼上
            PasteRange4.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, Summary_Sheet.get_Range("A3", $"{lastColumnName_summary}3").Copy(Type.Missing));

            #endregion

            MyUtility.Excel.CopyToXls(this.detailData, "", "Cutting_R01.xltx", 1, false, null, objApp, false, Detail_Sheet);// 將datatable copy to excel
            MyUtility.Excel.CopyToXls(this.summaryData, "", "Cutting_R01.xltx", 2, false, null, objApp, false, Summary_Sheet);// 將datatable copy to excel

            #region 寫入橫向列的日期
            int ColumnIndex = 3;
            foreach (var day in this.Days)
            {
                string dateStr = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                Detail_Sheet.Cells[1, ColumnIndex] = dateStr;
                // 假日的話粉紅色
                //if (removeDays.Where(o => (o.Date.ToString("MM/dd") + $"({o.Date.DayOfWeek.ToString().Substring(0, 3)}.)") == dateStr && o.IsHoliday).Any())
                //{
                //    Detail_Sheet.Cells[1, ColumnIndex].Interior.ColorIndex = 38;
                //}
                if (day.IsHoliday)
                {
                    Detail_Sheet.Cells[1, ColumnIndex].Interior.ColorIndex = 38;
                }
                ColumnIndex++;
            }
            string colName = MyExcelPrg.GetExcelColumnName(ColumnIndex - 1); // 因為最後一個也有遞增，因此會多加一次，扣回來
            Detail_Sheet.get_Range($"A:{colName}").Columns.AutoFit();
            ColumnIndex = 3;
            int deleteCount = 0;
            // 刪除
            foreach (var day in this.Days)
            {
                if (removeDays.Where(o => o.Date == day.Date).Any())
                {
                    string str = MyExcelPrg.GetExcelColumnName(ColumnIndex - deleteCount);
                    Detail_Sheet.get_Range($"{str}:{str}").EntireColumn.Delete();
                    deleteCount++;
                }
                ColumnIndex++;
            }

            ColumnIndex = 2;
            foreach (var day in this.Days)
            {
                string dateStr = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                Summary_Sheet.Cells[2, ColumnIndex] = dateStr;
                // 假日的話粉紅色
                //if (removeDays.Where(o => (o.Date.ToString("MM/dd") + $"({o.Date.DayOfWeek.ToString().Substring(0, 3)}.)") == dateStr && o.IsHoliday).Any())
                //{
                //    Summary_Sheet.Cells[2, ColumnIndex].Interior.ColorIndex = 38;
                //}

                if (day.IsHoliday)
                {
                    Summary_Sheet.Cells[2, ColumnIndex].Interior.ColorIndex = 38;
                }
                ColumnIndex++;
            }
            colName = MyExcelPrg.GetExcelColumnName(ColumnIndex - 1);
            Summary_Sheet.get_Range($"A1:{colName}1").Merge();
            Summary_Sheet.get_Range($"A:{colName}").Columns.AutoFit();

            ColumnIndex = 2;
            deleteCount = 0;
            foreach (var day in this.Days)
            {
                if (removeDays.Where(o => o.Date == day.Date).Any())
                {
                    string str = MyExcelPrg.GetExcelColumnName(ColumnIndex - deleteCount);
                    Summary_Sheet.get_Range($"{str}:{str}").EntireColumn.Delete();
                    deleteCount++;
                }
                ColumnIndex++;
            }
            #endregion

            #region Save Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_R01");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();

            return true;
        }

        public bool Check_Subprocess_LeadTime()
        {
            DataTable PoID_dt;
            DataTable GarmentTb;
            DataTable LeadTime_dt;
            DualResult result;

            string cmd = $@"

SELECT  DISTINCT OrderID, s.MDivisionID, s.FactoryID
INTO #OrderList
FROM SewingSchedule s WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID
WHERE o.LocalOrder = 0
AND ( 
	(Cast(s.Inline as Date) >= '{SewingDate_s.Value.ToString("yyyy/MM/dd HH:mm:ss")}' AND Cast( s.Inline as Date) <= '{SewingDate_e.Value.ToString("yyyy/MM/dd HH:mm:ss")}' )
	OR
	(Cast(s.Offline as Date) >= '{SewingDate_s.Value.ToString("yyyy/MM/dd HH:mm:ss")}' AND Cast( s.Offline as Date) <= '{SewingDate_e.Value.ToString("yyyy/MM/dd HH:mm:ss")}' )
)
";
            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                cmd += Environment.NewLine + $@"AND s.MDivisionID='{this.MDivisionID}'";
            }
            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                cmd += Environment.NewLine + $@"AND s.FactoryID='{this.FactoryID}'";
            }

            cmd += $@"

SELECT DIStINCT  b.POID ,a.OrderID ,b.FtyGroup, a.MDivisionID, a.FactoryID
FROM #OrderList a
INNER JOIN Orders b ON a.OrderID= b.ID 

drop table #OrderList
";
            result = DBProxy.Current.Select(null, cmd, out PoID_dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            List<string> PoID_List = PoID_dt.AsEnumerable().Select(o => o["POID"].ToString()).Distinct().ToList();
            this.FtyFroup = PoID_dt.AsEnumerable().Select(o => o["FtyGroup"].ToString()).Distinct().ToList();
            List<string> Msg = new List<string>();


            foreach (DataRow dr in PoID_dt.Rows)
            {
                string POID = dr["POID"].ToString();
                string OrderID = dr["OrderID"].ToString();
                string MDivisionID = dr["MDivisionID"].ToString();
                string FactoryID = dr["FactoryID"].ToString();

                PublicPrg.Prgs.GetGarmentListTable(string.Empty, POID, "", out GarmentTb);

                List<string> AnnotationList = GarmentTb.AsEnumerable().Where(o => !MyUtility.Check.Empty(o["Annotation"].ToString())).Select(o => o["Annotation"].ToString()).Distinct().ToList();


                List<string> AnnotationList_Final = new List<string>();

                foreach (var Annotation in AnnotationList)
                {
                    foreach (var item in Annotation.Split('+'))
                    {
                        string input = "";
                        for (int i = 0; i <= item.Length - 1; i++)
                        {
                            // 排除掉數字
                            int x = 0;
                            if (!int.TryParse(item[i].ToString(), out x))
                            {
                                input += item[i].ToString();
                            }
                        }
                        if (!AnnotationList_Final.Contains(input) && MyUtility.Check.Seek($"SELECT 1 FROM Subprocess WHERE ID='{input}' "))
                        {
                            AnnotationList_Final.Add(input);
                        }
                    }
                }

                string AnnotationStr = AnnotationList_Final.OrderBy(o => o.ToString()).JoinToString("+");

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
WHERE Subprocess.IDs = '{AnnotationStr}'
and s.MDivisionID = '{MDivisionID}'
and s.FactoryID = '{FactoryID}'
";
                result = DBProxy.Current.Select(null, chk_LeadTime, out LeadTime_dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                // 收集需要顯示訊息的Subprocess ID
                if (LeadTime_dt.Rows.Count == 0 && AnnotationStr != string.Empty)
                {
                    Msg.Add(MDivisionID + ";" + FactoryID + ";" + AnnotationStr);
                }
                else
                {
                    // 記錄下加工段的Lead Time
                    LeadTime o = new LeadTime()
                    {
                        OrderID = OrderID,
                        LeadTimeDay = MyUtility.Check.Empty(AnnotationStr) ? 0 : Convert.ToInt32(LeadTime_dt.Rows[0]["LeadTime"]) //加工段為空，LeadTimeDay = 0
                    };
                    this.LeadTimeList.Add(o);
                }
            }

            if (Msg.Count > 0)
            {
                string message = "<" + Msg.Distinct().OrderBy(o => o).JoinToString(">" + Environment.NewLine + "<") + ">";
                message = message.Replace(";", "><");
                message += Environment.NewLine + @"Please set cutting lead time in [Cutting_B09. Subprocess Lead Time].
When the settings are complete, can be export excel!
";

                MyUtility.Msg.InfoBox(message);
                return false;
            }
            return true;
        }
    }
}