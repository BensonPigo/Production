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
,[Paper] =t.Paper
,[DescriptionDetail] = t.DescriptionDetail
INTO #Formula_Change_Table
FROM dbo.Part t
INNER JOIN dbo.SciTrade_To_Pms_Part s
ON t.id=s.Refno  AND s.Type='P'
AND t.Formula<> s.Formula
;
	INSERT INTO dbo.PartFormula_History ([PartID],[OldFormula],[NewFormula],[AddName],[AddDate])
	SELECT [PartID],[OldFormula],[NewFormula],[AddName],[AddDate] FROM #Formula_Change_Table
;
DROP TABLE #Formula_Change_Table
;
	--Step.2. Merge
update t set t.Description = s.Description
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
			 , t.MachineBrandID = s.MachineBrandID
			 , t.Needle = s.Needle
			 , t.ControlPart = s.ControlParts
			 , t.MOQ = isnull(convert(int, s.MOQ),0)
			 , t.Paper = isnull(s.Paper,0)
			 , t.DescriptionDetail = isnull(s.DescriptionDetail,'')
from dbo.Part t
inner join ( select * from dbo.SciTrade_To_Pms_Part WITH (NOLOCK) where type = 'P' ) as s on t.id = s.Refno 

insert into dbo.Part(ID 		, Description 	, Partno 		, MasterGroupID 	, MachineGroupID 	, MachineBrandID
	 	 	 , UseUnit 			, PoUnit 		, Price 		, PurchaseBatchQty 	, Junk
			 , PurchaseFrom 	, SuppID 		, CurrencyID 	, Formula	 		, Fix
			 , AddName  		, AddDate 		, EditName 		, EditDate 			, Lock, Needle , ControlPart
			 ,	MOQ             , Paper			, DescriptionDetail)
			 select s.refno 			, s.Description , s.Partno 		, s.MasterGroupID 	, s.MachineGroupID 	, s.MachineBrandID
					, s.UnitID 		, s.POUnitID 	, s.Price 		, s.BatchQty 		, s.Junk
					, 'T'  			, s.SuppID 		, s.CurrencyID 	, s.Formula 		, s.Fix
					, s.AddName 		, s.AddDate  	, s.EditName  	, s.EditDate 		, s.Lock , s.Needle , s.ControlParts
					, isnull(convert(int, s.MOQ),0)
					,isnull(s.Paper,0)
					,isnull(s.DescriptionDetail,'')
			 from dbo.SciTrade_To_Pms_Part s WITH (NOLOCK) 
			 where s.type = 'P' and not exists(select 1 from dbo.Part t where t.id=s.Refno)

	---------- Misc, type='O'---------------------
	update t set t.Model= s.Model,
				 t.MiscBrandID= s.MachineBrandID,
				 t.Description= s.Description,
				 t.UnitID= s.PoUnitID,
				 t.CurrencyID= s.CurrencyID,
				 t.Price= s.Price,
				 t.SuppID= iif(t.PurchaseFrom = 'T', s.SuppID, t.SuppID), --PurchaseFrom='T'�ɤ~�ݭn��s
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
				 t.AssetTypeID = s.AssetTypeID,
				 t.Improvement = s.Improvement
	from dbo.Misc  t
	inner join dbo.SciTrade_To_Pms_Part s on t.id=s.Refno and s.type='O' 

	insert into dbo.Misc(ID
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
												,AssetTypeID
												,Improvement)
				select	s.refno,
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
						s.AssetTypeID,
						s.Improvement
				from dbo.SciTrade_To_Pms_Part s
				where s.type='O'  and not exists(select 1 from dbo.Misc t where t.id = s.Refno)

				
    ---------- MiscOther, type='R'---------------------
    update t set t.Model= s.Model,
                 t.MtlTypeID=s.MtlTypeID,
                 t.Description= s.Description,
                 t.UnitID= s.PoUnitID,
                 t.CurrencyID= s.CurrencyID,
                 t.Price= s.Price,
                 t.IsMachine= s.IsMachine,
                 t.IsAsset= s.IsAsset,
                 t.Remark= s.Remark,
                 t.Junk= s.Junk,
                 t.AddName= s.AddName,
                 t.AddDate= s.AddDate,
                 t.EditName= s.EditName,
                 t.EditDate= s.EditDate,
                 t.DescriptionDetail = s.DescriptionDetail
    from dbo.MiscOther  t
    inner join dbo.SciTrade_To_Pms_Part s on t.id=s.Refno and t.BrandID = s.BrandID and t.SuppID = s.SuppID and s.type='R' 

    insert into dbo.MiscOther(ID
                            ,Model
                            ,BrandID
                            ,MtlTypeID
                            ,Description
                            ,UnitID
                            ,CurrencyID
                            ,Price
                            ,SuppID
                            ,PurchaseFrom
                            ,IsMachine
                            ,IsAsset
                            ,Remark
                            ,Junk
                            ,AddName
                            ,AddDate
                            ,EditName
                            ,EditDate
                            ,DescriptionDetail)
                select  s.refno,
                        s.Model,
                        s.BrandID,
                        s.MtlTypeID,
                        s.Description,
                        s.PoUnitID,
                        s.CurrencyID,
                        s.Price,
                        s.SuppID,
                        'T',
                        s.IsMachine,
                        s.IsAsset,
                        s.Remark,
                        s.Junk,
                        s.AddName,
                        s.AddDate,
                        s.EditName,
                        s.EditDate,
                        s.DescriptionDetail
                from dbo.SciTrade_To_Pms_Part s
                where s.type='R' and not exists(select 1 from dbo.MiscOther t where t.id = s.Refno and t.BrandID = s.BrandID and t.SuppID = s.SuppID)


	-----------------PartQuot , type='P'-----------------------------
	select distinct a.* 
	into #tmpPartQuot
	from SciTrade_To_Pms_MmsQuot a WITH (NOLOCK) 
	inner join  SciTrade_To_Pms_Part b WITH (NOLOCK) on a.PartUkey=b.Ukey where a.type='P'

	update t set
		t.purchaseFrom='T',
		t.CurrencyID=s.CurrencyID,
		t.Price=s.Price,
		t.AddName=s.AddName,
		t.AddDate=s.AddDate,
		t.EditName=s.EditName,
		t.EditDate=s.EditDate,
		T.ldefault = S.IsDefault
	from dbo.PartQuot t
	inner join #tmpPartQuot s on t.id=s.refno and  t.PartBrandID = s.mmsBrandID and t.SuppID = s.SuppID
	where t.PurchaseFrom = 'T'

	insert into dbo.PartQuot(	id,
													purchaseFrom,
													suppid,
													CurrencyID,
													Price,
													AddName,
													AddDate,
													EditName,
													EditDate,
													PartBrandID,
													ldefault)
										select		s.Refno,
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
										from #tmpPartQuot s
										where not exists(select 1 from dbo.PartQuot t where		t.id=s.refno and  
																								t.PartBrandID = s.mmsBrandID and 
																								t.SuppID = s.SuppID and 
																								t.PurchaseFrom = 'T')
	
	delete t
	from dbo.PartQuot t 
	where	t.purchaseFrom='T' and 
			t.id in (select refno from SciTrade_To_Pms_Part WITH (NOLOCK)) and
			not exists(select 1 from #tmpPartQuot s where  t.id=s.refno and  
														   t.PartBrandID = s.mmsBrandID and 
														   t.SuppID = s.SuppID)

	drop table #tmpPartQuot

	---------------PartBrand-------------------------------
	select *
	into #tmpTrade_To_PmsPartBrand
	from SciTrade_To_Pms_MmsBrand with (nolock) where type='P'

	update t set t.Name= 	      s.Name 
				,t.Junk= 	      s.Junk 
				,t.AddName= 	      s.AddName 
				,t.AddDate= 	      s.AddDate 
				,t.EditName= 	      s.EditName 
				,t.EditDate= 	      s.EditDate 
	from dbo.PartBrand t
	inner join #tmpTrade_To_PmsPartBrand s on t.id=s.id

	insert into dbo.PartBrand(ID
													,Name 
													,Junk 
													,AddName 
													,AddDate 
													,EditName 
													,EditDate)
						select	s.ID
								,s.Name 
								,s.Junk 
								,s.AddName 
								,s.AddDate 
								,s.EditName 
								,s.EditDate
						from #tmpTrade_To_PmsPartBrand s
						where not exists(select 1 from dbo.PartBrand t where t.id=s.id)

	delete t
	from dbo.PartBrand t
	where not exists(select 1 from #tmpTrade_To_PmsPartBrand s where t.id=s.id)

	drop table #tmpTrade_To_PmsPartBrand
--------------MiscBrand--------------------------------------
	-- O: Miscellaneous
	select *
	into #tmpTrade_To_PmsMiscBrand
	from SciTrade_To_Pms_MmsBrand with (nolock) where type='O'

	update t set t.AddName= 	      s.AddName   
				,t.AddDate= 	      s.AddDate   
				,t.EditName= 	      s.EditName   
				,t.EditDate= 	      s.EditDate 
	from dbo.MiscBrand t
	inner join #tmpTrade_To_PmsMiscBrand s on t.id=s.id

	insert into dbo.MiscBrand(ID
													,Name 
													,Junk
													,AddName 
													,AddDate 
													,EditName 
													,EditDate ,
													[LOCAL])
					select	s.id  ,
							s.Name  , 
							s.Junk   ,
							s.AddName ,  
							s.AddDate  , 
							s.EditName  , 
							s.EditDate,   
							0
					from #tmpTrade_To_PmsMiscBrand s
					where not exists(select 1 from dbo.MiscBrand t where t.id=s.id)

	delete	t
	from dbo.MiscBrand t
	where not exists(select 1 from #tmpTrade_To_PmsMiscBrand s where t.id=s.id)

	drop table #tmpTrade_To_PmsMiscBrand

	---------------MachBrand-----------------------------------
	select * into #tmpTrade_To_PmsMachineBrand
	from SciTrade_To_Pms_MmsBrand WITH (NOLOCK) where type='M'

	update t set t.Name= 	      s.Name   
				,t.Junk= 	      s.Junk   
				,t.AddName= 	      s.AddName   
				,t.AddDate= 	      s.AddDate   
				,t.EditName= 	      s.EditName   
				,t.EditDate= 	      s.EditDate   
	from dbo.MachineBrand t
	inner join #tmpTrade_To_PmsMachineBrand s on t.id=s.id

	insert into dbo.MachineBrand(ID
		  ,Name 
		  ,Junk 
		  ,AddName 
		  ,AddDate 
		  ,EditName 
		  ,EditDate )
		  select s.ID   ,
				 s.Name  , 
				 s.Junk   ,
				 s.AddName ,  
				 s.AddDate  , 
				 s.EditName  , 
				 s.EditDate 
		  from #tmpTrade_To_PmsMachineBrand s
		  where not exists(select 1 from dbo.MachineBrand t where t.id=s.id)

	drop table #tmpTrade_To_PmsMachineBrand

	----------------RepairPO-------------------------
	update a
		set a.Status = case when b.Status is null and a.Status = 'Complete' then 'Confirmed'
						when b.Status in ('Complete', 'Junk') then b.Status
				   else a.Status
				   end 
	from RepairPO a
	inner join SciTrade_To_Pms_RepairReq b on a.ID = b.ID
	
	----------------RepairPO_Detail-------------------------

	update dbo.RepairPO_Detail
	set ShipQty = b.ShipQty ,
		ShipFoc = b.ShipFoc ,
		EstCost = b.EstCost ,
		ActCost = b.ActCost ,
		NewCost = b.NewCost ,
		ETA = b.ETA ,
		Cancel = b.Cancel ,
		Complete = b.Complete ,
		TPEPOID = b.TradePOID ,
		ResponsibleFTY  = b.ResponsibleFTY 
	from dbo.RepairPO_Detail a
	inner join SciTrade_To_Pms_RepairReq_Detail b on a.id=b.ID and a.Seq2=b.Seq2

	----------------PartPO2-------------------------

	update dbo.PartPO_Detail
	set 
		Junk = b.Cancel 
		,TPEPOID = b.MmsPoID
		,FinalBrand = b.FinalBrand
	from dbo.PartPO_Detail a
	inner join SciTrade_To_Pms_MmsReq_Detail b on a.id=b.ID and a.SEQ2=b.Seq2


	----------------��z�Ҧ��u�t--------------------
	declare @Sayfty table(id varchar(10)) --�u�t�N�X
	insert @Sayfty select id from dbo.Factory
	
	-- if type<>'M'
	select
	a.ID,
	a.Seq1,
	a.Seq2,
	a.PartID,
	a.PartReqID,
	[NewSeq1] = b.Seq1,
	b.SuppDelivery,
	b.EstETA,
	[Complete] = ISNULL(b.Complete,0),
	B.Qty,
	B.Foc,
	B.ShipQty,
	B.ShipFoc,
	B.ShipETA,
	B1.CurrencyID,
	B.Price
	into #tmpMmsPO_Detail
	from  dbo.PartPO_Detail A
	INNER JOIN dbo.SciTrade_To_Pms_MmsPO_Detail B  on	a.PartID=b.Refno AND 
														a.SEQ2=b.Seq2 and 
														a.id = b.MmsReqID and 
														b.Junk=0 AND 
														b.ID=a.TPEPOID
	INNER JOIN  dbo.SciTrade_To_Pms_MmsPO B1 on B1.ID = B.ID
	INNER JOIN  dbo.PartPO C ON A.ID=C.ID
	WHERE C.FactoryID in (select id from @Sayfty)

	UPDATE A
	SET
	A.SEQ1=b.NewSeq1,
	A.SuppDelivery=b.SuppDelivery,
	A.EstETA=b.EstETA,
	A.Complete = ISNULL(b.Complete,0),
	A.TPEQty=B.Qty,
	A.Foc=B.Foc,
	A.ShipQty=B.ShipQty,
	A.ShipFoc=B.ShipFoc,
	A.ShipETA=B.ShipETA,
	A.TPECurrencyID = B.CurrencyID,
	A.TPEPrice = B.Price
	FROM dbo.PartPO_Detail A
	INNER JOIN #tmpMmsPO_Detail B  on	A.ID = B.ID and 
										A.Seq1 = B.Seq1 and 
										A.Seq2 = B.Seq2 and 
										A.PartID = B.PartID and
										A.PartReqID = B.PartReqID

	drop table #tmpMmsPO_Detail

	UPDATE dbo.MiscPO_Detail
	SET TPEPOID = B.id,
	SEQ1=b.Seq1,
	SuppDelivery=b.SuppDelivery,
	EstETA=b.EstETA,
	TPEQty = B.Qty,
	Foc = B.Foc,
	ShipQty = B.ShipQty,
	ShipFoc = B.ShipFoc,
	ShipETA = B.ShipETA,
	TPECurrencyID = c.CurrencyID,
	TPEPrice = b.Price
	FROM dbo.MiscPO_Detail A
	INNER JOIN dbo.SciTrade_To_Pms_MmsPO_Detail B  on  a.MiscID=b.Refno 
													and  a.SEQ2=b.Seq2 
													and a.id = b.MmsReqID
	INNER JOIN  dbo.SciTrade_To_Pms_MmsPO C ON B.ID=C.ID
	WHERE C.Type ='O'
	and C.FactoryID in (select id from @Sayfty)

	UPDATE a
	SET Junk = b.Cancel
	FROM dbo.MiscPO_Detail a
	INNER JOIN dbo.SciTrade_To_Pms_MmsReq_Detail b ON a.ID = b.ID and a.Seq2 = b.Seq2

	-- ------------MachinePO--------------------
	select	a.CDate
			,a.FactoryID
			,a.CurrencyID
			,a.Junk
			,a.ApvName
			,a.Handle
			,a.Amount
			,a.Vatrate
			,a.Vat
			,a.Remark
			,a.ApvDate
			,a.AddName
			,a.AddDate
			,a.EditName
			,a.EditDate
			,a.ID
			,a.SuppID
			,b.MdivisionID 
			into #tmpTrade_To_PmsMachinePO
	from dbo.SciTrade_To_Pms_MmsPO a WITH (NOLOCK) left join Production.dbo.scifty b WITH (NOLOCK) on a.factoryid = b.id
	where a.factoryid in (select id from @Sayfty) and a.type = 'M'

	update t set	t.CDate = s.CDate ,
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
	from dbo.MachinePO t
	inner join #tmpTrade_To_PmsMachinePO s on t.id=s.id
	and not (s.junk = 0 and s.ApvDate is null)

	insert into dbo.MachinePO(ID ,	  CDate ,PurchaseFrom ,  FactoryID ,   CurrencyID ,   Handle,    LocalSuppID ,  Amount ,   Vatrate ,   Vat ,        Remark ,        Approve ,        ApproveDate ,        AddName ,        AddDate ,        EditName ,        EditDate
		 ,MDivisionID,Status)
	select
	s.ID ,	s.CDate , 'T' ,	s.FactoryID ,s.CurrencyID ,	s.Handle,s.SuppID ,		s.Amount ,		s.Vatrate ,	Vat ,		s.Remark ,		s.ApvName ,		s.ApvDate ,		s.AddName ,		s.AddDate ,		s.EditName ,		s.EditDate
,MDivisionID, IIF(s.Junk = 1,'Junked',IIF(s.ApvName != '',ApvName,'New'))
	from #tmpTrade_To_PmsMachinePO s
	where not exists(select 1 from dbo.MachinePO t where t.id=s.id)
	and not (s.junk = 0 and s.ApvDate is null)
	
	select t.ID
	into #deleteMachinePOID
	from dbo.MachinePO t
	inner join #tmpTrade_To_PmsMachinePO s on t.id=s.id
	and s.junk = 0 and s.ApvDate is null

	delete dbo.MachinePO where ID in(select ID from #deleteMachinePOID)
	
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
		t.ShipETA=s.ShipETA,
		t.TPECurrencyID = s1.CurrencyID,
		t.TPEPrice = s.Price
		from  dbo.PartPO_Detail as  t
		inner join dbo.SciTrade_To_Pms_MmsPO_Detail s on t.id=s.MmsReqID and t.seq2=s.seq2 and s.Junk=0 AND s.ID=t.TPEPOID
		inner join dbo.SciTrade_To_Pms_MmsPO s1 on s1.ID=s.ID
		inner join dbo.PartPO a on t.id=a.ID
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
		t.TPECurrencyID = a.CurrencyID,
		t.TPEPrice = s.Price
		from  dbo.MiscPO_Detail as  t
		inner join dbo.SciTrade_To_Pms_MmsPO_Detail s on t.id=s.MmsReqID  and t.seq2=s.seq2
		inner join dbo.SciTrade_To_Pms_MmsPO a on s.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where a.Type='O'
		
------------------MachinePO_Detail Type='M'----------------------
	update t set t.MachineGroupID= s.MachineGroupID,
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
	from dbo.MachinePO_Detail t
	inner join dbo.SciTrade_To_Pms_MachinePO_Detail s WITH (NOLOCK) on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2
	inner join dbo.SciTrade_To_Pms_MmsPO sM WITH (NOLOCK) on sM.id = s.ID
	where not (sM.junk = 0 and sM.ApvDate is null)

	insert into dbo.MachinePO_Detail(ID ,Seq1 ,Seq2 ,MasterGroupID ,MachineGroupID ,MachineBrandID ,Model ,Description 
				,Qty ,FOC ,Price ,Remark ,MachineReqID ,Junk ,RefNo ,DescriptionDetail ,UnitID ,Delivery ,SuppEstETA
				,Complete , ShipQty, ShipFOC,ShipETA)
	select	s.ID ,s.Seq1 ,s.Seq2 ,s.MasterGroupID ,s.MachineGroupID ,s.MachineBrandID ,s.Model ,s.Description
				,s.Qty ,s.FOC ,s.Price ,s.Remark ,s.MmsReqID ,s.Junk ,ISNULL(s.RefNo ,'') ,s.DescriptionDetail ,s.UnitID ,s.Delivery ,s.SuppEstETA
				,s.Complete ,s.ShipQty ,s.ShipFOC ,s.ShipETA
	from dbo.SciTrade_To_Pms_MachinePO_Detail s
	inner join dbo.SciTrade_To_Pms_MmsPO sM WITH (NOLOCK) on sM.id = s.ID
	where not exists(select 1 from dbo.MachinePO_Detail t where t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2)
	and not (sM.junk = 0 and sM.ApvDate is null)

	delete dbo.MachinePO_Detail where ID in(select ID from #deleteMachinePOID)
	drop table #deleteMachinePOID
	
		-- �R�����s�b Trade ��Ӫ���� , �o�������Y, �S��������
	delete MachinePO_Detail
	where exists(select 1 from #tmpTrade_To_PmsMachinePO t where ID = MachinePO_Detail.ID)
	and not exists(select 1 from #tmpTrade_To_PmsMachinePO t inner join SciTrade_To_Pms_MachinePO_Detail d on t.id = d.id where t.ID = MachinePO_Detail.ID and d.seq1 = MachinePO_Detail.seq1 and d.seq2 = MachinePO_Detail.seq2)
	drop table #tmpTrade_To_PmsMachinePO
	
------------------MachinePO_Detail Type='R'----------------------
	update t set
         [Cdate]        = sM.[Cdate]
        ,[FactoryID]	= sM.[FactoryID]
        ,[MDivisionID]	= b.MDivisionID
        ,[CurrencyID]	= sM.[CurrencyID]
        ,[Amount]		= sM.[Amount]
        ,[Vatrate]		= sM.[Vatrate]
        ,[Vat]			= sM.[Vat]
        ,[Remark]		= sM.[Remark]
        ,[ApvName]		= sM.[ApvName]
        ,[ApvDate]		= sM.[ApvDate]
		,[Junk]			= sM.[Junk]
        ,[AddName]		= sM.[AddName]
        ,[AddDate]		= sM.[AddDate]
        ,[EditName]		= sM.[EditName]
        ,[EditDate]		= sM.[EditDate]
	from MiscOtherPO t
	inner join dbo.SciTrade_To_Pms_MmsPO sM WITH (NOLOCK) on sM.id = t.ID
	left join Production.dbo.scifty b on sM.FactoryID = b.ID
	where sM.type = 'R'

	update t set 
         [MiscOtherID] = s.RefNo
        ,[BrandID]	   = sM.[BrandID]
        ,[SuppID]	   = sM.[SuppID]
        ,[UnitID]	   = s.[UnitID]
		,Qty		   = s.Qty
        ,[TPEPrice]	   = s.Price
        ,[TPEQty]	   = s.ShipQty
        ,[TPEFoc]	   = s.Foc
		,MachineReqID  = s.MmsReqID
		,Junk		   = s.Junk
	from dbo.MiscOtherPO_Detail t
	inner join dbo.SciTrade_To_Pms_MmsPO_Detail s WITH (NOLOCK) on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2
	inner join dbo.SciTrade_To_Pms_MmsPO sM WITH (NOLOCK) on sM.id = s.ID
	where sM.type = 'R'

	insert into MiscOtherPO
           ([ID]
           ,[Cdate]
           ,[FactoryID]
           ,[MDivisionID]
           ,[PurchaseFrom]
           ,[CurrencyID]
           ,[Amount]
           ,[Vatrate]
           ,[Vat]
           ,[Remark]
		   ,[ApvName]
           ,[ApvDate]
		   ,[Junk]
           ,[AddName]
           ,[AddDate]
           ,[EditName]
           ,[EditDate])
	select
		 sM.[ID]
        ,sM.[Cdate]
        ,sM.[FactoryID]
        ,b.[MDivisionID]
        ,'T'
        ,sM.[CurrencyID]
        ,sM.[Amount]
        ,sM.[Vatrate]
        ,sM.[Vat]
        ,sM.[Remark]
		,sM.[ApvName]
        ,sM.[ApvDate]
		,sM.[Junk]
        ,sM.[AddName]
        ,sM.[AddDate]
        ,sM.[EditName]
        ,sM.[EditDate]
	from dbo.SciTrade_To_Pms_MmsPO sM
	left join MiscOtherPO t WITH (NOLOCK) on sM.id = t.ID
	left join Production.dbo.scifty b on sM.FactoryID = b.ID
	where sM.type = 'R'
	and t.id is null
	
	INSERT INTO [dbo].[MiscOtherPO_Detail]
			([ID]
			,[Seq1]
			,[Seq2]
			,[MiscOtherID]
			,[BrandID]
			,[SuppID]
			,[UnitID]
			,[Qty]
			,[TPEPrice]
			,[TPEQty]
			,[TPEFoc]
			,MachineReqID
			,Junk)
	select
		 s.[ID]
		,s.[Seq1]
		,s.[Seq2]
		,s.RefNo
		,sM.[BrandID]
		,sM.[SuppID]
		,s.[UnitID]
		,s.[Qty]
		,s.Price
		,s.ShipQty
		,s.Foc
		,s.MmsReqID
		,s.Junk
	from dbo.SciTrade_To_Pms_MmsPO sM 
	inner join dbo.SciTrade_To_Pms_MmsPO_Detail s WITH (NOLOCK) on sM.id = s.ID
	left join dbo.MiscOtherPO_Detail t on t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2
	where sM.type = 'R'
	and t.ID is null

------------------MachinePO_Detail_TPEAP----------------------
	select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty), b.ExportID
	into #tmpMachinePO_Detail_TPEAP
	from dbo.MachinePO_Detail a
	inner join dbo.SciTrade_To_Pms_MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
	group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price, b.ExportID

	update t set	t.APDATE = s.APDATE,
					t.VoucherID = s.VoucherID,
					t.Price = s.Price,
					t.Qty = s.Qty
	from dbo.MachinePO_Detail_TPEAP t
	inner join #tmpMachinePO_Detail_TPEAP s on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2 and t.TPEPOID = s.TPEPOID and t.ExportID = s.ExportID

	insert into dbo.MachinePO_Detail_TPEAP(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty,ExportID)
		select s.ID,s.Seq1,s.Seq2,s.TPEPOID,s.APDATE,s.VoucherID,s.Price,s.Qty,s.ExportID
		from #tmpMachinePO_Detail_TPEAP s
		where not exists(select 1 from dbo.MachinePO_Detail_TPEAP t where t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2 and t.TPEPOID = s.TPEPOID and t.ExportID = s.ExportID)

	drop table #tmpMachinePO_Detail_TPEAP

	
------------------MachinePO_Detail_TPETTbefore----------------------
INSERT INTO [dbo].MachinePO_Detail_TPETTbefore([ID],[APDate],[VoucherID],TPEPOID)
select a.[ID],a.[APDate],a.[VoucherID],a.[POID]
from SciTrade_To_Pms_MmsTTbefore a
left join MachinePO_Detail_TPETTbefore b on a.ID = b.ID and a.[APDate] = b.[APDate] and a.[VoucherID] = b.[VoucherID] and a.[POID] = b.TPEPOID
where b.ID is null

------------------MachinePO_Detail_TPESurcharge----------------------

INSERT INTO [dbo].[MachinePO_Detail_TPESurcharge]([ID],[APDate],[VoucherID],TPEPOID,[Seq1],[Seq2])
select a.[ID],a.[APDate],a.[VoucherID],a.[POID],a.[Seq1],a.[Seq2]
from SciTrade_To_Pms_MmsSurcharge a
left join MachinePO_Detail_TPESurcharge b on a.ID = b.ID and a.[APDate] = b.[APDate] and a.[VoucherID] = b.[VoucherID] and a.[POID] = b.TPEPOID
and a.[Seq1] = b.[Seq1] and a.[Seq2] = b.[Seq2]
where b.ID is null



------------------MiscPO_Detail_TPEAP----------------------
	select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty) 
	into #tmpMiscPO_Detail
	from dbo.MiscPO_Detail a
	inner join dbo.SciTrade_To_Pms_MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
	group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price

	update t set t.TPEPOID = s.TPEPOID,
					t.APDATE = s.APDATE,
					t.VoucherID = s.VoucherID,
					t.Price = s.Price,
					t.Qty = s.Qty
	from dbo.MiscPO_Detail_TPEAP t
	inner join #tmpMiscPO_Detail s on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2

	insert into dbo.MiscPO_Detail_TPEAP(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty)
		select s.ID,s.Seq1,s.Seq2,s.TPEPOID,s.APDATE,s.VoucherID,s.Price,s.Qty
		from #tmpMiscPO_Detail s
		where not exists(select 1 from dbo.MiscPO_Detail_TPEAP t where t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2)

	drop table #tmpMiscPO_Detail

------------------PartPO_Detail_TPEAP----------------------
	select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty) 
	into #tmpPartPO_Detail_TPEAP
	from dbo.PartPO_Detail a
	inner join dbo.SciTrade_To_Pms_MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
	group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price

	update t set t.TPEPOID = s.TPEPOID,
					t.APDATE = s.APDATE,
					t.VoucherID = s.VoucherID,
					t.Price = s.Price,
					t.Qty = s.Qty
	from dbo.PartPO_Detail_TPEAP t
	inner join #tmpPartPO_Detail_TPEAP s on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2

	insert into dbo.PartPO_Detail_TPEAP(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty)
			select s.ID,s.Seq1,s.Seq2,s.TPEPOID,s.APDATE,s.VoucherID,s.Price,s.Qty
			from #tmpPartPO_Detail_TPEAP s
			where not exists(select 1 from dbo.PartPO_Detail_TPEAP t where t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2)
	
	drop table #tmpPartPO_Detail_TPEAP

	--------------Partunit-------------------------------

	update t set t.addname=s.addname,
				 t.adddate=s.adddate,
				 t.editname=s.editname,
				 t.editdate=s.editdate
	from dbo.MMSUnit t
	inner join SciTrade_To_Pms_MmsUnit s on t.id=s.id

	insert into dbo.MMSUnit([ID]
												,[AddName]
												,[AddDate]
												,[EditName]
												,[EditDate])
				select  s.[ID]     
						,s.[AddName]
						,s.[AddDate]
						,s.[EditName]
						,s.[EditDate]
				from SciTrade_To_Pms_MmsUnit s
				where not exists(select 1 from dbo.MMSUnit t where t.id=s.id)

	  -----------MachineMasterGroup------------------------
	  update t set t.ID				 = s.ID,				
				t.Description		 = s.Description,		
				t.DescriptionCH	 = s.DescriptionCH	,
				t.Junk			 = s.Junk	,		
				t.AddName			 = s.AddName,			
				t.AddDate			 = s.AddDate,			
				t.EditName		 = s.EditName,		
				t.EditDate		 = s.EditDate	
	  from dbo.MachineMasterGroup t
	  inner join dbo.SciTrade_To_Pms_MachineMasterGroup s on t.id=s.id

	  insert into dbo.MachineMasterGroup(
						ID			 ,
						Description	 ,
						DescriptionCH,	
						Junk			 ,
						AddName		 ,
						AddDate		 ,
						EditName		 ,
						EditDate		)
					select s.ID			 ,
					   s.Description	 ,
					   s.DescriptionCH,	
					   s.Junk			 ,
					   s.AddName		 ,
					   s.AddDate		 ,
					   s.EditName		 ,
					   s.EditDate	
					from dbo.SciTrade_To_Pms_MachineMasterGroup s
					where not exists(select 1 from dbo.MachineMasterGroup t where t.id=s.id)

	  -----------MachineGroup------------------------
		update t set t.ID= s.ID,
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
					 t.MasterGroupID = s.MasterGroupID,
					 t.IsBD= s.IsBD,
					 t.IsSew= s.IsSew,
					 t.IsDP= s.IsDP
		from dbo.MachineGroup t
		inner join dbo.SciTrade_To_Pms_MachineGroup s on t.id=s.id AND t.MasterGroupID = s.MasterGroupID

		insert into dbo.MachineGroup(ID
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
				,IsBD
				,IsSew
				,IsDP)
				select s.ID,
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
				s.MasterGroupID,
				s.IsBD,
				s.IsSew,
				s.IsDP
				from dbo.SciTrade_To_Pms_MachineGroup s
				where not exists(select 1 from dbo.MachineGroup t where t.id=s.id AND t.MasterGroupID = s.MasterGroupID)
		
		-----------RepairType------------------------
		update t set t.ID= s.ID,
					 t.Name		   = s.Name			,
					 t.Description  = s.Description	,
					 t.Junk		   = s.Junk			,
					 t.AddDate	   = s.AddDate		,
					 t.AddName	   = s.AddName		,
					 t.EditDate	   = s.EditDate		,
					 t.EditName	   = s.EditName
		from dbo.RepairType t
		inner join dbo.SciTrade_To_Pms_RepairType s on t.id=s.id

		insert into dbo.RepairType(Id
					  ,Name
					  ,Description
					  ,Junk
					  ,AddDate
					  ,AddName
					  ,EditDate
					  ,EditName)
				select s.Id
					  ,s.Name
					  ,s.Description
					  ,s.Junk
					  ,s.AddDate
					  ,s.AddName
					  ,s.EditDate
					  ,s.EditName
				from dbo.SciTrade_To_Pms_RepairType s
				where not exists(select 1 from dbo.RepairType t where t.id=s.id)

	 -----------TPEMachine------------------------
	 select * into #tmpTPEMachine
	 from dbo.SciTrade_To_Pms_Part 
	 where type = 'M'

	 update t set	t.MasterGroupID		    = s.MasterGroupID
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
	 from dbo.TPEMachine t
	 inner join #tmpTPEMachine s on t.ID = s.Refno 

	 insert into dbo.TPEMachine(ID,MasterGroupID,MachineGroupID,Model,MachineBrandID,Description
			,DescriptionDetail,Origin,Picture1,Picture2,Junk
			,AddName,AddDate,EditName,EditDate)
			select s.Refno 			, s.MasterGroupID  , s.MachineGroupID , s.Model 		, s.MachineBrandID 	, s.Description
			 , s.DescriptionDetail 		, s.Origin 	, '' 		, '' 		, s.Junk
			 , s.AddName 		, s.AddDate  	, s.EditName  	, s.EditDate
			from #tmpTPEMachine s
			where not exists(select 1 from dbo.TPEMachine t where t.ID = s.Refno )

	drop table #tmpTPEMachine
	
	 -----------PartPrice_History ------------------------
	select *
	into	#tmpPartPrice_History
	from dbo.SciTrade_To_Pms_TradeHIS_MMS WITH (NOLOCK) 
	where TableName = 'Part' and HisType = 'ControlParts' 

	 update t set t.PartID 		= s.RefNo  
				,t.HisType		= s.HisType
				,t.OldValue		= s.OldValue
				,t.NewValue		= s.NewValue
				,t.Remark		= s.Remark
				,t.AddName		= s.AddName
				,t.AddDate		= s.AddDate
	 from dbo.PartPrice_History t
	 inner join #tmpPartPrice_History s on t.TradeHisMMSUkey = s.Ukey  

	 insert into dbo.PartPrice_History(TradeHisMMSUkey,  PartID,  HisType,  OldValue,  NewValue
		   ,Remark,  AddName,  AddDate)
		   select s.Ukey    , s.RefNo , s.HisType 	, s.OldValue , s.NewValue 		
			, s.Remark 	 , s.AddName , s.AddDate 
		   from #tmpPartPrice_History s
		   where not exists(select 1 from dbo.PartPrice_History t where t.TradeHisMMSUkey = s.Ukey  )

	drop table #tmpPartPrice_History

	------------MachinePending------------------
	update t set t.TPEComplete = s.TPEComplete
	from dbo.MachinePending t
	inner join dbo.SciTrade_To_Pms_MachinePending s on t.id=s.id
	where t.status = 'Confirmed'

	------------MachinePending_Detail------------------
	declare @Tdebit table(id varchar(13),MachineID varchar(16),TPEReject int,TPEApvDate datetime)

	select	md.id
			,md.seq
			,md.TPEApvDate
			,md.TPEApvName
			,md.TPEReject
			,m.status
	into #tmpMachinePending_Detail
	from dbo.SciTrade_To_Pms_MachinePending_Detail md
	inner join dbo.MachinePending m on m.id = md.id

	update t 
	set t.TPEReject = s.TPEReject
	,t.TPEApvName = s.TPEApvName 
	,t.TPEApvDate = s.TPEApvDate 
	from dbo.MachinePending_Detail t
	inner join #tmpMachinePending_Detail s on t.id=s.id and t.seq = s.seq
	where s.status = 'Confirmed' and s.TPEApvDate is not null

	insert into @Tdebit(id, MachineID, TPEReject,TPEApvDate)
	select t.ID, t.MachineID, s.TPEReject,s.TPEApvDate
	from dbo.MachinePending_Detail t
	inner join #tmpMachinePending_Detail s on t.id=s.id and t.seq = s.seq
	where s.status = 'Confirmed' and s.TPEApvDate is not null

	drop table #tmpMachinePending_Detail

	update dbo.Machine set Status = 'Pending',EstFinishRepairDate =null 
	where ID in (select MachineID from @Tdebit where TPEReject = 0)  
	and status != 'Disposed'

	update m set m.Status = 'Good'
	from dbo.Machine m
    where exists(
        select 1
        from @Tdebit t
        where t.MachineID = m.ID
        and TPEReject = 1
        and (
            select top 1 ml.EditDate
            from MachineLend ml
            inner join MachineLend_Detail mld on mld.ID = ml.ID
            where ml.Status <> 'New'
            and mld.MachineID = t.MachineID
            order by EditDate desc
        ) < t.TPEApvDate
    )

	update md set Results = 'Reject'
	from dbo.MachinePending_Detail md
	where exists(select 1 from @Tdebit t where t.ID = md.ID and t.MachineID = md.MachineID and TPEReject = 1)

	--update exp_Part��X��ƪ�SendToTPE���
	UPDATE a
	SET TranstoTPE = CONVERT(date, GETDATE())
	FROM dbo.PartPO a
	LEFT JOIN SciPms_To_Trade_PartPO b ON a.ID = b.ID
	WHERE a.TranstoTPE  IS NULL
	
	UPDATE a
	SET TranstoTPE = CONVERT(date, GETDATE())
	FROM dbo.RepairPO a
	INNER JOIN SciPms_To_Trade_RepairPO b ON a.ID = b.ID
	WHERE a.TranstoTPE  IS NULL
	
	UPDATE a
	SET TranstoTPE = CONVERT(date, GETDATE())
	FROM dbo.MiscPO a
	LEFT JOIN SciPms_To_Trade_MiscPO b ON a.ID = b.ID
	WHERE a.TranstoTPE  IS NULL

	Update dbo.MachineReturn
	Set TranstoTPE = CONVERT(date, GETDATE())
	Where  TranstoTPE  is null

	update s set
	SendToTPE=getdate()
	from dbo.MachinePending s
	inner join SciPms_To_Trade_MachinePending b on b.id = s.ID
	WHERE s.SendToTPE IS NULL

	---------------MMSProject--------------
	update t set t.Name             = s.Name
				,t.Description      = s.Description
				,t.Junk             = s.Junk
				,t.IsDefault        = s.IsDefault
				,t.SCIProject       = s.SCIProject
				,t.LocalProject     = t.LocalProject
				,t.AddName          = s.AddName
				,t.AddDate          = s.AddDate
				,t.EditName         = s.EditName
				,t.EditDate         = s.EditDate
	from dbo.MMSProject t with(nolock)
	inner join dbo.SciTrade_To_Pms_MMSProject s with(nolock) on t.id = s.id
	where t.LocalProject = 0

	insert into dbo.MMSProject(ID,
							   Name,
							   Description,
							   Junk,
							   IsDefault,
							   SCIProject,
							   AddName,
							   AddDate,
							   EditName,
							   EditDate)
						select s.ID,
							   s.Name,
							   s.Description,
							   s.Junk,
							   s.IsDefault,
							   s.SCIProject,
							   s.AddName,
							   s.AddDate,
							   s.EditName,
							   s.EditDate
						from dbo.SciTrade_To_Pms_MMSProject s with(nolock)
						where not exists(select 1 from dbo.MMSProject t with(nolock) where t.id = s.id and t.LocalProject = 0)

	delete t
	from dbo.MMSProject t
	where not exists(select 1 from dbo.SciTrade_To_Pms_MMSProject s with(nolock) where t.id=s.id) and t.LocalProject = 0

	END