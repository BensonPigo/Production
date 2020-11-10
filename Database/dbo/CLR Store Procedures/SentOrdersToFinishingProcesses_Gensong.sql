create procedure [SentOrdersToFinishingProcesses_Gensong](@orderID nvarchar(max), @transTable nvarchar(max))
AS external name SqlCallWebAPI.Gensong_FinishingProcesses.SentOrdersToFinishingProcesses;
go