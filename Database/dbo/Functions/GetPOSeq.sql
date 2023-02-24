-- =============================================
-- Author:		Jeff
-- Create date: 2023/02/16
-- Description:	From trade 只取需要部份 for < WH P01 Material Compare >
-- =============================================
Create Function [dbo].[GetPOSeq]
(
	 @Seq1		VarChar(3)
	,@Seq2Count	Numeric(4,0)
)
Returns @NewSeq Table
	(
	   Seq1 VarChar(3)
	 , Seq2 VarChar(2)
	)
As
Begin
	Set @Seq2Count = IIF(IsNull(@Seq2Count, 0) = 0, 1, @Seq2Count);

	Declare @Seq1_New VarChar(3);
	Declare @Seq2_New VarChar(3);

	Declare @Max_Seq Int;
	Declare @Seq_Quotient Int;
	Declare @Seq_Remainder Int;
	Set @Max_Seq = 99;

	Set @Seq_Quotient = Floor(@Seq2Count / @Max_Seq);
	Set @Seq_Remainder = @Seq2Count % @Max_Seq;

	If @Seq_Remainder = 0
	Begin
		Set @Seq_Quotient -= 1;
		Set @Seq_Remainder = 99;
	End;

	--當為ECFA項次，大項第三碼為1-9遞增
	--一般項次，大項第三碼為A-Z遞增
	Set @Seq1_New = IIF(  @Seq_Quotient = 0
						, @Seq1
						, Left(@Seq1, 2) + IIF(Len(@Seq1) = 3, IIF(ASCII(Right(@Seq1, 1)) > 64, Char(ASCII(Right(@Seq1, 1)) + @Seq_Quotient)
																							  , Char(48 + Right(@Seq1, 1) + @Seq_Quotient)
																  )
															 , Char(64 + @Seq_Quotient)
											  )
					   );
	Set @Seq2_New = Format(IIF(@Seq_Remainder = 0, @Max_Seq, @Seq_Remainder), '00')

	Insert Into @NewSeq (Seq1, Seq2) Values (@Seq1_New, @Seq2_New);

	-------------------------------------------
	-- Return the result of the function
	Return;
End