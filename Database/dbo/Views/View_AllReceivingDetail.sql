


CREATE view [dbo].[View_AllReceivingDetail]
as 
SELECT	a.[ID],
		b.[MDivisionID],
		b.[FactoryID],
		a.[PoId],
		a.[Seq1],
		a.[Seq2],
		a.[Roll],
		a.[Dyelot],
		a.[StockType],
		a.[ShipQty],
		a.[StockQty],
		a.[ActualQty],
		b.[WhseArrival],
		b.InvNo,
		b.ExportId,
		b.ETA,
		a.PoUnit,
		a.StockUnit,
		b.PackingReceive,
		a.Remark,
		b.Status,
		b.Type,
		[DataFrom] = 'Receiving'
  FROM [dbo].[Receiving_Detail] a with (nolock)
  inner join [dbo].[Receiving] b with (nolock) on a.Id = b.Id
union ALL
SELECT	a.[ID],
		b.[MDivisionID],
		b.[FactoryID],
		a.[PoId],
		a.[Seq1],
		a.[Seq2],
		a.[Roll],
		a.[Dyelot],
		a.[StockType],
		[ShipQty] = a.Qty,
		[StockQty] = a.Qty,
		[ActualQty] = a.Qty,
		[WhseArrival] = IssueDate,
		[InvNo] = null,
		[ExportId] = null,
		[ETA] = null,
		[PoUnit] = '',
		[StockUnit] = '',
		[PackingReceive] = null,
		a.Remark,
		b.Status,
		[Type] = '',
		[DataFrom] = 'TransferIn'
  FROM [dbo].[TransferIn_Detail] a with (nolock)
  inner join [dbo].[TransferIn] b with (nolock) on a.Id = b.Id




