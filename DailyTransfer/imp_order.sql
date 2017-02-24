-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/25>
-- Description:	<import order>
-- =============================================
Create PROCEDURE [dbo].[imp_Order]
AS
BEGIN

	SET NOCOUNT ON;

	declare @OldDate date = (select max(UpdateDate) from Production.dbo.OrderComparisonList WITH (NOLOCK)) --�̫�פJ��Ƥ��
	declare @dToDay date = CONVERT(date, GETDATE()) --���Ѥ��
-----------------�פJ�q���ˮ֪�------------------------
	delete from Production.dbo.OrderComparisonList
	where ISNULL(UpdateDate,'')='' and isnull(OrderId,'')='' and isnull(FactoryID,'')=''

------------------TempTable -------------
	--TempOrder
	select a.*,
	CAST( '' as varchar(8)) as FTY_Group,
	cast (null as date) as SDPDate,
	cast(0 as bit) as PulloutComplete,
	cast('' as varchar(10) ) as MCHandle,
	cast(null as date) as MDClose
	into #TOrder
	from Trade_To_Pms.dbo.Orders a WITH (NOLOCK)
	inner join Production.dbo.Factory b WITH (NOLOCK) on a.FactoryID=b.ID

	update #TOrder
	set FTY_Group =IIF(b.FTYGroup is null,a.FactoryID,b.FTYGroup) , MDivisionID=b.MDivisionID
	from #TOrder a
	inner join Production.dbo.Factory b on a.FactoryID=b.id

	
-------------------------------------------------------------------------Order
	Merge Production.dbo.OrderComparisonList as t
	Using (select c.*, a.StyleID as AStyleID,a.FactoryID as AFactory 
		from #TOrder a
		inner join Production.dbo.Orders c WITH (NOLOCK) on a.id=c.id
		where a.qty > 0 and a.IsForecast = '0'
	) as s
	on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
	when matched then
		update set
		t.TransferDate = @OldDate,
		t.OriginalQty=s.qty,
		t.OriginalBuyerDelivery=s.BuyerDelivery,
		t.DeleteOrder=1,
		t.OriginalStyleID=s.AStyleID,
		t.TransferToFactory =iif(s.AFactory is not null and s.AFactory in (select id from Production.dbo.Factory WITH (NOLOCK)),s.AFactory,t.TransferToFactory)		
	when not matched by target then
		insert(OrderId,UpdateDate,FactoryID)
		values(s.id,@dToDay,s.FactoryID);
	
		--��欰Cutting�����,�мgCutPlan���l�檺�u�t���
		Update a
		set a.MDivisionid = b.FTY_Group
		from Production.dbo.Cutplan a
		inner join #TOrder b on a.ID=b.ID
		inner join Production.dbo.Orders c on b.ID=c.ID
		where b.qty > 0 and b.IsForecast = '0'

	
		----delete  SewingSchedule_Detail
		delete c
		from #TOrder a 
		inner join Production.dbo.SewingSchedule b on a.id=b.orderid and b.FactoryID<>a.FTY_Group
		inner join Production.dbo.SewingSchedule_Detail c on c.ID=b.ID
		where a.qty > 0 and a.IsForecast = '0'
		
		--delete SewingSchedule
		delete b
		from #TOrder a 		
		inner join Production.dbo.SewingSchedule b on a.id=b.OrderID and b.FactoryID<>a.FTY_Group
		where a.qty > 0 and a.IsForecast = '0'

		--delete cutting
		delete b
		from #TOrder a 		
		inner join Production.dbo.Cutting b on a.id=b.ID and b.FactoryID<>a.FTY_Group
		where a.qty > 0 and a.IsForecast = '0'

		--delete order 
		delete b
		from #TOrder a 		
		inner join Production.dbo.Orders b on a.id=b.id and b.FtyGroup <> a.FTY_Group
		where a.qty > 0 and a.IsForecast = '0'
			
		--OrderComparisonList ��� production.order
		Merge Production.dbo.OrderComparisonList as t
		Using (select b.id,b.qty,b.FactoryID,b.BuyerDelivery,
		 a.StyleID as AStyleID,a.FactoryID as AFactory 
		from #TOrder a		
		inner join Production.dbo.Orders b WITH (NOLOCK) on a.id=b.id and a.factoryid <> b.factoryid
		where a.qty > 0 and a.IsForecast = '0'
		) as s
		on t.orderid=s.id and t.factoryid<>s.factoryid and t.updateDate = @dToDay
		when matched then
			update set 
			t.TransferDate=@OldDate,
			t.OriginalQty=s.qty,
			t.OriginalBuyerDelivery=s.BuyerDelivery,
			t.DeleteOrder=1,
			t.OriginalStyleID=s.AStyleID,
			t.TransferToFactory =iif(s.AFactory is not null and s.AFactory in (select id from Production.dbo.Factory WITH (NOLOCK)),s.AFactory,t.TransferToFactory)
		when not matched by target then 
		insert(OrderId,UpdateDate,FactoryID,TransferDate,OriginalQty,OriginalBuyerDelivery,DeleteOrder,OriginalStyleID,
		TransferToFactory)
		values(s.id,@dToDay,s.FactoryID,@OldDate,s.qty,s.BuyerDelivery,1,s.AStyleID,
		iif(s.AFactory is not null and s.AFactory in (select id from Production.dbo.Factory WITH (NOLOCK)),s.AFactory,''));

		

		----OrderComparisonList ��� Trade_To_Pms.order
		Merge Production.dbo.OrderComparisonList as t
		Using (select a.id,a.FactoryID,a.qty,a.BuyerDelivery,a.SciDelivery,a.styleid,a.KPILETA,a.LETA,a.CMPQDate
		,a.EachConsApv,a.MnorderApv,a.SMnorderApv,a.MnorderApv2
		, b.StyleID as AStyleID,b.FactoryID as AFactory 
		from #TOrder a		
		inner join Production.dbo.Orders b WITH (NOLOCK) on a.id=b.id and a.factoryid <> b.factoryid
		where a.qty > 0 and a.IsForecast = '0'
		) as s
		on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
		when matched then
			update set 
			t.NewQty=s.qty,
			t.NewBuyerDelivery=s.BuyerDelivery,
			t.NewSCIDelivery=s.SciDelivery,
			t.OriginalStyleID=s.styleid,
			t.KPILETA=s.KPILETA,
			t.NewLETA = s.LETA,
			t.TransferDate=@OldDate,
			t.NewOrder=1,
			t.NewCMPQDate=s.CMPQDate,
			t.NewEachConsApv=s.EachConsApv,
			t.OriginalMnorderApv=s.MnorderApv,
			t.OriginalSMnorderApv = s.SMnorderApv,
			t.MnorderApv2=s.MnorderApv2						
		when not matched by target then 
		insert(OrderId,UpdateDate,FactoryID,NewQty,NewBuyerDelivery,NewSCIDelivery,OriginalStyleID,KPILETA,NewLETA,	TransferDate,NewOrder,NewCMPQDate,NewEachConsApv,OriginalMnorderApv,OriginalSMnorderApv,MnorderApv2)
		values(s.id,@dToDay,s.FactoryID,s.qty,s.BuyerDelivery,s.SciDelivery,styleid,KPILETA,LETA,@OldDate,1,CMPQDate,EachConsApv,MnorderApv,SMnorderApv,MnorderApv2);

	--�ݶ�J Order.SDPDate = Buyer Delivery - �񰲤�(�����)--
		--�p�G�R�a��f�餣�O�u�t�񰲤�,SDDate=BuyerDelivery
		update #TOrder
		set SDPDate = BuyerDelivery	

		--�p�G�R�a��f���n�J�찲��,SDDate�N���e�@��
		update a
		set a.SDPDate = DATEADD(day,-1, a.BuyerDelivery) 
		from #TOrder  a
		inner join Production.dbo.Factory b on a.FactoryID=b.ID
		inner join Production.dbo.Holiday c on b.id=c.factoryid and c.HolidayDate=a.BuyerDelivery

		
		-- �վ�#TOrder not matched
		update t
		set t.MCHandle = (select localMR from Production.dbo.Style where BrandID=t.BrandID and id=t.styleid and SeasonID=t.SeasonID ),
		t.PulloutComplete =iif((t.GMTComplete='P' OR t.GMTComplete='' or t.GMTComplete is null)   ,0,1),
		t.MDClose = iif((t.GMTComplete='P' OR t.GMTComplete='' or t.GMTComplete is null) ,t.MDClose,convert(date,getdate()))                                                                                             
		from #TOrder as t
		left join Production.dbo.Orders as s1 on t.ID=s1.ID
		where s1.ID is null
		
		
---------------------Order--------------------------------------
		--------------Order.id= AOrder.id  if eof()
		declare @OrderT table (ID varchar(13),isInsert bit) 

		Merge Production.dbo.Orders as t
		Using   #TOrder as s
		on t.id=s.id
		when matched then 
		update set
				t.ProgramID = s.    ProgramID ,
				t.ProjectID = s.    ProjectID ,
				t.Category = s.    Category ,
				t.OrderTypeID = s.    OrderTypeID ,
				t.BuyMonth = s.    BuyMonth ,
				t.Dest = s.       Dest ,
				t.Model = s.       Model ,
				t.HsCode1 = s.       HsCode1 ,
				t.HsCode2 = s.       HsCode2 ,
				t.PayTermARID = s.       PayTermARID ,
				t.ShipTermID = s.       ShipTermID ,
				t.ShipModeList = s.       ShipModeList ,
				t.PoPrice = s.       PoPrice ,
				t.CFMPrice = s.       CFMPrice ,
				t.CurrencyID = s.       CurrencyID ,
				t.Commission = s.       Commission ,
				t.BrandAreaCode = s.       BrandAreaCode ,
				t.BrandFTYCode = s.       BrandFTYCode ,
				t.CTNQty = s.       CTNQty ,
				t.CustCDID = s.       CustCDID ,
				t.CustPONo = s.       CustPONo ,
				t.Customize1 = s.       Customize1 ,
				t.Customize2 = s.       Customize2 ,
				t.Customize3 = s.       Customize3 ,
				t.CMPUnit = s.       CMPUnit ,
				t.CMPPrice = s.       CMPPrice ,
				t.CMPQDate = s.       CMPQDate ,
				t.CMPQRemark = s.       CMPQRemark ,
				t.EachConsApv = s.       EachConsApv ,
				t.MnorderApv = s.       MnorderApv ,
				t.CRDDate = s.       CRDDate ,
				t.InitialPlanDate = s.       InitialPlanDate ,
				t.PlanDate = s.       PlanDate ,
				t.FirstProduction = s.       FirstProduction ,
				t.FirstProductionLock = s.       FirstProductionLock ,
				t.OrigBuyerDelivery = s.       OrigBuyerDelivery ,
				t.ExCountry = s.       ExCountry ,
				t.InDCDate = s.       InDCDate ,
				t.CFMShipment = s.       CFMShipment ,
				t.PFETA = s.       PFETA ,
				t.PackLETA = s.       PackLETA ,
				t.LETA = s.       LETA ,
				t.MRHandle = s.       MRHandle ,
				t.SMR = s.       SMR ,
				t.ScanAndPack = s.       ScanAndPack ,
				t.VasShas = s.       VasShas ,
				t.SpecialCust = s.       SpecialCust ,
				t.TissuePaper = s.       TissuePaper ,
				t.Packing = s.       Packing ,
				--t.SDPDate = s.SDPDate, --�u�t����u�ݭnINSERT��w�]��,����UPDATE
				t.MarkFront = s.       MarkFront ,
				t.MarkBack = s.       MarkBack ,
				t.MarkLeft = s.       MarkLeft ,
				t.MarkRight = s.       MarkRight ,
				t.Label = s.       Label ,
				t.OrderRemark = s.       OrderRemark ,
				t.ArtWorkCost = s.       ArtWorkCost ,
				t.StdCost = s.       StdCost ,
				t.CtnType = s.       CtnType ,
				t.FOCQty = s.       FOCQty ,
				t.SMnorderApv = s.       SMnorderApv ,
				t.FOC = s.       FOC ,
				t.MnorderApv2 = s.       MnorderApv2 ,
				t.Packing2 = s.       Packing2 ,
				t.SampleReason = s.       SampleReason ,
				t.RainwearTestPassed = s.       RainwearTestPassed ,
				t.SizeRange = s.       SizeRange ,
				t.MTLComplete = s.       MTLComplete ,
				t.SpecialMark = s.       SpecialMark ,
				t.OutstandingRemark = iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingRemark,s.OutstandingRemark),
				t.OutstandingInCharge = iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingInCharge,s.OutstandingInCharge),
				t.OutstandingDate =  iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingDate,s.OutstandingDate),
				t.OutstandingReason = iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingReason,s.OutstandingReason),
				t.StyleUkey = s.       StyleUkey ,
				t.POID = s.       POID ,
				t.IsNotRepeatOrMapping = s.       IsNotRepeatOrMapping ,
				t.SplitOrderId = s.       SplitOrderId ,
				t.FtyKPI = s.       FtyKPI ,
				t.EditName = iif((s.EditDate <= t.EditDate AND s.EditDate is null) OR (s.EditDate is not null AND s.EditDate <= t.EditDate),t.EditName, s.EditName) ,
				t.EditDate = iif((s.EditDate <= t.EditDate AND s.EditDate is null) OR (s.EditDate is not null AND s.EditDate <= t.EditDate),t.EditDate, s.EditDate) ,
				t.IsForecast = s.       IsForecast ,
				t.PulloutComplete =iif((s.GMTComplete='C' OR s.GMTComplete='S') and t.PulloutComplete=0  ,1,t.PulloutComplete),
				t.PFOrder = s.       PFOrder ,
				t.KPILETA = s.       KPILETA ,
				t.MTLETA = s.       MTLETA ,
				t.SewETA = s.       SewETA ,
				t.PackETA = s.       PackETA ,
				t.MTLExport = s.       MTLExport ,
				t.DoxType = s.       DoxType ,
				t.FtyGroup = s.       FTY_Group ,
				t.MDivisionID = s.       MDivisionID ,
				t.KPIChangeReason = s.       KPIChangeReason ,
				t.MDClose = iif((s.GMTComplete='C' OR s.GMTComplete='S') and t.PulloutComplete=0  ,@dToDay,t.MDClose),
				t.CPUFactor = s.       CPUFactor ,
				t.SizeUnit = s.       SizeUnit ,
				t.CuttingSP = s.       CuttingSP ,
				t.IsMixMarker = s.       IsMixMarker ,
				t.EachConsSource = s.       EachConsSource ,
				t.KPIEachConsApprove = s.       KPIEachConsApprove ,
				t.KPICmpq = s.       KPICmpq ,
				t.KPIMNotice = s.       KPIMNotice ,
				t.GMTComplete  = s.       GMTComplete  ,
				t.GFR = s.       GFR	,
				t.FactoryID=s.FactoryID,
				t.BrandID=s.BrandID, 
				t.StyleID=s.StyleID, 
				t.SeasonID=s.SeasonID, 
				t.BuyerDelivery=s.BuyerDelivery,
				t. SciDelivery=s.SciDelivery, 
				t.CFMDate=s.CFMDate, 
				t.Junk=s.Junk,
				t.CdCodeID=s.CdCodeID, 
				t.CPU=s.CPU, 
				t.Qty=s.Qty, 
				t.StyleUnit=s.StyleUnit, 				
				t.AddName=s.AddName, 
				t.AddDate=s.AddDate,
				t.PulloutDate=s.PulloutDate,
				t.InspDate=s.InspDate
		when not matched by target then
insert(ID,	 BrandID,   ProgramID,   StyleID,   SeasonID,   ProjectID,     Category,   OrderTypeID,   
BuyMonth,   Dest,   Model,   HsCode1,   HsCode2,   PayTermARID,   ShipTermID,   ShipModeList,   CdCodeID,   CPU,   Qty,   StyleUnit,   PoPrice,   CFMPrice,   CurrencyID,   Commission,   FactoryID,   BrandAreaCode,   BrandFTYCode,   CTNQty,   CustCDID,   CustPONo,   Customize1,   Customize2,   Customize3,   CFMDate,   BuyerDelivery,   SciDelivery,   SewOffLine,   CutInLine,   CutOffLine,   PulloutDate,   CMPUnit,   CMPPrice,   CMPQDate,  CMPQRemark,    EachConsApv,   MnorderApv,   CRDDate,   InitialPlanDate,   PlanDate,   FirstProduction,   FirstProductionLock,   OrigBuyerDelivery,   ExCountry,   InDCDate,   CFMShipment,   PFETA,   PackLETA,   LETA,   MRHandle,   SMR,   ScanAndPack,   VasShas,   SpecialCust,   TissuePaper,   Junk,   Packing,   MarkFront,   MarkBack,   MarkLeft,   MarkRight,   Label,   OrderRemark,   ArtWorkCost,   StdCost,   CtnType,   FOCQty,   SMnorderApv,   FOC,   MnorderApv2,   Packing2,   SampleReason,   RainwearTestPassed,   SizeRange,   MTLComplete,   SpecialMark,   OutstandingRemark,   OutstandingInCharge,   OutstandingDate,   OutstandingReason,   StyleUkey,   POID,   IsNotRepeatOrMapping,   SplitOrderId,   FtyKPI,   AddName,   AddDate,   EditName,   EditDate,   IsForecast,GMTComplete,      PFOrder,    InspDate,   KPILETA,   MTLETA,   SewETA,   PackETA,   MTLExport,   DoxType,   FtyGroup,    MDivisionID,   MCHandle,     KPIChangeReason,  MDClose,   CPUFactor,   SizeUnit,   CuttingSP,   IsMixMarker,   EachConsSource,   KPIEachConsApprove,   KPICmpq,   KPIMNotice,   GFR,SDPDate ,PulloutComplete,SewINLINE)
values(s.ID ,s.BrandID ,s.ProgramID ,s.StyleID ,s.SeasonID ,s.ProjectID ,s.Category ,s.OrderTypeID ,
s.BuyMonth ,s.Dest ,s.Model ,s.HsCode1 ,s.HsCode2 ,s.PayTermARID ,s.ShipTermID ,s.ShipModeList ,s.CdCodeID ,s.CPU ,s.Qty ,s.StyleUnit ,s.PoPrice ,s.CFMPrice ,s.CurrencyID ,s.Commission ,s.FactoryID ,s.BrandAreaCode ,s.BrandFTYCode ,s.CTNQty ,s.CustCDID ,s.CustPONo ,s.Customize1 ,s.Customize2 ,s.Customize3 ,s.CFMDate ,s.BuyerDelivery ,s.SciDelivery ,s.SewOffLine ,s.CutInLine ,s.CutOffLine ,s.PulloutDate ,s.CMPUnit ,s.CMPPrice ,s.CMPQDate ,s.CMPQRemark ,s.EachConsApv ,s.MnorderApv ,s.CRDDate ,s.InitialPlanDate ,s.PlanDate ,s.FirstProduction ,s.FirstProductionLock ,s.OrigBuyerDelivery ,s.ExCountry ,s.InDCDate ,s.CFMShipment ,s.PFETA ,s.PackLETA ,s.LETA ,s.MRHandle ,s.SMR ,s.ScanAndPack ,s.VasShas ,s.SpecialCust ,s.TissuePaper ,s.Junk ,s.Packing ,s.MarkFront ,s.MarkBack ,s.MarkLeft ,s.MarkRight ,s.Label ,s.OrderRemark ,s.ArtWorkCost ,s.StdCost ,s.CtnType ,s.FOCQty ,s.SMnorderApv ,s.FOC ,s.MnorderApv2 ,s.Packing2 ,s.SampleReason ,s.RainwearTestPassed ,s.SizeRange ,s.MTLComplete ,s.SpecialMark ,s.OutstandingRemark ,s.OutstandingInCharge ,s.OutstandingDate ,s.OutstandingReason ,s.StyleUkey ,s.POID ,s.IsNotRepeatOrMapping ,s.SplitOrderId ,s.FtyKPI ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate ,s.IsForecast,s.GMTComplete ,s.PFOrder ,s.InspDate ,s.KPILETA ,s.MTLETA ,s.SewETA ,s.PackETA ,s.MTLExport ,s.DoxType ,s.FTY_Group ,s.MDivisionID ,S.MCHandle ,s.KPIChangeReason , S.MDClose ,s.CPUFactor ,s.SizeUnit ,s.CuttingSP ,s.IsMixMarker ,s.EachConsSource ,s.KPIEachConsApprove ,s.KPICmpq ,s.KPIMNotice ,s.GFR,s.SDPDate ,s.PulloutComplete,s.SewINLINE)
		output inserted.id, iif(deleted.id is null,1,0) into @OrderT; --�Ninsert =1 , update =0 ����ܹL��id output;

		-----------TrsOrder AOrder<>Order ----------------
		Merge Production.dbo.OrderComparisonList as t
		Using ( select * from #Torder where id in (select id from @OrderT where isInsert=1)) as s
		on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
		when matched then
			update set
			t.NewQty = s.Qty,
			t.NewBuyerDelivery = s.BuyerDelivery, 
			t.NewSCIDelivery =s.SciDelivery, 
			t.OriginalStyleID = s.StyleID, 
			t.KPILETA =s.KPILETA, 
			t.NewLETA =s.LETA,
			t.TransferDate = @OldDate, 
			t.NewOrder =1,
			t.NewCMPQDate = s.CMPQDate, 
			t.NewEachConsApv = s.EachConsApv,
			t.OriginalMnorderApv =s.MnorderApv, 
			t.OriginalSMnorderApv = s.SMnorderApv,
			t.MnorderApv2 = s.MnorderApv2
		when not matched by target then 
			insert(OrderId,UpdateDate,FactoryID)
			values(s.Id,@dToDay,s.FactoryID);

		-----------TrsOrder AOrder=Order ----------------
		Merge Production.dbo.OrderComparisonList as t
		Using ( select A.ID,A.FactoryID,
a.qty ,B.Qty AS Aqty,iif(a.qty<>b.qty,1,0) as diffQty,
A.BuyerDelivery,b.BuyerDelivery as ABuyerDelivery,iif(a.BuyerDelivery<>b.BuyerDelivery,1,0) as diffDiv,
A.SciDelivery,b.SciDelivery as ASciDelivery,iif(a.SciDelivery <> b.SciDelivery,1,0) as diffSciD, 
A.CMPQDate,b.CMPQDate as ACMPQDate,iif(a.CMPQDate <> b.CMPQDate,1,0) as diffCmpD, 
A.EachConsApv,b.EachConsApv as ACutDate,iif(a.EachConsApv <> b.EachConsApv ,1,0) as diffCutD, 
A.MnorderApv,b.MnorderApv as AMnorderApv,iif(a.MnorderApv <> b.MnorderApv,1,0) as diffMnorderApv, 
A.SMnorderApv,b.SMnorderApv as ASMnorderApv,iif(a.SMnorderApv <> b.SMnorderApv,1,0) as diffSMnorderApv, 
A.MnorderApv2,b.MnorderApv2 as AMnorderApv2,iif(a.MnorderApv2 <> b.MnorderApv2,1,0) as diffMnorderApv2, 
A.Junk,b.Junk as AJunk,iif(a.Junk <> b.Junk,1,0) as diffJunk,
A.KPILETA,b.LETA as AKPILETA,iif(a.KPILETA <> b.KPILETA,1,0) as diffKPILETA,
A.LETA, b.LETA as ALETA , IIF( A.LETA <> B.LETA,1,0) AS diffLETA,		
a.StyleID,b.StyleID as AStyleID ,IIF(a.StyleID<> b.StyleID ,1,0) as diffStyleID
		from Production.dbo.Orders a WITH (NOLOCK)
		inner join #Torder b on a.id=b.id
		where a.id in (select id from @OrderT where isInsert=0)
			and (A.QTY <> B.QTY OR 
			A.BuyerDelivery <> B.BuyerDelivery OR 
			A.StyleID <> B.StyleID OR 
			A.EachConsApv<>B.EachConsApv OR 
			A.CMPQDate<>B.CMPQDate OR 
			A.SciDelivery<>B.SciDelivery OR 
			A.MnorderApv<>B.MnorderApv OR 
			A.SMnorderApv<>B.SMnorderApv OR 
			A.MnorderApv2<>B.MnorderApv2 OR 
			A.Junk<>B.Junk OR 
			(A.KPILETA IS NOT NULL
			AND A.LETA <> B.LETA))
		) as s
		on t.orderid=s.id and t.updateDate = @dToDay  -- CHECK
			when matched then
				update set
				t.OriginalQty =iif(diffQty=1,s.qty,t.OriginalQty),
				t.NewQty = iif(diffQty=1,s.Aqty,t.NewQty),
				t.OriginalBuyerDelivery = iif(diffDiv=1 or diffSciD=1,s.BuyerDelivery,t.OriginalBuyerDelivery),
				t.NewBuyerDelivery = iif(diffDiv=1 or diffSciD=1,s.ABuyerDelivery,t.NewBuyerDelivery),
				t.OriginalSCIDelivery=iif(diffSciD=1,s.SciDelivery,t.OriginalSCIDelivery),
				t.NewSCIDelivery = iif(diffSciD=1,s.ASciDelivery,t.NewSCIDelivery),
				t.OriginalStyleID = s.AStyleID,
				t.OriginalCMPQDate = iif(diffCmpD=1,s.CMPQDate,t.OriginalCMPQDate),
				t.NewCMPQDate = iif(diffCmpD=1,s.ACMPQDate,t.NewCMPQDate),
				t.OriginalEachConsApv = iif(diffCutD=1, s.EachConsApv,t.OriginalEachConsApv),
				t.NewEachConsApv = iif(diffCutD=1,s.ACutDate,t.NewEachConsApv),
				t.OriginalMnorderApv = iif(diffMnorderApv=1,s.MnorderApv,t.OriginalMnorderApv),
				t.NewMnorderApv = iif(diffMnorderApv=1,s.AMnorderApv,t.NewMnorderApv),
				t.OriginalSMnorderApv = iif(diffSMnorderApv=1,s.SMnorderApv,t.OriginalSMnorderApv),
				t.NewSMnorderApv = iif(diffSMnorderApv=1,s.ASMnorderApv,t.NewSMnorderApv),
				t.MnorderApv2 = iif(diffMnorderApv2=1,s.AMnorderApv2,t.MnorderApv2),
				t.JunkOrder = iif(diffJunk=1,s.AJunk,t.JunkOrder),
				t.KPILETA = iif((diffKPILETA=1 and s.KPILETA is not null),s.KPILETA,t.KPILETA),
				t.OriginalLETA= iif((diffKPILETA=1 and s.KPILETA is not null),s.LETA,t.OriginalLETA),
				t.NewLETA=iif((diffKPILETA=1 and s.KPILETA is not null),s.ALETA,t.NewLETA)
			when not matched by target then 
				insert(orderid,UpdateDate,TransferDate, factoryid)
				values(s.Id,   @dToDay,   @OldDate,   s.FactoryID);				



	--------------Order_Qty--------------------------Qty BreakDown

	Merge Production.dbo.Order_Qty as t
	Using (select a.* from Trade_To_Pms.dbo.order_qty a WITH (NOLOCK) inner join #Torder  b on a.id=b.id) as s
	on t.id=s.id AND T.ARTICLE=S.ARTICLE AND t.sizeCode=s.sizeCode
	when matched then
		update set
		t.Qty = s.		 Qty ,
		t.AddName = s.	     AddName ,
		t.AddDate = s.	     AddDate ,
		t.EditName = s.	      EditName ,
		t.EditDate = s.	      EditDate ,
		t.OriQty = s.OriQty
	when not matched by target then 
		insert(ID ,Article ,SizeCode ,Qty ,AddName ,AddDate ,EditName ,EditDate,OriQty )
		values(s.ID ,s.Article ,s.SizeCode ,s.Qty ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate,s.OriQty )
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then
	delete;

	----------Order_QtyShip--------------
	Merge Production.dbo.Order_QtyShip as t
	using (select a.* from Trade_To_Pms.dbo.Order_QtyShip a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
	on t.id=s.id and t.seq=s.seq
		when matched then 
		update set
		t.ShipmodeID = s.       ShipmodeID ,
		t.BuyerDelivery = s.       BuyerDelivery ,
		t.FtyKPI = s.       FtyKPI ,
		t.ReasonID = s.       ReasonID ,
		t.Qty = s.       Qty ,
		t.AddName = s.       AddName ,
		t.AddDate = s.       AddDate ,
		t.EditName = s.       EditName ,
		t.EditDate = s.       EditDate,
		t.OriQty = s.OriQty
	when not matched by target then
		insert(  Id ,  Seq ,  ShipmodeID ,  BuyerDelivery ,  FtyKPI ,  ReasonID ,  Qty ,  AddName ,  AddDate ,  EditName ,  EditDate,OriQty )
		values(s.Id ,s.Seq ,s.ShipmodeID ,s.BuyerDelivery ,s.FtyKPI ,s.ReasonID ,s.Qty ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate ,s.OriQty)
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;

	-----------Order_QtyShip_Detail--------------------------�վ�: �ӷ����Production���Y��� 
		Merge Production.dbo.Order_QtyShip_detail as t
		Using (select a.* from Trade_To_Pms.dbo.Order_QtyShip_detail as a WITH (NOLOCK) inner join #TOrder b on a.id=b.id  ) as s
		on t.ukey=s.ukey
		when matched then
			update set
			t.id=s.id,
			t.Seq = s.Seq ,
			t.Article = s.Article ,
			t.SizeCode = s.SizeCode ,
			t.Qty = s.Qty ,
			t.AddName = s.AddName ,
			t.AddDate = s.AddDate ,
			t.EditName = s.EditName ,
			t.EditDate = s.EditDate ,
			t.OriQty=s.OriQty
		when not matched by target then 
			insert (Id ,Seq ,Article ,SizeCode ,Qty ,AddName ,AddDate ,EditName ,EditDate ,Ukey,OriQty )
			values (s.Id ,s.Seq ,s.Article ,s.SizeCode ,s.Qty ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate ,s.Ukey ,s.OriQty)
		when not matched by source  AND T.ID IN (SELECT ID FROM #Torder) then 
		delete;

		-----------------Order_UnitPrice------------
		Merge Production.dbo.Order_UnitPrice as t
		Using (select a.* from Trade_To_Pms.dbo.Order_UnitPrice a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when Matched then
			update set			
			t.POPrice = s.POPrice ,
			t.QuotCost = s.QuotCost ,
			t.DestPrice = s.DestPrice ,
			t.AddName = s.AddName ,
			t.AddDate = s.AddDate ,
			t.EditName = s.EditName ,
			t.EditDate = s.EditDate 
		when not Matched by target then
			insert(Id ,Article ,SizeCode ,POPrice ,QuotCost ,DestPrice ,AddName ,AddDate ,EditName ,EditDate )
			values(s.Id ,s.Article ,s.SizeCode ,s.POPrice ,s.QuotCost ,s.DestPrice ,s.AddName ,s.AddDate ,s.EditName ,s.EditDate )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
				delete;

		--------------Order_TmsCost-----TMS & Cost
		Merge  Production.dbo.Order_TmsCost as t
		Using  (select a.* from Trade_To_Pms.dbo.Order_TmsCost a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.ArtworkTypeID=s.ArtworkTypeID
		when matched then 
			update set			
			t.Seq= s.Seq,
			t.Qty= s.Qty,
			t.ArtworkUnit= s.ArtworkUnit,
			t.TMS= s.TMS,
			t.Price= s.Price,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate		
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
		delete;

		------------------  insert Order_TmsCost
			INSERT INTO Production.dbo.Order_TmsCost(ID,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,InhouseOSP,LocalSuppID,AddName,AddDate,EditName,EditDate)
		SELECT A.ID,A.ArtworkTypeID,A.Seq,A.Qty,A.ArtworkUnit,A.TMS,A.Price,C.InhouseOSP,
		IIF(C.InhouseOSP='O',
			(SELECT top 1 t.LocalSuppId FROM Production.dbo.Style_Artwork_Quot T WITH (NOLOCK)
			inner join  production.dbo.Style_Artwork a WITH (NOLOCK) on t.Ukey=a.Ukey
			inner join Trade_To_Pms.DBO.Order_TmsCost  b WITH (NOLOCK) on a.ArtworkTypeID=b.ArtworkTypeID
			WHERE T.Ukey IN (SELECT A.Ukey 
			FROM Production.dbo.Style A	WITH (NOLOCK)
			INNER JOIN #TOrder B ON A.ID=B.StyleID AND A.BRANDID=B.BrandID AND A.SeasonID=B.SeasonID) ),
			(SELECT LocalSuppID FROM Production.dbo.Order_TmsCost WITH (NOLOCK) WHERE ID=A.ID)),
			A.AddName,A.AddDate,A.EditName,A.EditDate 
		FROM Trade_To_Pms.dbo.Order_TmsCost A WITH (NOLOCK)
		INNER JOIN #TOrder B ON A.ID=B.ID
		INNER JOIN Production.dbo.ArtworkType C WITH (NOLOCK) ON A.ArtworkTypeID=C.ID
		where a.Id not in (select id from Production.dbo.Order_TmsCost WITH (NOLOCK))
	
	
		-----------------Order_SizeCode---------------------------�ؤo�� Size Spec(�s�ؤo�X)
		--20170110 willy �վ㶶��: �R��>�ק�>�s�W
		Merge Production.dbo.Order_SizeCode as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id where a.sizecode is not null) as s
		on t.id=s.id and t.sizecode=s.sizecode and t.ukey=s.ukey
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete
		when matched then
			update set
			t.Seq= s.Seq,
			t.SizeGroup= s.SizeGroup
		When not matched by target then 
			insert(Id,Seq,SizeGroup,SizeCode,ukey)
			values(s.Id,s.Seq,s.SizeGroup,s.SizeCode,s.ukey);

		----------------Order_Sizeitem------------------------------�ؤo�� Size Spec(�s�q�k���)
		Merge Production.dbo.Order_Sizeitem as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Sizeitem a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 			
			t.SizeUnit= s.SizeUnit,
			t.SizeDesc= s.Description
		when not matched by Target then
			insert(Id,SizeItem,SizeUnit,SizeDesc,ukey)
			values(s.Id,s.SizeItem,s.SizeUnit,s.Description,s.ukey)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
	
		-------------Order_SizeSpec--------------------------------�ؤo�� Size Spec(�s�ؤo�X)
		Merge Production.dbo.Order_SizeSpec as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeSpec a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on  t.ukey=s.ukey
		when matched then 
			update set
			t.SizeSpec= s.SizeSpec
		when not matched by target then
			insert(Id,SizeItem,SizeCode,SizeSpec,ukey)
			values(s.Id,s.SizeItem,s.SizeCode,s.SizeSpec,s.ukey)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

------------Order_ColorCombo---------------(�D�ưt���)
		Merge Production.dbo.Order_ColorCombo as t
		Using (select a.* from Trade_To_Pms.dbo.Order_ColorCombo a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.lectracode=s.lectracode
		when matched then 
			update set
			t.ColorID= s.ColorID,
			t.FabricCode= s.FabricCode,
			t.PatternPanel= s.PatternPanel,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then 
			insert(Id,Article,ColorID,FabricCode,LectraCode,PatternPanel,AddName,AddDate,EditName,EditDate)
			values(s.Id,s.Article,s.ColorID,s.FabricCode,s.LectraCode,s.PatternPanel,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
			
	
		-------------Order_FabricCode------------------����vs���OvsQT
		Merge Production.dbo.Order_FabricCode as t
		Using (select a.* from Trade_To_Pms.dbo.Order_FabricCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.Lectracode=s.Lectracode
		when matched then 
			update set
			t.PatternPanel= s.PatternPanel,
			t.FabricCode= s.FabricCode,
			t.Lectracode= s.Lectracode,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate,
			t.Order_BOFUkey= s.Order_BOFUkey
		when not matched by target then 
			insert(Id,PatternPanel,FabricCode,Lectracode,AddName,AddDate,EditName,EditDate,Order_BOFUkey)
			values(s.Id,s.PatternPanel,s.FabricCode,s.Lectracode,s.AddName,s.AddDate,s.EditName,s.EditDate,s.Order_BOFUkey)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

-------------Order_FabricCode_QT-----------------
		Merge Production.dbo.Order_FabricCode_QT as t
		Using (select a.* from Trade_To_Pms.dbo.Order_FabricCode_QT a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.Lectracode=s.Lectracode and t.seqno=s.seqno
		when matched then 
			update set
			t.FabricCode= s.FabricCode,
			t.QTFabricCode= s.QTFabricCode,
			t.QTLectraCode= s.QTLectraCode,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate,
			t.PatternPanel= s.PatternPanel,
			t.QTPatternPanel= s.QTPatternPanel
		when not matched by target then 
			insert(Id,FabricCode,LectraCode,SeqNO,QTFabricCode,QTLectraCode,AddName,AddDate,EditName,EditDate,PatternPanel,QTPatternPanel)
			values(s.Id,s.FabricCode,s.LectraCode,s.SeqNO,s.QTFabricCode,s.QTLectraCode,s.AddName,s.AddDate,s.EditName,s.EditDate,s.PatternPanel,s.QTPatternPanel)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
		-------------Order_Bof -----------------------Bill of Fabric

		Merge Production.dbo.Order_Bof as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Bof a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
			t.id=s.id,
			t.FabricCode= s.FabricCode,
			t.Refno= s.Refno,
			t.SCIRefno= s.SCIRefno,
			t.SuppID= s.SuppID,
			t.ConsPC= s.ConsPC,
			t.Seq1= s.Seq1,
			t.Kind= s.Kind,
			t.Remark= s.Remark,
			t.LossType= s.LossType,
			t.LossPercent= s.LossPercent,
			t.RainwearTestPassed= s.RainwearTestPassed,
			t.HorizontalCutting= s.HorizontalCutting,
			t.ColorDetail= s.ColorDetail,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then 
			insert(Id,FabricCode,Refno,SCIRefno,SuppID,ConsPC,Seq1,Kind,Ukey,Remark,LossType,LossPercent,RainwearTestPassed,HorizontalCutting,ColorDetail,AddName,AddDate,EditName,EditDate)
			values(s.Id,s.FabricCode,s.Refno,s.SCIRefno,s.SuppID,s.ConsPC,s.Seq1,s.Kind,s.Ukey,s.Remark,s.LossType,s.LossPercent,s.RainwearTestPassed,s.HorizontalCutting,s.ColorDetail,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		---------Order_Bof_Expend--------------Bill of Fabric -�ζq�i�}
		Merge Production.dbo.Order_Bof_Expend as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Bof_Expend a WITH (NOLOCK)
		inner join #Torder b on a.id=b.id) as s
		--inner join Production.dbo.Order_Bof b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
			t.Id= s.Id,
			t.Order_BOFUkey= s.Order_BOFUkey,
			t.ColorId= s.ColorId,
			t.SuppColor= s.SuppColor,
			t.OrderQty= s.OrderQty,
			t.Price= s.Price,
			t.UsageQty= s.UsageQty,
			t.UsageUnit= s.UsageUnit,
			t.Width= s.Width,
			t.SysUsageQty= s.SysUsageQty,
			t.QTLectraCode= s.QTLectraCode,
			t.Remark= s.Remark,
			t.OrderIdList= s.OrderIdList,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then 
			insert(  Id,  Order_BOFUkey,  ColorId,  SuppColor,  OrderQty,  Price,  UsageQty,  UsageUnit,  Width,  SysUsageQty,  QTLectraCode,  Remark,  OrderIdList,  AddName,  AddDate,  EditName,  EditDate,UKEY)
			values(s.Id,s.Order_BOFUkey,s.ColorId,s.SuppColor,s.OrderQty,s.Price,s.UsageQty,s.UsageUnit,s.Width,s.SysUsageQty,s.QTLectraCode,s.Remark,s.OrderIdList,s.AddName,s.AddDate,s.EditName,s.EditDate,s.UKEY)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		-------------Order_BOA------------------Bill of Accessory

		Merge Production.dbo.Order_BOA as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
			t.id=s.id,
			t.Refno= s.Refno,
			t.SCIRefno= s.SCIRefno,
			t.SuppID= s.SuppID,
			t.Seq= s.Seq1,
			t.ConsPC= s.ConsPC,
			t.BomTypeSize= s.BomTypeSize,
			t.BomTypeColor= s.BomTypeColor,			
			t.PatternPanel= s.PatternPanel,
			t.SizeItem= s.SizeItem,
			t.BomTypeZipper= s.BomTypeZipper,
			t.Remark= s.Remark,
			t.ProvidedPatternRoom= s.ProvidedPatternRoom,
			t.ColorDetail= s.ColorDetail,
			t.isCustCD= s.isCustCD,
			t.lossType= s.lossType,
			t.LossPercent= s.LossPercent,
			t.LossQty= s.LossQty,
			t.LossStep= s.LossStep,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate,
			t.SizeItem_Elastic= s.SizeItem_Elastic,
			t.BomTypePo= s.BomTypePo,
			t.Keyword=s.Keyword
		when not matched by target then
			insert(Id,Ukey,Refno,SCIRefno,SuppID,Seq,ConsPC,BomTypeSize,PatternPanel,SizeItem,BomTypeZipper,Remark,ProvidedPatternRoom,ColorDetail,isCustCD,lossType,LossPercent,LossQty,LossStep,AddName,AddDate,EditName,EditDate,SizeItem_Elastic,BomTypePo,Keyword)			
			values(s.Id,s.Ukey,s.Refno,s.SCIRefno,s.SuppID,s.Seq1,s.ConsPC,s.BomTypeSize,s.PatternPanel,s.SizeItem,s.BomTypeZipper,s.Remark,s.ProvidedPatternRoom,s.ColorDetail,s.isCustCD,s.lossType,s.LossPercent,s.LossQty,s.LossStep,s.AddName,s.AddDate,s.EditName,s.EditDate,s.SizeItem_Elastic,s.BomTypePo,s.Keyword)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		-----------------Order_BOA_Expend----------------Bill of accessory -�ζq�i�}
		Merge Production.dbo.Order_BOA_Expend as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_Expend a WITH (NOLOCK)	
		inner join #Torder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
			t.id=s.id,
			t.Order_BOAUkey= s.Order_BOAUkey,
			t.OrderQty= s.OrderQty,
			t.Refno= s.Refno,
			t.SCIRefno= s.SCIRefno,
			t.Price= s.Price,
			t.UsageQty= s.UsageQty,
			t.UsageUnit= s.UsageUnit,
			t.Article= s.Article,
			t.ColorId= s.ColorId,
			t.SuppColor= s.SuppColor,
			t.SizeCode= s.SizeCode,
			t.Sizespec= s.Sizespec,
			t.SizeUnit= s.SizeUnit,
			t.OrderIdList= s.OrderIdList,
			t.SysUsageQty= s.SysUsageQty,
			t.Remark= s.Remark,			
			t.BomZipperInsert= s.BomZipperInsert,			
			t.BomCustPONo= s.BomCustPONo,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate,
			t.Keyword = s.Keyword
		when not matched by target then
			insert(Id,UKEY,Order_BOAUkey,OrderQty,Refno,SCIRefno,Price,UsageQty,UsageUnit,Article,ColorId,SuppColor,SizeCode,Sizespec,SizeUnit,OrderIdList,SysUsageQty,Remark,BomZipperInsert,BomCustPONo,AddName,AddDate,EditName,EditDate,Keyword)			
			values(s.Id,s.UKEY,s.Order_BOAUkey,s.OrderQty,s.Refno,s.SCIRefno,s.Price,s.UsageQty,s.UsageUnit,s.Article,s.ColorId,s.SuppColor,s.SizeCode,s.Sizespec,s.SizeUnit,s.OrderIdList,s.SysUsageQty,s.Remark,s.BomZipperInsert,s.BomCustPONo,s.AddName,s.AddDate,s.EditName,s.EditDate,s.Keyword)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		---------------Order_MarkerList------------Marker List

		Merge Production.dbo.Order_MarkerList as t
		Using (select a.* from Trade_To_Pms.dbo.Order_MarkerList a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
			t.id=s.id,
			t.Seq= s.Seq,
			t.MarkerName= s.MarkerName,
			t.FabricCode= s.FabricCode,
			t.FabricCombo= s.FabricCombo,
			t.LectraCode= s.LectraCode,
			t.isQT= s.isQT,
			t.MarkerLength= s.MarkerLength,
			t.ConsPC= s.ConsPC,
			t.Cuttingpiece= s.Cuttingpiece,
			t.ActCuttingPerimeter= s.ActCuttingPerimeter,
			t.StraightLength= s.StraightLength,
			t.CurvedLength= s.CurvedLength,
			t.Efficiency= s.Efficiency,
			t.Remark= s.Remark,
			t.MixedSizeMarker= s.MixedSizeMarker,
			t.MarkerNo= s.MarkerNo,
			t.MarkerUpdate= s.MarkerUpdate,
			t.MarkerUpdateName= s.MarkerUpdateName,
			t.AllSize= s.AllSize,
			t.PhaseID= s.PhaseID,
			t.SMNoticeID= s.SMNoticeID,
			t.MarkerVersion= s.MarkerVersion,
			t.Direction= s.Direction,
			t.CuttingWidth= s.CuttingWidth,
			t.Width= s.Width,
			t.Type= s.Type,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then
			insert(Id,Ukey,Seq,MarkerName,FabricCode,FabricCombo,LectraCode,isQT,MarkerLength,ConsPC,Cuttingpiece,ActCuttingPerimeter,StraightLength,CurvedLength,Efficiency,Remark,MixedSizeMarker,MarkerNo,MarkerUpdate,MarkerUpdateName,AllSize,PhaseID,SMNoticeID,MarkerVersion,Direction,CuttingWidth,Width,Type,AddName,AddDate,EditName,EditDate)
			values(s.Id,s.Ukey,s.Seq,s.MarkerName,s.FabricCode,s.FabricCombo,s.LectraCode,s.isQT,s.MarkerLength,s.ConsPC,s.Cuttingpiece,s.ActCuttingPerimeter,s.StraightLength,s.CurvedLength,s.Efficiency,s.Remark,s.MixedSizeMarker,s.MarkerNo,s.MarkerUpdate,s.MarkerUpdateName,s.AllSize,s.PhaseID,s.SMNoticeID,s.MarkerVersion,s.Direction,s.CuttingWidth,s.Width,s.Type,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		------Order_MarkerList_SizeQty----------------
		Merge Production.dbo.Order_MarkerList_SizeQty as t
		Using (select a.* from Trade_To_Pms.dbo.Order_MarkerList_SizeQty a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.order_MarkerListUkey=s.order_MarkerListUkey and t.sizecode=s.sizecode
		when matched then 
			update set
			t.id=s.id,
			t.SizeCode= s.SizeCode,
			t.Qty= s.Qty
		when not matched by target then
			insert(Order_MarkerListUkey,Id,SizeCode,Qty)
			values(s.Order_MarkerListUkey,s.Id,s.SizeCode,s.Qty)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		--------Order_ArtWork-----------------
		Merge Production.dbo.Order_ArtWork as t
		Using (select a.* from Trade_To_Pms.dbo.Order_ArtWork a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
			t.id=s.id,
			t.ArtworkTypeID= s.ArtworkTypeID,
			t.Article= s.Article,
			t.PatternCode= s.PatternCode,
			t.PatternDesc= s.PatternDesc,
			t.ArtworkID= s.ArtworkID,
			t.ArtworkName= s.ArtworkName,
			t.Qty= s.Qty,
			t.TMS= s.TMS,
			t.Price= s.Price,
			t.Cost= s.Cost,
			t.Remark= s.Remark,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then 
			insert(ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,Qty,TMS,Price,Cost,Remark,Ukey,AddName,AddDate,EditName,EditDate)
			values(s.ID,s.ArtworkTypeID,s.Article,s.PatternCode,s.PatternDesc,s.ArtworkID,s.ArtworkName,s.Qty,s.TMS,s.Price,s.Cost,s.Remark,s.Ukey,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;


		---------Order_EachCons--------------------Each Cons
		Merge Production.dbo.Order_EachCons as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set 
			t.Id=					s.Id,
			t.Seq=					s.Seq,
			t.MarkerName=			s.MarkerName,
			t.FabricCombo=			s.FabricCombo,
			t.MarkerLength=		replace(s.MarkerLength,'��','Y'),
			t.LectraCode=			s.LectraCode,
			t.ConsPC= s.ConsPC,
			t.CuttingPiece= s.CuttingPiece,
			t.ActCuttingPerimeter=	replace(s.ActCuttingPerimeter,'Yd','Y'),
			t.StraightLength=	replace(s.StraightLength,'Yd','Y'),
			t.FabricCode= s.FabricCode,
			t.CurvedLength=		replace(s.CurvedLength,'Yd','Y'),
			t.Efficiency= s.Efficiency,
			t.Article= s.Article,
			t.Remark= s.Remark,
			t.MixedSizeMarker= s.MixedSizeMarker,
			t.MarkerNo= s.MarkerNo,
			t.MarkerUpdate= s.MarkerUpdate,
			t.MarkerUpdateName= s.MarkerUpdateName,
			t.AllSize= s.AllSize,
			t.PhaseID= s.PhaseID,
			t.SMNoticeID= s.SMNoticeID,
			t.MarkerVersion= s.MarkerVersion,
			t.Direction= s.Direction,
			t.CuttingWidth= s.CuttingWidth,
			t.Width= s.Width,
			t.TYPE= s.TYPE,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate,
			t.isQT= s.isQT,
			t.MarkerDownloadID= s.MarkerDownloadID,
			t.OrderCUkey_Old= s.OrderCUkey_Old
		when not matched by target then 
			insert(Id,Ukey,Seq,MarkerName,FabricCombo,MarkerLength,LectraCode,ConsPC,CuttingPiece,ActCuttingPerimeter,StraightLength,FabricCode,CurvedLength,Efficiency,Article,Remark,MixedSizeMarker,MarkerNo,MarkerUpdate,MarkerUpdateName,AllSize,PhaseID,SMNoticeID,MarkerVersion,Direction,CuttingWidth,Width,TYPE,AddName,AddDate,EditName,EditDate,isQT,MarkerDownloadID,OrderCUkey_Old)
			values(s.Id,s.Ukey,s.Seq,s.MarkerName,s.FabricCombo,s.MarkerLength,s.LectraCode,s.ConsPC,s.CuttingPiece,s.ActCuttingPerimeter,s.StraightLength,s.FabricCode,s.CurvedLength,s.Efficiency,s.Article,s.Remark,s.MixedSizeMarker,s.MarkerNo,s.MarkerUpdate,s.MarkerUpdateName,s.AllSize,s.PhaseID,s.SMNoticeID,s.MarkerVersion,s.Direction,s.CuttingWidth,s.Width,s.TYPE,s.AddName,s.AddDate,s.EditName,s.EditDate,s.isQT,s.MarkerDownloadID,s.OrderCUkey_Old)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;

		--------Order_EachCons_SizeQty----------------Each cons - Size & Qty
		Merge Production.dbo.Order_EachCons_SizeQty as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_SizeQty a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Order_EachConsUkey=s.Order_EachConsUkey and t.sizecode=s.sizecode	
		when matched then 
			update set 
			t.Id= s.Id,
			t.SizeCode= s.SizeCode,
			t.Qty= s.Qty
		when not matched by target then 
			insert(Order_EachConsUkey,Id,SizeCode,Qty)
			values(s.Order_EachConsUkey,s.Id,s.SizeCode,s.Qty)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;

		-------Order_EachCons_Color--------------------Each cons - �ζq�i�}
		Merge Production.dbo.Order_EachCons_Color as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Color a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Ukey=s.Ukey	
		when matched then 
			update set 
			t.Id= s.Id,
			t.Order_EachConsUkey= s.Order_EachConsUkey,
			t.Ukey= s.Ukey,
			t.ColorID= s.ColorID,
			t.CutQty= s.CutQty,
			t.Layer= s.Layer,
			t.Orderqty= s.Orderqty,
			t.SizeList= s.SizeList,
			t.Variance= s.Variance,
			t.YDS= s.YDS
		when not matched by target then 
			insert(Id,Order_EachConsUkey,Ukey,ColorID,CutQty,Layer,Orderqty,SizeList,Variance,YDS)
			values(s.Id,s.Order_EachConsUkey,s.Ukey,s.ColorID,s.CutQty,s.Layer,s.Orderqty,s.SizeList,s.Variance,s.YDS)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
		
		---------Order_EachCons_Color_Article-------Each cons - �ζq�i�}����
		Merge Production.dbo.Order_EachCons_Color_Article as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Color_Article a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Ukey=s.Ukey	
		when matched then 
			update set 
			t.Id= s.Id,
			t.Order_EachCons_ColorUkey= s.Order_EachCons_ColorUkey,
			t.Article= s.Article,
			t.ColorID= s.ColorID,
			t.SizeCode= s.SizeCode,
			t.Orderqty= s.Orderqty,
			t.Layer= s.Layer,
			t.CutQty= s.CutQty,
			t.Variance= s.Variance
		when not matched by target then 
			insert(Id,Order_EachCons_ColorUkey,Article,ColorID,SizeCode,Orderqty,Layer,CutQty,Variance,Ukey)
			values(s.Id,s.Order_EachCons_ColorUkey,s.Article,s.ColorID,s.SizeCode,s.Orderqty,s.Layer,s.CutQty,s.Variance,s.Ukey)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;

		----------Order_EachCons_PatternPanel---------------PatternPanel
			Merge Production.dbo.Order_EachCons_PatternPanel as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_PatternPanel a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.PatternPanel=s.PatternPanel and t.Order_EachConsUkey=s.Order_EachConsUkey and t.LectraCode=s.LectraCode
		When matched then 
			update set 
			t.Id= s.Id,
			--t.PatternPanel= s.PatternPanel,
			--t.Order_EachConsUkey= s.Order_EachConsUkey,
			--t.LectraCode= s.LectraCode,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then 
			insert(Id,PatternPanel,Order_EachConsUkey,LectraCode,AddName,AddDate,EditName,EditDate)
			values(s.Id,s.PatternPanel,s.Order_EachConsUkey,s.LectraCode,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source and t.id in (select id from #TOrder) then 
			delete;

		
		------------Order_Article----------------------Art
		Merge Production.dbo.Order_Article as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Article a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article
		when matched then 
			update set 
			t.Seq= s.Seq,
			t.TissuePaper= s.TissuePaper
		when not matched by target then
			insert(id,Seq,Article,TissuePaper)
			values(s.id,s.Seq,s.Article,s.TissuePaper)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;

		-----------Order_BOA_KeyWord---------------------Bill of Other - Key word

		Merge Production.dbo.Order_BOA_KeyWord as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_KeyWord a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 
			t.id=s.id,
			t.Order_BOAUkey= s.Order_BOAUkey,
			t.KeyWordID= s.KeyWordID,
			t.Relation=s.Relation
		when not matched by target then
			insert(ID,Ukey,Order_BOAUkey,KeyWordID,Relation)
			values(s.ID,s.Ukey,s.Order_BOAUkey,s.KeyWordID,s.Relation)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;
	

		------------Order_BOA_CustCD----------Bill of Other - �ζq�i�}
		Merge Production.dbo.Order_BOA_CustCD as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_CustCD a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.Order_BOAUkey=s.Order_BOAUkey and t.custcdid=s.custcdid and t.refno=s.refno
		when matched then 
			update set 
			t.id=s.id,
			t.CustCDID= s.CustCDID,
			t.Refno= s.Refno,
			t.SCIRefno= s.SCIRefno,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then
			insert(Id,Order_BOAUkey,CustCDID,Refno,SCIRefno,AddName,AddDate,EditName,EditDate)
			values(s.Id,s.Order_BOAUkey,s.CustCDID,s.Refno,s.SCIRefno,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		-----------------Order_PFHis-----------Pull forward���v�O��
		Merge Production.dbo.Order_PFHis as t
		Using (select a.* from Trade_To_Pms.dbo.Order_PFHis a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.Ukey=s.Ukey
		when matched then 
			update set 
			t.id=s.id,
			t.NewSciDelivery= s.NewSciDelivery,
			t.OldSciDelivery= s.OldSciDelivery,
			t.LETA= s.LETA,
			t.Remark= s.Remark,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate
		when not matched by target then
			insert(Id,NewSciDelivery,OldSciDelivery,LETA,Remark,AddName,AddDate,Ukey)
			values(s.Id,s.NewSciDelivery,s.OldSciDelivery,s.LETA,s.Remark,s.AddName,s.AddDate,s.Ukey)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;
		----------------Order_QtyCTN------------Qty breakdown per Carton
		Merge Production.dbo.Order_QtyCTN as t
		Using (select a.* from Trade_To_Pms.dbo.Order_QtyCTN a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when matched then 
			update set 
			t.Article= s.Article,
			t.SizeCode= s.SizeCode,
			t.Qty= s.Qty,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then
			insert(Id,Article,SizeCode,Qty,AddName,AddDate,EditName,EditDate)
			values(s.Id,s.Article,s.SizeCode,s.Qty,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------------MNOder-------------------------M/NOtice
		Merge Production.dbo.MNOrder as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id
		when matched then 
			update set 
			t.BrandID= s.BrandID,
			t.ProgramID= s.ProgramID,
			t.StyleID= s.StyleID,
			t.SeasonID= s.SeasonID,
			t.Qty= s.Qty,
			t.OrderUnit= s.OrderUnit,
			t.FactoryID= s.FactoryID,
			t.CTNQty= s.CTNQty,
			t.CustCDID= s.CustCDID,
			t.CustPONO= s.CustPONO,
			t.Customize1= s.Customize1,
			t.BuyerDelivery= s.BuyerDelivery,
			t.MRHandle= s.MRHandle,
			t.SMR= s.SMR,
			t.PACKING= s.PACKING,
			t.Packing2= s.Packing2,
			t.MarkBack= s.MarkBack,
			t.MarkFront= s.MarkFront,
			t.MarkLeft= s.MarkLeft,
			t.MarkRight= s.MarkRight,
			t.Label= s.Label,
			t.SizeRange= s.SizeRange,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.POID =s.POID
		when not matched by target then
			insert(ID,BrandID,ProgramID,StyleID,SeasonID,Qty,OrderUnit,FactoryID,CTNQty,CustCDID,CustPONO,Customize1,BuyerDelivery,MRHandle,SMR,PACKING,Packing2,MarkBack,MarkFront,MarkLeft,MarkRight,Label,SizeRange,AddName,AddDate,POID)
			values(s.ID,s.BrandID,s.ProgramID,s.StyleID,s.SeasonID,s.Qty,s.OrderUnit,s.FactoryID,s.CTNQty,s.CustCDID,s.CustPONO,s.Customize1,s.BuyerDelivery,s.MRHandle,s.SMR,s.PACKING,s.Packing2,s.MarkBack,s.MarkFront,s.MarkLeft,s.MarkRight,s.Label,s.SizeRange,s.AddName,s.AddDate,s.POID);
		
		----------------MNOrder_Qty---------------------------M/NOtice Qty breakdown
		Merge Production.dbo.MNOrder_Qty as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_Qty a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when matched then 
			update set 
			t.Article= s.Article,
			t.SizeCode= s.SizeCode,
			t.Qty= s.Qty,
			t.AddName= s.AddName,
			t.AddDate= s.AddDate,
			t.EditName= s.EditName,
			t.EditDate= s.EditDate
		when not matched by target then
			insert(ID,Article,SizeCode,Qty,AddName,AddDate,EditName,EditDate)
			values(s.ID,s.Article,s.SizeCode,s.Qty,s.AddName,s.AddDate,s.EditName,s.EditDate)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------MNOrder_Color---------------------M/NOtice ��-Color Description
		Merge Production.dbo.MNOrder_Color as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_Color a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.colorid=s.colorid and t.seqno=s.seqno
		when matched then 
			update set 
			t.ColorName= s.ColorName,
			t.Seqno= s.Seqno,
			t.ColorMultiple= s.ColorMultiple,
			t.ColorMultipleName= s.ColorMultipleName
		when not matched by target then
			insert(ID,ColorID,ColorName,Seqno,ColorMultiple,ColorMultipleName)
			values(s.ID,s.ColorID,s.ColorName,s.Seqno,s.ColorMultiple,s.ColorMultipleName)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------MNOrder_SizeCode-----------------M/NOtice-�ؤo�� Size Spec(�s�ؤo�X)
		Merge Production.dbo.MNOrder_SizeCode as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_SizeCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.SizeCode=s.SizeCode 
		when matched then 
			update set 
			t.Seq= s.Seq,
			t.SizeGroup= s.SizeGroup
		when not matched by target then
			insert(Id,Seq,SizeGroup,SizeCode)
			values(s.Id,s.Seq,s.SizeGroup,s.SizeCode)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------MNOrder_SizeItem-----------------M/NOtice-�ؤo�� Size Spec(�s�q�k���)
		Merge Production.dbo.MNOrder_SizeItem as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_SizeItem a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.SizeItem=s.SizeItem 
		when matched then 
			update set 
			t.SizeDesc= s.SizeDesc,
			t.SizeUnit= s.SizeUnit
		when not matched by target then
			insert(ID,SizeItem,SizeDesc,SizeUnit)
			values(s.ID,s.SizeItem,s.SizeDesc,s.SizeUnit)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;


		--------------MNOrder_SizeSpec-----------------M/NOtice-�ؤo�� Size Spec(�s�ؤo�X)
		Merge Production.dbo.MNOrder_SizeSpec as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_SizeSpec a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.SizeItem=s.SizeItem and t.SizeCode=s.SizeCode and t.ukey=s.ukey
		when matched then 
			update set 
			t.SizeSpec= s.SizeSpec
		when not matched by target then
			insert(ID,SizeItem,SizeCode,SizeSpec,ukey)
			values(s.ID,s.SizeItem,s.SizeCode,s.SizeSpec,s.ukey)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------MNOrder_ColorCombo-----------------M/NOtice-Color Comb. (�D��-�t���)

		Merge Production.dbo.MNOrder_ColorCombo as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_ColorCombo a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.Article=s.Article and t.LectraCode=s.LectraCode
		when matched then 
			update set 
			t.ColorID= s.ColorID,
			t.FabricCode= s.FabricCode,
			t.PatternPanel= s.PatternPanel
		when not matched by target then
			insert(ID,Article,ColorID,FabricCode,PatternPanel,LectraCode)
			values(s.ID,s.Article,s.ColorID,s.FabricCode,s.PatternPanel,s.LectraCode)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------MNOrder_FabricCode-----------------M/NOtice-�t��� Color Comb. (�D��-����vs���OvsQT)
		Merge Production.dbo.MNOrder_FabricCode as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_FabricCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.LectraCode=s.LectraCode
		when matched then 
			update set 
			t.PatternPanel= s.PatternPanel,
			t.FabricCode= s.FabricCode
		when not matched by target then
			insert(ID,PatternPanel,FabricCode,LectraCode)
			values(s.ID,s.PatternPanel,s.FabricCode,s.LectraCode)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		--------------MNOrder_BOF-----------------M/NOtice-Fabric
		Merge Production.dbo.MNOrder_BOF as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_BOF a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.FabricCode=s.FabricCode
		when matched then 
			update set 
			t.Refno= s.Refno,
			t.SCIRefno= s.SCIRefno,
			t.SuppID= s.SuppID,
			t.Description= s.Description,
			t.FabricUkey_Old= s.FabricUkey_Old,
			t.FabricVer_OLd = s.FabricVer_OLd 
		when not matched by target then
			insert(  ID,  FabricCode,  Refno,  SCIRefno,  SuppID,  Description,  FabricUkey_Old,  FabricVer_OLd )
			values(s.ID,s.FabricCode,s.Refno,s.SCIRefno,s.SuppID,s.Description,s.FabricUkey_Old,s.FabricVer_OLd )
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

			--------------MNOrder_BOA-----------------M/NOtice-Fabric
		Merge Production.dbo.MNOrder_BOA as t
		Using (select a.* from Trade_To_Pms.dbo.MNOrder_BOA a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 
			t.id=s.id,
			t.Refno= s.Refno,
			t.SCIRefno= s.SCIRefno,
			t.SuppID= s.SuppID,
			t.Seq= s.Seq,
			t.UsedQty= s.UsedQty,
			t.BomTypeSize= s.BomTypeSize,
			t.BomTypeColor= s.BomTypeColor,
			t.BomTypePono= s.BomTypePono,
			t.PatternPanel= s.PatternPanel,
			t.SizeItem= s.SizeItem,
			t.BomTypeZipper= s.BomTypeZipper,
			t.Remark= s.Remark,
			t.Description= s.Description,
			t.FabricVer_Old= s.FabricVer_Old,
			t.FabricUkey_Old= s.FabricUkey_Old
		when not matched by target then
			insert(  Id,  UKey,  Refno,  SCIRefno,  SuppID,  Seq,  UsedQty,  BomTypeSize,  BomTypeColor,  BomTypePono,  PatternPanel,  SizeItem,  BomTypeZipper,  Remark,  Description,  FabricVer_Old,  FabricUkey_Old )			
			values(s.Id,s.UKey,s.Refno,s.SCIRefno,s.SuppID,s.Seq,s.UsedQty,s.BomTypeSize,s.BomTypeColor,s.BomTypePono,s.PatternPanel,s.SizeItem,s.BomTypeZipper,s.Remark,s.Description,s.FabricVer_Old,s.FabricUkey_Old)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

--declare @Odate_s datetime = (SELECT TOP 1 DateStart FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')
declare @Odate_s datetime = (SELECT TOP 1 DateStart FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')
declare @Odate_e datetime = (SELECT TOP 1 DateEnd FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')


--32246
select * into #tmpOrders from Production.dbo.Orders a WITH (NOLOCK)
where (
a.BuyerDelivery between @Odate_s and @Odate_e
or a.SciDelivery between @Odate_s and @Odate_e)
and a.LocalOrder = 0


	Merge  Production.dbo.OrderComparisonList as t
	Using (select a.*,b.styleid as AStyle from #tmpOrders a inner join #TOrder b on a.id=b.id) as s
	on t.orderid=s.id and t.factoryid=s.factoryid and t.updateDate = @dToDay
	when matched then
		update set
		t.TransferDate=@OldDate,
		t.OriginalQty =s.qty,
		t.OriginalBuyerDelivery= s.BuyerDelivery,
		t.DeleteOrder=1,
		t.OriginalStyleID = iif(s.AStyle is not null,s.AStyle,s.styleid),
		t.TransferToFactory = s.FactoryID
	when not matched by target then
			insert (orderid,UpdateDate,  factoryid)
			values (   s.id,   @dToDay,s.factoryid);

	-----------------No Change!------------------------------------------------------------------------------------------

	Merge Production.dbo.OrderComparisonList as t
	Using Production.dbo.Factory as s
	on t.factoryid=s.id and UpdateDate =@dToDay
	when not matched by Target then 
	insert(OrderId,    UpdateDate,  TransferDate,FactoryID)
	values('No Change!',@dToDay,    @OldDate,    s.ID);
-----------------------------------------------------------------------------------------------------------
		------------------Leo--------------------------------------

----�R�����P�_�����n�̷�#Torder���϶��@�R��

-------------------------------------Order_Article
Delete b
from Production.dbo.Order_Article b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_Artwork
Delete b
from Production.dbo.Order_Artwork b
where id in (select id from #tmpOrders as t
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOA_CustCD
Delete b
from Production.dbo.Order_BOA_CustCD b
where id in (select id from #tmpOrders as t
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOA_Expend
Delete b
from Production.dbo.Order_BOA_Expend b
where id in (select id from #tmpOrders as t
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOA_KeyWord
Delete b
from Production.dbo.Order_BOA_KeyWord b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOA_Shell
Delete b
from Production.dbo.Order_BOA_Shell b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOA
Delete b
from Production.dbo.Order_BOA b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOF_Expend
Delete b
from Production.dbo.Order_BOF_Expend b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOF_Shell
Delete b
from Production.dbo.Order_BOF_Shell b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BOF
Delete b
from Production.dbo.Order_BOF b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))

-------------------------------------Order_ColorCombo
Delete b
from Production.dbo.Order_ColorCombo b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))

-------------------------------------Order_CTNData
Delete b
from Production.dbo.Order_CTNData b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_EachCons_Article
Delete b
from Production.dbo.Order_EachCons_Article b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_EachCons_Color
Delete b
from Production.dbo.Order_EachCons_Color b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_EachCons_Color_Article
Delete b
from Production.dbo.Order_EachCons_Color_Article b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_EachCons_PatternPanel
Delete b
from Production.dbo.Order_EachCons_PatternPanel b
where id in (select id from #tmpOrders as t
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_EachCons_SizeQty
Delete b
from Production.dbo.Order_EachCons_SizeQty b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_EachCons
Delete b
from Production.dbo.Order_EachCons b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_FabricCode_Article
Delete b
from Production.dbo.Order_FabricCode_Article b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_FabricCode_QT
Delete b
from Production.dbo.Order_FabricCode_QT b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_FabricCode
Delete b
from Production.dbo.Order_FabricCode b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_History
Delete b
from Production.dbo.Order_History b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_MarkerList_Article
Delete b
from Production.dbo.Order_MarkerList_Article b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_MarkerList_PatternPanel
Delete b
from Production.dbo.Order_MarkerList_PatternPanel b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_MarkerList_SizeQty
Delete b
from Production.dbo.Order_MarkerList_SizeQty b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_MarkerList
Delete b
from Production.dbo.Order_MarkerList b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_PFHis
Delete b
from Production.dbo.Order_PFHis b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_Qty
Delete b
from Production.dbo.Order_Qty b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_QtyCTN
Delete b
from Production.dbo.Order_QtyCTN b
where id in (select id from #tmpOrders as t
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_QtyShip
Delete b
from Production.dbo.Order_QtyShip b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_QtyShip_Detail
Delete b
from Production.dbo.Order_QtyShip_Detail b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_SizeCode
Delete b
from Production.dbo.Order_SizeCode b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_SizeItem
Delete b
from Production.dbo.Order_SizeItem b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_SizeSpec
Delete b
from Production.dbo.Order_SizeSpec b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_Surcharge
Delete b
from Production.dbo.Order_Surcharge b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_TmsCost
Delete b
from Production.dbo.Order_TmsCost b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_UnitPrice
Delete b
from Production.dbo.Order_UnitPrice b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder
Delete b
from Production.dbo.MNOrder b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_BOA
Delete b
from Production.dbo.MNOrder_BOA b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_BOF
Delete b
from Production.dbo.MNOrder_BOF b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_Color
Delete b
from Production.dbo.MNOrder_Color b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_ColorCombo
Delete b
from Production.dbo.MNOrder_ColorCombo b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_FabricCode
Delete b
from Production.dbo.MNOrder_FabricCode b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_Qty
Delete b
from Production.dbo.MNOrder_Qty b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_SizeCode
Delete b
from Production.dbo.MNOrder_SizeCode b
where id in (select id from #tmpOrders as t
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_SizeItem
Delete b
from Production.dbo.MNOrder_SizeItem b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------MNOrder_SizeSpec
Delete b
from Production.dbo.MNOrder_SizeSpec b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------[dbo].[PO]
Delete b
from Production.dbo.PO b
where id in (select POID from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------[dbo].[PO_Supp]
Delete b
from Production.dbo.PO_Supp b
where id in (select POID from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------[dbo].[PO_Supp_Detail]
Delete b
from Production.dbo.PO_Supp_Detail b
where id in (select POID from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------[dbo].[PO_Supp_Detail_OrderList]
Delete b
from Production.dbo.PO_Supp_Detail_OrderList b
where id in (select POID from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------[dbo].[Cutting]
Delete b
from Production.dbo.Cutting b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------CuttingTape[dbo].[CuttingTape]
Delete b
from Production.dbo.CuttingTape b
where POID in (select POID from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))

-------------------------------------[dbo].[CuttingTape_Detail]
Delete b
from Production.dbo.CuttingTape_Detail b
where POID in (select POID from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))

------------------------�R�����Y�h�����order �̫�R��
Delete a
from Production.dbo.Orders as a 
where a.id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))


drop table #tmpOrders
drop table #TOrder


END
------------------------------------------------------------------------------------------------------------------------


