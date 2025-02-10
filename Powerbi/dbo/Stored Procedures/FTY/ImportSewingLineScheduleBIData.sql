
CREATE PROCEDURE [dbo].[ImportSewingLineScheduleBIData]

@StartDate Date,
@EndDate Date

AS
BEGIN

	SET NOCOUNT ON;
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SDate varchar(20) = cast(@StartDate as varchar)--dateadd(DAY,-60,getdate())
declare @EDate varchar(20) = cast(@EndDate as varchar)--dateadd(DAY,120,getdate())

SET @SqlCmd1 = 
'
SELECT * into #Final FROM OPENQUERY(['+@current_PMS_ServerName+'],  ''exec production.dbo.GetSewingLineScheduleData '''''+ @SDate+'''''
,'''''+ @EDate +'''''
'')
where SewingDay between '''+@SDate+'''  and '''+@EDate+'''

MERGE INTO P_SewingLineSchedule t --要被insert/update/delete的表
USING #Final s --被參考的表
   ON t.APSNo=s.APSNo  
   AND t.SewingDay=s.SewingDay 
   AND t.MDivisionID = s.MDivisionID 
   AND t.FactoryID = s.FactoryID
WHEN MATCHED THEN
	UPDATE SET
	t.APSNo =  s.APSNo,
	t.SewingLineID =  s.SewingLineID,
	t.SewingDay =  s.SewingDay,
	t.SewingStartTime =  s.SewingStartTime,
	t.SewingEndTime =  s.SewingEndTime,
	t.MDivisionID =  s.MDivisionID,
	t.FactoryID =  s.FactoryID,
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
	t.Sewer =  s.Sewer,
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
	t.SewingInlineCategory = s.SewingInlineCategory
	'	

set @SqlCmd2 = '
WHEN NOT MATCHED BY TARGET THEN
	INSERT(
	[APSNo]
      ,[SewingLineID]
      ,[SewingDay]
      ,[SewingStartTime]
      ,[SewingEndTime]
      ,[MDivisionID]
      ,[FactoryID]
      ,[PO]
      ,[POCount]
      ,[SP]
      ,[SPCount]
      ,[EarliestSCIdelivery]
      ,[LatestSCIdelivery]
      ,[EarliestBuyerdelivery]
      ,[LatestBuyerdelivery]
      ,[Category]
      ,[Colorway]
      ,[ColorwayCount]
      ,[CDCode]
      ,[ProductionFamilyID]
      ,[Style]
      ,[StyleCount]
      ,[OrderQty]
      ,[AlloQty]
      ,[StardardOutputPerDay]
      ,[CPU]
      ,[WorkHourPerDay]
      ,[StardardOutputPerHour]
      ,[Efficienycy]
      ,[ScheduleEfficiency]
      ,[LineEfficiency]
      ,[LearningCurve]
      ,[SewingInline]
      ,[SewingOffline]
      ,[PFRemark]
      ,[MTLComplete]
      ,[KPILETA]
      ,[MTLETA]
      ,[ArtworkType]
      ,[InspectionDate]
      ,[Remarks]
      ,[CuttingOutput]
      ,[SewingOutput]
      ,[ScannedQty]
      ,[ClogQty]
      ,[Sewer]
      ,[SewingCPU]
      ,[BrandID]
      ,[Orig_WorkHourPerDay]
      ,[New_SwitchTime]
      ,[FirststCuttingOutputDate]
	  ,[CDCodeNew] ,[ProductType] ,[FabricType] ,[Lining] ,[Gender] ,[Construction]
	  ,[TTL_PRINTING (PCS)]
	  ,[TTL_PRINTING PPU (PPU)]
	  ,SubCon
	  ,[Subcon Qty]
	  ,[Std Qty for printing]
	  ,StyleName
	  ,StdQtyEMB
	  ,EMBStitch
	  ,EMBStitchCnt
	  ,TtlQtyEMB
	  ,PrintPcs
	  ,InlineCategory
	  ,StyleSeason
	  ,SewingInlineCategory
	)
	VALUES(
	s.APSNo,
	s.SewingLineID,
	s.SewingDay,
	s.SewingStartTime,
	s.SewingEndTime,
	s.MDivisionID,
	s.FactoryID,
	s.PO,
	s.POCount,
	s.SP,
	s.SPCount,
	s.EarliestSCIdelivery,
	s.LatestSCIdelivery,
	s.EarliestBuyerdelivery,
	s.LatestBuyerdelivery,
	s.Category,
	s.Colorway,
	s.ColorwayCount,
	s.CDCode,
	s.ProductionFamilyID,
	s.Style,
	s.StyleCount,
	s.OrderQty,
	s.AlloQty,
	s.StardardOutputPerDay,
	s.CPU,
	s.New_WorkHourPerDay,
	s.StardardOutputPerHour,
	s.Efficienycy,
	s.ScheduleEfficiency,
	s.LineEfficiency,
	s.LearningCurve,
	s.SewingInline,
	s.SewingOffline,
	s.PFRemark,
	s.MTLComplete,
	s.KPILETA,
	s.MTLETA,
	s.ArtworkType,
	s.InspectionDate,
	s.Remarks,
	s.CuttingOutput,
	s.SewingOutput,
	s.ScannedQty,
	s.ClogQty,
	s.Sewer,
	s.SewingCPU,
	s.BrandID,
	s.Orig_WorkHourPerDay,
	s.New_SwitchTime,
	s.FirststCuttingOutputDate,
	s.CDCodeNew , s.ProductType, s.FabricType, s.Lining, s.Gender, s.Construction,
	s.[TTL_PRINTING (PCS)],
	s.[TTL_PRINTING PPU (PPU)],
	s.SubCon,
	s.[Subcon Qty],
	s.[Std Qty for printing],
	s.StyleName,
	s.StdQtyEMB,
	s.EMBStitch,
	s.EMBStitchCnt,
	s.TtlQtyEMB,
	s.PrintPcs,
	s.InlineCategory,
	s.StyleSeason,
	s.SewingInlineCategory
	)
WHEN NOT MATCHED BY SOURCE AND T.SewingDay between '''+@SDate+''' and '''+@EDate+''' THEN
DELETE ;

drop table #Final

update b
    set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = ''P_SewingLineSchedule''
'


SET @SqlCmd_Combin = @SqlCmd1+@SqlCmd2
EXEC sp_executesql @SqlCmd_Combin

END

Go