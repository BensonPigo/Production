

CREATE FUNCTION [dbo].[QtyBySetPerSubprocess]
(
	/*
	 * @Order
		訂單號碼
	 * @SubprocessID
		外加工段
	 * @IsSpectialReader
		特殊的外加工段
		e.g. Sorting, Loading...
	 * @InStartDate
		篩選裁片收進的起始日
	 * @InEndDate
		篩選裁片收進的結束日
	 * @OutStartDate
		篩選裁片完成加工段的起始日
	 * @OutEndDate
		篩選裁片完成加工段的結束日
	 * @IsNeedCombinBundleGroup
		是否要依照 BundleGroup 算成衣件數
	 */
	@OrderID varchar (13)
	, @SubprocessID varchar (10)
	, @IsSpectialReader bit = 0
	, @InStartDate datetime = null
	, @InEndDate datetime = null
	, @OutStartDate datetime = null
	, @OutEndDate datetime = null
	, @IsNeedCombinBundleGroup bit = 0
)
RETURNS 
@QtyBySetPerSubprocess TABLE 
(
	/*
	-----------------------------------------------------------------------------
	-------------------回傳By OrderID, Article, SizeCode計算的Table--------------
	-----------------------------------------------------------------------------
	 *	OrderID
		訂單號碼
	 *	Article
		訂單配色組
	 *	SizeCode
		訂單尺碼
	 *	QtyBySet
		此訂單完成一件成衣須要的裁片數量
	 *	QtyBySubprocess
		此訂單一件成衣須要執行『某外加工段』的裁片數量
	 *	InQtyBySet
		進入此外加工並且可組成成衣的數量
	 *	OutQtyBySet
		完成此外加工並且可組成成衣的數量
	 *	InQtyByPcs
		進入此外加工的裁片數量
	 *	OutQtyByPcs
		完成此外加工的裁片數量
	 */
	OrderID varchar(13),
	Article varchar(8),
	SizeCode varchar(100),
	QtyBySet int,
	QtyBySubprocess int,
	InQtyBySet int,
	OutQtyBySet int,
	InQtyByPcs int,
	OutQtyByPcs int
)
AS
BEGIN
	declare @AllOrders Table (
		OrderID varchar(13),
		POID varchar(13),
		PatternPanel varchar(2),
		FabricPanelCode varchar(2),
		Article varchar(8),
		SizeCode varchar(100)
	)

	declare @QtyBySetPerCutpart Table(
		OrderID varchar(13),
		POID varchar(13),
		PatternPanel varchar(2),
		FabricPanelCode varchar(2),
		Article varchar(8),
		SizeCode varchar(100),
		PatternCode varchar(20),
		QtyBySet int,
		QtyBySubprocess int, 
		num int
	)

	declare @CutpartBySet Table(
		OrderID varchar(13),
		Article varchar(8),
		SizeCode varchar(100),
		QtyBySet int,
		QtyBySubprocess int
	)

	declare @BundleInOutQty Table(
		OrderID varchar(13),
		BundleGroup int,
		Size varchar(100),
		Article varchar(8),
		PatternPanel varchar(2),
		FabricPanelCode varchar(2),
		PatternCode varchar(20),
		InQty int,
		OutQty int,
		OriInQty int,
		OriOutQty int,
		num int
	)

	declare @FinalQtyBySet Table(
		OrderID varchar(13),
		Article varchar(8),
		SizeCode varchar(100),
		InQty int,
		OutQty int
	)

	/*
	 * 1.	尋找指定訂單 Fabric Combo + Fabric Panel Code
	 *		使用資料表 Bundle 去除重複即可得到每張訂單 Fabric Combo + Fabric Panel Code + Article + SizeCode
	 * 2.	找出所有 Fabric Combo + Fabric Pancel Code + Article + SizeCode -> Cartpart (包含同部位數量)
			使用資料表 Bundle_Detail
			條件 訂單號碼 + Fabric Combo + Fabric Panel Code + Article + SizeCode
			top 1 Bundle Group 當作基準計算每個部位數量
			數量分成以下 2 種
			a.	QtyBySet
				數量直接加總
			b.	QtyBySubprocess
				部位有須要用 X 外加工計算
	 * 3.	加總每個訂單各 Fabric Combo 所有捆包的『數量』
	 */
	 -- Step 1. -- 
	insert into @AllOrders
	select	distinct
			bun.Orderid
			, bun.POID
			, bun.PatternPanel
			, bun.FabricPanelCode
			, bun.Article
			, bun.Sizecode
	from Bundle bun
	inner join Orders os on  bun.MDivisionID = os.MDivisionID and bun.Orderid = os.ID
	where bun.Orderid = @OrderID
	
	-- Step 2. --
	insert into @QtyBySetPerCutpart
	select	*
			, num = count (1) over (partition by OrderID, PatternPanel, FabricPanelCode, Article, Sizecode)
	from @AllOrders st1
	outer apply (
		select	bunD.Patterncode
				, QtyBySet = count (1)
				, QtyBySubprocess = sum (isnull (QtyBySubprocess.v, 0))
		from (
			select	top 1
					bunD.ID
					, bunD.BundleGroup
			from Bundle_Detail bunD
			inner join Bundle bun on bunD.Id = bun.ID
			where bun.Orderid = st1.Orderid
				  and bun.PatternPanel = st1.PatternPanel
				  and bun.FabricPanelCode = st1.FabricPanelCode
				  and bun.Article = st1.Article
				  and bun.Sizecode = st1.Sizecode
		) getGroupInfo
		inner join Bundle_Detail bunD on getGroupInfo.Id = bunD.Id
										 and getGroupInfo.BundleGroup = bunD.BundleGroup
		outer apply (
			select v = (select 1
						where exists (select 1								  
									  from Bundle_Detail_Art BunDArt
									  where BunDArt.Bundleno = bunD.BundleNo
											and BunDArt.SubprocessId = @SubprocessID))
		) QtyBySubprocess
		group by bunD.Patterncode
	) CutpartCount
		
	-- Step 3. --
	insert into @CutpartBySet
	select	st2.Orderid
			, st2.Article
			, st2.Sizecode
			, QtyBySet = sum (st2.QtyBySet)
			, QtyBySubprocess = sum (st2.QtyBySubProcess)
	from @QtyBySetPerCutpart st2
	group by st2.Orderid, st2.Article, st2.Sizecode

	-- Query by Set per Subprocess--
	/*
	 * 計算成衣件數 by 外加工
	 * 1.	找出時間區間內指定訂單中裁片的進出資訊
	 * 2.	成衣件數判斷
			a.	需判斷 BundleGroup
				a.1	加總 OrderID, Size, PatternPanel, FabricPanelCode, Article 每個部位的數量
				a.2 依照以下順序取最小值
					FabricPanelCode
					PatternPanel
					Article
					Size
			b.	不需判斷 BundleGroup
				b.1	判斷每個 Group 是否有缺少部位
					缺少部位直接補 0
				b.2	找出每個 BoundleGroup 最小值
				b.3	加總 OrderID, Size, PatternPanel, FabricPanelCode, Article 每個捆包的數量
				b.4	依照以下順序取最小值
					FabricPanelCode
					PatternPanel
					Article
					Size
	 *	3.	最終算出每張訂單目前可完成的成衣件數
	 */
	-- Step 1. --	
	insert into @BundleInOutQty
	select	st0.Orderid
			, BundleGroup = RFID.BundleGroup
			, Size = Size.v
			, st0.Article
			, st0.PatternPanel
			, st0.FabricPanelCode
			, st0.PatternCode
			, InQty = sum (isnull (RFID.InQty, 0))
			, OutQty = sum (isnull (RFID.OutQty, 0))
			, OriInQty = sum (isnull (RFID.InQty, 0))
			, OriOutQty = sum (isnull (RFID.OutQty, 0))
			, num = count (1) over (partition by st0.Orderid, Size.v, PatternPanel, FabricPanelCode, RFID.BundleGroup)
	from @QtyBySetPerCutpart st0
	outer apply (
		Select	v = OSize.SizeCode
		from Order_SizeCode OSize
		where st0.POID = OSize.Id and st0.Sizecode = OSize.SizeCode
	) Size
	outer apply (
		select	InQty = sum(bunD.Qty) / iif (@IsSpectialReader = 1, st0.QtyBySet
																  , st0.QtyBySubprocess)
				, OutQty = 0
				, bunD.BundleGroup
				, Size = bunD.SizeCode
		from BundleInOut bunIO
		inner join Bundle_Detail bunD on bunIO.BundleNo = bunD.BundleNo
		inner join Bundle bun on bunD.Id = bun.ID
		where bun.Orderid = st0.Orderid
				and bun.Sizecode = Size.v
				and bun.PatternPanel = st0.PatternPanel
				and bun.FabricPanelCode = st0.FabricPanelCode
				and bunD.Patterncode = st0.Patterncode
				and bun.Article = st0.Article
				and bunIO.SubProcessId = @SubprocessID
				and bunIO.InComing is not null
				and isnull(bunIO.RFIDProcessLocationID,'') = ''
				and (@InStartDate is null or @InStartDate <= bunIo.InComing)
				and (@InEndDate is null or bunIO.InComing <= @InEndDate)
		group by bunD.BundleGroup, bunD.SizeCode

		union all
		select	InQty = 0
				,OutQty = sum (bunD.Qty) / iif (@IsSpectialReader = 1, st0.QtyBySet
																	 , st0.QtyBySubprocess)
				, bunD.BundleGroup
				, Size = bunD.SizeCode
		from BundleInOut bunIO		
		inner join Bundle_Detail bunD on bunIO.BundleNo = bunD.BundleNo
		inner join Bundle bun on bunD.Id = bun.ID
		where bun.Orderid = st0.Orderid
				and bun.Sizecode = Size.v
				and bun.PatternPanel = st0.PatternPanel
				and bun.FabricPanelCode = st0.FabricPanelCode
				and bunD.Patterncode = st0.Patterncode
				and bun.Article = st0.Article
				and bunIO.SubProcessId = @SubprocessID
				and bunIO.OutGoing is not null
				and isnull(bunIO.RFIDProcessLocationID,'') = ''
				and (@OutStartDate is null or @OutStartDate <= bunIO.OutGoing)
				and (@OutEndDate is null or bunIO.OutGoing <= @OutEndDate)
		group by bunD.BundleGroup, bunD.SizeCode
	) RFID
	where (@IsSpectialReader = 1 or st0.QtyBySubprocess != 0)
	group by st0.OrderID, RFID.BundleGroup, Size.v, st0.PatternPanel, st0.FabricPanelCode, st0.Article, st0.PatternCode, st0.num

	-- 篩選 BundleGroup Step.1 --
	if (@IsNeedCombinBundleGroup = 1)
	begin
		update bunInOut
		set bunInOut.InQty = 0
			, bunInOut.OutQty = 0
		from @BundleInOutQty bunInOut
		inner join @QtyBySetPerCutpart bas on bunInOut.OrderID = bas.OrderID
											  and bunInOut.PatternPanel = bas.PatternPanel
											  and bunInOut.FabricPanelCode = bas.FabricPanelCode
											  and bunInOut.Article = bas.Article
											  and bunInOut.Size = bas.SizeCode
		where bunInOut.num < bas.num
	end
	
	-- Step 2. --
	if (@IsNeedCombinBundleGroup = 1)
	begin
		insert into @FinalQtyBySet
		select	OrderID
				, Article
				, Size
				, InQty = min (InQty)
				, OutQty = min (OutQty)
		from (
			select	OrderID
					, Size
					, Article
					, PatternPanel
					, InQty = min (InQty)
					, OutQty = min (OutQty)
			from (
				select	OrderID
						, Size
						, Article
						, PatternPanel
						, FabricPanelCode
						, InQty = sum (InQty)
						, OutQty = sum (OutQty)
				from (
					select	OrderID
							, Size
							, Article
							, PatternPanel
							, FabricPanelCode
							, BundleGroup
							, InQty = min (InQty)
							, OutQty = min (OutQty)
					from @BundleInOutQty
					group by OrderID, Size, Article, PatternPanel, FabricPanelCode, BundleGroup
				) minGroupCutpart							
				group by OrderID, Size, Article, PatternPanel, FabricPanelCode
			) sumGroup
			group by OrderID, Size, Article, PatternPanel
		) minFabricPanelCode
		group by OrderID, Size, Article
	end
	else
	begin
		insert into @FinalQtyBySet
		select	OrderID
				, Article
				, Size
				, InQty = min (InQty)
				, OutQty = min (OutQty)
		from (
			select	OrderID
					, Size
					, Article
					, PatternPanel
					, InQty = min (InQty)
					, OutQty = min (OutQty)
			from (
				select	OrderID
						, Size
						, Article
						, PatternPanel
						, FabricPanelCode
						, InQty = min (InQty)
						, OutQty = min (OutQty)
				from (
					select	OrderID
							, Size
							, PatternPanel
							, FabricPanelCode
							, Article
							, PatternCode
							, InQty = sum (InQty)
							, OutQty = sum (OutQty)
					from @BundleInOutQty
					group by OrderID, Size, Article, PatternPanel, FabricPanelCode, PatternCode
				) sumbas
				group by OrderID, Size, Article, PatternPanel, FabricPanelCode
			) minCutpart
			group by OrderID, Size, Article, PatternPanel
		) minFabricPanelCode
		group by OrderID, Size, Article
	end
	
	-- Result Data --
	insert into @QtyBySetPerSubprocess
	select	OrderID = cbs.OrderID
			, cbs.Article
			, cbs.Sizecode
			, QtyBySet = cbs.QtyBySet
			, QtyBySubprocess = cbs.QtyBySubprocess
			, InQtyBySet = sub.InQty
			, OutQtyBySet = sub.OutQty
			, InQtyByPcs
			, OutQtyByPcs
	from @CutpartBySet cbs
	left join @FinalQtyBySet sub on cbs.Orderid = sub.Orderid and cbs.Sizecode = sub.SizeCode and cbs.Article = sub.Article
	outer apply (
		select	InQtyByPcs = sum (isnull (bunIO.OriInQty, 0))
				, OutQtyByPcs = sum (isnull (bunIO.OriOutQty, 0))
		from @BundleInOutQty bunIO
		where cbs.OrderID = bunIO.OrderID and cbs.Sizecode = bunIO.Size and cbs.Article = bunIO.Article
	) IOQtyPerPcs

	RETURN ;
END