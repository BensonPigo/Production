Create FUNCTION [dbo].[QtyBySetPerSubprocess_PatternPanel]
(
	/*
	
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！

	這邊如果有修改，Planning R15用到的共用Function [QtyBySetPerSubprocess]要一起修改，這兩個是做同樣的事情
	
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！
	請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！請注意！！！！
	*/

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
	 * @IsMorethenOrderQty
		回傳Qty值是否超過訂單數, (生產有可能超過)
	 */
	@OrderID varchar (13)
	, @SubprocessID varchar (300)
	, @InStartDate datetime = null
	, @InEndDate datetime = null
	, @OutStartDate datetime = null
	, @OutEndDate datetime = null
	, @IsNeedCombinBundleGroup bit = 0
	, @IsMorethenOrderQty bit = 1
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
	PatternPanel varchar(2),
	QtyBySet int,
	QtyBySubprocess int,
	InQtyBySet int,
	OutQtyBySet int,
	InQtyByPcs int,
	OutQtyByPcs int,
	FinishedQtyBySet int,
	SubprocessId varchar(15)
)
AS
BEGIN
	declare @AllOrders Table (
		OrderID varchar(13),
		POID varchar(13),
		PatternPanel varchar(2),
		FabricPanelCode varchar(2),
		Article varchar(8),
		SizeCode varchar(100),
		PatternDesc nvarchar(100)
	)

	declare @QtyBySetPerCutpart Table(
		OrderID varchar(13),
		SubprocessId varchar(15),
		POID varchar(13),
		PatternPanel varchar(2),
		FabricPanelCode varchar(2),
		Article varchar(8),
		SizeCode varchar(100),
		PatternCode varchar(20),
		QtyBySet int,
		QtyBySubprocess int
	)

	declare @CutpartBySet Table(
		OrderID varchar(13),
		SubprocessId varchar(15),
		Article varchar(8),
		SizeCode varchar(100),
		PatternPanel varchar(2),
		QtyBySet int,
		QtyBySubprocess int
	)

	declare @BundleInOutDetail Table(
		OrderID varchar(13),
		SubprocessId varchar(15),
		InOutRule tinyint,
		BundleGroup int,
		Size varchar(100),
		Article varchar(8),
		PatternPanel varchar(2),
		FabricPanelCode varchar(2),
		PatternCode varchar(20),
		InComing datetime,
		OutGoing datetime,
		Qty int,
		IsPair bit,
		m int
	)

	declare @BundleInOutQty Table(
		OrderID varchar(13),
		SubprocessId varchar(15),
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
		FinishedQty int,
		num int,
		num_In int,
		num_Out int,
		num_F int
	)
	
	declare @FinalQtyBySet Table(
		OrderID varchar(13),
		SubprocessId varchar(15),
		Article varchar(8),
		SizeCode varchar(100),
		PatternPanel varchar(2),
		InQty int,
		OutQty int,
		FinishQty int
	)
	declare @SubProcess Table(
		SubprocessId varchar(15),
		IsRFIDDefault bit,
		InOutRule tinyint
	)

	--拆分傳入的subprocess
	insert into @SubProcess
	select b.Id,b.IsRFIDDefault,b.InOutRule
	from dbo.SplitString(@SubprocessID,',') a
	inner join SubProcess b with (nolock) on a.Data = b.Id

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
			, bd.Sizecode
			, bd.PatternDesc
	from Bundle_Detail bd
	inner join Bundle bun on bun.id = bd.id
	inner join Orders os on bun.Orderid = os.ID and  bun.MDivisionID = os.MDivisionID
	where bun.Orderid = @OrderID
	
	-- Step 2. --
	insert into @QtyBySetPerCutpart
	select distinct	st1.OrderID,
			sub.SubprocessId,
			st1.POID,
			st1.PatternPanel,
			st1.FabricPanelCode,
			st1.Article,
			st1.SizeCode,
			CutpartCount.PatternCode,
			CutpartCount.QtyBySet,
			CutpartCount.QtyBySubprocess
	from @AllOrders st1
	cross join @SubProcess sub
	outer apply (
		select	bunD.Patterncode
				, QtyBySet = count (1)
				, QtyBySubprocess = sum (isnull (QtyBySubprocess.v, 0))
		from (
			select top 1 ID = iif(x1.ID is null,x2.ID ,x1.ID),BundleGroup = iif(x1.ID is null,x2.BundleGroup ,x1.BundleGroup)
			from (select st1.Orderid,st1.PatternPanel,st1.FabricPanelCode,st1.Article,st1.Sizecode,st1.PatternDesc)x0
			outer apply (
				select	top 1
						bunD.ID
						, bunD.BundleGroup
						, bunD.BundleNo
				from Bundle bun
				INNER JOIn Orders o ON bun.Orderid=o.ID AND bun.MDivisionid=o.MDivisionID  /*2019/10/03 ISP20191382 */
				inner join Bundle_Detail bunD on bunD.Id = bun.ID
				where bun.Orderid = x0.Orderid
					  and bun.PatternPanel = x0.PatternPanel
					  and bun.FabricPanelCode = x0.FabricPanelCode
					  and bun.Article = x0.Article
					  and bunD.Sizecode = x0.Sizecode
					  and bunD.PatternDesc = x0.PatternDesc
					  and exists (select 1
									  from Bundle_Detail_Art BunDArt
									  where BunDArt.Bundleno = bunD.BundleNo
											and BunDArt.SubprocessId = sub.SubprocessId)
				order by bun.AddDate desc
			)x1
			outer apply(
				select	top 1
						bunD.ID
						, bunD.BundleGroup
						, bunD.BundleNo
				from Bundle bun
				INNER JOIn Orders o ON bun.Orderid=o.ID AND bun.MDivisionid=o.MDivisionID  /*2019/10/03 ISP20191382 */
				inner join Bundle_Detail bunD on bunD.Id = bun.ID
				where bun.Orderid = x0.Orderid
					  and bun.PatternPanel = x0.PatternPanel
					  and bun.FabricPanelCode = x0.FabricPanelCode
					  and bun.Article = x0.Article
					  and bunD.Sizecode = x0.Sizecode
					  and bunD.PatternDesc = x0.PatternDesc
				order by bun.AddDate desc
			)x2
		) getGroupInfo
		inner join Bundle_Detail bunD on getGroupInfo.Id = bunD.Id
										 and getGroupInfo.BundleGroup = bunD.BundleGroup
		outer apply (
			select v = (select 1
						where exists (select 1
									  from Bundle_Detail_Art BunDArt
									  where BunDArt.Bundleno = bunD.BundleNo
											and BunDArt.SubprocessId = sub.SubprocessId))
		) QtyBySubprocess
		group by bunD.Patterncode
	) CutpartCount
		
	-- Step 3. --
	insert into @CutpartBySet
	select	st2.Orderid
			, st2.SubprocessId
			, st2.Article
			, st2.Sizecode
			, st2.PatternPanel
			, QtyBySet = sum (st2.QtyBySet)
			, QtyBySubprocess = sum (st2.QtyBySubProcess)
	from @QtyBySetPerCutpart st2
	group by st2.Orderid, st2.SubprocessId, st2.Article, st2.Sizecode, st2.PatternPanel

	-- Query by Set per Subprocess--
	/*
	 * 計算成衣件數 by 外加工
	 * 1.	找出時間區間內指定訂單中裁片的進出資訊
			各個 SubProcess 完成與否應該要參照 SubProces.InOutRule 的設定
			Only In - 有 InComing 的紀錄就算完成
			Only Out - 有 OutGoing 的紀錄就算完成
			其他狀態 - 必須要有 InComing & OutGoing 才算完成
			
			因此請協助調整 SQL Function (QtyBySetPerSubprocess)
			回傳的資料表新增一個欄位
			FinishedQtyBySet
			InOutRule
			0-NotSetting
			1-OnlyIn
			2-OnlyOut
			3-FromInToOut
			4-FromOutToIn

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

	 insert into @BundleInOutDetail
	 select	st0.Orderid
			, sub.SubprocessId
			, sub.InOutRule
			, bunD.BundleGroup
			, os.SizeCode
			, st0.Article
			, st0.PatternPanel
			, st0.FabricPanelCode
			, st0.PatternCode
			, bunIO.InComing
			, bunIO.OutGoing
			, bunD.Qty
			, [IsPair]=ISNULL(TopIsPair.IsPair,0)
			, iif (sub.IsRFIDDefault = 1, st0.QtyBySet, st0.QtyBySubprocess)
	from @QtyBySetPerCutpart st0
	inner join @SubProcess sub on st0.SubprocessId = sub.SubprocessId
	left join Order_SizeCode os with (nolock) on os.ID = st0.POID and os.SizeCode = st0.SizeCode
	outer apply(
		select bunD.BundleGroup, bunD.Qty, bunD.BundleNo,bunD.IsPair
		from Bundle bun with (nolock) 
		inner join Bundle_Detail bunD with (nolock)  on bunD.Id = bun.id 
		where bun.Orderid = st0.Orderid and 
								bun.PatternPanel = st0.PatternPanel and
								bun.FabricPanelCode = st0.FabricPanelCode and
								bun.Article = st0.Article  and
								bunD.Patterncode = st0.Patterncode and
								bunD.Sizecode = os.SizeCode
	)bund
	outer apply(
		select	top 1
				bunD.ID
				, bunD.BundleGroup
				, bunD.BundleNo
				, bunD.IsPair
		from Bundle bun
		INNER JOIn Orders o ON bun.Orderid=o.ID AND bun.MDivisionid=o.MDivisionID  
		inner join Bundle_Detail bunD on bunD.Id = bun.ID
		where bun.Orderid = st0.Orderid 
			and bun.PatternPanel = st0.PatternPanel 
			and bun.FabricPanelCode = st0.FabricPanelCode 
			and bun.Article = st0.Article  
			and bunD.Patterncode = st0.Patterncode 
			and bunD.Sizecode = os.SizeCode
		order by bun.AddDate desc
	)TopIsPair
	left join BundleInOut bunIO with (nolock)  on bunIO.BundleNo = bunD.BundleNo and bunIO.SubProcessId = sub.SubprocessId and isnull(bunIO.RFIDProcessLocationID,'') = ''
	where (sub.IsRFIDDefault = 1 or st0.QtyBySubprocess != 0)

	insert into @BundleInOutQty
	select	Orderid
			, SubprocessId
			, BundleGroup
			, Size
			, Article
			, PatternPanel
			, FabricPanelCode
			, PatternCode
			, InQty = sum(iif(InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate),Qty,0)) / iif(IsPair=1,IIF(m=1,2,m),1) 
			, OutQty = sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate),Qty,0)) / iif(IsPair=1,IIF(m=1,2,m),1) 
			, OriInQty = sum(iif(InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate),Qty,0)) --原始裁片數總和
			, OriOutQty = sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate),Qty,0)) --原始裁片數總和
			, FinishedQty = (case	when InOutRule = 1 then sum(iif(InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate),Qty,0))
									when InOutRule = 2 then sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate),Qty,0))
									else sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate) and
										  InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate)
										  ,Qty,0)) end) / iif(IsPair=1,IIF(m=1,2,m),1) 
			 ,num = count(1)
			, num_In = sum(iif(InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate),1,0)) 
			, num_Out= sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate),1,0))
			, num_F  = case	when InOutRule = 1 then sum(iif(InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate),1,0))
									when InOutRule = 2 then sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate),1,0))
									else sum(iif(OutGoing is not null and (@OutStartDate is null or @OutStartDate <= OutGoing) and (@OutEndDate is null or OutGoing <= @OutEndDate) and
										  InComing is not null and (@InStartDate is null or @InStartDate <= InComing) and (@InEndDate is null or InComing <= @InEndDate)
										  ,1,0)) end
	from @BundleInOutDetail	
	group by OrderID, SubprocessId, InOutRule, BundleGroup, Size, PatternPanel, FabricPanelCode, Article, PatternCode,IsPair,m

	-- 篩選 BundleGroup Step.1 --

	
	-- Step 2. --	
	if (@IsNeedCombinBundleGroup = 1)
	begin
		insert into @FinalQtyBySet
		select	OrderID
				, SubprocessId
				, Article
				, Size
				, PatternPanel
				, InQty = min (InQty)
				, OutQty = min (OutQty)
				, FinishedQty = min (FinishedQty)
		from (
			select	OrderID
					, SubprocessId
					, Size
					, Article
					, PatternPanel
					, FabricPanelCode
					, InQty = sum (InQty)
					, OutQty = sum (OutQty)
					, FinishedQty = sum (FinishedQty)
			from (
				select	OrderID
						, SubprocessId
						, Size
						, Article
						, PatternPanel
						, FabricPanelCode
						, BundleGroup
						, InQty = min (InQty)
						, OutQty = min (OutQty)
						, FinishedQty = min (FinishedQty)
				from @BundleInOutQty
				group by OrderID, SubprocessId, Size, Article, PatternPanel, FabricPanelCode, BundleGroup
			) minGroupCutpart							
			group by OrderID, SubprocessId, Size, Article, PatternPanel, FabricPanelCode
		) sumGroup
		group by OrderID, SubprocessId, Size, Article, PatternPanel
	end
	else
	begin
		insert into @FinalQtyBySet
		select	OrderID
				, SubprocessId
				, Article
				, Size
				, PatternPanel
				, InQty = min (InQty)
				, OutQty = min (OutQty)
				, FinishedQty = min (FinishedQty)
		from (
			select	OrderID
					, SubprocessId
					, Size
					, Article
					, PatternPanel
					, FabricPanelCode
					, InQty = min (InQty)
					, OutQty = min (OutQty)
					, FinishedQty = min (FinishedQty)
			from (
				select	OrderID
						, SubprocessId
						, Size
						, PatternPanel
						, FabricPanelCode
						, Article
						, PatternCode
						, InQty = sum (InQty)
						, OutQty = sum (OutQty)
						, FinishedQty = sum (FinishedQty)
				from @BundleInOutQty
				group by OrderID, SubprocessId, Size, Article, PatternPanel, FabricPanelCode, PatternCode
			) sumbas
			group by OrderID, SubprocessId, Size, Article, PatternPanel, FabricPanelCode
		) minCutpart
		group by OrderID, SubprocessId, Size, Article, PatternPanel
	end


	
	-- Result Data --
	insert into @QtyBySetPerSubprocess
	select	OrderID = cbs.OrderID
			, cbs.Article
			, cbs.Sizecode
			, cbs.PatternPanel
			, QtyBySet = cbs.QtyBySet
			, QtyBySubprocess = cbs.QtyBySubprocess
			, InQtyBySet = case when @IsMorethenOrderQty = 1 then sub.InQty
							when sub.InQty>oq.qty then oq.qty
							else sub.InQty
							end
			, OutQtyBySet = case when @IsMorethenOrderQty = 1 then sub.OutQty
							when sub.OutQty>oq.qty then oq.qty
							else sub.OutQty
							end
			, InQtyByPcs
			, OutQtyByPcs
			, FinishedQtyBySet = case when @IsMorethenOrderQty = 1 then sub.FinishQty
							when sub.FinishQty>oq.qty then oq.qty
							else sub.FinishQty
							end
			, sub.SubprocessId
	from @CutpartBySet cbs
	left join Order_Qty oq on oq.id = cbs.OrderID and oq.SizeCode = cbs.SizeCode and oq.Article = cbs.Article
	left join @FinalQtyBySet sub on cbs.Orderid = sub.Orderid and cbs.Sizecode = sub.SizeCode and cbs.Article = sub.Article and cbs.PatternPanel = sub.PatternPanel
			and cbs.SubprocessId = sub.SubprocessId
	outer apply (
		select	InQtyByPcs = sum (isnull (bunIO.OriInQty, 0))
				, OutQtyByPcs = sum (isnull (bunIO.OriOutQty, 0))
		from @BundleInOutQty bunIO
		where cbs.OrderID = bunIO.OrderID and cbs.Sizecode = bunIO.Size and cbs.Article = bunIO.Article and bunIO.SubprocessId = sub.SubprocessId and bunIO.PatternPanel = sub.PatternPanel
	) IOQtyPerPcs
	where sub.SubprocessId is not null
	RETURN ;
END