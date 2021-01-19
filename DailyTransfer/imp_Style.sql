
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
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_Location
from Production.dbo.Style_Location as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_Location as b
on a.StyleUkey= b.StyleUkey AND a.Location	= b.Location
where b.StyleUkey is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
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
	   b.StyleUkey
      ,b.Location
      ,b.Rate
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate
from Trade_To_Pms.dbo.Style_Location as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_Location as a WITH (NOLOCK) where a.StyleUkey=b.StyleUkey AND a.Location	= b.Location)

--  STYLE
--PMS多的欄位
--,[LocalMR]
--,[LocalStyle]
--,[PPMeeting]
--,[NoNeedPPMeeting]
--,[SampleApv]
--,[Type]
---------------------------UPDATE STYLE BY UKEY
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a.Ukey	= b.Ukey
,a.ProgramID	= b.ProgramID
,a.Model	= IsNull(b.Model,'')
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
,a.TPEEditName	= b.EditName
,a.TPEEditDate 	= b.EditDate
,a.SizeUnit	= b.SizeUnit
,a.ModularParent	= b.ModularParent
,a.CPUAdjusted	= b.CPUAdjusted
,a.LocalStyle = 0
,a.ThickFabric = b.ThickFabric
,a.DyeingID   = b.DyeingID
,a.ExpectionFormStatus   = b.ExpectionFormStatus
,a.ExpectionFormDate   = b.ExpectionFormDate
,a.ThickFabricBulk = b.ThickFabricBulk
,a.HangerPack	= b.HangerPack
,a.FitType	= b.FitType
,a.GearLine	= b.GearLine
from Production.dbo.Style as a 
inner join Trade_To_Pms.dbo.Style as b ON a.ID	= b.ID AND a.BrandID	= b.BrandID AND a.SeasonID	= b.SeasonID


RAISERROR('imp_Style - Starts',0,0)
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
delete Production.dbo.AutomationStyle
RAISERROR('imp_Style - Starts',0,0)
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
,TPEEditName
,TPEEditDate 
,SizeUnit
,ModularParent
,CPUAdjusted
,LocalStyle
,ThickFabric
,DyeingID
,Picture1
,Picture2
,ExpectionFormStatus
,ExpectionFormDate
,ThickFabricBulk
,HangerPack
,FitType
,GearLine
)
output	inserted.ID,
		inserted.SeasonID,
		inserted.BrandID
into AutomationStyle(ID, SeasonID, BrandID)
select 
 b.ID
,b.Ukey
,b.BrandID
,b.ProgramID
,b.SeasonID
,IsNull(b.Model,'')
,b.Description
,b.StyleName
,b.CdCodeID
,b.ApparelType
,b.FabricType
,b.Contents
,b.GMTLT
,b.CPU
,b.Factories
,b.FTYRemark
,b.Phase
,b.SampleSMR
,b.SampleMRHandle
,b.BulkSMR
,b.BulkMRHandle
,b.Junk
,b.RainwearTestPassed
,b.SizePage
,b.SizeRange
,b.Gender
,b.CTNQty
,b.StdCost
,b.Processes
,b.ArtworkCost
,b.Label
,b.Packing
,b.IETMSID
,b.IETMSVersion
,b.IEImportName
,b.IEImportDate
,b.ApvDate
,b.ApvName
,b.CareCode
,b.SpecialMark
,b.Lining
,b.StyleUnit
,b.ExpectionForm
,b.ExpectionFormRemark
,b.ComboType
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
,b.SizeUnit
,b.ModularParent
,b.CPUAdjusted
,0
,b.ThickFabric
,b.DyeingID
,b.Picture1
,b.Picture2
,b.ExpectionFormStatus
,b.ExpectionFormDate
,b.ThickFabricBulk
,b.HangerPack
,b.FitType
,b.GearLine
from Trade_To_Pms.dbo.Style as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Style as a WITH (NOLOCK) where a.ID=b.ID and a.BrandID=b.BrandID and a.SeasonID=b.SeasonID and a.LocalStyle=1)
AND not exists(select id from Production.dbo.Style as a WITH (NOLOCK) where a.Ukey=b.Ukey )


--Style4
--Style_TmsCost
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_TmsCost
from Production.dbo.Style_TmsCost as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_TmsCost as b
on a.StyleUkey= b.StyleUkey AND a.ArtworkTypeID	= b.ArtworkTypeID
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_Artwork
from Production.dbo.Style_Artwork as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_Artwork as b
on a.TradeUkey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
,a.SMNoticeID = b.SMNoticeID
,a.PatternVersion = b.PatternVersion
,a.PPU = b.PPU
,a.InkType = b.InkType
,a.Colors = b.Colors
,a.Length = b.Length
,a.Width = b.Width
,a.AntiMigration = isnull(b.AntiMigration,0)
,a.PrintType = b.PrintType
from Production.dbo.Style_Artwork as a 
inner join Trade_To_Pms.dbo.Style_Artwork as b ON a.TradeUkey=b.Ukey
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
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
,SMNoticeID
,PatternVersion
,PPU
,InkType
,Colors
,Length
,Width
,AntiMigration
,PrintType
)
select 
 b.StyleUkey
,b.ArtworkTypeID
,b.Article
,b.PatternCode
,b.PatternDesc
,b.ArtworkID
,b.ArtworkName
,b.Qty
,b.Price
,b.Cost
,b.Remark
--,Ukey
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
,b.TMS
,b.Ukey
,b.SMNoticeID
,b.PatternVersion
,b.PPU
,b.InkType
,b.Colors
,b.Length
,b.Width
,isnull(b.AntiMigration,0)
,b.PrintType
from Trade_To_Pms.dbo.Style_Artwork as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_Artwork as a WITH (NOLOCK) where a.TradeUkey = b.Ukey)


--STYLE1
--Style_QtyCTN
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
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
 b.StyleUkey
,b.CustCDID
,b.Qty
,b.CountryID
,b.Continent
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
,b.UKey

from Trade_To_Pms.dbo.Style_QtyCTN as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_QtyCTN as a WITH (NOLOCK) where a.UKey = b.UKey)

--STYLE5
--Style_SizeCode(需再確認ukey欄位)
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_SizeCode
from Production.dbo.Style_SizeCode as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_SizeCode as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_SizeCode(
StyleUkey
,Seq
,SizeGroup
,SizeCode
,UKey

)
select 
 b.StyleUkey
,b.Seq
,b.SizeGroup
,b.SizeCode
,b.UKey

from Trade_To_Pms.dbo.Style_SizeCode as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_SizeCode as a WITH (NOLOCK) where a.Ukey = b.Ukey)
and b.SizeCode is not null

--STYLE51
--Style_SizeTol
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_SizeTol
from Production.dbo.Style_SizeTol as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_SizeTol as b
on a.StyleUkey = b.StyleUkey and a.SizeGroup = b.SizeGroup and a.SizeItem = b.SizeItem
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a.Lower	= b.Lower
,a.Upper	= b.Upper
from Production.dbo.Style_SizeTol as a 
inner join Trade_To_Pms.dbo.Style_SizeTol as b ON a.StyleUkey = b.StyleUkey and a.SizeGroup = b.SizeGroup and a.SizeItem = b.SizeItem
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_SizeTol(
StyleUkey
,SizeGroup
,SizeItem
,Lower
,Upper
)
select 
 b.StyleUkey
,b.SizeGroup
,b.SizeItem
,b.Lower
,b.Upper
from Trade_To_Pms.dbo.Style_SizeTol as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_SizeTol as a WITH (NOLOCK) where a.StyleUkey = b.StyleUkey and a.SizeGroup = b.SizeGroup and a.SizeItem = b.SizeItem)

--STYLE52
--STYLE_SICESPEC(需再確認ukey欄位)
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_SizeSpec
from Production.dbo.Style_SizeSpec as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey =t.Ukey
left join Trade_To_Pms.dbo.Style_SizeSpec as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_SizeSpec(
StyleUkey
,SizeItem
,SizeCode
,SizeSpec
,UKey

)
select 
 b.StyleUkey
,b.SizeItem
,b.SizeCode
,b.SizeSpec
,b.UKey

from Trade_To_Pms.dbo.Style_SizeSpec as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_SizeSpec as a WITH (NOLOCK) where a.Ukey = b.Ukey)
--STYLEG
--STYLE_ARTICLE
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_Article
from Production.dbo.Style_Article as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_Article as b
on a.StyleUkey	= b.StyleUkey AND a.Article	= b.Article
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_Article(
StyleUkey
,Seq
,Article
,TissuePaper
,ArticleName
,Contents
)
select 
 b.StyleUkey
,b.Seq
,b.Article
,b.TissuePaper
,b.ArticleName
,b.Contents
from Trade_To_Pms.dbo.Style_Article as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_Article as a WITH (NOLOCK) where a.StyleUkey	= b.StyleUkey AND a.Article	= b.Article)

------------Style_Article_PadPrint----------------------Art
Merge Production.dbo.Style_Article_PadPrint as t
Using (select a.* from Trade_To_Pms.dbo.Style_Article_PadPrint a ) as s
on t.Styleukey=s.Styleukey and t.article=s.article and t.colorid = s.colorid
when matched then 
	update set 
		t.qty			= s.qty
when not matched by target then
	insert (
		Styleukey	, Article	, colorid,  qty
	) values (
		s.Styleukey , s.Article , s.colorid, s.qty
	)
when not matched by source AND T.Styleukey IN (SELECT ukey FROM Trade_To_Pms.dbo.Style) then  
	delete;
--STYLEA
--Style_MarkerList
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_MarkerList
from Production.dbo.Style_MarkerList as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_MarkerList as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a. StyleUkey	= b. StyleUkey
--,a.Ukey	= b.Ukey
,a.Seq	= b.Seq
,a.MarkerName	= b.MarkerName
,a.FabricCode	= b.FabricCode
,a.FabricCombo	= b.FabricCombo
,a.FabricPanelCode	= b.FabricPanelCode
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
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_MarkerList(
 StyleUkey
,Ukey
,Seq
,MarkerName
,FabricCode
,FabricCombo
,FabricPanelCode
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
 b.StyleUkey
,b.Ukey
,b.Seq
,b.MarkerName
,b.FabricCode
,b.FabricCombo
,b.FabricPanelCode
,b.MarkerLength
,b.ConsPC
,b.CuttingPiece
,b.ActCuttingPerimeter
,b.StraightLength
,b.CurvedLength
,b.Efficiency
,b.Remark
,b.MixedSizeMarker
,b.MarkerNo
,b.MarkerUpdate
,b.MarkerUpdateName
,b.AllSize
,b.PhaseID
,b.SMNoticeID
,b.MarkerVersion
,b.Direction
,b.CuttingWidth
,b.Width
,b.Type
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate

from Trade_To_Pms.dbo.Style_MarkerList as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_MarkerList as a WITH (NOLOCK) where a.Ukey = b.Ukey)

-----------------Style_MarkerList_Article-------------------
RAISERROR('imp_Style - Starts',0,0)
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
	RAISERROR('imp_Style - Starts',0,0)
	Merge Production.dbo.Style_MarkerList_PatternPanel as t
	Using trade_to_pms.dbo.Style_MarkerList_PatternPanel as s
	On t.Style_MarkerListUkey=s.Style_MarkerListUkey and t.FabricPanelCode=s.FabricPanelCode
	when matched then 
		update set
		t.StyleUkey= s.StyleUkey,
		--t.Style_MarkerListUkey= s.Style_MarkerListUkey,
		t.PatternPanel= s.PatternPanel,
		--t.FabricPanelCode= s.FabricPanelCode,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
	when not matched by target then 
		insert(StyleUkey
			,Style_MarkerListUkey
			,PatternPanel
			,FabricPanelCode
			,AddName
			,AddDate
			,EditName
			,EditDate			)
		values(s.StyleUkey,
			s.Style_MarkerListUkey,
			s.PatternPanel,
			s.FabricPanelCode,
			s.AddName,
			s.AddDate,
			s.EditName,
			s.EditDate			)
	when not matched by source  and t.styleUkey in (select distinct styleUkey from production.dbo.style_markerlist) then
		delete;

--------Style_MarkerList_SizeQty---------------------
	RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_FabricCode
from Production.dbo.Style_FabricCode as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_FabricCode as b
on a.StyleUkey = b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.FabricPanelCode	= b.FabricPanelCode
a.FabricCode	= b.FabricCode
,a.PatternPanel	= b.PatternPanel
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
,a.QTWidth	= b.QTWidth
from Production.dbo.Style_FabricCode as a 
inner join Trade_To_Pms.dbo.Style_FabricCode as b ON a.StyleUkey = b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_FabricCode(
StyleUkey
,FabricPanelCode
,FabricCode
,PatternPanel
,AddName
,AddDate
,EditName
,EditDate
,QTWidth
)
select 
 b.StyleUkey
,b.FabricPanelCode
,b.FabricCode
,b.PatternPanel
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
,b.QTWidth
from Trade_To_Pms.dbo.Style_FabricCode as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_FabricCode as a WITH (NOLOCK) where a.StyleUkey = b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode)
--STYLE8
--Style_BOF
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_BOF
from Production.dbo.Style_BOF as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_BOF as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
,a.MatchFabric = b.MatchFabric
,a.HRepeat = b.HRepeat
,a.VRepeat = b.VRepeat
,a.OneTwoWay = b.OneTwoWay
,a.HorizontalCutting = b.HorizontalCutting
,a.VRepeat_C = b.VRepeat_C
from Production.dbo.Style_BOF as a 
inner join Trade_To_Pms.dbo.Style_BOF as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
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
,MatchFabric
,HRepeat
,VRepeat
,OneTwoWay 
,HorizontalCutting
,VRepeat_C
)
select 
 b.StyleUkey
,b.FabricCode
,b.Refno
,b.SCIRefno
,b.Kind
,b.Ukey
,b.SuppIDBulk
,b.SuppIDSample
,b.consPc
,b.MatchFabric
,b.HRepeat
,b.VRepeat
,b.OneTwoWay 
,b.HorizontalCutting
,b.VRepeat_C
from Trade_To_Pms.dbo.Style_BOF as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_BOF as a WITH (NOLOCK) where a.Ukey = b.Ukey)
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
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_BOA
from Production.dbo.Style_BOA as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_BOA as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
,a.FabricPanelCode = b.FabricPanelCode

from Production.dbo.Style_BOA as a 
inner join Trade_To_Pms.dbo.Style_BOA as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
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
,FabricPanelCode
)
select 
 b.StyleUkey
,b.Ukey
,b.Refno
,b.SCIRefno
,b.SEQ1
,b.ConsPC
,b.PatternPanel
,b.SizeItem
,b.ProvidedPatternRoom
,b.Remark
,b.ColorDetail
,b.IsCustCD
,b.BomTypeZipper
,b.BomTypeSize
,b.BomTypeColor
,b.BomTypePo
,b.SuppIDBulk
,b.SuppIDSample
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
,b.FabricPanelCode
from Trade_To_Pms.dbo.Style_BOA as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_BOA as a WITH (NOLOCK) where a.Ukey = b.Ukey)

-----------------------[Style_BOA_CustCD]-----------------------
/*
	RAISERROR('imp_Style - Starts',0,0)
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

*/
------------------Style_BOA_KeyWord-------------------
	RAISERROR('imp_Style - Starts',0,0)
	Merge Production.dbo.Style_BOA_KeyWord as t
	Using Trade_to_Pms.dbo.Style_BOA_KeyWord as s
	on t.Style_BOAUkey=s.Style_BOAUkey and t.KeyWordID=s.KeyWordID 
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
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_ColorCombo
from Production.dbo.Style_ColorCombo as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_ColorCombo as b
on a. StyleUkey	= b. StyleUkey AND a.Article	= b.Article AND a.FabricPanelCode	= b.FabricPanelCode
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
--a. StyleUkey	= b. StyleUkey
--,a.Article	= b.Article
a.ColorID	= b.ColorID
,a.FabricCode	= b.FabricCode
--,a.FabricPanelCode	= b.FabricPanelCode
,a.PatternPanel	= b.PatternPanel
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Style_ColorCombo as a 
inner join Trade_To_Pms.dbo.Style_ColorCombo as b ON a. StyleUkey	= b. StyleUkey AND a.Article	= b.Article AND a.FabricPanelCode	= b.FabricPanelCode
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_ColorCombo(
 StyleUkey
,Article
,ColorID
,FabricCode
,FabricPanelCode
,PatternPanel
,AddName
,AddDate
,EditName
,EditDate
)
select 
 b.StyleUkey
,b.Article
,b.ColorID
,b.FabricCode
,b.FabricPanelCode
,b.PatternPanel
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
from Trade_To_Pms.dbo.Style_ColorCombo as b
where not exists(select 1 from Production.dbo.Style_ColorCombo as a WITH (NOLOCK) where a. StyleUkey	= b. StyleUkey AND a.Article	= b.Article AND a.FabricPanelCode	= b.FabricPanelCode)
--STYLEJ
--Style_HSCode
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_HSCode
from Production.dbo.Style_HSCode as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_HSCode as b
on a.UKEY = b.UKEY
where b.UKEY is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
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
 b.StyleUkey
,b.UKEY
,b.Article
,b.CountryID
,b.Continent
,b.HSCode1
,b.HSCode2
,b.CATNo1
,b.CATNo2
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate

from Trade_To_Pms.dbo.Style_HSCode as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_HSCode as a WITH (NOLOCK) where a.UKEY = b.UKEY)
--STYLEMI
--Style_MiAdidasColorCombo
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_MiAdidasColorCombo
from Production.dbo.Style_MiAdidasColorCombo as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b
on a.StyleUkey	= b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode AND a.Ukey_old	= b.Ukey_old
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.FabricPanelCode	= b.FabricPanelCode
a.SetupID	= b.SetupID
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
--,a.Ukey_old	= b.Ukey_old

from Production.dbo.Style_MiAdidasColorCombo as a 
inner join Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b ON a.StyleUkey	= b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode AND a.Ukey_old	= b.Ukey_old
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_MiAdidasColorCombo(
StyleUkey
,FabricPanelCode
,SetupID
,AddName
,AddDate
,EditName
,EditDate
,Ukey_old
)
select 
 b.StyleUkey
,b.FabricPanelCode
,b.SetupID
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
,b.Ukey_old

from Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_MiAdidasColorCombo as a WITH (NOLOCK) where a.StyleUkey	= b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode AND a.Ukey_old	= b.Ukey_old)
--STYLELT
--Style_GMTLTFty
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_GMTLTFty
from Production.dbo.Style_GMTLTFty as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_GMTLTFty as b
on a.StyleUkey	= b.StyleUkey AND a.FactoryID	= b.FactoryID
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
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
 b.StyleUkey
,b.FactoryID
,b.GMTLT
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate
from Trade_To_Pms.dbo.Style_GMTLTFty as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_GMTLTFty as a WITH (NOLOCK) where a.StyleUkey	= b.StyleUkey AND a.FactoryID	= b.FactoryID)
--STYLEK
--Style_SimilarStyle
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_SimilarStyle
from Production.dbo.Style_SimilarStyle as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.MasterStyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_SimilarStyle as b
on a.MasterStyleUkey	= b.MasterStyleUkey AND a.ChildrenStyleUkey	= b.ChildrenStyleUkey
where b.MasterStyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
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
RAISERROR('imp_Style - Starts',0,0)
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
 b.MasterBrandID
,b.MasterStyleID
,b.MasterSeasonID
,b.MasterStyleUkey
,b.ChildrenBrandID
,b.ChildrenStyleID
,b.ChildrenSeasonID
,b.ChildrenStyleUkey
,b.AddName
,b.AddDate
,b.EditName
,b.EditDate

from Trade_To_Pms.dbo.Style_SimilarStyle as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_SimilarStyle as a WITH (NOLOCK) where a.MasterStyleUkey	= b.MasterStyleUkey AND a.ChildrenStyleUkey	= b.ChildrenStyleUkey)

-----------------Style_SizeItem-------------------
RAISERROR('imp_Style - Starts',0,0)

Merge Production.dbo.Style_SizeItem as t
Using (select a.* from Trade_To_Pms.dbo.Style_SizeItem a ) as s
on t.Ukey=s.Ukey 
when matched then 
	update set 
		t.StyleUkey= s.StyleUkey,
		t.StyleUkey_Old= s.StyleUkey_Old,
		t.SizeItem= s.SizeItem,
		t.SizeUnit= s.SizeUnit,
		t.Description= s.Description,
		t.TolMinus= s.TolMinus,
		t.TolPlus= s.TolPlus
when not matched by target then
	insert (
		StyleUkey,StyleUkey_Old,SizeItem,SizeUnit,Description,Ukey,TolMinus,TolPlus	) 
		values (
		s.StyleUkey,s.StyleUkey_Old,s.SizeItem,s.SizeUnit,s.Description,s.Ukey,s.TolMinus,s.TolPlus	)
when not matched by source  AND T.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete;

-----------------Style_ThreadColorCombo_Operation-------------------
Merge Production.dbo.Style_ThreadColorCombo_Operation as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_Operation a ) as s
on t.Style_ThreadColorComboUkey=s.Style_ThreadColorComboUkey and t.Seq = s.Seq and t.OperationID = s.OperationID
when matched then 
	update set	t.ComboType	= s.ComboType	,
				t.Frequency	= s.Frequency	,
				t.AddName	= s.AddName		,
				t.AddDate	= s.AddDate		,
				t.EditName	= s.EditName	,
				t.EditDate	= s.EditDate	,
				t.Ukey		= s.Ukey
when not matched by target then
	insert (Style_ThreadColorComboUkey ,
			Seq						   ,
			OperationID				   ,
			ComboType				   ,
			Frequency				   ,
			AddName					   ,
			AddDate					   ,
			EditName				   ,
			EditDate				   ,
			Ukey) 
		values (s.Style_ThreadColorComboUkey ,
				s.Seq						   ,
				s.OperationID				   ,
				s.ComboType				   ,
				s.Frequency				   ,
				s.AddName					   ,
				s.AddDate					   ,
				s.EditName				   ,
				s.EditDate				   ,
				s.Ukey	)
when not matched by source then 
	delete;

-----------------Style_ThreadColorCombo_Detail-------------------
Merge Production.dbo.Style_ThreadColorCombo_Detail as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_Detail a ) as s
on t.Style_ThreadColorComboUkey=s.Style_ThreadColorComboUkey and t.Seq = s.Seq and t.Article = s.Article
when matched then 
	update set	t.SCIRefNo	= s.SCIRefNo   ,
				t.SuppId	= s.SuppId	   ,
				t.ColorID	= s.ColorID	   ,
				t.SuppColor	= s.SuppColor  ,
				t.AddName	= s.AddName	   ,
				t.AddDate	= s.AddDate	   ,
				t.EditName	= s.EditName   ,
				t.EditDate	= s.EditDate   ,
				t.Ukey		= s.Ukey
when not matched by target then
	insert (Style_ThreadColorComboUkey ,
			Seq						   ,
			SCIRefNo				   ,
			SuppId					   ,
			Article					   ,
			ColorID					   ,
			SuppColor				   ,
			AddName					   ,
			AddDate					   ,
			EditName				   ,
			EditDate				   ,
			Ukey
			) 
		values (s.Style_ThreadColorComboUkey ,
				s.Seq						   ,
				s.SCIRefNo				   ,
				s.SuppId					   ,
				s.Article					   ,
				s.ColorID					   ,
				s.SuppColor				   ,
				s.AddName					   ,
				s.AddDate					   ,
				s.EditName				   ,
				s.EditDate				   ,
				s.Ukey	)
when not matched by source then 
	delete;

-----------------Style_ThreadColorCombo-------------------
Merge Production.dbo.Style_ThreadColorCombo as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo a ) as s
on t.StyleUkey=s.StyleUkey and t.Thread_ComboID = s.Thread_ComboID and t.MachineTypeID = s.MachineTypeID
when matched then 
	update set	t.SeamLength	  = s.SeamLength	,
				t.ConsPC		  = s.ConsPC		,
				t.AddName		  = s.AddName		,
				t.AddDate		  = s.AddDate		,
				t.EditName		  = s.EditName		,
				t.EditDate		  = s.EditDate		,
				t.Ukey			  = s.Ukey
when not matched by target then
	insert (StyleUkey		,
			Thread_ComboID,
			MachineTypeID	,
			SeamLength	,
			ConsPC		,
			AddName		,
			AddDate		,
			EditName		,
			EditDate		,
			Ukey
			) 
		values (s.StyleUkey		,
				s.Thread_ComboID,
				s.MachineTypeID	,
				s.SeamLength	,
				s.ConsPC		,
				s.AddName		,
				s.AddDate		,
				s.EditName		,
				s.EditDate		,
				s.Ukey	)
when not matched by source then 
	delete;

END

