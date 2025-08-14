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
        public Base_ViewModel P_FabricInspDailyReport_Detail(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Now.AddDays(-7);
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {

                QA_R08_ViewModel model = new QA_R08_ViewModel()
                {
                    InspectionDateFrom = null,
                    InspectionDateTo = null,
                    EditDateFrom = item.SDate,
                    EditDateTo = item.EDate,
                    Inspectors = string.Empty,
                    POIDFrom = string.Empty,
                    POIDTo = string.Empty,
                    RefNoFrom = string.Empty,
                    RefNoTo = string.Empty,
                    BrandID = string.Empty,
                    IsSummary = false,
                };

                Base_ViewModel resultReport = new QA_R08().Get_QA_R08(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.DtArr[0];

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", item.SDate),
                new SqlParameter("@EndDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            using (sqlConn)
            {
                string sql = new Base().SqlBITableHistory("P_FabricInspDailyReport_Detail", "P_FabricInspDailyReport_Detail_History", "#tmpP_FabricInspDailyReport_Detail", "((p.AddDate >= @StartDate and p.AddDate <= @EndDate) or (p.InspectionFinishTime >= @StartDate and p.InspectionFinishTime <= @EndDate))", needJoin: false) + Environment.NewLine;
                sql += $@" 
                delete p
				from P_FabricInspDailyReport_Detail p
				where	((p.AddDate >= @StartDate and p.AddDate <= @EndDate) or (p.InspectionFinishTime >= @StartDate and p.InspectionFinishTime <= @EndDate))
				and not exists(select 1 from #tmpP_FabricInspDailyReport_Detail t 
											where	p.POID = t.POID and
													p.ReceivingID = t.ReceivingID and
													p.SEQ = t.SEQ and
													p.Roll = t.Roll and
													p.Dyelot = t.Dyelot and
													p.InspSeq = t.InspSeq)

				update p set p.InspectionStatus                     = ISNULL(t.InspectionStatus, '')
							,p.InspDate								= t.InspDate	
							,p.InspectorName						= ISNULL(t.InspectorName, '')
							,p.BrandID								= ISNULL(t.BrandID, '')
							,p.FactoryID							= ISNULL(t.Factory, '')
							,p.Style								= ISNULL(t.StyleID, '')
							,p.StockType							= ISNULL(t.StockType, '')
							,p.WKNo									= ISNULL(t.Wkno, '')
							,p.SuppID								= ISNULL(t.SuppID, '')
							,p.SuppName								= ISNULL(t.SuppName, '')
							,p.ATA									= t.ATA
							,p.RefNo								= ISNULL(t.RefNo, '')
							,p.Color								= ISNULL(t.Color, '')
							,p.ArrivedYDS							= ISNULL(t.ArrivedYDS, 0)						
							,p.ActualYDS							= ISNULL(t.ActualYDS, 0)						
							,p.LthOfDiff							= ISNULL(t.LthOfDiff, 0)						
							,p.TransactionID						= ISNULL(t.TransactionID, '')					
							,p.QCIssueQty							= ISNULL(t.QCIssueQty, 0)
							,p.QCIssueTransactionID					= ISNULL(t.QCIssueTransactionID, '')
							,p.CutWidth								= ISNULL(t.CutWidth, 0)
							,p.ActualWidth							= ISNULL(t.ActualWidth, 0)
							,p.Speed								= ISNULL(t.Speed, 0)
							,p.TotalDefectPoints					= ISNULL(t.TotalDefectPoints, 0)
							,p.PointRatePerRoll						= ISNULL(t.PointRatePerRoll, 0)
							,p.Grade								= ISNULL(t.Grade, '')
							,p.SortOut								= ISNULL(t.SortOut, '')
							,p.InspectionStartTime					= t.InspectionStartTime	
							,p.InspectionFinishTime					= t.InspectionFinishTime
							,p.MachineDownTime						= ISNULL(t.MachineDownTime, 0)
							,p.MachineRunTime						= ISNULL(t.MachineRunTime, 0)
							,p.Remark								= ISNULL(t.Remark, '')
							,p.MCHandle								= ISNULL(t.MCHandle, '')
							,p.WeaveType							= ISNULL(t.WeaveType, '')
							,p.MachineID							= ISNULL(t.MachineID, '')
							,p.AddDate								= t.AddDate		
							,p.[BIFactoryID]						= @BIFactoryID
							,p.[BIInsertDate]						= GETDATE()
							,p.[BIStatus]                           = 'New'
				from P_FabricInspDailyReport_Detail p
				inner join #tmpP_FabricInspDailyReport_Detail t on	p.POID = t.POID and
																	p.ReceivingID = t.ReceivingID and
																	p.SEQ = t.SEQ and
																	p.Roll = t.Roll and
																	p.Dyelot = t.Dyelot and
																	p.InspSeq = t.InspSeq
				where t.InspDate is not null

				insert into P_FabricInspDailyReport_Detail(
						InspectionStatus
						, InspDate
						, Inspector
						, InspectorName
						, BrandID
						, FactoryID
						, Style
						, POID
						, SEQ
						, StockType
						, WKNo
						, SuppID
						, SuppName
						, ATA
						, Roll
						, Dyelot
						, RefNo
						, Color
						, ArrivedYDS
						, ActualYDS
						, LthOfDiff
						, TransactionID
						, QCIssueQty
						, QCIssueTransactionID
						, CutWidth
						, ActualWidth
						, Speed
						, TotalDefectPoints
						, PointRatePerRoll
						, Grade
						, SortOut
						, InspectionStartTime
						, InspectionFinishTime
						, MachineDownTime
						, MachineRunTime
						, Remark
						, MCHandle 
						, WeaveType
						, MachineID
						, ReceivingID
						, InspSeq
						, AddDate
						, BIFactoryID
						, BIInsertDate
						, BIStatus
				)
				select	 ISNULL(t.InspectionStatus, '')
						,t.InspDate
						,ISNULL(t.Inspector, '')
						,ISNULL(t.InspectorName, '')
						,ISNULL(t.BrandID, '')
						,ISNULL(t.Factory, '')
						,ISNULL(t.StyleID, '')
						,t.POID
						,t.SEQ
						,ISNULL(t.StockType, '')
						,ISNULL(t.WKNo, '')
						,ISNULL(t.SuppID, '')
						,ISNULL(t.SuppName, '')
						,t.ATA
						,t.Roll
						,t.Dyelot
						,ISNULL(t.RefNo, '')
						,ISNULL(t.Color, '')
						,ISNULL(t.ArrivedYDS, 0)
						,ISNULL(t.ActualYDS, 0)
						,ISNULL(t.LthOfDiff, 0)
						,ISNULL(t.TransactionID, '')
						,ISNULL(t.QCIssueQty, 0)
						,ISNULL(t.QCIssueTransactionID, '')
						,ISNULL(t.CutWidth, 0)
						,ISNULL(t.ActualWidth, 0)
						,ISNULL(t.Speed, 0)
						,ISNULL(t.TotalDefectPoints, 0)
						,ISNULL(t.PointRatePerRoll, 0)	
						,ISNULL(t.Grade, '')
						,ISNULL(t.SortOut, '')
						,t.InspectionStartTime
						,t.InspectionFinishTime
						,ISNULL(t.MachineDownTime, 0)
						,ISNULL(t.MachineRunTime, 0)
						,ISNULL(t.Remark, '')
						,ISNULL(t.MCHandle, '')
						,ISNULL(t.WeaveType, '')
						,ISNULL(t.MachineID, '')
						,t.ReceivingID
						,t.InspSeq
						,t.AddDate
						,@BIFactoryID
						,GETDATE()
						,'New'
				from #tmpP_FabricInspDailyReport_Detail t
				where not exists(	select 1 
									from P_FabricInspDailyReport_Detail p
									where	p.POID = t.POID and
											p.ReceivingID = t.ReceivingID and
											p.SEQ = t.SEQ and
											p.Roll = t.Roll and
											p.Dyelot = t.Dyelot and
											p.InspSeq = t.InspSeq )
				and t.InspDate is not null";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpP_FabricInspDailyReport_Detail");
            }

            return finalResult;
        }
    }
}
