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
----------------------�R���DTABLE�h�����
Delete Production.dbo.Brand
from Production.dbo.Brand as a left join Trade_To_Pms.dbo.Brand as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET 
      -- a. ID	      = b. ID
      a. NameCH	      = b. NameCH
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
	  ,a. LossSampleAccessory = b.LossSampleAccessory
	  ,a. OTDExtension = b.OTDExtension
	  ,a. UseRatioRule = b.UseRatioRule
	  ,a. UseRatioRule_Thick = b.UseRatioRule_Thick
from Production.dbo.Brand as a inner join Trade_To_Pms.dbo.Brand as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
	  ,LossSampleAccessory
	  ,OTDExtension
	  ,UseRatioRule
	  ,UseRatioRule_Thick
from Trade_To_Pms.dbo.Brand as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Brand as a WITH (NOLOCK) where a.id = b.id)

----------------------Brand_Month
Delete Production.dbo.Brand_Month
from Production.dbo.Brand_Month as a left join Trade_To_Pms.dbo.Brand_Month as b
on a.id = b.id and a.Year = b.Year and a.Month = b.Month
where b.id is null

UPDATE a
SET 	
     a.[MonthLabel]=b.[MonthLabel]
    ,a.[AddName]	 =b.[AddName]
    ,a.[AddDate]	 =b.[AddDate]
    ,a.[EditName]	 =b.[EditName]
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
	 [ID]
    ,[Year]
    ,[Month]
    ,[MonthLabel]
    ,[AddName]
    ,[AddDate]
    ,[EditName]
    ,[EditDate]
from Trade_To_Pms.dbo.Brand_Month as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Brand_Month as a WITH (NOLOCK) where a.id = b.id and a.Year = b.Year and a.Month = b.Month)

--Season
--ASeason Season
----------------------�R���DTABLE�h�����
Delete Production.dbo.Season
from Production.dbo.Season as a left join Trade_To_Pms.dbo.Season as b
on a.id = b.id and a.BrandID = b.BrandID
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET 
      -- a.ID		 =b.ID
      --,a.BrandID		      =b.BrandID
      a.CostRatio		      =b.CostRatio
      ,a.SeasonSCIID		      =b.SeasonSCIID
      ,a.Month		      =b.Month
      ,a.Junk		      =b.Junk
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Season as a inner join Trade_To_Pms.dbo.Season as b ON a.id=b.id and a.BrandID = b.BrandID
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Season as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Season as a WITH (NOLOCK) where a.id = b.id and a.BrandID = b.BrandID)


--Supp  Supp
--ASupp
----------------------�R���DTABLE�h�����
Delete Production.dbo.Supp
from Production.dbo.Supp as a left join Trade_To_Pms.dbo.Supp as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET 
--a.ID		  =b.ID
      a.Junk		      =b.Junk
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
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Supp as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Supp as a WITH (NOLOCK) where a.id = b.id)


--Ability
--AAbility
--CDCode
----------------------�R���DTABLE�h�����
Delete Production.dbo.CDCode
from Production.dbo.CDCode as a left join Trade_To_Pms.dbo.CDCode as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID					 =b.ID
      a.Junk		         =b.Junk
      ,a.Description		 =b.Description
      ,a.Cpu		          =b.Cpu
      ,a.ComboPcs		      =b.ComboPcs
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate
      ,a.ProductionFamilyID		      =b.ProductionFamilyID
from Production.dbo.CDCode as a inner join Trade_To_Pms.dbo.CDCode as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.CDCode as b WITH (NOLOCK)
where not exists(select id from Production.dbo.CDCode as a WITH (NOLOCK) where a.id = b.id)


--Country Country
--ACountry
----------------------�R���DTABLE�h�����
Delete Production.dbo.Country
from Production.dbo.Country as a left join Trade_To_Pms.dbo.Country as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
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
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Country as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Country as a WITH (NOLOCK) where a.id = b.id)

--WeaveType
merge Production.dbo.WeaveType t 
using Trade_To_Pms.dbo.WeaveType s
on t.id = s.id
when matched then update set
	 t.isFabricLoss=s.isFabricLoss
	,t.Junk		   =s.Junk
	,t.AddName	   =s.AddName
	,t.AddDate	   =s.AddDate
	,t.EditName	   =s.EditName
	,t.EditDate	   =s.EditDate
when not matched by target then
	insert(id,isFabricLoss,Junk,AddName,AddDate,EditName,EditDate)
	values(s.id,s.isFabricLoss,s.Junk,s.AddName,s.AddDate,s.EditName,s.EditDate)
when not matched by source then
	delete
;
--Mtltype Mtltype  
--AMtltype
----------------------�R���DTABLE�h�����
Delete Production.dbo.MtlType
from Production.dbo.MtlType as a left join Trade_To_Pms.dbo.MtlType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
--------TRADE��EditDate > PMS EditDate �NUPDATE All �p�Gfalse �N��UPDATE EditDate & EditName ��L�@��UPDATE

------------------------------------------------------------------------------------------------------
--***��ƥ洫�����󭭨�***
--1. �u�����oProduction.dbo.DateInfo
declare @DateInfoName varchar(30) ='imp_MtlType';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.���o�w�]��
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.��sPms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

UPDATE a
SET  
      --a.ID	      =b.ID		
      a.FullName	      =b.FullName		
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
	  ,a.IsTrimCardOther = b.isTrimCardOther	
	  ,a.IsThread        = b.IsThread
	  ,a.LossQtyCalculateType        = b.LossQtyCalculateType

from Production.dbo.MtlType as a inner join Trade_To_Pms.dbo.MtlType as b ON a.id=b.id
where b.EditDate between @DateStart and @DateEnd
-------------------------- INSERT INTO ��
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
	  ,isTrimCardOther
	  ,IsThread
	  ,LossQtyCalculateType

from Trade_To_Pms.dbo.MtlType as b WITH (NOLOCK)
where not exists(select id from Production.dbo.MtlType as a WITH (NOLOCK) where a.id = b.id)


--Artworktype Artworktype 
--PMS �h,[InhouseOSP]
--      ,[AccountNo]
--      ,[BcsLt]
--      ,[CutLt]
--IsSubprocess ����s~INSERT�ᡦO��
----------------------�R���DTABLE�h�����
Delete Production.dbo.ArtworkType
from Production.dbo.ArtworkType as a left join Trade_To_Pms.dbo.ArtworkType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ID	      =b.ID		
      a.Abbreviation	      =b.Abbreviation		
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
	  ,a.IsPrintToCMP	  =b.IsPrintToCMP
	  ,a.IsLocalPurchase = b.IsLocalPurchase
from Production.dbo.ArtworkType as a inner join Trade_To_Pms.dbo.ArtworkType as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
	  ,IsPrintToCMP
	  ,IsLocalPurchase
from Trade_To_Pms.dbo.ArtworkType as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ArtworkType as a WITH (NOLOCK) where a.id = b.id)
-------------------------------Artworktype_Detail
merge Production.dbo.Artworktype_Detail as t
Using Trade_TO_Pms.dbo.Artworktype_Detail as s
on t.ArtworktypeID = s.ArtworktypeID and t.MachineTypeID = s.MachineTypeID
when not matched by target then
	insert(ArtworktypeID,MachineTypeID)
	values(ArtworktypeID,MachineTypeID)
when not matched by source then
	delete;

--ArtworkType_FTY
---------------------------
Begin
	update a
		set a.IsShowinIEP01 = b.IsShowinIEP01
		  , a.IsShowinIEP03 = b.IsShowinIEP03
		  , a.IsSewingline = b.IsSewingline
	from Production.dbo.ArtworkType_FTY a WITH (NOLOCK)
	inner join Trade_To_Pms.dbo.ArtworkType_FTY b WITH (NOLOCK) on a.ArtworkTypeID = b.ArtworkTypeID and a.FactoryID = b.FactoryID
	where exists (select 1 from Production.dbo.Factory f WITH (NOLOCK) where a.FactoryID = f.ID)

	insert into Production.dbo.ArtworkType_FTY ([ArtworkTypeID], [FactoryID], [IsShowinIEP01], [IsShowinIEP03], [IsSewingline])
	select b.ArtworkTypeID, b.FactoryID, b.IsShowinIEP01, b.IsShowinIEP03, b.IsSewingline
	from Trade_To_Pms.dbo.ArtworkType_FTY b WITH (NOLOCK)
	inner join Production.dbo.Factory f WITH (NOLOCK) on b.FactoryID = f.ID
	where not exists (select 1 from Production.dbo.ArtworkType_FTY a WITH (NOLOCK) where a.ArtworkTypeID = b.ArtworkTypeID and a.FactoryID = b.FactoryID)

	delete a
	from Production.dbo.ArtworkType_FTY a WITH (NOLOCK)
	where not exists (select 1 from Trade_To_Pms.dbo.ArtworkType_FTY b WITH (NOLOCK) where a.ArtworkTypeID = b.ArtworkTypeID and a.FactoryID = b.FactoryID)
END
---------------------------

--Artworktype1 MachineType �L�h�����
--AArtworkType1
----------------------MachineType--
--Last Ver:Mantis_5495�R���쥻delete/updata/insert��ϥ�merge�@�k
merge Production.dbo.MachineType as t
Using Trade_TO_Pms.dbo.MachineType as s
on t.id = s.id
when matched then
		update set 
		t.Description		=s.Description	
		,t.DescCH		    =s.DescCH		    
		,t.ISO				=s.ISO		    
		,t.ArtworkTypeID	=s.ArtworkTypeID	
		,t.Mold				=s.Mold		    
		,t.RPM				=s.RPM		    
		,t.Stitches			=s.Stitches		
		,t.Picture1			=s.Picture1		
		,t.Picture2			=s.Picture2		
		,t.MachineAllow		=s.MachineAllow	
		,t.ManAllow			=s.ManAllow		
		,t.MachineGroupID	=s.MachineGroupID	
		,t.Junk				=s.Junk		    
		,t.AddName			=s.AddName		
		,t.AddDate			=s.AddDate		
		,t.EditName			=s.EditName		
		,t.EditDate			=s.EditDate		
		,t.isThread         =s.isThread
		,t.MasterGroupID	=s.MasterGroupID	
		,t.Hem				=s.Hem
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
when not matched by source then
	delete;

update a
	set a.IsDesignatedArea = b.IsDesignatedArea
from Production.dbo.MachineType a WITH (NOLOCK)
inner join Trade_To_Pms.dbo.MachineTypeTPE b WITH (NOLOCK) on a.ID = b.ID

--CustCD CustCD
--ACustCD
--PMS�h�� [Dest]
----------------------�R���DTABLE�h�����
Delete Production.dbo.CustCD
from Production.dbo.CustCD as a left join Trade_To_Pms.dbo.CustCD as b
on a.id = b.id and a.BrandID=b.BrandID
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.BrandID		     =b.BrandID
      a.Junk		      =b.Junk
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
	  ,a.Kit		          =b.Kit
	  ,a.HealthID		      =b.HealthID

from Production.dbo.CustCD as a inner join Trade_To_Pms.dbo.CustCD as b ON a.id=b.id and a.BrandID=b.BrandID
-------------------------- INSERT INTO ��
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
	  ,Kit
	  ,HealthID
from Trade_To_Pms.dbo.CustCD as b WITH (NOLOCK)
where not exists(select id from Production.dbo.CustCD as a WITH (NOLOCK) where a.id = b.id and a.BrandID=b.BrandID)


--Reason ReasonType
--AReason
--Reason1 Reason
----------------------�R���DTABLE�h�����
Delete Production.dbo.Reason
from Production.dbo.Reason as a left join Trade_To_Pms.dbo.Reason as b
on a.id = b.id and a.ReasonTypeID = b.ReasonTypeID
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ReasonTypeID	        = b.ReasonTypeID	
      --,a.ID	       = b.ID	
      a.Name	       = b.Name	
      ,a.Remark	       = b.Remark	
      ,a.No	       = b.No	
      ,a.ReasonGroup	       = b.ReasonGroup	
      ,a.Kpi	       = b.Kpi	
      ,a.AccountID	       = b.AccountID	
      ,a.FactoryKpi	       = b.FactoryKpi	
      ,a.AddName	       = b.AddName	
      ,a.AddDate	       = b.AddDate	
      ,a.EditName	       = b.EditName	
      ,a.EditDate	       = b.EditDate	
      ,a.Junk	       = b.Junk	

from Production.dbo.Reason as a inner join Trade_To_Pms.dbo.Reason as b ON a.id=b.id and a.ReasonTypeID = b.ReasonTypeID
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Reason as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Reason as a WITH (NOLOCK) where a.id = b.id and a.ReasonTypeID = b.ReasonTypeID)


--SHIPTerm SHIPTerm
--AShipTerm
----------------------�R���DTABLE�h�����
Delete Production.dbo.SHIPTerm
from Production.dbo.SHIPTerm as a left join Trade_To_Pms.dbo.SHIPTerm as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
		--a.ID	   =b.ID	
      a.Description	  =b.Description	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.SHIPTerm as a inner join Trade_To_Pms.dbo.SHIPTerm as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.SHIPTerm as b WITH (NOLOCK)
where not exists(select id from Production.dbo.SHIPTerm as a WITH (NOLOCK) where a.id = b.id)

--Factory
--PMS �h�����
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
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID	    =b.ID	
      -- a.MDivisionID	      =b.MDivisionID	
      a.Junk	      =b.Junk
	  ,a.NameCH	      =b.NameCH	
      ,a.CountryID	      =b.CountryID		
      ,a.AddressCH	      =b.AddressCH	
      ,a.CurrencyID	      =b.CurrencyID	
      ,a.CPU	      =b.CPU	
      ,a.ZipCode	      =b.ZipCode	
      ,a.PortSea	      =b.PortSea	
      ,a.PortAir	      =b.PortAir	
      ,a.KitId	      =b.KitId	
      ,a.ExpressGroup	      =b.ExpressGroup	
      ,a.IECode	      =b.IECode	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.KPICode	      =b.KPICode	
      ,a.Type	      =b.Type	
      ,a.Zone	      =b.Zone	
      ,a.FactorySort	      =b.FactorySort	
	  ,a.IsSCI        =b.IsSCI
	  ,a.TestDocFactoryGroup = b.TestDocFactoryGroup
	  ,a.FtyZone      =b.FtyZone
	  ,a.Foundry	  =b.Foundry
	  ,a.ProduceM	  =b.MDivisionID
	  ,a.LoadingFactoryGroup	  =b.LoadingFactoryGroup
from Production.dbo.Factory as a inner join Trade_To_Pms.dbo.Factory as b ON a.id=b.id
--Factory1
--Factory_TMS
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
-------------------------- �ھ� Factory_TMS Primary Key ��s�s�b PMS �����
UPDATE a
SET  
      -- a.ID	      =b.ID	
      --,a.Year	      =b.Year	
      --,a.ArtworkTypeID	      =b.ArtworkTypeID	
     -- ,a.Month	      =b.Month	
      a.TMS	      =b.TMS	

from Production.dbo.Factory_Tms as a inner join Trade_To_Pms.dbo.Factory_Tms as b ON a.id=b.id and a.Year=b.Year and a.ArtworkTypeID=b.ArtworkTypeID and a.Month=b.Month
-------------------------- �ھ� Factory_TMS Primary Key �s�W���s�b PMS �����
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

from Trade_To_Pms.dbo.Factory_Tms as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Factory_Tms as a WITH (NOLOCK) where a.id = b.id and a.Year=b.Year and a.ArtworkTypeID=b.ArtworkTypeID and a.Month=b.Month)

-------------------------- �ھ� Factory_TMS Primary Key �R�����s�b Trade �����
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
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID		     =b.ID
      --,a.BrandID		      =b.BrandID
      --,a.CDCodeID		      =b.CDCodeID
      a.BrandAreaCode		      =b.BrandAreaCode
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
      -- a.ID		     =b.ID
     -- ,a.BrandID		      =b.BrandID
      --,a.CDCodeID		      =b.CDCodeID
      a.BrandAreaCode		      =b.BrandAreaCode
      ,a.BrandFTYCode		      =b.BrandFTYCode
      ,a.BrandVendorCode		      =b.BrandVendorCode
      ,a.BrandReportCode		      =b.BrandReportCode
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      --,a.EditName		      =b.EditName
      --,a.EditDate		      =b.EditDate

from Production.dbo.Factory_BrandDefinition as a inner join Trade_To_Pms.dbo.Factory_BrandDefinition as b ON a.id=b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID
where b.EditDate <= a.EditDate
-------------------------- INSERT INTO ��
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

from Trade_To_Pms.dbo.Factory_BrandDefinition as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Factory_BrandDefinition as a WITH (NOLOCK) where a.id = b.id and a.BrandID=b.BrandID and a.CDCodeID=b.CDCodeID)




--Factory5
--Factory_WorkHour
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ID	     =b.ID	
      --,a.Year	      =b.Year	
      --,a.Month	      =b.Month	
      a.HalfMonth1	      =b.HalfMonth1	
      ,a.HalfMonth2	      =b.HalfMonth2	

from Production.dbo.Factory_WorkHour as a inner join Trade_To_Pms.dbo.Factory_WorkHour as b ON a.id=b.id and a.Year=b.Year and a.Month=b.Month
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Factory_WorkHour as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Factory_WorkHour as a WITH (NOLOCK) where a.id = b.id and a.Year=b.Year and a.Month=b.Month)


--SCI_FTY
--SCIFTY ---delete

Delete Production.dbo.SCIFty
from Production.dbo.SCIFty as a 
left join Trade_To_Pms.dbo.Factory as b on a.id = b.id and b.IsSCI=1
where b.id is null

---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID	     =b.ID	
      a.Junk	      =b.Junk	
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
      ,a.Type	      =b.Type	
      ,a.Zone	      =b.Zone	
	  ,a.FtyZone      =b.FtyZone 
from Production.dbo.SCIFty as a inner join Trade_To_Pms.dbo.Factory as b ON a.id=b.id
where b.IsSCI=1

-------------------------- INSERT INTO ��
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
	  ,Type
	  ,Zone
	  ,FtyZone 
from Trade_To_Pms.dbo.Factory as b WITH (NOLOCK)
where not exists(select id from Production.dbo.SCIFty as a WITH (NOLOCK) where a.id = b.id)
and b.IsSCI=1

--Unit   Unit 
----------------------�R���DTABLE�h�����
Delete Production.dbo.Unit
from Production.dbo.Unit as a left join Trade_To_Pms.dbo.Unit as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
     --  a.ID	      =b.ID	
      a.PriceRate	      =b.PriceRate	
      ,a.Round	      =b.Round	
      ,a.Description	      =b.Description	
      ,a.ExtensionUnit	      =isnull(b.ExtensionUnit,'')	
      ,a.Junk	      =b.TradeJunk	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
	  ,a.MiAdidasRound    =b.MiAdidasRound
	  ,a.RoundStep        =b.RoundStep
	  ,a.StockRound		  =b.StockRound

from Production.dbo.Unit as a inner join Trade_To_Pms.dbo.Unit as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
       ID
      ,PriceRate
      ,Round
      ,Description
      ,isnull(ExtensionUnit,'')
      ,TradeJunk
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
	  ,MiAdidasRound
	  ,RoundStep
	  ,StockRound
from Trade_To_Pms.dbo.Unit as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Unit as a WITH (NOLOCK) where a.id = b.id)


--�o��}�l��PMS_TEST�����ճ�
--UnitRate
--Unit_Rate
----------------------�R���DTABLE�h�����
Delete Production.dbo.Unit_Rate
from Production.dbo.Unit_Rate as a left join Trade_To_Pms.dbo.Unit_Rate as b
on a.UnitFrom = b.UnitFrom and a.UnitTo = b.UnitTo
where b.UnitFrom is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.UnitFrom	    =b.UnitFrom	
     -- ,a.UnitTo	      =b.UnitTo	
      a.Rate	      =b.Rate	
      ,a.RateValue	      =b.RateValue	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Unit_Rate as a inner join Trade_To_Pms.dbo.Unit_Rate as b ON a.UnitFrom = b.UnitFrom and a.UnitTo = b.UnitTo
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Unit_Rate as b WITH (NOLOCK)
where not exists(select UnitFrom from Production.dbo.Unit_Rate as a WITH (NOLOCK) where a.UnitFrom = b.UnitFrom  and a.UnitTo = b.UnitTo)


--TPI_Pass1 
--TPEPass1<<<PMS
--Pass1<<TRADE
----------------------�R���DTABLE�h�����
Delete Production.dbo.TPEPass1
from Production.dbo.TPEPass1 as a left join Trade_To_Pms.dbo.Pass1 as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
 --a.ID	     =b.ID	
      a.Name	      =b.Name	
      ,a.ExtNo	      =b.ExtNo	
      ,a.EMail	      =b.EMail	
from Production.dbo.TPEPass1 as a inner join Trade_To_Pms.dbo.Pass1 as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Pass1 as b WITH (NOLOCK)
where not exists(select id from Production.dbo.TPEPass1 as a WITH (NOLOCK) where a.id = b.id)

--Color  PMS�h�@��[Ukey]
--Color
------------------------�R���DTABLE�h�����
Delete Production.dbo.Color
from Production.dbo.Color as a left join Trade_To_Pms.dbo.Color as b
on a.id = b.id and a.BrandId = b.BrandId
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
 --a.BrandId		 =b.BrandId
     -- ,a.ID		      =b.ID
      a.Name		      =b.Name
      ,a.Varicolored		      =b.Varicolored
      ,a.JUNK		      =b.JUNK
      ,a.VIVID		      =b.VIVID
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate
	  ,a.ukey =b.ukey

from Production.dbo.Color as a inner join Trade_To_Pms.dbo.Color as b ON a.id=b.id and a.BrandId = b.BrandId
-------------------------- INSERT INTO ��
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
	  ,ukey
from Trade_To_Pms.dbo.Color as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Color as a WITH (NOLOCK) where a.id = b.id and a.BrandId = b.BrandId)


--Currency
--Currency
----------------------�R���DTABLE�h�����
Delete Production.dbo.Currency
from Production.dbo.Currency as a left join Trade_To_Pms.dbo.Currency as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID		      =b.ID
      a.StdRate		      =b.StdRate
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
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Currency as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Currency as a WITH (NOLOCK) where a.id = b.id)


--Fab_Loss
--LossRateFabric   
----------------------�R���DTABLE�h�����
Delete Production.dbo.LossRateFabric
from Production.dbo.LossRateFabric as a left join Trade_To_Pms.dbo.LossRateFabric as b
on a.WeaveTypeID = b.WeaveTypeID
where b.WeaveTypeID is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.WeaveTypeID	     =b.WeaveTypeID		
      a.LossType	      =b.LossType		
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
	  ,a.MaxLossQty		 = b.MaxLossQty
	  ,a.MinGmtQty		 = b.MinGmtQty
	  ,a.MinLossQty		 = b.MinLossQty
	  ,a.PerGmtQty		 = b.PerGmtQty
	  ,a.PlsLossQty		 = b.PlsLossQty
from Production.dbo.LossRateFabric as a inner join Trade_To_Pms.dbo.LossRateFabric as b ON a.WeaveTypeID=b.WeaveTypeID
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.LossRateFabric as b WITH (NOLOCK)
where not exists(select WeaveTypeID from Production.dbo.LossRateFabric as a WITH (NOLOCK) where a.WeaveTypeID = b.WeaveTypeID)
--Acc_Loss
--LossRateAccessory
--PMS�h�@��,[Waste] �ۤv�O��
----------------------�R���DTABLE�h�����
Delete Production.dbo.LossRateAccessory
from Production.dbo.LossRateAccessory as a left join Trade_To_Pms.dbo.LossRateAccessory as b
on a.MtltypeId = b.MtltypeId
where b.MtltypeId is null
and a.Waste = 0

-------------------------- INSERT INTO ��INSERT waste�����
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
from Trade_To_Pms.dbo.LossRateAccessory as b WITH (NOLOCK)
where not exists(select MtltypeId from Production.dbo.LossRateAccessory as a WITH (NOLOCK) where a.MtltypeId = b.MtltypeId)

----------------------�R���DTABLE�h�����
Delete Production.dbo.LossRateAccessory_Limit
from Production.dbo.LossRateAccessory_Limit as a left join Trade_To_Pms.dbo.LossRateAccessory_Limit as b
on a.MtltypeId = b.MtltypeId and a.UsageUnit = b.UsageUnit
where b.MtltypeId is null

-------------------------- INSERT INTO ��INSERT waste�����
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
	MtltypeId
      ,UsageUnit
	  ,LimitUp
	  ,AddName
	  ,AddDate
	  ,EditName
	  ,EditDate
from Trade_To_Pms.dbo.LossRateAccessory_Limit as b WITH (NOLOCK)
where not exists(select MtltypeId from Production.dbo.LossRateAccessory_Limit as a WITH (NOLOCK)
				 where a.MtltypeId = b.MtltypeId and a.UsageUnit = b.UsageUnit)


-------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a--�S�O�B�zwaste�����
SET  
    --  a.MtltypeId	     =b.MtltypeId	
      a.LossUnit	      =b.LossUnit	
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
      ,a.IgnoreLimitUpBrand	      =b.IgnoreLimitUpBrand	
	  

from Production.dbo.LossRateAccessory as a inner join Trade_To_Pms.dbo.LossRateAccessory as b ON a.MtltypeId=b.MtltypeId
where a.LossUnit=1

UPDATE a--�S�O�B�zwaste�����
SET  
      --a.MtltypeId	     =b.MtltypeId	
      a.LossUnit	      =b.LossUnit	
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



--Cutparts   ���L�� TRADE�٥��ǳƧ���
--Carrier
--Carrier  
----------------------�R���DTABLE�h�����
Delete Production.dbo.Carrier
from Production.dbo.Carrier as a left join Trade_To_Pms.dbo.Carrier as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ID		 =b.ID
      a.SuppID		      =b.SuppID
      ,a.Account		      =b.Account
      ,a.Junk		      =b.Junk
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
      ,a.EditDate		      =b.EditDate

from Production.dbo.Carrier as a inner join Trade_To_Pms.dbo.Carrier as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Carrier as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Carrier as a WITH (NOLOCK) where a.id = b.id)




--PayTerm
--PayTermAP    
--PayTermAR
----------------------�R���DTABLE�h�����
Delete Production.dbo.PayTermAP
from Production.dbo.PayTermAP as a left join Trade_To_Pms.dbo.PayTermAP as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID			      =b.ID
      a.Description			      =b.Description
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
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.PayTermAP as b WITH (NOLOCK)
where not exists(select id from Production.dbo.PayTermAP as a WITH (NOLOCK) where a.id = b.id)


-----------------PayTermAR 20161209 add
Merge Production.dbo.PayTermAR as t
Using Trade_TO_Pms.dbo.PayTermAR as s
	on t.id=s.id
	when matched then
		update set
		t.Description= s.Description,
		t.Term= s.Term,
		t.BeforeAfter= s.BeforeAfter,
		t.BaseDate= s.BaseDate,
		t.AccountDay= s.AccountDay,
		t.CloseAccountDay= s.CloseAccountDay,
		t.CloseMonth= s.CloseMonth,
		t.CloseDay= s.CloseDay,
		t.DueAccountday= s.DueAccountday,
		t.DueMonth= s.DueMonth,
		t.DueDay= s.DueDay,
		t.JUNK= s.JUNK,
		t.SamplePI= s.SamplePI,
		t.BulkPI= s.BulkPI,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
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
	values(s.ID,
s.Description,
s.Term,
s.BeforeAfter,
s.BaseDate,
s.AccountDay,
s.CloseAccountDay,
s.CloseMonth,
s.CloseDay,
s.DueAccountday,
s.DueMonth,
s.DueDay,
s.JUNK,
s.SamplePI,
s.BulkPI,
s.AddName,
s.AddDate,
s.EditName,
s.EditDate
)
	 when not matched by source then
	 delete;


--Mockup
--Mockup
----------------------�R���DTABLE�h�����
Delete Production.dbo.Mockup
from Production.dbo.Mockup as a left join Trade_To_Pms.dbo.Mockup as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID	 =b.ID	
      a.Description	      =b.Description	
      ,a.SMV	      =b.SMV	
      ,a.CPU	      =b.CPU	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Mockup as a inner join Trade_To_Pms.dbo.Mockup as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Mockup as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Mockup as a WITH (NOLOCK) where a.id = b.id)


--OrderType  ���μgDELETE
--OrderType  
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       -- a.ID	 =b.ID	
      --,a.BrandID	      =b.BrandID	
      a.PhaseID	      =b.PhaseID	
      ,a.ProjectID	      =b.ProjectID	
      ,a.KPIProjectID	      =b.KPIProjectID	
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
	  ,a.IsGMTMaster	  =b.IsGMTMaster
	  ,a.IsGMTDetail      =b.IsGMTDetail
	  ,a.IsDevSample      =b.IsDevSample
	  ,a.CalByBOFConsumption      =b.CalByBOFConsumption
from Production.dbo.OrderType as a inner join Trade_To_Pms.dbo.OrderType as b ON a.id=b.id and a.BrandID =b.BrandID
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.OrderType as b WITH (NOLOCK)
where not exists(select id from Production.dbo.OrderType as a WITH (NOLOCK) where a.id = b.id and a.BrandID =b.BrandID)


--CustPrg
--Program  
----------------------�R���DTABLE�h�����
Delete Production.dbo.Program
from Production.dbo.Program as a left join Trade_To_Pms.dbo.Program as b
on a.id = b.id and a.BrandID = b.BrandID
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
     -- a.ID	      =b.ID	
     -- ,a.BrandID	      =b.BrandID	
      a.RateCost	      =b.RateCost	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
	  ,a.MiAdidas         =b.MiAdidas

from Production.dbo.Program as a inner join Trade_To_Pms.dbo.Program as b ON a.id=b.id and a.BrandID = b.BrandID
-------------------------- INSERT INTO ��
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
 ID
      ,BrandID
      ,RateCost
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
	  ,MiAdidas
from Trade_To_Pms.dbo.Program as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Program as a WITH (NOLOCK) where a.id = b.id and a.BrandID = b.BrandID)


--FabFactor
--MtlFactor �L���P���
----------------------�R���DTABLE�h�����
Delete Production.dbo.MtlFactor
from Production.dbo.MtlFactor as a left join Trade_To_Pms.dbo.MtlFactor as b
on a.id = b.id and a.Type = b.Type
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
     --  a.Type	     =b.Type	
      --,a.ID	      =b.ID	
      a.Pattern	      =b.Pattern	
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
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.MtlFactor as b WITH (NOLOCK)
where not exists(select id from Production.dbo.MtlFactor as a WITH (NOLOCK) where a.id = b.id and a.Type = b.Type)


--Cstrutype
--Construction �L���P���
----------------------�R���DTABLE�h�����
Delete Production.dbo.Construction
from Production.dbo.Construction as a left join Trade_To_Pms.dbo.Construction as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.Id		     =b.Id	
      a.Name		      =b.Name	
      ,a.CuttingLayer		      =b.CuttingLayer	
      ,a.Junk		      =b.Junk	
      ,a.AddName		      =b.AddName	
      ,a.AddDate		      =b.AddDate	
      ,a.EditName		      =b.EditName	
      ,a.EditDate		      =b.EditDate	

from Production.dbo.Construction as a inner join Trade_To_Pms.dbo.Construction as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Construction as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Construction as a WITH (NOLOCK) where a.id = b.id)


--Port
--Port �L�h�����
----------------------�R���DTABLE�h�����
Delete Production.dbo.Port
from Production.dbo.Port as a left join Trade_To_Pms.dbo.Port as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
     --  a.ID	     =b.ID	
      a.CountryID	      =b.CountryID	
      ,a.Remark	      =b.Remark	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.Junk	      =b.Junk	
      ,a.Name	      =b.Name	
      ,a.AirPort	  =b.AirPort	
      ,a.SeaPort	  =b.SeaPort
from Production.dbo.Port as a inner join Trade_To_Pms.dbo.Port as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
select ID
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
from Trade_To_Pms.dbo.Port as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Port as a WITH (NOLOCK) where a.id = b.id)

--Formlist ���L �ثe�L��XTABLE
--DO releasememvar WITH 'FS_CMTPlus'
----------------------�R���DTABLE�h�����
Delete Production.dbo.FSRCpuCost
from Production.dbo.FSRCpuCost as a left join Trade_To_Pms.dbo.FSRCpuCost as b
on a.ShipperID = b.ShipperID
where b.ShipperID is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ShipperID	    =b.ShipperID	
      a.AddDate	      =b.AddDate	
      ,a.AddName	      =b.AddName	
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =b.EditName	

from Production.dbo.FSRCpuCost as a inner join Trade_To_Pms.dbo.FSRCpuCost as b ON a.ShipperID=b.ShipperID
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.FSRCpuCost as b WITH (NOLOCK)
where not exists(select ShipperID from Production.dbo.FSRCpuCost as a WITH (NOLOCK) where a.ShipperID = b.ShipperID)


--DO releasememvar WITH 'FS_CMTPlus1'
	----------------------�R���DTABLE�h�����
Delete Production.dbo.FSRCpuCost_Detail
from Production.dbo.FSRCpuCost_Detail as a left join Trade_To_Pms.dbo.FSRCpuCost_Detail as b
on a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate
where b.ShipperID is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ShipperID	    =b.ShipperID	
     -- ,a.BeginDate	      =b.BeginDate	
    --  ,a.EndDate	      =b.EndDate	
      a.CpuCost	      =b.CpuCost	
      ,a.AddDate	      =b.AddDate	
      ,a.AddName	      =b.AddName	
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =b.EditName	

from Production.dbo.FSRCpuCost_Detail as a inner join Trade_To_Pms.dbo.FSRCpuCost_Detail as b ON a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.FSRCpuCost_Detail as b WITH (NOLOCK)
where not exists(select ShipperID from Production.dbo.FSRCpuCost_Detail as a WITH (NOLOCK) where a.ShipperID = b.ShipperID AND a.BeginDate  =b.BeginDate AND  a.EndDate =b.EndDate)



--DO releasememvar WITH 'FtyShipper'  FtyShipper
----------------------�R���DTABLE�h�����
Delete Production.dbo.FtyShipper
from Production.dbo.FtyShipper as a left join Trade_To_Pms.dbo.FtyShipper as b  
on a.BrandID = b.BrandID and a.FactoryID=b.FactoryID
where b.BrandID is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.BrandID	    =b.BrandID	
     -- ,a.FactoryID	      =b.FactoryID	
      a.AddDate	      =b.AddDate	
      ,a.AddName	      =b.AddName	
      ,a.EditDate	      =b.EditDate	
      ,a.EditName	      =b.EditName	

from Production.dbo.FtyShipper as a inner join Trade_To_Pms.dbo.FtyShipper as b ON a.BrandID = b.BrandID and a.FactoryID=b.FactoryID

-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.FtyShipper as b WITH (NOLOCK)
where not exists(select BrandID from Production.dbo.FtyShipper as a WITH (NOLOCK) where a.BrandID = b.BrandID and a.FactoryID=b.FactoryID	)

--FtyShipper_Detail
--DO releasememvar WITH 'FtyShipper1'
----------------------�R���DTABLE�h�����
Delete Production.dbo.FtyShipper_Detail
from Production.dbo.FtyShipper_Detail as a left join Trade_To_Pms.dbo.FtyShipper_Detail as b
on a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate  and a.SeasonID	=b.SeasonID 
where b.BrandID is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.BrandID	 =b.BrandID	
      --,a.FactoryID	      =b.FactoryID	
      --,a.BeginDate	      =b.BeginDate	
      a.EndDate	      =b.EndDate	
      ,a.ShipperID	      =b.ShipperID	

from Production.dbo.FtyShipper_Detail as a
inner join Trade_To_Pms.dbo.FtyShipper_Detail as b ON a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate  and a.SeasonID	=b.SeasonID
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.FtyShipper_Detail(
       BrandID
      ,FactoryID
      ,BeginDate
      ,EndDate
      ,ShipperID
	  ,SeasonID
)
select 
       BrandID
      ,FactoryID
      ,BeginDate
      ,EndDate
      ,ShipperID
	  ,SeasonID
from Trade_To_Pms.dbo.FtyShipper_Detail as b WITH (NOLOCK)
where not exists(select BrandID from Production.dbo.FtyShipper_Detail as a WITH (NOLOCK) 
where a.BrandID = b.BrandID and a.FactoryID =b.FactoryID  and a.BeginDate	=b.BeginDate and a.SeasonID	=b.SeasonID)

--Phrase1
--Phrase �L�h���
----------------------�R���DTABLE�h�����
Delete Production.dbo.Phrase
from Production.dbo.Phrase as a left join Trade_To_Pms.dbo.Phrase as b
on a.PhraseTypeName = b.PhraseTypeName and a.Name = b.Name
where b.PhraseTypeName is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.PhraseTypeName	 =b.PhraseTypeName	
      a.Junk	      =b.Junk	
     -- ,a.Name	      =b.Name	
      ,a.Seq	      =b.Seq	
      ,a.Description	      =b.Description	
      ,a.Module	      =b.Module	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.Phrase as a inner join Trade_To_Pms.dbo.Phrase as b ON a.PhraseTypeName=b.PhraseTypeName and a.Name = b.Name
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Phrase as b WITH (NOLOCK)
where not exists(select PhraseTypeName from Production.dbo.Phrase as a WITH (NOLOCK) where a.PhraseTypeName = b.PhraseTypeName and a.Name = b.Name)


--Company
--Company �L�h������
----------------------�R���DTABLE�h�����
Delete Production.dbo.Company
from Production.dbo.Company as a left join Trade_To_Pms.dbo.Company as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ID	      =b.ID	
      a.Title	      =b.Title	
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
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.Company as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Company as a WITH (NOLOCK) where a.id = b.id)

--MiFabCode
--ADIDASMiSetup_ColorComb �L�h�����
----------------------�R���DTABLE�h�����
Delete Production.dbo.ADIDASMiSetup_ColorComb
from Production.dbo.ADIDASMiSetup_ColorComb as a left join Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ID	      =b.ID	
      a.ExcelName	      =b.ExcelName	
      ,a.ExcelColumn	      =b.ExcelColumn	
      ,a.isArtwork	      =b.isArtwork	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.ADIDASMiSetup_ColorComb as a inner join Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b ON a.id=b.id
-------------------------- INSERT INTO ��
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
from Trade_To_Pms.dbo.ADIDASMiSetup_ColorComb as b WITH (NOLOCK)
where not exists(select id from Production.dbo.ADIDASMiSetup_ColorComb as a WITH (NOLOCK) where a.id = b.id)


--------------------------ShipMode-------

Merge Production.dbo.ShipMode as t
using Trade_To_Pms.dbo.ShipMode as s
on t.id=s.id
	when matched then
		update set 
		t.ID= s.ID,
		t.Description= s.Description,
		t.UseFunction= s.UseFunction,
		t.Junk= s.Junk,
		t.ShareBase= s.ShareBase,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
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
				values(s.ID,
		s.Description,
		s.UseFunction,
		s.Junk,
		s.ShareBase,
		s.AddName,
		s.AddDate,
		s.EditName,
		s.EditDate,
		s.ID
		)
	when not matched by source then
		delete;



--------------------------DropDownList--------

Merge Production.dbo.DropDownList as t
Using Trade_To_Pms.dbo.DropDownList as s
on t.type=s.type and t.id=s.id
	when matched then
	update set
		t.Type= s.Type,
		t.ID= s.ID,
		t.Name= s.Name,
		t.RealLength= s.RealLength,
		t.Description= s.Description,
		t.Seq= s.Seq
	when not matched by target then
		insert(Type
				,ID
				,Name
				,RealLength
				,Description
				,Seq
				)
						values(s.Type,
				s.ID,
				s.Name,
				s.RealLength,
				s.Description,
				s.Seq
				)
	when not matched by source then
		delete;

	---------KeyWord--------------

	Merge Production.dbo.KeyWord as t
	Using Trade_To_Pms.dbo.KeyWord as s
	on t.id=s.id
		when matched then
		update set 
		t.Description= s.Description,
		t.Junk= s.Junk,
		t.Prefix= s.Prefix,
		t.Fieldname= s.Fieldname,
		t.Postfix= s.Postfix,
		t.IsSize= s.IsSize,
		t.IsPatternPanel= s.IsPatternPanel,
		t.AddName= s.AddName,
		t.AddDate= s.AddDate,
		t.EditName= s.EditName,
		t.EditDate= s.EditDate
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
		)
			values(s.ID,
		s.Description,
		s.Junk,
		s.Prefix,
		s.Fieldname,
		s.Postfix,
		s.IsSize,
		s.IsPatternPanel,
		s.AddName,
		s.AddDate,
		s.EditName,
		s.EditDate
		)
		when not matched by source then
			delete;
  
		--Fabric_Supp
		---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
		UPDATE a
		SET  
			   a.AbbCH	       = b.AbbCH	      
			  ,a.AbbEN	       = b.AbbEN	      
			  ,a.AddDate	   = b.AddDate	  
			  ,a.AddName	   = b.AddName	  
			  ,a.AllowanceRate = b.AllowanceRate
			  ,a.AllowanceType = b.AllowanceType
			  ,a.BrandID	   = b.BrandID	  
			  ,a.Delay		   = b.Delay		  
			  ,a.DelayMemo	   = b.DelayMemo	  
			  ,a.EditDate	   = b.EditDate	  
			  ,a.EditName	   = b.EditName	  
			  ,a.IsECFA		   = b.IsECFA		  
			  ,a.ItemType	   = b.ItemType	  
			  ,a.Junk		   = b.Junk		  
			  ,a.Keyword	   = b.Keyword	  
			  ,a.Lock		   = b.Lock		  
			  ,a.LTDay		   = b.LTDay		  
			  ,a.NOForecast	   = b.NOForecast	  
			  ,a.OldSys_Ukey   = b.OldSys_Ukey  
			  ,a.OldSys_Ver	   = b.OldSys_Ver	  
			  ,a.OrganicCotton = b.OrganicCotton
			  ,a.POUnit		   = b.POUnit		  
			  ,a.PreShrink	   = b.PreShrink	  
			  ,a.Refno		   = b.Refno		  
			  ,a.Remark		   = b.Remark		  
			  ,a.SeasonID	   = b.SeasonID	  
			  ,a.ShowSuppColor = b.ShowSuppColor
			  ,a.SuppRefno	   = b.SuppRefno
			  ,a.SustainableMaterial	   = b.SustainableMaterial

		from Production.dbo.Fabric_Supp as a 
		inner join Trade_To_Pms.dbo.Fabric_Supp as b ON a.SuppID=b.SuppID and a.SCIRefno = b.SCIRefno
		-------------------------- INSERT INTO ��
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
				,Keyword	  
				,Lock		  
				,LTDay		  
				,NOForecast	  
				,OldSys_Ukey  
				,OldSys_Ver	  
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
				,SustainableMaterial
		)
		select 
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
				,Keyword	  
				,Lock		  
				,LTDay		  
				,NOForecast	  
				,OldSys_Ukey  
				,OldSys_Ver	  
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
				,SustainableMaterial

		from Trade_To_Pms.dbo.Fabric_Supp as b WITH (NOLOCK)
		where not exists(select SuppID from Production.dbo.Fabric_Supp as a WITH (NOLOCK) where a.SuppID = b.SuppID and a.SCIRefno = b.SCIRefno)

--------Color_Multiple---------------

Merge Production.dbo.Color_Multiple as t
Using Trade_To_Pms.dbo.Color_Multiple as s
on t.colorUkey=s.colorUkey and t.Seqno=s.Seqno
	when matched then
	update set
	t.ID= s.ID,	
	t.BrandID= s.BrandID,	
	t.ColorID= s.ColorID,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
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
	values(s.ID,
	s.ColorUkey,
	s.BrandID,
	s.Seqno,
	s.ColorID,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate	)
when not matched by source then
	delete;


--------Color_SuppColor---------------

Merge Production.dbo.Color_SuppColor as t
Using Trade_To_Pms.dbo.Color_SuppColor as s
on t.ukey=s.ukey
when matched then
	update set
	t.ID= s.ID,
	t.Ukey= s.Ukey,
	t.BrandId= s.BrandId,
	t.ColorUkey= s.ColorUkey,
	t.SeasonID= s.SeasonID,
	t.SuppID= s.SuppID,
	t.SuppColor= s.SuppColor,
	t.ProgramID= s.ProgramID,
	t.StyleID= s.StyleID,
	t.Refno= s.Refno,
	t.Remark= s.Remark,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
	t.EditDate= s.EditDate
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
	)
	values(s.ID,
	s.Ukey,
	s.BrandId,
	s.ColorUkey,
	s.SeasonID,
	s.SuppID,
	s.SuppColor,
	s.ProgramID,
	s.StyleID,
	s.Refno,
	s.Remark,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate)
when not matched by source then 
	delete;

----update system.ProphetSingleSizeDeduct���
update Production.dbo.System
set ProphetSingleSizeDeduct =
(select ProphetSingleSizeDeduct from Trade_To_Pms.dbo.Tradesystem s)

	
--------Buyer---------------

Merge Production.dbo.Buyer as t
Using Trade_To_Pms.dbo.Buyer as s
on t.ID=s.ID
when matched then
	update set
	t.CountryID= s.CountryID,
	t.NameCH= s.NameCH,
	t.NameEN= s.NameEN,
	t.Tel= s.Tel,
	t.Fax= s.Fax,
	t.Contact1= s.Contact1,
	t.Contact2= s.Contact2,
	t.AddressCH= s.AddressCH,
	t.AddressEN= s.AddressEN,
	t.BillTo1= s.BillTo1,
	t.BillTo2= s.BillTo2,
	t.BillTo3= s.BillTo3,
	t.BillTo4= s.BillTo4,
	t.BillTo5= s.BillTo5,
	t.CurrencyID= s.CurrencyID,
	t.Remark= s.Remark,
	t.ZipCode= s.ZipCode,
	t.Email= s.Email,
	t.MrTeam= s.MrTeam,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
	t.EditDate= s.EditDate,
	t.Junk= s.Junk
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
	values(s.ID,
	s.CountryID,
	s.NameCH,
	s.NameEN,
	s.Tel,
	s.Fax,
	s.Contact1,
	s.Contact2,
	s.AddressCH,
	s.AddressEN,
	s.BillTo1,
	s.BillTo2,
	s.BillTo3,
	s.BillTo4,
	s.BillTo5,
	s.CurrencyID,
	s.Remark,
	s.ZipCode,
	s.Email,
	s.MrTeam,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate,
	s.Junk)
when not matched by source then 
	delete;	


--------CutReason---------------

Merge Production.dbo.CutReason as t
Using Trade_To_Pms.dbo.CutReason as s
on t.Type=s.Type and t.ID=s.ID 
when matched then
	update set
	t.Description= s.Description,
	t.Remark= s.Remark,
	t.Junk= s.Junk,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
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
	values(s.Type,
	s.ID,
	s.Description,
	s.Remark,
	s.Junk,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate)
when not matched by source then 
	delete;	

--------IEReason---------------

Merge Production.dbo.IEReason as t
Using Trade_To_Pms.dbo.IEReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= s.Description,
	t.Junk= s.Junk,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
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
	values(s.Type,
	s.ID,
	s.Description,
	s.Junk,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate)
when not matched by source then 
	delete;	


--------PackingReason---------------

Merge Production.dbo.PackingReason as t
Using Trade_To_Pms.dbo.PackingReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= s.Description,
	t.Junk= s.Junk,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
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
	values(s.Type,
	s.ID,
	s.Description,
	s.Junk,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate)
when not matched by source and t.type <> 'ER' then 
	delete;	

--------PPICReason---------------

Merge Production.dbo.PPICReason as t
Using Trade_To_Pms.dbo.PPICReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= s.Description,
	t.Remark= s.Remark,
	t.Junk= s.Junk,
	t.TypeForUse= s.TypeForUse,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
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
	values(s.Type,
	s.ID,
	s.Description,
	s.Remark,
	s.Junk,
	s.TypeForUse,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate)
when not matched by source then 
	delete;	


--------SubProcess---------------
delete Production.dbo.AutomationSubProcess

insert into Production.dbo.AutomationSubProcess(ID)
select s.ID
from Trade_To_Pms.dbo.SubProcess s with (nolock)
left Join Production.dbo.SubProcess t with (nolock) on s.ID = t.ID
where s.Junk <> t.Junk or t.Junk is null

Merge Production.dbo.SubProcess as t
Using Trade_To_Pms.dbo.SubProcess as s
on t.ID=s.ID
when matched then
	update set
	t.ArtworkTypeId= s.ArtworkTypeId,
	t.IsSelection= s.IsSelection,
	t.IsRFIDProcess= s.IsRFIDProcess,
	t.IsRFIDDefault= s.IsRFIDDefault,
	t.ShowSeq= s.ShowSeq,
	t.Junk= s.Junk,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
	t.EditDate= s.EditDate,
	t.BCSDate= s.BCSDate,
	t.InOutRule  = s.InOutRule ,
	t.FullName  = s.FullName ,
	t.IsLackingAndReplacement  = s.IsLackingAndReplacement,
	t.IsBoundedProcess  = s.IsBoundedProcess ,
	t.IsSubprocessInspection  = s.IsSubprocessInspection 

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
	values(s.ID,
	s.ArtworkTypeId,
	s.IsSelection,
	s.IsRFIDProcess,
	s.IsRFIDDefault,
	s.ShowSeq,
	s.Junk,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate,
	s.BCSDate,
	s.InOutRule,
	s.FullName,
	s.IsLackingAndReplacement,
	s.IsBoundedProcess,
	s.IsSubprocessInspection)
when not matched by source then 
	delete;	


--------WhseReason---------------

Merge Production.dbo.WhseReason as t
Using Trade_To_Pms.dbo.WhseReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= s.Description,
	t.Remark= s.Remark,
	t.Junk= s.Junk,
	t.ActionCode= s.ActionCode,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
	t.EditDate= s.EditDate,
	t.No= s.No
when not matched by target then
	insert(Type
	,ID
	,Description
	,Remark
	,Junk
	,ActionCode
	,AddName
	,AddDate
	,EditName
	,EditDate
	,No
	)
	values(s.Type,
	s.ID,
	s.Description,
	s.Remark,
	s.Junk,
	s.ActionCode,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate,
	s.No)
when not matched by source then 
	delete;	

--------ClogReason---------------

Merge Production.dbo.ClogReason as t
Using Trade_To_Pms.dbo.ClogReason as s
on t.Type=s.Type and t.ID=s.ID
when matched then
	update set
	t.Description= s.Description,
	t.Remark= s.Remark,
	t.Junk= s.Junk,
	t.AddName= s.AddName,
	t.AddDate= s.AddDate,
	t.EditName= s.EditName,
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
	values(s.Type,
	s.ID,
	s.Description,
	s.Remark,
	s.Junk,
	s.AddName,
	s.AddDate,
	s.EditName,
	s.EditDate)
when not matched by source then 
	delete;	

--------ThreadAllowanceScale---------------

Merge Production.dbo.ThreadAllowanceScale as t
Using Trade_To_Pms.dbo.ThreadAllowanceScale as s
on t.ID = s.ID
when matched then
      update set
      t.LowerBound = s.LowerBound,
      t.UpperBound = s.UpperBound,
      t.Allowance = s.Allowance,
      t.Remark = s.Remark,
      t.AddName = s.AddName,
      t.AddDate= s.AddDate,
      t.EditName = s.EditName,
      t.EditDate = s.EditDate
when not matched by target then
      insert (
            ID , LowerBound  , UpperBound  , Allowance  , Remark    , AddName
            , AddDate   , EditName    , EditDate
      ) values (
           s.ID, s.LowerBound, s.UpperBound, s.Allowance, s.Remark  , s.AddName
            , s.AddDate , s.EditName  , s.EditDate
      )
when not matched by source then 
      delete;     

--------SubconReason---------------

Merge Production.dbo.SubconReason as t
Using Trade_To_Pms.dbo.SubconReason as s
on t.ID = s.ID and t.Type = s.Type
when matched then
      update set	t.Reason		 = s.Reason		 ,
					t.Responsible	 = s.Responsible ,
					t.Junk			 = s.Junk		 ,
					t.AddDate		 = s.AddDate	 ,
					t.AddName		 = s.AddName	 ,
					t.EditDate		 = s.EditDate	 ,
					t.EditName		 = s.EditName	 ,
					t.Status		 = s.Status
when not matched by target then
      insert (Type,ID,Reason,Responsible,Junk,AddDate,AddName,EditDate,EditName ,Status
      ) values (s.Type,s.ID,s.Reason,s.Responsible,s.Junk,s.AddDate,s.AddName,s.EditDate,s.EditName ,Status
      )
when not matched by source then 
      delete;     

--------SewingReason---------------

Merge Production.dbo.SewingReason as sr
Using Trade_To_Pms.dbo.SewingReason as tsr
on sr.ID = tsr.ID AND sr.Type=tsr.Type
when matched then
      update set
       sr.Type = tsr.Type,
       sr.ID = tsr.ID,
       sr.Description= tsr.Description,
       sr.Junk = tsr.Junk,
       sr.AddName = tsr.AddName,
       sr.AddDate = tsr.AddDate,
       sr.EditName = tsr.EditName,
       sr.EditDate = tsr.EditDate,
       sr.ForDQSCheck = tsr.ForDQSCheck

when not matched by target then
      insert (
            Type , ID  , Description  , Junk  , AddName    , AddDate
            , EditName   , EditDate   , ForDQSCheck  
      ) values (
            tsr.Type , tsr.ID  , tsr.Description  , tsr.Junk  , tsr.AddName    , tsr.AddDate
            , tsr.EditName   , tsr.EditDate		  , tsr.ForDQSCheck  
      )
when not matched by source then 
      delete;     

--------GMTBooking---------------
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET   
      a.Vessel     =b.Vessel
      ,a.ETD       =b.ETD 
      ,a.ETA       =b.ETA 
      ,a.InvDate   =b.InvDate 
      ,a.FCRDate   =b.FCRDate 
	  ,a.BLNo	   =b.BLNo
	  ,a.BL2No	   =b.BL2No
	  ,a.InvoiceApproveDate	   =b.InvoiceApproveDate
	  ,a.IntendDeliveryDate	   =b.IntendDeliveryDate
	  ,a.ActFCRDate	   =b.ActFCRDate
from Production.dbo.GMTBooking as a inner join Trade_To_Pms.dbo.GarmentInvoice as b ON a.id=b.id
where b.InvDate is not null

UPDATE a
SET   
     a.Foundry = b.Foundry
from Production.dbo.GMTBooking as a 
inner join Trade_To_Pms.dbo.GarmentInvoice as b ON a.id=b.id

----GarmentInvoice_Foundry


UPDATE t
SET 
	 t.GW = s.GW
	,t.CBM=s.CBM
	,t.Ratio=s.Ratio
FROM Production.dbo.GarmentInvoice_Foundry t WITH (NOLOCK)
inner join Trade_To_Pms.dbo.GarmentInvoice_Foundry s WITH (NOLOCK) on t.InvoiceNo=s.InvoiceNo and t.FactoryGroup=s.FactoryGroup

INSERT INTO Production.dbo.GarmentInvoice_Foundry 
	  (InvoiceNo,FactoryGroup,GW,CBM,Ratio)
select InvoiceNo,FactoryGroup,GW,CBM,Ratio 
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
select [ID],[Month],[AddName],[AddDate],[EditName],[EditDate],[Junk] from Trade_To_Pms.dbo.SeasonSCI


------FirstSaleCostSetting---------------
Merge Production.dbo.FirstSaleCostSetting as sr
Using Trade_To_Pms.dbo.FirstSaleCostSetting as tsr 
on sr.CountryID = tsr.CountryID AND sr.ArtWorkID=tsr.ArtWorkID AND sr.CostTypeID=tsr.CostTypeID AND sr.BeginDate=tsr.BeginDate
when matched then
      update set 
       sr.EndDate = tsr.EndDate, 
	   sr.IsJunk = tsr.IsJunk, 
	   sr.AddDate = tsr.AddDate, 
	   sr.AddName = tsr.AddName, 
	   sr.EditDate = tsr.EditDate, 
	   sr.EditName = tsr.EditName
when not matched by target then
      insert (CountryID, ArtWorkID, CostTypeID, BeginDate, EndDate, IsJunk, AddDate, AddName, EditDate, EditName) 
	  values (tsr.CountryID, tsr.ArtWorkID, tsr.CostTypeID, tsr.BeginDate, tsr.EndDate, tsr.IsJunk, tsr.AddDate, tsr.AddName, tsr.EditDate,tsr.EditName)
when not matched by source then 
      delete;

------Brand_ThreadCalculateRules---------------
Merge Production.dbo.Brand_ThreadCalculateRules as t
Using (select a.* from Trade_To_Pms.dbo.Brand_ThreadCalculateRules a ) as s
on t.ID=s.ID and t.FabricType = s.FabricType and t.ProgramID = s.ProgramID
when matched then 
	update set	t.UseRatioRule	= s.UseRatioRule,
				t.UseRatioRule_Thick	= s.UseRatioRule_Thick
when not matched by target then
	insert (ID,
			FabricType,
			UseRatioRule,
			UseRatioRule_Thick,
			ProgramID
			) 
		values (s.ID,
				s.FabricType,
				s.UseRatioRule,
				s.UseRatioRule_Thick,
				s.ProgramID	)
when not matched by source then 
	delete;

------MachineType_ThreadRatio---------------
Merge Production.dbo.MachineType_ThreadRatio as t
Using (select a.* from Trade_To_Pms.dbo.MachineType_ThreadRatio a ) as s
on t.ID=s.ID and t.SEQ = s.SEQ
when matched then 
	update set	t.ThreadLocation	   = s.ThreadLocation	 ,
				t.UseRatio			   = s.UseRatio			 ,
				t.Allowance			   = s.Allowance		 ,
				t.AllowanceTubular     = s.AllowanceTubular
when not matched by target then
	insert (ID				 ,
			SEQ				 ,
			ThreadLocation	 ,
			UseRatio	     ,
			Allowance        ,
			AllowanceTubular
			) 
		values (s.ID				 ,
				s.SEQ				 ,
				s.ThreadLocation	 ,
				s.UseRatio			 ,
				s.Allowance			 ,
				s.AllowanceTubular
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
	t.UseRatio = s.UseRatio,
	t.ukey = s.ukey
when not matched by target then
	insert(ID,Seq,UseRatioRule,UseRatio,Ukey)
	values(s.ID,s.Seq,s.UseRatioRule,s.UseRatio,s.Ukey)
when not matched by source then
	delete;

------MachineType_ThreadRatio_Regular---------------
Merge Production.dbo.MachineType_ThreadRatio_Regular as t
Using (select a.* from Trade_To_Pms.dbo.MachineType_ThreadRatio_Regular a ) as s
on t.ID=s.ID and t.SEQ = s.SEQ and t.UseRatioRule = s.UseRatioRule
when matched then 
	update set	t.UseRatio	   = s.UseRatio	 
when not matched by target then
	insert (ID				,
			Seq				,
			UseRatioRule	,
			UseRatio
			) 
		values (s.ID				,
				s.Seq				,
				s.UseRatioRule	,
				s.UseRatio)
when not matched by source then 
	delete;	



------FreightCollectByCustomer---------------
Merge Production.dbo.FreightCollectByCustomer as t
Using (select a.* from Trade_To_Pms.dbo.FreightCollectByCustomer a ) as s
on t.[Dest]=s.[Dest] and t.[BrandID] = s.[BrandID] and t.[CarrierID] = s.[CarrierID] and t.[Account]=s.[Account]
when matched then 
	update set	
		 t.[CustCDID]		=s.[CustCDID]	
		,t.[DestPort]		=s.[DestPort]	
		,t.[OrderTypeID]	=s.[OrderTypeID]
		,t.[Remarks]		=s.[Remarks]	
		,t.[AddDate]		=s.[AddDate]	
		,t.[AddName]		=s.[AddName]	
		,t.[EditDate]		=s.[EditDate]	
		,t.[EditName] 		=s.[EditName] 	
when not matched by target then
	insert ([BrandID],[Dest],[CarrierID],[Account],[CustCDID],[DestPort],[OrderTypeID],[Remarks],[AddDate],[AddName],[EditDate],[EditName]) 
	values (s.[BrandID],s.[Dest],s.[CarrierID],s.[Account],s.[CustCDID],s.[DestPort],s.[OrderTypeID],s.[Remarks],s.[AddDate],s.[AddName],s.[EditDate],s.[EditName])
when not matched by source then 
	delete;	
	
------[Carrier_Detail_Freight]---------------
Merge Production.dbo.[Carrier_Detail_Freight] as t
Using (select a.* from Trade_To_Pms.dbo.[Carrier_Detail_Freight] a ) as s
on t.[ID]=s.[ID] and t.[Ukey] = s.[Ukey]
when matched then 
	update set	
		 t.[Payer]		=s.[Payer]
		,t.[FromTag]	=s.[FromTag]
		,t.[FromInclude]=s.[FromInclude]
		,t.[FromExclude]=s.[FromExclude]
		,t.[ToTag]		=s.[ToTag]
		,t.[ToInclude]	=s.[ToInclude]
		,t.[ToExclude]	=s.[ToExclude]
		,t.[ToFty]		=s.[ToFty]
		,t.[AddName]	=s.[AddName]
		,t.[AddDate]	=s.[AddDate]
		,t.[EditName]	=s.[EditName]
		,t.[EditDate]	=s.[EditDate]	
when not matched by target then
	insert ([ID],[Payer],[FromTag],[FromInclude],[FromExclude],[ToTag],[ToInclude],[ToExclude],[ToFty],[AddName],[AddDate],[EditName],[EditDate])
	values (s.[ID],s.[Payer],s.[FromTag],s.[FromInclude],s.[FromExclude],s.[ToTag],s.[ToInclude],s.[ToExclude],s.[ToFty],s.[AddName],s.[AddDate],s.[EditName],s.[EditDate])
when not matched by source then 
	delete;	


-------FactoryExpress_SendingSchedule
UPDATE a
SET 
a.[Seq]			=b.[Seq],
a.[Country]		=b.[Country],
a.[RegionCode]	=b.[RegionCode],
a.[ToID]		=b.[ToID],
a.[ToAlias]		=b.[ToAlias],
a.[BeginDate]	=b.[BeginDate],
a.[SUN]			=b.[SUN],
a.[MON]			=b.[MON],
a.[TUE]			=b.[TUE],
a.[WED]			=b.[WED],
a.[THU]			=b.[THU],
a.[FRI]			=b.[FRI],
a.[SAT]			=b.[SAT],
a.[Junk]		=b.[Junk],
a.[AddName]		=b.[AddName],
a.[AddDate]		=b.[AddDate],
a.[EditName]	=b.[EditName],
a.[EditDate]	=b.[EditDate]
from Production.dbo.FactoryExpress_SendingSchedule as a 
inner join Trade_To_Pms.dbo.FactoryExpress_SendingSchedule as b 
	ON a.RegionCode=b.RegionCode and a.ToID = b.ToID
-------------------------- INSERT INTO ��
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
SELECT [Seq]
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
from Trade_To_Pms.dbo.FactoryExpress_SendingSchedule as b WITH (NOLOCK)
where not exists(
	select 1 
	from Production.dbo.FactoryExpress_SendingSchedule as a WITH (NOLOCK) 
	where a.RegionCode=b.RegionCode 
	and a.ToID = b.ToID 
)

----------------------�R���DTABLE�h�����
Delete Production.dbo.FactoryExpress_SendingSchedule
from Production.dbo.FactoryExpress_SendingSchedule as a 
left join Trade_To_Pms.dbo.FactoryExpress_SendingSchedule as b
on a.RegionCode=b.RegionCode and a.ToID = b.ToID
where b.ToID is null


-------FactoryExpress_SendingScheduleHistory
UPDATE a
SET 
a.[Ukey]		=b.[Ukey],
a.[Country]		=b.[Country],
a.[RegionCode]	=b.[RegionCode],
a.[ToID]		=b.[ToID],
a.[ToAlias]		=b.[ToAlias],
a.[BeginDate]	=b.[BeginDate],
a.[EndDate]	=b.[EndDate],
a.[SUN]			=b.[SUN],
a.[MON]			=b.[MON],
a.[TUE]			=b.[TUE],
a.[WED]			=b.[WED],
a.[THU]			=b.[THU],
a.[FRI]			=b.[FRI],
a.[SAT]			=b.[SAT],
a.[AddName]		=b.[AddName],
a.[AddDate]		=b.[AddDate],
a.[EditName]	=b.[EditName],
a.[EditDate]	=b.[EditDate]
from Production.dbo.FactoryExpress_SendingScheduleHistory as a 
inner join Trade_To_Pms.dbo.FactoryExpress_SendingScheduleHistory as b 
	ON a.Ukey=b.Ukey
-------------------------- INSERT INTO ��
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
SELECT [Ukey]
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
from Trade_To_Pms.dbo.FactoryExpress_SendingScheduleHistory as b WITH (NOLOCK)
where not exists(
	select 1 
	from Production.dbo.FactoryExpress_SendingScheduleHistory as a WITH (NOLOCK) 
	where a.Ukey=b.Ukey 
)


----------------------�R���DTABLE�h�����
Delete Production.dbo.FactoryExpress_SendingScheduleHistory
from Production.dbo.FactoryExpress_SendingScheduleHistory as a 
left join Trade_To_Pms.dbo.FactoryExpress_SendingScheduleHistory as b
on a.Ukey=b.Ukey 
where b.ToID is null



-------FIR_Grade-------
UPDATE a
SET  a.WeaveTypeID	= b.WeaveTypeID
    ,a.Percentage	= b.Percentage
    ,a.Grade		= b.Grade
    ,a.Result		= b.Result
    ,a.BrandID		= b.BrandID
FROM Production.dbo.FIR_Grade a 
INNER JOIN Trade_To_Pms.dbo.FIR_Grade as b  
ON a.WeaveTypeID=b.WeaveTypeID AND a.Percentage=b.Percentage AND a.BrandID=b.BrandID 


INSERT INTO Production.dbo.FIR_Grade
           (WeaveTypeID
           ,Percentage
           ,Grade
           ,Result
           ,BrandID)
SELECT   WeaveTypeID
		,Percentage
		,Grade
		,Result
		,BrandID
FROM Trade_To_Pms.dbo.FIR_Grade b
WHERE NOT EXISTS(
	SELECT  1
	FROM Production.dbo.FIR_Grade a WITH (NOLOCK)
	WHERE a.WeaveTypeID=b.WeaveTypeID AND a.Percentage=b.Percentage AND a.BrandID=b.BrandID 
)

DELETE Production.dbo.FIR_Grade
FROM Production.dbo.FIR_Grade a
LEFT JOIN Trade_To_Pms.dbo.FIR_Grade b ON a.WeaveTypeID=b.WeaveTypeID AND a.Percentage=b.Percentage AND a.BrandID=b.BrandID 
WHERE b.Grade is null


-----FtyStdRate_PRT-----
update tar set
 tar.Length    = S.Length    
,tar.Width	   = S.Width	   
,tar.Surcharge = S.Surcharge 
,tar.Price	   = S.Price	   
,tar.Ratio	   = S.Ratio	   
,tar.SEQ	   = S.SEQ	   
,tar.AddName   = S.AddName   
,tar.AddDate   = S.AddDate   
,tar.EditName  = S.EditName  
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
 tar.EndRange      = S.EndRange
,tar.BasedStitches = S.BasedStitches
,tar.BasedPay	   = S.BasedPay
,tar.AddedStitches = S.AddedStitches
,tar.AddedPay	   = S.AddedPay
,tar.ThreadRatio   = S.ThreadRatio
,tar.Ratio		   = S.Ratio
,tar.AddName	   = S.AddName
,tar.AddDate	   = S.AddDate
,tar.EditName	   = S.EditName
,tar.EditDate	   = S.EditDate
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

Merge Production.dbo.AccountNoSetting as t
Using Trade_To_Pms.dbo.AccountNoSetting as s
on t.ID=s.ID
when matched then
	update set
	t.UnselectableShipB03= s.UnselectableShipB03
	,t.AddDate= s.AddDate
	,t.AddName= s.AddName
	,t.EditDate= s.EditDate
	,t.EditName= s.EditName
when not matched by target then
	insert(ID, UnselectableShipB03 ,AddDate ,AddName ,EditDate ,EditName)
	values(s.ID, s.UnselectableShipB03 ,s.AddDate ,s.AddName ,s.EditDate ,s.EditName)
when not matched by source then 
	delete;	


--SubProDefectCode 
update tar set
 tar.Junk          = S.Junk
,tar.Description   = S.Description
,tar.AddName	   = S.AddName
,tar.AddDate	   = S.AddDate
,tar.EditName	   = S.EditName
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
	[SubProcessID]
	,[DefectCode]
	,[Junk]
	,[Description]
	,[AddDate]
	,[AddName]
	,[EditDate]
	,[Editname]
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
	   t.Remark = s.Remark
      ,t.Junk = s.Junk
      ,t.AddDate = s.AddDate
      ,t.AddName = s.AddName
      ,t.EditDate = s.EditDate
      ,t.EditName = s.EditName
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
SELECT PulloutPortID
      ,BrandID
      ,Remark
      ,Junk
      ,AddDate
      ,AddName
      ,EditDate
      ,EditName
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

-- ����JPulloutPort
UPDATE t
SET  
t.Name =			   s.Name,
t.InternationalCode =  s.InternationalCode,
t.CountryID =		   s.CountryID,
t.AirPort =			   s.AirPort,
t.SeaPort =			   s.SeaPort,
t.Remark =			   s.Remark,
t.AddName =			   s.AddName,
t.AddDate =			   s.AddDate,
t.EditName =		   s.EditName,
t.EditDate =		   s.EditDate,
t.Junk =			   s.Junk
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
SELECT [ID]
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
      ,[Junk]
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


-- ����JConsignee
UPDATE t
SET  
t.Junk =			   s.Junk,
t.AddName =			   s.AddName,
t.AddDate =			   s.AddDate,
t.EditName =		   s.EditName,
t.EditDate =		   s.EditDate
FROM Production.dbo.Consignee t
INNER JOIN Trade_To_Pms.dbo.Consignee as s ON t.id	=s.id

INSERT INTO Production.dbo.Consignee(
	   [ID]
      ,[Junk]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate])
SELECT [ID]
      ,[Junk]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
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

-- ����JConsignee_Detail
UPDATE t
SET  
t.ID =			   s.ID,
t.Email =		   s.Email
FROM Production.dbo.Consignee_Detail t
INNER JOIN Trade_To_Pms.dbo.Consignee_Detail as s ON t.Ukey = s.Ukey

INSERT INTO Production.dbo.Consignee_Detail(
	   [Ukey]
	  ,[ID]
      ,[Email])
SELECT [Ukey]
      ,[ID]
      ,[Email]
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
     a.[Registry]		= b.[Registry]
    ,a.[Expiration]		= b.[Expiration]
    ,a.[AddName]		= b.[AddName]
    ,a.[AddDate]		= b.[AddDate]
    ,a.[EditName]		= b.[EditName]
    ,a.[EditDate]		= b.[EditDate]
    ,a.[ApplyExtension]	= b.[ApplyExtension]
    ,a.[Remark]			= b.[Remark]
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
	 a.[ID]
	,a.[FactoryID]
	,a.[Registry]
	,a.[Expiration]
	,a.[AddName]
	,a.[AddDate]
	,a.[EditName]
	,a.[EditDate]
	,a.[ApplyExtension]
	,a.[Remark]
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
	,a.[MDivisionID]   = b.[MDivisionID]
	,a.[Type]		   = b.[Type]
	,a.[Target]		   = b.[Target]
	,a.[AddName]	   = b.[AddName]
	,a.[AddDate]	   = b.[AddDate]
	,a.[EditName]	   = b.[EditName]
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
    ,a.[MDivisionID]
    ,a.[Type]
    ,a.[Target]
    ,a.[AddName]
    ,a.[AddDate]
    ,a.[EditName]
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
	 [TypeName]		= b.[TypeName]
	,[Placket]		= b.[Placket]
	,[Definition]	= b.[Definition]
	,[CPU]			= b.[CPU]
	,[ComboPcs]		= b.[ComboPcs]
	,[Remark]		= b.[Remark]
	,[Junk]			= b.[Junk]
	,[AddName]		= b.[AddName]
	,[AddDate]		= b.[AddDate]
	,[EditName]		= b.[EditName]
	,[EditDate]		= b.[EditDate]
from Production.dbo.NewCDCode as a inner join Trade_To_Pms.dbo.NewCDCode as b ON a.[Classifty] = b.[Classifty] and a.[ID] = b.[ID]
-------------------------- INSERT INTO ��
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
SELECT [Classifty]
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
	a.[FullName]	= b.[FullName]
	,a.[Type]		= b.[Type]
	,a.[Junk]		= b.[Junk]
FROM Production.dbo.[QAMtlTypeSetting] a
INNER JOIN Trade_To_Pms.dbo.MtlType b ON a.id = b.id

INSERT INTO [dbo].[QAMtlTypeSetting]
           ([ID]
           ,[FullName]
           ,[Type]
           ,[Junk])
select
		 a.[ID]
		,a.[FullName]
		,a.[Type]
		,a.[Junk]
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
      ,a.Rate		      =b.Rate
      ,a.Remark		      =b.Remark
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
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
      ,a.Rate		      =b.Rate
      ,a.AddName		      =b.AddName
      ,a.AddDate		      =b.AddDate
      ,a.EditName		      =b.EditName
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
from Trade_To_Pms.dbo.FinanceTWRate as b WITH (NOLOCK)
where not exists(
    select 1 
    from Production.dbo.FinanceTWRate as a WITH (NOLOCK) 
    where a.RateTypeID = b.RateTypeID
    and a.BeginDate = b.BeginDate
    and a.OriginalCurrency = b.OriginalCurrency
    and a.ExchangeCurrency = b.ExchangeCurrency)

---------IEReasonType 

DELETE Production.dbo.IEReasonType
FROM Production.dbo.IEReasonType a
LEFT JOIN Trade_To_Pms.dbo.IEReasonType b ON a.Type = b.Type
WHERE b.Type is null

UPDATE a
SET
	 a.[Junk]		   = b.[Junk]
	,a.[AddName]	   = b.[AddName]
	,a.[AddDate]	   = b.[AddDate]
	,a.[EditName]	   = b.[EditName]
	,a.[EditDate]	   = b.[EditDate]
FROM Production.dbo.IEReasonType a
INNER JOIN Trade_To_Pms.dbo.IEReasonType b ON a.Type = b.Type

INSERT INTO [dbo].[IEReasonType]
           ([Type]
		   ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select a.[Type]
	,a.[Junk]
    ,a.[AddName]
    ,a.[AddDate]
    ,a.[EditName]
    ,a.[EditDate]
FROM Trade_To_Pms.dbo.IEReasonType a
LEFT JOIN Production.dbo.IEReasonType b ON a.Type = b.Type
WHERE b.Type is null

---------IEReasonTypeGroup 

DELETE Production.dbo.IEReasonTypeGroup
FROM Production.dbo.IEReasonTypeGroup a
LEFT JOIN Trade_To_Pms.dbo.IEReasonTypeGroup b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup
WHERE b.Type is null

UPDATE a
SET
	 a.[IEReasonApplyFunction]	= b.[IEReasonApplyFunction]
	,a.[Junk]					= b.[Junk]
	,a.[AddName]				= b.[AddName]
	,a.[AddDate]				= b.[AddDate]
	,a.[EditName]				= b.[EditName]
	,a.[EditDate]				= b.[EditDate]
FROM Production.dbo.IEReasonTypeGroup a
INNER JOIN Trade_To_Pms.dbo.IEReasonTypeGroup b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup

INSERT INTO [dbo].[IEReasonTypeGroup]
           ([Type]
		   ,[TypeGroup]
		   ,[IEReasonApplyFunction]
		   ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select a.[Type]
	,a.[TypeGroup]
	,a.[IEReasonApplyFunction]
	,a.[Junk]
    ,a.[AddName]
    ,a.[AddDate]
    ,a.[EditName]
    ,a.[EditDate]
FROM Trade_To_Pms.dbo.IEReasonTypeGroup a
LEFT JOIN Production.dbo.IEReasonTypeGroup b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup
WHERE b.Type is null

---------IEReasonLBRNotHit_1st 

DELETE Production.dbo.IEReasonLBRNotHit_1st
FROM Production.dbo.IEReasonLBRNotHit_1st a
LEFT JOIN Trade_To_Pms.dbo.IEReasonLBRNotHit_1st b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup and a.Code = b.Code
WHERE b.Type is null

UPDATE a
SET
	 a.[Ukey]		   = b.[Ukey]
	,a.[Name]		   = b.[Name]
	,a.[Remark]		   = b.[Remark]
	,a.[Junk]		   = b.[Junk]
	,a.[AddName]	   = b.[AddName]
	,a.[AddDate]	   = b.[AddDate]
	,a.[EditName]	   = b.[EditName]
	,a.[EditDate]	   = b.[EditDate]
FROM Production.dbo.IEReasonLBRNotHit_1st a
INNER JOIN Trade_To_Pms.dbo.IEReasonLBRNotHit_1st b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup and a.Code = b.Code

INSERT INTO [dbo].[IEReasonLBRNotHit_1st]
           ([Type]
		   ,[TypeGroup]
		   ,[Code]
		   ,[Ukey]
		   ,[Name]
		   ,[Remark]
		   ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select a.[Type]
	,a.[TypeGroup]
	,a.[Code]
	,a.[Ukey]
	,a.[Name]
	,a.[Remark]
	,a.[Junk]
    ,a.[AddName]
    ,a.[AddDate]
    ,a.[EditName]
    ,a.[EditDate]
FROM Trade_To_Pms.dbo.IEReasonLBRNotHit_1st a
LEFT JOIN Production.dbo.IEReasonLBRNotHit_1st b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup and a.Code = b.Code
WHERE b.Type is null

---------IEReasonLBRNotHit_Detail 

DELETE Production.dbo.IEReasonLBRNotHit_Detail
FROM Production.dbo.IEReasonLBRNotHit_Detail a
LEFT JOIN Trade_To_Pms.dbo.IEReasonLBRNotHit_Detail b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup and a.Code = b.Code
WHERE b.Type is null

UPDATE a
SET
	 a.[Ukey]		   = b.[Ukey]
	,a.[Name]		   = b.[Name]
	,a.[Remark]		   = b.[Remark]
	,a.[Junk]		   = b.[Junk]
	,a.[AddName]	   = b.[AddName]
	,a.[AddDate]	   = b.[AddDate]
	,a.[EditName]	   = b.[EditName]
	,a.[EditDate]	   = b.[EditDate]
FROM Production.dbo.IEReasonLBRNotHit_Detail a
INNER JOIN Trade_To_Pms.dbo.IEReasonLBRNotHit_Detail b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup and a.Code = b.Code

INSERT INTO [dbo].[IEReasonLBRNotHit_Detail]
           ([Type]
		   ,[TypeGroup]
		   ,[Code]
		   ,[Ukey]
		   ,[Name]
		   ,[Remark]
		   ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
select a.[Type]
	,a.[TypeGroup]
	,a.[Code]
	,a.[Ukey]
	,a.[Name]
	,a.[Remark]
	,a.[Junk]
    ,a.[AddName]
    ,a.[AddDate]
    ,a.[EditName]
    ,a.[EditDate]
FROM Trade_To_Pms.dbo.IEReasonLBRNotHit_Detail a
LEFT JOIN Production.dbo.IEReasonLBRNotHit_Detail b ON a.Type = b.Type and a.TypeGroup = b.TypeGroup and a.Code = b.Code
WHERE b.Type is null


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
	 Seq	    = b.Seq		
	,TestName   = b.TestName  
	,SystemType = b.SystemType
	,TestDetail = b.TestDetail
	,Scale	    = b.Scale	   
	,Criteria   = b.Criteria  
	,Criteria2  = b.Criteria2 
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
select		a.[Seq]
           ,a.[TestName]
           ,a.[Location]
           ,a.[SystemType]
           ,a.[ReportType]
           ,a.[MtlTypeID]
           ,a.[Washing]
           ,a.[FabricComposition]
           ,a.[TestDetail]
           ,a.[Scale]
           ,a.[Criteria]
           ,a.[Criteria2]
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
	Code = b.Code
from production.dbo.TypeSelection a
inner join Trade_To_Pms.dbo.TypeSelection b on a.VersionID = b.VersionID and a.Seq = b.Seq 

insert production.dbo.TypeSelection (VersionID,Seq,Code)
select 
	 a.VersionID
	,a.Seq
	,a.Code
from Trade_To_Pms.dbo.TypeSelection a
left join production.dbo.TypeSelection b on a.VersionID = b.VersionID and a.Seq = b.Seq 
where b.VersionID is null


--QABrandSetting
update a set
	a.PullingTest_PullForceUnit = b.PullingTest_PullForceUnit 
from  production.dbo.QABrandSetting a
inner join Trade_To_Pms.dbo.QABrandSetting b on a.BrandID = b.BrandID

INSERT INTO production.[dbo].[QABrandSetting]
           ([BrandID]
           ,PullingTest_PullForceUnit
           ,[AddDate]
           ,[AddName])
select 
	a.BrandID
	,a.PullingTest_PullForceUnit
	,GETDATE()
	,'SCIMIS'
from  Trade_To_Pms.dbo.QABrandSetting a
left join production.dbo.QABrandSetting b on a.BrandID = b.BrandID
where b.BrandID is null

--Brand_PullingTestStandarList
delete a
from production.dbo.Brand_PullingTestStandarList a
left join Trade_To_Pms.dbo.Brand_PullingTestStandarList b on a.BrandID = b.BrandID and a.TestItem = b.TestItem and a.PullForceUnit = b.PullForceUnit
where b.BrandID is null

update a set 
	PullForce = b.PullForce,
	Time = b.Time
from production.dbo.Brand_PullingTestStandarList a
inner join Trade_To_Pms.dbo.Brand_PullingTestStandarList b on a.BrandID = b.BrandID and a.TestItem = b.TestItem and a.PullForceUnit = b.PullForceUnit

insert production.dbo.Brand_PullingTestStandarList (BrandID,TestItem,PullForceUnit,PullForce,Time)
select a.BrandID,a.TestItem,a.PullForceUnit,a.PullForce,a.Time
from Trade_To_Pms.dbo.Brand_PullingTestStandarList a
left join production.dbo.Brand_PullingTestStandarList b on a.BrandID = b.BrandID and a.TestItem = b.TestItem and a.PullForceUnit = b.PullForceUnit
where b.BrandID is null

--GarmentTestShrinkage
delete a
from production.dbo.GarmentTestShrinkage a
left join Trade_To_Pms.dbo.GarmentTestShrinkage b on a.Ukey = b.Ukey 
where b.Ukey is null

update a set 
 [BrandID] = b.[BrandID]
,[LocationGroup] = b.[LocationGroup]
,[Location] = b.[Location]
,[Seq] = b.[Seq]
,[Type] = b.[Type]
from production.dbo.GarmentTestShrinkage a
inner join Trade_To_Pms.dbo.GarmentTestShrinkage b on a.Ukey = b.Ukey 

insert production.dbo.GarmentTestShrinkage 
           ([BrandID]
           ,[LocationGroup]
           ,[Location]
           ,[Seq]
           ,[Type])
select a.[BrandID],a.[LocationGroup],a.[Location],a.[Seq],a.[Type]
from Trade_To_Pms.dbo.GarmentTestShrinkage a
left join production.dbo.GarmentTestShrinkage b on  a.Ukey = b.Ukey 
where b.BrandID is null

END