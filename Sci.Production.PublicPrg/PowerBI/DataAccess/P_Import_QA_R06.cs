using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/R06已脫鉤 待討論
    /// </summary>
    public class P_Import_QA_R06
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_QA_R06(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                var today = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
                sDate = firstDayOfCurrentMonth.AddMonths(-6);
            }

            if (!eDate.HasValue)
            {
                var today = DateTime.Now;
                var firstDayOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
                eDate = firstDayOfNextMonth.AddDays(-1);
            }

            try
            {
                finalResult = this.Get_QA_R06_Data((DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable dataTable = finalResult.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_QA_R06", false);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel Get_QA_R06_Data(DateTime sdate, DateTime edate)
        {
            StringBuilder sqlcmdSP = new StringBuilder();

            sqlcmdSP.Append("exec dbo.P_Import_QA_R06");
            sqlcmdSP.Append($"'{sdate.ToString("yyyy/MM/dd")}',"); // sDate
            sqlcmdSP.Append($"'{edate.ToString("yyyy/MM/dd")}'"); // eDate

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sqlcmdSP.ToString(), out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate, DateTime edate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@SDate", sdate));
            lisSqlParameter.Add(new SqlParameter("@EDate", edate));

            using (sqlConn)
            {
                string sql = new Base().SqlBITableHistory("P_QA_R06", "P_QA_R06_History", "#Final", "p.WhseArrival >= @SDate AND p.WhseArrival <= @EDate", needJoin: false) + Environment.NewLine;
                sql += $@"
                DELETE t
                FROM POWERBIReportData.dbo.P_QA_R06 t
                WHERE NOT EXISTS (
                    SELECT 1 
                    FROM #Final s 
                    WHERE t.SuppID = s.SuppID 
                        AND t.Refno = s.Refno 
                        AND t.WhseArrival = s.WhseArrival 
                        AND t.FactoryID = s.FactoryID 
                        AND t.POID = s.POID
                )
                AND t.WhseArrival >= @SDate
                AND t.WhseArrival <= @EDate
                
                UPDATE t
                SET
                t.SupplierName = s.SupplierName,
                t.BrandID = s.BrandID,
                t.StockQty = s.StockQty,
                t.TotalInspYds = s.TotalInspYds,
                t.TotalPoCnt = s.TotalPoCnt,
                t.TotalDyelot = s.TotalDyelot,
                t.TotalDyelotAccepted = s.TotalDyelotAccepted,
                t.InspReport = s.InspReport,
                t.TestReport = s.TestReport,
                t.ContinuityCard = s.ContinuityCard,
                t.BulkDyelot = s.BulkDyelot,
                t.TotalPoint = s.TotalPoint,
                t.TotalRoll = s.TotalRoll,
                t.GradeARoll = s.GradeARoll,
                t.GradeBRoll = s.GradeBRoll,
                t.GradeCRoll = s.GradeCRoll,
                t.Inspected = s.Inspected,
                t.Yds = s.Yds,
                t.FabricPercent = s.FabricPercent,
                t.FabricLevel = s.FabricLevel,
                t.Point = s.Point,
                t.SHRINKAGEyards = s.SHRINKAGEyards,
                t.SHRINKAGEPercent = s.SHRINKAGEPercent,
                t.SHINGKAGELevel = s.SHINGKAGELevel,
                t.MIGRATIONyards = s.MIGRATIONyards,
                t.MIGRATIONPercent = s.MIGRATIONPercent,
                t.MIGRATIONLevel = s.MIGRATIONLevel,
                t.SHADINGyards = s.SHADINGyards,
                t.SHADINGPercent = s.SHADINGPercent,
                t.SHADINGLevel = s.SHADINGLevel,
                t.ActualYds = s.ActualYds,
                t.LACKINGYARDAGEPercent = s.LACKINGYARDAGEPercent,
                t.LACKINGYARDAGELevel = s.LACKINGYARDAGELevel,
                t.SHORTWIDTH = s.SHORTWIDTH,
                t.SHORTWidthPercent = s.SHORTWidthPercent,
                t.SHORTWIDTHLevel = s.SHORTWIDTHLevel,
                t.TotalDefectRate = s.TotalDefectRate,
                t.TotalLevel = s.TotalLevel,
                t.Clima = s.Clima,
                t.POID = s.POID				
                ,t.[BIFactoryID]			= s.[BIFactoryID]
				,t.[BIInsertDate]			= s.[BIInsertDate]
                FROM POWERBIReportData.dbo.P_QA_R06 t
                JOIN #Final s 
                    ON t.SuppID = s.SuppID 
                    AND t.Refno = s.Refno 
                    AND t.WhseArrival = s.WhseArrival 
                    AND t.FactoryID = s.FactoryID 
                    AND t.POID = s.POID


                INSERT INTO POWERBIReportData.dbo.P_QA_R06 
                (
                    SuppID, Refno, SupplierName, BrandID, StockQty,
                    TotalInspYds, TotalPoCnt, TotalDyelot, TotalDyelotAccepted, InspReport,
                    TestReport, ContinuityCard, BulkDyelot, TotalPoint, TotalRoll,
                    GradeARoll, GradeBRoll, GradeCRoll, Inspected, Yds,
                    FabricPercent, FabricLevel, Point, SHRINKAGEyards, SHRINKAGEPercent,
                    SHINGKAGELevel, MIGRATIONyards, MIGRATIONPercent, MIGRATIONLevel, SHADINGyards,
                    SHADINGPercent, SHADINGLevel, ActualYds, LACKINGYARDAGEPercent, LACKINGYARDAGELevel,
                    SHORTWIDTH, SHORTWidthPercent, SHORTWIDTHLevel, TotalDefectRate, TotalLevel,
                    WhseArrival, FactoryID, Clima, POID,[BIFactoryID],[BIInsertDate]
                )
                SELECT 
                s.SuppID, s.Refno, s.SupplierName, s.BrandID, s.StockQty,
                s.TotalInspYds, s.TotalPoCnt, s.TotalDyelot, s.TotalDyelotAccepted, s.InspReport,
                s.TestReport, s.ContinuityCard, s.BulkDyelot, s.TotalPoint, s.TotalRoll,
                s.GradeARoll, s.GradeBRoll, s.GradeCRoll, s.Inspected, s.Yds,
                s.FabricPercent, s.FabricLevel, s.Point, s.SHRINKAGEyards, s.SHRINKAGEPercent,
                s.SHINGKAGELevel, s.MIGRATIONyards, s.MIGRATIONPercent, s.MIGRATIONLevel, s.SHADINGyards,
                s.SHADINGPercent, s.SHADINGLevel, s.ActualYds, s.LACKINGYARDAGEPercent, s.LACKINGYARDAGELevel,
                s.SHORTWIDTH, s.SHORTWidthPercent, s.SHORTWIDTHLevel, s.TotalDefectRate, s.TotalLevel,
                s.WhseArrival, s.FactoryID, s.Clima, s.POID,[BIFactoryID],[BIInsertDate]
                FROM #Final s
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM POWERBIReportData.dbo.P_QA_R06 t
                    WHERE t.SuppID = s.SuppID 
                        AND t.Refno = s.Refno 
                        AND t.WhseArrival = s.WhseArrival 
                        AND t.FactoryID = s.FactoryID 
                        AND t.POID = s.POID
                )";
                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#Final");
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
