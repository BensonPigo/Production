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
    public class P_Import_AccessoryInspLabStatus
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_AccessoryInspLabStatus(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddMonths(-90);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetAccessoryInspLabStatus_Data((DateTime)sDate, (DateTime)eDate);
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
            string where = @" 	 ((p.AddDate >=  @SDate and p.AddDate <= @EDate) or (p.EditDate >=  @SDate and p.EditDate <=  @EDate))";
            string tmp = new Base().SqlBITableHistory("P_AccessoryInspLabStatus", "P_AccessoryInspLabStatus_History", "#tmp", where, false, true);

            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@SDate", sdate));
            lisSqlParameter.Add(new SqlParameter("@EDate", edate));

            using (sqlConn)
            {
                string sql = $@" 
				select  POID, SEQ, ReceivingID ,Ctn=COUNT(1)
				into #DuplicateData
				from #tmp
				GROUP BY POID, SEQ, ReceivingID
				HAVING COUNT(1) > 1

				SELECT POID, SEQ, ReceivingID , LastDate = MAX(iSNULL(EditDate,AddDate))
				INTO #LastUpdateData
				FROM #tmp t
				where exists(
					select 1 from #DuplicateData d
					where t.POID=d.POID and t.SEQ=d.SEQ and t.ReceivingID=d.ReceivingID
				)
				GROUP BY POID, SEQ, ReceivingID

				insert into P_AccessoryInspLabStatus
				(
					[POID], 
					[SEQ], 
					[Factory], 
					[BrandID], 
					[StyleID], 
					[SeasonID], 
					[ShipModeID], 
					[Wkno], 
					[Invo], 
					[ArriveWHDate], 
					[ArriveQty], 
					[Inventory], 
					[Bulk], 
					[BalanceQty], 
					[EarliestSCIDelivery], 
					[BuyerDelivery], 
					[RefNo], 
					[Article], 
					[MaterialType], 
					[Color], 
					[ColorName], 
					[Size], 
					[Unit], 
					[Supplier], 
					[OrderQty], 
					[InspectionResult], 
					[InspectedQty], 
					[RejectedQty], 
					[DefectType], 
					[InspectionDate], 
					[Inspector], 
					[Remark], 
					[NALaboratory], 
					[LaboratoryOverallResult], 
					[NAOvenTest], 
					[OvenTestResult], 
					[OvenScale], 
					[OvenTestDate], 
					[NAWashTest], 
					[WashTestResult], 
					[WashScale], 
					[WashTestDate], 
					[ReceivingID], 
					[AddDate], 
					[EditDate], 
					[CategoryType],
					[BIFactoryID],	
					[BIInsertDate]
				)
				select	
				t.[POID], 
				t.[SEQ], 
				t.[FactoryID], 
				t.[BrandID], 
				t.[StyleID], 
				t.[SeasonID], 
				t.[ShipModeID], 
				t.[ExportId], 
				t.[InvNo], 
				t.[WhseArrival], 
				t.[StockQty], 
				t.[InvStock], 
				t.[BulkStock], 
				t.[BalanceQty], 
				t.[MinSciDelivery], 
				t.[MinBuyerDelivery], 
				t.[Refno], 
				t.[Article], 
				t.[MaterialType], 
				t.[Color], 
				t.[ColorName], 
				t.[SizeSpec], 
				t.stockunit, 
				t.[Supplier], 
				t.[OrderQty], 
				t.[Result], 
				t.[InspectedQty], 
				t.[RejectedQty], 
				t.[DefectType], 
				t.[InspectionDate], 
				t.[Inspector], 
				t.[Remark], 
				t.[OvenEncode], 
				t.[LaboratoryOverallResult], 
				t.[NonOven], 
				t.[Oven], 
				t.[OvenScale], 
				t.[OvenDate], 
				t.[NonWash], 
				t.[Wash], 
				t.[WashScale], 
				t.[WashDate], 
				t.[ReceivingID], 
				t.[AddDate], 
				t.[EditDate],
				t.[CategoryType],
				t.[BIFactoryID],
				t.[BIInsertDate]
				from #tmp t
				where   not exists (select 1 from P_AccessoryInspLabStatus p where p.POID = t.POID and p.SEQ = t.SEQ and p.ReceivingID = t.ReceivingID)
				and ( 
					not exists( ----PKey無重複資料
						select * from #DuplicateData a
						where a.POID=t.POID and a.SEQ=t.SEQ and a.ReceivingID=t.ReceivingID
					)
					or
					(----PKey有重複資料，抓最後更新的PKey
						exists(
							select * from #DuplicateData b
							where b.POID=t.POID and b.SEQ=t.SEQ and b.ReceivingID=t.ReceivingID
						)
						and 
						exists(
							select * from #LastUpdateData a
							where a.POID=t.POID and a.SEQ=t.SEQ and a.ReceivingID=t.ReceivingID
							and a.LastDate = iSNULL(t.EditDate,t.AddDate)
						)
					)
				)


				 update p 
				 set p.[Factory]					 = t.FactoryID
					, p.[BrandID]  				 = t.BrandID
					, p.[StyleID]  				 = t.StyleID
					, p.[SeasonID]  				 = t.SeasonID
					, p.[ShipModeID]  			 = t.ShipModeID
					, p.[Wkno]  					 = t.ExportId	
					, p.[Invo]  					 = t.InvNo
					, p.[ArriveWHDate]  			 = t.WhseArrival
					, p.[ArriveQty]  				 = t.StockQty
					, p.[Inventory]  				 = t.InvStock
					, p.[Bulk]  					 = t.BulkStock
					, p.[BalanceQty]  			 = t.BalanceQty
					, p.[EarliestSCIDelivery]  	 = t.[MinSciDelivery]
					, p.[BuyerDelivery]  			 = t.[MinBuyerDelivery]
					, p.[RefNo]  					 = t.[Refno]
					, p.[Article]  				 = t.[Article]
					, p.[MaterialType]  			 = t.[MaterialType]
					, p.[Color]  					 = t.[Color]
					, p.[ColorName]  				 = t.[ColorName]
					, p.[Size]  					 = t.[SizeSpec]
					, p.[Unit]  					 = t.stockunit
					, p.[Supplier]  				 = t.[Supplier]
					, p.[OrderQty]  				 = t.[OrderQty]
					, p.[InspectionResult]  		 = t.Result
					, p.[InspectedQty]  			 = t.[InspectedQty]
					, p.[RejectedQty]  			 = t.[RejectedQty]
					, p.[DefectType]  			 = t.[DefectType]
					, p.[InspectionDate]  		 = t.[InspectionDate]
					, p.[Inspector]  				 = t.[Inspector]
					, p.[Remark]  				 = t.Remark
					, p.[NALaboratory]  			 = t.[OvenEncode]
					, p.[LaboratoryOverallResult]	 = t.[LaboratoryOverallResult]
					, p.[NAOvenTest]  			 = t.[NonOven]
					, p.[OvenTestResult]  		 = t.[Oven]
					, p.[OvenScale]  				 = t.[OvenScale]
					, p.[OvenTestDate]  			 = t.[OvenDate]
					, p.[NAWashTest]  			 = t.[NonWash]
					, p.[WashTestResult]  		 = t.[Wash]
					, p.[WashScale]  				 = t.[WashScale]
					, p.[WashTestDate]  			 = t.[WashDate]
					, p.[AddDate]  				 = t.AddDate
					, p.[EditDate]  				 = t.EditDate 
					, p.[CategoryType]  				 = t.CategoryType 
					, p.[BIFactoryID]  				 = t.BIFactoryID
					, p.[BIInsertDate]  			 = t.BIInsertDate	
				 from P_AccessoryInspLabStatus p
				 inner join  #tmp t on p.POID = t.POID and p.SEQ = t.SEQ and p.ReceivingID = t.ReceivingID

{tmp}

				 delete p
				 from P_AccessoryInspLabStatus p
				 where not exists (select 1 from #tmp t where p.POID = t.POID and p.SEQ = t.SEQ and p.ReceivingID = t.ReceivingID)
				 and ((p.AddDate >=  @SDate and p.AddDate <= @EDate) 
					or (p.EditDate >=  @SDate and p.EditDate <=  @EDate))

				if exists (select 1 from BITableInfo b where b.id = 'P_AccessoryInspLabStatus')
				begin
					update b
						set b.TransferDate = getdate()
					from BITableInfo b
					where b.id = 'P_AccessoryInspLabStatus'
				end
				else 
				begin
					insert into BITableInfo(Id, TransferDate)
					values('P_AccessoryInspLabStatus', getdate())
				end
                ";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmp");
                sqlConn.Close();
                sqlConn.Dispose();
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetAccessoryInspLabStatus_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @SDate date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @EDate date ='{edate.ToString("yyyy/MM/dd")}'

			select a.POID
		    , [SEQ] = concat(a.SEQ1, '-', a.SEQ2)
		    , [FactoryID] = x.FactoryID
		    , [BrandID] = x.BrandID
		    , [StyleID] = x.StyleID
		    , [SeasonID] = x.SeasonID
		    , [ShipModeID] = ISNULL(et.ShipModeID, '')
		    , [ExportId] = ISNULL(t.ExportId, '')
		    , [InvNo] = ISNULL(t.InvNo, '')
		    , [WhseArrival] = ISNULL(t.WhseArrival, '')
		    , [StockQty] = isnull(t.StockQty, 0)
		    , [InvStock] = total_t.InvStock
		    , [BulkStock] = total_t.BulkStock
		    , [BalanceQty] = total_t.BalanceQty
		    , [MinSciDelivery] = g.MinSciDelivery
		    , [MinBuyerDelivery] = g.MinBuyerDelivery
		    , [Refno] = a.Refno
		    , [Article] = isnull(Style.Article, '')
		    , [MaterialType] = isnull(fabric.MtlTypeID, '')
		    , [Color]  = isnull(psdsC.SpecValue, '')
		    , [ColorName] = isnull(c.Name, '')
		    , [SizeSpec] = isnull(psdsS.SpecValue, '')
		    , [stockunit] = PS.stockunit
		    , [Supplier] = concat(P.SuppID, '-', s.AbbEN)
		    , [OrderQty] = Round(Production.dbo.getUnitQty(PS.POUnit, PS.StockUnit, isnull(PS.Qty, 0)), 2)
		    , [Result] = a.Result
		    , [InspectedQty] = IIF(a.Status = 'Confirmed',a.InspQty, 0)
		    , [RejectedQty] = IIF(a.Status = 'Confirmed',a.RejectQty, 0)
		    , [DefectType] = ISNULL(IIF(a.Status = 'Confirmed', DefectText.Val , ''), '')
		    , [InspectionDate] = IIF(a.Status = 'Confirmed', a.InspDate,NULL)
		    , [Inspector] = ISNULL((select Pass1.Name from Production.dbo.Pass1 WITH (NOLOCK) where a.Inspector = pass1.id), '')
		    , [Remark] = a.Remark
		    , [OvenEncode] = iif(AIRL.NonOven = 1 and AIRL.NonWash =1, 'Y', '')
		    , [LaboratoryOverallResult] = isnull(AIRL.Result, '')
		    , [NonOven] = iif(AIRL.NonOven = 1, 'Y', '')
		    , [Oven] = iif(AIRL.OvenEncode = 1, AIRL.Oven, '')
		    , [OvenScale] = iif(AIRL.OvenEncode = 1, AIRL.OvenScale, '') 
		    , [OvenDate] = iif(AIRL.OvenEncode = 1, AIRL.OvenDate, null)
		    , [NonWash] = iif(AIRL.NonWash = 1, 'Y', '')
		    , [Wash] = iif(AIRL.WashEncode = 1, AIRL.Wash, '') 
		    , [WashScale] = iif(AIRL.WashEncode = 1, AIRL.WashScale, '')
		    , [WashDate] = iif(AIRL.WashEncode = 1, AIRL.WashDate, null)
		    , [ReceivingID] = a.ReceivingID
		    , [AddDate] = a.AddDate
		    , [EditDate] = a.EditDate
		    , [CategoryType] = isnull(MtlType.CategoryType, '')
			, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
            , [BIInsertDate] = GETDATE()   
	        from Production.dbo.AIR a WITH (NOLOCK) 
	        inner join Production.dbo.View_AllReceivingDetail t WITH (NOLOCK) on t.PoId = A.POID and t.Seq1 = A.SEQ1 and t.Seq2 = A.SEQ2 AND t.ID = a.ReceivingID
	        cross apply(
		        select [BalanceQty] = sum(fit.inqty - fit.outqty + fit.adjustqty - fit.ReturnQty)
		        ,[InvStock] =  sum(iif(rd.StockType = 'I',RD.StockQty, 0))
		        ,[BulkStock] = sum(iif(rd.StockType = 'B',RD.StockQty, 0))
		        from Production.dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
		        left join Production.dbo.FtyInventory fit WITH (NOLOCK)  on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
		        where rd.PoId = A.POID and rd.Seq1 = A.SEQ1 and rd.Seq2 = A.SEQ2 AND rd.Id=A.ReceivingID
	        ) total_t
	        left join Production.dbo.Export et with (nolock) on et.ID = t.ExportId
	        inner join (
			        select distinct id,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,O.StyleUkey
			        from Production.dbo.Orders o WITH (NOLOCK)  
			        where O.Category in ('B','S','M','T','A')
	        ) x on x. id = A.POID 
			inner join Production.dbo.PO_Supp P WITH (NOLOCK) on P.id = A.POID and P.SEQ1 = A.SEQ1 
			inner join Production.dbo.PO_Supp_Detail PS WITH (NOLOCK) on PS.ID = A.POID and PS.SEQ1 = A.SEQ1 and PS.SEQ2 = A.SEQ2
			inner join Production.dbo.supp s WITH (NOLOCK) on s.id = P.SuppID
			left join Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = PS.id and psdsC.seq1 = PS.seq1 and psdsC.seq2 = PS.seq2 and psdsC.SpecColumnID = 'Color'
			left join Production.dbo.PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = PS.id and psdsS.seq1 = PS.seq1 and psdsS.seq2 = PS.seq2 and psdsS.SpecColumnID = 'Size'
			left join Production.dbo.Color C WITH (NOLOCK) on C.ID = isnull(psdsC.SpecValue ,'') and C.BrandId = x.BrandId
			left join Production.dbo.fabric WITH (NOLOCK) on fabric.SCIRefno = PS.scirefno
			left join Production.dbo.MtlType WITH (NOLOCK) on MtlType.ID = fabric.MtlTypeID
			left join Production.dbo.AIR_Laboratory AIRL WITH (NOLOCK) on AIRL.ID = A.ID     
			OUTER APPLY (
				SELECT  [Val]=  STUFF((
				SELECT distinct concat(',', IIF(a.Defect = '' , '' , concat(ori.Data, '-', ISNULL(ad.Description, ''))))
				FROM Production.dbo.[SplitString](a.Defect,'+') ori
				LEFT JOIN Production.dbo.AccessoryDefect ad  WITH (NOLOCK) on ad.id = ori.Data
					FOR XML PATH('')
					),1,1,'')
			)DefectText
			OUTER APPLY (
				select [Article] = Stuff((
					select distinct concat( ',', tcd.Article) 
					from Production.dbo.Style s WITH (NOLOCK)
					Inner Join Production.dbo.Style_ThreadColorCombo as tc WITH (NOLOCK) On tc.StyleUkey = s.Ukey
					Inner Join Production.dbo.Style_ThreadColorCombo_Detail as tcd WITH (NOLOCK) On tcd.Style_ThreadColorComboUkey = tc.Ukey
					where s.Ukey = x.StyleUkey
					and tcd.SuppId = P.SuppId
					and tcd.SCIRefNo = PS.SCIRefNo
					and tcd.ColorID = isnull(psdsC.SpecValue ,'')
					and PS.SEQ1 like 'T%'
					and exists (select 1 from Production.dbo.MDivisionPoDetail m WITH (NOLOCK) where m.POID = x.ID)
				FOR XML PATH('')),1,1,'') 
			)Style
			OUTER APPLY (
				Select * 
				from Production.dbo.GetSCI(A.poid,x.Category)
			)g
			where ((a.AddDate >= @SDate and a.AddDate <= @EDate) 
			or (a.EditDate >= @SDate and a.EditDate <= @EDate))
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
