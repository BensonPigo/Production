
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

END

