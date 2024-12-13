Use POWERBIReportData
GO
Create PROCEDURE [dbo].[P_Import_StationHourlyOutput]
	@StartDate as date,
	@EndDate as date
As
BEGIN
	Select 
	[FactoryID]
	, [Date]
	, [Shift]
	, [Team]
	, [Line]
	, [Station]
	, [Capacity]
	, [Target]
	, [TotalOutput]
	, [ProblemsEncounter]
	, [ActionsTaken]
	, [Problems4MS]
	, [Ukey]
	, [StyleID] = Isnull(styleid.val, '')
	, [OrderID] = Isnull(OrderID.val, '')
	into #tmpStationHourlyOutput
	From ManufacturingExecution.dbo.StationHourlyOutput sho With(Nolock)
	Outer Apply
	(
		Select val = Stuff(
		(
			Select Distinct Concat('/ ' , s.id)
			From ManufacturingExecution.dbo.Inspection t With(Nolock)
			INNER join ManufacturingExecution.dbo.SciProduction_Style s With(Nolock) On s.Ukey = t.StyleUkey
			Where t.FactoryID = sho.FactoryID 
			And Cast(t.InspectionDate As Date) = Cast(sho.[Date] As Date)
			And t.[Shift] = sho.[Shift]
			And t.[Team] = sho.[Team]
			And t.[Line] = sho.[Line]
			For XML Path('')
		),1,1,'')
	)StyleID
	Outer Apply
	(
		Select val = Stuff(
		(
			Select Distinct Concat('/ ' , t.OrderID)
			From ManufacturingExecution.dbo.Inspection t With(Nolock) 
			Where t.FactoryID = sho.FactoryID 
			And Cast(t.InspectionDate As Date) = Cast(sho.[Date] As Date)
			And t.[Shift] = sho.[Shift]
			And t.[Team] = sho.[Team]
			And t.[Line] = sho.[Line]
			For XML Path('')
		),1,1,'')
	)OrderID
	Where sho.Date Between @StartDate and @EndDate

	Select 
	[Ukey] = shod.Ukey
	, [StationHourlyOutputUkey] = shod.StationHourlyOutputUkey
	, [Oclock] = shod.Oclock
	, [Qty] = shod.Qty
	, [FactoryID] = sho.FactoryID
	into #tmpStationHourlyOutput_Detail
	From ManufacturingExecution.dbo.StationHourlyOutput_Detail shod With(Nolock)
	Inner join #tmpStationHourlyOutput sho On sho.Ukey = shod.StationHourlyOutputUkey

	Insert into dbo.P_StationHourlyOutput (
	[FactoryID]
	, [Date]
	, [Shift]
	, [Team]
	, [Line]
	, [Station]
	, [Capacity]
	, [Target]
	, [TotalOutput]
	, [ProblemsEncounter]
	, [ActionsTaken]
	, [Problems4MS]
	, [Ukey]
	, [StyleID]
	, [OrderID])
	Select
	[FactoryID]
	, [Date]
	, [Shift]
	, [Team]
	, [Line]
	, [Station]
	, [Capacity]
	, [Target]
	, [TotalOutput]
	, [ProblemsEncounter]
	, [ActionsTaken]
	, [Problems4MS]
	, [Ukey]
	, [StyleID]
	, [OrderID]
	From #tmpStationHourlyOutput t
	Where Not Exists (
		Select 1 
		From dbo.P_StationHourlyOutput s 
		Where s.FactoryID = t.FactoryID 
		And s.Ukey = t.Ukey
	)

	Update s 
	Set s.[FactoryID] = t.[FactoryID]
	, s.[Date] = t.[Date]
	, s.[Shift] = t.[Shift]
	, s.[Team] = t.[Team]
	, s.[Line] = t.[Line]
	, s.[Station] = t.[Station]
	, s.[Capacity] = t.[Capacity]
	, s.[Target] = t.[Target]
	, s.[TotalOutput] = t.[TotalOutput]
	, s.[ProblemsEncounter] = t.[ProblemsEncounter]
	, s.[ActionsTaken] = t.[ActionsTaken]
	, s.[Problems4MS] = t.[Problems4MS]
	, s.[Ukey] = t.[Ukey]
	, s.[StyleID] = t.[StyleID]
	, s.[OrderID] = t.[OrderID]
	From dbo.P_StationHourlyOutput s
	Inner Join #tmpStationHourlyOutput t On t.FactoryID = s.FactoryID and t.Ukey = s.Ukey

	Delete s
	From dbo.P_StationHourlyOutput s
	Where Not Exists (
		Select 1 
		From #tmpStationHourlyOutput t
		Where t.FactoryID = s.FactoryID
		And t.Ukey = s.Ukey
	)
	And s.Date Between @StartDate and @EndDate

	Delete s
	From dbo.P_StationHourlyOutput_Detail s
	Where Exists (
		Select 1
		From #tmpStationHourlyOutput t
		Where t.FactoryID = s.FactoryID
		And t.Ukey = s.StationHourlyOutputUkey
	)

	Insert into dbo.P_StationHourlyOutput_Detail (
	[Ukey]
	, [FactoryID]
	, [StationHourlyOutputUkey]
	, [Oclock]
	, [Qty])
	Select 
	[Ukey]
	, [FactoryID]
	, [StationHourlyOutputUkey]
	, [Oclock]
	, [Qty]
	From #tmpStationHourlyOutput_Detail t
	Where Not Exists (
		Select 1
		From dbo.P_StationHourlyOutput_Detail s
		Where s.FactoryID = t.FactoryID
		And s.Ukey = t.Ukey 
	)

	IF Exists(Select 1 From BITableInfo where Id = 'P_StationHourlyOutput')
	Begin
		Update BITableInfo Set TransferDate = GetDate()
		Where Id = 'P_StationHourlyOutput'
	End
	Else
	Begin
		Insert Into BITableInfo(Id, TransferDate, IS_Trans) 
		Values ('P_StationHourlyOutput', GetDate(), 0)
	End
End