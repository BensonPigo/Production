
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
      a.Rate	      =isnull(b.Rate,0)
      ,a.AddName	      =isnull(b.AddName,'')
      ,a.AddDate	      =b.AddDate
      ,a.EditName	      =isnull(b.EditName,'')
      ,a.EditDate	      =b.EditDate
      ,a.ApparelType	  =isnull(b.ApparelType, '')
      ,a.FabricType	      =isnull(b.FabricType, '')
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
	  ,ApparelType
	  ,FabricType
)
select 
	   isnull(b.StyleUkey   ,0)
      ,isnull(b.Location    ,'')
      ,isnull(b.Rate        ,0)
      ,isnull(b.AddName     ,'')
      ,b.AddDate
      ,isnull(b.EditName    ,'')
      ,b.EditDate
	  ,isnull(b.ApparelType, '')
	  ,isnull(b.FabricType, '')
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
a.Ukey	= isnull( b.Ukey             ,0)
,a.ProgramID	= isnull( b.ProgramID,'')
,a.Model	= IsNull(b.Model,'')
,a.Description	= isnull( b.Description                   ,'')
,a.StyleName	= isnull( b.StyleName                     ,'')
,a.CdCodeID	= isnull( b.CdCodeID                          ,'')
,a.ApparelType	= isnull( b.ApparelType                   ,'')
,a.FabricType	= isnull( b.FabricType                    ,'')
,a.Contents	= isnull( b.Contents                          ,'')
,a.GMTLT	= isnull( b.GMTLT                             ,0)
,a.CPU	= isnull( b.CPU                                   ,0)
,a.Factories	= isnull( b.Factories                     ,'')
,a.FTYRemark	= isnull( b.FTYRemark                     ,'')
,a.Phase	= isnull( b.Phase                             ,'')
,a.SampleSMR	= isnull( b.SampleSMR                     ,'')
,a.SampleMRHandle	= isnull( b.SampleMRHandle            ,'')
,a.BulkSMR	= isnull( b.BulkSMR                           ,'')
,a.BulkMRHandle	= isnull( b.BulkMRHandle                  ,'')
,a.Junk	= isnull( b.Junk                                  ,0)
,a.RainwearTestPassed	= isnull( b.RainwearTestPassed    ,0)
,a.SizePage	= isnull( b.SizePage                          ,'')
,a.SizeRange	= isnull( b.SizeRange                     ,'')
,a.Gender	= isnull( b.Gender                            ,'')
,a.CTNQty	= isnull( b.CTNQty                            ,0)
,a.StdCost	= isnull( b.StdCost                           ,0)
,a.Processes	= isnull( b.Processes                     ,'')
,a.ArtworkCost	= isnull( b.ArtworkCost                   ,'')
,a.Picture1	= isnull( b.Picture1                          ,'')
,a.Picture2	= isnull( b.Picture2                          ,'')
,a.Label	= isnull( b.Label                             ,'')
,a.Packing	= isnull( b.Packing                           ,'')
,a.IETMSID	= isnull( b.IETMSID                           ,'')
,a.IETMSVersion	= isnull( b.IETMSVersion                  ,'')
,a.IEImportName	= isnull( b.IEImportName                  ,'')
,a.IEImportDate	= b.IEImportDate
,a.ApvDate	= b.ApvDate
,a.ApvName	= isnull( b.ApvName                           ,'')
,a.CareCode	= isnull( b.CareCode                          ,'')
,a.SpecialMark	= isnull( b.SpecialMark                   ,'')
,a.Lining	= isnull( b.Lining                            ,'')
,a.StyleUnit	= isnull( b.StyleUnit                     ,'')
,a.ExpectionForm	= isnull( b.ExpectionForm             ,0)
,a.ExpectionFormRemark	= isnull( b.ExpectionFormRemark   ,'')
,a.ComboType	= isnull( b.ComboType                     ,'')
,a.AddName	= isnull( b.AddName                           ,'')
,a.AddDate	= b.AddDate
,a.TPEEditName	= isnull( b.EditName                      ,'')
,a.TPEEditDate 	= b.EditDate
,a.SizeUnit	= isnull( b.SizeUnit                          ,'')
,a.ModularParent	= isnull( b.ModularParent             ,'')
,a.CPUAdjusted	= isnull( b.CPUAdjusted                   ,0)
,a.LocalStyle = 0
,a.ThickFabric = isnull( b.ThickFabric                    ,0)
,a.DyeingID   = isnull( b.DyeingID                        ,'')
,a.ExpectionFormStatus   = isnull( b.ExpectionFormStatus  ,'')
,a.ExpectionFormDate   =  b.ExpectionFormDate
,a.ThickFabricBulk = isnull( b.ThickFabricBulk            ,0)
,a.HangerPack	= isnull( b.HangerPack                    ,0)
,a.Construction	= isnull( b.Construction                  ,'')
,a.CDCodeNew	= isnull( b.CDCodeNew                     ,'')
,a.FitType	= isnull( b.FitType                           ,'')
,a.GearLine	= isnull( b.GearLine                          ,'')
,a.ThreadVersion = isnull( b.ThreadVersion                ,'')
,a.DevRegion = isnull( b.DevRegion                        ,'')
,a.DevOption = isnull( b.DevOption                        ,0)
,a.Teamwear = isnull(b.Teamwear, 0)
,a.BrandGender = isnull(b.BrandGender,'')
,a.Location = isnull(b.Location,'')
,a.NEWCO = isnull(b.NEWCO,'')
,a.AgeGroup = b.AgeGroup
,a.ThreadStatus = isnull(b.ThreadStatus,'')
,a.IETMSID_Thread = isnull(b.IETMSID_Thread,'')
,a.IETMSVersion_Thread = isnull(b.IETMSVersion_Thread,'')
,a.IsGSPPlus = isnull(b.IsGSPPlus, 0)
,a.TechConceptID = isnull(b.TechConceptID,'')
,a.CriticalStyle = isnull(b.CriticalStyle,'0')
from Production.dbo.Style as a 
inner join Trade_To_Pms.dbo.Style as b ON a.ID	= b.ID AND a.BrandID	= b.BrandID AND a.SeasonID	= b.SeasonID

-- Style_Artwork_Quot
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a.Ukey=isnull(b.Ukey, 0)
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
,Construction
,CDCodeNew
,FitType
,GearLine
,ThreadVersion
,DevRegion
,DevOption
,Teamwear
,BrandGender
,Location
,NEWCO
,AgeGroup
,ThreadStatus
,IETMSID_Thread
,IETMSVersion_Thread
,IsGSPPlus
,TechConceptID
,CriticalStyle
)
output	inserted.ID,
		inserted.SeasonID,
		inserted.BrandID
into AutomationStyle(ID, SeasonID, BrandID)
select 
 isnull(b.ID       ,'')
,isnull(b.Ukey     ,0)
,isnull(b.BrandID  ,'')
,isnull(b.ProgramID,'')
,isnull(b.SeasonID ,'')
,IsNull(b.Model,'')
,isnull(b.Description        ,'')
,isnull(b.StyleName          ,'')
,isnull(b.CdCodeID           ,'')
,isnull(b.ApparelType        ,'')
,isnull(b.FabricType         ,'')
,isnull(b.Contents           ,'')
,isnull(b.GMTLT              ,0)
,isnull(b.CPU                ,0)
,isnull(b.Factories          ,'')
,isnull(b.FTYRemark          ,'')
,isnull(b.Phase              ,'')
,isnull(b.SampleSMR          ,'')
,isnull(b.SampleMRHandle     ,'')
,isnull(b.BulkSMR            ,'')
,isnull(b.BulkMRHandle       ,'')
,isnull(b.Junk               ,0)
,isnull(b.RainwearTestPassed ,0)
,isnull(b.SizePage           ,'')
,isnull(b.SizeRange          ,'')
,isnull(b.Gender             ,'')
,isnull(b.CTNQty             ,0)
,isnull(b.StdCost            ,0)
,isnull(b.Processes          ,'')
,isnull(b.ArtworkCost        ,'')
,isnull(b.Label              ,'')
,isnull(b.Packing            ,'')
,isnull(b.IETMSID            ,'')
,isnull(b.IETMSVersion       ,'')
,isnull(b.IEImportName       ,'')
,b.IEImportDate
,b.ApvDate
,isnull(b.ApvName            ,'')
,isnull(b.CareCode           ,'')
,isnull(b.SpecialMark        ,'')
,isnull(b.Lining             ,'')
,isnull(b.StyleUnit          ,'')
,isnull(b.ExpectionForm      ,0)
,isnull(b.ExpectionFormRemark,'')
,isnull(b.ComboType          ,'')
,isnull(b.AddName            ,'')
,b.AddDate
,isnull(b.EditName           ,'')
,b.EditDate
,isnull(b.SizeUnit           ,'')
,isnull(b.ModularParent      ,'')
,isnull(b.CPUAdjusted        ,0)
,0
,isnull(b.ThickFabric        ,0)
,isnull(b.DyeingID           ,'')
,isnull(b.Picture1           ,'')
,isnull(b.Picture2           ,'')
,isnull(b.ExpectionFormStatus,'')
,b.ExpectionFormDate
,isnull(b.ThickFabricBulk    ,0)
,isnull(b.HangerPack         ,0)
,isnull(b.Construction       ,'')
,isnull(b.CDCodeNew          ,'')
,isnull(b.FitType            ,'')
,isnull(b.GearLine           ,'')
,isnull(b.ThreadVersion      ,'')
,isnull(b.DevRegion          ,'')
,isnull(b.DevOption          ,0)
,isnull(b.Teamwear,0)
,isnull(b.BrandGender,'')
,isnull(b.Location,'')
,isnull(b.NEWCO,'')
,b.AgeGroup
,isnull(b.ThreadStatus,'')
,isnull(b.IETMSID_Thread,'')
,isnull(b.IETMSVersion_Thread,'')
,isnull(b.IsGSPPlus, 0)
,isnull(b.TechConceptID, 0)
,isnull(b.CriticalStyle, '0')
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
a.Seq	= isnull( b.Seq                 ,'')
,a.Qty	= isnull( b.Qty                 ,0)
,a.ArtworkUnit	= isnull( b.ArtworkUnit ,'')
,a.TMS	= isnull( b.TMS                 ,0)
,a.Price	= isnull( b.Price           ,0)
,a.AddName	= isnull( b.AddName         ,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull( b.EditName        ,'')
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
 isnull(b.StyleUkey     ,0)
,isnull(b.ArtworkTypeID ,'')
,isnull(b.Seq           ,'')
,isnull(b.Qty           ,0)
,isnull(b.ArtworkUnit   ,'')
,isnull(b.TMS           ,0)
,isnull(b.Price         ,0)
,isnull(b.AddName       ,'')
,b.AddDate
,isnull(b.EditName,'')
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

-- 更新Style_Artwork_Quot.Price by ISP20230457
UPDATE a
SET  
a.Price = c1.Cost
from Production.dbo.Style_Artwork_Quot as a 
inner join Trade_To_Pms.dbo.Style as b ON a.StyleUkey = b.Ukey
inner join Production.dbo.Style_Artwork c on c.Ukey = a.Ukey
inner join Trade_To_Pms.dbo.Style_Artwork as c1 ON c.TradeUkey = c1.Ukey
inner join Production.dbo.LocalSupp l on a.LocalSuppId = l.ID
where c.Cost != c1.Cost
and l.IsSintexSubcon = 1

UPDATE a
SET  
 a.StyleUkey	= isnull( b.StyleUkey        ,0)
,a.ArtworkTypeID	= isnull( b.ArtworkTypeID,'')
,a.Article	= isnull( b.Article              ,'')
,a.PatternCode	= isnull( b.PatternCode      ,'')
,a.PatternDesc	= isnull( b.PatternDesc      ,'')
,a.ArtworkID	= isnull( b.ArtworkID        ,'')
,a.ArtworkName	= isnull( b.ArtworkName      ,'')
,a.Qty	= isnull( b.Qty                      ,0)
,a.Price	= isnull( b.Price                ,0)
,a.Cost	= isnull( b.Cost                     ,0)
,a.Remark	= isnull( b.Remark               ,'')
,a.AddName	= isnull( b.AddName              ,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull( b.EditName             ,'')
,a.EditDate	= b.EditDate
,a.TMS	= isnull( b.TMS                      ,0)
,a.SMNoticeID = isnull( b.SMNoticeID         ,'')
,a.PatternVersion = isnull( b.PatternVersion ,'')
,a.PPU = isnull( b.PPU                       ,0)
,a.InkType = isnull( b.InkType               ,'')
,a.Colors = isnull( b.Colors                 ,'')
,a.Length = isnull( b.Length                 ,0)
,a.Width = isnull( b.Width                   ,0)
,a.AntiMigration = isnull(b.AntiMigration,0)
,a.PrintType = isnull( b.PrintType                  ,'')
,a.PatternAnnotation = isnull( b.PatternAnnotation  ,'')
,a.InkTypePPU = isnull( b.InkTypePPU                ,'')
,a.PatternCodeSize = isnull( b.PatternCodeSize      ,'')
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
,PatternAnnotation
,InkTypePPU
,PatternCodeSize
)
select 
 isnull(b.StyleUkey     ,0)
,isnull(b.ArtworkTypeID ,'')
,isnull(b.Article       ,'')
,isnull(b.PatternCode   ,'')
,isnull(b.PatternDesc   ,'')
,isnull(b.ArtworkID     ,'')
,isnull(b.ArtworkName   ,'')
,isnull(b.Qty           ,0)
,isnull(b.Price         ,0)
,isnull(b.Cost          ,0)
,isnull(b.Remark        ,'')
--,Ukey
,isnull(b.AddName       ,'')
,b.AddDate
,isnull(b.EditName      ,'')
,b.EditDate
,isnull(b.TMS           ,0)
,isnull(b.Ukey          ,0)
,isnull(b.SMNoticeID    ,'')
,isnull(b.PatternVersion,'')
,isnull(b.PPU           ,0)
,isnull(b.InkType       ,'')
,isnull(b.Colors        ,'')
,isnull(b.Length        ,0)
,isnull(b.Width         ,0)
,isnull(b.AntiMigration,0)
,isnull(b.PrintType        ,'')
,isnull(b.PatternAnnotation,'')
,isnull(b.InkTypePPU       ,'')
,isnull(b.PatternCodeSize  ,'')
from Trade_To_Pms.dbo.Style_Artwork as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_Artwork as a WITH (NOLOCK) where a.TradeUkey = b.Ukey)


--STYLE1
--Style_QtyCTN
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
 a.StyleUkey	= isnull( b.StyleUkey,0)
,a.CustCDID	= isnull( b.CustCDID     ,'')
,a.Qty	= isnull( b.Qty              ,0)
,a.CountryID	= isnull( b.CountryID,'')
,a.Continent	= isnull( b.Continent,'')
,a.AddName	= isnull( b.AddName      ,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull( b.EditName,'')
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
 isnull(b.StyleUkey,0)
,isnull(b.CustCDID ,'')
,isnull(b.Qty      ,0)
,isnull(b.CountryID,'')
,isnull(b.Continent,'')
,isnull(b.AddName  ,'')
,b.AddDate
,isnull(b.EditName ,'')
,b.EditDate
,isnull(b.UKey     ,0)

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
a.StyleUkey	= isnull( b.StyleUkey    ,0)
,a.Seq	= isnull( b.Seq              ,'')
,a.SizeGroup	= isnull( b.SizeGroup,'')
,a.SizeCode	= isnull( b.SizeCode     ,'')
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
 isnull(b.StyleUkey,0)
,isnull(b.Seq      ,'')
,isnull(b.SizeGroup,'')
,isnull(b.SizeCode ,'')
,isnull(b.UKey     ,0)

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
a.Lower	= isnull(b.Lower, '')
,a.Upper	= isnull(b.Upper, '')
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
 isnull(b.StyleUkey,0)
,isnull(b.SizeGroup,'')
,isnull(b.SizeItem ,'')
,isnull(b.Lower    ,'')
,isnull(b.Upper    ,'')
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
a.StyleUkey	= isnull( b.StyleUkey,0)
,a.SizeItem	= isnull( b.SizeItem ,'')
,a.SizeCode	= isnull( b.SizeCode ,'')
,a.SizeSpec	= isnull( b.SizeSpec ,'')
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
 isnull(b.StyleUkey,0)
,isnull(b.SizeItem ,'')
,isnull(b.SizeCode ,'')
,isnull(b.SizeSpec ,'')
,isnull(b.UKey     ,0)

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
a.Seq	= isnull(b.Seq,0)
--,a.Article	= b.Article
,a.TissuePaper	= isnull(b.TissuePaper,0)
,a.ArticleName	= isnull(b.ArticleName,'')
,a.Contents	= isnull(b.Contents,'')
,a.CertificateNumber = isnull(b.CertificateNumber, '')
,a.SecurityCode = isnull(b.SecurityCode, '')
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
,SourceFile
,Description
,FDUploadDate
,BuyReadyDate
,CertificateNumber
,SecurityCode
)
select
 isnull(b.StyleUkey  ,0)
,isnull(b.Seq        ,0)
,isnull(b.Article    ,'')
,isnull(b.TissuePaper,0)
,isnull(b.ArticleName,'')
,isnull(b.Contents   ,'')
,isnull(b.SourceFile ,'')
,isnull(b.Description,'')
,b.FDUploadDate
,b.BuyReadyDate
,isnull(b.CertificateNumber, '')
,isnull(b.SecurityCode, '')
from Trade_To_Pms.dbo.Style_Article as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_Article as a WITH (NOLOCK) where a.StyleUkey	= b.StyleUkey AND a.Article	= b.Article)

------------Style_Article_PadPrint----------------------Art
Merge Production.dbo.Style_Article_PadPrint as t
Using (select a.* from Trade_To_Pms.dbo.Style_Article_PadPrint a ) as s
on t.Styleukey=s.Styleukey and t.article=s.article and t.colorid = s.colorid
when matched then 
	update set 
		t.qty			= isnull(s.qty,0)
when not matched by target then
	insert (
		Styleukey	, Article	, colorid,  qty
	)
    VALUES
    (
            isnull(s.styleukey ,0),
            isnull(s.article ,  ''),
            isnull(s.colorid,   ''),
            isnull(s.qty,       0)
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
a. StyleUkey	= isnull( b. StyleUkey                   ,0)
--,a.Ukey	= b.Ukey
,a.Seq	= isnull( b.Seq                                  ,0)
,a.MarkerName	= isnull( b.MarkerName                   ,'')
,a.FabricCode	= isnull( b.FabricCode                   ,'')
,a.FabricCombo	= isnull( b.FabricCombo                  ,'')
,a.FabricPanelCode	= isnull( b.FabricPanelCode          ,'')
,a.MarkerLength	= isnull( b.MarkerLength                 ,'')
,a.ConsPC	= isnull( b.ConsPC                           ,0)
,a.CuttingPiece	= isnull( b.CuttingPiece                 ,0)
,a.ActCuttingPerimeter	= isnull( b.ActCuttingPerimeter  ,'')
,a.StraightLength	= isnull( b.StraightLength           ,'')
,a.CurvedLength	= isnull( b.CurvedLength                 ,'')
,a.Efficiency	= isnull( b.Efficiency                   ,'')
,a.Remark	= isnull( b.Remark                           ,'')
,a.MixedSizeMarker	= isnull( b.MixedSizeMarker          ,'')
,a.MarkerNo	= isnull( b.MarkerNo                         ,'')
,a.MarkerUpdate	= b.MarkerUpdate
,a.MarkerUpdateName	= isnull( b.MarkerUpdateName         ,'')
,a.AllSize	= isnull( b.AllSize                          ,0)
,a.PhaseID	= isnull( b.PhaseID                          ,'')
,a.SMNoticeID	= isnull( b.SMNoticeID                   ,'')
,a.MarkerVersion	= isnull( b.MarkerVersion            ,'')
,a.Direction	= isnull( b.Direction                    ,'')
,a.CuttingWidth	= isnull( b.CuttingWidth                 ,'')
,a.Width	= isnull( b.Width                            ,'')
,a.Type	= isnull( b.Type                                 ,'')
,a.AddName	= isnull( b.AddName                          ,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull( b.EditName                         ,'')
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
 isnull(b.StyleUkey          ,0)
,isnull(b.Ukey               ,0)
,isnull(b.Seq                ,0)
,isnull(b.MarkerName         ,'')
,isnull(b.FabricCode         ,'')
,isnull(b.FabricCombo        ,'')
,isnull(b.FabricPanelCode    ,'')
,isnull(b.MarkerLength       ,'')
,isnull(b.ConsPC             ,0)
,isnull(b.CuttingPiece       ,0)
,isnull(b.ActCuttingPerimeter,'')
,isnull(b.StraightLength     ,'')
,isnull(b.CurvedLength       ,'')
,isnull(b.Efficiency         ,'')
,isnull(b.Remark             ,'')
,isnull(b.MixedSizeMarker    ,'')
,isnull(b.MarkerNo           ,'')
,b.MarkerUpdate
,isnull(b.MarkerUpdateName   ,'')
,isnull(b.AllSize            ,0)
,isnull(b.PhaseID            ,'')
,isnull(b.SMNoticeID         ,'')
,isnull(b.MarkerVersion      ,'')
,isnull(b.Direction          ,'')
,isnull(b.CuttingWidth       ,'')
,isnull(b.Width              ,'')
,isnull(b.Type               ,'')
,isnull(b.AddName            ,'')
,b.AddDate
,isnull(b.EditName,'')
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
		t.StyleUkey= isnull(s.StyleUkey,0),
		--t.Style_MarkerListUkey= s.Style_MarkerListUkey,
		--t.Article= s.Article,
		t.AddName= isnull(s.AddName,''),
		t.AddDate= s.AddDate,
		t.EditName= isnull(s.EditName,''),
		t.EditDate= s.EditDate
	when not matched by target then
		insert(StyleUkey
				,Style_MarkerListUkey
				,Article
				,AddName
				,AddDate
				,EditName
				,EditDate)
		values( isnull(s.StyleUkey,0),
				isnull(s.Style_MarkerListUkey,0),
				isnull(s.Article,''),
				isnull(s.AddName,''),
				s.AddDate,
				isnull(s.EditName,''),
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
		t.StyleUkey= isnull(s.StyleUkey,0),
		--t.Style_MarkerListUkey= s.Style_MarkerListUkey,
		t.PatternPanel= isnull(s.PatternPanel,''),
		--t.FabricPanelCode= s.FabricPanelCode,
		t.AddName= isnull(s.AddName,''),
		t.AddDate= s.AddDate,
		t.EditName= isnull(s.EditName,''),
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
		values(
            isnull(s.StyleUkey,0),
			isnull(s.Style_MarkerListUkey,0),
			isnull(s.PatternPanel,''),
			isnull(s.FabricPanelCode,''),
			isnull(s.AddName,''),
			s.AddDate,
			isnull(s.EditName,''),
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
		t.StyleUkey= isnull(s.StyleUkey,0),
		--t.SizeCode= s.SizeCode,
		t.Qty= isnull(s.Qty,0)
	when not matched by target then
		insert(Style_MarkerListUkey
				,StyleUkey
				,SizeCode
				,Qty				)
		values( isnull(s.Style_MarkerListUkey,0),
				isnull(s.StyleUkey,0),
				isnull(s.SizeCode,''),
				isnull(s.Qty,0)
                )
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
a.FabricCode	= isnull(b.FabricCode,'')
,a.PatternPanel	= isnull(b.PatternPanel,'')
,a.AddName	= isnull(b.AddName,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull(b.EditName,'')
,a.EditDate	= b.EditDate
,a.QTWidth	= isnull(b.QTWidth,0)
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
 isnull(b.StyleUkey       ,0)
,isnull(b.FabricPanelCode ,'')
,isnull(b.FabricCode      ,'')
,isnull(b.PatternPanel    ,'')
,isnull(b.AddName         ,'')
,b.AddDate
,isnull(b.EditName        ,'')
,b.EditDate
,isnull(b.QTWidth         ,0)
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
a.StyleUkey	= isnull( b.StyleUkey       ,0)
,a.FabricCode	= isnull( b.FabricCode  ,'')
,a.Refno	= isnull( b.Refno           ,'')
,a.SCIRefno	= isnull( b.SCIRefno        ,'')
,a.Kind	= isnull( b.Kind                ,'')
--,a.Ukey	= b.Ukey
,a.SuppIDBulk	= isnull( b.SuppIDBulk              ,'')
,a.SuppIDSample	= isnull( b.SuppIDSample            ,'')
,a.consPc = isnull( b.consPc                        ,0)
,a.MatchFabric = isnull( b.MatchFabric              ,'')
,a.HRepeat = isnull( b.HRepeat                      ,0)
,a.VRepeat = isnull( b.VRepeat                      ,0)
,a.OneTwoWay = isnull( b.OneTwoWay                  ,'')
,a.HorizontalCutting = isnull( b.HorizontalCutting  ,0)
,a.VRepeat_C = isnull( b.VRepeat_C                  ,0)
,a.Special = isnull(b.Special, 0)
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
,Special
)
select 
 isnull(b.StyleUkey        ,0)
,isnull(b.FabricCode       ,'')
,isnull(b.Refno            ,'')
,isnull(b.SCIRefno         ,'')
,isnull(b.Kind             ,'')
,isnull(b.Ukey             ,0)
,isnull(b.SuppIDBulk       ,'')
,isnull(b.SuppIDSample     ,'')
,isnull(b.consPc           ,0)
,isnull(b.MatchFabric      ,'')
,isnull(b.HRepeat          ,0)
,isnull(b.VRepeat          ,0)
,isnull(b.OneTwoWay        ,'')
,isnull(b.HorizontalCutting,0)
,isnull(b.VRepeat_C        ,0)
,isnull(b.Special, 0)
from Trade_To_Pms.dbo.Style_BOF as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_BOF as a WITH (NOLOCK) where a.Ukey = b.Ukey)
--STYLE9
--Style_BOA
--Pms多的欄位
--,[BomTypeStyle]
--      ,[BomTypeArticle]
--      ,[BomTypeCustCD]
--      ,[BomTypeFactory]
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
a.StyleUkey	= isnull( b.StyleUkey,0)
--,a.Ukey	= b.Ukey
,a.Refno	= isnull( b.Refno                          ,'')
,a.SCIRefno	= isnull( b.SCIRefno                       ,'')
,a.SEQ1	= isnull( b.SEQ1                               ,'')
,a.ConsPC	= isnull( b.ConsPC                         ,0)
,a.PatternPanel	= isnull( b.PatternPanel               ,'')
,a.SizeItem	= isnull( b.SizeItem                       ,'')
,a.ProvidedPatternRoom	= isnull( b.ProvidedPatternRoom,0)
,a.Remark	= isnull( b.Remark                         ,'')
,a.ColorDetail	= isnull( b.ColorDetail                ,'')
,a.IsCustCD	= isnull( b.IsCustCD                       ,0)
,a.BomTypeZipper	= isnull( b.BomTypeZipper          ,0)
,a.BomTypeSize	= isnull( b.BomTypeSize                ,0)
,a.BomTypeColor	= isnull( b.BomTypeColor               ,0)
,a.BomTypePo	= isnull( b.BomTypePo                  ,0)
,a.SuppIDBulk	= isnull( b.SuppIDBulk                 ,'')
,a.SuppIDSample	= isnull( b.SuppIDSample               ,'')
,a.AddName	= isnull( b.AddName,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull( b.EditName,'')
,a.EditDate	= b.EditDate
,a.FabricPanelCode = isnull( b.FabricPanelCode,'')
,a.BomTypeArticle          = isnull(b.BomTypeArticle          , 0)
,a.BomTypeCOO              = isnull(b.BomTypeCOO              , 0)
,a.BomTypeGender           = isnull(b.BomTypeGender           , 0)
,a.BomTypeCustomerSize     = isnull(b.BomTypeCustomerSize     , 0)
,a.CustomerSizeRelation    = isnull(b.CustomerSizeRelation    , '')
,a.BomTypeDecLabelSize     = isnull(b.BomTypeDecLabelSize     , 0)
,a.DecLabelSizeRelation    = isnull(b.DecLabelSizeRelation    , '')
,a.BomTypeBrandFactoryCode = isnull(b.BomTypeBrandFactoryCode , 0)
,a.BomTypeStyle            = isnull(b.BomTypeStyle            , 0)
,a.BomTypeStyleLocation    = isnull(b.BomTypeStyleLocation    , 0)
,a.BomTypeSeason           = isnull(b.BomTypeSeason           , 0)
,a.BomTypeCareCode         = isnull(b.BomTypeCareCode         , 0)
    , a.BomTypeBuyMonth = isnull(b.BomTypeBuyMonth,0)
    , a.BomTypeBuyerDlvMonth = isnull(b.BomTypeBuyerDlvMonth,0)

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
,BomTypeArticle
,BomTypeCOO
,BomTypeGender
,BomTypeCustomerSize
,CustomerSizeRelation
,BomTypeDecLabelSize
,DecLabelSizeRelation
,BomTypeBrandFactoryCode
,BomTypeStyle
,BomTypeStyleLocation
,BomTypeSeason
,BomTypeCareCode
    ,BomTypeBuyMonth
    ,BomTypeBuyerDlvMonth

)
select 
 isnull(b.StyleUkey           ,0)
,isnull(b.Ukey                ,0)
,isnull(b.Refno               ,'')
,isnull(b.SCIRefno            ,'')
,isnull(b.SEQ1                ,'')
,isnull(b.ConsPC              ,0)
,isnull(b.PatternPanel        ,'')
,isnull(b.SizeItem            ,'')
,isnull(b.ProvidedPatternRoom ,0)
,isnull(b.Remark              ,'')
,isnull(b.ColorDetail         ,'')
,isnull(b.IsCustCD            ,0)
,isnull(b.BomTypeZipper       ,0)
,isnull(b.BomTypeSize         ,0)
,isnull(b.BomTypeColor        ,0)
,isnull(b.BomTypePo           ,0)
,isnull(b.SuppIDBulk          ,'')
,isnull(b.SuppIDSample        ,'')
,isnull(b.AddName             ,'')
,b.AddDate
,isnull(b.EditName            ,'')
,b.EditDate
,isnull(b.FabricPanelCode     ,'')
,isnull(b.BomTypeArticle          , 0)
,isnull(b.BomTypeCOO              , 0)
,isnull(b.BomTypeGender           , 0)
,isnull(b.BomTypeCustomerSize     , 0)
,isnull(b.CustomerSizeRelation    , '')
,isnull(b.BomTypeDecLabelSize     , 0)
,isnull(b.DecLabelSizeRelation    , '')
,isnull(b.BomTypeBrandFactoryCode , 0)
,isnull(b.BomTypeStyle            , 0)
,isnull(b.BomTypeStyleLocation    , 0)
,isnull(b.BomTypeSeason           , 0)
,isnull(b.BomTypeCareCode         , 0)
    , isnull(b.BomTypeBuyMonth,0)
    , isnull(b.BomTypeBuyerDlvMonth,0)
from Trade_To_Pms.dbo.Style_BOA as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_BOA as a WITH (NOLOCK) where a.Ukey = b.Ukey)

----Style_BOA_Location
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
     [Location] = isnull(b.[Location], '')
    ,[AddName]  = isnull(b.[AddName] , '')
    ,[AddDate]  = b.[AddDate]
    ,[EditName] = isnull(b.[EditName], '')
    ,[EditDate] = b.[EditDate]
from Production.dbo.Style_BOA_Location as a 
inner join Trade_To_Pms.dbo.Style_BOA_Location as b ON a.StyleUkey=b.StyleUkey and a.Style_BOAUkey=b.Style_BOAUkey
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_BOA_Location
           ([StyleUkey]
           ,[Style_BOAUkey]
           ,[Location]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
            isnull([StyleUkey]    , 0)
           ,isnull([Style_BOAUkey], 0)
           ,isnull([Location]     , '')
           ,isnull([AddName]      , '')
           ,AddDate
           ,isnull([EditName]     , '')
           ,[EditDate]
from Trade_To_Pms.dbo.Style_BOA_Location as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_BOA_Location as a WITH (NOLOCK) where a.StyleUkey=b.StyleUkey and a.Style_BOAUkey=b.Style_BOAUkey)

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
		t.StyleUkey= isnull(s.StyleUkey,0),
		--t.Style_BOAUkey= s.Style_BOAUkey,
		--t.SEQ= '',
		--t.Prefix= s.Prefix,
		t.KeyWordID= isnull(s.KeyWordID,'')
		--t.Postfix= s.Postfix,
		--t.Code= s.Code,
		--t.AddName= s.AddName,
		--t.AddDate= s.AddDate,
		--t.EditName= s.EditName,
		--t.EditDate= s.EditDate,
	when not matched by target then 
		insert(StyleUkey,Style_BOAUkey,KeyWordID)
       VALUES
       (
              isnull(s.styleukey,0),
              isnull(s.style_boaukey,0),
              isnull(s.keywordid,'')
       )
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
a.ColorID	= isnull(b.ColorID,'')
,a.FabricCode	= isnull(b.FabricCode,'')
--,a.FabricPanelCode	= b.FabricPanelCode
,a.PatternPanel	= isnull(b.PatternPanel,'')
,a.AddName	= isnull(b.AddName,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull(b.EditName,'')
,a.EditDate	= b.EditDate
,a.FabricType = isnull(b.FabricType,'')
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
,FabricType
)
select 
 isnull(b.StyleUkey      ,0)
,isnull(b.Article        ,'')
,isnull(b.ColorID        ,'')
,isnull(b.FabricCode     ,'')
,isnull(b.FabricPanelCode,'')
,isnull(b.PatternPanel   ,'')
,isnull(b.AddName        ,'')
,b.AddDate
,isnull(b.EditName,'')
,b.EditDate
,isnull(b.FabricType,'')
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
a.StyleUkey	= isnull( b.StyleUkey,0)
--,a.UKEY	= b.UKEY
,a.Article	= isnull( b.Article,'')
,a.CountryID	= isnull( b.CountryID,'')
,a.Continent	= isnull( b.Continent,'')
,a.HSCode1	= isnull( b.HSCode1      ,'')
,a.HSCode2	= isnull( b.HSCode2      ,'')
,a.CATNo1	= isnull( b.CATNo1       ,'')
,a.CATNo2	= isnull( b.CATNo2       ,'')
,a.AddName	= isnull( b.AddName      ,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull( b.EditName     ,'')
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
 isnull(b.StyleUkey,0)
,isnull(b.UKEY     ,0)
,isnull(b.Article  ,'')
,isnull(b.CountryID,'')
,isnull(b.Continent,'')
,isnull(b.HSCode1  ,'')
,isnull(b.HSCode2  ,'')
,isnull(b.CATNo1   ,'')
,isnull(b.CATNo2   ,'')
,isnull(b.AddName  ,'')
,b.AddDate
,isnull(b.EditName,'')
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
on a.StyleUkey	= b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode 
where b.StyleUkey is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
--a.StyleUkey	= b.StyleUkey
--,a.FabricPanelCode	= b.FabricPanelCode
a.SetupID	= isnull(b.SetupID,'')
,a.AddName	= isnull(b.AddName,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull(b.EditName,'')
,a.EditDate	= b.EditDate

from Production.dbo.Style_MiAdidasColorCombo as a 
inner join Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b ON a.StyleUkey	= b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode 
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
)
select 
 isnull(b.StyleUkey       ,0)
,isnull(b.FabricPanelCode ,'')
,isnull(b.SetupID         ,'')
,isnull(b.AddName         ,'')
,b.AddDate
,isnull(b.EditName        ,'')
,b.EditDate

from Trade_To_Pms.dbo.Style_MiAdidasColorCombo as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_MiAdidasColorCombo as a WITH (NOLOCK) where a.StyleUkey	= b.StyleUkey AND a.FabricPanelCode	= b.FabricPanelCode)
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
 isnull(b.StyleUkey,0)
,isnull(b.FactoryID,'')
,isnull(b.GMTLT    ,0)
,isnull(b.AddName  ,'')
,b.AddDate
,isnull(b.EditName,'')
,b.EditDate
from Trade_To_Pms.dbo.Style_GMTLTFty as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_GMTLTFty as a WITH (NOLOCK) where a.StyleUkey	= b.StyleUkey AND a.FactoryID	= b.FactoryID)
--STYLEK
--Style_SimilarStyle
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_SimilarStyle
from Production.dbo.Style_SimilarStyle as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on t.Id = a.MasterStyleID
left join Trade_To_Pms.dbo.Style_SimilarStyle as b
on a.MasterBrandID	= b.MasterBrandID AND a.MasterStyleID	= b.MasterStyleID and a.ChildrenBrandID = b.ChildrenBrandID and a.ChildrenStyleID = b.ChildrenStyleID
where b.MasterStyleID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a.AddName	= isnull(b.AddName,'')
,a.AddDate	= b.AddDate
,a.EditName	= isnull(b.EditName,'')
,a.EditDate	= b.EditDate
from Production.dbo.Style_SimilarStyle as a 
inner join Trade_To_Pms.dbo.Style_SimilarStyle as b ON a.MasterBrandID	= b.MasterBrandID AND a.MasterStyleID	= b.MasterStyleID and a.ChildrenBrandID = b.ChildrenBrandID and a.ChildrenStyleID = b.ChildrenStyleID
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_SimilarStyle(
MasterBrandID
,MasterStyleID
,ChildrenBrandID
,ChildrenStyleID
,AddName
,AddDate
,EditName
,EditDate

)
select 
 isnull(b.MasterBrandID  ,'')
,isnull(b.MasterStyleID  ,'')
,isnull(b.ChildrenBrandID,'')
,isnull(b.ChildrenStyleID,'')
,isnull(b.AddName,'')
,b.AddDate
,isnull(b.EditName,'')
,b.EditDate
from Trade_To_Pms.dbo.Style_SimilarStyle as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_SimilarStyle as a WITH (NOLOCK) 
			where a.MasterBrandID	= b.MasterBrandID AND a.MasterStyleID	= b.MasterStyleID and a.ChildrenBrandID = b.ChildrenBrandID and a.ChildrenStyleID = b.ChildrenStyleID)

-----------------Style_SizeItem-------------------
RAISERROR('imp_Style - Starts',0,0)

Merge Production.dbo.Style_SizeItem as t
Using (select a.* from Trade_To_Pms.dbo.Style_SizeItem a ) as s
on t.Ukey=s.Ukey 
when matched then 
	update set 
		t.StyleUkey= isnull( s.StyleUkey,        0),
		t.SizeItem= isnull( s.SizeItem,          ''),
		t.SizeUnit= isnull( s.SizeUnit,          ''),
		t.Description= isnull( s.Description,    ''),
		t.TolMinus= isnull( s.TolMinus,          ''),
		t.TolPlus= isnull( s.TolPlus,            '')
when not matched by target then
	insert (
		StyleUkey,SizeItem,SizeUnit,Description,Ukey,TolMinus,TolPlus	) 
		values (
              isnull(s.styleukey,     0),
              isnull(s.sizeitem,      ''),
              isnull(s.sizeunit,      ''),
              isnull(s.description,   ''),
              isnull(s.ukey,          0),
              isnull(s.tolminus,      ''),
              isnull(s.tolplus,       '')
              )
when not matched by source  AND T.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete;

-----------------Style_ThreadColorCombo_Detail-------------------
Merge Production.dbo.Style_ThreadColorCombo_Detail as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_Detail a ) as s
on t.Style_ThreadColorComboUkey=s.Style_ThreadColorComboUkey and t.Seq = s.Seq and t.Article = s.Article
when matched then 
	update set	t.SCIRefNo	= isnull( s.SCIRefNo   ,''),
				t.SuppId	= isnull( s.SuppId	   ,''),
				t.ColorID	= isnull( s.ColorID	   ,''),
				t.SuppColor	= isnull( s.SuppColor  ,''),
				t.AddName	= isnull( s.AddName	   ,''),
				t.AddDate	= s.AddDate	   ,
				t.EditName	= isnull( s.EditName   ,''),
				t.EditDate	= s.EditDate   ,
				t.Ukey		= isnull( s.Ukey,0)
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
		values (
                 isnull(s.Style_ThreadColorComboUkey,0)
				,isnull(s.Seq                       ,'')
				,isnull(s.SCIRefNo                  ,'')
				,isnull(s.SuppId                    ,'')
				,isnull(s.Article                   ,'')
				,isnull(s.ColorID                   ,'')
				,isnull(s.SuppColor                 ,'')
				,isnull(s.AddName                   ,'')
				,s.AddDate
				,isnull(s.EditName                  ,'')
				,s.EditDate
				,isnull(s.Ukey                      ,0)
                )
;
----刪除條件：Trade不存在，且表頭還存在
DELETE t
FROM Production.dbo.Style_ThreadColorCombo_Detail t
WHERE NOT EXISTS(
	SELECT 1 FROM Trade_To_Pms.dbo.Style_ThreadColorCombo_Detail s
	WHERE t.Style_ThreadColorComboUkey=s.Style_ThreadColorComboUkey AND t.Seq = s.Seq AND t.Article = s.Article
)
AND EXISTS(
	SELECT 1 
	FROM Production.dbo.Style_ThreadColorCombo st
	INNER JOIN Trade_To_Pms.dbo.Style s ON st.StyleUkey = s.Ukey
	WHERE st.Ukey = t.Style_ThreadColorComboUkey
)

-----------------Style_ThreadColorCombo_Operation-------------------
Merge Production.dbo.Style_ThreadColorCombo_Operation as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_Operation a ) as s
on t.Style_ThreadColorComboUkey=s.Style_ThreadColorComboUkey and t.Seq = s.Seq and t.OperationID = s.OperationID
when matched then 
	update set	t.ComboType	= isnull( s.ComboType	,''),
				t.Frequency	= isnull( s.Frequency	,0),
				t.AddName	= isnull( s.AddName		,''),
				t.AddDate	= s.AddDate		,
				t.EditName	= isnull( s.EditName	,''),
				t.EditDate	= s.EditDate	,
				t.Ukey		= isnull( s.Ukey,0)
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
		values (isnull(s.Style_ThreadColorComboUkey ,       0),
				isnull(s.Seq						   ,    ''),
				isnull(s.OperationID				   ,    ''),
				isnull(s.ComboType				   ,        ''),
				isnull(s.Frequency				   ,        0),
				isnull(s.AddName					   ,    ''),
				s.AddDate					   ,
				isnull(s.EditName				   ,''),
				s.EditDate				   ,
				isnull(s.Ukey,0)
                )
;
----刪除條件：Trade不存在，且表頭還存在
DELETE t
FROM Production.dbo.Style_ThreadColorCombo_Operation t
WHERE NOT EXISTS(
	SELECT 1 FROM Trade_To_Pms.dbo.Style_ThreadColorCombo_Operation s
	WHERE t.Style_ThreadColorComboUkey=s.Style_ThreadColorComboUkey and t.Seq = s.Seq and t.OperationID = s.OperationID
)
AND EXISTS(
	SELECT 1 
	FROM Production.dbo.Style_ThreadColorCombo st
	INNER JOIN Trade_To_Pms.dbo.Style s ON st.StyleUkey = s.Ukey
	WHERE st.Ukey = t.Style_ThreadColorComboUkey
)


-----------------Style_ThreadColorCombo-------------------
Merge Production.dbo.Style_ThreadColorCombo as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo a ) as s
on t.StyleUkey=s.StyleUkey and t.Thread_ComboID = s.Thread_ComboID and t.MachineTypeID = s.MachineTypeID
when matched then 
	update set	t.SeamLength	  = isnull(s.SeamLength	,0),
				t.ConsPC		  = isnull(s.ConsPC		,0),
				t.AddName		  = isnull(s.AddName	,''),
				t.AddDate		  = s.AddDate	,
				t.EditName		  = isnull(s.EditName	,''),
				t.EditDate		  = s.EditDate	,
				t.Ukey			  = isnull(s.Ukey,0)
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
		values (isnull(s.StyleUkey		,0),
				isnull(s.Thread_ComboID, ''),
				isnull(s.MachineTypeID	,''),
				isnull(s.SeamLength	,    0),
				isnull(s.ConsPC		,    0),
				isnull(s.AddName		,''),
				s.AddDate,
				isnull(s.EditName		,''),
				s.EditDate,
				isnull(s.Ukey,           '')
                )
when not matched by source AND t.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete;
	

-----------------Style_ThreadColorCombo_History_Detail-------------------
Merge Production.dbo.Style_ThreadColorCombo_History_Detail as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_History_Detail a ) as s
on t.Style_ThreadColorCombo_HistoryUkey=s.Style_ThreadColorCombo_HistoryUkey 
	and t.Seq = s.Seq 
	and t.Article = s.Article 
when matched then 
   update SET t.SCIRefNo = isnull( s.SCIRefNo         ,'')
      ,t.SuppId = isnull( s.SuppId                    ,'')
      ,t.ColorID = isnull( s.ColorID                  ,'')
      ,t.SuppColor = isnull( s.SuppColor              ,'')
      ,t.AddName = isnull( s.AddName                  ,'')
      ,t.AddDate =  s.AddDate
      ,t.EditName = isnull( s.EditName                ,'')
      ,t.EditDate =  s.EditDate
      ,t.UseRatio = isnull( s.UseRatio                ,0)
      ,t.Ukey = isnull( s.Ukey                        ,0)
      ,t.Allowance = isnull( s.Allowance              ,0)
      ,t.AllowanceTubular = isnull( s.AllowanceTubular,0)
      ,t.UseRatioHem = isnull(s.UseRatioHem, 0)
when not matched by target then
	INSERT (Style_ThreadColorCombo_HistoryUkey
           ,Seq
           ,SCIRefNo
           ,SuppId
           ,Article
           ,ColorID
           ,SuppColor
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
           ,UseRatio
		   ,Ukey
           ,Allowance
           ,AllowanceTubular
           ,UseRatioHem)
		VALUES  (
            isnull(s.Style_ThreadColorCombo_HistoryUkey,0)
           ,isnull(s.Seq                               ,'')
           ,isnull(s.SCIRefNo                          ,'')
           ,isnull(s.SuppId                            ,'')
           ,isnull(s.Article                           ,'')
           ,isnull(s.ColorID                           ,'')
           ,isnull(s.SuppColor                         ,'')
           ,isnull(s.AddName                           ,'')
           ,s.AddDate
           ,isnull(s.EditName                          ,'')
           ,s.EditDate
           ,isnull(s.UseRatio                          ,0)
		   ,isnull(s.Ukey                              ,0)
           ,isnull(s.Allowance                         ,0)
           ,isnull(s.AllowanceTubular                  ,0)
           ,isnull(s.UseRatioHem, 0)
		   )
;

----刪除條件：Trade不存在，且表頭還存在
DELETE t
FROM Production.dbo.Style_ThreadColorCombo_History_Detail t
WHERE NOT EXISTS(
	SELECT 1 FROM Trade_To_Pms.dbo.Style_ThreadColorCombo_History_Detail s
	WHERE t.Style_ThreadColorCombo_HistoryUkey=s.Style_ThreadColorCombo_HistoryUkey 	
	AND t.Seq = s.Seq 
	AND t.Article = s.Article 
)
AND EXISTS(
	SELECT 1 
	FROM Production.dbo.Style_ThreadColorCombo_History st
	INNER JOIN Trade_To_Pms.dbo.Style s ON st.StyleUkey = s.Ukey
	WHERE st.Ukey = t.Style_ThreadColorCombo_HistoryUkey
)

-----------------Style_ThreadColorCombo_History_Operation-------------------
Merge Production.dbo.Style_ThreadColorCombo_History_Operation as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_History_Operation a ) as s
on t.Style_ThreadColorCombo_HistoryUkey=s.Style_ThreadColorCombo_HistoryUkey 
	and t.Seq = s.Seq 
	and t.OperationID = s.OperationID 
when matched then 
   update SET t.ComboType = isnull( s.ComboType     ,'')
      ,t.Frequency = isnull( s.Frequency            ,0)
      ,t.AddName = isnull( s.AddName                ,'')
      ,t.AddDate =  s.AddDate
      ,t.EditName = isnull( s.EditName              ,'')
      ,t.EditDate =  s.EditDate
      ,t.Ukey = isnull( s.Ukey                      ,0)
	  ,t.MachineTypeHem = isnull( s.MachineTypeHem  ,0)
	  ,t.OperationHem = isnull( s.OperationHem      ,0)
	  ,t.Tubular = isnull( s.Tubular                ,0)
	  ,t.Segment = isnull( s.Segment                ,0)
	  ,t.SeamLength = isnull( s.SeamLength          ,0)
when not matched by target then
	INSERT (Style_ThreadColorCombo_HistoryUkey
           ,Seq
           ,OperationID
           ,ComboType
           ,Frequency
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
		   ,Ukey
		   ,MachineTypeHem
		   ,OperationHem
		   ,Tubular
		   ,Segment
		   ,SeamLength)
		VALUES  (
            isnull(s.Style_ThreadColorCombo_HistoryUkey,0)
           ,isnull(s.Seq                               ,'')
           ,isnull(s.OperationID                       ,'')
           ,isnull(s.ComboType                         ,'')
           ,isnull(s.Frequency                         ,0)
           ,isnull(s.AddName                           ,'')
           ,s.AddDate
           ,isnull(s.EditName                          ,'')
           ,s.EditDate
		   ,isnull(s.Ukey                              ,0)
		   ,isnull(s.MachineTypeHem                    ,0)
		   ,isnull(s.OperationHem                      ,0)
		   ,isnull(s.Tubular                           ,0)
		   ,isnull(s.Segment                           ,0)
		   ,isnull(s.SeamLength                        ,0)
           )
;
----刪除條件：Trade不存在，且表頭還存在
DELETE t
FROM Production.dbo.Style_ThreadColorCombo_History_Operation t
WHERE NOT EXISTS(
	SELECT 1 FROM Trade_To_Pms.dbo.Style_ThreadColorCombo_History_Operation s
	WHERE t.Style_ThreadColorCombo_HistoryUkey=s.Style_ThreadColorCombo_HistoryUkey 
	AND t.Seq = s.Seq 
	AND t.OperationID = s.OperationID 
)
AND EXISTS(
	SELECT 1 
	FROM Production.dbo.Style_ThreadColorCombo_History st
	INNER JOIN Trade_To_Pms.dbo.Style s ON st.StyleUkey = s.Ukey
	WHERE st.Ukey = t.Style_ThreadColorCombo_HistoryUkey
)
-----------------Style_ThreadColorCombo_History_Version-------------------
Merge Production.dbo.Style_ThreadColorCombo_History_Version as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_History_Version a ) as s
on t.Ukey=s.Ukey 
when matched then 
   update SET t.StyleUkey = isnull( s.StyleUkey             ,0)
      ,t.Version = isnull( s.Version                        ,'')
      ,t.UseRatioRule = isnull( s.UseRatioRule              ,'')
      ,t.ThickFabricBulk = isnull( s.ThickFabricBulk        ,0)
      ,t.FarmOutQuilting = isnull( s.FarmOutQuilting        ,0)
      ,t.LockHandle = isnull( s.LockHandle                  ,'')
      ,t.LockDate = s.LockDate
      ,t.Category = isnull( s.Category                      ,'')
      ,t.TPDate =  s.TPDate
      ,t.IETMSID_Thread = isnull( s.IETMSID_Thread          ,'')
      ,t.IETMSVersion_Thread = isnull( s.IETMSVersion_Thread,'')
	  ,t.AddName = isnull( s.AddName                        ,'')
	  ,t.AddDate = s.AddDate
	  ,t.VersionCOO = isnull(s.VersionCOO, '')
when not matched by target then
	INSERT (StyleUkey
           ,Version
           ,UseRatioRule
           ,ThickFabricBulk
           ,FarmOutQuilting
           ,LockHandle
           ,LockDate
           ,Category
           ,TPDate
           ,IETMSID_Thread
           ,IETMSVersion_Thread
		   ,AddName
		   ,AddDate
           ,VersionCOO)
		VALUES (
			isnull(s.StyleUkey          ,0)
           ,isnull(s.Version            ,'')
           ,isnull(s.UseRatioRule       ,'')
           ,isnull(s.ThickFabricBulk    ,0)
           ,isnull(s.FarmOutQuilting    ,0)
           ,isnull(s.LockHandle         ,'')
           ,s.LockDate
           ,isnull(s.Category           ,'')
           ,s.TPDate
           ,isnull(s.IETMSID_Thread     ,'')
           ,isnull(s.IETMSVersion_Thread,'')
		   ,isnull(s.AddName            ,'')
		   ,s.AddDate
           ,isnull(s.VersionCOO, '')
           )
when not matched by source AND t.StyleUkey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete
;

-----------------Style_ThreadColorCombo_History-------------------
Merge Production.dbo.Style_ThreadColorCombo_History as t
Using (select a.* from Trade_To_Pms.dbo.Style_ThreadColorCombo_History a ) as s
on t.StyleUkey=s.StyleUkey 
	and t.Thread_ComboID = s.Thread_ComboID 
	and t.MachineTypeID = s.MachineTypeID 
	and t.LockDate = s.LockDate
when matched then 
   update SET t.SeamLength = isnull( s.SeamLength           ,0)
      ,t.ConsPC = isnull( s.ConsPC                          ,0)
      ,t.AddName = isnull( s.AddName                        ,'')
      ,t.AddDate = s.AddDate
      ,t.EditName = isnull( s.EditName                      ,'')
      ,t.EditDate = s.EditDate
      ,t.Category = isnull( s.Category                      ,'')
      ,t.Ukey = isnull( s.Ukey                              ,0)
      ,t.TPDate =  s.TPDate
      ,t.IETMSID_Thread = isnull( s.IETMSID_Thread          ,'')
      ,t.IETMSVersion_Thread = isnull( s.IETMSVersion_Thread,'')
	  ,t.Version = isnull( s.Version                        ,'')
when not matched by target then
	INSERT (StyleUkey
           ,Thread_ComboID
           ,MachineTypeID
           ,SeamLength
           ,ConsPC
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
           ,Ukey
           ,LockDate
           ,Category
           ,TPDate
           ,IETMSID_Thread
           ,IETMSVersion_Thread
		   ,Version)
		VALUES (
			isnull(s.StyleUkey          ,0)
           ,isnull(s.Thread_ComboID     ,'')
           ,isnull(s.MachineTypeID      ,'')
           ,isnull(s.SeamLength         ,0)
           ,isnull(s.ConsPC             ,0)
           ,isnull(s.AddName            ,'')
           ,s.AddDate
           ,isnull(s.EditName           ,'')
           ,s.EditDate
           ,isnull(s.Ukey               ,0)
           ,s.LockDate
           ,isnull(s.Category           ,'')
           ,s.TPDate
           ,isnull(s.IETMSID_Thread     ,'')
           ,isnull(s.IETMSVersion_Thread,'')
		   ,isnull(s.Version,'')
           )
when not matched by source AND t.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete
;

-----------------Style_QTThreadColorCombo_History_Detail-------------------
Merge Production.dbo.Style_QTThreadColorCombo_History_Detail as t
Using (select a.* from Trade_To_Pms.dbo.Style_QTThreadColorCombo_History_Detail a ) as s
on t.Style_QTThreadColorCombo_HistoryUkey = s.Style_QTThreadColorCombo_HistoryUkey 
	AND t.Seq = s.Seq 
	AND t.Article =s.Article 
when matched then 
	update set t.SCIRefNo = isnull( s.SCIRefNo,'')
      ,t.SuppId = isnull( s.SuppId            ,'')
      ,t.ColorID = isnull( s.ColorID          ,'')
      ,t.SuppColor = isnull( s.SuppColor      ,'')
      ,t.AddName = isnull( s.AddName          ,'')
      ,t.AddDate =  s.AddDate
      ,t.EditName = isnull( s.EditName        ,'')
      ,t.EditDate = s.EditDate
      ,t.Ratio = isnull( s.Ratio              ,0)
      ,t.Ukey = isnull( s.Ukey                ,0)
when not matched by target then
	insert  (Style_QTThreadColorCombo_HistoryUkey
           ,Seq
           ,SCIRefNo
           ,SuppId
           ,Article
           ,ColorID
           ,SuppColor
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
           ,Ratio
           ,Ukey)
		values  (
            isnull(s.Style_QTThreadColorCombo_HistoryUkey,0)
           ,isnull(s.Seq                                 ,'')
           ,isnull(s.SCIRefNo                            ,'')
           ,isnull(s.SuppId                              ,'')
           ,isnull(s.Article                             ,'')
           ,isnull(s.ColorID                             ,'')
           ,isnull(s.SuppColor                           ,'')
           ,isnull(s.AddName                             ,'')
           ,s.AddDate
           ,isnull(s.EditName                            ,'')
           ,s.EditDate
           ,isnull(s.Ratio                               ,0)
           ,isnull(s.Ukey                                ,0)
           )
;

----刪除條件：Trade不存在，且表頭還存在
DELETE t
FROM Production.dbo.Style_QTThreadColorCombo_History_Detail t
WHERE NOT EXISTS(
	SELECT 1 FROM Trade_To_Pms.dbo.Style_QTThreadColorCombo_History_Detail s
	WHERE t.Style_QTThreadColorCombo_HistoryUkey = s.Style_QTThreadColorCombo_HistoryUkey AND t.Seq = s.Seq AND t.Article =s.Article 
)
AND EXISTS(
	SELECT 1 
	FROM Production.dbo.Style_QTThreadColorCombo_History st
	INNER JOIN Trade_To_Pms.dbo.Style s ON st.StyleUkey = s.Ukey
	WHERE st.Ukey = t.Style_QTThreadColorCombo_HistoryUkey
)

-----------------Style_QTThreadColorCombo_History-------------------
Merge Production.dbo.Style_QTThreadColorCombo_History as t
Using (select a.* from Trade_To_Pms.dbo.Style_QTThreadColorCombo_History a ) as s
on t.StyleUkey = s.StyleUkey 
	AND t.Thread_Quilting_SizeUkey = s.Thread_Quilting_SizeUkey 
	AND t.FabricPanelCode =s.FabricPanelCode 
	AND t.LockDate =s.LockDate 
when matched then 
	update set t.AddName = isnull( s.AddName,'')
		  ,t.AddDate =  s.AddDate
		  ,t.EditName = isnull( s.EditName,'')
		  ,t.EditDate =  s.EditDate
		  ,t.HSize = isnull( s.HSize,0)
		  ,t.VSize = isnull( s.VSize,0)
		  ,t.ASize = isnull( s.ASize,0)
		  ,t.NeedleDistance = isnull( s.NeedleDistance,0)
		  ,t.FabricCode = isnull( s.FabricCode        ,'')
		  ,t.SCIRefno = isnull( s.SCIRefno            ,'')
		  ,t.Width = isnull( s.Width                  ,0)
		  ,t.Ukey = isnull( s.Ukey                    ,0)
		  ,t.Version = isnull( s.Version              ,'')
when not matched by target then
	insert (StyleUkey
           ,Thread_Quilting_SizeUkey
           ,FabricPanelCode
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
           ,LockDate
           ,HSize
           ,VSize
           ,ASize
           ,NeedleDistance
           ,FabricCode
           ,SCIRefno
           ,Width
           ,Ukey
		   ,Version)
		values (
            isnull(s.StyleUkey               ,0)
           ,isnull(s.Thread_Quilting_SizeUkey,0)
           ,isnull(s.FabricPanelCode         ,'')
           ,isnull(s.AddName                 ,'')
           ,s.AddDate
           ,isnull(s.EditName                ,'')
           ,s.EditDate
           ,s.LockDate
           ,isnull(s.HSize                   ,0)
           ,isnull(s.VSize                   ,0)
           ,isnull(s.ASize                   ,0)
           ,isnull(s.NeedleDistance          ,0)
           ,isnull(s.FabricCode              ,'')
           ,isnull(s.SCIRefno                ,'')
           ,isnull(s.Width                   ,0)
           ,isnull(s.Ukey                    ,0)
		   ,isnull(s.Version                 ,'')
           )
when not matched by source AND t.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete
;

-----------------Style_QTThreadColorCombo_Detail-------------------
Merge Production.dbo.Style_QTThreadColorCombo_Detail as t
Using (select a.* from Trade_To_Pms.dbo.Style_QTThreadColorCombo_Detail a ) as s
on t.Style_QTThreadColorComboUkey = s.Style_QTThreadColorComboUkey AND t.Seq = s.Seq AND t.Article =s.Article 
when matched then 
	update set t.SCIRefNo = isnull( s.SCIRefNo  ,'')
			  ,t.SuppId = isnull( s.SuppId      ,'')
			  ,t.ColorID = isnull( s.ColorID    ,'')
			  ,t.SuppColor = isnull( s.SuppColor,'')
			  ,t.AddName = isnull( s.AddName    ,'')
			  ,t.AddDate = s.AddDate
			  ,t.EditName = isnull( s.EditName  ,'')
			  ,t.EditDate = s.EditDate
			  ,t.Ukey = isnull( s.Ukey          ,0)
when not matched by target then
	insert  (Style_QTThreadColorComboUkey
           ,Seq
           ,SCIRefNo
           ,SuppId
           ,Article
           ,ColorID
           ,SuppColor
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
           ,Ukey)
		values (
            isnull(s.Style_QTThreadColorComboUkey,0)
           ,isnull(s.Seq                         ,'')
           ,isnull(s.SCIRefNo                    ,'')
           ,isnull(s.SuppId                      ,'')
           ,isnull(s.Article                     ,'')
           ,isnull(s.ColorID                     ,'')
           ,isnull(s.SuppColor                   ,'')
           ,isnull(s.AddName                     ,'')
           ,s.AddDate
           ,isnull(s.EditName                    ,'')
           ,s.EditDate
           ,isnull(s.Ukey                        ,0)
           )
;
----刪除條件：Trade不存在，且表頭還存在
DELETE t
FROM Production.dbo.Style_QTThreadColorCombo_Detail t
WHERE NOT EXISTS(
	SELECT 1 FROM Trade_To_Pms.dbo.Style_QTThreadColorCombo_Detail s
	WHERE t.Style_QTThreadColorComboUkey = s.Style_QTThreadColorComboUkey AND t.Seq = s.Seq AND t.Article =s.Article 
)
AND EXISTS(
	SELECT 1 
	FROM Production.dbo.Style_QTThreadColorCombo st
	INNER JOIN Trade_To_Pms.dbo.Style s ON st.StyleUkey = s.Ukey
	WHERE st.Ukey = t.Style_QTThreadColorComboUkey
)

-----------------Style_QTThreadColorCombo-------------------
Merge Production.dbo.Style_QTThreadColorCombo as t
Using (select a.* from Trade_To_Pms.dbo.Style_QTThreadColorCombo a ) as s
on t.Ukey=s.Ukey
when matched then 
	update set	t.Thread_Quilting_SizeUkey	  = isnull( s.Thread_Quilting_SizeUkey	,0),
				t.FabricPanelCode		  = isnull( s.FabricPanelCode		,        ''),
				t.AddName		  = isnull( s.AddName		,                        ''),
				t.AddDate		  = s.AddDate,
				t.EditName		  = isnull( s.EditName		,                        ''),
				t.EditDate		  = s.EditDate		
when not matched by target then
	insert  (StyleUkey
           ,Thread_Quilting_SizeUkey
           ,FabricPanelCode
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate
		   ,Ukey)
		values  (
                isnull(s.StyleUkey,0)
			   ,isnull(s.Thread_Quilting_SizeUkey,0)
			   ,isnull(s.FabricPanelCode,'')
			   ,isnull(s.AddName,'')
			   ,s.AddDate
			   ,isnull(s.EditName,'')
			   ,s.EditDate
			   ,isnull(s.Ukey,0)
               )
when not matched by source AND t.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete;
	
------------Thread_Replace_Detail_Detail
UPDATE a
   SET [Thread_Replace_DetailUkey] =isnull(b.[Thread_Replace_DetailUkey],0)
      ,[SuppID] = 					isnull(b.[SuppID]                   ,'')
      ,[ToSCIRefno] = 				isnull(b.[ToSCIRefno]               ,'')
      ,[ToBrandColorID] = 			isnull(b.[ToBrandColorID]           ,'')
      ,[ToBrandSuppColor] = 		isnull(b.[ToBrandSuppColor]         ,'')
      ,[AddName] = 					isnull(b.[AddName]                  ,'')
      ,[AddDate] = 					b.[AddDate]
      ,[EditName] =					isnull(b.[EditName]                 ,'')
      ,[EditDate] = 				b.[EditDate]
from Production.dbo.Thread_Replace_Detail_Detail a
inner join Trade_To_Pms.dbo.Thread_Replace_Detail_Detail b on b.Ukey = a.Ukey


INSERT INTO [dbo].[Thread_Replace_Detail_Detail]
           ([Thread_Replace_DetailUkey]
           ,[SuppID]
           ,[ToSCIRefno]
           ,[ToBrandColorID]
           ,[ToBrandSuppColor]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.[Thread_Replace_DetailUkey],0)
	,isnull(a.[SuppID]                   ,'')
	,isnull(a.[ToSCIRefno]               ,'')
	,isnull(a.[ToBrandColorID]           ,'')
	,isnull(a.[ToBrandSuppColor]         ,'')
	,isnull(a.[AddName]                  ,'')
	,a.[AddDate]
	,isnull(a.[EditName],'')
	,a.[EditDate]
from Trade_To_Pms.dbo.Thread_Replace_Detail_Detail a
left join Production.dbo.Thread_Replace_Detail_Detail b on b.Ukey = a.Ukey
where b.Ukey is null

------------Thread_Replace_Detail
UPDATE a
   SET [Thread_ReplaceUkey]=isnull(b.[Thread_ReplaceUkey],0)
      ,[StartDate]		   =b.[StartDate]
      ,[EndDate]		   =b.[EndDate]
      ,[AddName]		   =isnull(b.[AddName]           ,'')
      ,[AddDate]		   =b.[AddDate]
      ,[EditName]		   =isnull(b.[EditName],'')
      ,[EditDate]		   =b.[EditDate]
from Production.dbo.Thread_Replace_Detail a
inner join Trade_To_Pms.dbo.Thread_Replace_Detail b on b.Ukey = a.Ukey


INSERT INTO [dbo].[Thread_Replace_Detail]
           ([Thread_ReplaceUkey]
           ,[StartDate]
           ,[EndDate]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.[Thread_ReplaceUkey],0)
	,a.[StartDate]
	,a.[EndDate]
	,isnull(a.[AddName],'')
	,a.[AddDate]
	,isnull(a.[EditName],'')
	,a.[EditDate]
from Trade_To_Pms.dbo.Thread_Replace_Detail a
left join Production.dbo.Thread_Replace_Detail b on b.Ukey = a.Ukey
where b.Ukey is null


------------Thread_Replace
UPDATE a
   SET [BrandID]      = isnull(b.[BrandID],'')
      ,[FromSCIRefno] = isnull(b.[FromSCIRefno],'')
      ,[FromSuppColor]= isnull(b.[FromSuppColor],'')
      ,[AddName]	  = isnull(b.[AddName],'')
      ,[AddDate]	  = b.[AddDate]
      ,[EditName]	  = isnull(b.[EditName],'')
      ,[EditDate]	  =b.[EditDate]
from Production.dbo.Thread_Replace a
inner join Trade_To_Pms.dbo.Thread_Replace b on b.Ukey = a.Ukey


INSERT INTO [dbo].[Thread_Replace]
           ([BrandID]
           ,[FromSCIRefno]
           ,[FromSuppColor]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.[BrandID]      ,'')
	,isnull(a.[FromSCIRefno] ,'')
	,isnull(a.[FromSuppColor],'')
	,isnull(a.[AddName]      ,'')
	,a.[AddDate]
	,isnull(a.[EditName],'')
	,a.[EditDate]
from Trade_To_Pms.dbo.Thread_Replace a
left join Production.dbo.Thread_Replace b on b.Ukey = a.Ukey
where b.Ukey is null


-----------------Style_RRLR_Report-------------------
Merge Production.dbo.Style_RRLR_Report as t
Using (select a.* from Trade_To_Pms.dbo.Style_RRLR_Report a ) as s
on t.StyleUkey=s.StyleUkey and t.SuppID=s.SuppID
and t.Refno=s.Refno and t.ColorID=s.ColorID
when matched then 
	update set	t.Material	  = ISNULL(s.Material,'')	,
				t.LabDipStatus		  = ISNULL(s.LabDipStatus,'')		,
				t.RR = IIF(s.RR IS NULL OR TRY_CONVERT(bit, s.RR) IS NULL,  0  ,s.RR),   --NULL or 非bit值的東西，塞0
				t.RRRemark = ISNULL(s.RRRemark,''),
				t.LifecycleState = ISNULL(s.LifecycleState,''),
				t.LR = IIF(s.LR IS NULL OR TRY_CONVERT(bit, s.LR) IS NULL,  0  ,s.LR),	  --NULL or 非bit值的東西，塞0
				t.AddName		  = ISNULL(s.AddName,'')		,
				t.AddDate		  = s.AddDate
when not matched by target then
	insert  (  [StyleUkey]
			  ,[SuppID]
			  ,[Refno]
			  ,[Material]
			  ,[ColorID]
			  ,[LabDipStatus]
			  ,[RR]
			  ,[RRRemark]
			  ,[LifecycleState]
			  ,[LR]
			  ,[AddName]
			  ,[AddDate])
	  values  (isnull(s.[StyleUkey],0)
			  ,ISNULL(s.[SuppID],'')
			  ,ISNULL(s.[Refno],'')
			  ,ISNULL(s.[Material],'')
			  ,ISNULL(s.[ColorID],'')
			  ,ISNULL(s.[LabDipStatus],'')
			  ,IIF(s.RR IS NULL OR TRY_CONVERT(bit, s.RR) IS NULL,  0  ,s.RR) --NULL or 非bit值的東西，塞0
			  ,ISNULL(s.[RRRemark],'')
			  ,ISNULL(s.[LifecycleState],'')
			  ,IIF(s.LR IS NULL OR TRY_CONVERT(bit, s.LR) IS NULL,  0  ,s.LR) --NULL or 非bit值的東西，塞0
			  ,ISNULL(s.[AddName],'')
			  ,s.[AddDate])
when not matched by source AND t.Styleukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.Style) then 
	delete;


------------Style_ArtworkTestDox
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_ArtworkTestDox
from Production.dbo.Style_ArtworkTestDox as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_ArtworkTestDox as b
on a.Ukey = b.Ukey
where b.Ukey is null


UPDATE a
   SET [StyleUkey]=isnull(b.[StyleUkey],0)
      ,[ArtworkTypeID]=isnull(b.[ArtworkTypeID],'')
      ,[ArtworkID]=isnull(b.[ArtworkID],'')
      ,[Article]=isnull(b.[Article],'')
      ,[F_FabricPanelCode]=isnull(b.[F_FabricPanelCode],'')
      ,[F_Refno]=isnull(b.[F_Refno],'')
      ,[A_FabricPanelCode]=isnull(b.[A_FabricPanelCode],'')
	  ,[A_Refno]=isnull(b.[A_Refno],'')
	  ,[FabricFaceSide]=isnull(b.[FabricFaceSide],'')
	  ,[PrintType]=isnull(b.[PrintType],'')
	  ,[TestNo]=isnull(b.[TestNo],'')
	  ,[TestResult]=isnull(b.[TestResult],'')
      ,[Remark]=isnull(b.[Remark],'')
      ,[AddName]=isnull(b.[AddName],'')
      ,[AddDate]=b.[AddDate]
      ,[EditName]=isnull(b.[EditName],'')
	  ,[EditDate]=b.[EditDate]
	  ,[SubstrateFormSendDate]=	b.[SubstrateFormSendDate]
	  ,[FactoryID]=isnull(b.[FactoryID],'')
	  ,[OrderID]=isnull(b.[OrderID],'')
	  ,[IsA_FabricPanelCodeCanEmpty]=isnull(b.[IsA_FabricPanelCodeCanEmpty],0)
from Production.dbo.Style_ArtworkTestDox a
inner join Trade_To_Pms.dbo.Style_ArtworkTestDox b on b.Ukey = a.Ukey


INSERT INTO [dbo].[Style_ArtworkTestDox]
(	   [Ukey]
      ,[StyleUkey]
      ,[ArtworkTypeID]
      ,[ArtworkID]
      ,[Article]
      ,[F_FabricPanelCode]
      ,[F_Refno]
      ,[A_FabricPanelCode]
      ,[A_Refno]
      ,[FabricFaceSide]
      ,[PrintType]
      ,[TestNo]
      ,[TestResult]
      ,[Remark]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[SubstrateFormSendDate]
      ,[FactoryID]
      ,[OrderID]
      ,[IsA_FabricPanelCodeCanEmpty]
)
select
	   a.[Ukey]
      ,isnull(a.[StyleUkey],0)
      ,isnull(a.[ArtworkTypeID],'')
      ,isnull(a.[ArtworkID],'')
      ,isnull(a.[Article],'')
      ,isnull(a.[F_FabricPanelCode],'')
      ,isnull(a.[F_Refno],'')
      ,isnull(a.[A_FabricPanelCode],'')
      ,isnull(a.[A_Refno],'')
      ,isnull(a.[FabricFaceSide],'')
      ,isnull(a.[PrintType],'')
      ,isnull(a.[TestNo],'')
      ,isnull(a.[TestResult],'')
      ,isnull(a.[Remark],'')
      ,isnull(a.[AddName],'')
      ,a.[AddDate]
      ,isnull(a.[EditName],'')
      ,a.[EditDate]
      ,a.[SubstrateFormSendDate]
      ,isnull(a.[FactoryID],'')
      ,isnull(a.[OrderID],'')
      ,isnull(a.[IsA_FabricPanelCodeCanEmpty],0)
from Trade_To_Pms.dbo.Style_ArtworkTestDox a
left join Production.dbo.Style_ArtworkTestDox b on b.Ukey = a.Ukey
where b.Ukey is null

------------Style_CustOrderSize
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_CustOrderSize
from Production.dbo.Style_CustOrderSize as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Style_CustOrderSize as b
on a.StyleUkey = b.StyleUkey
where b.StyleUkey is null

UPDATE a
   SET [StyleUkey]=isnull(b.[StyleUkey],0)
	  ,[SizeCode]=isnull(b.[SizeCode],'')
	  ,[SizeSpec]=isnull(b.[SizeSpec],'')
      ,[AddName]=isnull(b.[AddName],'')
      ,[AddDate]=b.[AddDate]
      ,[EditName]=isnull(b.[EditName],'')
	  ,[EditDate]=b.[EditDate]
from Production.dbo.Style_CustOrderSize a
inner join Trade_To_Pms.dbo.Style_CustOrderSize b on b.StyleUkey = a.StyleUkey

INSERT INTO [dbo].[Style_CustOrderSize]
(	  
      [StyleUkey]
	  ,[SizeCode]
	  ,[SizeSpec]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
)
select	  
      isnull(a.[StyleUkey],0)
	  ,isnull(a.[SizeCode],'')
	  ,isnull(a.[SizeSpec],'')
      ,isnull(a.[AddName],'')
      ,a.[AddDate]
      ,isnull(a.[EditName],'')
      ,a.[EditDate]
from Trade_To_Pms.dbo.Style_CustOrderSize a
left join Production.dbo.Style_CustOrderSize b on b.StyleUkey = a.StyleUkey
where b.StyleUkey is null

------------Style_UnitPrice
----------------------刪除主TABLE多的資料
select distinct * into #tmp_Style_UnitPrice from Trade_To_Pms.dbo.Style_UnitPrice 


RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Style_UnitPrice
from Production.dbo.Style_UnitPrice as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join #tmp_Style_UnitPrice as b
on a.Ukey = b.Ukey
where b.Ukey is null

UPDATE a
   SET 
	  [StyleUkey] = isnull(b.[StyleUkey],0)      
      ,[CountryID] = isnull(b.[CountryID],'')
      ,[CurrencyID] = ISNULL(b.[CurrencyID],'')
      ,[PoPrice] = ISNULL(b.[PoPrice],0)
      ,[QuotCost] = ISNULL(b.[QuotCost],0)
      ,[CustCDID] = ISNULL(b.[CustCDID],'')
      ,[DestPrice] = ISNULL(b.[DestPrice],0)
      ,[OriginalPrice] = ISNULL(b.[OriginalPrice],0)
      ,[AddName] = ISNULL(b.[AddName],'')
      ,[AddDate] = b.[AddDate]
      ,[EditName] = ISNULL(b.[EditName],'')
      ,[EditDate] = b.[EditDate]
      ,[COO] = ISNULL(b.[COO],'')
      ,[FactoryID] = ISNULL(b.[FactoryID],'')
from Production.dbo.Style_UnitPrice a
inner join #tmp_Style_UnitPrice b on b.Ukey = a.Ukey

INSERT INTO [dbo].[Style_UnitPrice]
(	  
      [StyleUkey]
      ,[Ukey]
      ,[CountryID]
      ,[CurrencyID]
      ,[PoPrice]
      ,[QuotCost]
      ,[CustCDID]
      ,[DestPrice]
      ,[OriginalPrice]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[COO]
      ,[FactoryID]
)
select	  
      isnull(a.[StyleUkey],0)
      ,isnull(a.[Ukey],0)
      ,isnull(a.[CountryID],'')
      ,isnull(a.[CurrencyID],'')
      ,isnull(a.[PoPrice],0)
      ,isnull(a.[QuotCost],0)
      ,isnull(a.[CustCDID],'')
      ,isnull(a.[DestPrice],0)
      ,isnull(a.[OriginalPrice],0)
      ,isnull(a.[AddName],'')
      ,a.[AddDate]
      ,isnull(a.[EditName],'')
      ,a.[EditDate]
      ,isnull(a.[COO],'')
      ,isnull(a.[FactoryID],'')
from #tmp_Style_UnitPrice a
left join Production.dbo.Style_UnitPrice b on b.Ukey = a.Ukey
where b.Ukey is null

drop table #tmp_Style_UnitPrice
------------Marker
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Marker
from Production.dbo.Marker as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey=t.Ukey
left join Trade_To_Pms.dbo.Marker as b
on a.Ukey = b.Ukey
where b.Ukey is null

UPDATE a
   SET 
        [ID] = isnull(b.id,''),	
		[Version]  = isnull(b.[Version],''),
		[BrandID]  = isnull(b.[BrandID],''),
		[StyleID]  = isnull(b.[StyleID],'') ,
		[SeasonID] = isnull(b.[SeasonID],'') ,
		[MarkerNo] = isnull(b.[MarkerNo],'') ,
		[ActFtyMarker] = isnull(b.[ActFtyMarker],'') ,
		[PatternID]    = isnull(b.[PatternID],'') ,
		[PatternNo]    = isnull(b.[PatternNo],'') ,
		[PatternVersion]  = ISNULL(b.[PatternVersion],''),
		[MarkerName] = ISNULL(b.[MarkerName] ,''),
		[MarkerFormat] = ISNULL(b.[MarkerFormat] ,''),
		[EstFinDate] = b.[EstFinDate] ,
		[ActFinDate] = b.[ActFinDate],
		[PLUS] = isnull(b.[PLUS],0),
		[RevisedReason] = isnull(b.[RevisedReason] ,''),
		[Status] = ISNULL(b.[Status] ,''),
		[CFMName] = ISNULL(b.[CFMName] ,''),
		[StyleRemark] = ISNULL(b.[StyleRemark] ,''),
		[HisRemark] = ISNULL(b.[HisRemark] ,''),
		[PendingRemark] = ISNULL(b.[PendingRemark] ,''),
		[StyleUkey] = ISNULL(b.[StyleUkey] ,0),
		[AddName] = ISNULL(b.[AddName] ,''),
		[AddDate]  = b.[AddDate] ,
		[EditName] = ISNULL(b.[EditName],'') ,
		[EditDate] = b.[EditDate] ,
		[KeepPreviousVersion] = ISNULL(b.[KeepPreviousVersion] ,0),
		[SizeReason] = ISNULL(b.[SizeReason] ,''),
		[MarkerNoLoss] = ISNULL(b.[MarkerNoLoss] ,0)
	from Production.dbo.Marker a
inner join Trade_To_Pms.dbo.Marker b on b.Ukey = a.Ukey


INSERT INTO [dbo].[Marker]
(	   [ID]
      ,[Version]
      ,[BrandID]
      ,[StyleID]
      ,[SeasonID]
      ,[MarkerNo]
      ,[ActFtyMarker]
      ,[PatternID]
      ,[PatternNo]
      ,[PatternVersion]
      ,[MarkerName]
      ,[MarkerFormat]
      ,[EstFinDate]
      ,[ActFinDate]
      ,[PLUS]
      ,[RevisedReason]
      ,[Status]
      ,[CFMName]
      ,[UKey]
      ,[StyleRemark]
      ,[HisRemark]
      ,[PendingRemark]
      ,[StyleUkey]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
	  ,[KeepPreviousVersion]
      ,[SizeReason]
      ,[MarkerNoLoss]
)
select
	   ISNULL(a.[ID],'')
      ,ISNULL(a.[Version],'')
      ,ISNULL(a.[BrandID],'')
      ,ISNULL(a.[StyleID],'')
      ,ISNULL(a.[SeasonID],'')
      ,ISNULL(a.[MarkerNo],'')
      ,ISNULL(a.[ActFtyMarker],'')
      ,ISNULL(a.[PatternID],'')
      ,ISNULL(a.[PatternNo],'')
      ,ISNULL(a.[PatternVersion],'')
      ,ISNULL(a.[MarkerName],'')
      ,ISNULL(a.[MarkerFormat],'')
      ,a.[EstFinDate]
      ,a.[ActFinDate]
      ,ISNULL(a.[PLUS],0)
      ,ISNULL(a.[RevisedReason],'')
      ,ISNULL(a.[Status],'')
      ,ISNULL(a.[CFMName],'')
      ,ISNULL(a.[UKey],0)
      ,ISNULL(a.[StyleRemark],'')
      ,ISNULL(a.[HisRemark],'')
      ,ISNULL(a.[PendingRemark],'')
      ,ISNULL(a.[StyleUkey],0)
      ,ISNULL(a.[AddName],'')
      ,a.[AddDate]
      ,ISNULL(a.[EditName],'')
      ,a.[EditDate]
      ,ISNULL(a.[KeepPreviousVersion],0)
      ,ISNULL(a.[SizeReason],'')
      ,ISNULL(a.[MarkerNoLoss],0)
from Trade_To_Pms.dbo.Marker a
left join Production.dbo.Marker b on b.Ukey = a.Ukey
where b.Ukey is null

------------Marker_ML
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Marker_ML
from Production.dbo.Marker_ML as a 
inner join Trade_To_Pms.dbo.Marker t on t.Ukey = a.MarkerUkey
left join Trade_To_Pms.dbo.Marker_ML as b
on a.ID = b.ID and a.Version = b.Version and a.MarkerName = b.MarkerName
where b.ID is null

UPDATE a
   SET 
        [ID] = ISNULL(b.[ID] ,'')
      ,[Version] = ISNULL(b.[Version] ,'')
      ,[MarkerUkey] = ISNULL(b.[MarkerUkey],0)
      ,[MarkerName] = ISNULL(b.[MarkerName],'')
      ,[FabricPanelCode] = ISNULL(b.[FabricPanelCode],'')
      ,[SCIRefno] = ISNULL(b.[SCIRefno],'')
      ,[MarkerLength] = ISNULL(b.[MarkerLength],'')
      ,[PatternPanel] = ISNULL(b.[PatternPanel],'')
      ,[FabricCode] = ISNULL(b.[FabricCode],'')
      ,[Width] = ISNULL(b.[Width],'')
      ,[Efficiency] = ISNULL(b.[Efficiency],'')
      ,[Remark] = ISNULL(b.[Remark],'')
      ,[ConsPC] = ISNULL(b.[ConsPC],'')
      ,[Article] = isnull(b.[Article],'')
      ,[ActCuttingPerimeter] = ISNULL(b.[ActCuttingPerimeter],'')
      ,[Perimeter] = ISNULL(b.[Perimeter],'')
      ,[StraightLength] = ISNULL(b.[StraightLength],'')
      ,[CurvedLength] = ISNULL(b.[CurvedLength],'')
      ,[TotalCuttingPieceNum] = ISNULL(b.[TotalCuttingPieceNum],'')
      ,[AllSize] = ISNULL(b.[AllSize],0)
      ,[OneTwoWay] = ISNULL(b.[OneTwoWay],0)
      ,[V_Repeat] = ISNULL(b.[V_Repeat],0)
      ,[H_Repeat] = ISNULL(b.[H_Repeat],0)
      ,[MatchFabric] = ISNULL(b.[MatchFabric],'')
      ,[HorizontalCutting] = ISNULL(b.[HorizontalCutting],0)
      ,[Mtl_Key] = ISNULL(b.[Mtl_Key],'')
      ,[Mtl_Ver] = ISNULL(b.[Mtl_Ver],'')
	from Production.dbo.Marker_ML a
inner join Trade_To_Pms.dbo.Marker_ML b 
on a.ID = b.ID and a.Version = b.Version and a.MarkerName = b.MarkerName


INSERT INTO [dbo].[Marker_ML]
(	   [ID]
      ,[Version]
      ,[MarkerUkey]
      ,[MarkerName]
      ,[FabricPanelCode]
      ,[SCIRefno]
      ,[MarkerLength]
      ,[PatternPanel]
      ,[FabricCode]
      ,[Width]
      ,[Efficiency]
      ,[Remark]
      ,[ConsPC]
      ,[Article]
      ,[ActCuttingPerimeter]
      ,[Perimeter]
      ,[StraightLength]
      ,[CurvedLength]
      ,[TotalCuttingPieceNum]
      ,[AllSize]
      ,[OneTwoWay]
      ,[V_Repeat]
      ,[H_Repeat]
      ,[MatchFabric]
      ,[HorizontalCutting]
      ,[Mtl_Key]
      ,[Mtl_Ver]
)
select
	   ISNULL(a.[ID],'')
      ,ISNULL(a.[Version],'')
      ,ISNULL(a.[MarkerUkey],0)
      ,ISNULL(a.[MarkerName],'')
      ,ISNULL(a.[FabricPanelCode],'')
      ,ISNULL(a.[SCIRefno],'')
      ,ISNULL(a.[MarkerLength],'')
      ,ISNULL(a.[PatternPanel],'')
      ,ISNULL(a.[FabricCode],'')
      ,ISNULL(a.[Width],'')
      ,ISNULL(a.[Efficiency],'')
      ,ISNULL(a.[Remark],'')
      ,ISNULL(a.[ConsPC],'')
      ,ISNULL(a.[Article],'')
      ,ISNULL(a.[ActCuttingPerimeter],'')
      ,ISNULL(a.[Perimeter],'')
      ,ISNULL(a.[StraightLength],'')
      ,ISNULL(a.[CurvedLength],'')
      ,ISNULL(a.[TotalCuttingPieceNum],'')
      ,ISNULL(a.[AllSize],0)
      ,ISNULL(a.[OneTwoWay],0)
      ,ISNULL(a.[V_Repeat],0)
      ,ISNULL(a.[H_Repeat],0) 
      ,ISNULL(a.[MatchFabric],'')
      ,ISNULL(a.[HorizontalCutting],0)
      ,ISNULL(a.[Mtl_Key],'')
      ,ISNULL(a.[Mtl_Ver],'')
from Trade_To_Pms.dbo.Marker_ML a
left join Production.dbo.Marker_ML b 
on a.ID = b.ID and a.Version = b.Version and a.MarkerName = b.MarkerName
where b.ID is null

------------Marker_ML_SizeQty
----------------------刪除主TABLE多的資料
RAISERROR('imp_Style - Starts',0,0)
Delete Production.dbo.Marker_ML_SizeQty
from Production.dbo.Marker_ML_SizeQty as a 
inner join Trade_To_Pms.dbo.Marker t on t.Ukey = a.MarkerUkey
left join Trade_To_Pms.dbo.Marker_ML_SizeQty as b
on a.ID = b.ID and a.Version = b.Version and a.MarkerName = b.MarkerName
and a.SizeCode = b.SizeCode
where b.ID is null


UPDATE a
   SET 
       [ID] = ISNULL(b.[ID],'')
      ,[Version] = ISNULL(b.[Version],'')
      ,[MarkerName] = ISNULL(b.[MarkerName],'')
      ,[SizeCode] = ISNULL(b.[SizeCode],'')
      ,[Qty] = ISNULL(b.[Qty],0)
	from Production.dbo.Marker_ML_SizeQty a
inner join Trade_To_Pms.dbo.Marker_ML_SizeQty b 
on a.ID = b.ID and a.Version = b.Version and a.MarkerName = b.MarkerName
and a.SizeCode = b.SizeCode


INSERT INTO [dbo].[Marker_ML_SizeQty]
(	   [ID]
      ,[Version]
      ,[MarkerUkey]
      ,[MarkerName]
      ,[SizeCode]
      ,[Qty]
)
select
	   ISNULL(a.[ID],'')    ,ISNULL(a.[Version],'')
      ,ISNULL(a.[MarkerUkey],0)
      ,ISNULL(a.[MarkerName],'')
      ,ISNULL(a.[SizeCode],'')
      ,ISNULL(a.[Qty],0)
from Trade_To_Pms.dbo.Marker_ML_SizeQty a
left join Production.dbo.Marker_ML_SizeQty b 
on a.ID = b.ID and a.Version = b.Version and a.MarkerName = b.MarkerName
and a.SizeCode = b.SizeCode
where b.ID is null

----------------------Style_FabricCode_QT----Start
Delete Production.dbo.Style_FabricCode_QT
from Production.dbo.Style_FabricCode_QT as a 
INNER JOIN Trade_To_Pms.dbo.Style as t on a.StyleUkey = t.Ukey
left join Trade_To_Pms.dbo.Style_FabricCode_QT as b
on a.StyleUkey= b.StyleUkey AND a.FabricPanelCode = b.FabricPanelCode and a.SeqNO = b.SeqNO
where b.StyleUkey is null
---------------------------UPDATE 

UPDATE a
SET	 a.QTFabricPanelCode	= isnull(b.QTFabricPanelCode, '')
	,a.AddName				= isnull(b.AddName, '')			
	,a.AddDate				= b.AddDate			
	,a.EditName				= isnull(b.EditName, '')			
	,a.EditDate				= b.EditDate	
from Production.dbo.Style_FabricCode_QT as a 
inner join Trade_To_Pms.dbo.Style_FabricCode_QT as b ON a.StyleUkey= b.StyleUkey AND a.FabricPanelCode = b.FabricPanelCode and a.SeqNO = b.SeqNO
-------------------------- INSERT

INSERT INTO Production.dbo.Style_FabricCode_QT(
 StyleUkey
,FabricPanelCode
,SeqNO
,QTFabricPanelCode
,AddName
,AddDate
,EditName
,EditDate
)
select 
		 b.StyleUkey	
		,b.FabricPanelCode
		,b.SeqNO
		,isnull(b.QTFabricPanelCode, '')		
		,isnull(b.AddName, '')	
		,b.AddDate
		,isnull(b.EditName, '')	
		,b.EditDate
from Trade_To_Pms.dbo.Style_FabricCode_QT as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Style_FabricCode_QT as a WITH (NOLOCK) where a.StyleUkey= b.StyleUkey AND a.FabricPanelCode = b.FabricPanelCode and a.SeqNO = b.SeqNO)
----------------------Style_FabricCode_QT----End


END
 