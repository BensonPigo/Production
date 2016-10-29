
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Base]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--	Cust
--ACust
----------------------刪除主TABLE多的資料
Delete Production.dbo.Brand
from Production.dbo.Brand as a left join Trade_To_Pms.dbo.Brand as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET 
       a. ID	      = b. ID
      ,a. NameCH	      = b. NameCH
      ,a. NameEN	      = b. NameEN
      ,a. CountryID	      = b. CountryID
      ,a. BuyerID	      = b. BuyerID
      ,a. Tel	      = b. Tel
      ,a. Fax	      = b. Fax
      ,a. Contact1	      = b. Contact1
      ,a. Contact2	      = b. Contact2
      ,a. AddressCH	      = b. AddressCH
      ,a. AddressEN	      = b. AddressEN
      ,a. CurrencyID	      = b. CurrencyID
      ,a. Remark	      = b. Remark
      ,a. Customize1	      = b. Customize1
      ,a. Customize2	      = b. Customize2
      ,a. Customize3	      = b. Customize3
      ,a. Commission	      = b. Commission
      ,a. ZipCode	      = b. ZipCode
      ,a. Email	      = b. Email
      ,a. MrTeam	      = b. MrTeam
      ,a. BrandGroup	      = b. BrandGroup
      ,a. ApparelXlt	      = b. ApparelXlt
      ,a. LossSampleFabric	      = b. LossSampleFabric
      ,a. PayTermARIDBulk	      = b. PayTermARIDBulk
      ,a. PayTermARIDSample	      = b. PayTermARIDSample
      ,a. BrandFactoryAreaCaption	      = b. BrandFactoryAreaCaption
      ,a. BrandFactoryCodeCaption	      = b. BrandFactoryCodeCaption
      ,a. BrandFactoryVendorCaption	      = b. BrandFactoryVendorCaption
      ,a. ShipCode	      = b. ShipCode
      ,a. Junk	      = b. Junk
      ,a. AddName	      = b. AddName
      ,a. AddDate	      = b. AddDate
      ,a. EditName	      = b. EditName
      ,a. EditDate	      = b. EditDate
from Production.dbo.Brand as a inner join Trade_To_Pms.dbo.Brand as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Brand
 (
		ID
      ,NameCH
      ,NameEN
      ,CountryID
      ,BuyerID
      ,Tel
      ,Fax
      ,Contact1
      ,Contact2
      ,AddressCH
      ,AddressEN
      ,CurrencyID
      ,Remark
      ,Customize1
      ,Customize2
      ,Customize3
      ,Commission
      ,ZipCode
      ,Email
      ,MrTeam
      ,BrandGroup
      ,ApparelXlt
      ,LossSampleFabric
      ,PayTermARIDBulk
      ,PayTermARIDSample
      ,BrandFactoryAreaCaption
      ,BrandFactoryCodeCaption
      ,BrandFactoryVendorCaption
      ,ShipCode
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
SELECT ID
      ,NameCH
      ,NameEN
      ,CountryID
      ,BuyerID
      ,Tel
      ,Fax
      ,Contact1
      ,Contact2
      ,AddressCH
      ,AddressEN
      ,CurrencyID
      ,Remark
      ,Customize1
      ,Customize2
      ,Customize3
      ,Commission
      ,ZipCode
      ,Email
      ,MrTeam
      ,BrandGroup
      ,ApparelXlt
      ,LossSampleFabric
      ,PayTermARIDBulk
      ,PayTermARIDSample
      ,BrandFactoryAreaCaption
      ,BrandFactoryCodeCaption
      ,BrandFactoryVendorCaption
      ,ShipCode
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.Brand as b
where not exists(select id from Production.dbo.Brand as a where a.id = b.id)

--Season
--ASeason Season
----------------------刪除主TABLE多的資料
Delete Production.dbo.Season
from Production.dbo.Season as a left join Trade_To_Pms.dbo.Season as b
on a.id = b.id and a.BrandID = b.BrandID
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET 
       a.ID		 =b.ID
      ,a.BrandID		      =b.BrandID
      ,a.CostRatio		      =b.CostRatio
      ,a.SeasonSCIID		      =b.SeasonSCIID
      ,a.Month		      =b.Month
      ,a.Junk		      =b.Junk
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Season as a inner join Trade_To_Pms.dbo.Season as b ON a.id=b.id and a.BrandID = b.BrandID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Season
(
       ID
      ,BrandID
      ,CostRatio
      ,SeasonSCIID
      ,Month
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       ID
      ,BrandID
      ,CostRatio
      ,SeasonSCIID
      ,Month
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.Season as b
where not exists(select id from Production.dbo.Season as a where a.id = b.id and a.BrandID = b.BrandID)


--Supp  Supp
--ASupp
----------------------刪除主TABLE多的資料
Delete Production.dbo.Supp
from Production.dbo.Supp as a left join Trade_To_Pms.dbo.Supp as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET 
a.ID		  =b.ID
      ,a.Junk		      =b.Junk
      ,a.AbbCH		      =b.AbbCH
      ,a.AbbEN		      =b.AbbEN
      ,a.NameCH		      =b.NameCH
      ,a.NameEN		      =b.NameEN
      ,a.CountryID		      =b.CountryID
      ,a.ThirdCountry		      =b.ThirdCountry
      ,a.Tel		      =b.Tel
      ,a.Fax		      =b.Fax
      ,a.AddressCH		      =b.AddressCH
      ,a.AddressEN		      =b.AddressEN
      ,a.ZipCode		      =b.ZipCode
      ,a.Delay		      =b.Delay
      ,a.DelayMemo		      =b.DelayMemo
      ,a.LockDate		      =b.LockDate
      ,a.LockMemo		      =b.LockMemo
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate
      ,a.Currencyid		      =b.Currencyid

from Production.dbo.Supp as a inner join Trade_To_Pms.dbo.Supp as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Supp(
		ID
      ,Junk
      ,AbbCH
      ,AbbEN
      ,NameCH
      ,NameEN
      ,CountryID
      ,ThirdCountry
      ,Tel
      ,Fax
      ,AddressCH
      ,AddressEN
      ,ZipCode
      ,Delay
      ,DelayMemo
      ,LockDate
      ,LockMemo
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Currencyid
)
select 
		ID
      ,Junk
      ,AbbCH
      ,AbbEN
      ,NameCH
      ,NameEN
      ,CountryID
      ,ThirdCountry
      ,Tel
      ,Fax
      ,AddressCH
      ,AddressEN
      ,ZipCode
      ,Delay
      ,DelayMemo
      ,LockDate
      ,LockMemo
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Currencyid
from Trade_To_Pms.dbo.Supp as b
where not exists(select id from Production.dbo.Supp as a where a.id = b.id)


--Ability
--AAbility
--CDCode
----------------------刪除主TABLE多的資料
Delete Production.dbo.CDCode
from Production.dbo.CDCode as a left join Trade_To_Pms.dbo.CDCode as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID					 =b.ID
      ,a.Junk		         =b.Junk
      ,a.Description		 =b.Description
      ,a.Cpu		          =b.Cpu
      ,a.ComboPcs		      =b.ComboPcs
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate
from Production.dbo.CDCode as a inner join Trade_To_Pms.dbo.CDCode as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.CDCode(
		ID
      ,Junk
      ,Description
      ,Cpu
      ,ComboPcs
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
		ID
      ,Junk
      ,Description
      ,Cpu
      ,ComboPcs
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.CDCode as b
where not exists(select id from Production.dbo.CDCode as a where a.id = b.id)


--Country Country
--ACountry
----------------------刪除主TABLE多的資料
Delete Production.dbo.Country
from Production.dbo.Country as a left join Trade_To_Pms.dbo.Country as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.NameCH		      =b.NameCH
      ,a.NameEN		      =b.NameEN
      ,a.Alias		      =b.Alias
      ,a.Junk		      =b.Junk
      ,a.MtlFormA		      =b.MtlFormA
      ,a.Continent		      =b.Continent
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Country as a inner join Trade_To_Pms.dbo.Country as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Country(
 ID
      ,NameCH
      ,NameEN
      ,Alias
      ,Junk
      ,MtlFormA
      ,Continent
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
 ID
      ,NameCH
      ,NameEN
      ,Alias
      ,Junk
      ,MtlFormA
      ,Continent
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.Country as b
where not exists(select id from Production.dbo.Country as a where a.id = b.id)


--Mtltype Mtltype  
--AMtltype
----------------------刪除主TABLE多的資料
Delete Production.dbo.MtlType
from Production.dbo.MtlType as a left join Trade_To_Pms.dbo.MtlType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
--------TRADE的EditDate > PMS EditDate 就UPDATE All 如果false 就不UPDATE EditDate & EditName 其他一樣UPDATE
UPDATE a
SET  
       a.ID	      =b.ID		
      ,a.FullName	      =b.FullName		
      ,a.Type	      =b.Type		
      ,a.Junk	      =b.Junk		
      ,a.IrregularCost	      =b.IrregularCost		
      ,a.CheckZipper	      =b.CheckZipper		
      ,a.ProductionType	      =b.ProductionType		
      ,a.OutputUnit	      =b.OutputUnit		
      ,a.IsExtensionUnit	      =b.IsExtensionUnit		
      ,a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =b.EditName		
      ,a.EditDate	      =b.EditDate		

from Production.dbo.MtlType as a inner join Trade_To_Pms.dbo.MtlType as b ON a.id=b.id
where b.EditDate > a.EditDate

UPDATE a
SET  
       a.ID	      =b.ID		
      ,a.FullName	      =b.FullName		
      ,a.Type	      =b.Type		
      ,a.Junk	      =b.Junk		
      ,a.IrregularCost	      =b.IrregularCost		
      ,a.CheckZipper	      =b.CheckZipper		
      ,a.ProductionType	      =b.ProductionType		
      ,a.OutputUnit	      =b.OutputUnit		
      ,a.IsExtensionUnit	      =b.IsExtensionUnit		
      ,a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      --,a.EditName	      =b.EditName		
      --,a.EditDate	      =b.EditDate		

from Production.dbo.MtlType as a inner join Trade_To_Pms.dbo.MtlType as b ON a.id=b.id
where b.EditDate <= a.EditDate
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.MtlType(
ID
      ,FullName
      ,Type
      ,Junk
      ,IrregularCost
      ,CheckZipper
      ,ProductionType
      ,OutputUnit
      ,IsExtensionUnit
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
ID
      ,FullName
      ,Type
      ,Junk
      ,IrregularCost
      ,CheckZipper
      ,ProductionType
      ,OutputUnit
      ,IsExtensionUnit
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.MtlType as b
where not exists(select id from Production.dbo.MtlType as a where a.id = b.id)


--Artworktype Artworktype 
--PMS 多,[InhouseOSP]
--      ,[AccountNo]
--      ,[BcsLt]
--      ,[CutLt]
--IsSubprocess 不更新~INSERT丟’O’
----------------------刪除主TABLE多的資料
Delete Production.dbo.ArtworkType
from Production.dbo.ArtworkType as a left join Trade_To_Pms.dbo.ArtworkType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID		
      ,a.Abbreviation	      =b.Abbreviation		
      ,a.Classify	      =b.Classify		
      ,a.Seq	      =b.Seq		
      ,a.Junk	      =b.Junk		
      ,a.ArtworkUnit	      =b.ArtworkUnit		
      ,a.ProductionUnit	      =b.ProductionUnit		
      ,a.IsTMS	      =b.IsTMS		
      ,a.IsPrice	      =b.IsPrice		
      ,a.IsArtwork	      =b.IsArtwork		
      ,a.IsTtlTMS	      =b.IsTtlTMS		
      ,a.Remark	      =b.Remark		
      ,a.ReportDropdown	      =b.ReportDropdown		
      ,a.UseArtwork	      =b.UseArtwork		
      ,a.SystemType	      =b.SystemType		
      ,a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =b.EditName		
      ,a.EditDate	      =b.EditDate		

from Production.dbo.ArtworkType as a inner join Trade_To_Pms.dbo.ArtworkType as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.ArtworkType(
       ID
      ,Abbreviation
      ,Classify
      ,Seq
      ,Junk
      ,ArtworkUnit
      ,ProductionUnit
      ,IsTMS
      ,IsPrice
      ,IsArtwork
      ,IsTtlTMS
      ,Remark
      ,ReportDropdown
      ,UseArtwork
      ,SystemType
	  ,InhouseOSP
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       ID
      ,Abbreviation
      ,Classify
      ,Seq
      ,Junk
      ,ArtworkUnit
      ,ProductionUnit
      ,IsTMS
      ,IsPrice
      ,IsArtwork
      ,IsTtlTMS
      ,Remark
      ,ReportDropdown
      ,UseArtwork
      ,SystemType
	  ,'O'
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.ArtworkType as b
where not exists(select id from Production.dbo.ArtworkType as a where a.id = b.id)



--Artworktype1 MachineType 無多的欄位
--AArtworkType1
----------------------刪除主TABLE多的資料
Delete Production.dbo.MachineType
from Production.dbo.MachineType as a left join Trade_To_Pms.dbo.MachineType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
	   a.ID		      =b.ID
      ,a.Description	=b.Description
      ,a.DescCH		      =b.DescCH
      ,a.ISO		      =b.ISO
      ,a.ArtworkTypeID		      =b.ArtworkTypeID
      ,a.ArtworkTypeDetail		      =b.ArtworkTypeDetail
      ,a.Mold		      =b.Mold
      ,a.RPM		      =b.RPM
      ,a.Stitches		      =b.Stitches
      ,a.Picture1		      =b.Picture1
      ,a.Picture2		      =b.Picture2
      ,a.MachineAllow		      =b.MachineAllow
      ,a.ManAllow		      =b.ManAllow
      ,a.MachineGroupID		      =b.MachineGroupID
      ,a.Junk		      =b.Junk
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate
from Production.dbo.MachineType as a inner join Trade_To_Pms.dbo.MachineType as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.MachineType(
		ID
      ,Description
      ,DescCH
      ,ISO
      ,ArtworkTypeID
      ,ArtworkTypeDetail
      ,Mold
      ,RPM
      ,Stitches
      ,Picture1
      ,Picture2
      ,MachineAllow
      ,ManAllow
      ,MachineGroupID
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
		ID
      ,Description
      ,DescCH
      ,ISO
      ,ArtworkTypeID
      ,ArtworkTypeDetail
      ,Mold
      ,RPM
      ,Stitches
      ,Picture1
      ,Picture2
      ,MachineAllow
      ,ManAllow
      ,MachineGroupID
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.MachineType as b
where not exists(select id from Production.dbo.MachineType as a where a.id = b.id)


--CustCD CustCD
--ACustCD
--PMS多的 [Dest]
----------------------刪除主TABLE多的資料
Delete Production.dbo.CustCD
from Production.dbo.CustCD as a left join Trade_To_Pms.dbo.CustCD as b
on a.id = b.id and a.BrandID=b.BrandID
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.BrandID		     =b.BrandID
      ,a.Junk		      =b.Junk
      ,a.ID		      =b.ID
      ,a.CountryID		      =b.CountryID
      ,a.City		      =b.City
      ,a.QuotaArea		      =b.QuotaArea
      ,a.ScanAndPack		      =b.ScanAndPack
      ,a.ZipperInsert		      =b.ZipperInsert
      ,a.SpecialCust		      =b.SpecialCust
      ,a.VasShas		      =b.VasShas
      ,a.Guid		      =b.Guid
      ,a.Factories		      =b.Factories
      ,a.PayTermARIDBulk		      =b.PayTermARIDBulk
      ,a.PayTermARIDSample		      =b.PayTermARIDSample
      ,a.ProformaInvoice		      =b.ProformaInvoice
      ,a.BankIDSample		      =b.BankIDSample
      ,a.BankIDBulk		      =b.BankIDBulk
      ,a.BrandLabel		      =b.BrandLabel
      ,a.MarkFront		      =b.MarkFront
      ,a.MarkBack		      =b.MarkBack
      ,a.MarkLeft		      =b.MarkLeft
      ,a.MarkRight		      =b.MarkRight
      ,a.BillTo		      =b.BillTo
      ,a.ShipTo		      =b.ShipTo
      ,a.Consignee		      =b.Consignee
      ,a.Notify		      =b.Notify
      ,a.Anotify		      =b.Anotify
      ,a.ShipRemark		      =b.ShipRemark
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.CustCD as a inner join Trade_To_Pms.dbo.CustCD as b ON a.id=b.id and a.BrandID=b.BrandID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.CustCD(
BrandID
      ,Junk
      ,ID
      ,CountryID
      ,City
      ,QuotaArea
      ,ScanAndPack
      ,ZipperInsert
      ,SpecialCust
      ,VasShas
      ,Guid
      ,Factories
      ,PayTermARIDBulk
      ,PayTermARIDSample
      ,ProformaInvoice
      ,BankIDSample
      ,BankIDBulk
      ,BrandLabel
      ,MarkFront
      ,MarkBack
      ,MarkLeft
      ,MarkRight
      ,BillTo
      ,ShipTo
      ,Consignee
      ,Notify
      ,Anotify
      ,ShipRemark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
BrandID
      ,Junk
      ,ID
      ,CountryID
      ,City
      ,QuotaArea
      ,ScanAndPack
      ,ZipperInsert
      ,SpecialCust
      ,VasShas
      ,Guid
      ,Factories
      ,PayTermARIDBulk
      ,PayTermARIDSample
      ,ProformaInvoice
      ,BankIDSample
      ,BankIDBulk
      ,BrandLabel
      ,MarkFront
      ,MarkBack
      ,MarkLeft
      ,MarkRight
      ,BillTo
      ,ShipTo
      ,Consignee
      ,Notify
      ,Anotify
      ,ShipRemark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.CustCD as b
where not exists(select id from Production.dbo.CustCD as a where a.id = b.id and a.BrandID=b.BrandID)


--Reason ReasonType
--AReason
--Reason1 Reason
----------------------刪除主TABLE多的資料
Delete Production.dbo.Reason
from Production.dbo.Reason as a left join Trade_To_Pms.dbo.Reason as b
on a.id = b.id and a.ReasonTypeID = b.ReasonTypeID
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ReasonTypeID	        = b.ReasonTypeID	
      ,a.ID	       = b.ID	
      ,a.Name	       = b.Name	
      ,a.Remark	       = b.Remark	
      ,a.No	       = b.No	
      ,a.ReasonGroup	       = b.ReasonGroup	
      ,a.Kpi	       = b.Kpi	
      ,a.AccountNo	       = b.AccountNo	
      ,a.FactoryKpi	       = b.FactoryKpi	
      ,a.AddName	       = b.AddName	
      ,a.AddDate	       = b.AddDate	
      ,a.EditName	       = b.EditName	
      ,a.EditDate	       = b.EditDate	
      ,a.Junk	       = b.Junk	

from Production.dbo.Reason as a inner join Trade_To_Pms.dbo.Reason as b ON a.id=b.id and a.ReasonTypeID = b.ReasonTypeID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Reason(
       ReasonTypeID
      ,ID
      ,Name
      ,Remark
      ,No
      ,ReasonGroup
      ,Kpi
      ,AccountNo
      ,FactoryKpi
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Junk

)
select 
       ReasonTypeID
      ,ID
      ,Name
      ,Remark
      ,No
      ,ReasonGroup
      ,Kpi
      ,AccountNo
      ,FactoryKpi
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Junk

from Trade_To_Pms.dbo.Reason as b
where not exists(select id from Production.dbo.Reason as a where a.id = b.id and a.ReasonTypeID = b.ReasonTypeID)


--SHIPTerm SHIPTerm
--AShipTerm
----------------------刪除主TABLE多的資料
Delete Production.dbo.SHIPTerm
from Production.dbo.SHIPTerm as a left join Trade_To_Pms.dbo.SHIPTerm as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
		a.ID	   =b.ID	
      ,a.Description	  =b.Description	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.SHIPTerm as a inner join Trade_To_Pms.dbo.SHIPTerm as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.SHIPTerm(
		ID
      ,Description
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
		ID
      ,Description
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.SHIPTerm as b
where not exists(select id from Production.dbo.SHIPTerm as a where a.id = b.id)

--Factory
--PMS 多的欄位
--,[FTYGroup]
--,[KeyWord]
--,[TINNo]
--,[VATAccNo]
--,[WithholdingRateAccNo]
--,[CreditBankAccNo]
--,[Manager]
--,[UseAPS]
--,[UseSBTS]
--,[CheckDeclare]
--,[LocalCMT]
--,[IsSampleRoom]
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	    =b.ID	
      ,a.MDivisionID	      =b.MDivisionID	
      ,a.Junk	      =b.Junk	
      ,a.Abb	      =b.Abb	
      ,a.NameCH	      =b.NameCH	
      ,a.NameEN	      =b.NameEN	
      ,a.CountryID	      =b.CountryID	
      ,a.Tel	      =b.Tel	
      ,a.Fax	      =b.Fax	
      ,a.AddressCH	      =b.AddressCH	
      ,a.AddressEN	      =b.AddressEN	
      ,a.CurrencyID	      =b.CurrencyID	
      ,a.CPU	      =b.CPU	
      ,a.ZipCode	      =b.ZipCode	
      ,a.PortSea	      =b.PortSea	
      ,a.PortAir	      =b.PortAir	
      ,a.KitId	      =b.KitId	
      ,a.ExpressGroup	      =b.ExpressGroup	
      ,a.IECode	      =b.IECode	
      ,a.NegoRegion	      =b.NegoRegion	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.KPICode	      =b.KPICode	
      ,a.Type	      =b.Type	
      ,a.Zone	      =b.Zone	
      ,a.FactorySort	      =b.FactorySort	

from Production.dbo.Factory as a inner join Trade_To_Pms.dbo.Factory as b ON a.id=b.id
--Factory1
--Factory_TMS
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID	
      ,a.Year	      =b.Year	
      ,a.ArtworkTypeID	      =b.ArtworkTypeID	
      ,a.Month	      =b.Month	
      ,a.TMS	      =b.TMS	

from Production.dbo.Factory_Tms as a inner join Trade_To_Pms.dbo.Factory_Tms as b ON a.id=b.id and a.Year=b.Year and a.ArtworkTypeID=b.ArtworkTypeID and a.Month=b.Month
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Factory_Tms(
      ID
      ,Year
      ,ArtworkTypeID
      ,Month
      ,TMS

)
select 
      ID
      ,Year
      ,ArtworkTypeID
      ,Month
      ,TMS

from Trade_To_Pms.dbo.Factory_Tms as b
where not exists(select id from Production.dbo.Factory_Tms as a where a.id = b.id and a.Year=b.Year and a.ArtworkTypeID=b.ArtworkTypeID and a.Month=b.Month)



--Factory4
--Factory_BrandDefinition
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID		     =b.ID
      ,a.BrandID		      =b.BrandID
      ,a.CDCodeID		      =b.CDCodeID
      ,a.BrandAreaCode		      =b.BrandAreaCode
      ,a.BrandFTYCode		      =b.BrandFTYCode
      ,a.BrandVendorCode		      =b.BrandVendorCode
      ,a.BrandReportCode		      =b.BrandReportCode
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Factory_BrandDefinition as a inner join Trade_To_Pms.dbo.Factory_BrandDefinition as b ON a.id=b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID
where b.EditDate > a.EditDate

UPDATE a
SET  
       a.ID		     =b.ID
      ,a.BrandID		      =b.BrandID
      ,a.CDCodeID		      =b.CDCodeID
      ,a.BrandAreaCode		      =b.BrandAreaCode
      ,a.BrandFTYCode		      =b.BrandFTYCode
      ,a.BrandVendorCode		      =b.BrandVendorCode
      ,a.BrandReportCode		      =b.BrandReportCode
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      --,a.EditName		      =b.EditName
      --,a.EditDate		      =b.EditDate

from Production.dbo.Factory_BrandDefinition as a inner join Trade_To_Pms.dbo.Factory_BrandDefinition as b ON a.id=b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID
where b.EditDate <= a.EditDate
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Factory_BrandDefinition(
ID
      ,BrandID
      ,CDCodeID
      ,BrandAreaCode
      ,BrandFTYCode
      ,BrandVendorCode
      ,BrandReportCode
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
ID
      ,BrandID
      ,CDCodeID
      ,BrandAreaCode
      ,BrandFTYCode
      ,BrandVendorCode
      ,BrandReportCode
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Factory_BrandDefinition as b
where not exists(select id from Production.dbo.Factory_BrandDefinition as a where a.id = b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID)




--Factory5
--Factory_WorkHour
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	     =b.ID	
      ,a.Year	      =b.Year	
      ,a.Month	      =b.Month	
      ,a.HalfMonth1	      =b.HalfMonth1	
      ,a.HalfMonth2	      =b.HalfMonth2	

from Production.dbo.Factory_WorkHour as a inner join Trade_To_Pms.dbo.Factory_WorkHour as b ON a.id=b.id and a.Year=b.Year and a.Month=b.Month
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Factory_WorkHour(
       ID
      ,Year
      ,Month
      ,HalfMonth1
      ,HalfMonth2

)
select 
       ID
      ,Year
      ,Month
      ,HalfMonth1
      ,HalfMonth2

from Trade_To_Pms.dbo.Factory_WorkHour as b
where not exists(select id from Production.dbo.Factory_WorkHour as a where a.id = b.id and a.Year=b.Year and a.Month=b.Month)




--SCI_FTY
--SCIFTY
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	     =b.ID	
      ,a.Junk	      =b.Junk	
      ,a.Abb	      =b.Abb	
      ,a.NameCH	      =b.NameCH	
      ,a.NameEN	      =b.NameEN	
      ,a.CountryID	      =b.CountryID	
      ,a.Tel	      =b.Tel	
      ,a.Fax	      =b.Fax	
      ,a.AddressCH	      =b.AddressCH	
      ,a.AddressEN	      =b.AddressEN	
      ,a.CurrencyID	      =b.CurrencyID	
      ,a.ExpressGroup	      =b.ExpressGroup	
      ,a.PortAir	      =b.PortAir	
      ,a.MDivisionID	      =b.MDivisionID	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.SCIFty as a inner join Trade_To_Pms.dbo.Factory as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.SCIFty(
       ID
      ,Junk
      ,Abb
      ,NameCH
      ,NameEN
      ,CountryID
      ,Tel
      ,Fax
      ,AddressCH
      ,AddressEN
      ,CurrencyID
      ,ExpressGroup
      ,PortAir
      ,MDivisionID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       ID
      ,Junk
      ,Abb
      ,NameCH
      ,NameEN
      ,CountryID
      ,Tel
      ,Fax
      ,AddressCH
      ,AddressEN
      ,CurrencyID
      ,ExpressGroup
      ,PortAir
      ,MDivisionID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Factory as b
where not exists(select id from Production.dbo.SCIFty as a where a.id = b.id)

--Unit   Unit 
----------------------刪除主TABLE多的資料
Delete Production.dbo.Unit
from Production.dbo.Unit as a left join Trade_To_Pms.dbo.Unit as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID	
      ,a.PriceRate	      =b.PriceRate	
      ,a.Round	      =b.Round	
      ,a.Description	      =b.Description	
      ,a.ExtensionUnit	      =b.ExtensionUnit	
      ,a.Junk	      =b.TradeJunk	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Unit as a inner join Trade_To_Pms.dbo.Unit as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Unit(
       ID
      ,PriceRate
      ,Round
      ,Description
      ,ExtensionUnit
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       ID
      ,PriceRate
      ,Round
      ,Description
      ,ExtensionUnit
      ,TradeJunk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Unit as b
where not exists(select id from Production.dbo.Unit as a where a.id = b.id)


--這邊開始用PMS_TEST做測試喔
--UnitRate
--Unit_Rate
----------------------刪除主TABLE多的資料
Delete Production.dbo.Unit_Rate
from Production.dbo.Unit_Rate as a left join Trade_To_Pms.dbo.Unit_Rate as b
on a.UnitFrom = b.UnitFrom and a.UnitTo = b.UnitTo
where b.UnitFrom is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.UnitFrom	    =b.UnitFrom	
      ,a.UnitTo	      =b.UnitTo	
      ,a.Rate	      =b.Rate	
      ,a.RateValue	      =b.RateValue	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Unit_Rate as a inner join Trade_To_Pms.dbo.Unit_Rate as b ON a.UnitFrom = b.UnitFrom and a.UnitTo = b.UnitTo
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Unit_Rate(
       UnitFrom
      ,UnitTo
      ,Rate
      ,RateValue
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       UnitFrom
      ,UnitTo
      ,Rate
      ,RateValue
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Unit_Rate as b
where not exists(select UnitFrom from Production.dbo.Unit_Rate as a where a.UnitFrom = b.UnitFrom  and a.UnitTo = b.UnitTo)


--TPI_Pass1 
--TPEPass1<<<PMS
--Pass1<<TRADE
----------------------刪除主TABLE多的資料
Delete Production.dbo.TPEPass1
from Production.dbo.TPEPass1 as a left join Trade_To_Pms.dbo.Pass1 as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
 a.ID	     =b.ID	
      ,a.Name	      =b.Name	
      ,a.ExtNo	      =b.ExtNo	
      ,a.EMail	      =b.EMail	
from Production.dbo.TPEPass1 as a inner join Trade_To_Pms.dbo.Pass1 as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.TPEPass1(
       ID
      ,Name
      ,ExtNo
      ,EMail
)
select 
       ID
      ,Name
      ,ExtNo
      ,EMail
from Trade_To_Pms.dbo.Pass1 as b
where not exists(select id from Production.dbo.TPEPass1 as a where a.id = b.id)

--Color  PMS多一個[Ukey]
--Color
------------------------刪除主TABLE多的資料
Delete Production.dbo.Color
from Production.dbo.Color as a left join Trade_To_Pms.dbo.Color as b
on a.id = b.id and a.BrandId = b.BrandId
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
 a.BrandId		 =b.BrandId
      ,a.ID		      =b.ID
      ,a.Name		      =b.Name
      ,a.Varicolored		      =b.Varicolored
      ,a.JUNK		      =b.JUNK
      ,a.VIVID		      =b.VIVID
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Color as a inner join Trade_To_Pms.dbo.Color as b ON a.id=b.id and a.BrandId = b.BrandId
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Color(
       BrandId
      ,ID
      ,Name
      ,Varicolored
      ,JUNK
      ,VIVID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       BrandId
      ,ID
      ,Name
      ,Varicolored
      ,JUNK
      ,VIVID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Color as b
where not exists(select id from Production.dbo.Color as a where a.id = b.id and a.BrandId = b.BrandId)


--Currency
--Currency
----------------------刪除主TABLE多的資料
Delete Production.dbo.Currency
from Production.dbo.Currency as a left join Trade_To_Pms.dbo.Currency as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID		      =b.ID
      ,a.StdRate		      =b.StdRate
      ,a.NameCH		      =b.NameCH
      ,a.NameEN		      =b.NameEN
      ,a.Junk		      =b.Junk
      ,a.Exact		      =b.Exact
      ,a.Symbol		      =b.Symbol
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Currency as a inner join Trade_To_Pms.dbo.Currency as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Currency(
ID
      ,StdRate
      ,NameCH
      ,NameEN
      ,Junk
      ,Exact
      ,Symbol
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
ID
      ,StdRate
      ,NameCH
      ,NameEN
      ,Junk
      ,Exact
      ,Symbol
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Currency as b
where not exists(select id from Production.dbo.Currency as a where a.id = b.id)


--Fab_Loss
--LossRateFabric   
----------------------刪除主TABLE多的資料
Delete Production.dbo.LossRateFabric
from Production.dbo.LossRateFabric as a left join Trade_To_Pms.dbo.LossRateFabric as b
on a.WeaveTypeID = b.WeaveTypeID
where b.WeaveTypeID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.WeaveTypeID	     =b.WeaveTypeID		
      ,a.LossType	      =b.LossType		
      ,a.Limit	      =b.Limit		
      ,a.LimitDown	      =b.LimitDown		
      ,a.TWLimitDown	      =b.TWLimitDown		
      ,a.NonTWLimitDown	      =b.NonTWLimitDown		
      ,a.LimitUp	      =b.LimitUp		
      ,a.TWLimitUp	      =b.TWLimitUp		
      ,a.NonTWLimitUP	      =b.NonTWLimitUP		
      ,a.Allowance	      =b.Allowance		
      ,a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =b.EditName		
      ,a.EditDate	      =b.EditDate		

from Production.dbo.LossRateFabric as a inner join Trade_To_Pms.dbo.LossRateFabric as b ON a.WeaveTypeID=b.WeaveTypeID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.LossRateFabric(
       WeaveTypeID
      ,LossType
      ,Limit
      ,LimitDown
      ,TWLimitDown
      ,NonTWLimitDown
      ,LimitUp
      ,TWLimitUp
      ,NonTWLimitUP
      ,Allowance
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
       WeaveTypeID
      ,LossType
      ,Limit
      ,LimitDown
      ,TWLimitDown
      ,NonTWLimitDown
      ,LimitUp
      ,TWLimitUp
      ,NonTWLimitUP
      ,Allowance
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.LossRateFabric as b
where not exists(select WeaveTypeID from Production.dbo.LossRateFabric as a where a.WeaveTypeID = b.WeaveTypeID)
--Acc_Loss
--LossRateAccessory
--PMS多一個,[Waste] 自己記算
----------------------刪除主TABLE多的資料
Delete Production.dbo.LossRateAccessory
from Production.dbo.LossRateAccessory as a left join Trade_To_Pms.dbo.LossRateAccessory as b
on a.MtltypeId = b.MtltypeId
where b.MtltypeId is null
and a.Waste = 0

-------------------------- INSERT INTO 不INSERT waste的欄位
INSERT INTO Production.dbo.LossRateAccessory(
MtltypeId
      ,LossUnit
      ,LossTW
      ,LossNonTW
      ,PerQtyTW
      ,PlsQtyTW
      ,PerQtyNonTW
      ,PlsQtyNonTW
      ,FOCTW
      ,FOCNonTW
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
MtltypeId
      ,LossUnit
      ,LossTW
      ,LossNonTW
      ,PerQtyTW
      ,PlsQtyTW
      ,PerQtyNonTW
      ,PlsQtyNonTW
      ,FOCTW
      ,FOCNonTW
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.LossRateAccessory as b
where not exists(select MtltypeId from Production.dbo.LossRateAccessory as a where a.MtltypeId = b.MtltypeId)


-------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a--特別處理waste的欄位
SET  
      a.MtltypeId	     =b.MtltypeId	
      ,a.LossUnit	      =b.LossUnit	
      ,a.LossTW	      =b.LossTW	
      ,a.LossNonTW	      =b.LossNonTW	
      ,a.PerQtyTW	      =b.PerQtyTW	
      ,a.PlsQtyTW	      =b.PlsQtyTW	
      ,a.PerQtyNonTW	      =b.PerQtyNonTW	
      ,a.PlsQtyNonTW	      =b.PlsQtyNonTW	
      ,a.FOCTW	      =b.FOCTW	
      ,a.FOCNonTW	      =b.FOCNonTW	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.Waste	      = (b.LossTW + b.LossNonTW)/2
	  

from Production.dbo.LossRateAccessory as a inner join Trade_To_Pms.dbo.LossRateAccessory as b ON a.MtltypeId=b.MtltypeId
where a.LossUnit=1

UPDATE a--特別處理waste的欄位
SET  
      a.MtltypeId	     =b.MtltypeId	
      ,a.LossUnit	      =b.LossUnit	
      ,a.LossTW	      =b.LossTW	
      ,a.LossNonTW	      =b.LossNonTW	
      ,a.PerQtyTW	      =b.PerQtyTW	
      ,a.PlsQtyTW	      =b.PlsQtyTW	
      ,a.PerQtyNonTW	      =b.PerQtyNonTW	
      ,a.PlsQtyNonTW	      =b.PlsQtyNonTW	
      ,a.FOCTW	      =b.FOCTW	
      ,a.FOCNonTW	      =b.FOCNonTW	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.Waste	      =IIF((b.PerQtyTW + b.PerQtyNonTW)=0,'0',(b.PlsQtyTW + b.PlsQtyNonTW)/(b.PerQtyTW + b.PerQtyNonTW) * 100) 
from Production.dbo.LossRateAccessory as a inner join Trade_To_Pms.dbo.LossRateAccessory as b ON a.MtltypeId=b.MtltypeId
where a.LossUnit <> 1



--Cutparts   跳過先 TRADE還未準備完成
--Carrier
--Carrier  
----------------------刪除主TABLE多的資料
Delete Production.dbo.Carrier
from Production.dbo.Carrier as a left join Trade_To_Pms.dbo.Carrier as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID		 =b.ID
      ,a.SuppID		      =b.SuppID
      ,a.Account		      =b.Account
      ,a.Junk		      =b.Junk
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Carrier as a inner join Trade_To_Pms.dbo.Carrier as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Carrier(
       ID
      ,SuppID
      ,Account
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       ID
      ,SuppID
      ,Account
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Carrier as b
where not exists(select id from Production.dbo.Carrier as a where a.id = b.id)



--PayTerm
--PayTermAP    
----------------------刪除主TABLE多的資料
Delete Production.dbo.PayTermAP
from Production.dbo.PayTermAP as a left join Trade_To_Pms.dbo.PayTermAP as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID			      =b.ID
      ,a.Description			      =b.Description
      ,a.Term			      =b.Term
      ,a.BeforeAfter			      =b.BeforeAfter
      ,a.BaseDate			      =b.BaseDate
      ,a.AccountDay			      =b.AccountDay
      ,a.CloseAccountDay			      =b.CloseAccountDay
      ,a.CloseMonth			      =b.CloseMonth
      ,a.CloseDay			      =b.CloseDay
      ,a.DueAccountday			      =b.DueAccountday
      ,a.DueMonth			      =b.DueMonth
      ,a.DueDay			      =b.DueDay
      ,a.JUNK			      =b.JUNK
      ,a.AddName			      =b.AddName
      ,a.AddDate			      =b.AddDate
      ,a.EditName			      =b.EditName
      ,a.EditDate			      =b.EditDate

from Production.dbo.PayTermAP as a inner join Trade_To_Pms.dbo.PayTermAP as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.PayTermAP(
		ID
      ,Description
      ,Term
      ,BeforeAfter
      ,BaseDate
      ,AccountDay
      ,CloseAccountDay
      ,CloseMonth
      ,CloseDay
      ,DueAccountday
      ,DueMonth
      ,DueDay
      ,JUNK
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
		ID
      ,Description
      ,Term
      ,BeforeAfter
      ,BaseDate
      ,AccountDay
      ,CloseAccountDay
      ,CloseMonth
      ,CloseDay
      ,DueAccountday
      ,DueMonth
      ,DueDay
      ,JUNK
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.PayTermAP as b
where not exists(select id from Production.dbo.PayTermAP as a where a.id = b.id)


--Mtype
--MachineGroup
--MACHINE 多一個 [InspLeadTime]
----------------------刪除主TABLE多的資料
Delete Machine_Test.dbo.MachineGroup
from Machine_Test.dbo.MachineGroup as a left join Trade_To_Pms.dbo.MachineGroup as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID	
      ,a.Description	      =b.Description	
      ,a.DescCH	      =b.DescriptionCH	
      ,a.Substitute	      =b.Substitute	
      ,a.Junk	      =b.Junk	
      ,a.Picture1	      =b.Picture1	
      ,a.Picture2	      =b.Picture2	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Machine_Test.dbo.MachineGroup as a inner join Trade_To_Pms.dbo.MachineGroup as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Machine_Test.dbo.MachineGroup(
ID
      ,Description
      ,DescCH
      ,Substitute
      ,Junk
      ,Picture1
      ,Picture2
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
ID
      ,Description
      ,DescriptionCH
      ,Substitute
      ,Junk
      ,Picture1
      ,Picture2
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.MachineGroup as b
where not exists(select id from Machine_Test.dbo.MachineGroup as a where a.id = b.id)


--Mockup
--Mockup
----------------------刪除主TABLE多的資料
Delete Production.dbo.Mockup
from Production.dbo.Mockup as a left join Trade_To_Pms.dbo.Mockup as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	 =b.ID	
      ,a.Description	      =b.Description	
      ,a.SMV	      =b.SMV	
      ,a.CPU	      =b.CPU	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Mockup as a inner join Trade_To_Pms.dbo.Mockup as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Mockup(
       ID
      ,Description
      ,SMV
      ,CPU
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
      ID
      ,Description
      ,SMV
      ,CPU
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Mockup as b
where not exists(select id from Production.dbo.Mockup as a where a.id = b.id)


--OrderType  不用寫DELETE
--OrderType  
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
        a.ID	 =b.ID	
      ,a.BrandID	      =b.BrandID	
      ,a.PhaseID	      =b.PhaseID	
      ,a.ProjectID	      =b.ProjectID	
      ,a.Junk	      =b.Junk	
      ,a.CpuRate	      =b.CpuRate	
      ,a.Category	      =b.Category	
      ,a.PriceDays	      =b.PriceDays	
      ,a.MtlLetaDays	      =b.MtlLetaDays	
      ,a.EachConsDays	      =b.EachConsDays	
      ,a.KPI	      =b.KPI	
      ,a.Remark	      =b.Remark	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.OrderType as a inner join Trade_To_Pms.dbo.OrderType as b ON a.id=b.id and a.BrandID =b.BrandID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.OrderType(
		ID
      ,BrandID
      ,PhaseID
      ,ProjectID
      ,Junk
      ,CpuRate
      ,Category
      ,PriceDays
      ,MtlLetaDays
      ,EachConsDays
      ,KPI
      ,Remark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
		 ID
      ,BrandID
      ,PhaseID
      ,ProjectID
      ,Junk
      ,CpuRate
      ,Category
      ,PriceDays
      ,MtlLetaDays
      ,EachConsDays
      ,KPI
      ,Remark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.OrderType as b
where not exists(select id from Production.dbo.OrderType as a where a.id = b.id and a.BrandID =b.BrandID)


--CustPrg
--Program  
----------------------刪除主TABLE多的資料
Delete Production.dbo.Program
from Production.dbo.Program as a left join Trade_To_Pms.dbo.Program as b
on a.id = b.id and a.BrandID = b.BrandID
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID	
      ,a.BrandID	      =b.BrandID	
      ,a.RateCost	      =b.RateCost	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Program as a inner join Trade_To_Pms.dbo.Program as b ON a.id=b.id and a.BrandID = b.BrandID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Program(
 ID
      ,BrandID
      ,RateCost
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
 ID
      ,BrandID
      ,RateCost
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Program as b
where not exists(select id from Production.dbo.Program as a where a.id = b.id and a.BrandID = b.BrandID)


--FabFactor
--MtlFactor 無不同欄位
----------------------刪除主TABLE多的資料
Delete Production.dbo.MtlFactor
from Production.dbo.MtlFactor as a left join Trade_To_Pms.dbo.MtlFactor as b
on a.id = b.id and a.Type = b.Type
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.Type	     =b.Type	
      ,a.ID	      =b.ID	
      ,a.Pattern	      =b.Pattern	
      ,a.PatternCode	      =b.PatternCode	
      ,a.Drape	      =b.Drape	
      ,a.DrapeCode	      =b.DrapeCode	
      ,a.Color	      =b.Color	
      ,a.ColorCode	      =b.ColorCode	
      ,a.Rate	      =b.Rate	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.MtlFactor as a inner join Trade_To_Pms.dbo.MtlFactor as b ON a.id=b.id and a.Type = b.Type
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.MtlFactor(
 Type
      ,ID
      ,Pattern
      ,PatternCode
      ,Drape
      ,DrapeCode
      ,Color
      ,ColorCode
      ,Rate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
 Type
      ,ID
      ,Pattern
      ,PatternCode
      ,Drape
      ,DrapeCode
      ,Color
      ,ColorCode
      ,Rate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.MtlFactor as b
where not exists(select id from Production.dbo.MtlFactor as a where a.id = b.id and a.Type = b.Type)


--Cstrutype
--Construction 無不同欄位
----------------------刪除主TABLE多的資料
Delete Production.dbo.Construction
from Production.dbo.Construction as a left join Trade_To_Pms.dbo.Construction as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.Id		     =b.Id	
      ,a.Name		      =b.Name	
      ,a.CuttingLayer		      =b.CuttingLayer	
      ,a.Junk		      =b.Junk	
      ,a.AddName		      =b.AddName	
      ,a.AddDate		      =b.AddDate	
      ,a.EditName		      =b.EditName	
      ,a.EditDate		      =b.EditDate	

from Production.dbo.Construction as a inner join Trade_To_Pms.dbo.Construction as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Construction(
       Id
      ,Name
      ,CuttingLayer
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       Id
      ,Name
      ,CuttingLayer
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Construction as b
where not exists(select id from Production.dbo.Construction as a where a.id = b.id)


--Port
--Port 無多傳欄位
----------------------刪除主TABLE多的資料
Delete Production.dbo.Port
from Production.dbo.Port as a left join Trade_To_Pms.dbo.Port as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	     =b.ID	
      ,a.CountryID	      =b.CountryID	
      ,a.Remark	      =b.Remark	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.Junk	      =b.Junk	

from Production.dbo.Port as a inner join Trade_To_Pms.dbo.Port as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Port(
ID
      ,CountryID
      ,Remark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Junk

)
select 
ID
      ,CountryID
      ,Remark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Junk

from Trade_To_Pms.dbo.Port as b
where not exists(select id from Production.dbo.Port as a where a.id = b.id)







--Formlist 跳過 目前無轉出TABLE
--DO releasememvar WITH 'FS_CMTPlus'
----------------------刪除主TABLE多的資料
Delete Production.dbo.FSRCpuCost
from Production.dbo.FSRCpuCost as a left join Trade_To_Pms.dbo.FSRCpuCost as b
on a.ShipperID = b.ShipperID
where b.ShipperID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ShipperID	    =b.ShipperID	
      ,a.AddDate	      =b.AddDate	
      ,a.AddName	      =b.AddName	
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =b.EditName	

from Production.dbo.FSRCpuCost as a inner join Trade_To_Pms.dbo.FSRCpuCost as b ON a.ShipperID=b.ShipperID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FSRCpuCost(
       ShipperID
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName

)
select 
       ShipperID
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName

from Trade_To_Pms.dbo.FSRCpuCost as b
where not exists(select ShipperID from Production.dbo.FSRCpuCost as a where a.ShipperID = b.ShipperID)


--DO releasememvar WITH 'FS_CMTPlus1'
	----------------------刪除主TABLE多的資料
Delete Production.dbo.FSRCpuCost_Detail
from Production.dbo.FSRCpuCost_Detail as a left join Trade_To_Pms.dbo.FSRCpuCost_Detail as b
on a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate
where b.ShipperID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ShipperID	    =b.ShipperID	
      ,a.BeginDate	      =b.BeginDate	
      ,a.EndDate	      =b.EndDate	
      ,a.CpuCost	      =b.CpuCost	
      ,a.AddDate	      =b.AddDate	
      ,a.AddName	      =b.AddName	
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =b.EditName	

from Production.dbo.FSRCpuCost_Detail as a inner join Trade_To_Pms.dbo.FSRCpuCost_Detail as b ON a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FSRCpuCost_Detail(
       ShipperID
      ,BeginDate
      ,EndDate
      ,CpuCost
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName

)
select 
       ShipperID
      ,BeginDate
      ,EndDate
      ,CpuCost
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName

from Trade_To_Pms.dbo.FSRCpuCost_Detail as b
where not exists(select ShipperID from Production.dbo.FSRCpuCost_Detail as a where a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate)



--DO releasememvar WITH 'FtyShipper'  FtyShipper
----------------------刪除主TABLE多的資料
Delete Production.dbo.FtyShipper
from Production.dbo.FtyShipper as a left join Trade_To_Pms.dbo.FtyShipper as b  
on a.BrandID = b.BrandID and a.FactoryID=b.FactoryID
where b.BrandID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.BrandID	    =b.BrandID	
      ,a.FactoryID	      =b.FactoryID	
      ,a.AddDate	      =b.AddDate	
      ,a.AddName	      =b.AddName	
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =b.EditName	

from Production.dbo.FtyShipper as a inner join Trade_To_Pms.dbo.FtyShipper as b ON a.BrandID = b.BrandID and a.FactoryID=b.FactoryID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FtyShipper(
       BrandID
      ,FactoryID
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName

)
select 
       BrandID
      ,FactoryID
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName

from Trade_To_Pms.dbo.FtyShipper as b
where not exists(select BrandID from Production.dbo.FtyShipper as a where a.BrandID = b.BrandID and a.FactoryID=b.FactoryID	)

--FtyShipper_Detail
--DO releasememvar WITH 'FtyShipper1'
----------------------刪除主TABLE多的資料
Delete Production.dbo.FtyShipper_Detail
from Production.dbo.FtyShipper_Detail as a left join Trade_To_Pms.dbo.FtyShipper_Detail as b
on a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate
where b.BrandID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.BrandID	 =b.BrandID	
      ,a.FactoryID	      =b.FactoryID	
      ,a.BeginDate	      =b.BeginDate	
      ,a.EndDate	      =b.EndDate	
      ,a.ShipperID	      =b.ShipperID	

from Production.dbo.FtyShipper_Detail as a inner join Trade_To_Pms.dbo.FtyShipper_Detail as b ON a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FtyShipper_Detail(
       BrandID
      ,FactoryID
      ,BeginDate
      ,EndDate
      ,ShipperID

)
select 
       BrandID
      ,FactoryID
      ,BeginDate
      ,EndDate
      ,ShipperID

from Trade_To_Pms.dbo.FtyShipper_Detail as b
where not exists(select BrandID from Production.dbo.FtyShipper_Detail as a where a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate)



--Phrase1
--Phrase 無多欄位
----------------------刪除主TABLE多的資料
Delete Production.dbo.Phrase
from Production.dbo.Phrase as a left join Trade_To_Pms.dbo.Phrase as b
on a.PhraseTypeName = b.PhraseTypeName and a.Name = b.Name
where b.PhraseTypeName is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.PhraseTypeName	 =b.PhraseTypeName	
      ,a.Junk	      =b.Junk	
      ,a.Name	      =b.Name	
      ,a.Seq	      =b.Seq	
      ,a.Description	      =b.Description	
      ,a.Module	      =b.Module	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Phrase as a inner join Trade_To_Pms.dbo.Phrase as b ON a.PhraseTypeName=b.PhraseTypeName and a.Name = b.Name
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Phrase(
 PhraseTypeName
      ,Junk
      ,Name
      ,Seq
      ,Description
      ,Module
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
 PhraseTypeName
      ,Junk
      ,Name
      ,Seq
      ,Description
      ,Module
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Phrase as b
where not exists(select PhraseTypeName from Production.dbo.Phrase as a where a.PhraseTypeName = b.PhraseTypeName and a.Name = b.Name)


--Company
--Company 無多傳欄位來
----------------------刪除主TABLE多的資料
Delete Production.dbo.Company
from Production.dbo.Company as a left join Trade_To_Pms.dbo.Company as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID	
      ,a.Title	      =b.Title	
      ,a.Abbr	      =b.Abbr	
      ,a.Country	      =b.Country	
      ,a.Junk	      =b.Junk	
      ,a.Currency	      =b.Currency	
      ,a.NameCH	      =b.NameCH	
      ,a.NameEN	      =b.NameEN	
      ,a.hasTax	      =b.hasTax	
      ,a.IsDefault	      =b.IsDefault	
      ,a.VatNO	      =b.VatNO	
      ,a.AddressCH	      =b.AddressCH	
      ,a.AddressEN	      =b.AddressEN	
      ,a.Tel	      =b.Tel	
      ,a.Fax	      =b.Fax	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Company as a inner join Trade_To_Pms.dbo.Company as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Company(
       ID
      ,Title
      ,Abbr
      ,Country
      ,Junk
      ,Currency
      ,NameCH
      ,NameEN
      ,hasTax
      ,IsDefault
      ,VatNO
      ,AddressCH
      ,AddressEN
      ,Tel
      ,Fax
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       ID
      ,Title
      ,Abbr
      ,Country
      ,Junk
      ,Currency
      ,NameCH
      ,NameEN
      ,hasTax
      ,IsDefault
      ,VatNO
      ,AddressCH
      ,AddressEN
      ,Tel
      ,Fax
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.Company as b
where not exists(select id from Production.dbo.Company as a where a.id = b.id)


--MiFabCode
--ADIDASMiSetup_ColorComb 無多給欄位
----------------------刪除主TABLE多的資料
Delete Production.dbo.ADIDASMiSetup_ColorComb
from Production.dbo.ADIDASMiSetup_ColorComb as a left join Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.ID	      =b.ID	
      ,a.ExcelName	      =b.ExcelName	
      ,a.ExcelColumn	      =b.ExcelColumn	
      ,a.isArtwork	      =b.isArtwork	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.ADIDASMiSetup_ColorComb as a inner join Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.ADIDASMiSetup_ColorComb(
ID
      ,ExcelName
      ,ExcelColumn
      ,isArtwork
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
ID
      ,ExcelName
      ,ExcelColumn
      ,isArtwork
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

from Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b
where not exists(select id from Production.dbo.ADIDASMiSetup_ColorComb as a where a.id = b.id)




  
END




