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
Using (
	select * 
	from Trade_To_Pms.dbo.Part WITH (NOLOCK) 
	where type = 'P' 
)as s on t.id=s.Refno 
when matched then 
	update set 
		t.Description = s.Description
		, t.Partno= s.Partno
		, t.UseUnit= s.UnitID
		, t.PoUnit= s.PoUnitID
		, t.Price= s.Price
		, t.PurchaseBatchQty= s.BatchQty
		, t.Junk= s.Junk
		, t.SuppID= s.SuppID
		, t.CurrencyID= s.CurrencyID
		, t.Formula= s.Formula
		, t.Fix= s.Fix
		, t.AddName= s.AddName
		, t.AddDate= s.AddDate
		, t.Lock = isnull(s.Lock,0)

	when not matched by target and s.type='P' then 
		insert 
			(ID 				, Description 	, Partno 		, MachineGroupID 	, MachineBrandID
	 	 	 , UseUnit 			, PoUnit 		, Price 		, PurchaseBatchQty 	, Junk
			 , PurchaseFrom 	, SuppID 		, CurrencyID 	, Formula	 		, Fix
			 , AddName  		, AddDate 		, EditName 		, EditDate 			, Lock)
		values
			(s.refno 			, s.Description , s.Partno 		, s.MachineGroupID 	, s.MachineBrandID
			 , s.UnitID 		, s.POUnitID 	, s.Price 		, s.BatchQty 		, s.Junk
			 , 'T'  			, s.SuppID 		, s.CurrencyID 	, s.Formula 		, s.Fix
			 , s.AddName 		, s.AddDate  	, s.EditName  	, s.EditDate 		, s.Lock);

	---------- Misc, type='O'---------------------
	Merge Machine.dbo.Misc as t
	Using Trade_To_Pms.dbo.Part as s
	on t.id=s.Refno and s.type='O' 
	when matched then 
		update set 
t.Model= s.Model,
t.MiscBrandID= s.MachineBrandID,
t.Description= s.Description,
t.UnitID= s.PoUnitID,
t.CurrencyID= s.CurrencyID,
t.Price= s.Price,
--t.SuppID= s.SuppID, MMS可以編輯,所以不用update
t.IsMachine= s.IsMachine,
t.IsAsset= s.IsAsset,
t.Remark= s.Remark,
t.InspLeadTime= s.Leadtime,
t.AccountID= s.AccountID,
t.Junk= s.Junk,
t.AddName= s.AddName,
t.AddDate= s.AddDate,
t.EditName= s.EditName,
t.EditDate= s.EditDate

	when not matched by target and s.type='O' then 
		insert ( ID
,Model
,MiscBrandID
,Description
,UnitID
,CurrencyID
,Price
,SuppID
,PurchaseFrom
,IsMachine
,IsAsset
,Remark
,InspLeadTime
,AccountID
,Junk
,AddName
,AddDate
,EditName
,EditDate	)

values(s.refno,
s.Model,
s.MachineBrandID,
s.Description,
s.PoUnitID,
s.CurrencyID,
s.Price,
s.SuppID,
'T',
s.IsMachine,
s.IsAsset,
s.Remark,
s.Leadtime,
s.AccountID,
s.Junk,
s.AddName,
s.AddDate,
s.EditName,
s.EditDate
 );

 
	-----------------PartQuot , type='P'-----------------------------

	Merge [Machine].[dbo].[PartQuot] as t
	Using (select distinct a.* from [Trade_To_Pms].[dbo].[MmsQuot] a WITH (NOLOCK) inner join  [Trade_To_Pms].[dbo].Part b WITH (NOLOCK)
	on a.PartUkey=b.Ukey where a.type='P')	 as s
	on t.id=s.refno and  t.ukey=s.id 
	when matched then
		update set 
		t.purchaseFrom='T',
		t.suppid=s.suppid,
		t.CurrencyID=s.CurrencyID,
		t.PartBrandID=s.mmsBrandID,
		t.Price=s.Price,
		t.AddName=s.AddName,
		t.AddDate=s.AddDate,
		t.EditName=s.EditName,
		t.EditDate=s.EditDate,
		T.ldefault = S.IsDefault
	when not matched by target then
	insert (id,
		purchaseFrom,
		suppid,
		CurrencyID,
		Price,
		AddName,
		AddDate,
		EditName,
		EditDate,
		PartBrandID,
		ldefault
				)
		values(s.Refno,
		'T',
		s.suppid,
		s.CurrencyID,
		s.Price,
		s.AddName,
		s.AddDate,
		s.EditName,
		s.EditDate,
		s.mmsBrandID,
		S.IsDefault
		)
	when not matched by source and t.purchaseFrom='T' and t.id in (select refno from [Trade_To_Pms].[dbo].Part WITH (NOLOCK))  then
		delete;

		
	

	---------------PartBrand-------------------------------
	Merge Machine.dbo.PartBrand as t
	Using (select * from [Trade_To_Pms].[dbo].[MmsBrand] WITH (NOLOCK) where type='P') as s
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
	Using (select * from [Trade_To_Pms].[dbo].[MmsBrand] WITH (NOLOCK) where type='O') as s
	on t.id=s.id
	when matched then
			update set
		  --t.Name= 	      s.Name    MMS可以編輯,所以不用update
		  --,t.Junk= 	      s.Junk    MMS可以編輯,所以不用update
		  t.AddName= 	      s.AddName   
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
		  ,EditDate ,
		  [LOCAL]
		)
		values(
		  s.id  ,
		  s.Name  , 
		  s.Junk   ,
		  s.AddName ,  
		  s.AddDate  , 
		  s.EditName  , 
		  s.EditDate,   
		  0
		)
	when not matched by source  then 
	delete;

	---------------MachBrand-----------------------------------

	Merge [Machine].[dbo].[MachineBrand] as t
	Using (select * from [Trade_To_Pms].[dbo].[MmsBrand] WITH (NOLOCK) where type='M') as s
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
	insert @Sayfty select id from Machine.dbo.Factory
	
	-- if type<>'M'
	UPDATE Machine.DBO.PartPO_Detail
	SET TPEPOID = B.id,
	SEQ1=b.Seq1,
	SuppDelivery=b.SuppDelivery,
	EstETA=b.EstETA
	FROM Machine.DBO.PartPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on a.PartID=b.Refno and  a.SEQ2=b.Seq2 and a.id = b.MmsReqID
	INNER JOIN  Trade_To_Pms.DBO.MmsPO C ON B.ID=C.ID
	WHERE C.Type ='P'
	and C.FactoryID in (select id from @Sayfty)

	UPDATE Machine.DBO.MiscPO_Detail
	SET TPEPOID = B.id,
	SEQ1=b.Seq1,
	SuppDelivery=b.SuppDelivery,
	EstETA=b.EstETA
	FROM Machine.DBO.MiscPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on  a.MiscID=b.Refno 
													and  a.SEQ2=b.Seq2 
													and a.id = b.MmsReqID
	INNER JOIN  Trade_To_Pms.DBO.MmsPO C ON B.ID=C.ID
	WHERE C.Type ='O'
	and C.FactoryID in (select id from @Sayfty)
	-- ------------MachinePO--------------------
	declare @T table (id varchar(13))

		Merge Machine.dbo.MachinePO as t
	Using (select a.*,b.MdivisionID from Trade_To_Pms.DBO.MmsPO a WITH (NOLOCK) left join Production.dbo.scifty b WITH (NOLOCK) on a.factoryid = b.id
	 where a.factoryid in (select id from @Sayfty) and type = 'M')  as s
	on t.id=s.id
	when matched  then
		update set
		t.CDate = s.CDate ,
		t.PurchaseFrom = 'T' ,		
		t.FactoryID = s.FactoryID ,
		t.MDivisionID = s.MDivisionID,
		t.CurrencyID = s.CurrencyID ,
		t.Status= IIF(s.Junk = 1,'Junked',IIF(s.ApvName != '','Approved','New')),
		t.Handle = s.Handle ,
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
		(ID ,	  CDate ,PurchaseFrom ,  FactoryID ,   CurrencyID ,   Handle,    LocalSuppID ,  Amount ,   Vatrate ,   Vat ,        Remark ,        Approve ,        ApproveDate ,        AddName ,        AddDate ,        EditName ,        EditDate
		 ,MDivisionID,Status)
		values
		(s.ID ,	s.CDate , 'T' ,	s.FactoryID ,s.CurrencyID ,	s.Handle,s.SuppID ,		s.Amount ,		s.Vatrate ,	Vat ,		s.Remark ,		s.ApvName ,		s.ApvDate ,		s.AddName ,		s.AddDate ,		s.EditName ,		s.EditDate
		,MDivisionID, IIF(s.Junk = 1,'Junked',IIF(s.ApvName != '',ApvName,'New')))
	output inserted.id into @T;

-----------------MachinePO_Detail Type <>'M'---------------------
update t
		set t.TpePOID = s.id,
		t.seq1=s.seq1,
		t.SuppDelivery=s.SuppDelivery,
		t.EstETA=s.EstETA
		from  Machine.dbo.PartPO_Detail as  t
		inner join Trade_to_Pms.dbo.MmsPO_Detail s on t.id=s.MmsReqID  and t.seq2=s.seq2
		inner join Trade_To_Pms.DBO.MmsPO a on s.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where a.Type='P'
		

		update t
		set t.TpePOID = s.id,
		t.seq1=s.seq1,
		t.SuppDelivery=s.SuppDelivery,
		t.EstETA=s.EstETA
		from  Machine.dbo.MiscPO_Detail as  t
		inner join Trade_to_Pms.dbo.MmsPO_Detail s on t.id=s.MmsReqID  and t.seq2=s.seq2
		inner join Trade_To_Pms.DBO.MmsPO a on s.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where a.Type='O'
		

------------------MachinePO_Detail Type='M'----------------------

	Merge Machine.[dbo].[MachinePO_Detail] as t
	using (select * from Trade_To_PMS.dbo.MachinePO_Detail a WITH (NOLOCK)
where a.id in (select id from @T)) as s
	on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2
	when matched then 
				update set
				t.MachineGroupID= s.MachineGroupID,
				t.MachineBrandID= s.MachineBrandID,
				t.Model= s.Model,
				t.Description= s.Description,
				t.Qty= s.Qty,
				t.FOC= s.FOC,
				t.Price= s.Price,				
				t.Remark= s.Remark,
				t.MachineReqID= s.MmsReqID,
				t.Junk= s.Junk
		when not matched by target then
		insert  (ID,Seq1,Seq2,MachineGroupID,MachineBrandID,Model,Description,Qty,FOC,Price,Remark,MachineReqID,Junk)
		values	(s.ID,s.Seq1,s.Seq2,s.MachineGroupID,s.MachineBrandID,s.Model,s.Description,s.Qty,s.FOC,s.Price,s.Remark,s.MmsReqID,s.Junk)
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

	  -----------MachineGroup------------------------

		Merge Machine.dbo.MachineGroup as t
		using Trade_To_Pms.dbo.MachineGroup as s 
		on t.id=s.id
		when matched then
				update set 
				t.ID= s.ID,
				t.Description= s.Description,
				t.DescCH= s.DescriptionCH,
				t.Substitute= s.Substitute,
				t.Junk= s.Junk,
				t.Picture1= s.Picture1,
				t.Picture2= s.Picture2,				
				t.AddName= s.AddName,
				t.AddDate= s.AddDate,
				t.EditName= s.EditName,
				t.EditDate= s.EditDate
		when not matched by target then
				insert(ID
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
				values(s.ID,
				s.Description,
				s.DescriptionCH,
				s.Substitute,
				s.Junk,
				s.Picture1,
				s.Picture2,
				s.AddName,
				s.AddDate,
				s.EditName,
				s.EditDate
				);
		



END


