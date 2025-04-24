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
    public class P_Import_RightFirstTimeDailyReport
    {
        /// <inheritdoc/>
        public Base_ViewModel P_RightFirstTimeDailyReport(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R20 biModel = new QA_R20();
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
                QA_R20_ViewModel qa_R20 = new QA_R20_ViewModel()
                {
                    CDate1 = sDate,
                    CDate2 = eDate,
                    Factory = string.Empty,
                    Brand = string.Empty,
                    Line = string.Empty,
                    Cell = string.Empty,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetQA_R20Data(qa_R20);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;
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
            string where = @" not exists (
    select 1 from #tmp t 
    where p.FactoryID = t.Factory
	and p.CDate = t.CDate
	and p.OrderID = t.OrderID
	and p.Team = t.Team
	and p.Shift = t.Shift
	and p.Line = t.Line
) 
and p.CDate between @SDate and @EDate";

            string tmp = new Base().SqlBITableHistory("P_RightFirstTimeDailyReport", "P_RightFirstTimeDailyReport_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };

                string sql = $@"	
update t
set t.[Destination] = s.[Destination]
, t.[BrandID] = s.[Brand]
, t.[StyleID] = s.[Style]
, t.[BuyerDelivery] = s.[BuyerDelivery]
, t.[CDCodeID] = s.[CDCode]
, t.[CDCodeNew] = s.[CDCodeNew]
, t.[ProductType] = s.[ProductType]
, t.[FabricType] = s.[FabricType]
, t.[Lining] = s.[Lining]
, t.[Gender] = s.[Gender]
, t.[Construction] = s.[Construction]
, t.[Cell] = s.[Cell]
, t.[InspectQty] = s.[InspectQty]
, t.[RejectQty] = s.[RejectQty]
, t.[RFTPercentage] = s.[RFT (%)]
, t.[Over] = s.[Over]
, t.[QC] = s.[QC]
, t.[Remark] = s.[Remark]
, t.[BIFactoryID] = s.[BIFactoryID]
, t.[BIInsertDate] = s.[BIInsertDate]
from P_RightFirstTimeDailyReport t 
inner join #tmp s
	on t.FactoryID = s.Factory
	and t.CDate = s.CDate
	and t.OrderID = s.OrderID
	and t.Team = s.Team
	and t.Shift = s.Shift
	and t.Line = s.Line

insert into P_RightFirstTimeDailyReport (
 [FactoryID],[CDate],[OrderID],[Destination],[BrandID],[StyleID],[BuyerDelivery],[CDCodeID]
,[CDCodeNew],[ProductType],[FabricType],[Lining],[Gender],[Construction],[Team],[Shift]
,[Line],[Cell],[InspectQty],[RejectQty],[RFTPercentage],[Over],[QC],[Remark],[BIFactoryID], [BIInsertDate]
)
select s.[Factory],s.[CDate],s.[OrderID],s.[Destination],s.[Brand],s.[Style],s.[BuyerDelivery],s.[CDCode]
,s.[CDCodeNew],s.[ProductType],s.[FabricType],s.[Lining],s.[Gender],s.[Construction],s.[Team],s.[Shift]
,s.[Line],s.[Cell],s.[InspectQty],s.[RejectQty],s.[RFT (%)],s.[Over],s.[QC],s.[Remark],[BIFactoryID], [BIInsertDate]
from #tmp s
where not exists (
    select 1 from P_RightFirstTimeDailyReport t 
    where t.FactoryID = s.Factory
	and t.CDate = s.CDate
	and t.OrderID = s.OrderID
	and t.Team = s.Team
	and t.Shift = s.Shift
	and t.Line = s.Line
)

{tmp}

delete t 
from dbo.P_RightFirstTimeDailyReport t
where not exists (
    select 1 from #tmp s 
    where t.FactoryID = s.Factory
	and t.CDate = s.CDate
	and t.OrderID = s.OrderID
	and t.Team = s.Team
	and t.Shift = s.Shift
	and t.Line = s.Line
)
and t.CDate between @SDate and @EDate

if exists (select 1 from BITableInfo where Id = 'P_RightFirstTimeDailyReport')
begin
	update BITableInfo set TransferDate = getdate(), IS_Trans = 0
	where Id = 'P_RightFirstTimeDailyReport'
end
else 
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans)
	values('P_RightFirstTimeDailyReport', getdate(), 0)
end
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
