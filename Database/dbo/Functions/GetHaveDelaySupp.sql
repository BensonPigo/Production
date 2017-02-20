

CREATE FUNCTION [dbo].[GetHaveDelaySupp]
(
	@poid varchar(13)
)
RETURNS bit
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	Declare @mtldelay date,
			@returnvalue bit;
	select @mtldelay = MTLDelay from PO where ID = @poid
	IF @mtldelay is null
		BEGIN
			select @mtldelay = a.Delay 
			from (select ps.SuppID,s.Delay from PO_Supp ps
	  			  left join Supp s on ps.SuppID = s.ID and s.Delay is not null
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