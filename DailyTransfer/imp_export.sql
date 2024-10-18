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

	select e.*,PrepaidFtyImportFee2 = iif(isnull(e.MainWKID08, '')<>'', MainWKID08.PrepaidFtyImportFee2, MainExportID08.PrepaidFtyImportFee2) 
	into #TExport 
	from Trade_To_Pms.dbo.Export e WITH (NOLOCK)
	outer apply(
		select PrepaidFtyImportFee2=e2.PrepaidFtyImportFee
		from Trade_To_Pms.dbo.Export e2
		where e.MainWKID08  = e2.id
	)MainWKID08 
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
		t.ScheduleID = isnull( s.ScheduleID        , '')
		, t.ScheduleDate = s.ScheduleDate
		, t.LoadDate = s.LoadDate
		, t.CloseDate = s.CloseDate
		, t.Etd = s.Etd
		, t.Eta = s.Eta
		, t.ExportCountry = isnull( s.ExportCountry, '')
		, t.ImportCountry = isnull( s.ImportCountry, '')
		, t.ExportPort = isnull( s.ExportPort      , '')
		, t.ImportPort = isnull( s.ImportPort      , '')
		, t.CYCFS = isnull( s.CYCFS                , '')
		, t.ShipModeID = isnull( s.ShipModeID      , '')
		, t.ShipmentTerm = isnull( s.ShipmentTerm  , '')
		, t.FactoryID = isnull( s.FactoryID        , '')
		, t.ShipMark = isnull( s.ShipMark          , '')
		, t.ShipMarkDesc = isnull( s.ShipMarkDesc  , '')
		, t.Consignee = isnull( s.Consignee        , '')
		, t.Handle = isnull( s.Handle              , '')
		, t.Posting = isnull( s.Posting            , 0)
		, t.Payer = isnull( s.Payer                , '')
		, t.CompanyID = isnull( s.CompanyID        , 0)
		, t.Confirm = isnull( s.Confirm            , 0)
		, t.LastEdit = s.LastEdit
		, t.Remark = isnull( s.Remark              , '')
		, t.Ecfa = isnull( s.Ecfa                  , 0)
		, t.FormStatus = isnull( s.FormStatus      , '')
		, t.Carrier = isnull( s.Carrier            , '')
		, t.Forwarder = isnull( s.Forwarder        , '')
		, t.Vessel = isnull( s.Vessel              , '')
		, t.ShipTo = isnull( s.ShipTo              , '')
		, t.Sono = isnull( s.Sono                  , '')
		, t.Blno = isnull( s.Blno                  , '')
		, t.InvNo = isnull( s.InvNo                , '')
		, t.Exchange = isnull( s.Exchange          , 0)
		, t.Packages = isnull( s.Packages          , 0)
		, t.WeightKg = isnull( s.WeightKg          , 0)
		, t.NetKg = isnull( s.NetKg                , 0)
		, t.Cbm = isnull( s.Cbm                    , 0)
		, t.CbmFor = isnull( s.CbmFor              , 0)
		, t.Takings = isnull( s.Takings            , 0)
		, t.TakingFee = isnull( s.TakingFee        , 0)
		, t.Broker = isnull( s.Broker              , '')
		, t.Insurer = isnull( s.Insurer            , '')
		, t.Trailer1 = isnull( s.Trailer1          , '')
		, t.Trailer2 = isnull( s.Trailer2          , '')
		, t.Freight = isnull( s.Freight            , 0)
		, t.Insurance = isnull( s.Insurance        , 0)
		, t.Junk = isnull( s.Junk                  , 0)
		, t.AddName = isnull( s.AddName            , '')
		, t.AddDate = s.AddDate 
		, t.EditName= iif(s.[EditDate] <= t.EditDate,t.EditName,s.[EditName])
		, t.EditDate= iif(s.[EditDate] <= t.EditDate,t.EditDate,s.[EditDate])
		, t.MainExportID = isnull( s.MainExportID                          , '')
		, t.Replacement = isnull( s.Replacement                            , 0)
		, t.Delay = isnull( s.Delay                                        , 0)
		, t.PrepaidFtyImportFee = isnull( s.PrepaidFtyImportFee            , 0)
		, t.NoImportCharges =  isnull(iif(s.PrepaidFtyImportFee2 > 0, 1 ,t.NoImportCharges), 0)
		, t.MainExportID08 = isnull( s.MainExportID08                      , '')
		, t.FormE = isnull( s.FormE                                        , 0)
		, t.SQCS = isnull( s.SQCS                                          , 0)
		, t.FtyTruckFee  = isnull( s.FtyTruckFee                           , 0)
		, t.FtyTrucker = isnull( s.FtyTrucker                              , '')
		, t.OTFee = isnull( s.OTFee                                        , 0)
		, t.CIFTerms = isnull( s.CIFTerms                                  , 0)
		, t.FtyDisburseSD = isnull( s.FtyDisburseSD                        , '')
		, t.MainWKID08 = isnull(s.MainWKID08, '')
		, t.OrderCompanyID = isnull(s.OrderCompany, '')
	  when not matched  by target then 
		insert (ID ,ScheduleID ,ScheduleDate ,LoadDate ,CloseDate ,Etd ,Eta ,ExportCountry ,ImportCountry ,ExportPort ,ImportPort 
		,CYCFS ,ShipModeID ,ShipmentTerm ,FactoryID ,ShipMark ,ShipMarkDesc ,Consignee ,Handle ,Posting ,Payer ,CompanyID 
		,Confirm ,LastEdit ,Remark ,Ecfa ,FormStatus ,Carrier ,Forwarder ,Vessel ,ShipTo ,Sono ,Blno ,InvNo ,Exchange 
		,Packages ,WeightKg ,NetKg ,Cbm ,CbmFor ,Takings ,TakingFee ,PortArrival ,DocArrival ,Broker 
		,Insurer ,Trailer1 ,Trailer2 ,Freight ,Insurance ,Junk ,AddName ,AddDate ,EditName ,EditDate,MainExportID ,Replacement ,Delay ,PrepaidFtyImportFee,NoImportCharges
		,MainExportID08 ,FormE, SQCS, FtyTruckFee, FtyTrucker, OTFee, CIFTerms,FtyDisburseSD,MainWKID08
        ,OrderCompanyID)
       VALUES
       (
              isnull(s.id ,                 ''),
              isnull(s.scheduleid ,         ''),
              s.scheduledate ,
              s.loaddate ,
              s.closedate ,
              s.etd ,
              s.eta ,
              isnull(s.exportcountry ,      ''),
              isnull(s.importcountry ,      ''),
              isnull(s.exportport ,         ''),
              isnull(s.importport ,         ''),
              isnull(s.cycfs,               ''),
              isnull(s.shipmodeid ,         ''),
              isnull(s.shipmentterm ,       ''),
              isnull(s.factoryid ,          ''),
              isnull(s.shipmark ,           ''),
              isnull(s.shipmarkdesc ,       ''),
              isnull(s.consignee ,          ''),
              isnull(s.handle ,             ''),
              isnull(s.posting ,            0),
              isnull(s.payer ,              ''),
              isnull(s.companyid ,          0),
              isnull(s.confirm ,            0),
              s.lastedit ,
              isnull(s.remark ,             ''),
              isnull(s.ecfa ,               0),
              isnull(s.formstatus,          ''),
              isnull(s.carrier ,            ''),
              isnull(s.forwarder ,          ''),
              isnull(s.vessel ,             ''),
              isnull(s.shipto ,             ''),
              isnull(s.sono ,               ''),
              isnull(s.blno ,               ''),
              isnull(s.invno ,              ''),
              isnull(s.exchange ,           0),
              isnull(s.packages ,           0),
              isnull(s.weightkg ,           0),
              isnull(s.netkg ,              0),
              isnull(s.cbm ,                0),
              isnull(s.cbmfor ,             0),
              isnull(s.takings ,            0),
              isnull(s.takingfee ,          0),
              s.portarrival ,
              s.docarrival ,
              isnull(s.broker ,             ''),
              isnull(s.insurer ,            ''),
              isnull(s.trailer1 ,           ''),
              isnull(s.trailer2 ,           ''),
              isnull(s.freight ,            0),
              isnull(s.insurance ,          0),
              isnull(s.junk ,               0),
              isnull(s.addname ,            ''),
              s.adddate ,
              isnull(s.editname ,           ''),
              s.editdate,
              isnull(s.mainexportid ,       ''),
              isnull(s.replacement ,        0),
              isnull(s.delay,               0),
              isnull(s.prepaidftyimportfee, 0),
              iif(isnull(s.prepaidftyimportfee2, 0) > 0, 1 ,0) ,
              isnull(s.mainexportid08 ,''),
              isnull(s.forme,          0),
              isnull(s.sqcs,           0),
              isnull(s.ftytruckfee,    0),
              isnull(s.ftytrucker,     ''),
              isnull(s.otfee,          0),
              isnull(s.cifterms,       0),
              isnull(s.ftydisbursesd,  ''),
              isnull(s.MainWKID08, ''),
              isnull(s.OrderCompany, '')
       )
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
		  PE2.[ID] = isnull( TE2.[ID]                        , '')
		  , PE2.[PoID] = isnull( TE2.[PoID]                  , '')
		  , PE2.[Seq1] = isnull( TE2.[Seq1]                  , '')
		  , PE2.[Seq2] = isnull( TE2.[Seq2]                  , '')
		  , PE2.[FormXReceived] =  TE2.[FormXReceived]
		  , PE2.[FormXFTYEdit] =  TE2.[FormXFTYEdit]
	      , PE2.[ExportIDOld]= isnull( TE2.[ExportIDOld]     , '')
	      , PE2.[Qty]= isnull(TE2.[Qty]                      , 0)
	      , PE2.[Foc]= isnull(TE2.[Foc]                      , 0)
	      , PE2.[Carton]= isnull(TE2.[Carton]                , '')
	      , PE2.[Confirm]= isnull(TE2.[Confirm]              , 0)
	      , PE2.[UnitId]= isnull(TE2.[UnitId]                , '')
	      , PE2.[Price]= isnull(TE2.[Price]                  , 0)
	      , PE2.[NetKg]= isnull(TE2.[NetKg]                  , 0)
	      , PE2.[WeightKg]= isnull(TE2.[WeightKg]            , 0)
	      , PE2.[Remark]= isnull(TE2.[Remark]                , '')
	      , PE2.[PayDesc]= isnull(TE2.[PayDesc]              , '')
	      , PE2.[LastEta]= TE2.[LastEta]
	      , PE2.[Refno]= isnull(TE2.[Refno]                  , '')
	      , PE2.[SuppID]= isnull(TE2.[SuppID]                , '')
	      , PE2.[Pino]= isnull(TE2.[Pino]                    , '')
	      , PE2.[Description]= isnull(TE2.[Description]      , '')
	      , PE2.[UnitOld]= isnull(TE2.[UnitOld]              , '')
	      , PE2.[PinoOld]= isnull(TE2.[PinoOld]              , '')
	      , PE2.[SuppIDOld]= isnull(TE2.[SuppIDOld]          , '')
	      , PE2.[PriceOld]= isnull(TE2.[PriceOld]            , 0)
	      , PE2.[ShipPlanID]= isnull(TE2.[ShipPlanID]        , '')
	      , PE2.[PoHandle]= isnull(TE2.[PoHandle]            , '')
	      , PE2.[PcHandle]= isnull(TE2.[PcHandle]            , '')
	      , PE2.[IsFormA]= isnull(TE2.[IsFormA]              , 0)
	      , PE2.[FormXDraftCFM]= TE2.[FormXDraftCFM]
	      , PE2.[FormXINV]= isnull(TE2.[FormXINV]            , '')
	      , PE2.[FormXEdit]= TE2.[FormXEdit]
	      , PE2.[FormXPayINV]= isnull(TE2.[FormXPayINV]      , '')
	      , PE2.[FormXType]= isnull(TE2.[FormXType]          , '')
	      , PE2.[FormXAwb]= isnull(TE2.[FormXAwb]            , '')
	      , PE2.[FormXCarrier]= isnull(TE2.[FormXCarrier]    , '')
	      , PE2.[FormXRemark]= isnull(TE2.[FormXRemark]      , '')
	      , PE2.[AddName]= isnull(TE2.[AddName]              , '')
	      , PE2.[AddDate]= TE2.[AddDate]
	      , PE2.[EditDate]= TE2.[EditDate]
	      , PE2.[EditName]= isnull(TE2.[EditName]            , '')
		  , PE2.PoType = isnull( Te2.PoType                  , '')
		  , Pe2.FabricType= isnull(Te2.FabricType            , '')
		  , pe2.[BalanceQty] = isnull( te2.[BalanceQty]      , 0)
		  , pe2.[BalanceFOC]= isnull(te2.[BalanceFOC]        , 0)
		  , pe2.[ShipPlanHandle]= isnull(te2.[ShipPlanHandle], '')
		  , pe2.currencyID=isnull((select currencyID from supp where id=te2.suppid), '')
		  , pe2.InvoiceNo = isnull(te2.InvoiceNoAP, '')
		  , pe2.SCIRefno = isnull(te2.SCIRefno, '')
		  , pe2.Duty = isnull(te2.Duty, '')
		  , pe2.DutyID = isnull(te2.DutyID, '')
		  , pe2.Export_ShareAmount_Ukey = isnull(te2.Export_ShareAmount_Ukey, 0)
		  , pe2.EarlyShipReason = isnull(te2.EarlyShipReason, '')
	  when not matched by target then
      INSERT (
        [ID],
        [PoID],
        [Seq1],
        [Seq2],
        [ExportIDOld],
        [Ukey],
        [Qty],
        [Foc],
        [Carton],
        [Confirm],
        [UnitId],
        [Price],
        [NetKg],
        [WeightKg],
        [Remark],
        [PayDesc],
        [LastEta],
        [Refno],
        [SuppID],
        [Pino],
        [Description],
        [UnitOld],
        [PinoOld],
        [SuppIDOld],
        [PriceOld],
        [ShipPlanID],
        [ShipPlanHandle],
        [PoHandle],
        [PcHandle],
        [IsFormA],
        [FormXDraftCFM],
        [FormXINV],
        [FormXReceived],
        [FormXFTYEdit],
        [FormXEdit],
        [FormXPayINV],
        [FormXType],
        [FormXAwb],
        [FormXCarrier],
        [FormXRemark],
        [AddName],
        [AddDate],
        [EditDate],
        [EditName],
        [BalanceQty],
        [BalanceFOC],
        PoType,
        FabricType,
        InvoiceNo,
        SCIRefno,
        Duty,
        DutyID,
        Export_ShareAmount_Ukey,
        EarlyShipReason
    )
    VALUES
    (
        ISNULL(TE2.ID, '')
        , ISNULL(TE2.PoID, '')
        , ISNULL(TE2.Seq1, '')
        , ISNULL(TE2.Seq2, '')
        , ISNULL(TE2.ExportIDOld, '')
        , ISNULL(TE2.Ukey, 0)
        , ISNULL(TE2.Qty, 0)
        , ISNULL(TE2.Foc, 0)
        , ISNULL(TE2.Carton, '')
        , ISNULL(TE2.Confirm, 0)
        , ISNULL(TE2.UnitID, '')
        , ISNULL(TE2.Price, 0)
        , ISNULL(TE2.NetKg, 0)
        , ISNULL(TE2.WeightKg, 0)
        , ISNULL(TE2.Remark, '')
        , ISNULL(TE2.PayDesc, '')
        , TE2.LastEta
        , ISNULL(TE2.Refno, '')
        , ISNULL(TE2.SuppID, '')
        , ISNULL(TE2.Pino, '')
        , ISNULL(TE2.Description, '')
        , ISNULL(TE2.UnitOld, '')
        , ISNULL(TE2.PinoOld, '')
        , ISNULL(TE2.SuppIDOld, '')
        , ISNULL(TE2.PriceOld, 0)
        , ISNULL(TE2.ShipPlanID, '')
        , ISNULL(TE2.ShipPlanHandle, '')
        , ISNULL(TE2.PoHandle, '')
        , ISNULL(TE2.PcHandle, '')
        , ISNULL(TE2.IsFormA, 0)
        , TE2.FormXDraftCFM
        , ISNULL(TE2.FormXINV, '')
        , TE2.FormXReceived
        , TE2.FormXFTYEdit
        , TE2.FormXEdit
        , ISNULL(TE2.FormXPayINV, '')
        , ISNULL(TE2.FormXType, '')
        , ISNULL(TE2.FormXAwb, '')
        , ISNULL(TE2.FormXCarrier, '')
        , ISNULL(TE2.FormXRemark, '')
        , ISNULL(TE2.AddName, '')
        , TE2.AddDate
        , TE2.EditDate
        , ISNULL(TE2.EditName, '')
        , ISNULL(TE2.BalanceQty, 0)
        , ISNULL(TE2.BalanceFOC, 0)
        , ISNULL(TE2.PoType, '')
        , ISNULL(TE2.FabricType, '')
        , ISNULL(TE2.InvoiceNoAP, '')
        , ISNULL(TE2.SCIRefno, '')
        , ISNULL(TE2.Duty, '')
        , ISNULL(TE2.DutyID, '')
        , ISNULL(TE2.Export_ShareAmount_Ukey, 0)
        , ISNULL(TE2.EarlyShipReason, '')
    )
    when not matched by source and PE2.id in (select id from @T)then
        delete;


					
-----------------------Export_ShipAdvice_Container-----------------------------


INSERT INTO Production.dbo.Export_ShipAdvice_Container
        (Ukey,Export_DetailUkey,ContainerType,ContainerNo,AddName,AddDate,EditName,EditDate)
SELECT isnull(ukey,              0),
       isnull(export_detailukey, 0),
       isnull(containertype,     ''),
       isnull(containerno,       ''),
       isnull(addname,           ''),
       adddate,
       isnull(editname,          ''),
       editdate
FROM   trade_to_pms.dbo.export_shipadvice_container s 
WHERE NOT EXISTS (SELECT 1 FROM Production.dbo.Export_ShipAdvice_Container WHERE Ukey = s.Ukey)

UPDATE t
SET  t.Export_DetailUkey = isnull( s.Export_DetailUkey, 0)
    ,t.ContainerType = isnull( s.ContainerType        , '')
    ,t.ContainerNo = isnull( s.ContainerNo            , '')
    ,t.AddName = isnull( s.AddName                    , '')
    ,t.AddDate =  s.AddDate
    ,t.EditName = isnull( s.EditName                  , '')
    ,t.EditDate = s.EditDate
FROM Production.dbo.Export_ShipAdvice_Container t
INNER JOIN Trade_To_Pms.dbo.Export_ShipAdvice_Container s ON t.Ukey = s.Ukey


DELETE t 
FROM Production.dbo.Export_ShipAdvice_Container t 
WHERE NOT EXISTS(SELECT 1 FROM Trade_To_Pms.dbo.Export_ShipAdvice_Container s where t.Ukey = s.Ukey)
----只刪除轉出區間內，有少的Export_DetailUkey
AND t.Export_DetailUkey IN (		
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
		 t.[Export_DetailUkey] = isnull(s.[Export_DetailUkey], 0)
		,t.[Id]				   = isnull(s.[Id]               , '')
		,t.[PoID]			   = isnull(s.[PoID]             , '')
		,t.[Seq1]			   = isnull(s.[Seq1]             , '')
		,t.[Seq2]			   = isnull(s.[Seq2]             , '')
		,t.[Carton]			   = isnull(s.[Carton]           , '')
		,t.[LotNo]			   = isnull(s.[LotNo]            , '')
		,t.[Qty]			   = isnull(s.[Qty]              , 0)
		,t.[Foc]			   = isnull(s.[Foc]              , 0)
		,t.[NetKg]			   = isnull(s.[NetKg]            , 0)
		,t.[WeightKg]		   = isnull(s.[WeightKg]         , 0)
		,t.[EditName]		   = isnull(s.[EditName]         , '')
		,t.[EditDate]		   = s.[EditDate]
	when not matched by target then 	
	insert([Export_DetailUkey],[Id],[PoID],[Seq1],[Seq2],[Carton],[LotNo],[Qty],[Foc],[NetKg],[WeightKg],[EditName],[EditDate])
       VALUES
       (
              isnull(s.[Export_DetailUkey], 0),
              isnull(s.[Id],                ''),
              isnull(s.[PoID],              ''),
              isnull(s.[Seq1],              ''),
              isnull(s.[Seq2],              ''),
              isnull(s.[Carton],            ''),
              isnull(s.[LotNo],             ''),
              isnull(s.[Qty],               0),
              isnull(s.[Foc],               0),
              isnull(s.[NetKg],             0),
              isnull(s.[WeightKg] ,         0),
              isnull(s.[EditName],          ''),
              s.[EditDate]
       )
	when not matched by source and exists (select ID from @T where id = t.id)then
	  		delete;
			
-----------------------POShippingList_Line-----------------------------
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='POShippingList';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= (Select DateStart From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)
if @DateEnd is Null
	set @DateEnd = (Select DateEnd From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);

--刪除:存在Trade_To_Pms.Export_Detail不存在Trade_To_Pms.POShippingList_Line
declare @deleteukeys table (POShippingList_Ukey bigint)
delete t
output deleted.POShippingList_Ukey into @deleteukeys
from Production.dbo.POShippingList_Line t
where not exists (select 1 from Trade_To_Pms.dbo.POShippingList_Line s where s.POShippingList_Ukey = t.POShippingList_Ukey and s.QRCode = t.QRCode and s.Line = t.Line)
and t.AddDate between @DateStart and @DateEnd

--更新:存在Trade_To_Pms.Export_Detail存在Production.POShippingList_Line
update t set
	 [RefNo]				= isnull(s.[RefNo]              , '')
	,[Description]			= isnull(s.[Description]        , '')
	,[MaterialColor]		= isnull(s.[MaterialColor]      , '')
	,[Weight]				= isnull(s.[Weight]             , '')
	,[WeightUnitID]			= isnull(s.[WeightUnitID]       , '')
	,[Width]				= isnull(s.[Width]              , '')
	,[WidthUnitID]			= isnull(s.[WidthUnitID]        , '')
	,[Length]				= isnull(s.[Length]             , '')
	,[LengthUnitID]			= isnull(s.[LengthUnitID]       , '')
	,[Height]				= isnull(s.[Height]             , '')
	,[HeightUnitID]			= isnull(s.[HeightUnitID]       , '')
	,[Thickness]			= isnull(s.[Thickness]          , '')
	,[ThicknessUnitID]		= isnull(s.[ThicknessUnitID]    , '')
	,[SizeSpec]				= isnull(s.[SizeSpec]           , '')
	,[Price]				= isnull(s.[Price]              , '')
	,[BatchNo]				= isnull(s.[BatchNo]            , '')
	,[PackageNo]			= isnull(s.[PackageNo]          , '')
	,[ShipQty]				= isnull(s.[ShipQty]            , 0)
	,[ShipQtyUnitID]		= isnull(s.[ShipQtyUnitID]      , '')
	,[FOC]					= isnull(s.[FOC]                , 0)
	,[FOCUnitID]			= isnull(s.[FOCUnitID]          , '')
	,[NW]					= isnull(s.[NW]                 , 0)
	,[NWUnitID]				= isnull(s.[NWUnitID]           , '')
	,[GW]					= isnull(s.[GW]                 , 0)
	,[GWUnitID]				= isnull(s.[GWUnitID]           , '')
	,[AdditionalOptional1]	= isnull(s.[AdditionalOptional1], '')
	,[AdditionalOptional2]	= isnull(s.[AdditionalOptional2], '')
	,[AdditionalOptional3]	= isnull(s.[AdditionalOptional3], '')
	,[AdditionalOptional4]	= isnull(s.[AdditionalOptional4], '')
	,[AdditionalOptional5]	= isnull(s.[AdditionalOptional5], '')
	,[AddName]				= isnull(s.[AddName]            , '')
	,[AddDate]				=s.[AddDate]
from Trade_To_Pms.dbo.POShippingList_Line s
inner join Production.dbo.POShippingList_Line t on s.POShippingList_Ukey = t.POShippingList_Ukey and s.QRCode = t.QRCode and s.Line = t.Line

--新增:存在Trade_To_Pms.Export_Detail不存在Production.POShippingList_Line
INSERT INTO [dbo].[POShippingList_Line]
           ([POShippingList_Ukey]
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
	 isnull(s.[POShippingList_Ukey], 0)
	,isnull(s.[QRCode]             , '')
	,isnull(s.[Line]               , '')
	,isnull(s.[RefNo]              , '')
	,isnull(s.[Description]        , '')
	,isnull(s.[MaterialColor]      , '')
	,isnull(s.[Weight]             , '')
	,isnull(s.[WeightUnitID]       , '')
	,isnull(s.[Width]              , '')
	,isnull(s.[WidthUnitID]        , '')
	,isnull(s.[Length]             , '')
	,isnull(s.[LengthUnitID]       , '')
	,isnull(s.[Height]             , '')
	,isnull(s.[HeightUnitID]       , '')
	,isnull(s.[Thickness]          , '')
	,isnull(s.[ThicknessUnitID]    , '')
	,isnull(s.[SizeSpec]           , '')
	,isnull(s.[Price]              , '')
	,isnull(s.[BatchNo]            , '')
	,isnull(s.[PackageNo]          , '')
	,isnull(s.[ShipQty]            , 0)
	,isnull(s.[ShipQtyUnitID]      , '')
	,isnull(s.[FOC]                , 0)
	,isnull(s.[FOCUnitID]          , '')
	,isnull(s.[NW]                 , 0)
	,isnull(s.[NWUnitID]           , '')
	,isnull(s.[GW]                 , 0)
	,isnull(s.[GWUnitID]           , '')
	,isnull(s.[AdditionalOptional1], '')
	,isnull(s.[AdditionalOptional2], '')
	,isnull(s.[AdditionalOptional3], '')
	,isnull(s.[AdditionalOptional4], '')
	,isnull(s.[AdditionalOptional5], '')
	,isnull(s.[AddName]            , '')
	,s.[AddDate]
from Trade_To_Pms.dbo.POShippingList_Line s
left join Production.dbo.POShippingList_Line t on s.POShippingList_Ukey = t.POShippingList_Ukey and s.QRCode = t.QRCode and s.Line = t.Line
where t.QRCode is null

-----------------------POShippingList-----------------------------
--更新: 存在表身 Production.dbo.POShippingList_Line, 表頭存在
UPDATE t set
	 [IssueDate]		= s.[IssueDate]
	,[POID]				= isnull(s.[POID]           , '')
	,[Seq1]				= isnull(s.[Seq1]           , '')
	,[T1Name]			= isnull(s.[T1Name]         , '')
	,[T1MR]				= isnull(s.[T1MR]           , '')
	,[T1FtyName]		= isnull(s.[T1FtyName]      , '')
	,[T1FtyBrandCode]	= isnull(s.[T1FtyBrandCode] , '')
	,[T2Name]			= isnull(s.[T2Name]         , '')
	,[T2SuppName]		= isnull(s.[T2SuppName]     , '')
	,[T2SuppBrandCode]	= isnull(s.[T2SuppBrandCode], '')
	,[T2SuppCountry]	= isnull(s.[T2SuppCountry]  , '')
	,[BrandID]			= isnull(s.[BrandID]        , '')
	,[CurrencyID]		= isnull(s.[CurrencyID]     , '')
	,[PackingNo]		= isnull(s.[PackingNo]      , '')
	,[PackingDate]		= s.[PackingDate]
	,[InvoiceNo]		= isnull(s.[InvoiceNo]      , '')
	,[InvoiceDate]		= s.[InvoiceDate]
	,[CloseDate]		= s.[CloseDate]
	,[Vessel]			= isnull(s.[Vessel]         , '')
	,[ETD]				= s.[ETD]
	,[FinalShipmodeID]	= isnull(s.[FinalShipmodeID], '')
	,[SuppID]			= isnull(s.[SuppID]         , '')
	,[AddName]			= isnull(s.[AddName]        , '')
	,[AddDate]			=s.[AddDate]
from Trade_To_Pms.dbo.POShippingList s
inner join Production.dbo.POShippingList t on s.Ukey = t.Ukey

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
	 isnull(s.[Ukey]           , 0)
	,s.[IssueDate]
	,isnull(s.[POID]           , '')
	,isnull(s.[Seq1]           , '')
	,isnull(s.[T1Name]         , '')
	,isnull(s.[T1MR]           , '')
	,isnull(s.[T1FtyName]      , '')
	,isnull(s.[T1FtyBrandCode] , '')
	,isnull(s.[T2Name]         , '')
	,isnull(s.[T2SuppName]     , '')
	,isnull(s.[T2SuppBrandCode], '')
	,isnull(s.[T2SuppCountry]  , '')
	,isnull(s.[BrandID]        , '')
	,isnull(s.[CurrencyID]     , '')
	,isnull(s.[PackingNo]      , '')
	,s.[PackingDate]
	,isnull(s.[InvoiceNo]      , '')
	,s.[InvoiceDate]
	,s.[CloseDate]
	,isnull(s.[Vessel]         , '')
	,s.[ETD]
	,isnull(s.[FinalShipmodeID], '')
	,isnull(s.[SuppID]         , '')
	,isnull(s.[AddName]        , '')
	,s.[AddDate]
from Trade_To_Pms.dbo.POShippingList s
left join Production.dbo.POShippingList t on s.Ukey = t.Ukey
where t.Ukey is null
and exists(select 1 from Production.dbo.POShippingList_Line where POShippingList_Ukey = s.Ukey)

--刪除: 上面做完後,表身為空,表頭刪除
delete pl
from Production.dbo.POShippingList pl
where exists(select 1 from @deleteukeys d where d.POShippingList_Ukey = pl.Ukey)
and not exists(select 1 from POShippingList_Line pll where pl.ukey = pll.POShippingList_Ukey)
 
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
			  t.Seq				= isnull(  s.Seq      , '')
			, t.Type			= isnull(  s.Type     , '')
			, t.CartonQty		= isnull(  s.CartonQty, 0)
			, t.WeightKg		= isnull(  s.WeightKg , 0)
			, t.AddName			= isnull(  s.AddName  , '')
			, t.AddDate			= s.AddDate
			, t.EditName		= isnull(  s.EditName , '')
			, t.EditDate		=  s.EditDate
	when not matched by target then 
		insert (   [ID],  [Seq],  [Type],  [Container],  [CartonQty],  [WeightKg],  [AddName],  [AddDate],  [EditName],  [EditDate])
       VALUES
       (
              isnull(s.[ID],        ''),
              isnull(s.[Seq],       ''),
              isnull(s.[Type],      ''),
              isnull(s.[Container], ''),
              isnull(s.[CartonQty], 0),
              isnull(s.[WeightKg],  0),
              isnull(s.[AddName],   ''),
              s.[AddDate],
              isnull(s.[EditName],  ''),
              s.[EditDate]
       )
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
	
	update m set	m.Consignee	 = isnull( tm.Consignee	   , ''),
				m.CartonNo		 = isnull( tm.CartonNo	   , ''),
				m.ExportID		 = isnull( tm.ExportID	   , ''),
				m.Handle		 = isnull( tm.Handle	   , ''),
				m.Mailed		 = isnull( tm.Mailed	   , 0),
				m.Junk			 = isnull( tm.Junk		   , 0),
				m.AddName		 = isnull( tm.AddName	   , ''),
				m.AddDate		 =  tm.AddDate,
				m.TPEEditName	 = isnull( tm.EditName	   , ''),
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
				select	isnull(tm.ID		   , ''),
						isnull(tm.Consignee  ,   ''),
						isnull(tm.CartonNo   ,   ''),
						isnull(tm.ExportID   ,   ''),
						isnull(tm.Handle	   , ''),
						isnull(tm.Mailed	   , 0),
						isnull(tm.Junk	   ,     0),
						isnull(tm.AddName	   , ''),
						tm.AddDate	   ,
						isnull(tm.EditName   ,   ''),
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

	update md set	md.ID				= isnull(tmd.ID,             ''),
					md.SuppID			= isnull(tmd.SuppID,         ''),
					md.InvoiceNo		= isnull(tmd.InvoiceNo,      ''),
					md.FormType			= isnull(tmd.FormType,       ''),
					md.FormNo			= isnull(tmd.FormNo,         ''),
					md.TPEReceiveDate	= tmd.TPEReceiveDate, 
					md.TPERemark		= isnull(tmd.Remark,         ''),
					md.Junk				= isnull(tmd.Junk,           0),
					md.TPEAddName		= isnull(tmd.AddName,        ''),
					md.TPEAddDate		= tmd.AddDate, 
					md.TPEEditName		= isnull(tmd.EditName,       ''),
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
							select	isnull(tmd.Ukey		  ,     0),
									isnull(tmd.ID			  , ''),
									isnull(tmd.SuppID		  , ''),
									isnull(tmd.InvoiceNo	  , ''),
									isnull(tmd.FormType	  ,     ''),
									isnull(tmd.FormNo		  , ''),
									tmd.TPEReceiveDate,
									isnull(tmd.Remark		  , ''),
									isnull(tmd.Junk		  ,     0),
									isnull(tmd.AddName		  , ''),
									tmd.AddDate		  ,
									isnull(tmd.EditName	  ,     ''),
									tmd.EditDate
							from #tmpMtlCertificate_Detail tmd
							where not exists( select 1 from Production.dbo.MtlCertificate_Detail md with (nolock) where md.Ukey = tmd.Ukey)

drop table #tmpMtlCertificate,#tmpMtlCertificate_Detail

-----------------------FormType-----------------------------
Delete a from Production.dbo.FormType a where not exists(select 1 from Trade_To_Pms.dbo.FormType b where a.ID = b.ID)

update a set	a.Name	   = isnull( b.Name			,''),
				a.Remark	   = isnull( b.Remark	,''),
				a.Junk	   = isnull( b.Junk			,0),
				a.AddName	   = isnull( b.AddName	,''),
				a.AddDate	   =  b.AddDate	,
				a.EditName   = isnull( b.EditName	,''),
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
					select	isnull(a.ID		,    ''),
							isnull(a.Name	,    ''),
							isnull(a.Remark	,    ''),
							isnull(a.Junk	,    0),
							isnull(a.AddName	,''),
							a.AddDate	,
							isnull(a.EditName,   ''),
							a.EditDate
					from Trade_To_Pms.dbo.FormType a
					where not exists (select 1 from Production.dbo.FormType b where a.ID = b.ID)


-----------------------TransferExport-----------------------------
update a set	
	a.OTFee = isnull(b.OTFee, '')
	, a.CloseDate = b.CloseDate
	, a.LoadDate = b.LoadDate
	, a.ImportPort = ISNULL(b.ImportPort     , '')
	, a.ExportPort = ISNULL(b.ExportPort     , '')
	, a.CompanyID = ISNULL(b.CompanyID       , 0)
	, a.ShipmentTerm = ISNULL(b.ShipmentTerm , '')
	, a.ShipModeID = ISNULL(b.ShipModeID     , '')
	, a.Payer = ISNULL(b.Payer               , '')
	, a.Etd = b.Etd
	, a.eta = b.eta
	, a.Handle = ISNULL(b.Handle             , '')
	, a.Forwarder = ISNULL(b.Forwarder       , '')
	, a.Vessel = ISNULL(b.Vessel             , '')
	, a.Carrier = ISNULL(b.Carrier           , '')
	, a.Blno = ISNULL(b.Blno                 , '')
	, a.Confirm = ISNULL(b.Confirm           , 0)
	, a.ConfirmTime = b.ConfirmTime
	, a.OrderCompanyID = ISNULL(b.OrderCompany, 0)
from Production.dbo.TransferExport a
inner join Trade_To_Pms.dbo.TransferExport b on b.ID = a.ID

INSERT INTO Production.dbo.TransferExport_ShipAdvice_Container
	(Ukey,TransferExport_Detail_Ukey,ContainerType,ContainerNo,AddName,AddDate,EditName,EditDate)
SELECT isnull(ukey,                       0),
       isnull(transferexport_detail_ukey, 0),
       isnull(containertype,              ''),
       isnull(containerno,                ''),
       isnull(addname,                    ''),
       adddate,
       isnull(editname,                   ''),
       editdate
FROM   trade_to_pms.dbo.transferexport_shipadvice_container s 
WHERE NOT EXISTS (SELECT 1 FROM Production.dbo.TransferExport_ShipAdvice_Container WHERE Ukey = s.Ukey)

UPDATE t
SET
	 t.TransferExport_Detail_Ukey = isnull(s.TransferExport_Detail_Ukey,0)
	,t.ContainerType			  = isnull(s.ContainerType             ,'')
	,t.ContainerNo				  = isnull(s.ContainerNo               ,'')
	,t.AddName				      = isnull(s.AddName                   ,'')
	,t.AddDate				      = s.AddDate
	,t.EditName				      = isnull(s.EditName                  ,'')
	,t.EditDate				      = s.EditDate
FROM Production.dbo.TransferExport_ShipAdvice_Container t
INNER JOIN Trade_To_Pms.dbo.TransferExport_ShipAdvice_Container s ON t.Ukey = s.Ukey

DELETE t 
FROM Production.dbo.TransferExport_ShipAdvice_Container t 
WHERE NOT EXISTS(SELECT 1 FROM Trade_To_Pms.dbo.TransferExport_ShipAdvice_Container s where t.Ukey = s.Ukey)
----只刪除轉出區間內，有少的 TransferExport
AND exists (		
	SELECT 1
	FROM Trade_To_Pms.dbo.TransferExport a
	inner join Trade_To_Pms.dbo.TransferExport_Detail b on a.ID = b.ID
	WHERE b.Ukey = t.TransferExport_Detail_Ukey	
)
END


