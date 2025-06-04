
using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    internal class P_Import_StationHourlyOutput
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_StationHourlyOutput(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddMinutes(-3);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetStationHourlyOutput_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable[] dataTable = resultReport.DtArr;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_StationHourlyOutput", false);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable[] dt, DateTime sdate, DateTime edate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            TransactionScope transactionscope = new TransactionScope();
            DataTable dtSummray = dt[0];
            DataTable dtDetail = dt[1];
            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@StartDate", sdate));
            lisSqlParameter.Add(new SqlParameter("@EndDate", edate));
            using (transactionscope)
            {
                try
                {
                    DualResult result;
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    string sql = string.Empty;
                    using (sqlConn)
                    {
                        sql = $@" 
                        Insert into dbo.P_StationHourlyOutput (
						[FactoryID]
						, [Date]
						, [Shift]
						, [Team]
						, [Line]
						, [Station]
						, [Capacity]
						, [Target]
						, [TotalOutput]
						, [ProblemsEncounter]
						, [ActionsTaken]
						, [Problems4MS]
						, [Problems4MSDesc]
						, [Ukey]
						, [StyleID]
						, [OrderID]
						, [BIFactoryID]
						, [BIInsertDate]
						)
						Select
						[FactoryID]
						, [Date]
						, [Shift]
						, [Team]
						, [Line]
						, [Station]
						, [Capacity]
						, [Target]
						, [TotalOutput]
						, [ProblemsEncounter]
						, [ActionsTaken]
						, [Problems4MS]
						, [Problems4MSDesc]
						, [Ukey]
						, [StyleID]
						, [OrderID]
						, [BIFactoryID]
						, [BIInsertDate]
						From #tmpStationHourlyOutput t
						Where Not Exists (
							Select 1 
							From dbo.P_StationHourlyOutput s 
							Where s.FactoryID = t.FactoryID 
							And s.Ukey = t.Ukey
						)

						Update s 
						Set s.[FactoryID] = t.[FactoryID]
						, s.[Date] = t.[Date]
						, s.[Shift] = t.[Shift]
						, s.[Team] = t.[Team]
						, s.[Line] = t.[Line]
						, s.[Station] = t.[Station]
						, s.[Capacity] = t.[Capacity]
						, s.[Target] = t.[Target]
						, s.[TotalOutput] = t.[TotalOutput]
						, s.[ProblemsEncounter] = t.[ProblemsEncounter]
						, s.[ActionsTaken] = t.[ActionsTaken]
						, s.[Problems4MS] = t.[Problems4MS]
						, s.[Problems4MSDesc] = t.[Problems4MSDesc]
						, s.[Ukey] = t.[Ukey]
						, s.[StyleID] = t.[StyleID]
						, s.[OrderID] = t.[OrderID]
						, s.[BIFactoryID]		  = t.[BIFactoryID]
						, s.[BIInsertDate]	  = t.[BIInsertDate]
						From dbo.P_StationHourlyOutput s
						Inner Join #tmpStationHourlyOutput t On t.FactoryID = s.FactoryID and t.Ukey = s.Ukey";
                        sql += new Base().SqlBITableHistory("P_StationHourlyOutput", "P_StationHourlyOutput_History", "#tmpStationHourlyOutput", "p.Date Between @StartDate and @EndDate", needJoin: false) + Environment.NewLine;
                        sql += $@"
						Delete s
						From dbo.P_StationHourlyOutput s
						Where Not Exists (
							Select 1 
							From #tmpStationHourlyOutput t
							Where t.FactoryID = s.FactoryID
							And t.Ukey = s.Ukey
						)
						And s.Date Between @StartDate and @EndDate";

                        sql += $@"
						Insert into P_StationHourlyOutput_Detail_History
						(
							[FactoryID]   
							,[Ukey]		  
							,[BIFactoryID] 
							,[BIInsertDate]
						)
						Select
						 p.[FactoryID]   
						,p.[Ukey]		  
						,p.[BIFactoryID] 
						,GETDATE()
						From P_StationHourlyOutput_Detail p
						Where Exists (
							Select 1
							From #tmpStationHourlyOutput t
							Where t.FactoryID = p.FactoryID
							And t.Ukey = p.StationHourlyOutputUkey
						)";

                        sql += $@"
                        Delete s
						From dbo.P_StationHourlyOutput_Detail s
						Where Exists (
							Select 1
							From #tmpStationHourlyOutput t
							Where t.FactoryID = s.FactoryID
							And t.Ukey = s.StationHourlyOutputUkey
						)";
                        result = TransactionClass.ProcessWithDatatableWithTransactionScope(dtSummray, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpStationHourlyOutput");
                        if (!result.Result)
                        {
                            transactionscope.Dispose();
                            throw result.GetException();
                        }

                        sql = $@"
						Insert into dbo.P_StationHourlyOutput_Detail (
						[Ukey]
						, [FactoryID]
						, [StationHourlyOutputUkey]
						, [Oclock]
						, [Qty]
						, [BIFactoryID]
						, [BIInsertDate]
						)
						Select 
						[Ukey]
						, [FactoryID]
						, [StationHourlyOutputUkey]
						, [Oclock]
						, [Qty]
						, [BIFactoryID]
						, [BIInsertDate]
						From #tmpStationHourlyOutput_Detail t
						Where Not Exists (
							Select 1
							From dbo.P_StationHourlyOutput_Detail s
							Where s.FactoryID = t.FactoryID
							And s.Ukey = t.Ukey 
						)";

                        result = TransactionClass.ProcessWithDatatableWithTransactionScope(dtDetail, null, sql, out DataTable dataTable1, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpStationHourlyOutput_Detail");
                        if (!result.Result)
                        {
                            transactionscope.Dispose();
                            throw result.GetException();
                        }
                    }

                    finalResult.Result = result;
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    finalResult.Result = new DualResult(false, ex);
                    transactionscope.Dispose();
                }
                finally
                {
                    transactionscope.Dispose();
                }
            }

            return finalResult;
        }

        private Base_ViewModel GetStationHourlyOutput_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @StartDate date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @EndDate date ='{edate.ToString("yyyy/MM/dd")}'

			Select 
	        [FactoryID]
	        , [Date]
	        , [Shift]
	        , [Team]
	        , [Line]
	        , [Station]
	        , [Capacity]
	        , [Target]
	        , [TotalOutput]
	        , [ProblemsEncounter]
	        , [ActionsTaken]
	        , [Problems4MS]
	        , [Ukey]
	        , [StyleID] = Isnull(styleid.val, '')
	        , [OrderID] = Isnull(OrderID.val, '')
            , [Problems4MSDesc] = Isnull(ps.Description, '')
			, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
			, [BIInsertDate] = GETDATE()
			into #tmpStationHourlyOutput
	        From ManufacturingExecution.dbo.StationHourlyOutput sho With(Nolock)
            LEFT JOIN ManufacturingExecution.dbo.ProblemsSetting ps WITH(NOLOCK) ON ps.ID = sho.[Problems4MS]
	        Outer Apply
	        (
		        Select val = Stuff(
		        (
			        Select Distinct Concat('/ ' , s.id)
			        From ManufacturingExecution.dbo.Inspection t With(Nolock)
			        INNER join ManufacturingExecution.dbo.SciProduction_Style s With(Nolock) On s.Ukey = t.StyleUkey
			        Where t.FactoryID = sho.FactoryID 
			        And Cast(t.InspectionDate As Date) = Cast(sho.[Date] As Date)
			        And t.[Shift] = sho.[Shift]
			        And t.[Team] = sho.[Team]
			        And t.[Line] = sho.[Line]
			        For XML Path('')
		        ),1,1,'')
	        )StyleID
	        Outer Apply
	        (
		        Select val = Stuff(
		        (
			        Select Distinct Concat('/ ' , t.OrderID)
			        From ManufacturingExecution.dbo.Inspection t With(Nolock) 
			        Where t.FactoryID = sho.FactoryID 
			        And Cast(t.InspectionDate As Date) = Cast(sho.[Date] As Date)
			        And t.[Shift] = sho.[Shift]
			        And t.[Team] = sho.[Team]
			        And t.[Line] = sho.[Line]
			        For XML Path('')
		        ),1,1,'')
	        )OrderID
	        Where sho.Date Between @StartDate and @EndDate

			Select * from #tmpStationHourlyOutput

			Select 
			[Ukey] = shod.Ukey
			, [StationHourlyOutputUkey] = shod.StationHourlyOutputUkey
			, [Oclock] = shod.Oclock
			, [Qty] = shod.Qty
			, [FactoryID] = sho.FactoryID
			, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
			, [BIInsertDate] = GETDATE()
			From [ExtendServer].ManufacturingExecution.dbo.StationHourlyOutput_Detail shod With(Nolock)
			Inner join #tmpStationHourlyOutput sho On sho.Ukey = shod.StationHourlyOutputUkey";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("ManufacturingExecution", sqlcmd, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;

            return resultReport;
        }
    }
}
