
Create FUNCTION GetQualityWebAIComment
(
	@inputType varchar(300) ='',
	@StyleUkey bigint =0,
	@StyleID varchar(20)='',
	@BrandID varchar(20)='',
	@SeasonID varchar(20)=''
)

RETURNS nvarchar(1000)
AS
BEGIN

	declare @AIComment as nvarchar(1000) = ''
	declare @IsRRLR as bit=0
	declare @IsRRLR_ACH as bit=0
	declare @IsRRLR_CF as bit=0

	select TOP 1 @IsRRLR = ad.IsRRLR
	--INTO #AIComment
	from ExtendServer.ManufacturingExecution.dbo.AIComment_Detail ad
	where ad.AICommentUkey in (
		select Ukey from ExtendServer.ManufacturingExecution.dbo.AIComment where FunctionName='QualityWeb'
	)
	and ad.Type IN (
		select DATA
		from dbo.SplitString(@inputType,',')
	)

	if @StyleUkey > 0
	begin
		select @BrandID = BrandID
		from Style s 
		where  s.Ukey = @StyleUkey
		;
		select @IsRRLR_ACH = IIF(COUNT(1) > 0 , 1 ,0)
		from Style s 
		inner join Style_RRLR_Report srr on s.Ukey=srr.StyleUkey
		where  s.Ukey = @StyleUkey
		and srr.RRRemark like '%ACH%'
		;
		select @IsRRLR_CF = IIF(COUNT(1) > 0 , 1 ,0)
		from Style s 
		inner join Style_RRLR_Report srr on s.Ukey=srr.StyleUkey
		where  s.Ukey = @StyleUkey
		and srr.RRRemark like '%ACH%'
	end
	else
	begin
		select @BrandID = BrandID
		from Style s 
		where  s.ID = @StyleID and s.BrandID= @BrandID and s.SeasonID= @SeasonID
		;
		select @IsRRLR_ACH = IIF(COUNT(1) > 0 , 1 ,0)
		from Style s 
		inner join Style_RRLR_Report srr on s.Ukey=srr.StyleUkey
		where  s.ID = @StyleID and s.BrandID= @BrandID and s.SeasonID= @SeasonID
		and srr.RRRemark like '%ACH%'
		;
		select @IsRRLR_CF = IIF(COUNT(1) > 0 , 1 ,0)
		from Style s 
		inner join Style_RRLR_Report srr on s.Ukey=srr.StyleUkey
		where  s.ID = @StyleID and s.BrandID= @BrandID and s.SeasonID= @SeasonID
		and srr.RRRemark like '%CF%'
	end

	SELECT @AIComment= REPLACE( STUFF((
				select '#' +   ad.Type + ': ' +ad.Comment
				from ExtendServer.ManufacturingExecution.dbo.AIComment_Detail ad
				where ad.AICommentUkey in (
					select Ukey from ExtendServer.ManufacturingExecution.dbo.AIComment where FunctionName='QualityWeb'
				)
				and ad.IsRRLR = 1
				and ad.Type IN (
					select DATA
					from dbo.SplitString(@inputType,',')
				)
				and (@BrandID = 'ADIDAS' OR @BrandID ='REEBOK')
				FOR XML PATH('')
			),1,1,'')
		,'#',CHAR(10)+CHAR(13))
	+CHAR(10)+CHAR(13)+
	+ (CASE  WHEN @IsRRLR = 0 THEN ''
					 WHEN @IsRRLR_ACH = 1 and @IsRRLR_CF = 1 THEN '(There is RR/LR .' + 'With shade achievability issue, please ensure shading within tolerance as agreement.'+' Lower color fastness waring, please check if need to apply tissue paper.' +')'
					 WHEN @IsRRLR_ACH = 1 THEN '(There is RR/LR .' + 'With shade achievability issue, please ensure shading within tolerance as agreement.' +')'
					 WHEN @IsRRLR_CF = 1 THEN '(There is RR/LR .' + 'Lower color fastness waring, please check if need to apply tissue paper.' +')'
					ELSE''
				END
			)
	+CHAR(10)+CHAR(13)+
	REPLACE( STUFF((
				select '#' +   ad.Type + ': ' +ad.Comment
				from ExtendServer.ManufacturingExecution.dbo.AIComment_Detail ad
				where ad.AICommentUkey in (
					select Ukey from ExtendServer.ManufacturingExecution.dbo.AIComment where FunctionName='QualityWeb'
				)
				and ad.IsRRLR = 0
				and ad.Type IN (
					select DATA
					from dbo.SplitString(@inputType,',')
				)
				and (@BrandID = 'ADIDAS' OR @BrandID ='REEBOK')
				FOR XML PATH('')
			),1,1,'')
		,'#',CHAR(10)+CHAR(13))

	return @AIComment;
END
GO