-- =============================================
-- Author:		Jeff
-- Create date: 2017/10/24
-- =============================================
CREATE FUNCTION [dbo].[P_GetStyleLocation_Rate]
(
	@StyleUkey BIGINT,@ComboType varchar(1),@ServerName varchar(20)
)
RETURNS NUMERIC(5,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Rate as NUMERIC(5,2);
	
	IF(@ServerName='[PMS\testing\ph1]')
	BEGIN
		IF (select styleunit from [PMS\testing\ph1].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\ph1].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\ph2]')
	BEGIN
		IF (select styleunit from [PMS\testing\ph2].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\ph2].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\ESP]')
	BEGIN
		IF (select styleunit from [PMS\testing\ESP].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\ESP].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END
	IF(@ServerName='[PMS\testing\SNP]')
	BEGIN
		IF (select styleunit from [PMS\testing\SNP].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SNP].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\SPT]')
	BEGIN
		IF (select styleunit from [PMS\testing\SPT].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SPT].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\SPR]')
	BEGIN
		IF (select styleunit from [PMS\testing\SPR].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SPR].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\SPS]')
	BEGIN
		IF (select styleunit from [PMS\testing\SPS].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SPS].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END
	IF(@ServerName='[PMS\testing\HXG]')
	BEGIN
		IF (select styleunit from [PMS\testing\HXG].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\HXG].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\HZG]')
	BEGIN
		IF (select styleunit from [PMS\testing\HZG].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\HZG].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\testing\NAI]')
	BEGIN
		IF (select styleunit from [PMS\testing\NAI].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\NAI].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	
	---------------------------------pmsdb
	IF(@ServerName='[PMS\pmsdb\ph1]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\ph1].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\ph1].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\pmsdb\ph2]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\ph2].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\ph2].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\pmsdb\ESP]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\ESP].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\ESP].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\pmsdb\SNP]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\SNP].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SNP].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\pmsdb\SPT]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\SPT].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SPT].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END
	IF(@ServerName='[PMS\pmsdb\SPR]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\SPR].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SPR].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END
	IF(@ServerName='[PMS\pmsdb\SPS]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\SPS].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SPS].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END
	IF(@ServerName='[PMS\pmsdb\HXG]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\HXG].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\HXG].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	IF(@ServerName='[PMS\pmsdb\HZG]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\HZG].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\HZG].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END
	IF(@ServerName='[PMS\pmsdb\NAI]')
	BEGIN
		IF (select styleunit from [PMS\pmsdb\NAI].Production.dbo.style where ukey = @StyleUkey)  = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\NAI].Production.dbo.Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
		End	
	END

	RETURN @Rate

END