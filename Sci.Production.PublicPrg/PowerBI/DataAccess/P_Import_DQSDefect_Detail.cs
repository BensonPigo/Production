using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static System.Windows.Forms.AxHost;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 MES/Endline/R088 已脫鉤 待討論
    /// </summary>
    public class P_Import_DQSDefect_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_DQSDefect_Detail(DateTime? sDate, DateTime? eDate)
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

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetDQSDefect_Detail_Data((DateTime)sDate, (DateTime)eDate);
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

        private Base_ViewModel GetDQSDefect_Detail_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@" 
			declare @sDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'
            declare @eDate varchar(20) = '{edate.ToString("yyyy/MM/dd")}'
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
            , [BIFactoryID] =  (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
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
                Result = this.DBProxy.Select("Production", sqlcmd, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 

				Insert Into P_DQSDefect_Detail_History
				Select Ukey, FactoryID, BIFactoryID, BIInsertDate
				FROM P_DQSDefect_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

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
                ,P.[InspectionDetailUkey]     = T.[InspectionDetailUkey]
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
                ,[BIFactoryID]
                ,[BIInsertDate]  
                ,[InspectionDetailUkey]
                from #Final_DQSDefect_Detail  t
                where not exists 
                (
					select 1 from P_DQSDefect_Detail P 
					where
					P.[FactoryID] = T.[FactoryID] AND P.[InspectionDetailUkey] = T.[InspectionDetailUkey]	
				)

                Delete p
				from P_DQSDefect_Detail p
				where not exists 
				(
					select 1 from #Final_DQSDefect_Detail t 
					where
					P.[FactoryID] = T.[FactoryID] AND P.[InspectionDetailUkey] = T.[InspectionDetailUkey]		
				)

                update b set b.TransferDate = getdate(), b.IS_Trans = 1
                from BITableInfo b
                where b.id = 'P_DQSDefect_Detail'";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_DQSDefect_Detail");
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
