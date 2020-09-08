create procedure [SentJsonFromAutomationErrMsg](@ukey bigint, @actionName nvarchar(10), @sentResult nvarchar(max) OUTPUT)
AS external name SqlCallWebAPI.AutomationProcedure.SentJsonFromAutomationErrMsg;
go