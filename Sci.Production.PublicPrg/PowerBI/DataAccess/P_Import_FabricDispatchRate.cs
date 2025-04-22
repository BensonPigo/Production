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
    public class P_Import_FabricDispatchRate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_FabricDispatchRate(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                if (!sDate.HasValue)
                {
                    sDate = DateTime.Now.AddMonths(-3);
                }

                Base_ViewModel resultReport = this.GetFabricDispatchRateData(sDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, sDate.Value);
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

        /// <inheritdoc/>
        private Base_ViewModel GetFabricDispatchRateData(DateTime? sDate)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                };

                string sql = @"	
select f.FTYGroup
	, p.EstCutDate
	, p.DispatchTime
	, p.IssueQty
into #tmp
from P_IssueFabricByCuttingTransactionList p
inner join [MainServer].Production.dbo.Factory f on f.ID = p.FactoryID
where p.EstCutDate >= @SDate

	
select [FactoryID] = t.FTYGroup
	, [EstCutDate] = t.EstCutDate
	, [RequestedDispatchYards] = isnull(t2.RequestedDispatchYards, 0)
	, [DispatchYards] = isnull(t3.DispatchYards, 0)
	, [FabricDispatchRate] = cast((case when isnull(t2.RequestedDispatchYards, 0) = 0 then 0
								when t3.DispatchYards >= t2.RequestedDispatchYards then 1
								else isnull(t3.DispatchYards, 0) * 1.0 / t2.RequestedDispatchYards end) * 100 as decimal(5, 2))
    , [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    , [BIInsertDate] = GETDATE()
from (select distinct FTYGroup, EstCutDate from #tmp) t
left join (
	select t.FTYGroup
		, t.EstCutDate
		, [RequestedDispatchYards] = SUM(IssueQty)
	from #tmp t
	group by t.FTYGroup, t.EstCutDate
) t2 on t.FTYGroup = t2.FTYGroup and t.EstCutDate = t2.EstCutDate
left join (
	select t.FTYGroup
		, t.EstCutDate
		, [DispatchYards] = SUM(IssueQty)
	from #tmp t
	where t.EstCutDate >= t.DispatchTime
	group by t.FTYGroup, t.EstCutDate
) t3 on t.FTYGroup = t3.FTYGroup and t.EstCutDate = t3.EstCutDate 

drop table #tmp
";
                finalResult = new Base_ViewModel
                {
                    Result = DBProxy.Current.SelectByConn(conn: sqlConn, cmdtext: sql, parameters: sqlParameters, datas: out DataTable dataTable),
                    Dt = dataTable,
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string tmp = new Base().SqlBITableHistory("P_FabricDispatchRate", "P_FabricDispatchRate_History", "#tmp", string.Empty, false, true);

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                };
                string sql = $@"	
--delete p
--from P_FabricDispatchRate p
--where EstCutDate >= @SDate
--and not exists(select 1 from #tmp t where p.EstCutDate = t.EstCutDate and p.FactoryID = t.FactoryID)

update p 
	set p.FabricDispatchRate = t.FabricDispatchRate
       ,p.BIFactoryID = t.BIFactoryID
       ,p.BIInsertDate = t.BIInsertDate
from P_FabricDispatchRate p
inner join #tmp t on p.EstCutDate = t.EstCutDate and p.FactoryID = t.FactoryID

insert into P_FabricDispatchRate(EstCutDate,FactoryID,FabricDispatchRate, BIFactoryID, BIInsertDate)
select	 t.EstCutDate
		,t.FactoryID
		,t.FabricDispatchRate
        ,t.BIFactoryID
        ,t.BIInsertDate
from #tmp t
where not exists(	select 1 
					from P_FabricDispatchRate p
					where p.EstCutDate = t.EstCutDate and p.FactoryID = t.FactoryID)

{tmp}

if exists(select 1 from BITableInfo where Id = 'P_FabricDispatchRate')
begin
	update BITableInfo set TransferDate = getdate()
	where Id = 'P_FabricDispatchRate'
end
else
begin
	insert into BITableInfo(Id, TransferDate) values('P_FabricDispatchRate', GETDATE())
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
