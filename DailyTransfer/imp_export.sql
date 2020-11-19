-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/20>
-- Description:	<import export>
-- =============================================
Create PROCEDURE [dbo].[imp_Export]

AS
BEGIN

	SET NOCOUNT ON;

		declare @Sayfty table(id varchar(10)) --?u?t?N?X
		insert @Sayfty select id from Production.dbo.Factory
					   union
					   select id from Production.dbo.SciMachine_Factory	

	---create temp table---------

	select * into #TExport from Trade_To_Pms.dbo.Export e WITH (NOLOCK)
	outer apply(
		select PrepaidFtyImportFee2=e2.PrepaidFtyImportFee
		from Trade_To_Pms.dbo.Export e2
		where e.MainExportID08 = e2.id
	)MainExportID08
	where FactoryID in (select id from @Sayfty)

	

--------------------------Export---------------------------
	declare @T table (id varchar(13))

	Merge Production.dbo.Export as t
	using #TExport as s
	on t.id=s.id
		when matched then
		update set 
		t.ScheduleID = s.ScheduleID 
		, t.ScheduleDate = s.ScheduleDate 
		, t.LoadDate = s.LoadDate 
		, t.CloseDate = s.CloseDate 
		, t.Etd = s.Etd 
		, t.Eta = s.Eta 
		, t.ExportCountry = s.ExportCountry 
		, t.ImportCountry = s.ImportCountry 
		, t.ExportPort = s.ExportPort 
		, t.ImportPort = s.ImportPort 
		, t.CYCFS = s.CYCFS 
		, t.ShipModeID = s.ShipModeID 
		, t.ShipmentTerm = s.ShipmentTerm 
		, t.FactoryID = s.FactoryID 
		, t.ShipMark = s.ShipMark 
		, t.ShipMarkDesc = s.ShipMarkDesc 
		, t.Consignee = s.Consignee 
		, t.Handle = s.Handle 
		, t.Posting = s.Posting 
		, t.Payer = s.Payer 
		, t.CompanyID = s.CompanyID 
		, t.Confirm = s.Confirm 
		, t.LastEdit = s.LastEdit 
		, t.Remark = s.Remark 
		, t.Ecfa = s.Ecfa 
		, t.FormStatus = s.FormStatus 
		, t.Carrier = s.Carrier 
		, t.Forwarder = s.Forwarder 
		, t.Vessel = s.Vessel 
		, t.ShipTo = s.ShipTo 
		, t.Sono = s.Sono 
		, t.Blno = s.Blno 
		, t.InvNo = s.InvNo 
		, t.Exchange = s.Exchange 
		, t.Packages = s.Packages 
		, t.WeightKg = s.WeightKg 
		, t.NetKg = s.NetKg 
		, t.Cbm = s.Cbm 
		, t.CbmFor = s.CbmFor 
		, t.Takings = s.Takings 
		, t.TakingFee = s.TakingFee 
		, t.Broker = s.Broker 
		, t.Insurer = s.Insurer 
		, t.Trailer1 = s.Trailer1 
		, t.Trailer2 = s.Trailer2 
		, t.Freight = s.Freight 
		, t.Insurance = s.Insurance 
		, t.Junk = s.Junk 
		, t.AddName = s.AddName 
		, t.AddDate = s.AddDate 
		, t.EditName= iif(s.[EditDate] <= t.EditDate,t.EditName,s.[EditName])
		, t.EditDate= iif(s.[EditDate] <= t.EditDate,t.EditDate,s.[EditDate])
		, t.MainExportID = s.MainExportID
		, t.Replacement = s.Replacement
		, t.Delay = s.Delay
		, t.PrepaidFtyImportFee = s.PrepaidFtyImportFee
		, t.NoImportCharges = iif(s.PrepaidFtyImportFee2 > 0, 1 ,0)
		, t.MainExportID08 = s.MainExportID08
		, t.FormE = s.FormE
		, t.SQCS = s.SQCS
		, t.FtyTruckFee  = s.FtyTruckFee 
		, t.FtyTrucker = s.FtyTrucker
		, t.OTFee = s.OTFee
		, t.CIFTerms = s.CIFTerms
	  when not matched  by target then 
		insert (ID ,ScheduleID ,ScheduleDate ,LoadDate ,CloseDate ,Etd ,Eta ,ExportCountry ,ImportCountry ,ExportPort ,ImportPort 
		,CYCFS ,ShipModeID ,ShipmentTerm ,FactoryID ,ShipMark ,ShipMarkDesc ,Consignee ,Handle ,Posting ,Payer ,CompanyID 
		,Confirm ,LastEdit ,Remark ,Ecfa ,FormStatus ,Carrier ,Forwarder ,Vessel ,ShipTo ,Sono ,Blno ,InvNo ,Exchange 
		,Packages ,WeightKg ,NetKg ,Cbm ,CbmFor ,Takings ,TakingFee ,PortArrival ,DocArrival ,Broker 
		,Insurer ,Trailer1 ,Trailer2 ,Freight ,Insurance ,Junk ,AddName ,AddDate ,EditName ,EditDate,MainExportID ,Replacement ,Delay ,PrepaidFtyImportFee,NoImportCharges
		,MainExportID08 ,FormE, SQCS, FtyTruckFee, FtyTrucker, OTFee, CIFTerms)
	    values( 
		s.ID ,s.ScheduleID ,s.ScheduleDate ,s.LoadDate ,s.CloseDate ,s.Etd ,s.Eta ,s.ExportCountry ,s.ImportCountry ,s.ExportPort ,s.ImportPort 
		,s.CYCFS,s.ShipModeID ,s.ShipmentTerm ,s.FactoryID ,s.ShipMark ,s.ShipMarkDesc ,s.Consignee ,s.Handle ,s.Posting ,s.Payer ,s.CompanyID 
		,s.Confirm ,s.LastEdit ,s.Remark ,s.Ecfa ,s.FormStatus,s.Carrier ,s.Forwarder ,s.Vessel ,s.ShipTo ,s.Sono ,s.Blno ,s.InvNo ,s.Exchange 
		,s.Packages ,s.WeightKg ,s.NetKg ,s.Cbm ,s.CbmFor ,s.Takings ,s.TakingFee ,s.PortArrival ,s.DocArrival ,s.Broker 
		,s.Insurer ,s.Trailer1 ,s.Trailer2 ,s.Freight ,s.Insurance ,s.Junk ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate,s.MainExportID ,s.Replacement ,s.Delay, s.PrepaidFtyImportFee, iif(s.PrepaidFtyImportFee2 > 0, 1 ,0)
		,s.MainExportID08 ,s.FormE, s.SQCS, s.FtyTruckFee, s.FtyTrucker, s.OTFee, s.CIFTerms)
	  output inserted.id into @T; 

-----------------------Export_detail-----------------------------
	RAISERROR('Import Export - Starts',0,0)

	Merge  Production.dbo.Export_Detail as PE2 
	using (
		select * 
		from Trade_To_Pms.dbo.Export_Detail WITH (NOLOCK) 
		where Trade_To_Pms.dbo.Export_Detail.id in (select ID from @T)
	) as TE2 on  PE2.Ukey = TE2.Ukey 
				 and PE2.ShipPlanHandle = TE2.ShipPlanHandle
	 when matched then 
		update set
		  PE2.[ID] = TE2.[ID]
		  , PE2.[PoID] = TE2.[PoID]
		  , PE2.[Seq1] = TE2.[Seq1]
		  , PE2.[Seq2] = TE2.[Seq2]
		  , PE2.[FormXReceived] = TE2.[FormXReceived]
		  , PE2.[FormXFTYEdit] = TE2.[FormXFTYEdit]
	      , PE2.[ExportIDOld]= TE2.[ExportIDOld]
	      , PE2.[Qty]=TE2.[Qty]
	      , PE2.[Foc]=TE2.[Foc]
	      , PE2.[Carton]=TE2.[Carton]
	      , PE2.[Confirm]=TE2.[Confirm]
	      , PE2.[UnitId]=TE2.[UnitId]
	      , PE2.[Price]=TE2.[Price]
	      , PE2.[NetKg]=TE2.[NetKg]
	      , PE2.[WeightKg]=TE2.[WeightKg]
	      , PE2.[Remark]=TE2.[Remark]
	      , PE2.[PayDesc]=TE2.[PayDesc]
	      , PE2.[LastEta]=TE2.[LastEta]
	      , PE2.[Refno]=TE2.[Refno]
	      , PE2.[SuppID]=TE2.[SuppID]
	      , PE2.[Pino]=TE2.[Pino]
	      , PE2.[Description]=TE2.[Description]
	      , PE2.[UnitOld]=TE2.[UnitOld]
	      , PE2.[PinoOld]=TE2.[PinoOld]
	      , PE2.[SuppIDOld]=TE2.[SuppIDOld]
	      , PE2.[PriceOld]=TE2.[PriceOld]
	      , PE2.[ShipPlanID]=TE2.[ShipPlanID]
	      , PE2.[PoHandle]=TE2.[PoHandle]
	      , PE2.[PcHandle]=TE2.[PcHandle]
	      , PE2.[IsFormA]=TE2.[IsFormA]
	      , PE2.[FormXDraftCFM]=TE2.[FormXDraftCFM]
	      , PE2.[FormXINV]=TE2.[FormXINV]      
	      , PE2.[FormXEdit]=TE2.[FormXEdit]
	      , PE2.[FormXPayINV]=TE2.[FormXPayINV]
	      , PE2.[FormXType]=TE2.[FormXType]
	      , PE2.[FormXAwb]=TE2.[FormXAwb]
	      , PE2.[FormXCarrier]=TE2.[FormXCarrier]
	      , PE2.[FormXRemark]=TE2.[FormXRemark]
	      , PE2.[AddName]=TE2.[AddName]
	      , PE2.[AddDate]=TE2.[AddDate]
	      , PE2.[EditDate]=TE2.[EditDate]
	      , PE2.[EditName]=TE2.[EditName]
		  , PE2.PoType = Te2.PoType
		  , Pe2.FabricType=Te2.FabricType
		  , pe2.[BalanceQty] = te2.[BalanceQty]
		  , pe2.[BalanceFOC]=te2.[BalanceFOC]
		  , pe2.[ShipPlanHandle]=te2.[ShipPlanHandle]
		  , pe2.currencyID=(select isnull(currencyID,'') from supp where id=te2.suppid)
		  , pe2.InvoiceNo = te2.InvoiceNo
	  when not matched by target then 
		insert (    [ID],    [PoID],    [Seq1],    [Seq2],    [ExportIDOld],    [Ukey],    [Qty],    [Foc],    [Carton],    [Confirm],    [UnitId],    [Price],    [NetKg],    [WeightKg],    [Remark],    [PayDesc],    [LastEta],    [Refno],    [SuppID],    [Pino],    [Description],    [UnitOld],    [PinoOld],    [SuppIDOld],    [PriceOld],    [ShipPlanID],    [ShipPlanHandle],    [PoHandle],    [PcHandle],    [IsFormA],    [FormXDraftCFM],    [FormXINV],    [FormXReceived],    [FormXFTYEdit],    [FormXEdit],    [FormXPayINV],    [FormXType],    [FormXAwb],    [FormXCarrier],    [FormXRemark],    [AddName],    [AddDate],    [EditDate],    [EditName],    [BalanceQty],    [BalanceFOC],    PoType,    FabricType,  InvoiceNo)
		values (TE2.[ID],TE2.[PoID],TE2.[Seq1],TE2.[Seq2],TE2.[ExportIDOld],TE2.[Ukey],TE2.[Qty],TE2.[Foc],TE2.[Carton],TE2.[Confirm],TE2.[UnitID],TE2.[Price],TE2.[NetKg],TE2.[WeightKg],TE2.[Remark],TE2.[PayDesc],TE2.[LastEta],TE2.[Refno],TE2.[SuppID],TE2.[Pino],TE2.[Description],TE2.[UnitOld],TE2.[PinoOld],TE2.[SuppIDOld],TE2.[PriceOld],TE2.[ShipPlanID],TE2.[ShipPlanHandle],TE2.[PoHandle],TE2.[PcHandle],TE2.[IsFormA],TE2.[FormXDraftCFM],TE2.[FormXINV],TE2.[FormXReceived],TE2.[FormXFTYEdit],TE2.[FormXEdit],TE2.[FormXPayINV],TE2.[FormXType],TE2.[FormXAwb],TE2.[FormXCarrier],TE2.[FormXRemark],TE2.[AddName],TE2.[AddDate],TE2.[EditDate],TE2.[EditName],TE2.[BalanceQty],TE2.[BalanceFOC],Te2.PoType,Te2.FabricType,
Te2.InvoiceNo)
	  when not matched by source and PE2.id in (select id from @T)then
	  	delete;


					
-----------------------Export_ShipAdvice_Container-----------------------------


INSERT INTO Production.dbo.Export_ShipAdvice_Container
        (Ukey,Export_Detail_Ukey,ContainerType,ContainerNo,AddName,AddDate,EditName,EditDate)
SELECT Ukey,Export_Detail_Ukey,ContainerType,ContainerNo,AddName,AddDate,EditName,EditDate
FROM Trade_To_Pms.dbo.Export_ShipAdvice_Container s
WHERE NOT EXISTS (SELECT 1 FROM Production.dbo.Export_ShipAdvice_Container WHERE Ukey = s.Ukey)

UPDATE t
SET  t.Export_Detail_Ukey = s.Export_Detail_Ukey
    ,t.ContainerType = s.ContainerType
    ,t.ContainerNo = s.ContainerNo
    ,t.AddName = s.AddName
    ,t.AddDate = s.AddDate
    ,t.EditName = s.EditName
    ,t.EditDate = s.EditDate
FROM Production.dbo.Export_ShipAdvice_Container t
INNER JOIN Trade_To_Pms.dbo.Export_ShipAdvice_Container s ON t.Ukey = s.Ukey


DELETE t 
FROM Production.dbo.Export_ShipAdvice_Container t 
WHERE NOT EXISTS(SELECT 1 FROM Trade_To_Pms.dbo.Export_ShipAdvice_Container s where t.Ukey = s.Ukey)
----只刪除轉出區間內，有少的Export_Detail_Ukey
AND t.Export_Detail_Ukey IN (		
			SELECT a.Ukey
			FROM Trade_To_Pms.dbo.Export_Detail a WITH (NOLOCK) 
			WHERE a.ID in (SELECT ID FROM @T)		
	)

-----------------------Export_Detail_Carton-----------------------------
	RAISERROR('Import Export_Detail_Carton - Starts',0,0)
	Merge Production.dbo.Export_Detail_Carton as t
	Using (select * from Trade_To_Pms.dbo.Export_Detail_Carton a WITH (NOLOCK) 
		where exists (select ukey from Trade_To_Pms.dbo.Export_Detail WITH (NOLOCK) where ukey = a.Export_DetailUkey) 
		and exists (select ID from @T where id = a.id))as s
	on t.ukey = s.ukey
	when matched then
		update set
		 t.[Export_DetailUkey] =s.[Export_DetailUkey]
		,t.[Id]				   =s.[Id]
		,t.[PoID]			   =s.[PoID]
		,t.[Seq1]			   =s.[Seq1]
		,t.[Seq2]			   =s.[Seq2]
		,t.[Carton]			   =s.[Carton]
		,t.[LotNo]			   =s.[LotNo]
		,t.[Qty]			   =s.[Qty]
		,t.[Foc]			   =s.[Foc]
		,t.[NetKg]			   =s.[NetKg]
		,t.[WeightKg]		   =s.[WeightKg]
		,t.[EditName]		   =s.[EditName]
		,t.[EditDate]		   =s.[EditDate]
	when not matched by target then 	
	insert([Export_DetailUkey],[Id],[PoID],[Seq1],[Seq2],[Carton],[LotNo],[Qty],[Foc],[NetKg],[WeightKg],[EditName],[EditDate])
	values(s.[Export_DetailUkey],s.[Id],s.[PoID],s.[Seq1],s.[Seq2],s.[Carton],s.[LotNo],s.[Qty],s.[Foc],s.[NetKg],s.[WeightKg]
	,s.[EditName],s.[EditDate])
	when not matched by source and exists (select ID from @T where id = t.id)then
	  		delete;
			
-----------------------POShippingList_Line-----------------------------
--刪除:存在Trade_To_Pms.Export_Detail不存在Trade_To_Pms.POShippingList_Line
delete t
from Production.dbo.POShippingList_Line t
where exists (select 1 from Trade_To_Pms.dbo.Export_Detail s where s.Ukey = t.Export_Detail_Ukey)
and not exists (select 1 from Trade_To_Pms.dbo.POShippingList_Line s where s.POShippingList_Ukey = t.POShippingList_Ukey and s.QRCode = t.QRCode and s.Line = t.Line)

--更新:存在Trade_To_Pms.Export_Detail存在Production.POShippingList_Line
update t set
	 [Export_Detail_Ukey]	=s.[Export_Detail_Ukey]
	,[RefNo]				=s.[RefNo]
	,[Description]			=s.[Description]
	,[MaterialColor]		=s.[MaterialColor]
	,[Weight]				=s.[Weight]
	,[WeightUnitID]			=s.[WeightUnitID]
	,[Width]				=s.[Width]
	,[WidthUnitID]			=s.[WidthUnitID]
	,[Length]				=s.[Length]
	,[LengthUnitID]			=s.[LengthUnitID]
	,[Height]				=s.[Height]
	,[HeightUnitID]			=s.[HeightUnitID]
	,[Thickness]			=s.[Thickness]
	,[ThicknessUnitID]		=s.[ThicknessUnitID]
	,[SizeSpec]				=s.[SizeSpec]
	,[Price]				=s.[Price]
	,[BatchNo]				=s.[BatchNo]
	,[PackageNo]			=s.[PackageNo]
	,[ShipQty]				=s.[ShipQty]
	,[ShipQtyUnitID]		=s.[ShipQtyUnitID]
	,[FOC]					=s.[FOC]
	,[FOCUnitID]			=s.[FOCUnitID]
	,[NW]					=s.[NW]
	,[NWUnitID]				=s.[NWUnitID]
	,[GW]					=s.[GW]
	,[GWUnitID]				=s.[GWUnitID]
	,[AdditionalOptional1]	=s.[AdditionalOptional1]
	,[AdditionalOptional2]	=s.[AdditionalOptional2]
	,[AdditionalOptional3]	=s.[AdditionalOptional3]
	,[AdditionalOptional4]	=s.[AdditionalOptional4]
	,[AdditionalOptional5]	=s.[AdditionalOptional5]
	,[AddName]				=s.[AddName]
	,[AddDate]				=s.[AddDate]
from Trade_To_Pms.dbo.POShippingList_Line s
inner join Production.dbo.POShippingList_Line t on s.POShippingList_Ukey = t.POShippingList_Ukey and s.QRCode = t.QRCode and s.Line = t.Line
where exists (select 1 from Trade_To_Pms.dbo.Export_Detail s where s.Ukey = t.Export_Detail_Ukey)

--刪除:存在Trade_To_Pms.Export_Detail不存在Production.POShippingList_Line
INSERT INTO [dbo].[POShippingList_Line]
           ([POShippingList_Ukey]
           ,[Export_Detail_Ukey]
           ,[QRCode]
           ,[Line]
           ,[RefNo]
           ,[Description]
           ,[MaterialColor]
           ,[Weight]
           ,[WeightUnitID]
           ,[Width]
           ,[WidthUnitID]
           ,[Length]
           ,[LengthUnitID]
           ,[Height]
           ,[HeightUnitID]
           ,[Thickness]
           ,[ThicknessUnitID]
           ,[SizeSpec]
           ,[Price]
           ,[BatchNo]
           ,[PackageNo]
           ,[ShipQty]
           ,[ShipQtyUnitID]
           ,[FOC]
           ,[FOCUnitID]
           ,[NW]
           ,[NWUnitID]
           ,[GW]
           ,[GWUnitID]
           ,[AdditionalOptional1]
           ,[AdditionalOptional2]
           ,[AdditionalOptional3]
           ,[AdditionalOptional4]
           ,[AdditionalOptional5]
           ,[AddName]
           ,[AddDate])
select
	 s.[POShippingList_Ukey]
	,s.[Export_Detail_Ukey]
	,s.[QRCode]
	,s.[Line]
	,s.[RefNo]
	,s.[Description]
	,s.[MaterialColor]
	,s.[Weight]
	,s.[WeightUnitID]
	,s.[Width]
	,s.[WidthUnitID]
	,s.[Length]
	,s.[LengthUnitID]
	,s.[Height]
	,s.[HeightUnitID]
	,s.[Thickness]
	,s.[ThicknessUnitID]
	,s.[SizeSpec]
	,s.[Price]
	,s.[BatchNo]
	,s.[PackageNo]
	,s.[ShipQty]
	,s.[ShipQtyUnitID]
	,s.[FOC]
	,s.[FOCUnitID]
	,s.[NW]
	,s.[NWUnitID]
	,s.[GW]
	,s.[GWUnitID]
	,s.[AdditionalOptional1]
	,s.[AdditionalOptional2]
	,s.[AdditionalOptional3]
	,s.[AdditionalOptional4]
	,s.[AdditionalOptional5]
	,s.[AddName]
	,s.[AddDate]
from Trade_To_Pms.dbo.POShippingList_Line s
left join Production.dbo.POShippingList_Line t on s.POShippingList_Ukey = t.POShippingList_Ukey and s.QRCode = t.QRCode and s.Line = t.Line
where exists (select 1 from Trade_To_Pms.dbo.Export_Detail e where e.Ukey = s.Export_Detail_Ukey)
and t.POShippingList_Ukey is null

-----------------------POShippingList-----------------------------
--更新: 存在表身 Production.dbo.POShippingList_Line, 表頭存在
UPDATE t set
	 [IssueDate]		=s.[IssueDate]
	,[POID]				=s.[POID]
	,[Seq1]				=s.[Seq1]
	,[T1Name]			=s.[T1Name]
	,[T1MR]				=s.[T1MR]
	,[T1FtyName]		=s.[T1FtyName]
	,[T1FtyBrandCode]	=s.[T1FtyBrandCode]
	,[T2Name]			=s.[T2Name]
	,[T2SuppName]		=s.[T2SuppName]
	,[T2SuppBrandCode]	=s.[T2SuppBrandCode]
	,[T2SuppCountry]	=s.[T2SuppCountry]
	,[BrandID]			=s.[BrandID]
	,[CurrencyID]		=s.[CurrencyID]
	,[PackingNo]		=s.[PackingNo]
	,[PackingDate]		=s.[PackingDate]
	,[InvoiceNo]		=s.[InvoiceNo]
	,[InvoiceDate]		=s.[InvoiceDate]
	,[CloseDate]		=s.[CloseDate]
	,[Vessel]			=s.[Vessel]
	,[ETD]				=s.[ETD]
	,[FinalShipmodeID]	=s.[FinalShipmodeID]
	,[SuppID]			=s.[SuppID]
	,[AddName]			=s.[AddName]
	,[AddDate]			=s.[AddDate]
from Trade_To_Pms.dbo.POShippingList s
inner join Production.dbo.POShippingList t on s.Ukey = t.Ukey
and exists(select 1 from Production.dbo.POShippingList_Line where POShippingList_Ukey = s.Ukey)

--新增: 存在表身 Production.dbo.POShippingList_Line, 表頭不存在
INSERT INTO [dbo].[POShippingList]
           ([Ukey]
           ,[IssueDate]
           ,[POID]
           ,[Seq1]
           ,[T1Name]
           ,[T1MR]
           ,[T1FtyName]
           ,[T1FtyBrandCode]
           ,[T2Name]
           ,[T2SuppName]
           ,[T2SuppBrandCode]
           ,[T2SuppCountry]
           ,[BrandID]
           ,[CurrencyID]
           ,[PackingNo]
           ,[PackingDate]
           ,[InvoiceNo]
           ,[InvoiceDate]
           ,[CloseDate]
           ,[Vessel]
           ,[ETD]
           ,[FinalShipmodeID]
		   ,[SuppID]
           ,[AddName]
           ,[AddDate])
select
	 s.[Ukey]
	,s.[IssueDate]
	,s.[POID]
	,s.[Seq1]
	,s.[T1Name]
	,s.[T1MR]
	,s.[T1FtyName]
	,s.[T1FtyBrandCode]
	,s.[T2Name]
	,s.[T2SuppName]
	,s.[T2SuppBrandCode]
	,s.[T2SuppCountry]
	,s.[BrandID]
	,s.[CurrencyID]
	,s.[PackingNo]
	,s.[PackingDate]
	,s.[InvoiceNo]
	,s.[InvoiceDate]
	,s.[CloseDate]
	,s.[Vessel]
	,s.[ETD]
	,s.[FinalShipmodeID]
	,s.[SuppID]
	,s.[AddName]
	,s.[AddDate]
from Trade_To_Pms.dbo.POShippingList s
left join Production.dbo.POShippingList t on s.Ukey = t.Ukey
where t.Ukey is null
and exists(select 1 from Production.dbo.POShippingList_Line where POShippingList_Ukey = s.Ukey)

--刪除: 上面做完後,表身為空,表頭刪除
delete pl
from Production.dbo.POShippingList pl
left join Production.dbo.POShippingList_Line pll on pl.ukey = pll.POShippingList_Ukey
where pll.POShippingList_Ukey is null

-----------------------Export_Container-----------------------------
	RAISERROR('Import Export_Container - Starts',0,0)

	Merge Production.dbo.Export_Container as t 
	using (
		select *
		from Trade_To_Pms.dbo.Export_Container ec WITH (NOLOCK)
		where exists (
					select 1
					from Production.dbo.Export e
					where e.id = ec.id)
	) as s on t.ID = s.ID AND t.Container = s.Container
	when matched then 
		update set
			  t.Seq				=  s.Seq
			, t.Type			=  s.Type
			, t.CartonQty		=  s.CartonQty
			, t.WeightKg		=  s.WeightKg
			, t.AddName			=  s.AddName
			, t.AddDate			=  s.AddDate
			, t.EditName		=  s.EditName
			, t.EditDate		=  s.EditDate
	when not matched by target then 
		insert (   [ID],  [Seq],  [Type],  [Container],  [CartonQty],  [WeightKg],  [AddName],  [AddDate],  [EditName],  [EditDate])
		values ( s.[ID],s.[Seq],s.[Type],s.[Container],s.[CartonQty],s.[WeightKg],s.[AddName],s.[AddDate],s.[EditName],s.[EditDate])
	;
	
	DELETE pms
	FROM Production.dbo.Export_Container pms
	WHERE   EXISTS (
				SELECT 1 
				FROM Trade_To_Pms.dbo.Export e 
				WHERE e.ID=pms.ID					  
					  and FactoryID in (select id from @Sayfty)
			) -- 存在於Trade轉出的WK#
			AND NOT EXISTS(
				SELECT 1 
				FROM Trade_To_Pms.dbo.Export_Container ec 
				WHERE ec.ID=pms.ID 
					  AND ec.Container=pms.Container	
			)--不存在Trade轉出的Container
	;


-- Export_ShareAmount

--刪除主TABLE多的資料
Delete Production.dbo.Export_ShareAmount
from Production.dbo.Export_ShareAmount as a 
left join Trade_To_Pms.dbo.Export_ShareAmount as b
on a.id = b.id 
where b.id is null
AND EXISTS(
	select 1 
	from Trade_To_Pms.dbo.Export t 
	where t.ID = a.Id
)
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣
UPDATE a
SET 
	a.Id			= ISNULL(b.Id,''),
	a.ShipPlanID	= ISNULL(b.ShipPlanID,''),
	a.SuppID		= ISNULL(b.SuppID,''),
	a.SMR			= ISNULL(b.SMR,''),
	a.Handle		= ISNULL(b.Handle,''),
	a.PCHandle		= ISNULL(b.PCHandle,''),
	a.DutyID		= ISNULL(b.DutyID,''),
	a.Duty			= ISNULL(b.Duty,''),
	a.Freight		= ISNULL(b.Freight,0),
	a.Insurance		= ISNULL(b.Insurance,0),
	a.Tax			= ISNULL(b.Tax,0),
	a.GW			= ISNULL(b.GW,0),
	a.Remark		= ISNULL(b.Remark,''),
	a.Status		= ISNULL(b.Status,''),
	a.StatusUpdate	= b.StatusUpdate,
	a.AddName		= ISNULL(b.AddName,''),
	a.AddDate		= b.AddDate,
	a.EditName		= ISNULL(b.EditName,''),
	a.EditDate		= b.EditDate,
	a.ShareGW		= ISNULL(b.ShareGW,0),
	a.GWUpdateDate	= b.GWUpdateDate,
	a.Mailed		= ISNULL(b.Mailed,0),
	a.POID			= ISNULL(b.POID,'')
from Production.dbo.Export_ShareAmount as a 
inner join Trade_To_Pms.dbo.Export_ShareAmount as b on b.ukey = a.ukey

-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Export_ShareAmount
 (
	 Ukey
	,Id
	,ShipPlanID
	,SuppID
	,SMR
	,Handle
	,PCHandle
	,DutyID
	,Duty
	,Freight
	,Insurance
	,Tax
	,GW
	,Remark
	,Status
	,StatusUpdate
	,AddName
	,AddDate
	,EditName
	,EditDate
	,ShareGW
	,GWUpdateDate
	,Mailed
	,POID

)
SELECT 
	 Ukey
	,ISNULL(Id,'')
	,ISNULL(ShipPlanID,'')
	,ISNULL(SuppID,'')
	,ISNULL(SMR,'')
	,ISNULL(Handle,'')
	,ISNULL(PCHandle,'')
	,ISNULL(DutyID,'')
	,ISNULL(Duty,'')
	,ISNULL(Freight,0)
	,ISNULL(Insurance,0)
	,ISNULL(Tax,0)
	,ISNULL(GW,0)
	,ISNULL(Remark,'')
	,ISNULL(Status,'')
	,StatusUpdate
	,ISNULL(AddName,'')
	,AddDate
	,ISNULL(EditName,'')
	,EditDate
	,ISNULL(ShareGW,0)
	,GWUpdateDate
	,ISNULL(Mailed,0)
	,ISNULL(POID,'')
from Trade_To_Pms.dbo.Export_ShareAmount as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Export_ShareAmount as a WITH (NOLOCK) 
where a.Ukey = b.Ukey)

drop table #TExport;

-----------------------MtlCertificate-----------------------------
	select m.ID			,
		m.Consignee	,
		m.CartonNo	,
		m.ExportID	,
		m.Handle		,
		m.Mailed		,
		m.Junk		,
		m.AddName		,
		m.AddDate		,
		m.EditName	,
		m.EditDate
	into #tmpMtlCertificate
	from Trade_To_Pms.dbo.MtlCertificate m
	inner join Production.dbo.Factory f on f.ID = m.Consignee
	where f.IsOriginalFty = 1
	
	update m set	m.Consignee		 = tm.Consignee	   ,
				m.CartonNo		 = tm.CartonNo	   ,
				m.ExportID		 = tm.ExportID	   ,
				m.Handle		 = tm.Handle	   ,
				m.Mailed		 = tm.Mailed	   ,
				m.Junk			 = tm.Junk		   ,
				m.AddName		 = tm.AddName	   ,
				m.AddDate		 = tm.AddDate	   ,
				m.TPEEditName	 = tm.EditName	   ,
				m.TPEEditDate	 = tm.EditDate
	from Production.dbo.MtlCertificate m
	inner join #tmpMtlCertificate tm on tm.ID = M.ID
	
	insert into Production.dbo.MtlCertificate(	ID		   ,
												Consignee  ,
												CartonNo   ,
												ExportID   ,
												Handle	   ,
												Mailed	   ,
												Junk	   ,
												AddName	   ,
												AddDate	   ,
												TPEEditName,
												TPEEditDate
												)
				select	tm.ID		   ,
						tm.Consignee  ,
						tm.CartonNo   ,
						tm.ExportID   ,
						tm.Handle	   ,
						tm.Mailed	   ,
						tm.Junk	   ,
						tm.AddName	   ,
						tm.AddDate	   ,
						tm.EditName   ,
						tm.EditDate
				from #tmpMtlCertificate tm
				where not exists(select 1 from Production.dbo.MtlCertificate m with (nolock) where tm.ID = m.ID)

-----------------------MtlCertificate_Detail-----------------------------
	select 
	Ukey		   ,
	ID			   ,
	SuppID		   ,
	InvoiceNo	   ,
	FormType	   ,
	FormNo		   ,
	TpeReceiveDate ,
	Remark		   ,
	Junk		   ,
	AddName		   ,
	AddDate		   ,
	EditName	   ,
	EditDate
	into #tmpMtlCertificate_Detail
	from Trade_To_Pms.dbo.MtlCertificate_Detail md
	where exists(select 1 from #tmpMtlCertificate tm where tm.ID = md.ID)

	update md set	md.ID				=tmd.ID,
					md.SuppID			=tmd.SuppID,
					md.InvoiceNo		=tmd.InvoiceNo,
					md.FormType			=tmd.FormType,
					md.FormNo			=tmd.FormNo,
					md.TPEReceiveDate	=tmd.TPEReceiveDate,
					md.TPERemark		=tmd.Remark,
					md.Junk				=tmd.Junk,
					md.TPEAddName		=tmd.AddName,
					md.TPEAddDate		=tmd.AddDate,
					md.TPEEditName		=tmd.EditName,
					md.TPEEditDate		=tmd.EditDate
	from Production.dbo.MtlCertificate_Detail md
	inner join #tmpMtlCertificate_Detail tmd on md.ukey = tmd.ukey

	insert into Production.dbo.MtlCertificate_Detail(	Ukey		   ,
														ID			   ,
														SuppID		   ,
														InvoiceNo	   ,
														FormType	   ,
														FormNo		   ,
														TPEReceiveDate ,
														TPERemark	   ,
														Junk		   ,
														TPEAddName	   ,
														TPEAddDate	   ,
														TPEEditName	   ,
														TPEEditDate)
							select	tmd.Ukey		  ,
									tmd.ID			  ,
									tmd.SuppID		  ,
									tmd.InvoiceNo	  ,
									tmd.FormType	  ,
									tmd.FormNo		  ,
									tmd.TPEReceiveDate,
									tmd.Remark		  ,
									tmd.Junk		  ,
									tmd.AddName		  ,
									tmd.AddDate		  ,
									tmd.EditName	  ,
									tmd.EditDate
							from #tmpMtlCertificate_Detail tmd
							where not exists( select 1 from Production.dbo.MtlCertificate_Detail md with (nolock) where md.Ukey = tmd.Ukey)

drop table #tmpMtlCertificate,#tmpMtlCertificate_Detail

-----------------------FormType-----------------------------
Delete a from Production.dbo.FormType a where not exists(select 1 from Trade_To_Pms.dbo.FormType b where a.ID = b.ID)

update a set	a.Name	   = b.Name			,
				a.Remark	   = b.Remark	,
				a.Junk	   = b.Junk			,
				a.AddName	   = b.AddName	,
				a.AddDate	   = b.AddDate	,
				a.EditName   = b.EditName	,
				a.EditDate   = b.EditDate	
from Production.dbo.FormType a
inner join Trade_To_Pms.dbo.FormType b on b.ID = a.ID

insert into Production.dbo.FormType(	ID		,
										Name	,
										Remark	,
										Junk	,
										AddName	,
										AddDate	,
										EditName,
										EditDate
										)
					select	a.ID		,
							a.Name	,
							a.Remark	,
							a.Junk	,
							a.AddName	,
							a.AddDate	,
							a.EditName,
							a.EditDate
					from Trade_To_Pms.dbo.FormType a
					where not exists (select 1 from Production.dbo.FormType b where a.ID = b.ID)


END


