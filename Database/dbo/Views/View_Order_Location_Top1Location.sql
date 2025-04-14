CREATE VIEW [dbo].[View_Order_Location_Top1Location]
AS
	select *
	from (
		select ol.orderID, ol.[Location]
			, R_ID = row_number() over(partition by ol.orderID, ol.[Location] order by ol.Rate desc, ol.[Location] desc)
		from Order_Location ol with(nolock)
	) ol
	where ol.R_ID = 1
GO
