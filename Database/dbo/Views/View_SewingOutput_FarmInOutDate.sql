CREATE VIEW [dbo].[View_SewingOutput_FarmInOutDate]
AS 

	select so.OutputDate, sodd.OrderId, sodd.Article, sodd.SizeCode
		, [FarmOutDate] = DATEADD(DAY, 1, so.OutputDate)
		, [FarmInDate] = DATEADD(day, 2,so.OutputDate)
		, [Qty] = sum (sodd.QAQty)
	from SewingOutput so with(nolock)
	inner join SewingOutput_Detail_Detail sodd with(nolock) on so.id = sodd.ID
	where exists (select 1 from View_Order_Location_Top1Location t with(nolock) where t.OrderId = sodd.OrderId and t.Location = sodd.ComboType)
	group by so.OutputDate, sodd.OrderId, sodd.Article, sodd.SizeCode

GO
