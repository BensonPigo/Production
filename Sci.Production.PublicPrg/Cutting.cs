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
        /// 均分數量 EX:10均分4份→3,3,2,2
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <param name="TotalNumeric"></param>
        /// <param name="deleteZero"></param>
        public static void AverageNumeric(DataRow[] dr, string columnName = "Qty", int TotalNumeric = 0, bool deleteZero = false)
        {
            if (dr.Count() == 0) return;
            int rowCount = dr.Count();
            int eachqty = TotalNumeric / rowCount;
            int modqty = TotalNumeric % rowCount; //剩餘數

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
                        if (modqty > 0) dr2[columnName] = eachqty + 1;//每組分配一個Qty 當分配完表示沒了
                        else dr2[columnName] = eachqty;
                        modqty--; //剩餘數一定小於rowcount所以會有筆數沒有拿到
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

        public static List<Day> GetDays(int leadtime, DateTime date_where, List<string> FtyFroup)
        {
            DateTime start = date_where.AddDays(-leadtime).Date;

            List<Day> DayList = new List<Day>();

            string sqlcmd = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{date_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{FtyFroup.JoinToString("','")}')
";
            DataTable dt2;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt2);
            // 開始組合時間軸
            for (int Day = 0; Day <= leadtime; Day++)
            {
                Day day = new Day();
                day.Date = date_where.AddDays(-Day).Date;

                // 是否行事曆設定假日
                bool IsHoliday = dt2.AsEnumerable().Where(o => MyUtility.Convert.GetDate(o["HolidayDate"]) == day.Date).Any();
                // 是行事曆設定假日 or 星期天
                if (IsHoliday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    IsHoliday = true;

                    // 為避免假日推移的影響，讓時間軸不夠長，因此每遇到一次假日，就要加長一次時間軸
                    leadtime++;
                    start = start.AddDays(-1);

                    sqlcmd = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{start.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{date_where.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{FtyFroup.JoinToString("','")}')
";
                    DBProxy.Current.Select(null, sqlcmd, out dt2);
                }

                day.IsHoliday = IsHoliday;
                DayList.Add(day);
            }

            return DayList;
        }

        public static List<Day> GetRangeHoliday(DateTime date1, DateTime date2, List<string> FtyFroup)
        {
            List<Day> DayList = new List<Day>();

            string sqlcmd = $@"
SELECT FactoryID ,[HolidayDate] = Cast(HolidayDate as Date)
FROM Holiday WITH(NOLOCK)
WHERE HolidayDate >= '{date1.ToString("yyyy/MM/dd")}'
AND HolidayDate <= '{date2.ToString("yyyy/MM/dd")}'
AND FactoryID IN ('{FtyFroup.JoinToString("','")}')
";
            DataTable dt2;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt2);

            int days =  (int)(date2.Date - date1.Date).TotalDays;
            for (int Day = 0; Day <= days; Day++)
            {
                Day day = new Day();
                day.Date = date2.AddDays(-Day).Date;

                // 是否行事曆設定假日
                bool IsHoliday = dt2.AsEnumerable().Where(o => MyUtility.Convert.GetDate(o["HolidayDate"]) == day.Date).Any();
                // 是行事曆設定假日 or 星期天
                if (IsHoliday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    IsHoliday = true;
                }

                day.IsHoliday = IsHoliday;
                DayList.Add(day);
            }

            return DayList;
        }

        /// <summary>
        /// 取得 by APSNo & 每日 的標準數
        /// </summary>
        /// <param name="OrderIDs"></param>
        /// <returns></returns>
        public static List<DailyStdQty> GetStdQty(List<string> OrderIDs)
        {
            string sqlcmd = $@"
SELECT s.OrderID,s.ComboType,x.*
FROM SewingSchedule  s
outer apply(select * from [dbo].[getDailystdq](s.APSNo))x
WHERE OrderID IN ('{OrderIDs.JoinToString("','")}')
and x.APSNo is not null
";
            DataTable tmpDt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out tmpDt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }

            List<DailyStdQty> APSNoDailyStdQty = tmpDt.AsEnumerable().Select(s => new DailyStdQty
            {
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                APSNo = MyUtility.Convert.GetString(s["APSNo"]),
                Date = Convert.ToDateTime(s["Date"]).Date,
                StdQty = MyUtility.Convert.GetInt(s["StdQ"])
            }).ToList();

            return APSNoDailyStdQty;
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
            string Marker5_Group = string.Empty;
            foreach (DataRow row1 in dtf2.Rows)
            {
                string curr_Marker5_Group = MyUtility.Convert.GetString(row1["MarkerName"]).Substring(0, 5);
                if (curr_Marker5_Group != Marker5_Group && i > 0)
                {
                    DataRow newrow = dtf3.NewRow();
                    newrow["TypeofCuttingNode"] = MyUtility.Convert.GetString(row1["TypeofCuttingNode_Group"]);
                    dtf3.Rows.Add(newrow);
                }

                Marker5_Group = MyUtility.Convert.GetString(row1["MarkerName"]).Substring(0,5);

                dtf3.ImportRow(row1);
                i++;
            }

            DataRow newrowLast = dtf3.NewRow();
            newrowLast["TypeofCuttingNode"] = Marker5_Group;
            dtf3.Rows.Add(newrowLast);
            dtf3.Columns.Remove("TypeofCuttingNode_Group");
            #endregion
            // Total
            DataRow newrowTotal = dtf3.NewRow();
            newrowTotal["MarkerName"] = "Total";
            newrowTotal["CutQty"] = dtf3.Compute("sum(CutQty)",string.Empty);
            newrowTotal["Cons"] = dtf3.Compute("sum(Cons)", string.Empty);
            dtf3.Rows.Add(newrowTotal);
            return dtf3;
        }
        #endregion

        #region Cal WIP        
        /// <summary>
        /// 取得Cutting成套的數量
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static List<GarmentQty> GetCutPlanQty(List<string> OrderIDs)
        {

            DataTable HeadDt;
            DataTable tmpDt;
            DualResult result;

            //取得該訂單的組成
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
AND o.id IN ('{OrderIDs.JoinToString("','")}')
AND (exists(select 1 from Order_EachCons_Article oea where  oea.Id = o.POID and oea.Article = oq.Article )
	or not exists (select 1 from Order_EachCons_Article oea where  oea.Id = o.POID)
)
--AND o.id='20032468LL004'
";

            result = DBProxy.Current.Select(null, tmpCmd, out HeadDt);
            var headList = HeadDt.AsEnumerable()
                .Select(s => new
                {
                    OrderID =  MyUtility.Convert.GetString(s["OrderID"]),
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
WHERE WOD.OrderID IN ('{OrderIDs.JoinToString("','")}')
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

            List<GarmentQty> GarmentQtys = new List<GarmentQty>();

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
                int Qty = 0;

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
                            .Select(s => new { s.Key.FabricPanelCode, sumQty = s.Sum(sum => sum.Qty) }).Min(m => m.sumQty); ;
                    }

                    if (nowFabricPanelCode.Count() == dueFabricPanelCode.Count() && nowFabricPanelCode.All(dueFabricPanelCode.Contains))
                    {
                        // 先依據部位加總, 再取最小值, 即此 EstCutDate以前 & 此 Size 可組成的成衣件數
                        int minSizeQty = preEstCutDatebySize.GroupBy(g => new { g.FabricPanelCode })
                            .Select(s => new { s.Key.FabricPanelCode, sumQty = s.Sum(sum => sum.Qty) }).Min(m => m.sumQty); ;

                        // 到此日期的成衣數 - 先前數, 所有 Size 加總
                        Qty += minSizeQty - p_minSizeQty;
                    }
                }

                if (Qty > 0)
                {
                    GarmentQty gar = new GarmentQty();
                    gar.OrderID = key.OrderID;
                    gar.EstCutDate = key.EstCutDate;
                    gar.Qty = Qty;
                    GarmentQtys.Add(gar);
                }
            }

            return GarmentQtys;
        }

        /// <summary>
        /// 取得 by FabricPanelCode 每日的 Cut Plan Qty 
        /// </summary>
        /// <param name="OrderIDs"></param>
        /// <returns></returns>
        public static List<FabricPanelCodeCutPlanQty> GetSPFabricPanelCodeList(List<string> OrderIDs)
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
AND o.id IN ('{OrderIDs.JoinToString("','")}')
order by o.ID,cons.FabricPanelCode
";
            #endregion

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }

            List<FabricPanelCodeCutPlanQty> FabricPanelCodeCutPlanQty = tmpDt.AsEnumerable().Select(s => new FabricPanelCodeCutPlanQty
            {
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"])
            }).ToList();

            return FabricPanelCodeCutPlanQty;
        }

        /// <summary>
        /// 取得 by FabricPanelCode 每日的 Cut Plan Qty 
        /// </summary>
        /// <param name="OrderIDs"></param>
        /// <returns></returns>
        public static List<FabricPanelCodeCutPlanQty> GetCutPlanQty_byFabricPanelCode(List<string> OrderIDs)
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
WHERE WOD.OrderID IN ('{OrderIDs.JoinToString("','")}')
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

            List<FabricPanelCodeCutPlanQty> FabricPanelCodeCutPlanQty = tmpDt.AsEnumerable().Select(s => new FabricPanelCodeCutPlanQty
            {
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                EstCutDate = (DateTime)s["EstCutDate"],
                FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"]),
                Qty = MyUtility.Convert.GetInt(s["Qty"])
            }).ToList();

            return FabricPanelCodeCutPlanQty;
        }

        /// <summary>
        /// 取得所有In/Off Line資料 (成套數量內部自行處理)
        /// </summary>
        /// <param name="dt_SewingSchedule">SewingSchedule的Datatable</param>
        /// <param name="Days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <returns></returns>
        public static List<InOffLineList> GetInOffLineList(DataTable dt_SewingSchedule, List<Day> Days, DateTime? startdate = null, DateTime? Enddate = null, DateTime? ori_startdate = null, DateTime? ori_Enddate = null, System.ComponentModel.BackgroundWorker bw = null)
        {
            if (startdate == null)
            {
                startdate = Days.Min(m => m.Date.Date).Date;
            }
            if (Enddate == null)
            {
                Enddate = Days.Max(m => m.Date.Date).Date;
            }
            decimal processInt = 10; // 給進度條顯示值
            decimal pc = 10;
            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList> AllDataTmp = new List<InOffLineList>();
            List<InOffLineList> AllData = new List<InOffLineList>();

            List<string> allOrder = dt_SewingSchedule.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            #region LeadTimeList
            List<LeadTime> LeadTimeList = GetLeadTimeList(allOrder);
            if (LeadTimeList == null)
            {                
                return null; // 表示Lead Time有缺
            }
            #endregion

            List<DailyStdQty> StdQtyList = GetStdQty(allOrder);
            if (bw != null) { if (bw.CancellationPending == true) return null; bw.ReportProgress((int)processInt); } // 10%

            List<GarmentQty> GarmentList = GetCutPlanQty(allOrder);
            processInt = processInt + 5;
            if (bw != null) { if (bw.CancellationPending == true) return null; bw.ReportProgress((int)processInt); } // 15%

            if (allOrder.Count > 0)
            {
                pc = (decimal)70 / allOrder.Count; // 此迴圈佔 70% → 85%
            }

            // 處理不同 OrderID
            foreach (string OrderID in allOrder)
            {
                if (OrderID == "20042084GG002")
                {
                }

                // 此OrderID的SewingSchedule資料
                var sameOrderId = dt_SewingSchedule.AsEnumerable().Where(o => o["OrderID"].ToString() == OrderID);
                // 這筆訂單的起始與結束時間
                DateTime Start = sameOrderId.Min(o => Convert.ToDateTime(o["Inline"]));
                DateTime End = sameOrderId.Max(o => Convert.ToDateTime(o["offline"]));

                InOffLineList nOnj = new InOffLineList();
                // SP#
                nOnj.OrderID = OrderID;
                nOnj.InOffLines = new List<InOffLine>();

                // 所有Order ID、以及相對應 要扣去的Lead Time(天)
                int LeadTime = LeadTimeList.Where(o => o.OrderID == OrderID).FirstOrDefault().LeadTimeDay;

                foreach (DataRow dr in sameOrderId)
                {
                    string ApsNO = dr["APSNo"].ToString();
                    int AlloQty = MyUtility.Convert.GetInt(dr["AlloQty"]); // 此 SewingSchedule 的上限

                    // 這筆 SewingSchedule 日期範圍
                    for (DateTime APSday = Convert.ToDateTime(dr["Inline"]).Date; APSday <= Convert.ToDateTime(dr["Offline"]).Date; APSday = APSday.AddDays(1))
                    {
                        // 原始日期，不在初始篩選範圍內
                        if (ori_startdate !=null && ori_Enddate !=null && (ori_startdate > APSday || ori_Enddate < APSday))
                        {
                            continue;
                        }

                        // 原始日期不存在當日標準數
                        if (!StdQtyList.Where(w => w.OrderID == OrderID && w.APSNo == ApsNO && w.Date == APSday).Any())
                        {
                            continue;
                        }

                        DateTime Pdate = APSday; // 紀錄推算後的日期
                        #region 原始日 - LeadTime 之間有多少天 Holiday  PS:時間軸 Days 傳入時範圍是剛好的
                        if (!Days.Where(w => w.Date == APSday && w.IsHoliday).Any()) // 假日不推算
                        {
                            int HolidayCount = Days.Where(w => w.Date >= APSday.AddDays(-LeadTime) && w.Date <= APSday && w.IsHoliday).Count();
                            if (HolidayCount > 0)
                            {
                                for (int i = HolidayCount; true;)
                                {
                                    int newCount = Days.Where(w => w.Date >= APSday.Date.AddDays(-i - LeadTime) && w.Date <= APSday && w.IsHoliday).Count();
                                    if (newCount > HolidayCount)
                                    {
                                        HolidayCount = newCount;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            Pdate = APSday.AddDays(-HolidayCount - LeadTime);
                        }
                        #endregion
                        // 推算後的日期，不在最終顯示範圍
                        if (startdate > Pdate || Enddate < Pdate)
                        {
                            continue;
                        }
                        // 如果這 OrderID & 這天(用推算後比較) 已經紀錄在 nOnj, 則跳過這天, 因下方標準數,裁減數是以(天)計算成套,故同天不用重複算
                        if (nOnj.InOffLines.Where(w => w.DateWithLeadTime == Pdate).Any())
                        {
                            continue;
                        }

                        // 若這筆 ApsNO(排程), 在前一圈(日)已經到達上限, 則直接換下筆 ApsNO(排程)
                        // 此處取得累計標準數, 不可用nOnj紀錄的數值, 因上個判斷同orderid同天不會做紀錄
                        int AccStdQtybyApsNO = StdQtyList.Where(w => w.OrderID == OrderID && w.APSNo == ApsNO && w.Date < APSday).Sum(s => s.StdQty);
                        if (AccStdQtybyApsNO >= AlloQty)
                        {
                            break;
                        }

                        // 當天成套
                        int StdQty = GetStdQtyByDate(OrderID, APSday);
                        // 當天之前(包含當天)成套數
                        int AccuStdQty = GetAccuStdQtyByDate(OrderID, APSday);
                        // 取裁剪數量
                        int Cutqty = GarmentList.Where(o => o.OrderID == OrderID && o.EstCutDate == Pdate).Select(s => s.Qty).FirstOrDefault();
                        // 累計裁剪量 = 先前累計裁剪量 + 當天裁剪量，因此是 <= 
                        int accuCutQty = GarmentList.Where(o => o.OrderID == OrderID && o.EstCutDate <= Pdate).Sum(o => o.Qty);

                        InOffLine nLineObj = new InOffLine()
                        {
                            DateWithLeadTime = Pdate,
                            ApsNO = ApsNO,
                            CutQty = Cutqty,
                            AccuCutQty = accuCutQty,
                            StdQty = StdQty,
                            AccuStdQty = AccuStdQty,
                        };

                        nOnj.InOffLines.Add(nLineObj);
                    }
                }
                if (nOnj.InOffLines.Any())
                {
                    AllDataTmp.Add(nOnj);
                }
                
                if (bw != null)
                {
                    processInt = processInt + pc;
                    if (bw.CancellationPending == true) return null;
                    bw.ReportProgress((int)processInt);
                }
            }

            #region 相同日期GROUP BY
            foreach (var BySP in AllDataTmp)
            {
                if (BySP.OrderID == "20040358GG")
                {
                }
                InOffLineList n = new InOffLineList();
                n.OrderID = BySP.OrderID;
                n.InOffLines = new List<InOffLine>();
                var groupData = BySP.InOffLines.GroupBy(o => new { o.DateWithLeadTime, o.CutQty, o.StdQty, o.AccuCutQty, o.AccuStdQty })
                                .Select(x => new InOffLine
                                {
                                    DateWithLeadTime = x.Key.DateWithLeadTime,
                                    CutQty = x.Key.CutQty,
                                    StdQty = x.Key.StdQty,
                                    AccuCutQty = x.Key.AccuCutQty,
                                    AccuStdQty = x.Key.AccuStdQty,
                                }).OrderBy(o => o.DateWithLeadTime).ToList();

                n.InOffLines = groupData;
                AllData.Add(n);
            }
            #endregion

            return AllData;
        }

        public static List<InOffLineList_byFabricPanelCode> GetInOffLineList_byFabricPanelCode(DataTable dt_SewingSchedule, List<Day> Days, DateTime? startdate = null, DateTime? Enddate = null, DateTime? ori_startdate = null, DateTime? ori_Enddate = null, System.ComponentModel.BackgroundWorker bw = null)
        {
            if (startdate == null)
            {
                startdate = Days.Min(m => m.Date.Date).Date;
            }
            if (Enddate == null)
            {
                Enddate = Days.Max(m => m.Date.Date).Date;
            }
            decimal processInt = 10; // 給進度條顯示值
            decimal pc = 10;

            List<DataTable> resultList = new List<DataTable>();

            List<InOffLineList_byFabricPanelCode> AllDataTmp = new List<InOffLineList_byFabricPanelCode>();
            List<InOffLineList_byFabricPanelCode> AllData = new List<InOffLineList_byFabricPanelCode>();

            List<string> allOrder = dt_SewingSchedule.AsEnumerable().Select(o => o["OrderID"].ToString()).Distinct().ToList();

            #region LeadTimeList
            List<LeadTime> LeadTimeList = GetLeadTimeList(allOrder);
            if (LeadTimeList == null)
            {
                return null; // 表示Lead Time有缺
            }
            #endregion

            List<DailyStdQty> StdQtyList = GetStdQty(allOrder);
            if (bw != null) { if (bw.CancellationPending == true) return null; bw.ReportProgress((int)processInt); } // 10%

            List<FabricPanelCodeCutPlanQty> SPFabricPanelCodeList = GetSPFabricPanelCodeList(allOrder); // 基底用來跑主迴圈 SP + FabricPanelCode

            List<FabricPanelCodeCutPlanQty> CutPlanQtyList = GetCutPlanQty_byFabricPanelCode(allOrder);
            processInt = processInt + 5;
            if (bw != null) { if (bw.CancellationPending == true) return null; bw.ReportProgress((int)processInt); } // 15%

            if (SPFabricPanelCodeList.Count > 0)
            {
                pc = (decimal)70 / SPFabricPanelCodeList.Count; // 此迴圈佔 70% → 85%
            }

            // 處理不同 OrderID, FabricPanelCode
            foreach (var item in SPFabricPanelCodeList)
            {
                string OrderID = item.OrderID;
                string FabricPanelCode = item.FabricPanelCode;

                var sameOrderId = dt_SewingSchedule.AsEnumerable().Where(o => o["OrderID"].ToString() == OrderID);

                // 這筆訂單的起始與結束時間
                DateTime Start = sameOrderId.Min(o => Convert.ToDateTime(o["Inline"]));
                DateTime End = sameOrderId.Max(o => Convert.ToDateTime(o["offline"]));

                InOffLineList_byFabricPanelCode nOnj = new InOffLineList_byFabricPanelCode();
                // SP#
                nOnj.OrderID = OrderID;
                nOnj.FabricPanelCode = FabricPanelCode;
                nOnj.InOffLines = new List<InOffLine>();

                // 所有Order ID、以及相對應 要扣去的Lead Time
                int LeadTime = LeadTimeList.Where(o => o.OrderID == OrderID).FirstOrDefault().LeadTimeDay;

                foreach (DataRow dr in sameOrderId)
                {
                    string ApsNO = dr["APSNo"].ToString();
                    int AlloQty = MyUtility.Convert.GetInt(dr["AlloQty"]); // 此 SewingSchedule 的上限

                    // 這筆 SewingSchedule 日期範圍
                    for (DateTime APSday = Convert.ToDateTime(dr["Inline"]).Date; APSday <= Convert.ToDateTime(dr["Offline"]).Date; APSday = APSday.AddDays(1))
                    {
                        // 原始日期，不在初始篩選範圍內
                        if (ori_startdate != null && ori_Enddate != null && (ori_startdate > APSday || ori_Enddate < APSday))
                        {
                            continue;
                        }

                        // 原始日期不存在當日標準數
                        if (!StdQtyList.Where(w => w.OrderID == OrderID && w.APSNo == ApsNO && w.Date == APSday).Any())
                        {
                            continue;
                        }

                        DateTime Pdate = APSday; // 紀錄推算後的日期
                        #region 原始日 - LeadTime 之間有多少天 Holiday  PS:時間軸 Days 傳入時範圍是剛好的
                        if (!Days.Where(w => w.Date == APSday && w.IsHoliday).Any()) // 假日不推算
                        {
                            int HolidayCount = Days.Where(w => w.Date >= APSday.AddDays(-LeadTime) && w.Date <= APSday && w.IsHoliday).Count();
                            if (HolidayCount > 0)
                            {
                                for (int i = HolidayCount; true;)
                                {
                                    int newCount = Days.Where(w => w.Date >= APSday.Date.AddDays(-i - LeadTime) && w.Date <= APSday && w.IsHoliday).Count();
                                    if (newCount > HolidayCount)
                                    {
                                        HolidayCount = newCount;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            Pdate = APSday.AddDays(-HolidayCount - LeadTime);
                        }
                        #endregion
                        // 推算後的日期，不在最終顯示範圍
                        if (startdate > Pdate || Enddate < Pdate)
                        {
                            continue;
                        }
                        // 如果這 OrderID & FabricPanelCode & 這天(用推算後比較) 已經紀錄在 nOnj, 則跳過這天, 因下方標準數,裁減數是以(天)計算成套,故同天不用重複算
                        if (nOnj.InOffLines.Where(w => w.DateWithLeadTime == Pdate).Any())
                        {
                            continue;
                        }

                        // 若這筆 ApsNO(排程), 在前一圈(日)已經到達上限, 則直接換下筆 ApsNO(排程)
                        // 此處取得累計標準數, 不可用nOnj紀錄的數值, 因上個判斷同orderid同天不會做紀錄
                        int AccStdQtybyApsNO = StdQtyList.Where(w => w.OrderID == OrderID && w.APSNo == ApsNO && w.Date < APSday).Sum(s => s.StdQty);
                        if (AccStdQtybyApsNO >= AlloQty)
                        {
                            break;
                        }


                        // 當天成套
                        int StdQty = GetStdQtyByDate(OrderID, APSday);
                        // 當天之前(包含當天)成套數
                        int AccuStdQty = GetAccuStdQtyByDate(OrderID, APSday);
                        // 取裁剪數量
                        int Cutqty = CutPlanQtyList.Where(o => o.OrderID == OrderID && o.FabricPanelCode == FabricPanelCode && o.EstCutDate == Pdate).Select(s => s.Qty).FirstOrDefault();
                        // 累計裁剪量 = 先前累計裁剪量 + 當天裁剪量，因此是 <= day.Date.Date
                        int accuCutQty = CutPlanQtyList.Where(o => o.OrderID == OrderID && o.FabricPanelCode == FabricPanelCode && o.EstCutDate <= Pdate).Sum(o => o.Qty);

                        InOffLine nLineObj = new InOffLine()
                        {
                            DateWithLeadTime = Pdate,
                            ApsNO = ApsNO,
                            CutQty = Cutqty,
                            AccuCutQty = accuCutQty,
                            StdQty = StdQty,
                            AccuStdQty = AccuStdQty,
                        };

                        nOnj.InOffLines.Add(nLineObj);
                    }
                }
                if (nOnj.InOffLines.Any())
                {
                    AllDataTmp.Add(nOnj);
                }

                if (bw != null)
                {
                    processInt = processInt + pc;
                    if (bw.CancellationPending == true) return null;
                    bw.ReportProgress((int)processInt);
                }
            }

            #region 相同日期GROUP BY
            foreach (var BySP in AllDataTmp)
            {
                InOffLineList_byFabricPanelCode n = new InOffLineList_byFabricPanelCode();
                n.OrderID = BySP.OrderID;
                n.FabricPanelCode = BySP.FabricPanelCode;
                n.InOffLines = new List<InOffLine>();
                var groupData = BySP.InOffLines.GroupBy(o => new { o.DateWithLeadTime, o.CutQty, o.StdQty, o.AccuCutQty, o.AccuStdQty })
                                .Select(x => new InOffLine
                                {
                                    DateWithLeadTime = x.Key.DateWithLeadTime,
                                    CutQty = x.Key.CutQty,
                                    StdQty = x.Key.StdQty,
                                    AccuCutQty = x.Key.AccuCutQty,
                                    AccuStdQty = x.Key.AccuStdQty,
                                }).OrderBy(o => o.DateWithLeadTime).ToList();

                n.InOffLines = groupData;
                AllData.Add(n);
            }
            #endregion
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

            if (AllData == null)
            {
                return null;
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

                                Day nextDay = new Day();

                                // /以隔天為起點，開始找下一個非假日，
                                for (int i = DayIndex + 1; i <= Days.Count - 1; i++)
                                {
                                    // 取得下一個不是假日的日期
                                    if (!Days[i].IsHoliday)
                                    {
                                        nextDay.Date = Days[i].Date;
                                        break;
                                    }
                                }


                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
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
                                Day nextDay = new Day();
                                Day nexNextDay = new Day();
                                bool HasNextDay = false;
                                bool HasNexNextDay = false;

                                // /以隔天為起點，開始找下一個非假日
                                for (int i = DayIndex + 1; i <= Days.Count - 1; i++)
                                {
                                    // 取得下一個不是假日的日期
                                    if (!Days[i].IsHoliday)
                                    {
                                        nextDay.Date = Days[i].Date;
                                        HasNextDay = true;
                                    }
                                    if (HasNextDay)
                                    {
                                        // 再以這個非假日為起點，找到下一個非假日
                                        for (int y = i + 1; y <= Days.Count - 1; y++)
                                        {
                                            if (!Days[y].IsHoliday)
                                            {
                                                nexNextDay.Date = Days[y].Date;
                                                HasNexNextDay = true;
                                                // 找完就迴圈掰掰
                                                break;
                                            }
                                        }
                                    }
                                    if (HasNexNextDay)
                                    {
                                        break;
                                    }
                                }

                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                var findData_nextNextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nexNextDay.Date);
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
        /// <param name="Days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <param name="AllData">In/Off Line資料</param>
        /// <returns></returns>
        public static List<DataTable> GetCutting_WIP_DataTable(List<Day> Days, List<InOffLineList> AllData)
        {
            List<DataTable> resultList = new List<DataTable>();

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

            if (AllData == null)
            {
                return null;
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
            foreach (var BySP in AllData.OrderBy(o => o.OrderID).ToList())
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
                if (BySP.OrderID == "MAILO20030015")
                {

                }
                foreach (var item in BySP.InOffLines)
                {
                    // 紀錄時間軸上的Index
                    int DayIndex = 0;
                    foreach (var day in Days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[(index)]["SP"] = BySP.OrderID;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            if (item.DateWithLeadTime == day.Date)
                            {
                                if (!day.IsHoliday)
                                {
                                    decimal CutQty = item.CutQty;
                                    decimal StdQty = item.StdQty;
                                    decimal AccuCutQty = item.AccuCutQty;
                                    decimal AccuStdQty = item.AccuStdQty;
                                    decimal cellValue = 0;

                                    #region 準備下一天 嚇嚇一天需要資料 （下一天不一定是明天日期）
                                    Day nextDay = new Day();
                                    Day nexNextDay = new Day();
                                    bool HasNextDay = false;
                                    bool HasNexNextDay = false;

                                    // /以隔天為起點，開始找下一個非假日
                                    for (int i = DayIndex + 1; i <= Days.Count - 1; i++)
                                    {
                                        // 取得下一個不是假日的日期
                                        if (!Days[i].IsHoliday)
                                        {
                                            nextDay.Date = Days[i].Date;
                                            HasNextDay = true;
                                        }
                                        if (HasNextDay)
                                        {
                                            // 再以這個非假日為起點，找到下一個非假日
                                            for (int y = i + 1; y <= Days.Count - 1; y++)
                                            {
                                                if (!Days[y].IsHoliday)
                                                {
                                                    nexNextDay.Date = Days[y].Date;
                                                    HasNexNextDay = true;
                                                    // 找完就迴圈掰掰
                                                    break;
                                                }
                                            }
                                        }
                                        if (HasNexNextDay)
                                        {
                                            break;
                                        }
                                    }

                                    var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                    var findData_nextNextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nexNextDay.Date);
                                    bool hasNextDayData = findData_nextDay.Any();
                                    bool hasNextNextDayData = findData_nextNextDay.Any();
                                    int NextDayStdQty = 0;
                                    if (hasNextDayData)
                                    {
                                        NextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                    }
                                    #endregion

                                    if (AccuCutQty <= AccuStdQty)
                                    {
                                        cellValue = StdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / StdQty;
                                    }
                                    else if (StdQty == 0 || NextDayStdQty == 0 || ((AccuCutQty - AccuStdQty) / NextDayStdQty) <= 1)
                                    {
                                        // 若沒有下一天的資料，則全部視作0 （下一天不一定是明天日期）
                                        cellValue = NextDayStdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / NextDayStdQty;
                                    }
                                    else
                                    {
                                        int NextNextDayStdQty = 0;
                                        if (hasNextNextDayData)
                                        {
                                            NextNextDayStdQty = findData_nextNextDay.Sum(o => o.StdQty);
                                        }

                                        // 沒意外應該只有一筆，不過還是用SUM
                                        cellValue = NextNextDayStdQty == 0 ? 1 : 1 + ((AccuCutQty - AccuStdQty - NextDayStdQty) / NextNextDayStdQty);
                                    }
                                    summaryDt.Rows[(index)][strDate] = cellValue;
                                }
                                else
                                {
                                    //summaryDt.Rows[(index)][strDate] = DBNull.Value;
                                }
                            }
                            else
                            {
                                //summaryDt.Rows[(index)][strDate] = DBNull.Value;
                            }
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

        /// <summary>
        /// Cutting WIP 資料表 (所有In/Off Line資料由外部傳入) Index 0 為Summary、1 為Detail
        /// </summary>
        /// <param name="Days">處理過的時間軸，可見Cutting R01報表搜尋"處理報表上橫向日期的時間軸 (扣除Lead Time)" </param>
        /// <param name="AllData">In/Off Line資料</param>
        /// <returns></returns>
        public static List<DataTable> GetCutting_WIP_DataTable(List<Day> Days, List<InOffLineList_byFabricPanelCode> AllData)
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
            foreach (var day in Days)
            {
                string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";
                detailDt.ColumnsStringAdd(strDate);
                summaryDt.ColumnsStringAdd(strDate);
                idx++;
            }

            if (AllData == null)
            {
                return null;
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

                        detailDt.Rows[(index * 4)]["Fab. Panel Code"] = BySP.FabricPanelCode;
                        detailDt.Rows[(index * 4) + 1]["Fab. Panel Code"] = BySP.FabricPanelCode;
                        detailDt.Rows[(index * 4) + 2]["Fab. Panel Code"] = BySP.FabricPanelCode;
                        detailDt.Rows[(index * 4) + 3]["Fab. Panel Code"] = BySP.FabricPanelCode;

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
                if (BySP.OrderID == "20080032AB001")
                {

                }
                foreach (var item in BySP.InOffLines)
                {
                    // 紀錄時間軸上的Index
                    int DayIndex = 0;
                    foreach (var day in Days)
                    {
                        string strDate = day.Date.ToString("MM/dd") + $"({day.Date.DayOfWeek.ToString().Substring(0, 3)}.)";

                        summaryDt.Rows[(index)]["SP"] = BySP.OrderID;
                        summaryDt.Rows[(index)]["Fab. Panel Code"] = BySP.FabricPanelCode;

                        if (item.DateWithLeadTime == day.Date)
                        {
                            if (!day.IsHoliday)
                            {
                                decimal CutQty = item.CutQty;
                                decimal StdQty = item.StdQty;
                                decimal AccuCutQty = item.AccuCutQty;
                                decimal AccuStdQty = item.AccuStdQty;
                                decimal cellValue = 0;

                                #region 準備下一天 嚇嚇一天需要資料 （下一天不一定是明天日期）
                                Day nextDay = new Day();
                                Day nexNextDay = new Day();
                                bool HasNextDay = false;
                                bool HasNexNextDay = false;

                                // /以隔天為起點，開始找下一個非假日
                                for (int i = DayIndex + 1; i <= Days.Count - 1; i++)
                                {
                                    // 取得下一個不是假日的日期
                                    if (!Days[i].IsHoliday)
                                    {
                                        nextDay.Date = Days[i].Date;
                                        HasNextDay = true;
                                    }
                                    if (HasNextDay)
                                    {
                                        // 再以這個非假日為起點，找到下一個非假日
                                        for (int y = i + 1; y <= Days.Count - 1; y++)
                                        {
                                            if (!Days[y].IsHoliday)
                                            {
                                                nexNextDay.Date = Days[y].Date;
                                                HasNexNextDay = true;
                                                // 找完就迴圈掰掰
                                                break;
                                            }
                                        }
                                    }
                                    if (HasNexNextDay)
                                    {
                                        break;
                                    }
                                }

                                var findData_nextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nextDay.Date);
                                var findData_nextNextDay = BySP.InOffLines.Where(o => o.DateWithLeadTime == nexNextDay.Date);
                                bool hasNextDayData = findData_nextDay.Any();
                                bool hasNextNextDayData = findData_nextNextDay.Any();
                                int NextDayStdQty = 0;
                                if (hasNextDayData)
                                {
                                    NextDayStdQty = findData_nextDay.Sum(o => o.StdQty);
                                }
                                #endregion

                                if (AccuCutQty <= AccuStdQty)
                                {
                                    cellValue = StdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / StdQty;
                                }
                                else if (StdQty == 0 || NextDayStdQty == 0 || ((AccuCutQty - AccuStdQty) / NextDayStdQty) <= 1)
                                {
                                    // 若沒有下一天的資料，則全部視作0 （下一天不一定是明天日期）
                                    cellValue = NextDayStdQty == 0 ? 0 : (AccuCutQty - AccuStdQty) / NextDayStdQty;
                                }
                                else
                                {
                                    int NextNextDayStdQty = 0;
                                    if (hasNextNextDayData)
                                    {
                                        NextNextDayStdQty = findData_nextNextDay.Sum(o => o.StdQty);
                                    }

                                    // 沒意外應該只有一筆，不過還是用SUM
                                    cellValue = NextNextDayStdQty == 0 ? 1 : 1 + ((AccuCutQty - AccuStdQty - NextDayStdQty) / NextNextDayStdQty);
                                }
                                summaryDt.Rows[(index)][strDate] = cellValue;
                            }
                            else
                            {
                                //summaryDt.Rows[(index)][strDate] = DBNull.Value;
                            }
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


            string cmd = $@"
SELECT  DISTINCT OrderID, s.MDivisionID, s.FactoryID
INTO #OrderList
FROM SewingSchedule s WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON s.OrderID=o.ID
WHERE o.LocalOrder = 0
AND OrderID in ('{string.Join("','", OrderIDs)}')
";

            cmd += $@"
SELECT DIStINCT  b.POID ,a.OrderID ,b.FtyGroup, a.MDivisionID, a.FactoryID
FROM #OrderList a
INNER JOIN Orders b ON a.OrderID= b.ID 

drop table #OrderList
";
            result = DBProxy.Current.Select(null, cmd, out PoID_dt);
            if (!result)
            {
                return null;
            }

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
                    return null;
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
                    LeadTimeList.Add(o);
                }
            }

            return LeadTimeList;
        }

        public static int GetStdQtyByDate(string OrderID, DateTime SewingDate)
        {
            string sqlcmd = $@"
SELECT s.ComboType, x.APSNo,x.Date
	,StdQ = IIF(SUM(x.StdQ)over(partition by s.APSNo order by x.Date) > s.AlloQty
				, iif(x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty)<0,0,x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty))
				, x.StdQ)
into #currBase
FROM SewingSchedule  s
outer apply(select * from [dbo].[getDailystdq](s.APSNo))x
WHERE OrderID='{OrderID}'

---- 該日期之前
SELECT ComboType, [StdQ]=SUM(StdQ)
INTO #beforeTmp
FROM #currBase
where Date < '{SewingDate.ToString("yyyy/MM/dd")}'
GROUP BY ComboType

---- 該日期當天
SELECT ComboType, [StdQ]=SUM(StdQ)
INTO #today
FROM #currBase
where Date = '{SewingDate.ToString("yyyy/MM/dd")}'
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
	WHERE o.ID='{OrderID}'
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
WHERE o.ID='{OrderID}'


DROP TABLE #today,#beforeTmp,#before,#sum,#tmp,#currBase
";
            string StdQty = MyUtility.GetValue.Lookup(sqlcmd);
            int rtn = MyUtility.Check.Empty(StdQty) ? 0 : Convert.ToInt32(StdQty);
            return rtn;
        }

        public static int GetAccuStdQtyByDate(string OrderID, DateTime beforeSewingDate)
        {
            string sqlcmd = $@"
SELECT s.OrderID,s.ComboType, x.APSNo,x.Date
	,StdQ = IIF(SUM(x.StdQ)over(partition by s.APSNo order by x.Date) > s.AlloQty
				, iif(x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty)<0,0,x.StdQ - (SUM(x.StdQ)over(partition by s.APSNo order by x.Date) - s.AlloQty))
				, x.StdQ)
into #currBase
FROM SewingSchedule  s
outer apply(select * from [dbo].[getDailystdq](s.APSNo))x
WHERE OrderID='{OrderID}'

SELECT ComboType, [StdQ]=SUM(StdQ)
INTO #beforeTmp
FROM #currBase
WHERE OrderID='{OrderID}' and Date <= '{beforeSewingDate.ToString("yyyy/MM/dd")}'
group by ComboType

---- 取裁片數最少的 = 成套件數
SELECT MIN( ISNULL(u.StdQ,0))
FROM Orders o with(nolock)
INNER JOIN Style_Location s with(nolock) ON o.StyleUkey = s.StyleUkey
LEFT JOIN #beforeTmp u ON u.ComboType = s.Location
WHERE o.ID='{OrderID}'

DROP TABLE #beforeTmp
";
            string AccuStdQty = MyUtility.GetValue.Lookup(sqlcmd);
            int rtn = MyUtility.Check.Empty(AccuStdQty) ? 0 : Convert.ToInt32(AccuStdQty);
            return rtn;
        }

        #region 類別

        /// <summary>
        /// 該訂單，在該日期成套的數量
        /// </summary>
        public class GarmentQty
        {
            public string OrderID { get; set; }
            public DateTime EstCutDate { get; set; }
            public int Qty { get; set; }
        }

        /// <summary>
        /// 該訂單，在該日期成套的數量
        /// </summary>
        public class FabricPanelCodeCutPlanQty
        {
            public string OrderID { get; set; }
            public DateTime EstCutDate { get; set; }
            public string FabricPanelCode { get; set; }
            public int Qty { get; set; }
        }

        /// <summary>
        /// 存放SQL撈出來資料用
        /// </summary>
        public class GarmentQty_Detail
        {
            public DateTime EstCutDate { get; set; }
            public string OrderID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public string FabricCombo { get; set; }
            public string FabricPanelCode { get; set; }
            public int Qty { get; set; }
        }


        /// <summary>
        /// 剩餘
        /// </summary>
        public class LostInfo
        {
            //public DateTime EstCutDate { get; set; }
            public string OrderID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public string FabricCombo { get; set; }
            public string FabricPanelCode { get; set; }
            public int LostQty { get; set; }
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
            public bool IsDateMove { get; set; }

            public List<InOffLine> InOffLines { get; set; }
        }

        public class InOffLineList_byFabricPanelCode
        {
            public string OrderID { get; set; }
            public string FabricPanelCode { get; set; }
            public bool IsDateMove { get; set; }

            public List<InOffLine> InOffLines { get; set; }
        }

        public class MoveDate
        {
            public string OrderID { get; set; }

            public int UKey { get; set; }
            public DateTime Ori_DateWithLeadTime { get; set; }
            public int MoveCount { get; set; }

        }

        public class InOffLine
        {
            public int? UKey { get; set; }
            public string ApsNO { get; set; }
            public int CutQty { get; set; }
            public int AccuCutQty { get; set; }
            public int StdQty { get; set; }
            public int AccuStdQty { get; set; }
            public DateTime DateWithLeadTime { get; set; }
            public decimal WIP { get; set; }

            public bool overAlloQty { get; set; }
        }

        public class OriPushDayCount
        {
            public string OrderID { get; set; }
            public DateTime OriDateWithLeadTime { get; set; }
            public int OriCount { get; set; }
        }

        public class DailyStdQty
        {
            public string OrderID { get; set; }
            public string ComboType { get; set; }
            public string APSNo { get; set; }
            public DateTime Date { get; set; }
            public int StdQty { get; set; }
        }
        #endregion
        #endregion
    }
}