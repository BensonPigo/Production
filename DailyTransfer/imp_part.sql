-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/22>
-- Description:	<import part>
-- =============================================
CREATE PROCEDURE [dbo].[imp_part]
AS
BEGIN

	---------- Parts, type='P'---------------------
	Merge Machine.dbo.Part as t
	Using (select * from Trade_To_Pms.dbo.Part where type='P')as s
	on t.id=s.Refno --and t.purchasefrom='T'
	when matched then 
	update set 
t.Description= s.Description,
t.Partno= s.Partno,
t.MachineGroupID= s.MachineGroupID,
t.MachineBrandID= s.MachineBrandID,
--t.Needle= s.Needle,
--t.ControlPart= s.ControlPart,
--t.UseUnit= s.UseUnit,
--t.PoUnit= s.PoUnit,
--t.LocalPrice= s.LocalPrice,
t.Price= s.Price,
--t.Minstock= s.Minstock,
--t.PurchaseBatchQty= s.PurchaseBatchQty,
t.Junk= s.Junk,
t.PurchaseFrom= 'T',
t.LocalSuppID= s.SuppID,
t.SuppID= s.SuppID,
t.CurrencyID= s.CurrencyID,
t.LocalCurrencyID= s.CurrencyID,
t.Formula= s.Formula,
t.Fix= s.Fix,
--t.Pic= s.Pic,
t.AddName= s.AddName,
t.AddDate= s.AddDate,
t.EditName= s.EditName,
t.EditDate= s.EditDate

	when not matched by target and s.type='P' then 
		insert ( ID
,Description
,Partno
,MachineGroupID
,MachineBrandID
--,Needle
--,ControlPart
--,UseUnit
--,PoUnit
--,LocalPrice
,Price
--,Minstock
--,PurchaseBatchQty
,Junk
,PurchaseFrom
,LocalSuppID
,SuppID
,CurrencyID
,LocalCurrencyID
,Formula
,Fix
--,Pic
,AddName
,AddDate
,EditName
,EditDate
	)
values(s.refno,
s.Description,
s.Partno,
s.MachineGroupID,
s.MachineBrandID,
--s.Needle,
--s.ControlPart,
--s.UseUnit,
--s.PoUnit,
--s.LocalPrice,
s.Price,
--s.Minstock,
--s.PurchaseBatchQty,
s.Junk,
'T',
s.SuppID,
s.SuppID,
s.CurrencyID,
s.CurrencyID,
s.Formula,
s.Fix,
--s.Pic,
s.AddName,
s.AddDate,
s.EditName,
s.EditDate
 );

	---------- Misc, type='O'---------------------
	Merge Machine.dbo.Misc as t
	Using Trade_To_Pms.dbo.Part as s
	on t.id=s.Refno and s.type='O' 
	when matched then 
		update set 
		--t.ID= s.ID,
t.Model= s.Model,
--t.MiscBrandID= s.MiscBrandID,
t.Description= s.Description,
t.UnitID= s.UnitID,
t.CurrencyID= s.CurrencyID,
t.Price= s.Price,
t.SuppID= s.SuppID,
t.PurchaseFrom= 'T',
--t.Inspect= s.Inspect,
t.IsMachine= s.IsMachine,
t.IsAsset= s.IsAsset,
--t.PurchaseType= s.PurchaseType,
t.Remark= s.Remark,
--t.PIC= s.PIC,
--t.InspLeadTime= s.InspLeadTime,
t.AccountID= s.AccountID,
t.Junk= s.Junk,
t.AddName= s.AddName,
t.AddDate= s.AddDate,
t.EditName= s.EditName,
t.EditDate= s.EditDate

	when not matched by target and s.type='O' then 
		insert ( ID
,Model
--,MiscBrandID
,Description
,UnitID
,CurrencyID
,Price
,SuppID
,PurchaseFrom
--,Inspect
,IsMachine
,IsAsset
--,PurchaseType
,Remark
--,PIC
--,InspLeadTime
,AccountID
,Junk
,AddName
,AddDate
,EditName
,EditDate	)

values(s.refno,
s.Model,
--s.MiscBrandID,
s.Description,
s.UnitID,
s.CurrencyID,
s.Price,
s.SuppID,
'T',
--s.Inspect,
s.IsMachine,
s.IsAsset,
--s.PurchaseType,
s.Remark,
--s.PIC,
--s.InspLeadTime,
s.AccountID,
s.Junk,
s.AddName,
s.AddDate,
s.EditName,
s.EditDate
 );
 
	-----------------PartQuot , type='P'-----------------------------

	Merge [Machine].[dbo].[PartQuot] as t
	Using (select  a.* from [Trade_To_Pms].[dbo].[MmsQuot] a  inner join  [Trade_To_Pms].[dbo].Part b
	on a.refno=b.refno where a.type='P')	 as s
	--on t.id=s.refno and t.purchaseFrom='T' and t.PartBrandID=s.MMSBrandID and t.suppid=s.suppid AND t.ldefault=s.isdefault
	on t.id=s.refno and  t.ukey=s.id --and t.purchaseFrom='T' 
	when matched then
		update set 
		t.purchaseFrom='T',
		t.suppid=s.suppid,
		t.CurrencyID=s.CurrencyID,
		t.Price=s.Price,
		t.AddName=s.AddName,
		t.AddDate=s.AddDate,
		t.EditName=s.EditName,
		t.EditDate=s.EditDate
	when not matched by target then
	insert (id,
		purchaseFrom,
		suppid,
		CurrencyID,
		Price,
		AddName,
		AddDate,
		EditName,
		EditDate
				)
		values(s.Refno,
		'T',
		s.suppid,
		s.CurrencyID,
		s.Price,
		s.AddName,
		s.AddDate,
		s.EditName,
		s.EditDate
		)
	when not matched by source and t.purchaseFrom='T' and t.id in (select refno from [Trade_To_Pms].[dbo].Part)  then
		delete;

		
	

	---------------PartBrand-------------------------------
	Merge Machine.dbo.PartBrand as t
	Using (select * from [Trade_To_Pms].[dbo].[MmsBrand] where type='P') as s
	on t.id=s.id
	when matched then 
		update set
		t.Name= 	      s.Name 
		,t.Junk= 	      s.Junk 
		,t.AddName= 	      s.AddName 
		,t.AddDate= 	      s.AddDate 
		,t.EditName= 	      s.EditName 
		,t.EditDate= 	      s.EditDate 
	when not matched by target then 
		insert(
		ID
		,Name 
		,Junk 
		,AddName 
		,AddDate 
		,EditName 
		,EditDate
		)
		values
		(
		s.ID
		,s.Name 
		,s.Junk 
		,s.AddName 
		,s.AddDate 
		,s.EditName 
		,s.EditDate
		)
	when not matched by source then
	delete;

--------------MiscBrand--------------------------------------
	-- O: Miscellaneous         
	Merge [Machine].[dbo].[MiscBrand] as t
	Using (select * from [Trade_To_Pms].[dbo].[MmsBrand] where type='O') as s
	on t.id=s.id
	when matched then
			update set
		  t.Name= 	      s.Name   
		  ,t.Junk= 	      s.Junk   
		  ,t.AddName= 	      s.AddName   
		  ,t.AddDate= 	      s.AddDate   
		  ,t.EditName= 	      s.EditName   
		  ,t.EditDate= 	      s.EditDate   
	when not matched  by target then
		insert (ID
		  ,Name 
		  ,Junk
		  ,AddName 
		  ,AddDate 
		  ,EditName 
		  ,EditDate 
		)
		values(
		  s.id  ,
		  s.Name  , 
		  s.Junk   ,
		  s.AddName ,  
		  s.AddDate  , 
		  s.EditName  , 
		  s.EditDate   
		)
	when not matched by source  then 
	delete;

	---------------MachBrand-----------------------------------

	Merge [Machine].[dbo].[MachineBrand] as t
	Using (select * from [Trade_To_Pms].[dbo].[MmsBrand] where type='M') as s
	on t.id=s.id
	when matched then
			update set
		  t.Name= 	      s.Name   
		  ,t.Junk= 	      s.Junk   
		  ,t.AddName= 	      s.AddName   
		  ,t.AddDate= 	      s.AddDate   
		  ,t.EditName= 	      s.EditName   
		  ,t.EditDate= 	      s.EditDate   
	when not matched by target then
		insert (ID
		  ,Name 
		  ,Junk 
		  ,AddName 
		  ,AddDate 
		  ,EditName 
		  ,EditDate 
		)
		values(
		  s.ID   ,
		  s.Name  , 
		  s.Junk   ,
		  s.AddName ,  
		  s.AddDate  , 
		  s.EditName  , 
		  s.EditDate   
		);
	

	----------------PartPO2-------------------------

	update [Machine].[dbo].[PartPO_Detail]
	set Junk = b.Cancel 
	from [Machine].[dbo].[PartPO_Detail] a
	inner join [Trade_To_Pms].[dbo].[MmsReq_Detail] b on a.id=b.ID and a.SEQ2=b.Seq2


	----------------整理所有工廠--------------------
	declare @Sayfty table(id varchar(10)) --工廠代碼
	insert @Sayfty select id from Production.dbo.Factory
	
	-- if type<>'M'
	UPDATE Machine.DBO.PartPO_Detail
	SET TPEPOID = B.id,
	SEQ1=b.Seq1
	FROM Machine.DBO.PartPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on a.PartID=b.Refno and  a.SEQ2=b.Seq2
	INNER JOIN  Trade_To_Pms.DBO.MmsPO C ON B.ID=C.ID
	WHERE C.Type ='P'
	and C.FactoryID in (select id from @Sayfty)

	UPDATE Machine.DBO.MiscPO_Detail
	SET TPEPOID = B.id,
	SEQ1=b.Seq1
	FROM Machine.DBO.MiscPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on a.MiscID=b.Refno and  a.SEQ2=b.Seq2
	INNER JOIN  Trade_To_Pms.DBO.MmsPO C ON B.ID=C.ID
	WHERE C.Type ='O'
	and C.FactoryID in (select id from @Sayfty)
	-- ------------MachinePO--------------------
	declare @T table (id varchar(13))

	Merge Machine.dbo.MachinePO as t
	Using (select * from Trade_To_Pms.DBO.MmsPO where factoryid in (select id from @Sayfty) and type = 'M')  as s
	on t.id=s.id
	when matched  then
		update set
		t.CDate = s.CDate ,
		t.PurchaseFrom = 'T' ,		
		t.FactoryID = s.FactoryID ,
		t.CurrencyID = s.CurrencyID ,
		t.Handle = s.Handle ,
		t.LocalSuppID = s.SuppID ,
		t.Amount = s.Amount ,
		t.Vatrate = s.Vatrate ,
		t.Vat = s.Vat ,
		t.Remark = s.Remark ,
		t.Approve = s.ApvName ,		
		t.ApproveDate = s.ApvDate ,
		t.AddName = s.AddName ,
		t.AddDate = s.AddDate ,
		t.EditName = s.EditName ,
		t.EditDate = s.EditDate 
	when not matched by target then
		insert
		(
			 ID ,
        CDate ,
        PurchaseFrom ,
        FactoryID ,
        CurrencyID ,
        Handle ,
        LocalSuppID ,
        Amount ,
        Vatrate ,
        Vat ,
        Remark ,
        Approve ,
        ApproveDate ,
        AddName ,
        AddDate ,
        EditName ,
        EditDate 
		)
		values(s. ID ,
		s.CDate ,
		'T' ,
		s.FactoryID ,
		s.CurrencyID ,
		s.Handle ,
		s.SuppID ,
		s.Amount ,
		s.Vatrate ,
		s.Vat ,
		s.Remark ,
		s.ApvName ,
		s.ApvDate ,
		s.AddName ,
		s.AddDate ,
		s.EditName ,
		s.EditDate 
)
output inserted.id into @T;

------------------MachinePO_Detail----------------------

	Merge Machine.[dbo].[MachinePO_Detail] as t
	using (select * from  Trade_To_Pms.[dbo].[MmsPO_Detail] where id in (select id from @T)) as s
	on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2
	when matched then 
				update set
				t.Qty= s.Qty,
				t.FOC= s.FOC,
				t.Price= s.Price,
				t.Remark= s.Remark,
				t.Junk= s.Junk

		when not matched by target then
		insert  (ID,Seq1,Seq2,Qty,FOC,Price,Remark,Junk)
		values	(s.ID,s.Seq1,s.Seq2,s.Qty,s.FOC,s.Price,s.Remark,s.Junk	)
		when not matched by source and t.id in (select id from @T) then
		delete ;

	
		
	--------------Partunit-------------------------------
		Merge [Machine].[dbo].[MMSUnit] as t
	using [Trade_To_Pms].[dbo].[MmsUnit] as s
	on t.id=s.id
		when matched then 
		update set
		t.addname=s.addname,
		t.adddate=s.adddate,
		t.editname=s.editname,
		t.editdate=s.editdate
		when not matched by target then
		insert([ID]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate])
	  values
	  (
	   s.[ID]     
      ,s.[AddName]
      ,s.[AddDate]
      ,s.[EditName]
      ,s.[EditDate]
	  );
	

END


