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
    public class P_Import_Changeover
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_Changeover(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddMonths(-7);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now.AddMonths(7);
            }

            try
            {
                Base_ViewModel resultReport = this.GetChangeover_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate, (DateTime)eDate);
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
                string sql = new Base().SqlBITableHistory("P_Changeover", "P_Changeover_History", "#tmpFinal", "p.TransferDate between @SDate and @EDate", needJoin: false) + Environment.NewLine;
                sql += $@"
				delete t
				from P_Changeover t
				where not exists(
					select 1 from #tmpFinal s
					where t.FactoryID = s.FactoryID
					and t.TransferDate = s.TransferDate
				)
				and t.TransferDate between @SDate and @EDate

				update t 
				set      t.TransferDate						   = s.TransferDate					
						,t.[FactoryID]						   = s.[FactoryID]					
						,t.[ChgOverInTransferDate]			   = s.[ChgOverInTransferDate]					
						,t.[ChgOverIn1Day]					   = s.[ChgOverIn1Day]					
						,t.[ChgOverIn7Days]					   = s.[ChgOverIn7Days]					
						,t.[COTInPast1Day]					   = s.[COTInPast1Day]							
						,t.[COTInPast7Days]					   = s.[COTInPast7Days]					
						,t.[COPTInPast1Day]					   = s.[COPTInPast1Day]			
						,t.[COPTInPast7Days]				   = s.[COPTInPast7Days]
						,t.[BIFactoryID]  = s.[BIFactoryID]
						,t.[BIInsertDate]  = s.[BIInsertDate]
				from P_Changeover t
				inner join #tmpFinal s on t.FactoryID = s.FactoryID and t.TransferDate = s.TransferDate

				insert into P_Changeover([TransferDate]
					  ,[FactoryID]
					  ,[ChgOverInTransferDate]
					  ,[ChgOverIn1Day]
					  ,[ChgOverIn7Days]
					  ,[COTInPast1Day]
					  ,[COTInPast7Days]
					  ,[COPTInPast1Day]
					  ,[COPTInPast7Days]
					  ,[BIFactoryID] 
					  ,[BIInsertDate]
				)
				SELECT [TransferDate]
					  ,[FactoryID]
					  ,[ChgOverInTransferDate]
					  ,[ChgOverIn1Day]
					  ,[ChgOverIn7Days]
					  ,[COTInPast1Day]
					  ,[COTInPast7Days]
					  ,[COPTInPast1Day]
					  ,[COPTInPast7Days]
					  ,[BIFactoryID] 
					  ,[BIInsertDate]
				 from #tmpFinal t
				 where not exists(
						select 1 from P_Changeover s
						where t.FactoryID = s.FactoryID 
						and t.TransferDate = s.TransferDate
				  )";
                sql += new Base().SqlBITableInfo("P_Changeover", false);
                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpFinal");
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetChangeover_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @SDate date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @EDate date ='{edate.ToString("yyyy/MM/dd")}'

			-- 遞迴取出今天-7天包含今日 共8天日期
			DECLARE  @t TABLE
			(
			StartDate date,
			EndDate date
			);

			INSERT  INTO  @t
					( StartDate, EndDate )
			VALUES  ( @SDate,   @EDate	);

			;WITH CTE (Dates,EndDate) AS
			(
				SELECT StartDate AS Dates,EndDate AS EndDate
				FROM @t
				UNION ALL --注意這邊使用 UNION ALL
				SELECT DATEADD(DAY,1,Dates),EndDate
				FROM CTE 
				WHERE DATEADD(DAY,1,Dates) <= EndDate --判斷是否目前遞迴月份小於結束日期
			),
			F as(
				select distinct FactoryID from P_StyleChangeover with(nolock)
			)
			SELECT Dates,FactoryID
			into #tmpAllDate
			FROM CTE, F
			OPTION (MAXRECURSION 0); -- 如果日期範圍很大，需要設置遞迴的最大次數


			select 
			 [TransferDate] = d.Dates
			,[FactoryID] = FactoryID
			,[ChgOverInTransferDate]  = isnull(
			(
				SELECT COUNT(*) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) = CONVERT(DATE, d.Dates)
				and s.FactoryID = d.FactoryID
			),0)
			,[ChgOverIn1Day]  = isnull(
			(
				SELECT COUNT(*) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) = DATEADD(DAY, 1, CONVERT(DATE, d.Dates))
				and s.FactoryID = d.FactoryID
			),0)
			,[ChgOverIn7Days] = isnull(
			(
				SELECT COUNT(*) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) >= DATEADD(DAY, 1, CONVERT(DATE, d.Dates))
				AND CONVERT(DATE, Inline) <= DATEADD(DAY, 7, CONVERT(DATE, d.Dates))
				and s.FactoryID = d.FactoryID
			),0)
			,[COTInPast1Day] = isnull(
			(
				SELECT CONVERT(numeric(8,2),AVG(isnull([COT(min)],0))) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) = DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
				and s.FactoryID = d.FactoryID
			),0)
			,[COTInPast7Days] = isnull(
			(
				SELECT CONVERT(numeric(8,2),AVG(isnull([COT(min)],0))) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) BETWEEN DATEADD(DAY, -7, CONVERT(DATE, d.Dates)) 
				AND DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
				and s.FactoryID = d.FactoryID
			),0)
			,[COPTInPast1Day] = isnull(
			(
				SELECT CONVERT(numeric(8,2), AVG(isnull([COPT(min)],0))) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) = DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
				and s.FactoryID = d.FactoryID
			),0)
			,[COPTInPast7Days] = isnull(
			(
				SELECT CONVERT(numeric(8,2),AVG(isnull([COPT(min)],0))) 
				FROM P_StyleChangeover s with(nolock)
				WHERE CONVERT(DATE, Inline) BETWEEN DATEADD(DAY, -7, CONVERT(DATE, d.Dates)) 
				AND DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
				and s.FactoryID = d.FactoryID
			),0)
			, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
            , [BIInsertDate] = GetDate()
			from #tmpAllDate d
			where CONVERT(DATE, d.Dates) <= CONVERT(date,GETDATE())
			";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sqlcmd, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;

            return resultReport;
        }
    }
}
