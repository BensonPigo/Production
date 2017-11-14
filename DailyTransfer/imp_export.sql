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


	---create temp table---------

	select * into #TExport from Trade_To_Pms.dbo.Export WITH (NOLOCK)
	where FactoryID in (select id from @Sayfty)

	update TE1
	set TE1.factoryID = PF.FTYGroup
	from #TExport TE1
	inner join Production.dbo.Factory PF on pf.ID = TE1.FactoryID
	

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
	  when not matched  by target then 
		insert (ID ,ScheduleID ,ScheduleDate ,LoadDate ,CloseDate ,Etd ,Eta ,ExportCountry ,ImportCountry ,ExportPort ,ImportPort 
		,CYCFS ,ShipModeID ,ShipmentTerm ,FactoryID ,ShipMark ,ShipMarkDesc ,Consignee ,Handle ,Posting ,Payer ,CompanyID 
		,Confirm ,LastEdit ,Remark ,Ecfa ,FormStatus ,Carrier ,Forwarder ,Vessel ,ShipTo ,Sono ,Blno ,InvNo ,Exchange 
		,Packages ,WeightKg ,NetKg ,Cbm ,CbmFor ,Takings ,TakingFee ,PortArrival ,DocArrival ,Broker 
		,Insurer ,Trailer1 ,Trailer2 ,Freight ,Insurance ,Junk ,AddName ,AddDate ,EditName ,EditDate)
	    values( 
		s.ID ,s.ScheduleID ,s.ScheduleDate ,s.LoadDate ,s.CloseDate ,s.Etd ,s.Eta ,s.ExportCountry ,s.ImportCountry ,s.ExportPort ,s.ImportPort 
		,s.CYCFS,s.ShipModeID ,s.ShipmentTerm ,s.FactoryID ,s.ShipMark ,s.ShipMarkDesc ,s.Consignee ,s.Handle ,s.Posting ,s.Payer ,s.CompanyID 
		,s.Confirm ,s.LastEdit ,s.Remark ,s.Ecfa ,s.FormStatus,s.Carrier ,s.Forwarder ,s.Vessel ,s.ShipTo ,s.Sono ,s.Blno ,s.InvNo ,s.Exchange 
		,s.Packages ,s.WeightKg ,s.NetKg ,s.Cbm ,s.CbmFor ,s.Takings ,s.TakingFee ,s.PortArrival ,s.DocArrival ,s.Broker 
		,s.Insurer ,s.Trailer1 ,s.Trailer2 ,s.Freight ,s.Insurance ,s.Junk ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate)
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
	  when not matched by target then 
		insert (    [ID],    [PoID],    [Seq1],    [Seq2],    [ExportIDOld],    [Ukey],    [Qty],    [Foc],    [Carton],    [Confirm],    [UnitId],    [Price],    [NetKg],    [WeightKg],    [Remark],    [PayDesc],    [LastEta],    [Refno],    [SuppID],    [Pino],    [Description],    [UnitOld],    [PinoOld],    [SuppIDOld],    [PriceOld],    [ShipPlanID],    [ShipPlanHandle],    [PoHandle],    [PcHandle],    [IsFormA],    [FormXDraftCFM],    [FormXINV],    [FormXReceived],    [FormXFTYEdit],    [FormXEdit],    [FormXPayINV],    [FormXType],    [FormXAwb],    [FormXCarrier],    [FormXRemark],    [AddName],    [AddDate],    [EditDate],    [EditName],    [BalanceQty],    [BalanceFOC],    PoType,    FabricType)
		values (TE2.[ID],TE2.[PoID],TE2.[Seq1],TE2.[Seq2],TE2.[ExportIDOld],TE2.[Ukey],TE2.[Qty],TE2.[Foc],TE2.[Carton],TE2.[Confirm],TE2.[UnitID],TE2.[Price],TE2.[NetKg],TE2.[WeightKg],TE2.[Remark],TE2.[PayDesc],TE2.[LastEta],TE2.[Refno],TE2.[SuppID],TE2.[Pino],TE2.[Description],TE2.[UnitOld],TE2.[PinoOld],TE2.[SuppIDOld],TE2.[PriceOld],TE2.[ShipPlanID],TE2.[ShipPlanHandle],TE2.[PoHandle],TE2.[PcHandle],TE2.[IsFormA],TE2.[FormXDraftCFM],TE2.[FormXINV],TE2.[FormXReceived],TE2.[FormXFTYEdit],TE2.[FormXEdit],TE2.[FormXPayINV],TE2.[FormXType],TE2.[FormXAwb],TE2.[FormXCarrier],TE2.[FormXRemark],TE2.[AddName],TE2.[AddDate],TE2.[EditDate],TE2.[EditName],TE2.[BalanceQty],TE2.[BalanceFOC],Te2.PoType,Te2.FabricType)
	  when not matched by source and PE2.id in (select id from @T)then
	  	delete;


drop table #TExport;

END


