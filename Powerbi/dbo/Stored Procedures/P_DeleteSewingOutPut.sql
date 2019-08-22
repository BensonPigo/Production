CREATE PROCEDURE [dbo].[P_DeleteSewingOutPut]
	 @OutputDate date
AS
BEGIN

	delete from P_SewingDailyOutput 
	where OutputDate between dateadd(d, -2, @OutputDate) and dateadd(d, -1, @OutputDate) 

End
