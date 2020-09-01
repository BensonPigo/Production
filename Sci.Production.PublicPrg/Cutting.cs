using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        /// <summary>
        /// 馬克長的格式轉換(轉換成 99Y99-9/9+9" 這種格式)
        /// </summary>
        /// <inheritdoc />
        public static string MarkerLengthSampleTOTrade(string markerLength, string matchFabric)
        {
            // 馬克長的格式轉換(轉換成 99Y99-9/9+9" 這種格式)
            string newMarkerLength, yd, addstring;
            int length = markerLength.IndexOf("Yd", 1);
            /*
            yd = markerLength.Substring(0, length);

            if (length == 0) { newMarkerLength = "00Ｙ"; }
            else if (Convert.ToDecimal(yd) < 10) { newMarkerLength = "0" + yd + "Ｙ"; }
            else { newMarkerLength = yd + "Y"; }
            */
            if (length <= 0)
            {
                newMarkerLength = "00Ｙ";
            }
            else
            {
                yd = markerLength.Substring(0, length);
                newMarkerLength = yd.PadLeft(2, '0') + "Ｙ";
            }

            length = markerLength.IndexOf('"', 1);
            if (length <= 0)
            {
                newMarkerLength += "00-";
            }
            else
            {
                int ydno = markerLength.IndexOf("Yd", 1);

                if (ydno >= 0)
                {
                    ydno += 2; // 有找到Yd，就由Yd後的位置(x+2)開始抓
                }
                else
                {
                    ydno = 0;
                }

                string inch = markerLength.Substring(ydno, length - ydno);
                newMarkerLength += inch.PadLeft(2, '0') + "-";
            }

            // 當為"Body Mapping"時，[Marker Length]不必顯示+1
            addstring = matchFabric == "1" ? "\"" : "+1\"";
            length = markerLength.IndexOf('/', 1);
            newMarkerLength += (length <= 0 ? "0/0" : markerLength.Substring(length - 1, 3)) + addstring;

            return newMarkerLength;
        }

        #region BundleCardCheckSubprocess

        /// <summary>
        /// BundleCardCheckSubprocess(string[] ann, string patterncode,DataTable artTb, out bool lallpart)
        /// </summary>
        /// <param name="ann">ann</param>
        /// <param name="patterncode">patterncode</param>
        /// <param name="artTb">artTb</param>
        /// <param name="lallpart">lallpartas</param>
        /// <returns>string</returns>
        public static string BundleCardCheckSubprocess(string[] ann, string patterncode, DataTable artTb, out bool lallpart)
        {
            // artTb 是給前Form 使用同Garment List 的PatternCode 與Subrpocess
            string art = string.Empty;
            lallpart = true; // 是不是All part

            // 寫入判斷是否存在Subprocess
            for (int i = 0; i < ann.Length; i++)
            {
                string[] ann2 = ann[i].ToString().Split(' '); // 剖析Annotation
                if (ann2.Length > 0)
                {
                    #region 有分開字元需剖析
                    for (int j = 0; j < ann2.Length; j++)
                    {
                        if (MyUtility.Check.Seek(ann2[j], "subprocess", "Id"))
                        {
                            lallpart = false;

                            // Artwork 相同的也要顯示, ex: HT+HT
                            // if (art.IndexOf(ann2[j]) == -1)
                            // {
                            DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann2[j]));
                            if (existdr.Length == 0)
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann2[j];
                                artTb.Rows.Add(ndr_art);
                            }

                            if (art == string.Empty)
                            {
                                art = ann2[j];
                            }
                            else
                            {
                                art = art.Trim() + "+" + ann2[j];
                            }

                            // }
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

                            // 表示無在ArtTable內
                            if (existdr.Length == 0)
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann[i];
                                artTb.Rows.Add(ndr_art);
                            }

                            if (art == string.Empty)
                            {
                                art = ann[i];
                            }
                            else
                            {
                                art = art.Trim() + "+" + ann[i];
                            }
                        }
                    }
                    #endregion
                }
            }

            return art;
        }
        #endregion;

        /// <summary>
        /// 均分數量 EX:10均分4份→3,3,2,2
        /// </summary>
        /// <param name="dr">dr</param>
        /// <param name="columnName">Qty</param>
        /// <param name="totalNumeric">0</param>
        /// <param name="deleteZero">false</param>
        public static void AverageNumeric(DataRow[] dr, string columnName = "Qty", int totalNumeric = 0, bool deleteZero = false)
        {
            if (dr.Count() == 0)
            {
                return;
            }

            int rowCount = dr.Count();
            int eachqty = totalNumeric / rowCount;
            int modqty = totalNumeric % rowCount; // 剩餘數

            if (modqty == 0)
            {
                foreach (DataRow dr2 in dr)
                {
                    dr2[columnName] = eachqty;
                }
            }
            else
            {
                foreach (DataRow dr2 in dr)
                {
                    if (eachqty != 0)
                    {
                        if (modqty > 0)
                        {
                            dr2[columnName] = eachqty + 1; // 每組分配一個Qty 當分配完表示沒了
                        }
                        else
                        {
                            dr2[columnName] = eachqty;
                        }

                        modqty--; // 剩餘數一定小於rowcount所以會有筆數沒有拿到
                    }
                    else
                    {
                        // 這處理資料筆數小於總數. EX:3筆資料,總數只有2
                        if (modqty > 0)
                        {
                            dr2[columnName] = 1;
                            modqty--;
                        }
                        else
                        {
                            if (deleteZero)
                            {
                                dr2.Delete();
                            }
                            else
                            {
                                dr2[columnName] = 0;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Days
        /// </summary>
        /// <param name="leadtime">int</param>
        /// <param name="date_where">DateTime</param>
        /// <param name="ftyFroup">List<string></param>
        /// <returns>List<Day></returns>
        public static List<Day> GetDays(int leadtime, DateTime date_where, List<string> ftyFroup)
        {
            DateTime start = date_where.AddDays(-leadtime).Date;

            List<Day> dayList = new List<Day>();

            string sqlcmd = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{date_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{ftyFroup.JoinToString("','")}')
";
            DataTable dt2;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt2);

            // 開始組合時間軸
            for (int day1 = 0; day1 <= leadtime; day1++)
            {
                Day day = new Day();
                day.Date = date_where.AddDays(-day1).Date;

                // 是否行事曆設定假日
                bool isHoliday = dt2.AsEnumerable().Where(o => MyUtility.Convert.GetDate(o["HolidayDate"]) == day.Date).Any();

                // 是行事曆設定假日 or 星期天
                if (isHoliday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    isHoliday = true;

                    // 為避免假日推移的影響，讓時間軸不夠長，因此每遇到一次假日，就要加長一次時間軸
                    leadtime++;
                    start = start.AddDays(-1);

                    sqlcmd = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{date_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{ftyFroup.JoinToString("','")}')
";
                    DBProxy.Current.Select(null, sqlcmd, out dt2);
                }

                day.IsHoliday = isHoliday;
                dayList.Add(day);
            }

            return dayList;
        }

        /// <summary>
        /// GetRangeHoliday
        /// </summary>
        /// <param name="date1">DateTime date1</param>
        /// <param name="date2">DateTime date2</param>
        /// <param name="ftyFroup">FtyFroup</param>
        /// <returns>List<Day></returns>
        public static List<Day> GetRangeHoliday(DateTime date1, DateTime date2, List<string> ftyFroup)
        {
            List<Day> dayList = new List<Day>();

            string sqlcmd = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{date1.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{date2.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{ftyFroup.JoinToString("','")}')
";
            DataTable dt2;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt2);

            int days = (int)(date2.Date - date1.Date).TotalDays;
            for (int day1 = 0; day1 <= days; day1++)
            {
                Day day = new Day();
                day.Date = date2.AddDays(-day1).Date;

                // 是否行事曆設定假日
                bool isHoliday = dt2.AsEnumerable().Where(o => MyUtility.Convert.GetDate(o["HolidayDate"]) == day.Date).Any();

                // 是行事曆設定假日 or 星期天
                if (isHoliday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    isHoliday = true;
                }

                day.IsHoliday = isHoliday;
                dayList.Add(day);
            }

            return dayList;
        }

        /// <summary>
        /// 取得 by APSNo & 每日 的標準數
        /// </summary>
        /// <param name="orderIDs">List<string> OrderID</param>
        /// <returns>List<DailyStdQty></returns>
        public static List<DailyStdQty> GetStdQty(List<string> orderIDs)
        {
            string sqlcmd = $@"
SELECT s.OrderID,s.ComboType,x.*
FROM SewingSchedule  s
outer apply(select * from [dbo].[getDailystdq](s.APSNo))x
WHERE OrderID IN ('{orderIDs.JoinToString("','")}')
and x.APSNo is not null
";
            DataTable tmpDt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out tmpDt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }

            List<DailyStdQty> aPSNoDailyStdQty = tmpDt.AsEnumerable().Select(s => new DailyStdQty
            {
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                APSNo = MyUtility.Convert.GetString(s["APSNo"]),
                Date = Convert.ToDateTime(s["Date"]).Date,
                StdQty = MyUtility.Convert.GetInt(s["StdQ"]),
            }).ToList();

            return aPSNoDailyStdQty;
        }

        #region
        public static DataTable GetCuttingTapeData(string cuttingID)
        {
            DataTable[] dt;
            string sqlcmd = $@"
declare @CuttingSP varchar(13) = '{cuttingID}'

select 
	e.MarkerName,
    e.FabricCombo,
	e.Width,
	[TypeofCuttingNode_Group] = SUBSTRING(e.Remark ,1 ,CHARINDEX('#', e.Remark ) - 1),
	[TypeofCuttingNode] = SUBSTRING(e.Remark ,CHARINDEX('#', e.Remark ), LEN(e.Remark)),
	e.MarkerLength,
	e.ConsPC,
	a.Article,
    a.ColorID,
	SP='',
    a.CutQty,
    a.SizeCode,
    e.FabricPanelCode
from dbo.Order_EachCons e
left join dbo.Order_EachCons_Color c on c.Order_EachConsUkey = e.Ukey
left join dbo.Order_EachCons_Color_Article a on a.Order_EachCons_ColorUkey= c.Ukey
left join dbo.Order_EachCons_SizeQty s on s.Order_EachConsUkey = e.Ukey and s.SizeCode = a.SizeCode
where e.Id = @CuttingSP
and e. CuttingPiece = 1
ORDER BY e.MarkerName

select
	[SP] = o.ID  
	, [FabricCombo] = fab.PatternPanel, fab.FabricPanelCode, q.Article, fab.ColorID, q.SizeCode	
	, q.Qty, o.SewInLine
from dbo.orders o 
left join dbo.Order_Qty q on q.ID = o.ID  
outer apply(
	select
		s.ID, c.PatternPanel, c.FabricPanelCode, c.ColorID, c.Article 
	from dbo.orders s 
	left join dbo.Order_ColorCombo c on c.Id = s.ID and c.FabricType = 'F'
	where s.id  = @CuttingSP and c.Article = q.Article
) fab
where o.CuttingSP = @CuttingSP and o.Junk = 0
order by o.SewInLine
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return null;
            }

            if (dt[0].Rows.Count == 0)
            {
                return null;
            }

            // 準備欄位字串
            var colList = dt[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            string columnsName = string.Join(",", colList);

            DataTable dtf = dt[0].Clone();
            foreach (DataRow row1 in dt[0].Rows)
            {
                int cutQty = MyUtility.Convert.GetInt(row1["CutQty"]); // 可分配總數
                DataRow[] drs2 = dt[1].Select($"FabricCombo='{row1["FabricCombo"]}'and FabricPanelCode='{row1["FabricPanelCode"]}'and Article='{row1["Article"]}'and ColorID='{row1["ColorID"]}'and SizeCode='{row1["SizeCode"]}'");
                bool flag = false;
                foreach (DataRow row2 in drs2)
                {
                    DataRow newrow = dtf.NewRow();
                    row1.CopyTo(newrow, columnsName);
                    int qty2 = MyUtility.Convert.GetInt(row2["Qty"]);
                    if (cutQty < qty2 || cutQty == qty2)
                    {
                        if (flag && cutQty == 0)
                        {
                            break;
                        }

                        newrow["CutQty"] = cutQty;
                        newrow["SP"] = row2["SP"];
                        dtf.Rows.Add(newrow);
                        cutQty -= qty2;
                        break;
                    }

                    if (cutQty > qty2)
                    {
                        newrow["CutQty"] = qty2;
                        newrow["SP"] = row2["SP"];
                        dtf.Rows.Add(newrow);
                        flag = true;
                        cutQty -= qty2;
                    }
                }

                if (cutQty > 0)
                {
                    DataRow newrow = dtf.NewRow();
                    row1.CopyTo(newrow, columnsName);
                    newrow["CutQty"] = cutQty;
                    newrow["SP"] = "EXCESS";
                    dtf.Rows.Add(newrow);
                }
            }

            // CutQty分配完後計算Cons
            dtf.Columns.Add("Cons", typeof(decimal), "ConsPC*CutQty");

            // 排序
            DataTable dtf2 = dtf.AsEnumerable().
                OrderBy(o => MyUtility.Convert.GetString(o["MarkerName"])).
                ThenBy(o => MyUtility.Convert.GetString(o["ColorID"])).
                ThenBy(o => MyUtility.Convert.GetString(o["Article"])).
                ThenBy(o => MyUtility.Convert.GetString(o["SizeCode"])).
                ThenBy(o => MyUtility.Convert.GetString(o["SP"])).
                CopyToDataTable();
            #region 排序完後處理Type of Cutting Node **用Marker Name前5碼做群組分類, 取最後一筆字串
            DataTable dtf3 = dtf2.Clone();
            int i = 0;
            string marker5_Group = string.Empty;
            foreach (DataRow row1 in dtf2.Rows)
            {
                string curr_Marker5_Group = MyUtility.Convert.GetString(row1["MarkerName"]).Substring(0, 5);
                if (curr_Marker5_Group != marker5_Group && i > 0)
                {
                    DataRow newrow = dtf3.NewRow();
                    newrow["TypeofCuttingNode"] = MyUtility.Convert.GetString(row1["TypeofCuttingNode_Group"]);
                    dtf3.Rows.Add(newrow);
                }

                marker5_Group = MyUtility.Convert.GetString(row1["MarkerName"]).Substring(0, 5);

                dtf3.ImportRow(row1);
                i++;
            }

            DataRow newrowLast = dtf3.NewRow();
            newrowLast["TypeofCuttingNode"] = marker5_Group;
            dtf3.Rows.Add(newrowLast);
            dtf3.Columns.Remove("TypeofCuttingNode_Group");
            #endregion

            // Total
            DataRow newrowTotal = dtf3.NewRow();
            newrowTotal["MarkerName"] = "Total";
            newrowTotal["CutQty"] = dtf3.Compute("sum(CutQty)", string.Empty);
            newrowTotal["Cons"] = dtf3.Compute("sum(Cons)", string.Empty);
            dtf3.Rows.Add(newrowTotal);
            return dtf3;
        }
        #endregion

        #region Cal WIP

        /// <summary>
        /// 取得Cutting成套的數量
        /// </summary>
        /// <param name="orderIDs">List<string> orderIDs</param>
        /// <returns>List<GarmentQty></returns>
        public static List<GarmentQty> GetCutPlanQty(List<string> orderIDs)
        {
            DataTable headDt;
            DataTable tmpDt;
            DualResult result;

            // 取得該訂單的組成
            #region 取得by SP, Size 應該完成的 FabricPanelCode
            string tmpCmd = $@"
SELECT DISTINCT 
    [OrderID]=o.ID
    ,oq.Article
    ,oq.SizeCode
    ,FabricCombo = occ.PatternPanel
    ,occ.FabricPanelCode
from Orders o WITH (NOLOCK)
inner join Order_Qty oq WITH (NOLOCK) on oq.id = o.id
inner join Order_ColorCombo occ WITH (NOLOCK) on occ.id = o.POID and occ.Article = oq.Article and occ.FabricCode is not null and occ.FabricCode !=''
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0
inner join Order_EachCons_Color oec WITH (NOLOCK) on oec.Order_EachConsUkey = oe.Ukey and oec.ColorID = occ.ColorID
inner join Order_EachCons_Color_Article oeca WITH (NOLOCK) on oeca.Order_EachCons_ColorUkey = oec.Ukey and oeca.Article = oq.Article
AND o.id IN ('{orderIDs.JoinToString("','")}')
AND (exists(select 1 from Order_EachCons_Article oea where  oea.Id = o.POID and oea.Article = oq.Article )
	or not exists (select 1 from Order_EachCons_Article oea where  oea.Id = o.POID)
)
--AND o.id='20032468LL004'
";

            result = DBProxy.Current.Select(null, tmpCmd, out headDt);
            var headList = headDt.AsEnumerable()
                .Select(s => new
                {
                    OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                    Article = MyUtility.Convert.GetString(s["Article"]),
                    SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                    FabricCombo = MyUtility.Convert.GetString(s["FabricCombo"]),
                    FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"]),
                }).ToList();
            #endregion

            #region 取得所有部位(有) EstCutDate 的 Cutting 數量
            tmpCmd = $@"
SELECT
     WOD.OrderID
    ,WOD.Article 
    ,WOD.SizeCode
    ,wo.FabricCombo
    ,wo.FabricPanelCode
    ,[Qty]=SUM(WOD.Qty)
    ,[EstCutDate]=Cast(EstCutDate.EstCutDate as Date)
FROM WorkOrder_Distribute WOD WITH(NOLOCK)
INNER JOIN WorkOrder WO WITH(NOLOCK) ON WO.Ukey = WOD.WorkOrderUkey
INNER JOIN Cutplan_Detail CD WITH(NOLOCK) ON CD.WorkorderUkey = WO.Ukey
OUTER APPLY (
	SELECT TOP 1 EstCutDate FROM Cutplan WHERE ID = CD.ID
)EstCutDate
WHERE WOD.OrderID IN ('{orderIDs.JoinToString("','")}')
AND (SELECT EstCutDate FROM Cutplan WHERE ID = CD.ID AND Status='Confirmed') IS NOT NULL
--AND WOD.OrderID='20032468LL004'

GROUP BY WOD.OrderID
		,WOD.Article 
		,WOD.SizeCode
		,wo.FabricCombo
		,wo.FabricPanelCode 
		,EstCutDate.EstCutDate
ORDER BY WOD.OrderID
";
            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);

            var cutList = tmpDt.AsEnumerable()
                .Select(s => new
                {
                    OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                    Article = MyUtility.Convert.GetString(s["Article"]),
                    SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                    FabricCombo = MyUtility.Convert.GetString(s["FabricCombo"]),
                    FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"]),
                    Qty = MyUtility.Convert.GetInt(s["Qty"]),
                    EstCutDate = Convert.ToDateTime(s["EstCutDate"]),
                }).ToList();
            #endregion

            // 取出Cutting資料的Key：OrderID + EstCutDate | 回傳的資料 OrderID, EstCutDate, Qty(累計成衣件數)
            var keys = cutList.Select(o => new { o.OrderID, o.EstCutDate }).Distinct().OrderBy(o => o.OrderID).ThenBy(o => o.EstCutDate).ToList();

            List<GarmentQty> garmentQtys = new List<GarmentQty>();

            // 組成 by OrderID, EstCutDate
            foreach (var key in keys)
            {
                if (key.OrderID == "20050116GG001")
                {
                }

                // 取得此日期以前資料
                var ppreEstCutDate = cutList.Where(w => w.OrderID == key.OrderID && w.EstCutDate < key.EstCutDate).ToList();
                var preEstCutDate = cutList.Where(w => w.OrderID == key.OrderID && w.EstCutDate <= key.EstCutDate).ToList();

                // 處理已有Size
                var sizeList = preEstCutDate.Select(s => s.SizeCode).Distinct().ToList();
                int qty = 0;

                foreach (var sizeCode in sizeList)
                {
                    var ppreEstCutDatebySize = ppreEstCutDate.Where(w => w.SizeCode == sizeCode).ToList();
                    var preEstCutDatebySize = preEstCutDate.Where(w => w.SizeCode == sizeCode).ToList();

                    // 應有全部位
                    var dueFabricPanelCode = headList.Where(w => w.OrderID == key.OrderID && w.SizeCode == sizeCode)
                        .Select(s => s.FabricPanelCode).ToList();

                    // 已有部位
                    var p_nowFabricPanelCode = ppreEstCutDatebySize.Select(s => s.FabricPanelCode).Distinct().ToList();
                    var nowFabricPanelCode = preEstCutDatebySize.Select(s => s.FabricPanelCode).Distinct().ToList();
                    int p_minSizeQty = 0;

                    // 部位完全相同, 部位到齊
                    if (p_nowFabricPanelCode.Count() == dueFabricPanelCode.Count() && p_nowFabricPanelCode.All(dueFabricPanelCode.Contains))
                    {
                        // 先前完成的成衣件數
                        p_minSizeQty = preEstCutDatebySize.GroupBy(g => new { g.FabricPanelCode })
                            .Select(s => new { s.Key.FabricPanelCode, sumQty = s.Sum(sum => sum.Qty) }).Min(m => m.sumQty);
                    }

                    if (nowFabricPanelCode.Count() == dueFabricPanelCode.Count() && nowFabricPanelCode.All(dueFabricPanelCode.Contains))
                    {
                        // 先依據部位加總, 再取最小值, 即此 EstCutDate以前 & 此 Size 可組成的成衣件數
                        int minSizeQty = preEstCutDatebySize.GroupBy(g => new { g.FabricPanelCode })
                            .Select(s => new { s.Key.FabricPanelCode, sumQty = s.Sum(sum => sum.Qty) }).Min(m => m.sumQty);

                        // 到此日期的成衣數 - 先前數, 所有 Size 加總
                        qty += minSizeQty - p_minSizeQty;
                    }
                }

                if (qty > 0)
                {
                    GarmentQty gar = new GarmentQty();
                    gar.OrderID = key.OrderID;
                    gar.EstCutDate = key.EstCutDate;
                    gar.Qty = qty;
                    garmentQtys.Add(gar);
                }
            }

            return garmentQtys;
        }

        /// <summary>
        /// 取得 by FabricPanelCode 每日的 Cut Plan Qty
        /// </summary>
        /// <param name="orderIDs">List<string> orderIDs</param>
        /// <returns>List<FabricPanelCodeCutPlanQty></returns>
        public static List<FabricPanelCodeCutPlanQty> GetSPFabricPanelCodeList(List<string> orderIDs)
        {
            // by FabricPanelCode 沒有成套問題, 直接每天相同的 FabricPanelCode 加總即可
            DataTable tmpDt;
            DualResult result;
            #region SQL
            string tmpCmd = $@"
SELECT DISTINCT 
    [OrderID]=o.ID
    ,cons.FabricPanelCode
FROM Orders o WITH (NOLOCK)
INNER JOIN Order_qty oq ON o.ID=oq.ID
INNER JOIN Order_ColorCombo occ ON o.poid = occ.id AND occ.Article = oq.Article
INNER JOIN order_Eachcons cons ON occ.id = cons.id AND cons.FabricCombo = occ.PatternPanel AND cons.CuttingPiece='0'
inner join Order_EachCons_Color oec WITH (NOLOCK) on oec.Order_EachConsUkey = cons.Ukey and oec.ColorID = occ.ColorID
inner join Order_EachCons_Color_Article oeca WITH (NOLOCK) on oeca.Order_EachCons_ColorUkey = oec.Ukey and oeca.Article = oq.Article
WHERE occ.FabricCode !='' AND occ.FabricCode IS NOT NULL
AND o.id IN ('{orderIDs.JoinToString("','")}')
order by o.ID,cons.FabricPanelCode
";
            #endregion

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }

            List<FabricPanelCodeCutPlanQty> fabricPanelCodeCutPlanQty = tmpDt.AsEnumerable().Select(s => new FabricPanelCodeCutPlanQty
            {
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"]),
            }).ToList();

            return fabricPanelCodeCutPlanQty;
        }

        /// <summary>
        /// 取得 by FabricPanelCode 每日的 Cut Plan Qty
        /// </summary>
        /// <param name="orderIDs">List<string> orderIDs</param>
        /// <returns>List<FabricPanelCodeCutPlanQty></returns>
        public static List<FabricPanelCodeCutPlanQty> GetCutPlanQty_byFabricPanelCode(List<string> orderIDs)
        {
            // by FabricPanelCode 沒有成套問題, 直接每天相同的 FabricPanelCode 加總即可
            DataTable tmpDt;
            DualResult result;
            #region SQL
            string tmpCmd = $@"
SELECT  WOD.OrderID
    ,wo.FabricPanelCode
    ,[Qty]=SUM(WOD.Qty)
    ,[EstCutDate]=Cast(EstCutDate.EstCutDate as Date)
FROM WorkOrder_Distribute WOD WITH(NOLOCK)
INNER JOIN WorkOrder WO WITH(NOLOCK) ON WO.Ukey = WOD.WorkOrderUkey
INNER JOIN Cutplan_Detail CD WITH(NOLOCK) ON CD.WorkorderUkey = WO.Ukey
OUTER APPLY (
	SELECT TOP 1 EstCutDate FROM Cutplan WHERE ID = CD.ID
)EstCutDate
WHERE WOD.OrderID IN ('{orderIDs.JoinToString("','")}')
AND (SELECT EstCutDate FROM Cutplan WHERE ID = CD.ID AND Status='Confirmed') IS NOT NULL
--AND WOD.OrderID='20032468LL004'
GROUP BY WOD.OrderID
		,wo.FabricPanelCode 
		,EstCutDate.EstCutDate
order by WOD.OrderID,EstCutDate.EstCutDate
";
            #endregion

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }

            List<FabricPanelCodeCutPlanQty> fabricPanelCodeCutPlanQty = tmpDt.AsEnumerable().Select(s => new FabricPanelCodeCutPlanQty
            {
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                EstCutDate = (DateTime)s["EstCutDate"],
                FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"]),
                Qty = MyUtility.Convert.GetInt(s["Qty"]),
            }).ToList();

            return fabricPanelCodeCutPlanQty;
        }

        /// <summary>
        /// 取得所有In/Off Line資料 (成套數量內部自行處理)
        /// </summary>
        /// <param name="dt_SewingSchedule">SewingSchedule的Datatable</param>
        /// <param name="days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <param name="startdate">start Date</param>
        /// <param name="enddate">end date</param>
        /// <param name="ori_startdate">original start date</param>
        /// <param name="ori_Enddate">original End date</param>
        /// <param name="bw">Background Worker</param>
        /// <returns>List<InOffLineList> allData </returns>
        public static List<InOffLineList> GetInOffLineList(DataTable dt_SewingSchedule, List<Day> days, DateTime? startdate = null, DateTime? enddate = null, DateTime? ori_startdate = null, DateTime? ori_Enddate = null, System.ComponentModel.BackgroundWorker bw = null)
        {
            if (startdate == null)
            {
                startdate = days.Min(m => m.Date.Date).Date;
            }

            if (enddate == null)
            {
                enddate = days.Max(m => m.Date.Date).Date;
            }

            decimal processInt = 10; // 給進度條顯示值
            decimal pc = 10;
            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList> allDataTmp = new List<InOffLineList>();
            List<InOffLineList> allData = new List<InOffLineList>();

            List<string> allOrder = dt_SewingSchedule.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            #region LeadTimeList
            string annotationStr;
            List<LeadTime> leadTimeList = GetLeadTimeList(allOrder, out annotationStr);
            if (leadTimeList == null)
            {
                return null; // 表示Lead Time有缺
            }
            #endregion

            List<DailyStdQty> stdQtyList = GetStdQty(allOrder);
            if (bw != null)
            {
                if (bw.CancellationPending == true)
{
    return null;
}

                bw.ReportProgress((int)processInt);
            } // 10%

            List<GarmentQty> garmentList = GetCutPlanQty(allOrder);
            processInt = processInt + 5;
            if (bw != null)
            {
                if (bw.CancellationPending == true)
{
    return null;
}

                bw.ReportProgress((int)processInt);
            } // 15%

            if (allOrder.Count > 0)
            {
                pc = (decimal)70 / allOrder.Count; // 此迴圈佔 70% → 85%
            }

            // 處理不同 OrderID
            foreach (string orderID in allOrder)
            {
                if (orderID == "20042084GG002")
                {
                }

                // 此OrderID的SewingSchedule資料
                var sameOrderId = dt_SewingSchedule.AsEnumerable().Where(o => o["OrderID"].ToString() == orderID);

                // 這筆訂單的起始與結束時間
                DateTime start = sameOrderId.Min(o => Convert.ToDateTime(o["Inline"]));
                DateTime end = sameOrderId.Max(o => Convert.ToDateTime(o["offline"]));

                InOffLineList nOnj = new InOffLineList();

                // SP#
                nOnj.OrderID = orderID;
                nOnj.InOffLines = new List<InOffLine>();

                // 所有Order ID、以及相對應 要扣去的Lead Time(天)
                int leadTime = leadTimeList.Where(o => o.OrderID == orderID).FirstOrDefault().LeadTimeDay;

                foreach (DataRow dr in sameOrderId)
                {
                    string apsNO = dr["APSNo"].ToString();
                    int alloQty = MyUtility.Convert.GetInt(dr["AlloQty"]); // 此 SewingSchedule 的上限

                    // 這筆 SewingSchedule 日期範圍
                    for (DateTime aPSday = Convert.ToDateTime(dr["Inline"]).Date; aPSday <= Convert.ToDateTime(dr["Offline"]).Date; aPSday = aPSday.AddDays(1))
                    {
                        // 原始日期，不在初始篩選範圍內
                        if (ori_startdate != null && ori_Enddate != null && (ori_startdate > aPSday || ori_Enddate < aPSday))
                        {
                            continue;
                        }

                        // 原始日期不存在當日標準數
                        if (!stdQtyList.Where(w => w.OrderID == orderID && w.APSNo == apsNO && w.Date == aPSday).Any())
                        {
                            continue;
                        }

                        DateTime pdate = aPSday; // 紀錄推算後的日期
                        #region 原始日 - LeadTime 之間有多少天 Holiday  PS:時間軸 Days 傳入時範圍是剛好的
                        if (!days.Where(w => w.Date == aPSday && w.IsHoliday).Any()) // 假日不推算
                        {
                            int holidayCount = days.Where(w => w.Date >= aPSday.AddDays(-leadTime) && w.Date <= aPSday && w.IsHoliday).Count();
                            if (holidayCount > 0)
                            {
                                for (int i = holidayCount; true;)
                                {
                                    int newCount = days.Where(w => w.Date >= aPSday.Date.AddDays(-i - leadTime) && w.Date <= aPSday && w.IsHoliday).Count();
                                    if (newCount > holidayCount)
                                    {
                                        holidayCount = newCount;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            pdate = aPSday.AddDays(-holidayCount - leadTime);
                        }
                        #endregion

                        // 推算後的日期，不在最終顯示範圍
                        if (startdate > pdate || enddate < pdate)
                        {
                            continue;
                        }

                        // 如果這 OrderID & 這天(用推算後比較) 已經紀錄在 nOnj, 則跳過這天, 因下方標準數,裁減數是以(天)計算成套,故同天不用重複算
                        if (nOnj.InOffLines.Where(w => w.DateWithLeadTime == pdate).Any())
                        {
                            continue;
                        }

                        // 若這筆 ApsNO(排程), 在前一圈(日)已經到達上限, 則直接換下筆 ApsNO(排程)
                        // 此處取得累計標準數, 不可用nOnj紀錄的數值, 因上個判斷同orderid同天不會做紀錄
                        int accStdQtybyApsNO = stdQtyList.Where(w => w.OrderID == orderID && w.APSNo == apsNO && w.Date < aPSday).Sum(s => s.StdQty);
                        if (accStdQtybyApsNO >= alloQty)
                        {
                            break;
                        }

                        // 當天成套
                        int stdQty = GetStdQtyByDate(orderID, aPSday);

                        // 當天之前(包含當天)成套數
                        int accuStdQty = GetAccuStdQtyByDate(orderID, aPSday);

                        // 取裁剪數量
                        int cutqty = garmentList.Where(o => o.OrderID == orderID && o.EstCutDate == pdate).Select(s => s.Qty).FirstOrDefault();

                        // 累計裁剪量 = 先前累計裁剪量 + 當天裁剪量，因此是 <=
                        int accuCutQty = garmentList.Where(o => o.OrderID == orderID && o.EstCutDate <= pdate).Sum(o => o.Qty);

                        InOffLine nLineObj = new InOffLine()
                        {
                            DateWithLeadTime = pdate,
                            ApsNO = apsNO,
                            CutQty = cutqty,
                            AccuCutQty = accuCutQty,
                            StdQty = stdQty,
                            AccuStdQty = accuStdQty,
                        };

                        nOnj.InOffLines.Add(nLineObj);
                    }
                }

                if (nOnj.InOffLines.Any())
                {
                    allDataTmp.Add(nOnj);
                }

                if (bw != null)
                {
                    processInt = processInt + pc;
                    if (bw.CancellationPending == true)
                    {
                        return null;
                    }

                    bw.ReportProgress((int)processInt);
                }
            }

            #region 相同日期GROUP BY
            foreach (var bySP in allDataTmp)
            {
                if (bySP.OrderID == "20040358GG")
                {
                }

                InOffLineList n = new InOffLineList();
                n.OrderID = bySP.OrderID;
                n.InOffLines = new List<InOffLine>();
                var groupData = bySP.InOffLines.GroupBy(o => new { o.DateWithLeadTime, o.CutQty, o.StdQty, o.AccuCutQty, o.AccuStdQty })
                                .Select(x => new InOffLine
                                {
                                    DateWithLeadTime = x.Key.DateWithLeadTime,
                                    CutQty = x.Key.CutQty,
                                    StdQty = x.Key.StdQty,
                                    AccuCutQty = x.Key.AccuCutQty,
                                    AccuStdQty = x.Key.AccuStdQty,
                                }).OrderBy(o => o.DateWithLeadTime).ToList();

                n.InOffLines = groupData;
                allData.Add(n);
            }
            #endregion

            return allData;
        }

        /// <summary>
        /// Get In Off LineList by FabricPanelCode
        /// </summary>
        /// <param name="dt_SewingSchedule">dataTable SewingSchedule</param>
        /// <param name="days">list days</param>
        /// <param name="startdate">start date</param>
        /// <param name="enddate">end date</param>
        /// <param name="ori_startdate">original start date</param>
        /// <param name="ori_Enddate">original End Date</param>
        /// <param name="bw">Background Worker</param>
        /// <returns>allData</returns>
        public static List<InOffLineList_byFabricPanelCode> GetInOffLineList_byFabricPanelCode(DataTable dt_SewingSchedule, List<Day> days, DateTime? startdate = null, DateTime? enddate = null, DateTime? ori_startdate = null, DateTime? ori_Enddate = null, System.ComponentModel.BackgroundWorker bw = null)
        {
            if (startdate == null)
            {
                startdate = days.Min(m => m.Date.Date).Date;
            }

            if (enddate == null)
            {
                enddate = days.Max(m => m.Date.Date).Date;
            }

            decimal processInt = 10; // 給進度條顯示值
            decimal pc = 10;

            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList_byFabricPanelCode> allDataTmp = new List<InOffLineList_byFabricPanelCode>();
            List<InOffLineList_byFabricPanelCode> allData = new List<InOffLineList_byFabricPanelCode>();

            List<string> allOrder = dt_SewingSchedule.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            #region LeadTimeList
            string annotationStr;
            List<LeadTime> leadTimeList = GetLeadTimeList(allOrder, out annotationStr);
            if (leadTimeList == null)
            {
                return null; // 表示Lead Time有缺
            }
            #endregion

            List<DailyStdQty> stdQtyList = GetStdQty(allOrder);
            if (bw != null)
            {
                if (bw.CancellationPending == true)
                {
                    return null;
                }

                bw.ReportProgress((int)processInt);
            } // 10%

            List<FabricPanelCodeCutPlanQty> sPFabricPanelCodeList = GetSPFabricPanelCodeList(allOrder); // 基底用來跑主迴圈 SP + FabricPanelCode

            List<FabricPanelCodeCutPlanQty> cutPlanQtyList = GetCutPlanQty_byFabricPanelCode(allOrder);
            processInt = processInt + 5;
            if (bw != null)
            {
                if (bw.CancellationPending == true)
                {
                    return null;
                }

                bw.ReportProgress((int)processInt);
            } // 15%

            if (sPFabricPanelCodeList.Count > 0)
            {
                pc = (decimal)70 / sPFabricPanelCodeList.Count; // 此迴圈佔 70% → 85%
            }

            // 處理不同 OrderID, FabricPanelCode
            foreach (var item in sPFabricPanelCodeList)
            {
                string orderID = item.OrderID;
                string fabricPanelCode = item.FabricPanelCode;

                var sameOrderId = dt_SewingSchedule.AsEnumerable().Where(o => o["OrderID"].ToString() == orderID);

                // 這筆訂單的起始與結束時間
                DateTime start = sameOrderId.Min(o => Convert.ToDateTime(o["Inline"]));
                DateTime end = sameOrderId.Max(o => Convert.ToDateTime(o["offline"]));

                InOffLineList_byFabricPanelCode nOnj = new InOffLineList_byFabricPanelCode();

                // SP#
                nOnj.OrderID = orderID;
                nOnj.FabricPanelCode = fabricPanelCode;
                nOnj.InOffLines = new List<InOffLine>();

                // 所有Order ID、以及相對應 要扣去的Lead Time
                int leadTime = leadTimeList.Where(o => o.OrderID == orderID).FirstOrDefault().LeadTimeDay;

                foreach (DataRow dr in sameOrderId)
                {
                    string apsNO = dr["APSNo"].ToString();
                    int alloQty = MyUtility.Convert.GetInt(dr["AlloQty"]); // 此 SewingSchedule 的上限

                    // 這筆 SewingSchedule 日期範圍
                    for (DateTime aPSday = Convert.ToDateTime(dr["Inline"]).Date; aPSday <= Convert.ToDateTime(dr["Offline"]).Date; aPSday = aPSday.AddDays(1))
                    {
                        // 原始日期，不在初始篩選範圍內
                        if (ori_startdate != null && ori_Enddate != null && (ori_startdate > aPSday || ori_Enddate < aPSday))
                        {
                            continue;
                        }

                        // 原始日期不存在當日標準數
                        if (!stdQtyList.Where(w => w.OrderID == orderID && w.APSNo == apsNO && w.Date == aPSday).Any())
                        {
                            continue;
                        }

                        DateTime pdate = aPSday; // 紀錄推算後的日期
                        #region 原始日 - LeadTime 之間有多少天 Holiday  PS:時間軸 Days 傳入時範圍是剛好的
                        if (!days.Where(w => w.Date == aPSday && w.IsHoliday).Any()) // 假日不推算
                        {
                            int holidayCount = days.Where(w => w.Date >= aPSday.AddDays(-leadTime) && w.Date <= aPSday && w.IsHoliday).Count();
                            if (holidayCount > 0)
                            {
                                for (int i = holidayCount; true;)
                                {
                                    int newCount = days.Where(w => w.Date >= aPSday.Date.AddDays(-i - leadTime) && w.Date <= aPSday && w.IsHoliday).Count();
                                    if (newCount > holidayCount)
                                    {
                                        holidayCount = newCount;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            pdate = aPSday.AddDays(-holidayCount - leadTime);
                        }
                        #endregion

                        // 推算後的日期，不在最終顯示範圍
                        if (startdate > pdate || enddate < pdate)
                        {
                            continue;
                        }

                        // 如果這 OrderID & FabricPanelCode & 這天(用推算後比較) 已經紀錄在 nOnj, 則跳過這天, 因下方標準數,裁減數是以(天)計算成套,故同天不用重複算
                        if (nOnj.InOffLines.Where(w => w.DateWithLeadTime == pdate).Any())
                        {
                            continue;
                        }

                        // 若這筆 ApsNO(排程), 在前一圈(日)已經到達上限, 則直接換下筆 ApsNO(排程)
                        // 此處取得累計標準數, 不可用nOnj紀錄的數值, 因上個判斷同orderid同天不會做紀錄
                        int accStdQtybyApsNO = stdQtyList.Where(w => w.OrderID == orderID && w.APSNo == apsNO && w.Date < aPSday).Sum(s => s.StdQty);
                        if (accStdQtybyApsNO >= alloQty)
                        {
                            break;
                        }

                        // 當天成套
                        int stdQty = GetStdQtyByDate(orderID, aPSday);

                        // 當天之前(包含當天)成套數
                        int accuStdQty = GetAccuStdQtyByDate(orderID, aPSday);

                        // 取裁剪數量
                        int cutqty = cutPlanQtyList.Where(o => o.OrderID == orderID && o.FabricPanelCode == fabricPanelCode && o.EstCutDate == pdate).Select(s => s.Qty).FirstOrDefault();

                        // 累計裁剪量 = 先前累計裁剪量 + 當天裁剪量，因此是 <= day.Date.Date
                        int accuCutQty = cutPlanQtyList.Where(o => o.OrderID == orderID && o.FabricPanelCode == fabricPanelCode && o.EstCutDate <= pdate).Sum(o => o.Qty);

                        InOffLine nLineObj = new InOffLine()
                        {
                            DateWithLeadTime = pdate,
                            ApsNO = apsNO,
                            CutQty = cutqty,
                            AccuCutQty = accuCutQty,
                            StdQty = stdQty,
                            AccuStdQty = accuStdQty,
                        };

                        nOnj.InOffLines.Add(nLineObj);
                    }
                }

                if (nOnj.InOffLines.Any())
                {
                    allDataTmp.Add(nOnj);
                }

                if (bw != null)
                {
                    processInt = processInt + pc;
                    if (bw.CancellationPending == true)
                    {
                        return null;
                    }

                    bw.ReportProgress((int)processInt);
                }
            }

            #region 相同日期GROUP BY
            foreach (var bySP in allDataTmp)
            {
                InOffLineList_byFabricPanelCode n = new InOffLineList_byFabricPanelCode();
                n.OrderID = bySP.OrderID;
                n.FabricPanelCode = bySP.FabricPanelCode;
                n.InOffLines = new List<InOffLine>();
                var groupData = bySP.InOffLines.GroupBy(o => new { o.DateWithLeadTime, o.CutQty, o.StdQty, o.AccuCutQty, o.AccuStdQty })
                                .Select(x => new InOffLine
                                {
                                    DateWithLeadTime = x.Key.DateWithLeadTime,
                                    CutQty = x.Key.CutQty,
                                    StdQty = x.Key.StdQty,
                                    AccuCutQty = x.Key.AccuCutQty,
                                    AccuStdQty = x.Key.AccuStdQty,
                                }).OrderBy(o => o.DateWithLeadTime).ToList();

                n.InOffLines = groupData;
                allData.Add(n);
            }
            #endregion
            return allData;
        }

        /// <summary>
        /// Cutting WIP 資料表 (所有In/Off Line資料在內部自行取得) Index 0 為Summary、1 為Detail
        /// </summary>
        /// <param name="dt">SewingSchedule的Datatable</param>
        /// <param name="days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <returns>resultList</returns>
        public static List<DataTable> GetCutting_WIP_DataTable(DataTable dt, List<Day> days)
        {
            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList> allData = new List<InOffLineList>();

            allData = GetInOffLineList(dt, days);

            DataTable detailDt = new DataTable();
            DataTable summaryDt = new DataTable();

            detailDt.ColumnsStringAdd("SP");
            detailDt.ColumnsStringAdd("Desc./Sewing Date");
            summaryDt.ColumnsStringAdd("SP");

            // 日期Column
            int idx = 3;
            foreach (var day in days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                idx++;
            }

            if (allData == null)
            {
                return null;
            }

            int orderCount = allData.Count;

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
            foreach (var bySP in allData)
            {
                foreach (var item in bySP.InOffLines)
                {
                    foreach (var day in days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        detailDt.Rows[index * 4]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 1]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 2]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 3]["SP"] = bySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            detailDt.Rows[index * 4][strDate] = item.CutQty;
                            detailDt.Rows[(index * 4) + 1][strDate] = item.StdQty;
                            detailDt.Rows[(index * 4) + 2][strDate] = item.AccuCutQty;
                            detailDt.Rows[(index * 4) + 3][strDate] = item.AccuStdQty;
                        }
                        else
                        {
                            detailDt.Rows[index * 4][strDate] = DBNull.Value;
                            detailDt.Rows[(index * 4) + 1][strDate] = DBNull.Value;
                            detailDt.Rows[(index * 4) + 2][strDate] = DBNull.Value;
                            detailDt.Rows[(index * 4) + 3][strDate] = DBNull.Value;
                        }
                    }
                }

                index++;
            }

            index = 0;
            foreach (var bySP in allData)
            {
                foreach (var item in bySP.InOffLines)
                {
                    int dayIndex = 0;
                    foreach (var day in days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[index]["SP"] = bySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            int cutQty = item.CutQty;
                            int stdQty = item.StdQty;
                            int accuCutQty = item.AccuCutQty;
                            int accuStdQty = item.AccuStdQty;
                            decimal cellValue = 0;
                            if (accuCutQty <= accuStdQty)
                            {
                                cellValue = stdQty == 0 ? 0 : (accuCutQty - accuStdQty) / stdQty;
                            }
                            else if (stdQty > 0 && ((accuCutQty - accuStdQty) / stdQty) <= 1)
                            {
                                Day nextDay = new Day();

                                // /以隔天為起點，開始找下一個非假日，
                                for (int i = dayIndex + 1; i <= days.Count - 1; i++)
                                {
                                    // 取得下一個不是假日的日期
                                    if (!days[i].IsHoliday)
                                    {
                                        nextDay.Date = days[i].Date;
                                        break;
                                    }
                                }

                                var findData_nextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                bool hasNextDayData = findData_nextDay.Any();

                                // 若沒有下一天的資料，則全部視作0 （下一天不一定是明天日期）
                                if (!hasNextDayData)
                                {
                                    cellValue = 0;
                                }
                                else
                                {
                                    int nextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                    cellValue = nextDayStdQty == 0 ? 0 : (accuCutQty - accuStdQty) / nextDayStdQty;
                                }
                            }
                            else
                            {
                                Day nextDay = new Day();
                                Day nexNextDay = new Day();
                                bool hasNextDay = false;
                                bool hasNexNextDay = false;

                                // /以隔天為起點，開始找下一個非假日
                                for (int i = dayIndex + 1; i <= days.Count - 1; i++)
                                {
                                    // 取得下一個不是假日的日期
                                    if (!days[i].IsHoliday)
                                    {
                                        nextDay.Date = days[i].Date;
                                        hasNextDay = true;
                                    }

                                    if (hasNextDay)
                                    {
                                        // 再以這個非假日為起點，找到下一個非假日
                                        for (int y = i + 1; y <= days.Count - 1; y++)
                                        {
                                            if (!days[y].IsHoliday)
                                            {
                                                nexNextDay.Date = days[y].Date;
                                                hasNexNextDay = true;

                                                // 找完就迴圈掰掰
                                                break;
                                            }
                                        }
                                    }

                                    if (hasNexNextDay)
                                    {
                                        break;
                                    }
                                }

                                var findData_nextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                var findData_nextNextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nexNextDay.Date);
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
                                    int nextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                    int nextNextDayStdQty = findData_nextNextDay.Sum(o => o.StdQty);

                                    cellValue = nextNextDayStdQty == 0 ? 0 : 1 + ((accuCutQty - accuStdQty - nextDayStdQty) / nextNextDayStdQty);
                                }
                            }

                            summaryDt.Rows[index][strDate] = cellValue;
                        }
                        else
                        {
                            summaryDt.Rows[index][strDate] = DBNull.Value;
                        }

                        dayIndex++;
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
        /// <param name="days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <param name="allData">In/Off Line資料</param>
        /// <returns>allData</returns>
        public static List<DataTable> GetCutting_WIP_DataTable(List<Day> days, List<InOffLineList> allData)
        {
            List<DataTable> resultList = new List<DataTable>();

            DataTable detailDt = new DataTable();
            DataTable summaryDt = new DataTable();

            detailDt.ColumnsStringAdd("SP");
            detailDt.ColumnsStringAdd("Desc./Sewing Date");
            summaryDt.ColumnsStringAdd("SP");

            // 日期Column
            int idx = 3;
            foreach (var day in days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                idx++;
            }

            if (allData == null)
            {
                return null;
            }

            int orderCount = allData.Count;

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
            foreach (var bySP in allData.OrderBy(o => o.OrderID).ToList())
            {
                if (bySP.OrderID == "20031214AB")
                {
                }

                foreach (var item in bySP.InOffLines)
                {
                    foreach (var day in days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        detailDt.Rows[index * 4]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 1]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 2]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 3]["SP"] = bySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            detailDt.Rows[index * 4][strDate] = item.CutQty;
                            detailDt.Rows[(index * 4) + 1][strDate] = item.StdQty;
                            detailDt.Rows[(index * 4) + 2][strDate] = item.AccuCutQty;
                            detailDt.Rows[(index * 4) + 3][strDate] = item.AccuStdQty;
                        }
                        else
                        {
                            // detailDt.Rows[(index * 4)][strDate] = DBNull.Value;
                            // detailDt.Rows[(index * 4) + 1][strDate] = DBNull.Value;
                            // detailDt.Rows[(index * 4) + 2][strDate] = DBNull.Value;
                            // detailDt.Rows[(index * 4) + 3][strDate] = DBNull.Value;
                        }
                    }
                }

                index++;
            }

            index = 0;
            foreach (var bySP in allData)
            {
                if (bySP.OrderID == "MAILO20030015")
                {
                }

                foreach (var item in bySP.InOffLines)
                {
                    // 紀錄時間軸上的Index
                    int dayIndex = 0;
                    foreach (var day in days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[index]["SP"] = bySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            if (item.DateWithLeadTime == day.Date)
                            {
                                if (!day.IsHoliday)
                                {
                                    decimal cutQty = item.CutQty;
                                    decimal stdQty = item.StdQty;
                                    decimal accuCutQty = item.AccuCutQty;
                                    decimal accuStdQty = item.AccuStdQty;
                                    decimal cellValue = 0;

                                    #region 準備下一天 嚇嚇一天需要資料 （下一天不一定是明天日期）
                                    Day nextDay = new Day();
                                    Day nexNextDay = new Day();
                                    bool hasNextDay = false;
                                    bool hasNexNextDay = false;

                                    // /以隔天為起點，開始找下一個非假日
                                    for (int i = dayIndex + 1; i <= days.Count - 1; i++)
                                    {
                                        // 取得下一個不是假日的日期
                                        if (!days[i].IsHoliday)
                                        {
                                            nextDay.Date = days[i].Date;
                                            hasNextDay = true;
                                        }

                                        if (hasNextDay)
                                        {
                                            // 再以這個非假日為起點，找到下一個非假日
                                            for (int y = i + 1; y <= days.Count - 1; y++)
                                            {
                                                if (!days[y].IsHoliday)
                                                {
                                                    nexNextDay.Date = days[y].Date;
                                                    hasNexNextDay = true;

                                                    // 找完就迴圈掰掰
                                                    break;
                                                }
                                            }
                                        }

                                        if (hasNexNextDay)
                                        {
                                            break;
                                        }
                                    }

                                    var findData_nextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                    var findData_nextNextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nexNextDay.Date);
                                    bool hasNextDayData = findData_nextDay.Any();
                                    bool hasNextNextDayData = findData_nextNextDay.Any();
                                    int nextDayStdQty = 0;
                                    if (hasNextDayData)
                                    {
                                        nextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                    }
                                    #endregion

                                    if (accuCutQty <= accuStdQty)
                                    {
                                        cellValue = stdQty == 0 ? 0 : (accuCutQty - accuStdQty) / stdQty;
                                    }
                                    else if (stdQty == 0 || nextDayStdQty == 0 || ((accuCutQty - accuStdQty) / nextDayStdQty) <= 1)
                                    {
                                        // 若沒有下一天的資料，則全部視作0 （下一天不一定是明天日期）
                                        cellValue = nextDayStdQty == 0 ? 0 : (accuCutQty - accuStdQty) / nextDayStdQty;
                                    }
                                    else
                                    {
                                        int nextNextDayStdQty = 0;
                                        if (hasNextNextDayData)
                                        {
                                            nextNextDayStdQty = findData_nextNextDay.Sum(o => o.StdQty);
                                        }

                                        // 沒意外應該只有一筆，不過還是用SUM
                                        cellValue = nextNextDayStdQty == 0 ? 1 : 1 + ((accuCutQty - accuStdQty - nextDayStdQty) / nextNextDayStdQty);
                                    }

                                    summaryDt.Rows[index][strDate] = cellValue;
                                }
                                else
                                {
                                    // summaryDt.Rows[(index)][strDate] = DBNull.Value;
                                }
                            }
                            else
                            {
                                // summaryDt.Rows[(index)][strDate] = DBNull.Value;
                            }
                        }
                        else
                        {
                            // summaryDt.Rows[(index)][strDate] = DBNull.Value;
                        }

                        dayIndex++;
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
        /// <param name="days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <param name="allData">In/Off Line資料</param>
        /// <returns>result List</returns>
        public static List<DataTable> GetCutting_WIP_DataTable(List<Day> days, List<InOffLineList_byFabricPanelCode> allData)
        {
            List<DataTable> resultList = new List<DataTable>();

            DataTable detailDt = new DataTable();
            DataTable summaryDt = new DataTable();

            detailDt.ColumnsStringAdd("SP");
            detailDt.ColumnsStringAdd("Desc./Sewing Date");
            detailDt.ColumnsStringAdd("Fab. Panel Code");
            summaryDt.ColumnsStringAdd("SP");
            summaryDt.ColumnsStringAdd("Fab. Panel Code");

            // 日期Column
            int idx = 3;
            foreach (var day in days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                idx++;
            }

            if (allData == null)
            {
                return null;
            }

            int orderCount = allData.Count;

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
            foreach (var bySP in allData)
            {
                if (bySP.OrderID == "20031214AB")
                {
                }

                foreach (var item in bySP.InOffLines)
                {
                    foreach (var day in days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        detailDt.Rows[index * 4]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 1]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 2]["SP"] = bySP.OrderID;
                        detailDt.Rows[(index * 4) + 3]["SP"] = bySP.OrderID;

                        detailDt.Rows[index * 4]["Fab. Panel Code"] = bySP.FabricPanelCode;
                        detailDt.Rows[(index * 4) + 1]["Fab. Panel Code"] = bySP.FabricPanelCode;
                        detailDt.Rows[(index * 4) + 2]["Fab. Panel Code"] = bySP.FabricPanelCode;
                        detailDt.Rows[(index * 4) + 3]["Fab. Panel Code"] = bySP.FabricPanelCode;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            detailDt.Rows[index * 4][strDate] = item.CutQty;
                            detailDt.Rows[(index * 4) + 1][strDate] = item.StdQty;
                            detailDt.Rows[(index * 4) + 2][strDate] = item.AccuCutQty;
                            detailDt.Rows[(index * 4) + 3][strDate] = item.AccuStdQty;
                        }
                        else
                        {
                            // detailDt.Rows[(index * 4)][strDate] = DBNull.Value;
                            // detailDt.Rows[(index * 4) + 1][strDate] = DBNull.Value;
                            // detailDt.Rows[(index * 4) + 2][strDate] = DBNull.Value;
                            // detailDt.Rows[(index * 4) + 3][strDate] = DBNull.Value;
                        }
                    }
                }

                index++;
            }

            index = 0;
            foreach (var bySP in allData)
            {
                if (bySP.OrderID == "20080032AB001")
                {
                }

                foreach (var item in bySP.InOffLines)
                {
                    // 紀錄時間軸上的Index
                    int dayIndex = 0;
                    foreach (var day in days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[index]["SP"] = bySP.OrderID;
                        summaryDt.Rows[index]["Fab. Panel Code"] = bySP.FabricPanelCode;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            if (!day.IsHoliday)
                            {
                                decimal cutQty = item.CutQty;
                                decimal stdQty = item.StdQty;
                                decimal accuCutQty = item.AccuCutQty;
                                decimal accuStdQty = item.AccuStdQty;
                                decimal cellValue = 0;

                                #region 準備下一天 嚇嚇一天需要資料 （下一天不一定是明天日期）
                                Day nextDay = new Day();
                                Day nexNextDay = new Day();
                                bool hasNextDay = false;
                                bool hasNexNextDay = false;

                                // /以隔天為起點，開始找下一個非假日
                                for (int i = dayIndex + 1; i <= days.Count - 1; i++)
                                {
                                    // 取得下一個不是假日的日期
                                    if (!days[i].IsHoliday)
                                    {
                                        nextDay.Date = days[i].Date;
                                        hasNextDay = true;
                                    }

                                    if (hasNextDay)
                                    {
                                        // 再以這個非假日為起點，找到下一個非假日
                                        for (int y = i + 1; y <= days.Count - 1; y++)
                                        {
                                            if (!days[y].IsHoliday)
                                            {
                                                nexNextDay.Date = days[y].Date;
                                                hasNexNextDay = true;

                                                // 找完就迴圈掰掰
                                                break;
                                            }
                                        }
                                    }

                                    if (hasNexNextDay)
                                    {
                                        break;
                                    }
                                }

                                var findData_nextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                var findData_nextNextDay = bySP.InOffLines.Where(o => o.DateWithLeadTime == nexNextDay.Date);
                                bool hasNextDayData = findData_nextDay.Any();
                                bool hasNextNextDayData = findData_nextNextDay.Any();
                                int nextDayStdQty = 0;
                                if (hasNextDayData)
                                {
                                    nextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                }
                                #endregion

                                if (accuCutQty <= accuStdQty)
                                {
                                    cellValue = stdQty == 0 ? 0 : (accuCutQty - accuStdQty) / stdQty;
                                }
                                else if (stdQty == 0 || nextDayStdQty == 0 || ((accuCutQty - accuStdQty) / nextDayStdQty) <= 1)
                                {
                                    // 若沒有下一天的資料，則全部視作0 （下一天不一定是明天日期）
                                    cellValue = nextDayStdQty == 0 ? 0 : (accuCutQty - accuStdQty) / nextDayStdQty;
                                }
                                else
                                {
                                    int nextNextDayStdQty = 0;
                                    if (hasNextNextDayData)
                                    {
                                        nextNextDayStdQty = findData_nextNextDay.Sum(o => o.StdQty);
                                    }

                                    // 沒意外應該只有一筆，不過還是用SUM
                                    cellValue = nextNextDayStdQty == 0 ? 1 : 1 + ((accuCutQty - accuStdQty - nextDayStdQty) / nextNextDayStdQty);
                                }

                                summaryDt.Rows[index][strDate] = cellValue;
                            }
                            else
                            {
                                // summaryDt.Rows[(index)][strDate] = DBNull.Value;
                            }
                        }
                        else
                        {
                            // summaryDt.Rows[(index)][strDate] = DBNull.Value;
                        }

                        dayIndex++;
                    }
                }

                index++;
            }

            resultList.Add(summaryDt);
            resultList.Add(detailDt);

            return resultList;
        }

        /// <summary>
        /// Get Lead Time List
        /// </summary>
        /// <param name="orderIDs">orders ID</param>
        /// <param name="annotationStr">annotation Str</param>
        /// <returns>leadTimeList</returns>
        public static List<LeadTime> GetLeadTimeList(List<string> orderIDs, out string annotationStr)
        {
            List<LeadTime> leadTimeList = new List<LeadTime>();

            DataTable poID_dt;
            DataTable garmentTb;
            DataTable leadTime_dt;
            DualResult result;
            annotationStr = string.Empty;

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
            result = DBProxy.Current.Select(null, cmd, out poID_dt);
            if (!result)
            {
                return null;
            }

            List<string> msg = new List<string>();

            foreach (DataRow dr in poID_dt.Rows)
            {
                string pOID = dr["POID"].ToString();
                string orderID = dr["OrderID"].ToString();
                string mDivisionID = dr["MDivisionID"].ToString();
                string factoryID = dr["FactoryID"].ToString();

                GetGarmentListTable(string.Empty, pOID, string.Empty, out garmentTb);

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
                            int x = 0;
                            if (!int.TryParse(item[i].ToString(), out x))
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

                string annotationStr1 = annotationList_Final.OrderBy(o => o.ToString()).JoinToString("+");
                annotationStr = annotationStr1;

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
WHERE Subprocess.IDs = '{annotationStr1}'
and s.MDivisionID = '{mDivisionID}'
and s.FactoryID = '{factoryID}'
";
                result = DBProxy.Current.Select(null, chk_LeadTime, out leadTime_dt);
                if (!result)
                {
                    return null;
                }

                // 收集需要顯示訊息的Subprocess ID
                if (leadTime_dt.Rows.Count == 0 && annotationStr1 != string.Empty)
                {
                    msg.Add(mDivisionID + ";" + factoryID + ";" + annotationStr1);
                }
                else
                {
                    // 記錄下加工段的Lead Time
                    LeadTime o = new LeadTime()
                    {
                        OrderID = orderID,
                        LeadTimeDay = MyUtility.Check.Empty(annotationStr1) ? 0 : Convert.ToInt32(leadTime_dt.Rows[0]["LeadTime"]), // 加工段為空，LeadTimeDay = 0
                        Subprocess = annotationStr1,
                    };
                    leadTimeList.Add(o);
                }
            }

            return leadTimeList;
        }

        public static int GetStdQtyByDate(string orderID, DateTime sewingDate)
        {
            string sqlcmd = $@"
SELECT s.ComboType, x.APSNo,x.Date
	,StdQ = IIF(SUM(x.StdQ)over(partition by s.APSNo order by x.Date) > s.AlloQty
				, iif(x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty)<0,0,x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty))
				, x.StdQ)
into #currBase
FROM SewingSchedule  s
outer apply(select * from [dbo].[getDailystdq](s.APSNo))x
WHERE OrderID='{orderID}'

---- 該日期之前
SELECT ComboType, [StdQ]=SUM(StdQ)
INTO #beforeTmp
FROM #currBase
where Date < '{sewingDate.ToString("yyyy/MM/dd")}'
GROUP BY ComboType

---- 該日期當天
SELECT ComboType, [StdQ]=SUM(StdQ)
INTO #today
FROM #currBase
where Date = '{sewingDate.ToString("yyyy/MM/dd")}'
GROUP BY ComboType

---- 計算之前剩下的裁片數
---- 判斷不同部位相差多少數量，即是剩餘的裁片，並且把比較多的那個數量記下來，此時還不知道是哪個部位
SELECT [LeftQty]=MAX(StdQ) - MIN(StdQ) ,[MaxStdQ]=MAX(StdQ) 
INTO #tmp
FROM (
	SELECT  s.Location ,[StdQ]=ISNULL(u.StdQ,0)
	FROM Orders o
	INNER JOIN Style_Location s ON o.StyleUkey = s.StyleUkey
	LEFT JOIN #beforeTmp u ON u.ComboType = s.Location
	WHERE o.ID='{orderID}'
)a


---- 回去「該日期之前」，用比較多的數量，回去找是哪個部位，如果會找到兩個一樣的數量，表示沒有剩餘的裁片，都剛好成套
SELECT b.ComboType,t.LeftQty
INTO #before
FROM #beforeTmp b
INNER JOIN #tmp t ON b.StdQ=t.MaxStdQ
WHERE t.LeftQty > 0

---- 計算當天 + 之前剩餘的裁片數
SELECT ComboType,[StdQ]=SUM(StdQ)
INTO #sum
FROM (
	SELECT * FROM #today 
	UNION
	SELECT * FROM #before
)a
GROUP BY ComboType

---- 取裁片數最少的 = 成套件數
SELECT MIN( ISNULL(u.StdQ,0))
FROM Orders o
INNER JOIN Style_Location s ON o.StyleUkey = s.StyleUkey
LEFT JOIN #sum u ON u.ComboType = s.Location
WHERE o.ID='{orderID}'


DROP TABLE #today,#beforeTmp,#before,#sum,#tmp,#currBase
";
            string stdQty = MyUtility.GetValue.Lookup(sqlcmd);
            int rtn = MyUtility.Check.Empty(stdQty) ? 0 : Convert.ToInt32(stdQty);
            return rtn;
        }

        /// <summary>
        /// Get Accu StdQty By Date
        /// </summary>
        /// <param name="orderID">order ID</param>
        /// <param name="beforeSewingDate">before SewingDate</param>
        /// <returns>accuStdQty</returns>
        public static int GetAccuStdQtyByDate(string orderID, DateTime beforeSewingDate)
        {
            string sqlcmd = $@"
SELECT s.OrderID,s.ComboType, x.APSNo,x.Date
	,StdQ = IIF(SUM(x.StdQ)over(partition by s.APSNo order by x.Date) > s.AlloQty
				, iif(x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty)<0,0,x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty))
				, x.StdQ)
into #currBase
FROM SewingSchedule  s
outer apply(select * from [dbo].[getDailystdq](s.APSNo))x
WHERE OrderID='{orderID}'

SELECT ComboType, [StdQ]=SUM(StdQ)
INTO #beforeTmp
FROM #currBase
WHERE OrderID='{orderID}' and Date <= '{beforeSewingDate.ToString("yyyy/MM/dd")}'
group by ComboType

---- 取裁片數最少的 = 成套件數
SELECT MIN( ISNULL(u.StdQ,0))
FROM Orders o with(nolock)
INNER JOIN Style_Location s with(nolock) ON o.StyleUkey = s.StyleUkey
LEFT JOIN #beforeTmp u ON u.ComboType = s.Location
WHERE o.ID='{orderID}'

DROP TABLE #beforeTmp
";
            string accuStdQty = MyUtility.GetValue.Lookup(sqlcmd);
            int rtn = MyUtility.Check.Empty(accuStdQty) ? 0 : Convert.ToInt32(accuStdQty);
            return rtn;
        }

        #region 類別

        /// <summary>
        /// 該訂單，在該日期成套的數量
        /// </summary>
        public class GarmentQty
        {
            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// EstCutDate
            /// </summary>
            public DateTime EstCutDate { get; set; }

            /// <summary>
            /// Qty
            /// </summary>
            public int Qty { get; set; }
        }

        /// <summary>
        /// 該訂單，在該日期成套的數量
        /// </summary>
        public class FabricPanelCodeCutPlanQty
        {
            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// EstCutDate
            /// </summary>
            public DateTime EstCutDate { get; set; }

            /// <summary>
            /// FabricPanelCode
            /// </summary>
            public string FabricPanelCode { get; set; }

            /// <summary>
            /// Qty
            /// </summary>
            public int Qty { get; set; }
        }

        /// <summary>
        /// 一件成衣，由哪些部位組成
        /// </summary>
        /*public class GarmentList
        {
            public DateTime EstCutDate { get; set; }
            // 是否缺部位，因此不成套
            public bool IsPanelShortage { get; set; }
            public string OrderID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public string PatternPanel { get; set; }
            public string FabricPanelCode { get; set; }

            //Delete
            public List<Panel> Panels { get; set; }
            public List<PanelQty> PanelQtys { get; set; }
        }*/

        /// <summary>
        /// 大部位名
        /// </summary>
        /*public class Panel
        {
            /// <summary>
            /// 大部位
            /// </summary>
            public string PatternPanel { get; set; }
            public string FabricPanelCodess { get; set; }
            public DateTime EstCutDate { get; set; }
            public int Qty { get; set; }

            /// <summary>
            /// 該大部位內的小部位
            /// </summary>
            public List<PanelCode> FabricPanelCodes { get; set; }
        }
        */

        /// <summary>
        /// 時間軸基礎物件
        /// </summary>
        public class Day
        {
            /// <summary>
            /// Date
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// IsHoliday
            /// </summary>
            public bool IsHoliday { get; set; }
        }

        /// <summary>
        /// OrderID對應的LeadTime
        /// </summary>
        public class LeadTime
        {
            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// LeadTimeDay
            /// </summary>
            public int LeadTimeDay { get; set; }

            /// <summary>
            /// Subprocess
            /// </summary>
            public string Subprocess { get; set; }
        }

        /// <summary>
        /// In Off Line List
        /// </summary>
        public class InOffLineList
        {
            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// IsDateMove
            /// </summary>
            public bool IsDateMove { get; set; }

            /// <summary>
            /// InOffLines
            /// </summary>
            public List<InOffLine> InOffLines { get; set; }
        }

        /// <summary>
        /// In Off LineList by Fabric PanelCode
        /// </summary>
        public class InOffLineList_byFabricPanelCode
        {
            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// FabricPanelCode
            /// </summary>
            public string FabricPanelCode { get; set; }

            /// <summary>
            /// IsDateMove
            /// </summary>
            public bool IsDateMove { get; set; }

            /// <summary>
            /// InOffLines
            /// </summary>
            public List<InOffLine> InOffLines { get; set; }
        }

        /// <summary>
        /// In Off Line
        /// </summary>
        public class InOffLine
        {
            /// <summary>
            /// UKey
            /// </summary>
            public int? UKey { get; set; }

            /// <summary>
            /// ApsNO
            /// </summary>
            public string ApsNO { get; set; }

            /// <summary>
            /// CutQty
            /// </summary>
            public int CutQty { get; set; }

            /// <summary>
            /// AccuCutQty
            /// </summary>
            public int AccuCutQty { get; set; }

            /// <summary>
            /// StdQty
            /// </summary>
            public int StdQty { get; set; }

            /// <summary>
            /// AccuStdQty
            /// </summary>
            public int AccuStdQty { get; set; }

            /// <summary>
            /// DateWithLeadTime
            /// </summary>
            public DateTime DateWithLeadTime { get; set; }

            /// <summary>
            /// WIP
            /// </summary>
            public decimal WIP { get; set; }

            /// <summary>
            /// overAlloQty
            /// </summary>
            public bool OverAlloQty { get; set; }
        }

        /// <summary>
        /// Daily Std Qty
        /// </summary>
        public class DailyStdQty
        {
            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// ComboType
            /// </summary>
            public string ComboType { get; set; }

            /// <summary>
            /// APSNo
            /// </summary>
            public string APSNo { get; set; }

            /// <summary>
            /// Date
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// StdQty
            /// </summary>
            public int StdQty { get; set; }
        }
        #endregion
        #endregion
    }
}