



-- =============================================
-- Author:		<Ray.Chiou>
-- Create date: <2016/06/13>
-- Description:	<指定訂單編號、顏色組、尺寸別取得訂單單價>
-- =============================================
CREATE FUNCTION [dbo].[P_GetPoPriceByArticleSize]
(
	@ID varchar(13),
	@Article Varchar(8),
	@SizeCode varchar(8),
	@ServerName varchar(20)
)
RETURNS  numeric(8,3)
AS
BEGIN


	-- Declare the return variable here
	Declare @DefaultPrice  numeric(8,3) 
	Declare @Return  numeric(8,3)

	IF(@ServerName='[PMS\testing\PH1]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\PH1].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\PH1].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\PH2]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\PH2].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\PH2].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\ESP]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\ESP].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\ESP].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\SNP]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\SNP].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\SNP].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\SPT]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\SPT].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\SPT].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\SPR]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\SPR].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\SPR].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\SPS]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\SPS].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\SPS].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\HXG]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\HXG].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\HXG].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\HZG]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\HZG].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\HZG].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\testing\NAI]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\testing\NAI].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\testing\NAI].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END


	---PMSDB
	
	IF(@ServerName='[PMS\pmsdb\PH1]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\PH1].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\PH1].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\PH2]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\PH2].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\PH2].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\ESP]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\ESP].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\ESP].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\SNP]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\SNP].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\SNP].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\SPT]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\SPT].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\SPT].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\SPR]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\SPR].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\SPR].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\SPS]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\SPS].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\SPS].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\HXG]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\HXG].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\HXG].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\HZG]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\HZG].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\HZG].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END
	
	IF(@ServerName='[PMS\pmsdb\NAI]')
	BEGIN
		Set @DefaultPrice = isnull((select Poprice from [PMS\pmsdb\NAI].Production.dbo.Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

		select @Return = Poprice from [PMS\pmsdb\NAI].Production.dbo.Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
	END


	set @Return = isnull(@Return,@DefaultPrice)
	Return @Return 



END