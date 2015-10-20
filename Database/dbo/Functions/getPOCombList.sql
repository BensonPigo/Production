
CREATE FUNCTION [dbo].[getPOCombList](@orderid varchar(13), @poid varchar(13))
RETURNS varchar(max)
BEGIN
	DECLARE @string nvarchar(max) --要回傳的字串
	IF @orderid <> @poid
		BEGIN
			SET @string = 'Master:'+@poid
		END
	ELSE

		BEGIN
			DECLARE cursor_Orders CURSOR FOR
			SELECT ID FROM Orders WHERE POID = @poid and ID <> @poid

			DECLARE @id varchar(13), --暫存Orders Id
					@left10id varchar(10),  --暫存Orders.Id的前10碼
					@tmpstring varchar(13)  --暫存要寫入的資料
			SET @string = @poid;
			SET @left10id = LEFT(@poid,10);
			--開始run cursor
			OPEN cursor_Orders
			--將第一筆資料填入變數
			FETCH NEXT FROM cursor_Orders INTO @id
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF(@left10id = LEFT(@id,10))
					SET @tmpstring = RIGHT(@id,3)
				ELSE
					BEGIN
						SET @tmpstring = @id
						SET @left10id = LEFT(@id,10)
					END
				SET @string = @string + '/' + @tmpstring
			FETCH NEXT FROM cursor_Orders INTO @id
			END
			--關閉cursor與參數的關聯
			CLOSE cursor_Orders
			--將cursor物件從記憶體移除
			DEALLOCATE cursor_Orders
		END

	RETURN @string
END