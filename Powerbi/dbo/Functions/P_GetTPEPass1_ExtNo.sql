


-- ============================
-- Author:		<JEFF.YEH>
-- Create date: <2017/03/25>
-- Description:	<TPEPASS1的id+Name+ExtNo>
-- =============================================
CREATE FUNCTION [dbo].[P_GetTPEPass1_ExtNo]
(
	-- Add the parameters for the function here
	@id as varchar(10),
	@ServerName varchar(20)
)
RETURNS varchar(45)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @rtn as varchar(45);

	
	IF(@ServerName='[PMS\testing\PH1]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\PH1].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\PH2]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\PH2].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\ESP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\ESP].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\SNP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\SNP].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\SPT]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\SPT].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\SPR]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\SPR].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\SPS]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\SPS].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\HXG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\HXG].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\HZG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\HZG].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\testing\NAI]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\testing\NAI].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	----PMSDB
	
	IF(@ServerName='[PMS\pmsdb\PH1]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\PH1].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\PH2]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\PH2].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\ESP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\ESP].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\SNP]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\SNP].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\SPT]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\SPT].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\SPR]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\SPR].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\SPS]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\SPS].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\HXG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\HXG].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\HZG]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\HZG].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	
	IF(@ServerName='[PMS\pmsdb\NAI]')
	BEGIN
		-- Add the T-SQL statements to compute the return value here
		SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
		from [PMS\pmsdb\NAI].Production.dbo.TPEPASS1 a WITH (NOLOCK) where a.ID = @id;
	END
	-- Return the result of the function
	RETURN @rtn;
END