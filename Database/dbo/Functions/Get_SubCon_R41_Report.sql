Create Function dbo.[Get_SubCon_R41_Report]
(
	@EditDate1    varchar(30) = ''
	,@NewImportDate1    varchar(30) = '' --for 第一次匯入用
)
RETURNs @RtnTable TABLE
(
	[Bundleno] varchar(12),
	[RFIDProcessLocationID] varchar(15),
	[EXCESS] varchar(5),
	[FabricKind] varchar(250),
	[Cut Ref#] varchar(10),
	[SP#]varchar(250),
	[Master SP#] varchar(15),
    [M] varchar(8),
    [Factory] varchar(10),
	[Category] varchar(1),
	[Program] nvarchar(50),
    [Style] varchar(15),
    [Season] varchar(10),
    [Brand] varchar(8),
    [Comb] varchar(4),
    Cutno numeric(6,0),
	[Fab_Panel Code] varchar(4),
    [Article] varchar(15),
    [Color] varchar(10),
    [Line] varchar(10),
    SewingLineID varchar(10),
    [Cell] varchar(10),
    [Pattern] varchar(20),
    [PtnDesc] nvarchar(200),
    [Group] numeric(7,0),
    [Size] varchar(8),
    [Artwork] nvarchar(500),
    [Qty] numeric(9,0),
    [Sub-process] varchar(50),
    [Post Sewing SubProcess] varchar(15),
    [No Bundle Card After Subprocess] varchar(15),
    LocationID varchar(10),
    Cdate date,
    BuyerDelivery date,
    SewInLine date,
	[InspectionDate] date,
    [InComing] datetime,
    [Out (Time)] datetime,
    [POSupplier] nvarchar(500),
    [AllocatedSubcon] nvarchar(500),
	AvgTime numeric(20,9),
	TimeRange nvarchar(50),
	EstCutDate date,
	CuttingOutputDate date,
	Item varchar(50),
	PanelNo varchar(24),
	CutCellID varchar(10),
	[SpreadingNo] nvarchar(500),
	[LastSewDate] date,
	[SewQty] int
)
As
Begin

Declare @Workorder table(
	BundleNo varchar(10),
	Location varchar(1),
	Article varchar(8),
	ColorID varchar(6),
	SizeCode varchar(8),
	Patterncode varchar(20),
	OrderID varchar(15)
)
insert into @Workorder
select distinct bd.BundleNo,
                bd.Location,
                b.Article,
                b.ColorID,
                bd.SizeCode,
                bd.Patterncode,
                b.OrderID
from dbo.Bundle b WITH (NOLOCK)
inner join dbo.Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id 
INNER JOIN dbo.Bundle_Detail_Order BDO with(nolock) on BDO.BundleNo = BD.BundleNo
inner join dbo.orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join dbo.factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join dbo.BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno --and bio.SubProcessId = s.Id
where 1=1
and (@NewImportDate1 is null or b.Cdate >= @NewImportDate1)
and 
(
	(@EditDate1 is null or b.Adddate >= @EditDate1)
or  (@EditDate1 is null or b.EditDate >= @EditDate1)
or  (@EditDate1 is null or bio.InComing >= @EditDate1)
or  (@EditDate1 is null or bio.OutGoing >= @EditDate1)
)


--取得Last Sew. Date相關資料
Declare @SewingInfo table(
	OrderId varchar(15),	
	Article varchar(8),
	SizeCode varchar(8),
	ComboType varchar(8),
	LastSewDate date,
	SewQty int
); 
with Sewing as(
select  vsl.OrderId, vsl.Article, vsl.SizeCode, vsl.ComboType, vsl.LastSewDate, vsl.SewQty
from View_SewingInfoLocation vsl with (nolock)
where  exists(
	select 1 from @Workorder tw 
	where   tw.OrderID = vsl.OrderId and
			tw.Article = vsl.Article and
			tw.SizeCode = vsl.SizeCode and
			tw.Location = vsl.ComboType
	)
)
insert into @SewingInfo(OrderId, Article, SizeCode, ComboType, LastSewDate, SewQty)
select  OrderId, 
        Article, 
        SizeCode,
        'ALLPARTS',
        [LastSewDate] = max(LastSewDate), 
        [SewQty] = sum(SewQty)
from Sewing
group by OrderId, Article, SizeCode

Declare @result TABLE
(
	[Bundleno] varchar(12),
	[RFIDProcessLocationID] varchar(15),
	[EXCESS] varchar(5),
	[Cut Ref#] varchar(10),
	[SP#]varchar(15),
	sps varchar(150),
	[Master SP#] varchar(15),
    [M] varchar(8),
    [Factory] varchar(10),
	[Category] varchar(1),
	[Program] nvarchar(50),
    [Style] varchar(15),
    [Season] varchar(10),
    [Brand] varchar(8),
    [Comb] varchar(4),
    Cutno numeric(6,0),
	[Fab_Panel Code] varchar(4),
    [Article] varchar(15),
    [Color] varchar(10),
    [Line] varchar(10),
    SewingLineID varchar(10),
    [Cell] varchar(10),
    [Pattern] varchar(20),
    [PtnDesc] nvarchar(200),
    [Group] numeric(7,0),
    [Size] varchar(8),
    [Artwork] nvarchar(500),
    [Qty] numeric(9,0),
    [Sub-process] varchar(50),
    [Post Sewing SubProcess] varchar(15),
    [No Bundle Card After Subprocess] varchar(15),
    LocationID varchar(10),
    Cdate date,
    BuyerDelivery date,
    SewInLine date,
    [InComing] datetime,
    [Out (Time)] datetime,
    [POSupplier] nvarchar(500),
    [AllocatedSubcon] nvarchar(500),
	AvgTime numeric(15,7),
	TimeRangeFail nvarchar(50),
	InOutRule tinyint,
	Item varchar(50),
	PanelNo varchar(24),
	CutCellID varchar(10),
	[SpreadingNo] nvarchar(500),
	[FabricKind] varchar(250),
    [BundleLocation] varchar(1),
    [InspectionDate] date
)
insert into @result
Select 
    [Bundleno] = bd.BundleNo,
    [RFIDProcessLocationID] = isnull(bio.RFIDProcessLocationID,''),
    [EXCESS] = iif(b.IsEXCESS = 0, '','Y'),
    [Cut Ref#] = isnull(b.CutRef,''),
    [SP#] = b.Orderid,
	sps =iif((select count(1) from dbo.Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo) = 1
		, (select OrderID from dbo.Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo)
		, dbo.GetSinglelineSP((select OrderID from dbo.Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo order by OrderID for XML RAW))),
    [Master SP#] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
	[Category]=o.Category,
	[Program]=o.ProgramID,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Comb] = b.PatternPanel,
    b.Cutno,
	[Fab_Panel Code] = b.FabricPanelCode,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Line] = b.SewinglineId,
    bio.SewingLineID,
    [Cell] = b.SewingCell,
    [Pattern] = bd.PatternCode,
    [PtnDesc] = bd.PatternDesc,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Artwork] = sub.sub,
    [Qty] = bd.Qty,
    [Sub-process] = s.Id,
    [Post Sewing SubProcess]= iif(ps.sub = 1,N'?',''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N'?',''),
    bio.LocationID,
    b.Cdate,
    o.BuyerDelivery,
    o.SewInLine,

    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing,
    [POSupplier] = iif(PoSuppFromOrderID.Value = '',PoSuppFromPOID.Value,PoSuppFromOrderID.Value),
    [AllocatedSubcon]=Stuff((					
					select concat(',',ls.abb)
					from dbo.order_tmscost ot
					inner join dbo.LocalSupp ls on ls.id = ot.LocalSuppID
					 where ot.id = o.id and ot.ArtworkTypeID=s.ArtworkTypeId 
					for xml path('')
					),1,1,''),
	AvgTime = case  when s.InOutRule = 1 then iif(bio.InComing is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.InComing,''))/24.0,2))
					when s.InOutRule = 2 then iif(bio.OutGoing is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.OutGoing,''))/24.0,2))
					when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then null
					when s.InOutRule = 3 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.InComing,''),isnull(bio.OutGoing,''))/24.0,2))
					when s.InOutRule = 4 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.OutGoing,''),isnull(bio.InComing,''))/24.0,2))
					end,
	TimeRangeFail = case	when s.InOutRule = 1 and bio.InComing is null then 'No Scan'
						when s.InOutRule = 2 and bio.OutGoing is null then 'No Scan'
						when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then 'No Scan'
						when s.InOutRule = 3 and (bio.OutGoing is null or bio.InComing is null) then 'Not Valid'
						when s.InOutRule = 4 and (bio.OutGoing is null or bio.InComing is null) then 'Not Valid'
						else '' end,
	s.InOutRule
	,b.Item
	,bio.PanelNo
	,bio.CutCellID
	,[SpreadingNo] = wk.SpreadingNo
	,[FabricKind] = FabricKind.val
    ,[BundleLocation] = w.Location
    ,[InspectionDate] = sp.InspectionDate
from @Workorder w 
inner join dbo.Bundle_Detail bd WITH (NOLOCK, Index(PK_Bundle_Detail)) on bd.BundleNo = w.BundleNo 
inner join dbo.Bundle b WITH(NOLOCK, index(PK_Bundle)) on b.ID = bd.ID
inner join dbo.orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join dbo.factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
outer apply(
    select s.ID,s.InOutRule,s.ArtworkTypeId
    from dbo.SubProcess s
        where exists (
                        select 1 from dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or  (s.IsSelection = 0 AND s.IsRFIDDefault = 1)
) s
left join dbo.BundleInOut bio WITH (NOLOCK, index(PK_BundleInOut)) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub 
outer apply(
    select sub = 1
    from dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = s.ID
) as ps
outer apply(
    select top 1 sub = 1
    from dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    --and bda.SubprocessId = s.ID
) as nbs 

outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from dbo.ArtworkPO ap with (nolock)
	                                                            inner join dbo.ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join dbo.LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = b.OrderId 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromOrderID
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' and isnull(PoSuppFromOrderID.Value,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from dbo.ArtworkPO ap with (nolock)
	                                                            inner join dbo.ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join dbo.LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = o.POID 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromPOID
outer apply(
	 select SpreadingNo = stuff((
		    Select distinct concat(',', wo.SpreadingNoID)
		    from dbo.WorkOrder wo WITH (NOLOCK) 
		    where   wo.CutRef = b.CutRef 
                    and wo.ID = b.POID
            and wo.SpreadingNoID is not null
            and wo.SpreadingNoID != ''
		    for xml path('')
	    ),1,1,'')
)wk
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME 
	FROM dbo.dropdownlist DD 
	OUTER apply(
			SELECT OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM dbo.order_colorcombo OCC WITH (NOLOCK)
			INNER JOIN dbo.order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = b.poid 
		AND LIST.patternpanel = b.patternpanel 
		AND DD.[type] = 'FabricKind' 
		AND DD.id = LIST.kind 
)FabricKind
outer apply(
    select [InspectionDate] = MAX(InspectionDate)
    from dbo.SubProInsRecord sp with(nolock)
    where sp.Bundleno = bd.Bundleno
    and sp.SubProcessID = s.ID
)sp
 where 1=1  

declare @GetCutDateTmp table(
	[Cut Ref#] varchar(10),
	[M] varchar(8),
	[EstCutDate] date,
	[CuttingOutputDate] date
 )
 insert into @GetCutDateTmp
 select	r.[Cut Ref#],
			r.M,
			[EstCutDate] = MAX(w.EstCutDate),
			[CuttingOutputDate] = MAX(co.cDate)
	from @result r
	inner join dbo.WorkOrder w with (nolock) on w.CutRef = r.[Cut Ref#] and w.id = r.[Master SP#]
	left join dbo.CuttingOutput_Detail cod with (nolock) on cod.WorkOrderUkey = w.Ukey
	left join dbo.CuttingOutput co  with (nolock) on co.ID = cod.ID
    where r.[Cut Ref#] <> ''
	group by r.[Cut Ref#],r.M

insert into @RtnTable
select
    [Bundleno] = isnull(r.[Bundleno],'') ,
    [RFIDProcessLocationID] = isnull(r.[RFIDProcessLocationID],''),
	[EXCESS] = isnull(r.[EXCESS],''),
	[FabricKind] = isnull(r.[FabricKind],''),
    [Cut Ref#] = isnull(r.[Cut Ref#],'') ,
    [SP#] = isnull(r.sps,''),
    [Master SP#] = isnull(r.[Master SP#],''),
    [M] = isnull(r.[M],''),
    [Factory] = isnull(r.[Factory],''),
	[Category] = isnull(r.[Category],''),
	[Program] = isnull(r.[Program],''),
    [Style] = isnull(r.[Style],''),
    [Season] = isnull(r.[Season],''),
    [Brand] = isnull(r.[Brand],''),
    [Comb] = isnull(r.[Comb],''),
    [Cutno] = isnull(r.Cutno,0),
	[Fab_Panel Code] = isnull(r.[Fab_Panel Code],''),
    [Article] = r.[Article],
    [Color] = isnull(r.[Color],''),
    [Line] = isnull(r.[Line],''),
    SewingLineID = isnull(r.SewingLineID,''),
    [Cell] = isnull(r.[Cell],''),
    [Pattern] = isnull(r.[Pattern],''),
    [PtnDesc] = isnull(r.[PtnDesc],''),
    [Group] = isnull(r.[Group],0),
    [Size] = isnull(r.[Size],''),
    [Artwork] = isnull(r.[Artwork],''),
    [Qty] = isnull(r.[Qty],0),
    [Sub-process] = isnull(r.[Sub-process],''),
    [Post Sewing SubProcess] = isnull(r.[Post Sewing SubProcess],''),
    [No Bundle Card After Subprocess] = isnull(r.[No Bundle Card After Subprocess],''),
    [LocationID] = isnull(r.LocationID,''),
    r.Cdate,
    r.[BuyerDelivery],
    r.[SewInLine],
    r.[InspectionDate],
    r.[InComing],
    r.[Out (Time)],
    [POSupplier] = isnull(r.[POSupplier],''),
    [AllocatedSubcon] = isnull(r.[AllocatedSubcon],''),
	AvgTime = isnull(r.AvgTime,0),
    [TimeRange] = case	when TimeRangeFail <> '' then TimeRangeFail
                        when AvgTime < 0 then 'Not Valid'
						when AvgTime >= 0 and AvgTime < 1 then '<1'
						when AvgTime >= 1 and AvgTime < 2 then '1-2'
						when AvgTime >= 2 and AvgTime < 3 then '2-3'
						when AvgTime >= 3 and AvgTime < 4 then '3-4'
						when AvgTime >= 4 and AvgTime < 5 then '4-5'
						when AvgTime >= 5 and AvgTime < 10 then '5-10'
						when AvgTime >= 10 and AvgTime < 20 then '10-20'
						when AvgTime >= 20 and AvgTime < 30 then '20-30'
						when AvgTime >= 30 and AvgTime < 40 then '30-40'
						when AvgTime >= 40 and AvgTime < 50 then '40-50'
						when AvgTime >= 50 and AvgTime < 60 then '50-60'
						else '>60' end,
    gcd.EstCutDate,
    gcd.CuttingOutputDate,
	Item = isnull(r.Item,'')
	,PanelNo = isnull(r.PanelNo,'')
	,CutCellID = isnull(r.CutCellID,'')
    ,SpreadingNo = isnull(r.SpreadingNo,'')
    ,[LastSewDate] = tsi.LastSewDate
    ,[SewQty] = isnull(tsi.SewQty,0)
from @result r
left join @GetCutDateTmp gcd on r.[Cut Ref#] = gcd.[Cut Ref#] and r.M = gcd.M 
left join @SewingInfo tsi on tsi.OrderId =   r.[SP#] and 
                                tsi.Article = r.[Article]     and
                                tsi.SizeCode  = r.[Size]   and
                                (tsi.ComboType = r.BundleLocation or tsi.ComboType = r.Pattern)
where 1 = 1 
order by [Bundleno],[Sub-process],[RFIDProcessLocationID] 

return

end