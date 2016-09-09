
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/22>
-- Description:	<import part>
-- =============================================
CREATE PROCEDURE [dbo].[imp_part]
AS
BEGIN
	

	----------Machine Parts---------------------
	Merge Machine.dbo.Part as t
	Using Trade_To_Pms.dbo.Part as s
	on t.id=s.Refno
	when matched then 
		update set 
		t.MachineGroupID=s.MachineGroupID,
		t.MachineBrandID=s.MachineBrandID,	
		t.Formula=s.Formula,
		t.Fix=s.Fix,
		t.AddName=s.AddName,
		t.AddDate=s.AddDate
	when not matched by target then 
		insert ( ID 
      , MachineGroupID 
      , MachineBrandID 
      , Price 
      , CurrencyID 
      , Formula 
      , Fix 
      , AddName 
      , AddDate 
      , EditName 
      , EditDate 
	)values(s.Refno 
      , s.MachineGroupID 
      , s.MachineBrandID 
      , s.Price 
      , s.CurrencyID 
      , s.Formula 
      , s.Fix 
      , s.AddName 
      , s.AddDate 
      , s.EditName 
      , s.EditDate );
	
	
	-----------------PartQuot-----------------------------

	Merge [Machine].[dbo].[PartQuot] as t
	Using (select  a.* from [Trade_To_Pms].[dbo].[MmsQuot] a  inner join  [Trade_To_Pms].[dbo].Part b
	on a.refno=b.refno )	 as s
	on t.id=s.refno and t.purchaseFrom='T' and t.PartBrandID=s.MMSBrandID and t.suppid=s.suppid
	when matched and t.purchaseFrom='T'  then
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

	----------------PartPO2-------------------------

	update [Machine_Test].[dbo].[PartPO_Detail]
	set Junk = b.Cancel 
	from [Machine_Test].[dbo].[PartPO_Detail] a
	inner join [Trade_To_Pms].[dbo].[MmsReq_Detail] b on a.id=b.ID and a.SEQ2=b.Seq2


	----------------整理所有工廠--------------------
	declare @Sayfty table(id varchar(10)) --工廠代碼
	insert @Sayfty select id from Production.dbo.Factory

	-- if type<>'M'
	UPDATE Machine.DBO.PartPO_Detail
	SET TPEPOID = B.id,SEQ1=b.Seq1
	FROM Machine.DBO.PartPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on a.id=b.Refno and a.SEQ2=b.Seq2
	INNER JOIN  Trade_To_Pms.DBO.MmsPO C ON B.ID=C.ID
	WHERE C.Type <>'M'
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
		Merge [Machine_Test].[dbo].[MMSUnit] as t
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
	---------------MachBrand-----------------------------------

	Merge [Machine_Test].[dbo].[MachineBrand] as t
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
	--------------MiscBrand--------------------------------------
	-- O: Miscellaneous         
	Merge [Machine_Test].[dbo].[MiscBrand] as t
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

END




