
--Function ActualPerimeter
CREATE FUNCTION [dbo].[GetActualPerimeter]
(
	@ActualPerimeterYD VarChar(15)
)
RETURNS  float
AS
BEGIN
	Declare @ActualPerimeter float

	Declare @yd float = (SELECT data FROM SplitString(@ActualPerimeterYD,'Yd') where no = 1)
	Declare @in float, @inn float
	if(SELECT count(1) FROM SplitString(@ActualPerimeterYD,'Yd'))>1
	begin
		select @in = Data from SplitString((SELECT Data FROM SplitString(@ActualPerimeterYD,'Yd') where no = 2),'"') where no = 1
		if (select count(1) from SplitString((SELECT Data FROM SplitString(@ActualPerimeterYD,'Yd') where no = 2),'"') )>1
		begin
			select @inn = data from SplitString((SELECT Data FROM SplitString(@ActualPerimeterYD,'Yd') where no = 2),'"') where no = 2
		end
	end

	set @ActualPerimeter = (@yd* 0.9144) + (isnull(@in,0)*0.0254) + (isnull(@inn,0)*0.0254/32) 

	RETURN @ActualPerimeter
END