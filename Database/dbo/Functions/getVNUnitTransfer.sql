
CREATE FUNCTION [dbo].[getVNUnitTransfer](@type varchar(20), @oriunit varchar(8), @customsunit varchar(8), @oriqty numeric(10,2), @width numeric(3,1), @pcswidth numeric(7,4), @pcslength numeric(7,4), @pcskg numeric(7,4),@ratevalue numeric(28,18),@rate varchar(22))
RETURNS numeric(16,6)
BEGIN
	DECLARE @returnQty numeric(16,6) --要回傳的數值
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