using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_ActualCutOutputReport
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_ActualCutOutputReport(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Now.AddDays(-7);
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                Cutting_R08_ViewModel model = new Cutting_R08_ViewModel()
                {
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    EstCutDate1 = item.SDate,
                    EstCutDate2 = item.EDate,
                    ActCutDate1 = item.SDate,
                    ActCutDate2 = item.EDate,
                    CuttingSP = string.Empty,
                    IsPowerBI = true,
                };

                Cutting_R08 biModel = new Cutting_R08();
                finalResult = biModel.GetActualCutOutput(model);
                if (!finalResult.Result)
                {
                    return finalResult;
                }

                // insert into PowerBI
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
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = $@" 

select *
into #tmp_Final
from (
	SELECT *,
		   ROW_NUMBER() OVER (
			   PARTITION BY FactoryID, EstCutDate, ActCutDate, CutCellid, ID, CutplanID,
							CutRef, SubSP, StyleID, Size, PerimeterYd, SizeCode, Cons, NoofWindow
			   ORDER BY WorkOrderUkey
		   ) AS rn
	FROM #tmp
) t 
where t.rn = 1

if @IsTrans = 1
begin
	insert into P_ActualCutOutputReport_History(WorkOrderUkey, BIFactoryID, BIInsertDate)
	SELECT p.WorkOrderUkey, p.BIFactoryID, GETDATE()
	FROM P_ActualCutOutputReport p
	WHERE NOT EXISTS (
		SELECT 1 
		FROM #tmp_Final t 
		WHERE p.WorkOrderUkey = t.WorkOrderUkey
	)
	AND p.EstCutDate >= @SDate
	AND p.EstCutDate <= @EDate;
end

DELETE p
FROM P_ActualCutOutputReport p
WHERE NOT EXISTS (
	SELECT 1 
	FROM #tmp_Final t 
	WHERE p.WorkOrderUkey = t.WorkOrderUkey
)
AND p.EstCutDate >= @SDate
AND p.EstCutDate <= @EDate;

-- 針對 CutRef 空白變有值得刪除。
if @IsTrans = 1
begin
	insert into P_ActualCutOutputReport_History(WorkOrderUkey, BIFactoryID, BIInsertDate)
	SELECT p.WorkOrderUkey, p.BIFactoryID, GETDATE()
	FROM P_ActualCutOutputReport p
	WHERE p.CutRef = ''
	AND EXISTS (
		SELECT 1 
		FROM [MainServer].Production.dbo.WorkOrderForOutput w 
		WHERE p.WorkOrderUkey = w.Ukey
		AND w.CutRef <> ''
	)
end

DELETE p 
FROM P_ActualCutOutputReport p
WHERE p.CutRef = ''
AND EXISTS (
	SELECT 1 
	FROM [MainServer].Production.dbo.WorkOrderForOutput w 
	WHERE p.WorkOrderUkey = w.Ukey
	AND w.CutRef <> ''
)

UPDATE p
	SET 
		FactoryID              = ISNULL(t.[FactoryID], '')
		,EstCutDate            = t.EstCutDate
		,ActCutDate            = t.ActCutDate
		,CutCellid             = ISNULL(t.[CutCellid], '')
		,SpreadingNoID         = ISNULL(t.[SpreadingNoID], '')
		,CutplanID             = ISNULL(t.[CutplanID], '')
		,CutRef                = ISNULL(t.[CutRef], '')
		,SP                    = ISNULL(t.[ID], '')
		,SubSP                 = ISNULL(t.[SubSP], '')
		,StyleID               = ISNULL(t.[StyleID], '')
		,Size                  = ISNULL(t.[Size], '')
		,noEXCESSqty           = ISNULL(t.[noEXCESSqty], 0)
		,Description           = ISNULL(t.[Description], '')
		,WeaveTypeID           = ISNULL(t.[WeaveTypeID], '')
		,FabricCombo           = ISNULL(t.[FabricCombo], '')
		,MarkerLength          = ISNULL(t.[MarkerLength], 0)
		,PerimeterYd           = ISNULL(t.[PerimeterYd], '')
		,Layer                 = ISNULL(t.[Layer], 0)
		,SizeCode              = ISNULL(t.[SizeCode], '')
		,Cons                  = ISNULL(t.[Cons], 0)
		,EXCESSqty             = ISNULL(t.[EXCESSqty], 0)
		,NoofRoll              = ISNULL(t.[NoofRoll], 0)
		,DyeLot                = ISNULL(t.[DyeLot], 0)
		,NoofWindow            = ISNULL(t.[NoofWindow], 0)
		,CuttingSpeed          = ISNULL(t.[ActualSpeed], 0)
		,PreparationTime       = ISNULL(t.[PreparationTime_min], 0)
		,ChangeoverTime        = ISNULL(t.[ChangeoverTime_min], 0)
		,SpreadingSetupTime    = ISNULL(t.[SpreadingSetupTime_min], 0)
		,MachSpreadingTime     = ISNULL(t.[MachineSpreadingTime_min], 0)
		,SeparatorTime         = ISNULL(t.[Separatortime_min], 0)
		,ForwardTime           = ISNULL(t.[ForwardTime_min], 0)
		,CuttingSetupTime      = ISNULL(t.[CuttingSetupTime_min], 0)
		,MachCuttingTime       = ISNULL(t.[MachCuttingTime_min], 0)
		,WindowTime            = ISNULL(t.[WindowTime_min], 0)
		,TotalSpreadingTime    = ISNULL(t.[TotalSpreadingTime_min], 0)
		,TotalCuttingTime      = ISNULL(t.[TotalCuttingTime_min], 0)
		,BIFactoryID           = @BIFactoryID
		,BIInsertDate          = GETDATE()
		,BIStatus			   = 'NEW'
	FROM P_ActualCutOutputReport p
	INNER JOIN #tmp_Final t ON p.WorkOrderUkey = t.WorkOrderUkey

INSERT INTO [dbo].[P_ActualCutOutputReport]
(
	 [FactoryID]
	,[EstCutDate]
	,[ActCutDate]
	,[CutCellid]
	,[SpreadingNoID]
	,[CutplanID]
	,[CutRef]
	,[SP]
	,[SubSP]
	,[StyleID]
	,[Size]
	,[noEXCESSqty]
	,[Description]
	,[WeaveTypeID]
	,[FabricCombo]
	,[MarkerLength]
	,[PerimeterYd]
	,[Layer]
	,[SizeCode]
	,[Cons]
	,[EXCESSqty]
	,[NoofRoll]
	,[DyeLot]
	,[NoofWindow]
	,[CuttingSpeed]
	,[PreparationTime]
	,[ChangeoverTime]
	,[SpreadingSetupTime]
	,[MachSpreadingTime]
	,[SeparatorTime]
	,[ForwardTime]
	,[CuttingSetupTime]
	,[MachCuttingTime]
	,[WindowTime]
	,[TotalSpreadingTime]
	,[TotalCuttingTime]
	,[BIFactoryID]	
	,[BIInsertDate]
	,[BIStatus]
	,[WorkOrderUkey]
)
select 
	ISNULL([FactoryID], '')
	,[EstCutDate]
	,[ActCutDate]
	,ISNULL([CutCellid], '')
	,ISNULL([SpreadingNoID], '')
	,ISNULL([CutplanID], '')
	,ISNULL([CutRef], '')
	,ISNULL(ID, '')
	,ISNULL([SubSP], '')
	,ISNULL([StyleID], '')
	,ISNULL([Size], '')
	,ISNULL([noEXCESSqty], 0)
	,ISNULL([Description], '')
	,ISNULL([WeaveTypeID], '')
	,ISNULL([FabricCombo], '')
	,ISNULL([MarkerLength], 0)
	,ISNULL([PerimeterYd], '')
	,ISNULL([Layer], 0)
	,ISNULL([SizeCode], '')
	,ISNULL([Cons], 0)
	,ISNULL([EXCESSqty], 0)
	,ISNULL([NoofRoll], 0)
	,ISNULL([DyeLot], 0)
	,ISNULL([NoofWindow], 0)
	,ISNULL(ActualSpeed, 0)
	,ISNULL([PreparationTime_min], 0)
	,ISNULL([ChangeoverTime_min], 0)
	,ISNULL([SpreadingSetupTime_min], 0)
	,ISNULL([MachineSpreadingTime_min], 0)
	,ISNULL([Separatortime_min], 0)
	,ISNULL([ForwardTime_min], 0)
	,ISNULL([CuttingSetupTime_min], 0)
	,ISNULL([MachCuttingTime_min], 0)
	,ISNULL([WindowTime_min], 0)
	,ISNULL([TotalSpreadingTime_min], 0)
	,ISNULL([TotalCuttingTime_min], 0)
	,@BIFactoryID
	,GETDATE()
	,'NEW'
	,d.WorkOrderUkey
from #tmp_Final d
where not exists(select 1 from P_ActualCutOutputReport p where p.WorkOrderUkey = d.WorkOrderUkey)
";
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#tmp", paramters: sqlParameters);
            }

            return finalResult;
        }
    }
}
