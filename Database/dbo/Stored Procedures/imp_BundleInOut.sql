CREATE PROCEDURE imp_BundleInOut
AS
BEGIN
	SET NOCOUNT ON;

	Declare @insert Table ([BundleNo] [varchar](10) ,[SubProcessId] [varchar](10));
	insert into Production.dbo.BundleInOut
		(BundleNo,SubProcessID,InComing,OutGoing,AddDate,SewingLineID,LocationID,RFIDProcessLocationID,PanelNo,CutCellID)
	OUTPUT Inserted.BundleNo,Inserted.SubProcessID INTO @insert
	select
		Fb.BundleNo,
		Fb.SubProcessID,
		Fb.PendingInComing,
		Fb.FinishedOutGoing,
		CmdTime,
		'','','','',''
	from FPS.dbo.BundleInOut Fb with(Nolock)
	where not exists(select 1 from Production.dbo.BundleInOut Pb with(nolock) where Pb.BundleNo = Fb.BundleNo and Pb.SubProcessID = Fb.SubProcessID)

	update Fb set Fb.SCIUpdated=1
	from [FPS].dbo.[BundleInOut] Fb
	inner join @insert i on i.BundleNo = Fb.BundleNo and i.SubProcessID = Fb.SubProcessID

	delete @insert

	update Pb set
		Pb.BundleNo = Fb.BundleNo,
		Pb.SubProcessID = Fb.SubProcessID,
		Pb.InComing = Fb.PendingInComing,
		Pb.OutGoing = Fb.FinishedOutGoing,
		Pb.EditDate = Fb.CmdTime,
		Pb.SewingLineID = '',
		Pb.LocationID ='',
		Pb.RFIDProcessLocationID='',
		Pb.PanelNo='',
		Pb.CutCellID=''
	OUTPUT Inserted.BundleNo,Inserted.SubProcessID INTO @insert
	from FPS.dbo.BundleInOut Fb with(Nolock)
	inner join Production.dbo.BundleInOut Pb with(nolock) on Pb.BundleNo = Fb.BundleNo and Pb.SubProcessID = Fb.SubProcessID
	
	update Fb set Fb.SCIUpdated=1
	from [FPS].dbo.[BundleInOut] Fb
	inner join @insert i on i.BundleNo = Fb.BundleNo and i.SubProcessID = Fb.SubProcessID

END