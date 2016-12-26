
Create Function [dbo].[GetUnitQty]
(
	  @UnitFrom		VarChar(8)		--來源單位
	 ,@UnitTo		VarChar(8)		--目的單位
	 ,@Qty			Numeric(15,4)	--需轉換單位的數量
)
Returns Numeric(15,4)
As
Begin
	Return (
		select iif( @UnitFrom = @UnitTo 
				, @Qty
				, @Qty * (select RateValue 
								  From dbo.Unit_Rate
								  Where UnitFrom = @UnitFrom
								     And UnitTo = @UnitTo)
							   ))
	/*

	Declare @TransQty Numeric(15,4);
	Declare @Rate VarChar(22);
	Declare @TransRate Numeric(15,6);
	
	Set @TransQty = @Qty;

	If @UnitFrom != @UnitTo	--單位不同時，才換算
	Begin
		Set @Rate = '0';
		Set @TransRate = 0;
		Select @Rate = Rate
		  From dbo.Unit_Rate
		 Where UnitFrom = @UnitFrom
		   And UnitTo = @UnitTo;
		
		If IsNull(@Rate, '') = ''
		Begin
			Set @Rate = '0';
		End;

		If CharIndex('/', @Rate) > 0
		Begin
			Declare @Length Int;				--字串長度
			Declare @Divide Int;				--'/'在文字中的位置
			Declare @Numerator Numeric(15,4);	--分子
			Declare @Denominator Numeric(15,4);	--分母

			Set @Length = Len(@Rate);
			Set @Divide = CharIndex('/', @Rate);
			
			Set @Numerator = Convert(Numeric(15,6), SubString(@Rate, 1, @Divide - 1));
			Set @Denominator = Convert(Numeric(15,6), SubString(@Rate, @Divide + 1, @Length - @Divide));

			Set @TransRate = @Numerator / @Denominator;
		End;
		Else
		Begin
			Set @TransRate = Convert(Numeric(15,6), @Rate);
		End;


		Set @TransQty = @Qty * @TransRate;
	End;

	Return @TransQty;
	
	*/
End