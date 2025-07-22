
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
    public class P_Import_SewingLineScheduleBIData
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SewingLineScheduleBIData(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R01 biModel = new PPIC_R01();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/01"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                PPIC_R01_ViewModel pIC_R01_ViewModel = new PPIC_R01_ViewModel()
                {
                    Inline = item.SDate,
                    Offline = item.EDate,
                    Line1 = string.Empty,
                    Line2 = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    BuyerDelivery1 = null,
                    BuyerDelivery2 = null,
                    SciDelivery1 = null,
                    SciDelivery2 = null,
                    Brand = string.Empty,
                    Subprocess = string.Empty,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetSewingLineScheduleData(pIC_R01_ViewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.DtArr[0], item);
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                string sql = @"	
				select s.*
				into #Update
				from P_SewingLineSchedule t 
				inner join #tmp s on t.APSNo = s.APSNo 
								AND t.SewingDay = s.SewingDay 
								AND t.SewingLineID = s.SewingLineID  
								AND t.Sewer = s.Sewer 
								AND t.FactoryID = s.FactoryID
				where isnull(t.SewingStartTime,'1900/01/01') <>  isnull(s.SewingStartTime,'1900/01/01')
				   or isnull(t.SewingEndTime,'1900/01/01') <>  isnull(s.SewingEndTime,'1900/01/01')
				   or isnull(t.MDivisionID,'') <>  isnull(s.MDivisionID,'')
				   or isnull(t.PO,'') <>  isnull(s.PO,'')
				   or isnull(t.POCount,0) <>  isnull(s.POCount,0)
				   or isnull(t.SP,'') <>  isnull(s.SP,'')
				   or isnull(t.SPCount,0) <>  isnull(s.SPCount,0)
				   or isnull(t.EarliestSCIdelivery,'1900/01/01') <>  isnull(s.EarliestSCIdelivery,'1900/01/01')
				   or isnull(t.LatestSCIdelivery,'1900/01/01') <>  isnull(s.LatestSCIdelivery,'1900/01/01')
				   or isnull(t.EarliestBuyerdelivery,'1900/01/01') <>  isnull(s.EarliestBuyerdelivery,'1900/01/01')
				   or isnull(t.LatestBuyerdelivery,'1900/01/01') <>  isnull(s.LatestBuyerdelivery,'1900/01/01') 
				   or isnull(t.Category,'') <>  isnull(s.Category,'')
				   or isnull(t.Colorway,'') <>  isnull(s.Colorway,'')
				   or isnull(t.ColorwayCount,0) <>  isnull(s.ColorwayCount,0)
				   or isnull(t.CDCode,'') <>  isnull(s.CDCode,'')
				   or isnull(t.ProductionFamilyID,'') <>  isnull(s.ProductionFamilyID,'')
				   or isnull(t.Style,'') <>  isnull(s.Style,'')
				   or isnull(t.StyleCount,0) <>  isnull(s.StyleCount,0)
				   or isnull(t.OrderQty,0) <>  isnull(s.OrderQty,0)
				   or isnull(t.AlloQty,0) <>  isnull(s.AlloQty,0)
				   or isnull(t.StardardOutputPerDay,0) <>  isnull(s.StardardOutputPerDay,0)
				   or isnull(t.CPU,0) <>  isnull(s.CPU,0)
				   or isnull(t.WorkHourPerDay,0) <>  isnull(s.New_WorkHourPerDay,0)
				   or isnull(t.StardardOutputPerHour,0) <>  isnull(s.StardardOutputPerHour,0)
				   or isnull(t.Efficienycy,0) <>  isnull(s.Efficienycy,0)
				   or isnull(t.ScheduleEfficiency,0) <>  isnull(s.ScheduleEfficiency,0)
				   or isnull(t.LineEfficiency,0) <>  isnull(s.LineEfficiency,0)
				   or isnull(t.LearningCurve,0) <>  isnull(s.LearningCurve,0)
				   or isnull(t.SewingInline,'1900/01/01') <>  isnull(s.SewingInline,'1900/01/01')
				   or isnull(t.SewingOffline,'1900/01/01') <>  isnull(s.SewingOffline,'1900/01/01')
				   or isnull(t.PFRemark,'') <>  isnull(s.PFRemark,'')
				   or isnull(t.MTLComplete,'') <>  isnull(s.MTLComplete,'')
				   or isnull(t.KPILETA,'1900/01/01') <>  isnull(s.KPILETA,'1900/01/01')
				   or isnull(t.MTLETA,'1900/01/01') <>  isnull(s.MTLETA,'1900/01/01')
				   or isnull(t.ArtworkType,'') <>  isnull(s.ArtworkType,'')
				   or isnull(t.InspectionDate,'1900/01/01') <>  isnull(s.InspectionDate,'1900/01/01')
				   or isnull(t.Remarks,'') <>  isnull(s.Remarks,'')
				   or isnull(t.CuttingOutput,0) <>  isnull(s.CuttingOutput,0)
				   or isnull(t.SewingOutput,0) <>  isnull(s.SewingOutput,0)
				   or isnull(t.ScannedQty,0) <>  isnull(s.ScannedQty,0)
				   or isnull(t.ClogQty,0) <>  isnull(s.ClogQty,0)
				   or isnull(t.SewingCPU,0) <>  isnull(s.SewingCPU,0)
				   or isnull(t.BrandID,'') <>  isnull(s.BrandID,'')
				   or isnull(t.Orig_WorkHourPerDay,0) <>  isnull(s.Orig_WorkHourPerDay,0)
				   or isnull(t.New_SwitchTime,0) <>  isnull(s.New_SwitchTime,0)
				   or isnull(t.FirststCuttingOutputDate,'1900/01/01') <>  isnull(s.FirststCuttingOutputDate,'1900/01/01')
				   or isnull(t.CDCodeNew,'') <> isnull(s.CDCodeNew,'')
				   or isnull(t.ProductType,'') <> isnull(s.ProductType,'')
				   or isnull(t.FabricType,'') <> isnull(s.FabricType,'')
				   or isnull(t.Lining,'') <> isnull(s.Lining,'')
				   or isnull(t.Gender,'') <> isnull(s.Gender,'') 
				   or isnull(t.Construction,'') <> isnull(s.Construction,'')
				   or isnull(t.[TTL_PRINTING (PCS)],0) <> isnull(s.[TTL_PRINTING (PCS)],0)
				   or isnull(t.[TTL_PRINTING PPU (PPU)],0) <> isnull(s.[TTL_PRINTING PPU (PPU)],0)
				   or isnull(t.SubCon,'') <> isnull(s.SubCon,'')
				   or isnull(t.[Subcon Qty],0) <> isnull(s.[Subcon Qty],0)
				   or isnull(t.[Std Qty for printing],0) <> isnull(s.[Std Qty for printing],0)
				   or isnull(t.StyleName,'') <> isnull(s.StyleName,'')
				   or isnull(t.StdQtyEMB,'') <> isnull(s.StdQtyEMB,'')
				   or isnull(t.EMBStitch,'') <> isnull(s.EMBStitch,'')
				   or isnull(t.EMBStitchCnt,0) <> isnull(s.EMBStitchCnt,0)
				   or isnull(t.TtlQtyEMB,0) <> isnull(s.TtlQtyEMB,0)
				   or isnull(t.PrintPcs,0) <> isnull(s.PrintPcs,0)
				   or isnull(t.InlineCategory,'') <> isnull(s.InlineCategory,'')
				   or isnull(t.StyleSeason,'') <> isnull(s.StyleSeason,'')
				   or isnull(t.AddDate,'1900/01/01') <> isnull(s.AddDate,'1900/01/01')
				   or isnull(t.EditDate,'1900/01/01') <> isnull(s.EditDate,'1900/01/01')
				   or isnull(t.SewingInlineCategory,'') <> isnull(s.SewingInlineCategory,'')

				UPDATE t 
					SET t.SewingStartTime =  s.SewingStartTime,
						t.SewingEndTime =  s.SewingEndTime,
						t.MDivisionID =  s.MDivisionID,
						t.PO =  s.PO,
						t.POCount =  s.POCount,
						t.SP =  s.SP,
						t.SPCount =  s.SPCount,
						t.EarliestSCIdelivery =  s.EarliestSCIdelivery,
						t.LatestSCIdelivery =  s.LatestSCIdelivery,
						t.EarliestBuyerdelivery =  s.EarliestBuyerdelivery,
						t.LatestBuyerdelivery =  s.LatestBuyerdelivery,
						t.Category =  s.Category,
						t.Colorway =  s.Colorway,
						t.ColorwayCount =  s.ColorwayCount,
						t.CDCode =  s.CDCode,
						t.ProductionFamilyID =  s.ProductionFamilyID,
						t.Style =  s.Style,
						t.StyleCount =  s.StyleCount,
						t.OrderQty =  s.OrderQty,
						t.AlloQty =  s.AlloQty,
						t.StardardOutputPerDay =  s.StardardOutputPerDay,
						t.CPU =  s.CPU,
						t.WorkHourPerDay =  s.New_WorkHourPerDay,
						t.StardardOutputPerHour =  s.StardardOutputPerHour,
						t.Efficienycy =  s.Efficienycy,
						t.ScheduleEfficiency =  s.ScheduleEfficiency,
						t.LineEfficiency =  s.LineEfficiency,
						t.LearningCurve =  s.LearningCurve,
						t.SewingInline =  s.SewingInline,
						t.SewingOffline =  s.SewingOffline,
						t.PFRemark =  s.PFRemark,
						t.MTLComplete =  s.MTLComplete,
						t.KPILETA =  s.KPILETA,
						t.MTLETA =  s.MTLETA,
						t.ArtworkType =  s.ArtworkType,
						t.InspectionDate =  s.InspectionDate,
						t.Remarks =  s.Remarks,
						t.CuttingOutput =  s.CuttingOutput,
						t.SewingOutput =  s.SewingOutput,
						t.ScannedQty =  s.ScannedQty,
						t.ClogQty =  s.ClogQty,
						t.SewingCPU =  s.SewingCPU,
						t.BrandID =  s.BrandID,
						t.Orig_WorkHourPerDay =  s.Orig_WorkHourPerDay,
						t.New_SwitchTime =  s.New_SwitchTime,
						t.FirststCuttingOutputDate =  s.FirststCuttingOutputDate,
						t.CDCodeNew = s.CDCodeNew,
						t.ProductType = s.ProductType,
						t.FabricType = s.FabricType,
						t.Lining = s.Lining,
						t.Gender = s.Gender,
						t.Construction = s.Construction,
						t.[TTL_PRINTING (PCS)] = s.[TTL_PRINTING (PCS)],
						t.[TTL_PRINTING PPU (PPU)] = s.[TTL_PRINTING PPU (PPU)],
						t.SubCon = s.SubCon,
						t.[Subcon Qty] = s.[Subcon Qty],
						t.[Std Qty for printing] = s.[Std Qty for printing],
						t.StyleName = s.StyleName,
						t.StdQtyEMB = s.StdQtyEMB,
						t.EMBStitch = s.EMBStitch,
						t.EMBStitchCnt = s.EMBStitchCnt,
						t.TtlQtyEMB = s.TtlQtyEMB,
						t.PrintPcs = s.PrintPcs,
						t.InlineCategory = s.InlineCategory,
						t.StyleSeason = s.StyleSeason,
						t.AddDate = s.AddDate,
						t.EditDate = s.EditDate,
						t.LastDownloadAPSDate = iif(u.LastDownloadAPSDate is not null, u.LastDownloadAPSDate, t.LastDownloadAPSDate),
						t.SewingInlineCategory = s.SewingInlineCategory
						,t.[BIFactoryID] = @BIFactoryID
						,t.[BIInsertDate] = GetDate()
						,t.[BIStatus] = 'New'
				from P_SewingLineSchedule t 
				inner join #tmp s on t.APSNo = s.APSNo 
								AND t.SewingDay = s.SewingDay 
								AND t.SewingLineID = s.SewingLineID  
								AND t.Sewer = s.Sewer 
								AND t.FactoryID = s.FactoryID
				Left join #Update u on t.APSNo = u.APSNo 
								AND t.SewingDay = u.SewingDay 
								AND t.SewingLineID = u.SewingLineID  
								AND t.Sewer = u.Sewer 
								AND t.FactoryID = u.FactoryID
				
				insert into P_SewingLineSchedule ([APSNo], [SewingLineID], [SewingDay], [SewingStartTime], [SewingEndTime]
				, [MDivisionID], [FactoryID], [PO], [POCount], [SP], [SPCount], [EarliestSCIdelivery], [LatestSCIdelivery]
				, [EarliestBuyerdelivery], [LatestBuyerdelivery], [Category], [Colorway], [ColorwayCount], [CDCode]
				, [ProductionFamilyID], [Style], [StyleCount], [OrderQty], [AlloQty], [StardardOutputPerDay], [CPU]
				, [WorkHourPerDay], [StardardOutputPerHour], [Efficienycy], [ScheduleEfficiency], [LineEfficiency]
				, [LearningCurve], [SewingInline], [SewingOffline], [PFRemark], [MTLComplete], [KPILETA], [MTLETA]
				, [ArtworkType], [InspectionDate], [Remarks], [CuttingOutput], [SewingOutput], [ScannedQty], [ClogQty], [Sewer]
				, [SewingCPU], [BrandID], [Orig_WorkHourPerDay], [New_SwitchTime], [FirststCuttingOutputDate], [TTL_PRINTING (PCS)]
				, [TTL_PRINTING PPU (PPU)], [SubCon], [CDCodeNew], [ProductType], [FabricType], [Lining], [Gender], [Construction]
				, [Subcon Qty], [Std Qty for printing], [StyleName], [StdQtyEMB], [EMBStitch], [EMBStitchCnt], [TtlQtyEMB]
				, [PrintPcs], [InlineCategory], [StyleSeason], [AddDate], [EditDate],[LastDownloadAPSDate],[SewingInlineCategory],[BIFactoryID],[BIInsertDate], [BIStatus])
				select 	s.APSNo, s.SewingLineID, s.SewingDay, s.SewingStartTime, s.SewingEndTime,
					s.MDivisionID, s.FactoryID, s.PO, s.POCount, s.SP, s.SPCount, s.EarliestSCIdelivery, s.LatestSCIdelivery,
					s.EarliestBuyerdelivery, s.LatestBuyerdelivery, s.Category, s.Colorway, s.ColorwayCount, s.CDCode,
					s.ProductionFamilyID, s.Style, s.StyleCount, s.OrderQty, s.AlloQty, s.StardardOutputPerDay, s.CPU,
					s.New_WorkHourPerDay, s.StardardOutputPerHour, s.Efficienycy, s.ScheduleEfficiency, s.LineEfficiency,
					s.LearningCurve, s.SewingInline, s.SewingOffline, s.PFRemark, s.MTLComplete, s.KPILETA, s.MTLETA,
					s.ArtworkType, s.InspectionDate, s.Remarks, s.CuttingOutput, s.SewingOutput, s.ScannedQty, s.ClogQty, s.Sewer,
					s.SewingCPU, s.BrandID, s.Orig_WorkHourPerDay, s.New_SwitchTime, s.FirststCuttingOutputDate, s.[TTL_PRINTING (PCS)],
					s.[TTL_PRINTING PPU (PPU)], s.SubCon, s.CDCodeNew, s.ProductType, s.FabricType, s.Lining, s.Gender, s.Construction,
					s.[Subcon Qty],	s.[Std Qty for printing], s.StyleName, s.StdQtyEMB, s.EMBStitch, s.EMBStitchCnt, s.TtlQtyEMB,
					s.PrintPcs, s.InlineCategory, s.StyleSeason, s.AddDate, s.EditDate, s.LastDownloadAPSDate, s.SewingInlineCategory,@BIFactoryID,GetDate(),'New'
				from #tmp s
				where not exists (select 1 from P_SewingLineSchedule t WITH(NOLOCK) where t.APSNo = s.APSNo 
																		AND t.SewingDay = s.SewingDay 
																		AND t.SewingLineID = s.SewingLineID  
																		AND t.Sewer = s.Sewer 
																		AND t.FactoryID = s.FactoryID)

				if @IsTrans = 1
				begin
					INSERT INTO P_SewingLineSchedule_History([FactoryID], [Ukey], BIFactoryID, BIInsertDate)   
					SELECT t.[FactoryID], t.[Ukey], t.BIFactoryID, GETDATE()  
					from P_SewingLineSchedule t
					where not exists (select 1 from #tmp s where t.APSNo = s.APSNo 
															AND t.SewingDay = s.SewingDay 
															AND t.SewingLineID = s.SewingLineID  
															AND t.Sewer = s.Sewer 
															AND t.FactoryID = s.FactoryID)
					and ((t.AddDate >= @SDate and  t.AddDate <= @EDate)
						or (t.EditDate >= @SDate and t.EditDate <= @EDate)
						or (t.SewingOffline >= @SDate and t.SewingOffline <= @EDate)
					)
				end

                delete t
				from P_SewingLineSchedule t
				where not exists (select 1 from #tmp s where t.APSNo = s.APSNo 
														AND t.SewingDay = s.SewingDay 
														AND t.SewingLineID = s.SewingLineID  
														AND t.Sewer = s.Sewer 
														AND t.FactoryID = s.FactoryID)
				and ((t.AddDate >= @SDate and  t.AddDate <= @EDate)
					or (t.EditDate >= @SDate and t.EditDate <= @EDate)
					or (t.SewingOffline >= @SDate and t.SewingOffline <= @EDate)
				)

				--刪除重複資料,只留最新資料
				SELECT  t.ukey,
						ROW_NUMBER() OVER(PARTITION BY APSNo, SewingDay, SewingLineID, Sewer, FactoryID ORDER BY EditDate Desc, AddDate Desc) AS rn
					into #Del
					from P_SewingLineSchedule t
				where exists(select 1 from #tmp s where t.APSNo = s.APSNo 
														AND t.SewingDay = s.SewingDay
														AND t.SewingLineID = s.SewingLineID
														AND t.Sewer = s.Sewer
														AND t.FactoryID = s.FactoryID)
				and((t.AddDate >= @SDate and  t.AddDate <= @EDate)
					or(t.EditDate >= @SDate and t.EditDate <= @EDate)
					or(t.SewingOffline >= @SDate and t.SewingOffline <= @EDate))

				if @IsTrans = 1
				begin
					INSERT INTO P_SewingLineSchedule_History([FactoryID], [Ukey], BIFactoryID, BIInsertDate)   
					SELECT p.[FactoryID], p.[Ukey], p.BIFactoryID, GETDATE()  
					FROM P_SewingLineSchedule p 
					WHERE ukey IN(SELECT ukey FROM #Del WHERE rn > 1)
				end

				DELETE FROM P_SewingLineSchedule
				WHERE ukey IN(SELECT ukey FROM #Del WHERE rn > 1)";

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
