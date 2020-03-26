using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region BundleCardCheckSubprocess
        /// <summary>
        /// BundleCardCheckSubprocess(string[] ann, string patterncode,DataTable artTb, out bool lallpart)
        /// </summary>
        /// <param name="ann"></param>
        /// <param name="patterncode"></param>
        /// <param name="artTb"></param>
        /// <param name="lallpartas"></param>
        /// <returns>string</returns>
        public static string BundleCardCheckSubprocess(string[] ann, string patterncode, DataTable artTb, out bool lallpart)
        {
            //artTb 是給前Form 使用同Garment List 的PatternCode 與Subrpocess
            string art = "";
            lallpart = true; //是不是All part
            for (int i = 0; i < ann.Length; i++) //寫入判斷是否存在Subprocess
            {
                string[] ann2 = ann[i].ToString().Split(' '); //剖析Annotation
                if (ann2.Length > 0)
                {
                    #region 有分開字元需剖析
                    for (int j = 0; j < ann2.Length; j++)
                    {
                        if (MyUtility.Check.Seek(ann2[j], "subprocess", "Id"))
                        {
                            lallpart = false;
                            //Artwork 相同的也要顯示, ex: HT+HT
                            //if (art.IndexOf(ann2[j]) == -1)
                            //{
                            DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann2[j]));
                            if (existdr.Length == 0)
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann2[j];
                                artTb.Rows.Add(ndr_art);
                            }
                            if (art == "") art = ann2[j];
                            else art = art.Trim() + "+" + ann2[j];
                            //}
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 無分開字元
                    if (MyUtility.Check.Seek(ann[i], "subprocess", "Id"))
                    {
                        lallpart = false;
                        if (art.IndexOf(ann[i]) == -1)
                        {
                            DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann[i]));
                            if (existdr.Length == 0) //表示無在ArtTable 內
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann[i];
                                artTb.Rows.Add(ndr_art);
                            }
                            if (art == "") art = ann[i];
                            else art = art.Trim() + "+" + ann[i];
                        }
                    }
                    #endregion
                }
            }
            return art;
        }
        #endregion;

        /// <summary>
        /// 取得Cutting成套的數量
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static List<GarmentList> GetCutPlanQty(List<string> OrderIDs)
        {

            DataTable tmpDt;
            DualResult result;
            List<GarmentList> GarmentListList = new List<GarmentList>();

            //取得該訂單的組成
            #region 取得該訂單的組成
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
AND o.id IN ('{OrderIDs.JoinToString("','")}')
";

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);

            // 整理出表頭
            var keys = tmpDt.AsEnumerable().Select(o => new
            {
                OrderID = o["OrderID"].ToString(),
                Article = o["Article"].ToString(),
                SizeCode = o["SizeCode"].ToString()
            }).Distinct().ToList();

            foreach (var Key in keys)
            {
                GarmentList obj = new GarmentList()
                {
                    OrderID = Key.OrderID,
                    Article = Key.Article,
                    SizeCode = Key.SizeCode,
                };

                obj.Panels = new List<Panel>();

                // 相同表頭的資料
                var detail = tmpDt.AsEnumerable().Where(o => o["OrderID"].ToString() == Key.OrderID
                        && o["Article"].ToString() == Key.Article
                        && o["SizeCode"].ToString() == Key.SizeCode).ToList();

                var detail_PatternPanel_Keys = detail.Select(o => o["PatternPanel"].ToString()).Distinct().ToList();

                // Panel
                foreach (var PatternPanel in detail_PatternPanel_Keys)
                {
                    Panel panel = new Panel() { PatternPanel = PatternPanel };

                    List<DataRow> FabricPanelCodes = detail.Where(o => o["PatternPanel"].ToString() == PatternPanel).ToList();

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

            // 取得所有部位Cutting 數量
            tmpCmd = $@"

SELECT  [EstCutDate]=(SELECT EstCutDate FROM Cutplan WHERE ID = CD.ID AND Status='Confirmed')
,WOD.OrderID
,WOD.Article 
,WOD.SizeCode
,wo.FabricCombo
,wo.FabricPanelCode
,[Qty]=SUM(WOD.Qty)
FROM WorkOrder_Distribute WOD WITH(NOLOCK)
INNER JOIN WorkOrder WO WITH(NOLOCK) ON WO.Ukey = WOD.WorkOrderUkey
INNER JOIN Cutplan_Detail CD WITH(NOLOCK) ON CD.WorkorderUkey = WO.Ukey
WHERE WOD.OrderID IN ('{OrderIDs.JoinToString("','")}')
AND (SELECT EstCutDate FROM Cutplan WHERE ID = CD.ID AND Status='Confirmed') IS NOT NULL
GROUP BY CD.ID
		,WOD.OrderID
		,WOD.Article 
		,WOD.SizeCode
		,wo.FabricCombo
		,wo.FabricPanelCode
--ORDER BY WOD.OrderID,WOD.Article,wo.fabricCombo,wo.FabricPanelCode,WOD.SizeCode
";

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);

            foreach (var garment in GarmentListList)
            {
                if (garment.OrderID == "20022206GG001")
                {

                }
                var exists_1 = tmpDt.AsEnumerable().Where(o =>
                o["OrderID"].ToString() == garment.OrderID &&
                o["Article"].ToString() == garment.Article &&
                o["SizeCode"].ToString() == garment.SizeCode);

                foreach (var panel in garment.Panels)
                {
                    var exists_2 = exists_1.Where(o => o["FabricCombo"].ToString() == panel.PatternPanel);

                    foreach (var fabricPanelCode in panel.FabricPanelCodes)
                    {
                        var exists = exists_2.Where(o => o["FabricPanelCode"].ToString() == fabricPanelCode.FabricPanelCode);

                        // 任何一個部位不存在，則記錄下來
                        if (!exists.Any())
                        {
                            garment.IsPanelShortage = true;
                            //garment.EstCutDate = null;
                            //garment.EstCutDate = Convert.ToDateTime(exists.FirstOrDefault()["EstCutDate"]);
                            fabricPanelCode.Qty = 0;
                        }
                        else
                        {
                            garment.IsPanelShortage = false;
                            garment.EstCutDate = Convert.ToDateTime(exists.FirstOrDefault()["EstCutDate"]);
                            fabricPanelCode.Qty = Convert.ToInt32(exists.FirstOrDefault()["Qty"]);
                        }
                    }
                }
            }

            // 移除缺少部位不成套的的
            GarmentListList.RemoveAll(o => o.IsPanelShortage);


            //int CutQty = GarmentListList.Sum(o => o.Panels.Sum(x => x.FabricPanelCodes.Min(y => y.Qty)));

            return GarmentListList;
        }

        /// <summary>
        /// 取得所有In/Off Line資料 (成套數量內部自行處理)
        /// </summary>
        /// <param name="dt">SewingSchedule的Datatable</param>
        /// <param name="Days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <returns></returns>
        public static List<InOffLineList> GetInOffLineList(DataTable dt, List<Day> Days)
        {

            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList> AllDataTmp = new List<InOffLineList>();
            List<InOffLineList> AllData = new List<InOffLineList>();

            List<string> allOrder = dt.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            List<GarmentList> GarmentList = GetCutPlanQty(allOrder);
            List<LeadTime> LeadTimeList = GetLeadTimeList(allOrder);

            if (LeadTimeList == null)
            {
                // 表示Lead Time有缺
                return null;
            }
            Dictionary<string, int> accu = new Dictionary<string, int>();

            foreach (string OrderID in allOrder)
            {
                var sameOrderId = dt.AsEnumerable().Where(o => o["OrderID"].ToString() == OrderID);

                if (OrderID == "20032395GG010")
                {

                }
                // 這筆訂單的起始與結束時間
                DateTime Start = sameOrderId.Min(o => Convert.ToDateTime(o["Inline"]));
                DateTime End = sameOrderId.Max(o => Convert.ToDateTime(o["offline"]));


                InOffLineList nOnj = new InOffLineList();
                // SP#
                nOnj.OrderID = OrderID;
                nOnj.InOffLines = new List<InOffLine>();

                // 所有Order ID、以及相對應 要扣去的Lead Time
                int LeadTime = LeadTimeList.Where(o => o.OrderID == OrderID).FirstOrDefault().LeadTimeDay;


                foreach (DataRow dr in sameOrderId)
                {
                    string ApsNO = dr["APSNo"].ToString();
                    // 
                    foreach (Day day in Days)
                    {
                        // 比Inline晚
                        bool Later_ThanInline = DateTime.Compare(day.Date, Convert.ToDateTime(dr["Inline"]).Date.AddDays((-1 * LeadTime))) >= 0;
                        // 比Offline早
                        bool Eaelier_ThanInline = DateTime.Compare(Convert.ToDateTime(dr["Offline"]).Date.AddDays((-1 * LeadTime)), day.Date) >= 0;

                        if (Later_ThanInline && Eaelier_ThanInline)
                        {
                            string StdQty = MyUtility.GetValue.Lookup($"SELECT StdQ FROM [dbo].[getDailystdq]('{ApsNO}') WHERE Date = '{day.Date.AddDays(LeadTime).ToString("yyyy/MM/dd")}'");
                            string AccuStdQty = MyUtility.GetValue.Lookup($"SELECT SUM(StdQ) FROM [dbo].[getDailystdq]('{ApsNO}') WHERE Date <= '{day.Date.AddDays(LeadTime).ToString("yyyy/MM/dd")}'");

                            // 取裁剪數量
                            int Cutqty = 0;
                            var sameData = GarmentList.Where(o => o.OrderID == OrderID && o.EstCutDate == day.Date.Date);

                            foreach (var item in sameData)
                            {
                                int thisSize_Qty = 0;
                                foreach (var panel in item.Panels)
                                {
                                    thisSize_Qty = panel.FabricPanelCodes.Min(o => o.Qty);
                                }
                                Cutqty += thisSize_Qty;
                            }

                            // 取累計裁剪數量

                            if (!accu.Where(o => o.Key == OrderID).Any())
                            {
                                accu.Add(OrderID, Cutqty);
                            }
                            else
                            {
                                accu[OrderID] = accu[OrderID] + Cutqty;
                            }

                            int accuCutQty = accu[OrderID];

                            //if (accuDatas.Count == 0)
                            //{// 如果是第一天，累計數量 = 當天裁剪數量
                            //    accuCutQty = Cutqty;
                            //}
                            //else
                            //{
                            //    // 如果不是第一天，累計數量 = 當天裁剪數量 + 之前累計的數量
                            //    foreach (var accuData in accuDatas)
                            //    {
                            //        foreach (var InOffLine in accuData.InOffLines)
                            //        {
                            //            accuCutQty = InOffLine.AccuStdQty + Cutqty;
                            //        }
                            //    }
                            //}

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

            // 相同日期GROUP BY
            foreach (var BySP in AllDataTmp)
            {
                if (BySP.OrderID == "20060222GG")
                {

                }
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
                }).OrderBy(o => o.DateWithLeadTime).ToList();

                // 處理累計
                for (int i = 0; i <= groupData.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        // 當天累計裁減量 = 當天裁減 + 前一天累計裁減
                        groupData[i].AccuStdQty = groupData[i].CutQty + groupData[i - 1].AccuCutQty;

                        // 當天累計標準裁減量 = 當天標準 + 前一天累計標準
                        groupData[i].AccuStdQty = groupData[i].StdQty + groupData[i - 1].AccuStdQty;
                    }
                }

                n.InOffLines = groupData;
                AllData.Add(n);
            }

            return AllData;
        }

        /// <summary>
        /// Cutting WIP 資料表 (所有In/Off Line資料在內部自行取得) Index 0 為Summary、1 為Detail
        /// </summary>
        /// <param name="dt">SewingSchedule的Datatable</param>
        /// <param name="Days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <returns></returns>
        public static List<DataTable> GetCutting_WIP_DataTable(DataTable dt, List<Day> Days)
        {
            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList> AllData = new List<InOffLineList>();

            AllData = GetInOffLineList(dt, Days);

            DataTable detailDt = new DataTable();
            DataTable summaryDt = new DataTable();

            detailDt.ColumnsStringAdd("SP");
            detailDt.ColumnsStringAdd("Desc./Sewing Date");
            summaryDt.ColumnsStringAdd("SP");

            // 日期Column
            int idx = 3;
            foreach (var day in Days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                idx++;
            }

            int orderCount = AllData.Count;

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

            // 開始寫入Row
            int index = 0;
            foreach (var BySP in AllData)
            {
                foreach (var item in BySP.InOffLines)
                {
                    foreach (var day in Days)
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
            index = 0;
            foreach (var BySP in AllData)
            {
                foreach (var item in BySP.InOffLines)
                {
                    int DayIndex = 0;
                    foreach (var day in Days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[(index)]["SP"] = BySP.OrderID;

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
                                if (Days.Count - 1 < (DayIndex + 1))
                                {
                                    cellValue = 0;
                                    continue;
                                }
                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == Days[DayIndex + 1].Date);
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
                                if (Days.Count - 1 < (DayIndex + 1) || Days.Count - 1 < (DayIndex + 2))
                                {
                                    cellValue = 0;
                                    continue;
                                }
                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == Days[DayIndex + 1].Date);
                                var findData_nextNextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == Days[DayIndex + 2].Date);
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

            resultList.Add(summaryDt);
            resultList.Add(detailDt);

            return resultList;
        }

        /// <summary>
        /// Cutting WIP 資料表 (所有In/Off Line資料由外部傳入) Index 0 為Summary、1 為Detail
        /// </summary>
        /// <param name="dt">SewingSchedule的Datatable</param>
        /// <param name="Days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <param name="AllData">In/Off Line資料</param>
        /// <returns></returns>
        public static List<DataTable> GetCutting_WIP_DataTable(DataTable dt, List<Day> Days, List<InOffLineList> AllData)
        {
            List<DataTable> resultList = new List<DataTable>();

            //List<InOffLineList> AllData = new List<InOffLineList>();

            //AllData = GetInOffLineList(dt, Days);

            DataTable detailDt = new DataTable();
            DataTable summaryDt = new DataTable();

            detailDt.ColumnsStringAdd("SP");
            detailDt.ColumnsStringAdd("Desc./Sewing Date");
            summaryDt.ColumnsStringAdd("SP");

            // 日期Column
            int idx = 3;
            foreach (var day in Days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                idx++;
            }

            int orderCount = AllData.Count;

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

            // 開始寫入Row
            int index = 0;
            foreach (var BySP in AllData)
            {
                if (BySP.OrderID == "20031214AB")
                {

                }
                foreach (var item in BySP.InOffLines)
                {
                    foreach (var day in Days)
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
                            //detailDt.Rows[(index * 4)][strDate] = DBNull.Value;
                            //detailDt.Rows[(index * 4) + 1][strDate] = DBNull.Value;
                            //detailDt.Rows[(index * 4) + 2][strDate] = DBNull.Value;
                            //detailDt.Rows[(index * 4) + 3][strDate] = DBNull.Value;
                        }
                    }
                }
                index++;
            }
            index = 0;
            foreach (var BySP in AllData)
            {
                foreach (var item in BySP.InOffLines)
                {
                    int DayIndex = 0;
                    foreach (var day in Days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[(index)]["SP"] = BySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            decimal CutQty = item.CutQty;
                            decimal StdQty = item.StdQty;
                            decimal AccuCutQty = item.AccuCutQty;
                            decimal AccuStdQty = item.AccuStdQty;
                            decimal cellValue = 0;
                            if (AccuCutQty <= AccuStdQty)
                            {
                                cellValue = StdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / StdQty;
                            }
                            else if (StdQty == 0 || ((AccuCutQty - AccuStdQty) / StdQty) <= 1)
                            {
                                if (Days.Count - 1 < (DayIndex + 1))
                                {
                                    cellValue = 0;
                                    continue;
                                }
                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == Days[DayIndex + 1].Date);
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
                                if (Days.Count - 1 < (DayIndex + 1) || Days.Count - 1 < (DayIndex + 2))
                                {
                                    cellValue = 0;
                                    continue;
                                }
                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == Days[DayIndex + 1].Date);
                                var findData_nextNextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == Days[DayIndex + 2].Date);
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
                            //summaryDt.Rows[(index)][strDate] = DBNull.Value;
                        }
                        DayIndex++;
                    }
                }
                index++;
            }

            resultList.Add(summaryDt);
            resultList.Add(detailDt);

            return resultList;
        }

        public static List<LeadTime> GetLeadTimeList(List<string> OrderIDs)
        {
            List<LeadTime> LeadTimeList = new List<LeadTime>();

            DataTable PoID_dt;
            DataTable GarmentTb;
            DataTable LeadTime_dt;
            DualResult result;

            foreach (string OrderID in OrderIDs)
            {
                string POID = MyUtility.GetValue.Lookup($"SELECT POID FROM Orders WITH(NOLOCK) WHERE ID='{OrderID}' ");

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
                    //    this.ShowErr(result);
                    //    return false;
                }

                // 收集需要顯示訊息的Subprocess ID
                if (LeadTime_dt.Rows.Count == 0 && AnnotationStr != string.Empty)
                {
                    //Msg.Add(AnnotationStr);
                    return null;
                }
                else
                {
                    // 記錄下加工段的Lead Time
                    LeadTime o = new LeadTime()
                    {
                        OrderID = OrderID,
                        LeadTimeDay = MyUtility.Check.Empty(AnnotationStr) ? 0 : Convert.ToInt32(LeadTime_dt.Rows[0]["LeadTime"]) //加工段為空，LeadTimeDay = 0
                    };
                    LeadTimeList.Add(o);
                }

            }
            return LeadTimeList;
        }

        #region 類別
        /// <summary>
        /// 一件成衣，由哪些部位組成
        /// </summary>
        public class GarmentList
        {
            public DateTime EstCutDate { get; set; }
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
        public class Panel
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

        public class PanelCode
        {
            public string FabricPanelCode { get; set; }
            public int Qty { get; set; }
        }

        /// <summary>
        /// 時間軸基礎物件
        /// </summary>
        public class Day
        {
            //public string FactoryID { get; set; }
            public DateTime Date { get; set; }
            public bool IsHoliday { get; set; }
        }
        /// <summary>
        /// OrderID對應的LeadTime
        /// </summary>
        public class LeadTime
        {
            public string OrderID { get; set; }
            public int LeadTimeDay { get; set; }
        }

        public class InOffLineList
        {
            public string OrderID { get; set; }

            public List<InOffLine> InOffLines { get; set; }
        }

        public class InOffLine
        {
            public string ApsNO { get; set; }
            public int CutQty { get; set; }
            public int AccuCutQty { get; set; }
            public int StdQty { get; set; }
            public int AccuStdQty { get; set; }
            public DateTime DateWithLeadTime { get; set; }
            public decimal WIP { get; set; }
        }

        #endregion
    }

}