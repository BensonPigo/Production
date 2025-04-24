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
    /// <summary>
    /// 此BI報表與 MES/Endline/R088 已脫鉤 待討論
    /// </summary>
    public class P_Import_DQSDefect_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_DQSDefect_Detail(DateTime? sDate)
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
                Base_ViewModel resultReport = this.GetDQSDefect_Detail_Data((DateTime)sDate);
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

        private Base_ViewModel GetDQSDefect_Detail_Data(DateTime sdate)
        {
            string sqlcmd = $@" 
			declare @sDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'
			select 
			  fac.Zone
			, [Brand] = ord.BrandID
			, [Buyer Delivery Date] = ord.BuyerDelivery
			, ins.Line
			, [Factory] = ins.FactoryID
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
			Order by Zone,[Brand],[Factory],Line,Team,[SP#],Article,[ProductType],Size,[DefectTypeID],[DefectCodeID]
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

				DELETE T FROM P_DQSDefect_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

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
                )
                select 
                  [Zone] = isnull([Zone],'')
                , Brand = isnull(Brand,'')
	            , [Buyer Delivery Date]
	            , Line = isnull(Line,'')
	            , [Factory] = isnull(Factory,'')
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
	            , [ReworkCardNo] = isnull(ReworkCardNo,''), [DefectTypeID] = isnull(DefectTypeID,''), [DefectCodeID] = isnull(DefectCodeID,''), DefectCodeLocalDesc = isnull(DefectCodeLocalDesc,''), [IsCriticalDefect] = isnull(IsCriticalDefect,'')
                ,[BIFactoryID]
                ,[BIInsertDate]   
                from #Final_DQSDefect_Detail  

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
