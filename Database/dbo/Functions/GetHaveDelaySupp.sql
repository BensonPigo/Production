﻿

CREATE FUNCTION [dbo].[GetHaveDelaySupp]
(
	@poid varchar(13)
)
RETURNS bit
AS
BEGIN
	Declare @mtldelay date,
			@returnvalue bit;
	select @mtldelay = MTLDelay from PO WITH (NOLOCK) where ID = @poid
	IF @mtldelay is null
		BEGIN
			select @mtldelay = a.Delay 
			from (select ps.SuppID,s.Delay from PO_Supp ps WITH (NOLOCK)
	  			  left join Supp s WITH (NOLOCK) on ps.SuppID = s.ID and s.Delay is not null
				  where ps.ID = @poid) a
			--where 

			IF @mtldelay is null
				SET @returnvalue = 0
			ELSE
				SET @returnvalue = 1
		END
	ELSE
		SET @returnvalue = 1

	return @returnvalue

END