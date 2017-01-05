
Create PROCEDURE [dbo].[Order_Report_FabColorCombination]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By CustCDID , 2 By PO
AS
BEGIN

declare @poid varchar(13) = (select POID from Orders where ID = @OrderID)
declare @tbl table (id varchar(13), Article varchar(8))

if(@ByType = 0)
	insert into @tbl SELECT id,Article FROM DBO.ORDER_ARTICLE WHERE ID = @OrderID
else if(@ByType = 1)
	insert into @tbl SELECT id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Trade.dbo.Orders where POID = @poid AND CustCDID = (select CustCDID from Orders where ID = @OrderID))
else if(@ByType = 2)
	insert into @tbl SELECT id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Trade.dbo.Orders where POID = @poid )
	
--FABRIC 主料，有FabricCode
SELECT Article,c.BrandID,d.ColorID,a.LectraCode,QTLectraCode=isnull(b.QTLectraCode,''),FabricCode 
into #tmp FROM dbo.Order_ColorCombo a
left join (
	SELECT Id,LectraCode,QTLectraCode FROM DBO.Order_FabricCode_QT where LectraCode <> QTLectraCode
) b on a.Id = b.Id and a.LectraCode = b.LectraCode
left join dbo.Orders c on a.Id = c.ID
outer apply (	
	select ColorID=STUFF((SELECT CHAR(10)+ColorID FROM dbo.Color_multiple d where BrandID = c.BrandID and d.ID = a.ColorID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')
) d
WHERE a.ID = @poid AND FABRICCODE != '' and a.Article in (select Article from @tbl)

if exists(select 1 from #tmp)
	begin
		declare @rptcol nvarchar(max) = STUFF((SELECT ',['+LectraCode+']' FROM #tmp group by LectraCode order by LectraCODE FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')
		declare @sql nvarchar(max) = 'SELECT CODE=Article,'+@rptcol+' FROM(
			select Article,ColorID,LectraCode from #tmp
			union select ''MAT.'',FabricCode,LectraCode from #tmp
			union select ''QT With'',QTLectraCode,LectraCode from #tmp
		) a pivot (
			max(ColorID) FOR LectraCode in ('+@rptcol+')
		) b'

		exec (@sql)
	end
else
	begin
		select ' ' = null
	end

drop table #tmp

END