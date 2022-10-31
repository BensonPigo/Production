CREATE PROCEDURE [dbo].[GetSewing_R01]
(
	 @Factory		varchar(80) ,
	 @OutputDate   date ,
	 @Team  varchar(5) =''
	 )
AS
BEGIN	
-------------------------------------------------------------
		select  s.OutputDate
		, s.Category
		, s.Shift
		, s.SewingLineID
		, [ActManPower] = s.Manpower
		, s.Team
		, sd.OrderId
		, sd.ComboType
		, sd.WorkHour
		, sd.QAQty
		, sd.InlineQty
		, [OrderCategory] = isnull(o.Category,'')
		, o.LocalOrder
		, [OrderCdCodeID] = isnull(st.CDCodeNew,'')
        , sty.CDCodeNew
	    , sty.ProductType
	    , sty.FabricType
	    , sty.Lining
	    , sty.Gender
	    , sty.Construction
		, [MockupCDCodeID] = isnull(mo.MockupID,'')
		, s.FactoryID
		, [OrderCPU] = isnull(o.CPU,0)
		, [OrderCPUFactor] = isnull(o.CPUFactor,0)
		, [MockupCPU] = isnull(mo.Cpu,0)
		, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
		, [OrderStyle] = isnull(o.StyleID,'')
		, [MockupStyle] = isnull(mo.StyleID,'')
		, [OrderSeason] = isnull(o.SeasonID,'')
		, [MockupSeason] = isnull(mo.SeasonID,'')
	    , [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType),100)/100
		, System.StdTMS
		, [ori_QAQty] = sd.QAQty
		, [ori_InlineQty] = sd.InlineQty
		, [SubconInSisterFty] = isnull(o.SubconInSisterFty,0)
        , [SubconInType] = isnull(o.SubconInType,0)
		into #tmpSewingDetail
		from System,SewingOutput s WITH (NOLOCK) 
		inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
		left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
		left join Style st with (nolock) on st.Ukey = o.StyleUkey
		left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
		Outer apply (
			SELECT ProductType = r2.Name
				, FabricType = r1.Name
				, Lining
				, Gender
				, Construction = d1.Name
				, s.CDCodeNew
			FROM Style s WITH(NOLOCK)
			left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
			left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
			left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
			where s.Ukey = o.StyleUkey
		)sty
		where s.OutputDate = @OutputDate
				and s.FactoryID = @Factory
				and (o.CateGory NOT IN ('G','A') or s.Category='M')
				and (@Team = '' or s.Team = @Team)
---------------------------------------------------------------------------------------------------------------------
		select OutputDate
			   , Category
			   , Shift
			   , SewingLineID
			   , ActManPower = ActManPower
			   , Team
			   , OrderId
			   , ComboType
			   , WorkHour = Round(sum(WorkHour),3)
			   , QAQty = sum(QAQty) 
			   , InlineQty = sum(InlineQty) 
			   , OrderCategory
			   , LocalOrder
			   , OrderCdCodeID
			   , CDCodeNew
			   , ProductType
			   , FabricType
			   , Lining
			   , Gender
			   , Construction
			   , MockupCDCodeID
			   , FactoryID
			   , OrderCPU
			   , OrderCPUFactor
			   , MockupCPU
			   , MockupCPUFactor
			   , OrderStyle
			   , MockupStyle
			   , OrderSeason
			   , MockupSeason
			   , Rate
			   , StdTMS
			   , ori_QAQty = sum(ori_QAQty) 
			   , ori_InlineQty = sum(ori_InlineQty) 
			   , SubconInType
		into #tmpSewingGroup
		from #tmpSewingDetail
		group by OutputDate, Category, Shift, SewingLineID, Team, OrderId
				 , ComboType, OrderCategory, LocalOrder, OrderCdCodeID
				 , MockupCDCodeID, FactoryID, OrderCPU, OrderCPUFactor
				 , MockupCPU, MockupCPUFactor, OrderStyle, MockupStyle
				 , OrderSeason, MockupSeason, Rate, StdTMS, SubconInType,ActManPower
				 , CDCodeNew, ProductType, FabricType, Lining, Gender, Construction

----↓計算累計天數 function table太慢直接寫在這
		select distinct scOutputDate = s.OutputDate 
			   , style = IIF(t.Category <> 'M', OrderStyle, MockupStyle)
			   , t.SewingLineID
			   , t.FactoryID
			   , t.Shift
			   , t.Team
			   , t.OrderId
			   , t.ComboType
		into #stmp
		from #tmpSewingGroup t
		inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID 
												   and s.OutputDate between dateadd(day,-90,t.OutputDate) and  t.OutputDate 
												   and s.FactoryID = t.FactoryID
		inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
		left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
		left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
		where (o.StyleID = OrderStyle or mo.StyleID = MockupStyle) and (o.CateGory != 'G' or t.Category='M')   
		order by style, s.OutputDate

		select w.Hours
			   , w.Date
			   , style = IIF(t.Category <> 'M', OrderStyle, MockupStyle)
			   , t.SewingLineID
			   , t.FactoryID
			   , t.Shift
			   , t.Team
			   , t.OrderId
			   , t.ComboType
		into #wtmp
		from #tmpSewingGroup t
		inner join  WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
												and w.SewingLineID = t.SewingLineID 
												and w.Date between dateadd(day,-90,t.OutputDate) and  t.OutputDate
												and w.Holiday=0
												and isnull(w.Hours,0) != 0
		select cumulate = IIF(Count(1)=0, 1, Count(1))
			   , s.style
			   , s.SewingLineID
			   , s.FactoryID
			   , s.Shift
			   , s.Team
			   , s.OrderId
			   , s.ComboType
		into #cl
		from #stmp s
		where s.scOutputDate > isnull((select date = max(Date)
								from #wtmp w 
								left join #stmp s2 on s2.scOutputDate = w.Date 
													  and w.style = s2.style 
													  and w.SewingLineID = s2.SewingLineID 
													  and w.FactoryID = s2.FactoryID 
													  and w.Shift = s2.Shift 
													  and w.Team = s2.Team
													  and w.OrderId = s2.OrderId 
													  and w.ComboType = s2.ComboType
								where s2.scOutputDate is null
									  and w.style = s.style 
									  and w.SewingLineID = s.SewingLineID 
									  and w.FactoryID = s.FactoryID 
									  and w.Shift = s.Shift 
									  and w.Team = s.Team 
									  and w.OrderId = s.OrderId 
									  and w.ComboType = s.ComboType),'1900/01/01')
		group by s.style, s.SewingLineID, s.FactoryID, s.Shift, s.Team
				 , s.OrderId, s.ComboType
-----↑計算累計天數
		select t.*
			   , LastShift = CASE WHEN t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1 
				and t.SubconInType in ('1','2') then 'I'
								  WHEN t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1 and t.SubconInType IN ('0','3') then 'IN'
							 ELSE t.Shift END
			   , FtyType = f.Type
			   , FtyCountry = f.CountryID
			   , CumulateDate = isnull(c.cumulate,1)
		into #tmp1stFilter
		from #tmpSewingGroup t
		left join #cl c on c.style = IIF(t.Category <> 'M', OrderStyle, MockupStyle) 
						   and c.SewingLineID = t.SewingLineID 
						   and c.FactoryID = t.FactoryID 
						   and c.Shift = t.Shift 
						   and c.Team = t.Team 
						   and c.OrderId = t.OrderId 
						   and c.ComboType = t.ComboType
		left join Factory f WITH (NOLOCK) on t.FactoryID = f.ID
		---↓最後組成
		select Shift =    CASE    WHEN LastShift='D' then 'Day'
								  WHEN LastShift='N' then 'Night'
								  WHEN LastShift='O' then 'Subcon-Out'
								  WHEN LastShift='I' then 'Subcon-In(Sister)'
								  else 'Subcon-In(Non Sister)' end				
			   , Team
			   , SewingLineID
			   , OrderId
			   , Style = IIF(Category='M',MockupStyle,OrderStyle) 
			   , CDNo = IIF(Category = 'M', MockupCDCodeID, OrderCdCodeID) + '-' + ComboType
			   , CDCodeNew
			   , ProductType
			   , FabricType
			   , Lining
			   , Gender
			   , Construction
			   , ActManPower = IIF(SHIFT = 'O'
									,MAX(ActManPower) OVER (PARTITION BY SHIFT,Team,SewingLineID)
									,ActManPower)
			   , WorkHour
			   , ManHour = ActManPower * WorkHour
			   , TargetCPU = ROUND(ROUND(ActManPower * WorkHour, 3) * 3600 / StdTMS, 3) 
			   , TMS = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * StdTMS
			   , CPUPrice = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
			   , TargetQty = IIF(IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   											   , OrderCPU * OrderCPUFactor * Rate) > 0
	   								, ROUND(ROUND(ActManPower * WorkHour, 2) * 3600 / StdTMS, 2) / IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   					    																										 , OrderCPU * OrderCPUFactor * Rate)
									, 0) 
			   , QAQty
			   , TotalCPU = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * QAQty
			   , CPUSewer = IIF(ROUND(ActManPower * WorkHour, 2) > 0
   										 , ROUND((IIF(Category = 'M', MockupCPU * MockupCPUFactor
   							     									, OrderCPU * OrderCPUFactor * Rate) * QAQty), 3) / ROUND(ActManPower * WorkHour, 3)
     									 , 0) 
			   , EFF = ROUND(IIF(ROUND(ActManPower * WorkHour, 2) > 0
	   									  , (ROUND(IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   						      									 , OrderCPU * OrderCPUFactor * Rate) * QAQty, 2) / (ROUND(ActManPower * WorkHour, 2) * 3600 / StdTMS)) * 100, 0)
	   									  , 1) 
			   , RFT = IIF(ori_InlineQty = 0, 0, ROUND(ori_QAQty* 1.0 / ori_InlineQty * 1.0 * 100 ,2))
			   , CumulateDate
			   , InlineQty
			   , Diff = QAQty - InlineQty
				,FactoryID 
			   , LastShift
			   , ComboType
		into #tmp
		from #tmp1stFilter
		where 1 =1
		order by LastShift,Team,SewingLineID,OrderId
		---↓最後Select 
		select * from #tmp where 1 =1 order by LastShift,Team,SewingLineID,OrderId
---------------------------------------------------------------------------------
		;with SubMaxActManpower as (
			select Shift
				   , Team
				   , SewingLineID
				   , ActManPower = max(ActManPower)
			from #tmp
			group by Shift, Team, SewingLineID
		),
		SubSummaryData as (
			select Shift
				   , Team
				   , TMS = sum(TMS * QAQty)
				   , QAQty = sum(QAQty)
				   , RFT = AVG(RFT)
			from #tmp
			group by Shift, Team
		),
		SubTotal as (
			select s.Shift
				   , s.Team
				   , TMS = case 
								when s.QAQty = 0 then 0
								else (s.TMS/s.QAQty)
						   end
				   , s.RFT
				   , ActManPower = Round(sum(m.ActManPower),2)
			from SubSummaryData s 
			left join SubMaxActManpower m on s.Shift = m.Shift 
											 and s.Team = m.Team
			group by s.Shift, s.Team, s.RFT, s.TMS, s.QAQty
		),
		GrandIncludeInOutMaxActManpower as (
			select Shift
				   , Team
				   , SewingLineID
				   , ActManPower = max(ActManPower) 
			from #tmp
			group by Shift, Team, SewingLineID
		),
		GrandIncludeInOutSummaryData as (
			select TMS = sum(TMS*QAQty)
				   , QAQty = sum(QAQty)
				   , RFT = AVG(RFT)
			from #tmp
		),
		GenTotal1 as (
			select TMS = Case 
							when s.QaQty = 0 then 0
							else (s.TMS/s.QAQty)
						 end
				   , s.RFT
				   , ActManPower = sum(m.ActManPower) - sum(iif(shift = 'Subcon-In', 0, isnull(d.ActManPower,0))) 
			from GrandIncludeInOutSummaryData s
			left join GrandIncludeInOutMaxActManpower m on 1 = 1
			outer apply(
				select ActManPower
				from GrandIncludeInOutMaxActManpower m2
				where m2.Shift = 'Subcon-In' 
					  and m2.Team = m.Team 
					  and m2.SewingLineID = m.SewingLineID	
			) d
			group by s.TMS, s.QAQty, s.RFT
		),
		GrandExcludeOutMaxActManpower as (
			select Shift
				   , Team
				   , SewingLineID
				   , ActManPower = max(ActManPower)
			from #tmp
			where LastShift <> 'O'
			group by Shift, Team, SewingLineID
		),
		GrandExcludeOutSummaryData as (
			select TMS = sum(TMS * QAQty)
				   , QAQty = sum(QAQty)
				   , RFT = AVG(RFT)
			from #tmp
			where LastShift <> 'O'
		),
		GenTotal2 as (
			select TMS = case
							when s.QaQty = 0 then 0
							else (s.TMS / s.QAQty)
						 end
				   , s.RFT
				   , ActManPower = sum(m.ActManPower) - sum(iif(shift = 'Subcon-In', 0, isnull(d.ActManPower,0))) 
			from GrandExcludeOutSummaryData s
			left join GrandExcludeOutMaxActManpower m on 1 = 1
			outer apply(
				select ActManPower
				from GrandExcludeOutMaxActManpower m2
				where m2.Shift = 'Subcon-In' and m2.Team = m.Team 
												 and m2.SewingLineID = m.SewingLineID	
			) d
			group by s.TMS, s.QAQty, s.RFT
		),
		GrandExcludeInOutMaxActManpower as (
			select Shift
				   , Team
				   , SewingLineID
				   , ActManPower = max(ActManPower)
			from #tmp
			where LastShift <> 'O' 
			and LastShift <> 'IN' 
			group by Shift, Team, SewingLineID
		),
		GrandExcludeInOutSummaryData as (
			select TMS = sum(TMS*QAQty)
				   , QAQty = sum(QAQty)
				   , RFT = AVG(RFT)
			from #tmp
			where LastShift <> 'O'
			and LastShift <> 'IN' 
		),
		GenTotal3 as (
			select TMS = case 
							when s.QaQty = 0 then 0
							else (s.TMS/s.QAQty)
						 end
				   , s.RFT
				   , ActManPower = sum(m.ActManPower)
			from GrandExcludeInOutSummaryData s
			left join GrandExcludeInOutMaxActManpower m on 1 = 1
			group by s.TMS, s.QAQty, s.RFT
		)
		select Type = 'Sub'
			   , Sort = '1'
			   , * 
		from SubTotal

		union all
		select Type = 'Grand'  
			   , Sort = '2' 
			   , Shift = '' 
			   , Team = ''
			   , TMS
			   , RFT
			   , ActManPower 
		from GenTotal1

		union all
		select Type = 'Grand'
			   , Sort = '3'
			   , Shift = '' 
			   , Team = ''
			   , TMS
			   , RFT
			   , ActManPower 
		from GenTotal2

		union all
		select Type = 'Grand'
			   , Sort = '4'
			   , Shift = ''
			   , Team = '' 
			   , TMS
			   , RFT
			   , ActManPower 
		from GenTotal3

---------------------------------------------------------------------------------
--準備台北資料(須排除這些)
	select ps.ID
	into #TPEtmp
	from PO_Supp ps
	inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
	inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
	inner join MtlType ml on ml.id = fb.MtlTypeID
	where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
	and ml.isThread=1 
	and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

;with tmpArtwork as (
	Select  ID,
            [DecimalNumber] =case   when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType WITH (NOLOCK) 
	where Classify in ('I','A','P') 
	      and IsTtlTMS = 0
          and IsPrintToCMP=1
),
tmpAllSubprocess as (
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = Round(sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](o.id ,a.ComboType), 100) / 100), ta.DecimalNumber) 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
	inner join tmpArtwork ta on ta.ID = ot.ArtworkTypeID
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O')) 
       -- and o.LocalOrder <> 1
		  and ot.Price > 0         
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
    group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,[dbo].[GetOrderLocation_Rate](o.id ,a.ComboType),ta.DecimalNumber
)
select ArtworkTypeID
	   , Price = sum(Price)
	   , rs = iif(att.ProductionUnit = 'TMS','CPU',iif(att.ProductionUnit = 'QTY','AMT',''))
from tmpAllSubprocess t
left join ArtworkType att WITH (NOLOCK) on att.id = t.ArtworkTypeID
group by ArtworkTypeID,att.ProductionUnit
order by ArtworkTypeID





END
