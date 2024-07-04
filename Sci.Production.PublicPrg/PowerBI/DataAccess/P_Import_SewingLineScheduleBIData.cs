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
        public Base_ViewModel P_SewingLineScheduleBIData(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R01 biModel = new PPIC_R01();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/01"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                PPIC_R01_ViewModel pIC_R01_ViewModel = new PPIC_R01_ViewModel()
                {
                    Inline = sDate,
                    Offline = eDate,
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

                DataTable detailTable = resultReport.DtArr[0];

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value);
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };
                string sql = @"	
delete t
from P_SewingLineSchedule t
where exists (select 1 from #tmp s where t.APSNo = s.APSNo 
										AND t.SewingDay = s.SewingDay 
										AND t.SewingLineID = s.SewingLineID  
										AND t.Sewer = s.Sewer 
										AND t.FactoryID = s.FactoryID)
and ((t.AddDate >= @SDate and  t.AddDate <= @EDate)
	or (t.EditDate >= @SDate and t.EditDate <= @EDate)
	or (t.SewingOffline >= @SDate and t.SewingOffline <= @EDate)
)
and exists (select 1 from P_SewingLineSchedule s where t.APSNo = s.APSNo 
										AND t.SewingDay = s.SewingDay 
										AND t.SewingLineID = s.SewingLineID  
										AND t.Sewer = s.Sewer 
										AND t.FactoryID = s.FactoryID
			group by s.APSNo, s.SewingDay, s.SewingLineID, s.Sewer, s.FactoryID
			having count(*) > 1	 
)

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
		t.LastDownloadAPSDate = s.LastDownloadAPSDate
from P_SewingLineSchedule t 
inner join #tmp s on t.APSNo = s.APSNo 
				AND t.SewingDay = s.SewingDay 
				AND t.SewingLineID = s.SewingLineID  
				AND t.Sewer = s.Sewer 
				AND t.FactoryID = s.FactoryID


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
, [PrintPcs], [InlineCategory], [StyleSeason], [AddDate], [EditDate],[LastDownloadAPSDate])
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
	s.PrintPcs, s.InlineCategory, s.StyleSeason, s.AddDate, s.EditDate, s.LastDownloadAPSDate
from #tmp s
where not exists (select 1 from P_SewingLineSchedule t WITH(NOLOCK) where t.APSNo = s.APSNo 
														AND t.SewingDay = s.SewingDay 
														AND t.SewingLineID = s.SewingLineID  
														AND t.Sewer = s.Sewer 
														AND t.FactoryID = s.FactoryID)

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

update b
	set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = 'P_SewingLineSchedule'
";
                finalResult = new Base_ViewModel()
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
