using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
#pragma warning disable SA1611 // Element parameters should be documented
        /// <summary>
        /// 取得哪些 annotation 是次要
        /// </summary>
        /// <inheritdoc/>
        public static List<string> GetNotMain(DataRow dr, DataRow[] drs)
        {
            List<string> annList = new List<string>();
            if (MyUtility.Convert.GetBool(dr["Main"]))
            {
                return annList;
            }

            string[] ann = MyUtility.Convert.GetString(dr["annotation"]).Split('+'); // 剖析Annotation 不去除數字 EX:AT01

            // 每一筆 Annotation 去回找是否有標記主裁片
            foreach (string item in ann)
            {
                string anno = Regex.Replace(item, @"[\d]", string.Empty);

                // 判斷此 Annotation 在Cutting B01 是否為 IsBoundedProcess
                string sqlcmd = $@"select 1 from Subprocess with(nolock) where id = '{anno}' and IsBoundedProcess =1 ";
                bool isBoundedProcess = MyUtility.Check.Seek(sqlcmd);

                // 是否有主裁片存在
                bool hasMain = drs.AsEnumerable().
                    Where(w => MyUtility.Convert.GetString(w["annotation"]).Split('+').Contains(item) && MyUtility.Convert.GetBool(w["Main"])).Any();

                if (isBoundedProcess && !hasMain)
                {
                    annList.Add(anno); // 去除字串中數字並加入List
                }
            }

            return annList;
        }

        /// <summary>
        /// 1.先判斷 PatternCode + PatternDesc 是否存在 GarmentTb
        /// 2.判斷選擇的 Artwork  EX:選擇 AT+HT, 在PatternCode + PatternDes找到 HT+AT01, 才算此筆為 GarmentTb 內的資料
        /// 3.判斷是否為次要裁
        /// 4.篩選相同PatternCode + PatternDesc 才要判斷是否有Main再帶出是否要X
        /// </summary>
        /// <inheritdoc/>
        public static void CheckNotMain(DataRow dr, DataTable garmentTb)
        {
            DataRow[] drs = garmentTb.Select($"PatternCode='{dr["PatternCode"]}'and PatternDesc = '{dr["PatternDesc"]}'");
            if (drs.Length == 0)
            {
                dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                dr.EndEdit();
                return;
            }

            DataRow dr1 = drs[0]; // 找到也只會有一筆
            string[] ann = Regex.Replace(dr1["annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation 去除字串中數字
            string[] anns = dr["art"].ToString().Split('+'); // 剖析Annotation, 已經是去除數字

            // 兩個陣列內容要完全一樣，不管順序
            if (!Prgs.CompareArr(ann, anns))
            {
                dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                dr.EndEdit();
                return;
            }

            List<string> notMainList = Prgs.GetNotMain(dr1, drs); // 帶入未去除數字的annotation資料
            string noBundleCardAfterSubprocess_String = string.Join("+", notMainList);
            dr["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
            dr.EndEdit();
        }

        /// <summary>
        /// 尚未解析前<相同的 annotation>，須有一個為左下Grid代表。
        /// 原本是 Main 直接標記為 IsMain
        /// 若無Main 則以 seq 最小為 IsMain
        /// IsMain 最後要寫入 Bundle_Detail_CombineSubprocess.IsMain / FtyStyleInnovationCombineSubprocess.IsMain
        /// </summary>
        /// <inheritdoc/>
        public static void SetCombineSubprocessGroup_IsMain(DataRow[] garmentar)
        {
            var annotationList = garmentar.Where(w => !MyUtility.Check.Empty(w["annotation"]))
                .Select(s => MyUtility.Convert.GetString(s["annotation"])).Distinct().ToList();
            var x = garmentar.Where(w => !MyUtility.Check.Empty(w["annotation"]));

            garmentar.AsEnumerable().ToList().ForEach(f => f["IsMain"] = f["Main"]);
            foreach (string annotation in annotationList)
            {
                var x2 = x.Where(w => MyUtility.Convert.GetString(w["annotation"]) == annotation);
                var x3 = x2.OrderBy(o => MyUtility.Convert.GetString(o["Seq"])).FirstOrDefault();
                if (!x2.Where(w => MyUtility.Convert.GetBool(w["Main"])).Any() && x3 != null)
                {
                    x3["IsMain"] = true;
                }
            }

            int combineSubprocessGroup = 1;
            foreach (string annotation in annotationList)
            {
                x.Where(w => MyUtility.Convert.GetString(w["annotation"]) == annotation
                        && MyUtility.Check.Empty(w["CombineSubprocessGroup"]))
                    .ToList()
                    .ForEach(f => f["CombineSubprocessGroup"] = combineSubprocessGroup);
                combineSubprocessGroup++;
            }
        }

        /// <summary>
        /// 取得最新 SubCutNo
        /// </summary>
        /// <param name="cutRef">cutRef</param>
        /// <param name="patternPanel">patternPanel</param>
        /// <param name="fabricPanelCode">fabricPanelCode</param>
        /// <param name="cutno">cutno</param>
        /// <returns>SubCutNo</returns>
        public static string GetSubCutNo(string cutRef, string patternPanel, string fabricPanelCode, string cutno)
        {
            string sqlcmd = $@"
select top 1 b.SubCutNo
from Bundle b
where b.CutRef='{cutRef}'
and b.PatternPanel  = '{patternPanel}'
and b.FabricPanelCode = '{fabricPanelCode}'
and b.Cutno = '{cutno}'
order by len(SubCutNo) desc, SubCutNo desc
";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drc))
            {
                return MyUtility.Excel.ConvertNumericToExcelColumn(Prgs.ExcelColumnNameToNumber(drc["SubCutNo"].ToString()) + 1);
            }
            else
            {
                return string.Empty;
            }
        }

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
        /// <param name="leadtime">leadtime</param>
        /// <param name="date_where">date_where</param>
        /// <param name="ftyFroup">ftyFroup</param>
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
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt2);

            // 開始組合時間軸
            for (int day1 = 0; day1 <= leadtime; day1++)
            {
                Day day = new Day
                {
                    Date = date_where.AddDays(-day1).Date,
                };

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
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt2);

            int days = (int)(date2.Date - date1.Date).TotalDays;
            for (int day1 = 0; day1 <= days; day1++)
            {
                Day day = new Day
                {
                    Date = date2.AddDays(-day1).Date,
                };

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

        /// <inheritdoc/>
        /// <summary>
        /// 取得 by APSNo & 每日 的標準數
        /// </summary>
        /// <param name="orderIDs">orderIDs</param>
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
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable tmpDt);
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

        /// <summary>
        /// Get Cutting TapeData
        /// </summary>
        /// <param name="cuttingID">cuttingID</param>
        /// <param name="sortby">sortby</param>
        /// <returns>DataTable</returns>
        public static DataTable GetCuttingTapeData(string cuttingID, string sortby = "")
        {
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

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dt);
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

            if (sortby == "Color")
            {
                dtf2 = dtf.AsEnumerable().
                OrderBy(o => MyUtility.Convert.GetString(o["MarkerName"]).Substring(0, 5)).
                ThenBy(o => MyUtility.Convert.GetString(o["ColorID"])).
                ThenBy(o => MyUtility.Convert.GetString(o["MarkerName"])).
                ThenBy(o => MyUtility.Convert.GetString(o["Article"])).
                ThenBy(o => MyUtility.Convert.GetString(o["SizeCode"])).
                ThenBy(o => MyUtility.Convert.GetString(o["SP"])).
                CopyToDataTable();
            }

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

        /// <summary>
        /// P10 Print Data
        /// </summary>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
        public class P10_PrintData
        {
            public string Group_right { get; set; }
            public string Group_left { get; set; }
            public string CutRef { get; set; }
            public string Tone { get; set; }
            public string Line { get; set; }
            public string Cell { get; set; }
            public string POID { get; set; }
            public string SP { get; set; }
            public string Style { get; set; }
            public string MarkerNo { get; set; }
            public string Body_Cut { get; set; }
            public string Parts { get; set; }
            public string Color { get; set; }
            public string Article { get; set; }
            public string Size { get; set; }
            public string SizeSpec { get; set; }
            public string Desc { get; set; }
            public string Artwork { get; set; }
            public string Quantity { get; set; }
            public string Barcode { get; set; }
            public string Season { get; set; }
            public string Brand { get; set; }
            public string Item { get; set; }
            public string EXCESS1 { get; set; }
            public string NoBundleCardAfterSubprocess1 { get; set; }
            public string Replacement1 { get; set; }
            public string CutCell { get; set; }
            public string ShipCode { get; set; }
            public string FabricPanelCode { get; set; }
            public string Comb { get; set; }
            public string Cut { get; set; }
            public int GroupCombCut { get; set; }
            public string No { get; set; }
            public string BundleID { get; set; }
            public string BundleNo { get; set; }
            public bool RFIDScan { get; set; }
            public string Dyelot { get; set; }
            public string ID { get; set; }
            public string PatternDesc { get; set; }
        }
#pragma warning restore SA1516 // Elements should be separated by blank line
#pragma warning restore SA1600 // Elements should be documented
        #endregion
        #endregion

        /// <summary>
        /// InitialRFIDScan(ISP20210568)
        /// </summary>
        /// <inheritdoc/>
        public static DualResult InitialRFIDScan(DataTable dtPattern)
        {
            try
            {
                EnumerableRowCollection<DataRow> initialTargetRow = dtPattern.AsEnumerable();

                // 如果 該 Bundle Group , 有All Part , 那就只有 All Part 那筆 Bundle_Detail 的 RFID Scan=1 , 其餘 = 0
                var existsAllPartsRow = initialTargetRow.Where(s => s["PatternCode"].ToString() == "ALLPARTS" && MyUtility.Convert.GetInt(s["parts"]) > 0);
                if (existsAllPartsRow.Any())
                {
                    existsAllPartsRow.First()["RFIDScan"] = 1;
                    return new DualResult(true);
                }

                var excludeALLPARTS = initialTargetRow.Where(s => s["PatternCode"].ToString() != "ALLPARTS");

                // 只有ALLPARTS Parts = 0的資料不勾選
                if (!excludeALLPARTS.Any())
                {
                    return new DualResult(true);
                }

                DataRow firstRow = excludeALLPARTS.First();

                var needRFIDScan = initialTargetRow.Where(s => s["PatternCode"].ToString() == firstRow["PatternCode"].ToString());

                int rowNum = 1;

                foreach (DataRow item in needRFIDScan)
                {
                    // 如果是isPair 只需預設勾選奇數筆
                    if (rowNum % 2 == 0 && MyUtility.Convert.GetBool(item["isPair"]))
                    {
                        rowNum++;
                        continue;
                    }

                    item["RFIDScan"] = 1;
                    rowNum++;
                }

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }
        }

        /// <summary>
        /// GetNo
        /// </summary>
        /// <param name="bundleNo">bundleNo</param>
        /// <param name="dt">dt</param>
        /// <param name="poid">poid</param>
        /// <param name="fabricPanelCode">fabricPanelCode</param>
        /// <param name="article">article</param>
        /// <param name="size">size</param>
        /// <param name="proxyPMS">在別的專案引用時使用的db class</param>
        /// <returns>string</returns>
        public static string GetNo(string bundleNo, DataTable dt = null, string poid = null, string fabricPanelCode = null, string article = null, string size = null, AbstractDBProxyPMS proxyPMS = null)
        {
            if (dt == null)
            {
                dt = GetNoDatas(poid, fabricPanelCode, article, size, proxyPMS);
            }

            DataRow[] drs = dt.Select($"BundleNo = '{bundleNo}'");
            if (drs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return drs[0]["No"].ToString();
            }
        }

        /// <summary>
        /// GetNoDatas
        /// </summary>
        /// <param name="poid">poid</param>
        /// <param name="fabricPanelCode">fabricPanelCode</param>
        /// <param name="article">article</param>
        /// <param name="size">size</param>
        /// <param name="proxyPMS">在別的專案引用時使用的db class</param>
        /// <returns>DataTable</returns>
        public static DataTable GetNoDatas(string poid, string fabricPanelCode, string article, string size, AbstractDBProxyPMS proxyPMS = null)
        {
            string sqlcmd = $@"
SELECT 1
FROM BUNDLE_DETAIL bd with(nolock)
INNER JOIN BUNDLE B with(nolock) ON B.ID = bd.ID
WHERE  B.POID ='{poid}' And B.FabricPanelCode='{fabricPanelCode}' And B.Article = '{article}' AND bd.SizeCode='{size}'
and bd.PrintGroup is null";

            bool existsPrintGroup = proxyPMS == null ? MyUtility.Check.Seek(sqlcmd) : proxyPMS.Seek(sqlcmd, "Production");

            if (!existsPrintGroup)
            {
                sqlcmd = $@"
SELECT bd.id, bd.PrintGroup, DR = DENSE_RANK() over(order by  bd.id, bd.PrintGroup), bd.BundleNo, bd.Qty, bd.Patterncode
into #tmp
FROM BUNDLE_DETAIL bd with(nolock)
INNER JOIN BUNDLE B with(nolock) ON B.ID = bd.ID
WHERE  B.POID ='{poid}' And B.FabricPanelCode='{fabricPanelCode}' AND bd.SizeCode='{size}'
ORDER BY bd.id,bd.PrintGroup

select
	x.BundleNo,
	No = CONCAT(x.startno, '~',  x.startno + Qty - 1)
	,x.Id, x.DR, x.Patterncode	
from(
	select t.BundleNo, t.Qty,
		startno = 1+isnull((select SUM(qty) from(select qty = min(qty) from #tmp where DR < t.DR group by DR)x), 0)
		,t.Id,t.DR,t.Patterncode
	from #tmp t
)x
order by BundleNo

drop table #tmp
";
            }

            // 舊規(未有PrintGroup之前)
            else
            {
                sqlcmd = $@"
SELECT bd.id, bd.BundleGroup, bd.BundleNo,bd.Patterncode, bd.Qty, IsPair
into #beforetmp
FROM BUNDLE_DETAIL bd with(nolock)
INNER JOIN BUNDLE B with(nolock) ON B.ID = bd.ID
WHERE  B.POID ='{poid}' And B.FabricPanelCode='{fabricPanelCode}' And B.Article = '{article}' AND bd.SizeCode='{size}'
ORDER BY BundleGroup,bd.BundleNo

--maxQty 為每組綁包的總數,在相同 ID, BundleGroup 加總數
--分子 Bundle_Detail_qty 在P15寫入,每組綁包都會寫入一筆, 但是沒有直接關係分別是哪一組綁包的
--直接除有幾個 BundleGroup, 是因P15寫入規則, 每組綁包資訊必須一樣才會合併在同一張單
select *,
	maxQty=(Select sum(Qty) from Bundle_Detail_qty bdq WITH (NOLOCK) Where bdq.id = bt.id)/(select count(distinct BundleGroup) from #beforetmp where id = bt.id)
into #tmp
from #beforetmp bt

--同Patterncode下有數量不同
--IsPair 兩個為一組
select t.*,	
	IsPairRn = IIF(IsPair = 0, 0, row_number() over(partition by ID,BundleGroup,Patterncode Order by BundleNo) % 2 + 1)	
into #tmpx0
from #tmp t

select t.*,
	tmpLastNo = IIF(Qty < maxQty, sum(qty) over(partition by ID,BundleGroup,Patterncode,IsPairRn Order by BundleNo), Qty)
into #tmpx1
from #tmpx0 t
order by bundleno

select distinct Id,BundleGroup,maxQty into #tmp2 from #tmp
select *, lastNo = SUM(maxQty) over(Order by Id,BundleGroup) into #tmp3 from #tmp2
select *, before = LAG(lastNo,1,0) over(Order by Id,BundleGroup) into #tmp4 from #tmp3

select
	x1.*,
	minPatterncodeNo = min(tmpLastNo)  over(partition by x1.ID,x1.BundleGroup,x1.Patterncode,x1.IsPairRn Order by x1.BundleNo),
	tmpbefore = t4.before + 1,
	lastno = t4.before + x1.tmpLastNo
into #tmp5
from #tmp4 t4
inner join #tmpx1 x1 on x1.Id = t4.Id and x1.BundleGroup = t4.BundleGroup

select t5.*,
	startNo = case when Qty = maxQty or tmpLastNo = minPatterncodeNo then tmpbefore
					else LAG(lastNo,1,0) over(partition by ID,BundleGroup,Patterncode,IsPairRn Order by BundleNo) + 1
					end
into #tmp6
from #tmp5 t5

select BundleNo,No = CONCAT(startNo,'~',lastno)
from #tmp6

drop table #tmpx1,#tmp,#tmp2,#tmp3,#tmp4,#tmp5,#tmp6
";
            }

            DataTable dt;

            DualResult result = proxyPMS == null ? DBProxy.Current.Select("Production", sqlcmd, out dt) : proxyPMS.Select("Production", sqlcmd, out dt);
            if (!result)
            {
                throw result.GetException();
            }

            return dt;
        }

        /// <summary>
        /// MarkerLength 欄位格式化
        /// </summary>
        /// <inheritdoc/>
        public static string SetMarkerLengthMaskString(string eventString)
        {
            if (eventString == string.Empty || eventString == "Y  - / + \"" || (int.TryParse(eventString, out int result) && result == 0))
            {
                return string.Empty;
            }

            eventString = eventString.Replace(" ", "0");
            if (eventString.Contains("Y"))
            {
                string[] strings = eventString.Split('Y');
                string[] strings2 = strings[1].Split('-');
                string[] strings3 = strings2[1].Split('/');
                string[] strings4 = strings3[1].Split('+');
                string[] strings5 = strings4[1].Split('\"');
                eventString = $"{strings[0].PadLeft(2, '0')}Y{strings2[0].PadLeft(2, '0')}-{strings3[0].PadLeft(1, '0')}/{strings4[0].PadLeft(1, '0')}+{strings5[0].PadLeft(1, '0')}\"";
            }
            else
            {
                eventString = eventString.PadRight(8, '0');
                eventString = $"{eventString.Substring(0, 2)}Y{eventString.Substring(2, 2)}-{eventString.Substring(4, 1)}/{eventString.Substring(5, 1)}+{eventString.Substring(6, 1)}\"";
            }

            return eventString == "00Y00-0/0+0\"" ? string.Empty : eventString;
        }

        /// <summary>
        /// 取得 Order_Qty by cuttingID
        /// </summary>
        /// <param name="poID">poID</param>
        /// <param name="dt">DataTable</param>
        /// <returns>DualResult</returns>
        public static DualResult GetAllOrderID(string poID, out DataTable dt)
        {
            string sqlcmd = $@"SELECT ID FROM Orders WITH(NOLOCK) WHERE POID = '{poID}' AND Junk=0 ORDER BY ID";
            return DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
        }

        /// <summary>
        /// Get CutRef Value
        /// </summary>
        /// <param name="tableName">TableName</param>
        /// <param name="columnName">ColumnName</param>
        /// <returns>NextValue</returns>
        public static string GetColumnValueNo(string tableName, string columnName)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@TableName", tableName),
                new SqlParameter("@ColumnName", columnName),
            };

            string newValue = string.Empty;
            if (MyUtility.Check.Empty(tableName) || MyUtility.Check.Empty(columnName))
            {
                MyUtility.Msg.WarningBox("Error: TableName or ColumnName cannot be empty, Please contact TPE IT for processing.");
                return newValue;
            }

            // Table and Column檢核
            string chksql = @"
select 1 from ColumnValue with(nolock) where TableName = @TableName and [Column] = @ColumnName;
";
            if (!MyUtility.Check.Seek(chksql, sqlParameters))
            {
                MyUtility.Msg.WarningBox("Error: TableName or ColumnName not found, Please contact TPE IT for processing.");
                return newValue;
            }

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    string tabName = tableName;
                    string colName = columnName;

                    // TableName = WorkOrderForPlanning or WorkOrderForOutput and ColumnName=CutRef
                    if ((string.Compare(tabName, "WorkOrderForPlanning", ignoreCase: true) == 0 ||
                        string.Compare(tabName, "WorkOrderForOutput", ignoreCase: true) == 0) &&
                        string.Compare(colName, "CutRef", ignoreCase: true) == 0)
                    {
                        string sqlcmd = @"
select [Value] 
from ColumnValue where TableName = @TableName and [Column] = @ColumnName;
";
                        string currentValue = MyUtility.GetValue.Lookup(sqlcmd, sqlParameters);
                        if (MyUtility.Check.Empty(currentValue))
                        {
                            transaction.Dispose();
                            return newValue;
                        }

                        newValue = MyUtility.GetValue.GetNextValue(currentValue, 0);
                        sqlParameters.Add(new SqlParameter("@NewValue", newValue));
                        sqlcmd = $@"
Update ColumnValue
set [Value] = @NewValue
where TableName = @TableName and [Column] = @ColumnName
";
                        DualResult result = DBProxy.Current.Execute(string.Empty, sqlcmd, sqlParameters);
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox(result.ToString());
                            transaction.Dispose();
                        }
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    MyUtility.Msg.WarningBox(ex.ToString());
                }
            }

            return newValue;
        }
#pragma warning restore SA1611 // Element parameters should be documented
    }
}