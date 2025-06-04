using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_ProdEffAnalysis
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_ProdEffAnalysis(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                var now = DateTime.Now;
                sDate = (now.Day < 4)
                    ? new DateTime(now.Year, now.Month, 1).AddMonths(-1)
                    : new DateTime(now.Year, now.Month, 1);
            }

            if (!eDate.HasValue)
            {
                var now = DateTime.Now;
                eDate = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
            }

            try
            {
                Base_ViewModel resultReport = this.GetProdEffAnalysis_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                if (resultReport.Result)
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_Import_ProdEffAnalysis", false);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate, DateTime edate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@SDate", sdate));
            lisSqlParameter.Add(new SqlParameter("@EDate", edate));

            using (sqlConn)
            {
                string sql = new Base().SqlBITableHistory("P_ProdEffAnalysis", "P_ProdEffAnalysis_History", "#tmpFinal", "[Month] between @SDate and @EDate", needJoin: false, needExists: false) + Environment.NewLine;
                sql += $@"
				delete t
                from P_ProdEffAnalysis t
                where  [Month] between @SDate and @EDate

                insert into P_ProdEffAnalysis(
                [Month]
                      ,[ArtworkType]
                      ,[Program]
                      ,[Style]
                      ,[FtyZone]
                      ,[FactoryID]
                      ,[Brand]
                      ,[NewCDCode]
                      ,[ProductType]
                      ,[FabricType]
                      ,[Lining]
                      ,[Gender]
                      ,[Construction]
                      ,[StyleDescription]
                      ,[Season]
                      ,[TotalQty]
                      ,[TotalCPU]
                      ,[TotalManHours]
                      ,[PPH]
                      ,[EFF]
                      ,[Remark]
                      ,[BIFactoryID]
                      ,[BIInsertDate]
                )
                select OutputDate
                      ,isnull([ArtworkType],'')
	                  ,isnull(ProgramID,'')
	                  ,isnull(StyleID,'')
	                  ,isnull(FtyZone,'')
	                  ,isnull(FactoryID,'')
	                  ,isnull(BrandID,'')
	                  ,isnull(CDCodeNew,'')
	                  ,isnull(ProductType,'')
	                  ,isnull(FabricType,'')
	                  ,isnull(Lining,'')
	                  ,isnull(Gender,'')
	                  ,isnull(Construction,'')
	                  ,isnull(StyleDesc,'')
	                  ,isnull(SeasonID,'')
	                  ,isnull([Total Qty],0)
	                  ,isnull([Total Artwork CPU],0)
	                  ,isnull([Total ManHours],'')
	                  ,isnull([PPH],'')
	                  ,isnull([EFF%],'')
	                  ,isnull([Remark],'')
                      ,isnull([BIFactoryID],'')
                      ,[BIInsertDate]
                from #tmpMain t
				 ";
                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpMain");
                sqlConn.Close();
                sqlConn.Dispose();
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetProdEffAnalysis_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @SDate date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @EDate date ='{edate.ToString("yyyy/MM/dd")}'

            -- Main data
            Select
            [RS] = iif(ProductionUnit = 'TMS', 'CPU', iif(ProductionUnit = 'QTY', 'AMT','')),
            ArtworkTypeID = IIF(at.IsTtlTMS=1 , 'SEWING', otc.ArtworkTypeID),
            at.IsTtlTMS,
            o.ID, 
            o.ProgramID,
            o.StyleID,
            o.SeasonID
            , [BrandID] = case 
		            when o.BrandID != 'SUBCON-I' then o.BrandID
		            when Order2.BrandID is not null then Order2.BrandID
		            when StyleBrand.BrandID is not null then StyleBrand.BrandID
		            else o.BrandID end
            , o.FactoryID
            ,o.POID 
            ,o.Category
            ,o.CdCodeID 
            ,[CPU] = o.CPU
            ,[ArtworkCPU] =  IIF(at.IsTtlTMS=1, sum(ROUND(iif(at.IsTtlTMS = 1, otc.Price,0),iif(at.ProductionUnit = 'QTY',4,3))) over (partition by sod.ID,sod.OrderId,sod.Article,sod.ComboType),ROUND(otc.Price,iif(at.ProductionUnit = 'QTY',4,3)))
            ,CPURate = o.CPUFactor * o.CPU  
            ,o.BuyerDelivery
            ,o.SCIDelivery
            ,so.SewingLineID 
            ,[ManPower] = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 ,so.ManPower)
            ,sod.ComboType
            ,[WorkHour] = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 ,sod.WorkHour)
            ,QAQty = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 , sod.QAQty )
            ,QARate = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 ,sod.QAQty * isnull(Production.dbo.[GetOrderLocation_Rate](o.id ,sod.ComboType)/100,1))
            ,CDDesc = s.Description 
            ,StyleDesc = s.Description
            ,s.ModularParent,
            s.CPUAdjusted
            ,OutputDate
            ,Shift
            , Team
            ,SCategory = so.Category
            ,CPUFactor = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 , o.CPUFactor)
            ,[FtyZone]=f.FtyZone
            ,orderid
            ,Rate = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 , isnull(Production.dbo.[GetOrderLocation_Rate]( o.id ,sod.ComboType)/100,1))
            ,ActManPower= iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 ,so.Manpower)
            , [MockupCPU] = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 , isnull(mo.Cpu,0))
            , [MockupCPUFactor] = iif(at.IsTtlTMS = 1 and  otc.ArtworkTypeID !='SEWING',0 ,isnull(mo.CPUFactor,0))
            into #stmp
            from Production.dbo.Orders o WITH (NOLOCK) 
            inner join Production.dbo.SewingOutput_Detail sod WITH (NOLOCK) on sod.OrderId = o.ID
            inner join Production.dbo.SewingOutput so WITH (NOLOCK) on so.ID = sod.ID and so.Shift <> 'O'  
            inner join Production.dbo.Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
            inner join Production.dbo.Factory f WITH (NOLOCK) on o.FactoryID=f.id
            inner join Production.dbo.Brand b WITH (NOLOCK) on o.BrandID=b.ID
            left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sod.OrderId
            outer apply( select BrandID from Production.dbo.orders o1 where o.CustPONo = o1.id) Order2
            outer apply( select top 1 BrandID from Production.dbo.Style where id = o.StyleID 
                and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
            inner join Production.dbo.Order_TmsCost otc on otc.id = o.ID
            inner join Production.dbo.ArtworkType at on at.ID = otc.ArtworkTypeID
            Where 1=1
            and f.IsProduceFty = '1'
            --排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
            and ((o.LocalOrder = 1 and o.SubconInType in ('1','2')) or (o.LocalOrder = 0 and o.SubconInType in ('0','3')))
            and so.OutputDate between @SDate and @EDate
             AND o.Category in ('B','S') AND f.Type <>'S' 
             and Classify in ('I','A','P') 
             and IsPrintToCMP=1

             --by Program
            select 
            a.ArtworkTypeID
            ,a.IsTtlTMS
            ,a.RS
                , a.ProgramID
                , a.StyleID
                , a.SeasonID
                , a.BrandID
                , a.FtyZone
                , a.FactoryID
                , a.POID
                , a.Category
                , a.CdCodeID 
                , sty.CDCodeNew
                , sty.ProductType
                , sty.FabricType
                , sty.Lining
                , sty.Gender
                , sty.Construction
	            ,artworkcpu
                , CPU = a.CPU
                , CPURate = (a.CPURate)
                , a.BuyerDelivery, a.SCIDelivery, a.SewingLineID , a.ComboType
                , ManPower = a.ManPower
                , WorkHour = Round(a.WorkHour,2) 
                , QARate = convert(numeric(12,2), a.QARate)
                , TotalManHour = ROUND( ActManPower * WorkHour, 2)
                , TotalCPUOut = ROUND(IIF(Category='M',MockupCPU*MockupCPUFactor, CPU*CPUFactor*Rate)*QAQty,3)
	            , TotalArtwrokCPUOut = (
		            case when Category = 'M' then round(ArtworkCPU*MockupCPUFactor * QAQty, iif([RS] = 'AMT' ,4,3))
		            when IsTtlTMS = 1 and ArtworkTypeID = 'SEWING' then Round(a.ArtworkCPU*a.CPUFactor*a.Rate * QAQty,iif([RS] = 'AMT' ,4,3))
		            else  round(ArtworkCPU*CPUFactor*Rate * QAQty, iif([RS] = 'AMT' ,4,3))
		            end
	            )
                , a.StyleDesc
                , a.ModularParent
                , CPUAdjusted = a.CPUAdjusted
                , QAQty = a.QAQty
                ,a.OutputDate
            into #tmpz
            from #stmp a
            Outer apply (
	            SELECT s.CDCodeNew
                    , ProductType = r2.Name
		            , FabricType = r1.Name
		            , s.Lining
		            , s.Gender
		            , Construction = d1.Name
	            FROM Production.dbo.Style s WITH(NOLOCK)
	            left join Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	            left join Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	            left join Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	            where s.ID = a.StyleID 
	            and s.SeasonID = a.SeasonID 
	            and s.BrandID = a.BrandID
            )sty
            where (IsTtlTMS = 0 or ArtworkTypeID ='Sewing')

 

            select StyleID,BrandID,StyleDesc,SeasonID,FactoryID,OutputDate = max(OutputDate)
            into #tmp_MaxOutputDate
            from #tmpz 
            group by StyleID,BrandID,StyleDesc,SeasonID,FactoryID

            select 
            OutputDate = EOMONTH(OutputDate),
            IsTtlTMS,
            [ArtworkType] = 'TTL '+　ArtworkTypeID　+' ('+RS+')', 
            ProgramID
                , StyleID
                , FtyZone
                , FactoryID
                , BrandID
                , CDCodeNew
                , ProductType
                , FabricType
                , Lining
                , Gender
                , Construction
                , StyleDesc
                , SeasonID
                , [Total Qty]=sum(QARate)
	            , [Total Artwork CPU] = sum(TotalArtwrokCPUOut)
	            , [Total CPU]=sum(TotalCPUOut)
                , [Total ManHours]=iif(ArtworkTypeID ='SEWING',CONVERT(Varchar , sum(TotalManHour)),'-')
                , [PPH]=iif(ArtworkTypeID ='SEWING',CONVERT(Varchar,Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)),'-')
                , [EFF%]=iif(ArtworkTypeID ='SEWING',CONVERT(Varchar,Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)),'-') 
                , [Remark]= iif(ArtworkTypeID ='SEWING',CONVERT(Varchar,case  when Max(OutputDate) is null then 'New Style'
                        else concat((Stuff((
							            select distinct concat(' ', t.SewingLineID)
							            from #tmpz t
							            where t.StyleID = #tmpz.StyleID
							            and t.BrandID = #tmpz.BrandID
							            and t.StyleDesc = #tmpz.StyleDesc
							            and t.SeasonID = #tmpz.SeasonID
							            and t.FactoryID = #tmpz.FactoryID
							            and exists (select 1 from #tmp_MaxOutputDate t2
										            where t2.StyleID = t.StyleID
										            and t2.BrandID = t.BrandID 
										            and t2.StyleDesc = t.StyleDesc
										            and t2.SeasonID = t.SeasonID
										            and t2.OutputDate = t.OutputDate
										            and t2.FactoryID = t.FactoryID)
							            FOR XML PATH('')) ,1,1,'')),'(',format(Max(OutputDate), 'yyyy/MM/dd'),')')
                        end),'-')
            into #tmp2
            from #tmpz 
            Group BY EOMONTH(OutputDate),ArtworkTypeID,IsTtlTMS, RS, ProgramID,StyleID,FtyZone,FactoryID,BrandID,CdCodeID,StyleDesc,SeasonID, CDCodeNew, ProductType, FabricType, Lining, Gender, Construction 

            select 
            * 
            , [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
            , [BIInsertDate] = GetDate()
            from 
            (
	            select * 
	            from (
	            select OutputDate
	            ,IsTtlTMS
	            ,[ArtworkType] = 'TTL AT (CPU)'
	            ,ProgramID
		            , StyleID
		            , FtyZone
		            , FactoryID
		            , BrandID
		            , CDCodeNew
		            , ProductType
		            , FabricType
		            , Lining
		            , Gender
		            , Construction  
		            , StyleDesc
		            , SeasonID
		            , [Total Qty]
		            , [Total Artwork CPU]
		            , [Total CPU]
		            , [Total ManHours]
		            , [PPH]
		            , [EFF%]
		            , [Remark] 
		            , row = ROW_NUMBER() over(partition by OutputDate ,IsTtlTMS, ProgramID,StyleID,FtyZone,FactoryID,BrandID, CDCodeNew, ProductType,FabricType, Lining, Gender, Construction, StyleDesc, SeasonID order by isnull([Total Artwork CPU],0) desc)
	            from #tmp2 
	            where [ArtworkType] like '%AT (HAND)%' or [ArtworkType] like '%AT (MACHINE)%'
	            ) a where [row] = 1

                union all

                select OutputDate ,
                IsTtlTMS,
                [ArtworkType], 
                ProgramID
                    , StyleID
                    , FtyZone
                    , FactoryID
                    , BrandID
                    , CDCodeNew
                    , ProductType
                    , FabricType
                    , Lining
                    , Gender
                    , Construction
                    , StyleDesc
                    , SeasonID
                    , [Total Qty]
	                , [Total Artwork CPU]
	                , [Total CPU]
                    , [Total ManHours]
                    , [PPH]
                    , [EFF%]
                    , [Remark] 
	                , row = iif([ArtworkType] like '%AT (HAND)%' or [ArtworkType] like '%AT (MACHINE)%', ROW_NUMBER() over(partition by OutputDate ,IsTtlTMS, ProgramID,StyleID,FtyZone,FactoryID,BrandID, CDCodeNew, ProductType,FabricType, Lining, Gender, Construction, StyleDesc, SeasonID order by isnull([Total Artwork CPU],0)),1)
                from #tmp2 
                where [ArtworkType] not like '%AT (HAND)%' 
                and [ArtworkType] not like '%AT (MACHINE)%'
            ) a

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
    }
}
