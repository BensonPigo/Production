-- =============================================
-- Create date: 2021/10/22
-- Description:	ISP20211230 QKPI Web FinalInspection Data Transfer to PMS QA.P32
-- =============================================
Create Procedure FinalInspectionDataToPMS

AS 
BEGIN

	SET NOCOUNT ON;

	declare @T table (id varchar(13))	

-- CFAInspectionRecord
Merge Production.dbo.CFAInspectionRecord as t
Using (
	select f.* 
	,[IsCombinePO] = iif(Forder.cnt >1 , 1 , 0)
	,[FirstInspection] = IIF(f.InspectionTimes = 1, 1 , 0)
	,[ClogReceivedPercentage] = clog.Value
	from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection f
	outer apply(
		select cnt = count(1) 
		from
		(
			select distinct OrderID
			from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_Order fo
			where f.id = fo.id
		) a
	) Forder
	outer apply(
		SELECT Value = CAST(ROUND( SUM(IIF( CFAReceiveDate IS NOT NULL OR ReceiveDate IS NOT NULL
								,ShipQty
								,0)
						) * 1.0 
						/  SUM(ShipQty) * 100 
		,0) AS INT) 
		FROM PackingList_Detail pd WITH(NOLOCK)
		left join CFAInspectionRecord_OrderSEQ co on co.OrderID = pd.OrderID
		and co.SEQ = pd.OrderShipmodeSeq
		left join CFAInspectionRecord c on c.ID = co.ID
		where 1=1
	) clog
	where f.submitdate is not null
) as s
on t.ID=s.ID
when matched then 
	update set
	t.ID					 = s.ID,
	t.AuditDate				 = s.AuditDate,
	t.FactoryID				 = isnull(s.FactoryID,''),
	t.MDivisionid			 = isnull(s.MDivisionid,''),
	t.SewingLineID			 = isnull(s.[SewingLineID],''),
	t.Team					 = isnull(s.Team,''),
	t.[Shift]				 = isnull(s.[Shift],''),
	t.Stage					 = isnull(s.InspectionStage,''),
	t.InspectQty			 = s.SampleSize,
	t.DefectQty				 = s.RejectQty,
	t.ClogReceivedPercentage = s.ClogReceivedPercentage,
	t.Result				 = s.InspectionResult,
	t.CFA					 = s.CFA,
	t.[Status]				 = 'Confirmed',
	t.Remark				 = isnull(s.OthersRemark,''),
	t.AddName				 = isnull(s.AddName,''),
	t.AddDate				 = s.AddDate,
	t.EditName				 = isnull(s.EditName,''),
	t.EditDate				 = s.EditDate,
	t.IsCombinePO			 = s.IsCombinePO,
	t.FirstInspection		 = s.FirstInspection,
	t.IsImportFromMES		 = 1
when not matched by target then
	insert  (
	ID						,					
	AuditDate				,
	FactoryID				,
	MDivisionid				,
	SewingLineID			,
	Team					,
	[Shift]					,
	Stage					,
	InspectQty				,
	DefectQty				,
	ClogReceivedPercentage	,
	Result					,
	CFA						,
	[Status]				,
	Remark					,
	AddName					,
	AddDate					,
	EditName				,
	EditDate				,
	IsCombinePO				,
	FirstInspection			,
	IsImportFromMES		
	)
	values  (
	s.ID,
	s.AuditDate,
	isnull(s.FactoryID,''),
	isnull(s.MDivisionid,''),
	isnull(s.[SewingLineID],''),
	'A',
	'D',
	isnull(s.InspectionStage,''),
	s.SampleSize,
	s.RejectQty,
	s.ClogReceivedPercentage,
	isnull(s.InspectionResult,''),
	isnull(s.CFA,''),
	'Confirmed',
	isnull(s.OthersRemark,''),
	s.AddName,
	s.AddDate,
	isnull(s.EditName,''),
	s.EditDate,
	s.IsCombinePO,
	s.FirstInspection,
	'1'
	)
when not matched by source and t.IsImportFromMES = 1 then 
	delete
	output inserted.id into @T;

-- CFAInspectionRecord_Detail
Merge Production.dbo.CFAInspectionRecord_Detail as t
Using (
	select fd.*
	from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_Detail fd	
	where fd.ID in (select ID from @T)
) as s
on t.ID=s.ID and t.GarmentDefectCodeID = s.GarmentDefectCodeID
when matched then 
	update set
	t.ID					 = s.ID,
	t.[GarmentDefectCodeID]	 = s.GarmentDefectCodeID,
	t.[GarmentDefectTypeID]  = s.GarmentDefectTypeID,
	t.[Qty]					 = s.Qty,
	t.[Action]				 = '',
	t.[Remark]				 = '',
	t.[CFAAreaID]			 = ''
when not matched by target then
	insert  (
	ID						,
	[GarmentDefectCodeID]	,	
	[GarmentDefectTypeID]	,
	[Qty]					,
	[Action]				,
	[Remark]				,
	[CFAAreaID]				
	)
	values  (
	s.ID,
	s.GarmentDefectCodeID,
	s.GarmentDefectTypeID,
	s.Qty,
	'',
	'',
	''
	)
when not matched by source and t.ID in (select ID from @T) then 
	delete;

-- CFAInspectionRecord_OrderSEQ
Merge Production.dbo.CFAInspectionRecord_OrderSEQ as t
Using (
	select foq.*
	,[CTNNo] = isnull(CTN.CTNNoList,'')
	from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_Order_QtyShip foq
	outer apply(
		select CTNNoList = Stuff((
			select concat(',',CTNNo)
			from (
					select 	distinct
						CTNNo
					from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_OrderCarton foc
					where foc.id = foq.ID
					and foc.OrderID = foq.OrderID
					and foc.Seq = foq.Seq
				) s
			for xml path ('')
		) , 1, 1, '')
	) CTN
	where foq.ID in (select ID from @T)
) as s
on t.ID=s.ID and t.OrderID = s.OrderID and t.Seq = s.Seq
when matched then 
	update set
	t.[ID]		= s.ID,
	t.[OrderID] = s.OrderID,
	t.[SEQ]		= s.Seq,
	t.[Carton]  = s.CTNNo

when not matched by target then
	insert  (
	[ID]		,
	[OrderID]	,
	[SEQ]		,
	[Carton]	
	)
	values  (
	s.ID,
	s.OrderID,
	s.Seq,
	s.CTNNo
	)
when not matched by source and t.ID in (select ID from @T) then 
	delete;



-- 更新PackingList
-- StaggeredCFAInspectionRecordID
-- Stagger, Pass
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID = c.ID
from Production.dbo.PackingList_Detail p
inner join Production.dbo.CFAInspectionRecord_OrderSEQ co on co.OrderID = p.OrderID
and co.SEQ = p.OrderShipmodeSeq
inner join Production.dbo.CFAInspectionRecord c on c.ID = co.ID
where exists(
	select 1 from  @T s
	where s.ID = c.ID
)
and exists(
	select 1 
	from SplitString(co.Carton,',') sp 
	where sp.Data = p.CTNStartNo
)
and c.Stage = 'Stagger'
and c.Result = 'Pass'
and p.StaggeredCFAInspectionRecordID = ''

-- not Stagger, not Pass
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID = ''
from Production.dbo.PackingList_Detail p
inner join Production.dbo.CFAInspectionRecord_OrderSEQ co on co.OrderID = p.OrderID
and co.SEQ = p.OrderShipmodeSeq
inner join Production.dbo.CFAInspectionRecord c on c.ID = co.ID
where exists(
	select 1 from  @T s
	where s.ID = c.ID
)
and c.Stage != 'Stagger'
and c.Result != 'Pass'
and p.StaggeredCFAInspectionRecordID != ''



-- 更新PackingList
-- FirstStaggeredCFAInspectionRecordID
-- Stagger, Pass
UPDATE PackingList_Detail
SET FirstStaggeredCFAInspectionRecordID = c.ID
from Production.dbo.PackingList_Detail p
inner join Production.dbo.CFAInspectionRecord_OrderSEQ co on co.OrderID = p.OrderID
	and co.SEQ = p.OrderShipmodeSeq
inner join [ExtendServer].[ManufacturingExecution].dbo.FinalInspection c on c.ID = co.ID
where exists(
	select 1 from  @T s
	where s.ID = c.ID
)
and exists(
	select 1 
	from SplitString(co.Carton,',') sp 
	where sp.Data = p.CTNStartNo
)
and c.InspectionStage = 'Stagger'
and c.InspectionTimes = 1
and p.FirstStaggeredCFAInspectionRecordID = ''


-- 更新CFAInspectionRecord_OrderSEQ
DELETE t
FROM CFAInspectionRecord_OrderSEQ t
LEFT JOIN (
	select * from Production.dbo.CFAInspectionRecord_OrderSEQ where ID in (select ID from @T)
) s ON t.ID = s.ID  AND t.OrderID = s.OrderID AND t.Seq = s.Seq AND t.Carton = s.Carton  
WHERE t.ID in (select ID from @T) 
AND ( s.ID IS NULL OR s.OrderID IS NULL OR s.Seq IS NULL OR s.Carton IS NULL )

INSERT CFAInspectionRecord_OrderSEQ   (ID, OrderID, Seq, Carton)
SELECT ID, OrderID, Seq, Carton
FROM (
	select * from Production.dbo.CFAInspectionRecord_OrderSEQ where ID in (select ID from @T)
) s
WHERE NOT EXISTS(
    SELECT 1 FROM CFAInspectionRecord_OrderSEQ t 
    WHERE t.ID = s.ID AND t.OrderID = s.OrderID AND t.Seq = s.Seq AND t.Carton = s.Carton 
)

END	