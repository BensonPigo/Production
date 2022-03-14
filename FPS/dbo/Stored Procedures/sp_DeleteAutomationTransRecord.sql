CREATE PROCEDURE [dbo].[sp_DeleteAutomationTransRecord]
AS
begin
    --只保留五個月的資料
	declare @DeleteDate date = DATEADD(MONTH, -4, DATEFROMPARTS(YEAR(getdate()),MONTH(getdate()),1))

	delete AutomationTransRecord where AddDate < @DeleteDate
end
