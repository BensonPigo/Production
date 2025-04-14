using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg
{
    /// <summary>
    /// SewingPrg
    /// </summary>
    public static class SewingPrg
    {
        /// <summary>
        /// 回傳產線連續身產style結果，對應Production.dbo.SewingReason type = 'IC'
        /// </summary>
        /// <param name="drMain">drMain</param>
        /// <param name="dtDetail">dtDetail</param>
        /// <returns>KeyValuePair</returns>
        public static KeyValuePair<string, DualResult> GetInlineCategory(DataRow drMain, DataTable dtDetail)
        {
            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                return new KeyValuePair<string, DualResult>(string.Empty, new DualResult(false, "Get InlineCategory no Detail data"));
            }

            foreach (DataRow drDetail in dtDetail.Rows)
            {
                if (!MyUtility.Check.Empty(drDetail["StyleUkey"]))
                {
                    continue;
                }

                DataRow drOrder;
                bool isExists = MyUtility.Check.Seek($"select StyleUkey, [OrderCategory] = Category from Orders where ID = '{drDetail["OrderID"]}'", out drOrder);

                if (isExists)
                {
                    drDetail["StyleUkey"] = drOrder["StyleUkey"];
                    drDetail["OrderCategory"] = drOrder["OrderCategory"];
                }
            }

            // 優先度1 sample單
            bool hasSample = dtDetail.AsEnumerable().Any(s => s.RowState != DataRowState.Deleted && s["OrderCategory"].ToString() == "S");
            if (hasSample)
            {
                return new KeyValuePair<string, DualResult>("00005", new DualResult(true));
            }

            int maxContinusDays = 0;
            foreach (long styleUkey in dtDetail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Select(s => MyUtility.Convert.GetLong(s["StyleUkey"])))
            {
                KeyValuePair<int, DualResult> resultContinusDays = CheckContinusProduceDays(
                drMain["SewingLineID"].ToString(),
                drMain["Team"].ToString(),
                drMain["FactoryID"].ToString(),
                MyUtility.Convert.GetDate(drMain["OutputDate"]),
                styleUkey);

                if (!resultContinusDays.Value)
                {
                    return new KeyValuePair<string, DualResult>(string.Empty, resultContinusDays.Value);
                }

                maxContinusDays = resultContinusDays.Key > maxContinusDays ? resultContinusDays.Key : maxContinusDays;
            }

            if (maxContinusDays > 29)
            {
                return new KeyValuePair<string, DualResult>("00004", new DualResult(true));
            }
            else if (maxContinusDays > 14)
            {
                return new KeyValuePair<string, DualResult>("00003", new DualResult(true));
            }
            else if (maxContinusDays > 3)
            {
                return new KeyValuePair<string, DualResult>("00002", new DualResult(true));
            }
            else
            {
                return new KeyValuePair<string, DualResult>("00001", new DualResult(true));
            }
        }

        /// <summary>
        /// 檢查傳入style連續生產天數，包含similar style
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="team">team</param>
        /// <param name="factoryID">factoryID</param>
        /// <param name="sewingDate">sewingDate</param>
        /// <param name="styleUkey">styleUkey</param>
        /// <returns>KeyValuePair</returns>
        public static KeyValuePair<int, DualResult> CheckContinusProduceDays(string line, string team, string factoryID, DateTime? sewingDate, long styleUkey)
        {
            if (MyUtility.Check.Empty(styleUkey))
            {
                return new KeyValuePair<int, DualResult>(0, new DualResult(false, "No pass in Style"));
            }

            string sqlcmd = $@"select [ContinusDays] = dbo.GetCheckContinusProduceDays({styleUkey},'{line}','{factoryID}','{team}','{Convert.ToDateTime(sewingDate).ToString("yyyy/MM/dd")}') ";

            DataTable dtResult;

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dtResult);

            if (!result)
            {
                return new KeyValuePair<int, DualResult>(0, result);
            }

            return new KeyValuePair<int, DualResult>(MyUtility.Convert.GetInt(dtResult.Rows[0]["ContinusDays"]), new DualResult(true));
        }

        /// <summary>
        /// 重跑傳入參數對應InlineCategory
        /// </summary>
        /// <param name="sewingLineID">sewingLineID</param>
        /// <param name="team">team</param>
        /// <param name="factoryID">factoryID</param>
        /// <param name="outputDate">outputDate</param>
        /// <returns>DualResult</returns>
        public static DualResult ReCheckInlineCategory(string sewingLineID, string team, string factoryID, DateTime? outputDate, SqlConnection sqlConn = null)
        {
            string sqlGetReRunData = $@"
--會影響的只有30天內的資料
select  top 30  so.ID, so.SewingLineID, so.Team, so.FactoryID, so.OutputDate, so.SewingReasonIDForTypeIC
into    #tmpMain
from    SewingOutput so with (nolock)
where   so.SewingLineID = '{sewingLineID}' and
        so.Team = '{team}' and
        so.FactoryID = '{factoryID}' and
        so.OutputDate > @OutputDate
order by so.OutputDate

select  *
from    #tmpMain
order by OutputDate

select  distinct tm.ID, o.StyleUkey, [OrderCategory] = o.Category
from    #tmpMain tm
inner join SewingOutput_Detail sod with (nolock) on sod.ID = tm.ID
inner join Orders o with (nolock) on o.ID = sod.OrderID

drop table #tmpMain

";
            DataTable[] dtReRuns;
            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@OutputDate", outputDate) };
            DualResult result;

            if (sqlConn != null)
            {
                result = DBProxy.Current.SelectByConn(sqlConn, sqlGetReRunData, listPar, out dtReRuns);
            }
            else
            {
                result = DBProxy.Current.Select(null, sqlGetReRunData, listPar, out dtReRuns);
            }

            if (!result)
            {
                return result;
            }

            DataTable dtMain = dtReRuns[0];
            int countChangeOver = 0;
            foreach (DataRow drMain in dtMain.Rows)
            {
                DataTable dtDetail = dtReRuns[1].AsEnumerable().Where(s => s["ID"].ToString() == drMain["ID"].ToString()).TryCopyToDataTable(dtReRuns[1]);
                if (dtDetail.Rows.Count == 0)
                {
                    continue;
                }

                KeyValuePair<string, DualResult> inlineCategoryResult = SewingPrg.GetInlineCategory(drMain, dtDetail);

                if (!inlineCategoryResult.Value)
                {
                    return inlineCategoryResult.Value;
                }

                if (inlineCategoryResult.Key == "00001")
                {
                    countChangeOver++;
                }
                else
                {
                    countChangeOver = 0;
                }

                if (drMain["SewingReasonIDForTypeIC"].ToString() == inlineCategoryResult.Key)
                {
                    continue;
                }

                string updateSewingOutput = $@"
update  SewingOutput set SewingReasonIDForTypeIC = '{inlineCategoryResult.Key}' where ID = '{drMain["ID"]}'
";

                if (sqlConn != null)
                {
                    result = DBProxy.Current.ExecuteByConn(sqlConn, updateSewingOutput);
                }
                else
                {
                    result = DBProxy.Current.Execute(null, updateSewingOutput);
                }

                if (!result)
                {
                    return result;
                }

                // 如果連續4天都是ChangeOver，表示後面的天數沒有被這次調整影響，所以就不用重算
                if (countChangeOver == 4)
                {
                    break;
                }
            }

            return new DualResult(true);
        }
    }
}
