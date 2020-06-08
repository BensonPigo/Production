

-- =============================================
-- Author:		Aaron
-- Create date: 2017/12/07
-- =============================================
CREATE FUNCTION [dbo].[P_GetOrderLocation_Rate]
(
	@OrderId varchar(13),@ComboType varchar(1),@ServerName varchar(20)
)
RETURNS NUMERIC(5,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Rate as NUMERIC(5,2);

	---------------------------------testing
	IF(@ServerName='[PMS\testing\ph1]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\ph1].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\ph1].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\ph1].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\ph2]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\ph2].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\ph2].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\ph2].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END

	IF(@ServerName='[PMS\testing\ESP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\ESP].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\ESP].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\ESP].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\SNP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\SNP].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\SNP].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SNP].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\SPT]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\SPT].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\SPT].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SPT].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\SPR]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\SPR].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\SPR].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SPR].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\SPS]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\SPS].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\SPS].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\SPS].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\HXG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\HXG].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\HXG].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\HXG].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\HZG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\HZG].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\HZG].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\HZG].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\testing\NAI]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\testing\NAI].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\testing\NAI].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\testing\NAI].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END

	
	---------------------------------pmsdb
	IF(@ServerName='[PMS\pmsdb\PH1]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\ph1].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\ph1].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\ph1].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\PH2]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\ph2].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\ph2].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\ph2].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END

	IF(@ServerName='[PMS\pmsdb\ESP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\ESP].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\ESP].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\ESP].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\SNP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\SNP].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\SNP].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SNP].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\SPT]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\SPT].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\SPT].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SPT].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\SPR]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\SPR].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\SPR].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SPR].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\SPS]')
	BEGIN
		 --Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\SPS].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\SPS].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\SPS].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\HXG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\HXG].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\HXG].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\HXG].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\HZG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\HZG].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\HZG].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\HZG].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END
	IF(@ServerName='[PMS\pmsdb\NAI]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		IF (select b.styleunit from [PMS\pmsdb\NAI].Production.dbo.orders a WITH (NOLOCK) inner join [PMS\pmsdb\NAI].Production.dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
		Begin
			set @Rate = 100
		End
		Else
		Begin
			select @Rate = Rate from [PMS\pmsdb\NAI].Production.dbo.Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
		End
		-- Return the result of the function
	
	END

	RETURN @Rate

END