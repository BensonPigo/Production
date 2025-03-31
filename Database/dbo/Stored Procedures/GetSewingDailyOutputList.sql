CREATE PROCEDURE [dbo].[GetSewingDailyOutputList]
     @M				VarChar(8)				
	 ,@Factory		varchar(80) 
	 ,@StartDate    date
	 ,@EndDate      date
	 ,@Category		varchar(80) = '' 
	 ,@Brand		varchar(8) = '' 
	 ,@CDCode		varchar(6) = '' 
	 ,@ProductType	varchar(30) = '' 
	 ,@FabricType	varchar(40) = '' 
	 ,@Lining		varchar(30) = '' 
	 ,@Construction	varchar(40) = '' 
	 ,@Gender		varchar(20) = '' 
	 ,@Shift		varchar(20) = ''
	 ,@Include_Artwork			bit = 1
	 ,@ShowAccumulate_output	bit = 0
	 ,@ExcludeSampleFty			bit = 0 
	 ,@OnlyCancelOrder			bit = 0
	 ,@ExcludeNonRevenue		bit = 0
	 ,@SubconOut				bit = 0
AS
begin

create table #CategoryCondition(
	Category varchar(5)
)

if(@Category like '%MOCKUP%')
	insert #CategoryCondition(Category) values('M')
	  
if(@Category like '%Bulk%')
	insert #CategoryCondition(Category) values('B')

if(@Category like '%Sample%')
	insert #CategoryCondition(Category) values('S')

if(@Category like '%Garment%')
	insert #CategoryCondition(Category) values('G')

if(@Category like '%Local Order%')
	insert #CategoryCondition(Category) values('L')

create table #FactoryList(
	FactoryID varchar(5)
)
insert #FactoryList(FactoryID) 
select FactoryID = Data
from dbo.SplitString(@Factory,',')	

--根據條件撈基本資料
select s.id
	,s.OutputDate
	,s.Category
	,s.Shift
	,s.SewingLineID
	,s.Team
	,s.MDivisionID
	,s.FactoryID
	,sd.OrderId
	,sd.ComboType
	,[ActManPower] = s.Manpower
	,sd.WorkHour
	,sd.QAQty
	,sd.InlineQty
	,o.LocalOrder
	,o.CustPONo
	,[OrderCategory] = isnull(o.Category,'')
	,[OrderType] = isnull(o.OrderTypeID,'')
	,[IsDevSample] = CASE WHEN ot.IsDevSample =1 THEN 'Y' ELSE 'N' END
	,[OrderBrandID] = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end    
    ,[OrderCdCodeID] = isnull(o.CdCodeID,'')
	,[OrderProgram] = isnull(o.ProgramID,'')  
	,[OrderCPU] = isnull(o.CPU,0) 
	,[OrderCPUFactor] = isnull(o.CPUFactor,0) 
	,[OrderStyle] = isnull(o.StyleID,'') 
	,[OrderSeason] = isnull(o.SeasonID,'')
	,[MockupBrandID] = isnull(mo.BrandID,'')   
	,[MockupCDCodeID] = isnull(mo.MockupID,'')
	,[MockupProgram] = isnull(mo.ProgramID,'') 
	,[MockupCPU] = isnull(mo.Cpu,0)
	,[MockupCPUFactor] = isnull(mo.CPUFactor,0)
	,[MockupStyle] = isnull(mo.StyleID,'')
	,[MockupSeason] = isnull(mo.SeasonID,'')	
    ,[Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100
	,System.StdTMS
	, [ori_QAQty] = sd.QAQty
	, [ori_InlineQty] = sd.InlineQty
    ,[BuyerDelivery] = format(o.BuyerDelivery,'yyyy/MM/dd')
    ,[OrderQty] = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInType
    ,[SewingReasonDesc] = isnull(sr.SewingReasonDesc,'')
	,[Remark] = isnull(ssd.SewingOutputRemark,'')
    ,o.SciDelivery 
    ,[NonRevenue]=IIF(o.NonRevenue=1,'Y','N')
    ,Cancel=iif(o.Junk=1,'Y','' )
    ,[InlineCategoryID] = s
	,[Inline Category] = iif(s.SewingReasonIDForTypeLO = '00005',cast('' as varchar(50)),(select CONCAT(InlineCategoryID.val, '-' + SR.Description) from SewingReason sr where sr.ID = InlineCategoryID.val and sr.Type='IC'))
    ,[Low output Reason] = (select CONCAT(s.SewingReasonIDForTypeLO, '-' + SR.Description) from SewingReason sr where sr.ID = s.SewingReasonIDForTypeLO and sr.Type='LO')
    ,[New Style/Repeat style] = cast('' as varchar(20))
	,[CumulateDateSimilar] = sd.CumulateSimilar
	,o.StyleUkey
	,SewingReasonIDForTypeIC = InlineCategoryID.val
into #tmpSewingDetail
from System WITH (NOLOCK),SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join Factory f WITH (NOLOCK) on s.FactoryID = f.ID
OUTER APPLY
(
	select val =
	CASE  WHEN o.Category = 'S' THEN '00005'
		  ELSE
            CASE
                WHEN sd.InlineCategoryCumulate > 29 THEN '00004'
                WHEN sd.InlineCategoryCumulate > 14 THEN '00003'
                WHEN sd.InlineCategoryCumulate > 3 THEN '00002'
                ELSE '00001'
            END
	END

)InlineCategoryID
outer apply
(
	select [SewingReasonDesc]=stuff((
		select concat(',',sr.ID+'-'+sr.Description)
		from SewingReason sr WITH (NOLOCK)
		inner join SewingOutput_Detail sd2 WITH (NOLOCK) on sd2.SewingReasonID=sr.ID
		where sr.Type='SO' 
		and sd2.id = sd.id 
		and sd2.OrderId = sd.OrderId
		for xml path('')
	),1,1,'')
)sr
outer apply
(
	select [SewingOutputRemark]=stuff((
		select concat(',',ssd.Remark)
		from SewingOutput_Detail ssd WITH (NOLOCK) 
		where ssd.ID = sd.ID
		and ssd.OrderId = sd.OrderId
		and isnull(ssd.Remark ,'') <> ''
		for xml path('')
	),1,1,'')
)ssd
outer apply( select BrandID from orders o1 WITH (NOLOCK) where o.CustPONo=o1.id )Order2
outer apply( select top 1 BrandID from Style WITH (NOLOCK) where id=o.StyleID and SeasonID=o.SeasonID and BrandID!='SUBCON-I' )StyleBrand
where	(@StartDate is null or s.OutputDate >= @StartDate) and (@EndDate is null or s.OutputDate <= @EndDate) and
		(@M = '' or s.MDivisionID = @M) and
		( NOT EXISTS( select 1 from #FactoryList) or s.FactoryID IN ( select FactoryID from #FactoryList) ) and
		(@Category <> 'MOCKUP' or s.Category = 'M') and
		(@ExcludeSampleFty = 0 or f.Type != 'S') and
		(@OnlyCancelOrder = 0 or o.Junk = 1) and
		(@ExcludeNonRevenue = 0 or isnull(o.NonRevenue, 0) = 0) and
		(@SubconOut = 0 or s.SubconOutFty <> '') and
		(@Shift <> 'Day+Night' or (s.Shift <> 'O' and o.LocalOrder <> 1 and o.SubconInType not in (1, 2))) and
		(@Shift <> 'Subcon-In' or (s.Shift <> 'O' and o.LocalOrder = 1 and o.SubconInType <> 0)) and
		(@Shift <> 'Subcon-Out' or s.Shift = 'O')

SELECT	FactoryID,
		OutputDate,
		SewinglineID,
		Team,
		StyleUkey,
		[NewStyleRepeatStyle] = dbo.IsRepeatStyleBySewingOutput(FactoryID, OutputDate, SewinglineID, Team, StyleUkey)
INTO	#tmpNewStyleRepeatStyle
from (	select distinct FactoryID, OutputDate, SewinglineID, Team, StyleUkey
		from #tmpSewingDetail ) a

update t set t.[New Style/Repeat style] = tp.NewStyleRepeatStyle
	,　[Inline Category] = iif(SewingReasonIDForTypeIC = '00005',(select CONCAT(t.InlineCategoryID, '-' + SR.Description) from SewingReason sr where sr.ID = t.InlineCategoryID and sr.Type='IC'),t.[Inline Category])
from #tmpSewingDetail t
inner join #tmpNewStyleRepeatStyle tp on	tp.FactoryID = t.FactoryID and
											tp.OutputDate = t.OutputDate and 
											tp.SewinglineID = t.SewinglineID and 
											tp.Team = t.Team and
											tp.StyleUkey = t.StyleUkey

--By Sewing單號 & SewingDetail的Orderid,ComboType 作加總 WorkHour,QAQty,InlineQty
select distinct OutputDate
	,Category
	,Shift
	,SewingLineID
	,Team
	,FactoryID
	,MDivisionID
	,OrderId
	,ComboType
	,[ActManPower] = s.Manpower
	,[WorkHour] = sum(Round(WorkHour,3))over(partition by id,OrderId,ComboType)
	,[QAQty] = sum(QAQty)over(partition by id,OrderId,ComboType)
	,[InlineQty] = sum(InlineQty)over(partition by id,OrderId,ComboType)
	,LocalOrder
	,CustPONo
	,OrderCategory
	,OrderType
	,IsDevSample
	,OrderBrandID 
	,OrderCdCodeID
	,OrderProgram
	,OrderCPU
	,OrderCPUFactor
	,OrderStyle
	,OrderSeason
	,MockupBrandID
	,MockupCDCodeID
	,MockupProgram
	,MockupCPU
	,MockupCPUFactor
	,MockupStyle
	,MockupSeason
	,Rate
	,StdTMS
	, ori_QAQty = sum(ori_QAQty)over(partition by id,OrderId,ComboType)
	, ori_InlineQty = sum(ori_InlineQty)over(partition by id,OrderId,ComboType)
    ,BuyerDelivery
    ,SciDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInType
    ,SewingReasonDesc
    ,NonRevenue
    ,Remark
    ,Cancel
    ,[Inline Category]
    ,[Low output Reason]
    ,[New Style/Repeat style]
	,[CumulateDateSimilar]
into #tmpSewingGroup
from #tmpSewingDetail t
outer apply(
	select s.Manpower from SewingOutput s
	where s.ID = t.ID
)s

select t.*
    ,[LastShift] = IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) 
    ,[FtyType] = f.Type
    ,[FtyCountry] = f.CountryID
    ,[SPFactory] = o.FactoryID
into #tmp1stFilter
from #tmpSewingGroup t
left join Factory f on t.FactoryID = f.ID
left join Orders o on t.OrderId = o.ID
where	(	
			not exists(select 1 from #CategoryCondition) or
			(exists(select 1 from #CategoryCondition where Category = 'L') and t.LocalOrder = 1) or
			(t.OrderCategory in (select Category from #CategoryCondition where Category <> 'M'))
		) and
		(@Brand = '' or t.OrderBrandID = @Brand or t.MockupBrandID = @Brand) and
		(@CDCode = '' or t.OrderCdCodeID = @CDCode or t.MockupCDCodeID = @CDCode)

if(@Include_Artwork = 1)
begin
	-----Artwork
select ID,Seq,ArtworkUnit,ProductionUnit
into #AT
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') and IsTtlTMS = 0 and Junk = 0

select ID,Seq
	,ArtworkType_Unit = concat(ID,iif(Unit='QTY','(Price)',iif(Unit = '','','('+Unit+')'))),Unit
	,ArtworkType_CPU = iif(Unit = 'TMS',concat(ID,'(CPU)'),'')
into #atall
from(
	Select ID,Seq,Unit = ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,ProductionUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit =''
	UNION
	Select ID,Seq,ProductionUnit from #AT where ArtworkUnit ='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,'' from #AT where ArtworkUnit ='' AND ProductionUnit =''
)a

select *
into #atall2
from(
	select a.ID,a.Seq,c=1,a.ArtworkType_Unit,a.Unit from #atall a
	UNION
	select a.ID,a.Seq,2,a.ArtworkType_CPU,iif(a.ArtworkType_CPU='','','CPU')from #atall a
	where a.ArtworkType_CPU !=''
)b

--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from PO_Supp ps WITH (NOLOCK)
inner join PO_Supp_Detail psd WITH (NOLOCK) on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb WITH (NOLOCK) on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml WITH (NOLOCK) on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

-----orderid & ArtworkTypeID & Seq
select distinct ot.ID,ot.ArtworkTypeID,ot.Seq,ot.Qty,ot.Price,ot.TMS,t.QAQty,t.FactoryID,t.Team,t.OutputDate,t.SewingLineID,t.SubConOutContractNumber,
                IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,t.Category,t.ComboType,t.SubconOutFty
into #idat
from #tmpSewingGroup t
inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
inner join orders o with(nolock) on o.ID = t.OrderId
inner join #AT A on A.ID = ot.ArtworkTypeID
where  ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
declare @columnsName nvarchar(max) = stuff((select concat(',[',ArtworkType_Unit,']') from #atall2 for xml path('')),1,1,'')
declare @NameZ nvarchar(max) = (select concat(',[',ArtworkType_Unit,']=isnull([',ArtworkType_Unit,'],0)')from #atall2 for xml path(''))
declare @NameFinal nvarchar(max) = replace((select concat('',col) 
from 
(select [col] = concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']'))
from #atall where id in ('AT (HAND)', 'AT (MACHINE)')
union all
select [col] = ',[TTL_AT(CPU)] = iif([TTL_AT (HAND)(CPU)] > [TTL_AT (MACHINE)(CPU)], [TTL_AT (HAND)(CPU)], [TTL_AT (MACHINE)(CPU)])'
union all
select [col] = concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']'))
from #atall where id not in ('AT (HAND)', 'AT (MACHINE)')
) a for xml path(N'')), '&gt;', '>')

declare @TTLZ nvarchar(max) = 
(select concat(',[',ArtworkType_Unit,']=sum(isnull(Rate*[',ArtworkType_Unit,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber)'
,iif(ArtworkType_CPU = '', '', concat(',[',ArtworkType_CPU,']=sum(isnull(Rate*[',ArtworkType_CPU,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber)'))
,',[TTL_',ArtworkType_Unit,']=Round(sum(o.QAQty*Rate*[',ArtworkType_Unit,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber),',iif(Unit='QTY','4','3'),')'
,iif(ArtworkType_CPU = '', '', concat(',[TTL_',ArtworkType_CPU,']=Round(sum(o.QAQty*Rate*[',ArtworkType_CPU,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber),',iif(Unit='QTY','4','3'),')'))
)from #atall for xml path(''))

end

-----by orderid & all ArtworkTypeID
declare @lastSql nvarchar(max) = ''

if(@Include_Artwork = 1)
begin
	set @lastSql = @lastSql + 'select orderid,SubconOutFty,SubConOutContractNumber,FactoryID,Team,OutputDate,SewingLineID,LastShift,Category,ComboType,qaqty '+@NameZ+N'
into #oid_at
from
(
	select orderid = i.ID,a.ArtworkType_Unit,i.qaqty,ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty))),
           i.FactoryID,i.Team,i.OutputDate,i.SewingLineID,i.LastShift,i.Category,i.ComboType,i.SubconOutFty,i.SubConOutContractNumber
	from #atall2 a left join #idat i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT(min(ptq) for ArtworkType_Unit in('+@columnsName+N'))as pt
where orderid is not null

'
end

set @lastSql = @lastSql + '
select	MDivisionID
		,FactoryID
		,FtyType
		,FtyCountry
        ,OutputDate
        ,SewingLineID
		,Shift
		,SubconOutFty
        ,SubConOutContractNumber
        ,Team
        ,OrderId
        ,CustPONo
        ,BuyerDelivery
		,SciDelivery
        ,Cancel
        ,OrderQty
		,Brand
		,Category
		,Program
		,OrderType
        ,IsDevSample
		,CPURate
		,Style
		,Season
		,ComboType
        ,CDCodeNew
	    ,ProductType
	    ,FabricType
	    ,Lining
	    ,Gender
	    ,Construction
		,ActManPower
		,WorkHour
		,[ManHour] = ManHour
		,[TargetCPU] = TargetCPU
		,[TMS] = TMS
		,[CPUPrice] = CPUPrice
		,[TargetQty] = TargetQty
		,QAQty
		,[TotalCPU] = TotalCPU
		,[CPUSewer] = CPUSewer
		,[EFF] = EFF
		,[RFT] = RFT
		,CumulateDateSimilar
		,DateRange
		,InlineQty'
		if(@ShowAccumulate_output = 1)
			set @lastSql = @lastSql + ',AccOutput
                                ,Balance'
		
		set @lastSql = @lastSql + '
		,Diff
		,[rate] = round(rate, 2)
        ,Remark        
        ,SewingReasonDesc
        ,SPFactory
        ,NonRevenue'

		if(@Include_Artwork = 1)
			set @lastSql = @lastSql + ' ' + @NameFinal + N' '

		set @lastSql = @lastSql + '
        ,[Inline Category]
        ,[Low output Reason]
        ,[New Style/Repeat style]
from(
	select distinct
		 t.MDivisionID,t.FactoryID
		,FtyType = iif(FtyType=''B'',''Bulk'',iif(FtyType=''S'',''Sample'',FtyType))
		,FtyCountry
        ,t.OutputDate
        ,t.SewingLineID
		,Shift =    CASE    WHEN t.LastShift=''D'' then ''Day''
                            WHEN t.LastShift=''N'' then ''Night''
                            WHEN t.LastShift=''O'' then ''Subcon-Out''
                            WHEN t.LastShift=''I'' and SubconInType in (''1'',''2'') then ''Subcon-In(Sister)''
                            else ''Subcon-In(Non Sister)'' end
		,t.SubconOutFty
        ,t.SubConOutContractNumber
        ,t.Team
        ,t.OrderId
        ,CustPONo
        ,t.BuyerDelivery
		,t.SciDelivery
        ,t.Cancel
        ,t.OrderQty
		,Brand = IIF(t.Category=''M'',MockupBrandID,OrderBrandID)
		,Category = IIF(t.OrderCategory=''M'',''Mockup'',IIF(LocalOrder = 1,''Local Order'',IIF(t.OrderCategory=''B'',''Bulk'',IIF(t.OrderCategory=''S'',''Sample'',IIF(t.OrderCategory=''G'',''Garment'','''')))))
		,Program = IIF(t.Category=''M'',MockupProgram,OrderProgram)
		,OrderType
        ,IsDevSample
		,CPURate = IIF(t.Category=''M'',MockupCPUFactor,OrderCPUFactor)
		,Style = IIF(t.Category=''M'',MockupStyle,OrderStyle)
		,Season = IIF(t.Category=''M'',MockupSeason,OrderSeason)
		,t.ComboType
        ,sty.CDCodeNew
	    ,sty.ProductType
	    ,sty.FabricType
	    ,sty.Lining
	    ,sty.Gender
	    ,sty.Construction
		,ActManPower = ActManPower
		,WorkHour
		,ManHour = ROUND(ActManPower*WorkHour,2)
		,TargetCPU = ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)
		,TMS = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
		,CPUPrice = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
		,TargetQty = IIF(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)/IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
		,t.QAQty
		,TotalCPU = ROUND(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty,3)
		,CPUSewer = IIF(ROUND(ActManPower*WorkHour,2)>0,(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/ROUND(ActManPower*WorkHour,2),0)
		,EFF = ROUND(IIF(ROUND(ActManPower*WorkHour,2)>0,((IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/(ROUND(ActManPower*WorkHour,2)*3600/StdTMS))*100,0),1)
		,RFT = IIF(isnull(A.InspectQty,0) = 0, 0, round(((A.InspectQty-A.RejectQty) / A.InspectQty)*100,2)) 
		,CumulateDateSimilar = CONVERT(VARCHAR, CumulateDateSimilar)
		,DateRange = IIF(CumulateDateSimilar >= 10,''>=10'',CONVERT(VARCHAR,CumulateDateSimilar))
		,InlineQty'

		if(@ShowAccumulate_output = 1)
			set @lastSql = @lastSql + ',[AccOutput] = acc_output.value
                                ,Balance =  t.OrderQty -  acc_output.value '
		
		set @lastSql = @lastSql + ',Diff = t.QAQty-InlineQty
		,rate
        ,t.Remark        
        ,t.SewingReasonDesc
        ,t.SPFactory
        ,t.NonRevenue
		'

		if(@Include_Artwork = 1)
			set @lastSql = @lastSql + ' ' + @TTLZ + N' '

		set @lastSql = @lastSql + '
        ,t.[Inline Category]
        ,t.[Low output Reason]
        ,t.[New Style/Repeat style]
    from #tmp1stFilter t
    Outer apply (
	    SELECT s.CDCodeNew
            , ProductType = r2.Name
		    , FabricType = r1.Name
		    , s.Lining
		    , s.Gender
		    , Construction = d1.Name
	    FROM Style s WITH(NOLOCK)
	    left join DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
	    left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
	    left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
	    where s.ID = IIF(t.Category=''M'', t.MockupStyle, t.OrderStyle)
	    and s.SeasonID = IIF(t.Category=''M'', t.MockupSeason, t.OrderSeason)
	    and s.BrandID = IIF(t.Category=''M'', t.MockupBrandID, t.OrderBrandID)
    ) sty '

		if(@ShowAccumulate_output = 1)
			set @lastSql = @lastSql + '
			outer  apply(select value = Sum(SD.QAQty)
						from SewingOutput_Detail SD WITH (NOLOCK)
						inner join SewingOutput S WITH (NOLOCK) on SD.ID=S.ID
						where SD.ComboType=t.ComboType
						  and SD.orderid=t.OrderId
						  and S.OutputDate <= t.OutputDate) acc_output'

		if(@Include_Artwork = 1)
			set @lastSql = @lastSql + '
				left join #oid_at o on o.orderid = t.OrderId and 
                           o.FactoryID = t.FactoryID and
                           o.Team = t.Team and
                           o.OutputDate = t.OutputDate and
                           o.SewingLineID = t.SewingLineID and
                           o.LastShift = t.LastShift and
                           o.Category = t.Category and
                           o.ComboType = t.ComboType and
                           o.SubconOutFty = t.SubconOutFty and
                           o.SubConOutContractNumber = t.SubConOutContractNumber
			'
		set @lastSql = @lastSql 
		+ '
		left join RFT A WITH (NOLOCK) on A.OrderID=t.OrderId
										 and A.CDate=t.OutputDate
										 and A.SewinglineID=t.SewinglineID
										 and A.FactoryID=t.FactoryID
										 and A.Shift=t.Shift
										 and A.Team=t.Team
		'
		set @lastSql = @lastSql + '
		where  1 = 1 '
		
		if(isnull(@ProductType, '') <> '')
			set @lastSql = @lastSql + ' and sty.ProductType = ''' + @ProductType + ''''

		if(isnull(@FabricType, '') <> '')
			set @lastSql = @lastSql + ' and sty.FabricType = ''' + @FabricType + ''''

		if(isnull(@Lining, '') <> '')
			set @lastSql = @lastSql + ' and sty.Lining = ''' + @Lining + ''''

		if(isnull(@Gender, '') <> '')
			set @lastSql = @lastSql + ' and sty.Gender = ''' + @Gender + ''''

		if(isnull(@Construction, '') <> '')
			set @lastSql = @lastSql + ' and sty.Construction = ''' + @Construction + ''''

		set @lastSql = @lastSql + ') a  order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId

		drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup
		'

		if(@Include_Artwork = 1)
			set @lastSql = @lastSql + ' 
			drop table #atall2,#AT,#atall,#idat,#oid_at,#TPEtmp'

		EXEC sp_executesql @lastSql
end
GO