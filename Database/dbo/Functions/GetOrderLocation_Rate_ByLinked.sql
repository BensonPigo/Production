CREATE FUNCTION [dbo].[GetOrderLocation_Rate_ByLinked]
(
	@OrderId varchar(13)
	,@ComboType varchar(1)
	,@MDivisionID varchar(8)
)
RETURNS NUMERIC(5,2)
AS
BEGIN 
	-- Declare the return variable here
	DECLARE @Rate as NUMERIC(5,2);  

	declare @styleunit as varchar(50) 
	if @MDivisionID = 'PM1' OR @MDivisionID = 'PM2' OR  @MDivisionID = 'PM4' 
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\PH1].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\PH1].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\PH1].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\PH1].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'PM3'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\PH2].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\PH2].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\PH2].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\PH2].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'VM2'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\ESP].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\ESP].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\ESP].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\ESP].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'VM3'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\SNP].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\SNP].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\SNP].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\SNP].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'VM1'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\SPT].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\SPT].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\SPT].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\SPT].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'KM1'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\SPR].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\SPR].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\SPR].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\SPR].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'KM2'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\SPS].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\SPS].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\SPS].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\SPS].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'CM2'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\HZG].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\HZG].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\HZG].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\HZG].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'CM1'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\HXG].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\HXG].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\HXG].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\HXG].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end
	else if @MDivisionID = 'CM3'
	begin
		select @styleunit = b.styleunit 
		from [PMS\pmsdb\SWR].[Production].dbo.orders a WITH (NOLOCK) 
		inner join [PMS\pmsdb\SWR].[Production].dbo.style b WITH (NOLOCK) on  b.ukey = a.StyleUkey 
		where a.id = @OrderId and a.MDivisionID = @MDivisionID

		select @Rate = Rate
		from [PMS\pmsdb\SWR].[Production].dbo.Order_Location ol With(nolock) 
		inner join [PMS\pmsdb\SWR].[Production].dbo.Orders o With(nolock) on ol.OrderId = o.ID		
		where ol.OrderId =@OrderId and ol.Location = @ComboType and o.MDivisionID = @MDivisionID 
	end

	IF @styleunit = 'PCS'
	Begin
		set @Rate = 100
	End 

	-- Return the result of the function
	RETURN @Rate

END