-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[imp_Base]
	
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
      -- a. ID	      = b. ID
      a. NameCH	      = isnull(b. NameCH, '')
      ,a. NameEN	      = isnull(b. NameEN, '')
      ,a. CountryID	      = isnull(b. CountryID, '')
      ,a. BuyerID	      = isnull(b. BuyerID, '')
      ,a. Tel	      = isnull(b. Tel, '')
      ,a. Fax	      = isnull(b. Fax, '')
      ,a. Contact1	      = isnull(b. Contact1, '')
      ,a. Contact2	      = isnull(b. Contact2, '')
      ,a. AddressCH	      = isnull(b. AddressCH, '')
      ,a. AddressEN	      = isnull(b. AddressEN, '')
      ,a. CurrencyID	      = isnull(b. CurrencyID, '')
      ,a. Remark	      = isnull(b. Remark, '')
      ,a. Customize1	      = isnull(b. Customize1, '')
      ,a. Customize2	      = isnull(b. Customize2, '')
      ,a. Customize3	      = isnull(b. Customize3, '')
      ,a. Commission	      = isnull(b. Commission, 0)
      ,a. ZipCode	      = isnull(b. ZipCode, '')
      ,a. Email	      = isnull(b. Email, '')
      ,a. MrTeam	      = isnull(b. MrTeam, '')
      ,a. BrandGroup	      = isnull(b. BrandGroup, '')
      ,a. ApparelXlt	      = isnull(b. ApparelXlt, '')
      ,a. LossSampleFabric	      = isnull(b. LossSampleFabric, 0)
      ,a. PayTermARIDBulk	      = isnull(b. PayTermARIDBulk, '')
      ,a. PayTermARIDSample	      = isnull(b. PayTermARIDSample, '')
      ,a. BrandFactoryAreaCaption	      = isnull(b. BrandFactoryAreaCaption, '')
      ,a. BrandFactoryCodeCaption	      = isnull(b. BrandFactoryCodeCaption, '')
      ,a. BrandFactoryVendorCaption	      = isnull(b. BrandFactoryVendorCaption, '')
      ,a. ShipCode	      = isnull(b. ShipCode, '')
      ,a. Junk	      = isnull(b. Junk, 0)
      ,a. AddName	      = isnull(b. AddName, '')
      ,a. AddDate	      = b. AddDate
      ,a. EditName	      = isnull(b. EditName, '')
      ,a. EditDate	      = b. EditDate
	  ,a. LossSampleAccessory = isnull(b.LossSampleAccessory, 0)
	  ,a. OTDExtension = isnull(b.OTDExtension, 0)
	  ,a. UseRatioRule = isnull(b.UseRatioRule, '')
	  ,a. UseRatioRule_Thick =isnull(b.UseRatioRule_Thick, '')
	  ,a. Serial = isnull(b.Serial, 0)
	  ,a. ShipTermID = isnull(b.ShipTermID, '')
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
	  ,LossSampleAccessory
	  ,OTDExtension
	  ,UseRatioRule
	  ,UseRatioRule_Thick
	  ,Serial
	  ,ShipTermID
)
SELECT ID
      ,isnull(NameCH, '')
      ,isnull(NameEN, '')
      ,isnull(CountryID, '')
      ,isnull(BuyerID, '')
      ,isnull(Tel, '')
      ,isnull(Fax, '')
      ,isnull(Contact1, '')
      ,isnull(Contact2, '')
      ,isnull(AddressCH, '')
      ,isnull(AddressEN, '')
      ,isnull(CurrencyID, '')
      ,isnull(Remark, '')
      ,isnull(Customize1, '')
      ,isnull(Customize2, '')
      ,isnull(Customize3, '')
      ,isnull(Commission, 0)
      ,isnull(ZipCode, '')
      ,isnull(Email, '')
      ,isnull(MrTeam, '')
      ,isnull(BrandGroup, '')
      ,isnull(ApparelXlt, '')
      ,isnull(LossSampleFabric, 0)
      ,isnull(PayTermARIDBulk, '')
      ,isnull(PayTermARIDSample, '')
      ,isnull(BrandFactoryAreaCaption, '')
      ,isnull(BrandFactoryCodeCaption, '')
      ,isnull(BrandFactoryVendorCaption, '')
      ,isnull(ShipCode, '')
      ,isnull(Junk, 0)
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
	  ,isnull(LossSampleAccessory, 0)
	  ,isnull(OTDExtension, 0)
	  ,isnull(UseRatioRule, '')
	  ,isnull(UseRatioRule_Thick, '')
	  ,isnull(Serial, 0)
	  ,isnull(ShipTermID,'')
from Trade_To_Pms.dbo.Brand as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Brand as a WITH (NOLOCK) where a.id = b.id)

----------------------Brand_Month
Delete Production.dbo.Brand_Month
from Production.dbo.Brand_Month as a left join Trade_To_Pms.dbo.Brand_Month as b
on a.id = b.id and a.Year = b.Year and a.Month = b.Month
where b.id is null

UPDATE a
SET 	
     a.[MonthLabel]=  isnull(b.[MonthLabel], '')
    ,a.[AddName]	 =isnull(b.[AddName], '')
    ,a.[AddDate]	 =b.[AddDate]
    ,a.[EditName]	 =isnull(b.[EditName], '')
    ,a.[EditDate]	 =b.[EditDate]
from Production.dbo.Brand_Month as a inner join Trade_To_Pms.dbo.Brand_Month as b ON a.id = b.id and a.Year = b.Year and a.Month = b.Month

INSERT INTO [dbo].[Brand_Month]
           ([ID]
           ,[Year]
           ,[Month]
           ,[MonthLabel]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
SELECT
	 isnull([ID], '')
    ,isnull([Year], '')
    ,isnull([Month], '')
    ,isnull([MonthLabel], '')
    ,isnull([AddName], '')
    ,[AddDate]
    ,isnull([EditName], '')
    ,[EditDate]
from Trade_To_Pms.dbo.Brand_Month as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Brand_Month as a WITH (NOLOCK) where a.id = b.id and a.Year = b.Year and a.Month = b.Month)

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
      -- a.ID		 =b.ID
      --,a.BrandID		      =b.BrandID
      a.CostRatio		      =isnull(b.CostRatio, 0)
      ,a.SeasonSCIID		      =isnull(b.SeasonSCIID, '')
      ,a.Month		      =isnull(b.Month, '')
      ,a.Junk		      =isnull(b.Junk, 0)
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
      ,a.EditDate		      =b.EditDate
      ,a.SeasonForDisplay	  =b.SeasonForDisplay
      ,a.EndlineDisplay		  =isnull(b.EndlineDisplay, '')

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
      ,SeasonForDisplay
	  ,EndlineDisplay
)
select 
       isnull(ID, '')
      ,isnull(BrandID, '')
      ,isnull(CostRatio, 0)
      ,isnull(SeasonSCIID, '')
      ,isnull(Month, '')
      ,isnull(Junk, 0)
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
      ,SeasonForDisplay
	  ,isnull(EndlineDisplay, '')
from Trade_To_Pms.dbo.Season as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Season as a WITH (NOLOCK) where a.id = b.id and a.BrandID = b.BrandID)


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
      a.Junk		      =isnull(b.Junk, 0)
      ,a.AbbCH		      =isnull(b.AbbCH, '')
      ,a.AbbEN		      =isnull(b.AbbEN, '')
      ,a.NameCH		      =isnull(b.NameCH, '')
      ,a.NameEN		      =isnull(b.NameEN, '')
      ,a.CountryID		      =isnull(b.CountryID, '')
      ,a.ThirdCountry		      =isnull(b.ThirdCountry, 0)
      ,a.Tel		      =isnull(b.Tel, '')
      ,a.Fax		      =isnull(b.Fax, '')
      ,a.AddressCH		      =isnull(b.AddressCH, '')
      ,a.AddressEN		      =isnull(b.AddressEN, '')
      ,a.ZipCode		      =isnull(b.ZipCode, '')
      ,a.Delay		      =b.Delay
      ,a.DelayMemo		      =isnull(b.DelayMemo, '')
      ,a.LockDate		      =b.LockDate
      ,a.LockMemo		      =isnull(b.LockMemo, '')
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
      ,a.EditDate		      =b.EditDate
      ,a.Currencyid		      =isnull(b.Currencyid, '')
      ,a.SuppGroupFabric		      =isnull(b.SuppGroupFabric, '')

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
      ,SuppGroupFabric
)
select 
	   isnull(ID, '')
      ,isnull(Junk, 0)
      ,isnull(AbbCH, '')
      ,isnull(AbbEN, '')
      ,isnull(NameCH, '')
      ,isnull(NameEN, '')
      ,isnull(CountryID, '')
      ,isnull(ThirdCountry, 0)
      ,isnull(Tel, '')
      ,isnull(Fax, '')
      ,isnull(AddressCH, '')
      ,isnull(AddressEN, '')
      ,isnull(ZipCode, '')
      ,Delay
      ,isnull(DelayMemo, '')
      ,LockDate
      ,isnull(LockMemo, '')
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
      ,isnull(Currencyid, '')
      ,isnull(SuppGroupFabric, '')
from Trade_To_Pms.dbo.Supp as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Supp as a WITH (NOLOCK) where a.id = b.id)

-- Supp_BrandSuppCode
Delete a
from Production.dbo.Supp_BrandSuppCode as a 
left join Trade_To_Pms.dbo.Supp_BrandSuppCode as b on a.ID = b.ID and a.BrandID = b.BrandID
where b.id is null

UPDATE a
SET 
      a.SuppCode		      =isnull(b.SuppCode, '')
      ,a.T2LO		      =isnull(b.T2LO, '')
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
      ,a.EditDate		      =b.EditDate
	  ,a.SuppName		      =isnull(b.SuppName, '')
from Production.dbo.Supp_BrandSuppCode as a
inner join Trade_To_Pms.dbo.Supp_BrandSuppCode as b ON a.ID = b.ID and a.BrandID = b.BrandID

INSERT INTO Production.dbo.Supp_BrandSuppCode(
		ID
      ,BrandID
      ,SuppCode
      ,T2LO
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,SuppName
)
select 
	   isnull(ID, '')
      ,isnull(BrandID, 0)
      ,isnull(SuppCode, '')
      ,isnull(T2LO, '')
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
      ,isnull(SuppName, '')
from Trade_To_Pms.dbo.Supp_BrandSuppCode as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Supp_BrandSuppCode as a WITH (NOLOCK) where a.ID = b.ID and a.BrandID = b.BrandID)

--Supp_ReplaceSupplier
merge Production.dbo.Supp_ReplaceSupplier t 
using Trade_To_Pms.dbo.Supp_ReplaceSupplier s
on t.ID = s.ID and t.BrandID = s.BrandID and t.CountryID = s.CountryID and t.IsECFA = s.IsECFA
when matched then update set
            t.[SuppID]    = s.[SuppID]
           ,t.[AddName]   = s.[AddName]
           ,t.[AddDate]   = s.[AddDate]
           ,t.[EditName]  = s.[EditName]
           ,t.[EditDate]  = s.[EditDate]
           ,t.[FactoryID] = s.[FactoryID]
when not matched by target then
	insert(
            [ID]
           ,[BrandID]
           ,[CountryID]
           ,[SuppID]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate]
           ,[IsECFA]
           ,[FactoryID]
           )
	values(
            s.[ID]
           ,s.[BrandID]
           ,s.[CountryID]
           ,s.[SuppID]
           ,s.[AddName]
           ,s.[AddDate]
           ,s.[EditName]
           ,s.[EditDate]
           ,s.[IsECFA]
           ,s.[FactoryID]
           )
;
--Supp_Replace_Detail
merge Production.dbo.Supp_Replace_Detail t 
using Trade_To_Pms.dbo.Supp_Replace_Detail s
on t.SuppGroupFabric = s.SuppGroupFabric and t.Seq = s.Seq
when matched then update set
           t.[Type]            = s.[Type]
           ,t.[FromCountry]     = s.[FromCountry]
           ,t.[ToCountry]       = s.[ToCountry]
           ,t.[SuppID]          = s.[SuppID]
           ,t.[AddName]         = s.[AddName]
           ,t.[AddDate]         = s.[AddDate]
           ,t.[EditName]        = s.[EditName]
           ,t.[EditDate]        = s.[EditDate]
           ,t.[FactoryKpiCode]  = s.[FactoryKpiCode]
when not matched by target then
	insert(
            [SuppGroupFabric]
           ,[Seq]
           ,[Type]
           ,[FromCountry]
           ,[ToCountry]
           ,[SuppID]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate]
           ,[FactoryKpiCode]
           )
	values(
            s.[SuppGroupFabric]
           ,s.[Seq]
           ,s.[Type]
           ,s.[FromCountry]
           ,s.[ToCountry]
           ,s.[SuppID]
           ,s.[AddName]
           ,s.[AddDate]
           ,s.[EditName]
           ,s.[EditDate]
           ,s.[FactoryKpiCode]
           )
;

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
      -- a.ID					 =b.ID
      a.Junk		         =isnull(b.Junk, 0)
      ,a.Description		 =isnull(b.Description, '')
      ,a.Cpu		          =isnull(b.Cpu, 0)
      ,a.ComboPcs		      =isnull(b.ComboPcs, 0)
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
      ,a.EditDate		      =b.EditDate
      ,a.ProductionFamilyID		      =isnull(b.ProductionFamilyID, '')
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
	  ,ProductionFamilyID
)
select 
	   isnull(ID, '')
      ,isnull(Junk, 0)
      ,isnull(Description, '')
      ,isnull(Cpu, 0)
      ,isnull(ComboPcs, 0)
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
	  ,isnull(ProductionFamilyID, '')
from Trade_To_Pms.dbo.CDCode as b WITH (NOLOCK)
where not exists(select id from Production.dbo.CDCode as a WITH (NOLOCK) where a.id = b.id)


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
      a.NameCH		      =isnull(b.NameCH, '')
      ,a.NameEN		      =isnull(b.NameEN, '')
      ,a.Alias		      =isnull(b.Alias, '')
      ,a.Junk		      =isnull(b.Junk, 0)
      ,a.MtlFormA		      =isnull(b.MtlFormA, 0)
      ,a.Continent		      =isnull(b.Continent, '')
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
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
       isnull(ID, '')
      ,isnull(NameCH, '')
      ,isnull(NameEN, '')
      ,isnull(Alias, '')
      ,isnull(Junk, 0)
      ,isnull(MtlFormA, 0)
      ,isnull(Continent, '')
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
from Trade_To_Pms.dbo.Country as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Country as a WITH (NOLOCK) where a.id = b.id)

--WeaveType
merge Production.dbo.WeaveType t 
using Trade_To_Pms.dbo.WeaveType s
on t.id = s.id
when matched then update set
	 t.isFabricLoss=isnull(s.isFabricLoss, 0)
	,t.Junk		   =isnull(s.Junk, 0)
	,t.AddName	   =isnull(s.AddName, '')
	,t.AddDate	   =s.AddDate
	,t.EditName	   =isnull(s.EditName, '')
	,t.EditDate	   =s.EditDate
when not matched by target then
	insert(id,isFabricLoss,Junk,AddName,AddDate,EditName,EditDate)
	values(s.id,s.isFabricLoss,s.Junk,s.AddName,s.AddDate,s.EditName,s.EditDate)
when not matched by source then
	delete
;
--Mtltype Mtltype  
--AMtltype
----------------------刪除主TABLE多的資料
Delete Production.dbo.MtlType
from Production.dbo.MtlType as a left join Trade_To_Pms.dbo.MtlType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
--------TRADE的EditDate > PMS EditDate 就UPDATE All 如果false 就不UPDATE EditDate & EditName 其他一樣UPDATE

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='imp_MtlType';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

UPDATE a
SET  
      a.FullName	      =isnull(b.FullName, '')		
      ,a.Type	      =isnull(b.Type, '')		
      ,a.Junk	      =isnull(b.Junk, 0)		
      ,a.IrregularCost	      =isnull(b.IrregularCost, 0)		
      ,a.CheckZipper	      =isnull(b.CheckZipper, 0)		
      ,a.ProductionType	      =isnull(b.ProductionType, '')		
      ,a.OutputUnit	      =isnull(b.OutputUnit, '')		
      ,a.IsExtensionUnit	      =isnull(b.IsExtensionUnit, 0)		
      ,a.AddName	      =isnull(b.AddName	, '')	
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =isnull(b.EditName, '')		
      ,a.EditDate	      =b.EditDate
	  ,a.IsTrimCardOther = isnull(b.isTrimCardOther, 0)	
	  ,a.IsThread        = isnull(b.IsThread, 0)
	  ,a.LossQtyCalculateType        = isnull(b.LossQtyCalculateType, '')
	  ,a.AllowTransPoForGarmentSP = isnull(b.AllowTransPoForGarmentSP, 0)
from Production.dbo.MtlType as a inner join Trade_To_Pms.dbo.MtlType as b ON a.id=b.id

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
	  ,isTrimCardOther
	  ,IsThread
	  ,LossQtyCalculateType
      ,AllowTransPoForGarmentSP
)
select 
       isnull(ID				  , '')
      ,isnull(FullName			  , '')
      ,isnull(Type				  , '')
      ,isnull(Junk				  , 0)
      ,isnull(IrregularCost		  , 0)
      ,isnull(CheckZipper		  , 0)
      ,isnull(ProductionType	  , '')
      ,isnull(OutputUnit		  , '')
      ,isnull(IsExtensionUnit	  , 0)
      ,isnull(AddName			  , '')
      ,AddDate			 
      ,isnull(EditName			  , '')
      ,EditDate			  
	  ,isnull(isTrimCardOther	  , 0)
	  ,isnull(IsThread			  , 0)
	  ,isnull(LossQtyCalculateType, '')
      ,isnull(b.AllowTransPoForGarmentSP, 0)
from Trade_To_Pms.dbo.MtlType as b WITH (NOLOCK)
where not exists(select id from Production.dbo.MtlType as a WITH (NOLOCK) where a.id = b.id)

--MtlType_Limit

delete MtlType_Limit
INSERT INTO MtlType_Limit
           ([ID]
           ,[PoUnit]
           ,[LimitDown]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
     isnull([ID], '')
    ,isnull([PoUnit], '')
    ,isnull([LimitDown], 0)
    ,isnull([AddName], '')
    ,[AddDate]
    ,isnull([EditName], '')
    ,[EditDate]
from Trade_To_Pms.dbo.MtlType_Limit

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
       --a.ID	      =b.ID		
      a.Abbreviation	      = isnull(b.Abbreviation		, '')
      ,a.Classify	      = isnull(b.Classify			   , '')
      ,a.Seq	      = isnull(b.Seq					   , '')
      ,a.Junk	      = isnull(b.Junk					   , 0)
      ,a.ArtworkUnit	      = isnull(b.ArtworkUnit		, '')
      ,a.ProductionUnit	      = isnull(b.ProductionUnit		, '')
      ,a.IsTMS	      = isnull(b.IsTMS					   , 0)
      ,a.IsPrice	      = isnull(b.IsPrice			   , 0)
      ,a.IsArtwork	      = isnull(b.IsArtwork			   , 0)
      ,a.IsTtlTMS	      = isnull(b.IsTtlTMS			   , 0)
      ,a.Remark	      = isnull(b.Remark					   , '')
      ,a.ReportDropdown	      = isnull(b.ReportDropdown		, 0)
      ,a.UseArtwork	      = isnull(b.UseArtwork			   , 0)
      ,a.SystemType	      = isnull(b.SystemType			   , '')
      ,a.AddName	      = isnull(b.AddName			   , '')
      ,a.AddDate	      = b.AddDate			  
      ,a.EditName	      = isnull(b.EditName			   , '')
      ,a.EditDate	      = b.EditDate			  
	  ,a.IsPrintToCMP	  = isnull(b.IsPrintToCMP		   , 0)
	  ,a.IsLocalPurchase = isnull( b.IsLocalPurchase	   , 0)
	  ,a.IsImportTestDox = isnull(b.IsImportTestDox,0)
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
	  ,IsPrintToCMP
	  ,IsLocalPurchase
	  ,IsImportTestDox
)
select 
       isnull(ID			 , '')
      ,isnull(Abbreviation	 , '')
      ,isnull(Classify		 , '')
      ,isnull(Seq			 , '')
      ,isnull(Junk			 , 0)
      ,isnull(ArtworkUnit	 , '')
      ,isnull(ProductionUnit , '')
      ,isnull(IsTMS			 , 0)
      ,isnull(IsPrice		 , 0)
      ,isnull(IsArtwork		 , 0)
      ,isnull(IsTtlTMS		 , 0)
      ,isnull(Remark		 , '')
      ,isnull(ReportDropdown , 0)
      ,isnull(UseArtwork	 , 0)
      ,isnull(SystemType	 , '')
	  ,'O'					
      ,isnull(AddName		 , '')
      ,AddDate				 
      ,isnull(EditName		 , '')
      ,EditDate				
	  ,isnull(IsPrintToCMP	 , 0)
	  ,isnull(IsLocalPurchase, 0)
	  ,isnull(IsImportTestDox,0)
from Trade_To_Pms.dbo.ArtworkType as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ArtworkType as a WITH (NOLOCK) where a.id = b.id)
-------------------------------Artworktype_Detail
merge Production.dbo.Artworktype_Detail as t
Using Trade_TO_Pms.dbo.Artworktype_Detail as s
on t.ArtworktypeID = s.ArtworktypeID and t.MachineTypeID = s.MachineTypeID
when not matched by target then
	insert(ArtworktypeID,MachineTypeID)
	values(isnull(ArtworktypeID, ''),isnull(MachineTypeID, ''))
when not matched by source then
	delete;

--Artworktype1 MachineType 無多的欄位
--AArtworkType1
----------------------MachineType--
--Last Ver:Mantis_5495刪除原本delete/updata/insert改使用merge作法
merge Production.dbo.MachineType as t
Using Trade_TO_Pms.dbo.MachineType as s
on t.id = s.id
when matched then
		update set 
		t.Description		=isnull(s.Description	, '')
		,t.DescCH		    =isnull(s.DescCH		, '')
		,t.ISO				=isnull(s.ISO		    , '')
		,t.ArtworkTypeID	=isnull(s.ArtworkTypeID	, '')
		,t.Mold				=isnull(s.Mold		    , 0)
		,t.RPM				=isnull(s.RPM		    , 0)
		,t.Stitches			=isnull(s.Stitches		, 0)
		,t.Picture1			=isnull(s.Picture1		, '')
		,t.Picture2			=isnull(s.Picture2		, '')
		,t.MachineAllow		=isnull(s.MachineAllow	, 0)
		,t.ManAllow			=isnull(s.ManAllow		, 0)
		,t.MachineGroupID	=isnull(s.MachineGroupID, '')
		,t.Junk				=isnull(s.Junk		    , 0)
		,t.AddName			=isnull(s.AddName		, '')
		,t.AddDate			=s.AddDate		
		,t.EditName			=isnull(s.EditName		, '')
		,t.EditDate			=s.EditDate		
		,t.isThread         =isnull(s.isThread		, 0)
		,t.MasterGroupID	=isnull(s.MasterGroupID	, '')
		,t.Hem				=isnull(s.Hem			, 0)
when not matched by target then
	insert(
		ID
		,Description
		,DescCH
		,ISO
		,ArtworkTypeID
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
		,isThread
		,MasterGroupID
		,Hem
	)
	values(
		 isnull(ID				, '')
		,isnull(Description		, '')
		,isnull(DescCH			, '')
		,isnull(ISO				, '')
		,isnull(ArtworkTypeID	, '')
		,isnull(Mold			, 0)
		,isnull(RPM				, 0)
		,isnull(Stitches		, 0)
		,isnull(Picture1		, '')
		,isnull(Picture2		, '')
		,isnull(MachineAllow	, 0)
		,isnull(ManAllow		, 0)
		,isnull(MachineGroupID	, '')
		,isnull(Junk			, 0)
		,isnull(AddName			, '')
		,AddDate			
		,isnull(EditName		, '')
		,EditDate	
		,isnull(isThread		, 0)
		,isnull(MasterGroupID	, '')
		,isnull(Hem				, 0)
	)
when not matched by source then
	delete;

------ Merge MachineType_Detail

merge Production.dbo.MachineType_Detail as t
Using Trade_TO_Pms.dbo.MachineType_Detail as s
on t.id = s.id and t.FactoryID = s.FactoryID
when matched then
		update set 
		t.IsSubprocess		=isnull(s.IsSubprocess	 , 0)
		,t.IsNonSewingLine	=isnull(s.IsNonSewingLine, 0)	    
		,t.IsNotShownInP01	=isnull(s.IsNotShownInP01, 0)	    
		,t.IsNotShownInP03	=isnull(s.IsNotShownInP03, 0)
		,t.IsNotShownInP05	=isnull(s.IsNotShownInP05, 0)
		,t.IsNotShownInP06	=isnull(s.IsNotShownInP06, 0)
when not matched by target then
	insert(
		[ID]
		,[FactoryID]
		,[IsSubprocess]
		,[IsNonSewingLine]
		,[IsNotShownInP01]
		,[IsNotShownInP03]
		,[IsNotShownInP05]
		,[IsNotShownInP06]
	)
	values(
	   isnull(s.[ID], '')
      ,isnull(s.[FactoryID], '')
      ,isnull(s.[IsSubprocess]	 , 0)
      ,isnull(s.[IsNonSewingLine], 0)
      ,isnull(s.[IsNotShownInP01], 0)
      ,isnull(s.[IsNotShownInP03], 0)
	  ,isnull(s.[IsNotShownInP05], 0)
	  ,isnull(s.[IsNotShownInP06], 0)
	)
when not matched by source then
	delete;

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
      -- a.BrandID		     =b.BrandID
      a.Junk		      = isnull(b.Junk							, 0)
      ,a.ID		      = isnull(b.ID									, '')
      ,a.CountryID		      = isnull(b.CountryID					, '')
      ,a.City		      = isnull(b.City							, '')
      ,a.QuotaArea		      = isnull(b.QuotaArea					, '')
      ,a.ScanAndPack		      = isnull(b.ScanAndPack			, 0)
      ,a.ZipperInsert		      = isnull(b.ZipperInsert			, '')
      ,a.SpecialCust		      = isnull(b.SpecialCust			, 0)
      ,a.VasShas		      = isnull(b.VasShas					, 0)
      ,a.Guid		      = isnull(b.Guid							, '')
      ,a.Factories		      = isnull(b.Factories					, '')
      ,a.PayTermARIDBulk		      = isnull(b.PayTermARIDBulk	, '')
      ,a.PayTermARIDSample		      = isnull(b.PayTermARIDSample	, '')
      ,a.ProformaInvoice		      = isnull(b.ProformaInvoice	, 0)
      ,a.BankIDSample		      = isnull(b.BankIDSample			, '')
      ,a.BankIDBulk		      = isnull(b.BankIDBulk					, '')
      ,a.BrandLabel		      = isnull(b.BrandLabel					, '')
      ,a.MarkFront		      = isnull(b.MarkFront					, '')
      ,a.MarkBack		      = isnull(b.MarkBack					, '')
      ,a.MarkLeft		      = isnull(b.MarkLeft					, '')
      ,a.MarkRight		      = isnull(b.MarkRight					, '')
      ,a.BillTo		      = isnull(b.BillTo							, '')
      ,a.ShipTo		      = isnull(b.ShipTo							, '')
      ,a.Consignee		      = isnull(b.Consignee					, '')
      ,a.Notify		      = isnull(b.Notify							, '')
      ,a.Anotify		      = isnull(b.Anotify					, '')
      ,a.ShipRemark		      = isnull(b.ShipRemark					, '')
      ,a.AddName		      = isnull(b.AddName					, '')
      ,a.AddDate		      = b.AddDate				
      ,a.EditName		      = isnull(b.EditName					, '')
      ,a.EditDate		      = b.EditDate				
	  ,a.Kit		          = isnull(b.Kit						, '')
	  ,a.HealthID		      = isnull(b.HealthID					, '')
	  ,a.ShipTermID		      = isnull(b.ShipTermID					, '')

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
	  ,Kit
	  ,HealthID
	  ,ShipTermID
)
select 
       isnull(BrandID			, '')
      ,isnull(Junk				, 0)
      ,isnull(ID				, '')
      ,isnull(CountryID			, '')
      ,isnull(City				, '')
      ,isnull(QuotaArea			, '')
      ,isnull(ScanAndPack		, 0)
      ,isnull(ZipperInsert		, '')
      ,isnull(SpecialCust		, 0)
      ,isnull(VasShas			, 0)
      ,isnull(Guid				, '')
      ,isnull(Factories			, '')
      ,isnull(PayTermARIDBulk	, '')
      ,isnull(PayTermARIDSample	, '')
      ,isnull(ProformaInvoice	, 0)
      ,isnull(BankIDSample		, '')
      ,isnull(BankIDBulk		, '')
      ,isnull(BrandLabel		, '')
      ,isnull(MarkFront			, '')
      ,isnull(MarkBack			, '')
      ,isnull(MarkLeft			, '')
      ,isnull(MarkRight			, '')
      ,isnull(BillTo			, '')
      ,isnull(ShipTo			, '')
      ,isnull(Consignee			, '')
      ,isnull(Notify			, '')
      ,isnull(Anotify			, '')
      ,isnull(ShipRemark		, '')
      ,isnull(AddName			, '')
      ,AddDate			
      ,isnull(EditName			, '')
      ,EditDate			
	  ,isnull(Kit				, '')
	  ,isnull(HealthID			, '')
	  ,isnull(ShipTermID			, '')
from Trade_To_Pms.dbo.CustCD as b WITH (NOLOCK)
where not exists(select id from Production.dbo.CustCD as a WITH (NOLOCK) where a.id = b.id and a.BrandID=b.BrandID)


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
       --a.ReasonTypeID	        = b.ReasonTypeID	
      --,a.ID	       = b.ID	
      a.Name	       = isnull(b.Name	, '')
      ,a.Remark	       = isnull(b.Remark	, '')
      ,a.No	       = isnull(b.No	, 0)
      ,a.ReasonGroup	       = isnull(b.ReasonGroup	, '')
      ,a.Kpi	       = isnull(b.Kpi	, 0)
      ,a.AccountID	       = isnull(b.AccountID	, '')
      ,a.FactoryKpi	       = isnull(b.FactoryKpi	, 0)
      ,a.AddName	       = isnull(b.AddName	, '')
      ,a.AddDate	       = b.AddDate	
      ,a.EditName	       = isnull(b.EditName	, '')
      ,a.EditDate	       = b.EditDate	
      ,a.Junk	       = isnull(b.Junk	, 0)

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
      ,AccountID
      ,FactoryKpi
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,Junk

)
select 
       isnull(ReasonTypeID, '')
      ,isnull(ID		  , '')
      ,isnull(Name		  , '')
      ,isnull(Remark	  , '')
      ,isnull(No		  , 0)
      ,isnull(ReasonGroup , '')
      ,isnull(Kpi		  , 0)
      ,isnull(AccountID	  , '')
      ,isnull(FactoryKpi  , 0)
      ,isnull(AddName	  , '')
      ,AddDate	 
      ,isnull(EditName	  , '')
      ,EditDate	  
      ,isnull(Junk		  , 0)
from Trade_To_Pms.dbo.Reason as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Reason as a WITH (NOLOCK) where a.id = b.id and a.ReasonTypeID = b.ReasonTypeID)


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
		--a.ID	   =b.ID	
      a.Description	  =isnull(b.Description	, '')
      ,a.AddName	      =isnull(b.AddName	, '')
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =isnull(b.EditName	, '')
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
		isnull(ID, '')
      ,isnull(Description, '')
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
from Trade_To_Pms.dbo.SHIPTerm as b WITH (NOLOCK)
where not exists(select id from Production.dbo.SHIPTerm as a WITH (NOLOCK) where a.id = b.id)

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
      a.Junk	      =isnull(b.Junk						   , 0)
	  ,a.NameCH	      =isnull(b.NameCH						   , '')
      ,a.CountryID	      =isnull(b.CountryID				   , '')
      ,a.AddressCH	      =isnull(b.AddressCH				   , '')
      ,a.CurrencyID	      =isnull(b.CurrencyID				   , '')
      ,a.CPU	      =isnull(b.CPU							   , 0)
      ,a.ZipCode	      =isnull(b.ZipCode					   , '')
      ,a.PortSea	      =isnull(b.PortSea					   , '')
      ,a.PortAir	      =isnull(b.PortAir					   , '')
      ,a.KitId	      =isnull(b.KitId						   , '')
      ,a.ExpressGroup	      =isnull(b.ExpressGroup		   , '')
      ,a.IECode	      =isnull(b.IECode						   , '')
      ,a.AddName	      =isnull(b.AddName					   , '')
      ,a.AddDate	      =b.AddDate						 
      ,a.EditName	      =isnull(b.EditName				   , '')
      ,a.EditDate	      =b.EditDate						 
      ,a.KPICode	      =isnull(b.KPICode					   , '')
      ,a.Type	      =isnull(b.Type						   , '')
      ,a.Zone	      =isnull(b.Zone						   , '')
      ,a.FactorySort	      =isnull(b.FactorySort			   , '')
	  ,a.IsSCI        =isnull(b.IsSCI						   , 0)
	  ,a.TestDocFactoryGroup = isnull(b.TestDocFactoryGroup	   , '')
	  ,a.FtyZone      =isnull(b.FtyZone						   , '')
	  ,a.Foundry	  =isnull(b.Foundry						   , 0)
	  ,a.ProduceM	  =isnull(b.MDivisionID					   , '')
	  ,a.LoadingFactoryGroup	  =isnull(b.LoadingFactoryGroup, '')
	  ,a.PadPrintGroup	=	isnull(b.PadPrintGroup,'')
	  ,a.IsSubcon = b.IsSubcon
	  ,a.IsECFA	= isnull(b.IsECFA, 0)
from Production.dbo.Factory as a inner join Trade_To_Pms.dbo.Factory as b ON a.id=b.id
--Factory1
--Factory_TMS
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
-------------------------- 根據 Factory_TMS Primary Key 更新存在 PMS 的資料
UPDATE a
SET  
      -- a.ID	      =b.ID	
      --,a.Year	      =b.Year	
      --,a.ArtworkTypeID	      =b.ArtworkTypeID	
     -- ,a.Month	      =b.Month	
      a.TMS	      =isnull(b.TMS	, 0)

from Production.dbo.Factory_Tms as a inner join Trade_To_Pms.dbo.Factory_Tms as b ON a.id=b.id and a.Year=b.Year and a.ArtworkTypeID=b.ArtworkTypeID and a.Month=b.Month
-------------------------- 根據 Factory_TMS Primary Key 新增不存在 PMS 的資料
INSERT INTO Production.dbo.Factory_Tms(
      ID
      ,Year
      ,ArtworkTypeID
      ,Month
      ,TMS

)
select 
       isnull(ID, '')
      ,isnull(Year, '')
      ,isnull(ArtworkTypeID, '')
      ,isnull(Month, '')
      ,isnull(TMS, 0)

from Trade_To_Pms.dbo.Factory_Tms as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Factory_Tms as a WITH (NOLOCK) where a.id = b.id and a.Year=b.Year and a.ArtworkTypeID=b.ArtworkTypeID and a.Month=b.Month)

-------------------------- 根據 Factory_TMS Primary Key 刪除不存在 Trade 的資料
delete pms_ft
from Production.dbo.Factory_TMS pms_ft
where not exists (
		select 1
		from Trade_To_Pms.dbo.Factory_Tms trade_ft
		where pms_ft.ID = trade_ft.ID
				and pms_ft.Year = trade_ft.Year
				and pms_ft.ArtworkTypeID = trade_ft.ArtworkTypeID
				and pms_ft.Month = trade_ft.Month
		)

--Factory4
--Factory_BrandDefinition
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.ID		     =b.ID
      --,a.BrandID		      =b.BrandID
      --,a.CDCodeID		      =b.CDCodeID
      a.BrandAreaCode		      =isnull(b.BrandAreaCode, '')
      ,a.BrandFTYCode		      =isnull(b.BrandFTYCode, '')
      ,a.BrandVendorCode		      =isnull(b.BrandVendorCode, '')
      ,a.BrandReportCode		      =isnull(b.BrandReportCode, '')
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
      ,a.EditDate		      =b.EditDate
      ,a.V_Code		          =b.V_Code

from Production.dbo.Factory_BrandDefinition as a inner join Trade_To_Pms.dbo.Factory_BrandDefinition as b ON a.id=b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID
where b.EditDate > a.EditDate

UPDATE a
SET  
      -- a.ID		     =b.ID
     -- ,a.BrandID		      =b.BrandID
      --,a.CDCodeID		      =b.CDCodeID
      a.BrandAreaCode		      =isnull(b.BrandAreaCode, '')
      ,a.BrandFTYCode		      =isnull(b.BrandFTYCode, '')
      ,a.BrandVendorCode		      =isnull(b.BrandVendorCode, '')
      ,a.BrandReportCode		      =isnull(b.BrandReportCode, '')
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.V_Code		          =b.V_Code
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
      ,V_Code
)
select 
       isnull(ID, '')
      ,isnull(BrandID, '')
      ,isnull(CDCodeID, '')
      ,isnull(BrandAreaCode, '')
      ,isnull(BrandFTYCode, '')
      ,isnull(BrandVendorCode, '')
      ,isnull(BrandReportCode, '')
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
      ,V_Code

from Trade_To_Pms.dbo.Factory_BrandDefinition as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Factory_BrandDefinition as a WITH (NOLOCK) where a.id = b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID)




--Factory5
--Factory_WorkHour
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       --a.ID	     =b.ID	
      --,a.Year	      =b.Year	
      --,a.Month	      =b.Month	
      a.HalfMonth1	      =isnull(b.HalfMonth1	, 0)
      ,a.HalfMonth2	      =isnull(b.HalfMonth2	, 0)

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
       isnull(ID		, '')
      ,isnull(Year		, '')
      ,isnull(Month		, '')
      ,isnull(HalfMonth1, 0)
      ,isnull(HalfMonth2, 0)
from Trade_To_Pms.dbo.Factory_WorkHour as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Factory_WorkHour as a WITH (NOLOCK) where a.id = b.id and a.Year=b.Year and a.Month=b.Month)


--SCI_FTY
--SCIFTY ---delete

Delete Production.dbo.SCIFty
from Production.dbo.SCIFty as a 
left join Trade_To_Pms.dbo.Factory as b on a.id = b.id and b.IsSCI=1
where b.id is null

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      a.Junk	      = isnull(b.Junk				 , 0)
      ,a.Abb	      = isnull(b.Abb				 , '')
      ,a.NameCH	      = isnull(b.NameCH				 , '')
      ,a.NameEN	      = isnull(b.NameEN				 , '')
      ,a.CountryID	      = isnull(b.CountryID		 , '')
      ,a.Tel	      = isnull(b.Tel				 , '')
      ,a.Fax	      = isnull(b.Fax				 , '')
      ,a.AddressCH	      = isnull(b.AddressCH		 , '')
      ,a.AddressEN	      = isnull(b.AddressEN		 , '')
      ,a.CurrencyID	      = isnull(b.CurrencyID		 , '')
      ,a.ExpressGroup	      = isnull(b.ExpressGroup, '')
      ,a.PortAir	      = isnull(b.PortAir		 , '')
      ,a.MDivisionID	      = isnull(b.MDivisionID , '')
      ,a.AddName	      = isnull(b.AddName		 , '')
      ,a.AddDate	      = b.AddDate		
      ,a.EditName	      = isnull(b.EditName		 , '')
      ,a.EditDate	      = b.EditDate	
      ,a.Type	      = isnull(b.Type				 , '')
      ,a.Zone	      = isnull(b.Zone				 , '')
	  ,a.FtyZone      = isnull(b.FtyZone			 , '')
	  ,a.IsSubcon     = ISNULL(b.IsSubcon, 0)
	  ,a.KPICode	  = ISNULL(b.KPICode, '')
	  ,a.ProduceRgCode= ISNULL(b.ProduceRgCode, '')
from Production.dbo.SCIFty as a inner join Trade_To_Pms.dbo.Factory as b ON a.id=b.id
where b.IsSCI=1

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
	  ,Type
	  ,Zone
	  ,FtyZone
	  ,IsSubcon
	  ,KPICode
	  ,ProduceRgCode
)
select 
       isnull(ID		   , '')
      ,isnull(Junk		   , 0)
      ,isnull(Abb		   , '')
      ,isnull(NameCH	   , '')
      ,isnull(NameEN	   , '')
      ,isnull(CountryID	   , '')
      ,isnull(Tel		   , '')
      ,isnull(Fax		   , '')
      ,isnull(AddressCH	   , '')
      ,isnull(AddressEN	   , '')
      ,isnull(CurrencyID   , '')
      ,isnull(ExpressGroup , '')
      ,isnull(PortAir	   , '')
      ,isnull(MDivisionID  , '')
      ,isnull(AddName	   , '')
      ,AddDate	  
      ,isnull(EditName	   , '')
      ,EditDate	   
	  ,isnull(Type		   , '')
	  ,isnull(Zone		   , '')
	  ,isnull(FtyZone      , '')
	  ,ISNULL(IsSubcon, 0)
	  ,ISNULL(KPICode, '')
	  ,ISNULL(ProduceRgCode, '')
from Trade_To_Pms.dbo.Factory as b WITH (NOLOCK)
where not exists(select id from Production.dbo.SCIFty as a WITH (NOLOCK) where a.id = b.id)
and b.IsSCI=1

--Unit   Unit 
----------------------刪除主TABLE多的資料
Delete Production.dbo.Unit
from Production.dbo.Unit as a left join Trade_To_Pms.dbo.Unit as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
     --  a.ID	      =b.ID	
      a.PriceRate	      =isnull(b.PriceRate	, 0)
      ,a.Round	      =isnull(b.Round	, 0)
      ,a.Description	      =isnull(b.Description	, '')
      ,a.ExtensionUnit	      =isnull(b.ExtensionUnit,'')	
      ,a.Junk	      =isnull(b.TradeJunk	, 0)
      ,a.AddName	      =isnull(b.AddName	, '')
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =isnull(b.EditName	, '')
      ,a.EditDate	      =b.EditDate	
	  ,a.MiAdidasRound    =isnull(b.MiAdidasRound, 0)
	  ,a.RoundStep        =isnull(b.RoundStep, 0)
	  ,a.StockRound		  =isnull(b.StockRound, 0)

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
	  ,MiAdidasRound
	  ,RoundStep
	  ,StockRound
)
select 
       isnull(ID		 , '')
      ,isnull(PriceRate	 , 0)
      ,isnull(Round		 , 0)
      ,isnull(Description, '')
      ,isnull(ExtensionUnit,'')
      ,isnull(TradeJunk, 0)
      ,isnull(AddName	   , '')
      ,AddDate	   
      ,isnull(EditName	   , '')
      ,EditDate	 
	  ,isnull(MiAdidasRound, 0)
	  ,isnull(RoundStep	   , 0)
	  ,isnull(StockRound   , 0)
from Trade_To_Pms.dbo.Unit as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Unit as a WITH (NOLOCK) where a.id = b.id)


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
      -- a.UnitFrom	    =b.UnitFrom	
     -- ,a.UnitTo	      =b.UnitTo	
      a.Rate	      =isnull(b.Rate	, '')
      ,a.RateValue	      =isnull(b.RateValue	, 0)
      ,a.AddName	      =isnull(b.AddName	, '')
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =isnull(b.EditName	, '')
      ,a.EditDate	      =b.EditDate	
	  ,a.Numerator        =b.Numerator
	  ,a.Denominator      =b.Denominator
from Production.dbo.Unit_Rate as a 
inner join Trade_To_Pms.dbo.Unit_Rate as b ON a.UnitFrom = b.UnitFrom and a.UnitTo = b.UnitTo
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
	  ,Numerator
	  ,Denominator
)
select 
       isnull(UnitFrom	, '')
      ,isnull(UnitTo	, '')
      ,isnull(Rate		, '')
      ,isnull(RateValue	, 0)
      ,isnull(AddName	, '')
      ,AddDate	
      ,isnull(EditName	, '')
      ,EditDate 
	  ,isnull(Numerator, 0)
	  ,isnull(Denominator, 0)
from Trade_To_Pms.dbo.Unit_Rate as b WITH (NOLOCK)
where not exists(select UnitFrom from Production.dbo.Unit_Rate as a WITH (NOLOCK) where a.UnitFrom = b.UnitFrom  and a.UnitTo = b.UnitTo)


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
 --a.ID	     =b.ID	
      a.Name	      =isnull(b.Name	, '')
      ,a.ExtNo	      =isnull(b.ExtNo	, '')
      ,a.EMail	      =isnull(b.EMail	, '')
from Production.dbo.TPEPass1 as a inner join Trade_To_Pms.dbo.Pass1 as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.TPEPass1(
       ID
      ,Name
      ,ExtNo
      ,EMail
)
select 
       isnull(ID, '')
      ,isnull(Name, '')
      ,isnull(ExtNo, '')
      ,isnull(EMail, '')
from Trade_To_Pms.dbo.Pass1 as b WITH (NOLOCK)
where not exists(select id from Production.dbo.TPEPass1 as a WITH (NOLOCK) where a.id = b.id)

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
 --a.BrandId		 =b.BrandId
     -- ,a.ID		      =b.ID
      a.Name		      =isnull(b.Name, '')
      ,a.Varicolored		      =isnull(b.Varicolored, 0)
      ,a.JUNK		      =isnull(b.JUNK, 0)
      ,a.VIVID		      =isnull(b.VIVID, 0)
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
      ,a.EditDate		      =b.EditDate
	  ,a.ukey =b.ukey
	  ,a.Picture = isnull(b.Picture,'')

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
	  ,ukey
	  ,Picture
)
select 
       isnull(BrandId		  , '')
      ,isnull(ID			  , '')
      ,isnull(Name			  , '')
      ,isnull(Varicolored	  , 0)
      ,isnull(JUNK			  , 0)
      ,isnull(VIVID			  , 0)
      ,isnull(AddName		  , '')
      ,AddDate		 
      ,isnull(EditName		  , '')
      ,EditDate		 
	  ,isnull(ukey            , 0)
	  ,isnull(Picture,'')
from Trade_To_Pms.dbo.Color as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Color as a WITH (NOLOCK) where a.id = b.id and a.BrandId = b.BrandId)


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
      -- a.ID		      =b.ID
      a.StdRate		      =isnull(b.StdRate	 , 0)
      ,a.NameCH		      =isnull(b.NameCH	 , '')
      ,a.NameEN		      =isnull(b.NameEN	 , '')
      ,a.Junk		      =isnull(b.Junk	 , 0)
      ,a.Exact		      =isnull(b.Exact	 , 0)
      ,a.Symbol		      =isnull(b.Symbol   , '')
      ,a.AddName		      =isnull(b.AddName, '')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName, '')
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
       isnull(ID	  , '')
      ,isnull(StdRate , 0)
      ,isnull(NameCH  , '')
      ,isnull(NameEN  , '')
      ,isnull(Junk	  , 0)
      ,isnull(Exact	  , 0)
      ,isnull(Symbol  , '')
      ,isnull(AddName , '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
from Trade_To_Pms.dbo.Currency as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Currency as a WITH (NOLOCK) where a.id = b.id)


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
       --a.WeaveTypeID	     =b.WeaveTypeID		
      a.LossType	      =isnull(b.LossType		     , 0)
      ,a.Limit	      =isnull(b.Limit					 , 0)
      ,a.LimitDown	      =isnull(b.LimitDown			 , 0)
      ,a.TWLimitDown	      =isnull(b.TWLimitDown		 , 0)
      ,a.NonTWLimitDown	      =isnull(b.NonTWLimitDown	, 0)
      ,a.LimitUp	      =isnull(b.LimitUp				 , 0)
      ,a.TWLimitUp	      =isnull(b.TWLimitUp			 , 0)
      ,a.NonTWLimitUP	      =isnull(b.NonTWLimitUP	, 0)
      ,a.Allowance	     =isnull(b.Allowance			 , 0)
      ,a.AddName	     =isnull(b.AddName				 , '')
      ,a.AddDate	     =b.AddDate				
      ,a.EditName	     =isnull(b.EditName				 , '')
      ,a.EditDate	     =b.EditDate				
	  ,a.MaxLossQty		 =isnull(b.MaxLossQty			 , 0)
	  ,a.MinGmtQty		 =isnull(b.MinGmtQty			 , 0)
	  ,a.MinLossQty		 =isnull(b.MinLossQty			 , 0)
	  ,a.PerGmtQty		 =isnull(b.PerGmtQty			 , 0)
	  ,a.PlsLossQty		 =isnull(b.PlsLossQty			 , 0)
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
	  ,MaxLossQty
	  ,MinGmtQty
	  ,MinLossQty
	  ,PerGmtQty
	  ,PlsLossQty
)
select 
       isnull(WeaveTypeID     , '')
      ,isnull(LossType		  , 0)
      ,isnull(Limit			  , 0)
      ,isnull(LimitDown		  , 0)
      ,isnull(TWLimitDown	  , 0)
      ,isnull(NonTWLimitDown  , 0)
      ,isnull(LimitUp		  , 0)
      ,isnull(TWLimitUp		  , 0)
      ,isnull(NonTWLimitUP	  , 0)
      ,isnull(Allowance		  , 0)
      ,isnull(AddName		  , '')
      ,AddDate	
      ,isnull(EditName		  , '')
      ,EditDate		 
	  ,isnull(MaxLossQty	  , 0)
	  ,isnull(MinGmtQty		  , 0)
	  ,isnull(MinLossQty	  , 0)
	  ,isnull(PerGmtQty		  , 0)
	  ,isnull(PlsLossQty	  , 0)
from Trade_To_Pms.dbo.LossRateFabric as b WITH (NOLOCK)
where not exists(select WeaveTypeID from Production.dbo.LossRateFabric as a WITH (NOLOCK) where a.WeaveTypeID = b.WeaveTypeID)
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
	  ,IgnoreLimitUpBrand
)
select 
       isnull(MtltypeId			, '')
      ,isnull(LossUnit			, 0)
      ,isnull(LossTW			, 0)
      ,isnull(LossNonTW			, 0)
      ,isnull(PerQtyTW			, 0)
      ,isnull(PlsQtyTW			, 0)
      ,isnull(PerQtyNonTW		, 0)
      ,isnull(PlsQtyNonTW		, 0)
      ,isnull(FOCTW				, 0)
      ,isnull(FOCNonTW			, 0)
      ,isnull(AddName			, '')
      ,AddDate		
      ,isnull(EditName			, '')
      ,EditDate			
	  ,isnull(IgnoreLimitUpBrand, '')
from Trade_To_Pms.dbo.LossRateAccessory as b WITH (NOLOCK)
where not exists(select MtltypeId from Production.dbo.LossRateAccessory as a WITH (NOLOCK) where a.MtltypeId = b.MtltypeId)

----------------------刪除主TABLE多的資料
Delete Production.dbo.LossRateAccessory_Limit
from Production.dbo.LossRateAccessory_Limit as a left join Trade_To_Pms.dbo.LossRateAccessory_Limit as b
on a.MtltypeId = b.MtltypeId and a.UsageUnit = b.UsageUnit
where b.MtltypeId is null

UPDATE a SET 
       LimitUp  = b.LimitUp 
      ,AddName  = b.AddName 
      ,AddDate  = b.AddDate 
      ,EditName = b.EditName
      ,EditDate = b.EditDate
from Production.dbo.LossRateAccessory_Limit as a 
inner join Trade_To_Pms.dbo.LossRateAccessory_Limit as b on a.MtltypeId = b.MtltypeId and a.UsageUnit = b.UsageUnit

-------------------------- INSERT INTO 不INSERT waste的欄位
INSERT INTO Production.dbo.LossRateAccessory_Limit(
	MtltypeId
      ,UsageUnit
	  ,LimitUp
	  ,AddName
	  ,AddDate
	  ,EditName
	  ,EditDate
)
select 
	   isnull(MtltypeId, '')
      ,isnull(UsageUnit, '')
	  ,isnull(LimitUp  , 0)
	  ,isnull(AddName  , '')
	  ,AddDate
	  ,isnull(EditName, '')
	  ,EditDate
from Trade_To_Pms.dbo.LossRateAccessory_Limit as b WITH (NOLOCK)
where not exists(select MtltypeId from Production.dbo.LossRateAccessory_Limit as a WITH (NOLOCK)
				 where a.MtltypeId = b.MtltypeId and a.UsageUnit = b.UsageUnit)


-------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a--特別處理waste的欄位
SET  
    --  a.MtltypeId	     =b.MtltypeId	
      a.LossUnit	      = isnull(b.LossUnit					, 0)
      ,a.LossTW	      = isnull(b.LossTW							, 0)
      ,a.LossNonTW	      = isnull(b.LossNonTW					, 0)
      ,a.PerQtyTW	      = isnull(b.PerQtyTW					, 0)
      ,a.PlsQtyTW	      = isnull(b.PlsQtyTW					, 0)
      ,a.PerQtyNonTW	      = isnull(b.PerQtyNonTW			, 0)
      ,a.PlsQtyNonTW	      = isnull(b.PlsQtyNonTW			, 0)
      ,a.FOCTW	      = isnull(b.FOCTW							, 0)
      ,a.FOCNonTW	      = isnull(b.FOCNonTW					, 0)
      ,a.AddName	      = isnull(b.AddName					, '')
      ,a.AddDate	      = b.AddDate					
      ,a.EditName	      = isnull(b.EditName					, '')
      ,a.EditDate	      = b.EditDate				
      ,a.Waste	      = isnull( (b.LossTW + b.LossNonTW)/2		, 0)
      ,a.IgnoreLimitUpBrand	      = isnull(b.IgnoreLimitUpBrand	, '')
	  

from Production.dbo.LossRateAccessory as a inner join Trade_To_Pms.dbo.LossRateAccessory as b ON a.MtltypeId=b.MtltypeId
where a.LossUnit=1

UPDATE a--特別處理waste的欄位
SET  
      --a.MtltypeId	     = isnull(b.MtltypeId	
      a.LossUnit	      = isnull(b.LossUnit		 , 0)
      ,a.LossTW	      = isnull(b.LossTW				 , 0)
      ,a.LossNonTW	      = isnull(b.LossNonTW		 , 0)
      ,a.PerQtyTW	      = isnull(b.PerQtyTW		 , 0)
      ,a.PlsQtyTW	      = isnull(b.PlsQtyTW		 , 0)
      ,a.PerQtyNonTW	      = isnull(b.PerQtyNonTW, 0)
      ,a.PlsQtyNonTW	      = isnull(b.PlsQtyNonTW, 0)
      ,a.FOCTW	      = isnull(b.FOCTW				 , 0)
      ,a.FOCNonTW	      = isnull(b.FOCNonTW	     , 0)
      ,a.AddName	      = isnull(b.AddName		, '')
      ,a.AddDate	      = b.AddDate	
      ,a.EditName	      = isnull(b.EditName		, '')
      ,a.EditDate	      = b.EditDate	
      ,a.Waste	      = isnull(IIF((b.PerQtyTW + b.PerQtyNonTW)= 0,'0',(b.PlsQtyTW + b.PlsQtyNonTW)/(b.PerQtyTW + b.PerQtyNonTW) * 100) , 0)
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
       --a.ID		 =b.ID
      a.SuppID		      = isnull(b.SuppID, '')
      ,a.Account		      = isnull(b.Account, '')
      ,a.Junk		      = isnull(b.Junk, 0)
      ,a.AddName		      = isnull(b.AddName, '')
      ,a.AddDate		      = b.AddDate
      ,a.EditName		      = isnull(b.EditName, '')
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
       isnull(ID, '')
      ,isnull(SuppID, '')
      ,isnull(Account, '')
      ,isnull(Junk, 0)
      ,isnull(AddName, '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
from Trade_To_Pms.dbo.Carrier as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Carrier as a WITH (NOLOCK) where a.id = b.id)




--PayTerm
--PayTermAP    
--PayTermAR
----------------------刪除主TABLE多的資料
Delete Production.dbo.PayTermAP
from Production.dbo.PayTermAP as a left join Trade_To_Pms.dbo.PayTermAP as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.ID			      =b.ID
      a.Description			      = isnull(b.Description             , '')
      ,a.Term			      = isnull(b.Term						 , '')
      ,a.BeforeAfter			      = isnull(b.BeforeAfter		 , '')
      ,a.BaseDate			      = isnull(b.BaseDate				 , '')
      ,a.AccountDay			      = isnull(b.AccountDay				 , 0)
      ,a.CloseAccountDay			      = isnull(b.CloseAccountDay , '')
      ,a.CloseMonth			      = isnull(b.CloseMonth				 , 0)
      ,a.CloseDay			      = isnull(b.CloseDay				 , 0)
      ,a.DueAccountday			      = isnull(b.DueAccountday		 , '')
      ,a.DueMonth			      = isnull(b.DueMonth				 , 0)
      ,a.DueDay			      = isnull(b.DueDay						 , 0)
      ,a.JUNK			      = isnull(b.JUNK						 , 0)
      ,a.AddName			      = isnull(b.AddName, '')
      ,a.AddDate			      = b.AddDate
      ,a.EditName			      = isnull(b.EditName, '')
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
	   isnull(ID              , '')
      ,isnull(Description	  , '')
      ,isnull(Term			  , '')
      ,isnull(BeforeAfter	  , '')
      ,isnull(BaseDate		  , '')
      ,isnull(AccountDay	  , 0)
      ,isnull(CloseAccountDay , '')
      ,isnull(CloseMonth	  , 0)
      ,isnull(CloseDay		  , 0)
      ,isnull(DueAccountday	  , '')
      ,isnull(DueMonth		  , 0)
      ,isnull(DueDay		  , 0)
      ,isnull(JUNK			  , 0)
      ,isnull(AddName		  , '')
      ,AddDate
      ,isnull(EditName, '')
      ,EditDate
from Trade_To_Pms.dbo.PayTermAP as b WITH (NOLOCK)
where not exists(select id from Production.dbo.PayTermAP as a WITH (NOLOCK) where a.id = b.id)


-----------------PayTermAR 20161209 add
Merge Production.dbo.PayTermAR as t
Using Trade_TO_Pms.dbo.PayTermAR as s
	on t.id=s.id
	when matched then
		update set
		t.Description= isnull( s.Description,            ''),
		t.Term= isnull( s.Term,							 ''),
		t.BeforeAfter= isnull( s.BeforeAfter,			 ''),
		t.BaseDate= isnull( s.BaseDate,					 ''),
		t.AccountDay= isnull( s.AccountDay,				 0),
		t.CloseAccountDay= isnull( s.CloseAccountDay,	 ''),
		t.CloseMonth= isnull( s.CloseMonth,				 0),
		t.CloseDay= isnull( s.CloseDay,					 0),
		t.DueAccountday= isnull( s.DueAccountday,		 ''),
		t.DueMonth= isnull( s.DueMonth,					 0),
		t.DueDay= isnull( s.DueDay,						 0),
		t.JUNK= isnull( s.JUNK,							 0),
		t.SamplePI= isnull( s.SamplePI,					 0),
		t.BulkPI= isnull( s.BulkPI,						 0),
		t.AddName= isnull( s.AddName,					 ''),
		t.AddDate= s.AddDate,							 
		t.EditName= isnull( s.EditName,					 ''),
		t.EditDate= s.EditDate
	when not matched by target then
	insert(ID
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
,SamplePI
,BulkPI
,AddName
,AddDate
,EditName
,EditDate
)
	values(
isnull(s.ID,			  ''),
isnull(s.Description,	  ''),
isnull(s.Term,			  ''),
isnull(s.BeforeAfter,	  ''),
isnull(s.BaseDate,		  ''),
isnull(s.AccountDay,	  0),
isnull(s.CloseAccountDay, ''),
isnull(s.CloseMonth,	  0),
isnull(s.CloseDay,		  0),
isnull(s.DueAccountday,	  ''),
isnull(s.DueMonth,		  0),
isnull(s.DueDay,		  0),
isnull(s.JUNK,			  0),
isnull(s.SamplePI,		  0),
isnull(s.BulkPI,		  0),
isnull(s.AddName,		  ''),
s.AddDate,				 
isnull(s.EditName,        ''),
s.EditDate
)
	 when not matched by source then
	 delete;


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
      -- a.ID	 =b.ID	
      a.Description	      =isnull(b.Description	,'')
      ,a.SMV	      =isnull(b.SMV	,0)
      ,a.CPU	      =isnull(b.CPU	,0)
      ,a.AddName	      =isnull(b.AddName	,'')
      ,a.AddDate	      =b.AddDate	 
      ,a.EditName	      =isnull(b.EditName	,'')
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
       isnull(ID	,'')
      ,isnull(Description	,'')
      ,isnull(SMV	,0)
      ,isnull(CPU	,0)
      ,isnull(AddName	,'')
      ,AddDate
      ,isnull(EditName	,'')
      ,EditDate
from Trade_To_Pms.dbo.Mockup as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Mockup as a WITH (NOLOCK) where a.id = b.id)


--OrderType  不用寫DELETE
--OrderType  
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       -- a.ID	 =b.ID	
      --,a.BrandID	      =b.BrandID	
      a.PhaseID	      = isnull(b.PhaseID						,'')
      ,a.ProjectID	      = isnull(b.ProjectID					,'')
      ,a.KPIProjectID	      = isnull(b.KPIProjectID			,'')
      ,a.Junk	      = isnull(b.Junk							,0)
      ,a.CpuRate	      = isnull(b.CpuRate					,0)
      ,a.Category	      = isnull(b.Category					,'')
      ,a.PriceDays	      = isnull(b.PriceDays					,0)
      ,a.MtlLetaDays	      = isnull(b.MtlLetaDays			,0)
      ,a.EachConsDays	      = isnull(b.EachConsDays			,0)
      ,a.KPI	      = isnull(b.KPI							,0)
      ,a.Remark	      = isnull(b.Remark							,'')
      ,a.AddName	      = isnull(b.AddName					,'')
      ,a.AddDate	      = b.AddDate				
      ,a.EditName	      = isnull(b.EditName					,'')
      ,a.EditDate	      = b.EditDate				
	  ,a.IsGMTMaster	  = isnull(b.IsGMTMaster				,0)
	  ,a.IsGMTDetail      = isnull(b.IsGMTDetail				,0)
	  ,a.IsDevSample      = isnull(b.IsDevSample				,0)
	  ,a.CalByBOFConsumption      = isnull(b.CalByBOFConsumption,0)
from Production.dbo.OrderType as a inner join Trade_To_Pms.dbo.OrderType as b ON a.id=b.id and a.BrandID =b.BrandID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.OrderType(
		ID
      ,BrandID
      ,PhaseID
      ,ProjectID
      ,KPIProjectID
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
	  ,IsGMTMaster
	  ,IsGMTDetail
	  ,IsDevSample
	  ,CalByBOFConsumption
)
select 
	   isnull(ID				 ,'')
      ,isnull(BrandID			 ,'')
      ,isnull(PhaseID			 ,'')
      ,isnull(ProjectID			 ,'')
      ,isnull(KPIProjectID		 ,'')
      ,isnull(Junk				 ,0)
      ,isnull(CpuRate			 ,0)
      ,isnull(Category			 ,'')
      ,isnull(PriceDays			 ,0)
      ,isnull(MtlLetaDays		 ,0)
      ,isnull(EachConsDays		 ,0)
      ,isnull(KPI				 ,0)
      ,isnull(Remark			 ,'')
      ,isnull(AddName			 ,0)
      ,AddDate		
      ,isnull(EditName			 ,'')
      ,EditDate			
	  ,isnull(IsGMTMaster		 ,0)
	  ,isnull(IsGMTDetail		 ,0)
	  ,isnull(IsDevSample		 ,0)
	  ,isnull(CalByBOFConsumption,0)
from Trade_To_Pms.dbo.OrderType as b WITH (NOLOCK)
where not exists(select id from Production.dbo.OrderType as a WITH (NOLOCK) where a.id = b.id and a.BrandID =b.BrandID)


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
     -- a.ID	      =b.ID	
     -- ,a.BrandID	      =b.BrandID	
      a.RateCost	      =isnull(b.RateCost	,0)
      ,a.AddName	      =isnull(b.AddName	,'')
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =isnull(b.EditName	,'')
      ,a.EditDate	      =b.EditDate	
	  ,a.MiAdidas         =isnull(b.MiAdidas,0)

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
	  ,MiAdidas

)
select 
       isnull(ID	  ,'')
      ,isnull(BrandID ,'')
      ,isnull(RateCost,0)
      ,isnull(AddName ,'')
      ,AddDate
      ,isnull(EditName,'')
      ,EditDate
	  ,isnull(MiAdidas,0)
from Trade_To_Pms.dbo.Program as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Program as a WITH (NOLOCK) where a.id = b.id and a.BrandID = b.BrandID)


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
     --  a.Type	     =b.Type	
      --,a.ID	      = isnull(b.ID	
      a.Pattern	      = isnull(b.Pattern	         ,'')
      ,a.PatternCode	      = isnull(b.PatternCode,'')
      ,a.Drape	      = isnull(b.Drape				 ,'')
      ,a.DrapeCode	      = isnull(b.DrapeCode		 ,'')
      ,a.Color	      = isnull(b.Color				 ,'')
      ,a.ColorCode	      = isnull(b.ColorCode		 ,'')
      ,a.Rate	      = isnull(b.Rate				 ,0)
      ,a.AddName	      = isnull(b.AddName		 ,'')
      ,a.AddDate	      = b.AddDate		
      ,a.EditName	      = isnull(b.EditName		 ,'')
      ,a.EditDate	      = b.EditDate		

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
       isnull(Type       ,'')
      ,isnull(ID		 ,'')
      ,isnull(Pattern	 ,'')
      ,isnull(PatternCode,'')
      ,isnull(Drape		 ,'')
      ,isnull(DrapeCode	 ,'')
      ,isnull(Color		 ,'')
      ,isnull(ColorCode	 ,'')
      ,isnull(Rate		 ,0)
      ,isnull(AddName	 ,'')
      ,AddDate	 
      ,isnull(EditName	 ,'')
      ,EditDate
from Trade_To_Pms.dbo.MtlFactor as b WITH (NOLOCK)
where not exists(select id from Production.dbo.MtlFactor as a WITH (NOLOCK) where a.id = b.id and a.Type = b.Type)


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
       --a.Id		     =b.Id	
      a.Name		      = isnull(b.Name	               ,'')
      ,a.CuttingLayer		      = isnull(b.CuttingLayer	,0)
	  ,a.ManualCutLayer   = iif(a.ManualCutLayer = 0, isnull(b.CuttingLayer, 0), a.ManualCutLayer)
	  ,a.AutoCutLayer     = iif(a.AutoCutLayer = 0, isnull(b.CuttingLayer, 0), a.AutoCutLayer)
      ,a.Junk		      = isnull(b.Junk				   ,0)
      ,a.AddName		      = isnull(b.AddName		   ,'')
      ,a.AddDate		      = b.AddDate		  
      ,a.EditName		      = isnull(b.EditName		   ,'')
      ,a.EditDate		      = b.EditDate		

from Production.dbo.Construction as a inner join Trade_To_Pms.dbo.Construction as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Construction(
       Id
      ,Name
      ,CuttingLayer
	  ,ManualCutLayer
	  ,AutoCutLayer
      ,Junk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       isnull(Id           ,'')
      ,isnull(Name		   ,'')
      ,isnull(CuttingLayer ,0)
	  ,isnull(CuttingLayer, 0)
	  ,isnull(CuttingLayer, 0)
      ,isnull(Junk		   ,0)
      ,isnull(AddName	   ,'')
      ,AddDate	 
      ,isnull(EditName	   ,'')
      ,EditDate
from Trade_To_Pms.dbo.Construction as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Construction as a WITH (NOLOCK) where a.id = b.id)


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
     --  a.ID	     =b.ID	
      a.CountryID	      = isnull(b.CountryID	,'')
      ,a.Remark	      = isnull(b.Remark			,'')
      ,a.AddName	      = isnull(b.AddName	,'')
      ,a.AddDate	      = b.AddDate	
      ,a.EditName	      = isnull(b.EditName	,'')
      ,a.EditDate	      = b.EditDate	
      ,a.Junk	      = isnull(b.Junk			,0)
      ,a.Name	      = isnull(b.Name			,'')
      ,a.AirPort	  = isnull(b.AirPort		,0)
      ,a.SeaPort	  = isnull(b.SeaPort		,0)
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
      ,Name
      ,AirPort
      ,SeaPort

)
select isnull(ID	   ,'')
      ,isnull(CountryID,'')
      ,isnull(Remark   ,'')
      ,isnull(AddName  ,'')
      ,AddDate  
      ,isnull(EditName ,'')
      ,EditDate 
      ,isnull(Junk	   ,0)
      ,isnull(Name	   ,'')
      ,isnull(AirPort  ,0)
      ,isnull(SeaPort  ,0)
from Trade_To_Pms.dbo.Port as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Port as a WITH (NOLOCK) where a.id = b.id)

--Formlist 跳過 目前無轉出TABLE
--DO releasememvar WITH 'FS_CMTPlus'
----------------------刪除主TABLE多的資料
Delete Production.dbo.FSRCpuCost
from Production.dbo.FSRCpuCost as a left join Trade_To_Pms.dbo.FSRCpuCost as b
on a.ShipperID = b.ShipperID and a.OrderCompanyID = b.OrderCompany
where b.ShipperID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       --a.ShipperID	    =b.ShipperID	
      a.AddDate	      =b.AddDate	
      ,a.AddName	      =isnull(b.AddName	 ,'')
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =isnull(b.EditName	 ,'')

from Production.dbo.FSRCpuCost as a inner join Trade_To_Pms.dbo.FSRCpuCost as b ON a.ShipperID=b.ShipperID and  a.OrderCompanyID = b.OrderCompany
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FSRCpuCost(
       ShipperID
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName
	  ,OrderCompanyID
)
select 
       isnull(ShipperID,'')
      ,AddDate
      ,isnull(AddName,'')
      ,EditDate
      ,isnull(EditName,'')
	  ,OrderCompany
from Trade_To_Pms.dbo.FSRCpuCost as b WITH (NOLOCK)
where not exists(select ShipperID from Production.dbo.FSRCpuCost as a WITH (NOLOCK) where a.ShipperID = b.ShipperID and a.OrderCompanyID = b.OrderCompany)


--DO releasememvar WITH 'FS_CMTPlus1'
	----------------------刪除主TABLE多的資料
Delete Production.dbo.FSRCpuCost_Detail
from Production.dbo.FSRCpuCost_Detail as a 
left join Trade_To_Pms.dbo.FSRCpuCost_Detail as b
on a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate
and a.OrderCompanyID = b.OrderCompany
where b.ShipperID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.ShipperID	    =b.ShipperID	
     -- ,a.BeginDate	      =b.BeginDate	
    --  ,a.EndDate	      =b.EndDate	
      a.CpuCost	      =isnull(b.CpuCost	,0)
      ,a.AddDate	      =b.AddDate	
      ,a.AddName	      =isnull(b.AddName	,'')
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =isnull(b.EditName	,'')

from Production.dbo.FSRCpuCost_Detail as a 
inner join Trade_To_Pms.dbo.FSRCpuCost_Detail as b 
ON a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate and a.OrderCompanyID = b.OrderCompany
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
	  ,OrderCompanyID
)
select 
       isnull(ShipperID,'')
      ,BeginDate
      ,EndDate 
      ,isnull(CpuCost  ,0)
      ,AddDate  
      ,isnull(AddName  ,'')
      ,EditDate 
      ,isnull(EditName ,'')
	  ,isnull(OrderCompany,0)
from Trade_To_Pms.dbo.FSRCpuCost_Detail as b WITH (NOLOCK)
where not exists(
	select ShipperID 
	from Production.dbo.FSRCpuCost_Detail as a WITH (NOLOCK) 
	where a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate and a.OrderCompanyID = b.OrderCompany
)



--DO releasememvar WITH 'FtyShipper'  FtyShipper
----------------------刪除主TABLE多的資料
Delete Production.dbo.FtyShipper
from Production.dbo.FtyShipper as a left join Trade_To_Pms.dbo.FtyShipper as b  
on a.BrandID = b.BrandID and a.FactoryID=b.FactoryID
where b.BrandID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.BrandID	    =b.BrandID	
     -- ,a.FactoryID	      =b.FactoryID	
      a.AddDate	      =b.AddDate	
      ,a.AddName	      =isnull(b.AddName	,'')
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =isnull(b.EditName	,'')

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
       isnull(BrandID  ,'')
      ,isnull(FactoryID,'')
      ,AddDate 
      ,isnull(AddName  ,'')
      ,EditDate 
      ,isnull(EditName ,'')
from Trade_To_Pms.dbo.FtyShipper as b WITH (NOLOCK)
where not exists(select BrandID from Production.dbo.FtyShipper as a WITH (NOLOCK) where a.BrandID = b.BrandID and a.FactoryID=b.FactoryID	)

--FtyShipper_Detail
--DO releasememvar WITH 'FtyShipper1'
----------------------刪除主TABLE多的資料
Delete Production.dbo.FtyShipper_Detail
from Production.dbo.FtyShipper_Detail as a left join Trade_To_Pms.dbo.FtyShipper_Detail as b
on a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate  and a.SeasonID	=b.SeasonID 
where b.BrandID is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.BrandID	 =b.BrandID	
      --,a.FactoryID	      =b.FactoryID	
      --,a.BeginDate	      =b.BeginDate	
      a.EndDate	      =b.EndDate	
      ,a.ShipperID	      =isnull(b.ShipperID	,'')

from Production.dbo.FtyShipper_Detail as a
inner join Trade_To_Pms.dbo.FtyShipper_Detail as b ON a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate  and a.SeasonID	=b.SeasonID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FtyShipper_Detail(
       BrandID
      ,FactoryID
      ,BeginDate
      ,EndDate
      ,ShipperID
	  ,SeasonID
)
select 
       isnull(BrandID  ,'')
      ,isnull(FactoryID,'')
      ,BeginDate
      ,EndDate  
      ,isnull(ShipperID,'')
	  ,isnull(SeasonID ,'')
from Trade_To_Pms.dbo.FtyShipper_Detail as b WITH (NOLOCK)
where not exists(select BrandID from Production.dbo.FtyShipper_Detail as a WITH (NOLOCK) 
where a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate and a.SeasonID	=b.SeasonID)

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
      -- a.PhraseTypeName	 =b.PhraseTypeName	
      a.Junk	      = isnull(b.Junk				 ,0)
     -- ,a.Name	      = isnull(b.Name				 ,'')
      ,a.Seq	      = isnull(b.Seq				 ,0)
      ,a.Description	      = isnull(b.Description,'')
      ,a.Module	      = isnull(b.Module				 ,'')
      ,a.AddName	      = isnull(b.AddName		 ,'')
      ,a.AddDate	      = b.AddDate		
      ,a.EditName	      = isnull(b.EditName	     ,'')
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
       isnull(PhraseTypeName,'')
      ,isnull(Junk			,0)
      ,isnull(Name			,'')
      ,isnull(Seq			,0)
      ,isnull(Description	,'')
      ,isnull(Module		,'')
      ,isnull(AddName		,'')
      ,AddDate		
      ,isnull(EditName		,'')
      ,EditDate
from Trade_To_Pms.dbo.Phrase as b WITH (NOLOCK)
where not exists(select PhraseTypeName from Production.dbo.Phrase as a WITH (NOLOCK) where a.PhraseTypeName = b.PhraseTypeName and a.Name = b.Name)


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
       --a.ID	      =b.ID	
      a.Title	      = isnull(b.Title	         ,'')
      ,a.Abbr	      = isnull(b.Abbr			 ,'')
      ,a.Country	      = isnull(b.Country	 ,'')
      ,a.Junk	      = isnull(b.Junk			 ,0)
      ,a.Currency	      = isnull(b.Currency	 ,'')
      ,a.NameCH	      = isnull(b.NameCH			 ,'')
      ,a.NameEN	      = isnull(b.NameEN			 ,'')
      ,a.hasTax	      = isnull(b.hasTax			 ,0)
      ,a.IsDefault	      = isnull(b.IsDefault	 ,0)
      ,a.VatNO	      = isnull(b.VatNO			 ,'')
      ,a.AddressCH	      = isnull(b.AddressCH	 ,'')
      ,a.AddressEN	      = isnull(b.AddressEN	 ,'')
      ,a.Tel	      = isnull(b.Tel			 ,'')
      ,a.Fax	      = isnull(b.Fax			 ,'')
      ,a.AddName	      = isnull(b.AddName	 ,'')
      ,a.AddDate	      = b.AddDate	
      ,a.EditName	      = isnull(b.EditName	 ,'')
      ,a.EditDate	      =b.EditDate	
      ,a.IsOrderCompany	  = isnull(b.IsOrderCompany	 ,0)

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
      ,IsOrderCompany
)
select 
       isnull(ID		,'')
      ,isnull(Title		,'')
      ,isnull(Abbr		,'')
      ,isnull(Country	,'')
      ,isnull(Junk		,0)
      ,isnull(Currency	,'')
      ,isnull(NameCH	,'')
      ,isnull(NameEN	,'')
      ,isnull(hasTax	,0)
      ,isnull(IsDefault	,0)
      ,isnull(VatNO		,'')
      ,isnull(AddressCH	,'')
      ,isnull(AddressEN	,'')
      ,isnull(Tel		,'')
      ,isnull(Fax		,'')
      ,isnull(AddName	,'')
      ,AddDate
      ,isnull(EditName  ,'')
      ,EditDate
      ,isnull(IsOrderCompany,0)
from Trade_To_Pms.dbo.Company as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Company as a WITH (NOLOCK) where a.id = b.id)

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
       --a.ID	      =b.ID	
      a.ExcelName	      = isnull(b.ExcelName	     ,'')
      ,a.ExcelColumn	      = isnull(b.ExcelColumn,'')
      ,a.isArtwork	      = isnull(b.isArtwork		 ,0)
      ,a.AddName	      = isnull(b.AddName		 ,'')
      ,a.AddDate	      = b.AddDate		
      ,a.EditName	      = isnull(b.EditName		 ,'')
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
      ,isnull(ExcelName   ,'')
      ,isnull(ExcelColumn ,'')
      ,isnull(isArtwork	  ,0)
      ,isnull(AddName	  ,'')
      ,AddDate	 
      ,isnull(EditName	  ,'')
      ,EditDate
from Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ADIDASMiSetup_ColorComb as a WITH (NOLOCK) where a.id = b.id)


--------------------------ShipMode-------

Merge Production.dbo.ShipMode as t
using Trade_To_Pms.dbo.ShipMode as s
on t.id=s.id
	when matched then
		update set 
		t.ID= isnull( s.ID,                 ''),
		t.Description= isnull( s.Description,''),
		t.UseFunction= isnull( s.UseFunction,''),
		t.Junk= isnull( s.Junk,				0),
		t.ShareBase= isnull( s.ShareBase,	''),
		t.AddName= isnull( s.AddName,		''),
		t.AddDate=  s.AddDate,	
		t.EditName= isnull( s.EditName,     ''),
		t.EditDate=  s.EditDate
	when not matched by target then
		insert(ID
		,Description
		,UseFunction
		,Junk
		,ShareBase
		,AddName
		,AddDate
		,EditName
		,EditDate
		, shipgroup
		)
				values(
		isnull(s.ID,		 ''),
		isnull(s.Description,''),
		isnull(s.UseFunction,''),
		isnull(s.Junk,		 0),
		isnull(s.ShareBase,	 ''),
		isnull(s.AddName,	 ''),
		s.AddDate,	
		isnull(s.EditName,	 ''),
		s.EditDate,	
		isnull(s.ID          ,'')
		)
	when not matched by source then
		delete;



--------------------------DropDownList--------

Merge Production.dbo.DropDownList as t
Using Trade_To_Pms.dbo.DropDownList as s
on t.type=s.type and t.id=s.id
	when matched then
	update set
		t.Type= isnull( s.Type,              ''),
		t.ID= isnull( s.ID,					 ''),
		t.Name= isnull( s.Name,				 ''),
		t.RealLength= isnull( s.RealLength,	 0),
		t.Description= isnull( s.Description,''),
		t.Seq= isnull( s.Seq				 ,0),
		t.Conditions = isnull(s.Conditions,'')
	when not matched by target then
		insert(Type
				,ID
				,Name
				,RealLength
				,Description
				,Seq
				,Conditions
				)
						values(isnull(s.Type,''),
				isnull(s.ID,				 ''),
				isnull(s.Name,				 ''),
				isnull(s.RealLength,		 0),
				isnull(s.Description,		 ''),
				isnull(s.Seq				 ,0),
				isnull(s.Conditions,'')
				)
	when not matched by source then
		delete;

	---------KeyWord--------------

	Merge Production.dbo.KeyWord as t
	Using Trade_To_Pms.dbo.KeyWord as s
	on t.id=s.id
		when matched then
		update set 
		t.Description= isnull( s.Description,       ''),
		t.Junk= isnull( s.Junk,						0),
		t.Prefix= isnull( s.Prefix,					''),
		t.Fieldname= isnull( s.Fieldname,			''),
		t.Postfix= isnull( s.Postfix,				''),
		t.IsSize= isnull( s.IsSize,					0),
		t.IsPatternPanel= isnull( s.IsPatternPanel,	0),
		t.AddName= isnull( s.AddName,				''),
		t.AddDate=  s.AddDate,		
		t.EditName= isnull( s.EditName,				''),
		t.EditDate= s.EditDate,
		t.SubKeyword= isnull(s.SubKeyword, 0),
		t.CannotOperateStock= isnull(s.CannotOperateStock, 0)
	when not matched by target then 
		insert(ID
		,Description
		,Junk
		,Prefix
		,Fieldname
		,Postfix
		,IsSize
		,IsPatternPanel
		,AddName
		,AddDate
		,EditName
		,EditDate
        ,SubKeyword
        ,CannotOperateStock
		)
			values(
		isnull(s.ID,             ''),
		isnull(s.Description,	 ''),
		isnull(s.Junk,			 0),
		isnull(s.Prefix,		 ''),
		isnull(s.Fieldname,		 ''),
		isnull(s.Postfix,		 ''),
		isnull(s.IsSize,		 0),
		isnull(s.IsPatternPanel, 0),
		isnull(s.AddName,		 ''),
		s.AddDate,	
		isnull(s.EditName,		 ''),
		s.EditDate,
        isnull(s.SubKeyword, 0),
        isnull(s.CannotOperateStock, 0)
		)
		when not matched by source then
			delete;
  

        --Keyword_BomType
	    Merge Production.dbo.Keyword_BomType as t
	    Using Trade_To_Pms.dbo.Keyword_BomType as s
	    on t.id=s.id and t.BomType=s.BomType 
		    when matched then
		    update set 
		    t.BomTypeFieldName = isnull(s.BomTypeFieldName, '')
	    when not matched by target then 
		    insert (
                ID
		        ,BomType
		        ,BomTypeFieldName
		    )
		    values(
                 isnull(ID, '')
		        ,isnull(BomType, '')
		        ,isnull(BomTypeFieldName, '')
		    )
		    when not matched by source then
			    delete;





		--Fabric_Supp
		---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
		UPDATE a
		SET  
			   a.AbbCH	       = isnull(b.AbbCH	       ,'')
			  ,a.AbbEN	       = isnull(b.AbbEN	       ,'')
			  ,a.AddDate	   = b.AddDate	   
			  ,a.AddName	   = isnull(b.AddName	   ,'')
			  ,a.AllowanceRate = isnull(b.AllowanceRate,0)
			  ,a.AllowanceType = isnull(b.AllowanceType,0)
			  ,a.BrandID	   = isnull(b.BrandID	   ,'')
			  ,a.Delay		   = b.Delay		 
			  ,a.DelayMemo	   = isnull(b.DelayMemo	   ,'')
			  ,a.EditDate	   = b.EditDate	   
			  ,a.EditName	   = isnull(b.EditName	   ,'')
			  ,a.IsECFA		   = isnull(b.IsECFA		,0)
			  ,a.ItemType	   = isnull(b.ItemType	   ,'')
			  ,a.Junk		   = isnull(b.Junk		   ,0)
			  ,a.Lock		   = isnull(b.Lock		   ,0)
			  ,a.LTDay		   = isnull(b.LTDay		   ,0)
			  ,a.OrganicCotton = isnull(b.OrganicCotton,0)
			  ,a.POUnit		   = isnull(b.POUnit		,'')
			  ,a.PreShrink	   = isnull(b.PreShrink	   ,0)
			  ,a.Refno		   = isnull(b.Refno		   ,'')
			  ,a.Remark		   = isnull(b.Remark		,'')
			  ,a.SeasonID	   = isnull(b.SeasonID	   ,'')
			  ,a.ShowSuppColor = isnull(b.ShowSuppColor,0)
			  ,a.SuppRefno	   = isnull(b.SuppRefno	   ,'')
			  ,a.IsDefault	   = isnull(b.IsDefault, 0)
		from Production.dbo.Fabric_Supp as a 
		inner join Trade_To_Pms.dbo.Fabric_Supp as b ON a.SuppID=b.SuppID and a.SCIRefno = b.SCIRefno
		-------------------------- INSERT INTO 抓
		INSERT INTO Production.dbo.Fabric_Supp(
				 AbbCH	      
				,AbbEN	      
				,AddDate	  
				,AddName	  
				,AllowanceRate
				,AllowanceType
				,BrandID	  
				,Delay		  
				,DelayMemo	  
				,EditDate	  
				,EditName	  
				,IsECFA		  
				,ItemType	  
				,Junk		  
				,Lock		  
				,LTDay		  
				,OrganicCotton
				,POUnit		  
				,PreShrink	  
				,Refno		  
				,Remark		  
				,SeasonID	  
				,ShowSuppColor
				,SuppRefno	  
				,ukey
				,SuppID
				,SCIRefno
                ,IsDefault
		)
		select 
				 isnull(AbbCH	      	   ,'')
				,isnull(AbbEN	      	   ,'')
				,AddDate	  		 
				,isnull(AddName	  		   ,'')
				,isnull(AllowanceRate	   ,0)
				,isnull(AllowanceType	   ,0)
				,isnull(BrandID	  		   ,'')
				,Delay		  	   
				,isnull(DelayMemo	  	   ,'')
				,EditDate	  	   
				,isnull(EditName	  	   ,'')
				,isnull(IsECFA		  	   ,0)
				,isnull(ItemType	  	   ,'')
				,isnull(Junk		  	   ,0)
				,isnull(Lock		  	   ,0)
				,isnull(LTDay		  	   ,0)
				,isnull(OrganicCotton	   ,0)
				,isnull(POUnit		  	   ,'')
				,isnull(PreShrink	  	   ,0)
				,isnull(Refno		  	   ,'')
				,isnull(Remark		  	   ,'')
				,isnull(SeasonID	  	   ,'')
				,isnull(ShowSuppColor	   ,0)
				,isnull(SuppRefno	  	   ,'')
				,isnull(ukey			   ,0)
				,isnull(SuppID			   ,'')
				,isnull(SCIRefno		   ,'')
                ,isnull(IsDefault, 0)
		from Trade_To_Pms.dbo.Fabric_Supp as b WITH (NOLOCK)
		where not exists(select SuppID from Production.dbo.Fabric_Supp as a WITH (NOLOCK) where a.SuppID = b.SuppID and a.SCIRefno = b.SCIRefno)

--------Color_Multiple---------------

Merge Production.dbo.Color_Multiple as t
Using Trade_To_Pms.dbo.Color_Multiple as s
on t.colorUkey=s.colorUkey and t.Seqno=s.Seqno
	when matched then
	update set
	t.ID= isnull( s.ID,			   ''),
	t.BrandID= isnull( s.BrandID,	''),
	t.ColorID= isnull( s.ColorID,  ''),
	t.AddName= isnull( s.AddName,  ''),
	t.AddDate=  s.AddDate,  
	t.EditName= isnull( s.EditName,''),
	t.EditDate= s.EditDate
when not matched by target then 
	insert(ID
	,ColorUkey
	,BrandID
	,Seqno
	,ColorID
	,AddName
	,AddDate
	,EditName
	,EditDate
	)
	values(
	isnull(s.ID,	   ''),
	isnull(s.ColorUkey,0),
	isnull(s.BrandID,  ''),
	isnull(s.Seqno,	   ''),
	isnull(s.ColorID,  ''),
	isnull(s.AddName,  ''),
	s.AddDate, 
	isnull(s.EditName, ''),
	s.EditDate	)
when not matched by source then
	delete;


--------Color_SuppColor---------------

Merge Production.dbo.Color_SuppColor as t
Using Trade_To_Pms.dbo.Color_SuppColor as s
on t.ukey=s.ukey
when matched then
	update set
	t.ID= isnull( s.ID,					 ''),
	t.Ukey= isnull( s.Ukey,				 0),
	t.BrandId= isnull( s.BrandId,		 ''),
	t.ColorUkey= isnull( s.ColorUkey,	 0),
	t.SeasonID= isnull( s.SeasonID,		 ''),
	t.SuppID= isnull( s.SuppID,			 ''),
	t.SuppColor= isnull( s.SuppColor,	 ''),
	t.ProgramID= isnull( s.ProgramID,	 ''),
	t.StyleID= isnull( s.StyleID,		 ''),
	t.Refno= isnull( s.Refno,			 ''),
	t.Remark= isnull( s.Remark,			 ''),
	t.AddName= isnull( s.AddName,		 ''),
	t.AddDate=  s.AddDate,		
	t.EditName= isnull( s.EditName,      ''),
	t.EditDate= s.EditDate,
	t.SuppGroupFabric= isnull(s.SuppGroupFabric, ''),
	t.MtlTypeId= isnull(s.MtlTypeId, '')
when not matched by target then
	insert(ID
	,Ukey
	,BrandId
	,ColorUkey
	,SeasonID
	,SuppID
	,SuppColor
	,ProgramID
	,StyleID
	,Refno
	,Remark
	,AddName
	,AddDate
	,EditName
	,EditDate
    ,SuppGroupFabric
    ,MtlTypeId
	)
	values(
	isnull(s.ID,	   ''),
	isnull(s.Ukey,	   0),
	isnull(s.BrandId,  ''),
	isnull(s.ColorUkey,0),
	isnull(s.SeasonID, ''),
	isnull(s.SuppID,   ''),
	isnull(s.SuppColor,''),
	isnull(s.ProgramID,''),
	isnull(s.StyleID,  ''),
	isnull(s.Refno,	   ''),
	isnull(s.Remark,   ''),
	isnull(s.AddName,  ''),
	s.AddDate,  
	isnull(s.EditName, ''),
    s.EditDate,
    isnull(s.SuppGroupFabric, ''),
    isnull(s.MtlTypeId, '')
    )
when not matched by source then 
	delete;

----update system.ProphetSingleSizeDeduct欄位
update Production.dbo.System
set ProphetSingleSizeDeduct =
isnull((select ProphetSingleSizeDeduct from Trade_To_Pms.dbo.Tradesystem s),0)

	
--------Buyer---------------

Merge Production.dbo.Buyer as t
Using Trade_To_Pms.dbo.Buyer as s
on t.ID=s.ID
when matched then
	update set
	t.CountryID= isnull( s.CountryID,  ''),
	t.NameCH= isnull( s.NameCH,		   ''),
	t.NameEN= isnull( s.NameEN,		   ''),
	t.Tel= isnull( s.Tel,			   ''),
	t.Fax= isnull( s.Fax,			   ''),
	t.Contact1= isnull( s.Contact1,	   ''),
	t.Contact2= isnull( s.Contact2,	   ''),
	t.AddressCH= isnull( s.AddressCH,  ''),
	t.AddressEN= isnull( s.AddressEN,  ''),
	t.BillTo1= isnull( s.BillTo1,	   ''),
	t.BillTo2= isnull( s.BillTo2,	   ''),
	t.BillTo3= isnull( s.BillTo3,	   ''),
	t.BillTo4= isnull( s.BillTo4,	   ''),
	t.BillTo5= isnull( s.BillTo5,	   ''),
	t.CurrencyID= isnull( s.CurrencyID,''),
	t.Remark= isnull( s.Remark,		   ''),
	t.ZipCode= isnull( s.ZipCode,	   ''),
	t.Email= isnull( s.Email,		   ''),
	t.MrTeam= isnull( s.MrTeam,		   ''),
	t.AddName= isnull( s.AddName,	   ''),
	t.AddDate=  s.AddDate,	   
	t.EditName= isnull( s.EditName,	   ''),
	t.EditDate=  s.EditDate,	   
	t.Junk= isnull( s.Junk			  , 0)
when not matched by target then
	insert(ID
	,CountryID
	,NameCH
	,NameEN
	,Tel
	,Fax
	,Contact1
	,Contact2
	,AddressCH
	,AddressEN
	,BillTo1
	,BillTo2
	,BillTo3
	,BillTo4
	,BillTo5
	,CurrencyID
	,Remark
	,ZipCode
	,Email
	,MrTeam
	,AddName
	,AddDate
	,EditName
	,EditDate
	,Junk
	)
	values(
	isnull(s.ID,        ''),
	isnull(s.CountryID,	''),
	isnull(s.NameCH,	''),
	isnull(s.NameEN,	''),
	isnull(s.Tel,		''),
	isnull(s.Fax,		''),
	isnull(s.Contact1,	''),
	isnull(s.Contact2,	''),
	isnull(s.AddressCH,	''),
	isnull(s.AddressEN,	''),
	isnull(s.BillTo1,	''),
	isnull(s.BillTo2,	''),
	isnull(s.BillTo3,	''),
	isnull(s.BillTo4,	''),
	isnull(s.BillTo5,	''),
	isnull(s.CurrencyID,''),
	isnull(s.Remark,	''),
	isnull(s.ZipCode,	''),
	isnull(s.Email,		''),
	isnull(s.MrTeam,	''),
	isnull(s.AddName,	''),
	s.AddDate,	
	isnull(s.EditName,	''),
	s.EditDate,	
	isnull(s.Junk		,0)
	)
when not matched by source then 
	delete;	


--------CutReason---------------

Merge Production.dbo.CutReason as t
Using Trade_To_Pms.dbo.CutReason as s
on t.Type=s.Type and t.ID=s.ID 
when matched then
	update set
	t.Description= isnull( s.Description,''),
	t.Remark= isnull( s.Remark,			 ''),
	t.Junk= isnull( s.Junk,				 0),
	t.AddName= isnull( s.AddName,		 ''),
	t.AddDate=  s.AddDate,		 
	t.EditName= isnull( s.EditName,		 ''),
	t.EditDate= s.EditDate
when not matched by target then
	insert(Type
	,ID
	,Description
	,Remark
	,Junk
	,AddName
	,AddDate
	,EditName
	,EditDate
	)
	values(
	isnull(s.Type,		 ''),
	isnull(s.ID,		 ''),
	isnull(s.Description,''),
	isnull(s.Remark,	 ''),
	isnull(s.Junk,		 0),
	isnull(s.AddName,	 ''),
	s.AddDate,	 
	isnull(s.EditName,   ''),
	s.EditDate)
when not matched by source then 
	delete;	

--------IEReason---------------

Merge Production.dbo.IEReason as t
Using Trade_To_Pms.dbo.IEReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= isnull( s.Description, ''),
	t.Junk= isnull( s.Junk,				  0),
	t.AddName= isnull( s.AddName,		  ''),
	t.AddDate=  s.AddDate,		
	t.EditName= isnull( s.EditName,		  ''),
	t.EditDate= s.EditDate
when not matched by target then
	insert(Type
	,ID
	,Description
	,Junk
	,AddName
	,AddDate
	,EditName
	,EditDate
	)
	values(
	isnull(s.Type,       ''),
	isnull(s.ID,		 ''),
	isnull(s.Description,''),
	isnull(s.Junk,		 0),
	isnull(s.AddName,	 ''),
	s.AddDate,	 
	isnull(s.EditName,	 ''),
	s.EditDate)
when not matched by source then 
	delete;	


--------PackingReason---------------

Merge Production.dbo.PackingReason as t
Using Trade_To_Pms.dbo.PackingReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= isnull( s.Description,''),
	t.Junk= isnull( s.Junk,				 0),
	t.AddName= isnull( s.AddName,		 ''),
	t.AddDate=  s.AddDate,		
	t.EditName= isnull( s.EditName,		 ''),
	t.EditDate= s.EditDate
when not matched by target then
	insert(Type
	,ID
	,Description
	,Junk
	,AddName
	,AddDate
	,EditName
	,EditDate
	)
	values(
	isnull(s.Type,         ''),
	isnull(s.ID,		   ''),
	isnull(s.Description,  ''),
	isnull(s.Junk,		   0),
	isnull(s.AddName,	   ''),
	s.AddDate,	   
	isnull(s.EditName,	   ''),
	s.EditDate)
when not matched by source and t.type <> 'ER' then 
	delete;	

--------PPICReason---------------

Merge Production.dbo.PPICReason as t
Using Trade_To_Pms.dbo.PPICReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= isnull( s.Description,''),
	t.Remark= isnull( s.Remark,			 ''),
	t.Junk= isnull( s.Junk,				 0),
	t.TypeForUse= isnull( s.TypeForUse,	 ''),
	t.AddName= isnull( s.AddName,		 ''),
	t.AddDate=  s.AddDate,		
	t.EditName= isnull( s.EditName,		 ''),
	t.EditDate= s.EditDate
when not matched by target then
	insert(Type
	,ID
	,Description
	,Remark
	,Junk
	,TypeForUse
	,AddName
	,AddDate
	,EditName
	,EditDate
	)
	values(
	isnull(s.Type,		  ''),
	isnull(s.ID,		  ''),
	isnull(s.Description, ''),
	isnull(s.Remark,	  ''),
	isnull(s.Junk,		  0),
	isnull(s.TypeForUse,  ''),
	isnull(s.AddName,	  ''),
	s.AddDate,	  
	isnull(s.EditName,    ''),
	s.EditDate)
when not matched by source then 
	delete;	


--------SubProcess---------------
delete Production.dbo.AutomationSubProcess

insert into Production.dbo.AutomationSubProcess(ID)
select isnull(s.ID,'')
from Trade_To_Pms.dbo.SubProcess s with (nolock)
left Join Production.dbo.SubProcess t with (nolock) on s.ID = t.ID
where s.Junk <> t.Junk or t.Junk is null

Merge Production.dbo.SubProcess as t
Using Trade_To_Pms.dbo.SubProcess as s
on t.ID=s.ID
when matched then
	update set
	t.ArtworkTypeId= isnull( s.ArtworkTypeId,					   ''),
	t.IsSelection= isnull( s.IsSelection,						   0),
	t.IsRFIDProcess= isnull( s.IsRFIDProcess,					   0),
	t.IsRFIDDefault= isnull( s.IsRFIDDefault,					   0),
	t.ShowSeq= isnull( s.ShowSeq,								   ''),
	t.Junk= isnull( s.Junk,										   0),
	t.AddName= isnull( s.AddName,								   ''),
	t.AddDate=  s.AddDate,								  
	t.EditName= isnull( s.EditName,								   ''),
	t.EditDate=  s.EditDate,								
	t.BCSDate= isnull( s.BCSDate,								   0),
	t.InOutRule  = isnull( s.InOutRule ,						   0),
	t.FullName  = isnull( s.FullName ,							   ''),
	t.IsLackingAndReplacement  = isnull( s.IsLackingAndReplacement,0),
	t.IsBoundedProcess  = isnull( s.IsBoundedProcess ,			   0),
	t.IsSubprocessInspection  = isnull( s.IsSubprocessInspection   ,0)

when not matched by target then
	insert(ID
	,ArtworkTypeId
	,IsSelection
	,IsRFIDProcess
	,IsRFIDDefault
	,ShowSeq
	,Junk
	,AddName
	,AddDate
	,EditName
	,EditDate
	,BCSDate
	,InOutRule 
	,FullName
	,IsLackingAndReplacement
	,IsBoundedProcess
	,IsSubprocessInspection
	)
	values(
	isnull(s.ID,                   	 ''),
	isnull(s.ArtworkTypeId,			 ''),
	isnull(s.IsSelection,			 0),
	isnull(s.IsRFIDProcess,			 0),
	isnull(s.IsRFIDDefault,			 0),
	isnull(s.ShowSeq,				 ''),
	isnull(s.Junk,					 0),
	isnull(s.AddName,				 ''),
	s.AddDate,				
	isnull(s.EditName,				 ''),
	s.EditDate,			
	isnull(s.BCSDate,				 0),
	isnull(s.InOutRule,				 0),
	isnull(s.FullName,				 ''),
	isnull(s.IsLackingAndReplacement,0),
	isnull(s.IsBoundedProcess,		 0),
	isnull(s.IsSubprocessInspection, 0)
	)
when not matched by source then 
	delete;	


--------WhseReason---------------

DELETE Production.dbo.WhseReason
WHERE NOT EXISTS (
    SELECT 1
    FROM Trade_To_Pms.dbo.WhseReason AS s
    WHERE Production.dbo.WhseReason.Type = s.Type AND Production.dbo.WhseReason.ID = s.ID
);

UPDATE t
SET
    t.Description = ISNULL(s.Description, ''),
    t.Remark = ISNULL(s.Remark, ''),
    t.Junk = ISNULL(s.Junk, 0),
    t.ActionCode = ISNULL(s.ActionCode, ''),
    t.AddName = ISNULL(s.AddName, ''),
    t.AddDate = s.AddDate,
    t.EditName = ISNULL(s.EditName, ''),
    t.EditDate = s.EditDate,
    t.No = ISNULL(s.No, 0)
FROM Production.dbo.WhseReason AS t
INNER JOIN Trade_To_Pms.dbo.WhseReason AS s ON t.Type = s.Type AND t.ID = s.ID
WHERE s.Type <> 'DR'
;

UPDATE t
SET
    t.Description = ISNULL(s.Description, ''),
    t.Junk = ISNULL(s.Junk, 0),
    t.ActionCode = ISNULL(s.ActionCode, ''),
    t.AddName = ISNULL(s.AddName, ''),
    t.AddDate = s.AddDate,
    t.EditName = IIF(s.EditDate IS NULL OR s.EditDate < t.EditDate, ISNULL(t.EditName, ''), ISNULL(s.EditName, '')),
    t.EditDate = IIF(s.EditDate IS NULL OR s.EditDate < t.EditDate, t.EditDate, s.EditDate),
    t.No = ISNULL(s.No, 0)
FROM Production.dbo.WhseReason AS t
INNER JOIN Trade_To_Pms.dbo.WhseReason AS s ON t.Type = s.Type AND t.ID = s.ID
WHERE s.Type = 'DR'

INSERT INTO Production.dbo.WhseReason (Type, ID, Description, Remark, Junk, ActionCode, AddName, AddDate, EditName, EditDate, No)
SELECT 
    ISNULL(Type, ''),
    ISNULL(ID, ''),
    ISNULL(Description, ''),
    ISNULL(Remark, ''),
    ISNULL(Junk, 0),
    ISNULL(ActionCode, ''),
    ISNULL(AddName, ''),
    AddDate,
    ISNULL(EditName, ''),
    EditDate,
    ISNULL(No, 0)
FROM Trade_To_Pms.dbo.WhseReason AS s
WHERE NOT EXISTS (
    SELECT 1
    FROM Production.dbo.WhseReason AS t
    WHERE t.Type = s.Type AND t.ID = s.ID
)
AND s.Type <> 'DR'

INSERT INTO Production.dbo.WhseReason (Type, ID, Description, Junk, ActionCode, AddName, AddDate, EditName, EditDate, No)
SELECT 
    ISNULL(Type, ''),
    ISNULL(ID, ''),
    ISNULL(Description, ''),
    ISNULL(Junk, 0),
    ISNULL(ActionCode, ''),
    ISNULL(AddName, ''),
    AddDate,
    ISNULL(EditName, ''),
    EditDate,
    ISNULL(No, 0)
FROM Trade_To_Pms.dbo.WhseReason AS s
WHERE NOT EXISTS (
    SELECT 1
    FROM Production.dbo.WhseReason AS t
    WHERE t.Type = s.Type AND t.ID = s.ID
)
AND s.Type = 'DR'

--------ClogReason---------------

Merge Production.dbo.ClogReason as t
Using Trade_To_Pms.dbo.ClogReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= isnull( s.Description,''),
	t.Remark= isnull( s.Remark,			 ''),
	t.Junk= isnull( s.Junk,				 0),
	t.AddName= isnull( s.AddName,		 ''),
	t.AddDate=  s.AddDate,		
	t.EditName= isnull( s.EditName,		 ''),
	t.EditDate= s.EditDate
when not matched by target then
	insert(Type
	,ID
	,Description
	,Remark
	,Junk
	,AddName
	,AddDate
	,EditName
	,EditDate
	)
	values(
	isnull(s.Type,        ''),
	isnull(s.ID,		  ''),
	isnull(s.Description, ''),
	isnull(s.Remark,	  ''),
	isnull(s.Junk,		  0),
	isnull(s.AddName,	  ''),
	s.AddDate,	
	isnull(s.EditName,	  ''),
	s.EditDate
	)
when not matched by source then 
	delete;	

--------ThreadAllowanceScale---------------

Merge Production.dbo.ThreadAllowanceScale as t
Using Trade_To_Pms.dbo.ThreadAllowanceScale as s
on t.ID = s.ID
when matched then
      update set
      t.LowerBound = isnull( s.LowerBound,0),
      t.UpperBound = isnull( s.UpperBound,0),
      t.Allowance = isnull( s.Allowance,  0),
      t.Remark = isnull( s.Remark,		  ''),
      t.AddName = isnull( s.AddName,	  ''),
      t.AddDate=  s.AddDate,	
      t.EditName = isnull( s.EditName,	  ''),
      t.EditDate = s.EditDate
when not matched by target then
      insert (
            ID , LowerBound  , UpperBound  , Allowance  , Remark    , AddName
            , AddDate   , EditName    , EditDate
      ) values (
             isnull(s.ID		,'')
		   , isnull(s.LowerBound,0)
		   , isnull(s.UpperBound,0)
		   , isnull(s.Allowance	,0)
		   , isnull(s.Remark  	,'')
		   , isnull(s.AddName	,'')
		   , s.AddDate	
		   , isnull(s.EditName  ,'')
		   , s.EditDate
      )
when not matched by source then 
      delete;     
      
--------Thread_AllowanceScale---------------

Merge Production.dbo.Thread_AllowanceScale as t
Using Trade_To_Pms.dbo.Thread_AllowanceScale as s
on t.ID = s.ID and t.Type = s.Type
when matched then
update set                  
     t.[LowerBound]                     = isnull(s.[LowerBound], 0)
    ,t.[UpperBound]                     = isnull(s.[UpperBound], 0)
    ,t.[Allowance]                      = isnull(s.[Allowance], 0)
    ,t.[Remark]                         = isnull(s.[Remark], '')
    ,t.[AddName]                        = isnull(s.[AddName], '')
    ,t.[AddDate]                        = s.[AddDate]                       
    ,t.[EditName]                       = isnull(s.[EditName], '')
    ,t.[EditDate]                       = s.[EditDate]                      
    ,t.[Allowance_UserQtyBelowStandard] = isnull(s.[Allowance_UserQtyBelowStandard], 0)
when not matched by target then
      insert(
            [ID]
           ,[LowerBound]
           ,[UpperBound]
           ,[Allowance]
           ,[Remark]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate]
           ,[Allowance_UserQtyBelowStandard]
           ,[Type]
           )
       values(
            isnull(s.[ID], '')
           ,isnull(s.[LowerBound], 0)
           ,isnull(s.[UpperBound], 0)
           ,isnull(s.[Allowance], 0)
           ,isnull(s.[Remark], '')
           ,isnull(s.[AddName], '')
           ,s.[AddDate]
           ,isnull(s.[EditName], '')
           ,s.[EditDate]
           ,isnull(s.[Allowance_UserQtyBelowStandard], 0)
           ,isnull(s.[Type], '')
           )
           ;
           
--------ThreadCommon---------------
Merge Production.dbo.ThreadCommon as t
Using Trade_To_Pms.dbo.ThreadCommon as s
on t.Ukey = s.Ukey
when matched then
update set
            t.[BrandID]  = isnull(s.[BrandID], '')
           ,t.[Refno]    = isnull(s.[Refno], '')
           ,t.[ColorId]  = isnull(s.[ColorId], '')
           ,t.[AddDate]  = s.[AddDate] 
           ,t.[AddName]  = isnull(s.[AddName], '')
           ,t.[EditDate] = s.[EditDate]
           ,t.[EditName] = isnull(s.[EditName], '')
when not matched by target then
      insert(
            [Ukey]
           ,[BrandID]
           ,[Refno]
           ,[ColorId]
           ,[AddDate]
           ,[AddName]
           ,[EditDate]
           ,[EditName]
           )
       values(
            isnull(s.[Ukey], 0)
           ,isnull(s.[BrandID], '')
           ,isnull(s.[Refno], '')
           ,isnull(s.[ColorId], '')
           ,s.[AddDate]
           ,isnull(s.[AddName], '')
           ,s.[EditDate]
           ,isnull(s.[EditName], '')
           )
           ;

           
--------ThreadCommon_Detail---------------
Merge Production.dbo.ThreadCommon_Detail as t
Using Trade_To_Pms.dbo.ThreadCommon_Detail as s
on t.Ukey = s.Ukey
when matched then
update set
            t.[ThreadCommonUkey] = isnull(s.[ThreadCommonUkey], 0)
           ,t.[StartDate]        = s.[StartDate]
           ,t.[EndDate]          = s.[EndDate]
           ,t.[AddDate]          = s.[AddDate]
           ,t.[AddName]          = isnull(s.[AddName], '')
           ,t.[EditDate]         = s.[EditDate]
           ,t.[EditName]         = isnull(s.[EditName], '')
           ,t.[Type]             = isnull(s.[Type], '')
when not matched by target then
      insert(
            [Ukey]
           ,[ThreadCommonUkey]
           ,[StartDate]
           ,[EndDate]
           ,[AddDate]
           ,[AddName]
           ,[EditDate]
           ,[EditName]
           ,[Type]
           )
       values(
            isnull(s.[Ukey], 0)
           ,isnull(s.[ThreadCommonUkey], 0)
           ,s.[StartDate]
           ,s.[EndDate]
           ,s.[AddDate]
           ,isnull(s.[AddName], '')
           ,s.[EditDate]
           ,isnull(s.[EditName], '')
           ,isnull(s.[Type], '')
           )
           ;
--------SubconReason---------------

Merge Production.dbo.SubconReason as t
Using Trade_To_Pms.dbo.SubconReason as s
on t.ID = s.ID and t.Type = s.Type
when matched then
      update set	t.Reason		 = isnull( s.Reason		 ,''),
					t.Responsible	 = isnull( s.Responsible ,''),
					t.Junk			 = isnull( s.Junk		 ,0),
					t.AddDate		 =  s.AddDate	 ,
					t.AddName		 = isnull( s.AddName	 ,''),
					t.EditDate		 =  s.EditDate	 ,
					t.EditName		 = isnull( s.EditName	 ,''),
					t.Status		 = isnull( s.Status		 ,'')
when not matched by target then
      insert (Type,ID,Reason,Responsible,Junk,AddDate,AddName,EditDate,EditName ,Status
      ) values (
	   isnull(s.Type		,'')
	  ,isnull(s.ID			,'')
	  ,isnull(s.Reason		,'')
	  ,isnull(s.Responsible	,'')
	  ,isnull(s.Junk		,0)
	  ,s.AddDate		
	  ,isnull(s.AddName		,'')
	  ,s.EditDate	
	  ,isnull(s.EditName	,'')
	  ,isnull(Status        ,'')
      )
when not matched by source then 
      delete;     

--------SewingReason---------------

Merge Production.dbo.SewingReason as sr
Using Trade_To_Pms.dbo.SewingReason as tsr
on sr.ID = tsr.ID AND sr.Type=tsr.Type
when matched then
      update set
       sr.Type = isnull( tsr.Type,				 ''),
       sr.ID = isnull( tsr.ID,					 ''),
       sr.Description= isnull( tsr.Description,	 ''),
       sr.Junk = isnull( tsr.Junk,				 0),
       sr.AddName = isnull( tsr.AddName,		 ''),
       sr.AddDate =  tsr.AddDate,		
       sr.EditName = isnull( tsr.EditName,		 ''),
       sr.EditDate =  tsr.EditDate,    
       sr.ForDQSCheck = isnull( tsr.ForDQSCheck ,0)

when not matched by target then
      insert (
            Type , ID  , Description  , Junk  , AddName    , AddDate
            , EditName   , EditDate   , ForDQSCheck  
      ) values (
              isnull(tsr.Type		   , '')
			, isnull(tsr.ID			   , '')
			, isnull(tsr.Description   , '')
			, isnull(tsr.Junk		   , 0)
			, isnull(tsr.AddName	   , '')
			, tsr.AddDate	  
            , isnull(tsr.EditName 	   , '')
			, tsr.EditDate	  
			, isnull(tsr.ForDQSCheck  , 0)
      )
when not matched by source then 
      delete;     

--------GMTBooking---------------
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET   
      a.Vessel     = isnull(b.Vessel  , '')
      ,a.ETD       = b.ETD 	  
      ,a.ETA       = b.ETA 	
      ,a.InvDate   = b.InvDate
      ,a.FCRDate   = b.FCRDate 
	  ,a.BLNo	   = isnull(b.BLNo	  , '')
	  ,a.BL2No	   = isnull(b.BL2No	  , '')
	  ,a.InvoiceApproveDate	   =b.InvoiceApproveDate
	  ,a.IntendDeliveryDate	   =b.IntendDeliveryDate
	  ,a.ActFCRDate	   =b.ActFCRDate
from Production.dbo.GMTBooking as a inner join Trade_To_Pms.dbo.GarmentInvoice as b ON a.id=b.id
where b.InvDate is not null

UPDATE a
SET   
     a.Foundry = isnull(b.Foundry, 0)
from Production.dbo.GMTBooking as a 
inner join Trade_To_Pms.dbo.GarmentInvoice as b ON a.id=b.id

----GarmentInvoice_Foundry


UPDATE t
SET 
	 t.GW = isnull(s.GW	   , 0)
	,t.CBM=isnull(s.CBM	   , 0)
	,t.Ratio=isnull(s.Ratio, 0)
FROM Production.dbo.GarmentInvoice_Foundry t WITH (NOLOCK)
inner join Trade_To_Pms.dbo.GarmentInvoice_Foundry s WITH (NOLOCK) on t.InvoiceNo=s.InvoiceNo and t.FactoryGroup=s.FactoryGroup

INSERT INTO Production.dbo.GarmentInvoice_Foundry 
	  (InvoiceNo,FactoryGroup,GW,CBM,Ratio)
select
	 isnull(InvoiceNo   , '')
	,isnull(FactoryGroup, '')
	,isnull(GW			, 0)
	,isnull(CBM			, 0)
	,isnull(Ratio   	, 0)
FROM Trade_To_Pms.dbo.GarmentInvoice_Foundry as b WITH (NOLOCK)
where not exists(
	select 1 
	from Production.dbo.GarmentInvoice_Foundry as a WITH (NOLOCK) 
	where a.InvoiceNo = b.InvoiceNo
	and a.FactoryGroup = b.FactoryGroup
)

delete t
from Production.dbo.GarmentInvoice_Foundry t
where exists(
	select 1
	from Trade_To_Pms.dbo.GarmentInvoice_Foundry s WITH (NOLOCK)
	where t.InvoiceNo = s.InvoiceNo
)
and not exists(
	select 1
	from Trade_To_Pms.dbo.GarmentInvoice_Foundry s WITH (NOLOCK)
	where t.InvoiceNo = s.InvoiceNo
	and t.FactoryGroup = s.FactoryGroup
)




--------SeasonSCI---------------
truncate table production.dbo.SeasonSCI
insert into production.dbo.SeasonSCI([ID],[Month],[AddName],[AddDate],[EditName],[EditDate],[Junk])
select
	 isnull([ID], '')
	,isnull([Month], '')
	,isnull([AddName], '')
	,[AddDate]
	,isnull([EditName], '')
	,[EditDate]
	,isnull([Junk] , 0)
from Trade_To_Pms.dbo.SeasonSCI


------FirstSaleCostSetting---------------
Merge Production.dbo.FirstSaleCostSetting as sr
Using Trade_To_Pms.dbo.FirstSaleCostSetting as tsr 
on sr.CountryID = tsr.CountryID AND sr.ArtWorkID=tsr.ArtWorkID AND sr.CostTypeID=tsr.CostTypeID AND sr.BeginDate=tsr.BeginDate
when matched then
      update set 
       sr.EndDate = tsr.EndDate, 
	   sr.IsJunk = isnull(tsr.IsJunk,  0),
	   sr.AddDate = tsr.AddDate, 
	   sr.AddName = isnull(tsr.AddName,  ''),
	   sr.EditDate = tsr.EditDate, 
	   sr.EditName = isnull(tsr.EditName, '')
when not matched by target then
      insert (CountryID, ArtWorkID, CostTypeID, BeginDate, EndDate, IsJunk, AddDate, AddName, EditDate, EditName) 
	  values (
		  isnull(tsr.CountryID	, '')
		, isnull(tsr.ArtWorkID	, '')
		, isnull(tsr.CostTypeID	, '')
		, tsr.BeginDate	
		, tsr.EndDate	
		, isnull(tsr.IsJunk		, 0)
		, tsr.AddDate	
		, isnull(tsr.AddName	, '')
		, tsr.EditDate	
		, isnull(tsr.EditName   , '')
		)
when not matched by source then 
      delete;

------Brand_ThreadCalculateRules---------------
Merge Production.dbo.Brand_ThreadCalculateRules as t
Using (select a.* from Trade_To_Pms.dbo.Brand_ThreadCalculateRules a ) as s
on t.ID=s.ID and t.FabricType = s.FabricType and t.ProgramID = s.ProgramID
when matched then 
	update set	t.UseRatioRule	= isnull( s.UseRatioRule, ''),
				t.UseRatioRule_Thick	= isnull( s.UseRatioRule_Thick, '')
when not matched by target then
	insert (ID,
			FabricType,
			UseRatioRule,
			UseRatioRule_Thick,
			ProgramID
			) 
		values (isnull(s.ID,				''),
				isnull(s.FabricType,		''),
				isnull(s.UseRatioRule,		''),
				isnull(s.UseRatioRule_Thick,''),
				isnull(s.ProgramID	,''))
when not matched by source then 
	delete;

------MachineType_ThreadRatio---------------
Merge Production.dbo.MachineType_ThreadRatio as t
Using (select a.* from Trade_To_Pms.dbo.MachineType_ThreadRatio a ) as s
on t.ID=s.ID and t.SEQ = s.SEQ
when matched then 
	update set	t.ThreadLocation	   = isnull( s.ThreadLocation	 ,''),
				t.UseRatio			   = isnull( s.UseRatio			 ,0),
				t.Allowance			   = isnull( s.Allowance		 ,0),
				t.AllowanceTubular     = isnull( s.AllowanceTubular	 ,0)
when not matched by target then
	insert (ID				 ,
			SEQ				 ,
			ThreadLocation	 ,
			UseRatio	     ,
			Allowance        ,
			AllowanceTubular
			) 
		values ( isnull(s.ID				 ,''),
				 isnull(s.SEQ				 ,''),
				 isnull(s.ThreadLocation	 ,''),
				 isnull(s.UseRatio			 ,0),
				 isnull(s.Allowance			 ,0),
				 isnull(s.AllowanceTubular	 ,0)
				)
when not matched by source then 
	delete;

-------MachineType_ThreadRatio_Hem-----------
Merge Production.dbo.MachineType_ThreadRatio_Hem as t
Using (select * from Trade_To_Pms.dbo.MachineType_ThreadRatio_Hem) as s
on t.id = s.id
and t.Seq = s.Seq and t.UseRatioRule = s.UseRatioRule
when matched then
	update set
	t.UseRatio = isnull( s.UseRatio ,0),
	t.ukey = isnull( s.ukey ,0)
when not matched by target then
	insert(ID,Seq,UseRatioRule,UseRatio,Ukey)
	values(
	 isnull(s.ID			,'')
	,isnull(s.Seq			,'')
	,isnull(s.UseRatioRule	,'')
	,isnull(s.UseRatio		,0)
	,isnull(s.Ukey			,0)
	)
when not matched by source then
	delete;

------MachineType_ThreadRatio_Regular---------------
Merge Production.dbo.MachineType_ThreadRatio_Regular as t
Using (select a.* from Trade_To_Pms.dbo.MachineType_ThreadRatio_Regular a ) as s
on t.ID=s.ID and t.SEQ = s.SEQ and t.UseRatioRule = s.UseRatioRule
when matched then 
	update set	t.UseRatio	   = isnull(s.UseRatio	 ,0)
when not matched by target then
	insert (ID				,
			Seq				,
			UseRatioRule	,
			UseRatio
			) 
		values (isnull(s.ID				 ,''),
				isnull(s.Seq			 ,'')	,
				isnull(s.UseRatioRule	 ,''),
				isnull(s.UseRatio ,0))
when not matched by source then 
	delete;	



------FreightCollectByCustomer---------------

MERGE Production.dbo.FreightCollectByCustomer AS t
USING
(
    SELECT a.*
    FROM Trade_To_Pms.dbo.FreightCollectByCustomer AS a
) AS s
ON t.Dest = s.Dest
   AND t.BrandID = s.BrandID
   AND t.CarrierID = s.CarrierID
   AND t.Account = s.Account
    WHEN MATCHED
    THEN UPDATE SET 
                    t.CustCDID = ISNULL(s.CustCDID, ''), 
                    t.DestPort = ISNULL(s.DestPort, ''), 
                    t.OrderTypeID = ISNULL(s.OrderTypeID, ''), 
                    t.Remarks = ISNULL(s.Remarks, ''), 
                    t.AddDate = s.AddDate, 
                    t.AddName = ISNULL(s.AddName, ''), 
                    t.EditDate = s.EditDate, 
                    t.EditName = ISNULL(s.EditName, '')
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT(BrandID, 
             Dest, 
             CarrierID, 
             Account, 
             CustCDID, 
             DestPort, 
             OrderTypeID, 
             Remarks, 
             AddDate, 
             AddName, 
             EditDate, 
             EditName)
      VALUES
(ISNULL(s.BrandID,    ''), 
 ISNULL(s.Dest, 	  ''), 
 ISNULL(s.CarrierID,  ''), 
 ISNULL(s.Account, 	  ''), 
 ISNULL(s.CustCDID,   ''), 
 ISNULL(s.DestPort,   ''), 
 ISNULL(s.OrderTypeID,''), 
 ISNULL(s.Remarks, 	  ''), 
 s.AddDate, 	  
 ISNULL(s.AddName, 	  ''), 
 s.EditDate,   
 ISNULL(s.EditName	, '')
)
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;

------[Carrier_Detail_Freight]---------------

MERGE Production.dbo.[Carrier_Detail_Freight] AS t
USING Trade_To_Pms.dbo.[Carrier_Detail_Freight] AS s
ON t.[ID] = s.[ID]
   AND t.[Ukey] = s.[Ukey]
    WHEN MATCHED
    THEN UPDATE SET 
                    t.[Payer] = ISNULL(s.[Payer], ''), 
                    t.[FromTag] = ISNULL(s.[FromTag], ''), 
                    t.[FromInclude] = ISNULL(s.[FromInclude], ''), 
                    t.[FromExclude] = ISNULL(s.[FromExclude], ''), 
                    t.[ToTag] = ISNULL(s.[ToTag], ''), 
                    t.[ToInclude] = ISNULL(s.[ToInclude], ''), 
                    t.[ToExclude] = ISNULL(s.[ToExclude], ''), 
                    t.[ToFty] = ISNULL(s.[ToFty], ''), 
                    t.[AddName] = ISNULL(s.[AddName], ''), 
                    t.[AddDate] = s.[AddDate], 
                    t.[EditName] = ISNULL(s.[EditName], ''), 
                    t.[EditDate] = s.[EditDate]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT([ID], 
             [Payer], 
             [FromTag], 
             [FromInclude], 
             [FromExclude], 
             [ToTag], 
             [ToInclude], 
             [ToExclude], 
             [ToFty], 
             [AddName], 
             [AddDate], 
             [EditName], 
             [EditDate])
      VALUES
(ISNULL(s.[ID],         ''), 
 ISNULL(s.[Payer], 		''), 
 ISNULL(s.[FromTag], 	''), 
 ISNULL(s.[FromInclude],''),  
 ISNULL(s.[FromExclude],''),  
 ISNULL(s.[ToTag], 		''), 
 ISNULL(s.[ToInclude], 	''), 
 ISNULL(s.[ToExclude], 	''), 
 ISNULL(s.[ToFty], 		''), 
 ISNULL(s.[AddName], 	''), 
 s.[AddDate], 	
 ISNULL(s.[EditName], 	''), 
 s.[EditDate]	
)
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


-------FactoryExpress_SendingSchedule
UPDATE a
SET 
a.[Seq]			= isnull(b.[Seq],       0), 
a.[Country]		= isnull(b.[Country],	''), 
a.[RegionCode]	= isnull(b.[RegionCode],''), 
a.[ToID]		= isnull(b.[ToID],		''), 
a.[ToAlias]		= isnull(b.[ToAlias],	''), 
a.[BeginDate]	= b.[BeginDate],	
a.[SUN]			= isnull(b.[SUN],		0), 
a.[MON]			= isnull(b.[MON],		0), 
a.[TUE]			= isnull(b.[TUE],		0), 
a.[WED]			= isnull(b.[WED],		0), 
a.[THU]			= isnull(b.[THU],		0), 
a.[FRI]			= isnull(b.[FRI],		0), 
a.[SAT]			= isnull(b.[SAT],		0), 
a.[Junk]		= isnull(b.[Junk],		0), 
a.[AddName]		= isnull(b.[AddName],	''), 
a.[AddDate]		= b.[AddDate],	 
a.[EditName]	= isnull(b.[EditName],	''), 
a.[EditDate]	= b.[EditDate]	
from Production.dbo.FactoryExpress_SendingSchedule as a 
inner join Trade_To_Pms.dbo.FactoryExpress_SendingSchedule as b 
	ON a.RegionCode=b.RegionCode and a.ToID = b.ToID
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FactoryExpress_SendingSchedule
 (
	   [Seq]
      ,[Country]
      ,[RegionCode]
      ,[ToID]
      ,[ToAlias]
      ,[BeginDate]
      ,[SUN]
      ,[MON]
      ,[TUE]
      ,[WED]
      ,[THU]
      ,[FRI]
      ,[SAT]
      ,[Junk]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
)
SELECT isnull([Seq]       ,0)
      ,isnull([Country]	  ,'')
      ,isnull([RegionCode],'')
      ,isnull([ToID]	  ,'')
      ,isnull([ToAlias]	  ,'')
      ,[BeginDate]
      ,isnull([SUN]		  ,0)
      ,isnull([MON]		  ,0)
      ,isnull([TUE]		  ,0)
      ,isnull([WED]		  ,0)
      ,isnull([THU]		  ,0)
      ,isnull([FRI]		  ,0)
      ,isnull([SAT]		  ,0)
      ,isnull([Junk]	  ,0)
      ,isnull([AddName]	  ,'')
      ,[AddDate]	  
      ,isnull([EditName]  ,'')
      ,[EditDate]
from Trade_To_Pms.dbo.FactoryExpress_SendingSchedule as b WITH (NOLOCK)
where not exists(
	select 1 
	from Production.dbo.FactoryExpress_SendingSchedule as a WITH (NOLOCK) 
	where a.RegionCode=b.RegionCode 
	and a.ToID = b.ToID 
)

----------------------刪除主TABLE多的資料
Delete Production.dbo.FactoryExpress_SendingSchedule
from Production.dbo.FactoryExpress_SendingSchedule as a 
left join Trade_To_Pms.dbo.FactoryExpress_SendingSchedule as b
on a.RegionCode=b.RegionCode and a.ToID = b.ToID
where b.ToID is null


-------FactoryExpress_SendingScheduleHistory
UPDATE a
SET 
a.[Ukey]		= isnull(b.[Ukey],      0), 
a.[Country]		= isnull(b.[Country],	''), 
a.[RegionCode]	= isnull(b.[RegionCode],''), 
a.[ToID]		= isnull(b.[ToID],		''), 
a.[ToAlias]		= isnull(b.[ToAlias],	''), 
a.[BeginDate]	= b.[BeginDate],	
a.[EndDate]	= b.[EndDate],	
a.[SUN]			= isnull(b.[SUN],		0), 
a.[MON]			= isnull(b.[MON],		0), 
a.[TUE]			= isnull(b.[TUE],		0), 
a.[WED]			= isnull(b.[WED],		0), 
a.[THU]			= isnull(b.[THU],		0), 
a.[FRI]			= isnull(b.[FRI],		0), 
a.[SAT]			= isnull(b.[SAT],		0), 
a.[AddName]		= isnull(b.[AddName],	''), 
a.[AddDate]		= b.[AddDate],	
a.[EditName]	= isnull(b.[EditName],	''), 
a.[EditDate]	= b.[EditDate]	
from Production.dbo.FactoryExpress_SendingScheduleHistory as a 
inner join Trade_To_Pms.dbo.FactoryExpress_SendingScheduleHistory as b 
	ON a.Ukey=b.Ukey
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.FactoryExpress_SendingScheduleHistory
 (
	   [Ukey]
      ,[Country]
      ,[RegionCode]
      ,[ToID]
      ,[ToAlias]
      ,[BeginDate]
      ,[SUN]
      ,[MON]
      ,[TUE]
      ,[WED]
      ,[THU]
      ,[FRI]
      ,[SAT]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
)
SELECT isnull([Ukey]       ,	0)
      ,isnull([Country]	   ,	'')
      ,isnull([RegionCode] ,	'')
      ,isnull([ToID]	   ,	'')
      ,isnull([ToAlias]	   ,	'')
      ,[BeginDate]  
      ,isnull([SUN]		   ,	0)
      ,isnull([MON]		   ,	0)
      ,isnull([TUE]		   ,	0)
      ,isnull([WED]		   ,	0)
      ,isnull([THU]		   ,	0)
      ,isnull([FRI]		   ,	0)
      ,isnull([SAT]        ,	0)
      ,isnull([AddName]	   ,	'')
      ,[AddDate]	   
      ,isnull([EditName]   ,	'')
      ,[EditDate]
from Trade_To_Pms.dbo.FactoryExpress_SendingScheduleHistory as b WITH (NOLOCK)
where not exists(
	select 1 
	from Production.dbo.FactoryExpress_SendingScheduleHistory as a WITH (NOLOCK) 
	where a.Ukey=b.Ukey 
)


----------------------刪除主TABLE多的資料
Delete Production.dbo.FactoryExpress_SendingScheduleHistory
from Production.dbo.FactoryExpress_SendingScheduleHistory as a 
left join Trade_To_Pms.dbo.FactoryExpress_SendingScheduleHistory as b
on a.Ukey=b.Ukey 
where b.ToID is null



-------FIR_Grade-------
UPDATE a
SET  a.WeaveTypeID	= isnull( b.WeaveTypeID,	'')
    ,a.Percentage	= isnull( b.Percentage ,	0)
    ,a.Grade		= isnull( b.Grade	   ,	'')
    ,a.Result		= isnull( b.Result	   ,	'')
    ,a.BrandID		= isnull( b.BrandID	   ,	'')
    ,a.InspectionGroup = isnull(b.InspectionGroup, '')
    ,a.isFormatInP01 = isnull(b.isFormatInP01, 0)
    ,a.isResultNotInP01 = isnull(b.isResultNotInP01, 0)
    ,a.Description = isnull(b.Description, '')
    ,a.ShowGrade = isnull(b.ShowGrade, '')
FROM Production.dbo.FIR_Grade a 
INNER JOIN Trade_To_Pms.dbo.FIR_Grade as b  
ON a.WeaveTypeID=b.WeaveTypeID AND a.Percentage=b.Percentage AND a.BrandID=b.BrandID and a.InspectionGroup = b.InspectionGroup


INSERT INTO Production.dbo.FIR_Grade
           (WeaveTypeID
           ,Percentage
           ,Grade
           ,Result
           ,BrandID
           ,InspectionGroup
           ,isFormatInP01
           ,isResultNotInP01
           ,Description
           ,ShowGrade)
SELECT   isnull( WeaveTypeID, '')
		,isnull( Percentage	, 0)
		,isnull( Grade		, '')
		,isnull( Result		, '')
		,isnull( BrandID	, '')
        ,isnull(InspectionGroup, '')
        ,isnull(isFormatInP01, 0)
        ,isnull(isResultNotInP01, 0)
        ,isnull(Description, '')
        ,isnull(ShowGrade, '')
FROM Trade_To_Pms.dbo.FIR_Grade b
WHERE NOT EXISTS(
	SELECT  1
	FROM Production.dbo.FIR_Grade a WITH (NOLOCK)
	WHERE a.WeaveTypeID=b.WeaveTypeID AND a.Percentage=b.Percentage AND a.BrandID=b.BrandID and a.InspectionGroup = b.InspectionGroup
)

DELETE Production.dbo.FIR_Grade
FROM Production.dbo.FIR_Grade a
LEFT JOIN Trade_To_Pms.dbo.FIR_Grade b ON a.WeaveTypeID=b.WeaveTypeID AND a.Percentage=b.Percentage AND a.BrandID=b.BrandID and a.InspectionGroup = b.InspectionGroup
WHERE b.Grade is null


-----FtyStdRate_PRT-----
update tar set
 tar.Length    = isnull( S.Length    , 0)
,tar.Width	   = isnull( S.Width	 , 0)
,tar.Surcharge = isnull( S.Surcharge , 0)
,tar.Price	   = isnull( S.Price	 , 0)
,tar.Ratio	   = isnull( S.Ratio	 , 0)
,tar.SEQ	   = isnull( S.SEQ	   	 , '')
,tar.AddName   = isnull( S.AddName   , '')
,tar.AddDate   =  S.AddDate 
,tar.EditName  = isnull( S.EditName  , '')
,tar.EditDate  = S.EditDate  
from Trade_To_Pms.dbo.FtyStdRate_PRT S
inner join Production.dbo.FtyStdRate_PRT tar 
	on tar.Region = S.Region
	and tar.SeasonID = S.SeasonID
	and tar.InkType = S.InkType
	and tar.Colors = S.Colors
	and tar.Area = S.Area

INSERT INTO FtyStdRate_PRT(
	Region
	,SeasonID
	,InkType
	,Colors
	,Area
	,Length
	,Width
	,Surcharge
	,Price
	,Ratio
	,SEQ
	,AddName
	,AddDate
	,EditName
	,EditDate
)
SELECT 
	 isnull(Region   , '')
	,isnull(SeasonID , '')
	,isnull(InkType	 , '')
	,isnull(Colors	 , '')
	,isnull(Area	 , 0)
	,isnull(Length	 , 0)
	,isnull(Width	 , 0)
	,isnull(Surcharge, 0)
	,isnull(Price	 , 0)
	,isnull(Ratio	 , 0)
	,isnull(SEQ		 , '')
	,isnull(AddName	 , '')
	,AddDate	 
	,isnull(EditName , '')
	,EditDate
from Trade_To_Pms.dbo.FtyStdRate_PRT S
where not exists(
	select 1
	from Production.dbo.FtyStdRate_PRT tar
	where tar.Region = S.Region
	and tar.SeasonID = S.SeasonID
	and tar.InkType = S.InkType
	and tar.Colors = S.Colors
	and tar.Area = S.Area)

DELETE S
from Production.dbo.FtyStdRate_PRT S
where not exists(
	select 1
	from Trade_To_Pms.dbo.FtyStdRate_PRT tar
	where tar.Region = S.Region
	and tar.SeasonID = S.SeasonID
	and tar.InkType = S.InkType
	and tar.Colors = S.Colors
	and tar.Area = S.Area)

-----FtyStdRate_EMB-----
update tar set
 tar.EndRange      = isnull( S.EndRange     , 0)
,tar.BasedStitches = isnull( S.BasedStitches, 0)
,tar.BasedPay	   = isnull( S.BasedPay		, 0)
,tar.AddedStitches = isnull( S.AddedStitches, 0)
,tar.AddedPay	   = isnull( S.AddedPay		, 0)
,tar.ThreadRatio   = isnull( S.ThreadRatio	, 0)
,tar.Ratio		   = isnull( S.Ratio		, 0)
,tar.AddName	   = isnull( S.AddName		, '')
,tar.AddDate	   =  S.AddDate		
,tar.EditName	   = isnull( S.EditName		, '')
,tar.EditDate	   =  S.EditDate		
from Trade_To_Pms.dbo.FtyStdRate_EMB S
inner join Production.dbo.FtyStdRate_EMB tar 
	on tar.Region = S.Region
	and tar.SeasonID = S.SeasonID
	and tar.StartRange = S.StartRange

INSERT INTO FtyStdRate_EMB(
	 Region
	,SeasonID
	,StartRange
	,EndRange
	,BasedStitches
	,BasedPay
	,AddedStitches
	,AddedPay
	,ThreadRatio
	,Ratio
	,AddName
	,AddDate
	,EditName
	,EditDate
)
SELECT 
	 isnull(Region       , '')
	,isnull(SeasonID	 , '')
	,isnull(StartRange	 , 0)
	,isnull(EndRange	 , 0)
	,isnull(BasedStitches, 0)
	,isnull(BasedPay	 , 0)
	,isnull(AddedStitches, 0)
	,isnull(AddedPay	 , 0)
	,isnull(ThreadRatio	 , 0)
	,isnull(Ratio		 , 0)
	,isnull(AddName		 , '')
	,AddDate		
	,isnull(EditName	 , '')
	,EditDate
from Trade_To_Pms.dbo.FtyStdRate_EMB S
where not exists(
	select 1
	from Production.dbo.FtyStdRate_EMB tar
	where tar.Region = S.Region
	and tar.SeasonID = S.SeasonID
	and tar.StartRange = S.StartRange)

DELETE S
from Production.dbo.FtyStdRate_EMB S
where not exists(
	select 1
	from Trade_To_Pms.dbo.FtyStdRate_EMB tar
	where tar.Region = S.Region
	and tar.SeasonID = S.SeasonID
	and tar.StartRange = S.StartRange)

	
--------AccountNoSetting---------------

MERGE Production.dbo.AccountNoSetting AS t
USING Trade_To_Pms.dbo.AccountNoSetting AS s
ON t.ID = s.ID
    WHEN MATCHED
    THEN UPDATE SET 
                    t.UnselectableShipB03 = ISNULL(s.UnselectableShipB03, 0), 
					t.NeedShareExpense  = ISNULL(s.NeedShareExpense  ,0),
                    t.AddDate = s.AddDate, 
                    t.AddName = ISNULL(s.AddName, ''), 
                    t.EditDate = s.EditDate, 
                    t.EditName = ISNULL(s.EditName, '')
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT(ID, 
             UnselectableShipB03, 
             NeedShareExpense, 
             AddDate, 
             AddName, 
             EditDate, 
             EditName)
      VALUES
(ISNULL(s.ID,                 ''),
 ISNULL(s.UnselectableShipB03, 0),
 ISNULL(s.NeedShareExpense, 0),
 s.AddDate, 			  
 ISNULL(s.AddName, 			   ''),
 s.EditDate, 		  
 ISNULL(s.EditName			  , '')
)
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


--SubProDefectCode 
update tar set
 tar.Junk          = isnull( S.Junk       , 0)
,tar.Description   = isnull( S.Description, '')
,tar.AddName	   = isnull( S.AddName	  , '')
,tar.AddDate	   =  S.AddDate
,tar.EditName	   = isnull( S.EditName	  , '')
,tar.EditDate	   = S.EditDate
from Trade_To_Pms.dbo.SubProDefectCode S
inner join Production.dbo.SubProDefectCode tar 
	on tar.SubProcessID	 = S.SubProcessID	
	and tar.DefectCode	 = S.DefectCode	
	
INSERT INTO SubProDefectCode
(
	[SubProcessID]
	,[DefectCode]
	,[Junk]
	,[Description]
	,[AddDate]
	,[AddName]
	,[EditDate]
	,[Editname]
)
SELECT
	 isnull([SubProcessID], '')
	,isnull([DefectCode]  , '')
	,isnull([Junk]		  , 0)
	,isnull([Description] , '')
	,[AddDate]	 
	,isnull([AddName]	  , '')
	,[EditDate]	  
	,isnull([Editname]	  , '')
from Trade_To_Pms.dbo.SubProDefectCode S
where not exists(
	select 1
	from Production.dbo.SubProDefectCode tar
	where tar.SubProcessID	 = S.SubProcessID	
	and tar.DefectCode	 = S.DefectCode	)
	
DELETE S
from Production.dbo.SubProDefectCode S
where not exists(
	select 1
	from Trade_To_Pms.dbo.SubProDefectCode tar
	where tar.SubProcessID	 = S.SubProcessID	
	and tar.DefectCode	 = S.DefectCode	)

-----PortByBrandShipmode -----
update t SET
	   t.Remark = isnull( s.Remark        , '')
      ,t.Junk = isnull( s.Junk			  , 0)
      ,t.AddDate =  s.AddDate	 
      ,t.AddName = isnull( s.AddName	  , '')
      ,t.EditDate =  s.EditDate	  
      ,t.EditName = isnull( s.EditName	  , '')
from Trade_To_Pms.dbo.PortByBrandShipmode  s
inner join Production.dbo.PortByBrandShipmode t on t.PulloutPortID = S.PulloutPortID and t.BrandID = S.BrandID


INSERT INTO Production.dbo.PortByBrandShipmode
           (PulloutPortID
           ,BrandID
           ,Remark
           ,Junk
           ,AddDate
           ,AddName
           ,EditDate
           ,EditName)
SELECT isnull(PulloutPortID , '')
      ,isnull(BrandID	    , '')
      ,isnull(Remark	    , '')
      ,isnull(Junk		    , 0)
      ,AddDate	   
      ,isnull(AddName	    , '')
      ,EditDate	   
      ,isnull(EditName	    , '')
FROM Trade_To_Pms.dbo.PortByBrandShipmode  s
WHERE NOT EXISTS(
	SELECT 1
	FROM Production.dbo.PortByBrandShipmode t
	WHERE t.PulloutPortID = S.PulloutPortID and t.BrandID = S.BrandID
)

DELETE s
from Production.dbo.PortByBrandShipmode s
WHERE NOT EXISTS(
	SELECT 1
	FROM Trade_To_Pms.dbo.PortByBrandShipmode t
	WHERE t.PulloutPortID = S.PulloutPortID and t.BrandID = S.BrandID
)

-- 全轉入PulloutPort
UPDATE t
SET  
t.Name = isnull(			   s.Name,              ''),
t.InternationalCode = isnull(  s.InternationalCode,	''),
t.CountryID = isnull(		   s.CountryID,			''),
t.AirPort = isnull(			   s.AirPort,			0),
t.SeaPort = isnull(			   s.SeaPort,			0),
t.Remark = isnull(			   s.Remark,			''),
t.AddName = isnull(			   s.AddName,			''),
t.AddDate = 			   s.AddDate,		
t.EditName = isnull(		   s.EditName,			''),
t.EditDate = 		   s.EditDate,			
t.Junk = isnull(			   s.Junk				,0)
FROM Production.dbo.PulloutPort t
INNER JOIN Trade_To_Pms.dbo.PulloutPort as s 
ON t.id	=s.id


INSERT INTO Production.dbo.PulloutPort(
	   [ID]
      ,[Name]
      ,[InternationalCode]
      ,[CountryID]
      ,[AirPort]
      ,[SeaPort]
      ,[Remark]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[Junk])
SELECT isnull([ID]                ,'')
      ,isnull([Name]			  ,'')
      ,isnull([InternationalCode] ,'')
      ,isnull([CountryID]		  ,'')
      ,isnull([AirPort]			  ,0)
      ,isnull([SeaPort]			  ,0)
      ,isnull([Remark]			  ,'')
      ,isnull([AddName]			  ,'')
      ,[AddDate]		
      ,isnull([EditName]		  ,'')
      ,[EditDate]	
      ,isnull([Junk]			  ,0)
FROM Trade_To_Pms.dbo.PulloutPort b
WHERE NOT EXISTS(
	SELECT  1
	FROM Production.dbo.PulloutPort a WITH (NOLOCK)
	WHERE a.id=b.id
)

DELETE Production.dbo.PulloutPort
FROM Production.dbo.PulloutPort a
LEFT JOIN Trade_To_Pms.dbo.PulloutPort b ON a.id=b.id 
WHERE b.id is null


-- 全轉入Consignee
UPDATE t
SET  
t.Junk = isnull(			   s.Junk,0),
t.AddName = isnull(			   s.AddName,''),
t.AddDate = 			   s.AddDate,
t.EditName = isnull(		   s.EditName,''),
t.EditDate = 		   s.EditDate
FROM Production.dbo.Consignee t
INNER JOIN Trade_To_Pms.dbo.Consignee as s ON t.id	=s.id

INSERT INTO Production.dbo.Consignee(
	   [ID]
      ,[Junk]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate])
SELECT isnull([ID],'')
      ,isnull([Junk],0)
      ,isnull([AddName],'')
      ,[AddDate]
      ,isnull([EditName],'')
      ,[EditDate]
FROM Trade_To_Pms.dbo.Consignee b
WHERE NOT EXISTS(
	SELECT  1
	FROM Production.dbo.Consignee a WITH (NOLOCK)
	WHERE a.id = b.id
)

DELETE Production.dbo.Consignee
FROM Production.dbo.Consignee a
LEFT JOIN Trade_To_Pms.dbo.Consignee b ON a.id = b.id 
WHERE b.id is null

-- 全轉入Consignee_Detail
UPDATE t
SET  
t.ID =			   isnull(s.ID,''),
t.Email =		   isnull(s.Email,'')
FROM Production.dbo.Consignee_Detail t
INNER JOIN Trade_To_Pms.dbo.Consignee_Detail as s ON t.Ukey = s.Ukey

INSERT INTO Production.dbo.Consignee_Detail(
	   [Ukey]
	  ,[ID]
      ,[Email])
SELECT isnull([Ukey],0)
      ,isnull([ID],'')
      ,isnull([Email],'')
FROM Trade_To_Pms.dbo.Consignee_Detail b
WHERE NOT EXISTS(
	SELECT  1
	FROM Production.dbo.Consignee_Detail a WITH (NOLOCK)
	WHERE a.id = b.id
)

DELETE Production.dbo.Consignee_Detail
FROM Production.dbo.Consignee_Detail a
LEFT JOIN Trade_To_Pms.dbo.Consignee_Detail b ON a.Ukey = b.Ukey 
WHERE b.Ukey is null


--------HealthLabelSupp_FtyExpiration
DELETE Production.dbo.HealthLabelSupp_FtyExpiration
FROM Production.dbo.HealthLabelSupp_FtyExpiration a
LEFT JOIN Trade_To_Pms.dbo.HealthLabelSupp_FtyExpiration b ON a.id = b.id and a.FactoryID = b.FactoryID
WHERE b.id is null

UPDATE a
SET  
     a.[Registry]		= isnull(b.[Registry]      ,'')
    ,a.[Expiration]		= b.[Expiration]
    ,a.[AddName]		= isnull(b.[AddName]       ,'')
    ,a.[AddDate]		= b.[AddDate]     
    ,a.[EditName]		= isnull(b.[EditName]      ,'')
    ,a.[EditDate]		= b.[EditDate]
    ,a.[ApplyExtension]	= isnull(b.[ApplyExtension],'')
    ,a.[Remark]			= isnull(b.[Remark]        ,'')
FROM Production.dbo.HealthLabelSupp_FtyExpiration a
INNER JOIN Trade_To_Pms.dbo.HealthLabelSupp_FtyExpiration b ON a.id = b.id and a.FactoryID = b.FactoryID

INSERT INTO [dbo].[HealthLabelSupp_FtyExpiration]
           ([ID]
           ,[FactoryID]
           ,[Registry]
           ,[Expiration]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate]
           ,[ApplyExtension]
           ,[Remark])
select 
	  isnull(a.[ID]             ,'')
	, isnull(a.[FactoryID]      ,'')
	, isnull(a.[Registry]       ,'')
	, a.[Expiration]  
	, isnull(a.[AddName]        ,'')
	, a.[AddDate]
	, isnull(a.[EditName]       ,'')
	, a.[EditDate]       
	, isnull(a.[ApplyExtension] ,'')
	, isnull(a.[Remark]         ,'')
FROM Trade_To_Pms.dbo.HealthLabelSupp_FtyExpiration a
LEFT JOIN Production.dbo.HealthLabelSupp_FtyExpiration b ON a.id = b.id and a.FactoryID = b.FactoryID
WHERE b.id is null


---------ChgOverTarget 

DELETE Production.dbo.ChgOverTarget
FROM Production.dbo.ChgOverTarget a
LEFT JOIN Trade_To_Pms.dbo.ChgOverTarget b ON a.EffectiveDate = b.EffectiveDate and a.MDivisionID = b.MDivisionID and a.Type = b.Type
	and exists(select 1 from Production.dbo.MDivision where ID = b.MDivisionID)
WHERE b.EffectiveDate is null


UPDATE a
SET
	 a.[EffectiveDate] = b.[EffectiveDate]
	,a.[MDivisionID]   = isnull(b.[MDivisionID]  ,'')
	,a.[Type]		   = isnull(b.[Type]         ,'')
	,a.[Target]		   = isnull(b.[Target]       ,0)
	,a.[AddName]	   = isnull(b.[AddName]      ,'')
	,a.[AddDate]	   = b.[AddDate]   
	,a.[EditName]	   = isnull(b.[EditName]     ,'')
	,a.[EditDate]	   = b.[EditDate] 
FROM Production.dbo.ChgOverTarget a
INNER JOIN Trade_To_Pms.dbo.ChgOverTarget b ON a.EffectiveDate = b.EffectiveDate and a.MDivisionID = b.MDivisionID and a.Type = b.Type



INSERT INTO [dbo].[ChgOverTarget]
           ([EffectiveDate]
           ,[MDivisionID]
           ,[Type]
           ,[Target]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select 
	 a.[EffectiveDate]
    ,isnull(a.[MDivisionID],'')
    ,isnull(a.[Type]       ,'')
    ,isnull(a.[Target]     ,0)
    ,isnull(a.[AddName]    ,'')
    ,a.[AddDate]  
    ,isnull(a.[EditName]   ,'')
    ,a.[EditDate]
FROM Trade_To_Pms.dbo.ChgOverTarget a
LEFT JOIN Production.dbo.ChgOverTarget b ON a.EffectiveDate = b.EffectiveDate and a.MDivisionID = b.MDivisionID and a.Type = b.Type
WHERE b.EffectiveDate is null
and exists(select 1 from Production.dbo.MDivision where ID = a.MDivisionID)

---NewCDCode 

DELETE Production.dbo.NewCDCode 
FROM Production.dbo.NewCDCode  a
LEFT JOIN Trade_To_Pms.dbo.NewCDCode  b ON a.[Classifty] = b.[Classifty] and a.[ID] = b.[ID]
WHERE b.[Classifty] is null

UPDATE a
SET
	 [TypeName]		= isnull(b.[TypeName]  ,'')
	,[Placket]		= isnull(b.[Placket]   ,'')
	,[Definition]	= isnull(b.[Definition],'')
	,[CPU]			= isnull(b.[CPU]       ,0)
	,[ComboPcs]		= isnull(b.[ComboPcs]  ,0)
	,[Remark]		= isnull(b.[Remark]    ,'')
	,[Junk]			= isnull(b.[Junk]      ,0)
	,[AddName]		= isnull(b.[AddName]   ,'')
	,[AddDate]		= b.[AddDate]
	,[EditName]		= isnull(b.[EditName]  ,'')
	,[EditDate]		= b.[EditDate]
from Production.dbo.NewCDCode as a inner join Trade_To_Pms.dbo.NewCDCode as b ON a.[Classifty] = b.[Classifty] and a.[ID] = b.[ID]
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.NewCDCode
           ([Classifty]
           ,[TypeName]
           ,[ID]
           ,[Placket]
           ,[Definition]
           ,[CPU]
           ,[ComboPcs]
           ,[Remark]
           ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
SELECT      isnull([Classifty]  ,'')
           ,isnull([TypeName]   ,'')
           ,isnull([ID]         ,'')
           ,isnull([Placket]    ,'')
           ,isnull([Definition] ,'')
           ,isnull([CPU]        ,0)
           ,isnull([ComboPcs]   ,0)
           ,isnull([Remark]     ,'')
           ,isnull([Junk]       ,0)
           ,isnull([AddName]    ,'')
           ,[AddDate]  
           ,isnull([EditName]   ,'')
           ,[EditDate]
from Trade_To_Pms.dbo.NewCDCode as b WITH (NOLOCK)
where not exists(select id from Production.dbo.NewCDCode as a WITH (NOLOCK) where a.[Classifty] = b.[Classifty] and a.[ID] = b.[ID])

-------MtlType   2   QAMtlTypeSetting

update a
set junk = 1
FROM Production.dbo.[QAMtlTypeSetting] a
LEFT JOIN Trade_To_Pms.dbo.MtlType b ON a.id = b.id
WHERE b.id is null

UPDATE a
SET
	a.[FullName]	= isnull(b.[FullName],'')
	,a.[Type]		= isnull(b.[Type],'')
	,a.[Junk]		= isnull(b.[Junk],0)
FROM Production.dbo.[QAMtlTypeSetting] a
INNER JOIN Trade_To_Pms.dbo.MtlType b ON a.id = b.id

INSERT INTO [dbo].[QAMtlTypeSetting]
           ([ID]
           ,[FullName]
           ,[Type]
           ,[Junk])
select
		 isnull(a.[ID]       ,'')
		,isnull(a.[FullName] ,'')
		,isnull(a.[Type]     ,'')
		,isnull(a.[Junk]     ,0)
FROM Trade_To_Pms.dbo.MtlType a
LEFT JOIN Production.dbo.[QAMtlTypeSetting] b ON a.id = b.id
where b.id is null

---------FinanceRate
Delete Production.dbo.FinanceRate
from Production.dbo.FinanceRate as a 
left join Trade_To_Pms.dbo.FinanceRate as b on a.RateTypeID = b.RateTypeID
                                     and a.BeginDate = b.BeginDate
                                     and a.OriginalCurrency = b.OriginalCurrency
                                     and a.ExchangeCurrency = b.ExchangeCurrency
where b.RateTypeID is null

UPDATE a
SET   a.EndDate		      =b.EndDate
      ,a.Rate		      =isnull(b.Rate,0)
      ,a.Remark		      =isnull(b.Remark,'')
      ,a.AddName		      =isnull(b.AddName,'')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName,'')
      ,a.EditDate		      =b.EditDate

from Production.dbo.FinanceRate as a 
left join Trade_To_Pms.dbo.FinanceRate as b on a.RateTypeID = b.RateTypeID
                                     and a.BeginDate = b.BeginDate
                                     and a.OriginalCurrency = b.OriginalCurrency
                                     and a.ExchangeCurrency = b.ExchangeCurrency
where a.EndDate <> b.EndDate
or a.Rate <> b.Rate
or a.Remark <> b.Remark

INSERT INTO Production.dbo.FinanceRate(
       RateTypeID
      ,BeginDate
      ,EndDate
      ,OriginalCurrency
      ,ExchangeCurrency
      ,Rate
      ,Remark
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       isnull(RateTypeID       ,'')
      ,BeginDate     
      ,EndDate        
      ,isnull(OriginalCurrency ,'')
      ,isnull(ExchangeCurrency ,'')
      ,isnull(Rate             ,0)
      ,isnull(Remark           ,'')
      ,isnull(AddName          ,'')
      ,AddDate      
      ,isnull(EditName         ,'')
      ,EditDate        
from Trade_To_Pms.dbo.FinanceRate as b WITH (NOLOCK)
where not exists(
    select 1 
    from Production.dbo.FinanceRate as a WITH (NOLOCK) 
    where a.RateTypeID = b.RateTypeID
    and a.BeginDate = b.BeginDate
    and a.OriginalCurrency = b.OriginalCurrency
    and a.ExchangeCurrency = b.ExchangeCurrency)


---------FinanceTWRate
Delete Production.dbo.FinanceTWRate
from Production.dbo.FinanceTWRate as a 
left join Trade_To_Pms.dbo.FinanceTWRate as b on a.RateTypeID = b.RateTypeID
                                     and a.BeginDate = b.BeginDate
                                     and a.OriginalCurrency = b.OriginalCurrency
                                     and a.ExchangeCurrency = b.ExchangeCurrency
where b.RateTypeID is null

UPDATE a
SET   a.EndDate		      =b.EndDate
      ,a.Rate		      =isnull(b.Rate,0)
      ,a.AddName		      =isnull(b.AddName,'')
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =isnull(b.EditName,'')
      ,a.EditDate		      =b.EditDate

from Production.dbo.FinanceTWRate as a 
left join Trade_To_Pms.dbo.FinanceTWRate as b on a.RateTypeID = b.RateTypeID
                                     and a.BeginDate = b.BeginDate
                                     and a.OriginalCurrency = b.OriginalCurrency
                                     and a.ExchangeCurrency = b.ExchangeCurrency
where a.EndDate <> b.EndDate
or a.Rate <> b.Rate

INSERT INTO Production.dbo.FinanceTWRate(
       RateTypeID
      ,BeginDate
      ,EndDate
      ,OriginalCurrency
      ,ExchangeCurrency
      ,Rate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       isnull(RateTypeID        ,'')
      ,BeginDate   
      ,EndDate        
      ,isnull(OriginalCurrency  ,'')
      ,isnull(ExchangeCurrency  ,'')
      ,isnull(Rate              ,0)
      ,isnull(AddName           ,'')
      ,AddDate       
      ,isnull(EditName          ,'')
      ,EditDate
from Trade_To_Pms.dbo.FinanceTWRate as b WITH (NOLOCK)
where not exists(
    select 1 
    from Production.dbo.FinanceTWRate as a WITH (NOLOCK) 
    where a.RateTypeID = b.RateTypeID
    and a.BeginDate = b.BeginDate
    and a.OriginalCurrency = b.OriginalCurrency
    and a.ExchangeCurrency = b.ExchangeCurrency)

---------------GarmentDefectType

update b set
	 b.[ID]			 = isnull(a.[ID]            ,'')
	,b.[Description] = isnull(a.[Description]   ,'')
	,b.[Junk]		 = isnull(a.[Junk]          ,0)
	,b.[AddName]	 = isnull(a.[AddName]       ,'')
	,b.[AddDate]	 = a.[AddDate]      
	,b.[EditName]	 = isnull(a.[EditName]      ,'')
	,b.[EditDate]	 = a.[EditDate]   
	,b.[Seq]		 = isnull(a.[Seq]           ,0)
FROM Trade_To_Pms.dbo.[GarmentDefectType] a
inner JOIN Production.dbo.[GarmentDefectType] b ON a.ID = b.ID

INSERT INTO [dbo].[GarmentDefectType]
           ([ID]
           ,[Description]
           ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate]
           ,[Seq])
select		isnull(a.[ID]           ,'')
           ,isnull(a.[Description]  ,'')
           ,isnull(a.[Junk]         ,0)
           ,isnull(a.[AddName]      ,'')
           ,a.[AddDate]    
           ,isnull(a.[EditName]     ,'')
           ,a.[EditDate]  
           ,isnull(a.[Seq]          ,0)
FROM Trade_To_Pms.dbo.[GarmentDefectType] a
LEFT JOIN Production.dbo.[GarmentDefectType] b ON a.ID = b.ID
WHERE b.ID is null

---------------GarmentDefectCode
update b set
	 b.[ID]						= isnull(a.[ID]                 ,'')
	,b.[Description]			= isnull(a.[Description]        ,'')
	,b.[GarmentDefectTypeID]	= isnull(a.[GarmentDefectTypeID],'')
	,b.[AddName]				= isnull(a.[AddName]            ,'')
	,b.[AddDate]				= a.[AddDate]      
	,b.[EditName]				= isnull(a.[EditName]           ,'')
	,b.[EditDate]				= a.[EditDate]       
	,b.[Junk]					= isnull(a.[Junk]               ,0)
	,b.[Seq]					= isnull(a.[Seq]                ,0)
	,b.[ReworkTotalFailCode]	= isnull(a.[ReworkTotalFailCode],'')
	,b.[IsCFA]					= isnull(a.[IsCFA]              ,0)
	,b.[IsCriticalDefect]		= isnull(a.[IsCriticalDefect]   ,0)
FROM Trade_To_Pms.dbo.[GarmentDefectCode] a
inner JOIN Production.dbo.[GarmentDefectCode] b ON a.ID = b.ID

INSERT INTO [dbo].[GarmentDefectCode]
           ([ID]
           ,[Description]
           ,[GarmentDefectTypeID]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate]
           ,[Junk]
           ,[Seq]
           ,[ReworkTotalFailCode]
           ,[IsCFA]
           ,[IsCriticalDefect])
select
			isnull(a.[ID]                  ,'')
           ,isnull(a.[Description]         ,'')
           ,isnull(a.[GarmentDefectTypeID] ,'')
           ,isnull(a.[AddName]             ,'')
           ,a.[AddDate]
           ,isnull(a.[EditName]            ,'')
           ,a.[EditDate]
           ,isnull(a.[Junk]                ,0)
           ,isnull(a.[Seq]                 ,0)
           ,isnull(a.[ReworkTotalFailCode] ,'')
           ,isnull(a.[IsCFA]               ,0)
           ,isnull(a.[IsCriticalDefect]    ,0)
FROM Trade_To_Pms.dbo.[GarmentDefectCode] a
LEFT JOIN Production.dbo.[GarmentDefectCode] b ON a.ID = b.ID
WHERE b.ID is null

--FabricDefect
update a set	a.Type = isnull(b.Type,''),
				a.DescriptionEN = isnull(b.DescriptionEN,''),
				a.Junk = isnull(b.Junk ,0),
				a.AddName = isnull(b.AddName,''),
				a.AddDate = b.AddDate,
				a.EditName = isnull(b.EditName,''),
				a.EditDate = b.EditDate
FROM Production.dbo.FabricDefect a
INNER JOIN Trade_To_Pms.dbo.FabricDefect b ON a.ID = b.ID

update a set a.Junk = 1
FROM Production.dbo.FabricDefect a
where not exists(select 1 from Trade_To_Pms.dbo.FabricDefect b where a.ID = b.ID)

insert into Production.dbo.FabricDefect(ID, Type, DescriptionEN, Junk, AddName, AddDate, EditName, EditDate)
select	
      isnull(b.ID           ,'')
    , isnull(b.Type         ,'')
    , isnull(b.DescriptionEN,'')
    , isnull(b.Junk         ,0)
    , isnull(b.AddName      ,'')
    , b.AddDate  
    , isnull(b.EditName     ,'')
    , b.EditDate
from Trade_To_Pms.dbo.FabricDefect b
where not exists(select 1 from Production.dbo.FabricDefect a where a.ID = b.ID)

--AccessoryDefect
update a set	a.Description = isnull( b.Description,''),
				a.Junk = isnull( b.Junk,0),
				a.AddName = isnull( b.AddName,''),
				a.AddDate =  b.AddDate,
				a.EditName = isnull( b.EditName,''),
				a.EditDate =  b.EditDate
FROM Production.dbo.AccessoryDefect a
INNER JOIN Trade_To_Pms.dbo.AccessoryDefect b ON a.ID = b.ID

update a set a.Junk = 1
FROM Production.dbo.AccessoryDefect a
where not exists(select 1 from Trade_To_Pms.dbo.AccessoryDefect b where a.ID = b.ID)

insert into Production.dbo.AccessoryDefect(ID, Description, Junk, AddName, AddDate, EditName, EditDate)
select	
      isnull(b.ID           ,'')
    , isnull(b.Description  ,'')
    , isnull(b.Junk         ,0)
    , isnull(b.AddName      ,'')
    , b.AddDate
    , isnull(b.EditName     ,'')
    , b.EditDate
from Trade_To_Pms.dbo.AccessoryDefect b
where not exists(select 1 from Production.dbo.AccessoryDefect a where a.ID = b.ID)

---Adidas_FGWT
delete a
from production.dbo.Adidas_FGWT a
left join Trade_To_Pms.dbo.Adidas_FGWT b on 
		a.[Location] 		  = b.[Location] 
	and a.[ReportType] 		  = b.[ReportType] 
	and a.[MtlTypeID] 		  = b.[MtlTypeID] 
	and a.[Washing] 		  = b.[Washing] 
	and a.[FabricComposition] = b.[FabricComposition]
where b.[Location] is null

update a set
	 Seq	    = isnull( b.Seq		  ,0)
	,TestName   = isnull( b.TestName  ,'')
	,SystemType = isnull( b.SystemType,'')
	,TestDetail = isnull( b.TestDetail,'')
	,Scale	    = isnull( b.Scale	  ,'')
	,Criteria   = isnull( b.Criteria  ,0)
	,Criteria2  = isnull( b.Criteria2 ,0)
from production.dbo.Adidas_FGWT a
inner join Trade_To_Pms.dbo.Adidas_FGWT b on 
		a.[Location] 		  = b.[Location] 
	and a.[ReportType] 		  = b.[ReportType] 
	and a.[MtlTypeID] 		  = b.[MtlTypeID] 
	and a.[Washing] 		  = b.[Washing] 
	and a.[FabricComposition] = b.[FabricComposition]
	
insert into production.dbo.Adidas_FGWT 
           ([Seq]
           ,[TestName]
           ,[Location]
           ,[SystemType]
           ,[ReportType]
           ,[MtlTypeID]
           ,[Washing]
           ,[FabricComposition]
           ,[TestDetail]
           ,[Scale]
           ,[Criteria]
           ,[Criteria2])
select		isnull(a.[Seq]              ,0)
           ,isnull(a.[TestName]         ,'')
           ,isnull(a.[Location]         ,'')
           ,isnull(a.[SystemType]       ,'')
           ,isnull(a.[ReportType]       ,'')
           ,isnull(a.[MtlTypeID]        ,'')
           ,isnull(a.[Washing]          ,'')
           ,isnull(a.[FabricComposition],'')
           ,isnull(a.[TestDetail]       ,'')
           ,isnull(a.[Scale]            ,'')
           ,isnull(a.[Criteria]         ,0)
           ,isnull(a.[Criteria2]        ,0)
from Trade_To_Pms.dbo.Adidas_FGWT a
LEFT join production.dbo.Adidas_FGWT b on 
		a.[Location] 		  = b.[Location] 
	and a.[ReportType] 		  = b.[ReportType] 
	and a.[MtlTypeID] 		  = b.[MtlTypeID] 
	and a.[Washing] 		  = b.[Washing] 
	and a.[FabricComposition] = b.[FabricComposition]
where b.Location is null


-- TypeSelection
delete a
from production.dbo.TypeSelection a
left join Trade_To_Pms.dbo.TypeSelection b on a.VersionID = b.VersionID and a.Seq = b.Seq 
where b.VersionID is null

update a set 
	Code = isnull(b.Code,'')
from production.dbo.TypeSelection a
inner join Trade_To_Pms.dbo.TypeSelection b on a.VersionID = b.VersionID and a.Seq = b.Seq 

insert production.dbo.TypeSelection (VersionID,Seq,Code)
select 
	 isnull(a.VersionID,0)
	,isnull(a.Seq      ,0)
	,isnull(a.Code     ,'')
from Trade_To_Pms.dbo.TypeSelection a
left join production.dbo.TypeSelection b on a.VersionID = b.VersionID and a.Seq = b.Seq 
where b.VersionID is null


--QABrandSetting
update a set
	a.PullingTest_PullForceUnit = isnull(b.PullingTest_PullForceUnit ,'')
from  production.dbo.QABrandSetting a
inner join Trade_To_Pms.dbo.QABrandSetting b on a.BrandID = b.BrandID

INSERT INTO production.[dbo].[QABrandSetting]
           ([BrandID]
           ,PullingTest_PullForceUnit
           ,[AddDate]
           ,[AddName])
select 
	 isnull(a.BrandID,'')
	,isnull(a.PullingTest_PullForceUnit,'')
	,GETDATE()
	,'SCIMIS'
from  Trade_To_Pms.dbo.QABrandSetting a
left join production.dbo.QABrandSetting b on a.BrandID = b.BrandID
where b.BrandID is null

--GarmentTestShrinkage
delete a
from production.dbo.GarmentTestShrinkage a
left join Trade_To_Pms.dbo.GarmentTestShrinkage b on a.Ukey = b.Ukey 
where b.Ukey is null

update a set 
 [BrandID] = isnull( b.[BrandID],'')
,[LocationGroup] = isnull( b.[LocationGroup],'')
,[Location] = isnull( b.[Location],'')
,[Seq] = isnull( b.[Seq],0)
,[Type] = isnull( b.[Type],'')
,Category = isnull( b.Category,'')
from production.dbo.GarmentTestShrinkage a
inner join Trade_To_Pms.dbo.GarmentTestShrinkage b on a.Ukey = b.Ukey 

insert production.dbo.GarmentTestShrinkage 
           ([BrandID]
           ,[LocationGroup]
           ,[Location]
           ,[Seq]
           ,[Type]
           ,Category)
select
     isnull(a.[BrandID]        ,'')
    ,isnull(a.[LocationGroup]  ,'')
    ,isnull(a.[Location]       ,'')
    ,isnull(a.[Seq]            ,0)
    ,isnull(a.[Type]           ,'')
    ,isnull(a.Category           ,'')
from Trade_To_Pms.dbo.GarmentTestShrinkage a
left join production.dbo.GarmentTestShrinkage b on  a.Ukey = b.Ukey 
where b.BrandID is null

--PadPrint
delete a
from Production.dbo.PadPrint a
left join Trade_To_Pms.dbo.PadPrint b on a.Ukey = b.Ukey 
where b.Ukey is null
and (a.AddDate >= (select DateStart from Trade_To_Pms.dbo.DateInfo where Name = 'PadPrint')
 or a.EditDate >= (select DateStart from Trade_To_Pms.dbo.DateInfo where Name = 'PadPrint'))
update a set 
	 [Refno]      = isnull(b.[Refno]      ,'')
	,[BrandID]	  = isnull(b.[BrandID]    ,'')
	,[Category]	  = isnull(b.[Category]   ,'')
	,[SuppID]	  = isnull(b.[SuppID]     ,'')
	,[CurrencyID] = isnull(b.[CurrencyID] ,'')
	,[Junk]		  = isnull(b.[Junk]       ,0)
	,[Remark]	  = isnull(b.[Remark]     ,'')
	,[AddName]	  = isnull(b.[AddName]    ,'')
	,[AddDate]	  = b.[AddDate]   
	,[EditName]	  = isnull(b.[EditName]   ,'')
	,[EditDate]	  = b.[EditDate]  
from production.dbo.PadPrint a
inner join Trade_To_Pms.dbo.PadPrint b on a.Ukey = b.Ukey 

insert production.dbo.PadPrint 
           (Ukey
		   ,[Refno]
           ,[BrandID]
           ,[Category]
           ,[SuppID]
           ,[CurrencyID]
           ,[Junk]
           ,[Remark]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.Ukey          ,0)
	,isnull(a.[Refno]       ,'')
    ,isnull(a.[BrandID]     ,'')
    ,isnull(a.[Category]    ,'')
    ,isnull(a.[SuppID]      ,'')
    ,isnull(a.[CurrencyID]  ,'')
    ,isnull(a.[Junk]        ,0)
    ,isnull(a.[Remark]      ,'')
    ,isnull(a.[AddName]     ,'')
    ,a.[AddDate]  
    ,isnull(a.[EditName]    ,'')
    ,a.[EditDate]
from Trade_To_Pms.dbo.PadPrint a
left join production.dbo.PadPrint b on  a.Ukey = b.Ukey 
where b.Ukey is null

--PadPrint_Mold
delete a
from Production.dbo.PadPrint_Mold a
left join Trade_To_Pms.dbo.PadPrint_Mold b on a.[PadPrint_ukey] = b.[PadPrint_ukey] and a.[MoldID] = b.[MoldID]
where b.[PadPrint_ukey] is null
and (a.AddDate >= (select DateStart from Trade_To_Pms.dbo.DateInfo where Name = 'PadPrint')
 or a.EditDate >= (select DateStart from Trade_To_Pms.dbo.DateInfo where Name = 'PadPrint'))

update a set 
	 [Refno]		 = isnull( b.[Refno]        ,'')
	,[BrandID]		 = isnull( b.[BrandID]      ,'')
	,[Season]		 = isnull( b.[Season]       ,'')
	,[LabelFor]		 = isnull( b.[LabelFor]     ,'')
	,[MainSize]		 = isnull( b.[MainSize]     ,'')
	,[Gender]		 = isnull( b.[Gender]       ,'')
	,[AgeGroup]		 = isnull( b.[AgeGroup]     ,'')
	,[SizeSpec]		 = isnull( b.[SizeSpec]     ,'')
	,[Part]			 = isnull( b.[Part]         ,'')
	,[MadeIn]		 = isnull( b.[MadeIn]       ,'')
	,[Region]		 = isnull( b.[Region]       ,'')
	,[AddName]		 = isnull( b.[AddName]      ,'')
	,[AddDate]		 =  b.[AddDate]            
	,[EditName]		 = isnull( b.[EditName]     ,'')
	,[EditDate]		 = b.[EditDate]
from production.dbo.PadPrint_Mold a
inner join Trade_To_Pms.dbo.PadPrint_Mold b on a.[PadPrint_ukey] = b.[PadPrint_ukey] and a.[MoldID] = b.[MoldID]

insert production.dbo.PadPrint_Mold 
           ([PadPrint_ukey]
           ,[MoldID]
           ,[Refno]
           ,[BrandID]
           ,[Season]
           ,[LabelFor]
           ,[MainSize]
           ,[Gender]
           ,[AgeGroup]
           ,[SizeSpec]
           ,[Part]
           ,[MadeIn]
           ,[Region]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.[PadPrint_ukey],0)
	,isnull(a.[MoldID]       ,'')
	,isnull(a.[Refno]        ,'')
	,isnull(a.[BrandID]      ,'')
	,isnull(a.[Season]       ,'')
	,isnull(a.[LabelFor]     ,'')
	,isnull(a.[MainSize]     ,'')
	,isnull(a.[Gender]       ,'')
	,isnull(a.[AgeGroup]     ,'')
	,isnull(a.[SizeSpec]     ,'')
	,isnull(a.[Part]         ,'')
	,isnull(a.[MadeIn]       ,'')
	,isnull(a.[Region]       ,'')
	,isnull(a.[AddName]      ,'')
	,a.[AddDate]   
	,isnull(a.[EditName]     ,'')
	,a.[EditDate]  
from Trade_To_Pms.dbo.PadPrint_Mold a
left join production.dbo.PadPrint_Mold b on a.[PadPrint_ukey] = b.[PadPrint_ukey] and a.[MoldID] = b.[MoldID]
where b.[PadPrint_ukey] is null

--PadPrint_Mold_Spec
delete a
from Production.dbo.PadPrint_Mold_Spec a
left join Trade_To_Pms.dbo.PadPrint_Mold_Spec b on a.PadPrint_ukey = b.PadPrint_ukey and a.MoldID = b.MoldID and a.Side = b.Side
where b.PadPrint_ukey is null
and (a.AddDate >= (select DateStart from Trade_To_Pms.dbo.DateInfo where Name = 'PadPrint')
 or a.EditDate >= (select DateStart from Trade_To_Pms.dbo.DateInfo where Name = 'PadPrint'))

update a set 
	 [SizePage]		 = isnull( b.[SizePage]       ,'')
	,[SourceSize]	 = isnull( b.[SourceSize]     ,'')
	,[CustomerSize]	 = isnull( b.[CustomerSize]   ,'')
	,[MoldRef]		 = isnull( b.[MoldRef]        ,'')
	,[Version]		 = isnull( b.[Version]        ,'')
	,[ReversionMold] = isnull( b.[ReversionMold]  ,'')
	,[Junk]			 = isnull( b.[Junk]           ,0)
	,[Reason]		 = isnull( b.[Reason]         ,'')
	,[AddName]		 = isnull( b.[AddName]        ,'')
	,[AddDate]		 =  b.[AddDate]
	,[EditName]		 = isnull( b.[EditName]       ,'')
	,[EditDate]		 = b.[EditDate]
from production.dbo.PadPrint_Mold_Spec a
inner join Trade_To_Pms.dbo.PadPrint_Mold_Spec b on a.PadPrint_ukey = b.PadPrint_ukey and a.MoldID = b.MoldID and a.Side = b.Side

insert production.dbo.PadPrint_Mold_Spec 
           ([PadPrint_ukey]
           ,[MoldID]
           ,[Side]
           ,[SizePage]
           ,[SourceSize]
           ,[CustomerSize]
           ,[MoldRef]
           ,[Version]
           ,[ReversionMold]
           ,[Junk]
           ,[Reason]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.[PadPrint_ukey],0)
	,isnull(a.[MoldID]       ,'')
	,isnull(a.[Side]         ,'')
	,isnull(a.[SizePage]     ,'')
	,isnull(a.[SourceSize]   ,'')
	,isnull(a.[CustomerSize] ,'')
	,isnull(a.[MoldRef]      ,'')
	,isnull(a.[Version]      ,'')
	,isnull(a.[ReversionMold],'')
	,isnull(a.[Junk]         ,0)
	,isnull(a.[Reason]       ,'')
	,isnull(a.[AddName]      ,'')
	,a.[AddDate]    
	,isnull(a.[EditName]     ,'')
	,a.[EditDate]
from Trade_To_Pms.dbo.PadPrint_Mold_Spec a
left join production.dbo.PadPrint_Mold_Spec b on a.PadPrint_ukey = b.PadPrint_ukey and a.MoldID = b.MoldID and a.Side = b.Side
where b.PadPrint_ukey is null

--Style_SpecialMark
delete a
from Production.dbo.Style_SpecialMark a
left join Trade_To_Pms.dbo.Style_SpecialMark b on a.ID = b.ID and a.BrandID = b.BrandID
where b.ID is null

update a set 
	[Name]			= ISNULL(b.[Name], '')
	, [Remark]		= ISNULL(b.[Remark], '')
	, [Junk]		= ISNULL(b.[Junk], 0)
	, [IsGMTTest]		= ISNULL(b.[IsGMTTest], 0)
	, [AddName]		= ISNULL(b.[AddName], '')
	, [AddDate]		= b.[AddDate]
	, [EditName]	= ISNULL(b.[EditName], '')
	, [EditDate]	= b.[EditDate]
from Production.dbo.Style_SpecialMark a
inner join Trade_To_Pms.dbo.Style_SpecialMark b on a.ID = b.ID and a.BrandID = b.BrandID

insert Production.dbo.Style_SpecialMark 
	([ID]
	, [BrandID]
	, [Name]
	, [Remark]
	, [Junk]
	, [IsGMTTest]
	, [AddName]
	, [AddDate]
	, [EditName]
	, [EditDate])
select
	 [ID] = ISNULL(a.[ID], '')
	 , [BrandID] = ISNULL(a.[BrandID], '')
	 , [Name] = ISNULL(a.[Name], '')
	 , [Remark] = ISNULL(a.[Remark], '')
	 , [Junk] = ISNULL(a.[Junk], 0)
	 , [IsGMTTest] = ISNULL(a.[IsGMTTest], 0)
	 , [AddName] = ISNULL(a.[AddName], '')
	 , a.[AddDate]
	 , [EditName] = ISNULL(a.[EditName], '')
	 , a.[EditDate]
from Trade_To_Pms.dbo.Style_SpecialMark a
left join Production.dbo.Style_SpecialMark b on a.ID = b.ID and a.BrandID = b.BrandID
where b.ID is null

-- MailGroup
Merge Production.dbo.MailGroup as t
using Trade_To_Pms.dbo.MailGroup as s
on t.code=s.code and t.FactoryID = s.FactoryID
	when matched then
		update set 
		t.ToAddress= s.ToAddress,
		t.CCAddress= s.CCAddress,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
	when not matched by target then
		insert(
		Code
		,FactoryID
		,ToAddress
		,CCAddress
		,AddName
		,AddDate
		,EditName
		,EditDate
		)
		values(
		s.Code
		,s.FactoryID
		,s.ToAddress
		,s.CCAddress
		,s.AddName
		,s.AddDate
		,s.EditName
		,s.EditDate
		);

-- Mailto
Merge Production.dbo.Mailto as t
using Trade_To_Pms.dbo.Mailto as s
on t.ID=s.ID
	when matched then
		update set 
		t.Description = s.Description,
		t.ToAddress= s.ToAddress,
		t.CCAddress= s.CCAddress,
		t.Subject = s.Subject,
		t.Content = s.Content,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
	when not matched by target then
		insert(
		ID
		,Description
		,ToAddress
		,CCAddress
		,Subject
		,Content
		,AddName
		,AddDate
		,EditName
		,EditDate
		)
		values(
		 s.ID
		,s.Description
		,s.ToAddress
		,s.CCAddress
		,s.Subject
		,s.Content
		,s.AddName
		,s.AddDate
		,s.EditName
		,s.EditDate
		);


-- Brand_SizeCode
delete a
from production.dbo.Brand_SizeCode a
left join Trade_To_Pms.dbo.Brand_SizeCode b on a.BrandID = b.BrandID  and a.SizeCode = b.SizeCode
where b.BrandID is null

update a set 
	[AgeGroupID]  = isnull(b.AgeGroupID,'')
	,[AddName]	  = isnull(b.[AddName],'')
	,[AddDate]	  = b.[AddDate]
	,[EditName]	  = isnull(b.[EditName],'')
	,[EditDate]	  = b.[EditDate]
from production.dbo.[Brand_SizeCode] a
inner join Trade_To_Pms.dbo.[Brand_SizeCode] b on a.BrandID = b.BrandID  and a.SizeCode = b.SizeCode

insert production.dbo.[Brand_SizeCode] 
           ([BrandID] 
		   ,[SizeCode]
           ,[AgeGroupID]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
	 isnull(a.[BrandID],'')
	,isnull(a.[SizeCode],'')
    ,isnull(a.[AgeGroupID],'')
    ,isnull(a.[AddName],'')
    ,a.[AddDate]
    ,isnull(a.[EditName],'')
    ,a.[EditDate]
from Trade_To_Pms.dbo.[Brand_SizeCode] a
left join production.dbo.[Brand_SizeCode] b on  a.BrandID = b.BrandID  and a.SizeCode = b.SizeCode
where b.BrandID is null

--BomType
delete a
from Production.dbo.BomType a
left join Trade_To_Pms.dbo.BomType b on a.ID = b.ID
where b.ID is null

update a set 
	  [Name]		    = ISNULL(b.[Name], '')
	, Seq		        = ISNULL(b.Seq, '')
	, IsInformationSpec = ISNULL(b.IsInformationSpec, 0)
	, [Junk]		    = ISNULL(b.[Junk], 0)
	, [AddName]		    = ISNULL(b.[AddName], '')
	, [AddDate]		    = b.[AddDate]
	, [EditName]	    = ISNULL(b.[EditName], '')
	, [EditDate]	    = b.[EditDate]
from Production.dbo.BomType a
inner join Trade_To_Pms.dbo.BomType b on a.ID = b.ID

insert Production.dbo.BomType 
           ([ID]
           ,[Name]
           ,[Seq]
           ,[IsInformationSpec]
           ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select
     isnull(a.[ID]               , '')
    ,isnull(a.[Name]             , '')
    ,isnull(a.[Seq]              , 0)
    ,isnull(a.[IsInformationSpec], 0)
    ,isnull(a.[Junk]             , 0)
    ,isnull(a.[AddName]          , '')
    ,a.[AddDate]
    ,isnull(a.[EditName]         , '')
    ,a.[EditDate]
from Trade_To_Pms.dbo.BomType a
left join Production.dbo.BomType b on a.ID = b.ID
where b.ID is null


/*MaterialDocument */
----------------------Delete
Delete [Production].[dbo].[MaterialDocument]
from [Production].[dbo].[MaterialDocument]as a
left join [Trade_To_Pms].[dbo].[MaterialDocument]  as b
on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
where b.DocumentName is null and b.BrandID is null
---------------------------UPDATE
UPDATE a
SET 
--a.DocumentName
--,a.BrandID
a.Description = b.Description
,a.FabricType = b.FabricType
,a.Target = b.Target
,a.FileRule = b.FileRule
,a.Expiration = b.Expiration
,a.Filepath = b.Filepath
,a.ActiveSeason = b.ActiveSeason
,a.EndSeason = b.EndSeason
,a.Responsibility = b.Responsibility
,a.Category = b.Category
,a.ExcludeProgram = b.ExcludeProgram
,a.ExcludeReplace = b.ExcludeReplace
,a.ExcludeStock = b.ExcludeStock
,a.MtlTypeClude = b.MtlTypeClude
,a.SupplierClude = b.SupplierClude
,a.Junk = b.Junk
,a.AddDate = b.AddDate
,a.AddName = b.AddName
,a.EditDate = b.EditDate
,a.Editname = b.Editname
,a.ExcludeRibItem = b.ExcludeRibItem
from [Production].[dbo].[MaterialDocument] as a with(nolock)
inner join [Trade_To_Pms].[dbo].[MaterialDocument]  as b with(nolock) on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].[MaterialDocument]
 (
	DocumentName
	,BrandID
	,Description
	,FabricType
	,Target
	,FileRule
	,Expiration
	,Filepath
	,ActiveSeason
	,EndSeason
	,Responsibility
	,Category
	,ExcludeProgram
	,ExcludeReplace
	,ExcludeStock
	,MtlTypeClude
	,SupplierClude
	,Junk
	,AddDate
	,AddName
	,EditDate
	,Editname
    ,ExcludeRibItem
)
SELECT
	DocumentName
	,BrandID
	,Description
	,FabricType
	,Target
	,FileRule
	,Expiration
	,Filepath
	,ActiveSeason
	,EndSeason
	,Responsibility
	,Category
	,ExcludeProgram
	,ExcludeReplace
	,ExcludeStock
	,MtlTypeClude
	,SupplierClude
	,Junk
	,AddDate
	,AddName
	,EditDate
	,Editname
    ,ExcludeRibItem
from [Trade_To_Pms].[dbo].[MaterialDocument] as b WITH (NOLOCK)
where not exists(select DocumentName,BrandID from [Production].[dbo].[MaterialDocument] as a WITH (NOLOCK) where a.DocumentName = b.DocumentName and a.BrandID = b.BrandID)


/*MaterialDocument_MtlType */
----------------------Delete
Delete [Production].[dbo].MaterialDocument_MtlType
from [Production].[dbo].MaterialDocument_MtlType as a with(nolock)
left join [Trade_To_Pms].[dbo].MaterialDocument_MtlType  as b with(nolock)
on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.MtltypeId = b.MtltypeId
where b.DocumentName is null
	and b.BrandID is null 
	and b.MtltypeId is null
	and exists(select 1 from [Trade_To_Pms].[dbo].[MaterialDocument] t where t.DocumentName = b.DocumentName  and t.BrandID = b.BrandID) 


-------------------------- INSERT INTO
INSERT INTO [Production].[dbo].MaterialDocument_MtlType
 (
	DocumentName
	,BrandID
	,MtltypeId
)
SELECT
	DocumentName
	,BrandID
	,MtltypeId
from [Trade_To_Pms].[dbo].[MaterialDocument_MtlType] as b WITH (NOLOCK)
where not exists(select DocumentName,BrandID,MtltypeId from [Production].[dbo].[MaterialDocument_MtlType] as a WITH (NOLOCK) where a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.MtltypeId = b.MtltypeId)

/*MaterialDocument_Responsbility */
----------------------Delete
Delete [Production].[dbo].MaterialDocument_Responsbility
from [Production].[dbo].MaterialDocument_Responsbility as a with(nolock)
left join [Trade_To_Pms].[dbo].MaterialDocument_Responsbility  as b with(nolock) on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.SuppID = b.SuppID
where 
b.DocumentName is null 
and b.BrandID is null
and b.SuppID is null

---------------------------UPDATE
UPDATE a
SET 
--DocumentName
--BrandID
--SuppID
a.Responsibility = b.Responsibility
from [Production].[dbo].MaterialDocument_Responsbility as a with(nolock)
inner join [Trade_To_Pms].[dbo].MaterialDocument_Responsbility  as b with(nolock) on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].MaterialDocument_Responsbility
 (
	DocumentName
	,BrandID
	,SuppID
	,Responsibility
)
SELECT
	DocumentName
	,BrandID
	,SuppID
	,Responsibility
from [Trade_To_Pms].[dbo].MaterialDocument_Responsbility as b WITH (NOLOCK)
where not exists(select DocumentName,BrandID from [Production].[dbo].MaterialDocument_Responsbility as a WITH (NOLOCK) where a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.SuppID = b.SuppID)


/*MaterialDocument_Supplier*/
----------------------Delete
Delete [Production].[dbo].MaterialDocument_Supplier
from [Production].[dbo].MaterialDocument_Supplier as a left join [Trade_To_Pms].[dbo].MaterialDocument_Supplier  as b
on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.SuppID = b.SuppID
where 
b.DocumentName is null 
and b.BrandID is null
and b.SuppID is null
and exists(select 1 from [Trade_To_Pms].[dbo].[MaterialDocument] t where t.DocumentName = b.DocumentName  and t.BrandID = b.BrandID) 
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].MaterialDocument_Supplier
 (
	DocumentName
	,BrandID
	,SuppID
)
SELECT
	DocumentName
	,BrandID
	,SuppID
from [Trade_To_Pms].[dbo].MaterialDocument_Supplier as b WITH (NOLOCK)
where not exists(select DocumentName,BrandID from [Production].[dbo].MaterialDocument_Supplier as a WITH (NOLOCK) where a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.SuppID = b.SuppID)


/*MaterialDocument_WeaveType*/
----------------------Delete
Delete [Production].[dbo].MaterialDocument_WeaveType
from [Production].[dbo].MaterialDocument_WeaveType as a left join [Trade_To_Pms].[dbo].MaterialDocument_WeaveType  as b
on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.WeaveTypeId = b.WeaveTypeId
where 
b.DocumentName is null 
and b.BrandID is null
and b.WeaveTypeId is null
and exists(select 1 from [Trade_To_Pms].[dbo].[MaterialDocument] t where t.DocumentName = b.DocumentName  and t.BrandID = b.BrandID) 
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].MaterialDocument_WeaveType
 (
	DocumentName
	,BrandID
	,WeaveTypeId
)
SELECT
	DocumentName
	,BrandID
	,WeaveTypeId
from [Trade_To_Pms].[dbo].MaterialDocument_WeaveType as b WITH (NOLOCK)
where not exists(select DocumentName,BrandID from [Production].[dbo].MaterialDocument_WeaveType as a WITH (NOLOCK) where a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.WeaveTypeId = b.WeaveTypeId)


/*BrandRelation*/
----------------------Delete
Delete [Production].[dbo].BrandRelation
from [Production].[dbo].BrandRelation as a left join [Trade_To_Pms].[dbo].BrandRelation  as b
on a.SuppGroup = b.SuppGroup and a.BrandID = b.BrandID and a.SuppID = b.SuppID
where 
b.SuppGroup is null 
and b.BrandID is null
and b.SuppID is null

---------------------------UPDATE 
UPDATE a
SET 
--BrandID
--SuppGroup
--SuppID
a.AddDate = b.AddDate
,a.AddName = b.AddName
,a.EditDate = b.EditDate
,a.Editname = b.Editname
from [Production].[dbo].BrandRelation as a with(nolock)
inner join [Trade_To_Pms].[dbo].BrandRelation  as b with(nolock) on a.SuppGroup = b.SuppGroup and a.BrandID = b.BrandID and a.SuppID = b.SuppID
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].BrandRelation
 (
	 BrandID
	,SuppGroup
	,SuppID
	,AddDate
	,AddName
	,EditDate
	,Editname
)
SELECT
	 BrandID
	,SuppGroup
	,SuppID
	,AddDate
	,AddName
	,EditDate
	,Editname
from [Trade_To_Pms].[dbo].BrandRelation as b WITH (NOLOCK)
where not exists(select BrandID,SuppGroup,SuppID from [Production].[dbo].BrandRelation as a WITH (NOLOCK) where a.SuppGroup = b.SuppGroup and a.BrandID = b.BrandID and a.SuppID = b.SuppID)

/*Exchange*/
----------------------Delete
Delete a
from [Production].[dbo].Exchange as a 
left join [Trade_To_Pms].[dbo].Exchange  as b
on a.ExchangeTypeID = b.ExchangeTypeID and a.CurrencyFrom = b.CurrencyFrom and a.CurrencyTo = b.CurrencyTo and a.DateStart = b.DateStart
where 
b.ExchangeTypeID is null 
and b.CurrencyFrom is null
and b.CurrencyTo is null
and b.DateStart is null
---------------------------UPDATE 
UPDATE a
SET a.DateEnd = b.DateEnd
	,a.Rate = isnull(b.Rate, 0)
	,a.Remark = isnull(b.Remark, '')
	,a.AddDate = b.AddDate
	,a.AddName = isnull(b.AddName, '')
	,a.EditDate = b.EditDate
	,a.EditName = isnull(b.EditName, '')
	,a.Junk = isnull(b.Junk, 0)
from [Production].[dbo].Exchange as a with(nolock)
inner join [Trade_To_Pms].[dbo].Exchange  as b with(nolock) on a.ExchangeTypeID = b.ExchangeTypeID and a.CurrencyFrom = b.CurrencyFrom and a.CurrencyTo = b.CurrencyTo and a.DateStart = b.DateStart
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].Exchange
 (
	 ExchangeTypeID
	,CurrencyFrom
	,CurrencyTo
	,DateStart
	,DateEnd
	,Rate
	,Remark
	,AddDate
	,AddName
	,EditDate
	,EditName
	,Junk
)
SELECT
	 ExchangeTypeID
	,CurrencyFrom
	,CurrencyTo
	,DateStart
	,DateEnd
	,isnull(Rate, 0)
	,isnull(Remark, '')
	,AddDate
	,isnull(AddName, '')
	,EditDate
	,isnull(EditName, '')
	,isnull(Junk, 0)
from [Trade_To_Pms].[dbo].Exchange as b WITH (NOLOCK)
where not exists(select 1 from [Production].[dbo].Exchange as a WITH (NOLOCK) where a.ExchangeTypeID = b.ExchangeTypeID and a.CurrencyFrom = b.CurrencyFrom and a.CurrencyTo = b.CurrencyTo and a.DateStart = b.DateStart)

/*AutomatedLineMappingConditionSetting*/
delete [Production].[dbo].AutomatedLineMappingConditionSetting

insert into [Production].[dbo].AutomatedLineMappingConditionSetting
			(
				Ukey
				,MDivisionID
				,FactoryID
				,Functions
				,Verify
				,Condition1
				,Condition2
				,Condition3
				,Junk
				,AddName
				,AddDate
				,EditName
				,EditDate
			)
select	Ukey
		,MDivisionID
		,FactoryID
		,Functions
		,Verify
		,Condition1
		,Condition2
		,Condition3
		,Junk
		,AddName
		,AddDate
		,EditName
		,EditDate
from	[Trade_To_Pms].[dbo].AutomatedLineMappingConditionSetting

/*ExpressDuty*/
delete [Production].[dbo].ExpressDuty

insert into [Production].[dbo].ExpressDuty
			(
				ID
				,Name
				,Description
				,Remark
				,Mail
				,Junk
				,IsTransferExport
				,NeedTaskTeamApprove
				,AddName
				,AddDate
				,EditName
				,EditDate
			)
select	ID
		,Name
		,Description
		,Remark
		,Mail
		,Junk
		,IsTransferExport
		,NeedTaskTeamApprove
		,AddName
		,AddDate
		,EditName
		,EditDate
from	[Trade_To_Pms].[dbo].ExpressDuty

/*ExpressDuty_Functions*/
delete [Production].[dbo].ExpressDuty_Functions

insert into [Production].[dbo].ExpressDuty_Functions
			(
				ExpressDutyID
				,FunctionID
				,AddName
				,AddDate
			)
select	ExpressDutyID
				,FunctionID
				,AddName
				,AddDate
from	[Trade_To_Pms].[dbo].ExpressDuty_Functions

/*ShareRule*/
----------------------Delete
Delete a
from [Production].[dbo].ShareRule as a 
left join [Trade_To_Pms].[dbo].ShareRule  as b on a.AccountID = b.AccountID and a.ExpenseReason = b.ExpenseReason and a.ShareBase = b.ShareBase
where	b.AccountID is null and
		b.ExpenseReason is null and
		b.ShareBase is null
---------------------------UPDATE 
UPDATE a
SET a.ShipModeID = b.ShipModeID,
	a.Junk = isnull(b.Junk, 0)
from [Production].[dbo].ShareRule as a with(nolock)
inner join [Trade_To_Pms].[dbo].ShareRule  as b with(nolock) on a.AccountID = b.AccountID and a.ExpenseReason = b.ExpenseReason and a.ShareBase = b.ShareBase
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].ShareRule
 (
	 AccountID
	 ,ExpenseReason
	 ,ShareBase
	 ,ShipModeID
	 ,Junk
)
SELECT b.AccountID
	  ,b.ExpenseReason
	  ,b.ShareBase
	  ,isnull(b.ShipModeID, '')
	  ,isnull(b.Junk, 0)
from [Trade_To_Pms].[dbo].ShareRule as b WITH (NOLOCK)
where not exists(select 1 from [Production].[dbo].ShareRule a WITH (NOLOCK) where a.AccountID = b.AccountID and a.ExpenseReason = b.ExpenseReason and a.ShareBase = b.ShareBase)


-------- insert MaterialDocument_Brand 
DELETE [Production].[dbo].[MaterialDocument_Brand]
from [Production].[dbo].[MaterialDocument_Brand] a
left join [Trade_To_Pms].[dbo].[MaterialDocument_Brand] b
on a.DocumentName = b.DocumentName and a.BrandID = b.BrandID and a.MergedBrand = b.MergedBrand
where b.DocumentName is null 


INSERT INTO [Production].[dbo].MaterialDocument_Brand (DocumentName,BrandID,MergedBrand)
SELECT DocumentName,BrandID,MergedBrand 
FROM  [Trade_To_Pms].[dbo].MaterialDocument_Brand t
WHERE not exists(
	select 1 
	from [Production].[dbo].MaterialDocument_Brand s
	where s.DocumentName = t.DocumentName
	and s.BrandID = t.BrandID
	and s.MergedBrand = t.MergedBrand
)

END


/*MtlType_Brand */
----------------------Delete
Delete t
from [Production].[dbo].MtlType_Brand as t with(nolock)
left join [Trade_To_Pms].[dbo].MtlType_Brand  as s with(nolock)on t.ID = s.ID and t.BrandID = s.BrandID 
where s.ID is null

---------------------------UPDATE
UPDATE a
SET  a.BossLockDay = ISNULL(b.BossLockDay,0)
	,a.IsSustainableMaterial = ISNULL(b.IsSustainableMaterial,0)
	,a.IsSustainableMaterialforSMTT = ISNULL(b.IsSustainableMaterialforSMTT,0)
	,a.IsBCIforSMTT = ISNULL(b.IsBCIforSMTT,0)
from [Production].[dbo].MtlType_Brand as a with(nolock)
inner join [Trade_To_Pms].[dbo].MtlType_Brand  as b with(nolock) on a.ID = b.ID and a.BrandID = b.BrandID

-------------------------- INSERT INTO
INSERT INTO [Production].[dbo].MtlType_Brand
 (
		ID
		,BrandID
		,BossLockDay
		,IsSustainableMaterial
		,IsSustainableMaterialforSMTT
		,IsBCIforSMTT
)
SELECT
		ID
		,BrandID
		,ISNULL(BossLockDay,0)
		,ISNULL(IsSustainableMaterial,0)
		,ISNULL(IsSustainableMaterialforSMTT,0)
		,ISNULL(IsBCIforSMTT,0)
from [Trade_To_Pms].[dbo].MtlType_Brand as b WITH (NOLOCK)
where not exists (select 1 from [Production].[dbo].MtlType_Brand as a WITH (NOLOCK) where a.ID = b.ID )