-- =============================================
-- Author:		Aaron
-- Create date: 2017/12/07
-- Description:	檢查傳入OrderId在Order_Location是否有資料，沒資料就補
-- =============================================
CREATE PROCEDURE [dbo].[Ins_OrderLocation]
	@OrderId varchar(13)
AS
BEGIN
	
	/*MERGE Order_Location AS tar
	USING (SELECT o.id,sl.Location,sl.Rate,sl.AddName,sl.AddDate,sl.EditName,sl.EditDate
		FROM orders o
		inner join Style_Location sl WITH (NOLOCK) on o.StyleUkey = sl.StyleUkey
		where o.id = @OrderId)  AS s
	ON tar.OrderID = s.id and tar.Location = s.Location
	WHEN NOT MATCHED BY TARGET 
	    THEN INSERT(OrderId,Location,Rate,AddName,AddDate,EditName,EditDate) VALUES(s.Id,s.Location,s.Rate,s.AddName,s.AddDate,s.EditName,s.EditDate);*/

	declare @cnt int
	select @cnt = count(*) from Order_Location where OrderId = @OrderId
	if (@cnt > 0)
	begin
		RETURN
	end

	INSERT into  Order_Location(OrderId,Location,Rate,AddName,AddDate,EditName,EditDate)
		SELECT o.id,sl.Location,sl.Rate,sl.AddName,sl.AddDate,sl.EditName,sl.EditDate
		FROM orders o
		inner join Style_Location sl WITH (NOLOCK) on o.StyleUkey = sl.StyleUkey
		where o.id = @OrderId

END
