
Create Function [dbo].[GetCeiling]
(
	 @Value		Numeric(15,4)	--傳入值
	,@Round		Int				--無條件進位至此設定的小數位數(0=整數)
	,@Step		Numeric(18,4)	--若傳入值不等於0時，依此條件值做無條件進位
)
Returns Numeric(15,4)
As
Begin
	Declare @CeilingValue Numeric(15,4);

	Set @CeilingValue = 0;
	Set @Step = IsNull(@Step, 0);

	If @Value != 0
	Begin
		If @Step != 0
		Begin
			Set @CeilingValue = Ceiling(@Value / @Step) * @Step;
		End;
		Else
		Begin
			Set @Round = Power(10, @Round);		--取10的N次方
			Set @CeilingValue = Ceiling(@Value * @Round) / @Round;
		End;
	End;

	Return @CeilingValue;
End