using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class QA_R08
    {
        /// <inheritdoc/>
        public QA_R08()
        {
            DBProxy.Current.DefaultTimeout = 3600;
        }

        /// <inheritdoc/>
        public Base_ViewModel Get_QA_R08(QA_R08_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@InspectionDateFrom", SqlDbType.DateTime) { Value = (object)model.InspectionDateFrom ?? DBNull.Value },
                new SqlParameter("@InspectionDateTo", SqlDbType.DateTime) { Value = (object)model.InspectionDateTo ?? DBNull.Value },
                new SqlParameter("@EditDateFrom", SqlDbType.DateTime) { Value = (object)model.EditDateFrom ?? DBNull.Value },
                new SqlParameter("@EditDateTo", SqlDbType.DateTime) { Value = (object)model.EditDateTo ?? DBNull.Value },
                new SqlParameter("@Inspectors", SqlDbType.VarChar) { Value = (object)model.Inspectors ?? DBNull.Value },
                new SqlParameter("@POIDFrom", SqlDbType.VarChar) { Value = (object)model.POIDFrom ?? DBNull.Value },
                new SqlParameter("@POIDTo", SqlDbType.VarChar) { Value = (object)model.POIDTo ?? DBNull.Value },
                new SqlParameter("@RefNoFrom", SqlDbType.VarChar) { Value = (object)model.RefNoFrom ?? DBNull.Value },
                new SqlParameter("@RefNoTo", SqlDbType.VarChar) { Value = (object)model.RefNoTo ?? DBNull.Value },
                new SqlParameter("@BrandID", SqlDbType.VarChar) { Value = (object)model.BrandID ?? DBNull.Value },
            };

            string sqlcmd = @"
Declare @InspMachine_FabPrepareTime_Woven int
Declare @InspMachine_FabPrepareTime_Other int
Declare @InspMachine_DefectPointTime int

select  @InspMachine_FabPrepareTime_Woven = InspMachine_FabPrepareTime_Woven,
        @InspMachine_FabPrepareTime_Other = InspMachine_FabPrepareTime_Other,
        @InspMachine_DefectPointTime = InspMachine_DefectPointTime
from    [ExtendServer].ManufacturingExecution.dbo.system

SELECT  InspectionStatus
        ,InspDate
        ,Inspector
        ,InspectorName
        ,BrandID
        ,Factory
        ,StyleID
        ,POID
        ,SEQ
        ,StockType
        ,Wkno
        ,SuppID
        ,SuppName
        ,ATA
        ,Roll
        ,Dyelot
        ,RefNo
        ,Color
        ,ArrivedYDS
        ,ActualYDS
        ,LthOfDiff
        ,TransactionID
        ,QCIssueQty
        ,QCIssueTransactionID
        ,CutWidth
        ,ActualWidth
        ,Speed
        ,TotalDefectPoints
		,PointRatePerRoll
        ,Grade
        ,InspectionStartTime
        ,InspectionFinishTime 
        ,MachineDownTime
        ,MachineRunTime
        ,Remark
        ,MCHandle
        ,WeaveType
	    ,ReceivingID 
        ,MachineIoTUkey
        ,InspSeq
        ,AddDate
into #tmp
FROM dbo.GetQA_R08_Detail(@InspectionDateFrom, @InspectionDateTo, @Inspectors, @POIDFrom, @POIDTo, @RefNoFrom, @RefNoTo, @BrandID, @EditDateFrom, @EditDateTo, @InspMachine_FabPrepareTime_Woven, @InspMachine_FabPrepareTime_Other, @InspMachine_DefectPointTime)
";

            if (model.IsSummary)
            {
                sqlcmd += @"
select * into #tmpGroupActualYDS from (
	select inspector
	,[QCName] = InspectorName
	,[inspected date] = InspDate
	,[Roll] = count([Roll])
	,[Actual YDS] = ROUND(sum(ActualYDS), 1)
	from #tmp
	where InspectorName is not null
	group by inspector, InspectorName, InspDate

	union all

	select inspector
	,[QCName] = (select Name from [ExtendServer].ManufacturingExecution.dbo.Pass1 where id = Inspector)
	,[inspected date] = InspDate
	,[Roll] = count([Roll])
	,[Actual YDS] = ROUND(sum(ActualYDS), 1)
	from #tmp
	where InspectorName is null
	group by inspector, InspDate
) a

select * from #tmpGroupActualYDS
order by inspector,[inspected date]

-- 取得所有QC人員
select  distinct Inspector, QCName 
from #tmpGroupActualYDS
order by Inspector

-- 依日期 加總Roll and Yard
select [inspected date] = InspDate, [Roll] = count(Roll), [Actual YDS] = sum(ActualYDS) 
from #tmp
group by InspDate
order by InspDate

-- 取得Woven Yard
select [inspected date] = InspDate, [Actual YDS] = sum(ActualYDS) , WeaveTypeID = 'Woven'
from #tmp
where WeaveType = 'Woven'
group by InspDate
order by InspDate

-- 取得Knit Yard
select [inspected date] = InspDate, [Actual YDS] = sum(ActualYDS) , WeaveTypeID = 'Knit'
from #tmp
where WeaveType = 'knit'
group by InspDate
order by InspDate
 
";
            }
            else
            {
                sqlcmd += $@"

DECLARE @QASortOutStandard decimal(5,2) = (SELECT QASortOutStandard FROM [SYSTEM])

select  t.InspectionStatus
        ,t.InspDate
        ,t.Inspector
        ,t.InspectorName
        ,t.BrandID
        ,t.Factory
        ,t.StyleID
        ,t.POID
        ,t.SEQ
        ,t.StockType
        ,t.Wkno
        ,t.SuppID
        ,t.SuppName
        ,t.ATA
        ,t.Roll
        ,t.Dyelot
        ,t.RefNo
        ,t.Color
        ,t.ArrivedYDS
        ,t.ActualYDS
        ,t.LthOfDiff
        ,t.TransactionID
        ,t.QCIssueQty
        ,t.QCIssueTransactionID
        ,t.CutWidth
        ,t.ActualWidth
        ,t.Speed
        ,t.TotalDefectPoints
		,t.PointRatePerRoll
        ,t.Grade
        ,[SortOut] = IIf (t.PointRatePerRoll >= @QASortOutStandard , 'Y', 'N')
        ,t.InspectionStartTime
        ,t.InspectionFinishTime 
        ,t.MachineDownTime
        ,t.MachineRunTime
        ,t.Remark
        ,t.MCHandle
        ,t.WeaveType
        ,m.MachineID
        ,t.ReceivingID  
        ,t.InspSeq
        ,t.AddDate
from #tmp t
left join [ExtendServer].ManufacturingExecution.dbo.MachineIoT m on t.MachineIoTUkey = m.Ukey
ORDER BY t.InspDate, t.Inspector, t.POID, t.SEQ, t.Roll, t.Dyelot";
            }

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmd, listPar, out DataTable[] dtArr),
            };

            resultReport.DtArr = dtArr;
            return resultReport;
        }
    }
}
