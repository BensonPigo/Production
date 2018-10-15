
CREATE Function [dbo].[GetHalfMonWithYear]
(
	  @Date			Date,
	  @isSCIDelivery	bit
)
Returns VarChar(7)
As
Begin

	--Declare @HalfMon VarChar(7);
	--Declare @Year Int;
	--Declare @Month Int;
	--Declare @Day Int;

	--Set @Year = DatePart(YYYY, @Date);
	--Set @Month = DatePart(mm, @Date);
	--Set @Day = DatePart(dd, @Date);

	--If @Day Between 8 And 22
	--Begin
	--	Set @HalfMon = Format(@Year,'0000') + Format(@Month, '00') + '1'; --當月
	--End;
	--Else
	--Begin
	--	If @Day Between 1 And 7
	--	Begin			
	--		set @Date = dateadd(month,-1,@Date);
	--		Set @Month = DatePart(mm, @Date);
	--		Set @Year = DatePart(YYYY, @Date);
	--	End;
	--	Set @HalfMon = Format(@Year,'0000') + Format(@Month, '00') + '2'; --上下半月
	--End;
declare @res VarChar(7) = ''

	if (@isSCIDelivery = 1)
	begin
		set @res = format(DATEADD(day,-7,@Date),'yyyyMM') + iif(DATEPART(day,@Date) between 8 and 22 , '1', '2');
	end
	else
	begin
		set @res = format(@Date,'yyyyMM') + iif(DATEPART(day,@Date) between 1 and 15 , '1', '2');
	end	

	return @res
End