-- =============================================
-- Author:		<Ray>
-- Create date: <2016/07/05>
-- Description:	<指定ComboPcs,ComboType取得套裝配比>
-- =============================================
CREATE FUNCTION [dbo].[GetSuitRate]
	(
	 @ComboPcs		numeric(1,0),
	 @ComboType		varchar(1)
	)
RETURNS numeric(3,1)
AS
BEGIN
Declare @Return numeric(3,1) = 1

if @ComboPcs = 2  
Begin 
	Set @Return = iif(@ComboType= 'T',0.6,iif(@ComboType= 'B',0.4,@Return))
end

if @ComboPcs = 3  
Begin 
	Set @Return = iif(@ComboType= 'T',0.4,iif(@ComboType= 'B',0.3,iif(@ComboType= 'I',0.3,@Return)))
end

if @ComboPcs = 4  
Begin 
	Set @Return = iif(@ComboType= 'T',0.3,iif(@ComboType= 'B',0.3,iif(@ComboType= 'I',0.2,iif(@ComboType= '0',0.2,@Return))))
end

RETURN @Return
END


GO

