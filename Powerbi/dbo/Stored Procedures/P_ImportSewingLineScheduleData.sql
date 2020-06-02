-- =============================================
-- Create date: 2020/03/20
-- Description:	Data Query Logic by PMS.PPIC R01, Import Data to P_SewingLineSchedule
-- =============================================
CREATE PROCEDURE [dbo].[P_ImportSewingLineScheduleData]
(
	 @LinkServerName varchar(50)
)
AS
BEGIN

	SET NOCOUNT ON;

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @StartDate Date = '2020/01/01'--dateadd(DAY,-30,getdate())
declare @EndDate Date = dateadd(YEAR,20,getdate())
--注意，Store Procedure 產生出來的欄位順序，必須與下列INSERT的順序一致
SET @SqlCmd1 = 
'
declare @StartDate Date = ''2020/01/01''--dateadd(DAY,-30,getdate())
declare @EndDate Date = dateadd(year,20,getdate())

SELECT * into #Final FROM OPENQUERY(['+@LinkServerName+'], ''exec production.dbo.GetSewingLineScheduleData '''''+ CAST(@StartDate as VARCHAR(100))+'''''
,'''''+ CAST(@EndDate as VARCHAR(100))+'''''
'')
where SewingDay between @StartDate and @EndDate

MERGE INTO P_SewingLineSchedule t --要被insert/update/delete的表
USING #Final s --被參考的表
   ON t.APSNo=s.APSNo  
   AND t.SewingDay=s.SewingDay 
   AND t.MDivisionID = s.MDivisionID 
   AND t.FactoryID = s.FactoryID
   --and t.ScheduleEfficiency = s.ScheduleEfficiency
   --and t.AlloQty = s.AlloQty
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
	t.FirststCuttingOutputDate =  s.FirststCuttingOutputDate

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
	s.FirststCuttingOutputDate	
	)
WHEN NOT MATCHED BY SOURCE AND T.SewingDay between @StartDate and @EndDate 
	and t.MDivisionID in (select distinct MDivisionID from #Final ) THEN
DELETE ;

drop table #Final

'

SET @SqlCmd_Combin = @SqlCmd1
EXEC sp_executesql @SqlCmd_Combin

END