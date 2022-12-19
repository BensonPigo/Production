
CREATE Function [dbo].[GetCPURate]
	(
	  @OrderTypeID	VarChar(20)
	 ,@ProgramID   	VarChar(12)
	 ,@Category		VarChar(1)
	 ,@BrandID		VarChar(8)
	 ,@TableType    VarChar(1)	--'S':Style; 'O':Order; 'M':MockOrder
	)
Returns @tmpCpuRate Table
	(
	 CpuRate Numeric(3,1)
	)
As
Begin
	Declare @CpuRate Numeric(3,1) = 0;
	Declare @SampleRate Numeric(2,0) = 0;

	Select Top 1 @SampleRate = System.SampleRate
	  From dbo.System;
	
	If (@TableType = 'M')	--固定一倍. 
	Begin
		Set @CpuRate = 1;
	End;
	Else
	Begin
		If IsNull(@ProgramID, '') != ''
		Begin
 			Select Top 1 @CpuRate = Program.RateCost
			  From Program WITH (NOLOCK)
			 Where ID = @ProgramID
			   And BrandID = @BrandID;
		End;

		If @CpuRate=0
		Begin
			Select Top 1 @CpuRate = OrderType.CpuRate
			  From OrderType WITH (NOLOCK)
			 Where ID = @OrderTypeID
			   And BrandID = @BrandID ;
		End;
 
 		If IsNull(@CpuRate, 0) = 0 And @Category = 'S'
		Begin
 			Set @CpuRate = @SampleRate;
		End;

		If @CpuRate = 0
		Begin
 			Set @CpuRate = 1;
		End;
	End;
	
	Insert Into @tmpCpuRate (CpuRate) Values (@CpuRate);
	Return;

End