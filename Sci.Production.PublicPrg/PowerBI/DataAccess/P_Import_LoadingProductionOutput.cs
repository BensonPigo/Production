using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 裡面用的GetProductionOutputSummary在Planning.R05跟Centralized.R05報表都呼叫這支
    /// </summary>
    public class P_Import_LoadingProductionOutput
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_LoadingProductionOutput(DateTime? sDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            try
            {
                Base_ViewModel resultReport = this.GetLoadingProductionOutputData((DateTime)sDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_LoadingProductionOutput", false);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@Year", sdate.Year));

            using (sqlConn)
            {
                string sql = $@" 
				update t
				set	    
				t.MDivisionID =  s.MDivisionID,
				t.FtyZone =  s.FtyZone,
				t.FactoryID =  s.FactoryID,
				t.BuyerDelivery =  s.BuyerDelivery,
				t.SciDelivery =  s.SciDelivery,
				t.SCIKey =  s.SCIKey,
				t.SCIKeyHalf =  s.SCIKeyHalf,
				t.BuyerKey =  s.BuyerKey,
				t.BuyerKeyHalf =  s.BuyerKeyHalf,
				t.BuyerMonthHalf = s.BuyerMonthHalf,
				t.SPNO =  s.ID,
				t.Category =  s.Category,
				t.Cancelled =  s.Cancelled,
				t.IsCancelNeedProduction =  s.IsCancelNeedProduction,
				t.PartialShipment =  s.PartialShipment,
				t.LastBuyerDelivery =  s.LastBuyerDelivery,
				t.StyleID =  s.StyleID,
				t.SeasonID =  s.SeasonID,
				t.CustPONO =  s.CustPONO,
				t.BrandID =  s.BrandID,
				t.CPU =  s.CPU,
				t.Qty =  s.Qty,
				t.FOCQty =  s.FOCQty,
				t.PulloutQty =  s.PulloutQty,
				t.OrderShortageCPU =  s.OrderShortageCPU,
				t.TotalCPU =  s.TotalCPU,
				t.SewingOutput =  s.SewingOutput,
				t.SewingOutputCPU =  s.SewingOutputCPU,
				t.BalanceQty =  s.BalanceQty,
				t.BalanceCPU =  s.BalanceCPU,
				t.BalanceCPUIrregular =  s.BalanceCPUIrregular,
				t.SewLine =  s.SewLine,
				t.Dest =  s.Dest,
				t.OrderTypeID =  s.OrderTypeID,
				t.ProgramID =  s.ProgramID,
				t.CdCodeID =  s.CdCodeID,
				t.ProductionFamilyID =  s.ProductionFamilyID,
				t.FtyGroup =  s.FtyGroup,
				t.PulloutComplete =  s.PulloutComplete,
				t.SewInLine =  s.SewInLine,
				t.SewOffLine =  s.SewOffLine,
				t.TransFtyZone =  s.TransFtyZone,
				t.CDCodeNew =  s.CDCodeNew,
				t.ProductType =  s.ProductType,
				t.FabricType =  s.FabricType,
				t.Lining =  s.Lining,
				t.Gender =  s.Gender,
				t.Construction =  s.Construction,
				t.[FM Sister] = s.FMSister,
				t.[Sample Group] = s.SampleGroup,
				t.[Order Reason] = s.OrderReason,
				t.[BuyBackReason] = s.[BuyBackReason],
				t.[LastProductionDate] = s.[LastProductionDate],
				t.[CRDDate] = s.[CRDDate],
				t.[BIFactoryID] = s.[BIFactoryID],
				t.[BIInsertDate] = s.[BIInsertDate]
				from P_LoadingProductionOutput as t
				inner join #Final s 
				on t.FactoryID=s.FactoryID  
				   AND t.SPNO=s.ID 


				insert into P_LoadingProductionOutput
				(
					[MDivisionID]
					,[FtyZone]
					,[FactoryID]
					,[BuyerDelivery]
					,[SciDelivery]
					,[SCIKey]
					,[SCIKeyHalf]
					,[BuyerKey]
					,[BuyerKeyHalf]
					,[BuyerMonthHalf]
					,[SPNO]
					,[Category]
					,[Cancelled]
					,[IsCancelNeedProduction]
					,[PartialShipment]
					,[LastBuyerDelivery]
					,[StyleID]
					,[SeasonID]
					,[CustPONO]
					,[BrandID]
					,[CPU]
					,[Qty]
					,[FOCQty]
					,[PulloutQty]
					,[OrderShortageCPU]
					,[TotalCPU]
					,[SewingOutput]
					,[SewingOutputCPU]
					,[BalanceQty]
					,[BalanceCPU]
					,[BalanceCPUIrregular]
					,[SewLine]
					,[Dest]
					,[OrderTypeID]
					,[ProgramID]
					,[CdCodeID]
					,[ProductionFamilyID]
					,[FtyGroup]
					,[PulloutComplete]
					,[SewInLine]
					,[SewOffLine]
					,[TransFtyZone]
					,[CDCodeNew]
					,[ProductType]
					,[FabricType]
					,[Lining]
					,[Gender]
					,[Construction]
					,[FM Sister]
					,[Sample Group]
					,[Order Reason]
					,[BuyBackReason]
					,[LastProductionDate]
					,[CRDDate]
					,[BIFactoryID]
					,[BIInsertDate]
				)
				select  
				s.MDivisionID,
				s.FtyZone,
				s.FactoryID,
				s.BuyerDelivery,
				s.SciDelivery,
				s.SCIKey,
				s.SCIKeyHalf,
				s.BuyerKey,
				s.BuyerKeyHalf,
				s.BuyerMonthHalf,
				s.ID,
				s.Category,
				s.Cancelled,
				s.IsCancelNeedProduction,
				s.PartialShipment,
				s.LastBuyerDelivery,
				s.StyleID,
				s.SeasonID,
				s.CustPONO,
				s.BrandID,
				s.CPU,
				s.Qty,
				s.FOCQty,
				s.PulloutQty,
				s.OrderShortageCPU,
				s.TotalCPU,
				s.SewingOutput,
				s.SewingOutputCPU,
				s.BalanceQty,
				s.BalanceCPU,
				s.BalanceCPUIrregular,
				s.SewLine,
				s.Dest,
				s.OrderTypeID,
				s.ProgramID,
				s.CdCodeID,
				s.ProductionFamilyID,
				s.FtyGroup,
				s.PulloutComplete,
				s.SewInLine,
				s.SewOffLine,
				s.TransFtyZone,
				s.CDCodeNew,
				s.ProductType,
				s.FabricType,
				s.Lining,
				s.Gender,
				s.Construction,
				s.FMSister,
				s.SampleGroup,
				s.OrderReason,
				s.[BuyBackReason],
				s.[LastProductionDate],
				s.[CRDDate]
				,s.[BIFactoryID]
				,s.[BIInsertDate]
				from #Final s
				where not exists
				(
					select 1 from P_LoadingProductionOutput t 
					where t.FactoryID=s.FactoryID  
					AND t.SPNO = s.ID
				)

				-- 先寫入 Histroy
				insert into P_LoadingProductionOutput_History(FactoryID, Ukey, BIFactoryID, BIInsertDate)
				select t.FactoryID, t.Ukey, t.BIFactoryID, GETDATE()
				from P_LoadingProductionOutput t
				where 
				(
					YEAR(BuyerDelivery) = @Year
					or
					Year(cast(dateadd(day,-7,SciDelivery) as date)) = @Year	
				)
				and exists (select 1 from #Final f where t.FactoryID=f.FactoryID AND t.MDivisionID=f.MDivisionID  ) 
				and not exists (select 1 from #Final s where t.FactoryID=s.FactoryID AND t.SPNO=s.ID );

				delete t
				from P_LoadingProductionOutput t WITH (NOLOCK)
				where 
				(
					YEAR(BuyerDelivery) = @Year
					or
					Year(cast(dateadd(day,-7,SciDelivery) as date)) = @Year	
				)
				and exists	   (select 1 from #Final f where t.FactoryID=f.FactoryID AND t.MDivisionID=f.MDivisionID  ) 
				and not exists (select 1 from #Final s where t.FactoryID=s.FactoryID AND t.SPNO=s.ID );

				-- 先寫入 Histroy
				insert into P_LoadingProductionOutput_History(FactoryID, Ukey, BIFactoryID, BIInsertDate)
				select t.FactoryID, t.Ukey, t.BIFactoryID, GETDATE()
				from P_LoadingProductionOutput t
				left join [MainServer].Production.dbo.Orders o on t.SPNO = o.ID and t.FactoryID = o.FactoryID
				where o.ID is null
	
				delete t
				from P_LoadingProductionOutput t
				left join [MainServer].Production.dbo.Orders o on t.SPNO = o.ID and t.FactoryID = o.FactoryID
				where o.ID is null
                ";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#Final");
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetLoadingProductionOutputData(DateTime sdate)
        {
            StringBuilder sqlcmdSP = new StringBuilder();

            sqlcmdSP.Append("exec dbo.GetProductionOutputSummary");
            sqlcmdSP.Append(!MyUtility.Check.Empty(sdate.Year) ? $" '{sdate.Year}'," : "'',"); // Year
            sqlcmdSP.Append("'',"); // Brand
            sqlcmdSP.Append("'',"); // Mdivision
            sqlcmdSP.Append("'',"); // Factory
            sqlcmdSP.Append("'',"); // Zone
            sqlcmdSP.Append(" 1,"); // DateType
            sqlcmdSP.Append(" 1,"); // ChkOrder
            sqlcmdSP.Append(" 1,"); // ChkForecast
            sqlcmdSP.Append(" 1,"); // ChkFtylocalOrder
            sqlcmdSP.Append(" 1,"); // ExcludeSampleFactory
            sqlcmdSP.Append(" 1,"); // ChkMonthly
            sqlcmdSP.Append(" 1,"); // @IncludeCancelOrder
            sqlcmdSP.Append(" 0,"); // IsFtySide 工廠端限制ForeCast單 僅顯示SCI delivery or buyer delivery 小於等於 當月份+4個月的月底+7天
            sqlcmdSP.Append(" 1,"); // @IsPowerBI
            sqlcmdSP.Append(" 0 "); // @IsByCMPLockDate

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmdSP.ToString(), out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            DualResult result;
            DBProxy.Current.OpenConnection("Production", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				declare @strID nvarchar(15) = N'SubCON-Out_'
		
				select * 
				, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
				, [BIInsertDate] = GetDate()
				from 
				(
					-- 非外代工
					select
					t.[MDivisionID],
					t.[FtyZone],
					t.[FactoryID],
					T.BuyerDelivery,
					T.SciDelivery,
					T.SCIKey,
					T.SCIKeyHalf,
					T.BuyerKey,
					T.BuyerKeyHalf,
					t.BuyerMonthHalf,
					t.[ID],
					T.Category ,
					T.Cancelled,
					T.IsCancelNeedProduction,
					t.[Buyback],
					T.PartialShipment,
					T.LastBuyerDelivery,
					T.StyleID,
					T.SeasonID,
					T.CustPONO,
					T.BrandID,
					T.CPU,
					T.Qty,
					T.FOCQty,
					T.PulloutQty,
					T.OrderShortageCPU,
					t.[TotalCPU],
					t.[SewingOutput],
					t.[SewingOutputCPU],
					t.[BalanceQty],
					t.[BalanceCPU],
					t.[BalanceCPUIrregular],
					T.SewLine,
					T.Dest,
					T.OrderTypeID,
					T.ProgramID,
					CdCodeID = '',
					ProductionFamilyID = '',
					T.FtyGroup,
					t.[PulloutComplete],
					T.SewInLine,
					T.SewOffLine,
					T.TransFtyZone 
					,[CDCodeNew] = sty.CDCodeNew
					,[ProductType] = sty.ProductType
					,[FabricType] = sty.FabricType
					,[Lining] = sty.Lining
					,[Gender] = sty.Gender
					,[Construction] = sty.Construction
					,t.FMSister
					,t.SampleGroup
					,t.OrderReason
					,t.BuyBackReason
					,t.LastProductionDate
					,t.CRDDate
					from #tmp t
					Outer apply 
					(
						SELECT s.CDCodeNew
							, s.[ID]
							, ProductType = r2.Name
							, FabricType = r1.Name
							, Lining
							, Gender
							, Construction = d1.Name
						FROM Production.dbo.Orders o WITH(NOLOCK)
						left join Production.dbo.Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey
						left join Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
						left join Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
						left join Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
						where o.ID = t.ID
					)sty
					where exists(select 1 from Production.dbo.Factory f WITH(NOLOCK) where f.ID = t.FactoryID and f.IsProduceFty = 1)
					
					union all

					-- 外代工
					select 
					[MDivisionID] = F.MDivisionID,
					[FtyZone] = F.FtyZone,
					[FactoryID] = F.ID,
					T.BuyerDelivery,
					T.SciDelivery,
					T.SCIKey,
					T.SCIKeyHalf,
					T.BuyerKey,
					T.BuyerKeyHalf,
					t.BuyerMonthHalf,
					[ID] =  CONVERT(varchar(24), @strID + T.ID),
					T.Category ,
					T.Cancelled,
					T.IsCancelNeedProduction,
					[Buyback],
					T.PartialShipment,
					T.LastBuyerDelivery,
					T.StyleID,
					T.SeasonID,
					T.CustPONO,
					T.BrandID,
					T.CPU,
					T.Qty,
					T.FOCQty,
					T.PulloutQty,
					T.OrderShortageCPU,
					[TotalCPU] = -TotalCPU,
					[SewingOutput] = 0,
					[SewingOutputCPU] = 0,
					[BalanceQty] = 0,
					[BalanceCPU] = 0,
					[BalanceCPUIrregular] = 0,
					T.SewLine,
					T.Dest,
					T.OrderTypeID,
					T.ProgramID,
					CdCodeID = '',
					ProductionFamilyID = '',
					T.FtyGroup,
					[PulloutComplete],
					T.SewInLine,
					T.SewOffLine,
					T.TransFtyZone 
					,[CDCodeNew] = sty.CDCodeNew
					,[ProductType] = sty.ProductType
					,[FabricType] = sty.FabricType
					,[Lining] = sty.Lining
					,[Gender] = sty.Gender
					,[Construction] = sty.Construction
					,t.FMSister
					,t.SampleGroup
					,t.OrderReason
					,t.BuyBackReason
					,t.LastProductionDate
					,t.CRDDate
					from #tmp T
					LEFT JOIN Production.dbo.SCIFty f WITH(NOLOCK) ON f.ID= T.TransFtyZone
					Outer apply 
					(
						SELECT s.CDCodeNew
							, s.[ID]
							, ProductType = r2.Name
							, FabricType = r1.Name
							, Lining
							, Gender
							, Construction = d1.Name
						FROM Production.dbo.Orders o WITH(NOLOCK)
						left join Production.dbo.Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey
						left join Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
						left join Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
						left join Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
						where o.ID = T.ID
					)sty
					where TransFtyZone != ''
				) a
				drop table #tmp";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dataTables, null, sql, out DataTable finalDataTable, conn: sqlConn);
                resultReport.Dt = finalDataTable;
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return resultReport;
        }
    }
}
