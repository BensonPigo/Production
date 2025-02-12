using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CMPByDate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_CMPByDate(DateTime? sDate, DateTime? eDate, string biTableInfoID)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Base biBase = new Base();
            Sewing_R02 biModel = new Sewing_R02();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/01"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy/MM/01")).AddDays(-1).ToString("yyyy/MM/dd"));
            }

            try
            {
                string sql = @"
select f.MDivisionID, f.ID
from Factory f 
where f.ID in (
    select distinct f.FTYGroup
    from Factory f 
    where f.Junk = 0
    and f.FTYGroup <> ''
)";
                finalResult = new Base_ViewModel()
                {
                    Result = DBProxy.Current.Select("Production", sql, out DataTable factoryTable),
                };

                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                List<P_CMPByDate> p_CMPByDates = new List<P_CMPByDate>();
                Parallel.ForEach(factoryTable.AsEnumerable(), x =>
                {
                    string mDivisionID = x.Field<string>("MDivisionID");
                    string factory = x.Field<string>("ID");
                    for (DateTime date = sDate.Value; date <= eDate.Value; date = date.AddDays(1))
                    {
                        Sewing_R02_ViewModel sewing_R02_Model = new Sewing_R02_ViewModel()
                        {
                            StartOutputDate = date,
                            EndOutputDate = date,
                            Factory = factory,
                            M = mDivisionID,
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

                        DataTable dt = resultReport.DtArr[0];
                        DataTable dtSewingR02byDateDetail = resultReport.DtArr[1];

                        // EXCEL上的 Total CPU Included Subcon-In 加總
                        decimal totalCPUIncludeSubConIn = dtSewingR02byDateDetail.AsEnumerable().Sum(dr => dr.Field<decimal>("TotalCPU"));

                        // 取得 SPH ttlCPU
                        resultReport = biModel.GetSubprocessAndSPHTotalCPU(dt, sewing_R02_Model);
                        if (!resultReport.Result)
                        {
                            throw resultReport.Result.GetException();
                        }

                        DataTable dtSewingR04 = resultReport.DtArr[1];
                        decimal sph_ttlCPU = dtSewingR04.AsEnumerable().Sum(dr => dr.Field<decimal>("SPH_ttlCPU"));

                        #region 呼叫Pams API for [GPH] [SPH] [VPH]
                        AttendanceSummary_APICondition attendanceSummary_API = new AttendanceSummary_APICondition()
                        {
                            FactoryID = factory == "SPR" ? "SXR" : factory,
                            StartDate = date.ToString("yyyy/MM/dd"),
                            EndDate = date.ToString("yyyy/MM/dd"),
                            IsContainShare = mDivisionID.StartsWith("PM"),
                            IsLocal = false,
                        };

                        decimal gph_Manhours = 0, sph_Manhours = 0, ftyManhours = 0;
                        decimal manHoursRatio = 0, ftyHeadcount = 0, revenueDeptHeadcount = 0, manpowerRatio = 0;
                        DualResult result = biModel.GetPamsAttendanceSummaryAsync(attendanceSummary_API, out DataSet dsPams);
                        if (dsPams != null && dsPams.Tables.Count != 0)
                        {
                            gph_Manhours = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["GPH_Manhours"]);
                            sph_Manhours = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["SPH_Manhours"]);
                            ftyManhours = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["FtyManhours"]);
                            manHoursRatio = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["ManHoursRatio"]);
                            ftyHeadcount = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["FtyHeadcount"]);
                            revenueDeptHeadcount = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["RevenueDeptHeadcount"]);
                            manpowerRatio = MyUtility.Convert.GetDecimal(dsPams.Tables["other"].Rows[0]["ManpowerRatio"]);
                        }
                        #endregion

                        P_CMPByDate p_CMPByDate = new P_CMPByDate()
                        {
                            FactoryID = factory,
                            OutputDate = date,
                            GPHCPU = totalCPUIncludeSubConIn,
                            SPHCPU = sph_ttlCPU,
                            VPHCPU = totalCPUIncludeSubConIn + sph_ttlCPU,
                            GPHManhours = gph_Manhours,
                            SPHManhours = sph_Manhours,
                            VPHManhours = ftyManhours,
                            ManhoursRatio = manHoursRatio,
                            TotalActiveHeadcount = ftyHeadcount,
                            RevenumDeptHeadcount = revenueDeptHeadcount,
                            ManpowerRatio = manpowerRatio,
                        };

                        lock (p_CMPByDates)
                        {
                            p_CMPByDates.Add(p_CMPByDate);
                        }
                    }
                });

                // insert into PowerBI
                finalResult = this.UpdateBIData(p_CMPByDates.ToDataTable(), biTableInfoID);
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

        private Base_ViewModel UpdateBIData(DataTable dt, string biTableInfoID)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
update p set p.GPHCPU				    = t.GPHCPU
			,p.SPHCPU				    = t.SPHCPU
			,p.VPHCPU				    = t.VPHCPU
			,p.GPHManhours				= IIF(t.GPHManhours = 0, p.GPHManhours, t.GPHManhours)
			,p.SPHManhours				= IIF(t.SPHManhours = 0, p.SPHManhours, t.SPHManhours)
			,p.VPHManhours				= IIF(t.VPHManhours = 0, p.VPHManhours, t.VPHManhours)
			,p.GPH				        = IIF(t.GPHManhours = 0, p.GPH, t.GPH)
			,p.SPH		                = IIF(t.SPHManhours = 0, p.SPH, t.SPH)
            ,p.VPH		                = IIF(t.VPHManhours = 0, p.VPH,	t.VPH)
            ,p.ManhoursRatio		    = IIF(t.ManhoursRatio = 0, p.ManhoursRatio, t.ManhoursRatio)
            ,p.TotalActiveHeadcount		= IIF(t.TotalActiveHeadcount = 0, p.TotalActiveHeadcount, t.TotalActiveHeadcount)
            ,p.RevenumDeptHeadcount		= IIF(t.RevenumDeptHeadcount = 0, p.RevenumDeptHeadcount, t.RevenumDeptHeadcount)
            ,p.ManpowerRatio		    = IIF(t.ManpowerRatio = 0, p.ManpowerRatio, t.ManpowerRatio)
from P_CMPByDate p
inner join #tmp t on p.FactoryID = t.FactoryID and p.OutputDate = t.OutputDate

insert into P_CMPByDate([FactoryID], [OutputDate], [GPHCPU], [SPHCPU], [VPHCPU], [GPHManhours], [SPHManhours], [VPHManhours], [GPH], [SPH], [VPH], [ManhoursRatio], [TotalActiveHeadcount], [RevenumDeptHeadcount], [ManpowerRatio])
select [FactoryID], [OutputDate], [GPHCPU], [SPHCPU], [VPHCPU], [GPHManhours], [SPHManhours], [VPHManhours], [GPH], [SPH], [VPH], [ManhoursRatio], [TotalActiveHeadcount], [RevenumDeptHeadcount], [ManpowerRatio]
from #tmp t
where not exists(select 1 from P_CMPByDate p where p.FactoryID = t.FactoryID and p.OutputDate = t.OutputDate)

";

                sql += new Base().SqlBITableInfo(biTableInfoID, true);

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: null),
                };
            }

            return finalResult;
        }
    }
}
