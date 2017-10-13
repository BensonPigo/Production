
CREATE PROCEDURE [dbo].[Cutting_P01print_EachConsumption]
	@OrderID VARCHAR(13)
AS
BEGIN
	
	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders WITH (NOLOCK) where ID = @OrderID
	
	SELECT
	APPLYNO=d.SMNoticeID ,MARKERNO=d.MARKERNO ,CUTTINGSP=CuttingSP ,ORDERNO=RTRIM(POID) + d.spno ,STYLENO=StyleID+'-'+SeasonID ,QTY=SUM(Qty) ,FACTORY=FactoryID
	FROM dbo.Orders WITH (NOLOCK)
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WITH (NOLOCK) WHERE POID = @OrderID  order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno
	,STUFF((SELECT '/'+MarkerNo+'-'+MarkerVersion FROM dbo.Order_EachCons WITH (NOLOCK) WHERE Id = @OrderID group by MarkerNo,MarkerVersion FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as MarkerNo
	,STUFF((SELECT '/'+SMNoticeID FROM dbo.Order_EachCons WITH (NOLOCK) WHERE Id = @OrderID group by SMNoticeID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as SMNoticeID) d
	WHERE POID = @OrderID
	GROUP BY d.MarkerNo,d.SMNoticeID,CuttingSP,POID,d.spno,StyleID,SeasonID,FactoryID
	
	Select a.id,a.Ukey
	,'COMB' = a.FabricPanelCode
	--,'COMBdes' = e.Refno + ' ' + e.Description + '                Mark Width:' + a.Width + '  Weight(YDS):' + cast(e.Weight as nvarchar(20))
	,'COMBdes' = om.Remark+ '                Mark Width:' + a.Width + '  Weight(YDS):' + cast(e.Weight as nvarchar(20))
	,a.MarkerName,SizeCode,a.MarkerLength,a.ConsPC
	,a.Seq,a.REMARK,Qty
	,ColorID,Orderqty,Layer,CutQty,Variance,c.Order_EachConsUkey,c.YDS
	,MarkerDownloadID,Article into #tmp
	From dbo.Order_EachCons a WITH (NOLOCK)
	inner join dbo.Order_EachCons_SizeQty b  WITH (NOLOCK) on a.Id = b.Id and a.Ukey = b.Order_EachConsUkey
	inner join dbo.Order_EachCons_Color c WITH (NOLOCK) on a.Id = c.Id and a.Ukey = c.Order_EachConsUkey
	inner join dbo.Order_BOF d WITH (NOLOCK) on a.Id = d.Id and a.FabricCode = d.FabricCode
	inner join dbo.Order_MarkerList om with(nolock) on om.Id=a.Id and om.MarkerName=a.MarkerName and om.FabricPanelCode=a.FabricPanelCode
	inner join dbo.Fabric e WITH (NOLOCK) on d.SCIRefno = e.SCIRefno
	where a.id in (select CuttingSP from dbo.Orders WITH (NOLOCK) where Id = @OrderID)
	order by Seq

	select * into #SizeCodes from GetSizeCodeColumnByID(@OrderID,2) a where a.SizeCode in (select SizeCode from #tmp) order by Seq

	--SELECT SizeGroup FROM #SizeCodes group by SizeGroup

	--For Each Size Group
	declare scode cursor for
		select SizeGroup from #SizeCodes group by SizeGroup order by max(Seq)
	open scode

	declare @SizeGroup varchar(5)
	fetch next from scode into @SizeGroup
	while @@FETCH_STATUS=0
		begin
			
			declare @colStr nvarchar(max) = '' ; declare @colStr2 nvarchar(max) = '' ; declare @colStr3 nvarchar(max) = ''

			select @colStr =  STUFF((SELECT ',['+SizeCode+']' from #SizeCodes a where a.SizeGroup = @SizeGroup order by Seq FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')	
			select @colStr3= 'where ' + STUFF((SELECT ' ['+SizeCode+'] is not null or' from #SizeCodes a where a.SizeGroup = @SizeGroup order by Seq FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')
			select @colStr3 = substring(@colStr3,1,len(@colStr3)-3)
	
			declare @sql varchar(max) = 'SELECT SizeGroup='''+ @SizeGroup +''',MarkerDownloadID,COMB,COMBdes,MarkerName,REMARK,Article,'+@colStr+',MarkerLength as ''MAKER LEN.+1"'',ConsPC AS ''PC(SET)'',ColorID AS COLOR,Orderqty,Layer,CutQty,Variance AS ''Var.'', YDS as ''Cons.(YDS)'' FROM (
				select COMB,COMBdes,MarkerName,MarkerLength,ConsPC,REMARK,Article,SizeCode,c.ColorID,c.Orderqty,c.Layer,c.CutQty,c.Variance,c.YDS,Qty,Seq,MarkerDownloadID from #tmp a
				outer apply (select 
					ColorID=STUFF((SELECT char(10)+ColorID FROM #tmp where id = a.Id and Order_EachConsUkey = a.Ukey group by ColorID order by ColorID FOR XML PATH(''''), TYPE ).value(''.'',''NVARCHAR(MAX)''),1,1,'''')
					,Orderqty=STUFF((SELECT char(10)+cast(Orderqty as varchar) FROM #tmp where id = a.Id and Order_EachConsUkey = a.Ukey group by Orderqty,ColorID order by ColorID FOR XML PATH(''''), TYPE ).value(''.'',''NVARCHAR(MAX)''),1,1,'''')
					,Layer=STUFF((SELECT char(10)+cast(Layer as varchar) FROM #tmp where id = a.Id and Order_EachConsUkey = a.Ukey group by Layer,ColorID order by ColorID FOR XML PATH(''''), TYPE ).value(''.'',''NVARCHAR(MAX)''),1,1,'''')
					,CutQty=STUFF((SELECT char(10)+cast(CutQty as varchar) FROM #tmp where id = a.Id and Order_EachConsUkey = a.Ukey group by CutQty,ColorID order by ColorID FOR XML PATH(''''), TYPE ).value(''.'',''NVARCHAR(MAX)''),1,1,'''')
					,Variance=STUFF((SELECT char(10)+cast(Variance as varchar) FROM #tmp where id = a.Id and Order_EachConsUkey = a.Ukey group by Variance,ColorID order by ColorID FOR XML PATH(''''), TYPE ).value(''.'',''NVARCHAR(MAX)''),1,1,'''')
					,YDS=STUFF((SELECT char(10)+cast(YDS as varchar) FROM #tmp where id = a.Id and Order_EachConsUkey = a.Ukey group by YDS,ColorID order by ColorID FOR XML PATH(''''), TYPE ).value(''.'',''NVARCHAR(MAX)''),1,1,'''')
				) c
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