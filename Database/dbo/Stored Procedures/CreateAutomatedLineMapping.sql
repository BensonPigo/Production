CREATE PROCEDURE [dbo].[CreateAutomatedLineMapping]
	@FactoryID varchar(8),
	@StyleID varchar(15),
	@SeasonID varchar(10),
	@BrandID varchar(8),
	@ComboType varchar(1),
	@ManualSewer int,
	@WorkHour numeric(3, 1),
	@UserID varchar(10),
	@Phase varchar(7),
	@PlusMinusRange int,
	@NeedSaveData bit = 0
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

select	td.ID,
		td.Ukey,
		td.Seq,
		ts.TotalSewer,
		[Sewer] = Round(ts.TotalSewer * (td.SMV / t.TotalSewingTime), 4),
		td.PPA,
		td.IsSubprocess,
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
		t.ComboType = @ComboType
order by td.Seq


DECLARE tmpTimeStudy_Detail_cursor CURSOR FOR 
	select	Seq,
			GroupType,
			Sewer,
			Ukey,
			TotalSewer
	from #tmpTimeStudy_Detail
	where PPA <> 'C' and Sewer > 0 and IsSubprocess = 0
	order by TotalSewer, Seq;

Declare @Seq varchar(4)
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
	if	(@GroupSewer + @Sewer) > @MaxSewerGroupRanger or
		(@LastGroupType <> @GroupType and @CheckSewerGroupRanger > 0)
	begin
		set @GroupSeq = @GroupSeq + 1
	end

	update #tmpTimeStudy_Detail set GroupSeq = @GroupSeq where Ukey = @Ukey and TotalSewer = @TotalSewer
	set @LastGroupType = @GroupType
	set @LastTotalSewer = @TotalSewer
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
									from #tmpGroupSewer
									where ManualGroupSewer > 1 and TotalSewer = @TotalSewerForCal
									order by AvgManualSewerGroup Asc) and
					TotalSewer = @TotalSewerForCal
			
	
			set @DiffSewer = @DiffSewer + 1
		end
	end
FETCH NEXT FROM DiffSewer_Caculate_cursor INTO @TotalSewerForCal,  @DiffSewer
END
CLOSE DiffSewer_Caculate_cursor
DEALLOCATE DiffSewer_Caculate_cursor


update tgs set	tgs.SewerLimitLow = iif(tgs.AvgManualSewerGroup * SewerLowRatio.val > SewerLimit.val, SewerLimit.val, round(tgs.AvgManualSewerGroup * SewerLowRatio.val, 3)),
				tgs.SewerLimitMiddle = iif(tgs.AvgManualSewerGroup * SewerMiddleRatio.val > SewerLimit.val, SewerLimit.val, round(tgs.AvgManualSewerGroup * SewerMiddleRatio.val, 3)),
				tgs.SewerLimitHigh = iif(tgs.AvgManualSewerGroup * SewerHighRatio.val > SewerLimit.val, SewerLimit.val, round(tgs.AvgManualSewerGroup * SewerHighRatio.val, 3))
from #tmpGroupSewer tgs
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'Low') SewerLowRatio
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'Middle') SewerMiddleRatio
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'High') SewerHighRatio
outer apply (select [val] = cast(Name as float) from DropDownList with (nolock) where type = 'Pms_SewerLimit' and ID = 'Limit') SewerLimit


Create table #tmpReaultBase(
	TotalSewer int,
	StationNo varchar(2),
	TimeStudyDetailUkey bigint,
	DivSewer numeric(5, 4),
	OriSewer numeric(5, 4),
	GroupSeq int
)

DECLARE Create_StationNo_cursor CURSOR FOR 
	select td.Ukey, tgs.GroupSeq, td.Sewer, tgs.SewerLimitLow, tgs.SewerLimitMiddle, tgs.SewerLimitHigh, tgs.TotalSewer
	from #tmpGroupSewer tgs
	inner join #tmpTimeStudy_Detail td on tgs.GroupSeq = td.GroupSeq and tgs.TotalSewer = td.TotalSewer
	order by tgs.TotalSewer, tgs.GroupSeq, td.Seq

Declare @TimeStudyDetailUkey bigint
Declare @SewerLimitLow numeric(5, 4)
Declare @SewerLimitMiddle numeric(5, 4)
Declare @SewerLimitHigh numeric(5, 4)
Declare @StationNo int = 0
Declare @LastGroupAssignSewer numeric(5, 4)
Declare @AssignSewer numeric(5, 4)
Declare @UnAssignSewer numeric(5, 4)
Declare @GroupSeqCreateStation int
Declare @SewerCreateStation numeric(7, 4)
Declare @TotalSewerForCreate int
Declare @LastTotalSewerForCreate int

OPEN Create_StationNo_cursor  
FETCH NEXT FROM Create_StationNo_cursor INTO @TimeStudyDetailUkey,  @GroupSeqCreateStation, @SewerCreateStation, @SewerLimitLow, @SewerLimitMiddle, @SewerLimitHigh, @TotalSewerForCreate
WHILE @@FETCH_STATUS = 0 
BEGIN
	if @TotalSewerForCreate <> @LastTotalSewerForCreate
		set @StationNo = 0

	set @UnAssignSewer = @SewerCreateStation

	select @LastGroupAssignSewer = isnull(sum(DivSewer), 0)
	from #tmpReaultBase
	where GroupSeq = @GroupSeqCreateStation and
		  StationNo = @StationNo and
		  TotalSewer = @TotalSewerForCreate

	--如果同group 上一個站位有小於RangeLow的sewer，就必須在這站先補到Middle
	if(@LastGroupAssignSewer > 0 and @LastGroupAssignSewer < @SewerLimitLow)
	begin
		set @AssignSewer = iif(@SewerCreateStation >= @SewerLimitMiddle - @LastGroupAssignSewer, @SewerLimitMiddle - @LastGroupAssignSewer, @SewerCreateStation)

		insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer)
			values(@StationNo, @TimeStudyDetailUkey, @AssignSewer, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate)

		set @UnAssignSewer = @UnAssignSewer - @AssignSewer
	end
	
	if(@LastGroupAssignSewer = 0 or @UnAssignSewer > 0)
	begin
		set @StationNo = @StationNo + 1

		while @UnAssignSewer > @SewerLimitHigh
		begin
			insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer)
			values(@StationNo, @TimeStudyDetailUkey, @SewerLimitMiddle, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate)
			set @StationNo = @StationNo + 1
			set @UnAssignSewer = @UnAssignSewer - @SewerLimitMiddle
		end

		insert into #tmpReaultBase(StationNo, TimeStudyDetailUkey, DivSewer, OriSewer, GroupSeq, TotalSewer)
			values(@StationNo, @TimeStudyDetailUkey, @UnAssignSewer, @SewerCreateStation, @GroupSeqCreateStation, @TotalSewerForCreate)
	end

	set @LastTotalSewerForCreate = @TotalSewerForCreate
FETCH NEXT FROM Create_StationNo_cursor INTO @TimeStudyDetailUkey,  @GroupSeqCreateStation, @SewerCreateStation, @SewerLimitLow, @SewerLimitMiddle, @SewerLimitHigh, @TotalSewerForCreate
END
CLOSE Create_StationNo_cursor
DEALLOCATE Create_StationNo_cursor

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

select	[No] = isnull(tb.StationNo, ''),
		td.Seq,
		[Location] = tl.OperationID,
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
		[SewerDiffPercentage] = Round(tb.DivSewer / tb.OriSewer, 2),
		[DivSewer] = isnull(tb.DivSewer, 0),
		[OriSewer] = isnull(tb.OriSewer, 0),
		[TimeStudyDetailUkey] = td.Ukey,
		td.Thread_ComboID,
		td.IsNonSewingLine,
		[TotalSewer] = isnull(tb.TotalSewer, 0)
into #tmpAutomatedLineMapping_Detail
from  TimeStudy_Detail td 
left join #tmpReaultBase tb with (nolock) on tb.TimeStudyDetailUkey = td.Ukey
left join Operation o with (nolock) on td.OperationID = o.ID
left join #tmpLocation tl on td.Seq >= tl.Seq and td.Seq < tl.NextSeq

--AutomatedLineMapping
select	[StyleUkey] = s.Ukey,
		[Phase] = @Phase,
		[Version] = '',
		[FactoryID] = @FactoryID,
		[StyleID] = s.ID,
		s.SeasonID,
		s.BrandID,
		[ComboType] = @ComboType,
		[StyleCPU] = s.CPU,
		[SewerManpower] = @ManualSewer,
		[OriSewerManpower] = @ManualSewer,
		[PackerManpower] = Floor(iet.PackingProTMS / detailSummary.AvgGSDTime),
		[PresserManpower] = Floor(iet.PressingProTMS / detailSummary.AvgGSDTime),
		detailSummary.TotalGSDTime,
		[HighestGSDTime] = (select Max(GSD)
							from (
									select [GSD] = sum(GSD)
									from #tmpAutomatedLineMapping_Detail
									where	TotalSewer = @ManualSewer and
											OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
											PPA <> 'C' and
											IsNonSewingLine = 0 and
											No <> ''
									Group by No) a
							),
		[TimeStudyID] = t.ID,
		[TimeStudyStatus] = t.Status,
		[TimeStudyVersion] = t.Version,
		[WorkHour] = @WorkHour
from TimeStudy t with (nolock)
inner join Style s with (nolock) on s.ID = t.StyleID and
									s.BrandID = t.BrandID and
									s.SeasonID = s.SeasonID
outer apply (select [PressingProTMS] = iesPressing.ProTMS, [PackingProTMS] = iesPacking.ProTMS
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
				where   t.StyleID = s.ID and
						t.BrandID = s.BrandID and
						t.SeasonID = s.SeasonID and
						t.ComboType = @ComboType) iet
outer apply(select	[TotalGSDTime] = Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)),
					[AvgGSDTime] = Round(Sum(Round(tmd.GSD * tmd.SewerDiffPercentage, 2)) / @ManualSewer, 2)
			from #tmpAutomatedLineMapping_Detail tmd
			where	tmd.TotalSewer = @ManualSewer and
					tmd.OperationID not in ('PROCIPF00004', 'PROCIPF00003') and
					tmd.PPA <> 'C' and
					tmd.IsNonSewingLine = 0
			) detailSummary
where	t.ID = @TimeStudyID

drop table #tmpTotalSewerRange, #tmpTimeStudy_Detail, #tmpGroupSewer, #tmpReaultBase, #tmpLocation, #tmpAutomatedLineMapping_Detail

end
