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
    public class P_Import_InlineDefec
    {
        /// <inheritdoc/>
        public Base_ViewModel P_InlineDefecBIData(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R01 biModel = new PPIC_R01();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd"));
            }

            try
            {
                Inline_R08_ViewModel inline_R08_ViewModel = new Inline_R08_ViewModel()
                {
                    SDate = item.SDate.Value,
                    EDate = item.EDate.Value,
                    OrderID1 = string.Empty,
                    OrderID2 = string.Empty,
                    BrandID = string.Empty,
                    Zone = string.Empty,
                    FactoryID = string.Empty,
                    Team = string.Empty,
                    Line = string.Empty,
                };

                Base_ViewModel resultReport = this.GetInlineDefecData(inline_R08_ViewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.DtArr, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_InlineDefectDetail";
                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetInlineDefecData(Inline_R08_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@SDate", model.SDate),
                new SqlParameter("@EDate", model.EDate),
                new SqlParameter("@OrderID1", model.OrderID1),
                new SqlParameter("@OrderID2", model.OrderID2),
                new SqlParameter("@BrandID", model.BrandID),
                new SqlParameter("@Zone", model.Zone),
                new SqlParameter("@FactoryID", model.FactoryID),
                new SqlParameter("@Team", model.Team),
                new SqlParameter("@Line", model.Line),
            };

            string sql = @"
            exec dbo.Inline_R08  @SDate,
                     @EDate,
                     @OrderID1,
                     @OrderID2,
                     @BrandID,
                     @Zone,
                     @FactoryID,
                     @Team,
                     @Line
            ";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("ManufacturingExecution", sql, listPar, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            DataTable detail = dataTables[0].Clone();
            foreach (DataRow item in dataTables[0].Rows)
            {
                for (int i = 0; i < MyUtility.Convert.GetInt(item["RejectWIP"]); i++)
                {
                    detail.ImportRow(item);
                }
            }

            dataTables[0] = detail;
            resultReport.DtArr = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable[] dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DataTable detailTable = dt[0];
            DataTable summaryTable = dt[1];
            TransactionScope transactionscope = new TransactionScope();
            var paramters = new List<SqlParameter>
            {
                new SqlParameter("@SDate", item.SDate),
                new SqlParameter("@EDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };
            using (transactionscope)
            {
                DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                try
                {
                    DualResult result;
                    using (sqlConn)
                    {
                        string sql = new Base().SqlBITableHistory("P_InlineDefectSummary", "P_InlineDefectSummary_History", "#tmpSummy", "FirstInspectedDate >= @SDate AND FirstInspectedDate < @EDate", needJoin: false, needExists: false) + Environment.NewLine;
                        sql += @"	
                        DELETE FROM P_InlineDefectSummary
                        WHERE FirstInspectedDate >= @SDate AND FirstInspectedDate < @EDate

                        insert into P_InlineDefectSummary
                        (
                            [FirstInspectedDate]
                           ,[FactoryID]
                           ,[BrandID]
                           ,[StyleID]
                           ,[CustPoNo]
                           ,[OrderID]
                           ,[Article]
                           ,[Alias]
                           ,[CDCodeID]
                           ,[CDCodeNew]
                           ,[ProductType]
                           ,[FabricType]
                           ,[Lining]
                           ,[Gender]
                           ,[Construction]
                           ,[ProductionFamilyID]
                           ,[Team]
                           ,[QCName]
                           ,[Shift]
                           ,[Line]
                           ,[SewingCell]
                           ,[InspectedQty]
                           ,[RejectWIP]
                           ,[InlineWFT]
                           ,[InlineRFT]
                           ,[BIFactoryID]
                           ,[BIInsertDate]
                        )
                        select
                            t.[First Inspection Date]
                            ,isnull(t.Factory,'')
                            ,isnull(t.Brand	,'')
                            ,isnull(t.Style	,'')
                            ,isnull(t.[PO#]	,'')
                            ,isnull(t.[SP#]	,'')
                            ,isnull(t.Article,'')
                            ,isnull(t.[Destination],'')
                            ,isnull(t.CdCodeID,'')
                            ,isnull(t.CDCodeNew,'')
                            ,isnull(t.ProductType,'')
                            ,isnull(t.FabricType,'')
                            ,isnull(t.Lining,'')
                            ,isnull(t.Gender,'')
                            ,isnull(t.Construction,'')
                            ,isnull(t.ProductionFamilyID,'')
                            ,isnull(t.Team,'')
                            ,isnull(t.[QC Name],'')
                            ,isnull(t.[Shift],'')
                            ,isnull(t.Line,'')
                            ,isnull(t.[Cell],'')
                            ,isnull(t.[Inspected Qty],0)
                            ,isnull(t.[Reject Qty] ,0)
                            ,isnull(t.[Inline WFT(%)] ,0)
                            ,isnull(t.[Inline RFT(%)] ,0)
                            , @BIFactoryID
			                , GetDate()
                        from #tmpSummy t";
                        result = TransactionClass.ProcessWithDatatableWithTransactionScope(summaryTable, null, sqlcmd: sql, result: out DataTable dataTable, temptablename: "#tmpSummy", conn: sqlConn, paramters: paramters);

                        if (!result.Result)
                        {
                            throw result.GetException();
                        }

                        sql = new Base().SqlBITableHistory("P_InlineDefectDetail", "P_InlineDefectDetail_History", "#tmpDetail", "FirstInspectionDate >= @SDate AND FirstInspectionDate < @EDate", needJoin: false, needExists: false) + Environment.NewLine;
                        sql += @"
DELETE FROM P_InlineDefectDetail
WHERE FirstInspectionDate >= @SDate AND FirstInspectionDate < @EDate

insert into P_InlineDefectDetail
(
   [Zone]
  ,[BrandID]
  ,[BuyerDelivery]
  ,[FactoryID]
  ,[Line]
  ,[Team]
  ,[Shift]
  ,[CustPoNo]
  ,[StyleID]
  ,[OrderId]
  ,[Article]
  ,[FirstInspectionDate]
  ,[FirstInspectedTime]
  ,[InspectedQC]
  ,[ProductType]
  ,[Operation]
  ,[SewerName]
  ,[GarmentDefectTypeID]
  ,[GarmentDefectTypeDesc]
  ,[GarmentDefectCodeID]
  ,[GarmentDefectCodeDesc]
  ,[IsCriticalDefect]
  ,[BIFactoryID]
  ,[BIInsertDate]
)
select
    isnull(t.Zone,'')
    , isnull(t.Brand,'')
    , t.[Buyer Delivery Date]
    , isnull(t.[Factory],'')
    , isnull(t.Line,'')
    , isnull(t.Team ,'')
    , isnull(t.Shift,'')
    , isnull(t.[PO#],'')
    , isnull(t.[Style],'')
    , isnull(t.[SP#],'')
    , isnull(t.Article,'')
    , t.[First Inspection Date]
    , t.[Inspected Time]     
    , isnull(t.[Inspected QC],'')
    , isnull(t.[Product Type],'')
    , isnull(t.Operation,'')
    , isnull(t.[Sewer Name],'')
    , isnull(t.[DefectTypeID],'')
    , isnull(t.[DefectTypeDescritpion],'')
    , isnull(t.[DefectCodeID],'')
    , isnull(t.[DefectCodeDescritpion],'')  
    , isnull(t.IsCriticalDefect,'') 
    , @BIFactoryID
	, GetDate()
From #tmpDetail t
";
                        result = TransactionClass.ProcessWithDatatableWithTransactionScope(detailTable, null, sqlcmd: sql, result: out DataTable dataTable2, temptablename: "#tmpDetail", conn: sqlConn, paramters: paramters);

                        if (!result.Result)
                        {
                            throw result.GetException();
                        }
                    }

                    finalResult.Result = new DualResult(true);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    finalResult.Result = new DualResult(false, ex);
                    transactionscope.Dispose();
                }
                finally
                {
                    if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                    {
                        sqlConn.Close();
                        sqlConn.Dispose();
                    }

                    transactionscope.Dispose();
                }
            }

            return finalResult;
        }
    }
}
