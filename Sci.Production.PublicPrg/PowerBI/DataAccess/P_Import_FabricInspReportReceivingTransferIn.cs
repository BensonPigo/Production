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
        public Base_ViewModel P_FabricInspReport_ReceivingTransferIn(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R11 biModel = new QA_R11();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                QA_R11_ViewModel qa_R11 = new QA_R11_ViewModel()
                {
                    ArriveWHDate1 = sDate,
                    ArriveWHDate2 = eDate,
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
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value);
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
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };
                string sql = @"	
UPDATE t
SET 
	   t.[Wkno] = s.[Wkno]
      ,t.[StyleID] = s.[StyleID]
      ,t.[BrandID] = s.[BrandID]
      ,t.[Supplier] = s.[Supplier]
      ,t.[Refno] = s.[Refno]
      ,t.[Color] = s.[Color]
      ,t.[ArriveWHDate] = s.[ArriveWHDate]
      ,t.[ArriveQty] = s.[ArriveQty]
      ,t.[WeaveTypeID] = s.[WeaveTypeID]
      ,t.[CutWidth] = s.[CutWidth]
      ,t.[Weight] = s.[Weight]
      ,t.[Composition] = s.[Composition]
      ,t.[Desc] = s.[Desc]
      ,t.[FabricConstructionID] = s.[FabricConstructionID]
      ,t.[InspDate] = s.[InspDate]
      ,t.[Result] = s.[Result]
      ,t.[Grade] = s.[Grade]
	  ,t.[DefectCode] = s.[DefectCode]
      ,t.[DefectType] = s.[DefectType]
      ,t.[DefectDesc] = s.[DefectDesc]
      ,t.[Points] = s.[Points]
      ,t.[DefectRate] = s.[DefectRate]
      ,t.[Inspector] = s.[Inspector]
       ,t.[EditDate] = s.[EditDate]
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
,[DefectCode],[DefectType],[DefectDesc],[Points],[DefectRate],[Inspector],[AddDate],[EditDate]
)
select 	   s.[POID],s.[SEQ],s.[Wkno],s.[ReceivingID],s.[StyleID],s.[BrandID],s.[Supplier],s.[Refno],s.[Color],s.[ArriveWHDate],s.[ArriveQty]
,s.[WeaveTypeID],s.[Dyelot],s.[CutWidth],s.[Weight],s.[Composition],s.[Desc],s.[FabricConstructionID],s.[Roll],s.[InspDate]
,s.[Result],s.[Grade],s.[DefectCode],s.[DefectType],s.[DefectDesc],s.[Points],s.[DefectRate],s.[Inspector]
,s.[AddDate],s.[EditDate]
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


if exists (select 1 from BITableInfo b where b.id = 'P_FabricInspReport_ReceivingTransferIn')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_FabricInspReport_ReceivingTransferIn'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_FabricInspReport_ReceivingTransferIn', getdate())
	end

";
                finalResult = new Base_ViewModel()
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
