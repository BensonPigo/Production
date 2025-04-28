using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_LineMapping
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_LineMapping(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetLineMapping_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                finalResult = this.UpdateBIData(resultReport.Dt, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw resultReport.Result.GetException();
                }

                if (resultReport.Result)
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_LineMapping", true);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_LineMapping_Detail", true);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetLineMapping_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @SDate date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @EDate date ='{edate.ToString("yyyy/MM/dd")}'

			-- P_LineMapping
			select 
			*
			, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
			, [BIInsertDate] = getdate()
			from (
				select 
				[FactoryID] = isnull(l.FactoryID ,'')
				,[StyleUKey] = isnull(l.StyleUKey,0)
				,[ComboType] = isnull(l.ComboType,'')
				,[Version] = isnull(l.Version,'')
				,[Phase] = ISNULL(l.Phase,'')
				,[SewingLine] = l.SewingLineID
				,[isFrom] = 'IE P03'
				,[ID] = isnull(l.ID,'')
				,[Style] = ISNULL(l.StyleID,'')
				,[Season] = ISNULL(l.SeasonID,'')
				,[Brand] = isnull(l.BrandID,'')
				,[Team] = isnull(l.Team,'')
				,[Desc.] = isnull(s.Description,'')
				,[CPU/PC] = isnull(s.CPU,0)
				,[No. of Sewer] = isnull(l.CurrentOperators,0)
				,[LBR By GSD Time(%)] = 
				case when isnull(l.HighestGSD,0) = 0 then 0.00
				when ISNULL(l.CurrentOperators,0) = 0 then 0.00
				else round(convert(float,l.TotalGSD)/convert(float,l.HighestGSD)/convert(float,l.CurrentOperators) * 100,2) 
				end
				,[Total GSD Time] = isnull(l.TotalGSD,0)
				,[Avg. GSD Time] = 
				case when isnull(l.CurrentOperators,0) = 0 then 0
				else round(CONVERT(float,l.TotalGSD) / convert(float,l.CurrentOperators),2)
				end
				,[Highest GSD Time] = isnull(l.HighestGSD,0)
				,[LBR By Cycle Time(%)] = 
				case when isnull(l.HighestCycle,0) = 0 then 0.00
				when ISNULL(l.CurrentOperators,0) = 0 then 0.00
				else round(convert(float,l.TotalCycle)/convert(float,l.HighestCycle)/convert(float,l.CurrentOperators) * 100,2) 
				end
				,[Total Cycle Time] = isnull(l.TotalCycle,0)
				,[Avg. Cycle Time] = 
				case when ISNULL(l.CurrentOperators,0) = 0 then 0
				else round(CONVERT(float,l.TotalCycle) / CONVERT(float,l.CurrentOperators),2)
				end
				,[Highest Cycle Time] = isnull(l.HighestCycle,0)
				,[Total % Time Diff(%)] =
				case when ISNULL(l.TotalCycle,0) = 0 then 0
				else round( convert(float, (l.TotalGSD - l.TotalCycle)) / CONVERT(float,l.TotalCycle) * 100 ,0) 
				end
				,[No. of Hours] = isnull(l.WorkHour,0)
				,[Oprts of Presser] = 0 --P03沒有
				,[Oprts of Packer] = 0 --P03沒有
				,[Ttl Sew Line Oprts] = isnull( l.CurrentOperators,0)
				,[Target / Hr.(100%)] =
				case when isnull( l.TotalCycle,0) = 0 then 0
				else round(3600 * CONVERT(float,  l.CurrentOperators) / convert(float, l.TotalCycle),0)
				end
				,[Daily Demand / Shift] = 
				case when ISNULL(l.TotalCycle,0) = 0 then 0
				else round(3600 * CONVERT(float,  l.CurrentOperators) / convert(float, l.TotalCycle),0) *  l.WorkHour
				end
				,[Takt Time] = 
				case when ISNULL( l.CurrentOperators,0) = 0 then 0
				when ISNULL(l.TotalCycle,0) = 0 then 0
				else round((3600 * convert(float,l.WorkHour)) / (round(3600 * CONVERT(float,  l.CurrentOperators) / convert(float, l.TotalCycle),0) *  l.WorkHour),2)
				end
				,[EOLR] = 
				case when ISNULL(l.HighestCycle,0) = 0 then 0
				else ROUND(3600 / convert(float,l.HighestCycle) ,2)
				end
				,[PPH] = case when ISNULL(l.CurrentOperators,0) = 0 then 0
				when ISNULL(l.HighestCycle,0) = 0 then 0
				else ROUND(3600 / convert(float,l.HighestCycle) * s.CPU / convert(float,l.CurrentOperators),2) end
				,[GSD Status] = l.TimeStudyPhase
				,[GSD Version] = l.TimeStudyVersion
				,[Status] = isnull(l.Status,'')
				,[Add Name] = l.AddName
				,[Add Date] = l.AddDate
				,[Edit Name] = l.EditName
				,[Edit Date] = l.EditDate
				from Production.dbo.LineMapping l with(nolock)
				left join Production.dbo.Style s with(nolock) on s.Ukey = l.StyleUKey
				where 1=1
				and (
					l.AddDate between @SDate and @EDate
					or
					l.EditDate between @SDate and @EDate
				)

				union all

				select [FactoryID] = isnull(l.FactoryID,'')
				,[StyleUKey] = isnull(StyleUKey,0)
				,[ComboType] = isnull(l.ComboType,'')
				,[Version] = isnull(l.Version,'')
				,[Phase] = isnull(l.Phase,'')
				,[SewingLine] = isnull(l.SewingLineID,'')
				,[isFrom] = 'IE P06'
				,[ID] = isnull(l.ID,'')
				,[Style] = isnull(l.StyleID,'')
				,[Season] = isnull(l.SeasonID,'')
				,[Brand] = isnull(l.BrandID,'')
				,[Team] = isnull(l.Team,'')
				,[Desc.] = isnull(s.Description,'')
				,[CPU/PC] = isnull(s.CPU,0)
				,[No. of Sewer] = l.SewerManpower
				,[LBR By GSD Time(%)] =  
				case when isnull(l.HighestGSDTime,0) = 0 then 0.00
				when ISNULL(l.SewerManpower,0) = 0 then 0.00
				else round(convert(float,l.TotalGSDTime)/convert(float,l.HighestGSDTime)/convert(float,l.SewerManpower) * 100,2) 
				end
				,[Total GSD Time] = l.TotalGSDTime
				,[Avg. GSD Time] = 
				case when isnull(l.SewerManpower,0) = 0 then 0
				else round(CONVERT(float,l.TotalGSDTime) / convert(float,l.SewerManpower),2)
				end
				,[Highest GSD Time] = l.HighestGSDTime
				,[LBR By Cycle Time(%)] = 
				case when isnull(l.HighestCycleTime,0) = 0 then 0.00
				when isnull(l.SewerManpower,0) = 0 then 0.00
				else round(convert(float,l.TotalCycleTime)/convert(float,l.HighestCycleTime)/convert(float,l.SewerManpower) * 100,2) 
				end
				,[Total Cycle Time] = l.TotalCycleTime 
				,[Avg. Cycle Time] = 
				case when isnull(l.SewerManpower,0) = 0 then 0
				else round(CONVERT(float,l.TotalCycleTime) / convert(float,l.SewerManpower),2)
				end
				,[Highest Cycle Time] = l.HighestCycleTime
				,[Total % Time Diff(%)] = 
				case when isnull(l.TotalCycleTime,0) = 0 then 0
				else round( convert(float, (l.TotalGSDTime - l.TotalCycleTime)) / CONVERT(float,l.TotalCycleTime) * 100 ,0) 
				end
				,[No. of Hours] = l.WorkHour
				,[Oprts of Presser] = l.PresserManpower
				,[Oprts of Packer] = l.PackerManpower
				,[Ttl Sew Line Oprts] = l.SewerManpower + l.PresserManpower + l.PackerManpower 
				,[Target / Hr.(100%)] =
				case when isnull(l.TotalCycleTime,0) = 0 then 0
				else round(3600 * CONVERT(float, l.SewerManpower) / convert(float, l.TotalCycleTime),0)
				end
				,[Daily Demand / Shift] = 
				case when isnull(l.TotalCycleTime,0) = 0 then 0
				else round(3600 * CONVERT(float,  l.SewerManpower) / convert(float, l.TotalCycleTime),0) * l.WorkHour
				end
				,[Takt Time] = 
				case when isnull(l.TotalCycleTime,0) = 0 then 0
				else round((3600 * l.WorkHour) / (round(3600 * CONVERT(float,  l.SewerManpower) / convert(float, l.TotalCycleTime),0) * l.WorkHour),2)
				end
				,[EOLR] = 
				case when isnull(l.HighestCycleTime,0) = 0 then 0
				else round(3600 / convert(float,l.HighestCycleTime),2)
				end
				,[PPH] = 
				case when isnull(l.HighestCycleTime,0) = 0 then 0
				when ISNULL(l.SewerManpower,0) = 0 then 0
				else round( (3600 / convert(float,l.HighestCycleTime)) * isnull(s.CPU,0) / CONVERT(float,l.SewerManpower),2)
				end
				,[GSD Status] = l.TimeStudyStatus
				,[GSD Version] = l.TimeStudyVersion
				,[Status] = l.Status
				,[Add Name] = l.AddName
				,[Add Date] = l.AddDate
				,[Edit Name] = l.EditName
				,[Edit Date] = l.EditDate
				from Production.dbo.LineMappingBalancing l with(nolock)
				left join Production.dbo.Style s with(nolock) on s.Ukey = l.StyleUKey
				where l.Status='Confirmed'
				and (
					l.AddDate between @SDate and @EDate
					or
					l.EditDate between @SDate and @EDate
				)
			) a";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;

            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            var paramters = new List<SqlParameter>
            {
                new SqlParameter("@SDate", sDate),
                new SqlParameter("@EDate", eDate),
            };

            string strTmpName_Summary = "#tmpMain";
            string strTmpName_Detail = "#tmpDetail";
            try
            {
                DualResult result;
                DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                using (sqlConn)
                {
                    string sql = $@"
					alter table {strTmpName_Summary} alter column [FactoryID] varchar (8)
					alter table {strTmpName_Summary} alter column [StyleUKey] bigint
					alter table {strTmpName_Summary} alter column [ComboType] varchar (1)
					alter table {strTmpName_Summary} alter column [Version] tinyint
					alter table {strTmpName_Summary} alter column [Phase] varchar (7)
					alter table {strTmpName_Summary} alter column [SewingLine] varchar (8)
					alter table {strTmpName_Summary} alter column [IsFrom] varchar (6)
					alter table {strTmpName_Summary} alter column [Team] varchar (8)
					alter table {strTmpName_Summary} alter column [ID] bigint
					alter table {strTmpName_Summary} alter column [Style] varchar (15)
					alter table {strTmpName_Summary} alter column [Season] varchar (10)
					alter table {strTmpName_Summary} alter column [Brand] varchar (8)
					alter table {strTmpName_Summary} alter column [Desc.] varchar (100)
					alter table {strTmpName_Summary} alter column [CPU/PC] decimal
					alter table {strTmpName_Summary} alter column [No. of Sewer] tinyint
					alter table {strTmpName_Summary} alter column [LBR By GSD Time(%)] numeric (7, 2)
					alter table {strTmpName_Summary} alter column [Total GSD Time] numeric (7, 2)
					alter table {strTmpName_Summary} alter column [Avg. GSD Time] numeric (7, 2)
					alter table {strTmpName_Summary} alter column [Highest GSD Time] numeric (12, 2)
					alter table {strTmpName_Summary} alter column [LBR By Cycle Time(%)] numeric (7, 2)
					alter table {strTmpName_Summary} alter column [Total Cycle Time] numeric (7, 2)
					alter table {strTmpName_Summary} alter column [Avg. Cycle Time] numeric (7, 2)
					alter table {strTmpName_Summary} alter column [Highest Cycle Time] numeric (6, 2)
					alter table {strTmpName_Summary} alter column [Total % Time Diff(%)] int
					alter table {strTmpName_Summary} alter column [No. of Hours] numeric (3, 1)
					alter table {strTmpName_Summary} alter column [Oprts of Presser] tinyint
					alter table {strTmpName_Summary} alter column [Oprts of Packer] tinyint
					alter table {strTmpName_Summary} alter column [Ttl Sew Line Oprts] tinyint
					alter table {strTmpName_Summary} alter column [Target / Hr.(100%)] int
					alter table {strTmpName_Summary} alter column [Daily Demand / Shift] numeric (7, 1)
					alter table {strTmpName_Summary} alter column [Takt Time] numeric (6, 2)
					alter table {strTmpName_Summary} alter column [EOLR] numeric (6, 2)
					alter table {strTmpName_Summary} alter column [PPH] numeric (6, 2)
					alter table {strTmpName_Summary} alter column [GSD Status] varchar (15)
					alter table {strTmpName_Summary} alter column [GSD Version] varchar (2)
					alter table {strTmpName_Summary} alter column [Status] varchar (9)
					alter table {strTmpName_Summary} alter column [Add Name] varchar (10)
					alter table {strTmpName_Summary} alter column [Add Date] datetime
					alter table {strTmpName_Summary} alter column [Edit Name] varchar (10)
					alter table {strTmpName_Summary} alter column [Edit Date] datetime
					alter table {strTmpName_Summary} alter column [BIFactoryID] varchar (8)
					alter table {strTmpName_Summary} alter column [BIInsertDate] datetime
					";
                    sql += new Base().SqlBITableHistory("P_LineMapping", "P_LineMapping_History", "#tmpMain", "((p.[Add Date] between @SDate and @EDate) OR (p.[Edit Date] between @SDate and @EDate))", needJoin: false) + Environment.NewLine;
                    sql += $@"
					delete t
					from P_LineMapping t
					where not exists(
						select 1 from #tmpMain s
						where t.FactoryID = s.FactoryID
						and t.StyleUKey = s.StyleUKey
						and t.ComboType = s.ComboType
						and t.Version = s.Version
						and t.Phase = s.Phase
						and t.SewingLine = s.SewingLine
						and t.IsFrom = s.isFrom
						and t.Team = s.Team
					)
					and (
						t.[Add Date] between @SDate and @EDate
						or
						t.[Edit Date] between @SDate and @EDate
					)

					update t 
					set t.Style	= s.Style					
					,t.Season = s.Season					
					,t.Brand = s.Brand					
					,t.Team	= s.Team					
					,t.[Desc.] = s.[Desc.]					
					,t.[CPU/PC]	 = s.[CPU/PC]				
					,t.[No. of Sewer] = s.[No. of Sewer]
					,t.[LBR By GSD Time(%)] = s.[LBR By GSD Time(%)]					
					,t.[Total GSD Time] = s.[Total GSD Time]			
					,t.[Avg. GSD Time] = s.[Avg. GSD Time]
					,t.[Highest GSD Time] = s.[Highest GSD Time]
					,t.[LBR By Cycle Time(%)] = s.[LBR By Cycle Time(%)]
					,t.[Total Cycle Time] = s.[Total Cycle Time]
					,t.[Avg. Cycle Time] = s.[Avg. Cycle Time]
					,t.[Highest Cycle Time] = s.[Highest Cycle Time]
					,t.[Total % Time Diff(%)] = s.[Total % Time Diff(%)]
					,t.[No. of Hours] = s.[No. of Hours]
					,t.[Oprts of Presser] = s.[Oprts of Presser]
					,t.[Oprts of Packer] = s.[Oprts of Packer]
					,t.[Ttl Sew Line Oprts] = s.[Ttl Sew Line Oprts]
					,t.[Target / Hr.(100%)] = s.[Target / Hr.(100%)]
					,t.[Daily Demand / Shift] = s.[Daily Demand / Shift]
					,t.[Takt Time] = s.[Takt Time]
					,t.[EOLR] = s.[EOLR]
					,t.[PPH] = s.[PPH]
					,t.[GSD Status] = s.[GSD Status]
					,t.[GSD Version] = s.[GSD Version]
					,t.[Status] = s.[Status]
					,t.[Add Date] = s.[Add Date]
					,t.[Add Name] = s.[Add Name]
					,t.[Edit Date] = s.[Edit Date]
					,t.[Edit Name] = s.[Edit Name]
					,t.[BIFactoryID] = s.[BIFactoryID]
					,t.[BIInsertDate] = s.[BIInsertDate]
					from P_LineMapping t
					inner join #tmpMain s on t.FactoryID = s.FactoryID
						and t.StyleUKey = s.StyleUKey
						and t.ComboType = s.ComboType
						and t.Version = s.Version
						and t.Phase = s.Phase
						and t.SewingLine = s.SewingLine
						and t.IsFrom = s.isFrom
						and t.Team = s.Team
				
					insert into P_LineMapping(
					[FactoryID]
							,[StyleUKey]
							,[ComboType]
							,[Version]
							,[Phase]
							,[SewingLine]
							,[IsFrom]
							,[ID]
							,[Style]
							,[Season]
							,[Brand]
							,[Team]
							,[Desc.]
							,[CPU/PC]
							,[No. of Sewer]
							,[LBR By GSD Time(%)]
							,[Total GSD Time]
							,[Avg. GSD Time]
							,[Highest GSD Time]
							,[LBR By Cycle Time(%)]
							,[Total Cycle Time]
							,[Avg. Cycle Time]
							,[Highest Cycle Time]
							,[Total % Time Diff(%)]
							,[No. of Hours]
							,[Oprts of Presser]
							,[Oprts of Packer]
							,[Ttl Sew Line Oprts]
							,[Target / Hr.(100%)]
							,[Daily Demand / Shift]
							,[Takt Time]
							,[EOLR]
							,[PPH]
							,[GSD Status]
							,[GSD Version]
							,[Status]
							,[Add Name]
							,[Add Date]
							,[Edit Name]
							,[Edit Date]
							,[BIFactoryID]
							,[BIInsertDate]
					)
					select [FactoryID]
							,[StyleUKey]
							,[ComboType]
							,[Version]
							,[Phase]
							,[SewingLine]
							,[IsFrom]
							,[ID]
							,[Style]
							,[Season]
							,[Brand]
							,[Team]
							,[Desc.]
							,[CPU/PC]
							,[No. of Sewer]
							,[LBR By GSD Time(%)]
							,[Total GSD Time]
							,[Avg. GSD Time]
							,[Highest GSD Time]
							,[LBR By Cycle Time(%)]
							,[Total Cycle Time]
							,[Avg. Cycle Time]
							,[Highest Cycle Time]
							,[Total % Time Diff(%)]
							,[No. of Hours]
							,[Oprts of Presser]
							,[Oprts of Packer]
							,[Ttl Sew Line Oprts]
							,[Target / Hr.(100%)]
							,[Daily Demand / Shift]
							,[Takt Time]
							,[EOLR]
							,[PPH]
							,[GSD Status]
							,[GSD Version]
							,[Status]
							,[Add Name]
							,[Add Date]
							,[Edit Name]
							,[Edit Date]
							,[BIFactoryID]
							,[BIInsertDate]
					from #tmpMain t
					where not exists(
						select 1 from P_LineMapping s
						where t.FactoryID = s.FactoryID
						and t.StyleUKey = s.StyleUKey
						and t.ComboType = s.ComboType
						and t.Version = s.Version
						and t.Phase = s.Phase
						and t.SewingLine = s.SewingLine
						and t.IsFrom = s.isFrom
						and t.Team = s.Team
					)";
                    sql += $@"
					select *
					,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
					,[BIInsertDate] = getdate()
					into #tmpDetail
					from
					(
						select 
						[ID] = ld.ID
						,[IsFrom] = 'IE P03'
						,[No] = isnull(ld.No,'')
						,[Seq] = 0
						,[Location] = ''
						,[ST/MC Type] = ld.MachineTypeID
						,[MC Group] = ld.MasterPlusGroup
						,[OperationID] = isnull(ld.OperationID,'')
						,[Operation] = isnull(o.DescEN,'')
						,[Annotation] = isnull(ld.Annotation,'')
						,[Attachment] = isnull(ld.Attachment,'')
						,[PartID] = ld.SewingMachineAttachmentID
						,[Template] = isnull(ld.Template,'')
						,[GSD Time] = isnull(ld.GSD,0)
						,[Cycle Time] = isnull(ld.Cycle,0)
						,[%] = 0
						,[Div. Sewer] = 0
						,[Ori. Sewer] = 0
						,[Thread Combination] = isnull(ld.ThreadColor,'')
						,[Notice] = isnull(ld.Notice,'')
						,[OperatorID] = isnull(ld.EmployeeID,'')
						,[OperatorName] = isnull(e.Name,'')
						,[Skill] = isnull(e.Skill,'')
						,[Ukey] = ld.Ukey
						,[FactoryID] = l.FactoryID
						from [MainServer].Production.dbo.LineMapping_Detail ld with(nolock)
						inner join [MainServer].Production.dbo.LineMapping l with(nolock) on l.ID = ld.ID
						left join [MainServer].Production.dbo.Operation o with(nolock) on o.ID = ld.OperationID
						left join [MainServer].Production.dbo.Employee e with(nolock) on e.ID = ld.EmployeeID and e.FactoryID = l.FactoryID
						where exists(
							select 1 from #tmpMain s
							where s.ID = ld.ID	
							and s.isFrom = 'IE P03'
						)
						union all
						select 
						[ID] = ld.ID
						,[IsFrom] = 'IE P06'
						,[No] = isnull(ld.No,'')
						,[Seq] = ld.Seq
						,[Location] = ld.Location
						,[ST/MC Type] = ld.MachineTypeID
						,[MC Group] = ld.MasterPlusGroup
						,[OperationID] = isnull(ld.OperationID,'')
						,[Operation] = isnull(o.DescEN,'')
						,[Annotation] = isnull(ld.Annotation,'')
						,[Attachment] = isnull(ld.Attachment,'')
						,[PartID] = ld.SewingMachineAttachmentID
						,[Template] = isnull(ld.Template,'')
						,[GSD Time] = isnull(ld.GSD,0)
						,[Cycle Time] = isnull(ld.Cycle,0)
						,[%] = ld.SewerDiffPercentage
						,[Div. Sewer] = ld.DivSewer
						,[Ori. Sewer] = ld.OriSewer
						,[Thread Combination] = ld.ThreadComboID
						,[Notice] = ld.Notice
						,[OperatorID] = isnull(ld.EmployeeID,'')
						,[OperatorName] = isnull(e.Name,'')
						,[Skill] = isnull(e.Skill,'')
						,[Ukey] = ld.Ukey
						,[FactoryID] = l.FactoryID
						from [MainServer].Production.dbo.LineMappingBalancing_Detail ld with(nolock)
						inner join [MainServer].Production.dbo.LineMappingBalancing l with(nolock) on l.ID = ld.ID
						left join [MainServer].Production.dbo.Operation o with(nolock) on o.ID = ld.OperationID
						left join [MainServer].Production.dbo.Employee e with(nolock) on e.ID = ld.EmployeeID and e.FactoryID = l.FactoryID
						where exists(
							select 1 from #tmpMain s
							where s.ID = ld.ID	
							and s.isFrom = 'IE P06'
						)
					) a
					";
                    sql += $@"
					alter table {strTmpName_Detail} alter column [ID] bigint
					alter table {strTmpName_Detail} alter column [Ukey] bigint
					alter table {strTmpName_Detail} alter column [IsFrom] varchar (6)
					alter table {strTmpName_Detail} alter column [No] varchar (4)
					alter table {strTmpName_Detail} alter column [Seq] smallint
					alter table {strTmpName_Detail} alter column [Location] varchar (20)
					alter table {strTmpName_Detail} alter column [ST/MC Type] varchar (10)
					alter table {strTmpName_Detail} alter column [MC Group] varchar (4)
					alter table {strTmpName_Detail} alter column [OperationID] varchar (20)
					alter table {strTmpName_Detail} alter column [Operation] nvarchar (500)
					alter table {strTmpName_Detail} alter column [Annotation] nvarchar (200)
					alter table {strTmpName_Detail} alter column [Attachment] varchar (100)
					alter table {strTmpName_Detail} alter column [PartID] varchar (200)
					alter table {strTmpName_Detail} alter column [Template] varchar (100)
					alter table {strTmpName_Detail} alter column [GSD Time] numeric (6, 2)
					alter table {strTmpName_Detail} alter column [Cycle Time] numeric (6, 2)
					alter table {strTmpName_Detail} alter column [%] numeric (3, 2)
					alter table {strTmpName_Detail} alter column [Div. Sewer] numeric (5, 4)
					alter table {strTmpName_Detail} alter column [Ori. Sewer] numeric (5, 4)
					alter table {strTmpName_Detail} alter column [Thread Combination] varchar (10)
					alter table {strTmpName_Detail} alter column [Notice] nvarchar (200)
					alter table {strTmpName_Detail} alter column [OperatorID] varchar (10)
					alter table {strTmpName_Detail} alter column [OperatorName] nvarchar (50)
					alter table {strTmpName_Detail} alter column [Skill] nvarchar (200)
					alter table {strTmpName_Detail} alter column [FactoryID] varchar (8)
					alter table {strTmpName_Detail} alter column [BIFactoryID] varchar (8)
					alter table {strTmpName_Detail} alter column [BIInsertDate] datetime
					";
                    sql += new Base().SqlBITableHistory("P_LineMapping_Detail", "P_LineMapping_Detail_History", "#tmpDetail", "exists (select 1 from #tmpMain s where p.ID = s.ID)", needJoin: false) + Environment.NewLine;
                    sql += $@"
					delete t
					from P_LineMapping_Detail t
					where not exists(
						select 1 from #tmpDetail s
						where t.Ukey = s.Ukey
						and t.IsFrom = s.isFrom
					)
					and exists (
						select 1 from #tmpMain s where t.ID = s.ID
					)

					update t 
					set t.ID	= s.ID
					,t.No = s.No
					,t.[Seq] = s.[Seq]
					,t.[Location] = s.[Location]
					,t.[ST/MC Type] = s.[ST/MC Type]
					,t.[MC Group] = s.[MC Group]
					,t.[OperationID] = s.[OperationID]
					,t.[Operation] = s.[Operation]
					,t.[Annotation] = s.[Annotation]
					,t.[Attachment] = s.[Attachment]
					,t.[PartID] = s.[PartID]
					,t.[Template] = s.[Template]
					,t.[GSD Time] = s.[GSD Time]
					,t.[Cycle Time] = s.[Cycle Time]
					,t.[%] = s.[%]
					,t.[Div. Sewer] = s.[Div. Sewer]
					,t.[Ori. Sewer] = s.[Ori. Sewer]
					,t.[Thread Combination] = s.[Thread Combination]
					,t.[Notice] = s.[Notice]
					,t.[OperatorID] = s.[OperatorID]
					,t.[OperatorName] = s.[OperatorName]
					,t.[Skill] = s.[Skill]
					from P_LineMapping_Detail t
					inner join #tmpDetail s on t.Ukey = s.Ukey
					and t.IsFrom = s.isFrom

					insert into P_LineMapping_Detail(
						[ID]
						,[Ukey]
						,[IsFrom]
						,[No]
						,[Seq]
						,[Location]
						,[ST/MC Type]
						,[MC Group]
						,[OperationID]
						,[Operation]
						,[Annotation]
						,[Attachment]
						,[PartID]
						,[Template]
						,[GSD Time]
						,[Cycle Time]
						,[%]
						,[Div. Sewer]
						,[Ori. Sewer]
						,[Thread Combination]
						,[Notice]
						,[OperatorID]
						,[OperatorName]
						,[Skill]
					)
					select 
						[ID]
						,[Ukey]
						,[IsFrom]
						,[No]
						,[Seq]
						,[Location]
						,[ST/MC Type]
						,[MC Group]
						,[OperationID]
						,[Operation]
						,[Annotation]
						,[Attachment]
						,[PartID]
						,[Template]
						,[GSD Time]
						,[Cycle Time]
						,[%]
						,[Div. Sewer]
						,[Ori. Sewer]
						,[Thread Combination]
						,[Notice]
						,[OperatorID]
						,[OperatorName]
						,[Skill]
					from #tmpDetail t
					where not exists(
						select 1 from P_LineMapping_Detail s
						where t.Ukey = s.Ukey
						and t.IsFrom = s.isFrom
					)
					drop table #tmpMain;
					drop table #tmpDetail;
					";
                    result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable1, temptablename: "#tmpMain", conn: sqlConn, paramters: paramters);

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
