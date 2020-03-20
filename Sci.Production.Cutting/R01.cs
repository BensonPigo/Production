using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        DateTime? SewingDate_s, SewingDate_e;
        DateTime MinInLine, MaxOffLine;
        List<string> FtyFroup = new List<string>();
        List<InOffLineList> AllData = new List<InOffLineList>();
        List<Day> Days = new List<Day>();
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
            this.AllData.Clear();
            this.FtyFroup.Clear();
            this.Days.Clear();

            if (!(SewingDate.Value1.HasValue || SewingDate.Value2.HasValue))
            {
                MyUtility.Msg.InfoBox("<Sewing Date> can’t be empty!!");
                return false;
            }


            this.MDivisionID = this.txtMdivision.Text;
            this.FactoryID = this.txtfactory.Text;
            this.SewingDate_s = SewingDate.Value1.Value;
            this.SewingDate_e = SewingDate.Value2.Value.AddDays(1).AddSeconds(-1);

            if (!Check_Subprocess_LeadTime())
            {
                return false;
            }

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
FROM SewingSchedule
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
            int DayCount = Math.Abs(ts.Days);

            // 找出時間軸內，所有的假日
            DataTable dt2;
            string cmd2 = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM
(
	SElECt * 
	FROM Holiday
	WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
)a
WHERE HolidayDate <= '{end.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{this.FtyFroup.JoinToString("','")}')
";

            result = DBProxy.Current.Select(null, cmd2, out dt2);

            // 開始組合時間軸

            for (int Day = 0; Day <= DayCount - 1; Day++)
            {
                Day day = new Day();
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

            #region Order ID、對應每一天的資料

            List<string> allOrder = dt.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            // 取得訂單所有成套的裁剪數量 (已去除部位缺少的)
            List<GarmentList> GarmentListList = PublicPrg.Prgs.GetCut(allOrder);

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

                if (OrderID == "20040282GG")
                {

                }

                // 所有Order ID、以及相對應 要扣去的Lead Time
                int LeadTime = this.LeadTimeList.Where(o => o.OrderID == OrderID).FirstOrDefault().LeadTimeDay;

                foreach (DataRow dr in sameOrderId)
                {
                    string ApsNO = dr["APSNo"].ToString();
                    // 
                    foreach (Day day in this.Days)
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
                            int Cutqty = GarmentListList.Where(o =>o.OrderID == OrderID && o.EstCutDate == day.Date)
                                                        .Sum(o => o.Panels
                                                            .Sum(x => x.FabricPanelCodes
                                                                .Min(y => y.Qty)));
                            int accuCutQty = GarmentListList.Where(o => o.OrderID == OrderID && DateTime.Compare(o.EstCutDate, day.Date) <= 0)
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
                AllData.Add(nOnj);
            }

            #endregion


            return base.OnAsyncDataLoad(e);
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
FROM SewingSchedule
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

                if (OrderID == "20012166GG001")
                {

                }

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
                            if (!int.TryParse(item[i].ToString(), out int x))
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
                ,LeadTime=(SELECt LeadTime FROM SubprocessLeadTime WHERE ID = sd.ID)
FROM SubprocessLeadTime_Detail SD
OUTER APPLY(
	SELECT IDs=STUFF(
	 (
		SELECT '+'+SubprocessID
		FROM SubprocessLeadTime_Detail
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
                        LeadTimeDay =MyUtility.Check.Empty(AnnotationStr) ? 0 : Convert.ToInt32(LeadTime_dt.Rows[0]["LeadTime"]) //加工段為空，LeadTimeDay = 0
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

        /*
        /// <summary>
        /// DataTable 轉換成自行定義的類別集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> ConvertToClassList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            //取得DataTable所有的row data
            foreach (var row in table.Rows)
            {
                var item = MappingItem<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T MappingItem<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (row.Table.Columns.Contains(property.Name))
                {
                    //針對欄位的型態去轉換
                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime dt = new DateTime();
                        if (DateTime.TryParse(row[property.Name].ToString(), out dt))
                        {
                            property.SetValue(item, dt, null);
                        }
                        else
                        {
                            property.SetValue(item, null, null);
                        }
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        decimal val = new decimal();
                        decimal.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(double))
                    {
                        double val = new double();
                        double.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        int val = new int();
                        int.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        string val = row[property.Name].ToString();
                        property.SetValue(item, val, null);
                    }
                    else
                    {
                        if (row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(item, row[property.Name], null);
                        }
                    }
                }
            }
            return item;
        }
        */
        private class LeadTime
        {
            public string OrderID { get; set; }
            public int LeadTimeDay { get; set; }
        }
        private class InOffLineList
        {
            public string OrderID { get; set; }
            public List<InOffLine> InOffLines { get; set; }
        }
        private class InOffLine
        {
            public string ApsNO { get; set; }
            public int CutQty { get; set; }
            public int AccuCutQty { get; set; }
            public int StdQty { get; set; }
            public int AccuStdQty { get; set; }
            //public DateTime? InLine { get; set; }
            //public DateTime? OffLine { get; set; }
            public DateTime DateWithLeadTime { get; set; }
        }
        private class Day
        {
            //public string FactoryID { get; set; }
            public DateTime Date { get; set; }
            public bool IsHoliday { get; set; }
        }

        /*
        /// <summary>
        /// 一件成衣，由哪些部位組成
        /// </summary>
        private class GarmentList
        {
            public DateTime? EstCutDate { get; set; }
            // 是否缺部位，因此不成套
            public bool IsPanelShortage { get; set; }
            public string OrderID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public List<Panel> Panels { get; set; }
        }

        /// <summary>
        /// 大部位名
        /// </summary>
        private class Panel
        {
            /// <summary>
            /// 大部位
            /// </summary>
            public string PatternPanel { get; set; }

            /// <summary>
            /// 該大部位內的小部位
            /// </summary>
            public List<PanelCode> FabricPanelCodes { get; set; }
        }
        private class PanelCode
        {
            public string FabricPanelCode { get; set; }
            public int Qty { get; set; }
        }
        */
    }
}
