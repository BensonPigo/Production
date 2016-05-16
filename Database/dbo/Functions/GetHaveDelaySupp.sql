

CREATE FUNCTION [dbo].[GetHaveDelaySupp]
(
	@poid varchar(13)
)
RETURNS bit
AS
BEGIN
	Declare @mtldelay date,
			@returnvalue bit;
	select @mtldelay = MTLDelay from PO where ID = @poid
	IF @mtldelay is null
		BEGIN
			select @mtldelay = a.Delay 
			from (select ps.SuppID,s.Delay from PO_Supp ps
	  			  left join Supp s on ps.SuppID = s.ID
				  where ps.ID = @poid) a
			where a.Delay is not null

			IF @mtldelay is null
				SET @returnvalue = 0
			ELSE
				SET @returnvalue = 1
		END
	ELSE
		SET @returnvalue = 1

	return @returnvalue

END