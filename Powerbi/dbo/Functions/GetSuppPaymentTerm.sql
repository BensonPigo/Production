
Create Function [dbo].[GetSuppPaymentTerm]
(		
	@BrandID	VarChar(8)
	, @SuppID	VarChar(6)
	, @Category VarChar(1)
)
Returns VarChar(5)
As
Begin
	Declare @PaymentTermAPID VarChar(5) = ''

	Select @PaymentTermAPID = 
			Case When @Category in ('S', 'T') Then PayTermAPIDSample
			When @Category in ('B', 'M') Then  PayTermAPIDBulk
			Else ''
			End 
	From Trade.dbo.Supp_PayTermAp
	Where ID = @SuppID And BrandID = @BrandID 

	If Isnull(@PaymentTermAPID, '') = ''
	Begin 
		Select @PaymentTermAPID = 
			Case When @Category in ('S', 'T') Then PayTermAPIDSample
			When @Category in ('B', 'M') Then  PayTermAPIDBulk
			Else ''
			End
		From Trade.dbo.Supp
		Where ID = @SuppID
	End

	Return @PaymentTermAPID;
End