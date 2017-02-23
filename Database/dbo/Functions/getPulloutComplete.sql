﻿
CREATE FUNCTION [dbo].[getPulloutComplete](@id varchar(13), @pulloutcomplete bit)
RETURNS varchar(3)
BEGIN
	DECLARE @string varchar(3) --要回傳的字串
	IF @pulloutcomplete = 1
		BEGIN
			SET @string = 'OK'
		END
	ELSE
		BEGIN
			Select @string = Count(Distinct ID) from Pullout_Detail WITH (NOLOCK) where OrderID = @id and ShipQty > 0
		END

	RETURN @string
END