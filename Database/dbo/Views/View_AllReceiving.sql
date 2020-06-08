

CREATE view [dbo].[View_AllReceiving]
as 
SELECT	[ID],
		[WhseArrival],
		InvNo,
		ExportId,
		ETA,
		MDivisionID,
		[DataFrom] = 'Receiving'
  FROM [dbo].[Receiving]  with (nolock)
union ALL
SELECT	[ID],
		[WhseArrival] = IssueDate,
		[InvNo] = InvNo,
		[ExportId] = null,
		[ETA] = null,
		MDivisionID,
		[DataFrom] = 'TransferIn'
  FROM [dbo].[TransferIn]  with (nolock)




