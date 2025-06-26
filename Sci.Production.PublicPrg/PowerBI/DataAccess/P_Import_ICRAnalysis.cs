using Ict;
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
    public class P_Import_ICRAnalysis
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_ICRAnalysis(ExecutedList item)
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
                Base_ViewModel resultReport = this.GetICRAnalysis_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable);
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

        private Base_ViewModel GetICRAnalysis_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@StartDate", item.SDate),
                new SqlParameter("@EndDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
			select
			    ICR.ID,
			    f.MDivisionID,
			    ICR.Department,
			    f.KPICode,
			    ICR.OrderID,
			    o.StyleID,
			    o.SeasonID,
			    o.BrandID,
			    [TotalQty] = iif(o.POID <> ICR.OrderID, o.Qty, (select sum(Qty) from [MainServer].Production.dbo.orders with (nolock) where id = ICR.OrderID)),
			    [PO_Handle] = [dbo].[getTPEPass1_ExtNo](PO.POHandle) ,
			    [PO_SMR] = [dbo].[getTPEPass1_ExtNo](o.SMR),
			    [MR] = [dbo].[getTPEPass1_ExtNo](o.MRHandle),
			    [SMR] = [dbo].[getTPEPass1_ExtNo](o.SMR),
			    [IssueSubject] = (select CONCAT(ID,' - ', Name) from [MainServer].Production.dbo.Reason where ID = ICR.IrregularPOCostID And Reason.ReasonTypeID = 'PO_IrregularCost'),
			    ICR.Description,
			    ICR.RMtlAmtUSD,
			    ICR.OtherAmtUSD,
			    ICR.ActFreightUSD,
			    [TotalUSD] = ICR.RMtlAmtUSD + ICR.ActFreightUSD + ICR.OtherAmtUSD,
			    ICR.VoucherID,
			    ICR.VoucherDate,
			    o.POID,
			    ICR.IrregularPOCostID,
			    ICR.Status,
			    [AddDate] = format(ICR.AddDate, 'yyyy/MM/dd'), 
			    ICR.CFMDate,
			    FTY = iif(ICR.Responsible = 'S', ICR.BulkFTY, o.FactoryID)
			into #tmpBaseICR
			from [MainServer].Production.dbo.ICR with (nolock)
			left join [MainServer].Production.dbo.Orders o with (nolock) on ICR.OrderID = o.ID
			left join [MainServer].Production.dbo.PO with (nolock) on o.POID = PO.ID
			left join [MainServer].Production.dbo.Factory f with (nolock) on ICR.Department = f.ID
			where ICR.CFMDate is not null
			and 
			(
				(@StartDate is null or ICR.AddDate >= @StartDate)
				or
				(@EndDate is null or ICR.AddDate <= @EndDate)
				or
				(@StartDate is null or ICR.EditDate >= @StartDate)
				or
				(@EndDate is null or ICR.EditDate <= @EndDate)
			)

			select
			    ICRNo = ICR.ID,
			    Status = ICR.Status,
			    Mdivision = ICR.MDivisionID,
			    ResponsibilityFTY = ICR.Department,
			    FTY = ICR.FTY,
			    SDPKPICode = ICR.KPICode,
			    SPNo = ICR.OrderID,
			    StyleID = ICR.StyleID,
			    SeasonID = ICR.SeasonID,
			    BrandID = ICR.BrandID,
			    TotalQty = ICR.TotalQty,
			    POHandle = ICR.PO_Handle ,
			    POSMR = ICR.PO_SMR,
			    MR = ICR.MR,
			    SMR = ICR.SMR,
			    IssueSubject = ICR.IssueSubject,
			    ResponsibilityAndExplaination = ICR.Description,
			    RMtlAmtUSD = ICR.RMtlAmtUSD,
			    OtherAmtUSD = ICR.OtherAmtUSD,
			    ActFreightAmtUSD = ICR.ActFreightUSD,
			    TotalUSD = ICR.TotalUSD,
			    Createdate = ICR.AddDate,
			    Confirmeddate = ICR.CFMDate,
			    VoucherNo = ICR.VoucherID,
			    VoucherDate = ICR.VoucherDate,
			    [Seq] = ISNULL(CONCAT(ISNULL(icrd.Seq1, ''), '-', ISNULL(icrd.Seq2, '')), ''),
			    [SourceType] = (select DropDownList.Name 
    							    from [MainServer].Production.dbo.Fabric, [MainServer].Production.dbo.DropDownList 
    							    where psd.SCIRefno = Fabric.SCIRefno 
    							    and Fabric.Type = DropDownList.ID 
    							    and DropDownList.type = 'FabricType' ),
			    [WeaveType] = (SELECT WeaveTypeID FROM [MainServer].Production.dbo.Fabric 
    											    WHERE SCIRefno = (SELECT SCIRefno FROM [MainServer].Production.dbo.PO_Supp_Detail WHERE ID = ICR.POID AND Seq1 = icrd.Seq1 AND Seq2 = icrd.Seq2)),
			    IrregularMtlType = icrd.MtltypeID,
			    IrregularQty = icrd.ICRQty,
			    IrregularFOC = icrd.ICRFoc,
			    IrregularPriceUSD = icrd.PriceUSD,
			    [IrregularAmtUSD] = (Select Amount from dbo.GetAmountByUnit(icrd.PriceUSD, icrd.ICRQty, psd.POUnit,2)),
                [BIFactoryID] = @BIFactoryID,
                [BIInsertDate] = GETDATE()
			from #tmpBaseICR ICR
			left join [MainServer].Production.dbo.ICR_Detail icrd with (nolock) on ICR.ID = icrd.ID
			left join [MainServer].Production.dbo.PO_Supp_Detail psd with (nolock) on psd.ID = ICR.POID and psd.SEQ1= icrd.Seq1  and psd.SEQ2= icrd.Seq2
			
			Drop table #tmpBaseICR";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dt;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				update t
	            set
	                t.[ICRNo] = isnull(s.ICRNo,'')
	                ,t.[Status] = isnull(s.Status,'')
	                ,t.[Mdivision] = isnull(s.Mdivision,'')
	                ,t.[ResponsibilityFTY] = isnull(s.ResponsibilityFTY,'')
	                ,t.[FTY] = isnull(s.FTY,'')
	                ,t.[SDPKPICode] = isnull(s.SDPKPICode,'')
	                ,t.[SPNo] = isnull(s.SPNo,'')
	                ,t.[StyleID] = isnull(s.StyleID,'')
	                ,t.[SeasonID] = isnull(s.SeasonID,'')
	                ,t.[BrandID] = isnull(s.BrandID,'')
	                ,t.[TotalQty] = isnull(s.TotalQty,0)
	                ,t.[POHandle] = isnull(s.POHandle,'')
	                ,t.[POSMR] = isnull(s.POSMR,'')
	                ,t.[MR] = isnull(s.MR,'')
	                ,t.[SMR] = isnull(s.SMR,'')
	                ,t.[IssueSubject] = isnull(s.IssueSubject,'')
	                ,t.[ResponsibilityAndExplaination] = isnull(s.ResponsibilityAndExplaination,'')
	                ,t.[RMtlAmtUSD] = isnull(s.RMtlAmtUSD,0)
	                ,t.[OtherAmtUSD] = isnull(s.OtherAmtUSD,0)
	                ,t.[ActFreightAmtUSD] = isnull(s.ActFreightAmtUSD,0)
	                ,t.[TotalUSD] = isnull(s.TotalUSD,0)
	                ,t.[Createdate] = s.Createdate
	                ,t.[Confirmeddate] = s.Confirmeddate
	                ,t.[VoucherNo] = isnull(s.VoucherNo,'')
	                ,t.[VoucherDate] = s.VoucherDate
	                ,t.[Seq] = isnull(s.Seq,'')
	                ,t.[SourceType] = isnull(s.SourceType,'')
	                ,t.[WeaveType] = isnull(s.WeaveType,'')
	                ,t.[IrregularMtlType] = isnull(s.IrregularMtlType,'')
	                ,t.[IrregularQty] = isnull(s.IrregularQty,0)
	                ,t.[IrregularFOC] = isnull(s.IrregularFOC,0)
	                ,t.[IrregularPriceUSD] = isnull(s.IrregularPriceUSD,0)
	                ,t.[IrregularAmtUSD] = isnull(s.IrregularAmtUSD,0)
                    ,t.[BIFactoryID] = s.BIFactoryID
                    ,t.[BIInsertDate] = s.BIInsertDate
	            from P_ICRAnalysis t
	            inner join #tmpFinal s on t.ICRNo = s.ICRNo and t.Seq = s.Seq

	            insert into dbo.P_ICRAnalysis
                (
	                [ICRNo]
                    ,[Status]
                    ,[Mdivision]
                    ,[ResponsibilityFTY]
                    ,[FTY]
                    ,[SDPKPICode]
                    ,[SPNo]
                    ,[StyleID]
                    ,[SeasonID]
                    ,[BrandID]
                    ,[TotalQty]
                    ,[POHandle]
                    ,[POSMR]
                    ,[MR]
                    ,[SMR]
                    ,[IssueSubject]
                    ,[ResponsibilityAndExplaination]
                    ,[RMtlAmtUSD]
                    ,[OtherAmtUSD]
                    ,[ActFreightAmtUSD]
                    ,[TotalUSD]
                    ,[Createdate]
                    ,[Confirmeddate]
                    ,[VoucherNo]
                    ,[VoucherDate]
                    ,[Seq]
                    ,[SourceType]
                    ,[WeaveType]
                    ,[IrregularMtlType]
                    ,[IrregularQty]
                    ,[IrregularFOC]
                    ,[IrregularPriceUSD]
                    ,[IrregularAmtUSD]
                    ,[BIFactoryID]
                    ,[BIInsertDate]
	            )
	            select 
                    [ICRNo] = ISNULL([ICRNo] , '')
                    ,[Status] = ISNULL([Status], '')
                    ,[Mdivision] = ISNULL([Mdivision], '')
                    ,[ResponsibilityFTY] = ISNULL([ResponsibilityFTY], '')
                    ,[FTY] = ISNULL([FTY] , '')
                    ,[SDPKPICode] = ISNULL([SDPKPICode] ,'')
                    ,[SPNo] = ISNULL([SPNo], '')
                    ,[StyleID] = ISNULL([StyleID], '')
                    ,[SeasonID] = ISNULL([SeasonID], '')
                    ,[BrandID] = ISNULL([BrandID],'')
                    ,[TotalQty] = ISNULL([TotalQty] , 0)
                    ,[POHandle] = ISNULL([POHandle],'')
                    ,[POSMR] = ISNULL([POSMR],'')
                    ,[MR] = ISNULL([MR],'')
                    ,[SMR] = ISNULL([SMR],'')
                    ,[IssueSubject] = ISNULL([IssueSubject],'')
                    ,[ResponsibilityAndExplaination] = ISNULL([ResponsibilityAndExplaination],'')
                    ,[RMtlAmtUSD] = ISNULL([RMtlAmtUSD],0)
                    ,[OtherAmtUSD] = ISNULL([OtherAmtUSD],0)
                    ,[ActFreightAmtUSD] = ISNULL([ActFreightAmtUSD],0)
                    ,[TotalUSD] = ISNULL([TotalUSD],0)
                    ,[Createdate]
                    ,[Confirmeddate]
                    ,[VoucherNo] = ISNULL([VoucherNo],'')
                    ,[VoucherDate]
                    ,[Seq] = ISNULL([Seq],'')
                    ,[SourceType] = ISNULL([SourceType],'')
                    ,[WeaveType] = ISNULL([WeaveType],'')
                    ,[IrregularMtlType] = ISNULL([IrregularMtlType],'')
                    ,[IrregularQty] = ISNULL([IrregularQty],0)
                    ,[IrregularFOC] = ISNULL([IrregularFOC],0)
                    ,[IrregularPriceUSD] = ISNULL([IrregularPriceUSD],0)
                    ,[IrregularAmtUSD] = ISNULL([IrregularAmtUSD],0)
                    ,[BIFactoryID] = s.BIFactoryID
                    ,[BIInsertDate] = s.BIInsertDate
	            from #tmpFinal s
	            where not exists(select 1 from P_ICRAnalysis where ICRNo = s.ICRNo and Seq = s.Seq)

				";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#tmpFinal");
            }

            return finalResult;
        }
    }
}
