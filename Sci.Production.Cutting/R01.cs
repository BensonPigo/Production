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
FROM SewingSchedule WITH(NOLOCK)
WHERE Inline >= '{SewingDate_s.Value.ToString("yyyy/MM/dd HH:mm:ss")}'
AND Offline <= '{SewingDate_e.Value.ToString("yyyy/MM/dd HH:mm:ss")}' 
";
            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                cmd += Environment.NewLine + $@"AND MDivisionID='{this.MDivisionID}'";
            }
            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                cmd += Environment.NewLine + $@"AND FactoryID='{this.FactoryID}'";
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

            // 起點 = (最早Inline - 最大Lead Time)、終點 = (最晚Offline - 最小Lead Time)
            DateTime start = Convert.ToDateTime(this.MinInLine.AddDays((-1 * maxLeadTime)).ToString("yyyy/MM/dd"));
            DateTime end = Convert.ToDateTime(this.MaxOffLine.AddDays((-1 * minLeadTime)).ToString("yyyy/MM/dd"));

            // 算出總天數
            TimeSpan ts = end - start;
            int DayCount = Math.Abs(ts.Days) + 1;

            // 找出時間軸內，所有的假日
            DataTable dt2;
            string cmd2 = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM
(
	SElECt * 
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
)a
WHERE HolidayDate <= '{end.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{this.FtyFroup.JoinToString("','")}')
";

            result = DBProxy.Current.Select(null, cmd2, out dt2);

            // 開始組合時間軸

            for (int Day = 0; Day <= DayCount - 1; Day++)
            {
                PublicPrg.Prgs.Day day = new PublicPrg.Prgs.Day();
                day.Date = start.AddDays(Day);
                bool IsHoliday = false;

                // 假日或國定假日要註記
                if (dt2.Rows.Count > 0)
                {
                    IsHoliday = dt2.AsEnumerable().Where(o => Convert.ToDateTime(o["HolidayDate"]) == day.Date).Any();
                }
                if (day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    IsHoliday = true;
                }

                day.IsHoliday = IsHoliday;
                this.Days.Add(day);
            }

            #endregion
            List<string> allOrder = dt.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            #region Note 測試完成後移除
            /*
            #region Order ID、對應每一天的資料


            // 取得訂單所有成套的裁剪數量 (已去除部位缺少的)
            List<GarmentList> GarmentListList = PublicPrg.Prgs.GetCutPlanQty(allOrder);

            foreach (string OrderID in allOrder)
            {
                var sameOrderId = dt.AsEnumerable().Where(o => o["OrderID"].ToString() == OrderID);

                // 這筆訂單的起始與結束時間
                DateTime Start = sameOrderId.Min(o => Convert.ToDateTime(o["Inline"]));
                DateTime End = sameOrderId.Max(o => Convert.ToDateTime(o["offline"]));


                InOffLineList nOnj = new InOffLineList();
                // SP#
                nOnj.OrderID = OrderID;
                nOnj.InOffLines = new List<InOffLine>();

                // 所有Order ID、以及相對應 要扣去的Lead Time
                int LeadTime = this.LeadTimeList.Where(o => o.OrderID == OrderID).FirstOrDefault().LeadTimeDay;

                foreach (DataRow dr in sameOrderId)
                {
                    string ApsNO = dr["APSNo"].ToString();
                    // 
                    foreach (PublicPrg.Prgs.Day day in this.Days)
                    {
                        // 比Inline晚
                        bool Later_ThanInline = DateTime.Compare(day.Date, Convert.ToDateTime(dr["Inline"]).Date.AddDays((-1 * LeadTime))) >= 0;
                        // 比Offline早
                        bool Eaelier_ThanInline = DateTime.Compare(Convert.ToDateTime(dr["Offline"]).Date.AddDays((-1 * LeadTime)), day.Date) >= 0;

                        if (Later_ThanInline && Eaelier_ThanInline)
                        {
                            string StdQty = MyUtility.GetValue.Lookup($"SELECT StdQ FROM [dbo].[getDailystdq]('{ApsNO}') WHERE Date = '{day.Date.AddDays(LeadTime).ToString("yyyy/MM/dd")}'");
                            string AccuStdQty = MyUtility.GetValue.Lookup($"SELECT SUM(StdQ) FROM [dbo].[getDailystdq]('{ApsNO}') WHERE Date <= '{day.Date.AddDays(LeadTime).ToString("yyyy/MM/dd")}'");

                            // 取最小
                            int Cutqty = GarmentListList.Where(o => o.OrderID == OrderID && o.EstCutDate == day.Date.Date.AddDays((-1 * LeadTime)))
                                                        .Sum(o => o.Panels
                                                            .Sum(x => x.FabricPanelCodes
                                                                .Min(y => y.Qty)));
                            int accuCutQty = GarmentListList.Where(o => o.OrderID == OrderID && DateTime.Compare(o.EstCutDate, day.Date.Date.AddDays((-1 * LeadTime))) <= 0)
                                                        .Sum(o => o.Panels
                                                            .Sum(x => x.FabricPanelCodes
                                                                .Min(y => y.Qty)));

                            InOffLine nLineObj = new InOffLine()
                            {
                                DateWithLeadTime = day.Date,
                                ApsNO = ApsNO,
                                CutQty = Cutqty,
                                AccuCutQty = accuCutQty,
                                StdQty = MyUtility.Check.Empty(StdQty) ? 0 : Convert.ToInt32(StdQty),
                                AccuStdQty = MyUtility.Check.Empty(AccuStdQty) ? 0 : Convert.ToInt32(AccuStdQty),
                            };
                            nOnj.InOffLines.Add(nLineObj);
                        }
                    }
                }
                AllDataTmp.Add(nOnj);
            }

            #endregion


            #region 將資料轉換成DataTable

            DataTable detailDt = new DataTable();
            DataTable summaryDt = new DataTable();

            detailDt.ColumnsStringAdd("SP");
            detailDt.ColumnsStringAdd("Desc./Sewing Date");
            summaryDt.ColumnsStringAdd("SP");

            int idx = 3;
            foreach (var day in this.Days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                if (day.IsHoliday)
                {
                    string columnName = MyExcelPrg.GetExcelColumnName(idx);
                    // 儲存格設定填充粉紅色  http://yuchicsharplearn.blogspot.com/2016/05/c-excel_30.html
                    //Detail_Sheet.get_Range($"{columnName}1").Interior.ColorIndex = 38;
                }
                idx++;
            }
            int orderCount = this.AllDataTmp.Count;

            for (int i = 0; i <= orderCount - 1; i++)
            {
                DataRow dr1 = detailDt.NewRow();
                DataRow dr2 = detailDt.NewRow();
                DataRow dr3 = detailDt.NewRow();
                DataRow dr4 = detailDt.NewRow();

                // 固定純文字
                dr1["Desc./Sewing Date"] = "Cut Plan Qty";
                dr2["Desc./Sewing Date"] = "Std. Qty";
                dr3["Desc./Sewing Date"] = "Accu. Cut Plan Qty";
                dr4["Desc./Sewing Date"] = "Accu. Std. Qty";

                detailDt.Rows.Add(dr1);
                detailDt.Rows.Add(dr2);
                detailDt.Rows.Add(dr3);
                detailDt.Rows.Add(dr4);
            }

            for (int i = 0; i <= orderCount - 1; i++)
            {
                DataRow dr = summaryDt.NewRow();
                summaryDt.Rows.Add(dr);
            }


            // 相同日期GROUP BY
            foreach (var BySP in AllDataTmp)
            {
                InOffLineList n = new InOffLineList();
                n.OrderID = BySP.OrderID;
                n.InOffLines = new List<InOffLine>();
                var groupData = BySP.InOffLines.GroupBy(o => new { o.DateWithLeadTime }).Select(x => new InOffLine
                {
                    DateWithLeadTime = x.Key.DateWithLeadTime,
                    CutQty = x.Sum(o => o.CutQty),
                    StdQty = x.Sum(o => o.StdQty),
                    AccuCutQty = x.Sum(o => o.AccuCutQty),
                    AccuStdQty = x.Sum(o => o.AccuStdQty)
                }).ToList();

                n.InOffLines = groupData;
                this.AllData.Add(n);
            }

            int index = 0;
            foreach (var BySP in AllData)
            {
                foreach (var item in BySP.InOffLines)
                {
                    foreach (var day in this.Days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        detailDt.Rows[(index * 4)]["SP"] = BySP.OrderID;
                        detailDt.Rows[(index * 4) + 1]["SP"] = BySP.OrderID;
                        detailDt.Rows[(index * 4) + 2]["SP"] = BySP.OrderID;
                        detailDt.Rows[(index * 4) + 3]["SP"] = BySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            detailDt.Rows[(index * 4)][strDate] = item.CutQty;
                            detailDt.Rows[(index * 4) + 1][strDate] = item.StdQty;
                            detailDt.Rows[(index * 4) + 2][strDate] = item.AccuCutQty;
                            detailDt.Rows[(index * 4) + 3][strDate] = item.AccuStdQty;
                        }
                        else
                        {
                            detailDt.Rows[(index * 4)][strDate] = DBNull.Value;
                            detailDt.Rows[(index * 4) + 1][strDate] = DBNull.Value;
                            detailDt.Rows[(index * 4) + 2][strDate] = DBNull.Value;
                            detailDt.Rows[(index * 4) + 3][strDate] = DBNull.Value;
                        }
                    }
                }
                index++;
            }
            this.detailData = detailDt;

            index = 0;
            foreach (var BySP in AllData)
            {
                foreach (var item in BySP.InOffLines)
                {
                    int DayIndex = 0;
                    foreach (var day in this.Days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[(index)]["SP"] = BySP.OrderID;
                        //detailDt.Rows[(index * 4) + 1]["SP"] = BySP.OrderID;
                        //detailDt.Rows[(index * 4) + 2]["SP"] = BySP.OrderID;
                        //detailDt.Rows[(index * 4) + 3]["SP"] = BySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            int CutQty = item.CutQty;
                            int StdQty = item.StdQty;
                            int AccuCutQty = item.AccuCutQty;
                            int AccuStdQty = item.AccuStdQty;
                            decimal cellValue = 0;
                            if (AccuCutQty <= AccuStdQty)
                            {
                                cellValue = StdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / StdQty;
                            }
                            else if (StdQty > 0 && ((AccuCutQty - AccuStdQty) / StdQty) <= 1)
                            {
                                if (this.Days.Count - 1 < (DayIndex + 1))
                                {
                                    cellValue = 0;
                                    continue;
                                }
                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == this.Days[DayIndex + 1].Date);
                                bool hasNextDayData = findData_nextDay.Any();
                                // 若沒有下一天的資料，則全部視作0 （下一天不一定是明天日期）
                                if (!hasNextDayData)
                                {
                                    cellValue = 0;
                                }
                                else
                                {
                                    int NextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                    cellValue = NextDayStdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / NextDayStdQty;
                                }
                            }
                            else
                            {
                                if (this.Days.Count - 1 < (DayIndex + 1) || this.Days.Count - 1 < (DayIndex + 2))
                                {
                                    cellValue = 0;
                                    continue;
                                }
                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == this.Days[DayIndex + 1].Date);
                                var findData_nextNextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == this.Days[DayIndex + 2].Date);
                                bool hasNextDayData = findData_nextDay.Any();
                                bool hasNextNextDayData = findData_nextNextDay.Any();
                                // 若沒有下一天或下下一天的資料，則全部視作0
                                if (!hasNextNextDayData || !hasNextDayData)
                                {
                                    cellValue = 0;
                                }
                                else
                                {

                                    // 沒意外應該只有一筆，不過還是用SUM
                                    int NextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                    int NextNextDayStdQty = findData_nextNextDay.Sum(o => o.StdQty);

                                    cellValue = NextNextDayStdQty == 0 ? 0 : 1 + ((AccuCutQty - AccuStdQty - NextDayStdQty) / NextNextDayStdQty);
                                }
                            }
                            summaryDt.Rows[(index)][strDate] = cellValue;
                        }
                        else
                        {
                            summaryDt.Rows[(index)][strDate] = DBNull.Value;
                        }
                        DayIndex++;
                    }
                }
                index++;
            }
            this.summaryData = summaryDt;

            #endregion
            */
            #endregion

            this.AllData = GetInOffLineList(dt, this.Days);

            List<DataTable> LeadTimeList = PublicPrg.Prgs.GetCutting_WIP_DataTable(dt, this.Days, this.AllData);

            this.summaryData = LeadTimeList[0];
            this.detailData = LeadTimeList[1];
            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel processing...");
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_R01.xltx"); //預先開啟excel app
            //objApp.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet Summary_Sheet = objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet Detail_Sheet = objApp.ActiveWorkbook.Worksheets[2];

            #region 產生橫向日期表格

            //扣除無產出的日期
            List<PublicPrg.Prgs.Day> removeDays = new List<PublicPrg.Prgs.Day>();

            foreach (var day in this.Days)
            {
                //如果該日期，不是「有產出」，則刪掉
                if (!this.AllData.Where(x => x.InOffLines.Where(
                                                                    y => y.DateWithLeadTime == day.Date
                                                                ).Any()
                                       ).Any()
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
            string columnName = "";
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
            columnName = "";
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

SELECT  DISTINCT OrderID
INTO #OrderList
FROM SewingSchedule WITH(NOLOCK)
WHERE Inline >= '{SewingDate_s.Value.ToString("yyyy/MM/dd HH:mm:ss")}'
AND Offline <= '{SewingDate_e.Value.ToString("yyyy/MM/dd HH:mm:ss")}' 
";
            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                cmd += Environment.NewLine + $@"AND MDivisionID='{this.MDivisionID}'";
            }
            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                cmd += Environment.NewLine + $@"AND FactoryID='{this.FactoryID}'";
            }

            cmd += $@"
SELECT DIStINCT  b.POID ,a.OrderID ,b.FtyGroup
FROM #OrderList a
INNER JOIN Orders b ON a.OrderID= b.ID 
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
                ,LeadTime=(SELECt LeadTime FROM SubprocessLeadTime WITH(NOLOCK) WHERE ID = sd.ID)
FROM SubprocessLeadTime_Detail SD WITH(NOLOCK)
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
                    Msg.Add(AnnotationStr);
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
                string Message = "<" + Msg.Distinct().JoinToString(">" + Environment.NewLine + "<") + ">";
                Message += Environment.NewLine + @"Please set cutting lead time in [Cutting_B09. Subprocess Lead Time].
When the settings are complete, can be export excel!
";

                MyUtility.Msg.InfoBox(Message);
                return false;
            }
            return true;
        }
    }
}
