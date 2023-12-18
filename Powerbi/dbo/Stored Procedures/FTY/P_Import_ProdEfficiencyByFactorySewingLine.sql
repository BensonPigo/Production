Create PROCEDURE [dbo].[P_Import_ProdEfficiencyByFactorySewingLine]
	@StartDate as date
As
BEGIN	
	declare @SQLCMD nvarchar(max), @SQLCMD1 nvarchar(max), @SQLCMD2 nvarchar(max), @SQLCMD_final nvarchar(max)

	set @SQLCMD  = '
	select  [Year-Month]
			, FtyZone 
			, [Factory] = FactoryID
			, [Line] = SewingLineID
			, [TotalQty] = TtlQty
			, [TotalCPU] = TtlCPU
			, [TotalManhours] = TtlManhour
			, [PPH] = IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2))
			, [EFF] = IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from  Production.dbo.System WITH (NOLOCK) ))*100, 2)) 
	from (
		select FtyZone
				, FactoryID
				, [Year-Month]
				, SewingLineID
				, TtlCPU = Sum(ROUND( CPU*CPUFactor*Rate*QAQty,3))
				, TtlManhour = sum(ROUND( ActManPower * WorkHour, 2))
				, TtlQty = Sum(RateOutput) 
		from (
			select o.ID
					, [FtyZone] = f.FtyZone
					, o.FactoryID
					, o.CPU
					, s.SewingLineID
					, sd.WorkHour
					, sd.QAQty
					, o.CPUFactor
					, Rate = isnull(Production.[dbo].[GetOrderLocation_Rate]( o.id ,sd.ComboType)/100,1) 
					, s.OutputDate
					, ActManPower= s.Manpower
					, RateOutput = sd.QAQty  * isnull(Production.[dbo].[GetOrderLocation_Rate]( o.id ,sd.ComboType)/100,1) 
					, [Year-Month] = CONVERT(date,dateadd(day ,-1, dateadd(m, datediff(m,0,s.OutputDate)+1,0)))
			from Production.dbo.Orders o with(nolock)
			inner join Production.dbo.SewingOutput_Detail sd with(nolock) on sd.OrderId = o.ID
			inner join Production.dbo.SewingOutput s with(nolock) on s.ID = sd.ID
			inner join Production.dbo.Factory f with(nolock) on f.ID = o.FactoryID
			where o.Category = ''''B''''
			and s.Shift <> ''''O''''
			and ((o.LocalOrder = 1 and o.SubconInType in (''''1'''',''''2'''')) or (o.LocalOrder = 0 and o.SubconInType = 0)) 
			and s.OutputDate >= ''''' + cast(@StartDate as varchar)  + '''''
			and f.Type <> ''''S''''
		) t
		group by FtyZone, FactoryID, [Year-Month], SewingLineID
	)t
	Order by FtyZone, FactoryID, SewingLineID, [Year-Month]
	'
	set @SQLCMD2 = '	
	insert into P_ProdEfficiencyByFactorySewingLine([Year-Month], FtyZone, Factory, Line, TotalQty, TotalCPU, TotalManhours, PPH, [EFF])
	select t.[Year-Month], t.FtyZone, t.Factory, t.Line, t.TotalQty, t.TotalCPU, t.TotalManhours, t.PPH, t.[EFF]
	from #tmp t
	where not exists (select 1 from P_ProdEfficiencyByFactorySewingLine p where p.[Year-Month] = t.[Year-Month] and p.FtyZone = t.FtyZone and p.Factory = t.Factory and p.Line = t.Line)

	update p
		set p.[TotalQty] = t.[TotalQty]
			, p.[TotalCPU] = t.[TotalCPU]
			, p.[TotalManhours] = t.[TotalManhours]
			, p.[PPH] = t.[PPH]
			, p.[EFF] = t.[EFF]
	from P_ProdEfficiencyByFactorySewingLine p
	inner join #tmp t on p.[Year-Month] = t.[Year-Month] and p.FtyZone = t.FtyZone and p.Factory = t.Factory and p.Line = t.Line


	 delete p
	 from P_ProdEfficiencyByFactorySewingLine p
	 where not exists (select 1 from #tmp t where p.[Year-Month] = t.[Year-Month] and p.FtyZone = t.FtyZone and p.Factory = t.Factory and p.Line = t.Line)
	 and p.[Year-Month] >= ''' + cast(@StartDate as varchar) + '''
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

	if exists (select 1 from BITableInfo b where b.id = 'P_ProdEfficiencyByFactorySewingLine')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_ProdEfficiencyByFactorySewingLine'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_ProdEfficiencyByFactorySewingLine', getdate())
	end


END

 
GO