
using Ict;
using PostJobLog;
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
    public class P_Import_MonthlySewingOutputSummary
    {
        /// <inheritdoc/>
        public Base_ViewModel P_MonthlySewingOutputSummary(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Base biBase = new Base();
            Sewing_R02 biModel = new Sewing_R02();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/01"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy/MM/01")).AddDays(-1).ToString("yyyy/MM/dd"));
            }

            try
            {
                Sewing_R02_ViewModel sewing_R02_Model = new Sewing_R02_ViewModel()
                {
                    StartOutputDate = sDate,
                    EndOutputDate = eDate,
                    Factory = string.Empty,
                    M = string.Empty,
                    ReportType = 1,
                    StartSewingLine = string.Empty,
                    EndSewingLine = string.Empty,
                    OrderBy = 1,
                    ExcludeNonRevenue = false,
                    ExcludeSampleFactory = false,
                    ExcludeOfMockUp = false,
                };

                Base_ViewModel resultReport = biModel.GetMonthlyProductionOutputReport(sewing_R02_Model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[0];
                DataTable totalTable = this.GetTotalTable(detailTable);
                resultReport = biModel.GetSubprocessByFactoryOutPutDate(detailTable);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable subprocessData = resultReport.Dt;

                string factoryID = MyUtility.GetValue.Lookup("select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]");
                DateTime dateTime = DateTime.Now;
                List<P_MonthlySewingOutputSummary_ViewModel> dataList = new List<P_MonthlySewingOutputSummary_ViewModel>();
                foreach (DataRow dr in totalTable.Rows)
                {
                    var outputdateList = detailTable.AsEnumerable()
                        .Where(x => x.Field<string>("FactoryID").EqualString(dr["Fty"]))
                        .GroupBy(x => x.Field<DateTime>("OutputDate"))
                        .Select(x => x.Key).ToList();
                    List<APIData> pams = new List<APIData>();
                    sewing_R02_Model = new Sewing_R02_ViewModel()
                    {
                        M = biBase.GetMDivision(dr["Fty"].ToString()),
                        Factory = dr["Fty"].ToString(),
                        StartDate = DateTime.Parse(DateTime.Parse(dr["LastDatePerMonth"].ToString()).ToString("yyyy/MM/01")),
                        EndDate = DateTime.Parse(dr["LastDatePerMonth"].ToString()),
                        IsCN = this.IsCN(dr["Fty"].ToString()),
                    };
                    pams = biModel.GetPAMS(sewing_R02_Model).Where(x => outputdateList.Contains(x.Date)).ToList();
                    resultReport = biModel.GetWorkDay(detailTable.AsEnumerable().Where(x => x.Field<string>("FactoryID").EqualString(dr["Fty"])).CopyToDataTable(), sewing_R02_Model);
                    if (!resultReport.Result)
                    {
                        throw new Exception("Query Work Day fail\r\n" + resultReport.Result.Messages.ToString());
                    }

                    int workDay = resultReport.IntValue;

                    P_MonthlySewingOutputSummary_ViewModel model = new P_MonthlySewingOutputSummary_ViewModel()
                    {
                        Fty = dr["Fty"].ToString(),
                        Period = dr["Period"].ToString(),
                        LastDatePerMonth = DateTime.Parse(dr["LastDatePerMonth"].ToString()).ToString("yyyy/MM/dd"),
                        TtlQtyExclSubconOut = int.Parse(Math.Round(decimal.Parse(dr["TtlQtyExclSubconOut"].ToString()), 0).ToString()),
                        TtlCPUInclSubconIn = Math.Round(decimal.Parse(dr["TtlCPUInclSubconIn"].ToString()), 3),
                        SubconInTtlCPU = Math.Round(decimal.Parse(dr["SubconInTtlCPU"].ToString()), 3),
                        SubconOutTtlCPU = Math.Round(decimal.Parse(dr["SubconOutTtlCPU"].ToString()), 3),
                        PPH = Math.Round(decimal.Parse(dr["TtlCPUInclSubconIn"].ToString()) / decimal.Parse(dr["TtlManhours"].ToString()), 2),
                        AvgWorkHr = Math.Round(decimal.Parse(dr["TtlManhours"].ToString()) / decimal.Parse(dr["TtlManpower"].ToString()), 2),
                        TtlManpower = int.Parse(Math.Round(decimal.Parse(dr["TtlManpower"].ToString()), 0).ToString()),
                        TtlManhours = Math.Round(decimal.Parse(dr["TtlManhours"].ToString()), 1),
                        Eff = decimal.Parse(dr["TtlManhours"].ToString()) == 0 ? 0 : Math.Round((decimal.Parse(dr["TtlCPUInclSubconIn"].ToString()) / (decimal.Parse(dr["TtlManhours"].ToString()) * 3600 / 1400)) * 100, 1), // =IF(I10=0,0,ROUND((C10/(I10*3600/1400))*100,1))
                        AvgWorkHrPAMS = pams.Sum(y => y.SewTtlManpower) == 0 ? 0 : Math.Round(pams.Sum(y => y.SewTtlManhours) / pams.Sum(y => y.SewTtlManpower), 2),
                        TtlManpowerPAMS = int.Parse(Math.Round(pams.Sum(y => y.SewTtlManpower), 0).ToString()),
                        TtlManhoursPAMS = Math.Round(pams.Sum(y => y.SewTtlManhours), 4),
                        EffPAMS = pams.Sum(y => y.SewTtlManhours) == 0 ? 0 : Math.Round(decimal.Parse(dr["TtlCPUInclSubconIn"].ToString()) / (pams.Sum(y => y.SewTtlManhours) * 3600 / 1400) * 100, 1), // =IF(M6=0,0,ROUND((C6/(M6*3600/1400))*100,1))
                        TransferManpowerPAMS = int.Parse(Math.Round(pams.Sum(y => y.TransManpowerIn) - pams.Sum(y => y.TransManpowerOut), 0).ToString()),
                        TransferManhoursPAMS = Math.Round(pams.Sum(y => y.TransManhoursIn) - pams.Sum(y => y.TransManhoursOut), 4),
                        TtlRevenue = Math.Round(
                            decimal.Parse(dr["TtlCPUInclSubconIn"].ToString()) +
                            decimal.Parse(dr["SubconOutTtlCPU"].ToString()) +
                            subprocessData.AsEnumerable()
                                    .Where(s => s.Field<string>("FactoryID") == dr["Fty"].ToString()
                                            && s.Field<DateTime>("OutputDate") == DateTime.Parse(dr["LastDatePerMonth"].ToString()))
                                    .Select(s => s.Field<decimal>("Price")).FirstOrDefault(), 3),
                        TtlWorkDay = workDay,
                        BIFactoryID = factoryID,
                        BIInsertDate = dateTime,
                    };

                    dataList.Add(model);
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataList);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                if (finalResult.Result)
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_MonthlySewingOutputSummary", false);
                }
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        /// <summary>
        /// 重新計算 取得加總資料
        /// </summary>
        /// <param name="dt">Detail Data</param>
        /// <returns>DataTable</returns>
        private DataTable GetTotalTable(DataTable dt)
        {
            string sql = @"
select t.FactoryID
	, t.OutputDate
	, t.LastDatePerMonth
	, QAQty = isnull (q.QAQty, 0)
	, TotalCPU = isnull (q.TotalCPU, 0)
	, SInCPU = isnull (ic.TotalCPU, 0)
	, SoutCPU = isnull (oc.TotalCPU, 0)
	, CPUSewer = isnull (IIF(q.ManHour = 0, 0, isnull(q.TotalCPU, 0) / q.ManHour), 0)
	, AvgWorkHour = isnull (IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)), 0)
	, ManPower = isnull (mp.ManPower, 0)
	, ManHour = isnull (q.ManHour, 0)
	, Eff = isnull (IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2)), 0)
into #tmp_dayTotal
from (
	select distinct t.FactoryID
		, t.OutputDate
		, [LastDatePerMonth] = dateadd(day, -1, cast(FORMAT(dateadd(month, 1, t.OutputDate), 'yyyy/MM/01') as date))
	from #tmp t
) t
left join (
	select t.FactoryID
			, t.OutputDate
			, StdTMS
			, QAQty = Sum(QAQty)
			, ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
			, TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3) 
	from #tmp t
	where LastShift <> 'O'
	group by t.FactoryID, t.OutputDate, t.StdTMS
) q on t.FactoryID = q.FactoryID and t.OutputDate = q.OutputDate 
left join (
	select t.FactoryID
			, t.OutputDate
			, TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3) 
	from #tmp t
	where LastShift = 'I'
	group by t.FactoryID, t.OutputDate
) ic on t.FactoryID = ic.FactoryID and t.OutputDate = ic.OutputDate 
left join (
	select t.FactoryID
			, t.OutputDate
			, TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3) 
	from #tmp t
	where LastShift  = 'O'
	group by t.FactoryID, t.OutputDate
) oc on t.FactoryID = oc.FactoryID and t.OutputDate = oc.OutputDate 
left join (
		select a.FactoryID
				, a.OutputDate
				, ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
		from (
			select OutputDate
				   , FactoryID
				   , SewingLineID
				   , LastShift
				   , Team
				   , ManPower = Max(ActManPower)
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team
		) a
		outer apply(
			select ManPower
			from (
				select OutputDate
					   , FactoryID
					   , SewingLineID
					   , LastShift
					   , Team
					   , ManPower = Max(ActManPower)
				from #tmp
				where LastShift = 'I' 
				group by OutputDate, FactoryID, SewingLineID, LastShift, Team
			) m2
			where m2.Team = a.Team 
				  and m2.SewingLineID = a.SewingLineID	
				  and a.OutputDate = m2.OutputDate
				  and m2.FactoryID = a.FactoryID	
		) d
		group by a.FactoryID, a.OutputDate
) mp on t.FactoryID = mp.FactoryID and t.OutputDate = mp.OutputDate 


select [Fty] = t.FactoryID
	, [Period] = format(t.OutputDate, 'yyyyMM')
	, t.LastDatePerMonth
	, [TtlQtyExclSubconOut] = SUM(t.QAQty)
	, [TtlCPUInclSubconIn] = SUM(t.TotalCPU)
	, [SubconInTtlCPU] = SUM(SInCPU)
	, [SubconOutTtlCPU] = SUM(SoutCPU)
	, [PPH] = SUM(CPUSewer)
	, [AvgWorkHr] = AVG(AvgWorkHour)
	, [TtlManpower] = SUM(ManPower)
	, [TtlManhours] = SUM(ManHour)
	, [Eff] = SUM(Eff)
from #tmp_dayTotal t
group by t.FactoryID, format(t.OutputDate, 'yyyyMM'), t.LastDatePerMonth

drop table #tmp_dayTotal
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable),
            };
            if (!resultReport.Result)
            {
                throw resultReport.Result.GetException();
            }

            return dataTable;
        }

        /// <inheritdoc/>
        private bool IsCN(string factoryID)
        {
            string sql = $"select 1 from Factory f where f.ID = '{factoryID}' and f.MDivisionID in ('CM1', 'CM2')";
            return MyUtility.Check.Seek(sql, connectionName: "Production");
        }

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(List<P_MonthlySewingOutputSummary_ViewModel> dataList)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"
insert into P_MonthlySewingOutputSummary([Fty], [Period], [LastDatePerMonth], [TtlQtyExclSubconOut], [TtlCPUInclSubconIn], [SubconInTtlCPU], [SubconOutTtlCPU], [PPH], [AvgWorkHr], [TtlManpower], [TtlManhours], [Eff], [AvgWorkHrPAMS], [TtlManpowerPAMS], [TtlManhoursPAMS], [EffPAMS], [TransferManpowerPAMS], [TransferManhoursPAMS], [TtlRevenue], [TtlWorkDay], [BIFactoryID], [BIInsertDate])
select [Fty], [Period], [LastDatePerMonth], [TtlQtyExclSubconOut], [TtlCPUInclSubconIn], [SubconInTtlCPU], [SubconOutTtlCPU], [PPH], [AvgWorkHr], [TtlManpower], [TtlManhours], [Eff], [AvgWorkHrPAMS], [TtlManpowerPAMS], [TtlManhoursPAMS], [EffPAMS], [TransferManpowerPAMS], [TransferManhoursPAMS], [TtlRevenue], [TtlWorkDay], [BIFactoryID], [BIInsertDate]
from #tmp t
where not exists (select 1 from P_MonthlySewingOutputSummary p where t.[Fty] = p.[Fty] and t.[Period] = p.[Period])

update p
    set p.[LastDatePerMonth]			= t.[LastDatePerMonth]
        , p.[TtlQtyExclSubconOut]		= t.[TtlQtyExclSubconOut]
        , p.[TtlCPUInclSubconIn]		= t.[TtlCPUInclSubconIn]
        , p.[SubconInTtlCPU]			= t.[SubconInTtlCPU]
        , p.[SubconOutTtlCPU]			= t.[SubconOutTtlCPU]
        , p.[PPH]						= t.[PPH]
        , p.[AvgWorkHr]					= t.[AvgWorkHr]
        , p.[TtlManpower]				= t.[TtlManpower]
        , p.[TtlManhours]				= t.[TtlManhours]
        , p.[Eff]						= t.[Eff]
        , p.[AvgWorkHrPAMS]				= t.[AvgWorkHrPAMS]
        , p.[TtlManpowerPAMS]			= t.[TtlManpowerPAMS]
        , p.[TtlManhoursPAMS]			= t.[TtlManhoursPAMS]
        , p.[EffPAMS]					= t.[EffPAMS]
        , p.[TransferManpowerPAMS]		= t.[TransferManpowerPAMS]
        , p.[TransferManhoursPAMS]		= t.[TransferManhoursPAMS]
        , p.[TtlRevenue]				= t.[TtlRevenue]
        , p.[TtlWorkDay]				= t.[TtlWorkDay]
        , p. [BIFactoryID]              = t.[BIFactoryID]
        , p.[BIInsertDate]              = t.[BIInsertDate]
from P_MonthlySewingOutputSummary p
inner join #tmp t on t.[Fty] = p.[Fty] and t.[Period] = p.[Period]

";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dataList.ToDataTable(), null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return finalResult;
        }
    }
}
