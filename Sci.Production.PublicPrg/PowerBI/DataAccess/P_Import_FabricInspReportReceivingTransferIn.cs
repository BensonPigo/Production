using System;
using System.Collections.Generic;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using System.Data.SqlClient;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_FabricInspReportReceivingTransferIn
    {
        /// <inheritdoc/>
        public Base_ViewModel P_FabricInspReportReceivingTransferIn(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R11 biModel = new QA_R11();
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
                QA_R11_ViewModel qa_R11 = new QA_R11_ViewModel()
                {
                    ArriveWHDate1 = item.SDate,
                    ArriveWHDate2 = item.EDate,
                    SP1 = string.Empty,
                    SP2 = string.Empty,
                    Brand = string.Empty,
                    Refno1 = string.Empty,
                    Refno2 = string.Empty,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetQA_R11Data(qa_R11);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[1];

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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

            string where = @" 
not exists (
    select 1 from #tmp s 
    where p.ReceivingID = s.ReceivingID 
)
and 
(
	(p.AddDate between @SDate and @EDate)
	or
	(p.EditDate between @SDate and @EDate)
)
";

            string tmp = new Base().SqlBITableHistory("P_FabricInspReport_ReceivingTransferIn", "P_FabricInspReport_ReceivingTransferIn_History", "#tmp", where, false, true);

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = $@"	
UPDATE t
SET 
	   t.[Wkno]                     = ISNULL(s.[Wkno], '')
      ,t.[StyleID]                  = ISNULL(s.[StyleID], '')
      ,t.[BrandID]                  = ISNULL(s.[BrandID], '')
      ,t.[Supplier]                 = ISNULL(s.[Supplier], '')
      ,t.[Refno]                    = ISNULL(s.[Refno], '')
      ,t.[Color]                    = ISNULL(s.[Color], '')
      ,t.[ArriveWHDate]             = s.[ArriveWHDate]
      ,t.[ArriveQty]                = ISNULL(s.[ArriveQty], 0)
      ,t.[WeaveTypeID]              = ISNULL(s.[WeaveTypeID], '')
      ,t.[CutWidth]                 = ISNULL(s.[CutWidth], 0)
      ,t.[Weight]                   = ISNULL(s.[Weight], 0)
      ,t.[Composition]              = ISNULL(s.[Composition], '')
      ,t.[Desc]                     = ISNULL(s.[Desc], '')
      ,t.[FabricConstructionID]     = ISNULL(s.[FabricConstructionID], '')
      ,t.[InspDate]                 = s.[InspDate]
      ,t.[Result]                   = ISNULL(s.[Result], '')
      ,t.[Grade]                    = ISNULL(s.[Grade], '')
      ,t.[DefectType]               = ISNULL(s.[DefectType], '')
      ,t.[DefectDesc]               = ISNULL(s.[DefectDesc], '')
      ,t.[Points]                   = ISNULL(s.[Points], 0)
      ,t.[DefectRate]               = ISNULL(s.[DefectRate], 0)
      ,t.[Inspector]                = ISNULL(s.[Inspector], '')
      ,t.[EditDate]                 = s.[EditDate]
     , t.[BIFactoryID]              = @BIFactoryID
     , t.[BIInsertDate]             = GETDATE()
from P_FabricInspReport_ReceivingTransferIn t 
inner join #tmp s on t.POID = s.POID
	and t.SEQ = s.SEQ
	and t.ReceivingID = s.ReceivingID
	and t.Dyelot = s.Dyelot
	and t.Roll = s.Roll
	and t.DefectCode = s.DefectCode


insert into P_FabricInspReport_ReceivingTransferIn (
     [POID],[SEQ],[Wkno],[ReceivingID],[StyleID],[BrandID],[Supplier],[Refno],[Color],[ArriveWHDate],[ArriveQty],[WeaveTypeID]
,[Dyelot],[CutWidth],[Weight],[Composition],[Desc],[FabricConstructionID],[Roll],[InspDate],[Result],[Grade]
,[DefectCode],[DefectType],[DefectDesc],[Points],[DefectRate],[Inspector],[AddDate],[EditDate],[BIFactoryID], [BIInsertDate]
)
select s.[POID]
    , s.[SEQ]
    , [Wkno]                    = ISNULL(s.[Wkno], '')
    , s.[ReceivingID]
    , [StyleID]                 = ISNULL(s.[StyleID], '')
    , [BrandID]                 = ISNULL(s.[BrandID], '')
    , [Supplier]                = ISNULL(s.[Supplier], '')
    , [Refno]                   = ISNULL(s.[Refno], '')
    , [Color]                   = ISNULL(s.[Color], '')
    , s.[ArriveWHDate]
    , [ArriveQty]               = ISNULL(s.[ArriveQty], 0)
    , [WeaveTypeID]             = ISNULL(s.[WeaveTypeID], '')
    , s.[Dyelot]
    , [CutWidth]                = ISNULL(s.[CutWidth], 0)
    , [Weight]                  = ISNULL(s.[Weight], 0)
    , [Composition]             = ISNULL(s.[Composition], '')
    , [Desc]                    = ISNULL(s.[Desc], '')
    , [FabricConstructionID]    = ISNULL(s.[FabricConstructionID], '')
    , s.[Roll]
    , s.[InspDate]
    , [Result]                  = ISNULL(s.[Result], '')
    , [Grade]                   = ISNULL(s.[Grade], '')
    , s.[DefectCode]
    , [DefectType]              = ISNULL(s.[DefectType], '')
    , [DefectDesc]              = ISNULL(s.[DefectDesc], '')
    , [Points]                  = ISNULL(s.[Points], 0)
    , [DefectRate]              = ISNULL(s.[DefectRate], 0)
    , [Inspector]               = ISNULL(s.[Inspector], '')
    , s.[AddDate]
    , s.[EditDate]
    , @BIFactoryID
    , GETDATE()
from #tmp s
where not exists (
    select 1 from P_FabricInspReport_ReceivingTransferIn t 
    where  t.POID = s.POID
	and t.SEQ = s.SEQ
	and t.ReceivingID = s.ReceivingID
	and t.Dyelot = s.Dyelot
	and t.Roll = s.Roll
	and t.DefectCode = s.DefectCode
)

{tmp}

delete t 
from dbo.P_FabricInspReport_ReceivingTransferIn t
where not exists (
    select 1 from #tmp s 
    where t.ReceivingID = s.ReceivingID 
)
and 
(
	(T.AddDate between @SDate and @EDate)
	or
	(T.EditDate between @SDate and @EDate)
)
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
