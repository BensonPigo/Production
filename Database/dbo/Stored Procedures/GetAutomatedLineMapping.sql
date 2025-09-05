
CREATE PROCEDURE [dbo].[GetAutomatedLineMapping]
	@FactoryID varchar(8),
	@StyleID varchar(15),
	@SeasonID varchar(10),
	@BrandID varchar(8),
	@ComboType varchar(1),
	@ManualSewer int,
	@WorkHour numeric(3, 1),
	@UserID varchar(10),
	@Phase varchar(7),
	@PlusMinusRange int
AS
begin

;WITH TotalSewerRange AS (
    SELECT @ManualSewer - @PlusMinusRange AS TotalSewer
    UNION ALL
    SELECT TotalSewer + 1
    FROM TotalSewerRange
    WHERE TotalSewer < @ManualSewer + @PlusMinusRange
)
SELECT TotalSewer
into #tmpTotalSewerRange
FROM TotalSewerRange

Declare @TotalSMV numeric(16,4)
select @TotalSMV = sum(td.SMV)
from TimeStudy t with (nolock)
inner join TimeStudy_Detail td with (nolock) on t.ID = td.ID
where	t.StyleID = @StyleID and
		t.BrandID = @BrandID and
		t.SeasonID = @SeasonID and
		t.ComboType = @ComboType and
		td.OperationID not like '-%' and
		td.PPA <> 'C' and
		td.IsNonSewingLine = 0

select	td.ID,
		td.Ukey,
		[Seq] = iif(td.DesignateSeq <> '',td.DesignateSeq,td.SewingSeq),
		--td.Seq,
		ts.TotalSewer,
		[Sewer] = Round(ts.TotalSewer * (td.SMV / @TotalSMV), 4),
		td.PPA,
		td.IsNonSewingLine,
		--Template,SewingMachineAttachmentID,Mold 因為這三個內容中會再用","作分割項目，因為可能會有項目相同但順序不同的情況發生，所以要先拆解排序後再組起來
		--先檢查有沒有含","
		[GroupType] =	case	when td.Template not like '%,%' and td.SewingMachineAttachmentID not like '%,%' and td.Mold not like '%,%'
									then	CONCAT(td.MachineTypeID, td.MasterPlusGroup, td.Template, td.SewingMachineAttachmentID, td.Mold, td.Thread_ComboID)
								else	(	Stuff((	select ltrim(rtrim(Data)) 
													from dbo.SplitString(CONCAT(td.MachineTypeID, td.MasterPlusGroup, td.Template, td.SewingMachineAttachmentID, td.Mold, td.Thread_ComboID), ',')
													order by Data FOR XML PATH(''))
											,1,1,'') 	
										)
						end,
		[GroupSeq] = 0
into #tmpTimeStudy_Detail
from TimeStudy t with (nolock)
inner join TimeStudy_Detail td with (nolock) on t.ID = td.ID
cross join #tmpTotalSewerRange ts
where	t.StyleID = @StyleID and
		t.BrandID = @BrandID and
		t.SeasonID = @SeasonID and
		t.ComboType = @ComboType and
		td.OperationID not like '-%' and
		td.SewingSeq <> ''
order by td.Seq

DECLARE tmpTimeStudy_Detail_cursor CURSOR FOR 
	select	Seq,
			GroupType,
			Sewer,
			Ukey,
			TotalSewer
	from #tmpTimeStudy_Detail
	where PPA <> 'C' and Sewer > 0 and IsNonSewingLine = 0
	order by TotalSewer, Seq;

Declare @Seq varchar(4)
Declare @LastSeq varchar(4)
Declare @GroupType varchar(2000)
Declare @Sewer numeric(7, 4)
Declare @Ukey bigint

Declare @LastGroupType varchar(2000)
Declare @LastTotalSewer int

Declare @GroupSeq int = 1
Declare @GroupSewer numeric(7, 4)
Declare @TotalSewer int

Declare @MaxSewerGroupRanger numeric(3, 2)

Declare @sqlCheckSewerGroupRanger nvarchar(max)

Declare @CheckSewerGroupRanger tinyint

select @MaxSewerGroupRanger = max(Range2) from SewerGroupRange

OPEN tmpTimeStudy_Detail_cursor  
FETCH NEXT FROM tmpTimeStudy_Detail_cursor INTO @Seq,  @GroupType, @Sewer, @Ukey, @TotalSewer
WHILE @@FETCH_STATUS = 0 
BEGIN
	
	if @TotalSewer <> @LastTotalSewer
	begin
		set @GroupSeq = 1
		set @LastGroupType = ''
	end

	--分組判斷
	select @GroupSewer = isnull(sum(Sewer), 0) from #tmpTimeStudy_Detail where GroupSeq = @GroupSeq and TotalSewer = @TotalSewer

	--檢查使否有在SewerGroupRange設定內
	SELECT @sqlCheckSewerGroupRanger =  
		Stuff((
				select ' when ' + cast(@GroupSewer as varchar(9)) + sgr.Condition1 + cast(sgr.Range1 as varchar(5)) + ' and ' + cast(@GroupSewer as varchar(9)) + sgr.Condition2 + cast(sgr.Range2 as varchar(5)) + ' then ' + cast(sgr.Sewer as varchar(10)) 
				from SewerGroupRange sgr with (nolock) FOR XML PATH(''))
			,1,1,'') 
	
	set @sqlCheckSewerGroupRanger = 'select @SewerResult = case ' + REPLACE(REPLACE(@sqlCheckSewerGroupRanger, '&gt;', '>'), '&lt;', '<') + ' else 0 end '

	EXEC sp_executesql
    @stmt = @sqlCheckSewerGroupRanger,
    @params = N'@SewerResult tinyint OUTPUT',
    @SewerResult = @CheckSewerGroupRanger OUTPUT

	--
	--1.如果上組Sewer加總大於SewerGroupRange最大值，就直接新編一組
	--2.如果Seq不同，且上組Sewer加總大於SewerGroupRange最大值，就直接新編一組
	if	((@GroupSewer + @Sewer) > @MaxSewerGroupRanger or
		(@LastGroupType <> @GroupType and @CheckSewerGroupRanger > 0)) AND
		@LastSeq <> @Seq
	begin
		set @GroupSeq = @GroupSeq + 1
	end

	update #tmpTimeStudy_Detail set GroupSeq = @GroupSeq where Ukey = @Ukey and TotalSewer = @TotalSewer
	set @LastGroupType = @GroupType
	set @LastTotalSewer = @TotalSewer
	set @LastSeq = @Seq
FETCH NEXT FROM tmpTimeStudy_Detail_cursor INTO @Seq,  @GroupType, @Sewer, @Ukey, @TotalSewer
END
CLOSE tmpTimeStudy_Detail_cursor
DEALLOCATE tmpTimeStudy_Detail_cursor

Create table #tmpGroupSewer(
	TotalSewer int,
	GroupSeq int,
	OriSumSewer numeric(7, 4),
	GroupSewer tinyint,
	AvgSewerGroup numeric(5, 3),
	ManualGroupSewer tinyint,
	AvgManualSewerGroup numeric(5, 3),
	SewerLimitLow numeric(5, 4),
	SewerLimitMiddle numeric(5, 4),
	SewerLimitHigh numeric(5, 4)
)

Declare @sqlUpdateGroupSewer nvarchar(max) =
			'insert into #tmpGroupSewer(TotalSewer, GroupSeq, OriSumSewer, GroupSewer)
			select	TotalSewer,
					GroupSeq,
					sum(Sewer),
					[GroupSewer] = case ' + 
					Stuff((
						select ' when sum(Sewer) ' + sgr.Condition1 + cast(sgr.Range1 as varchar(5)) + ' and sum(Sewer) ' + sgr.Condition2 + cast(sgr.Range2 as varchar(5)) + ' then ' + cast(sgr.Sewer as varchar(10)) 
						from SewerGroupRange sgr with (nolock) FOR XML PATH(''))
					,1,1,'') +
			'		else CEILING(sum(Sewer)) end
			from #tmpTimeStudy_Detail where GroupSeq > 0
			group  by GroupSeq, TotalSewer
			'

set @sqlUpdateGroupSewer = REPLACE(REPLACE(@sqlUpdateGroupSewer, '&gt;', '>'), '&lt;', '<')

exec (@sqlUpdateGroupSewer)

update #tmpGroupSewer set	AvgSewerGroup = OriSumSewer / GroupSewer,
							ManualGroupSewer = GroupSewer,
							AvgManualSewerGroup = OriSumSewer / GroupSewer

DECLARE DiffSewer_Caculate_cursor CURSOR FOR 
	select	TotalSewer,
			[DiffSewer] = TotalSewer - sum(GroupSewer) 
	from #tmpGroupSewer
	group by TotalSewer
	having (TotalSewer - sum(GroupSewer)) <> 0

Declare @DiffSewer int = 0
Declare @TotalSewerForCal int = 0

OPEN DiffSewer_Caculate_cursor  
FETCH NEXT FROM DiffSewer_Caculate_cursor INTO @TotalSewerForCal,  @DiffSewer
WHILE @@FETCH_STATUS = 0 
BEGIN
	if @DiffSewer > 0 
	begin
		while @DiffSewer > 0
		begin
			update #tmpGroupSewer set ManualGroupSewer = ManualGroupSewer + 1,
									  AvgManualSewerGroup = OriSumSewer / (ManualGroupSewer + 1)
			where	GroupSeq = (	select top 1 GroupSeq
									from #tmpGroupSewer
									where ManualGroupSewer > 1 and TotalSewer = @TotalSewerForCal
									order by AvgManualSewerGroup desc) and
					TotalSewer = @TotalSewerForCal
	
			set @DiffSewer = @DiffSewer - 1
		end
	end
	
	if @DiffSewer < 0
	begin
		while @DiffSewer < 0
		begin
			update #tmpGroupSewer set ManualGroupSewer = ManualGroupSewer - 1,
									  AvgManualSewerGroup = OriSumSewer / (ManualGroupSewer - 1)
			where	GroupSeq = (	select top 1 GroupSeq
									from #tmpGroupSewer tgs
									inner join SewerGroupRange sg with (nolock) on sg.Sewer = (tgs.ManualGroupSewer - 1)
									where ManualGroupSewer > 1 and TotalSewer = @TotalSewerForCal
									order by (tgs.OriSumSewer - sg.Range2)/ tgs.ManualGroupSewer asc) and
					TotalSewer = @TotalSewerForCal
			
	
			set @DiffSewer = @DiffSewer + 1
		end
	end
FETCH NEXT FROM DiffSewer_Caculate_cursor INTO @TotalSewerForCal,  @DiffSewer
END
CLOSE DiffSewer_Caculate_cursor
DEALLOCATE DiffSewer_Caculate_cursor

--如果平衡調整人數時ManualGroupSewer有減少的話，就不限制Limit
update tgs set	tgs.SewerLimitLow = case	when tgs.AvgManualSewerGroup * SewerLowRatio.val > SewerLimit.val and tgs.GroupSewer <= tgs.ManualGroupSewer then SewerLimit.val
											else round(tgs.AvgManualSewerGroup * SewerLowRatio.val, 4) end,
				tgs.SewerLimitMiddle = case	when tgs.AvgManualSewerGroup * SewerMiddleRatio.val > SewerLimit.val and tgs.GroupSewer <= tgs.ManualGroupSewer then SewerLimit.val
											else round(tgs.AvgManualSewerGroup * SewerMiddleRatio.val, 4) end,
				tgs.SewerLimitHigh = case	when tgs.AvgManualSewerGroup * SewerHighRatio.val > SewerLimit.val and tgs.GroupSewer <= tgs.ManualGroupSewer then SewerLimit.val
											else round(tgs.AvgManualSewerGroup * SewerHighRatio.val, 4) end
from #tmpGroupSewer tgs
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'Low') SewerLowRatio
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'Middle') SewerMiddleRatio
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'High') SewerHighRatio
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'Limit') SewerLimit


Create table #tmpReaultBase(
	TotalSewer int,
	StationNo int,
	TimeStudyDetailUkey bigint,
	DivSewer numeric(5, 4),
	OriSewer numeric(5, 4),
	GroupSeq int,
	Seq varchar(4)
)

Create table #tmpCheckLimit(
	StationNo int,
	StationSewer numeric(6, 4)
)

-- Ukey -1 為有設定DesignSeq的資料要被視為同一個工段，先合併後分配完No後面再拆開
SELECT	[Ukey] = iif(count(*) > 1, -1, max(td.Ukey)),
		tgs.GroupSeq,
		td.Seq,
		[Sewer] = sum(td.Sewer),
		tgs.SewerLimitLow,
		tgs.SewerLimitMiddle,
		tgs.SewerLimitHigh,
		tgs.TotalSewer,
		tgs.ManualGroupSewer
into #tmpGroupSewer_Step1
from #tmpGroupSewer tgs
inner join #tmpTimeStudy_Detail td on tgs.GroupSeq = td.GroupSeq and tgs.TotalSewer = td.TotalSewer
group by	tgs.GroupSeq,
			td.Seq,
			tgs.SewerLimitLow,
			tgs.SewerLimitMiddle,
			tgs.SewerLimitHigh,
			tgs.TotalSewer,
			tgs.ManualGroupSewer

DECLARE Create_StationNo_cursor CURSOR FOR 
    select	Ukey, 
            GroupSeq, 
            Seq, 
            Sewer, 
            SewerLimitLow, 
            SewerLimitMiddle, 
            SewerLimitHigh, 
            TotalSewer,
            [NextGroupSeq] = LEAD(GroupSeq, 1, 0) OVER (PARTITION BY TotalSewer ORDER BY TotalSewer, GroupSeq, Seq),
            [GroupSumSewer] = SUM(Sewer) OVER (PARTITION BY TotalSewer, GroupSeq),
            ManualGroupSewer
    from #tmpGroupSewer_Step1
    order by TotalSewer, GroupSeq, Seq

Declare @TimeStudyDetailUkey bigint
Declare @SewerLimitLow numeric(5, 4)
Declare @SewerLimitMiddle numeric(5, 4)
Declare @SewerLimitHigh numeric(5, 4)
Declare @StationNo int = 0
Declare @LastGroupAssignSewer numeric(5, 4)
Declare @AssignSewer numeric(5, 4)
Declare @UnAssignSewer numeric(5, 4)
Declare @GroupSumSewer numeric(6, 4)
Declare @GroupSeqCreateStation int
Declare @NextGroupSeqCreateStation int
Declare @SewerCreateStation numeric(7, 4)
Declare @TotalSewerForCreate int
Declare @LastTotalSewerForCreate int
Declare @LimitGroupSewer int
Declare @AccGroupSewer int

Declare @StationNoForFix int = 0
Declare @StationSewer numeric(6, 4)
Declare @SewSeq varchar(4)

OPEN Create_StationNo_cursor  
FETCH NEXT FROM Create_StationNo_cursor INTO @TimeStudyDetailUkey,  @GroupSeqCreateStation, @SewSeq, @SewerCreateStation, @SewerLimitLow, @SewerLimitMiddle, @SewerLimitHigh, @TotalSewerForCreate, @NextGroupSeqCreateStation, @GroupSumSewer, @LimitGroupSewer
WHILE @@FETCH_STATUS = 0 
BEGIN
	if @TotalSewerForCreate <> @LastTotalSewerForCreate
		set @StationNo = 0

	select @LastGroupAssignSewer = isnull(sum(DivSewer), 0)
		from #tmpReaultBase
		where GroupSeq = @GroupSeqCreateStation and
			  StationNo = @StationNo and
			  TotalSewer = @TotalSewerForCreate

	select @AccGroupSewer = count(*)
	from (	select distinct StationNo
			from #tmpReaultBase
			where GroupSeq = @GroupSeqCreateStation and
				  TotalSewer = @TotalSewerForCreate) a

	if(@GroupSumSewer >= @SewerLimitLow and @GroupSumSewer <= @SewerLimitHigh)
	begin
		--如果整個Group都在安全範圍內直接insert同一個No就好
		if @LastGroupAssignSewer = 0
				set @StationNo = @StationNo + 1

		insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer, Seq)
				values(@StationNo, @TimeStudyDetailUkey, @SewerCreateStation, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate, @SewSeq)
	end
	else
	begin
		set @UnAssignSewer = @SewerCreateStation

		--補相同group前一站不足的部分
		if(	@LastGroupAssignSewer > 0 and 
			(@LastGroupAssignSewer < @SewerLimitLow or @AccGroupSewer = @LimitGroupSewer))
		begin
			set @AssignSewer = case when @AccGroupSewer = @LimitGroupSewer then @SewerCreateStation
									when @GroupSeqCreateStation <> @NextGroupSeqCreateStation and (@SewerCreateStation + @LastGroupAssignSewer) <= @SewerLimitHigh then @SewerCreateStation
									when @SewerCreateStation >= @SewerLimitMiddle - @LastGroupAssignSewer then @SewerLimitMiddle - @LastGroupAssignSewer
									else @SewerCreateStation end
			
			insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer, Seq)
				values(@StationNo, @TimeStudyDetailUkey, @AssignSewer, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate, @SewSeq)

			set @UnAssignSewer = @UnAssignSewer - @AssignSewer
		end
		
		--剩餘人力開新站，但不能超過限定站數，超過站數折將多餘人力補到前站
		while @UnAssignSewer > 0
			begin
				
				delete #tmpCheckLimit

				insert into #tmpCheckLimit(StationNo, StationSewer)
				select StationNo, sum(DivSewer)
				from #tmpReaultBase
				where GroupSeq = @GroupSeqCreateStation and
					  TotalSewer = @TotalSewerForCreate
				group by StationNo
				
				--檢查是否超過限定站數
				if @LimitGroupSewer = (select count(*) from #tmpCheckLimit)
				--ISP20231154 無法新增站數，將多餘人力往前站補
				--假設有3個No，第3個超過範圍，則優先併入第2個No值到到達最大值，若若還有剩餘人力，則再併入到第1個No，若還有剩則再併入到第3個No
				BEGIN
					SELECT top 1 @StationNoForFix = StationNo, @StationSewer = StationSewer
					from #tmpCheckLimit
					where StationSewer < @SewerLimitHigh
					order by iif(StationNo = @StationNo, 0, StationNo) desc

					if(isnull(@StationNoForFix, '') = '')
					BEGIN
						insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer, Seq)
						values(@StationNo, @TimeStudyDetailUkey, @UnAssignSewer, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate, @SewSeq)
						set @UnAssignSewer = 0
					end
					ELSE
					BEGIN
						set @AssignSewer = iif(@UnAssignSewer < @SewerLimitHigh - @StationSewer, @UnAssignSewer, @SewerLimitHigh - @StationSewer) 

						if exists (select 1 from #tmpReaultBase where StationNo = @StationNoForFix and TotalSewer = @TotalSewerForCreate and TimeStudyDetailUkey = @TimeStudyDetailUkey)
						BEGIN
							update #tmpReaultBase 
							set DivSewer = @AssignSewer + DivSewer
							where StationNo = @StationNoForFix and TotalSewer = @TotalSewerForCreate and TimeStudyDetailUkey = @TimeStudyDetailUkey
							
						END
						else
						begin
							insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer, Seq)
							values(@StationNoForFix, @TimeStudyDetailUkey, @AssignSewer, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate, @SewSeq)
						end

						set @UnAssignSewer = @UnAssignSewer - @AssignSewer
					END
					
				END
				else
				-- 還可新增站數
				BEGIN
					set @StationNo = @StationNo + 1
					set @AssignSewer = iif(@UnAssignSewer <= @SewerLimitHigh, @UnAssignSewer, @SewerLimitMiddle) 

					insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer, Seq)
					values(@StationNo, @TimeStudyDetailUkey, @AssignSewer, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate, @SewSeq)

					set @UnAssignSewer = @UnAssignSewer - @AssignSewer
				END
			end
	end

	set @LastTotalSewerForCreate = @TotalSewerForCreate
FETCH NEXT FROM Create_StationNo_cursor INTO @TimeStudyDetailUkey,  @GroupSeqCreateStation, @SewSeq, @SewerCreateStation, @SewerLimitLow, @SewerLimitMiddle, @SewerLimitHigh, @TotalSewerForCreate, @NextGroupSeqCreateStation, @GroupSumSewer, @LimitGroupSewer
END
CLOSE Create_StationNo_cursor
DEALLOCATE Create_StationNo_cursor

--將Ukey -1的的資料(合併工段)，依原工段拆開並均分DivSewer
select  t.TotalSewer,
		t.StationNo,
		[TimeStudyDetailUkey] = tg.Ukey,
		[DivSewer] = Round(t.DivSewer / t.OriSewer * tg.Sewer, 4),
		[OriSewer] = tg.Sewer ,
		t.GroupSeq,
		t.Seq,
		[AccuSumDivSewer] = sum(Round(t.DivSewer / t.OriSewer * tg.Sewer, 4)) OVER (PARTITION by tg.Ukey, tg.TotalSewer order by tg.Ukey, t.StationNo),
		[IsLast] = iif(LEAD(tg.Sewer) OVER (PARTITION by tg.Ukey, tg.TotalSewer order by tg.Ukey, t.StationNo) is null, 1, 0)
into #tmpReaultBaseForDesignSeq
from #tmpReaultBase t
inner join #tmpTimeStudy_Detail tg on t.TotalSewer = tg.TotalSewer and t.GroupSeq = tg.GroupSeq and t.Seq = tg.seq
where t.TimeStudyDetailUkey = -1

insert into #tmpReaultBase(TotalSewer, StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, Seq)
SELECT  t.TotalSewer,
		t.StationNo,
		t.TimeStudyDetailUkey,
		[DivSewer] = iif(t.IsLast = 1,t.OriSewer - LAG(t.AccuSumDivSewer,1,0) OVER (PARTITION by t.TimeStudyDetailUkey, t.TotalSewer order by t.TimeStudyDetailUkey, t.StationNo), t.DivSewer),
		t.OriSewer,
		t.GroupSeq,
		t.Seq
from #tmpReaultBaseForDesignSeq t	

delete from #tmpReaultBase where TimeStudyDetailUkey = -1

--AutomatedLineMapping_Detail
Declare @TimeStudyID bigint

select top 1 @TimeStudyID = ID from #tmpTimeStudy_Detail

select	td.Seq,
		[NextSeq] = LEAD(td.Seq,1,0) OVER (ORDER BY td.Seq),
		td.OperationID
into #tmpLocation
from TimeStudy_Detail td with (nolock)
where	td.ID = @TimeStudyID and
		td.OperationID like '-%' and
		td.SMV = 0
order by td.Seq
/*************************************** ISP20240132 需求***************************************/
DECLARE @EndSeq varchar(4)
SET @EndSeq = (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = @TimeStudyID ORDER BY Seq DESC);
 SELECT 
td.Seq
,[NextSeq] = CASE 
        WHEN LEAD(td.Seq, 1, 0) OVER (ORDER BY td.Seq) = 0 
        THEN @EndSeq
        ELSE LEAD(td.Seq, 1, 0) OVER (ORDER BY td.Seq)
    END
,td.OperationID
into #tmpOperation
from TimeStudy_Detail td WITH(NOLOCK)
where td.id = @TimeStudyID and td.OperationID LIKE '-%' and td.smv = 0 

--------------------------------------------------------------------------------------------------



select	[No] = isnull(RIGHT(REPLICATE('0', 2) + cast(tb.StationNo as varchar(3)), 2), ''),
		[Seq] =  ROW_NUMBER() OVER (PARTITION BY tb.TotalSewer ORDER BY  td.SewingSeq),
		[Location] = iif(td.[Location] = '' , isnull(t1.OperationID,''),isnull(td.[Location],'')),
		td.PPA,
		td.MachineTypeID,
		o.MasterPlusGroup,
		td.OperationID,
		[Annotation] =	case	when td.OperationID = 'PROCIPF00004' then '**Pressing'
								when td.OperationID = 'PROCIPF00003' then '**Packing'
								else td.Annotation end,
		[Attachment] = STUFF((	select concat(',' ,s.Data)
								from SplitString(td.Mold, ',') s
								where not exists (select 1 from Mold m WITH (NOLOCK) where s.Data = m.ID and (m.Junk = 1 or m.IsTemplate = 1)) 
								for xml path ('')) 
						,1,1,''),
		td.SewingMachineAttachmentID,
		[Template] = STUFF((
						select concat(',' ,s.Data)
						from SplitString(td.Template, ',') s
						where not exists (select 1 from Mold m WITH (NOLOCK) where s.Data = m.ID and (m.Junk = 1 or m.IsAttachment = 1)) 
						for xml path ('')) 
						,1,1,''),
		[GSD] = Round(td.SMV, 2),
		[SewerDiffPercentage] = iif(td.PPA = 'C', 1, Round(tb.DivSewer / tb.OriSewer, 2)),
		[DivSewer] = isnull(tb.DivSewer, 0),
		[OriSewer] = isnull(tb.OriSewer, 0),
		[TimeStudyDetailUkey] = td.Ukey,
		[ThreadComboID] = td.Thread_ComboID,
		td.IsNonSewingLine,
		[TotalSewer] = isnull(tb.TotalSewer, 0),
		[OperationDesc] = iif(isnull(o.DescEN, '') = '', td.OperationID, o.DescEN),
        [SewerDiffPercentageDesc] = iif(td.PPA = 'C', 1, round(Round(tb.DivSewer / tb.OriSewer, 2) * 100, 0)),
        [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey, TotalSewer),
		[IsNotShownInP05] = isnull(md.IsNotShownInP05,0)
        ,TimeStudySeq = td.Seq
into #tmpAutomatedLineMapping_Detail
from  TimeStudy_Detail td
left join #tmpReaultBase tb with (nolock) on tb.TimeStudyDetailUkey = td.Ukey
LEFT join MachineType_Detail md on md.ID = td.MachineTypeID and md.FactoryID = @FactoryID
left join Operation o with (nolock) on td.OperationID = o.ID
left join #tmpLocation tl on td.Seq >= tl.Seq and (td.Seq < tl.NextSeq or tl.NextSeq = 0)
OUTER APPLY
        (
	        SELECT TOP 1
	        OperationID FROM #tmpOperation t1 
	        WHERE t1.NextSeq = @EndSeq  OR t1.NextSeq > td.Seq  
	        ORDER BY t1.Seq asc
        )t1
where	td.ID = @TimeStudyID and
		td.OperationID not like '-%'

--處理SewerDiffPercentage加總不為1的資料
select	t.TotalSewer,
		t.TimeStudyDetailUkey,
		t.Seq,
		t.DivSewer,
		[TimeStudyDetailUkeySeq] = ROW_NUMBER() OVER (PARTITION BY t.TotalSewer, t.TimeStudyDetailUkey ORDER BY t.DivSewer asc),
		t.SewerDiffPercentage,
		[TimeStudyDetailUkeyTotalPercentage] = Sum(t.SewerDiffPercentage) OVER (PARTITION BY t.TotalSewer, t.TimeStudyDetailUkey)
into #tmpFixSewerDiffPercentage
from #tmpAutomatedLineMapping_Detail t
where	t.IsNonSewingLine = 0 and
		t.TimeStudyDetailUkeyCnt > 1
order by t.TotalSewer,
		 t.TimeStudyDetailUkey

update t set t.SewerDiffPercentage = tf.SewerDiffPercentage - (tf.TimeStudyDetailUkeyTotalPercentage - 1),
			 t.SewerDiffPercentageDesc = (tf.SewerDiffPercentage - (tf.TimeStudyDetailUkeyTotalPercentage - 1)) * 100
from #tmpAutomatedLineMapping_Detail t
inner join #tmpFixSewerDiffPercentage tf on tf.TotalSewer = t.TotalSewer and 
											tf.TimeStudyDetailUkey = t.TimeStudyDetailUkey and
											tf.Seq = t.Seq and
											tf.TimeStudyDetailUkeySeq = 1 AND
											tf.TimeStudyDetailUkeyTotalPercentage <> 1
		


--取得**Pressing與**Packing資料
Declare @PressingProTMS numeric(7, 2)
Declare @PackingProTMS numeric(7, 2)
Declare @PressingMachineTypeID varchar(10)
Declare @PackingMachineTypeID varchar(10)

select	@PressingProTMS = isnull(iesPressing.ProTMS, 0),
		@PressingMachineTypeID = iesPressing.MachineTypeID, 
		@PackingProTMS = isnull(iesPacking.ProTMS, 0),
		@PackingMachineTypeID = iesPacking.MachineTypeID
from timestudy t with (nolock)
inner join IETMS i with (nolock) on i.ID = t.IETMSID and i.Version = t.IETMSVersion
left join IETMS_Summary iesPressing with (nolock) on	iesPressing.IETMSUkey = i.Ukey and 
														iesPressing.Location = '' and 
														iesPressing.ArtworkTypeID = 'Pressing' and 
														iesPressing.MachineTypeID = (select MachineTypeID from Operation with (nolock) where ID = 'PROCIPF00004')
left join IETMS_Summary iesPacking with (nolock) on iesPacking.IETMSUkey = i.Ukey and 
													iesPacking.Location = '' and 
													iesPacking.ArtworkTypeID = 'Packing' and 
													iesPacking.MachineTypeID = (select MachineTypeID from Operation with (nolock) where ID = 'PROCIPF00003')
where   t.StyleID = @StyleID and
		t.BrandID = @BrandID and
		t.SeasonID = @SeasonID and
		t.ComboType = @ComboType

select	tmd.TotalSewer,
		[TotalGSDTime] = Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)),
		[AvgGSDTime] = Round(Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)) / NULLIF(@ManualSewer,0), 2),
		[PackerManpower] = iif(Floor(@PackingProTMS / Round(Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)) / NULLIF(@ManualSewer,0), 2)) = 0, 1, Floor(@PackingProTMS / Round(Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)) / NULLIF(@ManualSewer,0), 2))),
		[PresserManpower] = iif(Floor(@PressingProTMS / Round(Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)) / NULLIF(@ManualSewer,0), 2)) = 0, 1, Floor(@PressingProTMS / Round(Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)) / NULLIF(@ManualSewer,0), 2)))
into #detailSummary
from #tmpAutomatedLineMapping_Detail tmd
where	tmd.OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
		tmd.PPA <> 'C' and
		tmd.IsNonSewingLine = 0
group by tmd.TotalSewer

-- insert **Pressing與**Packing資料
DECLARE Create_detailPackerPresser CURSOR FOR 
	select	TotalSewer, PresserManpower,PackerManpower
	from #detailSummary

Declare @TotalSewerAdditional int
Declare @PackerManpower int
Declare @PresserManpower int
Declare @MaxNo int
Declare @WhileCnt int
Declare @SewerDiffPercentageRemaining numeric(3, 2)
Declare @SewerDiffPercentage numeric(3, 2)

OPEN Create_detailPackerPresser  
FETCH NEXT FROM Create_detailPackerPresser INTO @TotalSewerAdditional,  @PresserManpower, @PackerManpower
WHILE @@FETCH_STATUS = 0 
BEGIN
	select @MaxNo = max(cast(No as int)) from #tmpAutomatedLineMapping_Detail where TotalSewer = @TotalSewerAdditional
	
	--Pressing
	set @WhileCnt = 1
	set @SewerDiffPercentageRemaining = 1
	set @SewerDiffPercentage = 1.0 / @PresserManpower
	while @WhileCnt <= @PresserManpower
	begin
		set @MaxNo = @MaxNo + 1
		if @WhileCnt = @PresserManpower
			set @SewerDiffPercentage = @SewerDiffPercentageRemaining

		insert into #tmpAutomatedLineMapping_Detail(No,
													Seq,
													Location,
													PPA,
													MachineTypeID,
													MasterPlusGroup,
													OperationID,
													Annotation,
													Attachment,
													SewingMachineAttachmentID,
													Template,
													GSD,
													SewerDiffPercentage,
													DivSewer,
													OriSewer,
													TimeStudyDetailUkey,
													ThreadComboID,
													IsNonSewingLine,
													TotalSewer,
													OperationDesc,
													SewerDiffPercentageDesc,
													TimeStudyDetailUkeyCnt,
													IsNotShownInP05,
                                                    TimeStudySeq)
			values(RIGHT(REPLICATE('0', 2) + cast(@MaxNo as varchar(3)), 2),
				   0,
				   '',
				   '',
				   @PressingMachineTypeID,
				   '',
				   'PROCIPF00004',
				   '**Pressing',
				   '',
				   '',
				   '',
				   @PressingProTMS,
				   @SewerDiffPercentage, 
				   0,
				   0,
				   0,
				   '',
				   0,
				   @TotalSewerAdditional,
				   '**Pressing',
				   @SewerDiffPercentage * 100,
				   0,
				   (SELECT IsNotShownInP05 FROM MachineType_Detail WHERE ID = 'MM2PR' and FactoryID = @FactoryID),
                   0)
	
		set @SewerDiffPercentageRemaining = @SewerDiffPercentageRemaining - @SewerDiffPercentage
		set @WhileCnt = @WhileCnt + 1
	end

	--Packing
	set @WhileCnt = 1
	set @SewerDiffPercentageRemaining = 1
	set @SewerDiffPercentage = 1.0 / @PackerManpower
	while @WhileCnt <= @PackerManpower
	begin
		set @MaxNo = @MaxNo + 1
		if @WhileCnt = @PackerManpower
			set @SewerDiffPercentage = @SewerDiffPercentageRemaining

		insert into #tmpAutomatedLineMapping_Detail(No,
													Seq,
													Location,
													PPA,
													MachineTypeID,
													MasterPlusGroup,
													OperationID,
													Annotation,
													Attachment,
													SewingMachineAttachmentID,
													Template,
													GSD,
													SewerDiffPercentage,
													DivSewer,
													OriSewer,
													TimeStudyDetailUkey,
													ThreadComboID,
													IsNonSewingLine,
													TotalSewer,
													OperationDesc,
													SewerDiffPercentageDesc,
													TimeStudyDetailUkeyCnt,
													IsNotShownInP05,
                                                    TimeStudySeq)
			values(RIGHT(REPLICATE('0', 2) + cast(@MaxNo as varchar(3)), 2),
				   0,
				   '',
				   '',
				   @PackingMachineTypeID,
				   '',
				   'PROCIPF00003',
				   '**Packing',
				   '',
				   '',
				   '',
				   @PackingProTMS,
				   @SewerDiffPercentage, 
				   0,
				   0,
				   0,
				   '',
				   0,
				   @TotalSewerAdditional,
				   '**Packing',
				   @SewerDiffPercentage * 100,
				   0,
				  (SELECT IsNotShownInP05 FROM MachineType_Detail WHERE ID = 'MM2PA' and FactoryID = @FactoryID),
                  0)
	
		set @SewerDiffPercentageRemaining = @SewerDiffPercentageRemaining - @SewerDiffPercentage
		set @WhileCnt = @WhileCnt + 1
	end

FETCH NEXT FROM Create_detailPackerPresser INTO @TotalSewerAdditional,  @PresserManpower, @PackerManpower
END
CLOSE Create_detailPackerPresser
DEALLOCATE Create_detailPackerPresser

--AutomatedLineMapping
select	[StyleUkey] = s.Ukey,
		[Phase] = @Phase,
		[Version] = cast(null as int),
		[FactoryID] = @FactoryID,
		[StyleID] = s.ID,
		s.SeasonID,
		s.BrandID,
		[ComboType] = @ComboType,
		[StyleCPU] = s.CPU,
		[SewerManpower] = @ManualSewer,
		[OriSewerManpower] = @ManualSewer,
		[PackerManpower] = ds.PackerManpower,
		[PresserManpower] = ds.PresserManpower,
		[TotalGSDTime] = isnull(ds.TotalGSDTime,0),
		[HighestGSDTime] = isnull((select Max(GSD)
							from (
									select [GSD] = sum(GSD * SewerDiffPercentage)
									from #tmpAutomatedLineMapping_Detail
									where	TotalSewer = @ManualSewer and
											OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
											PPA <> 'C' and
											IsNonSewingLine = 0 and
											No <> ''
									Group by No) a
							),0),
		[TimeStudyID] = t.ID,
		[TimeStudyStatus] = t.Status,
		[TimeStudyVersion] = t.Version,
		[WorkHour] = @WorkHour
from TimeStudy t with (nolock)
inner join Style s with (nolock) on s.ID = t.StyleID and
									s.BrandID = t.BrandID and
									s.SeasonID = t.SeasonID
left join #detailSummary ds on ds.TotalSewer = @ManualSewer
where	t.ID = @TimeStudyID

--AutomatedLineMapping_Detail
select * 
from #tmpAutomatedLineMapping_Detail
where TotalSewer in (@ManualSewer, 0)
order by TotalSewer, No, Seq

--AutomatedLineMapping_DetailAuto, AutomatedLineMapping_DetailTemp
select	[SewerManpower] = TotalSewer,
		*
from #tmpAutomatedLineMapping_Detail
where TotalSewer <> 0
union all
select	[SewerManpower] = SewerGroup.TotalSewer,
		tmd.*
from (select distinct TotalSewer from #tmpAutomatedLineMapping_Detail where TotalSewer <> 0) SewerGroup
cross join #tmpAutomatedLineMapping_Detail tmd
where tmd.TotalSewer = 0
order by [SewerManpower], No, Seq

select	[SewerManpower] = TotalSewer,
		*
from #tmpAutomatedLineMapping_Detail
where TotalSewer <> 0
union all
select	[SewerManpower] = SewerGroup.TotalSewer,
		tmd.*
from (select distinct TotalSewer from #tmpAutomatedLineMapping_Detail where TotalSewer <> 0) SewerGroup
cross join #tmpAutomatedLineMapping_Detail tmd
where tmd.TotalSewer = 0
order by [SewerManpower], No, Seq

drop table #tmpTotalSewerRange, #tmpTimeStudy_Detail, #tmpGroupSewer, #tmpGroupSewer_Step1, #tmpReaultBaseForDesignSeq, #tmpReaultBase, #tmpLocation, #tmpAutomatedLineMapping_Detail, #detailSummary, #tmpCheckLimit, #tmpFixSewerDiffPercentage
,#tmpOperation
end