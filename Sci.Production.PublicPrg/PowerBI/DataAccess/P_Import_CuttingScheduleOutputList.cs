﻿using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CuttingScheduleOutputList
    {
        /// <inheritdoc/>
        public Base_ViewModel P_CuttingScheduleOutputList(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Cutting_R13 biModel = new Cutting_R13();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.AddDays(7).ToString("yyyy/MM/dd"));
            }

            try
            {
                Cutting_R13_ViewModel model = new Cutting_R13_ViewModel()
                {
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    StyleID = string.Empty,
                    CuttingSP1 = string.Empty,
                    CuttingSP2 = string.Empty,
                    Est_CutDate1 = sDate,
                    Est_CutDate2 = eDate,
                    ActCuttingDate1 = null,
                    ActCuttingDate2 = null,
                };

                Base_ViewModel resultReport = biModel.GetCuttingScheduleOutputData(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                DualResult result;
                DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                using (sqlConn)
                {
                    string sDateS = sDate.ToString("yyyy/MM/dd");
                    string eDateS = sDate.ToString("yyyy/MM/dd");
                    string sql = $@"	
/************* 刪除P_CuttingScheduleOutputList的資料，規則刪除相同的WorkOrder.ID*************/
Delete P_CuttingScheduleOutputList
from P_CuttingScheduleOutputList as a 
inner join #tmp as b on a.FactoryID = b.Factory and a.POID = b.[Master SP#] and a.EstCuttingDate = b.[Est.Cutting Date]

/************* 新增P_CuttingScheduleOutputList的資料(ActCuttingDate,LackingLayers新增時欄位都要為空)*************/
insert into P_CuttingScheduleOutputList
(
	[MDivisionID]				
	,[FactoryID]			
	,[Fabrication]			
	,[EstCuttingDate]		
	,[ActCuttingDate]		
	,[EarliestSewingInline]	
	,[POID]					
	,[BrandID]				
	,[StyleID]				
	,[FabRef]				
	,[SwitchToWorkorderType]
	,[CutRef]				
	,[CutNo]				
	,[SpreadingNoID]		
	,[CutCell]				
	,[Combination]			
	,[Layers]
	,[LayersLevel]
	,[LackingLayers]		
	,[Ratio]				
	,[Consumption]
	,[ActConsOutput]
	,[BalanceCons]
	,[MarkerName]			
	,[MarkerNo]
	,[MarkerLength]
	,[CuttingPerimeter]
	,[StraightLength]
	,[CurvedLength]
	,[DelayReason]
	,[Remark]
)
select 
	  [MDivisionID] = isnull([M],'')				
	, [FactoryID] = isnull([Factory],'')
	, [Fabrication] = isnull([Fabrication],'')
	, [Est.Cutting Date]
	, [ActCuttingDate] = null
	, [Earliest Sewing Inline]
	, [POID]=isnull([Master SP#],'')
	, [BrandID] = isnull([Brand],'')
	, [StyleID] = isnull([Style#],'')
	, [FabRef] = isnull([FabRef#],'')
	, [SwitchToWorkorderType] = isnull([Switch to Workorder],'')
	, [CutRef] = isnull([Ref#],'')
	, [CutNo] = isnull([Cut#],0)
	, [SpreadingNoID] = isnull([SpreadingNoID],'')
	, [CutCell] = isnull([Cut Cell],'')
	, [Combination] = isnull([Combination],'')
	, [Layers] = isnull([Layer],0)
	, [LayersLevel] = isnull([Layers Level],'')
	, [LackingLayers] = 0
	, [Ratio] = isnull([Ratio],'')
	, [Consumption] = isnull([Consumption],0)
	, [ActConsOutput] = isnull([Act. Cons. Output],'')
	, [BalanceCons] = isnull([Balance Cons.],'')
	, [MarkerName] = isnull([Marker Name],'')
	, [MarkerNo] = isnull([Marker No.],'')
	, [MarkerLength] = isnull([Marker Length],'')
	, [CuttingPerimeter] = isnull([Cutting Perimeter],'')
	, [StraightLength] = isnull([Straight Length],'')
	, [CurvedLength] = isnull([Curved Length],'')
	, [DelayReason] = isnull([Delay Reason],'')
	, [Remark] = isnull([Remark],'')
from #tmp

/************* 更新ActCuttingDate、LackingLayers欄位前的整合資料*************/
/*************找出CuttingOutput，有被新增及修改的資料*************/
SELECT [UpperSum] =  SUM(Layer)
	,[CuttingID] = CuttingID
	,[CutRef] = CutRef
	,[cDate]
	,[FactoryID] = co.FactoryID
into #cuttingSum
FROM [MainServer].[Production].[dbo].[CuttingOutput] co
INNER JOIN [MainServer].[Production].[dbo].[CuttingOutput_Detail] cod ON co.id = cod.ID
WHERE 
(co.EditDate BETWEEN '{sDateS}' AND '{eDateS}') OR
(co.AddDate BETWEEN '{sDateS}' AND '{eDateS}')
GROUP BY cod.CuttingID,cod.CutRef,co.cDate,co.FactoryID

/*************找出的workOrder的資料*************/
SELECT [LowerSum] = SUM(Layer)  
	,ID
	,wo.CutRef
	,wo.FactoryID
into #workOrderSUM
FROM [MainServer].[Production].[dbo].[WorkOrderForOutput] wo
inner join #cuttingSum cs with(NOLOCK) on wo.ID = cs.[CuttingID] and wo.CutRef = cs.CutRef
GROUP BY ID,wo.CutRef,wo.FactoryID

/*************找出workOrder與CuttingOutput的Layer相等的資料*************/
SELECT
	[ID] = b.id
	,[CutRef] =  a.CutRef
	,[LackingLayers] = UpperSum - acc.val
	,[cDate] = MincDate.MincoDate
	,[FactoryID] = b.FactoryID
into #sum
FROM #cuttingSum a 
inner join #workOrderSUM b on a.CuttingID = b.id and a.CutRef = b.CutRef
OUTER APPLY
(
	SELECT MincoDate = MIN(co.cdate)
	FROM [MainServer].[Production].[dbo].[CuttingOutput] co WITH (NOLOCK) 
	INNER JOIN [MainServer].[Production].[dbo].[CuttingOutput_Detail] cod WITH (NOLOCK) on co.id = cod.id
	Where cod.CutRef = b.[CutRef] and co.Status != 'New' and co.FactoryID = b.FactoryID and b.CutRef <> ''
)MincDate
OUTER APPLY
(
	SELECT val = sum(cd.Layer) 
	FROM [MainServer].[Production].[dbo].[CuttingOutput_Detail] cd WITH (NOLOCK)
	INNER JOIN [MainServer].[Production].[dbo].[CuttingOutput] c WITH (NOLOCK) ON cd.ID = c.ID
	WHERE cd.CutRef = b.[CutRef] and b.[CutRef] <> ''
)acc
where UpperSum = LowerSum

/*************將上面#tmp表與#sum表union起來*************/
select 
*
into #Integrate
FROM
(
	select 
		[ID]
		,[CutRef]
		,[LackingLayers]
		,[cDate]
		,[FactoryID]
	from #sum 
	union
	select 
		[ID] = t.[Master SP#]
		,[CutRef] = t.[Ref#]
		,[LackingLayers] = t.[LackingLayers]
		,[cDate] = t.[Act.Cutting Date]
		,[FactoryID] = t.[Factory]
	from #tmp t
)aa

update p set
	p.[ActCuttingDate] = t.[cDate],
	p.[LackingLayers] = t.[LackingLayers]
from P_CuttingScheduleOutputList p with(nolock)
inner join #Integrate t with(nolock) on t.[FactoryID] = p.[FactoryID] and t.[ID] = p.[POID] and p.[CutRef] = t.[CutRef]

IF EXISTS (select 1 from BITableInfo b where b.id = 'P_CuttingScheduleOutputList')
BEGIN
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_CuttingScheduleOutputList'
END
ELSE 
BEGIN
	insert into BITableInfo(Id, TransferDate)
	values('P_CuttingScheduleOutputList', getdate())
END
";
                    result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, temptablename: "#tmp", conn: sqlConn, paramters: null);
                    if (!result.Result)
                    {
                        throw result.GetException();
                    }
                }

                finalResult.Result = new DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new DualResult(false, ex);
            }

            return finalResult;
        }
    }
}
