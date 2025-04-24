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
    public class P_Import_SubProInsReport
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SubProInsReport(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R51 biModel = new QA_R51();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/01"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                QA_R51_ViewModel qa_R51_ViewModel = new QA_R51_ViewModel()
                {
                    StartInspectionDate = sDate,
                    EndInspectionDate = eDate,
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
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value);
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

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.DefaultTimeout = 1800;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
            Insert into P_SubProInsReport_History
            Select Ukey, [BIFactoryID], [BIInsertDate]
            From P_SubProInsReport
            WHERE InspectionDate between @StartDate and @EndDate

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
            ,OpreatorID
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
                    ,isnull(OpreatorID, '')
            from #tmp
            update b
		    set b.TransferDate = getdate()
			    , b.IS_Trans = 1
	        from BITableInfo b
	        where b.Id = 'P_SubProInsReport'";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", sDate),
                    new SqlParameter("@EndDate", eDate),
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
