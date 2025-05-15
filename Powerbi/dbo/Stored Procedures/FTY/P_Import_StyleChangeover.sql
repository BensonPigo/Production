Create PROCEDURE [dbo].[P_Import_StyleChangeover]
	@StartDate as date
As
BEGIN
	declare @SQLCMD nvarchar(max), @SQLCMD1 nvarchar(max), @SQLCMD2 nvarchar(max), @SQLCMD_final nvarchar(max)

	set @SQLCMD  = '
		select  [ID] = a.ID
				, [Factory] = isnull(a.FactoryID, '''''''')
				, [SewingLine] = isnull(a.SewingLineID, '''''''')
				, [Inline] = a.Inline
				, [OldSP] = isnull(b.OrderID, '''''''')
				, [OldStyle] = isnull(b.StyleID, '''''''')
				, [OldComboType] = isnull(b.ComboType, '''''''')
				, [NewSP] = isnull(a.OrderID, '''''''')
				, [NewStyle] = isnull(a.StyleID, '''''''')
				, [NewComboType] = isnull(a.ComboType, '''''''')
				, [Category] = isnull(a.Category, '''''''')
				, [COPT(min)] = isnull(a.COPT, 0)
				, [COT(min)] = isnull(a.COT, 0)
		from Production.[dbo].ChgOver a
		outer apply
		(
			select top 1 b.OrderID,b.StyleID,b.ComboType
			from Production.[dbo].ChgOver b
			where b.Inline = (select max(c.Inline) from Production.[dbo].ChgOver c where c.FactoryID = a.FactoryID and c.SewingLineID = a.SewingLineID and c.Inline < a.Inline )
			and b.FactoryID = a.FactoryID
			and b.SewingLineID = a.SewingLineID
		) b
		where a.Inline >= ''''' + cast(@StartDate as varchar)  + '''''
	'
	set @SQLCMD2 = '	
	insert into P_StyleChangeover([ID], [FactoryID], [SewingLine], [Inline], [OldSP], [OldStyle], [OldComboType], [NewSP], [NewStyle], [NewComboType], [Category], [COPT(min)], [COT(min)])
	select t.[ID], t.[Factory], t.[SewingLine], t.[Inline], t.[OldSP], t.[OldStyle], t.[OldComboType], t.[NewSP], t.[NewStyle], t.[NewComboType], t.[Category], t.[COPT(min)], t.[COT(min)]
	from #tmp t
	where not exists (select 1 from P_StyleChangeover p where p.[ID] = t.[ID])

	update p
		set p.[FactoryID] = t.[Factory]
			, p.[SewingLine] = t.[SewingLine]
			, p.[Inline] = t.[Inline]
			, p.[OldSP] = t.[OldSP]
			, p.[OldStyle] = t.[OldStyle]
			, p.[OldComboType] = t.[OldComboType]
			, p.[NewSP] = t.[NewSP]
			, p.[NewStyle] = t.[NewStyle]
			, p.[NewComboType] = t.[NewComboType]
			, p.[Category] = t.[Category]
			, p.[COPT(min)] = t.[COPT(min)]
			, p.[COT(min)] = t.[COT(min)]
	from P_StyleChangeover p
	inner join #tmp t on p.[ID] = t.[ID]


	 delete p
	 from P_StyleChangeover p
	 where not exists (select 1 from #tmp t where p.[ID] = t.[ID])
	 and p.Inline >= ''' + cast(@StartDate as varchar) + '''
	'

	set @SQLCMD_final = '
	SELECT * 
	into #tmp 
	FROM OPENQUERY([MainServer], ''' + @SQLCMD  + ''' )

	' +  @SQLCMD2 + '
	'

	--print @SQLCMD
	--print @SQLCMD_final
	EXEC sp_executesql @SQLCMD_final

	if exists (select 1 from BITableInfo b where b.id = 'P_StyleChangeover')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_StyleChangeover'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_StyleChangeover', getdate())
	end

END