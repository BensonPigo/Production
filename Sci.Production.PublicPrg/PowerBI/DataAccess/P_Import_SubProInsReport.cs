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
    public class P_Import_SubProInsReport
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SubProInsReport(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R51 biModel = new QA_R51();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/01"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                QA_R51_ViewModel qa_R51_ViewModel = new QA_R51_ViewModel()
                {
                    StartInspectionDate = item.SDate,
                    EndInspectionDate = item.EDate,
                    FormatType = "DefectType",
                    M = string.Empty,
                    Factory = string.Empty,
                    Shift = string.Empty,
                    SP = string.Empty,
                    Style = string.Empty,
                    SubProcess = string.Empty,
                    IsBI = true,
                };

                Base_ViewModel resultReport = biModel.Get_QA_R51(qa_R51_ViewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[0];

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
            if @IsTrans = 1
            begin
                Insert into P_SubProInsReport_History([Ukey], [BIFactoryID], [BIInsertDate])
                Select Ukey, [BIFactoryID], GETDATE()
                From P_SubProInsReport
                WHERE InspectionDate between @StartDate and @EndDate
            end

            delete P_SubProInsReport where InspectionDate between @StartDate and @EndDate

            insert into P_SubProInsReport(
            FactoryID						   
            ,SubProLocationID				   
            ,InspectionDate						
            ,SewInLine						   
            ,SewinglineID						
            ,Shift							   
            ,RFT								
            ,SubProcessID						
            ,BundleNo							
            ,Artwork						   
            ,OrderID							
            ,Alias							   
            ,BuyerDelivery					   
            ,BundleGroup						
            ,SeasonID						   
            ,StyleID							
            ,ColorID							
            ,SizeCode							
            ,PatternDesc					   
            ,Item							   
            ,Qty								
            ,RejectQty							
            ,Machine							
            ,Serial								
            ,Junk								
            ,Description						
            ,DefectCode							
            ,DefectQty							
            ,Inspector							
            ,Remark								
            ,AddDate						   
            ,RepairedDatetime				   
            ,RepairedTime						
            ,ResolveTime						
            ,SubProResponseTeamID				
            ,CustomColumn1	
            ,MDivisionID
            ,OperatorID
            ,OperatorName
            ,BIFactoryID
            ,BIInsertDate
            ,BIStatus
            )
            select	isnull(FactoryID, '')					
		            ,isnull(SubProLocationID, '')		
		            ,InspectionDate					
		            ,SewInLine					   
		            ,isnull(SewinglineID, '')			
		            ,isnull(Shift, '')					
		            ,isnull(CONVERT(NUMERIC(6,2), RFT), 0)						
		            ,isnull(SubProcessID, '')			
		            ,isnull(BundleNo, '')				
		            ,isnull(Artwork, '')				
		            ,isnull(OrderID, '')				
		            ,isnull(Alias, '')					
		            ,BuyerDelivery					   
		            ,isnull(BundleGroup, 0)				
		            ,isnull(SeasonID, '')				
		            ,isnull(StyleID, '')				
		            ,isnull(ColorID, '')				
		            ,isnull(SizeCode, '')				
		            ,isnull(PatternDesc, '')			  
		            ,isnull(Item, '')					
		            ,isnull(Qty, 0)						
		            ,isnull(RejectQty, 0)				
		            ,isnull(Machine, '')				
		            ,isnull(Serial, '')					
		            ,isnull(Junk, 0)					
		            ,isnull(Description, '')				
		            ,isnull(DefectCode, '')				
		            ,isnull(DefectQty, 0)				
		            ,isnull(Inspector, '')				
		            ,isnull(Remark, '')					
		            ,AddDate2				
		            ,RepairedDatetime			
		            ,isnull(RepairedTime, 0)			
		            ,isnull(ResolveTime, 0)				
		            ,isnull(SubProResponseTeamID, '')	
		            ,isnull(CustomColumn1, '')	
		            ,isnull(MDivisionID, '')
                    ,isnull(OperatorID, '')
                    ,isnull(OperatorName, '')
                    ,@BIFactoryID
                    ,GETDATE()
                    ,'New' 
            from #tmp
";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", item.SDate),
                    new SqlParameter("@EndDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                finalResult = new Base_ViewModel()
                {
                     Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sqlcmd, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }

    }
}
