-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/25>
-- Description:	<import order>
-- =============================================
Create PROCEDURE [dbo].[imp_Order]
AS
BEGIN

	SET NOCOUNT ON;

	declare @OldDate date = (select max(UpdateDate) from Production.dbo.OrderComparisonList WITH (NOLOCK)) --最後匯入資料日期
	declare @dToDay date = CONVERT(date, GETDATE()) --今天日期

	--LEO新增 @OldDate(TransferDate)
	-- @dToDay(UpdateDate)
	DELETE FROM Trade_To_Pms.dbo.DateInfo
	WHERE NAME='imp_Order_OldDate' or NAME='imp_Order_dToDay'

	INSERT INTO Trade_To_Pms.dbo.DateInfo (Name, DateStart, DateEnd)
                    VALUES  ('imp_Order_OldDate',@OldDate , @OldDate),
					        ('imp_Order_dToDay',@dToDay , @dToDay);

	declare @Odate_s datetime = (SELECT TOP 1 DateStart FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')
	declare @Odate_e datetime = (SELECT TOP 1 DateEnd FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')
-----------------匯入訂單檢核表------------------------
	delete from Production.dbo.OrderComparisonList
	where ISNULL(UpdateDate,'')='' and isnull(OrderId,'')='' and isnull(FactoryID,'')=''

------------------TempTable -------------
	--TempOrder
	select	a.*,
			CAST( '' as varchar(8)) as FTY_Group,
			cast (null as date) as SDPDate,
			cast(0 as bit) as PulloutComplete,
			cast('' as varchar(10) ) as MCHandle,
			cast(null as date) as MDClose
	into #TOrder
	from Trade_To_Pms.dbo.Orders a WITH (NOLOCK)
	inner join Production.dbo.Factory b WITH (NOLOCK) on a.FactoryID=b.ID

	update #TOrder
	set		FTY_Group = IIF(b.FTYGroup is null,a.FactoryID,b.FTYGroup) 
			, MDivisionID=b.MDivisionID
	from #TOrder a
	inner join Production.dbo.Factory b on a.FactoryID=b.id
	
-------------------------------------------------------------------------Order
		--��欰Cutting�����,����sMDivisionID
		Update a
		set a.MDivisionID = b.MDivisionID
		from Production.dbo.Cutting a
		inner join #TOrder b on a.ID = b.ID
		where a.MDivisionID <> b.MDivisionID and b.MDivisionID in( select distinct MDivisionID from Production..Factory)

		--轉單為Cutting母單時,覆寫CutPlan母子單的工廠欄位 
		Update a
		set a.FactoryID = b.FTY_Group
			, a.MDivisionID = f.MDivisionID
		from Production.dbo.WorkOrder a
		inner join #TOrder b on a.ID = b.ID
		inner join Production.dbo.Orders c on b.ID = c.ID 
											  and a.FactoryID != b.FTY_Group
		left join Production.dbo.Factory f on b.FTY_Group = f.ID
		where 	b.qty > 0 
				and b.IsForecast = '0'

		--delete cutting
		delete b
		from #TOrder a 		
		inner join Production.dbo.Cutting b on a.id = b.ID 
											   and b.FactoryID <> a.FTY_Group
		where	a.qty > 0 
				and a.IsForecast = '0'
				and not exists(select 1 from Production.dbo.MDivision m where m.ID = a.MDivisionID )

	--需填入 Order.SDPDate = Buyer Delivery - 放假日(船期表)--
		--如果買家到貨日不是工廠放假日,SDDate=BuyerDelivery
		update #TOrder
		set SDPDate = BuyerDelivery	

		--如果買家到貨日剛好遇到假日,SDDate就提前一天
		update a
		set a.SDPDate = DATEADD(day,-1, a.BuyerDelivery) 
		from #TOrder  a
		inner join Production.dbo.Factory b on a.FactoryID=b.ID
		inner join Production.dbo.Holiday c on b.id=c.factoryid 
											   and c.HolidayDate=a.BuyerDelivery

		
		-- 調整#TOrder not matched
		update t
		set		t.MCHandle = (select localMR 
							  from Production.dbo.Style 
							  where BrandID = t.BrandID 
									and id=t.styleid 
									and SeasonID=t.SeasonID )
				, t.PulloutComplete = iif((t.GMTComplete='P' OR t.GMTComplete='' or t.GMTComplete is null), 0, 1)
				, t.MDClose = iif((t.GMTComplete='P' OR t.GMTComplete='' or t.GMTComplete is null) 
				, t.MDClose
				, convert(date,getdate()))                                                                                             
		from #TOrder as t
		left join Production.dbo.Orders as s1 on t.ID=s1.ID
		where s1.ID is null

----------------取得 BuyerDelivery & SciDelivery 日期在 Trade 給的日期範圍中 Orders 的資料------------------------
	select * 
	into #tmpOrders 
	from Production.dbo.Orders a WITH (NOLOCK)
	where	(
				a.BuyerDelivery between @Odate_s and @Odate_e 
				or a.SciDelivery between @Odate_s and @Odate_e
			)
			and a.LocalOrder = 0
				
---------------------OrderComparisonList (1.Insert, 2.Delete, 3.ChangeFactory, 4.ChangeData, 5.NoChange)-----------------
---------------------除了 Delete 用區間比對，其餘的都用 PMS Orders
		----1.Insert 記錄 Trade 有 PMS 沒有的資料 (NewOrder = 1) 
		Merge Production.dbo.OrderComparisonList as t
		Using (	select a.* 
				from Trade_To_Pms.dbo.Orders a
				left join Production.dbo.Orders b on a.ID = b.ID
				where b.id is null
					  and a.FactoryID in (select ID from Production.dbo.Factory)) as s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToDay
		when matched then
			update set 
				t.NewOrder				= 1
				, t.OrderID				= s.ID
				, t.OriginalStyleID		= s.StyleID
				, t.NewQty				= s.Qty
				, t.NewBuyerDelivery	= s.BuyerDelivery
				, t.NewSCIDelivery		= s.SCIDelivery
				, t.MDivisionID			= s.MDivisionID
				, t.FactoryID			= s.FactoryID
				, t.BrandID				= s.BrandID
				, t.UpdateDate			= @dToDay--寫入到IMP MOCKUPORDER
				, t.TransferDate		= @OldDate--寫入到IMP MOCKUPORDER
		when not matched by target then
			insert (
				NewOrder		, OrderID		, OriginalStyleID	, NewQty	, NewBuyerDelivery
				, NewSCIDelivery, MDivisionID	, FactoryID			, UpdateDate, TransferDate
				, BrandID
			) values (
				1				, s.ID			, s.StyleID			, s.Qty		, s.BuyerDelivery
				, s.SCIDelivery	, s.MDivisionID	, s.FactoryID		, @dToDay	, @OldDate
				, s.BrandID
			);

		----2.Delete 記錄 Trade 沒有 PMS 有的資料 (DeleteOrder = 1)
		Merge Production.dbo.OrderComparisonList as t
		Using (	select a.*
				from #tmpOrders a
				left join Trade_To_Pms.dbo.Orders b on a.ID = b.ID and b.factoryid in (select ID from Production.dbo.Factory)
				where b.id is null) as s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToDay
		when matched then
			update set
				t.DeleteOrder				= 1
				, t.OrderID					= s.ID
				, t.OriginalStyleID			= s.StyleID
				, t.OriginalQty				= s.Qty
				, t.OriginalBuyerDelivery	= s.BuyerDelivery
				, t.OriginalSciDelivery		= s.SciDelivery
				, t.MDivisionID				= s.MDivisionID
				, t.FactoryID				= s.FactoryID
				, t.BrandID				    = s.BrandID
				, t.UpdateDate				= @dToday
				, t.TransferDate			= @OldDate
		when not matched by target then
			insert (
				DeleteOrder				, OrderID		, OriginalStyleID	, OriginalQty	, OriginalBuyerDelivery
				, OriginalSciDelivery	, MDivisionID	, FactoryID			, UpdateDate	, TransferDate
				, BrandID
			) values (
				1						, s.ID			, s.StyleID			, s.Qty			, s.BuyerDelivery
				, s.SCIDelivery			, s.MDivisionID	, s.FactoryID		, @dToDay		, @OldDate
				, s.BrandID
			);

		----3.ChangeFactory 記錄換工廠
		--------3.1.Delete 舊工廠的資料，資料帶入 PMS.Orders
		Merge Production.dbo.OrderComparisonList as t
		Using (	select	a.*
						, Transfer2Factroy = b.FactoryID
				from Production.dbo.Orders a
				join Trade_To_Pms.dbo.Orders b on a.ID = b.ID 
					 and a.FactoryID != b.FactoryID
					 and b.FactoryID in (select ID from Production.dbo.Factory)) as s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToDay
		when matched then 
			update set
				t.DeleteOrder				= 1
				, t.TransferToFactory		= s.Transfer2Factroy
				, t.OrderID					= s.ID
				, t.OriginalStyleID			= s.StyleID
				, t.OriginalQty				= s.Qty
				, t.OriginalBuyerDelivery	= s.BuyerDelivery
				, t.OriginalSciDelivery		= s.SciDelivery
				, t.MDivisionID				= s.MDivisionID
				, t.FactoryID				= s.FactoryID
				, t.BrandID				    = s.BrandID				
				, t.UpdateDate				= @dToday
				, t.TransferDate			= @OldDate
		when not matched by target then
			insert (
				DeleteOrder				, TransferToFactory		, OrderID		, OriginalStyleID	, OriginalQty
				, OriginalBuyerDelivery	, OriginalSciDelivery	, MDivisionID	, FactoryID			, UpdateDate
				, TransferDate
				, BrandID
			) values (
				1						, s.Transfer2Factroy	, s.ID			, s.StyleID			, s.Qty
				, s.BuyerDelivery		, s.SCIDelivery			, s.MDivisionID	, s.FactoryID		, @dToDay
				, @OldDate
				, s.BrandID
			);
		--------3.1.2.Delete 舊工廠的資料，資料帶入 PMS.Orders(跨M)
		Merge Production.dbo.OrderComparisonList as t
		Using (	
			select c.*
			, Transfer2Factroy = a.FactoryID
			from Trade_To_Pms.dbo.Orders a
			left join Production.dbo.Orders b on a.ID = b.ID and a.FactoryID != b.FactoryID
			inner join OrderComparisonList c on c.OrderID = a.id
			where b.id is null
			and a.FactoryID not in (select ID from Production.dbo.Factory)
			and c.UpdateDate = @dToDay
		) as s
		on t.OrderID = s.OrderID and t.FactoryID = s.FactoryID and t.UpdateDate = s.UpdateDate
		when matched then 
		update set
			t.DeleteOrder				= 1
			, t.TransferToFactory		= s.Transfer2Factroy
			, t.TransferDate			= @OldDate;


		 -------3.2.New 新工廠的資料，資料帶入 Trade.Orders
	    Merge Production.dbo.OrderComparisonList as t
		Using (	select b.*
				from Production.dbo.Orders a
				join Trade_To_Pms.dbo.Orders b on a.ID = b.ID 
					 and a.FactoryID != b.FactoryID
					 and b.FactoryID in (select ID from Production.dbo.Factory)) as s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToDay
		when matched then 
			update set
				t.NewOrder				= 1
				, t.OrderID				= s.ID
				, t.OriginalStyleID		= s.StyleID 
				, t.NewQty				= s.Qty
				, t.NewBuyerDelivery	= s.BuyerDelivery
				, t.NewSCIDelivery		= s.SCIDelivery
				, t.MDivisionID			= s.MDivisionID
				, t.FactoryID			= s.FactoryID	
				, t.BrandID				= s.BrandID				
				, t.UpdateDate			= @dToday
				, t.TransferDate		= @OldDate
		when not matched by target then
			insert (
				NewOrder		, OrderID		, OriginalStyleID	, NewQty	, NewBuyerDelivery
				, NewSCIDelivery, MDivisionID	, FactoryID			, UpdateDate, TransferDate
				, BrandID
			) values (
				1				, s.ID			, s.StyleID			, s.Qty		, s.BuyerDelivery
				, s.SCIDelivery	, s.MDivisionID	, s.FactoryID		, @dToDay	, @OldDate
				, s.BrandID
			);

		----4.ChangeData 記錄資料異動
		-------IIF => 如果 PMS & Trade 某一欄位相同，則新舊都存入 Null 代表沒有變動，
		-------                                不同，則 Trade 存入 New，PMS 存入 Original
		Merge Production.dbo.OrderComparisonList as t
		Using (select	ID					= A.ID
						, FactoryID			= A.FactoryID
						, BrandID			= b.BrandID
						, MDivisionID		= A.MDivisionID
						, O_Qty				= iif(isnull(a.qty, 0) != isnull(b.qty, 0), a.qty, 0)
						, N_Qty				= iif(isnull(a.qty, 0) != isnull(b.qty, 0), b.qty, 0)
						, O_BuyerDelivery	= iif(isnull(a.BuyerDelivery, '') != isnull(b.BuyerDelivery, ''), A.BuyerDelivery, null)
						, N_BuyerDelivery	= iif(isnull(a.BuyerDelivery, '') != isnull(b.BuyerDelivery, ''), b.BuyerDelivery, null)
						, O_SciDelivery		= iif(isnull(a.SciDelivery, '') != isnull(b.SciDelivery, ''), A.SciDelivery, null)
						, N_SciDelivery		= iif(isnull(a.SciDelivery, '') != isnull(b.SciDelivery, ''), b.SciDelivery, null)
						, O_CMPQDate		= iif(isnull(a.CMPQDate, '') != isnull(b.CMPQDate, ''), A.CMPQDate, null)
						, N_CMPQDate		= iif(isnull(a.CMPQDate, '') != isnull(b.CMPQDate, ''), b.CMPQDate, null)
						, O_EachConsApv		= iif(isnull(a.EachConsApv, '') != isnull(b.EachConsApv, ''), A.EachConsApv, null)
						, N_EachConsApv		= iif(isnull(a.EachConsApv, '') != isnull(b.EachConsApv, ''), b.EachConsApv, null)
						, O_MnorderApv		= iif(isnull(a.MnorderApv, '') != isnull(b.MnorderApv, ''), A.MnorderApv, null)
						, N_MnorderApv		= iif(isnull(a.MnorderApv, '') != isnull(b.MnorderApv, ''), b.MnorderApv, null)
						, O_SMnorderApv		= iif(isnull(a.SMnorderApv, '') != isnull(b.SMnorderApv, ''), A.SMnorderApv, null)
						, N_SMnorderApv		= iif(isnull(a.SMnorderApv, '') != isnull(b.SMnorderApv, ''), b.SMnorderApv, null)
						, O_MnorderApv2		= iif(isnull(a.MnorderApv2, '') != isnull(b.MnorderApv2, ''), A.MnorderApv2, null)
						, N_MnorderApv2		= iif(isnull(a.MnorderApv2, '') != isnull(b.MnorderApv2, ''), b.MnorderApv2, null)
						, O_Junk			= iif(isnull(a.Junk, '') != isnull(b.Junk, ''), A.Junk, 0)
						, N_Junk			= iif(isnull(a.Junk, '') != isnull(b.Junk, ''), b.Junk, 0)
						, O_KPILETA			= iif(isnull(a.KPILETA, '') != isnull(b.KPILETA, ''), A.KPILETA, null)
						, N_KPILETA			= iif(isnull(a.KPILETA, '') != isnull(b.KPILETA, ''), b.KPILETA, null)
						, O_LETA			= IIF(isnull(A.LETA, '') != isnull(B.LETA, ''), A.LETA, null)
						, N_LETA			= IIF(isnull(A.LETA, '') != isnull(B.LETA, ''), b.LETA, null)
						, O_Style			= IIF(isnull(a.StyleID, '') != isnull(b.StyleID, '') , a.StyleID, '')
						, N_Style			= IIF(isnull(a.StyleID, '') != isnull(b.StyleID, '') , b.StyleID, '')
						, O_CustPONo		= IIF(isnull(a.CustPONo, '') != isnull(b.CustPONo, '') , a.CustPONo, '')
						, N_CustPONo		= IIF(isnull(a.CustPONo, '') != isnull(b.CustPONo, '') , b.CustPONo, '')
						, O_ShipModeList	= IIF(isnull(a.ShipModeList, '') != isnull(b.ShipModeList, '') , a.ShipModeList, '')
						, N_ShipModeList	= IIF(isnull(a.ShipModeList, '') != isnull(b.ShipModeList, '') , b.ShipModeList, '')
				from Production.dbo.Orders a WITH (NOLOCK)
				inner join Trade_To_Pms.dbo.Orders b on a.id = b.id and a.FactoryID = b.FactoryID
				where	(isnull(A.QTY, 0) != isnull(B.QTY, 0) 
						OR isnull(A.BuyerDelivery, '') != isnull(B.BuyerDelivery, '')
						OR isnull(A.StyleID, '') != isnull(B.StyleID, '') 
						OR isnull(A.EachConsApv, '') != isnull(B.EachConsApv, '') 
						OR isnull(A.CMPQDate, '') != isnull(B.CMPQDate, '') 
						OR isnull(A.SciDelivery, '') != isnull(B.SciDelivery, '') 
						OR isnull(A.MnorderApv, '') != isnull(B.MnorderApv, '') 
						OR isnull(A.SMnorderApv, '') != isnull(B.SMnorderApv, '') 
						OR isnull(A.MnorderApv2, '') != isnull(B.MnorderApv2, '') 
						OR isnull(A.Junk, '') != isnull(B.Junk, '') 
						OR isnull(A.KPILETA, '') != isnull(B.KPILETA, '')
						OR isnull(A.LETA, '') != isnull(B.LETA, '')	
						OR isnull(A.CustPONo, '') != isnull(B.CustPONo, '')	
						OR isnull(A.ShipModeList, '') != isnull(B.ShipModeList, '')	
						)
						and b.FactoryID in (select ID from Factory)) s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToday
		when matched then 
			update set
				t.OrderID					= s.ID
				, t.FactoryID				= s.FactoryID
				, t.BrandID					= s.BrandID
				, t.MDivisionID				= s.MDivisionID
				, t.OriginalQty				= s.O_Qty
				, t.OriginalBuyerDelivery	= s.O_BuyerDelivery
				, t.OriginalSciDelivery		= s.O_SciDelivery
				, t.OriginalStyleID			= s.O_Style
				, t.OriginalCMPQDate		= s.O_CMPQDate
				, t.OriginalEachConsApv		= s.O_EachConsApv
				, t.OriginalMnorderApv		= s.O_MnorderApv
				, t.OriginalSMnorderApv		= s.O_SmnorderApv
				, t.OriginalLETA			= s.O_LETA
				, t.OriginalCustPONo		= s.O_CustPONo
				, t.OriginalShipModeList	= s.O_ShipModeList
				, t.NewQty					= s.N_Qty
				, t.NewBuyerDelivery		= s.N_BuyerDelivery
				, t.NewSciDelivery			= s.N_SciDelivery
				, t.NewStyleID				= s.N_Style
				, t.NewCMPQDate				= s.N_CMPQDate
				, t.NewEachConsApv			= s.N_EachConsApv
				, t.NewMnorderApv			= s.N_MnorderApv
				, t.NewSMnorderApv			= s.N_SMnorderApv
				, t.NewLETA					= s.N_LETA
				, t.NewCustPONo      		= s.N_CustPONo
				, t.NewShipModeList			= s.N_ShipModeList
				, t.KPILETA					= s.N_KPILETA
				, t.MnorderApv2				= s.N_MnorderApv2
				, t.JunkOrder				= s.N_Junk
				, t.UpdateDate				= @dToday
				, t.TransferDate			= @OldDate
		when not matched by target then 
			insert (
				OrderID					, FactoryID			, MDivisionID		, OriginalQty			, OriginalBuyerDelivery
				, OriginalSciDelivery	, OriginalStyleID	, OriginalCMPQDate	, OriginalEachConsApv	, OriginalMnorderApv
				, OriginalSMnorderApv	, OriginalLETA		, OriginalCustPONo
				, NewQty			    , NewBuyerDelivery	, NewSciDelivery
				, NewStyleID			, NewCMPQDate		, NewEachConsApv	, NewMnorderApv			, NewSMnorderApv
				, NewLETA				, NewCustPONo		 
				, KPILETA			    , MnorderApv2		, JunkOrder			, UpdateDate
				, TransferDate			, BrandID			
				, OriginalShipModeList	, NewShipModeList
			) values (
				s.ID					, s.FactoryID		, s.MDivisionID		, s.O_Qty				, s.O_BuyerDelivery
				, s.O_SciDelivery		, s.O_Style			, s.O_CMPQDate		, s.O_EachConsApv		, s.O_MnorderApv
				, s.O_SMnorderApv		, s.O_LETA			, s.O_CustPONo
				, s.N_Qty			    , s.N_BuyerDelivery	, s.N_SciDelivery
				, s.N_Style			  	, s.N_CMPQDate		, s.N_EachConsApv	, s.N_MnorderApv		, s.N_SMNorderApv
				, s.N_LETA				, s.N_CustPONo
				, s.N_KPILETA		    , s.N_MnorderApv2	, s.N_Junk			, @dToday
				, @OldDate				, s.BrandID
				, s.O_ShipModeList		, s.N_ShipModeList
			);

        ----5.No Change!
		Merge Production.dbo.OrderComparisonList as t
		Using Production.dbo.Factory as s
		on t.factoryid=s.id and UpdateDate =@dToDay
		when not matched by Target then 
			insert (
				OrderId			, UpdateDate	, TransferDate	, MDivisionID  , FactoryID
			) values (
				'No Change!'	, @dToDay		, @OldDate		, s.MDivisionID, s.ID
			);
		----------不變動上面規則，再補styleid
		Merge Production.dbo.OrderComparisonList as t
		Using (	select a.* 
				from Trade_To_Pms.dbo.Orders a
				left join #tmpOrders b on a.ID = b.ID
				where a.FactoryID in (select ID from Production.dbo.Factory)) as s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToDay
		when matched then
			update set 
			t.OriginalStyleID	= s.StyleID;
-----------------------------------------------------------------------------------------------------------
---------------------Order--------------------------------------
		--------------Order.id= AOrder.id  if eof()
		declare @OrderT table (ID varchar(13),isInsert bit) 

		Merge Production.dbo.Orders as t
		Using   #TOrder as s
		on t.id=s.id
		when matched then 
		update set
				t.ProgramID				= s.ProgramID ,
				t.ProjectID				= s.ProjectID ,
				t.Category				= s.Category ,
				t.OrderTypeID			= s.OrderTypeID ,
				t.BuyMonth				= s.BuyMonth ,
				t.Dest					= s.Dest ,
				t.Model					= s.Model ,
				t.HsCode1				= s.HsCode1 ,
				t.HsCode2				= s.HsCode2 ,
				t.PayTermARID			= s.PayTermARID ,
				t.ShipTermID			= s.ShipTermID ,
				t.ShipModeList			= s.ShipModeList ,
				t.PoPrice				= s.PoPrice ,
				t.CFMPrice				= s.CFMPrice ,
				t.CurrencyID			= s.CurrencyID ,
				t.Commission			= s.Commission ,
				t.BrandAreaCode			= s.BrandAreaCode ,
				t.BrandFTYCode			= s.BrandFTYCode ,
				t.CTNQty				= s.CTNQty ,
				t.CustCDID				= s.CustCDID ,
				t.CustPONo				= s.CustPONo ,
				t.Customize1			= s.Customize1 ,
				t.Customize2			= s.Customize2 ,
				t.Customize3			= s.Customize3 ,
				t.CMPUnit				= s.CMPUnit ,
				t.CMPPrice				= s.CMPPrice ,
				t.CMPQDate				= s.CMPQDate ,
				t.CMPQRemark			= s.CMPQRemark ,
				t.EachConsApv			= s.EachConsApv ,
				t.MnorderApv			= s.MnorderApv ,
				t.CRDDate				= s.CRDDate ,
				t.InitialPlanDate		= s.InitialPlanDate ,
				t.PlanDate				= s.PlanDate ,
				t.FirstProduction		= s.FirstProduction ,
				t.FirstProductionLock	= s.FirstProductionLock ,
				t.OrigBuyerDelivery		= s.OrigBuyerDelivery ,
				t.ExCountry				= s.ExCountry ,
				t.InDCDate				= s.InDCDate ,
				t.CFMShipment			= s.CFMShipment ,
				t.PFETA					= s.PFETA ,
				t.PackLETA				= s.PackLETA ,
				t.LETA					= s.LETA ,
				t.MRHandle				= s.MRHandle ,
				t.SMR					= s.SMR ,
				t.ScanAndPack			= s.ScanAndPack ,
				t.VasShas				= s.VasShas ,
				t.SpecialCust			= s.SpecialCust ,
				t.TissuePaper			= s.TissuePaper ,
				t.Packing				= s.Packing ,
				--t.SDPDate				= s.SDPDate, --工廠交期只需要INSERT填預設值,不須UPDATE
				t.MarkFront				= s.MarkFront ,
				t.MarkBack				= s.MarkBack ,
				t.MarkLeft				= s.MarkLeft ,
				t.MarkRight				= s.MarkRight ,
				t.Label					= s.Label ,
				t.OrderRemark			= s.OrderRemark ,
				t.ArtWorkCost			= s.ArtWorkCost ,
				t.StdCost				= s.StdCost ,
				t.CtnType				= s.CtnType ,
				t.FOCQty				= s.FOCQty ,
				t.SMnorderApv			= s.SMnorderApv ,
				t.FOC					= s.FOC ,
				t.MnorderApv2			= s.MnorderApv2 ,
				t.Packing2				= s.Packing2 ,
				t.SampleReason			= s.SampleReason ,
				t.RainwearTestPassed	= s.RainwearTestPassed ,
				t.SizeRange				= s.SizeRange ,
				t.MTLComplete			= s.MTLComplete ,
				t.SpecialMark			= s.SpecialMark ,
				t.OutstandingRemark		= iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingRemark,s.OutstandingRemark),
				t.OutstandingInCharge	= iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingInCharge,s.OutstandingInCharge),
				t.OutstandingDate		= iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingDate,s.OutstandingDate),
				t.OutstandingReason		= iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingReason,s.OutstandingReason),
				t.StyleUkey				= s.StyleUkey ,
				t.POID					= s.POID ,
				t.OrderComboID			= s.OrderComboID,
				t.IsNotRepeatOrMapping	= s.IsNotRepeatOrMapping ,
				t.SplitOrderId			= s.SplitOrderId ,
				t.FtyKPI				= s.FtyKPI ,
				t.EditName				= iif((s.EditDate <= t.EditDate AND s.EditDate is null) OR (s.EditDate is not null AND s.EditDate <= t.EditDate),t.EditName, s.EditName) ,
				t.EditDate				= iif((s.EditDate <= t.EditDate AND s.EditDate is null) OR (s.EditDate is not null AND s.EditDate <= t.EditDate),t.EditDate, s.EditDate) ,
				t.IsForecast			= s.IsForecast ,
				t.PulloutComplete		= iif((s.GMTComplete='C' OR s.GMTComplete='S') and t.PulloutComplete=0  ,1,t.PulloutComplete),
				t.PFOrder				= s.PFOrder ,
				t.KPILETA				= s.KPILETA ,
				t.MTLETA				= s.MTLETA ,
				t.SewETA				= s.SewETA ,
				t.PackETA				= s.PackETA ,
				t.MTLExport				= s.MTLExport ,
				t.DoxType				= s.DoxType ,
				t.MDivisionID			= s.MDivisionID ,
				t.KPIChangeReason		= s.KPIChangeReason ,
				t.MDClose				= iif((s.GMTComplete='C' OR s.GMTComplete='S') and t.PulloutComplete=0  ,@dToDay,t.MDClose),
				t.CPUFactor				= s.CPUFactor ,
				t.SizeUnit				= s.SizeUnit ,
				t.CuttingSP				= s.CuttingSP ,
				t.IsMixMarker			= s.IsMixMarker ,
				t.EachConsSource		= s.EachConsSource ,
				t.KPIEachConsApprove	= s.KPIEachConsApprove ,
				t.KPICmpq				= s.KPICmpq ,
				t.KPIMNotice			= s.KPIMNotice ,
				t.GMTComplete			= s.GMTComplete  ,
				t.GFR					= s.GFR	,
				t.FactoryID				= s.FactoryID,
				t.BrandID				= s.BrandID, 
				t.StyleID				= s.StyleID, 
				t.SeasonID				= s.SeasonID, 
				t.BuyerDelivery			= s.BuyerDelivery,
				t. SciDelivery			= s.SciDelivery, 
				t.CFMDate				= s.CFMDate, 
				t.Junk					= s.Junk,
				t.CdCodeID				= s.CdCodeID, 
				t.CPU					= s.CPU, 
				t.Qty					= s.Qty, 
				t.StyleUnit				= s.StyleUnit, 				
				t.AddName				= s.AddName, 
				t.AddDate				= s.AddDate,
				t.FtyGroup              = s.FTY_Group,
				t.ForecastSampleGroup   = s.ForecastSampleGroup,				
				t.DyeingLoss			= s.DyeingLoss,
				t.SubconInType = '0',
				t.LastProductionDate       = s.LastProductionDate,				
				t.EstPODD       = s.EstPODD,
				t.AirFreightByBrand    = s.AirFreightByBrand,
				t.AllowanceComboID = s.AllowanceComboID,
				t.ChangeMemoDate       = s.ChangeMemoDate,
				t.ForecastCategory     = s.ForecastCategory,
				t.OnSiteSample		   = s.OnSiteSample
		when not matched by target then
		insert (
			ID						, BrandID				, ProgramID				, StyleID				, SeasonID
			, ProjectID				, Category				, OrderTypeID			, BuyMonth				, Dest
			, Model					, HsCode1				, HsCode2				, PayTermARID			, ShipTermID
			, ShipModeList			, CdCodeID				, CPU					, Qty					, StyleUnit
			, PoPrice				, CFMPrice				, CurrencyID			, Commission			, FactoryID
			, BrandAreaCode			, BrandFTYCode			, CTNQty				, CustCDID				, CustPONo
			, Customize1			, Customize2			, Customize3			, CFMDate				, BuyerDelivery
			, SciDelivery			, SewOffLine			, CutInLine				, CutOffLine			
			, CMPUnit				, CMPPrice				, CMPQDate				, CMPQRemark			, EachConsApv
			, MnorderApv			, CRDDate				, InitialPlanDate		, PlanDate				, FirstProduction
			, FirstProductionLock	, OrigBuyerDelivery		, ExCountry				, InDCDate				, CFMShipment
			, PFETA					, PackLETA				, LETA					, MRHandle				, SMR
			, ScanAndPack			, VasShas				, SpecialCust			, TissuePaper			, Junk
			, Packing				, MarkFront				, MarkBack				, MarkLeft				, MarkRight
			, Label					, OrderRemark			, ArtWorkCost			, StdCost				, CtnType
			, FOCQty				, SMnorderApv			, FOC					, MnorderApv2			, Packing2
			, SampleReason			, RainwearTestPassed	, SizeRange				, MTLComplete			, SpecialMark
			, OutstandingRemark		, OutstandingInCharge	, OutstandingDate		, OutstandingReason		, StyleUkey
			, POID					, OrderComboID			, IsNotRepeatOrMapping	, SplitOrderId			, FtyKPI				
			, AddName				, AddDate				, EditName				, EditDate				, IsForecast			
			, GMTComplete			, PFOrder				, KPILETA				, MTLETA				
			, SewETA				, PackETA				, MTLExport				, DoxType						
			, MDivisionID			, MCHandle				, KPIChangeReason		, MDClose				, CPUFactor				
			, SizeUnit				, CuttingSP				, IsMixMarker			, EachConsSource		, KPIEachConsApprove	
			, KPICmpq				, KPIMNotice			, GFR					, SDPDate				, PulloutComplete		
			, SewINLINE				, FtyGroup				, ForecastSampleGroup	, DyeingLoss			, SubconInType
			, LastProductionDate	, EstPODD				, AirFreightByBrand		, AllowanceComboID      , ChangeMemoDate
			, ForecastCategory		, OnSiteSample

		) values (
			s.ID					, s.BrandID				, s.ProgramID			, s.StyleID				, s.SeasonID 
			, s.ProjectID			, s.Category			, s.OrderTypeID			, s.BuyMonth			, s.Dest 
			, s.Model				, s.HsCode1				, s.HsCode2				, s.PayTermARID			, s.ShipTermID 
			, s.ShipModeList		, s.CdCodeID			, s.CPU					, s.Qty					, s.StyleUnit 
			, s.PoPrice				, s.CFMPrice			, s.CurrencyID			, s.Commission			, s.FactoryID 
			, s.BrandAreaCode		, s.BrandFTYCode		, s.CTNQty				, s.CustCDID			, s.CustPONo 
			, s.Customize1			, s.Customize2			, s.Customize3			, s.CFMDate				, s.BuyerDelivery 
			, s.SciDelivery			, s.SewOffLine			, s.CutInLine			, s.CutOffLine			
			, s.CMPUnit				, s.CMPPrice			, s.CMPQDate			, s.CMPQRemark			, s.EachConsApv 
			, s.MnorderApv			, s.CRDDate				, s.InitialPlanDate		, s.PlanDate			, s.FirstProduction 
			, s.FirstProductionLock , s.OrigBuyerDelivery	, s.ExCountry			, s.InDCDate			, s.CFMShipment 
			, s.PFETA				, s.PackLETA			, s.LETA				, s.MRHandle			, s.SMR 
			, s.ScanAndPack			, s.VasShas				, s.SpecialCust			, s.TissuePaper			, s.Junk 
			, s.Packing				, s.MarkFront			, s.MarkBack			, s.MarkLeft			, s.MarkRight 
			, s.Label				, s.OrderRemark			, s.ArtWorkCost			, s.StdCost				, s.CtnType 
			, s.FOCQty				, s.SMnorderApv			, s.FOC					, s.MnorderApv2			, s.Packing2 
			, s.SampleReason		, s.RainwearTestPassed	, s.SizeRange			, s.MTLComplete			, s.SpecialMark 
			, s.OutstandingRemark	, s.OutstandingInCharge , s.OutstandingDate		, s.OutstandingReason	, s.StyleUkey 
			, s.POID				, s.OrderComboID		, s.IsNotRepeatOrMapping, s.SplitOrderId		, s.FtyKPI
			, s.AddName 			, s.AddDate				, s.EditName			, s.EditDate			, s.IsForecast			
			, s.GMTComplete 		, s.PFOrder				, s.KPILETA				, s.MTLETA				
			, s.SewETA				, s.PackETA				, s.MTLExport			, s.DoxType				
			, s.MDivisionID 		, S.MCHandle			, s.KPIChangeReason		, S.MDClose				, s.CPUFactor			
			, s.SizeUnit			, s.CuttingSP			, s.IsMixMarker			, s.EachConsSource		, s.KPIEachConsApprove	
			, s.KPICmpq 			, s.KPIMNotice			, s.GFR					, s.SDPDate				, s.PulloutComplete		
			, s.SewINLINE           , s.FTY_Group			, s.ForecastSampleGroup , s.DyeingLoss          , '0'
			, s.LastProductionDate	, s.EstPODD				, s.AirFreightByBrand	, s.AllowanceComboID    , s.ChangeMemoDate
			, s.ForecastCategory	, s.OnSiteSample
		)
		output inserted.id, iif(deleted.id is null,1,0) into @OrderT; --將insert =1 , update =0 把改變過的id output;

	--------------Order_Qty--------------------------Qty BreakDown

	Merge Production.dbo.Order_Qty as t
	Using (select a.* from Trade_To_Pms.dbo.order_qty a WITH (NOLOCK) inner join #Torder  b on a.id=b.id) as s
	on t.id=s.id AND T.ARTICLE=S.ARTICLE AND t.sizeCode=s.sizeCode
	when matched then
		update set
			t.Qty		= s.Qty ,
			t.AddName	= s.AddName ,
			t.AddDate	= s.AddDate ,
			t.EditName	= s.EditName ,
			t.EditDate	= s.EditDate ,
			t.OriQty	= s.OriQty
	when not matched by target then 
		insert (
			ID			, Article	, SizeCode		, Qty		,AddName 
			, AddDate	, EditName	, EditDate		, OriQty 
		) values (
			s.ID		, s.Article	, Rtrim(s.SizeCode)	, s.Qty		,s.AddName 
			, s.AddDate	, s.EditName, s.EditDate	, s.OriQty 
		)
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then
		delete;

	----------Order_Qty_Garment--------------
	Merge Production.dbo.Order_Qty_Garment as t
	Using (
		select a.*
			   , OrdersJunk = b.Junk
		from Trade_To_Pms.dbo.Order_Qty_Garment a With (NoLock) 
		inner join #TOrder b on a.id = b.id
	) as s on t.ID = s.ID
			  and t.OrderIDFrom = s.OrderIDFrom
			  and t.Article = s.Article
			  and t.SizeCode = s.SizeCode
  	when matched then 
  		update set
  			t.Qty			= s.Qty 
			, t.AddName		= s.AddName 
			, t.AddDate		= s.AddDate 
			, t.EditName	= s.EditName 
			, t.EditDate	= s.EditDate
			, t.Junk 		= s.OrdersJunk
	when not matched by target then
		insert (
			ID 			, OrderIDFrom	, Article 		, SizeCode 		, Qty
			, AddName 	, AddDate		, EditName 		, EditDate		, Junk
		) values (
			s.ID 		, s.OrderIDFrom	, s.Article 	, s.SizeCode 	, s.Qty
			, s.AddName , s.AddDate		, s.EditName 	, s.EditDate	, 0
		)
	when not matched by source and t.ID in (select ID from #TOrder) then 
		update set
			t.Junk = 1;    
    
	----------Order_QtyShip--------------
	Merge Production.dbo.Order_QtyShip as t
	using (select a.* from Trade_To_Pms.dbo.Order_QtyShip a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
	on t.id=s.id and t.seq=s.seq
		when matched then 
		update set
			t.ShipmodeID	= s.ShipmodeID ,
			t.BuyerDelivery = s.BuyerDelivery ,
			t.FtyKPI		= s.FtyKPI ,
			t.ReasonID		= s.ReasonID ,
			t.Qty			= s.Qty ,
			t.AddName		= s.AddName ,
			t.AddDate		= s.AddDate ,
			t.EditName		= s.EditName ,
			t.EditDate		= s.EditDate,
			t.OriQty		= s.OriQty
	when not matched by target then
		insert  (  
			Id		, Seq		, ShipmodeID	, BuyerDelivery		, FtyKPI		, ReasonID 
			, Qty	, AddName	, AddDate		, EditName			, EditDate		, OriQty 
		) values (
			s.Id	, s.Seq		, s.ShipmodeID	, s.BuyerDelivery	, s.FtyKPI		, s.ReasonID 
			, s.Qty	, s.AddName	, s.AddDate		, s.EditName		, s.EditDate	,s.OriQty
		)
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;

	-----------Order_QtyShip_Detail--------------------------調整: 來源比對Production表頭資料 
		Merge Production.dbo.Order_QtyShip_detail as t
		Using (select a.* from Trade_To_Pms.dbo.Order_QtyShip_detail as a WITH (NOLOCK) inner join #TOrder b on a.id=b.id  ) as s
		on t.ukey=s.ukey
		when matched then
			update set
				t.id		= s.id,
				t.Seq		= s.Seq ,
				t.Article	= s.Article ,
				t.SizeCode	= s.SizeCode ,
				t.Qty		= s.Qty ,
				t.AddName	= s.AddName ,
				t.AddDate	= s.AddDate ,
				t.EditName	= s.EditName ,
				t.EditDate	= s.EditDate ,
				t.OriQty	= s.OriQty
		when not matched by target then 
			insert (
				Id			, Seq			, Article		, SizeCode		, Qty	, AddName 
				, AddDate	, EditName		, EditDate		, Ukey			, OriQty 
			) values (
				s.Id		, s.Seq			, s.Article		, s.SizeCode	, s.Qty	, s.AddName 
				, s.AddDate	, s.EditName	, s.EditDate	, s.Ukey		, s.OriQty
			)
		when not matched by source  AND T.ID IN (SELECT ID FROM #Torder) then 
		delete;

		-----------------Order_UnitPrice------------
		Merge Production.dbo.Order_UnitPrice as t
		Using (select a.* from Trade_To_Pms.dbo.Order_UnitPrice a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when Matched then
			update set			
				t.POPrice	= s.POPrice ,
				t.QuotCost	= s.QuotCost ,
				t.DestPrice	= s.DestPrice ,
				t.AddName	= s.AddName ,
				t.AddDate	= s.AddDate ,
				t.EditName	= s.EditName ,
				t.EditDate	= s.EditDate 
		when not Matched by target then
			insert (
				Id				, Article	, SizeCode		, POPrice		, QuotCost 
				, DestPrice		, AddName	, AddDate		, EditName		, EditDate 
			) values (
				s.Id			, s.Article	, s.SizeCode	, s.POPrice		, s.QuotCost 
				, s.DestPrice	, s.AddName	, s.AddDate		, s.EditName	, s.EditDate 
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		--------------Order_TmsCost-----TMS & Cost
		Merge  Production.dbo.Order_TmsCost as t
		Using  (select a.* from Trade_To_Pms.dbo.Order_TmsCost a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.ArtworkTypeID=s.ArtworkTypeID
		when matched then 
			update set			
				t.Seq			= s.Seq,
				t.Qty			= s.Qty,
				t.ArtworkUnit	= s.ArtworkUnit,
				t.TMS			= s.TMS,
				t.Price			= s.Price,
				t.AddName		= s.AddName,
				t.AddDate		= s.AddDate,
				t.TPEEditName		= s.EditName,
				t.TPEEditDate		= s.EditDate		
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		------------------  insert Order_TmsCost
		INSERT INTO Production.dbo.Order_TmsCost (
			ID
			, ArtworkTypeID
			, Seq
			, Qty
			, ArtworkUnit
			, TMS
			, Price
			, InhouseOSP
			, LocalSuppID
			, AddName
			, AddDate
			, TPEEditName
			, TPEEditDate
		)
		SELECT	A.ID
				, A.ArtworkTypeID
				, A.Seq
				, A.Qty
				, A.ArtworkUnit
				, A.TMS
				, A.Price
				, C.InhouseOSP
				, IIF(C.InhouseOSP='O', (
											SELECT top 1 LocalSuppId
											from Style_Artwork_Quot saq with(nolock)
											inner join Style_artwork sa with(nolock) on saq.ukey = sa.ukey
											inner join Trade_To_Pms.DBO.Order_TmsCost ot WITH (NOLOCK) on sa.ArtworkTypeID=ot.ArtworkTypeID
											where sa.styleukey = b.styleukey and ot.id = a.id and ot.ArtworkTypeID = a.ArtworkTypeID
											Order by sa.ukey
										)
									  , (SELECT top 1 LocalSuppID 
										 FROM Production.dbo.Order_TmsCost WITH (NOLOCK) WHERE ID=A.ID)
					 )
				, A.AddName
				, A.AddDate
				, A.EditName
				, A.EditDate 
		FROM Trade_To_Pms.dbo.Order_TmsCost A WITH (NOLOCK)
		INNER JOIN #TOrder B ON A.ID=B.ID
		INNER JOIN Production.dbo.ArtworkType C WITH (NOLOCK) ON A.ArtworkTypeID=C.ID
		LEFT JOIN Production.dbo.Order_TmsCost D WITH (NOLOCK) ON D.id = a.Id and D.ArtworkTypeID = A.ArtworkTypeID
		where D.id is null
	
	
		-----------------Order_SizeCode---------------------------尺寸表 Size Spec(存尺寸碼)
		--20170110 willy 調整順序: 刪除>修改>新增
		Merge Production.dbo.Order_SizeCode as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id where a.sizecode is not null) as s
		on t.id=s.id and t.sizecode=s.sizecode and t.ukey=s.ukey
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete
		when matched then
			update set
				t.Seq		= s.Seq,
				t.SizeGroup	= s.SizeGroup,
				t.AddDate	= s.AddDate,
				t.EditDate	= s.EditDate
		When not matched by target then 
			insert (
				Id		, Seq	, SizeGroup		, SizeCode		, ukey		, AddDate		, EditDate
			) values (
				s.Id	, s.Seq	, s.SizeGroup	, s.SizeCode	, s.ukey		, s.AddDate		, s.EditDate
			);

		----------------Order_Sizeitem------------------------------尺寸表 Size Spec(存量法資料)
		Merge Production.dbo.Order_Sizeitem as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Sizeitem a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 			
				t.SizeUnit	= s.SizeUnit,
				t.SizeDesc	= s.Description,
				t.TolMinus	= s.TolMinus,
				t.TolPlus	= s.TolPlus
		when not matched by Target then
			insert (
				Id		, SizeItem		, SizeUnit		, SizeDesc		, ukey ,TolMinus ,TolPlus
			) values (
				s.Id	, s.SizeItem	, s.SizeUnit	, s.Description	, s.ukey,s.TolMinus ,s.TolPlus
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
	
		-------------Order_SizeSpec--------------------------------尺寸表 Size Spec(存尺寸碼)
		Merge Production.dbo.Order_SizeSpec as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeSpec a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on  t.ukey=s.ukey
		when matched then 
			update set
				t.SizeSpec	= s.SizeSpec
		when not matched by target then
			insert (
				Id		, SizeItem		, SizeCode		, SizeSpec		, ukey
			) values (
				s.Id	, s.SizeItem	, s.SizeCode	, s.SizeSpec	, s.ukey
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		-------------Order_SizeSpec_OrderCombo--------------------------------
		Merge Production.dbo.Order_SizeSpec_OrderCombo as t
		Using (
			select a.*
			from Trade_To_Pms.dbo.Order_SizeSpec_OrderCombo a With (NoLock)
			inner join #TOrder b on a.id = b.id 
		) as s on t.ukey = s.ukey
		when matched then 
			update set
				t.Id				= s.Id
				, t.OrderComboID	= s.OrderComboID
				, t.SizeItem		= s.SizeItem
				, t.SizeCode		= s.SizeCode
				, t.SizeSpec		= s.SizeSpec
		when not matched by target then
			insert (
				Id		, OrderComboID		, SizeItem		, SizeCode		, SizeSpec
				, Ukey
			) values (
				s.Id	, s.OrderComboID	, s.SizeItem	, s.SizeCode	, s.SizeSpec
				, s.Ukey
			)
		when not matched by source and T.id in (Select ID From #Torder) then 
			delete;

		------------Order_ColorCombo---------------(主料配色表)
		Merge Production.dbo.Order_ColorCombo as t
		Using (select a.* from Trade_To_Pms.dbo.Order_ColorCombo a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.FabricPanelCode=s.FabricPanelCode
		when matched then 
			update set
				t.ColorID		= s.ColorID,
				t.FabricCode	= s.FabricCode,
				t.PatternPanel	= s.PatternPanel,
				t.AddName		= s.AddName,
				t.AddDate		= s.AddDate,
				t.EditName		= s.EditName,
				t.EditDate		= s.EditDate,
				t.FabricType 	= s.FabricType
		when not matched by target then 
			insert (
				Id					, Article	, ColorID	, FabricCode	, FabricPanelCode
				, PatternPanel		, AddName	, AddDate	, EditName		, EditDate
				, FabricType
			) values (
				s.Id				, s.Article	, s.ColorID	, s.FabricCode	, s.FabricPanelCode
				, s.PatternPanel	, s.AddName	, s.AddDate	, s.EditName	, s.EditDate
				, s.FabricType
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
			
	
		-------------Order_FabricCode------------------部位vs布別vsQT
		Merge Production.dbo.Order_FabricCode as t
		Using (select a.* from Trade_To_Pms.dbo.Order_FabricCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.FabricPanelCode=s.FabricPanelCode
		when matched then 
			update set
				t.PatternPanel		= s.PatternPanel,
				t.FabricCode		= s.FabricCode,
				t.FabricPanelCode	= s.FabricPanelCode,
				t.AddName			= s.AddName,
				t.AddDate			= s.AddDate,
				t.EditName			= s.EditName,
				t.EditDate			= s.EditDate,
				t.Order_BOFUkey		= s.Order_BOFUkey
		when not matched by target then 
			insert (
				Id			, PatternPanel		, FabricCode	, FabricPanelCode	, AddName
				, AddDate	, EditName			, EditDate		, Order_BOFUkey
			) values (
				s.Id		, s.PatternPanel	, s.FabricCode	, s.FabricPanelCode	, s.AddName
				, s.AddDate	, s.EditName		, s.EditDate	, s.Order_BOFUkey
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

-------------Order_FabricCode_QT-----------------
	Merge Production.dbo.Order_FabricCode_QT as t
	Using (select a.* from Trade_To_Pms.dbo.Order_FabricCode_QT a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
	on t.id=s.id and t.FabricPanelCode=s.FabricPanelCode and t.seqno=s.seqno
	when matched then 
	update set
		t.QTFabricPanelCode	= s.QTFabricPanelCode,
		t.AddName			= s.AddName,
		t.AddDate			= s.AddDate,
		t.EditName			= s.EditName,
		t.EditDate			= s.EditDate
	when not matched by target then 
		insert (
			Id			, FabricPanelCode	, SeqNO		, QTFabricPanelCode		, AddName
			, AddDate	, EditName			, EditDate
		) values (
			s.Id		, s.FabricPanelCode	, s.SeqNO	, s.QTFabricPanelCode	, s.AddName
			, s.AddDate	, s.EditName		, s.EditDate
		)
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
		delete;

		-------------Order_Bof -----------------------Bill of Fabric

		Merge Production.dbo.Order_Bof as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Bof a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
				t.id					= s.id,
				t.FabricCode			= s.FabricCode,
				t.Refno					= s.Refno,
				t.SCIRefno				= s.SCIRefno,
				t.SuppID				= s.SuppID,
				t.ConsPC				= s.ConsPC,
				t.Seq1					= s.Seq1,
				t.Kind					= s.Kind,
				t.Remark				= s.Remark,
				t.LossType				= s.LossType,
				t.LossPercent			= s.LossPercent,
				t.RainwearTestPassed	= s.RainwearTestPassed,
				t.HorizontalCutting		= s.HorizontalCutting,
				t.ColorDetail			= s.ColorDetail,
				t.AddName				= s.AddName,
				t.AddDate				= s.AddDate,
				t.EditName				= s.EditName,
				t.EditDate				= s.EditDate,
				t.SpecialWidth          = s.SpecialWidth
		when not matched by target then 
			insert (
				Id				, FabricCode	, Refno					, SCIRefno				, SuppID
				, ConsPC		, Seq1			, Kind					, Ukey					, Remark
				, LossType		, LossPercent	, RainwearTestPassed	, HorizontalCutting		, ColorDetail
				, AddName		, AddDate		, EditName				, EditDate              , SpecialWidth 
			) values (
				s.Id			, s.FabricCode	, s.Refno				, s.SCIRefno			, s.SuppID
				, s.ConsPC		, s.Seq1		, s.Kind				, s.Ukey				, s.Remark
				, s.LossType	, s.LossPercent	, s.RainwearTestPassed	, s.HorizontalCutting	, s.ColorDetail
				, s.AddName		, s.AddDate		, s.EditName			, s.EditDate            , s.SpecialWidth
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		---------Order_Bof_Expend--------------Bill of Fabric -用量展開
		Merge Production.dbo.Order_Bof_Expend as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Bof_Expend a WITH (NOLOCK)
		inner join #Torder b on a.id=b.id) as s
		--inner join Production.dbo.Order_Bof b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
				t.Id				= s.Id,
				t.Order_BOFUkey		= s.Order_BOFUkey,
				t.ColorId			= s.ColorId,
				t.SuppColor			= s.SuppColor,
				t.OrderQty			= s.OrderQty,
				t.Price				= s.Price,
				t.UsageQty			= s.UsageQty,
				t.UsageUnit			= s.UsageUnit,
				t.Width				= s.Width,
				t.SysUsageQty			= s.SysUsageQty,
				t.QTFabricPanelCode	= s.QTFabricPanelCode,
				t.Remark			= s.Remark,
				t.OrderIdList		= s.OrderIdList,
				t.AddName			= s.AddName,
				t.AddDate			= s.AddDate,
				t.EditName			= s.EditName,
				t.EditDate	= s.EditDate,
				t.Special           = s.Special
		when not matched by target then 
			insert ( 
				Id						, Order_BOFUkey		, ColorId		, SuppColor		, OrderQty
				, Price					, UsageQty			, UsageUnit		, Width			, SysUsageQty
				, QTFabricPanelCode		, Remark			, OrderIdList	, AddName		, AddDate
				, EditName				, EditDate			, UKEY          , Special
			) values (
				s.Id					, s.Order_BOFUkey	, s.ColorId		, s.SuppColor	, s.OrderQty
				, s.Price				, s.UsageQty		, s.UsageUnit	, s.Width		, s.SysUsageQty
				, s.QTFabricPanelCode	, s.Remark			, s.OrderIdList	, s.AddName		, s.AddDate
				, s.EditName			, s.EditDate		, s.UKEY        , s.Special
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		-------------Order_BOA------------------Bill of Accessory

		Merge Production.dbo.Order_BOA as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
				t.id					= s.id,
				t.Refno					= s.Refno,
				t.SCIRefno				= s.SCIRefno,
				t.SuppID				= s.SuppID,
				t.Seq					= s.Seq1,
				t.ConsPC				= s.ConsPC,
				t.BomTypeSize			= s.BomTypeSize,
				t.BomTypeColor			= s.BomTypeColor,
				t.FabricPanelCode		= s.FabricPanelCode,
				t.PatternPanel			= s.PatternPanel,
				t.SizeItem				= s.SizeItem,
				t.BomTypeZipper			= s.BomTypeZipper,
				t.Remark				= s.Remark,
				t.ProvidedPatternRoom	= s.ProvidedPatternRoom,
				t.ColorDetail			= s.ColorDetail,
				t.isCustCD				= s.isCustCD,
				t.lossType				= s.lossType,
				t.LossPercent			= s.LossPercent,
				t.LossQty				= s.LossQty,
				t.LossStep				= s.LossStep,
				t.AddName				= s.AddName,
				t.AddDate				= s.AddDate,
				t.EditName				= s.EditName,
				t.EditDate				= s.EditDate,
				t.SizeItem_Elastic		= s.SizeItem_Elastic,
				t.BomTypePo				= s.BomTypePo,
				t.Keyword				= s.Keyword,
				t.Seq1					= s.Seq1
		when not matched by target then
			insert (
				Id					, Ukey				, Refno					, SCIRefno				, SuppID
				, Seq				, ConsPC			, BomTypeSize			, FabricPanelCode		, PatternPanel			
				, SizeItem			, BomTypeZipper		, Remark				, ProvidedPatternRoom	, ColorDetail			
				, isCustCD			, lossType			, LossPercent			, LossQty				, LossStep				
				, AddName			, AddDate			, EditName				, EditDate				, SizeItem_Elastic		
				, BomTypePo			, Keyword			, Seq1
			) values (
				s.Id				, s.Ukey			, s.Refno				, s.SCIRefno			, s.SuppID
				, s.Seq1			, s.ConsPC			, s.BomTypeSize			, s.FabricPanelCode		, s.PatternPanel		
				, s.SizeItem		, s.BomTypeZipper	, s.Remark				, s.ProvidedPatternRoom	, s.ColorDetail			
				, s.isCustCD		, s.lossType		, s.LossPercent			, s.LossQty				, s.LossStep			
				, s.AddName			, s.AddDate			, s.EditName			, s.EditDate			, s.SizeItem_Elastic	
				, s.BomTypePo		, s.Keyword			, s.Seq1
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
		
		-----------------Order_BoA_Article----------------
		Merge Production.dbo.Order_BoA_Article as t
		Using (
			select a.* 
			from Trade_To_Pms.dbo.Order_BoA_Article a With (NoLock)
			inner join #Torder b on a.id = b.id
		) as s on t.ukey = s.ukey
		when matched then
			update set
				t.Id				= s.Id
				, t.Order_BoAUkey	= s.Order_BoAUkey
				, t.Article			= s.Article
				, t.AddName			= s.AddName
				, t.AddDate			= s.AddDate
				, t.EditName		= s.EditName
				, t.EditDate		= s.EditDate
		when not matched then 
			insert (
				Id				, Order_BoaUkey		, Article	, AddName	, AddDate
				, EditName		, EditDate			, Ukey
			) values (
				s.Id			, s.Order_BoaUkey	, s.Article	, s.AddName	, s.AddDate
				, s.EditName	, s.EditDate		, s.Ukey
			)		
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;		
						
		-----------------Order_BOA_Expend----------------Bill of accessory -用量展開
		Merge Production.dbo.Order_BOA_Expend as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_Expend a WITH (NOLOCK)	
		inner join #Torder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
				t.id				= s.id,
				t.Order_BOAUkey		= s.Order_BOAUkey,
				t.OrderQty			= s.OrderQty,
				t.Refno				= s.Refno,
				t.SCIRefno			= s.SCIRefno,
				t.Price				= s.Price,
				t.UsageQty			= s.UsageQty,
				t.UsageUnit			= s.UsageUnit,
				t.Article			= s.Article,
				t.ColorId			= s.ColorId,
				t.SuppColor			= s.SuppColor,
				t.SizeCode			= s.SizeCode,
				t.Sizespec			= s.Sizespec,
				t.SizeUnit			= s.SizeUnit,
				t.OrderIdList		= s.OrderIdList,
				t.SysUsageQty		= s.SysUsageQty,
				t.Remark			= s.Remark,			
				t.BomZipperInsert	= s.BomZipperInsert,			
				t.BomCustPONo		= s.BomCustPONo,
				t.AddName			= s.AddName,
				t.AddDate			= s.AddDate,
				t.EditName			= s.EditName,
				t.EditDate			= s.EditDate,
				t.Keyword			= s.Keyword
		when not matched by target then
			insert (
				Id				, UKEY			, Order_BOAUkey		, OrderQty			, Refno
				, SCIRefno		, Price			, UsageQty			, UsageUnit			, Article
				, ColorId		, SuppColor		, SizeCode			, Sizespec			, SizeUnit
				, OrderIdList	, SysUsageQty	, Remark			, BomZipperInsert	, BomCustPONo
				, AddName		, AddDate		, EditName			, EditDate			, Keyword
			) values (
				s.Id			, s.UKEY		, s.Order_BOAUkey	, s.OrderQty		, s.Refno
				, s.SCIRefno	, s.Price		, s.UsageQty		, s.UsageUnit		, s.Article
				, s.ColorId		, s.SuppColor	, s.SizeCode		, s.Sizespec		, s.SizeUnit
				, s.OrderIdList	, s.SysUsageQty	, s.Remark			, s.BomZipperInsert	, s.BomCustPONo
				, s.AddName		, s.AddDate		, s.EditName		, s.EditDate		, s.Keyword
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		---------------Order_MarkerList------------Marker List

		Merge Production.dbo.Order_MarkerList as t
		Using (select a.* from Trade_To_Pms.dbo.Order_MarkerList a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
				t.id					=s.id,
				t.Seq					= s.Seq,
				t.MarkerName			= s.MarkerName,
				t.FabricCode			= s.FabricCode,
				t.FabricCombo			= s.FabricCombo,
				t.FabricPanelCode		= s.FabricPanelCode,
				t.isQT					= s.isQT,
				t.MarkerLength			= s.MarkerLength,
				t.ConsPC				= s.ConsPC,
				t.Cuttingpiece			= s.Cuttingpiece,
				t.ActCuttingPerimeter	= s.ActCuttingPerimeter,
				t.StraightLength		= s.StraightLength,
				t.CurvedLength			= s.CurvedLength,
				t.Efficiency			= s.Efficiency,
				t.Remark				= s.Remark,
				t.MixedSizeMarker		= s.MixedSizeMarker,
				t.MarkerNo				= s.MarkerNo,
				t.MarkerUpdate			= s.MarkerUpdate,
				t.MarkerUpdateName		= s.MarkerUpdateName,
				t.AllSize				= s.AllSize,
				t.PhaseID				= s.PhaseID,
				t.SMNoticeID			= s.SMNoticeID,
				t.MarkerVersion			= s.MarkerVersion,
				t.Direction				= s.Direction,
				t.CuttingWidth			= s.CuttingWidth,
				t.Width					= s.Width,
				t.Type					= s.Type,
				t.AddName				= s.AddName,
				t.AddDate				= s.AddDate,
				t.EditName				= s.EditName,
				t.EditDate				= s.EditDate
		when not matched by target then
			insert (
				Id					, Ukey					, Seq				, MarkerName		, FabricCode
				, FabricCombo		, FabricPanelCode		, isQT				, MarkerLength		, ConsPC
				, Cuttingpiece		, ActCuttingPerimeter	, StraightLength	, CurvedLength		, Efficiency
				, Remark			, MixedSizeMarker		, MarkerNo			, MarkerUpdate		, MarkerUpdateName
				, AllSize			, PhaseID				, SMNoticeID		, MarkerVersion		, Direction
				, CuttingWidth		, Width					, Type				, AddName			, AddDate
				, EditName			, EditDate
			) values (
				s.Id				, s.Ukey				, s.Seq				, s.MarkerName		, s.FabricCode
				, s.FabricCombo		, s.FabricPanelCode		, s.isQT			, s.MarkerLength	, s.ConsPC
				, s.Cuttingpiece	, s.ActCuttingPerimeter	, s.StraightLength	, s.CurvedLength	, s.Efficiency
				, s.Remark			, s.MixedSizeMarker		, s.MarkerNo		, s.MarkerUpdate	, s.MarkerUpdateName
				, s.AllSize			, s.PhaseID				, s.SMNoticeID		, s.MarkerVersion	, s.Direction
				, s.CuttingWidth	, s.Width				, s.Type			, s.AddName			, s.AddDate
				, s.EditName		, s.EditDate
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		------Order_MarkerList_SizeQty----------------
		Merge Production.dbo.Order_MarkerList_SizeQty as t
		Using (select a.* from Trade_To_Pms.dbo.Order_MarkerList_SizeQty a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.order_MarkerListUkey=s.order_MarkerListUkey and t.sizecode=s.sizecode
		when matched then 
			update set
				t.id		= s.id,
				t.SizeCode	= s.SizeCode,
				t.Qty		= s.Qty
		when not matched by target then
			insert (
				Order_MarkerListUkey	, Id	, SizeCode		, Qty
			) values (
				s.Order_MarkerListUkey	, s.Id	, s.SizeCode	, s.Qty
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		--------Order_ArtWork-----------------
		Merge Production.dbo.Order_ArtWork as t
		Using (select a.* from Trade_To_Pms.dbo.Order_ArtWork a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
				t.id			=s.id,
				t.ArtworkTypeID	= s.ArtworkTypeID,
				t.Article		= s.Article,
				t.PatternCode	= s.PatternCode,
				t.PatternDesc	= s.PatternDesc,
				t.ArtworkID		= s.ArtworkID,
				t.ArtworkName	= s.ArtworkName,
				t.Qty			= s.Qty,
				t.TMS			= s.TMS,
				t.Price			= s.Price,
				t.Cost			= s.Cost,
				t.Remark		= s.Remark,
				t.PPU			= s.PPU,
				t.AddName		= s.AddName,
				t.AddDate		= s.AddDate,
				t.EditName		= s.EditName,
				t.EditDate		= s.EditDate
		when not matched by target then 
			insert (
				ID				, ArtworkTypeID		, Article	, PatternCode	, PatternDesc
				, ArtworkID		, ArtworkName		, Qty		, TMS			, Price
				, Cost			, Remark			, Ukey		, AddName		, AddDate
				, PPU			, EditName		, EditDate
			) values (
				s.ID			, s.ArtworkTypeID	, s.Article	, s.PatternCode	, s.PatternDesc
				, s.ArtworkID	, s.ArtworkName		, s.Qty		, s.TMS			, s.Price
				, s.Cost		, s.Remark			, s.Ukey	, s.AddName		, s.AddDate
				, s.PPU	, s.EditName	, s.EditDate
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;


		---------Order_EachCons--------------------Each Cons
		Merge Production.dbo.Order_EachCons as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set 
				t.Id					= s.Id,
				t.Seq					= s.Seq,
				t.MarkerName			= s.MarkerName,
				t.FabricCombo			= s.FabricCombo,
				t.MarkerLength			= replace(s.MarkerLength,'Ｙ','Y'),
				t.FabricPanelCode		= s.FabricPanelCode,
				t.ConsPC				= s.ConsPC,
				t.CuttingPiece			= s.CuttingPiece,
				t.ActCuttingPerimeter	= s.ActCuttingPerimeter,
				t.StraightLength		= s.StraightLength,
				t.FabricCode			= s.FabricCode,
				t.CurvedLength			= s.CurvedLength,
				t.Efficiency			= s.Efficiency,
				t.Article				= s.Article,
				t.Remark				= s.Remark,
				t.MixedSizeMarker		= s.MixedSizeMarker,
				t.MarkerNo				= s.MarkerNo,
				t.MarkerUpdate			= s.MarkerUpdate,
				t.MarkerUpdateName		= s.MarkerUpdateName,
				t.AllSize				= s.AllSize,
				t.PhaseID				= s.PhaseID,
				t.SMNoticeID			= s.SMNoticeID,
				t.MarkerVersion			= s.MarkerVersion,
				t.Direction				= s.Direction,
				t.CuttingWidth			= s.CuttingWidth,
				t.Width					= s.Width,
				t.TYPE					= s.TYPE,
				t.AddName				= s.AddName,
				t.AddDate				= s.AddDate,
				t.EditName				= s.EditName,
				t.EditDate				= s.EditDate,
				t.isQT					= s.isQT,
				t.MarkerDownloadID		= s.MarkerDownloadID,
				t.OrderCUkey_Old		= s.OrderCUkey_Old
		when not matched by target then 
			insert (
				Id					, Ukey				, Seq				, MarkerName			, FabricCombo
				, MarkerLength		, FabricPanelCode	, ConsPC			, CuttingPiece			, ActCuttingPerimeter
				, StraightLength	, FabricCode		, CurvedLength		, Efficiency			, Article
				, Remark			, MixedSizeMarker	, MarkerNo			, MarkerUpdate			, MarkerUpdateName
				, AllSize			, PhaseID			, SMNoticeID		, MarkerVersion			, Direction
				, CuttingWidth		, Width				, TYPE				, AddName				, AddDate
				, EditName			, EditDate			, isQT				, MarkerDownloadID		, OrderCUkey_Old
			) values (
				s.Id				, s.Ukey			, s.Seq				, s.MarkerName			, s.FabricCombo
				, s.MarkerLength	, s.FabricPanelCode	, s.ConsPC			, s.CuttingPiece		, s.ActCuttingPerimeter
				, s.StraightLength	, s.FabricCode		, s.CurvedLength	, s.Efficiency			, s.Article
				, s.Remark			, s.MixedSizeMarker	, s.MarkerNo		, s.MarkerUpdate		, s.MarkerUpdateName
				, s.AllSize			, s.PhaseID			, s.SMNoticeID		, s.MarkerVersion		, s.Direction
				, s.CuttingWidth	, s.Width			, s.TYPE			, s.AddName				, s.AddDate
				, s.EditName		, s.EditDate		, s.isQT			, s.MarkerDownloadID	, s.OrderCUkey_Old
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;


		--------Order_EachCons_SizeQty----------------Each cons - Size & Qty
		Merge Production.dbo.Order_EachCons_SizeQty as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_SizeQty a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Order_EachConsUkey=s.Order_EachConsUkey and t.sizecode=s.sizecode	
		when matched then 
			update set 
				t.Id		= s.Id,
				t.SizeCode	= s.SizeCode,
				t.Qty		= s.Qty
		when not matched by target then 
			insert (
				Order_EachConsUkey		, Id	, SizeCode		, Qty
			) values (
				s.Order_EachConsUkey	, s.Id	, s.SizeCode	, s.Qty
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
	-------Order_EachCons_Article--------------------Each cons - 用量展開
	  Merge Production.dbo.Order_EachCons_Article as t
	  Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Article a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
	 on t.Order_EachConsUkey=s.Order_EachConsUkey and t.Article = s.Article
	 when matched then 
	 update set 
	  t.Id     = s.Id,
	  t.AddName    = s.AddName,
	  t.AddDate    = s.AddDate,
	 t.EditName    = s.EditName,
	 t.EditDate    = s.EditDate
	 when not matched by target then 
	  insert (
	   Id   , Order_EachConsUkey , Article   , AddName  , AddDate
	  , EditName  , EditDate
	  ) values (
	  s.Id  , s.Order_EachConsUkey , s.Article  , s.AddName  , s.AddDate
	 , s.EditName , s.EditDate
	  )
	  when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
		delete;
		-------Order_EachCons_Color--------------------Each cons - 用量展開
		Merge Production.dbo.Order_EachCons_Color as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Color a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Ukey=s.Ukey	
		when matched then 
			update set 
				t.Id					= s.Id,
				t.Order_EachConsUkey	= s.Order_EachConsUkey,
				t.Ukey					= s.Ukey,
				t.ColorID				= s.ColorID,
				t.CutQty				= s.CutQty,
				t.Layer					= s.Layer,
				t.Orderqty				= s.Orderqty,
				t.SizeList				= s.SizeList,
				t.Variance				= s.Variance,
				t.YDS					= s.YDS
		when not matched by target then 
			insert (
				Id			, Order_EachConsUkey	, Ukey			, ColorID		, CutQty
				, Layer		, Orderqty				, SizeList		, Variance		, YDS
			) values (
				s.Id		, s.Order_EachConsUkey	, s.Ukey		, s.ColorID		, s.CutQty
				, s.Layer	, s.Orderqty			, s.SizeList	, s.Variance	, s.YDS
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
		
		---------Order_EachCons_Color_Article-------Each cons - 用量展開明細
		Merge Production.dbo.Order_EachCons_Color_Article as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Color_Article a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Ukey=s.Ukey	
		when matched then 
			update set 
				t.Id						= s.Id,
				t.Order_EachCons_ColorUkey	= s.Order_EachCons_ColorUkey,
				t.Article					= s.Article,
				t.ColorID					= s.ColorID,
				t.SizeCode					= s.SizeCode,
				t.Orderqty					= s.Orderqty,
				t.Layer						= s.Layer,
				t.CutQty					= s.CutQty,
				t.Variance					= s.Variance
		when not matched by target then 
			insert (
				Id				, Order_EachCons_ColorUkey		, Article		, ColorID		, SizeCode
				, Orderqty		, Layer							, CutQty		, Variance		, Ukey
			) values (
				s.Id			, s.Order_EachCons_ColorUkey	, s.Article		, s.ColorID		, s.SizeCode
				, s.Orderqty	, s.Layer						, s.CutQty		, s.Variance	, s.Ukey
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;

		----------Order_EachCons_PatternPanel---------------PatternPanel
		Merge Production.dbo.Order_EachCons_PatternPanel as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_PatternPanel a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.PatternPanel=s.PatternPanel and t.Order_EachConsUkey=s.Order_EachConsUkey and t.FabricPanelCode=s.FabricPanelCode
		When matched then 
			update set 
				t.Id					= s.Id,
				--t.PatternPanel		= s.PatternPanel,
				--t.Order_EachConsUkey	= s.Order_EachConsUkey,
				--t.FabricPanelCode		= s.FabricPanelCode,
				t.AddName				= s.AddName,
				t.AddDate				= s.AddDate,
				t.EditName				= s.EditName,
				t.EditDate				= s.EditDate
		when not matched by target then 
			insert (
				Id			, PatternPanel		, Order_EachConsUkey	, FabricPanelCode	, AddName
				, AddDate	, EditName			, EditDate
			) values (
				s.Id		, s.PatternPanel	, s.Order_EachConsUkey	, s.FabricPanelCode	, s.AddName
				, s.AddDate	, s.EditName		, s.EditDate
			)
		when not matched by source and t.id in (select id from #TOrder) then 
			delete;

		
		------------Order_Article----------------------Art
		Merge Production.dbo.Order_Article as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Article a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article
		when matched then 
			update set 
				t.Seq			= s.Seq,
				t.TissuePaper	= s.TissuePaper
		when not matched by target then
			insert (
				id		, Seq	, Article	, TissuePaper
			) values (
				s.id	, s.Seq	, s.Article	, s.TissuePaper
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
		------------Order_Article_PadPrint----------------------Art
		Merge Production.dbo.Order_Article_PadPrint as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Article_PadPrint a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.colorid = s.colorid
		when matched then 
			update set 
				t.qty			= s.qty
		when not matched by target then
			insert (
				id	, Article	, colorid,  qty
			) values (
				s.id , s.Article , s.colorid, s.qty
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;


		-----------Order_BOA_KeyWord---------------------Bill of Other - Key word

		Merge Production.dbo.Order_BOA_KeyWord as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_KeyWord a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 
				t.id			= s.id,
				t.Order_BOAUkey	= s.Order_BOAUkey,
				t.KeyWordID		= s.KeyWordID,
				t.Relation		= s.Relation
		when not matched by target then
			insert (
				ID		, Ukey		, Order_BOAUkey		, KeyWordID		, Relation
			) values (
				s.ID	, s.Ukey	, s.Order_BOAUkey	, s.KeyWordID	, s.Relation
			)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;
	

		------------Order_BOA_CustCD----------Bill of Other - 用量展開
		Merge Production.dbo.Order_BOA_CustCD as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_CustCD a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.Order_BOAUkey=s.Order_BOAUkey and t.ColumnValue=s.ColumnValue
		when matched then 
			update set 
				t.id			= s.id,
				t.CustCDID		= s.CustCDID,
				t.Refno			= s.Refno,
				t.SCIRefno		= s.SCIRefno,
				t.AddName		= s.AddName,
				t.AddDate		= s.AddDate,
				t.EditName		= s.EditName,
				t.EditDate		= s.EditDate,
				t.ColumnValue	= s.ColumnValue
		when not matched by target then
			insert (
				Id			, Order_BOAUkey		, CustCDID		, Refno		, SCIRefno
				, AddName	, AddDate			, EditName		, EditDate	, ColumnValue
			) values (
				s.Id		, s.Order_BOAUkey	, s.CustCDID	, s.Refno	, s.SCIRefno
				, s.AddName	, s.AddDate			, s.EditName	, s.EditDate, s.ColumnValue
			)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		-----------------Order_PFHis-----------Pull forward歷史記錄
		Merge Production.dbo.Order_PFHis as t
		Using (select a.* from Trade_To_Pms.dbo.Order_PFHis a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.Ukey=s.Ukey
		when matched then 
			update set 
				t.id				= s.id,
				t.NewSciDelivery	= s.NewSciDelivery,
				t.OldSciDelivery	= s.OldSciDelivery,
				t.LETA				= s.LETA,
				t.Remark			= s.Remark,
				t.AddName			= s.AddName,
				t.AddDate			= s.AddDate
		when not matched by target then
			insert (
				Id			, NewSciDelivery	, OldSciDelivery	, LETA		, Remark
				, AddName	, AddDate			, Ukey
			) values (
				s.Id		, s.NewSciDelivery	, s.OldSciDelivery	, s.LETA	, s.Remark
				, s.AddName	, s.AddDate			, s.Ukey
			)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;
		----------------Order_QtyCTN------------Qty breakdown per Carton
		Merge Production.dbo.Order_QtyCTN as t
		Using (select a.* from Trade_To_Pms.dbo.Order_QtyCTN a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when matched then 
			update set 
				t.Article	= s.Article,
				t.SizeCode	= s.SizeCode,
				t.Qty		= s.Qty,
				t.AddName	= s.AddName,
				t.AddDate	= s.AddDate,
				t.EditName	= s.EditName,
				t.EditDate	= s.EditDate
		when not matched by target then
			insert (
				Id			, Article		, SizeCode		, Qty	, AddName
				, AddDate	, EditName		, EditDate
			) values (
				s.Id		, s.Article		, s.SizeCode	, s.Qty	, s.AddName
				, s.AddDate	, s.EditName	, s.EditDate
			)
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		----------------------[Order_ECMNFailed]
		Merge Production.dbo.[Order_ECMNFailed] t
		using (select a.* from Trade_To_Pms.dbo.Order_ECMNFailed a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id = s.id and t.type = s.type
			when matched  then	update set 
				 t.[KPIFailed]		=s.[KPIFailed]		
				,t.[KPIDate]		=s.[KPIDate]		
				,t.[FailedComment]	=s.[FailedComment]	
				,t.[ExpectApvDate]	=s.[ExpectApvDate]	
				,t.[AddName]		=s.[AddName]		
				,t.[AddDate]		=s.[AddDate]		
				,t.[EditName]		=s.[EditName]		
				,t.[EditDate]		=s.[EditDate]		
		when not matched by target then 	
			insert([ID],[Type],[KPIFailed],[KPIDate],[FailedComment],[ExpectApvDate],[AddName],[AddDate],[EditName],[EditDate])
			VALUES(s.[ID],s.[Type],s.[KPIFailed],s.[KPIDate],s.[FailedComment],s.[ExpectApvDate],s.[AddName],s.[AddDate],s.[EditName],s.[EditDate])
		when not matched by source and t.id in (select id from #TOrder) then
			delete
		;

----------------order_markerlist_Article-----------------
	Merge Production.dbo.order_markerlist_Article as t
	Using (select a.* from Trade_To_Pms.dbo.order_markerlist_Article a inner join #TOrder b on a.id = b.id) as s
	on t.[Order_MarkerlistUkey] = s.[Order_MarkerlistUkey] and t.[Article] = s.[Article]
	when matched then update set
		t.[Id] = s.[Id]
		,t.[AddName] = s.[AddName]
		,t.[AddDate] = s.[AddDate]
		,t.[EditName] = s.[EditName]
		,t.[EditDate] = s.[EditDate]
	when not matched by target then
		insert([Id],[Order_MarkerlistUkey],[Article],[AddName],[AddDate],[EditName],[EditDate])
		values(s.[Id],s.[Order_MarkerlistUkey],s.[Article],s.[AddName],s.[AddDate],s.[EditName],s.[EditDate])
	when not matched by source and t.id in (select id from #TOrder)then
			delete;

	----------Order_BuyBack--------------
	Merge Production.dbo.Order_BuyBack as t
	using (select a.* from Trade_To_Pms.dbo.Order_BuyBack a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
	on t.ID = s.ID 
	and t.OrderIDFrom = s.OrderIDFrom
		when matched then 
		update set
			t.BuyBackReason	= isnull(s.BuyBackReason,'') ,
			t.AddName	    = isnull(s.AddName,'') ,
			t.AddDate		= s.AddDate ,
			t.EditName		= isnull(s.EditName,'') ,
			t.EditDate		= s.EditDate 
	when not matched by target then
		insert  ([ID], [OrderIDFrom], [BuyBackReason], [AddName], [AddDate], [EditName], [EditDate]) 
		values (s.[ID], s.[OrderIDFrom], isnull(s.[BuyBackReason],''), isnull(s.[AddName],''), s.[AddDate], isnull(s.[EditName],''), s.[EditDate])
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;

	-----------Order_BuyBack_Qty------------------------ 
	Merge Production.dbo.Order_BuyBack_Qty as t
	Using (select a.* from Trade_To_Pms.dbo.Order_BuyBack_Qty as a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
	on t.ID = s.ID  
	and t.OrderIDFrom = s.OrderIDFrom
	and t.Article = s.Article
	and t.SizeCode = s.SizeCode
	when matched then
		update set
			t.Qty		= s.Qty,
			t.AddName	= isnull(s.AddName,'') ,
			t.AddDate	= s.AddDate ,
			t.EditName	= isnull(s.EditName,'') ,
			t.EditDate	= s.EditDate 
	when not matched by target then 
		insert ([ID], [OrderIDFrom], [Article], [SizeCode], [Qty], [AddName], [AddDate], [EditName], [EditDate]) 
		values (s.[ID], s.[OrderIDFrom], s.[Article], s.[SizeCode], s.[Qty], isnull(s.[AddName],''), s.[AddDate], isnull(s.[EditName],''), s.[EditDate]) 
	when not matched by source  AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;
		
----------------OrderChangeApplication-----------------
update t set	
	[ReasonID] = s.ReasonID
	,[OrderID] = s.OrderID	
	,[Status]  =s.Status
	,[SentName] = s.SentName
	,[SentDate] = s.SentDate
	,[ApprovedName] = s.ApprovedName
	,[ApprovedDate] = s.ApprovedDate
	,[ConfirmedName] = s.ConfirmedName
	,[ConfirmedDate] = s.ConfirmedDate
	,[RejectName] = s.RejectName
	,[RejectDate] = s.RejectDate
	,[ClosedName] = s.ClosedName
	,[ClosedDate] = s.ClosedDate
	,[JunkName] = s.JunkName
	,[JunkDate] = s.JunkDate
	,[AddName] = s.AddName
	,[AddDate] = s.AddDate
	,[TPEEditName] = s.EditName
	,[TPEEditDate] = s.EditDate
	,[ToOrderID] = s.ToOrderID
	,[NeedProduction] = s.NeedProduction
	,[OldQty] = s.OldQty
	,[RatioFty] = s.RatioFty
	,[RatioSubcon] = s.RatioSubcon
	,[RatioSCI] = s.RatioSCI
	,[RatioSupp] = s.RatioSupp
	,[RatioBuyer] = s.RatioBuyer
	,[ResponsibleFty] = s.ResponsibleFty 
	,[ResponsibleSubcon] = s.ResponsibleSubcon
	,[ResponsibleSCI] = s.ResponsibleSCI
	,[ResponsibleSupp] = s.ResponsibleSupp
	,[ResponsibleBuyer] = s.ResponsibleBuyer
	,[FactoryICRDepartment] = s.FactoryICRDepartment
	,[FactoryICRNo] = s.FactoryICRNo
	,[FactoryICRRemark] = s.FactoryICRRemark
	,[SubconDBCNo] = s.SubconDBCNo
	,[SubconDBCRemark] = s.SubconDBCRemark
	,[SubConName] = s.SubConName
	,[SCIICRDepartment] = s.SCIICRDepartment
	,[SCIICRNo] = s.SCIICRNo
	,[SCIICRRemark] = s.SCIICRRemark
	,[SuppDBCNo] = s.SuppDBCNo
	,[SuppDBCRemark] = s.SuppDBCRemark
	,[BuyerDBCDepartment] = s.BuyerDBCDepartment
	,[BuyerDBCNo] = s.BuyerDBCNo
	,[BuyerDBCRemark] = s.BuyerDBCRemark
	,[BuyerICRNo] = s.BuyerICRNo
	,[BuyerICRRemark] = s.BuyerICRRemark
	,[MRComment] = s.MRComment
	,[Remark] = s.Remark
	,[BuyerRemark] = s.BuyerRemark
	,[FTYComments] = s.FTYComments
	,[FactoryID] = s.FactoryID
from Production.dbo.OrderChangeApplication t
inner join Trade_To_Pms.dbo.OrderChangeApplication s on s.ID = t.ID
inner join Factory f on s.FactoryID = f.ID and f.IsProduceFty = 1

insert into Production.dbo.OrderChangeApplication ([ID], [ReasonID], [OrderID], [Status], [SentName], [SentDate]
, [ApprovedName], [ApprovedDate], [ConfirmedName], [ConfirmedDate], [RejectName], [RejectDate], [ClosedName], [ClosedDate], [JunkName], [JunkDate]
, [AddName], [AddDate], [ToOrderID], [NeedProduction], [OldQty], [RatioFty], [RatioSubcon], [RatioSCI], [RatioSupp], [RatioBuyer]
, [ResponsibleFty], [ResponsibleSubcon], [ResponsibleSCI], [ResponsibleSupp], [ResponsibleBuyer], [FactoryICRDepartment], [FactoryICRNo], [FactoryICRRemark]
, [SubconDBCNo], [SubconDBCRemark], [SubConName], [SCIICRDepartment], [SCIICRNo], [SCIICRRemark], [SuppDBCNo], [SuppDBCRemark], [BuyerDBCDepartment], [BuyerDBCNo]
, [BuyerDBCRemark], [BuyerICRNo], [BuyerICRRemark], [MRComment], [Remark], [BuyerRemark], [FTYComments], [FactoryID], [TPEEditName], [TPEEditDate])
select s.ID
	,s.ReasonID
	,s.OrderID
	,s.Status
	,s.SentName
	,s.SentDate
	,s.ApprovedName
	,s.ApprovedDate
	,s.ConfirmedName
	,s.ConfirmedDate
	,s.RejectName
	,s.RejectDate
	,s.ClosedName
	,s.ClosedDate
	,s.JunkName
	,s.JunkDate
	,s.AddName
	,s.AddDate
	,s.ToOrderID
	,s.NeedProduction
	,s.OldQty
	,s.RatioFty
	,s.RatioSubcon
	,s.RatioSCI
	,s.RatioSupp
	,s.RatioBuyer
	,s.ResponsibleFty
	,s.ResponsibleSubcon
	,s.ResponsibleSCI
	,s.ResponsibleSupp
	,s.ResponsibleBuyer
	,s.FactoryICRDepartment
	,s.FactoryICRNo
	,s.FactoryICRRemark
	,s.SubconDBCNo
	,s.SubconDBCRemark
	,s.SubConName
	,s.SCIICRDepartment
	,s.SCIICRNo
	,s.SCIICRRemark
	,s.SuppDBCNo
	,s.SuppDBCRemark
	,s.BuyerDBCDepartment
	,s.BuyerDBCNo
	,s.BuyerDBCRemark
	,s.BuyerICRNo
	,s.BuyerICRRemark
	,s.MRComment
	,s.Remark
	,s.BuyerRemark
	,s.FTYComments
	,s.FactoryID
	,s.EditName
	,s.EditDate
from Trade_To_Pms.dbo.OrderChangeApplication  s
inner join Production.dbo.Factory f on s.FactoryID =f.ID and f.IsProduceFty = 1
where not exists(select 1 from Production.dbo.OrderChangeApplication t where s.ID = t.ID)

----------------OrderChangeApplication_Detail-----------------
--ISP20200602
if cast(getdate() as date) = cast('20200420' as date)
begin
	drop table Production.dbo.OrderChangeApplication_Detail

	CREATE TABLE [dbo].[OrderChangeApplication_Detail](
		[Ukey] [bigint] NOT NULL,
		[ID] [varchar](13) NULL,
		[Seq] [varchar](2) NULL,
		[Article] [varchar](8) NULL,
		[SizeCode] [varchar](8) NULL,
		[Qty] [numeric](6, 0) NULL,
		[OriQty] [numeric](6, 0) NULL,
		[NowQty] [numeric](6, 0) NULL,
	 CONSTRAINT [PK_OrderChangeApplication_Detail] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[OrderChangeApplication_Detail] ADD  CONSTRAINT [DF_OrderChangeApplication_Detail_NowQty]  DEFAULT ((0)) FOR [NowQty]
end

delete t
from Production.dbo.OrderChangeApplication_Detail t
inner join Trade_To_Pms.dbo.OrderChangeApplication_Detail s on s.ID = t.ID and s.Ukey = t.Ukey

insert into Production.dbo.OrderChangeApplication_Detail([Ukey], [ID], [Seq], [Article], [SizeCode], [Qty], [OriQty], [NowQty])
select s.Ukey
	,s.ID
	,s.Seq
	,s.Article
	,s.SizeCode
	,s.Qty
	,s.OriQty
	,s.NowQty
from Trade_To_Pms.dbo.OrderChangeApplication_Detail s
where not exists (select 1 from Production.dbo.OrderChangeApplication_Detail t where t.ID = s.ID and t.Ukey = s.Ukey)

----------------OrderChangeApplication_Seq-----------------
--ISP20200602
if cast(getdate() as date) = cast('20200420' as date)
begin
	drop table OrderChangeApplication_Seq

	CREATE TABLE [dbo].[OrderChangeApplication_Seq](
		[Ukey] [bigint] NOT NULL,
		[ID] [varchar](13) NOT NULL,
		[Seq] [varchar](2) NOT NULL,
		[NewSeq] [varchar](2) NULL,
		[ShipmodeID] [varchar](10) NOT NULL,
		[BuyerDelivery] [date] NOT NULL,
		[FtyKPI] [date] NULL,
		[ReasonID] [varchar](5) NULL,
		[ReasonRemark] [nvarchar](150) NULL,
		[ShipModeRemark] [nvarchar](150) NULL,
	 CONSTRAINT [PK_OrderChangeApplication_Seq] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[OrderChangeApplication_Seq] ADD  CONSTRAINT [DF_OrderChangeApplication_Seq_ReasonID]  DEFAULT ('') FOR [ReasonID]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改客戶交期原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderChangeApplication_Seq', @level2type=N'COLUMN',@level2name=N'ReasonID'
end

delete t
from Production.dbo.OrderChangeApplication_Seq t
inner join Trade_To_Pms.dbo.OrderChangeApplication_Seq s on s.ID = t.ID and s.Ukey = t.Ukey

insert into Production.dbo.OrderChangeApplication_Seq([Ukey], [ID], [Seq], [NewSeq], [ShipmodeID], [BuyerDelivery], [FtyKPI], [ReasonID], [ReasonRemark], [ShipModeRemark])
select s.Ukey
	,s.ID
	,s.Seq
	,s.NewSeq
	,s.ShipmodeID
	,s.BuyerDelivery
	,s.FtyKPI
	,s.ReasonID
	,s.ReasonRemark
	,s.ShipModeRemark
from Trade_To_Pms.dbo.OrderChangeApplication_Seq s
where not exists (select 1 from Production.dbo.OrderChangeApplication_Seq t where t.ID = s.ID and t.Ukey = s.Ukey)

----------------OrderChangeApplication_History-----------------
INSERT INTO [dbo].[OrderChangeApplication_History]([ID],[Status],[StatusUser],[StatusDate])
select s.ID,s.Status,s.[StatusUser],s.[StatusDate]
from Trade_To_Pms.dbo.[OrderChangeApplication_History] s
left join Production.dbo.[OrderChangeApplication_History] t on s.ID = t.ID and s.Status = t.Status
where s.Status = 'Closed' and t.id is null

----------------ppaschedule-----------------

----若有跨M轉廠的訂單(orders.mdivisionid不同)。(例:PM1 to PM2)
----若這些訂單編號存在ppaschedule.orderid中，則刪除其ppaschedule, ppaschedule_detail的資料。
	delete c
	from #TOrder a 		
	inner join Production.dbo.ppaschedule b on b.Orderid = a.id
	inner join Production.dbo.ppaschedule_detail c on c.id = b.id
	where b.mdivisionid <> a.mdivisionid

	delete b
	from #TOrder a 		
	inner join Production.dbo.ppaschedule b on b.Orderid = a.id
	where b.mdivisionid <> a.mdivisionid
----以ppaschedule.AddDate或EditDate近3個月有異動範圍,MDivisionID，若不同，則刪除其ppaschedule, ppaschedule_detail的資料。
	delete c
	from Production.dbo.orders a 		
	inner join Production.dbo.ppaschedule b on b.Orderid = a.id
	inner join Production.dbo.ppaschedule_detail c on c.id = b.id
	where b.mdivisionid <> a.mdivisionid and (b.adddate > dateadd(day,90,getdate()) or b.editdate > dateadd(day,90,getdate()))

	delete b
	from Production.dbo.orders a 		
	inner join Production.dbo.ppaschedule b on b.Orderid = a.id
	where b.mdivisionid <> a.mdivisionid and (b.adddate > dateadd(day,90,getdate()) or b.editdate > dateadd(day,90,getdate()))


----更新的判斷必須要依照#Torder的區間作更新
Update b set b.MDivisionId='',FactoryID=''
from Production.dbo.WorkOrder b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))


----刪除的判斷必須要依照#Torder的區間作刪除

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
--Delete b
--from Production.dbo.Order_MarkerList_Article b
--where id in (select id from #tmpOrders as t 
--where not exists(select 1 from #TOrder as s where t.id=s.ID))
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
-------------------------------------Order_BuyBack
Delete b
from Production.dbo.Order_BuyBack b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))
-------------------------------------Order_BuyBack_Qty
Delete b
from Production.dbo.Order_BuyBack_Qty b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))

------------------------刪除表頭多的資料order 最後刪除
Delete a
from Production.dbo.Orders as a 
where a.id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))


drop table #tmpOrders
drop table #TOrder

---- 轉入 歷史資料 TradeHis_Order 必須與 Trade 資料相同----
Merge Production.dbo.TradeHis_Order as t
Using (
	select * 
	from Trade_To_Pms.dbo.TradeHis_Order
	where tableName = 'Orders' 
		  and histype = 'OrdersBuyerDelivery'
) as s on t.Ukey = s.Ukey
when matched then update set
	t.[TableName] = s.[TableName]
	, t.[HisType] = s.[HisType]
	, t.[SourceID] = s.[SourceID]
	, t.[ReasonTypeID] = s.[ReasonTypeID]
	, t.[ReasonID] = s.[ReasonID]
	, t.[OldValue] = s.[OldValue]
	, t.[NewValue] = s.[NewValue]
	, t.[Remark] = s.[Remark]
	, t.[AddName] = s.[AddName]
	, t.[AddDate] = s.[AddDate]
when not matched by target then
	insert (
		[UKEY] 		  , [TableName]  , [HisType]   , [SourceID]  , [ReasonTypeID] 
		, [ReasonID]  , [OldValue]   , [NewValue]  , [Remark]    , [AddName] 
		, [AddDate]
	) values (
		s.[UKEY]	  , s.[TableName], s.[HisType] , s.[SourceID], s.[ReasonTypeID]
		, s.[ReasonID], s.[OldValue] , s.[NewValue], s.[Remark]  , s.[AddName]
		, s.[AddDate]
	)
when not matched by source then
	delete;

END
------------------------------------------------------------------------------------------------------------------------


