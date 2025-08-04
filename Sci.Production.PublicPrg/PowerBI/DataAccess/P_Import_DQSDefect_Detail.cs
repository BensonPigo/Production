using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 MES/Endline/R08 已脫鉤 待討論
    /// </summary>
    public class P_Import_DQSDefect_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_DQSDefect_Detail(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                finalResult = this.GetDQSDefect_Detail_Data(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable dataTable = finalResult.Dt;

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

        private Base_ViewModel GetDQSDefect_Detail_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@eDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
			select 
			  fac.Zone
			, [Brand] = ord.BrandID
			, [Buyer Delivery Date] = ord.BuyerDelivery
			, ins.Line
			, [FactoryID] = ins.FactoryID
			, ins.Team 
			, ins.Shift
 			, [PO#] = ord.custpono  
			, [Style] = ord.styleid
			, [SP#] = ins.OrderId
			, ins.Article
			, ins.Status
			, [FixType] = iif(ins.FixType='Repl.','Replacement',ins.FixType)
			, [FirstInspectionDate] = cast(Ins.AddDate as date)
			, [FirstInspectedTime] = format(ins.AddDate,'HH:mm:ss')
			, [Inspected QC] = Inspection_QC.Name
			, [Fixed Time] = iif(ins.Status<>'Fixed','', format(ins.EditDate,'yyyy/MM/dd HH:mm') )
			, [Fixed QC] = iif(ins.Status<>'Fixed','',Inspection_fixQC.Name)
			, [ProductType] =  case when ins.Location = 'T' then 'TOP'
									when ins.Location = 'B' then 'BOTTOM'
									when ins.Location = 'I' then 'INNER'
									when ins.Location = 'O' then 'OUTER'
									else ''
								end
			, ins.Size
			, [DefectTypeDescritpion] = gdt.Description
			, [DefectCodeDescritpion] = gdc.Description
			, [Area] = ind.AreaCode
			, [ReworkCardNo] = ins.ReworkCardNo
			, [DefectTypeID] = ind.GarmentDefectTypeID
			, [DefectCodeID] = ind.GarmentDefectCodeID
			, [DefectCodeLocalDesc] = iif(isnull(gdc.LocalDescription,'') = '',gdc.Description,gdc.LocalDescription)
			, [IsCriticalDefect] = iif(isnull(IsCriticalDefect,0) = 0, '', 'Y')
            , [BIFactoryID] =  @BIFactoryID
			, [BIInsertDate] = GetDate()
            , [InspectionDetailUkey] = IND.UKEY
			from [ExtendServer].ManufacturingExecution.dbo.Inspection ins WITH(NOLOCK)
			inner join Production.dbo.orders ord WITH(NOLOCK) on ins.OrderId=ord.id
			inner join Production.dbo.Factory fac WITH(NOLOCK) on ins.FactoryID=fac.ID
			left  join [ExtendServer].ManufacturingExecution.dbo.Inspection_Detail ind WITH(NOLOCK) on ins.id=ind.ID	
			left  join Production.dbo.GarmentDefectCode gdc WITH(NOLOCK) on ind.GarmentDefectTypeID=gdc.GarmentDefectTypeID and ind.GarmentDefectCodeID=gdc.ID
			left  join Production.dbo.GarmentDefectType gdt WITH(NOLOCK) on gdc.GarmentDefectTypeID=gdt.ID
			outer apply(select name from [ExtendServer].ManufacturingExecution.dbo.pass1 p WITH(NOLOCK) where p.id= ins.AddName) Inspection_QC
			outer apply(select name from [ExtendServer].ManufacturingExecution.dbo.pass1 p WITH(NOLOCK) where p.id= ins.EditName) Inspection_fixQC
			where ins.Adddate >= @sDate
			and ins.Status <> 'Pass'
			Order by Zone,[Brand],[FactoryID],Line,Team,[SP#],Article,[ProductType],Size,[DefectTypeID],[DefectCodeID]
			";
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

                UPDATE P SET
                 P.[FtyZon]          		  = isnull(T.[Zone],'')
                ,P.[BrandID]				  = isnull(T.Brand,'')
                ,P.[BuyerDelivery]			  = T.[Buyer Delivery Date]
                ,P.[Line]					  = isnull(T.Line,'')
                ,P.[Team]					  = isnull(T.Team,'')
                ,P.[Shift]				      = isnull(T.[Shift],'')
                ,P.[POID]					  = isnull(T.[PO#],'')
                ,P.[StyleID]				  = isnull(T.Style,'')
                ,P.[SPNO]					  = isnull(T.[SP#],'')
                ,P.[Article]				  = isnull(T.Article,'')
                ,P.[Status]				      = isnull(T.[Status],'')
                ,P.[FixType]				  = isnull(T.FixType,'') 
                ,P.[FirstInspectDate]		  = T.[FirstInspectionDate]
                ,P.[FirstInspectTime]		  = T.[FirstInspectedTime]
                ,P.[InspectQCName]		      = isnull(T.[Inspected QC],'')
                ,P.[FixedTime]			      = isnull(T.[Fixed Time],'')
                ,P.[FixedQCName]			  = isnull(T.[Fixed QC],'')
                ,P.[ProductType]			  = isnull(T.ProductType,'')
                ,P.[SizeCode]				  = isnull(T.Size,'')
                ,P.[DefectTypeDesc]			  = isnull(T.[DefectTypeDescritpion],'')
                ,P.[DefectCodeDesc]			  = isnull(T.[DefectCodeDescritpion],'')
                ,P.[AreaCode]				  = isnull(T.Area,'')
                ,P.[ReworkCardNo]			  = isnull(T.ReworkCardNo,'')
                ,P.[GarmentDefectTypeID]	  = isnull(T.DefectTypeID,'')
                ,P.[GarmentDefectCodeID]	  = isnull(T.DefectCodeID,'')
                ,P.[DefectCodeLocalDesc]	  = isnull(T.DefectCodeLocalDesc,'')
                ,P.[IsCriticalDefect]		  = isnull(T.IsCriticalDefect,'')
                ,P.[BIFactoryID]              = T.[BIFactoryID]
                ,P.[BIInsertDate]             = T.[BIInsertDate]
                ,P.[BIStatus]                 = 'New'
                FROM P_DQSDefect_Detail P 
                INNER JOIN #Final_DQSDefect_Detail T ON P.[FactoryID] = T.[FactoryID] AND
										                P.[InspectionDetailUkey] = T.[InspectionDetailUkey]

				INSERT INTO [dbo].[P_DQSDefect_Detail]
                (
                    [FtyZon]
                    ,[BrandID]
                    ,[BuyerDelivery]
                    ,[Line]
                    ,[FactoryID]
                    ,[Team]
                    ,[Shift]
                    ,[POID]
                    ,[StyleID]
                    ,[SPNO]
                    ,[Article]
                    ,[Status]
                    ,[FixType]
                    ,[FirstInspectDate]
                    ,[FirstInspectTime]
                    ,[InspectQCName]
                    ,[FixedTime]
                    ,[FixedQCName]
                    ,[ProductType]
                    ,[SizeCode]
                    ,[DefectTypeDesc]
                    ,[DefectCodeDesc]
                    ,[AreaCode]
                    ,[ReworkCardNo]
                    ,[GarmentDefectTypeID]
                    ,[GarmentDefectCodeID]
		            ,[DefectCodeLocalDesc]
		            ,[IsCriticalDefect]
                    ,[BIFactoryID]
                    ,[BIInsertDate] 
                    ,[InspectionDetailUkey]
                    ,[BIStatus]
                )
                select 
                  [Zone] = isnull([Zone],'')
                , Brand = isnull(Brand,'')
	            , [Buyer Delivery Date]
	            , Line = isnull(Line,'')
	            , [FactoryID] = isnull(FactoryID,'')
	            , Team = isnull(Team,'')
	            , [Shift] = isnull([Shift],'')
 	            , [PO#] = isnull([PO#],'')
	            , [Style] = isnull(Style,'')
	            , [SP#] = isnull([SP#],'')
	            , Article = isnull(Article,'')
	            , [Status] = isnull([Status],'')
	            , [FixType] = isnull(FixType,'')
	            , [FirstInspectionDate]
	            , [FirstInspectedTime]
	            , [Inspected QC] = isnull([Inspected QC],'')
	            , [Fixed Time] = isnull([Fixed Time],'')
	            , [Fixed QC] = isnull([Fixed QC],'')
	            , [ProductType] = isnull(ProductType,'')
	            , [Size]= isnull(Size,'')
	            , [DefectTypeDescritpion] = isnull([DefectTypeDescritpion],'')
	            , [DefectCodeDescritpion] = isnull([DefectCodeDescritpion],'')
	            , [Area] = isnull(Area,'')
	            , [ReworkCardNo] = isnull(ReworkCardNo,'')
                , [DefectTypeID] = isnull(DefectTypeID,'')
                , [DefectCodeID] = isnull(DefectCodeID,'')
                , DefectCodeLocalDesc = isnull(DefectCodeLocalDesc,'')
                , [IsCriticalDefect] = isnull(IsCriticalDefect,'')
                , [BIFactoryID]
                , [BIInsertDate]  
                , [InspectionDetailUkey]
                , [BIStatus] = 'New'
                from #Final_DQSDefect_Detail  t
                where not exists 
                (
					select 1 from P_DQSDefect_Detail P 
					where
					P.[FactoryID] = T.[FactoryID] AND P.[InspectionDetailUkey] = T.[InspectionDetailUkey]	
				)

                if @IsTrans = 1
                begin
				    Insert Into P_DQSDefect_Detail_History([FactoryID], [Ukey], [InspectionDetailUkey], [BIFactoryID], [BIInsertDate])
				    Select FactoryID, Ukey, InspectionDetailUkey, BIFactoryID, GETDATE()
				    FROM P_DQSDefect_Detail p
                    where not exists 
				    (
					    select 1 from ManufacturingExecution.dbo.Inspection_Detail t 
					    where P.[InspectionDetailUkey] = T.[Ukey]	
				    )
                end

                Delete p
				from P_DQSDefect_Detail p
				where not exists 
				(
					select 1 from ManufacturingExecution.dbo.Inspection_Detail t 
					where P.[InspectionDetailUkey] = T.[Ukey]		
				)";

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_DQSDefect_Detail", paramters: sqlParameters);
            }

            return finalResult;
        }
    }
}
