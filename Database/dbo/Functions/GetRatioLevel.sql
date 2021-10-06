CREATE Function [dbo].[GetRatioLevel]
(
   @StyleUkey	Bigint
   , @ShowName	Bit = 0 -- 只回傳Name
)
Returns VarChar(51)
As
Begin
	Declare @UseRatioRule varchar(51) = ''

	Select @UseRatioRule = iif(s.ThickFabricBulk = 0
			, Isnull(bt.UseRatioRule, b.UseRatioRule)
			, Isnull(bt.UseRatioRule_Thick, b.UseRatioRule_Thick))
	From Production.dbo.Style s
	Left join Production.dbo.Brand b on s.BrandID = b.ID
	Left join Production.dbo.Brand_ThreadCalculateRules bt on b.ID = bt.ID 
		And bt.FabricType = s.FabricType 
		And bt.ProgramID = s.ProgramID
	Where s.Ukey = @StyleUkey

	If @ShowName = 1
	Begin 
		Select @UseRatioRule = Name From Production.dbo.DropDownList 
		Where Type = 'UseRatioRule' And ID = @UseRatioRule
	End 

	Return @UseRatioRule
End
