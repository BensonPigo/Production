using Ict;
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
    public class P_Import_LineMapping
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_LineMapping(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetLineMapping_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                finalResult = this.UpdateBIData(resultReport.DtArr, item);
                if (!finalResult.Result)
                {
                    throw resultReport.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_LineMapping_Detail";
                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetLineMapping_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@SDate", item.SDate),
                new SqlParameter("@EDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
-- P_LineMapping 匯總（含 P03 / P06）

-- P03
SELECT 
    FactoryID              = ISNULL(l.FactoryID, ''),
    StyleUKey              = ISNULL(l.StyleUKey, 0),
    ComboType              = ISNULL(l.ComboType, ''),
    Version                = ISNULL(l.Version, ''),
    Phase                  = ISNULL(l.Phase, ''),
    SewingLine             = l.SewingLineID,
    isFrom                 = 'IE P03',
    ID                     = ISNULL(l.ID, ''),
    Style                  = ISNULL(l.StyleID, ''),
    Season                 = ISNULL(l.SeasonID, ''),
    Brand                  = ISNULL(l.BrandID, ''),
    Team                   = ISNULL(l.Team, ''),
    [Desc.]                = ISNULL(s.Description, ''),
    [CPU/PC]               = ISNULL(s.CPU, 0),
    [No. of Sewer]         = ISNULL(l.CurrentOperators, 0),
    [LBR By GSD Time(%)]   = 
        CASE 
            WHEN ISNULL(l.HighestGSD, 0) = 0 OR ISNULL(l.CurrentOperators, 0) = 0 THEN 0.00
            ELSE ROUND(CONVERT(FLOAT, l.TotalGSD) / NULLIF(CONVERT(FLOAT, l.HighestGSD) * CONVERT(FLOAT, l.CurrentOperators), 0) * 100, 2)
        END,
    [Total GSD Time]       = ISNULL(l.TotalGSD, 0),
    [Avg. GSD Time]        = 
        CASE 
            WHEN ISNULL(l.CurrentOperators, 0) = 0 THEN 0 
            ELSE ROUND(CONVERT(FLOAT, l.TotalGSD) / NULLIF(CONVERT(FLOAT, l.CurrentOperators), 0), 2)
        END,
    [Highest GSD Time]     = ISNULL(l.HighestGSD, 0),
    [LBR By Cycle Time(%)] = 
        CASE 
            WHEN ISNULL(l.HighestCycle, 0) = 0 OR ISNULL(l.CurrentOperators, 0) = 0 THEN 0.00
            ELSE ROUND(CONVERT(FLOAT, l.TotalCycle) / NULLIF(CONVERT(FLOAT, l.HighestCycle) * CONVERT(FLOAT, l.CurrentOperators), 0) * 100, 2)
        END,
    [Total Cycle Time]     = ISNULL(l.TotalCycle, 0),
    [Avg. Cycle Time]      = 
        CASE 
            WHEN ISNULL(l.CurrentOperators, 0) = 0 THEN 0 
            ELSE ROUND(CONVERT(FLOAT, l.TotalCycle) / NULLIF(CONVERT(FLOAT, l.CurrentOperators), 0), 2)
        END,
    [Highest Cycle Time]   = ISNULL(l.HighestCycle, 0),
    [Total % Time Diff(%)] = 
        CASE 
            WHEN ISNULL(l.TotalCycle, 0) = 0 THEN 0
            ELSE ROUND(CONVERT(FLOAT, l.TotalGSD - l.TotalCycle) / NULLIF(CONVERT(FLOAT, l.TotalCycle), 0) * 100, 0)
        END,
    [No. of Hours]         = ISNULL(l.WorkHour, 0),
    [Oprts of Presser]     = 0,
    [Oprts of Packer]      = 0,
    [Ttl Sew Line Oprts]   = ISNULL(l.CurrentOperators, 0),
    [Target / Hr.(100%)]   = 
        CASE 
            WHEN ISNULL(l.TotalCycle, 0) = 0 THEN 0 
            ELSE ROUND(3600 * CONVERT(FLOAT, l.CurrentOperators) / NULLIF(CONVERT(FLOAT, l.TotalCycle), 0), 0)
        END,
    [Daily Demand / Shift] = 
        CASE 
            WHEN ISNULL(l.TotalCycle, 0) = 0 THEN 0 
            ELSE ROUND(3600 * CONVERT(FLOAT, l.CurrentOperators) / NULLIF(CONVERT(FLOAT, l.TotalCycle), 0), 0) * l.WorkHour
        END,
    [Takt Time] = 
        CASE 
            WHEN ISNULL(l.CurrentOperators, 0) = 0 OR ISNULL(l.TotalCycle, 0) = 0 THEN 0
            ELSE ROUND((3600 * l.WorkHour) / NULLIF((3600 * CONVERT(FLOAT, l.CurrentOperators) / CONVERT(FLOAT, l.TotalCycle)) * l.WorkHour, 0), 2)
        END,
    [EOLR] = 
        CASE 
            WHEN ISNULL(l.HighestCycle, 0) = 0 THEN 0 
            ELSE ROUND(3600 / CONVERT(FLOAT, l.HighestCycle), 2)
        END,
    [PPH] = 
        CASE 
            WHEN ISNULL(l.CurrentOperators, 0) = 0 OR ISNULL(l.HighestCycle, 0) = 0 THEN 0
            ELSE ROUND((3600 / CONVERT(FLOAT, l.HighestCycle)) * s.CPU / NULLIF(CONVERT(FLOAT, l.CurrentOperators), 0), 2)
        END,
    [GSD Status] = l.TimeStudyPhase,
    [GSD Version] = l.TimeStudyVersion,
    [Status] = ISNULL(l.Status, ''),
    [Add Name] = l.AddName,
    [Add Date] = l.AddDate,
    [Edit Name] = l.EditName,
    [Edit Date] = l.EditDate,
    BIFactoryID = @BIFactoryID,
    BIInsertDate = GETDATE()
INTO #tmpMain
FROM Production.dbo.LineMapping l WITH (NOLOCK)
LEFT JOIN Production.dbo.Style s WITH (NOLOCK) ON s.Ukey = l.StyleUKey
WHERE (l.AddDate BETWEEN @SDate AND @EDate OR l.EditDate BETWEEN @SDate AND @EDate)

UNION ALL

-- P06
SELECT 
    FactoryID              = ISNULL(l.FactoryID, ''),
    StyleUKey              = ISNULL(l.StyleUKey, 0),
    ComboType              = ISNULL(l.ComboType, ''),
    Version                = ISNULL(l.Version, ''),
    Phase                  = ISNULL(l.Phase, ''),
    SewingLine             = ISNULL(l.SewingLineID, ''),
    isFrom                 = 'IE P06',
    ID                     = ISNULL(l.ID, ''),
    Style                  = ISNULL(l.StyleID, ''),
    Season                 = ISNULL(l.SeasonID, ''),
    Brand                  = ISNULL(l.BrandID, ''),
    Team                   = ISNULL(l.Team, ''),
    [Desc.]                = ISNULL(s.Description, ''),
    [CPU/PC]               = ISNULL(s.CPU, 0),
    [No. of Sewer]         = l.SewerManpower,
    [LBR By GSD Time(%)]   = 
        CASE 
            WHEN ISNULL(l.HighestGSDTime, 0) = 0 OR ISNULL(l.SewerManpower, 0) = 0 THEN 0.00
            ELSE ROUND(CONVERT(FLOAT, l.TotalGSDTime) / NULLIF(CONVERT(FLOAT, l.HighestGSDTime) * CONVERT(FLOAT, l.SewerManpower), 0) * 100, 2)
        END,
    [Total GSD Time]       = l.TotalGSDTime,
    [Avg. GSD Time]        = 
        CASE 
            WHEN ISNULL(l.SewerManpower, 0) = 0 THEN 0 
            ELSE ROUND(CONVERT(FLOAT, l.TotalGSDTime) / NULLIF(CONVERT(FLOAT, l.SewerManpower), 0), 2)
        END,
    [Highest GSD Time]     = l.HighestGSDTime,
    [LBR By Cycle Time(%)] = 
        CASE 
            WHEN ISNULL(l.HighestCycleTime, 0) = 0 OR ISNULL(l.SewerManpower, 0) = 0 THEN 0.00
            ELSE ROUND(CONVERT(FLOAT, l.TotalCycleTime) / NULLIF(CONVERT(FLOAT, l.HighestCycleTime) * CONVERT(FLOAT, l.SewerManpower), 0) * 100, 2)
        END,
    [Total Cycle Time]     = l.TotalCycleTime,
    [Avg. Cycle Time]      = 
        CASE 
            WHEN ISNULL(l.SewerManpower, 0) = 0 THEN 0
            ELSE ROUND(CONVERT(FLOAT, l.TotalCycleTime) / NULLIF(CONVERT(FLOAT, l.SewerManpower), 0), 2)
        END,
    [Highest Cycle Time]   = l.HighestCycleTime,
    [Total % Time Diff(%)] = 
        CASE 
            WHEN ISNULL(l.TotalCycleTime, 0) = 0 THEN 0
            ELSE ROUND(CONVERT(FLOAT, l.TotalGSDTime - l.TotalCycleTime) / NULLIF(CONVERT(FLOAT, l.TotalCycleTime), 0) * 100, 0)
        END,
    [No. of Hours]         = l.WorkHour,
    [Oprts of Presser]     = l.PresserManpower,
    [Oprts of Packer]      = l.PackerManpower,
    [Ttl Sew Line Oprts]   = l.SewerManpower + l.PresserManpower + l.PackerManpower,
    [Target / Hr.(100%)]   = 
        CASE 
            WHEN ISNULL(l.TotalCycleTime, 0) = 0 THEN 0
            ELSE ROUND(3600 * CONVERT(FLOAT, l.SewerManpower) / NULLIF(CONVERT(FLOAT, l.TotalCycleTime), 0), 0)
        END,
    [Daily Demand / Shift] = 
        CASE 
            WHEN ISNULL(l.TotalCycleTime, 0) = 0 THEN 0
            ELSE ROUND(3600 * CONVERT(FLOAT, l.SewerManpower) / NULLIF(CONVERT(FLOAT, l.TotalCycleTime), 0), 0) * l.WorkHour
        END,
    [Takt Time] = 
        CASE 
            WHEN ISNULL(l.TotalCycleTime, 0) = 0 THEN 0
            ELSE ROUND((3600 * l.WorkHour) / NULLIF((3600 * CONVERT(FLOAT, l.SewerManpower) / CONVERT(FLOAT, l.TotalCycleTime)) * l.WorkHour, 0), 2)
        END,
    [EOLR] = 
        CASE 
            WHEN ISNULL(l.HighestCycleTime, 0) = 0 THEN 0
            ELSE ROUND(3600 / CONVERT(FLOAT, l.HighestCycleTime), 2)
        END,
    [PPH] = 
        CASE 
            WHEN ISNULL(l.SewerManpower, 0) = 0 OR ISNULL(l.HighestCycleTime, 0) = 0 THEN 0
            ELSE ROUND((3600 / CONVERT(FLOAT, l.HighestCycleTime)) * ISNULL(s.CPU, 0) / NULLIF(CONVERT(FLOAT, l.SewerManpower), 0), 2)
        END,
    [GSD Status] = l.TimeStudyStatus,
    [GSD Version] = l.TimeStudyVersion,
    [Status] = l.Status,
    [Add Name] = l.AddName,
    [Add Date] = l.AddDate,
    [Edit Name] = l.EditName,
    [Edit Date] = l.EditDate,
    BIFactoryID = @BIFactoryID,
    BIInsertDate = GETDATE()
FROM Production.dbo.LineMappingBalancing l WITH (NOLOCK)
LEFT JOIN Production.dbo.Style s WITH (NOLOCK) ON s.Ukey = l.StyleUKey
WHERE l.Status = 'Confirmed'
  AND (l.AddDate BETWEEN @SDate AND @EDate OR l.EditDate BETWEEN @SDate AND @EDate)


-- 建立 #tmpDetail 暫存表
SELECT 
    [ID]                  = ld.ID,
    [IsFrom]              = 'IE P03',
    [No]                  = ISNULL(ld.No, ''),
    [Seq]                 = 0,
    [Location]            = '',
    [ST/MC Type]          = ld.MachineTypeID,
    [MC Group]            = ld.MasterPlusGroup,
    [OperationID]         = ISNULL(ld.OperationID, ''),
    [Operation]           = ISNULL(o.DescEN, ''),
    [Annotation]          = ISNULL(ld.Annotation, ''),
    [Attachment]          = ISNULL(ld.Attachment, ''),
    [PartID]              = ld.SewingMachineAttachmentID,
    [Template]            = ISNULL(ld.Template, ''),
    [GSD Time]            = ISNULL(ld.GSD, 0),
    [Cycle Time]          = ISNULL(ld.Cycle, 0),
    [%]                   = 0,
    [Div. Sewer]          = 0,
    [Ori. Sewer]          = 0,
    [Thread Combination]  = ISNULL(ld.ThreadColor, ''),
    [Notice]              = ISNULL(ld.Notice, ''),
    [OperatorID]          = ISNULL(ld.EmployeeID, ''),
    [OperatorName]        = ISNULL(e.Name, ''),
    [Skill]               = ISNULL(e.Skill, ''),
    [Ukey]                = ld.Ukey,
    [FactoryID]           = l.FactoryID,
    BIFactoryID           = @BIFactoryID,
    BIInsertDate          = GETDATE()
INTO #tmpDetail
FROM [MainServer].Production.dbo.LineMapping_Detail ld WITH (NOLOCK)
INNER JOIN [MainServer].Production.dbo.LineMapping l WITH (NOLOCK) ON l.ID = ld.ID
LEFT JOIN [MainServer].Production.dbo.Operation o WITH (NOLOCK) ON o.ID = ld.OperationID
LEFT JOIN [MainServer].Production.dbo.Employee e WITH (NOLOCK) 
       ON e.ID = ld.EmployeeID AND e.FactoryID = l.FactoryID
WHERE EXISTS (
    SELECT 1 
    FROM #tmpMain s 
    WHERE s.ID = ld.ID AND s.isFrom = 'IE P03'
)

UNION ALL

-- P06
SELECT 
    [ID]                  = ld.ID,
    [IsFrom]              = 'IE P06',
    [No]                  = ISNULL(ld.No, ''),
    [Seq]                 = ld.Seq,
    [Location]            = ld.Location,
    [ST/MC Type]          = ld.MachineTypeID,
    [MC Group]            = ld.MasterPlusGroup,
    [OperationID]         = ISNULL(ld.OperationID, ''),
    [Operation]           = ISNULL(o.DescEN, ''),
    [Annotation]          = ISNULL(ld.Annotation, ''),
    [Attachment]          = ISNULL(ld.Attachment, ''),
    [PartID]              = ld.SewingMachineAttachmentID,
    [Template]            = ISNULL(ld.Template, ''),
    [GSD Time]            = ISNULL(ld.GSD, 0),
    [Cycle Time]          = ISNULL(ld.Cycle, 0),
    [%]                   = ld.SewerDiffPercentage,
    [Div. Sewer]          = ld.DivSewer,
    [Ori. Sewer]          = ld.OriSewer,
    [Thread Combination]  = ld.ThreadComboID,
    [Notice]              = ld.Notice,
    [OperatorID]          = ISNULL(ld.EmployeeID, ''),
    [OperatorName]        = ISNULL(e.Name, ''),
    [Skill]               = ISNULL(e.Skill, ''),
    [Ukey]                = ld.Ukey,
    [FactoryID]           = l.FactoryID,
    BIFactoryID           = @BIFactoryID,
    BIInsertDate          = GETDATE()
FROM [MainServer].Production.dbo.LineMappingBalancing_Detail ld WITH (NOLOCK)
INNER JOIN [MainServer].Production.dbo.LineMappingBalancing l WITH (NOLOCK) ON l.ID = ld.ID
LEFT JOIN [MainServer].Production.dbo.Operation o WITH (NOLOCK) ON o.ID = ld.OperationID
LEFT JOIN [MainServer].Production.dbo.Employee e WITH (NOLOCK) 
       ON e.ID = ld.EmployeeID AND e.FactoryID = l.FactoryID
WHERE EXISTS (
    SELECT 1 
    FROM #tmpMain s 
    WHERE s.ID = ld.ID AND s.isFrom = 'IE P06'
)


SELECT * FROM #tmpMain
SELECT * FROM #tmpDetail
";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable[] datatables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = datatables;

            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable[] dataTables, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            var paramters = new List<SqlParameter>
            {
                new SqlParameter("@SDate", item.SDate),
                new SqlParameter("@EDate", item.EDate),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            try
            {
                DualResult result;
                DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                using (sqlConn)
                {
                    string sql = new Base().SqlBITableHistory("P_LineMapping", "P_LineMapping_History", "#tmpMain", "((p.[Add Date] between @SDate and @EDate) OR (p.[Edit Date] between @SDate and @EDate))", needJoin: false) + Environment.NewLine;

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
                    result = TransactionClass.ProcessWithDatatableWithTransactionScope(dataTables[0], null, sqlcmd: sql, result: out DataTable tmpMain, temptablename: "#tmpMain", conn: sqlConn, paramters: paramters);

                    sql = new Base().SqlBITableHistory("P_LineMapping_Detail", "P_LineMapping_Detail_History", "#tmpDetail", "exists (select 1 from #tmpDetail s where p.ID = s.ID)", needJoin: false) + Environment.NewLine;
                    sql += $@"
					delete t
					from P_LineMapping_Detail t
					where not exists(
						select 1 from #tmpDetail s
						where t.Ukey = s.Ukey
						and t.IsFrom = s.isFrom
					)
					and exists (
						select 1 from #tmpDetail s where t.ID = s.ID
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
                    ,t.[BIFactoryID] = s.[BIFactoryID]
					,t.[BIInsertDate] = s.[BIInsertDate]
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
                        ,[BIFactoryID]
						,[BIInsertDate]
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
                        ,[BIFactoryID]
						,[BIInsertDate]
					from #tmpDetail t
					where not exists(
						select 1 from P_LineMapping_Detail s
						where t.Ukey = s.Ukey
						and t.IsFrom = s.isFrom
					)
					";
                    result = TransactionClass.ProcessWithDatatableWithTransactionScope(dataTables[1], null, sqlcmd: sql, result: out DataTable tmpDetail, temptablename: "#tmpDetail", conn: sqlConn, paramters: paramters);

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
