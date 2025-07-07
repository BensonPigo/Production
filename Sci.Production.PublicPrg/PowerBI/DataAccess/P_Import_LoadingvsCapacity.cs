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
    public class P_Import_LoadingvsCapacity
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_LoadingvsCapacity(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                var today = DateTime.Now;
                var lastYearStart = new DateTime(today.Year - 1, 1, 1);
                item.SDate = lastYearStart.AddDays(7);
            }

            if (!item.EDate.HasValue)
            {
                var currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
                item.EDate = currentYearStart.AddYears(3).AddDays(6);
            }

            try
            {
                Base_ViewModel resultReport = this.GetLoadingvsCapacity_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

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

        private Base_ViewModel GetLoadingvsCapacity_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@Date_S", item.SDate),
                new SqlParameter("@Date_E", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
            declare @YearMonth_S date =  DATEADD(YEAR,-1,DATEADD(YEAR,DATEDIFF(YEAR,0,getdate()),0))
            declare @YearMonth_E date = DATEADD(YEAR,3,DATEADD(YEAR,DATEDIFF(YEAR,0,getdate()),0)) -1

            declare @NoRestrictOrdersDelivery bit = (select TOP 1 NoRestrictOrdersDelivery from System)
			declare @SourceA  TABLE
			(
				[FACTORYID] [VARCHAR](25) NULL,
				[MDivisionID] [VARCHAR](25) NULL,
				[ArtworkTypeID] [VARCHAR](25) NULL,
				[sumValue] [decimal](20,10) NULL,
				[Key] [varchar](25) NULL,
				[Half key][varchar](25) NULL
			)	
			INSERT INTO @SourceA
			select 
			a.FactoryID
			, a.MDivisionID
			, a.ArtworkTypeID
			, sumValue = sum(Value)
			, a.[Key]
			, a.[Half key]
			from
			(
				/************************** Orders **************************/
				Select 
				tmsCost.ID
				,o.FactoryID
				,o.MDivisionID
				,ArtworkTypeID = artT.ID
				,[Value] = case When artT.ProductionUnit = 'TMS' Then isnull(convert(numeric(6,0), o.Qty) * tms * 1.0 / 1400 * getCPURate.CpuRate,0)
								When artT.ArtworkUnit = 'STITCH' Then isnull(convert(numeric(6,0),o.Qty) * tmsCost.Qty * 1.0 / 1000 * getCPURate.CpuRate,0)
								When artT.ArtworkUnit = 'PPU' Then isnull(convert(numeric(6,0),o.Qty) * tmsCost.Price * getCPURate.CpuRate,0)
								When artT.ProductionUnit = 'Qty' Then isnull(convert(numeric(6,0),o.Qty) * tmsCost.Qty * getCPURate.CpuRate,0)
								Else isnull(o.Qty * getCPURate.CpuRate,0)
								End
				,[Key] = convert(varchar(6),dateadd(day,-7,o.SCIDelivery),112)
				,[Half key] = case
							  when day(o.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, o.SCIDelivery),112),6) + '02'
							  when day(o.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, o.SCIDelivery, 112),6) + '01'
							  when day(o.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, o.SCIDelivery, 112),6) + '02'
							  else '' end
				From [Orders] o with (nolock)
				Left join [Order_TmsCost] tmsCost with (nolock) on tmsCost.Id = o.ID
				Left join ArtworkType artT with (nolock) on artT.id = tmsCost.ArtworkTypeID
				outer APPLY (SELECT CpuRate FROM [GetCPURate](o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O')) getCPURate
				Left join Factory f with (nolock) On o.FactoryID = f.ID
				WHERE 
				o.Category IN ('B', 'S') 
				and (o.SciDelivery between @Date_S and @Date_E)
				And (o.Junk = 0 OR (o.Junk = 1 AND o.NeedProduction = 1))
				And (o.IsBuyBack = 0 OR (o.IsBuyBack = 1 AND o.BuyBackReason = 'KeepPanel'))
				And (@NoRestrictOrdersDelivery = 1
					or 
					(@NoRestrictOrdersDelivery = 0 and (o.IsForecast = 0 OR (o.IsForecast = 1 AND (o.SciDelivery <= DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 6) OR o.BuyerDelivery < DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 0))))))
				And F.IsProduceFty = 1
				/************************** Forecast **************************/
				UNION
				Select 
				o.ID
				,o.FactoryID
				,o.MDivisionID
				,ArtworkTypeID = at.ID
				,[Value] = case When at.ProductionUnit = 'TMS' Then convert(numeric(6,0), o.Qty) * tms * 1.0 / 1400 * getCPURate.CpuRate
							   When at.ArtworkUnit = 'STITCH' Then  convert(numeric(6,0), o.Qty) * convert(numeric(6,0), tmsCost.Qty) * 1.0 / 1000 * getCPURate.CpuRate
							   When at.ArtworkUnit = 'PPU' Then isnull(convert(numeric(6,0), o.Qty) * tmsCost.Price * getCPURate.CpuRate,0)
							   When at.ProductionUnit = 'Qty' Then convert(numeric(6,0), o.Qty) * convert(numeric(6,0), tmsCost.Qty) * getCPURate.CpuRate
							   Else o.Qty * getCPURate.CpuRate
							   End
				,[Key] =  convert(varchar(6),dateadd(day,-7, o.BuyerDelivery), 112)
				,[Half key] = case
							  when day(o.BuyerDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, o.BuyerDelivery),112),6) + '02'
							  when day(o.BuyerDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, o.BuyerDelivery, 112),6) + '01'
							  when day(o.BuyerDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, o.BuyerDelivery, 112),6) + '02'
							  else ''	end
				From Orders as o with (nolock)
				Left join [Style]	with (nolock) on o.BrandID=Style.BrandID and o.StyleID =Style.ID and  o.SeasonID = Style.SeasonID
				Left join [Style_TmsCost] tmsCost with (nolock) on tmsCost.StyleUKey = Style.Ukey
				Left join [ArtworkType] at with (nolock) on at.id = tmsCost.ArtworkTypeID
				Left join Factory f with (nolock) On o.FactoryID = f.ID
				Outer Apply (select CpuRate From [GetCPURate](o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'S')) getCPURate
				WHERE 
				o.ForecastCategory IN ('B', 'S') 
				and o.BuyerDelivery between @Date_S and @Date_E 
				and (Style.Ukey is null OR Style.Junk = 0) and o.Category = ''
				and (@NoRestrictOrdersDelivery = 1
					or 
					(@NoRestrictOrdersDelivery = 0 and (o.IsForecast = 0 OR (o.IsForecast = 1 AND (o.SciDelivery <= DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 6) OR o.BuyerDelivery < DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 0))))))
				and F.IsProduceFty = 1

				/************************** FactoryOrder **************************/
				UNION
				Select
				o.ID
				, o.FactoryID
				, f.MDivisionID
				, ArtworkTypeID = artT.ID
				, Value = case When artT.ProductionUnit = 'TMS' Then convert(numeric(6,0), o.Qty) * tms * 1.0 / 1400 * getCPURate.CpuRate
					When artT.ArtworkUnit = 'STITCH' Then  convert(numeric(6,0), o.Qty) * convert(numeric(6,0), tmsCost.Qty) * 1.0 / 1000 * getCPURate.CpuRate
					When artT.ArtworkUnit = 'PPU' Then isnull(convert(numeric(6,0), o.Qty) * tmsCost.Price * getCPURate.CpuRate,0)
					When artT.ProductionUnit = 'Qty' Then convert(numeric(6,0), o.Qty) * convert(numeric(6,0), tmsCost.Qty) * getCPURate.CpuRate
					Else convert(numeric(6,0), o.Qty) * getCPURate.CpuRate
					End
				, [Key]               =  convert(varchar(6),dateadd(day,-7, o.SCIDelivery), 112)
				, [Half key] = case
								when day(o.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, o.SCIDelivery),112),6) + '02'
								when day(o.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, o.SCIDelivery, 112),6) + '01'
								when day(o.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, o.SCIDelivery, 112),6) + '02'
								else ''	end
				From Orders o with (nolock)
				Left join Factory f with (nolock) On o.FactoryID = f.ID
				Left join Style s	with (nolock) on o.BrandID=s.BrandID and o.StyleID =s.ID and  o.SeasonID = s.SeasonID
				Left join Style_TmsCost tmsCost with (nolock) on tmsCost.StyleUKey = s.Ukey
				Left join ArtworkType artT with (nolock) on artT.id = tmsCost.ArtworkTypeID
				Outer Apply (select 1 as CpuRate ) getCPURate
				WHERE 
				o.Junk = 0 
				and o.Qty > 0  
				and o.SubconInType in ('2', '3') 
				and o.SCIDelivery between @Date_S and @Date_E
				and o.LocalOrder = '1'
				and (@NoRestrictOrdersDelivery = 1
					or 
					(@NoRestrictOrdersDelivery = 0 and (o.IsForecast = 0 OR (o.IsForecast = 1 AND (o.SciDelivery <= DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 6) OR o.BuyerDelivery < DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 0))))))
				and F.IsProduceFty = 1

				/************************** 負的FactoryOrder **************************/
				UNION
				Select o.ID
				, FactoryID = o.ProgramID
				, Factory.MDivisionID
				, ArtworkTypeID = at.ID
				, Value = case When at.ProductionUnit = 'TMS' Then convert(numeric(6,0), o.Qty) * tms / GetStandardTMS.StdTMS * getCPURate.CpuRate
					When at.ArtworkUnit = 'STITCH' Then  convert(numeric(6,0), o.Qty) * convert(numeric(6,0), tmsCost.Qty) / 1000 * getCPURate.CpuRate
					When at.ArtworkUnit = 'PPU' Then isnull(convert(numeric(6,0), o.Qty) * tmsCost.Price * getCPURate.CpuRate,0)
					When at.ProductionUnit = 'Qty' Then convert(numeric(6,0), o.Qty) * convert(numeric(6,0), tmsCost.Qty) * getCPURate.CpuRate
					Else convert(numeric(6,0), o.Qty) * getCPURate.CpuRate
					End * -1
				, [Key]               =  convert(varchar(6),dateadd(day,-7, o.SCIDelivery), 112)
				, [Half key] = case
								when day(o.SCIDelivery) BETWEEN 1 and 7 then LEFT(CONVERT(varchar, dateadd(month, -1, o.SCIDelivery),112),6) + '02'
								when day(o.SCIDelivery) BETWEEN 8 and 22 then LEFT(CONVERT(varchar, o.SCIDelivery, 112),6) + '01'
								when day(o.SCIDelivery) BETWEEN 23 and 31 then LEFT(CONVERT(varchar, o.SCIDelivery, 112),6) + '02'
								else ''	end
				From Orders o with (nolock)
				Left join Factory with (nolock) On o.ProgramID = Factory.ID
				Left join Style	with (nolock) on o.BrandID=Style.BrandID and o.StyleID =Style.ID and  o.SeasonID = Style.SeasonID
				Left join Style_TmsCost tmsCost with (nolock) on tmsCost.StyleUKey = Style.Ukey
				Left join ArtworkType at with (nolock) on at.id = tmsCost.ArtworkTypeID
				Outer Apply (select 1 as CpuRate ) getCPURate
				Outer Apply (Select top 1 StdTMS From System) GetStandardTMS
				WHERE 
				o.Junk = 0 
				and o.Qty > 0 
				and o.SubconInType in ('2') 
				and o.SCIDelivery between @Date_S and @Date_E
				and o.LocalOrder = '1'
				and (@NoRestrictOrdersDelivery = 1
					or 
					(@NoRestrictOrdersDelivery = 0 and (o.IsForecast = 0 OR (o.IsForecast = 1 AND (o.SciDelivery <= DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 6) OR o.BuyerDelivery < DATEADD(m, DATEDIFF(m, 0, DATEADD(m, 5, GETDATE())), 0))))))
				and Factory.IsProduceFty = 1
			) a
			Group by a.FactoryID, a.MDivisionID, a.ArtworkTypeID, a.[Key], a.[Half key]

			Declare @KeyTable Table ([Half key] varchar(8) not null  primary key, [key] varchar(6) not null)
			Declare @tmpDate DATE = dateadd(m,-1, @YearMonth_S)
			While (@tmpDate <= @YearMonth_E)
			Begin
				Insert Into @KeyTable VALUES(Format(@tmpDate,'yyyyMM01'),Format(@tmpDate,'yyyyMM'))
				Insert Into @KeyTable VALUES(Format(@tmpDate,'yyyyMM02'),Format(@tmpDate,'yyyyMM'))
				Select @tmpDate = DATEADD(m, 1, @tmpDate )
			End

			Select 
			[MDivisionID]
			,[KpiCode]
			,[Key]
			,[Halfkey]
			,[ArtworkTypeID]
			,[CapacityCPU]
			,[LoadingCPU]
			,[TransferBIDate] 
            ,[BIFactoryID] =  @BIFactoryID
            ,[BIInsertDate] = GetDate()
			From
			(
				select Factory.mDivisionID
				, Factory.KpiCode
				, [Key] = keyTable.[Key]
				, [Halfkey] = keyTable.[Half key]
				, ArtworkTypeID = at.Id
				, [CapacityCPU] = SUM(iif(Right(keyTable.[Half key],2) = '01', isnull(GetCapacityHalf1.Capacity,0), isnull(GetCapacityHalf2.Capacity,0)))
				, [LoadingCPU] = SUM(isnull(tmpSumValue.sumValue,0))
				, TransferBIDate = getdate()
				From Factory with (nolock)
				Full join ArtworkType at with (nolock) on 1 = 1
				Full join @KeyTable keyTable on 1 = 1
				Left join Factory_TMS ftms with (nolock) on Factory.ID = ftms.id and at.Id = ftms.ArtworkTypeID and Concat(ftms.Year, ftms.Month) = keyTable.[key]
				Left join Factory_WorkHour fw with (nolock) on fw.ID = Factory.ID and fw.Year = ftms.Year and fw.Month = ftms.Month
				outer apply (Select top 1 StdTMS From System) GetStandardTMS
				outer apply (Select sum(isnull(sumValue,0))sumValue From @SourceA as tmpSumValue where tmpSumValue.[Half Key] = keyTable.[Half Key] and tmpSumValue.FactoryID = Factory.ID and tmpSumValue.ArtworkTypeID = at.ID and tmpSumValue.MDivisionID = Factory.MDivisionID)tmpSumValue
				outer apply (Select WorkDay = fw.HalfMonth1 + fw.HalfMonth2) GetWorkingDay
				outer apply (Select Capacity = iif(at.ArtworkUnit = '', isnull(ftms.TMS, 0) / convert( numeric(5,0) ,GetStandardTMS.StdTMS) * IIF(ftms.ArtworkTypeID = 'SEWING', 3600, 60), isnull(ftms.TMS, 0))) GetCapacity
				outer apply (Select Capacity = Round(iif(GetWorkingDay.WorkDay = 0, 0, GetCapacity.Capacity * fw.HalfMonth1 / GetWorkingDay.WorkDay),6)) GetCapacityHalf1
				outer apply (Select Capacity = Round(iif(GetWorkingDay.WorkDay = 0, 0, GetCapacity.Capacity * fw.HalfMonth2 / GetWorkingDay.WorkDay),6)) GetCapacityHalf2
				Where Factory.IsSCI = 1 And at.Junk = 0
				Group by  Factory.mDivisionID, at.Id, keyTable.[Key], keyTable.[Half Key], GetStandardTMS.StdTMS, Factory.KpiCode
			)b
			where [CapacityCPU] <> 0 
			or [LoadingCPU] <> 0
			order by mdivisionID,kpicode,[Halfkey],ArtworkTypeID";
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				if @IsTrans = 1
				begin
					insert into P_LoadingvsCapacity_History([MDivisionID], [FactoryID], [Key], [Halfkey], [ArtworkTypeID], [BIFactoryID], [BIInsertDate])
					select [MDivisionID], [FactoryID], [Key], [Halfkey], [ArtworkTypeID], [BIFactoryID], GETDATE()
					from P_LoadingvsCapacity
				end

				delete P_LoadingvsCapacity

				insert into P_LoadingvsCapacity([MDivisionID], [FactoryID], [Key], [Halfkey], [ArtworkTypeID], [Capacity(CPU)], [Loading(CPU)], [TransferBIDate], [BIFactoryID], [BIInsertDate])
				select 
					t.[MDivisionID],
					t.[KpiCode],
					t.[Key],
					t.[Halfkey],
					t.[ArtworkTypeID],
					t.[CapacityCPU],
					t.[LoadingCPU],
					t.[TransferBIDate],
					t.[BIFactoryID],
					t.[BIInsertDate]    
				from #tmp t
				where not exists
				(
					select 1 from P_LoadingvsCapacity p 
					where 
					p.[MDivisionID]		= t.[MDivisionID]	and 
					p.[FactoryID]		= t.[KpiCode]		and
					p.[Key]				= t.[Key]			and
					p.[Halfkey]			= t.[Halfkey]		and
					p.[ArtworkTypeID]	= t.[ArtworkTypeID] 
				)
				order by [MDivisionID],[KpiCode],[Halfkey] asc,[ArtworkTypeID]
				";

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#tmp", paramters: sqlParameters);
            }

            return finalResult;
        }
    }
}
