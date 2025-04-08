
-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE imp_Pattern
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--Imp_pattern
--表頭Pattern

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.BrandID	      = isnull(b.BrandID                ,'')
      ,a.ActFtyPattern	      = isnull(b.ActFtyPattern  ,'')
      ,a.PatternNO	      = isnull(b.PatternNO          ,'')
      ,a.RevisedReason	      = isnull(b.RevisedReason  ,'')
      ,a.PatternName	      = isnull(b.PatternName    ,'')
      ,a.EstFinDate	      = b.EstFinDate
      ,a.ActFinDate	      = b.ActFinDate
      ,a.CheckerName	      = isnull(b.CheckerName    ,'')
      ,a.CheckerDate	      = b.CheckerDate
      ,a.Status	      = isnull(b.Status                 ,'')
      ,a.CFMName	      = isnull(b.CFMName            ,'')
      ,a.UKey	      = isnull(b.UKey                   ,0)
      ,a.StyleRemark	      = isnull(b.StyleRemark    ,'')
      ,a.HisRemark	      = isnull(b.HisRemark          ,'')
      ,a.PendingRemark	      = isnull(b.PendingRemark  ,'')
      ,a.SizeRound	      = isnull(b.SizeRound          ,0)
      ,a.SizeRange	      = isnull(b.SizeRange          ,'')
      ,a.StyleUkey	      = isnull(b.StyleUkey          ,0)
      ,a.AddName	      = isnull(b.AddName            ,'')
      ,a.AddDate	      = b.AddDate
      ,a.EditName	      = isnull(b.EditName           ,'')
      ,a.EditDate	      =b.EditDate
from Production.dbo.Pattern as a inner join Trade_To_Pms.dbo.Pattern as b ON a.id=b.id and a.Version=b.Version
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Pattern(
ID
      ,Version
      ,BrandID
      ,ActFtyPattern
      ,PatternNO
      ,RevisedReason
      ,PatternName
      ,EstFinDate
      ,ActFinDate
      ,CheckerName
      ,CheckerDate
      ,Status
      ,CFMName
      ,UKey
      ,StyleRemark
      ,HisRemark
      ,PendingRemark
      ,SizeRound
      ,SizeRange
      ,StyleUkey
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
       isnull(ID            ,'')
      ,isnull(Version       ,'')
      ,isnull(BrandID       ,'')
      ,isnull(ActFtyPattern ,'')
      ,isnull(PatternNO     ,'')
      ,isnull(RevisedReason ,'')
      ,isnull(PatternName   ,'')
      ,EstFinDate
      ,ActFinDate
      ,isnull(CheckerName   ,'')
      ,CheckerDate
      ,isnull(Status        ,'')
      ,isnull(CFMName       ,'')
      ,isnull(UKey          ,0)
      ,isnull(StyleRemark   ,'')
      ,isnull(HisRemark     ,'')
      ,isnull(PendingRemark ,'')
      ,isnull(SizeRound     ,0)
      ,isnull(SizeRange     ,'')
      ,isnull(StyleUkey     ,0)
      ,isnull(AddName       ,'')
      ,AddDate
      ,isnull(EditName      ,'')
      ,EditDate

from Trade_To_Pms.dbo.Pattern as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Pattern as a WITH (NOLOCK) where a.id = b.id and a.Version=b.Version)

--表身
--Pattern_GL

----------------------刪除主TABLE多的資料
Delete Production.dbo.Pattern_GL
from Production.dbo.Pattern_GL as a inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
left join Trade_To_Pms.dbo.Pattern_GL as b on a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.PatternUKEY	      = isnull(b.PatternUKEY    ,0)
      ,a.PatternCode	      = isnull(b.PatternCode,'')
      ,a.PatternDesc	      = isnull(b.PatternDesc,'')
      ,a.Annotation	      = isnull(b.Annotation     ,'')
      ,a.Alone	      = isnull(b.Alone              ,'')
      ,a.PAIR	      = isnull(b.PAIR               ,'')
      ,a.DV	      = isnull(b.DV                     ,'')
      ,a.Remarks	      = isnull(b.Remarks        ,'')
	  ,a.Location	      = isnull(b.Location       ,'')
	  ,a.Main	=isnull(b.Main,0)
from Production.dbo.Pattern_GL as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
inner join Trade_To_Pms.dbo.Pattern_GL as b ON a.id=b.id and a.Version=b.Version and a.SEQ=b.SEQ
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Pattern_GL(
       ID
      ,Version
      ,PatternUKEY
      ,SEQ
      ,PatternCode
      ,PatternDesc
      ,Annotation
      ,Alone
      ,PAIR
      ,DV
      ,Remarks
	  ,Location
	  ,Main
)
select 
       isnull(b.ID       ,'')
      ,isnull(b.Version  ,'')
      ,isnull(PatternUKEY,0)
      ,isnull(SEQ        ,'')
      ,isnull(PatternCode,'')
      ,isnull(PatternDesc,'')
      ,isnull(Annotation ,'')
      ,isnull(Alone      ,'')
      ,isnull(PAIR       ,'')
      ,isnull(DV         ,'')
      ,isnull(Remarks    ,'')
	  ,isnull(b.Location ,'')
	  ,isnull(b.Main,0)
from Trade_To_Pms.dbo.Pattern_GL as b WITH (NOLOCK) inner join Trade_To_Pms.dbo.Pattern as c WITH (NOLOCK) on b.ID=c.ID and b.Version=c.Version
where not exists(select id from Production.dbo.Pattern_GL as a WITH (NOLOCK) where a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ)

--表身Pattern_GL_LectraCode


 ----------------------刪除主TABLE多的資料
Delete Production.dbo.Pattern_GL_LectraCode
from Production.dbo.Pattern_GL_LectraCode as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
left join Trade_To_Pms.dbo.Pattern_GL_LectraCode as b on a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ and a.ArticleGroup=b.ArticleGroup
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.PatternUKEY	      = isnull(b.PatternUKEY            ,0)
      ,a.SEQ	      = isnull(b.SEQ                        ,'')
      ,a.PatternCode	      = isnull(b.PatternCode        ,'')
      ,a.ArticleGroup	      = isnull(b.ArticleGroup       ,'')
      ,a.FabricPanelCode	      = isnull(b.FabricPanelCode,'')
      ,a.PatternPanel	      = isnull(b.PatternPanel       ,'')
      ,a.FabricCode	      = isnull(b.FabricCode             ,'')

from Production.dbo.Pattern_GL_LectraCode as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
inner join Trade_To_Pms.dbo.Pattern_GL_LectraCode as b on a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ and a.ArticleGroup=b.ArticleGroup
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Pattern_GL_LectraCode(
ID
      ,Version
      ,PatternUKEY
      ,SEQ
      ,PatternCode
      ,ArticleGroup
      ,FabricPanelCode
      ,PatternPanel
      ,FabricCode

)
select 
       isnull(b.ID           ,'')
      ,isnull(b.Version      ,'')
      ,isnull(PatternUKEY    ,0)
      ,isnull(SEQ            ,'')
      ,isnull(PatternCode    ,'')
      ,isnull(ArticleGroup   ,'')
      ,isnull(FabricPanelCode,'')
      ,isnull(PatternPanel   ,'')
      ,isnull(FabricCode     ,'')

from Trade_To_Pms.dbo.Pattern_GL_LectraCode as b WITH (NOLOCK) inner join Trade_To_Pms.dbo.Pattern as c WITH (NOLOCK) on b.ID=c.ID and b.Version=c.Version
where not exists(select id from Production.dbo.Pattern_GL_LectraCode as a WITH (NOLOCK) where  a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ and a.ArticleGroup=b.ArticleGroup)


--表身Pattern_GL_Article
----------------------刪除主TABLE多的資料
Delete Production.dbo.Pattern_GL_Article
from Production.dbo.Pattern_GL_Article as a
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
left join Trade_To_Pms.dbo.Pattern_GL_Article as b on a.id = b.id and a.SEQ=b.SEQ and a.Version=b.Version and a.ArticleGroup=b.ArticleGroup and a.Article=b.Article
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  	
      a.PatternUKEY	      = isnull(b.PatternUKEY		,0)
      ,a.SizeRange	      = isnull(b.SizeRange		  ,'')
      ,a.Remark	      = isnull(b.Remark		          ,'')
      ,a.AddName	      = isnull(b.AddName		  ,'')
      ,a.AddDate	      = b.AddDate
      ,a.EditName	      = isnull(b.EditName		  ,'')
      ,a.EditDate	      = b.EditDate
	  ,a.App			= isnull(b.App                ,'')
from Production.dbo.Pattern_GL_Article as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
inner join Trade_To_Pms.dbo.Pattern_GL_Article as b ON a.id = b.id and a.SEQ=b.SEQ and a.Version=b.Version and a.ArticleGroup=b.ArticleGroup and a.Article=b.Article
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Pattern_GL_Article(
ID
      ,Seq
      ,Version
      ,PatternUKEY
      ,ArticleGroup
      ,Article
      ,SizeRange
      ,Remark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
	  ,App
)
select 
       isnull(b.ID         ,'')
      ,isnull(Seq          ,'')
      ,isnull(b.Version    ,'')
      ,isnull(PatternUKEY  ,0)
      ,isnull(ArticleGroup ,'')
      ,isnull(Article      ,'')
      ,isnull(b.SizeRange  ,'')
      ,isnull(Remark       ,'')
      ,isnull(b.AddName    ,'')
      ,b.AddDate
      ,isnull(b.EditName   ,'')
      ,b.EditDate
	  ,isnull(b.App        ,'')
from Trade_To_Pms.dbo.Pattern_GL_Article as b WITH (NOLOCK)
inner join Trade_To_Pms.dbo.Pattern as c WITH (NOLOCK) on b.ID=c.ID and b.Version=c.Version
where not exists(select id from Production.dbo.Pattern_GL_Article as a WITH (NOLOCK) where a.id = b.id and a.SEQ=b.SEQ and a.Version=b.Version and a.ArticleGroup=b.ArticleGroup and a.Article=b.Article)

------Pattern_Annotation_Artwork------
DELETE Production.dbo.Pattern_Annotation_Artwork  
from Production.dbo.Pattern_Annotation_Artwork as a
left join Trade_To_Pms.dbo.Pattern_Annotation_Artwork as b on a.id = b.id
where b.id is null

UPDATE a
SET    a.ArtworkTypeID	  = isnull(b.ArtworkTypeID,'')
      ,a.NameCH			  = isnull(b.NameCH		  ,'')
      ,a.NameEN			  = isnull(b.NameEN		  ,'')
      ,a.IEPatternCode	  = isnull(b.IEPatternCode,0)
      ,a.Combine	      = isnull(b.Combine      ,0)

from Production.dbo.Pattern_Annotation_Artwork as a 
inner join Trade_To_Pms.dbo.Pattern_Annotation_Artwork as b ON a.id = b.id


INSERT INTO Production.dbo.Pattern_Annotation_Artwork(
	 ID
	,ArtworkTypeID
	,NameCH
	,NameEN
	,IEPatternCode
	,Combine
)
select 	 isnull(ID            ,'')
		,isnull(ArtworkTypeID ,'')
		,isnull(NameCH        ,'')
		,isnull(NameEN        ,'')
		,isnull(IEPatternCode ,0)
		,isnull(Combine       ,0)
from Trade_To_Pms.dbo.Pattern_Annotation_Artwork as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Pattern_Annotation_Artwork as a WITH (NOLOCK) where a.id = b.id )

UPDATE a
SET a.IsBoundedProcess  = isnull(b.Combine,0)
FROM Production.dbo.SubProcess a WITH (NOLOCK)
INNER JOIN Production.dbo.Pattern_Annotation_Artwork b  WITH (NOLOCK) ON a.ID=b.ID
------------------------------------------------------------------------------------------

------------Order_PatternPanelList--------------------
--找出【裁剪母單清單】
select distinct CuttingSP 
into #tmpCuttingSP
from Trade_To_Pms.dbo.orders where EachConsApv is not null


--列出裁剪母單下須要找的【色組與尺寸】清單 
SELECT	DISTINCT	o.CuttingSP,
					oq.Article,
					oq.SizeCode,
					[PatternUkey] = cast(0 as BIGINT),
					[ArticleGroup] = cast('' as varchar(6)),
					[FabricPanelCode] = cast('' as varchar(2)),
					[PatternPanel] = cast('' as varchar(2)),
					o.StyleUkey
INTO	#tmpOrder_PatternPanelList_step1
from #tmpCuttingSP cs
inner join Production.dbo.Orders o with (nolock) on o.CuttingSP = cs.CuttingSP
inner join Production.dbo.Order_Qty oq with (nolock) on oq.ID = o.ID
where o.Junk = 0 and o.Category in ('B', 'S')

--更新【PatternUkey】
SELECT  distinct
		cs.CuttingSP,
		[Article] = isnull(oea.Article, ''),
		oes.SizeCode,
		[PatternUkey] = p.UKey
into #tmpPatternUkey
from #tmpCuttingSP cs
inner join Production.dbo.Order_EachCons oe with (nolock) on oe.ID = cs.CuttingSP
inner join Production.dbo.Marker m with (nolock) on m.ID = oe.SMNoticeID and m.Version = oe.MarkerVersion
left join Production.dbo.Order_EachCons_Article oea with (nolock) on oea.Order_EachConsUkey = oe.Ukey
inner join Production.dbo.Order_EachCons_SizeQty oes with (nolock) on oes.Order_EachConsUkey = oe.Ukey
inner join Production.dbo.Pattern p with (nolock) on p.ID = m.PatternID and p.Version = m.PatternVersion

update topp set topp.PatternUkey = tpu.PatternUkey
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpPatternUkey tpu on tpu.CuttingSP = topp.CuttingSP and tpu.Article = topp.Article and tpu.SizeCode = topp.SizeCode

update topp set topp.PatternUkey = tpu.PatternUkey
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpPatternUkey tpu on tpu.CuttingSP = topp.CuttingSP and tpu.Article = '' and tpu.SizeCode = topp.SizeCode
where topp.PatternUkey = 0

--還未取到PatternUkey資料，從Production.dbo.GetPatternUkey取得
SELECT	a.StyleUkey,
		a.CuttingSP,
		a.SizeCode,
		[PatternUkey] = PatternUkey.val
into #tmpFromGetPatternUkey
from (	select distinct StyleUkey, CuttingSP, SizeCode
		from #tmpOrder_PatternPanelList_step1 where PatternUkey = 0) a
inner join Production.dbo.Orders o with (nolock) on o.ID = a.CuttingSP
outer APPLY (	select top 1 [val] = os.SizeGroup
				from Production.dbo.Order_SizeCode os with (nolock)
				where os.ID = o.POID and os.SizeCode = a.SizeCode) SizeGroup
outer APPLY (	select top 1 [val] = gp.PatternUkey	from Production.dbo.GetPatternUkey(a.CuttingSP, '', '', a.StyleUkey, SizeGroup.val) gp) PatternUkey

update topp set topp.PatternUkey = tpu.PatternUkey
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpFromGetPatternUkey tpu on tpu.CuttingSP = topp.CuttingSP and tpu.SizeCode = topp.SizeCode
where topp.PatternUkey = 0

--update ArticleGroup drop table #tmpArticleGroup
--因為同一個PatternUkey+Article會有多筆，先找第一筆再update回temp table
select	pgl.PatternUKEY,
		pgl.Article,
		pgl.ArticleGroup,
		pgl.SizeRange,
		[SizeCode] = cast('' as varchar(20))
into #tmpArticleGroup
from Production.dbo.Pattern_GL_Article pgl with (nolock)
where	exists(select 1 from #tmpOrder_PatternPanelList_step1 topp where topp.PatternUkey = pgl.PatternUkey) AND
		pgl.App in ('', '0') AND
		pgl.ArticleGroup in (select max(pgl2.ArticleGroup) 
							from Production.dbo.Pattern_GL_Article pgl2 with (nolock) 
							where	pgl.PatternUKEY = pgl2.PatternUKEY and pgl.Article = pgl2.Article  AND
									pgl2.App in ('', '0'))

--將SizeRange拆分後塞進#tmpArticleGroup.Size 
insert into #tmpArticleGroup(PatternUKEY, Article, ArticleGroup, SizeRange, SizeCode)
SELECT PatternUKEY, Article, ArticleGroup, SizeRange, SizeCode.Data
from #tmpArticleGroup t
outer apply (select * from dbo.SplitString(t.SizeRange, ',')) SizeCode
where sizerange <> ''

--Aricle Group 同時有指定 Aritcle 與 Size Range
UPDATE topp set topp.ArticleGroup = tar.ArticleGroup
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpArticleGroup tar on tar.PatternUKEY = topp.PatternUKEY and tar.Article = topp.Article and tar.SizeCode = topp.SizeCode
where tar.SizeCode <> ''

--Aricle Group 只有指定 Aritcle
UPDATE topp set topp.ArticleGroup = tar.ArticleGroup
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpArticleGroup tar on tar.PatternUKEY = topp.PatternUKEY and tar.Article = topp.Article and tar.SizeCode = ''
where topp.ArticleGroup = ''

--Aricle Group 只有指定 Size Range
UPDATE topp set topp.ArticleGroup = tar.ArticleGroup
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpArticleGroup tar on tar.PatternUKEY = topp.PatternUKEY and tar.SizeCode = topp.SizeCode
where topp.ArticleGroup = '' and tar.SizeCode <> ''

--Aricle Group 都沒有指定 ( 沒有特別指定的皆使用此 Article Group )
UPDATE topp set topp.ArticleGroup = tar.ArticleGroup
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpArticleGroup tar on tar.PatternUKEY = topp.PatternUKEY and tar.Article = '' and tar.SizeCode = ''
where topp.ArticleGroup = ''

--update FabricPanelCode select * from #tmpFabricPanelCode
select	distinct	pgl.PatternUKEY,
					pgl.ArticleGroup,
					pgl.FabricPanelCode
into #tmpFabricPanelCode
from Production.dbo.Pattern_GL_LectraCode pgl with (nolock)
where	exists(select 1 from #tmpOrder_PatternPanelList_step1 topp where topp.PatternUkey = pgl.PatternUKEY and topp.ArticleGroup = pgl.ArticleGroup) AND
		pgl.FabricPanelCode like '[A-Z]' and LEN(pgl.FabricPanelCode) = 1

-- 因為同PatternUKEY, ArticleGroup下會有多個FabricPanelCode，所以上面先distinct後，這邊重新join產生最終的結果
SELECT	topp.CuttingSP,
		topp.Article,
		topp.SizeCode,
		topp.PatternUkey,
		topp.ArticleGroup,
		tfp.FabricPanelCode,
		topp.PatternPanel
into #tmpOrder_PatternPanelList
from #tmpOrder_PatternPanelList_step1 topp
inner join #tmpFabricPanelCode tfp on tfp.PatternUKEY = topp.PatternUKEY and tfp.ArticleGroup = topp.ArticleGroup


--update PatternPanel 
UPDATE topp set topp.PatternPanel = oc.PatternPanel
from #tmpOrder_PatternPanelList topp
inner join Production.dbo.Order_ColorCombo oc with (nolock) on	oc.ID = topp.CuttingSP and
																oc.Article = topp.Article and
																oc.FabricPanelCode = topp.FabricPanelCode and
																oc.FabricType = 'F'

--更新Order_PatternPanelList select * from #tmpOrder_PatternPanelList
--delete
DELETE oppl
from Order_PatternPanelList oppl
where	exists(select 1 from #tmpCuttingSP tcs where tcs.CuttingSP = oppl.CuttingSP) AND
		not exists(select 1 from #tmpOrder_PatternPanelList topp 
					where oppl.CuttingSP = topp.CuttingSP
					and oppl.Article = topp.Article
					and oppl.SizeCode = topp.SizeCode
					and oppl.PatternUkey = topp.PatternUkey
					and oppl.ArticleGroup = topp.ArticleGroup
					and oppl.FabricPanelCode = topp.FabricPanelCode
					and oppl.PatternPanel = topp.PatternPanel
					)

insert into Order_PatternPanelList(CuttingSP
									,Article
									,SizeCode
									,FabricPanelCode
									,PatternPanel
									,PatternUkey
									,ArticleGroup)
SELECT	topp.CuttingSP
		,topp.Article
		,topp.SizeCode
		,topp.FabricPanelCode
		,topp.PatternPanel
		,topp.PatternUkey
		,topp.ArticleGroup
from #tmpOrder_PatternPanelList topp
WHERE	not exists(select 1 from Order_PatternPanelList oppl with (nolock) 
					where oppl.CuttingSP = topp.CuttingSP
					and oppl.Article = topp.Article
					and oppl.SizeCode = topp.SizeCode
					and oppl.PatternUkey = topp.PatternUkey
					and oppl.ArticleGroup = topp.ArticleGroup
					and oppl.FabricPanelCode = topp.FabricPanelCode
					and oppl.PatternPanel = topp.PatternPanel)


drop table #tmpCuttingSP, #tmpOrder_PatternPanelList_step1, #tmpOrder_PatternPanelList, #tmpPatternUkey, #tmpFromGetPatternUkey, #tmpFabricPanelCode, #tmpArticleGroup
------------Order_PatternPanelList END--------------------

END

