
Create Function [dbo].[CheckIsPullForward]
	(
	  @BuyerDelivery	Date
	 ,@SciDelivery		Date
	)
Returns Bit
As
Begin
	--Set NoCount On;
	Declare @IsPullForward Bit;
	
	Set @IsPullForward = 0;
	If DateDiff(Day, @SciDelivery, @BuyerDelivery) > 7
	Begin
		Set @IsPullForward = 1;
	End;

	Return @IsPullForward;
End