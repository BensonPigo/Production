-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/22>
-- Description:	<import part>
-- =============================================
CREATE PROCEDURE [dbo].[imp_part]
AS
BEGIN

	---------- Parts, type='P'---------------------
	--Step.1 Insert Table [PartFormula_History] if [Formula] is different
SELECT 
 [PartID]=ISNULL(t.ID,'')
,[OldFormula]=ISNULL(t.Formula,0)
,[NewFormula]=ISNULL(s.Formula,0)
,[AddName]=t.EditName
,[AddDate]=t.EditDate
INTO #Formula_Change_Table
FROM Machine.dbo.Part t
INNER JOIN Trade_To_Pms.dbo.Part s
ON t.id=s.Refno  AND s.Type='P'
AND t.Formula<> s.Formula
;
IF EXISTS (SELECT 1 FROM #Formula_Change_Table)
BEGIN
	INSERT INTO [Machine].[dbo].[PartFormula_History]
	SELECT * FROM #Formula_Change_Table
END
;
DROP TABLE #Formula_Change_Table
;
	--Step.2. Merge
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
		, t.Lock = s.Lock
		, t.MasterGroupID = s.MasterGroupID
        , t.MachineGroupID = s.MachineGroupID		
		, t.Needle = s.Needle
		, t.ControlPart = s.ControlParts
		, t.MOQ = isnull(convert(int, s.MOQ),0)
	when not matched by target and s.type='P' then 
		insert 
			(ID 				, Description 	, Partno 		, MasterGroupID 		, MachineGroupID 	, MachineBrandID
	 	 	 , UseUnit 			, PoUnit 		, Price 		, PurchaseBatchQty 	, Junk
			 , PurchaseFrom 	, SuppID 		, CurrencyID 	, Formula	 		, Fix
			 , AddName  		, AddDate 		, EditName 		, EditDate 			, Lock , Needle , ControlPart
			 ,	MOQ)
		values
			(s.refno 			, s.Description , s.Partno 		, s.MasterGroupID 	, s.MachineGroupID 	, s.MachineBrandID
			 , s.UnitID 		, s.POUnitID 	, s.Price 		, s.BatchQty 		, s.Junk
			 , 'T'  			, s.SuppID 		, s.CurrencyID 	, s.Formula 		, s.Fix
			 , s.AddName 		, s.AddDate  	, s.EditName  	, s.EditDate 		, s.Lock , s.Needle , s.ControlParts
			 , isnull(convert(int, s.MOQ),0));

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
--t.SuppID= s.SuppID, MMS�i�H�s��,�ҥH����update
t.IsMachine= s.IsMachine,
t.IsAsset= s.IsAsset,
t.Remark= s.Remark,
t.InspLeadTime= s.Leadtime,
t.AccountID= s.AccountID,
t.Junk= s.Junk,
t.AddName= s.AddName,
t.AddDate= s.AddDate,
t.EditName= s.EditName,
t.EditDate= s.EditDate,
t.DescriptionDetail = s.DescriptionDetail,
t.PurchaseBatchQty = s.BatchQty,
t.AssetTypeID = s.AssetTypeID

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
,EditDate
,DescriptionDetail
,PurchaseBatchQty
,AssetTypeID	)

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
s.EditDate,
s.DescriptionDetail,
s.BatchQty,
s.AssetTypeID
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
		  --t.Name= 	      s.Name    MMS�i�H�s��,�ҥH����update
		  --,t.Junk= 	      s.Junk    MMS�i�H�s��,�ҥH����update
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
	
	----------------RepairPO_Detail-------------------------

	update [Machine].[dbo].[RepairPO_Detail]
	set ShipQty = b.ShipQty ,
		ShipFoc = b.ShipFoc ,
		EstCost = b.EstCost ,
		ActCost = b.ActCost ,
		NewCost = b.NewCost ,
		ETA = b.ETA ,
		Cancel = b.Cancel ,
		Complete = b.Complete ,
		TPEPOID = b.TradePOID
	from [Machine].[dbo].[RepairPO_Detail] a
	inner join [Trade_To_Pms].[dbo].[RepairReq_Detail] b on a.id=b.ID and a.Seq2=b.Seq2

	----------------PartPO2-------------------------

	update [Machine].[dbo].[PartPO_Detail]
	set 
		Junk = b.Cancel 
		,TPEPOID = b.MmsPoID
	from [Machine].[dbo].[PartPO_Detail] a
	inner join [Trade_To_Pms].[dbo].[MmsReq_Detail] b on a.id=b.ID and a.SEQ2=b.Seq2


	----------------��z�Ҧ��u�t--------------------
	declare @Sayfty table(id varchar(10)) --�u�t�N�X
	insert @Sayfty select id from Machine.dbo.Factory
	
	-- if type<>'M'
	UPDATE A
	SET
	A.SEQ1=b.Seq1,
	A.SuppDelivery=b.SuppDelivery,
	A.EstETA=b.EstETA,
	A.Complete = ISNULL(b.Complete,0),
	A.TPEQty=B.Qty,
	A.Foc=B.Foc,
	A.ShipQty=B.ShipQty,
	A.ShipFoc=B.ShipFoc,
	A.ShipETA=B.ShipETA
	FROM Machine.DBO.PartPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on a.PartID=b.Refno AND a.SEQ2=b.Seq2 and a.id = b.MmsReqID and b.Junk=0 AND b.ID=a.TPEPOID
	INNER JOIN  Machine.DBO.PartPO C ON A.ID=C.ID
	WHERE C.FactoryID in (select id from @Sayfty)

	UPDATE Machine.DBO.MiscPO_Detail
	SET TPEPOID = B.id,
	SEQ1=b.Seq1,
	SuppDelivery=b.SuppDelivery,
	EstETA=b.EstETA,
	TPEQty = B.Qty,
	Foc = B.Foc,
	ShipQty = B.ShipQty,
	ShipFoc = B.ShipFoc,
	ShipETA = B.ShipETA
	FROM Machine.DBO.MiscPO_Detail A
	INNER JOIN Trade_To_Pms.DBO.MmsPO_Detail B  on  a.MiscID=b.Refno 
													and  a.SEQ2=b.Seq2 
													and a.id = b.MmsReqID
	INNER JOIN  Trade_To_Pms.DBO.MmsPO C ON B.ID=C.ID
	WHERE C.Type ='O'
	and C.FactoryID in (select id from @Sayfty)

	UPDATE a
	SET Junk = b.Cancel
	FROM Machine.dbo.MiscPO_Detail a
	INNER JOIN Trade_To_Pms.dbo.MmsReq_Detail b ON a.ID = b.ID

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
		set 
		t.seq1=s.seq1,
		t.SuppDelivery=s.SuppDelivery,
		t.EstETA=s.EstETA,
		t.Complete = isnull(s.Complete,0),
		t.TPEQty=s.Qty,
		t.Foc=s.Foc,
		t.ShipQty=s.ShipQty,
		t.ShipFoc=s.ShipFoc,
		t.ShipETA=s.ShipETA
		from  Machine.dbo.PartPO_Detail as  t
		inner join Trade_to_Pms.dbo.MmsPO_Detail s on t.id=s.MmsReqID and t.seq2=s.seq2 and s.Junk=0 AND s.ID=t.TPEPOID
		inner join Machine.dbo.PartPO a on t.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where 1=1
		

		update t
		set t.TpePOID = s.id,
		t.seq1=s.seq1,
		t.SuppDelivery=s.SuppDelivery,
		t.EstETA=s.EstETA,
		t.Complete = isnull(s.Complete,0),
		t.TPEQty=s.Qty,
		t.Foc=s.Foc,
		t.ShipQty=s.ShipQty,
		t.ShipFoc=s.ShipFoc,
		t.ShipETA=s.ShipETA,
		t.Junk = s.Junk
		from  Machine.dbo.MiscPO_Detail as  t
		inner join Trade_to_Pms.dbo.MmsPO_Detail s on t.id=s.MmsReqID  and t.seq2=s.seq2
		inner join Trade_To_Pms.DBO.MmsPO a on s.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where a.Type='O'
		

------------------MachinePO_Detail Type='M'----------------------

	Merge Machine.[dbo].[MachinePO_Detail] as t
	using (select * from Trade_To_PMS.dbo.MachinePO_Detail a WITH (NOLOCK)) as s
	on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2
	when matched then 
				update set
				t.MachineGroupID= s.MachineGroupID,
				t.MasterGroupID= s.MasterGroupID,
				t.MachineBrandID= s.MachineBrandID,
				t.Model= s.Model,
				t.Description= s.Description,
				t.Qty= s.Qty,
				t.FOC= s.FOC,
				t.Price= s.Price,				
				t.Remark= s.Remark,
				t.MachineReqID= s.MmsReqID,
				t.Junk= s.Junk,
				t.RefNo = ISNULL(s.RefNo,''),
				t.DescriptionDetail = s.DescriptionDetail,
				t.UnitID = s.UnitID,
				t.Delivery = s.Delivery,
				t.SuppEstETA = s.SuppEstETA,
				t.Complete = s.Complete,
				t.ShipQty = isnull(s.ShipQty,0),
				t.ShipFOC = isnull(s.ShipFOC,0),
				t.ShipETA = s.ShipETA 
		when not matched by target then
		insert  (ID ,Seq1 ,Seq2 ,MasterGroupID ,MachineGroupID ,MachineBrandID ,Model ,Description 
				,Qty ,FOC ,Price ,Remark ,MachineReqID ,Junk ,RefNo ,DescriptionDetail ,UnitID ,Delivery ,SuppEstETA
				,Complete , ShipQty, ShipFOC,ShipETA)
		values	(s.ID ,s.Seq1 ,s.Seq2 ,s.MasterGroupID ,s.MachineGroupID ,s.MachineBrandID ,s.Model ,s.Description
				 ,s.Qty ,s.FOC ,s.Price ,s.Remark ,s.MmsReqID ,s.Junk ,ISNULL(s.RefNo ,'') ,s.DescriptionDetail ,s.UnitID ,s.Delivery ,s.SuppEstETA
				 ,s.Complete ,s.ShipQty ,s.ShipFOC ,s.ShipETA);

------------------MachinePO_Detail_TPEAP----------------------
	Merge	Machine.[dbo].[MachinePO_Detail_TPEAP] as t
	using	(select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty), b.ExportID
				from Machine.[dbo].[MachinePO_Detail] a
				inner join Trade_To_PMS.dbo.MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
				group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price, b.ExportID
				) as s
	on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2 and t.TPEPOID = s.TPEPOID and t.ExportID = s.ExportID
	when matched then 
		update	set	t.APDATE = s.APDATE,
					t.VoucherID = s.VoucherID,
					t.Price = s.Price,
					t.Qty = s.Qty
	when not matched by target then
		insert	(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty,ExportID)
			values(s.ID,s.Seq1,s.Seq2,s.TPEPOID,s.APDATE,s.VoucherID,s.Price,s.Qty,s.ExportID);

------------------MiscPO_Detail_TPEAP----------------------
	Merge	Machine.[dbo].[MiscPO_Detail_TPEAP] as t
	using	(select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty) 
				from Machine.[dbo].[MiscPO_Detail] a
				inner join Trade_To_PMS.dbo.MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
				group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price
				) as s
	on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2
	when matched then 
		update	set	t.TPEPOID = s.TPEPOID,
					t.APDATE = s.APDATE,
					t.VoucherID = s.VoucherID,
					t.Price = s.Price,
					t.Qty = s.Qty
	when not matched by target then
		insert	(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty)
			values(s.ID,s.Seq1,s.Seq2,s.TPEPOID,s.APDATE,s.VoucherID,s.Price,s.Qty);

------------------PartPO_Detail_TPEAP----------------------
	Merge	Machine.[dbo].[PartPO_Detail_TPEAP] as t
	using	(select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty) 
				from Machine.[dbo].[PartPO_Detail] a
				inner join Trade_To_PMS.dbo.MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
				group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price
				) as s
	on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2
	when matched then 
		update	set	t.TPEPOID = s.TPEPOID,
					t.APDATE = s.APDATE,
					t.VoucherID = s.VoucherID,
					t.Price = s.Price,
					t.Qty = s.Qty
	when not matched by target then
		insert	(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty)
			values(s.ID,s.Seq1,s.Seq2,s.TPEPOID,s.APDATE,s.VoucherID,s.Price,s.Qty);

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

	  -----------MachineMasterGroup------------------------
	  Merge Machine.dbo.MachineMasterGroup as t
		using Trade_To_Pms.dbo.MachineMasterGroup as s 
		on t.id=s.id
		when matched then
				update set 
				t.ID				 = s.ID,				
				t.Description		 = s.Description,		
				t.DescriptionCH	 = s.DescriptionCH	,
				t.Junk			 = s.Junk	,		
				t.AddName			 = s.AddName,			
				t.AddDate			 = s.AddDate,			
				t.EditName		 = s.EditName,		
				t.EditDate		 = s.EditDate		
		when not matched by target then
				insert(ID			 ,
					   Description	 ,
					   DescriptionCH,	
					   Junk			 ,
					   AddName		 ,
					   AddDate		 ,
					   EditName		 ,
					   EditDate		
				)
				values(s.ID			 ,
					   s.Description	 ,
					   s.DescriptionCH,	
					   s.Junk			 ,
					   s.AddName		 ,
					   s.AddDate		 ,
					   s.EditName		 ,
					   s.EditDate	
				);

	  -----------MachineGroup------------------------

		Merge Machine.dbo.MachineGroup as t
		using Trade_To_Pms.dbo.MachineGroup as s 
		on t.id=s.id AND t.MasterGroupID = s.MasterGroupID
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
				t.EditDate= s.EditDate,
				t.MasterGroupID = s.MasterGroupID
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
				,MasterGroupID
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
				s.EditDate,
				s.MasterGroupID
				);
		
		-----------RepairType------------------------
		Merge Machine.dbo.RepairType as t
		using Trade_To_Pms.dbo.RepairType as s 
		on t.id=s.id
		when matched then
				update set 
				t.ID= s.ID,
				t.Name		   = s.Name			,
				t.Description  = s.Description	,
				t.Junk		   = s.Junk			,
				t.AddDate	   = s.AddDate		,
				t.AddName	   = s.AddName		,
				t.EditDate	   = s.EditDate		,
				t.EditName	   = s.EditName
		when not matched by target then
				insert(Id
					  ,Name
					  ,Description
					  ,Junk
					  ,AddDate
					  ,AddName
					  ,EditDate
					  ,EditName
				)
				values(s.Id
					  ,s.Name
					  ,s.Description
					  ,s.Junk
					  ,s.AddDate
					  ,s.AddName
					  ,s.EditDate
					  ,s.EditName
				);

	 -----------TPEMachine------------------------
	Merge Machine.dbo.TPEMachine as t
	Using (
		select * 
		from Trade_To_Pms.dbo.Part WITH (NOLOCK) 
		where type = 'M' 
	)as s on t.ID = s.Refno 
	when matched then 
		update set		
		 	t.MasterGroupID		    = s.MasterGroupID
			,t.MachineGroupID		= s.MachineGroupID
			,t.Model				= s.Model
			,t.MachineBrandID		= s.MachineBrandID
			,t.Description			= s.Description
			,t.DescriptionDetail	= s.DescriptionDetail
			,t.Origin				= s.Origin
			,t.Junk					= s.Junk
			,t.AddName				= s.AddName
			,t.AddDate				= s.AddDate
			,t.EditName				= s.EditName
			,t.EditDate				= s.EditDate
	when not matched by target and s.type='M' then 
		insert 
			(ID,MasterGroupID,MachineGroupID,Model,MachineBrandID,Description
			,DescriptionDetail,Origin,Picture1,Picture2,Junk
			,AddName,AddDate,EditName,EditDate
			)
		values
			(s.Refno 			, s.MasterGroupID  , s.MachineGroupID , s.Model 		, s.MachineBrandID 	, s.Description
			 , s.DescriptionDetail 		, s.Origin 	, '' 		, '' 		, s.Junk
			 , s.AddName 		, s.AddDate  	, s.EditName  	, s.EditDate 		);
	
	 -----------PartPrice_History ------------------------
	Merge Machine.dbo.PartPrice_History  as t
	Using (
		select * 
		from Trade_To_Pms.dbo.TradeHIS_MMS WITH (NOLOCK) 
		where TableName = 'Part' and HisType = 'ControlParts' 
	)as s on t.TradeHisMMSUkey = s.Ukey  
	when matched then 
		update set 
				 t.PartID 		= s.RefNo  
				,t.HisType		= s.HisType
				,t.OldValue		= s.OldValue
				,t.NewValue		= s.NewValue
				,t.Remark		= s.Remark
				,t.AddName		= s.AddName
				,t.AddDate		= s.AddDate
	when not matched by target then 
		insert 
           (TradeHisMMSUkey,  PartID,  HisType,  OldValue,  NewValue
		   ,Remark,  AddName,  AddDate)
		values
			(s.Ukey    , s.RefNo , s.HisType 	, s.OldValue , s.NewValue 		
			, s.Remark 	 , s.AddName , s.AddDate 		);
	
	------------MachinePending------------------
	Merge Machine.dbo.MachinePending  as t
		using Trade_To_Pms.dbo.MachinePending as s 
		on t.id=s.id
		when matched and t.status = 'Confirmed' then update set 
			t.TPEComplete = s.TPEComplete
	;
	
	------------MachinePending_Detail------------------
	declare @Tdebit table(id varchar(13),MachineID varchar(16),TPEReject int)
	Merge Machine.dbo.MachinePending_Detail  as t
	using (
		select md.*,m.status
		from Trade_To_Pms.dbo.MachinePending_Detail md
		inner join Machine.dbo.MachinePending m on m.id = md.id
	)as s 
	on t.id=s.id and t.seq = s.seq
	when matched and s.status = 'Confirmed' and s.TPEApvDate is not null then update set 
		t.TPEReject = s.TPEReject
	output inserted.ID,inserted.MachineID,inserted.TPEReject
	into @Tdebit
	;

	update Machine.dbo.Machine set Status = 'Pending',EstFinishRepairDate =null where ID in (select MachineID from @Tdebit where TPEReject = 0)  

	update m set m.Status = 'Good'
	from Machine.dbo.Machine m
	where exists(select 1 from @Tdebit t where t.MachineID = m.ID and TPEReject = 1)

	update md set Results = 'Reject'
	from MachinePending_Detail md
	where exists(select 1 from @Tdebit t where t.ID = md.ID and t.MachineID = md.MachineID and TPEReject = 1)

	END
