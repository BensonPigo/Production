
CREATE FUNCTION [dbo].[GetSizeCodeColumnByID]
(
	@OrderID varchar(13) = ''
	,@ByType int = 0 --0單張 , 1 By CustCDID , 2 By PO
)
RETURNS @tbl table(SizeGroup varchar(1),Seq varchar(2),SizeCode varchar(8))
AS
BEGIN
	declare @poid varchar(13) = (select POID from Orders where ID = @OrderID)

	declare @main table(id varchar(13),SizeGroup varchar(1),Seq varchar(2),SizeCode varchar(8))
	--main1 取得全部SizeCode
	insert into @main select id,IsNull(SizeGroup, ''),Seq,SizeCode from Order_SizeCode WHERE ID = @poid
	
	declare @id table (id varchar(13))
	if(@ByType = 0)
		insert into @id select id from dbo.Orders where ID = @OrderID
	else if(@ByType = 1)
		insert into @id select id from dbo.Orders where POID = @poid AND OrderComboID = @OrderID
	else if(@ByType = 2)
		insert into @id select id from dbo.Orders where ID in (select id from Orders where POID = @poid )
		
			
	--main2 取得哪些有QTY
	declare @main2 table(SizeGroup varchar(1),Seq varchar(2),SizeCode varchar(8))
	insert into @main2
	select IsNull(a.SizeGroup, ''),a.Seq,a.SizeCode from @main a inner join dbo.Order_Qty b on a.SizeCode = b.SizeCode
		where b.ID in (select id from @id)
	group by a.SizeGroup,a.Seq,a.SizeCode order by SizeGroup,a.Seq,a.SizeCode
		

	declare cs cursor for
	select distinct SizeGroup from @main2
	open cs
	declare @SizeGroup varchar(1)
	fetch next from cs into @SizeGroup
	while @@FETCH_STATUS=0
		begin
		
			declare @maxScode varchar(8) = (SELECT MAX(SizeCode) FROM @main2 WHERE SizeGroup = @SizeGroup)
			declare @minScode varchar(8) = (SELECT MIN(SizeCode) FROM @main2 WHERE SizeGroup = @SizeGroup)
			
			--if(@ByType = 0)
			--begin
			--	insert into @tbl
			--		select SizeGroup,Seq,SizeCode from @main where SizeGroup = @SizeGroup
			--		group by SizeGroup,Seq,SizeCode order by Seq
			--end
			--else
			--begin
				insert into @tbl
					select SizeGroup,Seq,SizeCode from @main where SizeGroup = @SizeGroup AND SizeCode BETWEEN @minScode AND @maxScode
					group by SizeGroup,Seq,SizeCode order by Seq
			--end

			fetch next from cs into @SizeGroup
		end
	close cs
	deallocate cs

	return
END