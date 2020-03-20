using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

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

            if (!(SewingDate.Value1.HasValue || SewingDate.Value2.HasValue))
            {
                MyUtility.Msg.InfoBox("<Sewing Date> can’t be empty!!");
                return false;
            }


            this.MDivisionID = this.txtMdivision.Text;
            this.FactoryID = this.txtfactory.Text;
            this.SewingDate_s = SewingDate.Value1.Value;
            this.SewingDate_e = SewingDate.Value2.Value;

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

            #region 所有Order ID、應扣去的Lead Time列表
            // this.LeadTimeList
            #endregion


            #region 所有Order ID、對應的所有Inline/Offline

            cmd = $@"
SELECT *
FROM SewingSchedule
WHERE Inline >= '{SewingDate_s.Value.ToString("yyyy/MM/dd")}'
AND Offline <= '{SewingDate_e.Value.ToString("yyyy/MM/dd")}' 
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

            //取出最早InLine / 最晚OffLine，先存下來待會用
            this.MinInLine = dt.AsEnumerable().Min(o => Convert.ToDateTime(o["Inline"]));
            this.MaxOffLine = dt.AsEnumerable().Max(o => Convert.ToDateTime(o["offline"]));

            foreach (string OrderID in dt.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct())
            {
                InOffLineList nOnj = new InOffLineList();
                nOnj.OrderID = OrderID;
                nOnj.InOffLines = new List<InOffLine>();

                if (OrderID == "20012166GG001")
                {

                }

                //取得該訂單的組成
                #region 取得該訂單的組成
                DataTable tmpDt;
                string tmpCmd = $@"
SELECT DISTINCT 
    [OrderID]=o.ID
    ,oq.Article
    ,oq.SizeCode
    ,occ.PatternPanel
    ,cons.FabricPanelCode
FROM Orders o WITH (NOLOCK)
INNER JOIN Order_qty oq ON o.ID=oq.ID
INNER JOIN Order_ColorCombo occ ON o.poid = occ.id AND occ.Article = oq.Article
INNER JOIN order_Eachcons cons ON occ.id = cons.id AND cons.FabricCombo = occ.PatternPanel AND cons.CuttingPiece='0'
WHERE occ.FabricCode !='' AND occ.FabricCode IS NOT NULL
AND o.id = '{OrderID}' 
";

                result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);

                List<GarmentList> GarmentListList = new List<GarmentList>();
                foreach (var Key in tmpDt.AsEnumerable().Select(o=>new
                            {
                                OrderID =o["OrderID"].ToString(),
                                Article = o["Article"].ToString(),
                                SizeCode = o["SizeCode"].ToString()
                            }).Distinct()
                        )
                {
                    GarmentList obj = new GarmentList()
                    {
                        OrderID = Key.OrderID,
                        Article = Key.Article,
                        SizeCode = Key.SizeCode,
                    };

                    obj.Panels = new List<Panel>();
                    var detail = tmpDt.AsEnumerable()
                        .Where(o => o["OrderID"].ToString() == Key.OrderID
                            && o["Article"].ToString() == Key.Article
                            && o["SizeCode"].ToString() == Key.SizeCode)
                        .Select(o =>  o["PatternPanel"].ToString()).Distinct().ToList();

                    // Panel
                    foreach (var PatternPanel in detail)
                    {
                        Panel panel = new Panel() { PatternPanel = PatternPanel };
                        List<DataRow> FabricPanelCodes = tmpDt.AsEnumerable()
                         .Where(o => o["OrderID"].ToString() == Key.OrderID
                             && o["Article"].ToString() == Key.Article
                             && o["SizeCode"].ToString() == Key.SizeCode
                             && o["PatternPanel"].ToString() == PatternPanel).ToList();
                        panel.FabricPanelCodes = new List<PanelCode>();
                        foreach (var row in FabricPanelCodes)
                        {
                            PanelCode code = new PanelCode() { FabricPanelCode = row["FabricPanelCode"].ToString() };
                            panel.FabricPanelCodes.Add(code);
                        }
                        obj.Panels.Add(panel);
                    }

                    GarmentListList.Add(obj);
                }
                #endregion

                // 取得各部位Cutting 數量


                //List<GarmentList> GarmentListList = ConvertToClassList<GarmentList>(tmpDt).ToList();

                foreach (DataRow dr in dt.AsEnumerable().Where(o => o["OrderID"].ToString() == OrderID))
                {
                    string ApsNO = dr["APSNo"].ToString();
                    int LeadTime = this.LeadTimeList.Where(o => o.OrderID == OrderID).FirstOrDefault().LeadTimeDay;

                    string StdQty = MyUtility.GetValue.Lookup($"SELECT StdQ FROM [dbo].[getDailystdq]('{ApsNO}') WHERE Date = '{Convert.ToDateTime(dr["Inline"]).ToString("yyyy/MM/dd")}'");
                    string AccuStdQty = MyUtility.GetValue.Lookup($"SELECT SUM(StdQ) FROM [dbo].[getDailystdq]('{ApsNO}') WHERE Date < '{Convert.ToDateTime(dr["Inline"]).ToString("yyyy/MM/dd")}'");


                    InOffLine nLineObj = new InOffLine()
                    {
                        ApsNO = ApsNO,
                        InLine = Convert.ToDateTime(dr["Inline"]).AddDays((-1* LeadTime)),
                        OffLine = Convert.ToDateTime(dr["Offline"]).AddDays((-1 * LeadTime)),

                        StdQty = MyUtility.Check.Empty(StdQty) ? 0 : Convert.ToInt32(StdQty),
                        AccuStdQty = MyUtility.Check.Empty(AccuStdQty) ? 0 : Convert.ToInt32(AccuStdQty),
                    };

                    nOnj.InOffLines.Add(nLineObj);
                }
                AllData.Add(nOnj);
            }

            #endregion

            #region 處理橫向日期的時間軸

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

            cmd = $@"
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

            result = DBProxy.Current.Select(null, cmd, out dt);

            // 開始組合時間軸

            for (int Day = 0; Day <= DayCount -1; Day++)
            {
                Day day = new Day();
                day.Date = start.AddDays(Day);
                bool IsHoliday = false;

                if (dt.Rows.Count > 0)
                {
                    IsHoliday = dt.AsEnumerable().Where(o => Convert.ToDateTime(o["HolidayDate"]) == day.Date).Any();
                }

                day.IsHoliday = IsHoliday;
                this.Days.Add(day);
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
WHERE Inline >= '{SewingDate_s.Value.ToString("yyyy/MM/dd")}'
AND Offline <= '{SewingDate_e.Value.ToString("yyyy/MM/dd")}' 
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
                if (LeadTime_dt.Rows.Count == 0)
                {
                    Msg.Add(AnnotationStr);
                }
                else
                {
                    // 記錄下加工段的Lead Time
                    LeadTime o = new LeadTime()
                    {
                        OrderID = OrderID,
                        LeadTimeDay = Convert.ToInt32(LeadTime_dt.Rows[0]["LeadTime"])
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
            public int StdQty { get; set; }
            public int AccuStdQty { get; set; }
            public DateTime? InLine { get; set; }
            public DateTime? OffLine { get; set; }
        }
        private class Day
        {
            //public string FactoryID { get; set; }
            public DateTime Date { get; set; }
            public bool IsHoliday { get; set; }
        }
        
        /// <summary>
        /// 一件成衣，由哪些部位組成
        /// </summary>
        private class GarmentList
        {
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
        }
    }
}
