


-- 建立 FUNCTION
CREATE FUNCTION [dbo].[getItemDesc](@category varchar(20),@refno varchar(21)) 
RETURNS nvarchar(max)  -- 回傳Description
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @desc nvarchar(max); -- 暫存localitem description
	
    SET @desc = ''

	select @desc=description from localitem p WHERE  RefNo = @refno;

    RETURN rtrim(@desc)
END