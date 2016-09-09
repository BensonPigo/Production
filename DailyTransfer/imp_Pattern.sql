
-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE imp_Pattern
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--Imp_pattern
--���YPattern

---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.ID	      =b.ID
      ,a.Version	      =b.Version
      ,a.BrandID	      =b.BrandID
      ,a.ActFtyPattern	      =b.ActFtyPattern
      ,a.PatternNO	      =b.PatternNO
      ,a.RevisedReason	      =b.RevisedReason
      ,a.PatternName	      =b.PatternName
      ,a.EstFinDate	      =b.EstFinDate
      ,a.ActFinDate	      =b.ActFinDate
      ,a.CheckerName	      =b.CheckerName
      ,a.CheckerDate	      =b.CheckerDate
      ,a.Status	      =b.Status
      ,a.CFMName	      =b.CFMName
      ,a.UKey	      =b.UKey
      ,a.StyleRemark	      =b.StyleRemark
      ,a.HisRemark	      =b.HisRemark
      ,a.PendingRemark	      =b.PendingRemark
      ,a.SizeRound	      =b.SizeRound
      ,a.SizeRange	      =b.SizeRange
      ,a.StyleUkey	      =b.StyleUkey
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
      ,a.EditName	      =b.EditName
      ,a.EditDate	      =b.EditDate
from Production.dbo.Pattern as a inner join Trade_To_Pms.dbo.Pattern as b ON a.id=b.id and a.Version=b.Version
-------------------------- INSERT INTO ��
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

from Trade_To_Pms.dbo.Pattern as b
where not exists(select id from Production.dbo.Pattern as a where a.id = b.id and a.Version=b.Version)

--��
--Pattern_GL

----------------------�R���DTABLE�h�����
Delete Production.dbo.Pattern_GL
from Production.dbo.Pattern_GL as a inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
left join Trade_To_Pms.dbo.Pattern_GL as b on a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.ID	       =b.ID
      ,a.Version	      =b.Version
      ,a.PatternUKEY	      =b.PatternUKEY
      ,a.SEQ	      =b.SEQ
      ,a.PatternCode	      =b.PatternCode
      ,a.PatternDesc	      =b.PatternDesc
      ,a.Annotation	      =b.Annotation
      ,a.Alone	      =b.Alone
      ,a.PAIR	      =b.PAIR
      ,a.DV	      =b.DV
      ,a.Remarks	      =b.Remarks

from Production.dbo.Pattern_GL as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
inner join Trade_To_Pms.dbo.Pattern_GL as b ON a.id=b.id and a.Version=b.Version and a.SEQ=b.SEQ
-------------------------- INSERT INTO ��
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

)
select 
       b.ID
      ,b.Version
      ,PatternUKEY
      ,SEQ
      ,PatternCode
      ,PatternDesc
      ,Annotation
      ,Alone
      ,PAIR
      ,DV
      ,Remarks

from Trade_To_Pms.dbo.Pattern_GL as b inner join Trade_To_Pms.dbo.Pattern as c on b.ID=c.ID and b.Version=c.Version
where not exists(select id from Production.dbo.Pattern_GL as a where a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ)

--��Pattern_GL_LectraCode


 ----------------------�R���DTABLE�h�����
Delete Production.dbo.Pattern_GL_LectraCode
from Production.dbo.Pattern_GL_LectraCode as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
left join Trade_To_Pms.dbo.Pattern_GL_LectraCode as b on a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ and a.ArticleGroup=b.ArticleGroup
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.ID	    =b.ID
      ,a.Version	      =b.Version
      ,a.PatternUKEY	      =b.PatternUKEY
      ,a.SEQ	      =b.SEQ
      ,a.PatternCode	      =b.PatternCode
      ,a.ArticleGroup	      =b.ArticleGroup
      ,a.LectraCode	      =b.LectraCode
      ,a.PatternPanel	      =b.PatternPanel
      ,a.FabricCode	      =b.FabricCode

from Production.dbo.Pattern_GL_LectraCode as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
inner join Trade_To_Pms.dbo.Pattern_GL_LectraCode as b on a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ and a.ArticleGroup=b.ArticleGroup
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Pattern_GL_LectraCode(
ID
      ,Version
      ,PatternUKEY
      ,SEQ
      ,PatternCode
      ,ArticleGroup
      ,LectraCode
      ,PatternPanel
      ,FabricCode

)
select 
       b.ID
      ,b.Version
      ,PatternUKEY
      ,SEQ
      ,PatternCode
      ,ArticleGroup
      ,LectraCode
      ,PatternPanel
      ,FabricCode

from Trade_To_Pms.dbo.Pattern_GL_LectraCode as b inner join Trade_To_Pms.dbo.Pattern as c on b.ID=c.ID and b.Version=c.Version
where not exists(select id from Production.dbo.Pattern_GL_LectraCode as a where  a.id = b.id and a.Version=b.Version and a.SEQ=b.SEQ and a.ArticleGroup=b.ArticleGroup)


--��Pattern_GL_Article
----------------------�R���DTABLE�h�����
Delete Production.dbo.Pattern_GL_Article
from Production.dbo.Pattern_GL_Article as a
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
left join Trade_To_Pms.dbo.Pattern_GL_Article as b on a.id = b.id and a.SEQ=b.SEQ and a.Version=b.Version and a.ArticleGroup=b.ArticleGroup and a.Article=b.Article
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.ID	    =b.ID		
      ,a.Seq	      =b.Seq		
      ,a.Version	      =b.Version		
      ,a.PatternUKEY	      =b.PatternUKEY		
      ,a.ArticleGroup	      =b.ArticleGroup		
      ,a.Article	      =b.Article		
      ,a.SizeRange	      =b.SizeRange		
      ,a.Remark	      =b.Remark		
      ,a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =b.EditName		
      ,a.EditDate	      =b.EditDate		

from Production.dbo.Pattern_GL_Article as a 
inner join Trade_To_Pms.dbo.Pattern as c on a.ID=c.ID and a.Version=c.Version
inner join Trade_To_Pms.dbo.Pattern_GL_Article as b ON a.id = b.id and a.SEQ=b.SEQ and a.Version=b.Version and a.ArticleGroup=b.ArticleGroup and a.Article=b.Article
-------------------------- INSERT INTO ��
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

)
select 
       b.ID
      ,Seq
      ,b.Version
      ,PatternUKEY
      ,ArticleGroup
      ,Article
      ,b.SizeRange
      ,Remark
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate

from Trade_To_Pms.dbo.Pattern_GL_Article as b
inner join Trade_To_Pms.dbo.Pattern as c on b.ID=c.ID and b.Version=c.Version
where not exists(select id from Production.dbo.Pattern_GL_Article as a where a.id = b.id and a.SEQ=b.SEQ and a.Version=b.Version and a.ArticleGroup=b.ArticleGroup and a.Article=b.Article)



END

