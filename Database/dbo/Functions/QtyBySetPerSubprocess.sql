
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
	declare @FinalQtyBySet Table(
		OrderID varchar(13),
		Article varchar(8),
		SizeCode varchar(100),
		QtyBySet int,
		QtyBySubprocess int,
		InQty int,
		OutQty int,
		InQtyByPcs int,
		OutQtyByPcs int,
		FinishQty int,
		SubprocessId varchar(15)
	)

	if (@IsNeedCombinBundleGroup = 1)
	begin
		insert into @FinalQtyBySet
		select	OrderID
				, Article
				, SizeCode
				, QtyBySet = isnull(sum(QtyBySet),0)
				, QtyBySubprocess = isnull(sum(QtyBySubprocess),0)
				, InQty = min (InQtyBySet)
				, OutQty = min (OutQtyBySet)
				, InQtyByPcs = isnull(sum(InQtyByPcs),0)
				, OutQtyByPcs = isnull(sum(OutQtyByPcs),0)
				, FinishedQty = min (FinishedQtyBySet)
				, SubprocessId
		from (
			select a.*
			from dbo.[QtyBySetPerSubprocess_PatternPanel](@OrderID,@SubprocessID,@InStartDate,@InEndDate,@OutStartDate,@OutEndDate,@IsNeedCombinBundleGroup,1)a
		) minFabricPanelCode
		group by OrderID, SubprocessId, SizeCode, Article
	end
	else
	begin
		insert into @FinalQtyBySet
		select	OrderID
				, Article
				, SizeCode
				, QtyBySet = isnull(sum(QtyBySet),0)
				, QtyBySubprocess = isnull(sum(QtyBySubprocess),0)
				, InQty = min (InQtyBySet)
				, OutQty = min (OutQtyBySet)
				, InQtyByPcs = isnull(sum(InQtyByPcs),0)
				, OutQtyByPcs = isnull(sum(OutQtyByPcs),0)
				, FinishedQty = min (FinishedQtyBySet)
				, SubprocessId
		from (
			select a.*
			from dbo.[QtyBySetPerSubprocess_PatternPanel](@OrderID,@SubprocessID,@InStartDate,@InEndDate,@OutStartDate,@OutEndDate,@IsNeedCombinBundleGroup,1)a
		) minFabricPanelCode
		group by OrderID, SubprocessId, SizeCode, Article
	end
	
	-- Result Data --
	insert into @QtyBySetPerSubprocess
	select	OrderID
			, f.Article
			, f.Sizecode
			, QtyBySet
			, QtyBySubprocess
			, InQtyBySet = case when @IsMorethenOrderQty = 1 then f.InQty
							when f.InQty>oq.qty then oq.qty
							else f.InQty
							end
			, OutQtyBySet = case when @IsMorethenOrderQty = 1 then f.OutQty
							when f.OutQty>oq.qty then oq.qty
							else f.OutQty
							end
			, InQtyByPcs
			, OutQtyByPcs
			, FinishedQtyBySet = case when @IsMorethenOrderQty = 1 then f.FinishQty
							when f.FinishQty>oq.qty then oq.qty
							else f.FinishQty
							end
			, SubprocessId
	from @FinalQtyBySet f
	left join Order_Qty oq on oq.id = f.OrderID and oq.SizeCode = f.SizeCode and oq.Article = f.Article
	RETURN ;
END

