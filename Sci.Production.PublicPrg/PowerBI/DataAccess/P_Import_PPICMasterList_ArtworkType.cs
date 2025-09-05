using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_PPICMasterList_ArtworkType
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_PPICMasterList_ArtworkType(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 2400,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                var currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
                item.SDate = currentYearStart.AddYears(-1).AddDays(7);
            }

            if (!item.EDate.HasValue)
            {
                var currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
                item.EDate = currentYearStart.AddYears(3).AddDays(6);
            }

            if (!item.SDate2.HasValue)
            {
                item.SDate2 = new DateTime(DateTime.Now.Year - 1, 1, 1);
            }

            if (!item.EDate2.HasValue)
            {
                item.EDate2 = new DateTime(DateTime.Now.Year + 3, 1, 1).AddDays(-1);
            }

            try
            {
                finalResult = this.GetP_PPICMasterList_ArtworkType_Data(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetP_PPICMasterList_ArtworkType_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@Date_S", item.SDate.Value.ToString("yyyy/MM/dd")),
                new SqlParameter("@Date_E", item.EDate.Value.ToString("yyyy/MM/dd")),
                new SqlParameter("@YearMonth_S", item.SDate2.Value.ToString("yyyy/MM/dd")),
                new SqlParameter("@YearMonth_E", item.EDate2.Value.ToString("yyyy/MM/dd")),
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            string sqlcmd = $@"
            -- 撈取ArtworkType相關資訊---
	        select *
	        into #UseArtworkType
	        from
	        (
		        select 
                ID
				, unit = nu.nUnit
				, SEQ
				, isOri = 1
		        FROM [MainServer].[Production].[dbo].ArtworkType with (nolock)
		        OUTER APPLY 
                (
			        SELECT [nUnit] = CASE WHEN ArtworkUnit = 'PPU' THEN 'PPU'
								        WHEN ProductionUnit = 'TMS' THEN 'TMS'
							        ELSE 'Price'
							        END 
		        ) nu
		        where Junk = 0
		        union
		        select 
                ID
				, ArtworkUnit
				, SEQ
				, isOri = 1
		        from [MainServer].[Production].[dbo].ArtworkType with (nolock)
		        where Junk = 0 
		        and ArtworkUnit NOT IN ('', 'PPU')
	        )a 

			--訂單串接ArtworkType資訊，訂單包含一般訂單與Local訂單，其中Local訂單有姊妹廠代工的狀況(SubconInType in '1','2')，若有此情況需產生兩筆同樣的資訊其中[Value],[TTL_Value],[SubconInType]一正一負進行紀錄

			-- Order_TmsCost
			Select *
			,[BIFactoryID] = @BIFactoryID
            ,[BIInsertDate] = GetDate()
			INTO #tmp
			FROM 
			(
				Select
				o.ID,
				o.FactoryID,
				ArtworkType = at.ID,
				at.SEQ,
				at.Unit,
				[Value] =  case at.Unit When 'TMS' Then t.TMS
							When 'STITCH' Then t.Qty
							When 'PCS' Then t.Qty
							Else t.Price
							End,
				TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
							When 'STITCH' Then o.Qty * t.Qty / 1000
							When 'PCS' Then o.Qty * t.Qty
							Else o.Qty * t.Price
							End,
				SubconInType = 0,
				[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
				[OrderDataKey] = CONCAT(o.ID, o.SubconInType)
				from [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
				inner join [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) on t.ID = o.ID
				inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
				where (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
				and o.Category in ('B','S')
				and o.LocalOrder = 0

				union all

				--Forecast Style_TmsCost
				Select 
				o.ID,
				o.FactoryID,
				ArtworkTypeID = at.ID,
				at.SEQ,
				at.Unit,
				Value = case at.Unit When 'TMS' Then t.TMS
						When 'STITCH' Then t.Qty * 1.0
						When 'PCS' Then t.Qty * 1.0
						Else t.Price
						End,
				TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
						When 'STITCH' Then o.Qty * t.Qty * 1.0 / 1000
						When 'PCS' Then o.Qty * t.Qty * 1.0
						Else o.Qty * t.Price
						End,
				SubconInType = 0,
				[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
				[OrderDataKey] = CONCAT(o.ID, o.SubconInType)
				From [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
				inner join [MainServer].[Production].[dbo].Style_TmsCost t WITH(NOLOCK) on t.StyleUkey = o.StyleUkey
				inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
				WHERE (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
				and o.IsForecast = 1

				union  all
			
				--loacl訂單 Order_TmsCost 轉入
				Select　
					o.ID,
					o.FactoryID,
					ArtworkType = at.ID,
					at.SEQ,
					at.Unit,
					[Value] =  case at.Unit When 'TMS' Then t.TMS
								When 'STITCH' Then t.Qty
								When 'PCS' Then t.Qty
								Else t.Price
								End,
					TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
								When 'STITCH' Then o.Qty * t.Qty / 1000
								When 'PCS' Then o.Qty * t.Qty
								Else o.Qty * t.Price
								End,
					SubconInType = o.SubconInType,
					[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
					[OrderDataKey] = CONCAT(o.ID, '-', o.SubconInType)
				from [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
				inner join [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) on t.ID = o.ID
				inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
				inner join [MainServer].[Production].[dbo].Factory f WITH(NOLOCK) on o.FactoryID = f.ID
				where (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
				and o.Junk = 0
				and o.LocalOrder = 1

				union  all

				-- Local訂單 Order_TmsCost 轉出
				Select
					o.ID,
					o.FactoryID,
					ArtworkType = at.ID,
					at.SEQ,
					at.Unit,
					[Value] =  case at.Unit When 'TMS' Then t.TMS
								When 'STITCH' Then t.Qty
								When 'PCS' Then t.Qty
								Else t.Price
								End * -1,
					TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
								When 'STITCH' Then o.Qty * t.Qty / 1000
								When 'PCS' Then o.Qty * t.Qty
								Else o.Qty * t.Price
								End * -1,
					SubconInType = '-' + o.SubconInType,
					[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
					[OrderDataKey] = CONCAT(o.ID, '--', o.SubconInType)
				from [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
				inner join [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) on t.ID = o.ID
				inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
				inner join [MainServer].[Production].[dbo].Factory f WITH(NOLOCK) on o.FactoryID = f.ID
				where (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
				and SubconInType in ('1','2')
				and o.Junk = 0
				and o.LocalOrder = 1
				and f.IsProduceFty = 0
			)A
			-- 檢視表
			Select * from #tmp

			-- 更新P_PPICMasterList_ArtworkType
			update p
			set 
			  p.[ArtworkTypeNo] = t.Seq
			, p.[ArtworkType] = t.ArtworkType
			, p.[Value] = ISNULL(t.[Value], 0)
			, p.[TotalValue] = ISNULL(t.TTL_Value, 0)
			, p.[ArtworkTypeUnit] = t.Unit
			, p.[OrderDataKey] = t.OrderDataKey
			, p.[FactoryID] = t.[FactoryID]
			, p.[BIFactoryID] = t.[BIFactoryID]
		    , p.[BIInsertDate] = t.[BIInsertDate]
			, p.[BIStatus] = 'New'
			from P_PPICMasterList_ArtworkType p
			inner join #tmp t on t.ID = p.[SP#] and t.SubconInType = p.[SubconInTypeID] and t.ArtworkTypeKey = p.[ArtworkTypeKey]

			-- 新增P_PPICMasterList_ArtworkType
			insert into [P_PPICMasterList_ArtworkType] ([SP#], [FactoryID], [ArtworkTypeNo], [ArtworkType], [Value], [TotalValue], [ArtworkTypeUnit], [SubconInTypeID], [ArtworkTypeKey], [OrderDataKey],[BIFactoryID], [BIInsertDate], [BIStatus])
			select t.ID, t.FactoryID, t.Seq, t.ArtworkType, ISNULL(t.[Value], 0), ISNULL(t.TTL_Value, 0), t.Unit, t.SubconInType, t.ArtworkTypeKey, t.OrderDataKey, BIFactoryID, BIInsertDate, 'New'
			from #tmp t
			where not exists (select 1 from P_PPICMasterList_ArtworkType p where t.ID = p.[SP#] and t.SubconInType = p.[SubconInTypeID] and t.ArtworkTypeKey = p.[ArtworkTypeKey])
			order by ID, ArtworkType, SEQ

			";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sqlcmd, sqlParameters, out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dt;
            return resultReport;
        }
    }
}
