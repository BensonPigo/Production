create procedure [SentJsonToAGV](@ukey bigint, @actionName nvarchar(10), @sentResult nvarchar(max) OUTPUT)
AS external name SqlCallWebAPI.StoredProcedures.SentJsonToAGV;
go