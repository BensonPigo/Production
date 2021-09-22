
Create Function dbo.GetGMTLT
(
	  @BrandID	    VarChar(8)				--
	 ,@StyleID 	    VarChar(20)				--
	 ,@SeasonId		VarChar(10)				--
	 ,@Facotry		VarChar(8)				--
	 ,@OrderID      VarChar(13)             --
)
Returns numeric(3, 0)
As
 
Begin
  
   Declare @GMTLT Numeric(3,0)
   Declare @Cnt Numeric(2,0)
   Declare @isFty bit

   Set @GMTLT = 0;

   
	Select @Cnt = Count(distinct sa.GarmentLT)
	from Orders o
	left join Order_Qty oq on o.ID = oq.ID
	left join Style_Article sa on o.StyleUkey = sa.StyleUkey and oq.Article = sa.Article
	where oq.Qty > 0
	and o.ID = @OrderID

	if (@BrandID not in ('Adidas', 'Reebok') or @Cnt != 1)
	Begin
		select @GMTLT = dbo.GetStyleGMTLT(@BrandID, @StyleID, @SeasonId, @Facotry)
	End;
	ELSE
	Begin
		select @GMTLT = sa.GarmentLT
		from Orders o
		left join Order_Qty oq on o.ID = oq.ID
		left join Style_Article sa on o.StyleUkey = sa.StyleUkey and oq.Article = sa.Article
		where oq.Qty > 0
		and o.ID = @OrderID

		if (@GMTLT = 0)
		Begin
			select @GMTLT = dbo.GetStyleGMTLT(@BrandID, @StyleID, @SeasonId, @Facotry)
		End;
	End;
	 	
    return @GMTLT
End