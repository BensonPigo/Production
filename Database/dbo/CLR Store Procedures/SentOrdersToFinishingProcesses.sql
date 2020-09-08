create procedure [SentOrdersToFinishingProcesses](@orderID nvarchar(max), @transTable nvarchar(max))
AS external name SqlCallWebAPI.Sunrise_FinishingProcesses.SentOrdersToFinishingProcesses;
go