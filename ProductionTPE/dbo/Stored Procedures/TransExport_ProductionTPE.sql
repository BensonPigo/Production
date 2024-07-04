USE [Trade]
GO

/****** Object:  StoredProcedure [dbo].[TransExport_ProductionTPE]    Script Date: 2019/12/24 �W�� 11:24:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[TransExport_ProductionTPE]
	
AS
BEGIN
	set transaction isolation level read uncommitted

	SET NOCOUNT ON;

	Declare @DbName VarChar(30);
    Set @DbName = DB_Name()

    If Upper(@DbName) = 'ProductionTPE'
    Begin
		Return;
    End;

	-------------CutReason-------------------
	RAISERROR('Export CutReason - Starts',0,0)
	select * into dbo.CutReason from (select * from ProductionTPE.dbo.CutReason) as tmp

	-------------IEReason-------------------
	RAISERROR('Export IEReason - Starts',0,0)
	select * into dbo.IEReason from (select * from ProductionTPE.dbo.IEReason) as tmp

	-------------PackingReason-------------------
	RAISERROR('Export PackingReason - Starts',0,0)
	select * into dbo.PackingReason from (select * from ProductionTPE.dbo.PackingReason ) as tmp
	
	-------------PPICReason-------------------
	RAISERROR('Export PPICReason - Starts',0,0)
	select * into dbo.PPICReason from (select * from ProductionTPE.dbo.PPICReason ) as tmp

	-------------SubProcess-------------------
	RAISERROR('Export SubProcess - Starts',0,0)
	select * into dbo.SubProcess from (select * from ProductionTPE.dbo.SubProcess ) as tmp

	-------------WhseReason-------------------
	RAISERROR('Export WhseReason - Starts',0,0)
	select * into dbo.WhseReason from (select * from ProductionTPE.dbo.WhseReason ) as tmp

	-------------ThreadAllowanceScale-------------------
	RAISERROR('Export ThreadAllowanceScale - Starts',0,0)
	select * into dbo.ThreadAllowanceScale from (select * from ProductionTPE.dbo.ThreadAllowanceScale ) as tmp

	-------------SewingReason-------------------
	RAISERROR('Export SewingReason - Starts',0,0)
	select * into dbo.SewingReason from (select * from ProductionTPE.dbo.SewingReason ) as tmp

	-------------SubconReason-------------------
	RAISERROR('Export SubconReason - Starts',0,0)
	select * into dbo.SubconReason from (select * from ProductionTPE.dbo.SubconReason ) as tmp

	-------------DQSReason-------------------
	RAISERROR('Export DQSReason - Starts',0,0)
	select * into dbo.DQSReason from (select * from ProductionTPE.dbo.DQSReason ) as tmp

	-------------ClogReason-------------------
	RAISERROR('Export ClogReason - Starts',0,0)
	select * into dbo.ClogReason from (select * from ProductionTPE.dbo.ClogReason ) as tmp

	-------------MeasurementTranslate-------------------
	RAISERROR('Export MeasurementTranslate - Starts',0,0)
	select * into dbo.MeasurementTranslate from (select * from ProductionTPE.dbo.MeasurementTranslate ) as tmp
	
	-------------FIR_Grade-------------------
	RAISERROR('Export FIR_Grade - Starts',0,0)
	select * into dbo.FIR_Grade from (select * from ProductionTPE.dbo.FIR_Grade ) as tmp
	
	-------------AccountNoSetting-------------------
	RAISERROR('AccountNoSetting - Starts',0,0)
	select * into dbo.AccountNoSetting from (select * from ProductionTPE.dbo.AccountNoSetting ) as tmp
	
	-------------SubProDefectCode-------------------
	RAISERROR('SubProDefectCode  - Starts',0,0)
	select * into dbo.SubProDefectCode from (select * from ProductionTPE.dbo.SubProDefectCode ) as tmp
	
	-------------PortByBrandShipmode-------------------
	RAISERROR('PortByBrandShipmode  - Starts',0,0)
	select * into dbo.PortByBrandShipmode from (select * from ProductionTPE.dbo.PortByBrandShipmode ) as tmp
	
	-------------PulloutPort-------------------
	RAISERROR('PulloutPort  - Starts',0,0)
	select * into dbo.PulloutPort from (select * from ProductionTPE.dbo.PulloutPort ) as tmp

	-------------Mold-------------------
	RAISERROR('MoldTPE  - Starts',0,0)
	select * into dbo.MoldTPE from (select * from ProductionTPE.dbo.MoldTPE ) as tmp

	-------------Consignee-------------------
	RAISERROR('Consignee  - Starts',0,0)
	select * into dbo.Consignee from (select * from ProductionTPE.dbo.Consignee ) as tmp

	-------------Consignee_Detail-------------------
	RAISERROR('Consignee_Detail  - Starts',0,0)
	select * into dbo.Consignee_Detail from (select * from ProductionTPE.dbo.Consignee_Detail ) as tmp

	-------------ChgOverTarget-------------------
	RAISERROR('ChgOverTarget  - Starts',0,0)
	select * into dbo.ChgOverTarget from (select * from ProductionTPE.dbo.ChgOverTarget where junk <> 1 ) as tmp

	-------------IEReasonType-------------------
	RAISERROR('IEReasonType  - Starts',0,0)
	select * into dbo.IEReasonType from (select * from ProductionTPE.dbo.IEReasonType ) as tmp

	-------------IEReasonTypeGroup-------------------
	RAISERROR('IEReasonTypeGroup  - Starts',0,0)
	select * into dbo.IEReasonTypeGroup from (select * from ProductionTPE.dbo.IEReasonTypeGroup ) as tmp

	-------------IEReasonLBRnotHit_1st-------------------
	RAISERROR('IEReasonLBRnotHit_1st  - Starts',0,0)
	select * into dbo.IEReasonLBRnotHit_1st from (select * from ProductionTPE.dbo.IEReasonLBRnotHit_1st ) as tmp

	-------------IEReasonLBRnotHit_Detail-------------------
	RAISERROR('IEReasonLBRnotHit_Detail  - Starts',0,0)
	select * into dbo.IEReasonLBRnotHit_Detail from (select * from ProductionTPE.dbo.IEReasonLBRnotHit_Detail ) as tmp
	
	-------------Adidas_FGWT-------------------
	RAISERROR('Adidas_FGWT  - Starts',0,0)
	select * into dbo.Adidas_FGWT from (select * from ProductionTPE.dbo.Adidas_FGWT ) as tmp
	
	-------------TypeSelection-------------------
	RAISERROR('TypeSelection  - Starts',0,0)
	select * into dbo.TypeSelection from (select * from ProductionTPE.dbo.TypeSelection ) as tmp
	
	-------------QABrandSetting-------------------
	RAISERROR('QABrandSetting  - Starts',0,0)
	select * into dbo.QABrandSetting from (select * from ProductionTPE.dbo.QABrandSetting ) as tmp
	
	-------------Brand_PullingTestStandarList-------------------
	RAISERROR('Brand_PullingTestStandarList  - Starts',0,0)
	select * into dbo.Brand_PullingTestStandarList from (select * from ProductionTPE.dbo.Brand_PullingTestStandarList ) as tmp
	
	-------------GarmentTestShrinkage-------------------
	RAISERROR('GarmentTestShrinkage  - Starts',0,0)
	select * into dbo.GarmentTestShrinkage from (select * from ProductionTPE.dbo.GarmentTestShrinkage ) as tmp

	-------------GarmentDefectType-------------------
	RAISERROR('GarmentDefectType  - Starts',0,0)
	select * into dbo.GarmentDefectType from (select * from ProductionTPE.dbo.GarmentDefectType ) as tmp

	-------------GarmentDefectCode-------------------
	RAISERROR('GarmentDefectCode  - Starts',0,0)
	select * into dbo.GarmentDefectCode from (select * from ProductionTPE.dbo.GarmentDefectCode ) as tmp
	-------------FabricDefect-------------------
	RAISERROR('FabricDefect  - Starts',0,0)
	select * into dbo.FabricDefect from (select * from ProductionTPE.dbo.FabricDefect ) as tmp

	-------------AccessoryDefect-------------------
	RAISERROR('AccessoryDefect  - Starts',0,0)
	select * into dbo.AccessoryDefect from (select * from ProductionTPE.dbo.AccessoryDefect ) as tmp

	-------------MachineType_Detail-------------------
	RAISERROR('MachineType_Detail  - Starts',0,0)
	select * into dbo.MachineType_Detail from (select * from ProductionTPE.dbo.MachineType_Detail ) as tmp

	-------------MailTo-------------------
	RAISERROR('MailTo  - Starts',0,0)
	select * into dbo.MailTo from (select * from ProductionTPE.dbo.MailTo where ID like 'P%') as tmp

	-------------MailGroup-------------------
	RAISERROR('MailGroup  - Starts',0,0)
	select * into dbo.MailGroup from (select * from ProductionTPE.dbo.MailGroup where Code like 'P%') as tmp

	-------------AutomatedLineMappingConditionSetting-------------------
	RAISERROR('AutomatedLineMappingConditionSetting  - Starts',0,0)
	select * into dbo.AutomatedLineMappingConditionSetting from (select * from ProductionTPE.dbo.AutomatedLineMappingConditionSetting) as tmp

	-------------ShareRule-------------------
	RAISERROR('ShareRule  - Starts',0,0)
	select * into dbo.ShareRule from (select * from ProductionTPE.dbo.ShareRule) as tmp

	-------------SewingMachineAttachment-------------------
	RAISERROR('SewingMachineAttachment  - Starts',0,0)
	select * into dbo.SewingMachineAttachment from (select * from ProductionTPE.dbo.SewingMachineAttachment ) as tmp
	
	-------------ChgOverCheckListBase-------------------
	RAISERROR('ChgOverCheckListBase  - Starts',0,0)
	select * into dbo.ChgOverCheckListBase from (select * from ProductionTPE.dbo.ChgOverCheckListBase ) as tmp

	set transaction isolation level read committed
END

GO


