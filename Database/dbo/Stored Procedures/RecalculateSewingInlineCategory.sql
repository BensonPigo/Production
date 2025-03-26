CREATE  PROCEDURE [dbo].[RecalculateSewingInlineCategory]
	@ID varchar(13)
AS
BEGIN 
	SET NOCOUNT ON; 

	if isnull(@ID, '') = ''
	begin
		return;
	end

	declare @StyleUkey bigint ,
		@SewingLineID varchar(3) ,
		@FactoryID varchar(5),
		@Team VARCHAR(3) =  null,
		@SewingDate date

	declare @InlineCategoryCumulate as int

	DECLARE cursorSewingOutput 
	CURSOR FOR
		select distinct o.StyleUkey, s.SewingLineID, s.FactoryID, s.Team, s.OutputDate	
		from SewingOutput s 
		inner join SewingOutput_Detail sd on s.ID = sd.ID
		inner join Orders o  on sd.Orderid = o.id
		where s.ID = @ID

	OPEN cursorSewingOutput
	FETCH NEXT FROM cursorSewingOutput INTO @StyleUkey, @SewingLineID, @FactoryID, @Team, @SewingDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
		select @InlineCategoryCumulate = dbo.[GetCheckContinusProduceDays] (@StyleUkey, @SewingLineID, @FactoryID, @Team, @SewingDate)
		
		-- 相同條件的 @StyleUkey, @SewingLineID, @FactoryID, @Team, @SewingDate 也需要更新
		update sd
			set sd.InlineCategoryCumulate = @InlineCategoryCumulate
		from SewingOutput_Detail sd
		where exists (select 1 from SewingOutput s where s.ID = sd.ID and s.SewingLineID = @SewingLineID and s.FactoryID = @FactoryID and s.Team = @Team and s.OutputDate = @SewingDate)
		and exists (select 1 from Orders o where sd.OrderId = o.ID and o.StyleUkey = @StyleUkey)
		 
		FETCH NEXT FROM cursorSewingOutput INTO @StyleUkey, @SewingLineID, @FactoryID, @Team, @SewingDate
	END


	CLOSE cursorSewingOutput
	DEALLOCATE cursorSewingOutput

END