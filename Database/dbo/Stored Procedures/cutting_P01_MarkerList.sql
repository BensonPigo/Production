﻿
CREATE PROCEDURE [dbo].[cutting_P01_MarkerList]
	@OrderID VARCHAR(13)
AS
BEGIN

	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders WITH (NOLOCK) where ID = @OrderID
	
	SELECT
	ORDERNO=RTRIM(POID) + d.spno ,STYLENO=StyleID+'-'+SeasonID ,QTY=SUM(Qty) ,FACTORY=FactoryID
	,REPORTNAME = (SELECT top 1 Title FROM dbo.Company WITH (NOLOCK) where Junk = 0 and IsDefault = 1 order by ID desc)
	FROM dbo.Orders WITH (NOLOCK)
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WITH (NOLOCK) WHERE POID = @OrderID  order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) d
	WHERE POID = @OrderID
	GROUP BY POID,d.spno,StyleID,SeasonID,FactoryID
	

	Select a.id,a.Ukey
	,'COMB' = FabricPanelCode
	,'COMBdes' = e.Refno + ' ' + e.Description + '                Mark Width:' + a.Width + '  Mark Weight:' + cast(e.Weight as nvarchar(20))
	,MarkerName,SizeCode,MarkerLength,a.ActCuttingPerimeter
	,Seq,a.REMARK,Qty
	into #tmp
	From dbo.Order_MarkerList a WITH (NOLOCK)
	inner join dbo.Order_MarkerList_SizeQty b WITH (NOLOCK) on a.Id = b.Id and a.Ukey = b.Order_MarkerListUkey	
	inner join dbo.Order_BOF d WITH (NOLOCK) on a.Id = d.Id and a.FabricCode = d.FabricCode
	inner join dbo.Fabric e WITH (NOLOCK) on d.SCIRefno = e.SCIRefno
	where a.id in (select CuttingSP from dbo.Orders WITH (NOLOCK) where Id = @OrderID)
	order by Seq


	--For Each Size Group
	select * into #SizeCodes from GetSizeCodeColumnByID(@OrderID,1) a where a.SizeCode in (select SizeCode from #tmp) order by Seq

	declare scode cursor for
		select SizeGroup from #SizeCodes group by SizeGroup order by max(Seq)
	open scode

	declare @SizeGroup varchar(5)
	fetch next from scode into @SizeGroup
	while @@FETCH_STATUS=0
		begin			

			declare @colStr nvarchar(max) = '' ; declare @colStr3 nvarchar(max) = ''

			select @colStr=STUFF((SELECT ',['+SizeCode+']' from #SizeCodes a where a.SizeGroup = @SizeGroup order by Seq FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')
			select @colStr3= 'where ' + STUFF((SELECT ' ['+SizeCode+'] is not null or' from #SizeCodes a where a.SizeGroup = @SizeGroup order by Seq FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')
			select @colStr3 = substring(@colStr3,1,len(@colStr3)-3)

			declare @sql varchar(max) = 'SELECT SizeGroup='''+@SizeGroup+''',COMB,COMBdes,MarkerName,REMARK,'+@colStr+',MarkerLength as ''MAKER LEN.+1"'',ActCuttingPerimeter AS ''Cut Perimeter'' FROM (
				select COMB,COMBdes,MarkerName,MarkerLength,ActCuttingPerimeter,REMARK,SizeCode,Qty,Seq from #tmp
			) a pivot (max(Qty) for SizeCode in ('+@colStr+')) b '+@colStr3+'
			order by Seq'

			--print @sql
			exec (@sql)
						
			fetch next from scode into @SizeGroup
		end
	close scode
	deallocate scode
		
	drop table #SizeCodes
	drop table #tmp
	
END