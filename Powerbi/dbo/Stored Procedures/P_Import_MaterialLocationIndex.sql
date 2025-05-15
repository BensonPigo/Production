-- =============================================
-- Description:	將WH.B02 Material Location Index 轉出BI Table
-- =============================================
CREATE PROCEDURE [dbo].[P_Import_MaterialLocationIndex]
	@StartDate date,
	@EndDate date
AS
BEGIN
	SET NOCOUNT ON;
	declare @SDate date;
	declare @EDate date;

	if @StartDate is null
	begin
		set @SDate = CONVERT(date, DATEADD(MONTH,-3, GETDATE()))
	end
	else	
	begin
		set @SDate = @StartDate
	end

	if @EndDate is null
	begin
		set @EDate = CONVERT(date, GETDATE())
	end
	else
	begin
		set @EDate = @EndDate
	end

	select [ID]
      ,[StockType]
      ,[Junk]
      ,[Description]
      ,[LocationType]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[IsWMS]
      ,[Capacity]
	into #tmpFinal
	from [MainServer].Production.dbo.MtlLocation
	where 1=1
	and (
		CONVERT(date, AddDate) between @SDate and @EDate
		or
		CONVERT(date, EditDate) between @SDate and @EDate
	)

-- 刪修新 BI資料P_MaterialLocationIndex
	delete t
	from P_MaterialLocationIndex t
	where not exists(
		select 1 from #tmpFinal s
		where s.ID = t.ID
		and s.StockType = t.StockType
	)
	and (
		CONVERT(date, t.AddDate) between @SDate and @EDate
		or
		CONVERT(date, t.EditDate) between @SDate and @EDate
	)

	update t
	set t.Junk = s.Junk
	,t.Description = s.Description
	,t.LocationType = s.LocationType
	,t.IsWMS = s.IsWMS
	,t.Capacity = s.Capacity
	,t.AddDate = s.AddDate
	,t.AddName = s.AddName
	,t.EditDate = s.EditDate
	,t.EditName = s.EditName
	from P_MaterialLocationIndex t
	inner join #tmpFinal s on s.ID = t.ID and s.StockType = t.StockType
	
	insert into P_MaterialLocationIndex(
	   [ID]
      ,[StockType]
      ,[Junk]
      ,[Description]
	  ,LocationType
      ,[IsWMS]
      ,[Capacity]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
	)
	select [ID]
      ,[StockType]
      ,[Junk]
      ,[Description]
	  ,LocationType
      ,[IsWMS]
      ,[Capacity]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
	 from #tmpFinal t
	 where not exists(
		select 1 from P_MaterialLocationIndex s
		where s.ID = t.ID
		and s.StockType = t.StockType
	 )

	 
if exists(select 1 from BITableInfo where Id = 'P_MaterialLocationIndex')
begin
	update BITableInfo set TransferDate = GETDATE()
	where Id = 'P_MaterialLocationIndex'
end
else
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_MaterialLocationIndex', GETDATE(), 0)
end

drop table #tmpFinal

END
GO