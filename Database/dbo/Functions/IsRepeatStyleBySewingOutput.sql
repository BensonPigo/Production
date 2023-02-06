
Create FUNCTION [dbo].[IsRepeatStyleBySewingOutput]
(
	@FactoryID varchar(8),
	@OutputDate date,
	@SewingLineID varchar(5),
	@Team varchar(5),
	@StyleUkey bigint
)
RETURNS varchar(20)
AS
BEGIN
	declare @tmp_WorkHour Table ([SewingLineID] varchar(5), [FactoryID] varchar(8), [Date] date)
	declare @tmp Table ([SewingLineID] varchar(5), [FactoryID] varchar(8), [Date] date, [IsOutPut] bit, [R_ID] int)

	insert into @tmp_WorkHour(SewingLineID, FactoryID, Date)
	select top 90 w.SewingLineID, w.FactoryID, w.Date
	from WorkHour w
	where w.[Date] <= @OutputDate
	and w.Holiday = 0
	and w.FactoryID = @FactoryID
	and w.SewingLineID = @SewingLineID
	and w.Hours > 0
	order by Date desc

	insert into @tmp(SewingLineID, FactoryID, Date, IsOutPut, R_ID)
	select w.* 
		, [IsOutPut] = ISNULL(so.IsOutPut, 0)
		, R_ID = ROW_NUMBER() over(order by Date)
	from @tmp_WorkHour w
	outer apply (
		select distinct IsOutPut = 1
		from	(	select	[StyleID] = s.ID, s.BrandID
					from Style s with (nolock)
					where s.Ukey = @StyleUkey
					union 
					select	[StyleID] = ssm.ChildrenStyleID, [BrandID] = ssm.ChildrenBrandID
					from Style s with (nolock)
					inner join Style_SimilarStyle ssm with (nolock) on ssm.MasterStyleID = s.ID and ssm.MasterBrandID = s.BrandID
					where s.Ukey = @StyleUkey
				) OriStyleInfo
		where exists(
			select 1
			from SewingOutput_Detail sod with (nolock)
			inner join Orders o with (nolock) on o.ID = sod.OrderId
			where sod.ID in (	select so.ID
								from SewingOutput so with (nolock)
								where so.OutputDate = w.[Date] 
								and so.SewingLineID = w.SewingLineID 
								and so.FactoryID = w.FactoryID 
								and so.Team = @Team  
								and so.Shift <> 'O' 
								and so.Category = 'O'
			)
			and o.BrandID = OriStyleInfo.BrandID 
			and o.StyleID = OriStyleInfo.StyleID
		)
	)so

	-- 連續日的第一產出日 需判斷狀態。
	declare @MinOutPutDate as date, @MinOutPutState as varchar(30), @Continuous as bit

	-- 找最初產出日
	select @MinOutPutDate = Min(Date)
	from @tmp t
	where date > (
		-- 找最近未產出日
		select a.[Date]
		from (select [Date] = MAX(Date) from @tmp where IsOutPut = 0) a 
	)

	IF @MinOutPutDate IS NULL
	BEGIN
		select @MinOutPutDate = Min(Date)
		from @tmp t
		where IsOutPut = 1
	END

	if @MinOutPutDate = @OutputDate　or @MinOutPutDate is null
	begin
		return 'New Style'
	end

	--如果三個月內有資料，判斷連續有資料的第一天的狀態是否為 [New Style] (要排除掉當地假日&Holiday)，是的話就顯示 [New Style]
	select @MinOutPutState = [dbo].[IsRepeatStyleBySewingOutput](@FactoryID, @MinOutPutDate, @SewingLineID, @Team, @StyleUkey)

	select @Continuous = iif (@OutputDate = MAX(Date), 1, 0)
	from (
		select *
			, [Group] = R_ID - ROW_NUMBER() over(partition by SewingLineID, FactoryID order by Date)
		from @tmp
		where Date >= @MinOutPutDate
	)a
	group by [Group]


	if @MinOutPutState = 'New Style' and @Continuous = 1
	begin
		return 'New Style'
	end

	return 'Repeat Style'	
	
END