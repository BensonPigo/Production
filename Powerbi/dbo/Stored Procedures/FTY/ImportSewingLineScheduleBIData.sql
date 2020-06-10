-- =============================================
-- Create date: 2020/03/20
-- Description:	Data Query Logic by PMS.PPIC R01, Import Data to P_SewingLineSchedule
-- =============================================
CREATE PROCEDURE [dbo].[ImportSewingLineScheduleBIData]

@StartDate Date,
@EndDate Date

AS
BEGIN

	SET NOCOUNT ON;

/*�P�_��eServer��, ���w�a�J������Server�W��*/
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	--�̤��PServer�ӧ��������ƾ�ServerName
	declare @current_PMS_ServerName nvarchar(50) 
	= (
		select [value] = 
			CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
				 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
				 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
				 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
				 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
				 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
				 WHEN @current_ServerName= 'SQL' THEN 'NDATA' -- HXG
				 when (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
			ELSE '' END
	)

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
	t.FirststCuttingOutputDate =  s.FirststCuttingOutputDate'

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
WHEN NOT MATCHED BY SOURCE AND T.SewingDay between '''+@SDate+''' and '''+@EDate+''' THEN
DELETE ;

drop table #Final'


SET @SqlCmd_Combin = @SqlCmd1+@SqlCmd2
EXEC sp_executesql @SqlCmd_Combin

END

Go