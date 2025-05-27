-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/25>
-- Description:	<import order>
-- =============================================
Create PROCEDURE [dbo].[imp_Order]
AS
BEGIN
	SET NOCOUNT ON;
	
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	declare @DateInfoName varchar(30) ='imp_Order_OldDate';
	declare @OldDate date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @OldDate is Null
		set @OldDate= (select max(UpdateDate) from Production.dbo.OrderComparisonList WITH (NOLOCK)) --最後匯入資料日期

--3.更新Trade_To_Pms.dbo.dateInfo
if exists(select 1 from Trade_To_Pms.dbo.dateInfo where Name = @DateInfoName )
	update Trade_To_Pms.dbo.dateInfo  set DateStart = @oldDate,DateEnd = @oldDate, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Trade_To_Pms.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@oldDate,@oldDate,@Remark);
	
--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @oldDate,DateEnd = @oldDate, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@oldDate,@oldDate,@Remark);
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	Set @DateInfoName ='imp_Order_dToDay';
	declare @dToDay date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	SET @Remark = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @dToDay is Null
		set @dToDay= CONVERT(date, GETDATE())		
		
--3.更新Trade_To_Pms.dbo.dateInfo
if exists(select 1 from Trade_To_Pms.dbo.dateInfo where Name = @DateInfoName )
	update Trade_To_Pms.dbo.dateInfo  set DateStart = @dToDay,DateEnd = @dToDay, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Trade_To_Pms.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@dToDay,@dToDay,@Remark);
	
--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @dToDay,DateEnd = @dToDay, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@dToDay,@dToDay,@Remark);
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	Set @DateInfoName  ='imp_Order';
	declare @Odate_s datetime = (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	declare @Odate_e datetime = (select DateEnd from Production.dbo.DateInfo where name = @DateInfoName);
	SET @Remark = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @Odate_s is Null
		set @Odate_s= (SELECT TOP 1 DateStart FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')
	if @Odate_e is Null
		set @Odate_e= (SELECT TOP 1 DateEnd FROM Trade_To_Pms.dbo.DateInfo WHERE NAME = 'ORDER')		

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @Odate_s,DateEnd = @Odate_e, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@Odate_s,@Odate_e,@Remark);
------------------------------------------------------------------------------------------------------

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
			cast(null as date) as MDClose,
			cast(null as date) as PulloutCmplDate 
	into #TOrder
	from Trade_To_Pms.dbo.Orders a WITH (NOLOCK)
	inner join Production.dbo.Factory b WITH (NOLOCK) on a.FactoryID=b.ID

	update #TOrder
	set		FTY_Group = IIF(b.FTYGroup is null,a.FactoryID,b.FTYGroup) 
			, MDivisionID=isnull(b.MDivisionID, '')
	from #TOrder a
	inner join Production.dbo.Factory b on a.FactoryID=b.id

	/* issue：ISP20231174 */
	UPDATE S SET S.BIPImportCuttingBCSCmdTime = NULL
	FROM #TOrder A
	INNER JOIN SewingSchedule S ON A.ID = S.OrderID
-------------------------------------------------------------------------Order
		--��欰Cutting�����,����sMDivisionID
		Update a
		set a.MDivisionID = isnull(b.MDivisionID, '')
            ,a.FactoryID = ISNULL(b.Fty_Group, '')
		from Production.dbo.Cutting a
		inner join #TOrder b on a.ID = b.ID
		where (a.MDivisionID <> b.MDivisionID and b.MDivisionID in( select distinct MDivisionID from Production..Factory))
              OR (a.FactoryID <> b.Fty_Group AND EXISTS (SELECT 1 FROM Production.dbo.Factory WHERE ID = b.Fty_Group))

		--轉單為Cutting母單時,覆寫CutPlan母子單的工廠欄位 
		Update a
		set a.FactoryID = isnull( b.FTY_Group, '')
			, a.MDivisionID = isnull( f.MDivisionID, '')
		from Production.dbo.WorkOrderForPlanning a
		inner join #TOrder b on a.ID = b.ID
		inner join Production.dbo.Orders c on b.ID = c.ID 
											  and a.FactoryID != b.FTY_Group
		left join Production.dbo.Factory f on b.FTY_Group = f.ID
		where 	(isnull(b.Junk,0) = 0 or (isnull(b.Junk,0) = 1 and b.NeedProduction=1))
				and b.IsForecast = '0'

		Update a
		set a.FactoryID = isnull( b.FTY_Group, '')
			, a.MDivisionID = isnull( f.MDivisionID, '')
		from Production.dbo.WorkOrderForOutput a
		inner join #TOrder b on a.ID = b.ID
		inner join Production.dbo.Orders c on b.ID = c.ID 
											  and a.FactoryID != b.FTY_Group
		left join Production.dbo.Factory f on b.FTY_Group = f.ID
		where 	(isnull(b.Junk,0) = 0 or (isnull(b.Junk,0) = 1 and b.NeedProduction=1))
				and b.IsForecast = '0'

		--delete cutting
		delete b
		from #TOrder a 		
		inner join Production.dbo.Cutting b on a.id = b.ID 
											   and b.FactoryID <> a.FTY_Group
		where	(isnull(a.Junk,0) = 0 or (isnull(a.Junk,0) = 1 and a.NeedProduction=1))
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
		set		t.MCHandle = isnull( (select localMR 
							  from Production.dbo.Style 
							  where BrandID = t.BrandID 
									and id=t.styleid 
									and SeasonID=t.SeasonID ), '')
				, t.PulloutComplete = iif((t.GMTComplete='P' OR t.GMTComplete='' or t.GMTComplete is null), 0, 1)
				, t.MDClose = iif((t.GMTComplete='P' OR t.GMTComplete='' or t.GMTComplete is null) 
				    , t.MDClose
				    , convert(date,getdate()))
				, PulloutCmplDate = IIF(t.CMPLTDATE > s1.PulloutCmplDate or (t.CMPLTDATE is not null and s1.PulloutCmplDate is null), cast(GETDATE() as date), s1.PulloutCmplDate)
		from #TOrder as t
		left join Production.dbo.Orders as s1 on t.ID=s1.ID
		where s1.ID is null

----------------取得 BuyerDelivery & SciDelivery 日期在 Trade 給的日期範圍中 Orders 的資料------------------------
	SELECT s.ID
	INTO #LastFiveData
	FROM Trade_To_Pms.dbo.Orders s 
	WHERE Cast(s.EditDate as Date) >= DATEADD( DAY ,-5,GETDATE() )

	select * 
	into #tmpOrders 
	from Production.dbo.Orders a WITH (NOLOCK)
	where	(
				a.BuyerDelivery between @Odate_s and @Odate_e 
				or a.SciDelivery between @Odate_s and @Odate_e

				--納入 Trade_To_Pms.dbo.Orders.EditDate 在 5天內的訂單
				OR EXISTS( 
					SELECT 1 FROM #LastFiveData s 
					WHERE s.ID = a.ID
				)
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
				, t.OrderID				= isnull( s.ID           , '')
				, t.NewStyleID		= isnull( s.StyleID      , '')
				, t.NewQty				= isnull( s.Qty          , 0)
				, t.NewBuyerDelivery	= s.BuyerDelivery
				, t.NewSCIDelivery		= s.SCIDelivery
				, t.MDivisionID			= isnull( s.MDivisionID  , '')
				, t.FactoryID			= isnull( s.FactoryID    , '')
				, t.BrandID				= isnull( s.BrandID      , '')
				, t.UpdateDate			= @dToDay--寫入到IMP MOCKUPORDER
				, t.TransferDate		= @OldDate--寫入到IMP MOCKUPORDER
				, t.NewFOC = iif(isnull(s.FOC, 0) = 1 , 'v','')
				, t.NewOrderTypeID = isnull(s.OrderTypeID, '')
		when not matched by target then
			insert (
				NewOrder		, OrderID		, NewStyleID	, NewQty	, NewBuyerDelivery
				, NewSCIDelivery, MDivisionID	, FactoryID			, UpdateDate, TransferDate
				, BrandID		, NewFOC		, NewOrderTypeID
			) 
       VALUES
       (
              1 ,
              isnull(s.id ,           ''),
              isnull(s.styleid ,      ''),
              isnull(s.qty ,          0),
              s.buyerdelivery ,
              s.scidelivery ,
              isnull(s.mdivisionid ,  ''),
              isnull(s.factoryid ,    ''),
              @dToDay ,
              @OldDate ,
              isnull(s.brandid,    ''),
			  iif(isnull(s.FOC, 0) = 1 , 'v',''),
			  isnull(s.OrderTypeID, '')
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
				, t.OrderID					= isnull( s.ID           , '')
				, t.OriginalStyleID			= isnull( s.StyleID      , '')
				, t.OriginalQty				= isnull( s.Qty          , 0)
				, t.OriginalBuyerDelivery	= s.BuyerDelivery
				, t.OriginalSciDelivery		= s.SciDelivery
				, t.MDivisionID				= isnull( s.MDivisionID  , '')
				, t.FactoryID				= isnull( s.FactoryID    , '')
				, t.BrandID				    = isnull( s.BrandID      , '')
				, t.UpdateDate				= @dToday
				, t.TransferDate			= @OldDate
				, t.OriginalFOC = iif(isnull(s.FOC, 0) = 1 , 'v','')
				, t.OriginalOrderTypeID = isnull(s.OrderTypeID, '')
		when not matched by target then
			insert (
				DeleteOrder				, OrderID		, OriginalStyleID	, OriginalQty	, OriginalBuyerDelivery
				, OriginalSciDelivery	, MDivisionID	, FactoryID			, UpdateDate	, TransferDate
				, BrandID	, OriginalFOC	, OriginalOrderTypeID
			)
           VALUES
           (
                  1 ,
                  isnull(s.id ,            ''),
                  isnull(s.styleid ,       ''),
                  isnull(s.qty ,           0),
                  s.buyerdelivery ,
                  s.scidelivery ,
                  isnull(s.mdivisionid ,   ''),
                  isnull(s.factoryid ,     ''),
                  @dToDay ,
                  @OldDate ,
                  isnull(s.brandid, ''),
				  iif(isnull(s.FOC, 0) = 1 , 'v',''),
				  isnull(s.OrderTypeID, '')
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
				, t.TransferToFactory		= isnull( s.Transfer2Factroy, '')
				, t.OrderID					= isnull( s.ID              , '')
				, t.OriginalStyleID			= isnull( s.StyleID         , '')
				, t.OriginalQty				= isnull( s.Qty             , 0)
				, t.OriginalBuyerDelivery	= s.BuyerDelivery
				, t.OriginalSciDelivery		= s.SciDelivery
				, t.MDivisionID				= isnull( s.MDivisionID     , '')
				, t.FactoryID				= isnull( s.FactoryID       , '')
				, t.BrandID				    = isnull( s.BrandID			, '')
				, t.UpdateDate				= @dToday
				, t.TransferDate			= @OldDate
				, t.OriginalFOC = iif(isnull(s.FOC, 0) = 1 , 'v','')
				, t.OriginalOrderTypeID = isnull(s.OrderTypeID, '')
		when not matched by target then
			insert (
				DeleteOrder				, TransferToFactory		, OrderID		, OriginalStyleID	, OriginalQty
				, OriginalBuyerDelivery	, OriginalSciDelivery	, MDivisionID	, FactoryID			, UpdateDate
				, TransferDate
				, BrandID		, OriginalFOC		, OriginalOrderTypeID
			) 
           VALUES
           (
                  1 ,
                  isnull(s.transfer2factroy ,''),
                  isnull(s.id ,              ''),
                  isnull(s.styleid ,         ''),
                  isnull(s.qty ,             0),
                  s.buyerdelivery ,
                  s.scidelivery ,
                  isnull(s.mdivisionid ,     ''),
                  isnull(s.factoryid ,       ''),
                  @dToDay ,
                  @OldDate ,
                  isnull(s.brandid, ''),
				  iif(isnull(s.FOC, 0) = 1 , 'v',''),
				  isnull(s.OrderTypeID, '')
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
			, t.TransferToFactory		= isnull(s.Transfer2Factroy, '')
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
				, t.OrderID				= isnull( s.ID           , '')
				, t.NewStyleID		= isnull( s.StyleID      , '')
				, t.NewQty				= isnull( s.Qty          , 0)
				, t.NewBuyerDelivery	=  s.BuyerDelivery
				, t.NewSCIDelivery		=  s.SCIDelivery
				, t.MDivisionID			= isnull( s.MDivisionID  , '')
				, t.FactoryID			= isnull( s.FactoryID	 , '')
				, t.BrandID				= isnull( s.BrandID		 , '')
				, t.UpdateDate			= @dToday
				, t.TransferDate		= @OldDate
				, t.NewFOC = iif(isnull(s.FOC, 0) = 1 , 'v','')
				, t.NewOrderTypeID = isnull(s.OrderTypeID, '')
		when not matched by target then
			insert (
				NewOrder		, OrderID		, NewStyleID	, NewQty	, NewBuyerDelivery
				, NewSCIDelivery, MDivisionID	, FactoryID			, UpdateDate, TransferDate
				, BrandID		, NewFOC		, NewOrderTypeID
			)
           VALUES
           (
                  1 ,
                  isnull(s.id ,           ''),
                  isnull(s.styleid ,      ''),
                  isnull(s.qty ,          0),
                  s.buyerdelivery ,
                  s.scidelivery ,
                  isnull(s.mdivisionid ,  ''),
                  isnull(s.factoryid ,    ''),
                  @dToDay ,
                  @OldDate ,
                  isnull(s.brandid,''),
				  iif(isnull(s.FOC, 0) = 1 , 'v',''),
				  isnull(s.OrderTypeID, '')
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
						, O_Junk			= iif(isnull(a.Junk, '') != isnull(b.Junk, ''), isnull(A.Junk,0), 0)
						, N_Junk			= iif(isnull(a.Junk, '') != isnull(b.Junk, ''), isnull(b.Junk,0), 0)
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
						, O_PFETA			= IIF(isnull(a.PFETA, '') != isnull(b.PFETA, '') , a.PFETA, null)
						, N_PFETA			= IIF(isnull(a.PFETA, '') != isnull(b.PFETA, '') , b.PFETA, null)
						, O_FOC = iif((iif(isnull(a.FOC, 0) != isnull(b.FOC, 0), a.FOC, 0) = 1),'v','')
						, N_FOC = iif((iif(isnull(a.FOC, 0) != isnull(b.FOC, 0), b.FOC, 0) = 1),'v','')
						, O_OrderTypeID = iif(isnull(a.OrderTypeID, '') != isnull(b.OrderTypeID, 0), a.OrderTypeID, '')
						, N_OrderTypeID = iif(isnull(a.OrderTypeID, '') != isnull(b.OrderTypeID, 0), b.OrderTypeID, '')
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
						OR isnull(A.PFETA, '') != isnull(B.PFETA, '')	
						OR isnull(A.FOC, 0) != isnull(B.FOC, 0)	
						OR isnull(A.OrderTypeID, '') != isnull(B.OrderTypeID, '')	
						)
						and b.FactoryID in (select ID from Factory)) s
		on t.OrderID = s.ID and t.FactoryID = s.FactoryID and t.UpdateDate = @dToday
		when matched then 
			update set
				t.OrderID					= isnull( s.ID             , '')
				, t.FactoryID				= isnull( s.FactoryID      , '')
				, t.BrandID					= isnull( s.BrandID        , '')
				, t.MDivisionID				= isnull( s.MDivisionID    , '')
				, t.OriginalQty				= isnull( s.O_Qty          , 0)
				, t.OriginalBuyerDelivery	= s.O_BuyerDelivery
				, t.OriginalSciDelivery		= s.O_SciDelivery
				, t.OriginalStyleID			= isnull( s.O_Style        , '')
				, t.OriginalCMPQDate		= s.O_CMPQDate
				, t.OriginalEachConsApv		= s.O_EachConsApv
				, t.OriginalMnorderApv		= s.O_MnorderApv
				, t.OriginalSMnorderApv		= s.O_SmnorderApv
				, t.OriginalLETA			= s.O_LETA
				, t.OriginalCustPONo		= isnull( s.O_CustPONo     , '')
				, t.OriginalShipModeList	= isnull( s.O_ShipModeList , '')
				, t.OriginalPFETA 			= s.O_PFETA
				, t.NewQty					= isnull( s.N_Qty          , 0)
				, t.NewBuyerDelivery		= s.N_BuyerDelivery
				, t.NewSciDelivery			= s.N_SciDelivery
				, t.NewStyleID				= isnull( s.N_Style        , '')
				, t.NewCMPQDate				= s.N_CMPQDate
				, t.NewEachConsApv			= s.N_EachConsApv
				, t.NewMnorderApv			= s.N_MnorderApv
				, t.NewSMnorderApv			= s.N_SMnorderApv
				, t.NewLETA					= s.N_LETA
				, t.NewCustPONo      		= isnull( s.N_CustPONo     , '')
				, t.NewShipModeList			= isnull( s.N_ShipModeList , '')
				, t.KPILETA					= s.N_KPILETA
				, t.MnorderApv2				= s.N_MnorderApv2
				, t.NewPFETA 				=  s.N_PFETA
				, t.JunkOrder				= isnull( s.N_Junk         , 0)
				, t.UpdateDate				= @dToday
				, t.TransferDate			= @OldDate
				, t.OriginalFOC				= isnull( s.O_FOC     , '')
				, t.NewFOC			= isnull( s.N_FOC     , '')
				, t.OriginalOrderTypeID			= isnull( s.O_OrderTypeID     , '')
				, t.NewOrderTypeID				= isnull( s.N_OrderTypeID     , '')
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
				, OriginalShipModeList	, NewShipModeList	, OriginalPFETA 	, NewPFETA 
				, OriginalFOC	, NewFOC	, OriginalOrderTypeID 	, NewOrderTypeID 
			) 
           VALUES
           (
                  isnull(s.id ,             ''),
                  isnull(s.factoryid ,      ''),
                  isnull(s.mdivisionid ,    ''),
                  isnull(s.o_qty ,          0),
                  s.o_buyerdelivery ,
                  s.o_scidelivery ,
                  isnull(s.o_style ,        ''),
                  s.o_cmpqdate ,
                  s.o_eachconsapv ,
                  s.o_mnorderapv ,
                  s.o_smnorderapv ,
                  s.o_leta ,
                  s.o_custpono ,
                  isnull(s.n_qty ,          0),
                  s.n_buyerdelivery ,
                  s.n_scidelivery ,
                  isnull(s.n_style ,        ''),
                  s.n_cmpqdate ,
                  s.n_eachconsapv ,
                  s.n_mnorderapv ,
                  s.n_smnorderapv ,
                  s.n_leta ,
                  isnull(s.n_custpono ,     ''),
                  s.n_kpileta ,
                  s.n_mnorderapv2 ,
                  isnull(s.n_junk ,         0),
                  @dToday ,
                  @OldDate ,
                  isnull(s.brandid ,        ''),
                  isnull(s.o_shipmodelist , ''),
                  isnull(s.n_shipmodelist , ''),
                  s.o_pfeta ,
                  s.n_pfeta ,
				  isnull( s.O_FOC     , '') ,
				  isnull( s.N_FOC     , ''), 
				  isnull( s.O_OrderTypeID     , ''), 
				  isnull( s.N_OrderTypeID     , '')
           );

        ----5.No Change!
		Merge Production.dbo.OrderComparisonList as t
		Using Production.dbo.Factory as s
		on t.factoryid=s.id and UpdateDate =@dToDay
		when not matched by Target then 
			insert (
				OrderId			, UpdateDate	, TransferDate	, MDivisionID  , FactoryID
			)
           VALUES
           (
                  'No Change!' ,
                  @dToDay ,
                  @OldDate ,
                  isnull(s.mdivisionid, ''),
                  isnull(s.id, '')
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
			t.OriginalStyleID	= isnull(s.StyleID, '');
-----------------------------------------------------------------------------------------------------------

---------------------PMS_Orders_History delete--------------------------------------
		------------------------將這次要新增的Orders先從歷史區移除
		delete t
		from Production.dbo.PMS_Orders_History t
		where exists (select 1 from #TOrder s where t.ID = s.ID)

		------------------------將 Orders要刪除的資料 且PO_Supp_Detail.ShipQty  > 0 的資料先搬入歷史區
		Merge Production.dbo.PMS_Orders_History as t
		Using (
			select a.*
			from Production.dbo.Orders as a 
			where a.id in (
				select id from #tmpOrders as t 
				where not exists(select 1 from #TOrder as s where t.id=s.ID)
			)
			and 
			(
				exists 
				(	
					select 1 
					from Production.dbo.PO_Supp_Detail p
					left join MDivisionPoDetail c on p.id = c.poid and p.SEQ1=c.Seq1 and p.SEQ2=c.Seq2
					where a.ID = p.ID and (p.ShipQty  > 0 or c.InQty > 0)
				)
				or　exists 
				(
					select 1 from Production.dbo.Invtrans i where i.InventoryPOID = a.ID and i.Type = '1'
				)						
				or exists 
				(
					select 1 
					from Production.dbo.PO_Supp_Detail p
					left join Production.dbo.TransferExport_Detail ted on p.id = ted.poid and p.Seq1　=　ted.Seq1 and p.Seq2 = ted.Seq2
					where a.ID = p.ID
				)
			)
		) as s
		on t.id = s.id
		when matched then 
		update set
				t.ProgramID				= isnull( s.ProgramID ,                ''),
				t.ProjectID				= isnull( s.ProjectID ,                ''),
				t.Category				= isnull( s.Category ,                 ''),
				t.OrderTypeID			= isnull( s.OrderTypeID ,              ''),
				t.BuyMonth				= isnull( s.BuyMonth ,                 ''),
				t.Dest					= isnull( s.Dest ,                     ''),
				t.Model					= isnull( s.Model ,                    ''),
				t.HsCode1				= isnull( s.HsCode1 ,                  ''),
				t.HsCode2				= isnull( s.HsCode2 ,                  ''),
				t.PayTermARID			= isnull( s.PayTermARID ,              ''),
				t.ShipTermID			= isnull( s.ShipTermID ,               ''),
				t.ShipModeList			= isnull( s.ShipModeList ,             ''),
				t.PoPrice				= isnull( s.PoPrice ,                  0),
				t.CFMPrice				= isnull( s.CFMPrice ,                 0),
				t.CurrencyID			= isnull( s.CurrencyID ,               ''),
				t.Commission			= isnull( s.Commission ,               0),
				t.BrandAreaCode			= isnull( s.BrandAreaCode ,            ''),
				t.BrandFTYCode			= isnull( s.BrandFTYCode ,             ''),
				t.CTNQty				= isnull( s.CTNQty ,                   0),
				t.CustCDID				= isnull( s.CustCDID ,                 ''),
				t.CustPONo				= isnull( s.CustPONo ,                 ''),
				t.Customize1			= isnull( s.Customize1 ,               ''),
				t.Customize2			= isnull( s.Customize2 ,               ''),
				t.Customize3			= isnull( s.Customize3 ,               ''),
				t.CMPUnit				= isnull( s.CMPUnit ,                  ''),
				t.CMPPrice				= isnull( s.CMPPrice ,                 0),
				t.CMPQDate				=  s.CMPQDate ,
				t.CMPQRemark			= isnull( s.CMPQRemark ,               ''),
				t.EachConsApv			=  s.EachConsApv ,  
				t.MnorderApv			=  s.MnorderApv , 
				t.CRDDate				=  s.CRDDate , 
				t.InitialPlanDate		=  s.InitialPlanDate , 
				t.PlanDate				=  s.PlanDate ,
				t.FirstProduction		=  s.FirstProduction ,
				t.FirstProductionLock	=  s.FirstProductionLock ,
				t.OrigBuyerDelivery		=  s.OrigBuyerDelivery ,
				t.ExCountry				=  s.ExCountry ,
				t.InDCDate				=  s.InDCDate ,
				t.CFMShipment			=  s.CFMShipment ,
				t.PFETA					=  s.PFETA ,
				t.PackLETA				=  s.PackLETA ,
				t.LETA					=  s.LETA ,
				t.MRHandle				= isnull( s.MRHandle ,                 ''),
				t.SMR					= isnull( s.SMR ,                      ''),
				t.ScanAndPack			= isnull( s.ScanAndPack ,              0),
				t.VasShas				= isnull( s.VasShas ,                  0),
				t.SpecialCust			= isnull( s.SpecialCust ,              0),
				t.TissuePaper			= isnull( s.TissuePaper ,              0),
				t.Packing				= isnull( s.Packing ,                  ''),
				t.SDPDate				=  s.SDPDate,
				t.MarkFront				= isnull( s.MarkFront ,                ''),
				t.MarkBack				= isnull( s.MarkBack ,                 ''),
				t.MarkLeft				= isnull( s.MarkLeft ,                 ''),
				t.MarkRight				= isnull( s.MarkRight ,                ''),
				t.Label					= isnull( s.Label ,                    ''),
				t.OrderRemark			= isnull( s.OrderRemark ,              ''),
				t.ArtWorkCost			= isnull( s.ArtWorkCost ,              ''),
				t.StdCost				= isnull( s.StdCost ,                  0),
				t.CtnType				= isnull( s.CtnType ,                  ''),
				t.FOCQty				= isnull( s.FOCQty ,                   0),
				t.SMnorderApv			=  s.SMnorderApv ,
				t.FOC					= isnull( s.FOC ,                      0),
				t.MnorderApv2			=  s.MnorderApv2 ,
				t.Packing2				= isnull( s.Packing2 ,                 ''),
				t.SampleReason			= isnull( s.SampleReason ,             ''),
				t.RainwearTestPassed	= isnull( s.RainwearTestPassed ,       0),
				t.SizeRange				= isnull( s.SizeRange ,                ''),
				t.MTLComplete			= isnull( s.MTLComplete ,              0),
				t.SpecialMark			= isnull( s.SpecialMark ,              ''),
				t.OutstandingRemark		= isnull( s.OutstandingRemark ,        ''),
				t.OutstandingInCharge	= isnull( s.OutstandingInCharge ,      ''),
				t.OutstandingDate		=  s.OutstandingDate ,
				t.OutstandingReason		= isnull( s.OutstandingReason ,        ''),
				t.StyleUkey				= isnull( s.StyleUkey ,                0),
				t.POID					= isnull( s.POID ,                     ''),
				t.OrderComboID			= isnull( s.OrderComboID ,             ''),
				t.IsNotRepeatOrMapping	= isnull( s.IsNotRepeatOrMapping ,     0),
				t.SplitOrderId			= isnull( s.SplitOrderId ,             ''),
				t.FtyKPI				=  s.FtyKPI ,
				t.EditName				= isnull( s.EditName ,                 ''),
				t.EditDate				=  s.EditDate ,
				t.IsForecast			= isnull( s.IsForecast ,               0),
				t.PulloutComplete		= isnull( s.PulloutComplete ,          0),
				t.PFOrder				= isnull( s.PFOrder ,                  0),
				t.KPILETA				=  s.KPILETA ,
				t.MTLETA				=  s.MTLETA ,
				t.SewETA				=  s.SewETA ,
				t.PackETA				=  s.PackETA ,
				t.MTLExport				= isnull( s.MTLExport ,                ''),
				t.DoxType				= isnull( s.DoxType ,                  ''),
				t.MDivisionID			= isnull( s.MDivisionID ,              ''),
				t.KPIChangeReason		= isnull( s.KPIChangeReason ,          ''),
				t.MDClose				=  s.MDClose ,
				t.CPUFactor				= isnull( s.CPUFactor ,                0),
				t.SizeUnit				= isnull( s.SizeUnit ,                 ''),
				t.CuttingSP				= isnull( s.CuttingSP ,                ''),
				t.IsMixMarker			= isnull( s.IsMixMarker ,              0),
				t.EachConsSource		= isnull( s.EachConsSource ,           ''),
				t.KPIEachConsApprove	=  s.KPIEachConsApprove ,
				t.KPICmpq				=  s.KPICmpq ,
				t.KPIMNotice			=  s.KPIMNotice ,
				t.GMTComplete			= isnull( s.GMTComplete  ,             ''),
				t.GFR					= isnull( s.GFR	,                      0),
				t.FactoryID				= isnull( s.FactoryID,                 ''),
				t.BrandID				= isnull( s.BrandID,                   ''),
				t.StyleID				= isnull( s.StyleID,                   ''),
				t.SeasonID				= isnull( s.SeasonID,                  ''),
				t.BuyerDelivery			=  s.BuyerDelivery,
				t. SciDelivery			=  s.SciDelivery,
				t.CFMDate				=  s.CFMDate,
				t.Junk					= isnull( s.Junk ,                     0),
				t.CdCodeID				= isnull( s.CdCodeID,                  ''),
				t.CPU					= isnull( s.CPU,                       0),
				t.Qty					= isnull( s.Qty,                       0),
				t.StyleUnit				= isnull( s.StyleUnit, 				   ''),
				t.AddName				= isnull( s.AddName,                   ''),
				t.AddDate				=  s.AddDate,
				t.FtyGroup              = isnull( s.FtyGroup,                  ''),
				t.ForecastSampleGroup   = isnull( s.ForecastSampleGroup,	   ''),
				t.DyeingLoss			= isnull( s.DyeingLoss,                0),
				t.SubconInType			= isnull( s.SubconInType,              ''),
				t.LastProductionDate    =  s.LastProductionDate,
				t.EstPODD				=  s.EstPODD,
				t.AirFreightByBrand    = isnull( s.AirFreightByBrand,          0),
				t.AllowanceComboID	   = isnull( s.AllowanceComboID,           ''),
				t.ChangeMemoDate       =  s.ChangeMemoDate,
				t.ForecastCategory     = isnull( s.ForecastCategory,           ''),
				t.OnSiteSample		   = isnull( s.OnSiteSample,               0),
				t.PulloutCmplDate	   =  s.PulloutCmplDate ,
				t.NeedProduction	   = isnull( s.NeedProduction ,            0),
				t.KeepPanels           = isnull( s.KeepPanels ,                0),
				t.IsBuyBack			   = isnull( s.IsBuyBack ,                 0),
				t.BuyBackReason           = isnull( s.BuyBackReason ,          ''),
				t.IsBuyBackCrossArticle   = isnull( s.IsBuyBackCrossArticle ,  0),
				t.IsBuyBackCrossSizeCode  = isnull( s.IsBuyBackCrossSizeCode , 0),
				t.KpiEachConsCheck	   =  s.KpiEachConsCheck, 
				t.CMPLTDATE	   =  s.CMPLTDATE,
				t.HangerPack = isnull( s.HangerPack,                           0),
				t.DelayCode = isnull( s.DelayCode,                             ''),
				t.DelayDesc = isnull( s.DelayDesc,                             ''),
				t.SizeUnitWeight = isnull( s.SizeUnitWeight,                   '')
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
			, ForecastCategory		, OnSiteSample			, PulloutCmplDate		, NeedProduction		, KeepPanels
			, IsBuyBack				, BuyBackReason			, IsBuyBackCrossArticle , IsBuyBackCrossSizeCode
			, KpiEachConsCheck		, CMPLTDATE				, HangerPack			, DelayCode				, DelayDesc
			, SizeUnitWeight
		) 
       VALUES
       (
              isnull(s.id ,                      ''),
              isnull(s.brandid ,                 ''),
              isnull(s.programid ,               ''),
              isnull(s.styleid ,                 ''),
              isnull(s.seasonid ,                ''),
              isnull(s.projectid ,               ''),
              isnull(s.category ,                ''),
              isnull(s.ordertypeid ,             ''),
              isnull(s.buymonth ,                ''),
              isnull(s.dest ,                    ''),
              isnull(s.model ,                   ''),
              isnull(s.hscode1 ,                 ''),
              isnull(s.hscode2 ,                 ''),
              isnull(s.paytermarid ,             ''),
              isnull(s.shiptermid ,              ''),
              isnull(s.shipmodelist ,            ''),
              isnull(s.cdcodeid ,                ''),
              isnull(s.cpu ,                     0),
              isnull(s.qty ,                     0),
              isnull(s.styleunit ,               ''),
              isnull(s.poprice ,                 0),
              isnull(s.cfmprice ,                0),
              isnull(s.currencyid ,              ''),
              isnull(s.commission ,              0),
              isnull(s.factoryid ,               ''),
              isnull(s.brandareacode ,           ''),
              isnull(s.brandftycode ,            ''),
              isnull(s.ctnqty ,                  0),
              isnull(s.custcdid ,                ''),
              isnull(s.custpono ,                ''),
              isnull(s.customize1 ,              ''),
              isnull(s.customize2 ,              ''),
              isnull(s.customize3 ,              ''),
              s.cfmdate ,      
              s.buyerdelivery ,
              s.scidelivery ,  
              s.sewoffline ,   
              s.cutinline ,
              s.cutoffline ,
              isnull(s.cmpunit ,                 ''),
              isnull(s.cmpprice ,                0),
              s.cmpqdate ,
              isnull(s.cmpqremark ,              ''),
              s.eachconsapv ,        
              s.mnorderapv ,         
              s.crddate ,            
              s.initialplandate ,    
              s.plandate ,           
              s.firstproduction ,    
              s.firstproductionlock ,
              s.origbuyerdelivery ,  
              s.excountry ,          
              s.indcdate ,           
              s.cfmshipment ,        
              s.pfeta ,
              s.packleta ,
              s.leta ,
              isnull(s.mrhandle ,                ''),
              isnull(s.smr ,                     ''),
              isnull(s.scanandpack ,             0),
              isnull(s.vasshas ,                 0),
              isnull(s.specialcust ,             0),
              isnull(s.tissuepaper ,             0),
              isnull(s.junk ,                    0),
              isnull(s.packing ,                 ''),
              isnull(s.markfront ,               ''),
              isnull(s.markback ,                ''),
              isnull(s.markleft ,                ''),
              isnull(s.markright ,               ''),
              isnull(s.label ,                   ''),
              isnull(s.orderremark ,             ''),
              isnull(s.artworkcost ,             ''),
              isnull(s.stdcost ,                 0),
              isnull(s.ctntype ,                 ''),
              isnull(s.focqty ,                  0),
              s.smnorderapv ,
              isnull(s.foc ,                     0),
              s.mnorderapv2 ,
              isnull(s.packing2 ,                ''),
              isnull(s.samplereason ,            ''),
              isnull(s.rainweartestpassed ,      0),
              isnull(s.sizerange ,               ''),
              isnull(s.mtlcomplete ,             0),
              isnull(s.specialmark ,             ''),
              isnull(s.outstandingremark ,       ''),
              isnull(s.outstandingincharge ,     ''),
              s.outstandingdate ,
              isnull(s.outstandingreason ,       ''),
              isnull(s.styleukey ,               0),
              isnull(s.poid ,                    ''),
              isnull(s.ordercomboid ,            ''),
              isnull(s.isnotrepeatormapping,     0),
              isnull(s.splitorderid ,            ''),
              s.ftykpi ,
              isnull(s.addname ,                 ''),
              s.adddate ,
              isnull(s.editname ,                ''),
              s.editdate ,
              isnull(s.isforecast ,              0),
              isnull(s.gmtcomplete ,             ''),
              isnull(s.pforder ,                 0),
              s.kpileta ,
              s.mtleta ,
              s.seweta ,
              s.packeta ,
              isnull(s.mtlexport ,               ''),
              isnull(s.doxtype ,                 ''),
              isnull(s.mdivisionid ,             ''),
              isnull(s.mchandle ,                ''),
              isnull(s.kpichangereason ,         ''),
              s.mdclose ,
              isnull(s.cpufactor ,               0),
              isnull(s.sizeunit ,                ''),
              isnull(s.cuttingsp ,               ''),
              isnull(s.ismixmarker ,            0),
              isnull(s.eachconssource ,          ''),
              s.kpieachconsapprove ,
              s.kpicmpq ,
              s.kpimnotice ,
              isnull(s.gfr ,                     0),
              s.sdpdate ,
              isnull(s.pulloutcomplete ,         0),
              s.sewinline ,
              isnull(s.ftygroup ,                ''),
              isnull(s.forecastsamplegroup ,     ''),
              isnull(s.dyeingloss ,              0),
              isnull(s.subconintype ,            ''),
              s.lastproductiondate ,
              s.estpodd ,
              isnull(s.airfreightbybrand ,       0),
              isnull(s.allowancecomboid ,        ''),
              s.changememodate ,
              isnull(s.forecastcategory ,        ''),
              isnull(s.onsitesample ,            0),
              s.pulloutcmpldate ,        
              isnull(s.needproduction ,          0),
              isnull(s.keeppanels ,              0),
              isnull(s.isbuyback ,               0),
              isnull(s.buybackreason ,           ''),
              isnull(s.isbuybackcrossarticle ,   0),
              isnull(s.isbuybackcrosssizecode ,  0),
              s.kpieachconscheck ,  
              s.cmpltdate ,        
              isnull(s.hangerpack ,              0),
              isnull(s.delaycode ,               ''),
              isnull(s.delaydesc ,               ''),
              isnull(s.sizeunitweight,           '')
       );

-----------------------------------------------------------------------------------------------------------
---------------------Order--------------------------------------
		--------------Order.id= AOrder.id  if eof()
		declare @OrderT table (ID varchar(13),isInsert bit) 

		Merge Production.dbo.Orders as t
		Using   #TOrder as s
		on t.id=s.id
		when matched then 
		update set
				t.ProgramID				= isnull( s.ProgramID ,          ''),
				t.ProjectID				= isnull( s.ProjectID ,          ''),
				t.Category				= isnull( s.Category ,           ''),
				t.OrderTypeID			= isnull( s.OrderTypeID ,        ''),
				t.BuyMonth				= isnull( s.BuyMonth ,           ''),
				t.Dest					= isnull( s.Dest ,               ''),
				t.Model					= isnull( s.Model ,              ''),
				t.HsCode1				= isnull( s.HsCode1 ,            ''),
				t.HsCode2				= isnull( s.HsCode2 ,            ''),
				t.PayTermARID			= isnull( s.PayTermARID ,        ''),
				t.ShipTermID			= isnull( s.ShipTermID ,         ''),
				t.ShipModeList			= isnull( s.ShipModeList ,       ''),
				t.PoPrice				= isnull( s.PoPrice ,            0),
				t.CFMPrice				= isnull( s.CFMPrice ,           0),
				t.CurrencyID			= isnull( s.CurrencyID ,         ''),
				t.Commission			= isnull( s.Commission ,         0),
				t.BrandAreaCode			= isnull( s.BrandAreaCode ,      ''),
				t.BrandFTYCode			= isnull( s.BrandFTYCode ,       ''),
				t.CTNQty				= isnull( s.CTNQty ,             0),
				t.CustCDID				= isnull( s.CustCDID ,           ''),
				t.CustPONo				= isnull( s.CustPONo ,           ''),
				t.Customize1			= isnull( s.Customize1 ,         ''),
				t.Customize2			= isnull( s.Customize2 ,         ''),
				t.Customize3			= isnull( s.Customize3 ,         ''),
				t.CMPUnit				= isnull( s.CMPUnit ,            ''),
				t.CMPPrice				= isnull( s.CMPPrice ,           0),
				t.CMPQDate				=  s.CMPQDate ,
				t.CMPQRemark			= isnull( s.CMPQRemark ,         ''),
				t.EachConsApv			=  s.EachConsApv ,        
				t.MnorderApv			=  s.MnorderApv ,         
				t.CRDDate				=  s.CRDDate ,            
				t.InitialPlanDate		=  s.InitialPlanDate ,    
				t.PlanDate				=  s.PlanDate ,           
				t.FirstProduction		=  s.FirstProduction ,    
				t.FirstProductionLock	=  s.FirstProductionLock ,
				t.OrigBuyerDelivery		=  s.OrigBuyerDelivery ,  
				t.ExCountry				=  s.ExCountry ,          
				t.InDCDate				=  s.InDCDate ,           
				t.CFMShipment			=  s.CFMShipment ,        
				t.PFETA					=  s.PFETA ,     
				t.PackLETA				=  s.PackLETA ,  
				t.LETA					=  s.LETA , 
				t.MRHandle				= isnull( s.MRHandle ,           ''),
				t.SMR					= isnull( s.SMR ,                ''),
				t.ScanAndPack			= isnull( s.ScanAndPack ,        0),
				t.VasShas				= isnull( s.VasShas ,            0),
				t.SpecialCust			= isnull( s.SpecialCust ,        0),
				t.TissuePaper			= isnull( s.TissuePaper ,        0),
				t.Packing				= isnull( s.Packing ,            ''),
				--t.SDPDate				= s.SDPDate, --工廠交期只需要INSERT填預設值,不須UPDATE
				t.MarkFront				= isnull( s.MarkFront ,   ''),
				t.MarkBack				= isnull( s.MarkBack ,    ''),
				t.MarkLeft				= isnull( s.MarkLeft ,    ''),
				t.MarkRight				= isnull( s.MarkRight ,   ''),
				t.Label					= isnull( s.Label ,       ''),
				t.OrderRemark			= isnull( s.OrderRemark , ''),
				t.ArtWorkCost			= isnull( s.ArtWorkCost , ''),
				t.StdCost				= isnull( s.StdCost ,     0),
				t.CtnType				= isnull( s.CtnType ,     ''),
				t.FOCQty				= isnull( s.FOCQty ,      0),
				t.SMnorderApv			= s.SMnorderApv ,
				t.FOC					= isnull(s.FOC ,0),
				t.MnorderApv2			= s.MnorderApv2 ,
				t.Packing2				= isnull( s.Packing2 ,           ''),
				t.SampleReason			= isnull( s.SampleReason ,       ''),
				t.RainwearTestPassed	= isnull( s.RainwearTestPassed , 0),
				t.SizeRange				= isnull( s.SizeRange ,          ''),
				t.MTLComplete			= isnull( s.MTLComplete ,        0),
				t.SpecialMark			= isnull( s.SpecialMark ,        ''),
				t.OutstandingRemark		= isnull(iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingRemark,s.OutstandingRemark), ''),
				t.OutstandingInCharge	= isnull(iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingInCharge,s.OutstandingInCharge), ''),
				t.OutstandingDate		= iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingDate,s.OutstandingDate),
				t.OutstandingReason		= isnull(iif((s.OutstandingDate <= t.OutstandingDate AND s.OutstandingDate is null) OR (s.OutstandingDate is not null  AND s.OutstandingDate <= t.OutstandingDate),t.OutstandingReason,s.OutstandingReason), ''),
				t.StyleUkey				= isnull( s.StyleUkey ,             0),
				t.POID					= isnull( s.POID ,                  ''),
				t.OrderComboID			= isnull( s.OrderComboID,           ''),
				t.IsNotRepeatOrMapping	= isnull( s.IsNotRepeatOrMapping ,  0),
				t.SplitOrderId			= isnull( s.SplitOrderId ,          ''),
				t.FtyKPI				= s.FtyKPI ,
				t.EditName				= ISNULL(iif((s.EditDate <= t.EditDate AND s.EditDate is null) OR (s.EditDate is not null AND s.EditDate <= t.EditDate),t.EditName, s.EditName), '') ,
				t.EditDate				= iif((s.EditDate <= t.EditDate AND s.EditDate is null) OR (s.EditDate is not null AND s.EditDate <= t.EditDate),t.EditDate, s.EditDate) ,
				t.IsForecast			= isnull(s.IsForecast ,0),
				t.PulloutComplete		= isnull(iif((s.GMTComplete='C' OR s.GMTComplete='S') and t.PulloutComplete=0  ,1,t.PulloutComplete),0),
				t.PFOrder				= isnull(s.PFOrder , 0),
				t.KPILETA				= s.KPILETA ,
				t.MTLETA				= s.MTLETA ,
				t.SewETA				= s.SewETA ,
				t.PackETA				= s.PackETA ,
				t.MTLExport				= isnull( s.MTLExport ,      ''),
				t.DoxType				= isnull( s.DoxType ,        ''),
				t.MDivisionID			= isnull( s.MDivisionID ,    ''),
				t.KPIChangeReason		= isnull( s.KPIChangeReason ,''),
				t.MDClose				= iif((s.GMTComplete='C' OR s.GMTComplete='S') and t.PulloutComplete=0  ,@dToDay,t.MDClose),
				t.CPUFactor				= isnull( s.CPUFactor ,     ''),
				t.SizeUnit				= isnull( s.SizeUnit ,      ''),
				t.CuttingSP				= isnull( s.CuttingSP ,     ''),
				t.IsMixMarker			= isnull( s.IsMixMarker ,   ''),
				t.EachConsSource		= isnull( s.EachConsSource ,''),
				t.KPIEachConsApprove	= s.KPIEachConsApprove ,
				t.KPICmpq				= s.KPICmpq ,
				t.KPIMNotice			= s.KPIMNotice ,
				t.GMTComplete			= isnull( s.GMTComplete  , ''),
				t.GFR					= isnull( s.GFR	,          0),
				t.FactoryID				= isnull( s.FactoryID,     ''),
				t.BrandID				= isnull( s.BrandID,       ''),
				t.StyleID				= isnull( s.StyleID,       ''),
				t.SeasonID				= isnull( s.SeasonID,      ''),
				t.BuyerDelivery			= s.BuyerDelivery,
				t. SciDelivery			= s.SciDelivery, 
				t.CFMDate				= s.CFMDate, 
				t.Junk					= isnull(s.Junk,0),
				t.CdCodeID				= isnull( s.CdCodeID, ''),
				t.CPU					= isnull( s.CPU,                0),
				t.Qty					= isnull( s.Qty,                0),
				t.StyleUnit				= isnull( s.StyleUnit, 		    ''),
				t.AddName				= isnull( s.AddName,            ''),
				t.AddDate				=  s.AddDate,
				t.FtyGroup              = isnull( s.FTY_Group,          ''),
				t.ForecastSampleGroup   = isnull( s.ForecastSampleGroup, ''),
				t.DyeingLoss			= isnull( s.DyeingLoss,         0),
				t.SubconInType = '0',
				t.LastProductionDate       = s.LastProductionDate,				
				t.EstPODD       = s.EstPODD,
				t.AirFreightByBrand    = isnull (s.AirFreightByBrand, 0),
				t.AllowanceComboID = isnull (s.AllowanceComboID, ''),
				t.ChangeMemoDate       = s.ChangeMemoDate,
				t.ForecastCategory     = isnull (s.ForecastCategory, ''),
				t.OnSiteSample		   = isnull (s.OnSiteSample, 0),
				t.PulloutCmplDate	   = s.PulloutCmplDate,
				t.NeedProduction	   = isnull (s.NeedProduction, 0),
				t.KeepPanels           = isnull (s.KeepPanels, 0),
				t.IsBuyBack			   = isnull (s.IsBuyBack, 0),
				t.BuyBackReason           = isnull (s.BuyBackReason, ''),
				t.IsBuyBackCrossArticle           = isnull (s.IsBuyBackCrossArticle, 0),
				t.IsBuyBackCrossSizeCode           = isnull (s.IsBuyBackCrossSizeCode, 0),
				t.KpiEachConsCheck		=  s.KpiEachConsCheck,
				t.CMPLTDATE				=  s.CMPLTDATE,
				t.HangerPack			= isnull( s.HangerPack,                0),
				t.DelayCode				= isnull( s.DelayCode,                  ''),
				t.DelayDesc				= isnull( s.DelayDesc,                  ''),
				t.SizeUnitWeight		= isnull( s.SizeUnitWeight        ,''),
				t.OrganicCotton			= isnull(s.OrganicCotton, 0),
				t.DirectShip			= isnull(s.DirectShip,0),
				t.ScheETANoReplace		= s.ScheETANoReplace,
				t.SCHDLETA				= s.SCHDLETA,
				t.Transferdate			= s.Transferdate,
				t.Max_ScheETAbySP		= s.Max_ScheETAbySP,
				t.Sew_ScheETAnoReplace	= s.Sew_ScheETAnoReplace,
				t.MaxShipETA_Exclude5x	= s.MaxShipETA_Exclude5x,
				t.Customize4            = isnull(s.Customize4,''),
				t.Customize5            = isnull(s.Customize5,''),
				t.OrderCompanyID	    = isnull(s.OrderCompany, 0),
				t.JokerTag 			= isnull( s.JokerTag ,                0),
				t.HeatSeal 			= isnull( s.HeatSeal ,                0)
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
			, ForecastCategory		, OnSiteSample			, PulloutCmplDate		, NeedProduction		, KeepPanels
			, IsBuyBack				, BuyBackReason			, IsBuyBackCrossArticle , IsBuyBackCrossSizeCode
			, KpiEachConsCheck		, CMPLTDATE				, HangerPack			, DelayCode				, DelayDesc
			, SizeUnitWeight		, OrganicCotton         , DirectShip			, ScheETANoReplace		, SCHDLETA
			, Transferdate			, Max_ScheETAbySP		, Sew_ScheETAnoReplace	, MaxShipETA_Exclude5x  , OrderCompanyID
			, Customize4            , Customize5			, LocalMR			    , JokerTag 			    , HeatSeal 

		) 
       VALUES
       (
              isnull(s.id ,                 ''),
              isnull(s.brandid ,            ''),
              isnull(s.programid ,          ''),
              isnull(s.styleid ,            ''),
              isnull(s.seasonid ,           ''),
              isnull(s.projectid ,          ''),
              isnull(s.category ,           ''),
              isnull(s.ordertypeid ,        ''),
              isnull(s.buymonth ,           ''),
              isnull(s.dest ,               ''),
              isnull(s.model ,              ''),
              isnull(s.hscode1 ,            ''),
              isnull(s.hscode2 ,            ''),
              isnull(s.paytermarid ,        ''),
              isnull(s.shiptermid ,         ''),
              isnull(s.shipmodelist ,       ''),
              isnull(s.cdcodeid ,           ''),
              isnull(s.cpu ,                0),
              isnull(s.qty ,                0),
              isnull(s.styleunit ,          ''),
              isnull(s.poprice ,            0),
              isnull(s.cfmprice ,           0),
              isnull(s.currencyid ,         ''),
              isnull(s.commission ,         0),
              isnull(s.factoryid ,          ''),
              isnull(s.brandareacode ,      ''),
              isnull(s.brandftycode ,       ''),
              isnull(s.ctnqty ,             0),
              isnull(s.custcdid ,           ''),
              isnull(s.custpono ,           ''),
              isnull(s.customize1 ,         ''),
              isnull(s.customize2 ,         ''),
              isnull(s.customize3 ,         ''),
              s.cfmdate ,      
              s.buyerdelivery ,
              s.scidelivery ,  
              s.sewoffline ,   
              s.cutinline ,    
              s.cutoffline ,
              isnull(s.cmpunit ,            ''),
              isnull(s.cmpprice ,           0),
              s.cmpqdate ,
              isnull(s.cmpqremark ,         ''),
              s.eachconsapv ,        
              s.mnorderapv ,         
              s.crddate ,            
              s.initialplandate ,    
              s.plandate ,           
              s.firstproduction ,    
              s.firstproductionlock ,
              s.origbuyerdelivery ,  
              s.excountry ,          
              s.indcdate ,           
              s.cfmshipment ,        
              s.pfeta ,              
              s.packleta ,           
              s.leta ,
              isnull(s.mrhandle ,           ''),
              isnull(s.smr ,                ''),
              isnull(s.scanandpack ,        0),
              isnull(s.vasshas ,            0),
              isnull(s.specialcust ,        0),
              isnull(s.tissuepaper ,        0),
              isnull(s.junk,0) ,
              isnull(s.packing ,               ''),
              isnull(s.markfront ,             ''),
              isnull(s.markback ,              ''),
              isnull(s.markleft ,              ''),
              isnull(s.markright ,             ''),
              isnull(s.label ,                 ''),
              isnull(s.orderremark ,           ''),
              isnull(s.artworkcost ,           ''),
              isnull(s.stdcost ,               0),
              isnull(s.ctntype ,               ''),
              isnull(s.focqty ,                0),
              s.smnorderapv ,
              isnull(s.foc ,                   0),
              s.mnorderapv2 ,
              isnull(s.packing2 ,              ''),
              isnull(s.samplereason ,          ''),
              isnull(s.rainweartestpassed ,    0),
              isnull(s.sizerange ,             ''),
              isnull(s.mtlcomplete ,           0),
              isnull(s.specialmark ,           ''),
              isnull(s.outstandingremark ,     ''),
              isnull(s.outstandingincharge ,   ''),
              s.outstandingdate ,
              isnull(s.outstandingreason ,     ''),
              isnull(s.styleukey ,             0),
              isnull(s.poid ,                  ''),
              isnull(s.ordercomboid ,          ''),
              isnull(s.isnotrepeatormapping,   0),
              isnull(s.splitorderid ,          ''),
              s.ftykpi ,
              isnull(s.addname ,               ''),
              s.adddate ,
              isnull(s.editname ,              ''),
              s.editdate ,
              isnull(s.isforecast ,            0),
              isnull(s.gmtcomplete ,           ''),
              isnull(s.pforder ,               0),
              s.kpileta ,
              s.mtleta ,
              s.seweta ,
              s.packeta ,
              isnull(s.mtlexport ,             ''),
              isnull(s.doxtype ,               ''),
              isnull(s.mdivisionid ,           ''),
              isnull(s.mchandle ,              ''),
              isnull(s.kpichangereason ,       ''),
              s.mdclose ,
              isnull(s.cpufactor ,             0),
              isnull(s.sizeunit ,              ''),
              isnull(s.cuttingsp ,             ''),
              isnull(s.ismixmarker ,           0),
              isnull(s.eachconssource ,        ''),
              s.kpieachconsapprove ,
              s.kpicmpq ,
              s.kpimnotice ,
              isnull(s.gfr ,                   0),
              s.sdpdate ,
              isnull(s.pulloutcomplete ,       0),
              s.sewinline ,
              isnull (s.fty_group ,             ''),
              isnull (s.forecastsamplegroup ,   ''),
              isnull (s.dyeingloss ,            0),
              '0' ,
              s.lastproductiondate ,
              s.estpodd ,
              isnull (s.airfreightbybrand , 0) ,
              isnull (s.allowancecomboid ,  '') ,
              s.changememodate ,
              isnull (s.forecastcategory ,  '') ,
              isnull (s.onsitesample ,      0) ,
              s.pulloutcmpldate ,
              isnull (s.needproduction, 0) ,
              isnull (s.keeppanels, 0) ,
              isnull (s.isbuyback, 0),
              isnull (s.buybackreason, '') ,
              isnull (s.isbuybackcrossarticle, 0) ,
              isnull (s.isbuybackcrosssizecode, 0) ,
              s.kpieachconscheck ,
              s.cmpltdate ,
              isnull(s.hangerpack ,      0) ,
              isnull(s.delaycode ,       '') ,
              isnull(s.delaydesc ,       '') ,
              isnull(s.sizeunitweight,   '') ,
              isnull(s.OrganicCotton, 0) ,
              isnull(s.DirectShip,0),
              s.ScheETANoReplace,
              s.SCHDLETA,
			  s.Transferdate,
			  s.Max_ScheETAbySP,
			  s.Sew_ScheETAnoReplace,
			  s.MaxShipETA_Exclude5x,
			  isnull(s.OrderCompany, 0),
              isnull(s.Customize4, ''),
              isnull(s.Customize5, ''),
			  isnull( 
			  (
				select localMR 
				from Production.dbo.Style 
				where BrandID = s.BrandID 
				and id = s.styleid 
				and SeasonID = s.SeasonID ), 
				''),
              isnull(s.JokerTag ,      0),
              isnull(s.HeatSeal  ,      0)
       )
		output inserted.id, iif(deleted.id is null,1,0) into @OrderT; --將insert =1 , update =0 把改變過的id output;


	------------- Update Local Order CPU--------------
	update o
	set o.CPU = s.CPU
	,o.EditDate = GETDATE()
	,o.EditName = 'SCIMIS'
	from Production.dbo.orders o with(nolock) 
	inner join Production.dbo.Style s with(nolock) on s.Ukey = o.StyleUkey and s.CPU <> 0
	where o.LocalOrder=1
	and not exists (select 1 from Production.dbo.SewingOutput_Detail sd with(nolock) where sd.OrderId = o.ID)
	and o.CPU <> s.CPU

	--------------Order_Qty--------------------------Qty BreakDown
	--抓出轉入order_qty有異動的資料，作為後續傳送給廠商資料的依據
	Delete Production.dbo.AutomationOrderQty;

	insert into Production.dbo.AutomationOrderQty(ID, Article, SizeCode)
	SELECT
        isnull(tradeoq.id,       ''),
        isnull(tradeoq.article,  ''),
        isnull(tradeoq.sizecode, '')
	from Trade_To_Pms.dbo.order_qty tradeoq with (nolock)
	inner join #Torder  b on tradeoq.id=b.id
	left join Production.dbo.Order_Qty oq with (nolock) on tradeoq.id=oq.id AND tradeoq.ARTICLE=oq.ARTICLE AND tradeoq.sizeCode=oq.sizeCode
	where oq.Qty <> tradeoq.Qty or oq.Qty is null

	Merge Production.dbo.Order_Qty as t
	Using (select a.* from Trade_To_Pms.dbo.order_qty a WITH (NOLOCK) inner join #Torder  b on a.id=b.id) as s
	on t.id=s.id AND T.ARTICLE=S.ARTICLE AND t.sizeCode=s.sizeCode
	when matched then
		update set
			t.Qty		= isnull( s.Qty ,       0),
			t.AddName	= isnull( s.AddName ,   ''),
			t.AddDate	=  s.AddDate , 
			t.EditName	= isnull( s.EditName ,  ''),
			t.EditDate	=  s.EditDate ,
			t.OriQty	= isnull( s.OriQty,     0)
	when not matched by target then 
		insert (
			ID			, Article	, SizeCode		, Qty		,AddName 
			, AddDate	, EditName	, EditDate		, OriQty 
		) 
       VALUES
       (
              isnull(s.id ,             ''),
              isnull(s.article ,        ''),
              isnull(rtrim(s.sizecode) ,''),
              isnull(s.qty ,            0),
              isnull(s.addname ,        ''),
              s.adddate ,
              isnull(s.editname,        ''),
              s.editdate ,
              isnull(s.oriqty,          0)
       )
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then
		delete;

	----------Order_Qty_Garment--------------
	Merge Production.dbo.Order_Qty_Garment as t
	Using (
		select a.*
			   , OrdersJunk = isnull(b.Junk,0)
		from Trade_To_Pms.dbo.Order_Qty_Garment a With (NoLock) 
		inner join #TOrder b on a.id = b.id
	) as s on t.ID = s.ID
			  and t.OrderIDFrom = s.OrderIDFrom
			  and t.Article = s.Article
			  and t.SizeCode = s.SizeCode
  	when matched then 
  		update set
  			t.Qty			= isnull( s.Qty       , 0)
			, t.AddName		= isnull( s.AddName   , '')
			, t.AddDate		=  s.AddDate
			, t.EditName	= isnull( s.EditName  , '')
			, t.EditDate	=  s.EditDate
			, t.Junk 		= isnull( s.OrdersJunk, 0)
	when not matched by target then
		insert (
			ID 			, OrderIDFrom	, Article 		, SizeCode 		, Qty
			, AddName 	, AddDate		, EditName 		, EditDate		, Junk
		)
       VALUES
       (
              isnull(s.id ,          ''),
              isnull(s.orderidfrom , ''),
              isnull(s.article ,     ''),
              isnull(s.sizecode ,    ''),
              isnull(s.qty ,         0),
              isnull(s.addname ,     ''),
              s.adddate ,
              isnull(s.editname ,    ''),
              s.editdate ,
              0
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
			t.ShipmodeID	= isnull( s.ShipmodeID ,    ''),
			t.BuyerDelivery =  s.BuyerDelivery ,
			t.FtyKPI		=  s.FtyKPI ,   
			t.ReasonID		= isnull( s.ReasonID ,      ''),
			t.Qty			= isnull( s.Qty ,           0),
			t.AddName		= isnull( s.AddName ,       ''),
			t.AddDate		=  s.AddDate ,  
			t.EditName		= isnull( s.EditName ,      ''),
			t.EditDate		=  s.EditDate,    
			t.OriQty		= isnull( s.OriQty  ,       0)
	when not matched by target then
		insert  (  
			Id		, Seq		, ShipmodeID	, BuyerDelivery		, FtyKPI		, ReasonID 
			, Qty	, AddName	, AddDate		, EditName			, EditDate		, OriQty 
		)
       VALUES
       (
              isnull(s.id ,            ''),
              isnull(s.seq ,           ''),
              isnull(s.shipmodeid ,    ''),
              s.buyerdelivery ,
              s.ftykpi ,
              isnull(s.reasonid ,      ''),
              isnull(s.qty ,           0),
              isnull(s.addname ,       ''),
              s.adddate ,  
              isnull(s.editname ,      ''),
              s.editdate ,    
              isnull(s.oriqty  ,       0)
       )
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;

	--ISP20201565
	update oqs
		set oqs.CFAIs3rdInspect = 1
	from Order_QtyShip oqs
	where exists (
		select 1 from Orders o
		inner join CustCD c on o.BrandID = c.BrandID and o.CustCDID = c.ID
		where c.Need3rdInspect = 1 
		and o.ID = oqs.Id
	)
	-----------Order_QtyShip_Detail--------------------------調整: 來源比對Production表頭資料 
		Merge Production.dbo.Order_QtyShip_detail as t
		Using (select a.* from Trade_To_Pms.dbo.Order_QtyShip_detail as a WITH (NOLOCK) inner join #TOrder b on a.id=b.id  ) as s
		on t.ukey=s.ukey
		when matched then
			update set
				t.id		= isnull( s.id,''),
				t.Seq		= isnull( s.Seq ,''),
				t.Article	= isnull( s.Article ,''),
				t.SizeCode	= isnull( s.SizeCode ,''),
				t.Qty		= isnull( s.Qty ,0),
				t.AddName	= isnull( s.AddName ,''),
				t.AddDate	=  s.AddDate ,
				t.EditName	= isnull( s.EditName ,''),
				t.EditDate	=  s.EditDate ,
				t.OriQty	= isnull( s.OriQty,0)
		when not matched by target then 
			insert (
				Id			, Seq			, Article		, SizeCode		, Qty	, AddName 
				, AddDate	, EditName		, EditDate		, Ukey			, OriQty 
			)
           VALUES
           (
                 isnull(s.id ,''),
                 isnull(s.seq ,''),
                 isnull(s.article ,''),
                 isnull(s.sizecode ,''),
                 isnull(s.qty ,0),
                 isnull(s.addname ,''),
                 s.adddate ,
                 isnull(s.editname ,''),
                 s.editdate ,
                 isnull(s.ukey ,0),
                 isnull(s.oriqty,0)
           )
		when not matched by source  AND T.ID IN (SELECT ID FROM #Torder) then 
		delete;

		-----------------Order_UnitPrice------------
		Merge Production.dbo.Order_UnitPrice as t
		Using (select a.* from Trade_To_Pms.dbo.Order_UnitPrice a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when Matched then
			update set			
				t.POPrice	= isnull(s.POPrice ,    0),
				t.QuotCost	= isnull(s.QuotCost ,   0),
				t.DestPrice	= isnull(s.DestPrice ,  0),
				t.AddName	= isnull(s.AddName ,    ''),
				t.AddDate	= s.AddDate ,
				t.EditName	= isnull(s.EditName ,''),
				t.EditDate	= s.EditDate 
		when not Matched by target then
			insert (
				Id				, Article	, SizeCode		, POPrice		, QuotCost 
				, DestPrice		, AddName	, AddDate		, EditName		, EditDate 
			)
           VALUES
           (
                  isnull(s.id ,         ''),
                  isnull(s.article ,    ''),
                  isnull(s.sizecode ,   ''),
                  isnull(s.poprice ,    0),
                  isnull(s.quotcost ,   0),
                  isnull(s.destprice ,  0),
                  isnull(s.addname ,    ''),
                  s.adddate ,
                  isnull(s.editname ,''),
                  s.editdate
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		--------------Order_TmsCost-----TMS & Cost
		Merge  Production.dbo.Order_TmsCost as t
		Using  (select a.* from Trade_To_Pms.dbo.Order_TmsCost a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.ArtworkTypeID=s.ArtworkTypeID
		when matched then 
			update set			
				t.Seq			= isnull( s.Seq,        ''),
				t.Qty			= isnull( s.Qty,        0),
				t.ArtworkUnit	= isnull( s.ArtworkUnit,''),
				t.TMS			= isnull( s.TMS,        0),
				t.Price			= isnull( s.Price,      0),
				t.AddName		= isnull( s.AddName,    ''),
				t.AddDate		=  s.AddDate,
				t.TPEEditName		= isnull( s.EditName,''),
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
		SELECT	  isnull(A.ID           , '')
				, isnull(A.ArtworkTypeID, '')
				, isnull(A.Seq          , '')
				, isnull(A.Qty          , 0)
				, isnull(A.ArtworkUnit  , '')
				, isnull(A.TMS          , 0)
				, isnull(A.Price        , 0)
				, isnull(C.InhouseOSP   , '')
				, isnull(IIF(C.InhouseOSP='O', (
											SELECT top 1 LocalSuppId
											from Style_Artwork_Quot saq with(nolock)
											inner join Style_artwork sa with(nolock) on saq.ukey = sa.ukey
											inner join Trade_To_Pms.DBO.Order_TmsCost ot WITH (NOLOCK) on sa.ArtworkTypeID=ot.ArtworkTypeID
											where sa.styleukey = b.styleukey and ot.id = a.id and ot.ArtworkTypeID = a.ArtworkTypeID
											Order by sa.ukey
										)
									  , (SELECT top 1 LocalSuppID 
										 FROM Production.dbo.Order_TmsCost WITH (NOLOCK) WHERE ID=A.ID)
					 ), '')
				, isnull(A.AddName, '')
				, A.AddDate
				, isnull(A.EditName, '')
				, A.EditDate 
		FROM Trade_To_Pms.dbo.Order_TmsCost A WITH (NOLOCK)
		INNER JOIN #TOrder B ON A.ID=B.ID
		INNER JOIN Production.dbo.ArtworkType C WITH (NOLOCK) ON A.ArtworkTypeID=C.ID
		LEFT JOIN Production.dbo.Order_TmsCost D WITH (NOLOCK) ON D.id = a.Id and D.ArtworkTypeID = A.ArtworkTypeID
		where D.id is null

		--更新Local Order的Order_TmsCost
		select	[ID] = o.ID
				, ot.ArtworkTypeID
				, ot.Seq
				, ot.Qty
				, ot.ArtworkUnit
				, ot.TMS
				, ot.Price
				, C.InhouseOSP
				, [LocalSuppID] = IIF(C.InhouseOSP='O', (
											SELECT top 1 LocalSuppId
											from Style_Artwork_Quot saq with(nolock)
											inner join Style_artwork sa with(nolock) on saq.ukey = sa.ukey
											inner join Trade_To_Pms.DBO.Order_TmsCost otin WITH (NOLOCK) on sa.ArtworkTypeID = otin.ArtworkTypeID
											where sa.styleukey = tmpo.styleukey and otin.id = ot.id and otin.ArtworkTypeID = ot.ArtworkTypeID
											Order by sa.ukey
										)
									  , (SELECT top 1 LocalSuppID 
										 FROM Production.dbo.Order_TmsCost WITH (NOLOCK) WHERE ID = ot.ID)
					 )
				, ot.AddName
				, ot.AddDate
				, ot.EditName
				, ot.EditDate
		into #tmpLocalOrder_TmsCost
		from Production.dbo.Orders o with (nolock)
		inner join Trade_to_PMS.dbo.Orders tmpo on o.CustPONo = tmpo.ID
		inner join Trade_to_PMS.dbo.Order_TmsCost ot with (nolock) on ot.Id = tmpo.ID
		INNER JOIN Production.dbo.ArtworkType C WITH (NOLOCK) ON ot.ArtworkTypeID = C.ID
		where	o.LocalOrder = 1 and
				not exists(select 1 from Production.dbo.sewingoutput_detail sd with (nolock) where sd.OrderID = o.ID)

		delete Production.dbo.Order_TmsCost
		where ID in (select id from #tmpLocalOrder_TmsCost)
	
		insert into Production.dbo.Order_TmsCost(
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
		select
              isnull(ID            , '')
			, isnull(ArtworkTypeID , '')
			, isnull(Seq           , '')
			, isnull(Qty           , 0)
			, isnull(ArtworkUnit   , '')
			, isnull(TMS           , 0)
			, isnull(Price         , 0)
			, isnull(InhouseOSP    , '')
			, isnull(LocalSuppID   , '')
			, isnull(AddName       , '')
			, AddDate
			, isnull(EditName, '')
			, EditDate
		from #tmpLocalOrder_TmsCost
		-----------------Order_SizeCode---------------------------尺寸表 Size Spec(存尺寸碼)
		--20170110 willy 調整順序: 刪除>修改>新增
		Merge Production.dbo.Order_SizeCode as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id where a.sizecode is not null) as s
		on t.id=s.id and t.sizecode=s.sizecode and t.ukey=s.ukey
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete
		when matched then
			update set
				t.Seq		= isnull(s.Seq, ''),
				t.SizeGroup	= isnull(s.SizeGroup, ''),
				t.AddDate	= s.AddDate,
				t.EditDate	= s.EditDate
		When not matched by target then 
			insert (
				Id		, Seq	, SizeGroup		, SizeCode		, ukey		, AddDate		, EditDate
			) 
           VALUES
           (
                  isnull(s.id ,        ''),
                  isnull(s.seq ,       ''),
                  isnull(s.sizegroup , ''),
                  isnull(s.sizecode ,  ''),
                  isnull(s.ukey ,      0),
                  s.adddate ,
                  s.editdate
           );

		----------------Order_Sizeitem------------------------------尺寸表 Size Spec(存量法資料)
		Merge Production.dbo.Order_Sizeitem as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Sizeitem a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 			
				t.SizeUnit	= isnull( s.SizeUnit,   ''),
				t.SizeDesc	= isnull( s.Description,''),
				t.TolMinus	= isnull( s.TolMinus,   ''),
				t.TolPlus	= isnull( s.TolPlus    ,'')
		when not matched by Target then
			insert (
				Id		, SizeItem		, SizeUnit		, SizeDesc		, ukey ,TolMinus ,TolPlus
			) 
           VALUES
           (
                  isnull(s.id ,           ''),
                  isnull(s.sizeitem ,     ''),
                  isnull(s.sizeunit ,     ''),
                  isnull(s.description ,  ''),
                  isnull(s.ukey,          0),
                  isnull(s.tolminus ,     ''),
                  isnull(s.tolplus      , '')
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
	
		----------------Order_SizeTol------------------------------
		Merge Production.dbo.Order_SizeTol as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeTol a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ID=s.ID and t.SizeGroup = s.SizeGroup and t.SizeItem = s.SizeItem
		when matched then 
			update set 			
				t.Lower	= isnull(s.Lower, ''),
				t.Upper	= isnull(s.Upper, '')
		when not matched by Target then
			insert (
				Id		, SizeItem		, SizeGroup		, Lower		, Upper
			)
           VALUES
           (
                  isnull(s.id ,         ''),
                  isnull(s.sizeitem ,   ''),
                  isnull(s.sizegroup ,  ''),
                  isnull(s.lower ,      ''),
                  isnull(s.upper ,      '')
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		-------------Order_SizeSpec--------------------------------尺寸表 Size Spec(存尺寸碼)
		Merge Production.dbo.Order_SizeSpec as t
		Using (select a.* from Trade_To_Pms.dbo.Order_SizeSpec a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on  t.ukey=s.ukey
		when matched then 
			update set
				t.SizeSpec	= isnull(s.SizeSpec,'')
		when not matched by target then
			insert (
				Id		, SizeItem		, SizeCode		, SizeSpec		, ukey
			) 
           VALUES
           (
                  isnull(s.id ,       ''),
                  isnull(s.sizeitem , ''),
                  isnull(s.sizecode , ''),
                  isnull(s.sizespec , ''),
                  isnull(s.ukey,      0)
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
				t.Id				= isnull( s.Id          , '')
				, t.OrderComboID	= isnull( s.OrderComboID, '')
				, t.SizeItem		= isnull( s.SizeItem    , '')
				, t.SizeCode		= isnull( s.SizeCode    , '')
				, t.SizeSpec		= isnull( s.SizeSpec    , '')
		when not matched by target then
			insert (
				Id		, OrderComboID		, SizeItem		, SizeCode		, SizeSpec
				, Ukey
			)
           VALUES
           (
                 isnull(s.id ,          ''),
                 isnull(s.ordercomboid ,''),
                 isnull(s.sizeitem ,    ''),
                 isnull(s.sizecode ,    ''),
                 isnull(s.sizespec ,    ''),
                 isnull(s.ukey        , 0)
           )
		when not matched by source and T.id in (Select ID From #Torder) then 
			delete;

		------------Order_ColorCombo---------------(主料配色表)
		Merge Production.dbo.Order_ColorCombo as t
		Using (select a.* from Trade_To_Pms.dbo.Order_ColorCombo a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.FabricPanelCode=s.FabricPanelCode
		when matched then 
			update set
				t.ColorID		= isnull( s.ColorID,       ''),
				t.FabricCode	= isnull( s.FabricCode,    ''),
				t.PatternPanel	= isnull( s.PatternPanel,  ''),
				t.AddName		= isnull( s.AddName,       ''),
				t.AddDate		=  s.AddDate,   
				t.EditName		= isnull( s.EditName,      ''),
				t.EditDate		=  s.EditDate,  
				t.FabricType 	= isnull( s.FabricType   , '')
		when not matched by target then 
			insert (
				Id					, Article	, ColorID	, FabricCode	, FabricPanelCode
				, PatternPanel		, AddName	, AddDate	, EditName		, EditDate
				, FabricType
			)
           VALUES
           (
                 isnull(s.id ,              ''),
                 isnull(s.article ,         ''),
                 isnull(s.colorid ,         ''),
                 isnull(s.fabriccode ,      ''),
                 isnull(s.fabricpanelcode , ''),
                 isnull(s.patternpanel ,    ''),
                 isnull(s.addname ,         ''),
                 s.adddate , 
                 isnull(s.editname ,        ''),
                 s.editdate , 
                 isnull(s.fabrictype  ,     '')
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
			
	
		-------------Order_FabricCode------------------部位vs布別vsQT
		Merge Production.dbo.Order_FabricCode as t
		Using (select a.* from Trade_To_Pms.dbo.Order_FabricCode a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.FabricPanelCode=s.FabricPanelCode
		when matched then 
			update set
				t.PatternPanel		= isnull( s.PatternPanel,   ''),
				t.FabricCode		= isnull( s.FabricCode,     ''),
				t.FabricPanelCode	= isnull( s.FabricPanelCode,''),
				t.AddName			= isnull( s.AddName,        ''),
				t.AddDate			=  s.AddDate,      
				t.EditName			= isnull( s.EditName,       ''),
				t.EditDate			=  s.EditDate,   
				t.ConsPC			= isnull( s.ConsPC       ,  0)
		when not matched by target then 
			insert (
				Id			, PatternPanel		, FabricCode	, FabricPanelCode	, AddName
				, AddDate	, EditName			, EditDate		, ConsPC
			)
           VALUES
           (
                  isnull(s.id ,            ''),
                  isnull(s.patternpanel ,  ''),
                  isnull(s.fabriccode ,    ''),
                  isnull(s.fabricpanelcode,''),
                  isnull(s.addname ,       ''),
                  s.adddate , 
                  isnull(s.editname ,      ''),
                  s.editdate ,    
                  isnull(s.conspc    ,     0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

-------------Order_FabricCode_QT-----------------
	Merge Production.dbo.Order_FabricCode_QT as t
	Using (select a.* from Trade_To_Pms.dbo.Order_FabricCode_QT a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
	on t.id=s.id and t.FabricPanelCode=s.FabricPanelCode and t.seqno=s.seqno
	when matched then 
	update set
		t.QTFabricPanelCode	= isnull(s.QTFabricPanelCode, ''),
		t.AddName			= isnull(s.AddName, ''),
		t.AddDate			= s.AddDate,
		t.EditName			= isnull(s.EditName, ''),
		t.EditDate			= s.EditDate
	when not matched by target then 
		insert (
			Id			, FabricPanelCode	, SeqNO		, QTFabricPanelCode		, AddName
			, AddDate	, EditName			, EditDate
		)
       VALUES
       (
              isnull(s.id ,                  ''),
              isnull(s.fabricpanelcode ,     ''),
              isnull(s.seqno ,               ''),
              isnull(s.qtfabricpanelcode ,   ''),
              isnull(s.addname ,             ''),
              s.adddate ,
              isnull(s.editname ,            ''),
              s.editdate
       )
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
		delete;

		-------------Order_Bof -----------------------Bill of Fabric

		Merge Production.dbo.Order_Bof as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Bof a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
				t.id					= isnull( s.id,                ''),
				t.FabricCode			= isnull( s.FabricCode,        ''),
				t.Refno					= isnull( s.Refno,             ''),
				t.SCIRefno				= isnull( s.SCIRefno,          ''),
				t.SuppID				= isnull( s.SuppID,            ''),
				t.ConsPC				= isnull( s.ConsPC,            0),
				t.Seq1					= isnull( s.Seq1,              ''),
				t.Kind					= isnull( s.Kind,              ''),
				t.Remark				= isnull( s.Remark,            ''),
				t.LossType				= isnull( s.LossType,          0),
				t.LossPercent			= isnull( s.LossPercent,       0),
				t.RainwearTestPassed	= isnull( s.RainwearTestPassed,0),
				t.HorizontalCutting		= isnull( s.HorizontalCutting, 0),
				t.ColorDetail			= isnull( s.ColorDetail,       ''),
				t.AddName				= isnull( s.AddName,           ''),
				t.AddDate				=  s.AddDate,       
				t.EditName				= isnull( s.EditName,          ''),
				t.EditDate				=  s.EditDate,       
				t.SpecialWidth          = isnull( s.SpecialWidth,      0),
				t.LimitUp				= isnull( s.LimitUp,           0),
				t.LimitDown				= isnull( s.LimitDown,         0)
		when not matched by target then 
			insert (
				Id				, FabricCode	, Refno					, SCIRefno				, SuppID
				, ConsPC		, Seq1			, Kind					, Ukey					, Remark
				, LossType		, LossPercent	, RainwearTestPassed	, HorizontalCutting		, ColorDetail
				, AddName		, AddDate		, EditName				, EditDate              , SpecialWidth 
				, LimitUp		, LimitDown
			)
           VALUES
           (
                  isnull(s.id ,                ''),
                  isnull(s.fabriccode ,        ''),
                  isnull(s.refno ,             ''),
                  isnull(s.scirefno ,          ''),
                  isnull(s.suppid ,            ''),
                  isnull(s.conspc ,            0),
                  isnull(s.seq1 ,              ''),
                  isnull(s.kind ,              ''),
                  isnull(s.ukey ,              0),
                  isnull(s.remark ,            ''),
                  isnull(s.losstype ,          0),
                  isnull(s.losspercent ,       0),
                  isnull(s.rainweartestpassed ,0),
                  isnull(s.horizontalcutting , 0),
                  isnull(s.colordetail ,       ''),
                  isnull(s.addname ,           ''),
                  s.adddate ,   
                  isnull(s.editname ,          ''),
                  s.editdate ,  
                  isnull(s.specialwidth ,      0),
                  isnull(s.limitup ,           0),
                  isnull(s.limitdown,0)
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
				t.Id				= isnull( s.Id,                ''),
				t.Order_BOFUkey		= isnull( s.Order_BOFUkey,     0),
				t.ColorId			= isnull( s.ColorId,           ''),
				t.SuppColor			= isnull( s.SuppColor,         ''),
				t.OrderQty			= isnull( s.OrderQty,          0),
				t.Price				= isnull( s.Price,             0),
				t.UsageQty			= isnull( s.UsageQty,          0),
				t.UsageUnit			= isnull( s.UsageUnit,         ''),
				t.Width				= isnull( s.Width,             0),
				t.SysUsageQty			= isnull( s.SysUsageQty,   0),
				t.QTFabricPanelCode	= isnull( s.QTFabricPanelCode, ''),
				t.Remark			= isnull( s.Remark,            ''),
				t.OrderIdList		= isnull( s.OrderIdList,       ''),
				t.AddName			= isnull( s.AddName,           ''),
				t.AddDate			=  s.AddDate,
				t.EditName			= isnull( s.EditName,          ''),
				t.EditDate	= s.EditDate,
				t.Special           = isnull( s.Special, '')
		when not matched by target then 
			insert ( 
				Id						, Order_BOFUkey		, ColorId		, SuppColor		, OrderQty
				, Price					, UsageQty			, UsageUnit		, Width			, SysUsageQty
				, QTFabricPanelCode		, Remark			, OrderIdList	, AddName		, AddDate
				, EditName				, EditDate			, UKEY          , Special
			) 
           VALUES
           (
                  isnull(s.id ,               ''),
                  isnull(s.order_bofukey ,    0),
                  isnull(s.colorid ,          ''),
                  isnull(s.suppcolor ,        ''),
                  isnull(s.orderqty ,         0),
                  isnull(s.price ,            0),
                  isnull(s.usageqty ,         0),
                  isnull(s.usageunit ,        ''),
                  isnull(s.width ,            0),
                  isnull(s.sysusageqty ,      0),
                  isnull(s.qtfabricpanelcode ,''),
                  isnull(s.remark ,           ''),
                  isnull(s.orderidlist ,      ''),
                  isnull(s.addname ,          ''),
                  s.adddate ,   
                  isnull(s.editname ,         ''),
                  s.editdate ,      
                  isnull(s.ukey ,             0),
                  isnull(s.special ,          '')
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
			
		--Order_BOF_Expend_OrderList
		Merge Production.dbo.Order_BOF_Expend_OrderList as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOF_Expend_OrderList a WITH (NOLOCK) inner join #Torder b on a.id = b.id) as s
		on t.Order_BOF_ExpendUkey = s.Order_BOF_ExpendUkey and t.OrderID = s.OrderID
		when matched then 
			update set
                t.[id]       = isnull(s.[id], '')
               ,t.[AddName]  = isnull(s.[AddName], '')
               ,t.[AddDate]  = s.[AddDate] 
               ,t.[EditName] = isnull(s.[EditName], '')
               ,t.[EditDate] = s.[EditDate]
		when not matched by target then
			insert(
                [id]
               ,[Order_BOF_ExpendUkey]
               ,[OrderID]
               ,[AddName]
               ,[AddDate]
               ,[EditName]
               ,[EditDate]
               )
           values (
                isnull(s.[id], '')
               ,isnull(s.[Order_BOF_ExpendUkey], 0)
               ,isnull(s.[OrderID], '')
               ,isnull(s.[AddName], '')
               ,s.[AddDate]
               ,isnull(s.[EditName], '')
               ,s.[EditDate]
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
		-------------Order_BOA------------------Bill of Accessory
		
		Merge Production.dbo.Order_BOA as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set
				t.id					= isnull( s.id,                  ''),
				t.Refno					= isnull( s.Refno,               ''),
				t.SCIRefno				= isnull( s.SCIRefno,            ''),
				t.SuppID				= isnull( s.SuppID,              ''),
				t.Seq					= isnull( s.Seq1,                ''),
				t.ConsPC				= isnull( s.ConsPC,              0),
				t.BomTypeSize			= isnull( s.BomTypeSize,         0),
				t.BomTypeColor			= isnull( s.BomTypeColor,        0),
				t.FabricPanelCode		= isnull( s.FabricPanelCode,     ''),
				t.PatternPanel			= isnull( s.PatternPanel,        ''),
				t.SizeItem				= isnull( s.SizeItem,            ''),
				t.BomTypeZipper			= isnull( s.BomTypeZipper,       0),
				t.Remark				= isnull( s.Remark,              ''),
				t.ProvidedPatternRoom	= isnull( s.ProvidedPatternRoom, 0),
				t.ColorDetail			= isnull( s.ColorDetail,         ''),
				t.isCustCD				= isnull( s.isCustCD,            0),
				t.lossType				= isnull( s.lossType,            0),
				t.LossPercent			= isnull( s.LossPercent,         0),
				t.LossQty				= isnull( s.LossQty,             0),
				t.LossStep				= isnull( s.LossStep,            0),
				t.AddName				= isnull( s.AddName,             ''),
				t.AddDate				=  s.AddDate,
				t.EditName				= isnull( s.EditName,            ''),
				t.EditDate				=  s.EditDate,
				t.SizeItem_Elastic		= isnull( s.SizeItem_Elastic,    ''),
				t.BomTypePo				= isnull( s.BomTypePo,           0),
				t.Keyword				= isnull( s.Keyword,             ''),
				t.Seq1					= isnull( s.Seq1,                ''),
				t.BomTypeMatching		= isnull( s.BomTypeMatching,     0),
				t.BomTypeCalculatePCS	= isnull( s.BomTypeCalculatePCS, 0),
				t.SizeItem_PCS			= isnull( s.SizeItem_PCS,        ''),
				t.LimitUp				= isnull( s.LimitUp,             0),
				t.LimitDown				= isnull( s.LimitDown,           0),
                t.BomTypeArticle          = isnull(s.BomTypeArticle         , 0),
                t.BomTypeCOO              = isnull(s.BomTypeCOO             , 0),
                t.BomTypeGender           = isnull(s.BomTypeGender          , 0),
                t.BomTypeCustomerSize     = isnull(s.BomTypeCustomerSize    , 0),
                t.CustomerSizeRelation    = isnull(s.CustomerSizeRelation   , ''),
                t.BomTypeDecLabelSize     = isnull(s.BomTypeDecLabelSize    , 0),
                t.DecLabelSizeRelation    = isnull(s.DecLabelSizeRelation   , ''),
                t.BomTypeBrandFactoryCode = isnull(s.BomTypeBrandFactoryCode, 0),
                t.BomTypeStyle            = isnull(s.BomTypeStyle           , 0),
                t.BomTypeStyleLocation    = isnull(s.BomTypeStyleLocation   , 0),
                t.BomTypeSeason           = isnull(s.BomTypeSeason          , 0),
                t.BomTypeCareCode         = isnull(s.BomTypeCareCode        , 0)
                , t.BomTypeBuyMonth = isnull(s.BomTypeBuyMonth,0)
                , t.BomTypeBuyerDlvMonth = isnull(s.BomTypeBuyerDlvMonth,0)
		when not matched by target then
			insert (
				Id					, Ukey				, Refno					, SCIRefno				, SuppID
				, Seq				, ConsPC			, BomTypeSize			, FabricPanelCode		, PatternPanel			
				, SizeItem			, BomTypeZipper		, Remark				, ProvidedPatternRoom	, ColorDetail			
				, isCustCD			, lossType			, LossPercent			, LossQty				, LossStep				
				, AddName			, AddDate			, EditName				, EditDate				, SizeItem_Elastic		
				, BomTypePo			, Keyword			, Seq1					, BomTypeMatching		, BomTypeCalculatePCS
				, SizeItem_PCS		, LimitUp			, LimitDown
                ,BomTypeArticle          
                ,BomTypeCOO              
                ,BomTypeGender           
                ,BomTypeCustomerSize     
                ,CustomerSizeRelation    
                ,BomTypeDecLabelSize     
                ,DecLabelSizeRelation    
                ,BomTypeBrandFactoryCode 
                ,BomTypeStyle            
                ,BomTypeStyleLocation    
                ,BomTypeSeason           
                ,BomTypeCareCode     
               ,BomTypeBuyMonth
               ,BomTypeBuyerDlvMonth    
			)
           VALUES
           (
                  isnull(s.id ,                 ''),
                  isnull(s.ukey ,               0),
                  isnull(s.refno ,              ''),
                  isnull(s.scirefno ,           ''),
                  isnull(s.suppid ,             ''),
                  isnull(s.seq1 ,               ''),
                  isnull(s.conspc ,             0),
                  isnull(s.bomtypesize ,        0),
                  isnull(s.fabricpanelcode ,    ''),
                  isnull(s.patternpanel ,       ''),
                  isnull(s.sizeitem ,           ''),
                  isnull(s.bomtypezipper ,      0),
                  isnull(s.remark ,             ''),
                  isnull(s.providedpatternroom ,0),
                  isnull(s.colordetail ,        ''),
                  isnull(s.iscustcd ,           0),
                  isnull(s.losstype ,           0),
                  isnull(s.losspercent ,        0),
                  isnull(s.lossqty ,            0),
                  isnull(s.lossstep ,           0),
                  isnull(s.addname ,            ''),
                  s.adddate ,
                  isnull(s.editname ,           ''),
                  s.editdate ,
                  isnull(s.sizeitem_elastic ,   ''),
                  isnull(s.bomtypepo ,          0),
                  isnull(s.keyword ,            ''),
                  isnull(s.seq1 ,               ''),
                  isnull(s.bomtypematching ,    0),
                  isnull(s.bomtypecalculatepcs ,0),
                  isnull(s.sizeitem_pcs ,       ''),
                  isnull(s.limitup ,            0),
                  isnull(s.limitdown,           0)
                , isnull(s.BomTypeArticle         , 0)
                , isnull(s.BomTypeCOO             , 0)
                , isnull(s.BomTypeGender          , 0) 
                , isnull(s.BomTypeCustomerSize    , 0) 
                , isnull(s.CustomerSizeRelation   , '')
                , isnull(s.BomTypeDecLabelSize    , 0)
                , isnull(s.DecLabelSizeRelation   , '')
                , isnull(s.BomTypeBrandFactoryCode, 0)
                , isnull(s.BomTypeStyle           , 0) 
                , isnull(s.BomTypeStyleLocation   , 0) 
                , isnull(s.BomTypeSeason          , 0) 
                , isnull(s.BomTypeCareCode        , 0)
                , isnull(s.BomTypeBuyMonth,0)
                , isnull(s.BomTypeBuyerDlvMonth,0)
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;


        ---Order_BOA_Location
		Merge Production.dbo.Order_BOA_Location as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_Location a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.Order_BOAUkey = s.Order_BOAUkey and t.Location = s.Location
		when matched then 
			update set
                t.[ID]       = isnull(s.[ID]      , '')
               ,t.[AddName]  = isnull(s.[AddName] , '')
               ,t.[AddDate]  = s.[AddDate]
               ,t.[EditName] = isnull(s.[EditName], '')
               ,t.[EditDate] = s.[EditDate]

		when not matched by target then
			insert (
                [ID]
               ,[Order_BOAUkey]
               ,[Location]
               ,[AddName]
               ,[AddDate]
               ,[EditName]
               ,[EditDate]    
			) values (
                isnull([ID]           , '')
               ,isnull([Order_BOAUkey], 0)
               ,isnull([Location]     , '')
               ,isnull([AddName]      , '')
               ,[AddDate]
               ,isnull([EditName]     , '')
               ,[EditDate] 
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
				t.Id				= isnull( s.Id            , '')
				, t.Order_BoAUkey	= isnull( s.Order_BoAUkey , 0)
				, t.Article			= isnull( s.Article       , '')
				, t.AddName			= isnull( s.AddName       , '')
				, t.AddDate			=  s.AddDate
				, t.EditName		= isnull( s.EditName , '')
				, t.EditDate		= s.EditDate
		when not matched then 
			insert (
				Id				, Order_BoaUkey		, Article	, AddName	, AddDate
				, EditName		, EditDate			, Ukey
			) 
       VALUES
       (
              isnull(s.id ,           ''),
              isnull(s.order_boaukey ,0),
              isnull(s.article ,      ''),
              isnull(s.addname ,      ''),
              s.adddate ,
              isnull(s.editname ,     ''),
              s.editdate ,
              isnull(s.ukey,          0)
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
				t.id				= isnull( s.id,             ''),
				t.Order_BOAUkey		= isnull( s.Order_BOAUkey,  0),
				t.OrderQty			= isnull( s.OrderQty,       0),
				t.Refno				= isnull( s.Refno,          ''),
				t.SCIRefno			= isnull( s.SCIRefno,       ''),
				t.Price				= isnull( s.Price,          0),
				t.UsageQty			= isnull( s.UsageQty,       0),
				t.UsageUnit			= isnull( s.UsageUnit,      ''),
				t.Article			= isnull( s.Article,        ''),
				t.SuppColor			= isnull( s.SuppColor,      ''),
				t.SizeCode			= isnull( s.SizeCode,       ''),
				t.OrderIdList		= isnull( s.OrderIdList,    ''),
				t.SysUsageQty		= isnull( s.SysUsageQty,    0),
				t.Remark			= isnull( s.Remark,			''),
				t.AddName			= isnull( s.AddName,        ''),
				t.AddDate			= s.AddDate,  
				t.EditName			= isnull( s.EditName,       ''),
				t.EditDate			= s.EditDate,  
				t.Keyword			= isnull( s.Keyword,        ''),
				t.Special			= isnull( s.Special,        ''),
				t.Keyword_Original	= isnull(s.Keyword_Original, '')
		when not matched by target then
			insert (
				Id				, UKEY			, Order_BOAUkey		, OrderQty			, Refno
				, SCIRefno		, Price			, UsageQty			, UsageUnit			, Article
				, SuppColor		, SizeCode
				, OrderIdList	, SysUsageQty	, Remark
				, AddName		, AddDate		, EditName			, EditDate			, Keyword
				, Special
				, Keyword_Original
			) values (
                  isnull(s.id ,             ''),
                  isnull(s.ukey ,           0),
                  isnull(s.order_boaukey ,  0),
                  isnull(s.orderqty ,       0),
                  isnull(s.refno ,          ''),
                  isnull(s.scirefno ,       ''),
                  isnull(s.price ,          0),
                  isnull(s.usageqty ,       0),
                  isnull(s.usageunit ,      ''),
                  isnull(s.article ,        ''),
                  isnull(s.suppcolor ,      ''),
                  isnull(s.sizecode ,       ''),
                  isnull(s.orderidlist ,    ''),
                  isnull(s.sysusageqty ,    0),
                  isnull(s.remark ,         ''),
                  isnull(s.addname ,        ''),
                  s.adddate , 
                  isnull(s.editname ,       ''),
                  s.editdate , 
                  isnull(s.keyword ,        ''),
                  isnull(s.special,          '')
				, isnull(s.Keyword_Original, '')
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
	    -----Order_BOA_Expend_Spec
		Merge Production.dbo.Order_BOA_Expend_Spec as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_Expend_Spec a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Order_BOA_ExpendUkey=s.Order_BOA_ExpendUkey and t.SpecColumnID=s.SpecColumnID
		when matched then 
			update set
                t.[Id]        = isnull(s.[Id]       , '')
               ,t.[SpecValue] = isnull(s.[SpecValue], '')
		when not matched by target then
			insert(
                [Id]
               ,[Order_BOA_ExpendUkey]
               ,[SpecColumnID]
               ,[SpecValue]
               )
           values (
                isnull(s.[Id]                  , '')
               ,isnull(s.[Order_BOA_ExpendUkey], 0)
               ,isnull(s.[SpecColumnID]        , '')
               ,isnull(s.[SpecValue]           , '')
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		--Order_BOA_Expend_Keyword
		Merge Production.dbo.Order_BOA_Expend_Keyword as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_Expend_Keyword a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Order_BOA_ExpendUkey=s.Order_BOA_ExpendUkey and t.KeywordField=s.KeywordField
		when matched then 
			update set
                t.[Id]        = isnull(s.[Id]       , '')
               ,t.KeywordValue = isnull(s.KeywordValue, '')
		when not matched by target then
			insert(
                [Id]
               ,[Order_BOA_ExpendUkey]
               ,[KeywordField]
               ,[KeywordValue]
               )
           values (
                isnull([Id]                  , '')
               ,isnull([Order_BOA_ExpendUkey], 0)
               ,isnull([KeywordField]        , '')
               ,isnull([KeywordValue]        , '')
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;
			
		--Order_BOA_Expend_OrderList
		Merge Production.dbo.Order_BOA_Expend_OrderList as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_Expend_OrderList a WITH (NOLOCK) inner join #Torder b on a.id = b.id) as s
		on t.Order_BOA_ExpendUkey = s.Order_BOA_ExpendUkey and t.OrderID = s.OrderID
		when matched then 
			update set
                t.[id]       = isnull(s.[id], '')
               ,t.[AddName]  = isnull(s.[AddName], '')
               ,t.[AddDate]  = s.[AddDate] 
               ,t.[EditName] = isnull(s.[EditName], '')
               ,t.[EditDate] = s.[EditDate]
		when not matched by target then
			insert(
                [id]
               ,[Order_BOA_ExpendUkey]
               ,[OrderID]
               ,[AddName]
               ,[AddDate]
               ,[EditName]
               ,[EditDate]
               )
           values (
                isnull(s.[id], '')
               ,isnull(s.[Order_BOA_ExpendUkey], 0)
               ,isnull(s.[OrderID], '')
               ,isnull(s.[AddName], '')
               ,s.[AddDate]
               ,isnull(s.[EditName], '')
               ,s.[EditDate]
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		---------------Order_BOA_Matching
		Merge Production.dbo.Order_BOA_Matching as t
		Using (
			select a.* 
			from Trade_To_Pms.dbo.Order_BOA_Matching a With (NoLock)
			inner join #Torder b on a.id = b.id
		) as s on t.[Order_BOAUkey] = s.[Order_BOAUkey] and t.Seq = s.Seq
		when matched then
			update set
				 t.[ID]				= isnull(s.[ID]             ,'')
				,t.[Order_BOAUkey]	= isnull(s.[Order_BOAUkey]  ,0)
				,t.[Seq]			= isnull(s.[Seq]            ,'')
				,t.[AddName]		= isnull(s.[AddName]        ,'')
				,t.[AddDate]		= s.[AddDate]    
				,t.[EditName]		= isnull(s.[EditName]       ,'')
				,t.[EditDate]		=s.[EditDate]
		when not matched then 
			insert  ([ID],[Order_BOAUkey],[Seq],[AddName],[AddDate],[EditName],[EditDate])
           VALUES
           (
                  isnull(s.[ID],            ''),
                  isnull(s.[Order_BOAUkey], 0),
                  isnull(s.[Seq],           ''),
                  isnull(s.[AddName],       ''),
                  s.[AddDate],
                  isnull(s.[EditName],''),
                  s.[EditDate]
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;		
			
		---------------Order_BOA_Matching_Detail
		Merge Production.dbo.Order_BOA_Matching_Detail as t
		Using (
			select a.* 
			from Trade_To_Pms.dbo.Order_BOA_Matching_Detail a With (NoLock)
			inner join #Torder b on a.id = b.id
		) as s on t.[Order_BOAUkey] = s.[Order_BOAUkey] and t.Seq = s.Seq  and t.SizeCode = s.SizeCode 
		when matched then
			update set
				 t.[ID]			   = isnull( s.[ID]             ,'')
				,t.[Order_BOAUkey] = isnull( s.[Order_BOAUkey]  ,0)
				,t.[Seq]		   = isnull( s.[Seq]            ,'')
				,t.[SizeCode]	   = isnull( s.[SizeCode]       ,'')
				,t.[MatchingRatio] = isnull( s.[MatchingRatio]  ,0)
				,t.[AddName]	   = isnull( s.[AddName]        ,'')
				,t.[AddDate]	   =  s.[AddDate]
				,t.[EditName]	   = isnull( s.[EditName]       ,'')
				,t.[EditDate]	   = s.[EditDate]
		when not matched then 
			insert  ([ID],[Order_BOAUkey],[Seq],[SizeCode],[MatchingRatio],[AddName],[AddDate],[EditName],[EditDate])
           VALUES
           (
                  isnull(s.[ID],            ''),
                  isnull(s.[Order_BOAUkey], 0),
                  isnull(s.[Seq],           ''),
                  isnull(s.[SizeCode],      ''),
                  isnull(s.[MatchingRatio], 0),
                  isnull(s.[AddName],       ''),
                  s.[AddDate],
                  isnull(s.[EditName],''),
                  s.[EditDate]
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;	



		---------------Order_MarkerList------------Marker List

		Merge Production.dbo.Order_MarkerList as t
		Using (select a.* from Trade_To_Pms.dbo.Order_MarkerList a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
				t.id					= isnull(s.id,                   ''),
				t.Seq					= isnull( s.Seq,                 ''),
				t.MarkerName			= isnull( s.MarkerName,          ''),
				t.FabricCode			= isnull( s.FabricCode,          ''),
				t.FabricCombo			= isnull( s.FabricCombo,         ''),
				t.FabricPanelCode		= isnull( s.FabricPanelCode,     ''),
				t.isQT					= isnull( s.isQT,                0),
				t.MarkerLength			= isnull( s.MarkerLength,        ''),
				t.ConsPC				= isnull( s.ConsPC,              0),
				t.Cuttingpiece			= isnull( s.Cuttingpiece,        0),
				t.ActCuttingPerimeter	= isnull( s.ActCuttingPerimeter, ''),
				t.StraightLength		= isnull( s.StraightLength,      ''),
				t.CurvedLength			= isnull( s.CurvedLength,        ''),
				t.Efficiency			= isnull( s.Efficiency,          ''),
				t.Remark				= isnull( s.Remark,              ''),
				t.MixedSizeMarker		= isnull( s.MixedSizeMarker,     ''),
				t.MarkerNo				= isnull( s.MarkerNo,            ''),
				t.MarkerUpdate			=  s.MarkerUpdate,  
				t.MarkerUpdateName		= isnull( s.MarkerUpdateName,    ''),
				t.AllSize				= isnull( s.AllSize,             0),
				t.PhaseID				= isnull( s.PhaseID,             ''),
				t.SMNoticeID			= isnull( s.SMNoticeID,          ''),
				t.MarkerVersion			= isnull( s.MarkerVersion,       ''),
				t.Direction				= isnull( s.Direction,           ''),
				t.CuttingWidth			= isnull( s.CuttingWidth,        ''),
				t.Width					= isnull( s.Width,               ''),
				t.Type					= isnull( s.Type,                ''),
				t.AddName				= isnull( s.AddName,             ''),
				t.AddDate				=  s.AddDate,      
				t.EditName				= isnull( s.EditName,            ''),
				t.EditDate				= s.EditDate,
				t.MarkerType     		= isnull( s.MarkerType,           0)
		when not matched by target then
			insert (
				Id					, Ukey					, Seq				, MarkerName		, FabricCode
				, FabricCombo		, FabricPanelCode		, isQT				, MarkerLength		, ConsPC
				, Cuttingpiece		, ActCuttingPerimeter	, StraightLength	, CurvedLength		, Efficiency
				, Remark			, MixedSizeMarker		, MarkerNo			, MarkerUpdate		, MarkerUpdateName
				, AllSize			, PhaseID				, SMNoticeID		, MarkerVersion		, Direction
				, CuttingWidth		, Width					, Type				, AddName			, AddDate
				, EditName			, EditDate              , MarkerType
			)
           VALUES
           (
                  isnull(s.id ,                  ''),
                  isnull(s.ukey ,                0),
                  isnull(s.seq ,                 0),
                  isnull(s.markername ,          ''),
                  isnull(s.fabriccode ,          ''),
                  isnull(s.fabriccombo ,         ''),
                  isnull(s.fabricpanelcode ,     ''),
                  isnull(s.isqt ,                0),
                  isnull(s.markerlength ,        ''),
                  isnull(s.conspc ,              0),
                  isnull(s.cuttingpiece ,        0),
                  isnull(s.actcuttingperimeter , ''),
                  isnull(s.straightlength ,      ''),
                  isnull(s.curvedlength ,        ''),
                  isnull(s.efficiency ,          ''),
                  isnull(s.remark ,              ''),
                  isnull(s.mixedsizemarker ,     ''),
                  isnull(s.markerno ,            ''),
                  s.markerupdate , 
                  isnull(s.markerupdatename ,    ''),
                  isnull(s.allsize ,             0),
                  isnull(s.phaseid ,             ''),
                  isnull(s.smnoticeid ,          ''),
                  isnull(s.markerversion ,       ''),
                  isnull(s.direction ,           ''),
                  isnull(s.cuttingwidth ,        ''),
                  isnull(s.width ,               ''),
                  isnull(s.type ,                ''),
                  isnull(s.addname ,             ''),
                  s.adddate ,
                  isnull(s.editname ,            ''),
                  s.editdate,
				  isnull( s.MarkerType,           0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		---------------Order_MarkerList------------Order_MarkerList_PatternPanel
		Delete a
		from Production.dbo.Order_MarkerList_PatternPanel as a
		inner join #Torder b on a.id = b.id
		where not exists(select 1 from Trade_To_Pms.dbo.Order_MarkerList_PatternPanel 
								  where a.Order_MarkerlistUkey = Order_MarkerlistUkey and a.FabricPanelCode = FabricPanelCode)
		---------------------------UPDATE 
		
		UPDATE a
		SET	 a.Id			  = b.Id			
			,a.PatternPanel	  = b.PatternPanel	
			,a.AddName		  = b.AddName		
			,a.AddDate		  = b.AddDate		
			,a.EditName		  = b.EditName		
			,a.EditDate		  = b.EditDate		
		from Production.dbo.Order_MarkerList_PatternPanel as a
		inner join #Torder o on a.id = o.id
		inner join Trade_To_Pms.dbo.Order_MarkerList_PatternPanel as b ON a.Order_MarkerlistUkey = b.Order_MarkerlistUkey and a.FabricPanelCode = b.FabricPanelCode
		-------------------------- INSERT
		
		INSERT INTO Production.dbo.Order_MarkerList_PatternPanel(
		 Id
		,PatternPanel
		,Order_MarkerlistUkey
		,FabricPanelCode
		,AddName
		,AddDate
		,EditName
		,EditDate
		)
		select 
				 b.Id
				,b.PatternPanel
				,b.Order_MarkerlistUkey
				,b.FabricPanelCode
				,b.AddName
				,b.AddDate
				,b.EditName
				,b.EditDate
		from Trade_To_Pms.dbo.Order_MarkerList_PatternPanel as b WITH (NOLOCK)
		inner join #Torder o on b.id = o.id
		where not exists(select 1 from Production.dbo.Order_MarkerList_PatternPanel as a WITH (NOLOCK) 
						 where a.Order_MarkerlistUkey = b.Order_MarkerlistUkey and a.FabricPanelCode = b.FabricPanelCode)

		------Order_MarkerList_SizeQty----------------
		Merge Production.dbo.Order_MarkerList_SizeQty as t
		Using (select a.* from Trade_To_Pms.dbo.Order_MarkerList_SizeQty a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.order_MarkerListUkey=s.order_MarkerListUkey and t.sizecode=s.sizecode
		when matched then 
			update set
				t.id		= isnull( s.id,      ''),
				t.SizeCode	= isnull( s.SizeCode,''),
				t.Qty		= isnull( s.Qty,     0)
		when not matched by target then
			insert (
				Order_MarkerListUkey	, Id	, SizeCode		, Qty
			)
           VALUES
           (
                  isnull(s.order_markerlistukey ,0),
                  isnull(s.id ,                  ''),
                  isnull(s.sizecode ,            ''),
                  isnull(s.qty,                  0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;

		--------Order_ArtWork-----------------
		Merge Production.dbo.Order_ArtWork as t
		Using (select a.* from Trade_To_Pms.dbo.Order_ArtWork a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set
				t.id			= isnull(s.id,            ''),
				t.ArtworkTypeID	= isnull( s.ArtworkTypeID,''),
				t.Article		= isnull( s.Article,      ''),
				t.PatternCode	= isnull( s.PatternCode,  ''),
				t.PatternDesc	= isnull( s.PatternDesc,  ''),
				t.ArtworkID		= isnull( s.ArtworkID,    ''),
				t.ArtworkName	= isnull( s.ArtworkName,  ''),
				t.Qty			= isnull( s.Qty,          0),
				t.TMS			= isnull( s.TMS,          0),
				t.Price			= isnull( s.Price,        0),
				t.Cost			= isnull( s.Cost,         0),
				t.Remark		= isnull( s.Remark,       ''),
				t.PPU			= isnull( s.PPU,          0),
				t.AddName		= isnull( s.AddName,      ''),
				t.AddDate		=  s.AddDate,
				t.EditName		= isnull( s.EditName,     ''),
				t.EditDate		=  s.EditDate,
				t.InkType		= isnull( s.InkType,      ''),
				t.Length 		= isnull( s.Length,       0),
				t.Width 		= isnull( s.Width,        0),
				t.Colors		= isnull( s.Colors,       '')
		when not matched by target then 
			insert (
				ID				, ArtworkTypeID		, Article	, PatternCode	, PatternDesc
				, ArtworkID		, ArtworkName		, Qty		, TMS			, Price
				, Cost			, Remark			, Ukey		, AddName		, AddDate
				, PPU			, EditName			, EditDate	, InkType		, Length
				, Width			, Colors
			)
           VALUES
           (
                  isnull(s.id ,            ''),
                  isnull(s.artworktypeid , ''),
                  isnull(s.article ,       ''),
                  isnull(s.patterncode ,   ''),
                  isnull(s.patterndesc ,   ''),
                  isnull(s.artworkid ,     ''),
                  isnull(s.artworkname ,   ''),
                  isnull(s.qty ,           0),
                  isnull(s.tms ,           0),
                  isnull(s.price ,         0),
                  isnull(s.cost ,          0),
                  isnull(s.remark ,        ''),
                  isnull(s.ukey ,          0),
                  isnull(s.addname ,       ''),
                  s.adddate ,
                  isnull(s.ppu ,           0),
                  isnull(s.editname ,      ''),
                  s.editdate,
                  isnull(s.inktype ,       ''),
                  isnull(s.length ,        0),
                  isnull(s.width ,         0),
                  isnull(s.colors,         '')
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
			delete;


		---------Order_EachCons--------------------Each Cons
		Merge Production.dbo.Order_EachCons as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey	
		when matched then 
			update set 
				t.Id					= isnull( s.Id,         ''),
				t.Seq					= isnull( s.Seq,        0),
				t.MarkerName			= isnull( s.MarkerName, ''),
				t.FabricCombo			= isnull( s.FabricCombo,''),
				t.MarkerLength			= replace(s.MarkerLength,'Ｙ','Y'),
				t.FabricPanelCode		= isnull( s.FabricPanelCode,    ''),
				t.ConsPC				= isnull( s.ConsPC,             0),
				t.CuttingPiece			= isnull( s.CuttingPiece,       0),
				t.ActCuttingPerimeter	= isnull( s.ActCuttingPerimeter,''),
				t.StraightLength		= isnull( s.StraightLength,     ''),
				t.FabricCode			= isnull( s.FabricCode,         ''),
				t.CurvedLength			= isnull( s.CurvedLength,       ''),
				t.Efficiency			= isnull( s.Efficiency,         ''),
				t.Article				= isnull( s.Article,            ''),
				t.Remark				= isnull( s.Remark,             ''),
				t.MixedSizeMarker		= isnull( s.MixedSizeMarker,    ''),
				t.MarkerNo				= isnull( s.MarkerNo,           ''),
				t.MarkerUpdate			=  s.MarkerUpdate, 
				t.MarkerUpdateName		= isnull( s.MarkerUpdateName,   ''),
				t.AllSize				= isnull( s.AllSize,            0),
				t.PhaseID				= isnull( s.PhaseID,            ''),
				t.SMNoticeID			= isnull( s.SMNoticeID,         ''),
				t.MarkerVersion			= isnull( s.MarkerVersion,      ''),
				t.Direction				= isnull( s.Direction,          ''),
				t.CuttingWidth			= isnull( s.CuttingWidth,       ''),
				t.Width					= isnull( s.Width,              ''),
				t.TYPE					= isnull( s.TYPE,               ''),
				t.AddName				= isnull( s.AddName,            ''),
				t.AddDate				=  s.AddDate, 
				t.EditName				= isnull( s.EditName,           ''),
				t.EditDate				=  s.EditDate, 
				t.isQT					= isnull( s.isQT,               0),
				t.MarkerDownloadID		= isnull( s.MarkerDownloadID,   ''),
				t.MarkerType		    = isnull( s.MarkerType,          0)
		when not matched by target then 
			insert (
				Id					, Ukey				, Seq				, MarkerName			, FabricCombo
				, MarkerLength		, FabricPanelCode	, ConsPC			, CuttingPiece			, ActCuttingPerimeter
				, StraightLength	, FabricCode		, CurvedLength		, Efficiency			, Article
				, Remark			, MixedSizeMarker	, MarkerNo			, MarkerUpdate			, MarkerUpdateName
				, AllSize			, PhaseID			, SMNoticeID		, MarkerVersion			, Direction
				, CuttingWidth		, Width				, TYPE				, AddName				, AddDate
				, EditName			, EditDate			, isQT				, MarkerDownloadID		, MarkerType
			) 
           VALUES
           (
                  isnull(s.id ,                 ''),
                  isnull(s.ukey ,               0),
                  isnull(s.seq ,                0),
                  isnull(s.markername ,         ''),
                  isnull(s.fabriccombo ,        ''),
                  isnull(s.markerlength ,       ''),
                  isnull(s.fabricpanelcode ,    ''),
                  isnull(s.conspc ,             0),
                  isnull(s.cuttingpiece ,       0),
                  isnull(s.actcuttingperimeter ,''),
                  isnull(s.straightlength ,     ''),
                  isnull(s.fabriccode ,         ''),
                  isnull(s.curvedlength ,       ''),
                  isnull(s.efficiency ,         ''),
                  isnull(s.article ,            ''),
                  isnull(s.remark ,             ''),
                  isnull(s.mixedsizemarker ,    ''),
                  isnull(s.markerno ,           ''),
                  s.markerupdate , 
                  isnull(s.markerupdatename ,   ''),
                  isnull(s.allsize ,            0),
                  isnull(s.phaseid ,            ''),
                  isnull(s.smnoticeid ,         ''),
                  isnull(s.markerversion ,      ''),
                  isnull(s.direction ,          ''),
                  isnull(s.cuttingwidth ,       ''),
                  isnull(s.width ,              ''),
                  isnull(s.type ,               ''),
                  isnull(s.addname ,            ''),
                  s.adddate ,
                  isnull(s.editname ,           ''),
                  s.editdate ,
                  isnull(s.isqt ,               0),
                  isnull(s.markerdownloadid ,   ''),
				  isnull(s.MarkerType,          0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;


		--------Order_EachCons_SizeQty----------------Each cons - Size & Qty
		Merge Production.dbo.Order_EachCons_SizeQty as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_SizeQty a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Order_EachConsUkey=s.Order_EachConsUkey and t.sizecode=s.sizecode	
		when matched then 
			update set 
				t.Id		= isnull( s.Id,       ''),
				t.SizeCode	= isnull( s.SizeCode, ''),
				t.Qty		= isnull( s.Qty,      0)
		when not matched by target then 
			insert (
				Order_EachConsUkey		, Id	, SizeCode		, Qty
			)
           VALUES
           (
                  isnull(s.order_eachconsukey ,0),
                  isnull(s.id ,                ''),
                  isnull(s.sizecode ,          ''),
                  isnull(s.qty,                0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
	-------Order_EachCons_Article--------------------Each cons - 用量展開
	  Merge Production.dbo.Order_EachCons_Article as t
	  Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Article a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
	 on t.Order_EachConsUkey=s.Order_EachConsUkey and t.Article = s.Article
	 when matched then 
	 update set 
	  t.Id     = isnull( s.Id,          ''),
	  t.AddName    = isnull( s.AddName, ''),
	  t.AddDate    =  s.AddDate,
	 t.EditName    = isnull( s.EditName,''),
	 t.EditDate    = s.EditDate
	 when not matched by target then 
	  insert (
	   Id   , Order_EachConsUkey , Article   , AddName  , AddDate
	  , EditName  , EditDate
	  )
       VALUES
       (
              isnull(s.id ,                 ''),
              isnull(s.order_eachconsukey , 0),
              isnull(s.article ,            ''),
              isnull(s.addname ,            ''),
              s.adddate ,
              isnull(s.editname ,           ''),
              s.editdate
       )
	  when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
		delete;
		-------Order_EachCons_Color--------------------Each cons - 用量展開
		Merge Production.dbo.Order_EachCons_Color as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Color a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Ukey=s.Ukey	
		when matched then 
			update set 
				t.Id					= isnull( s.Id,                  ''),
				t.Order_EachConsUkey	= isnull( s.Order_EachConsUkey,  0),
				t.Ukey					= isnull( s.Ukey,                0),
				t.ColorID				= isnull( s.ColorID,             ''),
				t.CutQty				= isnull( s.CutQty,              0),
				t.Layer					= isnull( s.Layer,               0),
				t.Orderqty				= isnull( s.Orderqty,            0),
				t.SizeList				= isnull( s.SizeList,            ''),
				t.Variance				= isnull( s.Variance,            0),
				t.YDS					= isnull( s.YDS,                 0)
		when not matched by target then 
			insert (
				Id			, Order_EachConsUkey	, Ukey			, ColorID		, CutQty
				, Layer		, Orderqty				, SizeList		, Variance		, YDS
			) 
           VALUES
           (
                  isnull(s.id ,                ''),
                  isnull(s.order_eachconsukey ,0),
                  isnull(s.ukey ,              0),
                  isnull(s.colorid ,           ''),
                  isnull(s.cutqty ,            0),
                  isnull(s.layer ,             0),
                  isnull(s.orderqty ,          0),
                  isnull(s.sizelist ,          ''),
                  isnull(s.variance ,          0),
                  isnull(s.yds,                0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
		
		---------Order_EachCons_Color_Article-------Each cons - 用量展開明細
		Merge Production.dbo.Order_EachCons_Color_Article as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_Color_Article a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.Ukey=s.Ukey	
		when matched then 
			update set 
				t.Id						= isnull( s.Id,                       ''),
				t.Order_EachCons_ColorUkey	= isnull( s.Order_EachCons_ColorUkey, 0),
				t.Article					= isnull( s.Article,                  ''),
				t.ColorID					= isnull( s.ColorID,                  ''),
				t.SizeCode					= isnull( s.SizeCode,                 ''),
				t.Orderqty					= isnull( s.Orderqty,                 0),
				t.Layer						= isnull( s.Layer,                    0),
				t.CutQty					= isnull( s.CutQty,                   0),
				t.Variance					= isnull( s.Variance,                 0)
		when not matched by target then 
			insert (
				Id				, Order_EachCons_ColorUkey		, Article		, ColorID		, SizeCode
				, Orderqty		, Layer							, CutQty		, Variance		, Ukey
			)
           VALUES
           (
                  isnull(s.id ,                        ''),
                  isnull(s.order_eachcons_colorukey ,  0),
                  isnull(s.article ,                   ''),
                  isnull(s.colorid ,                   ''),
                  isnull(s.sizecode ,                  ''),
                  isnull(s.orderqty ,                  0),
                  isnull(s.layer ,                     0),
                  isnull(s.cutqty ,                    0),
                  isnull(s.variance ,                  0),
                  isnull(s.ukey, 0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;

		----------Order_EachCons_PatternPanel---------------PatternPanel
		Merge Production.dbo.Order_EachCons_PatternPanel as t
		Using (select a.* from Trade_To_Pms.dbo.Order_EachCons_PatternPanel a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
		on t.PatternPanel=s.PatternPanel and t.Order_EachConsUkey=s.Order_EachConsUkey and t.FabricPanelCode=s.FabricPanelCode
		When matched then 
			update set 
				t.Id					= isnull(s.Id,''),
				t.AddName				= isnull(s.AddName,''),
				t.AddDate				= s.AddDate,
				t.EditName				= isnull(s.EditName,''),
				t.EditDate				= s.EditDate
		when not matched by target then 
			insert (
				Id			, PatternPanel		, Order_EachConsUkey	, FabricPanelCode	, AddName
				, AddDate	, EditName			, EditDate
			)
           VALUES
           (
                  isnull(s.id ,                 ''),
                  isnull(s.patternpanel ,       ''),
                  isnull(s.order_eachconsukey , 0),
                  isnull(s.fabricpanelcode ,    ''),
                  isnull(s.addname ,            ''),
                  s.adddate ,
                  isnull(s.editname ,''),
                  s.editdate
           )
		when not matched by source and t.id in (select id from #TOrder) then 
			delete;

		
		------------Order_Article----------------------Art
		Merge Production.dbo.Order_Article as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Article a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article
		when matched then 
			update set 
				t.Seq			= isnull(s.Seq, 0),
				t.TissuePaper	= isnull(s.TissuePaper, 0),
				t.CertificateNumber	= isnull(s.CertificateNumber, ''),
				t.SecurityCode	= isnull(s.SecurityCode, '')
		when not matched by target then
			insert (
				id		, Seq	, Article	, TissuePaper, CertificateNumber, SecurityCode
			) values (
				isnull(s.id, ''),
                isnull(s.Seq, 0),
                isnull(s.Article, ''),
                isnull(s.TissuePaper, 0),
                isnull(s.CertificateNumber, ''),
                isnull(s.SecurityCode, '')
			)
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;
		------------Order_Article_PadPrint----------------------Art
		Merge Production.dbo.Order_Article_PadPrint as t
		Using (select a.* from Trade_To_Pms.dbo.Order_Article_PadPrint a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.colorid = s.colorid
		when matched then 
			update set 
				t.qty			= isnull(s.qty, 0)
		when not matched by target then
			insert (
				id	, Article	, colorid,  qty
			) 
           VALUES
           (
                  isnull(s.id ,     ''),
                  isnull(s.article ,''),
                  isnull(s.colorid, ''),
                  isnull(s.qty,     0)
           )
		when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then  
			delete;


		-----------Order_BOA_KeyWord---------------------Bill of Other - Key word

		Merge Production.dbo.Order_BOA_KeyWord as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_KeyWord a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.ukey=s.ukey
		when matched then 
			update set 
				t.id			= isnull( s.id,             ''),
				t.Order_BOAUkey	= isnull( s.Order_BOAUkey,  0),
				t.KeyWordID		= isnull( s.KeyWordID,      ''),
				t.Relation		= isnull( s.Relation,       '')
		when not matched by target then
			insert (
				ID		, Ukey		, Order_BOAUkey		, KeyWordID		, Relation
			) 
           VALUES
           (
                  isnull(s.id ,            ''),
                  isnull(s.ukey ,          0),
                  isnull(s.order_boaukey , 0),
                  isnull(s.keywordid ,     ''),
                  isnull(s.relation,       '')
           )
		when not matched by source and t.id in (select id from #TOrder) then
			delete;
	

		------------Order_BOA_CustCD----------Bill of Other - 用量展開
		Merge Production.dbo.Order_BOA_CustCD as t
		Using (select a.* from Trade_To_Pms.dbo.Order_BOA_CustCD a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.Order_BOAUkey=s.Order_BOAUkey and t.ColumnValue=s.ColumnValue
		when matched then 
			update set 
				t.id			= isnull( s.id,         ''),
				t.CustCDID		= isnull( s.CustCDID,   ''),
				t.Refno			= isnull( s.Refno,      ''),
				t.SCIRefno		= isnull( s.SCIRefno,   ''),
				t.AddName		= isnull( s.AddName,    ''),
				t.AddDate		= s.AddDate,
				t.EditName		= isnull( s.EditName,   ''),
				t.EditDate		= s.EditDate, 
				t.ColumnValue	= isnull( s.ColumnValue ,'')
		when not matched by target then
			insert (
				Id			, Order_BOAUkey		, CustCDID		, Refno		, SCIRefno
				, AddName	, AddDate			, EditName		, EditDate	, ColumnValue
			) 
           VALUES
           (
                  isnull(s.id ,           ''), 
                  isnull(s.order_boaukey ,0),
                  isnull(s.custcdid ,     ''),
                  isnull(s.refno ,        ''),
                  isnull(s.scirefno ,     ''),
                  isnull(s.addname ,      ''),
                  s.adddate ,
                  isnull(s.editname ,     ''),
                  s.editdate,
                  isnull(s.columnvalue,   '')
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
				t.AddDate			= s.AddDate,
				t.PackLETA			= s.PackLETA
		when not matched by target then
			insert (
				Id			, NewSciDelivery	, OldSciDelivery	, LETA		, Remark
				, AddName	, AddDate			, Ukey              , PackLETA
			)
           VALUES
           (
                  isnull(s.id ,''),
                  s.newscidelivery ,
                  s.oldscidelivery ,
                  s.leta ,
                  isnull(s.remark ,''),
                  isnull(s.addname ,''),
                  s.adddate ,
                  isnull(s.ukey,0),
                  s.PackLETA
           )
		when not matched by source and t.id in (select id from #TOrder) then
			delete;
		----------------Order_QtyCTN------------Qty breakdown per Carton
		Merge Production.dbo.Order_QtyCTN as t
		Using (select a.* from Trade_To_Pms.dbo.Order_QtyCTN a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id=s.id and t.article=s.article and t.sizecode=s.sizecode
		when matched then 
			update set 
				t.Article	= isnull( s.Article,  ''),
				t.SizeCode	= isnull( s.SizeCode, ''),
				t.Qty		= isnull( s.Qty,      0),
				t.AddName	= isnull( s.AddName,  ''),
				t.AddDate	=  s.AddDate,
				t.EditName	= isnull( s.EditName, ''),
				t.EditDate	= s.EditDate
		when not matched by target then
			insert (
				Id			, Article		, SizeCode		, Qty	, AddName
				, AddDate	, EditName		, EditDate
			)
           VALUES
           (
                   isnull(s.id ,       ''),
                   isnull(s.article ,  ''),
                   isnull(s.sizecode , ''),
                   isnull(s.qty ,      0),
                   isnull(s.addname ,  ''),
                   s.adddate ,
                   isnull(s.editname , ''),
                   s.editdate
           )
		when not matched by source and t.id in (select id from #TOrder) then
			delete;

		----------------------[Order_ECMNFailed]
		Merge Production.dbo.[Order_ECMNFailed] t
		using (select a.* from Trade_To_Pms.dbo.Order_ECMNFailed a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
		on t.id = s.id and t.type = s.type
			when matched  then	update set 
				 t.[KPIFailed]		= isnull(s.[KPIFailed]		,'')
				,t.[KPIDate]		= s.[KPIDate]
				,t.[FailedComment]	= isnull(s.[FailedComment]	,'')
				,t.[ExpectApvDate]	= s.[ExpectApvDate]
				,t.[AddName]		= isnull(s.[AddName]		,'')
				,t.[AddDate]		= s.[AddDate]
				,t.[EditName]		= isnull(s.[EditName]		,'')
				,t.[EditDate]		=s.[EditDate]		
		when not matched by target then 	
			insert([ID],[Type],[KPIFailed],[KPIDate],[FailedComment],[ExpectApvDate],[AddName],[AddDate],[EditName],[EditDate])
           VALUES
           (
                  isnull(s.[ID],            ''),
                  isnull(s.[Type],          ''),
                  isnull(s.[KPIFailed],     ''),
                  s.[KPIDate],
                  isnull(s.[FailedComment], ''),
                  s.[ExpectApvDate],
                  isnull(s.[AddName],       ''),
                  s.[AddDate],
                  isnull(s.[EditName],''),
                  s.[EditDate]
           )
		when not matched by source and t.id in (select id from #TOrder) then
			delete
		;

------------Order_ShipPerformance----------------------
	Merge Production.dbo.Order_ShipPerformance as t
	Using (select a.* from Trade_To_Pms.dbo.Order_ShipPerformance a inner join #TOrder b on a.id = b.id) as s
	on t.[ID] = s.[ID] and t.[Seq] = s.[Seq]
	when matched then update set
		t.[BookDate] = s.[BookDate]
		,t.[PKManifestCreateDate] =  s.[PKManifestCreateDate]
		,t.[AddName] = isnull( s.[AddName],'')
		,t.[AddDate] =  s.[AddDate]
		,t.[EditName] = isnull( s.[EditName],'')
		,t.[EditDate] = s.[EditDate]
	when not matched by target then
		insert([Id],[Seq],[BookDate],[PKManifestCreateDate],[AddName],[AddDate],[EditName],[EditDate])
       VALUES
       (
              isnull(s.[Id],        ''),
              isnull(s.[Seq],       ''),
              s.[BookDate],
              s.[PKManifestCreateDate],
              isnull(s.[AddName],''),
              s.[AddDate],
              isnull(s.[EditName],''),
              s.[EditDate]
       )
	when not matched by source and t.id in (select id from #TOrder)then
			delete;

----------------order_markerlist_Article-----------------
	Merge Production.dbo.order_markerlist_Article as t
	Using (select a.* from Trade_To_Pms.dbo.order_markerlist_Article a inner join #TOrder b on a.id = b.id) as s
	on t.[Order_MarkerlistUkey] = s.[Order_MarkerlistUkey] and t.[Article] = s.[Article]
	when matched then update set
		t.[Id] = s.[Id]
		,t.[AddName] = isnull(s.[AddName],'')
		,t.[AddDate] = s.[AddDate]
		,t.[EditName] = isnull(s.[EditName],'')
		,t.[EditDate] = s.[EditDate]
	when not matched by target then
		insert([Id],[Order_MarkerlistUkey],[Article],[AddName],[AddDate],[EditName],[EditDate])
           VALUES
           (
                  isnull(s.[Id],                   ''),
                  isnull(s.[Order_MarkerlistUkey], 0),
                  isnull(s.[Article],              ''),
                  isnull(s.[AddName],              ''),
                  s.[AddDate],
                  isnull(s.[EditName],''),
                  s.[EditDate]
           )
	when not matched by source and t.id in (select id from #TOrder)then
			delete;

	----------Order_BuyBack--------------
	Merge Production.dbo.Order_BuyBack as t
	using (select a.* from Trade_To_Pms.dbo.Order_BuyBack a WITH (NOLOCK) inner join #Torder b on a.id=b.id) as s
	on t.ID = s.ID 
	and t.OrderIDFrom = s.OrderIDFrom
		when matched then 
		update set
			t.AddName	    = isnull(s.AddName,'') ,
			t.AddDate		= s.AddDate ,
			t.EditName		= isnull(s.EditName,'') ,
			t.EditDate		= s.EditDate 
	when not matched by target then
		insert  ([ID], [OrderIDFrom], [AddName], [AddDate], [EditName], [EditDate]) 
       VALUES
       (
              isnull(s.[ID],''),
              isnull(s.[OrderIDFrom],''),
              isnull(s.[AddName],''),
              s.[AddDate],
              isnull(s.[EditName],''),
              s.[EditDate]
       )
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;

	-----------Order_BuyBack_Qty------------------------ 
	Merge Production.dbo.Order_BuyBack_Qty as t
	Using (select a.* from Trade_To_Pms.dbo.Order_BuyBack_Qty as a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
	on t.ID = s.ID  
	and t.OrderIDFrom = s.OrderIDFrom
	and t.Article = s.Article
	and t.SizeCode = s.SizeCode
	and t.ArticleFrom = s.ArticleFrom
	and t.SizeCodeFrom = s.SizeCodeFrom
	when matched then
		update set
			t.Qty		= isnull(s.Qty,0),
			t.AddName	= isnull(s.AddName,'') ,
			t.AddDate	= s.AddDate ,
			t.EditName	= isnull(s.EditName,'') ,
			t.EditDate	= s.EditDate 
	when not matched by target then 
		insert ([ID], [OrderIDFrom], [Article], [SizeCode], [Qty], [AddName], [AddDate], [EditName], [EditDate], [ArticleFrom], [SizeCodeFrom]) 
       VALUES
       (
              isnull(s.[ID],         ''),
              isnull(s.[OrderIDFrom],''),
              isnull(s.[Article],    ''),
              isnull(s.[SizeCode],   ''),
              isnull(s.[Qty],        0),
              isnull(s.[AddName],''),
              s.[AddDate],
              isnull(s.[EditName],''),
              s.[EditDate],
              isnull(s.[ArticleFrom],''),
              isnull(s.sizecodefrom,'')
       )
	when not matched by source  AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;
		
---------------Order_Label_Detail-------------------
	Merge Production.dbo.Order_Label_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.Order_Label_Detail as a WITH (NOLOCK) inner join #TOrder b on a.id=b.id) as s
	on t.Ukey = s.Ukey
	when matched then
		update set
            t.[Order_LabelUkey] = isnull( s.[Order_LabelUkey],0)
           ,t.[ID]				= isnull( s.[ID]             ,'')
           ,t.[LabelType]		= isnull( s.[LabelType]      ,'')
           ,t.[Seq]				= isnull( s.[Seq]            ,0)
           ,t.[RefNo]			= isnull( s.[RefNo]          ,'')
           ,t.[Description]		= isnull( s.[Description]    ,'')
           ,t.[Position]		= isnull( s.[Position]       ,'')
           ,t.[Order_BOAUkey]	= isnull( s.[Order_BOAUkey]  ,0)
           ,t.[Junk]			= isnull( s.[Junk]           ,0)
           ,t.[ConsPC]			= isnull( s.[ConsPC]         ,0)
	when not matched by target then 
		insert 
        ([Order_LabelUkey]
        ,[ID]
        ,[LabelType]
        ,[Seq]
        ,[RefNo]
        ,[Description]
        ,[Position]
        ,[Order_BOAUkey]
        ,[Ukey]
        ,[Junk]
        ,[ConsPC])
		values 
        (isnull(s.[Order_LabelUkey],0)
        ,isnull(s.[ID]             ,'')
        ,isnull(s.[LabelType]      ,'')
        ,isnull(s.[Seq]            ,0)
        ,isnull(s.[RefNo]          ,'')
        ,isnull(s.[Description]    ,'')
        ,isnull(s.[Position]       ,'')
        ,isnull(s.[Order_BOAUkey]  ,0)
        ,isnull(s.[Ukey]           ,0)
        ,isnull(s.[Junk]           ,0)
        ,isnull(s.[ConsPC]         ,0)
        ) 
	when not matched by source AND T.ID IN (SELECT ID FROM #Torder) then 
	delete;


----------------OrderChangeApplication-----------------
update t set	
	[ReasonID] = isnull( s.ReasonID                         ,'')
	,[OrderID] = isnull( s.OrderID	                        ,'')
	,[Status]  = isnull(s.Status                            ,'')
	,[SentName] = isnull( s.SentName                        ,'')
	,[SentDate] = s.SentDate
	,[ApprovedName] = isnull( s.ApprovedName                ,'')
	,[ApprovedDate] = s.ApprovedDate
	,[RejectName] = isnull( s.RejectName                    ,'')
	,[RejectDate] = s.RejectDate
	,[ClosedName] = isnull( s.ClosedName                    ,'')
	,[ClosedDate] = s.ClosedDate
	,[JunkName] = isnull( s.JunkName                        ,'')
	,[JunkDate] = s.JunkDate
	,[AddName] = isnull( s.AddName                          ,'')
	,[AddDate] = s.AddDate
	,[TPEEditName] = isnull( s.EditName                     ,'')
	,[TPEEditDate] =  s.EditDate
	,[ToOrderID] = isnull( s.ToOrderID                      ,'')
	,[NeedProduction] = isnull( s.NeedProduction            ,0)
	,[OldQty] = isnull( s.OldQty                            ,0)
	,[RatioFty] = isnull( s.RatioFty                        ,0)
	,[RatioSubcon] = isnull( s.RatioSubcon                  ,0)
	,[RatioSCI] = isnull( s.RatioSCI                        ,0)
	,[RatioSupp] = isnull( s.RatioSupp                      ,0)
	,[RatioBuyer] = isnull( s.RatioBuyer                    ,0)
	,[ResponsibleFty] = isnull( s.ResponsibleFty            ,0)
	,[ResponsibleSubcon] = isnull( s.ResponsibleSubcon      ,0)
	,[ResponsibleSCI] = isnull( s.ResponsibleSCI            ,0)
	,[ResponsibleSupp] = isnull( s.ResponsibleSupp          ,0)
	,[ResponsibleBuyer] = isnull( s.ResponsibleBuyer        ,0)
	,[FactoryICRDepartment] = isnull( s.FactoryICRDepartment,'')
	,[FactoryICRNo] = isnull( s.FactoryICRNo                ,'')
	,[FactoryICRRemark] = isnull( s.FactoryICRRemark        ,'')
	,[SubconDBCNo] = isnull( s.SubconDBCNo                  ,'')
	,[SubconDBCRemark] = isnull( s.SubconDBCRemark          ,'')
	,[SubConName] = isnull( s.SubConName                    ,'')
	,[SCIICRDepartment] = isnull( s.SCIICRDepartment        ,'')
	,[SCIICRNo] = isnull( s.SCIICRNo                        ,'')
	,[SCIICRRemark] = isnull( s.SCIICRRemark                ,'')
	,[SuppDBCNo] = isnull( s.SuppDBCNo                      ,'')
	,[SuppDBCRemark] = isnull( s.SuppDBCRemark              ,'')
	,[BuyerDBCDepartment] = isnull( s.BuyerDBCDepartment    ,'')
	,[BuyerDBCNo] = isnull( s.BuyerDBCNo                    ,'')
	,[BuyerDBCRemark] = isnull( s.BuyerDBCRemark            ,'')
	,[BuyerICRNo] = isnull( s.BuyerICRNo                    ,'')
	,[BuyerICRRemark] = isnull( s.BuyerICRRemark            ,'')
	,[MRComment] = isnull( s.MRComment                      ,'')
	,[Remark] = isnull( s.Remark                            ,'')
	,[BuyerRemark] = isnull( s.BuyerRemark                  ,'')
	,[FactoryID] = isnull( isnull(s.FactoryID, '')          ,'')
	,[KeepPanels] = isnull( s.KeepPanels                    ,0)
	,[GMCheck] = isnull( s.GMCheck                          ,0)
from Production.dbo.OrderChangeApplication t
inner join Trade_To_Pms.dbo.OrderChangeApplication s on s.ID = t.ID
inner join Factory f on s.FactoryID = f.ID and f.IsProduceFty = 1

insert into Production.dbo.OrderChangeApplication ([ID], [ReasonID], [OrderID], [Status], [SentName], [SentDate]
, [ApprovedName], [ApprovedDate], [RejectName], [RejectDate], [ClosedName], [ClosedDate], [JunkName], [JunkDate]
, [AddName], [AddDate], [ToOrderID], [NeedProduction], [OldQty], [RatioFty], [RatioSubcon], [RatioSCI], [RatioSupp], [RatioBuyer]
, [ResponsibleFty], [ResponsibleSubcon], [ResponsibleSCI], [ResponsibleSupp], [ResponsibleBuyer], [FactoryICRDepartment], [FactoryICRNo], [FactoryICRRemark]
, [SubconDBCNo], [SubconDBCRemark], [SubConName], [SCIICRDepartment], [SCIICRNo], [SCIICRRemark], [SuppDBCNo], [SuppDBCRemark], [BuyerDBCDepartment], [BuyerDBCNo]
, [BuyerDBCRemark], [BuyerICRNo], [BuyerICRRemark], [MRComment], [Remark], [BuyerRemark], [FactoryID], [TPEEditName], [TPEEditDate],[KeepPanels],[GMCheck])
select s.ID
	,isnull(s.ReasonID                ,'')
	,isnull(s.OrderID                 ,'')
	,isnull(s.Status                  ,'')
	,isnull(s.SentName                ,'')
	,s.SentDate
	,isnull(s.ApprovedName            ,'')
	,s.ApprovedDate
	,isnull(s.RejectName              ,'')
	,s.RejectDate
	,isnull(s.ClosedName              ,'')
	,s.ClosedDate
	,isnull(s.JunkName                ,'')
	,s.JunkDate
	,isnull(s.AddName                 ,'')
	,s.AddDate
	,isnull(s.ToOrderID               ,'')
	,isnull(s.NeedProduction          ,0)
	,isnull(s.OldQty                  ,0)
	,isnull(s.RatioFty                ,0)
	,isnull(s.RatioSubcon             ,0)
	,isnull(s.RatioSCI                ,0)
	,isnull(s.RatioSupp               ,0)
	,isnull(s.RatioBuyer              ,0)
	,isnull(s.ResponsibleFty          ,0)
	,isnull(s.ResponsibleSubcon       ,0)
	,isnull(s.ResponsibleSCI          ,0)
	,isnull(s.ResponsibleSupp         ,0)
	,isnull(s.ResponsibleBuyer        ,0)
	,isnull(s.FactoryICRDepartment    ,'')
	,isnull(s.FactoryICRNo            ,'')
	,isnull(s.FactoryICRRemark        ,'')
	,isnull(s.SubconDBCNo             ,'')
	,isnull(s.SubconDBCRemark         ,'')
	,isnull(s.SubConName              ,'')
	,isnull(s.SCIICRDepartment        ,'')
	,isnull(s.SCIICRNo                ,'')
	,isnull(s.SCIICRRemark            ,'')
	,isnull(s.SuppDBCNo               ,'')
	,isnull(s.SuppDBCRemark           ,'')
	,isnull(s.BuyerDBCDepartment      ,'')
	,isnull(s.BuyerDBCNo              ,'')
	,isnull(s.BuyerDBCRemark          ,'')
	,isnull(s.BuyerICRNo              ,'')
	,isnull(s.BuyerICRRemark          ,'')
	,isnull(s.MRComment               ,'')
	,isnull(s.Remark                  ,'')
	,isnull(s.BuyerRemark             ,'')
	,isnull(s.FactoryID, '')
	,isnull(s.EditName                ,'')
	,s.EditDate
	,isnull(s.KeepPanels              ,0)
	,isnull(s.GMCheck                 ,0)
from Trade_To_Pms.dbo.OrderChangeApplication  s
inner join Production.dbo.Factory f on s.FactoryID =f.ID and f.IsProduceFty = 1
where not exists(select 1 from Production.dbo.OrderChangeApplication t where s.ID = t.ID)

----------------OrderChangeApplication_Detail-----------------
update t
	set t.Seq = isnull(s.Seq            ,'')
		,t.Article = isnull( s.Article  ,'')
		,t.SizeCode = isnull( s.SizeCode,'')
		,t.Qty = isnull( s.Qty          ,0)
		,t.OriQty = isnull( s.OriQty    ,0)
		,t.NowQty = isnull( s.NowQty    ,0)
from Production.dbo.OrderChangeApplication_Detail t
inner join Trade_To_Pms.dbo.OrderChangeApplication_Detail s on s.ID = t.ID and s.Ukey = t.Ukey

delete t
from Production.dbo.OrderChangeApplication_Detail t
where not exists (select 1 from Trade_To_Pms.dbo.OrderChangeApplication_Detail where t.ID = ID and t.Ukey = Ukey)
and exists (
	select 1 from Trade_To_Pms.dbo.OrderChangeApplication oc  
	inner join Production.dbo.Factory f on oc.FactoryID = f.ID and f.IsProduceFty = 1
	where oc.ID = t.ID	
)

insert into Production.dbo.OrderChangeApplication_Detail([Ukey], [ID], [Seq], [Article], [SizeCode], [Qty], [OriQty], [NowQty])
select
    isnull(s.Ukey,0)
	,isnull(s.ID        ,'')
	,isnull(s.Seq       ,'')
	,isnull(s.Article   ,'')
	,isnull(s.SizeCode  ,'')
	,isnull(s.Qty       ,0)
	,isnull(s.OriQty    ,0)
	,isnull(s.NowQty    ,0)
from Trade_To_Pms.dbo.OrderChangeApplication_Detail s
where not exists (select 1 from Production.dbo.OrderChangeApplication_Detail t where t.ID = s.ID and t.Ukey = s.Ukey)
and exists (
	select 1 from Trade_To_Pms.dbo.OrderChangeApplication oc  
	inner join Production.dbo.Factory f on oc.FactoryID = f.ID and f.IsProduceFty = 1
	where oc.ID = s.ID	
)
----------------OrderChangeApplication_Seq-----------------
update t
	set t.Seq = isnull(s.Seq                        ,'')
		,t.NewSeq = isnull( s.NewSeq                ,'')
		,t.ShipmodeID = isnull( s.ShipmodeID        ,'')
		,t.BuyerDelivery =  s.BuyerDelivery 
		,t.FtyKPI =  s.FtyKPI
		,t.ReasonID = isnull( s.ReasonID            ,'')
		,t.ReasonRemark = isnull( s.ReasonRemark    ,'')
		,t.ShipModeRemark = isnull( s.ShipModeRemark,'')
from Production.dbo.OrderChangeApplication_Seq t
inner join Trade_To_Pms.dbo.OrderChangeApplication_Seq s on s.ID = t.ID and s.Ukey = t.Ukey

delete t
from Production.dbo.OrderChangeApplication_Seq t
where not exists (select 1 from Trade_To_Pms.dbo.OrderChangeApplication_Seq where t.ID = ID and t.Ukey = Ukey)
and exists (
	select 1 from Trade_To_Pms.dbo.OrderChangeApplication oc  
	inner join Production.dbo.Factory f on oc.FactoryID = f.ID and f.IsProduceFty = 1
	where oc.ID = t.ID	
)

insert into Production.dbo.OrderChangeApplication_Seq([Ukey], [ID], [Seq], [NewSeq], [ShipmodeID], [BuyerDelivery], [FtyKPI], [ReasonID], [ReasonRemark], [ShipModeRemark])
select 
    isnull(s.Ukey,0)
	,isnull(s.ID            ,'')
	,isnull(s.Seq           ,'')
	,isnull(s.NewSeq        ,'')
	,isnull(s.ShipmodeID    ,'')
	,s.BuyerDelivery
	,s.FtyKPI
	,isnull(s.ReasonID      ,'')
	,isnull(s.ReasonRemark  ,'')
	,isnull(s.ShipModeRemark,'')
from Trade_To_Pms.dbo.OrderChangeApplication_Seq s
where not exists (select 1 from Production.dbo.OrderChangeApplication_Seq t where t.ID = s.ID and t.Ukey = s.Ukey)
and exists (
	select 1 from Trade_To_Pms.dbo.OrderChangeApplication oc  
	inner join Production.dbo.Factory f on oc.FactoryID = f.ID and f.IsProduceFty = 1
	where oc.ID = s.ID	
)
----------------OrderChangeApplication_History-----------------
INSERT INTO [dbo].[OrderChangeApplication_History]([ID],[Status],[StatusUser],[StatusDate])
SELECT isnull(s.id,          ''),
       isnull(s.status,      ''),
       isnull(s.[statususer],''),
       s.[statusdate]
from Trade_To_Pms.dbo.[OrderChangeApplication_History] s
left join Production.dbo.[OrderChangeApplication_History] t on s.ID = t.ID and s.Status = t.Status
where s.Status = 'Closed' and t.id is null


----------------Order_PatternPanel-----------------
Select
 o.ID
,o.POID
,oq.Article
,oq.SizeCode
,occ.PatternPanel
,cons.FabricPanelCode
into #Order_PatternPanel
from Orders o WITH (NOLOCK)
inner join Order_qty oq on o.ID=oq.ID
inner join Order_ColorCombo occ on o.poid = occ.id and occ.Article = oq.Article
inner join order_Eachcons cons on occ.id = cons.id and cons.FabricCombo = occ.PatternPanel and cons.CuttingPiece='0'
where occ.FabricCode !='' and occ.FabricCode is not null 
and exists(select 1 from #TOrder where ID = o.ID)
group by o.ID, o.POID, oq.Article, oq.SizeCode, occ.PatternPanel, cons.FabricPanelCode

select *
into #tmp_Order_PatternPanel_Detele
from Order_PatternPanel t
where exists (select 1 from #Order_PatternPanel where t.ID = ID and t.Article = Article and t.SizeCode = SizeCode)
and not exists (select 1 from #Order_PatternPanel where t.ID = ID and t.Article = Article and t.SizeCode = SizeCode 
											and t.POID = POID and t.PatternPanel = PatternPanel and t.FabricPanelCode = FabricPanelCode)

delete t
from Order_PatternPanel t
where exists (select 1 from #tmp_Order_PatternPanel_Detele  where t.ID = ID and t.Article = Article and t.SizeCode = SizeCode)
 
insert into Order_PatternPanel ([ID], [POID], [Article], [SizeCode], [PatternPanel], [FabricPanelCode])
SELECT isnull([id],          ''),
       isnull([poid],        ''),
       isnull([article],     ''),
       isnull([sizecode],    ''),
       isnull([patternpanel],''),
       isnull([fabricpanelcode],'')
from #Order_PatternPanel t
where not exists (select 1 from Order_PatternPanel where t.ID = ID and t.Article = Article and t.SizeCode = SizeCode)

---------------Order_FtyMtlStdCost-------------
Delete a
from Production.dbo.Order_FtyMtlStdCost as a
inner join #Torder b on a.OrderID = b.id
where not exists(select 1 from Trade_To_Pms.dbo.Order_FtyMtlStdCost 
							where a.OrderID  = OrderID and a.SCIRefno = SCIRefno)
---------------------------UPDATE 
		
UPDATE a
SET	 a.PurchaseCompanyID	= b.PurchaseCompanyID			
	,a.PurchasePrice		= isnull(b.PurchasePrice, 0)
	,a.UsagePrice			= isnull(b.UsagePrice, 0)	
	,a.CurrencyID			= b.CurrencyID		
	,a.Cons					= b.Cons		
	,a.UsageUnit			= b.UsageUnit
	,a.PurchaseUnit			= b.PurchaseUnit
	,a.AddName				= b.AddName
	,a.AddDate				= b.AddDate
from Production.dbo.Order_FtyMtlStdCost as a
inner join #Torder o on a.OrderID = o.id
inner join Trade_To_Pms.dbo.Order_FtyMtlStdCost as b ON a.OrderID = b.OrderID and a.SCIRefno = b.SCIRefno
-------------------------- INSERT
		
INSERT INTO Production.dbo.Order_FtyMtlStdCost(
	[OrderID]
	, [SCIRefno]
	, [PurchaseCompanyID]
	, [PurchasePrice]
	, [UsagePrice]
	, [CurrencyID]
	, [Cons]
	, [UsageUnit]
	, [PurchaseUnit]
	, [AddName]
	, [AddDate]
)
select b.[OrderID]
	, b.[SCIRefno]
	, b.[PurchaseCompanyID]
	, [PurchasePrice] = isnull(b.[PurchasePrice], 0)
	, [UsagePrice] = isnull(b.[UsagePrice], 0)
	, b.[CurrencyID]
	, b.[Cons]
	, b.[UsageUnit]
	, b.[PurchaseUnit]
	, b.[AddName]
	, b.[AddDate]
from Trade_To_Pms.dbo.Order_FtyMtlStdCost as b WITH (NOLOCK)
inner join #Torder o on b.OrderID = o.id
where not exists(select 1 from Production.dbo.Order_FtyMtlStdCost as a WITH (NOLOCK) 
					where a.OrderID = b.OrderID and a.SCIRefno = b.SCIRefno)

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
from Production.dbo.WorkOrderForPlanning b
where id in (select id from #tmpOrders as t 
where not exists(select 1 from #TOrder as s where t.id=s.ID))

Update b set b.MDivisionId='',FactoryID=''
from Production.dbo.WorkOrderForOutput b
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

-------------------------------------[dbo].[PO_Supp_Detail]
-- 因為這條件比較多, 所以該先刪除PO_Supp_Detail
delete b
from Production.dbo.PO_Supp_Detail b
left join Production.dbo.MDivisionPoDetail c on b.id = c.poid and b.SEQ1=c.Seq1 and b.SEQ2=c.Seq2
where id in (
	select POID from #tmpOrders as t 
	where not exists(select 1 from #TOrder as s where t.id=s.ID)
)
and b.ShipQty = 0
and (c.poid is null or c.InQty = 0)
and not exists(select 1 from Production.dbo.Invtrans i where i.InventoryPOID = b.ID and i.InventorySeq1 = b.Seq1 and InventorySeq2 = b.Seq2 and i.Type = '1')
and not exists (select 1 from Production.dbo.TransferExport_Detail ted where b.id = ted.poid and b.Seq1 = ted.Seq1 and b.Seq2 = ted.Seq2)

-------------------------------------[dbo].[PO]
Delete b
from Production.dbo.PO b
where id in (
	select POID from #tmpOrders as t 
	where not exists(select 1 from #TOrder as s where t.id=s.ID)
)
and not exists(
	select 1 from Production.dbo.PO_Supp_Detail psd 
	where psd.ID = b.ID
)
-------------------------------------[dbo].[PO_Supp]
Delete b
from Production.dbo.PO_Supp b
where id in (
	select POID from #tmpOrders as t 
	where not exists(select 1 from #TOrder as s where t.id=s.ID)
)
and not exists(
	select 1 from Production.dbo.PO_Supp_Detail psd 
	where psd.ID = b.ID
	and psd.SEQ1 = b.SEQ1
)

-------------------------------------[dbo].[PO_Supp_Detail_OrderList]
Delete b
from Production.dbo.PO_Supp_Detail_OrderList b
where id in (
	select POID from #tmpOrders as t 
	where not exists(select 1 from #TOrder as s where t.id=s.ID)
)
and not exists (
	select 1 
	from Production.dbo.PO_Supp_Detail psd 
	where psd.id = b.id
	and psd.SEQ1 = b.SEQ1
	and psd.SEQ2 = b.SEQ2
)
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
where a.id in (
	select id from #tmpOrders as t 
	where not exists(select 1 from #TOrder as s where t.id=s.ID)
)


drop table #tmpOrders
drop table #TOrder
drop table #tmpLocalOrder_TmsCost
---- 轉入 歷史資料 TradeHis_Order 必須與 Trade 資料相同----
Merge Production.dbo.TradeHis_Order as t
Using (
	select * 
	from Trade_To_Pms.dbo.TradeHis_Order
	where (tableName = 'Orders' 
		  and histype = 'OrdersBuyerDelivery') or
		  (TableName = 'Order_QtyShip' and HisType = 'Order_QtyShipFtyKPI' and ReasonTypeID = 'Order_BuyerDelivery')
) as s on t.Ukey = s.Ukey
when matched then update set
	t.[TableName] = isnull( s.[TableName]          ,'')
	, t.[HisType] = isnull( s.[HisType]            ,'')
	, t.[SourceID] = isnull( s.[SourceID]          ,'')
	, t.[ReasonTypeID] = isnull( s.[ReasonTypeID]  ,'')
	, t.[ReasonID] = isnull( s.[ReasonID]          ,'')
	, t.[OldValue] = isnull( s.[OldValue]          ,'')
	, t.[NewValue] = isnull( s.[NewValue]          ,'')
	, t.[Remark] = isnull( s.[Remark]              ,'')
	, t.[AddName] = isnull( s.[AddName]            ,'')
	, t.[AddDate] = s.[AddDate]
when not matched by target then
	insert (
		[UKEY] 		  , [TableName]  , [HisType]   , [SourceID]  , [ReasonTypeID] 
		, [ReasonID]  , [OldValue]   , [NewValue]  , [Remark]    , [AddName] 
		, [AddDate]
	) 
       VALUES
       (
              isnull(s.[UKEY] ,         0),
              isnull(s.[TableName],     ''),
              isnull(s.[HisType] ,      ''),
              isnull(s.[SourceID],      ''),
              isnull(s.[ReasonTypeID] , ''),
              isnull(s.[ReasonID],      ''),
              isnull(s.[OldValue] ,     ''),
              isnull(s.[NewValue],      ''),
              isnull(s.[Remark] ,       ''),
              isnull(s.[AddName] ,      ''),
              s.[AddDate]
       )
when not matched by source then
	delete;

END
------------------------------------------------------------------------------------------------------------------------


