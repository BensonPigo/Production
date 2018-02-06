
CREATE PROCEDURE [dbo].[usp_closeOrder]
@poid varchar(13), @type varchar(1)
AS
IF @type = '1'
	BEGIN
		update ChgOver set Status = 'Closed' where OrderID in (select ID from [dbo].Orders WITH (NOLOCK) where POID = @poid);
		update Orders set Finished = 1,GMTClose = GETDATE(),MDClose = iif(MDClose is null,GETDATE(),Orders.MDClose) where ID in (select ID from [dbo].Orders WITH (NOLOCK) where POID = @poid);
		update cutting set Finished = 1 where ID = @poid;
	END
ELSE
	IF @type = '2'
		BEGIN
			update ChgOver set Status = iif(ApvDate is null,'New','Approved') where OrderID in (select ID from [dbo].Orders WITH (NOLOCK) where POID = @poid);
			update Orders set Finished = 0,GMTClose = null,MDClose = null where ID in (select ID from [dbo].Orders WITH (NOLOCK) where POID = @poid);
			update cutting set Finished = 0 where ID = @poid;
		END