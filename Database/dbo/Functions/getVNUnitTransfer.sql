

CREATE FUNCTION [dbo].[getVNUnitTransfer](@type varchar(20), @oriunit varchar(8), @customsunit varchar(8), @oriqty numeric(13,4), @width numeric(5,2), @pcswidth numeric(5,2), @pcslength numeric(7,4), @pcskg numeric(7,4),@inputRatevalue numeric(28,18),@rate varchar(22), @Refno varchar(21) = '')
RETURNS numeric(16,6)
BEGIN
	DECLARE @returnQty numeric(16,6) --要回傳的數值
	Declare @ratevalue numeric(28,18)

	if(@type = 'SP_THREAD' and @Refno <> '')
	begin
		select @ratevalue = MeterToCone
		from LocalItem with (nolock) where Refno = @Refno
	end
	else
	begin
		set @ratevalue = @inputRatevalue
	end

	SET @returnQty = @oriqty
	IF @oriunit <> @customsunit
		BEGIN
			IF @type = 'F'
				BEGIN
					IF @customsunit = 'M2'
						BEGIN
							SET @returnQty = (@oriqty*@ratevalue)*(@width*0.0254)
						END
					ELSE
						BEGIN
							SET @returnQty = @oriqty*@ratevalue
						END
				END
			ELSE
				BEGIN
					IF @customsunit = 'M2'
						BEGIN
							IF @rate = ''
								BEGIN
									IF @pcslength <> 0
										BEGIN
											SET @returnQty = @oriqty*@pcslength*@pcswidth
										END
								END
							ELSE
								BEGIN
									SET @returnQty = @oriqty*@ratevalue*@pcswidth
								END
						END
					ELSE IF @customsunit = 'M'
						BEGIN
							IF @rate = ''
								BEGIN
									IF @pcslength <> 0
										BEGIN
											SET @returnQty = @oriqty*@pcslength
										END
								END
							ELSE
								BEGIN
									SET @returnQty = @oriqty*@ratevalue
								END
						END
					ELSE IF @customsunit = 'KGS' and @oriunit = 'PCS'
						BEGIN
							SET @returnQty = @oriqty*@pcskg
						END
					ELSE
						BEGIN
							SET @returnQty = @oriqty*@ratevalue
						END
				END
		END
	RETURN @returnQty
END