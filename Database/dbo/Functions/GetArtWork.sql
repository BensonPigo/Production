
Create FUNCTION GetArtwork
(
	@XML xml --= '<row Article="001" PatternUkey="233209"/><row Article="100" PatternUkey="233209"/>'
	,@SizeCode varchar(8) -- 這只能輸入一個 Size = '3030'
	,@FabricPanelCode varchar(1) -- = 'A'
)
RETURNS nvarchar(256)
AS
BEGIN
	declare  @Artwork nvarchar(256)

	declare @PatternUkey bigint =(select top 1  PatternUkey=Tbl.Col.value('@PatternUkey', 'bigint') FROM @xml.nodes('/row') Tbl(Col))

	declare @Article table (Article varchar(8))
	insert into @Article
	select Article=Tbl.Col.value('@Article', 'varchar(8)')
	FROM @xml.nodes('/row') Tbl(Col)

	--找出要篩選哪些 ArticleGroup
	declare  @ArticleGroup table (ArticleGroup varchar(6))

	insert into @ArticleGroup
	Select distinct ArticleGroup
	from Pattern_GL_Article WITH (NOLOCK)
	where PatternUkey = @PatternUkey
	and rtrim(ltrim(Article)) in (select Article from @Article)
	and SizeRange  = (
		select SizeRange
		from Pattern_GL_Article
		outer apply(select * from SplitString(SizeRange, ',') s)s
		where PatternUkey = @PatternUkey
		and rtrim(ltrim(Article)) in (select Article from @Article)
		and rtrim(ltrim(Data)) = @SizeCode
		group by SizeRange
		having COUNT(1) = 1
	)
	
	if not exists(select 1 from @ArticleGroup)
		insert into @ArticleGroup
		Select distinct ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where ArticleGroup <>'F_CODE' and PatternUkey = @PatternUkey and rtrim(ltrim(Article)) in(select Article from @Article) and rtrim(ltrim(SizeRange)) = @SizeCode
	if not exists(select 1 from @ArticleGroup)
		insert into @ArticleGroup
		Select distinct ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where ArticleGroup <>'F_CODE' and PatternUkey = @PatternUkey and rtrim(ltrim(Article)) in (select Article from @Article)
	if not exists(select 1 from @ArticleGroup)
		insert into @ArticleGroup
		Select distinct ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = @PatternUkey 

	select @Artwork = stuff((
		Select distinct concat('+', pg.Annotation)
		from Pattern_GL pg
		inner join Pattern_GL_LectraCode pgl on pgl.PatternUKEY = pg. PatternUKEY and pgl.ID = pg.ID and pgl.Version = pg.Version and pgl.SEQ = pg.SEQ
		Where pg.PatternUkey = @PatternUkey
		and Annotation <> ''
		and FabricPanelCode = @FabricPanelCode
		and ArticleGroup in (select ArticleGroup from @ArticleGroup)
		for xml path('')
	),1,1,'')

	if isnull(@Artwork, '') = ''
	select @Artwork = stuff((
		Select distinct concat('+', pg.Annotation)
		from Pattern_GL pg
		inner join Pattern_GL_LectraCode pgl on pgl.PatternUKEY = pg. PatternUKEY and pgl.ID = pg.ID and pgl.Version = pg.Version and pgl.SEQ = pg.SEQ
		Where pg.PatternUkey = @PatternUkey
		and Annotation <> ''
		and FabricPanelCode = @FabricPanelCode
		for xml path('')
	),1,1,'')

	return @Artwork ;
END