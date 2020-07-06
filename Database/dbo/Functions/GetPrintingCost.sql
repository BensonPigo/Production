-- =============================================
-- Author:		ISP20201044
-- Create date: 2020/07/03
-- Description:	複製Trade [GetPrintingCost]
-- =============================================
CREATE FUNCTION [dbo].[GetPrintingCost]
(
	 @Region VARCHAR(4)
	,@Season VARCHAR(10)
	,@InkType VARCHAR(50)
	,@Colors  VARCHAR(100)
	,@Length DECIMAL(5, 1)
	,@Width DECIMAL(5, 1)
	,@AntiMigration BIT
)
RETURNS @Result TABLE
(
	Cost DECIMAL(8, 3),
	CostWithRatio DECIMAL(8, 3)
)
AS
BEGIN
	DECLARE @RtnValue DECIMAL(8, 3);
	DECLARE @MaxEdge DECIMAL(5, 1);
	DECLARE @MinEdge DECIMAL(5, 1);
	DECLARE @Surcharge DECIMAL(5, 2);
	DECLARE @Ratio DECIMAL(5, 2);
	SET @RtnValue = 0;
	SET @MaxEdge = 0;
	SET @MinEdge = 0;
	SET @Surcharge = 0;
	SET @Ratio = 100;

	-- 前後級距面積
	DECLARE @FormalArea DECIMAL(12, 2);
	DECLARE @NextArea DECIMAL(12, 2);
	DECLARE @FormalArea_Triple DECIMAL(12, 2);
	DECLARE @NextArea_Triple DECIMAL(12, 2);
	SET @FormalArea = 0;
	SET @NextArea = 0;
	SET @FormalArea_Triple = 0;
	SET @NextArea_Triple = 0;

	-- 前後級距價格
	DECLARE @FormalPrice DECIMAL(6, 3);
	DECLARE @NextPrice DECIMAL(6, 3);
	DECLARE @FormalPrice_Triple DECIMAL(6, 3);
	DECLARE @NextPrice_Triple DECIMAL(6, 3);
	SET @FormalPrice = 0;
	SET @NextPrice = 0;
	SET @FormalPrice_Triple = 0;
	SET @NextPrice_Triple = 0;


	-- 取得邊長孰大小
	IF (@Length > @Width)
	BEGIN
		SET @MaxEdge = @Length
		SET @MinEdge = @Width
	END
	ELSE
	BEGIN 
		SET @MaxEdge = @Width
		SET @MinEdge = @Length
	END

	-- 取得Surcharge & Profit
	SELECT TOP 1 @Surcharge = Surcharge, @Ratio = Ratio
	FROM  Production.dbo.FtyStdRate_PRT
	WHERE Region = @Region AND SeasonID = @Season AND InkType = @InkType AND Colors = @Colors

	-- 取得次一級面積價格
	SELECT TOP 1 @FormalArea = Area, @FormalPrice = Price
	FROM  Production.dbo.FtyStdRate_PRT
	WHERE Region = @Region AND SeasonID = @Season AND InkType = @InkType AND Colors = @Colors AND Area <=  (@Length * @Width)
	ORDER BY Area DESC

	-- 取得高一級面積價格
	SELECT TOP 1 @NextArea = Area, @NextPrice = Price
	FROM Production.dbo.FtyStdRate_PRT
	WHERE Region = @Region AND SeasonID = @Season AND InkType = @InkType AND Colors = @Colors AND Area >=  (@Length * @Width)
	ORDER BY Area ASC

	-- 若無高一級面積價格,回傳0
	IF (@NextPrice = 0)
	BEGIN
		SET @RtnValue = 0
		INSERT INTO @Result (Cost, CostWithRatio) VALUES (0, 0)
		RETURN
	END

	IF (@FormalArea != 0)
	BEGIN
		-- 計算平均Cost
		IF (@FormalArea <> @NextArea)
		BEGIN
			SET @RtnValue = @FormalPrice + ((@Length * @Width ) - @FormalArea) * ((@NextPrice - @FormalPrice) / (@NextArea - @FormalArea))
		END
		ELSE
		BEGIN
			SET @RtnValue = @FormalPrice
		END

		-- 長寬差距大於3倍
		IF (@MaxEdge >= @MinEdge * 3) 
		BEGIN
			-- 取得低一級面積價格
			SELECT TOP 1 @FormalArea_Triple = Area, @FormalPrice_Triple = Price
			FROM  Production.dbo.FtyStdRate_PRT
			WHERE Region = @Region AND SeasonID = @Season AND InkType = @InkType AND Colors = @Colors AND Area <= (@MaxEdge * @MaxEdge)
			ORDER BY Area DESC

			-- 取得高一級面積價格
			SELECT TOP 1 @NextArea_Triple = Area, @NextPrice_Triple = Price
			FROM Production.dbo.FtyStdRate_PRT
			WHERE Region = @Region AND SeasonID = @Season AND InkType = @InkType AND Colors = @Colors AND Area >= (@MaxEdge * @MaxEdge)
			ORDER BY Area ASC

			-- 若無高一級面積價格,回傳0
			IF (@NextPrice_Triple = 0)
			BEGIN
				SET @RtnValue = 0
				INSERT INTO @Result (Cost, CostWithRatio) VALUES (0, 0)
				RETURN
			END

			-- 計算平均Cost
			IF (@FormalArea_Triple <> @NextArea_Triple)
			BEGIN
				SET @RtnValue = (@RtnValue + (@FormalPrice_Triple + ((@MaxEdge * @MaxEdge ) - @FormalArea_Triple) * ((@NextPrice_Triple - @FormalPrice_Triple) / (@NextArea_Triple - @FormalArea_Triple))))/2
			END
			ELSE
			BEGIN
				SET @RtnValue = (@RtnValue + @FormalPrice_Triple) / 2
			END
		END
	END
	ELSE
	BEGIN
		-- 若無次一級面積(面積小於最低級), 以最低級面積價格為預設
		SET @RtnValue = @NextPrice
	END

	-- Anti-Migration
	IF (@AntiMigration = 1)
	BEGIN
		SET @RtnValue = @RtnValue * 1.1
	END

	--Surcharge
	--SET @RtnValue = @RtnValue * (1 + (@Surcharge / 100));

	INSERT INTO @Result (Cost, CostWithRatio) VALUES (ROUND(@RtnValue, 3), ROUND(@RtnValue * (@Ratio / 100), 3));

	RETURN; 
END