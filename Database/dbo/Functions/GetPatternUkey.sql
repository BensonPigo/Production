-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>

-- Description:	
--Find Pattern.Uey For Garment 
--其中一組合條件找
--@Poid+@MarkerNo
--@Poid+@CutRef
--@Style

-- =============================================
CREATE FUNCTION GetPatternUkey
(	
	@Poid VARCHAR(16), -- 找R版時母單一定要輸入
	@CutRef VARCHAR(6),-- 裁次, 若一定有Cutref就傳這參數進來,這是用來找MarkerNo
	@MarkerNo varchar(10), -- 基本For 報表Cutting R02 R03, 因WorkOrder有些還沒有設定CutRef
	@Style bigint=null,-- 款式
	@SizeGroup varchar(1)= ''
)
RETURNS @PatternUkey TABLE (PatternUkey bigint
	, Index Idx_PatternUkey NonClustered (PatternUkey)) -- table index
AS
Begin
	Declare @Ukey bigint

	--先找R版
	IF isnull(@MarkerNo,'') != '' and isnull(@Poid,'') != ''
	Begin
		IF isnull(@SizeGroup,'') <> ''
		begin
			set @Ukey =(
				select top 1 p.UKey
				from Order_EachCons oe with(nolock)
				inner join SMNotice s with(nolock)on s.ID=oe.SMNoticeID
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id 
				where oe.MarkerNo = @MarkerNo and oe.id = @Poid
					and SUBSTRING(s.PatternNo,len(s.PatternNo),1) = 'R'
					and sd.PhaseID = 'Bulk'
					and p.Status='Completed'
					and SUBSTRING(s.PatternNo,len(s.PatternNo)-1,1) = @SizeGroup
				order by p.EditDate desc)
		end

		IF @Ukey is null
		Begin
			set @Ukey =(
				select top 1 p.UKey
				from Order_EachCons oe with(nolock)
				inner join SMNotice s with(nolock)on s.ID=oe.SMNoticeID
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id 
				where oe.MarkerNo = @MarkerNo and oe.id = @Poid
					and SUBSTRING(s.PatternNo,len(s.PatternNo),1) = 'R'
					and sd.PhaseID = 'Bulk'
					and p.Status='Completed'
				order by p.EditDate desc)
		End
	End

	IF isnull(@CutRef,'') !='' and isnull(@Poid,'') != '' and @Ukey is null
	Begin
		IF isnull(@SizeGroup,'') <> ''
		begin
			set @Ukey =(
				select top 1 p.UKey
				from WorkOrderForOutput w with(nolock)
				inner join Order_EachCons oe with(nolock)on oe.MarkerNo = w.MarkerNo and w.id = oe.id
				inner join SMNotice s with(nolock)on s.ID=oe.SMNoticeID
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id
				where w.cutref = @CutRef and w.id = @Poid
					and SUBSTRING(s.PatternNo,len(s.PatternNo),1) = 'R'
					and sd.PhaseID = 'Bulk'
					and p.Status='Completed'
					and SUBSTRING(s.PatternNo,len(s.PatternNo)-1,1) = @SizeGroup
				order by p.EditDate desc)
		End
		
		IF @Ukey is null
		Begin
			set @Ukey =(
				select top 1 p.UKey
				from WorkOrderForOutput w with(nolock)
				inner join Order_EachCons oe with(nolock)on oe.MarkerNo = w.MarkerNo and w.id = oe.id
				inner join SMNotice s with(nolock)on s.ID=oe.SMNoticeID
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id
				where w.cutref = @CutRef and w.id = @Poid
					and SUBSTRING(s.PatternNo,len(s.PatternNo),1) = 'R'
					and sd.PhaseID = 'Bulk'
					and p.Status='Completed'
				order by p.EditDate desc)
		End
	End

	-- 找N版
	IF @Style is not null and @Ukey is null
	Begin
		IF isnull(@SizeGroup,'') <> ''
		begin		
			set @Ukey =(		
				select top 1 p.ukey
				from SMNotice s with(nolock)
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id
				where s.StyleUkey = @Style
					and sd.PhaseID = 'Bulk'
					and p.Status='Completed'
					and SUBSTRING(p.PatternNo,len(p.PatternNo),1) = 'N'
					and SUBSTRING(s.PatternNo,len(s.PatternNo)-1,1) = @SizeGroup
				order by p.EditDate desc)
		End
		
		IF @Ukey is null
		Begin
			set @Ukey =(		
				select top 1 p.ukey
				from SMNotice s with(nolock)
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id
				where s.StyleUkey = @Style
					and sd.PhaseID = 'Bulk'
					and p.Status='Completed'
					and SUBSTRING(p.PatternNo,len(p.PatternNo),1) = 'N'
				order by p.EditDate desc)
		End
	End
	
	IF @Style is not null and @Ukey is null
	Begin
		IF isnull(@SizeGroup,'') <> ''
		begin		
			set @Ukey =(		
				select top 1 p.ukey
				from SMNotice s with(nolock)
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id
				where s.StyleUkey = @Style
					and p.Status='Completed'
					and SUBSTRING(p.PatternNo,len(p.PatternNo),1) = 'N'
					and SUBSTRING(s.PatternNo,len(s.PatternNo)-1,1) = @SizeGroup
				order by p.EditDate desc)
		End
		IF @Ukey is null
		Begin
			set @Ukey =(		
				select top 1 p.ukey
				from SMNotice s with(nolock)
				inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
				inner join Pattern p with(nolock)on p.id = sd.id
				where s.StyleUkey = @Style
					and p.Status='Completed'
					and SUBSTRING(p.PatternNo,len(p.PatternNo),1) = 'N'
				order by p.EditDate desc)
		End
	End

	insert into @PatternUkey values(@Ukey)

	RETURN;
End