
-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE imp_Style 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	------

	----------------------20161101ADD_Style_Location
Delete Production.dbo.Style_Location
from Production.dbo.Style_Location as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_Location as b
on a.StyleUkey= b.StyleUkey AND a.Location	= b.Location
where b.StyleUkey is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      a.Rate	      =b.Rate
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
      ,a.EditName	      =b.EditName
      ,a.EditDate	      =b.EditDate
from Production.dbo.Style_Location as a 
inner join Trade_To_Pms.dbo.Style_Location as b ON a.StyleUkey=b.StyleUkey AND a.Location	= b.Location
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Style_Location(
StyleUkey
      ,Location
      ,Rate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
StyleUkey
      ,Location
      ,Rate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.Style_Location as b
where not exists(select 1 from Production.dbo.Style_Location as a where a.StyleUkey=b.StyleUkey AND a.Location	= b.Location)

--  STYLE
--PMS多的欄位
--,[LocalMR]
--,[LocalStyle]
--,[PPMeeting]
--,[NoNeedPPMeeting]
--,[SampleApv]
--,[Type]
---------------------------UPDATE STYLE BY UKEY
UPDATE a
SET  
--a.ID	= b.ID
a.Ukey	= b.Ukey
--,a.BrandID	= b.BrandID
,a.ProgramID	= b.ProgramID
--,a.SeasonID	= b.SeasonID
,a.Model	= b.Model
,a.Description	= b.Description
,a.StyleName	= b.StyleName
,a.CdCodeID	= b.CdCodeID
,a.ApparelType	= b.ApparelType
,a.FabricType	= b.FabricType
,a.Contents	= b.Contents
,a.GMTLT	= b.GMTLT
,a.CPU	= b.CPU
,a.Factories	= b.Factories
,a.FTYRemark	= b.FTYRemark
,a.Phase	= b.Phase
,a.SampleSMR	= b.SampleSMR
,a.SampleMRHandle	= b.SampleMRHandle
,a.BulkSMR	= b.BulkSMR
,a.BulkMRHandle	= b.BulkMRHandle
,a.Junk	= b.Junk
,a.RainwearTestPassed	= b.RainwearTestPassed
,a.SizePage	= b.SizePage
,a.SizeRange	= b.SizeRange
,a.Gender	= b.Gender
,a.CTNQty	= b.CTNQty
,a.StdCost	= b.StdCost
,a.Processes	= b.Processes
,a.ArtworkCost	= b.ArtworkCost
,a.Picture1	= b.Picture1
,a.Picture2	= b.Picture2
,a.Label	= b.Label
,a.Packing	= b.Packing
,a.IETMSID	= b.IETMSID
,a.IETMSVersion	= b.IETMSVersion
,a.IEImportName	= b.IEImportName
,a.IEImportDate	= b.IEImportDate
,a.ApvDate	= b.ApvDate
,a.ApvName	= b.ApvName
,a.CareCode	= b.CareCode
,a.SpecialMark	= b.SpecialMark
,a.Lining	= b.Lining
,a.StyleUnit	= b.StyleUnit
,a.ExpectionForm	= b.ExpectionForm
,a.ExpectionFormRemark	= b.ExpectionFormRemark
,a.ComboType	= b.ComboType
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= IIF(a.EditDate >= b.EditDate,a.EditName,b.EditName)
,a.EditDate	= IIF(a.EditDate >= b.EditDate,a.EditDate,b.EditDate)
,a.SizeUnit	= b.SizeUnit
,a.ModularParent	= b.ModularParent
,a.CPUAdjusted	= b.CPUAdjusted
,a.LocalStyle = 0
from Production.dbo.Style as a inner join Trade_To_Pms.dbo.Style as b ON a.ID	= b.ID AND a.BrandID	= b.BrandID AND a.SeasonID	= b.SeasonID


-----------------------------------------------------------------------------------------------------------
-----------------------------UPDATE STYLE BY PK
--UPDATE a
--SET  
--a.ID	= b.ID
--,a.Ukey	= b.Ukey
--,a.BrandID	= b.BrandID
--,a.ProgramID	= b.ProgramID
--,a.SeasonID	= b.SeasonID
--,a.Model	= b.Model
--,a.Description	= b.Description
--,a.StyleName	= b.StyleName
--,a.CdCodeID	= b.CdCodeID
--,a.ApparelType	= b.ApparelType
--,a.FabricType	= b.FabricType
--,a.Contents	= b.Contents
--,a.GMTLT	= b.GMTLT
--,a.CPU	= b.CPU
--,a.Factories	= b.Factories
--,a.FTYRemark	= b.FTYRemark
--,a.Phase	= b.Phase
--,a.SampleSMR	= b.SampleSMR
--,a.SampleMRHandle	= b.SampleMRHandle
--,a.BulkSMR	= b.BulkSMR
--,a.BulkMRHandle	= b.BulkMRHandle
--,a.Junk	= b.Junk
--,a.RainwearTestPassed	= b.RainwearTestPassed
--,a.SizePage	= b.SizePage
--,a.SizeRange	= b.SizeRange
--,a.Gender	= b.Gender
--,a.CTNQty	= b.CTNQty
--,a.StdCost	= b.StdCost
--,a.Processes	= b.Processes
--,a.ArtworkCost	= b.ArtworkCost
--,a.Picture1	= b.Picture1
--,a.Picture2	= b.Picture2
--,a.Label	= b.Label
--,a.Packing	= b.Packing
--,a.IETMSID	= b.IETMSID
--,a.IETMSVersion	= b.IETMSVersion
--,a.IEImportName	= b.IEImportName
--,a.IEImportDate	= b.IEImportDate
--,a.ApvDate	= b.ApvDate
--,a.ApvName	= b.ApvName
--,a.CareCode	= b.CareCode
--,a.SpecialMark	= b.SpecialMark
--,a.Lining	= b.Lining
--,a.StyleUnit	= b.StyleUnit
--,a.ExpectionForm	= b.ExpectionForm
--,a.ExpectionFormRemark	= b.ExpectionFormRemark
--,a.ComboType	= b.ComboType
--,a.AddName	= b.AddName
--,a.AddDate	= b.AddDate
--,a.EditName	= IIF(a.EditDate >= b.EditDate,a.EditName,b.EditName)
--,a.EditDate	= IIF(a.EditDate >= b.EditDate,a.EditDate,b.EditDate)
--,a.SizeUnit	= b.SizeUnit
--,a.ModularParent	= b.ModularParent
--,a.CPUAdjusted	= b.CPUAdjusted
--,a.LocalStyle = 0
--from Production.dbo.Style as a 
--inner join Trade_To_Pms.dbo.Style as b ON a.ID=b.ID and a.BrandID=b.BrandID and a.SeasonID=b.SeasonID
--where a.LocalStyle=1
---------------------------------------------------------------
---------------------------UPDATE Style_Artwork_Quot(StyleD)
UPDATE a
SET  
a.Ukey=b.Ukey
from Production.dbo.Style_Artwork_Quot as a 
inner join Trade_To_Pms.dbo.Style as b ON a. StyleUkey=b.Ukey
inner join Production.dbo.Style as c ON c.ID=b.ID and c.BrandID=b.BrandID and c.SeasonID=b.SeasonID
where c.LocalStyle=1
and a.Ukey=c.Ukey
---------------------------------------------------------------------------------------------
-------------------------- INSERT STYLE BY以上兩種比對條件都找不到的時候 INSERT
INSERT INTO Production.dbo.Style(
ID
,Ukey
,BrandID
,ProgramID
,SeasonID
,Model
,Description
,StyleName
,CdCodeID
,ApparelType
,FabricType
,Contents
,GMTLT
,CPU
,Factories
,FTYRemark
,Phase
,SampleSMR
,SampleMRHandle
,BulkSMR
,BulkMRHandle
,Junk
,RainwearTestPassed
,SizePage
,SizeRange
,Gender
,CTNQty
,StdCost
,Processes
,ArtworkCost
,Picture1
,Picture2
,Label
,Packing
,IETMSID
,IETMSVersion
,IEImportName
,IEImportDate
,ApvDate
,ApvName
,CareCode
,SpecialMark
,Lining
,StyleUnit
,ExpectionForm
,ExpectionFormRemark
,ComboType
,AddName
,AddDate
,EditName
,EditDate
,SizeUnit
,ModularParent
,CPUAdjusted
,LocalStyle
)
select 
ID
,Ukey
,BrandID
,ProgramID
,SeasonID
,Model
,Description
,StyleName
,CdCodeID
,ApparelType
,FabricType
,Contents
,GMTLT
,CPU
,Factories
,FTYRemark
,Phase
,SampleSMR
,SampleMRHandle
,BulkSMR
,BulkMRHandle
,Junk
,RainwearTestPassed
,SizePage
,SizeRange
,Gender
,CTNQty
,StdCost
,Processes
,ArtworkCost
,Picture1
,Picture2
,Label
,Packing
,IETMSID
,IETMSVersion
,IEImportName
,IEImportDate
,ApvDate
,ApvName
,CareCode
,SpecialMark
,Lining
,StyleUnit
,ExpectionForm
,ExpectionFormRemark
,ComboType
,AddName
,AddDate
,EditName
,EditDate
,SizeUnit
,ModularParent
,CPUAdjusted
,0
from Trade_To_Pms.dbo.Style as b
where not exists(select id from Production.dbo.Style as a where a.ID=b.ID and a.BrandID=b.BrandID and a.SeasonID=b.SeasonID and a.LocalStyle=1)
AND not exists(select id from Production.dbo.Style as a where a.Ukey=b.Ukey )


--Style4
--Style_TmsCost
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_TmsCost
from Production.dbo.Style_TmsCost as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_TmsCost as b
on a.StyleUkey= b.StyleUkey AND a.ArtworkTypeID	= b.ArtworkTypeID
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.ArtworkTypeID	= b.ArtworkTypeID
a.Seq	= b.Seq
,a.Qty	= b.Qty
,a.ArtworkUnit	= b.ArtworkUnit
,a.TMS	= b.TMS
,a.Price	= b.Price
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
from Production.dbo.Style_TmsCost as a 
inner join Trade_To_Pms.dbo.Style_TmsCost as b ON a.StyleUkey=b.StyleUkey AND a.ArtworkTypeID	= b.ArtworkTypeID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_TmsCost(
StyleUkey
,ArtworkTypeID
,Seq
,Qty
,ArtworkUnit
,TMS
,Price
,AddName
,AddDate
,EditName
,EditDate

)
select 
b.StyleUkey
,b.ArtworkTypeID
,b.Seq
,b.Qty
,b.ArtworkUnit
,b.TMS
,b.Price
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate

from Trade_To_Pms.dbo.Style_TmsCost as b
where not exists(select 1 from Production.dbo.Style_TmsCost as a where a.StyleUkey=b.StyleUkey AND a.ArtworkTypeID	= b.ArtworkTypeID)



--STYLEB
--Style_Artwork
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_Artwork
from Production.dbo.Style_Artwork as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.TradeUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_Artwork as b
on a.TradeUkey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
 a.StyleUkey	= b.StyleUkey
,a.ArtworkTypeID	= b.ArtworkTypeID
,a.Article	= b.Article
,a.PatternCode	= b.PatternCode
,a.PatternDesc	= b.PatternDesc
,a.ArtworkID	= b.ArtworkID
,a.ArtworkName	= b.ArtworkName
,a.Qty	= b.Qty
,a.Price	= b.Price
,a.Cost	= b.Cost
,a.Remark	= b.Remark
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
,a.TMS	= b.TMS
--,a.TradeUkey	= b.Ukey

from Production.dbo.Style_Artwork as a 
inner join Trade_To_Pms.dbo.Style_Artwork as b ON a.TradeUkey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_Artwork(
StyleUkey
,ArtworkTypeID
,Article
,PatternCode
,PatternDesc
,ArtworkID
,ArtworkName
,Qty
,Price
,Cost
,Remark
--,Ukey
,AddName
,AddDate
,EditName
,EditDate
,TMS
,TradeUkey

)
select 
StyleUkey
,ArtworkTypeID
,Article
,PatternCode
,PatternDesc
,ArtworkID
,ArtworkName
,Qty
,Price
,Cost
,Remark
--,Ukey
,AddName
,AddDate
,EditName
,EditDate
,TMS
,Ukey

from Trade_To_Pms.dbo.Style_Artwork as b
where not exists(select 1 from Production.dbo.Style_Artwork as a where a.TradeUkey = b.Ukey)


--STYLE1
--Style_QtyCTN
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
 a.StyleUkey	= b.StyleUkey
,a.CustCDID	= b.CustCDID
,a.Qty	= b.Qty
,a.CountryID	= b.CountryID
,a.Continent	= b.Continent
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
--,a.UKey	= b.UKey

from Production.dbo.Style_QtyCTN as a 
inner join Trade_To_Pms.dbo.Style_QtyCTN as b ON a.UKey=b.UKey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_QtyCTN(
StyleUkey
,CustCDID
,Qty
,CountryID
,Continent
,AddName
,AddDate
,EditName
,EditDate
,UKey

)
select 
StyleUkey
,CustCDID
,Qty
,CountryID
,Continent
,AddName
,AddDate
,EditName
,EditDate
,UKey

from Trade_To_Pms.dbo.Style_QtyCTN as b
where not exists(select 1 from Production.dbo.Style_QtyCTN as a where a.UKey = b.UKey)

--STYLE5
--Style_SizeCode(需再確認ukey欄位)
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_SizeCode
from Production.dbo.Style_SizeCode as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_SizeCode as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
,a.Seq	= b.Seq
,a.SizeGroup	= b.SizeGroup
,a.SizeCode	= b.SizeCode
--,a.UKey	= b.UKey
from Production.dbo.Style_SizeCode as a 
inner join Trade_To_Pms.dbo.Style_SizeCode as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_SizeCode(
StyleUkey
,Seq
,SizeGroup
,SizeCode
,UKey

)
select 
StyleUkey
,Seq
,SizeGroup
,SizeCode
,UKey

from Trade_To_Pms.dbo.Style_SizeCode as b
where not exists(select 1 from Production.dbo.Style_SizeCode as a where a.Ukey = b.Ukey)
and b.SizeCode is not null
--STYLE52
--STYLE_SICESPEC(需再確認ukey欄位)
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_SizeSpec
from Production.dbo.Style_SizeSpec as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey =t.Ukey
left join Trade_To_Pms.dbo.Style_SizeSpec as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
,a.SizeItem	= b.SizeItem
,a.SizeCode	= b.SizeCode
,a.SizeSpec	= b.SizeSpec
--,a.UKey	= b.UKey
from Production.dbo.Style_SizeSpec as a 
inner join Trade_To_Pms.dbo.Style_SizeSpec as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_SizeSpec(
StyleUkey
,SizeItem
,SizeCode
,SizeSpec
,UKey

)
select 
StyleUkey
,SizeItem
,SizeCode
,SizeSpec
,UKey

from Trade_To_Pms.dbo.Style_SizeSpec as b
where not exists(select 1 from Production.dbo.Style_SizeSpec as a where a.Ukey = b.Ukey)
--STYLEG
--STYLE_ARTICLE
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_Article
from Production.dbo.Style_Article as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_Article as b
on a.StyleUkey	= b.StyleUkey AND a.Article	= b.Article
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
a.Seq	= b.Seq
--,a.Article	= b.Article
,a.TissuePaper	= b.TissuePaper
,a.ArticleName	= b.ArticleName
,a.Contents	= b.Contents
from Production.dbo.Style_Article as a 
inner join Trade_To_Pms.dbo.Style_Article as b ON a.StyleUkey	= b.StyleUkey AND a.Article	= b.Article
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_Article(
StyleUkey
,Seq
,Article
,TissuePaper
,ArticleName
,Contents
)
select 
StyleUkey
,Seq
,Article
,TissuePaper
,ArticleName
,Contents
from Trade_To_Pms.dbo.Style_Article as b
where not exists(select 1 from Production.dbo.Style_Article as a where a.StyleUkey	= b.StyleUkey AND a.Article	= b.Article)
--STYLEA
--Style_MarkerList
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_MarkerList
from Production.dbo.Style_MarkerList as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_MarkerList as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a. StyleUkey	= b. StyleUkey
--,a.Ukey	= b.Ukey
,a.Seq	= b.Seq
,a.MarkerName	= b.MarkerName
,a.FabricCode	= b.FabricCode
,a.FabricCombo	= b.FabricCombo
,a.LectraCode	= b.LectraCode
,a.MarkerLength	= b.MarkerLength
,a.ConsPC	= b.ConsPC
,a.CuttingPiece	= b.CuttingPiece
,a.ActCuttingPerimeter	= b.ActCuttingPerimeter
,a.StraightLength	= b.StraightLength
,a.CurvedLength	= b.CurvedLength
,a.Efficiency	= b.Efficiency
,a.Remark	= b.Remark
,a.MixedSizeMarker	= b.MixedSizeMarker
,a.MarkerNo	= b.MarkerNo
,a.MarkerUpdate	= b.MarkerUpdate
,a.MarkerUpdateName	= b.MarkerUpdateName
,a.AllSize	= b.AllSize
,a.PhaseID	= b.PhaseID
,a.SMNoticeID	= b.SMNoticeID
,a.MarkerVersion	= b.MarkerVersion
,a.Direction	= b.Direction
,a.CuttingWidth	= b.CuttingWidth
,a.Width	= b.Width
,a.Type	= b.Type
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Style_MarkerList as a 
inner join Trade_To_Pms.dbo.Style_MarkerList as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_MarkerList(
 StyleUkey
,Ukey
,Seq
,MarkerName
,FabricCode
,FabricCombo
,LectraCode
,MarkerLength
,ConsPC
,CuttingPiece
,ActCuttingPerimeter
,StraightLength
,CurvedLength
,Efficiency
,Remark
,MixedSizeMarker
,MarkerNo
,MarkerUpdate
,MarkerUpdateName
,AllSize
,PhaseID
,SMNoticeID
,MarkerVersion
,Direction
,CuttingWidth
,Width
,Type
,AddName
,AddDate
,EditName
,EditDate

)
select 
 StyleUkey
,Ukey
,Seq
,MarkerName
,FabricCode
,FabricCombo
,LectraCode
,MarkerLength
,ConsPC
,CuttingPiece
,ActCuttingPerimeter
,StraightLength
,CurvedLength
,Efficiency
,Remark
,MixedSizeMarker
,MarkerNo
,MarkerUpdate
,MarkerUpdateName
,AllSize
,PhaseID
,SMNoticeID
,MarkerVersion
,Direction
,CuttingWidth
,Width
,Type
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.Style_MarkerList as b
where not exists(select 1 from Production.dbo.Style_MarkerList as a where a.Ukey = b.Ukey)

-----------------Style_MarkerList_Article-------------------

	Merge Production.dbo.Style_MarkerList_Article as t
	Using Trade_To_Pms.dbo.Style_MarkerList_Article as s
	on t.style_MarkerListUkey=s.style_MarkerListUkey and t.article=s.article
	when matched then 
		update set
		t.StyleUkey= s.StyleUkey,
		--t.Style_MarkerListUkey= s.Style_MarkerListUkey,
		--t.Article= s.Article,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
	when not matched by target then
		insert(StyleUkey
				,Style_MarkerListUkey
				,Article
				,AddName
				,AddDate
				,EditName
				,EditDate)
		values(s.StyleUkey,
				s.Style_MarkerListUkey,
				s.Article,
				s.AddName,
				s.AddDate,
				s.EditName,
				s.EditDate)
	when not matched by source and t.styleUkey in (select distinct styleUkey from production.dbo.style_markerlist) then
		delete;


----------------Style_MarkerList_PatternPanel------------------
	
	Merge Production.dbo.Style_MarkerList_PatternPanel as t
	Using trade_to_pms.dbo.Style_MarkerList_PatternPanel as s
	On t.Style_MarkerListUkey=s.Style_MarkerListUkey and t.Lectracode=s.Lectracode
	when matched then 
		update set
		t.StyleUkey= s.StyleUkey,
		--t.Style_MarkerListUkey= s.Style_MarkerListUkey,
		t.PatternPanel= s.PatternPanel,
		--t.Lectracode= s.Lectracode,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
	when not matched by target then 
		insert(StyleUkey
			,Style_MarkerListUkey
			,PatternPanel
			,Lectracode
			,AddName
			,AddDate
			,EditName
			,EditDate			)
		values(s.StyleUkey,
			s.Style_MarkerListUkey,
			s.PatternPanel,
			s.Lectracode,
			s.AddName,
			s.AddDate,
			s.EditName,
			s.EditDate			)
	when not matched by source  and t.styleUkey in (select distinct styleUkey from production.dbo.style_markerlist) then
		delete;

--------Style_MarkerList_SizeQty---------------------
	
	Merge Production.dbo.Style_MarkerList_SizeQty as t
	Using trade_to_pms.dbo.Style_MarkerList_SizeQty as s
	on t.Style_MarkerListUkey=s.Style_MarkerListUkey and t.SizeCode=s.SizeCode
	when matched then
		update set
		--t.Style_MarkerListUkey= s.Style_MarkerListUkey,
		t.StyleUkey= s.StyleUkey,
		--t.SizeCode= s.SizeCode,
		t.Qty= s.Qty
	when not matched by target then
		insert(Style_MarkerListUkey
				,StyleUkey
				,SizeCode
				,Qty				)
		values(s.Style_MarkerListUkey,
				s.StyleUkey,
				s.SizeCode,
				s.Qty				)
	when not matched by source and t.styleUkey in (select distinct styleUkey from production.dbo.style_markerlist) then
		delete;










--STYLE61
--Style_FabricCode
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_FabricCode
from Production.dbo.Style_FabricCode as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_FabricCode as b
on a.StyleUkey = b.StyleUkey AND a.LectraCode	= b.LectraCode
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.LectraCode	= b.LectraCode
a.FabricCode	= b.FabricCode
,a.PatternPanel	= b.PatternPanel
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
,a.Style_BOFUkey	= b.Style_BOFUkey
,a.QTWidth	= b.QTWidth
from Production.dbo.Style_FabricCode as a 
inner join Trade_To_Pms.dbo.Style_FabricCode as b ON a.StyleUkey = b.StyleUkey AND a.LectraCode	= b.LectraCode
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_FabricCode(
StyleUkey
,LectraCode
,FabricCode
,PatternPanel
,AddName
,AddDate
,EditName
,EditDate
,Style_BOFUkey
,QTWidth
)
select 
StyleUkey
,LectraCode
,FabricCode
,PatternPanel
,AddName
,AddDate
,EditName
,EditDate
,Style_BOFUkey
,QTWidth
from Trade_To_Pms.dbo.Style_FabricCode as b
where not exists(select 1 from Production.dbo.Style_FabricCode as a where a.StyleUkey = b.StyleUkey AND a.LectraCode	= b.LectraCode)
--STYLE8
--Style_BOF
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_BOF
from Production.dbo.Style_BOF as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_BOF as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
,a.FabricCode	= b.FabricCode
,a.Refno	= b.Refno
,a.SCIRefno	= b.SCIRefno
,a.Kind	= b.Kind
--,a.Ukey	= b.Ukey
,a.SuppIDBulk	= b.SuppIDBulk
,a.SuppIDSample	= b.SuppIDSample
,a.consPc = b.consPc

from Production.dbo.Style_BOF as a 
inner join Trade_To_Pms.dbo.Style_BOF as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_BOF(
StyleUkey
,FabricCode
,Refno
,SCIRefno
,Kind
,Ukey
,SuppIDBulk
,SuppIDSample
,consPc
)
select 
StyleUkey
,FabricCode
,Refno
,SCIRefno
,Kind
,Ukey
,SuppIDBulk
,SuppIDSample
,consPc
from Trade_To_Pms.dbo.Style_BOF as b
where not exists(select 1 from Production.dbo.Style_BOF as a where a.Ukey = b.Ukey)
--STYLE9
--Style_BOA
--Pms多的欄位
--,[BomTypeStyle]
--      ,[BomTypeArticle]
--      ,[BomTypeCustCD]
--      ,[BomTypeFactory]
--      ,[BomTypeBuyMonth]
--      ,[BomTypeCountry]
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_BOA
from Production.dbo.Style_BOA as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_BOA as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
--,a.Ukey	= b.Ukey
,a.Refno	= b.Refno
,a.SCIRefno	= b.SCIRefno
,a.SEQ1	= b.SEQ1
,a.ConsPC	= b.ConsPC
,a.PatternPanel	= b.PatternPanel
,a.SizeItem	= b.SizeItem
,a.ProvidedPatternRoom	= b.ProvidedPatternRoom
,a.Remark	= b.Remark
,a.ColorDetail	= b.ColorDetail
,a.IsCustCD	= b.IsCustCD
,a.BomTypeZipper	= b.BomTypeZipper
,a.BomTypeSize	= b.BomTypeSize
,a.BomTypeColor	= b.BomTypeColor
,a.BomTypePo	= b.BomTypePo
,a.SuppIDBulk	= b.SuppIDBulk
,a.SuppIDSample	= b.SuppIDSample
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Style_BOA as a 
inner join Trade_To_Pms.dbo.Style_BOA as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_BOA(
StyleUkey
,Ukey
,Refno
,SCIRefno
,SEQ1
,ConsPC
,PatternPanel
,SizeItem
,ProvidedPatternRoom
,Remark
,ColorDetail
,IsCustCD
,BomTypeZipper
,BomTypeSize
,BomTypeColor
,BomTypePo
,SuppIDBulk
,SuppIDSample
,AddName
,AddDate
,EditName
,EditDate

)
select 
StyleUkey
,Ukey
,Refno
,SCIRefno
,SEQ1
,ConsPC
,PatternPanel
,SizeItem
,ProvidedPatternRoom
,Remark
,ColorDetail
,IsCustCD
,BomTypeZipper
,BomTypeSize
,BomTypeColor
,BomTypePo
,SuppIDBulk
,SuppIDSample
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.Style_BOA as b
where not exists(select 1 from Production.dbo.Style_BOA as a where a.Ukey = b.Ukey)

-----------------------[Style_BOA_CustCD]-----------------------

	Merge Production.dbo.Style_BOA_CustCD as t
	Using Trade_To_Pms.dbo.Style_BOA_CustCD as s
	on t.Style_BOAUkey=s.Style_BOAUkey and t.CustCDID=s.CustCDID
	when matched then
		update set
		t.StyleUkey= s.StyleUkey,
		--t.Style_BOAUkey= s.Style_BOAUkey,
		--t.CustCDID= s.CustCDID,
		t.Refno= s.Refno,
		t.SCIRefno= s.SCIRefno,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
	when not matched by target then 
		insert(StyleUkey
				,Style_BOAUkey
				,CustCDID
				,Refno
				,SCIRefno
				,AddName
				,AddDate
				,EditName
				,EditDate				)
		values(s.StyleUkey,
				s.Style_BOAUkey,
				s.CustCDID,
				s.Refno,
				s.SCIRefno,
				s.AddName,
				s.AddDate,
				s.EditName,
				s.EditDate				)
	when not matched by source and t.styleUkey in (select distinct styleUkey from production.dbo.style_markerlist) then
		delete; 


------------------Style_BOA_KeyWord-------------------

	Merge Production.dbo.Style_BOA_KeyWord as t
	Using Trade_to_Pms.dbo.Style_BOA_KeyWord as s
	on t.Style_BOAUkey=s.Style_BOAUkey 
	when matched then
		update set 
		t.StyleUkey= s.StyleUkey,
		--t.Style_BOAUkey= s.Style_BOAUkey,
		--t.SEQ= '',
		--t.Prefix= s.Prefix,
		t.KeyWordID= s.KeyWordID
		--t.Postfix= s.Postfix,
		--t.Code= s.Code,
		--t.AddName= s.AddName,
		--t.AddDate= s.AddDate,
		--t.EditName= s.EditName,
		--t.EditDate= s.EditDate,
	when not matched by target then 
		insert(StyleUkey,Style_BOAUkey,KeyWordID)
		values(s.StyleUkey,s.Style_BOAUkey,s.KeyWordID)
	when not matched by source and t.styleUkey in (select distinct styleUkey from production.dbo.style_markerlist) then
		delete;



--STYLE6
--Style_ColorCombo
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_ColorCombo
from Production.dbo.Style_ColorCombo as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_ColorCombo as b
on a. StyleUkey	= b. StyleUkey AND a.Article	= b.Article AND a.LectraCode	= b.LectraCode
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
--a. StyleUkey	= b. StyleUkey
--,a.Article	= b.Article
a.ColorID	= b.ColorID
,a.FabricCode	= b.FabricCode
--,a.LectraCode	= b.LectraCode
,a.PatternPanel	= b.PatternPanel
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Style_ColorCombo as a 
inner join Trade_To_Pms.dbo.Style_ColorCombo as b ON a. StyleUkey	= b. StyleUkey AND a.Article	= b.Article AND a.LectraCode	= b.LectraCode
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_ColorCombo(
 StyleUkey
,Article
,ColorID
,FabricCode
,LectraCode
,PatternPanel
,AddName
,AddDate
,EditName
,EditDate
)
select 
 StyleUkey
,Article
,ColorID
,FabricCode
,LectraCode
,PatternPanel
,AddName
,AddDate
,EditName
,EditDate
from Trade_To_Pms.dbo.Style_ColorCombo as b
where not exists(select 1 from Production.dbo.Style_ColorCombo as a where a. StyleUkey	= b. StyleUkey AND a.Article	= b.Article AND a.LectraCode	= b.LectraCode)
--STYLEJ
--Style_HSCode
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_HSCode
from Production.dbo.Style_HSCode as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_HSCode as b
on a.UKEY = b.UKEY
where b.UKEY is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
--,a.UKEY	= b.UKEY
,a.Article	= b.Article
,a.CountryID	= b.CountryID
,a.Continent	= b.Continent
,a.HSCode1	= b.HSCode1
,a.HSCode2	= b.HSCode2
,a.CATNo1	= b.CATNo1
,a.CATNo2	= b.CATNo2
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Style_HSCode as a 
inner join Trade_To_Pms.dbo.Style_HSCode as b ON a.UKEY=b.UKEY
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_HSCode(
StyleUkey
,UKEY
,Article
,CountryID
,Continent
,HSCode1
,HSCode2
,CATNo1
,CATNo2
,AddName
,AddDate
,EditName
,EditDate

)
select 
StyleUkey
,UKEY
,Article
,CountryID
,Continent
,HSCode1
,HSCode2
,CATNo1
,CATNo2
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.Style_HSCode as b
where not exists(select 1 from Production.dbo.Style_HSCode as a where a.UKEY = b.UKEY)
--STYLEMI
--Style_MiAdidasColorCombo
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_MiAdidasColorCombo
from Production.dbo.Style_MiAdidasColorCombo as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b
on a.StyleUkey	= b.StyleUkey AND a.LectraCode	= b.LectraCode AND a.Ukey_old	= b.Ukey_old
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.LectraCode	= b.LectraCode
a.SetupID	= b.SetupID
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
--,a.Ukey_old	= b.Ukey_old

from Production.dbo.Style_MiAdidasColorCombo as a 
inner join Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b ON a.StyleUkey	= b.StyleUkey AND a.LectraCode	= b.LectraCode AND a.Ukey_old	= b.Ukey_old
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_MiAdidasColorCombo(
StyleUkey
,LectraCode
,SetupID
,AddName
,AddDate
,EditName
,EditDate
,Ukey_old
)
select 
StyleUkey
,LectraCode
,SetupID
,AddName
,AddDate
,EditName
,EditDate
,Ukey_old

from Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b
where not exists(select 1 from Production.dbo.Style_MiAdidasColorCombo as a where a.StyleUkey	= b.StyleUkey AND a.LectraCode	= b.LectraCode AND a.Ukey_old	= b.Ukey_old)
--STYLELT
--Style_GMTLTFty
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_GMTLTFty
from Production.dbo.Style_GMTLTFty as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_GMTLTFty as b
on a.StyleUkey	= b.StyleUkey AND a.FactoryID	= b.FactoryID
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.FactoryID	= b.FactoryID
a.GMTLT	= b.GMTLT
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
from Production.dbo.Style_GMTLTFty as a 
inner join Trade_To_Pms.dbo.Style_GMTLTFty as b ON a.StyleUkey	= b.StyleUkey AND a.FactoryID	= b.FactoryID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_GMTLTFty(
StyleUkey
,FactoryID
,GMTLT
,AddName
,AddDate
,EditName
,EditDate
)
select 
StyleUkey
,FactoryID
,GMTLT
,AddName
,AddDate
,EditName
,EditDate
from Trade_To_Pms.dbo.Style_GMTLTFty as b
where not exists(select 1 from Production.dbo.Style_GMTLTFty as a where a.StyleUkey	= b.StyleUkey AND a.FactoryID	= b.FactoryID)
--STYLEK
--Style_SimilarStyle
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_SimilarStyle
from Production.dbo.Style_SimilarStyle as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.MasterStyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_SimilarStyle as b
on a.MasterStyleUkey	= b.MasterStyleUkey AND a.ChildrenStyleUkey	= b.ChildrenStyleUkey
where b.MasterStyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.MasterBrandID	= b.MasterBrandID
,a.MasterStyleID	= b.MasterStyleID
,a.MasterSeasonID	= b.MasterSeasonID
--,a.MasterStyleUkey	= b.MasterStyleUkey
,a.ChildrenBrandID	= b.ChildrenBrandID
,a.ChildrenStyleID	= b.ChildrenStyleID
,a.ChildrenSeasonID	= b.ChildrenSeasonID
--,a.ChildrenStyleUkey	= b.ChildrenStyleUkey
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Style_SimilarStyle as a 
inner join Trade_To_Pms.dbo.Style_SimilarStyle as b ON a.MasterStyleUkey	= b.MasterStyleUkey AND a.ChildrenStyleUkey	= b.ChildrenStyleUkey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_SimilarStyle(
MasterBrandID
,MasterStyleID
,MasterSeasonID
,MasterStyleUkey
,ChildrenBrandID
,ChildrenStyleID
,ChildrenSeasonID
,ChildrenStyleUkey
,AddName
,AddDate
,EditName
,EditDate

)
select 
MasterBrandID
,MasterStyleID
,MasterSeasonID
,MasterStyleUkey
,ChildrenBrandID
,ChildrenStyleID
,ChildrenSeasonID
,ChildrenStyleUkey
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.Style_SimilarStyle as b
where not exists(select 1 from Production.dbo.Style_SimilarStyle as a where a.MasterStyleUkey	= b.MasterStyleUkey AND a.ChildrenStyleUkey	= b.ChildrenStyleUkey)



-----Style_ProductionKits-----2016/11/11-----
----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_ProductionKits
from Production.dbo.Style_ProductionKits as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_ProductionKits as b
on a.ukey= b.ukey AND a.FactoryID=B.FactoryID
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
,a.ProductionKitsGroup = b.ProductionKitsGroup
,a.MDivisionID = c.MDivisionID
,a.FactoryID = b.FactoryID
,a.Article = b.Article
,a.DOC = b.DOC
,a.SendDate = b.SendDate
,a.ReceiveDate = b.ReceiveDate
,a.ProvideDate = b.ProvideDate
,a.SendName = b.SendName
,a.FtyHandle = b.FtyHandle
,a.MRHandle = b.MRHandle
,a.SMR = b.SMR
,a.PoHandle = b.PoHandle
,a.POSMR = b.POSMR
,a.OrderId = b.OrderId
,a.SCIDelivery = b.SCIDelivery
,a.IsPF = b.IsPF
,a.BuyerDelivery = b.BuyerDelivery
,a.AddOrderId = b.AddOrderId
,a.AddSCIDelivery = b.AddSCIDelivery
,a.AddIsPF = b.AddIsPF
,a.AddBuyerDelivery = b.AddBuyerDelivery
,a.MRLastDate = b.MRLastDate
,a.FtyLastDate = b.FtyLastDate
,a.MRRemark = b.MRRemark
,a.FtyRemark = b.FtyRemark
,a.FtyList = b.FtyList
,a.ReasonID = b.ReasonID
--,a.SendToQA = b.SendToQA
--,a.QAReceived = b.QAReceived
,a.StyleCUkey1_Old = b.StyleCUkey1_Old
from Production.dbo.Style_ProductionKits as a 
inner join Trade_To_Pms.dbo.Style_ProductionKits as b ON a.ukey=b.ukey AND a.FactoryID=b.FactoryID
left join Trade_To_Pms.dbo.Factory as c ON c.ID=b.FactoryID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Style_ProductionKits(
Ukey
,StyleUkey
,ProductionKitsGroup
,MDivisionID
,FactoryID
,Article
,DOC
,SendDate
,ReceiveDate
,ProvideDate
,SendName
,FtyHandle
,MRHandle
,SMR
,PoHandle
,POSMR
,OrderId
,SCIDelivery
,IsPF
,BuyerDelivery
,AddOrderId
,AddSCIDelivery
,AddIsPF
,AddBuyerDelivery
,MRLastDate
,FtyLastDate
,MRRemark
,FtyRemark
,FtyList
,ReasonID
--,SendToQA
--,QAReceived
,StyleCUkey1_Old)

select 
b.Ukey
,b.StyleUkey
,b.ProductionKitsGroup
,c.MDivisionID
,b.FactoryID
,b.Article
,b.DOC
,b.SendDate
,b.ReceiveDate
,b.ProvideDate
,b.SendName
,b.FtyHandle
,b.MRHandle
,b.SMR
,b.PoHandle
,b.POSMR
,b.OrderId
,b.SCIDelivery
,b.IsPF
,b.BuyerDelivery
,b.AddOrderId
,b.AddSCIDelivery
,b.AddIsPF
,b.AddBuyerDelivery
,b.MRLastDate
,b.FtyLastDate
,b.MRRemark
,b.FtyRemark
,b.FtyList
,b.ReasonID
--,SendToQA
--,QAReceived
,b.StyleCUkey1_Old
from Trade_To_Pms.dbo.Style_ProductionKits as b
left join Trade_To_Pms.dbo.Factory as c ON c.ID=b.FactoryID
where not exists(select 1 from Production.dbo.Style_ProductionKits as a where a.ukey=b.ukey AND a.FactoryID=b.FactoryID)
	and b.FactoryID in (select id from Production.dbo.Factory)




END

