using Ict;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.CallPmsAPI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MISCPurchaseOrderList
    {
        /// <inheritdoc/>
        public Base_ViewModel P_MISCPurchaseOrderList(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddDays(-14);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.LoadData(sDate, eDate);
                if (resultReport.Result)
                {
                    DataTable detailTable = resultReport.Dt;

                    // insert into PowerBI
                    finalResult = this.UpdateBIData(detailTable);
                    if (!finalResult.Result)
                    {
                        throw finalResult.Result.GetException();
                    }

                    finalResult.Result = new Ict.DualResult(true);
                }
                else
                {
                    finalResult.Result = new Ict.DualResult(false, null, resultReport.Result.ToMessages());
                }
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel LoadData(DateTime? sDate, DateTime? eDate)
        {
            Miscellaneous_R02_ViewModel miscellaneous_R02_ViewModel = new Miscellaneous_R02_ViewModel()
            {
                StartCreateDate = null,
                EndCreateDate = null,
                StartDeliveryDate = null,
                EndDeliveryDate = null,
                Type = string.Empty,
                MDivisionID = string.Empty,
                FactoryID = string.Empty,
                SubSupplier = string.Empty,
                Supplier = string.Empty,
                IsOutstanding = false,
                PurchaseFrom = string.Empty,
                OutstandingType = string.Empty,
                Status = "'New','Approved','Closed','Junked','Locked'",
                OrderBy = "Create Date",
                IsBI = true,
                SDate = sDate,
                EDate = eDate,
            };

            string setRgCode = MyUtility.GetValue.Lookup("select RgCode from system witch(nolock)  ", "Production");
            ResultInfo resultInfo = PackingA2BWebAPI.GetWebAPI<Miscellaneous_R02_Report>(setRgCode, "api/PowerBI/Miscellaneous/R02/GetReportData", 300, miscellaneous_R02_ViewModel);
            Base_ViewModel resultReport = new Base_ViewModel()
            {
                Result = new DualResult(resultInfo.Result.isSuccess, resultInfo.ErrCode),
                Dt = resultInfo.ResultDT.Empty() ? new DataTable() : CallWebAPI.ToTable<Miscellaneous_R02_Report>(resultInfo.ResultDT),
            };
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;

            string where = @"  NOT EXISTS (SELECT 1 FROM Machine.dbo.MiscPO M WHERE M.ID = P.PONo)";

            string tmp = new Base().SqlBITableHistory("P_MISCPurchaseOrderList", "P_MISCPurchaseOrderList_History", "#tmp", where, false, false);

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = $@" 
                Update P Set 
                 P.[PurchaseFrom]           = ISNULL(T.[PurchaseFrom],'')
                ,P.[MDivisionID]            = ISNULL(T.[MDivisionID],'')
                ,P.[FactoryID]              = ISNULL(T.[FactoryID],'')
                ,P.[PONo]                   = ISNULL(T.[PONo],'')
                ,P.[PRConfirmedDate]        = T.[PRConfirmedDate]
                ,P.[CreateDate]             = T.[CreateDate]
                ,P.[DeliveryDate]           = T.[DeliveryDate]
                ,P.[Type]                   = ISNULL(T.[Type],'')
                ,P.[Supplier]               = ISNULL(T.[Supplier],'')
                ,P.[Status]                 = ISNULL(T.[Status],'')
                ,P.[ReqNo]                  = ISNULL(T.[ReqNo],'')
                ,P.[PRDate]                 = T.[PRDate]
                ,P.[Code]                   = ISNULL(T.[Code],'')
                ,P.[Description]            = ISNULL(T.[Description],'')
                ,P.[POQty]                  = ISNULL(T.[POQty],0)
                ,P.[UnitID]                 = ISNULL(T.[UnitID],'')
                ,P.[CurrencyID]             = ISNULL(T.[CurrencyID],'')
                ,P.[UnitPrice]              = ISNULL(T.[UnitPrice],0)
                ,P.[UnitPrice_USD]          = ISNULL(T.[UnitPrice_USD],0)
                ,P.[POAmount]               = ISNULL(T.[POAmount],0)
                ,P.[POAmount_USD]           = ISNULL(T.[POAmount_USD],0)
                ,P.[AccInQty]               = ISNULL(T.[AccInQty],0)
                ,P.[TPEPO]                  = ISNULL(T.[TPEPO],'')
                ,P.[TPEQty]                 = ISNULL(T.[TPEQty],0)
                ,P.[TPECurrencyID]          = ISNULL(T.[TPECurrencyID],'')
                ,P.[TPEPrice]               = ISNULL(T.[TPEPrice],0)
                ,P.[TPEAmount]              = ISNULL(T.[TPEAmount],0)
                ,P.[ApQty]                  = ISNULL(T.[ApQty],0)
                ,P.[APAmount]               = ISNULL(T.[APAmount],0)
                ,P.[RentalDay]              = ISNULL(T.[RentalDay],0)
                ,P.[IncomingDate]           = T.[IncomingDate]
                ,P.[APApprovedDate]         = T.[APApprovedDate]
                ,P.[Invoice]                = ISNULL(T.[Invoice],'')
                ,P.[RequestReason]          = ISNULL(T.[RequestReason],'')
                ,P.[ProjectItem]            = ISNULL(T.[ProjectItem],'')
                ,P.[Project]                = ISNULL(T.[Project],'')
                ,P.[DepartmentID]           = ISNULL(T.[DepartmentID],'')
                ,P.[AccountID]              = ISNULL(T.[AccountID],'')
                ,P.[AccountName]            = ISNULL(T.[AccountName],'')
                ,P.[AccountCategory]        = ISNULL(T.[AccountCategory],'')
                ,P.[Budget]                 = ISNULL(T.[Budget],'')
                ,P.[InternalRemarks]        = ISNULL(T.[InternalRemarks],'')
                ,P.[APID]                   = ISNULL(T.[APID],'')
                ,P.[BIFactoryID]            = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
                ,P.[BIInsertDate]           = GetDate()
                From P_MISCPurchaseOrderList P
                inner join #tmp T on T.PONo = P.PONo AND T.Code = P.Code AND T.ReqNo = P.ReqNo
                   
                INSERT INTO P_MISCPurchaseOrderList
                (
                   [PurchaseFrom]
                  ,[MDivisionID]
                  ,[FactoryID]
                  ,[PONo]
                  ,[PRConfirmedDate]
                  ,[CreateDate]
                  ,[DeliveryDate]
                  ,[Type]
                  ,[Supplier]
                  ,[Status]
                  ,[ReqNo]
                  ,[PRDate]
                  ,[Code]
                  ,[Description]
                  ,[POQty]
                  ,[UnitID]
                  ,[CurrencyID]
                  ,[UnitPrice]
                  ,[UnitPrice_USD]
                  ,[POAmount]
                  ,[POAmount_USD]
                  ,[AccInQty]
                  ,[TPEPO]
                  ,[TPEQty]
                  ,[TPECurrencyID]
                  ,[TPEPrice]
                  ,[TPEAmount]
                  ,[ApQty]
                  ,[APAmount]
                  ,[RentalDay]
                  ,[IncomingDate]
                  ,[APApprovedDate]
                  ,[Invoice]
                  ,[RequestReason]
                  ,[ProjectItem]
                  ,[Project]
                  ,[DepartmentID]
                  ,[AccountID]
                  ,[AccountName]
                  ,[AccountCategory]
                  ,[Budget]
                  ,[InternalRemarks]
                  ,[APID]
                  ,[BIFactoryID]
                  ,[BIInsertDate]
                )
                SELECT 
                  ISNULL(T.[PurchaseFrom],'')
                , ISNULL(T.[MDivisionID],'')
                , ISNULL(T.[FactoryID],'')
                , ISNULL(T.[PONo],'')
                , T.[PRConfirmedDate]
                , T.[CreateDate]
                , T.[DeliveryDate]
                , ISNULL(T.[Type],'')
                , ISNULL(T.[Supplier],'')
                , ISNULL(T.[Status],'')
                , ISNULL(T.[ReqNo],'')
                , T.[PRDate]
                , ISNULL(T.[Code],'')
                , ISNULL(T.[Description],'')
                , ISNULL(T.[POQty],0)
                , ISNULL(T.[UnitID],'')
                , ISNULL(T.[CurrencyID],'')
                , ISNULL(T.[UnitPrice],0)
                , ISNULL(T.[UnitPrice_USD],0)
                , ISNULL(T.[POAmount],0)
                , ISNULL(T.[POAmount_USD],0)
                , ISNULL(T.[AccInQty],0)
                , ISNULL(T.[TPEPO],'')
                , ISNULL(T.[TPEQty],0)
                , ISNULL(T.[TPECurrencyID],'')
                , ISNULL(T.[TPEPrice],0)
                , ISNULL(T.[TPEAmount],0)
                , ISNULL(T.[ApQty],0)
                , ISNULL(T.[APAmount],0)
                , ISNULL(T.[RentalDay],0)
                , T.[IncomingDate]
                , T.[APApprovedDate]
                , ISNULL(T.[Invoice],'')
                , ISNULL(T.[RequestReason],'')
                , ISNULL(T.[ProjectItem],'')
                , ISNULL(T.[Project],'')
                , ISNULL(T.[DepartmentID],'')
                , ISNULL(T.[AccountID],'')
                , ISNULL(T.[AccountName],'')
                , ISNULL(T.[AccountCategory],'')
                , ISNULL(T.[Budget],'')
                , ISNULL(T.[InternalRemarks],'')
                , ISNULL(T.[APID],'')
                , [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
                , [BIInsertDate] = GetDate()
                FROM #TMP T 
                WHERE NOT EXISTS(SELECT 1 FROM P_MISCPurchaseOrderList P WHERE T.PONo = P.PONo AND T.Code = P.Code AND T.ReqNo = P.ReqNo)
{tmp}
                DELETE P 
                FROM P_MISCPurchaseOrderList P
                WHERE NOT EXISTS (SELECT 1 FROM Machine.dbo.MiscPO M WHERE M.ID = P.PONo)
    
                IF EXISTS (select 1 from BITableInfo b where b.id = 'P_MISCPurchaseOrderList')
                BEGIN
	                update BITableInfo set TransferDate = getdate()
	                where ID = 'P_MISCPurchaseOrderList'
                END
                ELSE 
                BEGIN
	                insert into BITableInfo(Id, TransferDate)
	                values('P_MISCPurchaseOrderList', getdate())
                END
                ";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn);
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
