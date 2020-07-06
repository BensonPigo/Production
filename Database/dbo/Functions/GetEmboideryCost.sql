-- =============================================
-- Author:		ISP20201044
-- Create date: 2020/07/03
-- Description:	複製Trade [GetEmboideryCost]
-- =============================================
CREATE FUNCTION [dbo].[GetEmboideryCost]
(	
	 @Region VARCHAR(4)
	,@Season VARCHAR(10)
	,@Stitches INT
	,@IncludeThread BIT
)
RETURNS DECIMAL(18, 3)
AS
BEGIN
	DECLARE @RtnValue DECIMAL(18, 9);
	DECLARE @BasedStitches INT;
	DECLARE @BasedPay DECIMAL(9, 4);
	DECLARE @AddedStitches DECIMAL(9, 0);
	DECLARE @AddedPay DECIMAL(18, 9);
	DECLARE @Ratio DECIMAL(5, 2);
	DECLARE @ThreadRatio DECIMAL(5, 2);

	SET @RtnValue = 0;
	SET @BasedStitches = 0;
	SET @BasedPay = 0;
	SET @AddedStitches = 0;
	SET @AddedPay = 0;
	SET @Ratio = 100;
	SET @ThreadRatio = 0;

	IF (@Stitches > 0)
	BEGIN
		SELECT TOP 1 @BasedStitches = BasedStitches,
					 @BasedPay = BasedPay,
					 @AddedStitches = AddedStitches,
					 @AddedPay = AddedPay,
					 @Ratio = Ratio,
					 @ThreadRatio = ThreadRatio
		FROM Production.dbo.FtyStdRate_EMB
		WHERE Region = @Region AND SeasonID = @Season AND @Stitches BETWEEN StartRange AND EndRange

		IF (@BasedPay = 0)
		BEGIN
			RETURN @RtnValue
		END

		IF @stitches > @BasedStitches
		BEGIN
			SET @RtnValue = @BasedPay + ((@stitches - @BasedStitches) / @AddedStitches) * @AddedPay
		END
		ELSE
		BEGIN
			SET @RtnValue = @BasedPay
		END

		-- 計算不含線的成本
		IF @IncludeThread <> 1
		BEGIN
			SET @RtnValue = @RtnValue * (1 - (@ThreadRatio / 100))
		END
	END

	RETURN ROUND(@RtnValue * (@Ratio / 100), 3);
END