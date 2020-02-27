
Create Function [dbo].GetOrderQtyUpperlimit
(
	@OrderID varchar(13),
	@Article varchar(8),
	@Size varchar(8)
)
Returns int
As
Begin
	Declare @return int;
	
	select @return = OrderQty.OrderQtyUpperlimit --加上比例後的上限數量
	from Order_Qty oq with(Nolock)
	inner join orders o with(Nolock) on o.ID = oq.ID
	outer apply(
		select value=1
		from Order_TmsCost ot with(nolock)
		where ot.ArtworkTypeID = 'Garment Dye' 
		and ot.Price > 0
		and ot.id=oq.id
		and o.LocalOrder<>1
	)b
	outer apply(select OrderQtyUpperlimit=iif(b.value is not null,round(cast(oq.Qty as decimal) * (1 + isnull(o.DyeingLoss, 0) / 100), 0), oq.Qty)) OrderQty
	where oq.id = @OrderID
	and oq.Article = @Article
	and oq.SizeCode = @Size

	Return @return;
End