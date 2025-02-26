

CREATE PROCEDURE P_Import_Capacity 

AS

BEGIN
	select　DISTINCT c.Year
			,c.FactoryID
			,c.Type
	into #GroupByTable
	from TradeDB.ProductionTPE.dbo.Capacity c
	inner join TradeDB.Trade.dbo.Factory f on c.FactoryID=f.ID
	where 1=1
	AND c.Status = 'Approved' 


	----取得每一組資料最新Approve的ID

	select　distinct a.*
	into #LatestApprovedOnly
	from #GroupByTable t
	outer apply(
		SELECT  * 
		FROM TradeDB.ProductionTPE.dbo.Capacity  
		WHERE ID IN (
			/*SELECT TOP 1 ID 
			from dbo.GetCapacityApproveInfo(t.Year,t.Type, t.FactoryID)
			ORDER BY ApvDate DESC, EditDate Desc*/
			----GetCapacityApproveInfo的SQL function不能用，所以只好把內容拉進來
			SELECT TOP 1 a.ID 
			 FROM TradeDB.ProductionTPE.dbo.Capacity a
			 outer apply (
				select top 1
				LastApv_id= tmp.id,
				LastApv_ApvDate= tmp.ApvDate,
				LastApv_EditDate= tmp.EditDate
				from TradeDB.ProductionTPE.dbo.Capacity tmp
				where tmp.year = a.year and tmp.type =a.type and tmp.FactoryID =a.FactoryID  and tmp.Status = 'Approved' AND tmp.ApvDate IS NOt NULL
				and tmp.EditDate < a.AddDate
				order by tmp.EditDate desc
			) b
			 where a.year = t.Year and a.type = ISNULL(t.Type,a.type ) and a.FactoryID = t.FactoryID 
			ORDER BY a.ApvDate DESC, a.EditDate Desc
		)
		AND ApvDate IS NOT NULL
	)a
	
	select c.ID
		,c.Status
		,c.FactoryID
		,f.MDivisionID
		,c.Year
		,Month =Cast( RIGHT(cd.yyyyMM,2) as int )
		,cd.yyyyMM
		,cd.CapaItem
		,[Key] = LEFT(cd.yyyyMM,4)+'/'+ RIGHT(cd.yyyyMM,2)
		,ArtworkTypeID = a.SEQ +'-'+ cd.ArtworkTypeID
		,WorkDays = IIF(cd.CapaItem IN  
			('CE1','BS1','GMDYE1','GW1','HSP1','CM1','PADP1','CPc1','CPu1','CS7'),cd.Value,'0')
		,WorkingHourDaily = IIF(cd.CapaItem IN  
			('CE2','BS2','GMDYE2','GW2','HSP2','CM2','PADP2','CPc2','CPu2','CS6'),cd.Value,'0')
		,TotalIndirectManpower = IIF(cd.CapaItem IN  
			('CS2'),cd.Value,'0')
		,NoofCells = IIF(cd.CapaItem IN  
			('CS3'),cd.Value,'0')
		,NoofSewerCells = IIF(cd.CapaItem IN  
			('CS4'),cd.Value,'0')
		,NoofSewers = IIF(cd.CapaItem IN  
			('CS11'),cd.Value,'0')
		,AbsentRate = IIF(cd.CapaItem IN  
			('CS5'),cd.Value,'0')
	
		,TotalAvailableSewers = IIF(cd.CapaItem IN  
			('CS12'),cd.Value,'0')
		,AverageProductivity = IIF(cd.CapaItem IN  
			('CS10'),cd.Value,'0')

		,FtyCPU = IIF(cd.CapaItem IN  
			('CS15'),cd.Value,'0')
		,SubconCPU = IIF(cd.CapaItem IN  
			('CS16'),cd.Value,'0')
		,TtlCPU = IIF(cd.CapaItem IN  
			('CS17'),cd.Value,'0')
		,AverageEfficiency = IIF(cd.CapaItem IN  
			('CE5', 'BS5', 'GMDYE5', 'GW5', 'HSP5', 'CM4', 'PADP5', 'CPc5', 'CPu5'),cd.Value,'0')
	
		,RemarkDayOffDate = IIF(cd.CapaItem IN  
			('CS19'),cd.Value,'')
		,MachineAvailable = IIF(cd.CapaItem IN  
			('CE3','BS3','GMDYE3','GW3','HSP3','CM3','PADP3'),cd.Value,'0')
		,TtlPrinter = IIF(cd.CapaItem IN  
			('CPc3','CPu3'),cd.Value,'0')
		,AverageAttendance = IIF(cd.CapaItem IN  
			('CPu6'),cd.Value,'0')
		,AverageOutputPerHour = IIF(cd.CapaItem IN  
			('CPc4'),cd.Value,'0')
		,OvalMachineOutputPerDay = IIF(cd.CapaItem IN  
			('CPu10'),cd.Value,'0')
		,AverageStitchesPerHour = IIF(cd.CapaItem IN  
			('CE4'),cd.Value,'0')
		,SubconOutMins = IIF(cd.CapaItem IN  
			('CE8'),cd.Value,'0')
		,SubconOutPcs = IIF(cd.CapaItem IN  
			('BS8','GMDYE8','GW8','HSP8','PADP8','CPc8'),cd.Value,'0')
		,Shift = IIF(cd.CapaItem IN  
			('CE6','BS6','GMDYE6','GW6','HSP6','CM5','PADP6)'),cd.Value,'0')
		,ApprovedDate = c.ApvDate
		,[MachineCapacity]=IIF(d.ID IN
			('CE9','BS9','GMDYE9','GW9','HSP9','CM6','PADP9','CPc9','CPu11'),cd.Value,'0')
		,Unit = IIF(a.ArtworkUnit IS NULL OR a.ArtworkUnit = '','TMS',a.ArtworkUnit )
    into #tmp
	from TradeDB.ProductionTPE.dbo.Capacity c
	inner join TradeDB.ProductionTPE.dbo.Capacity_Detail cd on c.ID = cd.ID
	left join TradeDB.Trade.dbo.Factory f on c.FactoryID = f.ID
	left join TradeDB.Trade.dbo.ArtworkType a ON cd.ArtworkTypeID=a.Id
	left join TradeDB.Trade.dbo.DropDownList d ON d.Type like 'Capa%' ANd  cd.CapaItem = d.ID
	where c.id IN(
		select   c.ID
		from #LatestApprovedOnly c
		inner join TradeDB.Trade.dbo.Factory f on c.FactoryID=f.ID
		left join TradeDB.Trade.dbo.Pass1fty pf on c.Applicant = pf.ID
		left join TradeDB.Trade.dbo.Pass1 p on c.Applicant = p.ID
	)
	
	select distinct ID,Status,FactoryID,MDivisionID,Year,Month ,yyyyMM,[Key],ArtworkTypeID,ApprovedDate
	into #keyTable
	from #tmp
	order by yyyyMM

	
	INSERT INTO P_Capacity
			   (ID			   ,FTY			   ,MDivision			   ,Year			   ,Month			   ,[Key]
			   ,ArtworkType			   ,WorkDays			   ,WorkingHourDaily			   ,TotalIndirectManpower			   ,Noofcells			   ,NoofSewerCell
			   ,NoofSewers			   ,AbsentRate			   ,TotalAvailableSewers			   ,AverageProductivity			   ,FTYCPU			   ,SubconCPU
			   ,TTLCPU			   ,RemarkDayOffDate			   ,MachineAvailableUnits			   ,TTLPrinter			   ,AverageAttendance			   ,AverageOutputPerHour
			   ,OvalMachineOutputPerDayPPU			   ,AverageStitchesPerHour1000Stiches			   ,SubconOut1000StichesMins			   ,SubconOutPcs			   ,ShiftDayandNight
			   ,MachineCapacity			   ,Unit			   ,ApprovedDate	,AverageEfficiency)

	select ID
		--,Status
		,FactoryID
		,MDivisionID
		,Year
		,Month 
		,[Key]
		,ArtworkTypeID
		,WorkDays = CONVERT(float, a.val )
		,WorkingHourDaily = CONVERT(float, b.val )
		,TotalIndirectManpower = CONVERT(float, c.val )
		,NoofCells = CONVERT(float, d.val )
		,NoofSewerCells = CONVERT(float, e.val )
		,NoofSewers = CONVERT(float, f.val )
		,AbsentRate = CONVERT(float, g.val )
		,TotalAvailableSewers = CONVERT(float, h.val )
		,AverageProductivity = CONVERT(float, i.val )
		,FtyCPU = CONVERT(float, j.val )
		,SubconCPU = CONVERT(float, kk.val )
		,TtlCPU = CONVERT(float, l.val )
		,RemarkDayOffDate = m.val 
		,MachineAvailable = CONVERT(float, n.val )
		,TtlPrinter = CONVERT(float, o.val )
		,AverageAttendance = CONVERT(float, p.val )
		,AverageOutputPerHour = CONVERT(float, q.val )
		,OvalMachineOutputPerDay = CONVERT(float, r.val )
		,AverageStitchesPerHour = CONVERT(float, s.val )
		,SubconOutMins = CONVERT(float, t.val )
		,SubconOutPcs = CONVERT(float, u.val )
		,Shift = CONVERT(float, v.val )
		,MachineCapacity= CONVERT(float, w.val )
		,Unit = (select TOP 1 Unit from #tmp t where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month AND t.Unit <> '')
		,ApprovedDate
		,AverageEfficiency = CONVERT(float, x.val )
	from #keyTable k
	outer apply(
		select val =  Sum(cast(t.WorkDays as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)a
	outer apply(
		select val =  Sum(cast(t.WorkingHourDaily as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)b
	outer apply(
		select val =  Sum(cast(t.TotalIndirectManpower as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)c
	outer apply(
		select val =  Sum(cast(t.NoofCells as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)d
	outer apply(
		select val =  Sum(cast(t.NoofSewerCells as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)e
	outer apply(
		select val =  Sum(cast(t.NoofSewers as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)f
	outer apply(
		select val =  Sum(cast(t.AbsentRate as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)g
	outer apply(
		select val =  Sum(cast(t.TotalAvailableSewers as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)h
	outer apply(
		select val =  Sum(cast(t.AverageProductivity as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)i
	outer apply(
		select val =  Sum(cast(t.FtyCPU as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)j
	outer apply(
		select val =  Sum(cast(t.SubconCPU as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)kk
	outer apply(
		select val =  Sum(cast(t.TtlCPU as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)l
	outer apply(
		select Val = STUFF((
			select distinct ',' + t.RemarkDayOffDate 
			from #tmp t
			where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
			for xml path('')
		),1,2,'')
	)m

	outer apply(
		select val =  Sum(cast(t.MachineAvailable as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)n
	outer apply(
		select val =  Sum(cast(t.TtlPrinter as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)o
	outer apply(
		select val =  Sum(cast(t.AverageAttendance as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)p
	outer apply(
		select val =  Sum(cast(t.AverageOutputPerHour as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)q
	outer apply(
		select val =  Sum(cast(t.OvalMachineOutputPerDay as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)r
	outer apply(
		select val =  Sum(cast(t.AverageStitchesPerHour as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)s
	outer apply(
		select val =  Sum(cast(t.SubconOutMins as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)t
	outer apply(
		select val =  Sum(cast(t.SubconOutPcs as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)u
	outer apply(
		select val =  Sum(cast(t.Shift as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)v
	outer apply(
		select val =  Sum(cast(t.MachineCapacity as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)w
	outer apply(
		select val =  Sum(cast(t.AverageEfficiency as float ))
		from #tmp t
		where t.ID = k.ID and t.Year=k.Year AND t.ArtworkTypeID=k.ArtworkTypeID AND t.Month = k.Month
	)x
	where NOT EXISTS( select 1 from P_Capacity p WHERE p.ID=k.ID)



	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.Id = 'P_Capacity'
End
