﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	Get Stock Unit
-- =============================================
CREATE FUNCTION [dbo].[getStockUnit]
(
	-- Add the parameters for the function here
	@scirefno varchar(30),
	@suppid varchar(6) = ''
)
RETURNS varchar(8)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @stockunit varchar(8);
	DECLARE @mtltypeid varchar(20);
	declare @tmpunit varchar(8);
	declare @isExt bit;
	declare @extunit varchar(8);
	set @stockunit = '';

	-- Add the T-SQL statements to compute the return value here
	if(@suppid = '')
	begin
		SELECT @tmpunit=CASE B.OutputUnit WHEN 1 THEN A.UsageUnit WHEN 2 THEN C.POUnit END
		, @isExt = b.IsExtensionUnit
		FROM DBO.Fabric A WITH (NOLOCK) INNER JOIN DBO.MtlType B WITH (NOLOCK) ON A.MtlTypeID = B.ID 
		INNER JOIN DBO.Fabric_Supp C WITH (NOLOCK) ON A.SCIRefno = C.SCIRefno 
		WHERE A.SCIRefno=@scirefno;

		if(isnull(@tmpunit,'') = '')
		begin
			select top 1 @tmpunit = psd.POUnit,@isExt = m.IsExtensionUnit
			from PO_Supp_Detail psd with (nolock)
			inner join Fabric f with (nolock) on psd.SCIRefno = f.SCIRefno
			inner join MtlType m with (nolock) on f.MtlTypeID = m.ID
			where psd.SCIRefno = @scirefno;
		end
	end
	else
	begin
		SELECT @tmpunit=CASE B.OutputUnit WHEN 1 THEN A.UsageUnit WHEN 2 THEN C.POUnit END
		, @isExt = b.IsExtensionUnit
		FROM DBO.Fabric A WITH (NOLOCK) INNER JOIN DBO.MtlType B WITH (NOLOCK) ON A.MtlTypeID = B.ID 
		INNER JOIN DBO.Fabric_Supp C WITH (NOLOCK) ON A.SCIRefno = C.SCIRefno 
		WHERE A.SCIRefno=@scirefno AND C.SuppID = @suppid;
	end

	if @isExt = '1'
	begin
		select @extunit=unit.ExtensionUnit from dbo.Unit WITH (NOLOCK) where id = @tmpunit;

		if @extunit is null or @extunit=''
		begin
			return @tmpunit;
		end
		set @stockunit = @extunit;
	end

	if @isExt = '0'
	begin
		set @stockunit = @tmpunit;
	end
	
	-- Return the result of the function
	RETURN @stockunit
END