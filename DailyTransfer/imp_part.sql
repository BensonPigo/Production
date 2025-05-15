
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
    SELECT
      ISNULL([PartID], ''),
      ISNULL([OldFormula], 0),
      ISNULL([NewFormula], 0),
      ISNULL([AddName], ''),
      [AddDate]
    FROM #Formula_Change_Table

;
DROP TABLE #Formula_Change_Table
;
	--Step.2. Merge
update t set t.Description = isnull( s.Description         ,'')
			 , t.Partno= isnull( s.Partno                  ,'')
			 , t.UseUnit= isnull( s.UnitID                 ,'')
			 , t.PoUnit= isnull( s.PoUnitID                ,'')
			 , t.Price= isnull( s.Price                    ,0)
			 , t.PurchaseBatchQty= isnull( s.BatchQty      ,0)
			 , t.Junk= isnull( s.Junk                      ,0)
			 , t.SuppID= isnull( s.SuppID                  ,'')
			 , t.CurrencyID= isnull( s.CurrencyID          ,'')
			 , t.Formula= isnull( s.Formula                ,0)
			 , t.Fix= isnull( s.Fix                        ,0)
			 , t.AddName= isnull( s.AddName                ,'')
			 , t.AddDate=  s.AddDate   
			 , t.Lock = isnull( s.Lock                     ,0)
			 , t.MasterGroupID = isnull( s.MasterGroupID   ,'')
			 , t.MachineGroupID = isnull( s.MachineGroupID ,'')
			 , t.MachineBrandID = isnull( s.MachineBrandID ,'')
			 , t.Needle = isnull( s.Needle                 ,0)
			 , t.ControlPart = isnull( s.ControlParts      ,0)
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
    SELECT
        s.refno,
        ISNULL(s.Description, ''),
        ISNULL(s.Partno, ''),
        ISNULL(s.MasterGroupID, ''),
        ISNULL(s.MachineGroupID, ''),
        ISNULL(s.MachineBrandID, ''),
        ISNULL(s.UnitID, ''),
        ISNULL(s.POUnitID, ''),
        ISNULL(s.Price, 0),
        ISNULL(s.BatchQty, 0),
        ISNULL(s.Junk, 0),
        'T',
        ISNULL(s.SuppID, ''),
        ISNULL(s.CurrencyID, ''),
        ISNULL(s.Formula, 0),
        ISNULL(s.Fix, 0),
        ISNULL(s.AddName, ''),
        s.AddDate,
        ISNULL(s.EditName, ''),
        s.EditDate,
        ISNULL(s.Lock, 0),
        ISNULL(s.Needle, 0),
        ISNULL(s.ControlParts, 0),
        ISNULL(CONVERT(int, s.MOQ), 0),
        ISNULL(s.Paper, 0),
        ISNULL(s.DescriptionDetail, '')
    FROM dbo.SciTrade_To_Pms_Part s WITH (NOLOCK)
    WHERE s.type = 'P' AND NOT EXISTS (SELECT 1 FROM dbo.Part t WHERE t.id = s.Refno)


	---------- Misc, type='O'---------------------
	update t set t.Model= isnull( s.Model,                ''),
				 t.MiscBrandID= isnull( s.MachineBrandID, ''),
				 t.Description= isnull( s.Description,    ''),
				 t.UnitID= isnull( s.PoUnitID,            ''),
				 t.CurrencyID= isnull( s.CurrencyID,      ''),
				 t.Price= isnull( s.Price,                0),
				 t.SuppID= isnull( iif(t.PurchaseFrom = 'T', s.SuppID, t.SuppID),''), --PurchaseFrom= isnull('T'時才需要更新
				 t.IsMachine= isnull( s.IsMachine,                  0),
				 t.IsAsset= isnull( s.IsAsset,                      0),
				 t.Remark= isnull( s.Remark,                        ''),
				 t.InspLeadTime= isnull( s.Leadtime,                0),
				 t.AccountID= isnull( s.AccountID,                  ''),
				 t.Junk= isnull( s.Junk,                            0),
				 t.AddName= isnull( s.AddName,                      ''),
				 t.AddDate=  s.AddDate,
				 t.EditName= isnull( s.EditName,                    ''),
				 t.EditDate=  s.EditDate,
				 t.DescriptionDetail = isnull( s.DescriptionDetail, ''),
				 t.PurchaseBatchQty = isnull( s.BatchQty,           0),
				 t.AssetTypeID = isnull( s.AssetTypeID,             ''),
				 t.Improvement = isnull( s.Improvement,             0)
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
				select	isnull(s.refno,             ''),
						isnull(s.Model,             ''),
						isnull(s.MachineBrandID,    ''),
						isnull(s.Description,       ''),
						isnull(s.PoUnitID,          ''),
						isnull(s.CurrencyID,        ''),
						isnull(s.Price,             0),
						isnull(s.SuppID,            ''),
						isnull('T',                 ''),
						isnull(s.IsMachine,         0),
						isnull(s.IsAsset,           0),
						isnull(s.Remark,            ''),
						isnull(s.Leadtime,          0),
						isnull(s.AccountID,         ''),
						isnull(s.Junk,              0),
						isnull(s.AddName,           ''),
						s.AddDate,
						isnull(s.EditName,          ''),
						s.EditDate,
						isnull(s.DescriptionDetail, ''),
						isnull(s.BatchQty,          0),
						isnull(s.AssetTypeID,       ''),
						isnull(s.Improvement,       0)
				from dbo.SciTrade_To_Pms_Part s
				where s.type='O'  and not exists(select 1 from dbo.Misc t where t.id = s.Refno)

				
    ---------- MiscOther, type='R'---------------------
    update t set t.Model= isnull( s.Model,                         ''),
                 t.MtlTypeID= isnull(s.MtlTypeID,                  ''),
                 t.Description= isnull( s.Description,             ''),
                 t.UnitID= isnull( s.PoUnitID,                     ''),
                 t.CurrencyID= isnull( s.CurrencyID,               ''),
                 t.Price= isnull( s.Price,                         0),
                 t.IsMachine= isnull( s.IsMachine,                 0),
                 t.IsAsset= isnull( s.IsAsset,                     0),
                 t.Remark= isnull( s.Remark,                       ''),
                 t.Junk= isnull( s.Junk,                           0),
                 t.AddName= isnull( s.AddName,                     ''),
                 t.AddDate=  s.AddDate,
                 t.EditName= isnull( s.EditName,                   ''),
                 t.EditDate=  s.EditDate,
                 t.DescriptionDetail = isnull( s.DescriptionDetail,'')
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
                select  isnull(s.refno,            ''),
                        isnull(s.Model,            ''),
                        isnull(s.BrandID,          ''),
                        isnull(s.MtlTypeID,        ''),
                        isnull(s.Description,      ''),
                        isnull(s.PoUnitID,         ''),
                        isnull(s.CurrencyID,       ''),
                        isnull(s.Price,            0),
                        isnull(s.SuppID,           ''),
                        isnull('T',                ''),
                        isnull(s.IsMachine,        0),
                        isnull(s.IsAsset,          0),
                        isnull(s.Remark,           ''),
                        isnull(s.Junk,             0),
                        isnull(s.AddName,          ''),
                        s.AddDate,
                        isnull(s.EditName,         ''),
                        s.EditDate,
                        isnull(s.DescriptionDetail,'')
                from dbo.SciTrade_To_Pms_Part s
                where s.type='R' and not exists(select 1 from dbo.MiscOther t where t.id = s.Refno and t.BrandID = s.BrandID and t.SuppID = s.SuppID)


	-----------------PartQuot , type='P'-----------------------------
	select distinct a.* 
	into #tmpPartQuot
	from SciTrade_To_Pms_MmsQuot a WITH (NOLOCK) 
	inner join  SciTrade_To_Pms_Part b WITH (NOLOCK) on a.PartUkey=b.Ukey where a.type='P'

	update t set
		t.purchaseFrom='T',
		t.CurrencyID= isnull(s.CurrencyID,''),
		t.Price= isnull(s.Price,          0),
		t.AddName= isnull(s.AddName,      ''),
		t.AddDate= s.AddDate,
		t.EditName= isnull(s.EditName,    ''),
		t.EditDate= s.EditDate,
		T.ldefault = isnull( S.IsDefault, 0)
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
										select		isnull(s.Refno,''),
													'T',
													isnull(s.suppid,    ''),
													isnull(s.CurrencyID,''),
													isnull(s.Price,     0),
													isnull(s.AddName,   ''),
													s.AddDate,
													isnull(s.EditName,  ''),
													s.EditDate,
													isnull(s.mmsBrandID,''),
													isnull(S.IsDefault,0)
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

	update t set t.Name= isnull( 	      s.Name         ,'')
				,t.Junk= isnull( 	      s.Junk         ,0)
				,t.AddName= isnull( 	      s.AddName  ,'')
				,t.AddDate=       s.AddDate
				,t.EditName= isnull( 	      s.EditName ,'')
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
						select	 isnull(s.ID,'')
								,isnull(s.Name ,'')
								,isnull(s.Junk ,0)
								,isnull(s.AddName ,'')
								,s.AddDate 
								,isnull(s.EditName ,'')
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

	update t set t.AddName= 	      isnull(s.AddName   ,'')
				,t.AddDate= 	      s.AddDate   
				,t.EditName= 	      isnull(s.EditName   ,'')
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
					select	isnull(s.id  ,      ''),
							isnull(s.Name  ,    ''),
							isnull(s.Junk   ,   0),
							isnull(s.AddName ,  ''),
							s.AddDate  , 
							isnull(s.EditName  , ''),
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

	update t set t.Name= 	      isnull(s.Name   ,'')
				,t.Junk= 	      isnull(s.Junk   ,0)
				,t.AddName= 	      isnull(s.AddName   ,'')
				,t.AddDate= 	      s.AddDate   
				,t.EditName= 	      isnull(s.EditName   ,'')
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
		  select isnull(s.ID   ,     ''),
				 isnull(s.Name  ,    ''),
				 isnull(s.Junk   ,   0),
				 isnull(s.AddName ,  ''),
				 s.AddDate  , 
				 isnull(s.EditName  , ''),
				 s.EditDate 
		  from #tmpTrade_To_PmsMachineBrand s
		  where not exists(select 1 from dbo.MachineBrand t where t.id=s.id)

	drop table #tmpTrade_To_PmsMachineBrand

	----------------RepairPO-------------------------
	update a
		set a.Status =isnull( case when b.Status is null and a.Status = 'Complete' then 'Confirmed'
						when b.Status in ('Complete', 'Junk') then b.Status
				   else a.Status
				   end ,'')
	from RepairPO a
	inner join SciTrade_To_Pms_RepairReq b on a.ID = b.ID
	
	----------------RepairPO_Detail-------------------------

	update dbo.RepairPO_Detail
	set ShipQty = isnull( b.ShipQty ,  0),
		ShipFoc = isnull( b.ShipFoc ,  0),
		EstCost = isnull( b.EstCost ,  0),
		ActCost = isnull( b.ActCost ,  0),
		NewCost = isnull( b.NewCost ,  0),
		ETA =  b.ETA ,
		Cancel = isnull( b.Cancel ,    0),
		Complete = isnull( b.Complete ,0),
		TPEPOID = isnull( b.TradePOID ,''),
		ResponsibleFTY  = isnull( b.ResponsibleFTY ,'')
	from dbo.RepairPO_Detail a
	inner join SciTrade_To_Pms_RepairReq_Detail b on a.id=b.ID and a.Seq2=b.Seq2

	----------------PartPO2-------------------------

	update dbo.PartPO_Detail
	set 
		Junk = isnull( b.Cancel           ,0)
		,TPEPOID = isnull( b.MmsPoID      ,'')
		,FinalBrand = isnull( b.FinalBrand,'')
	from dbo.PartPO_Detail a
	inner join SciTrade_To_Pms_MmsReq_Detail b on a.id=b.ID and a.SEQ2=b.Seq2


	----------------整理所有工廠--------------------
	declare @Sayfty table(id varchar(10)) --工廠代碼
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
	    A.SEQ1= isnull(b.NewSeq1,''),
	    A.SuppDelivery= b.SuppDelivery,
	    A.EstETA= b.EstETA,
	    A.Complete =  ISNULL(b.Complete,0),
	    A.TPEQty= isnull(B.Qty,0),
	    A.Foc= isnull(B.Foc,0),
	    A.ShipQty= isnull(B.ShipQty,0),
	    A.ShipFoc= isnull(B.ShipFoc,0),
	    A.ShipETA= B.ShipETA,
	    A.TPECurrencyID = isnull( B.CurrencyID,''),
	    A.TPEPrice = isnull( B.Price,0)
	FROM dbo.PartPO_Detail A
	INNER JOIN #tmpMmsPO_Detail B  on	A.ID = B.ID and 
										A.Seq1 = B.Seq1 and 
										A.Seq2 = B.Seq2 and 
										A.PartID = B.PartID and
										A.PartReqID = B.PartReqID

	drop table #tmpMmsPO_Detail

	UPDATE dbo.MiscPO_Detail
	SET
        TPEPOID = isnull( B.id,''),
	    SEQ1= isnull(b.Seq1,''),
	    SuppDelivery= b.SuppDelivery,
	    EstETA= b.EstETA,
	    TPEQty = isnull( B.Qty,0),
	    Foc = isnull( B.Foc,0),
	    ShipQty = isnull( B.ShipQty,0),
	    ShipFoc = isnull( B.ShipFoc,0),
	    ShipETA =  B.ShipETA,
	    TPECurrencyID = isnull( c.CurrencyID,''),
	    TPEPrice = isnull( b.Price,0)
	FROM dbo.MiscPO_Detail A
	INNER JOIN dbo.SciTrade_To_Pms_MmsPO_Detail B  on  a.MiscID=b.Refno 
													and  a.SEQ2=b.Seq2 
													and a.id = b.MmsReqID
	INNER JOIN  dbo.SciTrade_To_Pms_MmsPO C ON B.ID=C.ID
	WHERE C.Type ='O'
	and C.FactoryID in (select id from @Sayfty)

	UPDATE a
	SET Junk = isnull( b.Cancel,0)
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

	update t set	t.CDate =  s.CDate ,
					t.PurchaseFrom = 'T' ,
					t.FactoryID = isnull( s.FactoryID ,''),
					t.MDivisionID = isnull( s.MDivisionID,''),
					t.CurrencyID = isnull( s.CurrencyID ,''),
					t.Status= IIF(s.Junk = 1,'Junked',IIF(s.ApvName != '','Approved','New')),
					t.Handle = isnull( s.Handle ,''),
					t.Amount = isnull( s.Amount ,0),
					t.Vatrate = isnull( s.Vatrate ,0),
					t.Vat = isnull( s.Vat ,0),
					t.Remark = isnull( s.Remark ,''),
					t.Approve = isnull( s.ApvName ,''),
					t.ApproveDate =  s.ApvDate ,
					t.AddName = isnull( s.AddName ,''),
					t.AddDate = s.AddDate ,
					t.EditName = isnull( s.EditName ,''),
					t.EditDate = s.EditDate 
	from dbo.MachinePO t
	inner join #tmpTrade_To_PmsMachinePO s on t.id=s.id
	and not (s.junk = 0 and s.ApvDate is null)

	insert into dbo.MachinePO(ID ,	  CDate ,PurchaseFrom ,  FactoryID ,   CurrencyID ,   Handle,    LocalSuppID ,  Amount ,   Vatrate ,   Vat ,        Remark ,        Approve ,        ApproveDate ,        AddName ,        AddDate ,        EditName ,        EditDate
		 ,MDivisionID,Status)
	SELECT
        isnull(s.id,''),
        s.cdate,
        'T',
        isnull(s.factoryid,''),
        isnull(s.currencyid,''),
        isnull(s.handle,   ''),
        isnull(s.suppid,   ''),
        isnull(s.amount,   0),
        isnull(s.vatrate,  0),
        isnull(vat,        0),
        isnull(s.remark,   ''),
        isnull(s.apvname,  ''),
        s.apvdate,
        isnull(s.addname,''),
        s.adddate,
        isnull(s.editname,''),
        s.editdate 
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
		    t.seq1= isnull(s.seq1,''),
		    t.SuppDelivery= s.SuppDelivery,
		    t.EstETA= s.EstETA,
		    t.Complete = isnull(s.Complete,0),
		    t.TPEQty= isnull(s.Qty,0),
		    t.Foc= isnull(s.Foc,0),
		    t.ShipQty= isnull(s.ShipQty,0),
		    t.ShipFoc= isnull(s.ShipFoc,0),
		    t.ShipETA= s.ShipETA,
		    t.TPECurrencyID = isnull( s1.CurrencyID,''),
		    t.TPEPrice = isnull( s.Price,0)
		from  dbo.PartPO_Detail as  t
		inner join dbo.SciTrade_To_Pms_MmsPO_Detail s on t.id=s.MmsReqID and t.seq2=s.seq2 and s.Junk=0 AND s.ID=t.TPEPOID
		inner join dbo.SciTrade_To_Pms_MmsPO s1 on s1.ID=s.ID
		inner join dbo.PartPO a on t.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where 1=1
		
		update t
		set
            t.TpePOID = isnull( s.id,''),
		    t.seq1= isnull(s.seq1,''),
		    t.SuppDelivery= s.SuppDelivery,
		    t.EstETA= s.EstETA,
		    t.Complete = isnull(s.Complete,0),
		    t.TPEQty= isnull(s.Qty,0),
		    t.Foc= isnull(s.Foc,0),
		    t.ShipQty= isnull(s.ShipQty,0),
		    t.ShipFoc= isnull(s.ShipFoc,0),
		    t.ShipETA= s.ShipETA,
		    t.TPECurrencyID = isnull( a.CurrencyID,''),
		    t.TPEPrice = isnull( s.Price,0)
		from  dbo.MiscPO_Detail as  t
		inner join dbo.SciTrade_To_Pms_MmsPO_Detail s on t.id=s.MmsReqID  and t.seq2=s.seq2
		inner join dbo.SciTrade_To_Pms_MmsPO a on s.id=a.ID
		left join Production.dbo.scifty b on a.FactoryID=b.ID
		where a.Type='O'
		
------------------MachinePO_Detail Type='M'----------------------
	update t set t.MachineGroupID= isnull( s.MachineGroupID,''),
				t.MasterGroupID= isnull( s.MasterGroupID,   ''),
				t.MachineBrandID= isnull( s.MachineBrandID, ''),
				t.Model= isnull( s.Model,                   ''),
				t.Description= isnull( s.Description,       ''),
				t.Qty= isnull( s.Qty,                       0),
				t.FOC= isnull( s.FOC,                       0),
				t.Price= isnull( s.Price,				    0),
				t.Remark= isnull( s.Remark,                 ''),
				t.MachineReqID= isnull( s.MmsReqID,         ''),
				t.Junk= isnull( s.Junk,                     0),
				t.RefNo = ISNULL(s.RefNo,''),
				t.DescriptionDetail = isnull( s.DescriptionDetail,''),
				t.UnitID = isnull( s.UnitID,''),
				t.Delivery = s.Delivery,
				t.SuppEstETA = s.SuppEstETA,
				t.Complete = isnull( s.Complete,0),
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
    SELECT isnull(s.id,            ''),
           isnull(s.seq1,          ''),
           isnull(s.seq2,          ''),
           isnull(s.mastergroupid, ''),
           isnull(s.machinegroupid,''),
           isnull(s.machinebrandid,''),
           isnull(s.model,         ''),
           isnull(s.description,   ''),
           isnull(s.qty,           0),
           isnull(s.foc,           0),
           isnull(s.price,         0),
           isnull(s.remark,        ''),
           isnull(s.mmsreqid,      ''),
           isnull(s.junk,          0),
           Isnull(s.refno, ''),
           isnull(s.descriptiondetail,''),
           isnull(s.unitid,          ''),
           s.delivery,
           s.suppesteta,
           isnull(s.complete,        0),
           isnull(s.shipqty,         0),
           isnull(s.shipfoc,         0),
           s.shipeta
	from dbo.SciTrade_To_Pms_MachinePO_Detail s
	inner join dbo.SciTrade_To_Pms_MmsPO sM WITH (NOLOCK) on sM.id = s.ID
	where not exists(select 1 from dbo.MachinePO_Detail t where t.id=s.id and t.seq1=s.seq1 and t.seq2=s.seq2)
	and not (sM.junk = 0 and sM.ApvDate is null)

	delete dbo.MachinePO_Detail where ID in(select ID from #deleteMachinePOID)
	drop table #deleteMachinePOID
	
		-- 刪除不存在 Trade 轉來的資料 , 這次有表頭, 沒表身的部分
	delete MachinePO_Detail
	where exists(select 1 from #tmpTrade_To_PmsMachinePO t where ID = MachinePO_Detail.ID)
	and not exists(select 1 from #tmpTrade_To_PmsMachinePO t inner join SciTrade_To_Pms_MachinePO_Detail d on t.id = d.id where t.ID = MachinePO_Detail.ID and d.seq1 = MachinePO_Detail.seq1 and d.seq2 = MachinePO_Detail.seq2)
	drop table #tmpTrade_To_PmsMachinePO
	
------------------MachinePO_Detail Type='R'----------------------
	update t set
         [Cdate]        = sM.[Cdate]
        ,[FactoryID]	= isnull( sM.[FactoryID] ,'')
        ,[MDivisionID]	= isnull( b.MDivisionID  ,'')
        ,[CurrencyID]	= isnull( sM.[CurrencyID],'')
        ,[Amount]		= isnull( sM.[Amount]    ,0)
        ,[Vatrate]		= isnull( sM.[Vatrate]   ,0)
        ,[Vat]			= isnull( sM.[Vat]       ,0)
        ,[Remark]		= isnull( sM.[Remark]    ,'')
        ,[ApvName]		= isnull( sM.[ApvName]   ,'')
        ,[ApvDate]		= sM.[ApvDate]
		,[Junk]			= isnull( sM.[Junk]      ,0)
        ,[AddName]		= isnull( sM.[AddName]   ,'')
        ,[AddDate]		= sM.[AddDate]
        ,[EditName]		= isnull( sM.[EditName],'')
        ,[EditDate]		= sM.[EditDate]
		,Handle		= isnull( sM.Handle,'')
	from MiscOtherPO t
	inner join dbo.SciTrade_To_Pms_MmsPO sM WITH (NOLOCK) on sM.id = t.ID
	left join Production.dbo.scifty b on sM.FactoryID = b.ID
	where sM.type = 'R'

	update t set 
         [MiscOtherID] = isnull( s.RefNo       ,'')
        ,[BrandID]	   = isnull( sM.[BrandID]  ,'')
        ,[SuppID]	   = isnull( sM.[SuppID]   ,'')
        ,[UnitID]	   = isnull( s.[UnitID]    ,'')
		,Qty		   = isnull( s.Qty         ,0)
        ,[TPEPrice]	   = isnull( s.Price       ,0)
        ,[TPEQty]	   = isnull( s.ShipQty     ,0)
        ,[TPEFoc]	   = isnull( s.Foc         ,0)
		,MachineReqID  = isnull( s.MmsReqID    ,'')
		,Junk		   = isnull( s.Junk        ,0)
		,Delivery	   =  s.Delivery
		,ShipQty		   = isnull( s.ShipQty        ,0)
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
           ,[EditDate]
		   ,Handle)
	select
		 isnull(sM.[ID]       ,'')
        ,sM.[Cdate]
        ,isnull(sM.[FactoryID],'')
        ,isnull(b.[MDivisionID],'')
        ,'T'
        ,isnull(sM.[CurrencyID],'')
        ,isnull(sM.[Amount]    ,0)
        ,isnull(sM.[Vatrate]   ,0)
        ,isnull(sM.[Vat]       ,0)
        ,isnull(sM.[Remark]    ,'')
		,isnull(sM.[ApvName]   ,'')
        ,sM.[ApvDate]
		,isnull(sM.[Junk]      ,0)
        ,isnull(sM.[AddName]   ,'')
        ,sM.[AddDate]
        ,isnull(sM.[EditName],'')
        ,sM.[EditDate]		
        ,isnull(sM.Handle,'')
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
			,Junk
			,Delivery
			,ShipQty)
	select
		 isnull(s.[ID]      ,'')
		,isnull(s.[Seq1]    ,'')
		,isnull(s.[Seq2]    ,'')
		,isnull(s.RefNo     ,'')
		,isnull(sM.[BrandID],'')
		,isnull(sM.[SuppID] ,'')
		,isnull(s.[UnitID]  ,'')
		,isnull(s.[Qty]     ,0)
		,isnull(s.Price     ,0)
		,isnull(s.ShipQty   ,0)
		,isnull(s.Foc       ,0)
		,isnull(s.MmsReqID  ,'')
		,isnull(s.Junk      ,0)
		,s.Delivery
		,isnull( s.ShipQty        ,0)
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
					t.VoucherID = isnull( s.VoucherID,''),
					t.Price = isnull( s.Price,        0),
					t.Qty = isnull( s.Qty,            0)
	from dbo.MachinePO_Detail_TPEAP t
	inner join #tmpMachinePO_Detail_TPEAP s on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2 and t.TPEPOID = s.TPEPOID and t.ExportID = s.ExportID

	insert into dbo.MachinePO_Detail_TPEAP(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty,ExportID)
		SELECT
            isnull(s.id,       ''),
            isnull(s.seq1,     ''),
            isnull(s.seq2,     ''),
            isnull(s.tpepoid,  ''),
            s.apdate,
            isnull(s.voucherid,''),
            isnull(s.price,    0),
            isnull(s.qty,      0),
            isnull(s.exportid, '')
		from #tmpMachinePO_Detail_TPEAP s
		where not exists(select 1 from dbo.MachinePO_Detail_TPEAP t where t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2 and t.TPEPOID = s.TPEPOID and t.ExportID = s.ExportID)

	drop table #tmpMachinePO_Detail_TPEAP

	
------------------MachinePO_Detail_TPETTbefore----------------------
INSERT INTO [dbo].MachinePO_Detail_TPETTbefore([ID],[APDate],[VoucherID],TPEPOID)
SELECT isnull(a.[ID],''),
       a.[APDate],
       isnull(a.[VoucherID],''),
       isnull(a.[POID] ,'')
from SciTrade_To_Pms_MmsTTbefore a
left join MachinePO_Detail_TPETTbefore b on a.ID = b.ID and a.[APDate] = b.[APDate] and a.[VoucherID] = b.[VoucherID] and a.[POID] = b.TPEPOID
where b.ID is null

------------------MachinePO_Detail_TPESurcharge----------------------

INSERT INTO [dbo].[MachinePO_Detail_TPESurcharge]([ID],[APDate],[VoucherID],TPEPOID,[Seq1],[Seq2])
SELECT isnull(a.[ID],''),
       a.[APDate],
       isnull(a.[VoucherID],''),
       isnull(a.[POID],     ''),
       isnull(a.[Seq1],     ''),
       isnull(a.[Seq2],     '')
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

	update t set t.TPEPOID = isnull( s.TPEPOID,''),
					t.APDATE = s.APDATE,
					t.VoucherID = isnull( s.VoucherID,''),
					t.Price = isnull( s.Price,0),
					t.Qty = isnull( s.Qty,0)
	from dbo.MiscPO_Detail_TPEAP t
	inner join #tmpMiscPO_Detail s on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2

	insert into dbo.MiscPO_Detail_TPEAP(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty)
    SELECT
            isnull(s.id,       ''),
            isnull(s.seq1,     ''),
            isnull(s.seq2,     ''),
            isnull(s.tpepoid,  ''),
            s.apdate,
            isnull(s.voucherid,''),
            isnull(s.price,    0),
            isnull(s.qty,      0)
	from #tmpMiscPO_Detail s
	where not exists(select 1 from dbo.MiscPO_Detail_TPEAP t where t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2)

	drop table #tmpMiscPO_Detail

------------------PartPO_Detail_TPEAP----------------------
	select b.ID,a.Seq1,a.Seq2,[TPEPOID] = b.POID,b.APDATE,b.VoucherID,b.Price, [Qty] = sum(b.Qty) 
	into #tmpPartPO_Detail_TPEAP
	from dbo.PartPO_Detail a
	inner join dbo.SciTrade_To_Pms_MmsAP b on a.ID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
	group by b.ID,a.Seq1,a.Seq2,b.POID,b.APDATE,b.VoucherID,b.Price

	update t set t.TPEPOID = isnull( s.TPEPOID,''),
					t.APDATE =  s.APDATE,
					t.VoucherID = isnull( s.VoucherID,''),
					t.Price = isnull( s.Price,0),
					t.Qty = isnull( s.Qty,0)
	from dbo.PartPO_Detail_TPEAP t
	inner join #tmpPartPO_Detail_TPEAP s on t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2

	insert into dbo.PartPO_Detail_TPEAP(ID,Seq1,Seq2,TPEPOID,APDATE,VoucherID,Price,Qty)
	SELECT
            isnull(s.id,        ''),
            isnull(s.seq1,      ''),
            isnull(s.seq2,      ''),
            isnull(s.tpepoid,   ''),
            s.apdate,
            isnull(s.voucherid, ''),
            isnull(s.price,     0),
            isnull(s.qty,       0)
	from #tmpPartPO_Detail_TPEAP s
	where not exists(select 1 from dbo.PartPO_Detail_TPEAP t where t.ID = s.ID and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2)
	
	drop table #tmpPartPO_Detail_TPEAP

	--------------Partunit-------------------------------

	update t set t.addname=isnull(s.addname,''),
				 t.adddate=s.adddate,
				 t.editname=isnull(s.editname,''),
				 t.editdate=s.editdate
	from dbo.MMSUnit t
	inner join SciTrade_To_Pms_MmsUnit s on t.id=s.id

	insert into dbo.MMSUnit([ID]
												,[AddName]
												,[AddDate]
												,[EditName]
												,[EditDate])
				select   isnull(s.[ID]     ,'')
						,isnull(s.[AddName],'')
						,s.[AddDate]
						,isnull(s.[EditName],'')
						,s.[EditDate]
				from SciTrade_To_Pms_MmsUnit s
				where not exists(select 1 from dbo.MMSUnit t where t.id=s.id)

	  -----------MachineMasterGroup------------------------
	  update t set t.ID				 = isnull( s.ID,				''),
				t.Description		 = isnull( s.Description,		''),
				t.DescriptionCH	 = isnull( s.DescriptionCH	,       ''),
				t.Junk			 = isnull( s.Junk	,		        0),
				t.AddName			 = isnull( s.AddName,			''),
				t.AddDate			 =  s.AddDate,
				t.EditName		 = isnull( s.EditName,		        ''),
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
					select
                       isnull(s.ID			 ,''),
					   isnull(s.Description	 ,''),
					   isnull(s.DescriptionCH,''),
					   isnull(s.Junk		,0),
					   isnull(s.AddName		 ,''),
					   s.AddDate		 ,
					   isnull(s.EditName		,''),
					   s.EditDate	
					from dbo.SciTrade_To_Pms_MachineMasterGroup s
					where not exists(select 1 from dbo.MachineMasterGroup t where t.id=s.id)

	  -----------MachineGroup------------------------
		update t set t.ID= isnull( s.ID,''),
					 t.Description= isnull( s.Description,      ''),
					 t.DescCH= isnull( s.DescriptionCH,         ''),
					 t.Substitute= isnull( s.Substitute,        ''),
					 t.Junk= isnull( s.Junk,                    0),
					 t.Picture1= isnull( s.Picture1,            ''),
					 t.Picture2= isnull( s.Picture2,			''),
					 t.AddName= isnull( s.AddName,              ''),
					 t.AddDate=  s.AddDate,
					 t.EditName= isnull( s.EditName,            ''),
					 t.EditDate=  s.EditDate,
					 t.MasterGroupID = isnull( s.MasterGroupID, ''),
					 t.IsBD= isnull( s.IsBD,                    0),
					 t.IsSew= isnull( s.IsSew,                  0),
					 t.IsDP= isnull( s.IsDP,0)
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
				select
                    isnull(s.ID,            ''),    
				    isnull(s.Description,   ''),
				    isnull(s.DescriptionCH, ''),
				    isnull(s.Substitute,    ''),
				    isnull(s.Junk,          0),
				    isnull(s.Picture1,      ''),
				    isnull(s.Picture2,      ''),
				    isnull(s.AddName,       ''),
				    s.AddDate,
				    isnull(s.EditName,      ''),
				    s.EditDate,
				    isnull(s.MasterGroupID, ''),
				    isnull(s.IsBD,          0),
				    isnull(s.IsSew,         0),
				    isnull(s.IsDP,          0)
				from dbo.SciTrade_To_Pms_MachineGroup s
				where not exists(select 1 from dbo.MachineGroup t where t.id=s.id AND t.MasterGroupID = s.MasterGroupID)
		
		-----------RepairType------------------------
		update t set t.ID= isnull( s.ID,                        ''),
					 t.Name		   = isnull( s.Name			,   ''),
					 t.Description  = isnull( s.Description	,   ''),
					 t.Junk		   = isnull( s.Junk			,   0),
					 t.AddDate	   =  s.AddDate,
					 t.AddName	   = isnull( s.AddName		,   ''),
					 t.EditDate	   =  s.EditDate,
					 t.EditName	   = isnull( s.EditName,        '')
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
				select isnull(s.Id           ,'')
					  ,isnull(s.Name         ,'')
					  ,isnull(s.Description  ,'')
					  ,isnull(s.Junk         ,0)
					  ,s.AddDate
					  ,isnull(s.AddName      ,'')
					  ,s.EditDate
					  ,isnull(s.EditName     ,'')
				from dbo.SciTrade_To_Pms_RepairType s
				where not exists(select 1 from dbo.RepairType t where t.id=s.id)

	 -----------TPEMachine------------------------
	 select * into #tmpTPEMachine
	 from dbo.SciTrade_To_Pms_Part 
	 where type = 'M'

	 update t set	t.MasterGroupID		    = isnull( s.MasterGroupID       ,'')
					,t.MachineGroupID		= isnull( s.MachineGroupID      ,'')
					,t.Model				= isnull( s.Model               ,'')
					,t.MachineBrandID		= isnull( s.MachineBrandID      ,'')
					,t.Description			= isnull( s.Description         ,'')
					,t.DescriptionDetail	= isnull( s.DescriptionDetail   ,'')
					,t.Origin				= isnull( s.Origin              ,'')
					,t.Junk					= isnull( s.Junk                ,0)
					,t.AddName				= isnull( s.AddName             ,'')
					,t.AddDate				=  s.AddDate
					,t.EditName				= isnull( s.EditName,'')
					,t.EditDate				= s.EditDate
	 from dbo.TPEMachine t
	 inner join #tmpTPEMachine s on t.ID = s.Refno 

	 insert into dbo.TPEMachine(ID,MasterGroupID,MachineGroupID,Model,MachineBrandID,Description
			,DescriptionDetail,Origin,Picture1,Picture2,Junk
			,AddName,AddDate,EditName,EditDate)
    SELECT isnull(s.refno,              ''),
           isnull(s.mastergroupid,      ''),
           isnull(s.machinegroupid,     ''),
           isnull(s.model,              ''),
           isnull(s.machinebrandid,     ''),
           isnull(s.description,        ''),
           isnull(s.descriptiondetail,  ''),
           isnull(s.origin,             ''),
           '',
           '',
           isnull(s.junk,               0),
           isnull(s.addname,            ''),
           s.adddate,
           isnull(s.editname,           ''),
           s.editdate
    from #tmpTPEMachine s
    where not exists(select 1 from dbo.TPEMachine t where t.ID = s.Refno )

	drop table #tmpTPEMachine
	
	 -----------PartPrice_History ------------------------
	select *
	into	#tmpPartPrice_History
	from dbo.SciTrade_To_Pms_TradeHIS_MMS WITH (NOLOCK) 
	where TableName = 'Part' and HisType = 'ControlParts' 

	 update t set t.PartID 		= isnull( s.RefNo   ,'')
				,t.HisType		= isnull( s.HisType ,'')
				,t.OldValue		= isnull( s.OldValue,'')
				,t.NewValue		= isnull( s.NewValue,'')
				,t.Remark		= isnull( s.Remark  ,'')
				,t.AddName		= isnull( s.AddName ,'')
				,t.AddDate		= s.AddDate
	 from dbo.PartPrice_History t
	 inner join #tmpPartPrice_History s on t.TradeHisMMSUkey = s.Ukey  

	 insert into dbo.PartPrice_History(TradeHisMMSUkey,  PartID,  HisType,  OldValue,  NewValue
		   ,Remark,  AddName,  AddDate)
    SELECT  isnull(s.ukey,    0),
            isnull(s.refno,   ''),
            isnull(s.histype, ''),
            isnull(s.oldvalue,''),
            isnull(s.newvalue,''),
            isnull(s.remark,  ''),
            isnull(s.addname, ''),
            s.adddate 
	from #tmpPartPrice_History s
	where not exists(select 1 from dbo.PartPrice_History t where t.TradeHisMMSUkey = s.Ukey  )

	drop table #tmpPartPrice_History

	------------MachinePending------------------
	update t set t.TPEComplete = isnull(s.TPEComplete,0)
	from dbo.MachinePending t
	inner join dbo.SciTrade_To_Pms_MachinePending s on t.id=s.id
	where t.status = 'Confirmed'

	------------MachinePending_Detail------------------
	declare @Tdebit table(id varchar(13),MachineID varchar(16),TPEReject int,TPEApvDate datetime)

	select	 md.id
			,md.seq
			,md.TPEApvDate
			,md.TPEApvName
			,md.TPEReject
			,m.status
	into #tmpMachinePending_Detail
	from dbo.SciTrade_To_Pms_MachinePending_Detail md
	inner join dbo.MachinePending m on m.id = md.id

	update t 
	set t.TPEReject = isnull(s.TPEReject,0)
	,t.TPEApvName = isnull(s.TPEApvName ,'')
	,t.TPEApvDate = s.TPEApvDate 
	from dbo.MachinePending_Detail t
	inner join #tmpMachinePending_Detail s on t.id=s.id and t.seq = s.seq
	where s.status = 'Confirmed' and s.TPEApvDate is not null

	insert into @Tdebit(id, MachineID, TPEReject,TPEApvDate)
	SELECT
        isnull(t.id,       ''),
        isnull(t.machineid,''),
        isnull(s.tpereject, 0),
        s.TPEApvDate
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

	--update exp_Part轉出資料的SendToTPE資料
	UPDATE a
	SET TranstoTPE = CONVERT(date, GETDATE())
	FROM dbo.PartPO a
	INNER JOIN SciPms_To_Trade_PartPO b ON a.ID = b.ID
	WHERE a.TranstoTPE  IS NULL
	
	UPDATE a
	SET TranstoTPE = CONVERT(date, GETDATE())
	FROM dbo.RepairPO a
	INNER JOIN SciPms_To_Trade_RepairPO b ON a.ID = b.ID
	WHERE a.TranstoTPE  IS NULL
	
	UPDATE a
	SET TranstoTPE = CONVERT(date, GETDATE())
	FROM dbo.MiscPO a
	INNER JOIN SciPms_To_Trade_MiscPO b ON a.ID = b.ID
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