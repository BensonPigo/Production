using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// Provides methods for importing and processing fabric physical inspection data.
    /// </summary>
    /// <remarks>This class is responsible for handling operations related to fabric physical inspection data,
    /// including updating and managing data in the context of Power BI integration and historical records.</remarks>
    public class P_Import_FabricPhysicalInspectionList
    {
        /// <summary>
        /// 根據指定的執行參數，擷取並處理布料物理檢驗資料。
        /// </summary>
        /// <remarks>
        /// 若 <paramref name="item"/> 未指定起始日期（<see cref="ExecutedList.SDate"/>）或結束日期（<see cref="ExecutedList.EDate"/>），則會指派預設值。
        /// 起始日期預設為當前時間前三分鐘，結束日期預設為當天日期。
        /// </remarks>
        /// <param name="item">執行參數，包含可選的起始與結束日期，用於篩選及處理資料。</param>
        /// <returns>
        /// 回傳一個 <see cref="Base_ViewModel"/>，內含處理後的布料物理檢驗資料。
        /// <see cref="Base_ViewModel.Result"/> 屬性表示操作是否成功。
        /// </returns>
        public Base_ViewModel P_FabricPhysicalInspectionList(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                QA_R12_ViewModel model = new QA_R12_ViewModel()
                {
                    Transaction = 1,
                    ArriveWHDate1 = item.SDate,
                    ArriveWHDate2 = item.EDate,
                    SP1 = string.Empty,
                    SP2 = string.Empty,
                    WK1 = string.Empty,
                    WK2 = string.Empty,
                    InspectionDate1 = null,
                    InspectionDate2 = null,
                    Brand = string.Empty,
                    Inspection = "Physical",
                    InspectionResult = "All",
                    ByWKSeq = false,
                    ByRollDyelot = true,
                    IsPowerBI = true,
                };

                finalResult = new QA_R12().GetQA_R12Data(model);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = this.UpdateBIData(finalResult.DtArr[0], item);
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = @"	
if @IsTrans = 1
begin
    INSERT INTO P_FabricPhysicalInspectionList_History([FactoryID], [SP], [SEQ], [ReceivingID], [Roll], [Dyelot], [BIFactoryID], [BIInsertDate])
    SELECT [FactoryID], [SP], [SEQ], [ReceivingID], [Roll], [Dyelot], [BIFactoryID], GETDATE()
	from P_FabricPhysicalInspectionList p
	where not exists (
		select 1
		from (
			select o.FactoryID
				, f.POID
				, [Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2)
				, f.ReceivingID
				, rd.Roll
				, rd.Dyelot
			from [MainServer].Production.dbo.Fir f with (nolock)
			inner join [MainServer].Production.dbo.Orders o with (nolock) on o.ID = f.POID
			inner join [MainServer].Production.dbo.Receiving r with (nolock) on r.Id = f.ReceivingID
			inner join [MainServer].Production.dbo.Receiving_Detail rd with (nolock) on r.id = rd.id and f.POID = rd.POID and f.SEQ1 = rd.SEQ1 and f.SEQ2 = rd.SEQ2
			where r.WhseArrival >= DATEADD(YEAR, DATEDIFF(YEAR, 0, @SDate) - 2, 0)
			union
			select o.FactoryID
				, f.POID
				, [Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2)
				, f.ReceivingID
				, rd.Roll
				, rd.Dyelot
			from [MainServer].Production.dbo.Fir f with (nolock)
			inner join [MainServer].Production.dbo.Orders o with (nolock) on o.ID = f.POID
			inner join [MainServer].Production.dbo.TransferIn r with (nolock) on r.Id = f.ReceivingID
			inner join [MainServer].Production.dbo.TransferIn_Detail rd with (nolock) on r.id = rd.id and f.POID = rd.POID and f.SEQ1 = rd.SEQ1 and f.SEQ2 = rd.SEQ2
			where r.IssueDate >= DATEADD(YEAR, DATEDIFF(YEAR, 0, @SDate) - 2, 0)
		) f
		where p.FactoryID = f.FactoryID
		and p.SP = f.POID
		and p.SEQ = f.Seq
		and p.ReceivingID = f.ReceivingID
		and p.Roll = f.Roll
		and p.Dyelot = f.Dyelot
	)
end

-- 刪除兩年前的歷史資料
DELETE p
from P_FabricPhysicalInspectionList p
where not exists (
	select 1
	from (
		select o.FactoryID
			, f.POID
			, [Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2)
			, f.ReceivingID
			, rd.Roll
			, rd.Dyelot
		from [MainServer].Production.dbo.Fir f with (nolock)
		inner join [MainServer].Production.dbo.Orders o with (nolock) on o.ID = f.POID
		inner join [MainServer].Production.dbo.Receiving r with (nolock) on r.Id = f.ReceivingID
		inner join [MainServer].Production.dbo.Receiving_Detail rd with (nolock) on r.id = rd.id and f.POID = rd.POID and f.SEQ1 = rd.SEQ1 and f.SEQ2 = rd.SEQ2
		where r.WhseArrival >= DATEADD(YEAR, DATEDIFF(YEAR, 0, @SDate) - 2, 0)
		union
		select o.FactoryID
			, f.POID
			, [Seq] = CONCAT(f.SEQ1, ' ', f.SEQ2)
			, f.ReceivingID
			, rd.Roll
			, rd.Dyelot
		from [MainServer].Production.dbo.Fir f with (nolock)
		inner join [MainServer].Production.dbo.Orders o with (nolock) on o.ID = f.POID
		inner join [MainServer].Production.dbo.TransferIn r with (nolock) on r.Id = f.ReceivingID
		inner join [MainServer].Production.dbo.TransferIn_Detail rd with (nolock) on r.id = rd.id and f.POID = rd.POID and f.SEQ1 = rd.SEQ1 and f.SEQ2 = rd.SEQ2
		where r.IssueDate >= DATEADD(YEAR, DATEDIFF(YEAR, 0, @SDate) - 2, 0)
	) f
	where p.FactoryID = f.FactoryID
	and p.SP = f.POID
	and p.SEQ = f.Seq
	and p.ReceivingID = f.ReceivingID
	and p.Roll = f.Roll
	and p.Dyelot = f.Dyelot
)

UPDATE P
	SET p.[Category]				= ISNULL(t.[Category], '')
	, p.[Season]					= ISNULL(t.[SeasonID], '')
	, p.[WKNo]						= ISNULL(t.[ExportId], '')
	, p.[Invoice]					= ISNULL(t.[Invno], '')
	, p.[Style]						= ISNULL(t.[StyleID], '')
	, p.[Brand]						= ISNULL(t.[BrandID], '')
	, p.[SupplierName]				= ISNULL(t.[Suppid], '')
	, p.[Refno]						= ISNULL(t.[Refno], '')
	, p.[Color]						= ISNULL(t.[ColorID], '')
	, p.[CuttingDate]				= t.[Cutting Date]
	, p.[ArriveWHDate]				= t.[ArriveWH_Date]
	, p.[ArriveQty]					= ISNULL(t.[ArriveQty], 0)
	, p.[WeaveType]					= ISNULL(t.[WeaveTypeID], '')
	, p.[TotalRoll]					= ISNULL(t.[TotalRoll], 0)
	, p.[TotalDyeLot]				= ISNULL(t.[TotalLot], 0)
	, p.[AlreadyInspectedDyelot]	= ISNULL(t.[InspectedTotalLot], 0)
	, p.[NotInspectedDyelot]		= ISNULL(t.[NotInspectedDyelot], 0)
	, p.[NonInspection]				= ISNULL(t.[NonPhysical], '')
	, p.[PhysicalInspection]		= ISNULL(t.[Physical], '')
	, p.[PhysicalInspector]			= ISNULL(t.[PhysicalInspector], '')
	, p.[Approver]					= ISNULL(t.[Approver], '')
	, p.[ApproveDate]				= t.[ApproveDate]
	, p.[TicketYds]					= ISNULL(t.[TicketYds], 0)
	, p.[ActYdsInsdpected]			= ISNULL(t.[ActualYds], 0)
	, p.[LthOfDiff]					= ISNULL(t.[DiffLth], 0)
	, p.[TransactionID]				= ISNULL(t.[TransactionID], '')
	, p.[CutWidth]					= ISNULL(t.[Width], 0)
	, p.[FullWidth]					= ISNULL(t.[FullWidth], 0)
	, p.[ActualWidth]				= ISNULL(t.[ActualWidth], 0)
	, p.[TotalPoints]				= ISNULL(t.[TotalPoint], 0)
	, p.[PointRate]					= ISNULL(t.[PointRate], 0)
	, p.[Result]					= ISNULL(t.[Result], '')
	, p.[Grade]						= ISNULL(t.[Grade], '')
	, p.[Moisture]					= ISNULL(t.[Moisture], '')
	, p.[Remark]					= ISNULL(t.[Remark], '')
	, p.[InspDate]					= t.[InspDate]
	, p.[Inspector]					= ISNULL(t.[Inspector], '')
	, p.[OrderType]					= ISNULL(t.[OrderTypeID], '')
	, p.[BIFactoryID]				= @BIFactoryID
	, p.[BIInsertDate]				= GETDATE()
FROM P_FabricPhysicalInspectionList p
INNER JOIN #tmp t ON p.FactoryID = t.FactoryID
				and p.SP = t.POID
				and p.SEQ = t.SEQ
				and p.ReceivingID = t.ReceivingID
				and p.Roll = t.Roll
				and p.Dyelot = t.Dyelot


INSERT INTO P_FabricPhysicalInspectionList([FactoryID], [Category], [Season], [SP], [SEQ], [WKNo], [Invoice], [ReceivingID], [Style], [Brand], [SupplierName], [Refno], [Color], [CuttingDate], [ArriveWHDate], [ArriveQty], [WeaveType], [TotalRoll], [TotalDyeLot], [AlreadyInspectedDyelot], [NotInspectedDyelot], [NonInspection], [PhysicalInspection], [PhysicalInspector], [Approver], [ApproveDate], [Roll], [Dyelot], [TicketYds], [ActYdsInsdpected], [LthOfDiff], [TransactionID], [CutWidth], [FullWidth], [ActualWidth], [TotalPoints], [PointRate], [Result], [Grade], [Moisture], [Remark], [InspDate], [Inspector], [OrderType], [BIFactoryID], [BIInsertDate])
SELECT [FactoryID], ISNULL([Category], ''), ISNULL([SeasonID], ''), [POID], [SEQ], ISNULL([ExportId], ''), ISNULL([Invno], ''), [ReceivingID], ISNULL([StyleID], ''), ISNULL([BrandID], ''), ISNULL([Suppid], ''), ISNULL([Refno], ''), ISNULL([ColorID], ''), [Cutting Date], [ArriveWH_Date], ISNULL([ArriveQty], 0), ISNULL([WeaveTypeID], ''), ISNULL([TotalRoll], 0), ISNULL([TotalLot], 0), ISNULL([InspectedTotalLot], 0), ISNULL([NotInspectedDyelot], 0), ISNULL([NonPhysical], ''), ISNULL([Physical], ''), ISNULL([PhysicalInspector], ''), ISNULL([Approver], ''), [ApproveDate], [Roll], [Dyelot], ISNULL([TicketYds], 0), ISNULL([ActualYds], 0), ISNULL([DiffLth], 0), ISNULL([TransactionID], ''), ISNULL([Width], 0), ISNULL([FullWidth], 0), ISNULL([ActualWidth], 0), ISNULL([TotalPoint], 0), ISNULL([PointRate], 0), ISNULL([Result], ''), ISNULL([Grade], ''), ISNULL([Moisture], ''), ISNULL([Remark], ''), [InspDate], ISNULL([Inspector], ''), ISNULL([OrderTypeID], ''), @BIFactoryID, GETDATE()
FROM #tmp t
WHERE NOT EXISTS (SELECT 1 FROM P_FabricPhysicalInspectionList p WHERE p.FactoryID = t.FactoryID
													and p.SP = t.POID
													and p.SEQ = t.SEQ
													and p.ReceivingID = t.ReceivingID
													and p.Roll = t.Roll
													and p.Dyelot = t.Dyelot)

";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
