
CREATE FUNCTION [dbo].[getSewingOutputCumulateOfDays](@style VARCHAR(15), @sewingline VARCHAR(2), @outputdate DATE, @factory VARCHAR(8))
RETURNS varchar(max)
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @cumulate int --要回傳的字串

	DECLARE cursor_SewingData CURSOR FOR
	select distinct s.OutputDate
	from SewingOutput s
	inner join SewingOutput_Detail sd on s.ID = sd.ID
	left join Orders o on o.ID =  sd.OrderId
	left join MockupOrder mo on mo.ID = sd.OrderId
	where (o.StyleID = @style or mo.ID = @style)
	and s.SewingLineID = @sewingline
	and s.OutputDate < @outputdate
	and s.FactoryID = @factory
	order by s.OutputDate desc

	DECLARE @cursordate DATE, --暫存OutputDate
			@currentcountdate DATE,
			@exist INT,
			@countday INT,
			@recorddate DATE
	SET @cumulate = 1
	SET @currentcountdate = @outputdate
	SET @recorddate = @outputdate
	SET @exist = 0
	--開始run cursor
	OPEN cursor_SewingData
	--將第一筆資料填入變數
	FETCH NEXT FROM cursor_SewingData INTO @cursordate
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @cursordate <> DATEADD(day,-1,@currentcountdate)
			BEGIN

				DECLARE @i int,
						@max int,
						@hour numeric(3,1)
				SET @i = 1
				SET @max = DATEDIFF(day,@cursordate,@currentcountdate)
				WHILE(@i <= @max)
				BEGIN
					select @hour = Hours 
					from WorkHour
					where FactoryID = @factory
					and SewingLineID = @sewingline
					and Date = DATEADD(day,-@i,@currentcountdate) 
					IF @hour <> 0
						BEGIN
							IF DATEADD(day,-@i,@recorddate) <> @cursordate
								BEGIN
									SET @exist = 1
								END
							ELSE
								BEGIN
									SET @cumulate = @cumulate + 1
									SET @recorddate = DATEADD(day,-@i,@recorddate)
									SET @currentcountdate = @cursordate
									SET @hour = 0
								END
							BREAK
						END
					
					SET @i = @i + 1
				END
			END
		ELSE
			BEGIN
				SET @cumulate = @cumulate + 1
				SET @currentcountdate = @cursordate
				SET @recorddate = DATEADD(day,-1,@recorddate)
			END
		IF @exist = 1
			BREAK
	FETCH NEXT FROM cursor_SewingData INTO @cursordate
	END
	--關閉cursor與參數的關聯
	CLOSE cursor_SewingData
	--將cursor物件從記憶體移除
	DEALLOCATE cursor_SewingData

	RETURN @cumulate
END