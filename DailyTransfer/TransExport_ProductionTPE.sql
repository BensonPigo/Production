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

	-------------OperationDesc-------------------
	RAISERROR('Export OperationDesc - Starts',0,0)
	select * into dbo.OperationDesc from (select * from ProductionTPE.dbo.OperationDesc ) as tmp
	
	-------------MeasurementTranslate-------------------
	RAISERROR('Export MeasurementTranslate - Starts',0,0)
	select * into dbo.MeasurementTranslate from (select * from ProductionTPE.dbo.MeasurementTranslate ) as tmp
	
	-------------FIR_Grade-------------------
	RAISERROR('Export FIR_Grade - Starts',0,0)
	select * into dbo.FIR_Grade from (select * from ProductionTPE.dbo.FIR_Grade ) as tmp
	
	-------------AccountNoSetting-------------------
	RAISERROR('AccountNoSetting - Starts',0,0)
	select * into dbo.AccountNoSetting from (select * from ProductionTPE.dbo.AccountNoSetting ) as tmp

	set transaction isolation level read committed
END

GO


