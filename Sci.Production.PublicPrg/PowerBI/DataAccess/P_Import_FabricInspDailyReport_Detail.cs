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
    /// 與PMS/QA/R08_Detail 脫鉤
    /// </summary>
    public class P_Import_FabricInspDailyReport_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_FabricInspDailyReport_Detail(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddMonths(-3);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetFabricInspDailyReport_Detail_Data((DateTime)sDate, (DateTime)eDate);
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
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_FabricInspDailyReport_Detail", false);
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
            lisSqlParameter.Add(new SqlParameter("@StartDate", sdate));
            lisSqlParameter.Add(new SqlParameter("@EndDate", edate));

            using (sqlConn)
            {
                string sql = new Base().SqlBITableHistory("P_FabricInspDailyReport_Detail", "P_FabricInspDailyReport_Detail_History", "#tmpP_FabricInspDailyReport_Detail", "((p.AddDate >= @StartDate and p.AddDate <= @EndDate) or (p.EditDate >= @StartDate and p.EditDate <= @EndDate))", needJoin: false) + Environment.NewLine;
                sql += $@" 
                delete p
				from P_FabricInspDailyReport_Detail p
				where	((p.AddDate >= @StartDate and p.AddDate <= @EndDate) or (p.EditDate >= @StartDate and p.EditDate <= @EndDate)) and
						not exists(select 1 from #tmpP_FabricInspDailyReport_Detail t 
											where	p.InspDate = t.InspDate and
													p.Inspector = t.Inspector and
													p.POID = t.POID and
													p.SEQ = t.SEQ and
													p.ATA = t.ATA and
													p.Roll = t.Roll and
													p.Dyelot = t.Dyelot)

				update p set p.InspectorName						= t.InspectorName					
							,p.BrandID								= t.BrandID							
							,p.Factory								= t.Factory							
							,p.StyleID								= t.StyleID							
							,p.StockType							= t.StockType						
							,p.Wkno									= t.Wkno								
							,p.SuppID								= t.SuppID							
							,p.SuppName								= t.SuppName							
							,p.RefNo								= t.RefNo							
							,p.Color								= t.Color							
							,p.ArrivedYDS							= t.ArrivedYDS						
							,p.ActualYDS							= t.ActualYDS						
							,p.LthOfDiff							= t.LthOfDiff						
							,p.TransactionID						= t.TransactionID					
							,p.QCIssueQty							= t.QCIssueQty						
							,p.QCIssueTransactionID					= t.QCIssueTransactionID				
							,p.CutWidth								= t.CutWidth							
							,p.ActualWidth							= t.ActualWidth						
							,p.Speed								= t.Speed							
							,p.TotalDefectPoints					= t.TotalDefectPoints				
							,p.Grade								= t.Grade							
							,p.ActInspTimeStart						= t.ActInspTimeStart					
							,p.CalculatedInspTimeStartFirstTime		= t.CalculatedInspTimeStartFirstTime	
							,p.ActInspTimeFinish					= t.ActInspTimeFinish				
							,p.InspTimeFinishFirstTime				= t.InspTimeFinishFirstTime			
							,p.QCMachineStopTime					= t.QCMachineStopTime				
							,p.QCMachineRunTime						= t.QCMachineRunTime					
							,p.Remark								= t.Remark							
							,p.MCHandle								= t.MCHandle							
							,p.WeaveType							= t.WeaveType	
							,p.ReceivingID							= t.ReceivingID
							,p.AddDate								= t.AddDate							
							,p.EditDate								= t.EditDate	
							,p.[BIFactoryID]			= t.[BIFactoryID]
							,p.[BIInsertDate]			= t.[BIInsertDate]
				from P_FabricInspDailyReport_Detail p
				inner join #tmpP_FabricInspDailyReport_Detail t on	p.InspDate = t.InspDate and
																p.Inspector = t.Inspector and
																p.POID = t.POID and
																p.SEQ = t.SEQ and
																p.ATA = t.ATA and
																p.Roll = t.Roll and
																p.Dyelot = t.Dyelot

				insert into P_FabricInspDailyReport_Detail(
						InspDate
						,Inspector
						,InspectorName
						,BrandID
						,Factory
						,StyleID
						,POID
						,SEQ
						,StockType
						,Wkno
						,SuppID
						,SuppName
						,ATA
						,Roll
						,Dyelot
						,RefNo
						,Color
						,ArrivedYDS
						,ActualYDS
						,LthOfDiff
						,TransactionID
						,QCIssueQty
						,QCIssueTransactionID
						,CutWidth
						,ActualWidth
						,Speed
						,TotalDefectPoints
						,Grade
						,ActInspTimeStart
						,CalculatedInspTimeStartFirstTime
						,ActInspTimeFinish
						,InspTimeFinishFirstTime
						,QCMachineStopTime
						,QCMachineRunTime
						,Remark
						,MCHandle
						,WeaveType
						,ReceivingID
						,AddDate
						,EditDate
						,[BIFactoryID]	
						,[BIInsertDate]	
				)
				select	 t.InspDate
						,t.Inspector
						,t.InspectorName
						,t.BrandID
						,t.Factory
						,t.StyleID
						,t.POID
						,t.SEQ
						,t.StockType
						,t.Wkno
						,t.SuppID
						,t.SuppName
						,t.ATA
						,t.Roll
						,t.Dyelot
						,t.RefNo
						,t.Color
						,t.ArrivedYDS
						,t.ActualYDS
						,t.LthOfDiff
						,t.TransactionID
						,t.QCIssueQty
						,t.QCIssueTransactionID
						,t.CutWidth
						,t.ActualWidth
						,t.Speed
						,t.TotalDefectPoints
						,t.Grade
						,t.ActInspTimeStart
						,t.CalculatedInspTimeStartFirstTime
						,t.ActInspTimeFinish
						,t.InspTimeFinishFirstTime
						,t.QCMachineStopTime
						,t.QCMachineRunTime
						,t.Remark
						,t.MCHandle
						,t.WeaveType
						,t.ReceivingID
						,t.AddDate
						,t.EditDate
						,t.[BIFactoryID]	
						,t.[BIInsertDate]	
				from #tmpP_FabricInspDailyReport_Detail t
				where not exists(	select 1 
									from P_FabricInspDailyReport_Detail p
									where	p.InspDate = t.InspDate and
											p.Inspector = t.Inspector and
											p.POID = t.POID and
											p.SEQ = t.SEQ and
											p.ATA = t.ATA and
											p.Roll = t.Roll and
											p.Dyelot = t.Dyelot)";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpP_FabricInspDailyReport_Detail");
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetFabricInspDailyReport_Detail_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @Date_S date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @Date_E date ='{edate.ToString("yyyy/MM/dd")}'

			select 
			 InspDate
			,Inspector
			,InspectorName
			,BrandID
			,Factory
			,StyleID
			,POID
			,SEQ
			,StockType
			,Wkno
			,SuppID
			,SuppName
			,ATA
			,Roll
			,Dyelot
			,RefNo
			,Color
			,ArrivedYDS
			,ActualYDS
			,LthOfDiff
			,TransactionID
			,QCIssueQty
			,QCIssueTransactionID
			,CutWidth
			,ActualWidth
			,Speed
			,TotalDefectPoints
			,Grade
			,ActInspTimeStart
			,CalculatedInspTimeStartFirstTime
			,ActInspTimeFinish
			,InspTimeFinishFirstTime
			,QCMachineStopTime
			,QCMachineRunTime
			,Remark
			,MCHandle
			,WeaveType
			,ReceivingID
			,AddDate
			,EditDate
			from Production.dbo.GetQA_R08_Detail(null,null,'','','','','','',@Date_S, @Date_E)
			where	ATA is not null and InspDate is not null
			";

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
    }
}
