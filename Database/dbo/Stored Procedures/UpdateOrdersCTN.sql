-- =============================================
-- Description:	UpdateOrdersCTN
-- =============================================
CREATE PROCEDURE UpdateOrdersCTN
(
	@OrderID varchar(13)
)
AS
BEGIN

	SET NOCOUNT ON;

update Orders 
set TotalCTN = (
    select sum(b.CTNQty) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0), 
FtyCTN = (
    select sum(b.CTNQty) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0 and TransferDate is not null), 
ClogCTN = (
    select sum(b.CTNQty) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0 and ReceiveDate is not null
    and TransferCFADate is null AND CFAReturnClogDate is null), 
ClogLastReceiveDate = (
    select max(ReceiveDate) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0), 
cfaCTN = (
    select sum(b.CTNQty)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0 and b.CFAReceiveDate is not null),
DRYCTN = (
    select ISNULL(sum(b.CTNQty),0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0 and b.DRYReceiveDate is not null),
PackErrCTN = (
    select ISNULL(sum(b.CTNQty),0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = @OrderID and b.DisposeFromClog = 0 and b.PackErrTransferDate is not null)
where ID = @OrderID
 
END