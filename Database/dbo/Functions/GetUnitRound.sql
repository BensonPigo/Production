
Create Function [dbo].[GetUnitRound]
	(
	  @BrandID		VarChar(8)		--
	 ,@ProgramID	VarChar(12)		--
	 ,@Category		VarChar(1)		--
	 ,@UnitID		VarChar(8)		--
	)
Returns @tmpRound Table
	(
	  UnitRound		Numeric(2,0)
	 ,UsageRound	Numeric(2,0)	--for PO轉單用
	 ,RoundStep		Numeric(4,2)
	)
As
Begin
	Declare @UnitRound Numeric(2,0);
	Declare @UsageRound Numeric(2,0);
	Declare @RoundStep Numeric(4,2);
	Declare @IsMiAdidas Bit;

	Set @UnitRound = 0;

	Set @IsMiAdidas = 0;
	Select @IsMiAdidas = IsNull(Program.MiAdidas, 0)
	  From dbo.Program
	 Where Program.BrandID = @BrandID
	   And Program.ID = @ProgramID
	   And @Category = 'B';
	
	Select @UnitRound = IIF(@IsMiAdidas = 1, MiAdidasRound, [Round])
		 , @UsageRound = IIF(@IsMiAdidas = 1, MiAdidasRound, 1)
		 , @RoundStep = IIF(@IsMiAdidas = 1, 0, RoundStep)
	  From dbo.Unit
	 Where ID = @UnitID;
	
	Insert Into @tmpRound (UnitRound, UsageRound, RoundStep) Values (@UnitRound, @UsageRound, @RoundStep);
	Return;
End