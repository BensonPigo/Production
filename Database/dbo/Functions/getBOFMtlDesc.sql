
CREATE FUNCTION [dbo].[getBOFMtlDesc](@styleukey bigint)
RETURNS nvarchar(max)
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @string nvarchar(max),--要回傳的字串
			@desc nvarchar(max) --暫存Fabric Description
	DECLARE cursor_FabDesc CURSOR FOR
	select distinct '#'+f.Refno+' '+f.Description as Description
	from Style_BOF sb
	inner join Fabric f on sb.SCIRefno = f.SCIRefno
	where sb.Kind = '1' and sb.StyleUkey = @styleukey

	SET @string = '';
	--開始run cursor
	OPEN cursor_FabDesc
	--將第一筆資料填入變數
	FETCH NEXT FROM cursor_FabDesc INTO @desc
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @string = ''
			SET @string = @desc
		ELSE
			SET @string = @string + CHAR(13) + CHAR(10) +@desc
	FETCH NEXT FROM cursor_FabDesc INTO @desc
	END
	--關閉cursor與參數的關聯
	CLOSE cursor_FabDesc
	--將cursor物件從記憶體移除
	DEALLOCATE cursor_FabDesc

	RETURN @string
END