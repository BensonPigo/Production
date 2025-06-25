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
    public class P_Import_ReplacementReport
    {
        /// <inheritdoc/>
        public Base_ViewModel P_ReplacementReport(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R08 biModel = new PPIC_R08();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse("2020/01/01");
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse("2999/01/01");
            }

            try
            {
                PPIC_R08_ViewModel ppic_R08 = new PPIC_R08_ViewModel()
                {
                    CDate1 = item.SDate,
                    CDate2 = item.EDate,
                    IsPowerBI = true,
                    ReportType = "Resp. Dept. List",
                    Status = "ALL",
                };

                Base_ViewModel resultReport = biModel.GetPPIC_R08Data(ppic_R08);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[0];

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            string where = @" p.Cdate >= @SDate and p.Cdate <= @EDate";
            string tmp = new Base().SqlBITableHistory("P_ReplacementReport", "P_ReplacementReport_History", "#tmp", where, false, true);

            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                };
                string sql = $@"
UPDATE t
SET   　t.[Type] = s.[Type]
,t.[MDivisionID] = s.[MDivisionID]
,t.[SPNo] = s.[SPNo]
,t.[Style] = s.[Style]
,t.[Season] = s.[Season]
,t.[Brand] = s.[Brand]
,t.[Status] = s.[Status]
,t.[Cdate] = s.[Cdate]
,t.[FtyApvDate] = s.[FtyApvDate]
,t.[CompleteDate] = s.[CompleteDate]
,t.[LockDate] = s.[LockDate]
,t.[Responsibility] = s.[Responsibility]
,t.[TtlEstReplacementAMT] = s.[TtlEstReplacementAMT]
,t.[RMtlUS] = s.[RMtlUS]
,t.[ActFreightUS] = s.[ActFreightUS]
,t.[EstFreightUS] = s.[EstFreightUS]
,t.[SurchargeUS] = s.[SurchargeUS]
,t.[TotalUS] = s.[TotalUS]
,t.[ResponsibilityPercent] = s.[ResponsibilityPercent]
,t.[ShareAmount] = s.[ShareAmount]
,t.[VoucherNo] = s.[VoucherNo]
,t.[VoucherDate] = s.[VoucherDate]
,t.[POSMR] = s.[POSMR]
,t.[POHandle] = s.[POHandle]
,t.[PCSMR] = s.[PCSMR]
,t.[PCHandle] = s.[PCHandle]
,t.[Prepared] = s.[Prepared]
,t.[PPIC/Factory mgr] = s.[PPIC/Factory mgr]
,t.[BIFactoryID] = @BIFactoryID
,t.[BIInsertDate] = GETDATE()
from P_ReplacementReport t 
inner join #tmp s on t.ID = s.ID 
AND t.FactoryID = s.FactoryID 
AND t.ResponsibilityFty = s.ResponsibilityFty 
AND t.ResponsibilityDept = s.ResponsibilityDept


insert into P_ReplacementReport (
[ID],[Type],[MDivisionID],[FactoryID],[SPNo],[Style]
,[Season],[Brand],[Status],[Cdate],[FtyApvDate],[CompleteDate],[LockDate]
,[Responsibility],[TtlEstReplacementAMT]
,[RMtlUS],[ActFreightUS],[EstFreightUS],[SurchargeUS],[TotalUS],[ResponsibilityFty]
,[ResponsibilityDept],[ResponsibilityPercent],[ShareAmount],[VoucherNo],[VoucherDate]
,[POSMR],[POHandle],[PCSMR],[PCHandle],[Prepared],[PPIC/Factory mgr], BIFactoryID, BIInsertDate
)
select 	s.[ID],s.[Type],s.[MDivisionID],s.[FactoryID],s.[SPNo],s.[Style]
,s.[Season],s.[Brand],s.[Status],s.[Cdate],s.[FtyApvDate],s.[CompleteDate],s.[LockDate]
,s.[Responsibility],s.[TtlEstReplacementAMT]
,s.[RMtlUS],s.[ActFreightUS],s.[EstFreightUS],s.[SurchargeUS],s.[TotalUS],s.[ResponsibilityFty]
,s.[ResponsibilityDept],s.[ResponsibilityPercent],s.[ShareAmount],s.[VoucherNo],s.[VoucherDate]
,s.[POSMR],s.[POHandle],s.[PCSMR],s.[PCHandle],s.[Prepared],s.[PPIC/Factory mgr], @BIFactoryID, GETDATE()
from #tmp s
where not exists (
    select 1 from P_ReplacementReport t 
    where t.ID = s.ID 
	AND t.FactoryID = s.FactoryID 
	AND t.ResponsibilityFty = s.ResponsibilityFty 
	AND t.ResponsibilityDept = s.ResponsibilityDept
)

{tmp}

delete t 
from dbo.P_ReplacementReport t
where not exists (
    select 1 from #tmp s 
    where t.ID = s.ID 
	AND t.FactoryID = s.FactoryID 
	AND t.ResponsibilityFty = s.ResponsibilityFty 
	AND t.ResponsibilityDept = s.ResponsibilityDept
)
and t.Cdate >= @SDate 
and t.Cdate <= @EDate
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
