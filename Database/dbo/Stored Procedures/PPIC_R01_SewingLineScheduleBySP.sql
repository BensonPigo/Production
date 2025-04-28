CREATE PROCEDURE [dbo].[PPIC_R01_SewingLineScheduleBySP]
     @MDivisionID varchar(8) = '',
	 @FactoryID varchar(8) = '',
	 @SewingLineIDFrom varchar(5) = '',
	 @SewingLineIDTo varchar(5) = '',
	 @SewingDateFrom date = null,
	 @SewingDateTo date = null,
	 @BuyerDeliveryFrom date = null,
	 @BuyerDeliveryTo date = null,
	 @SciDeliveryFrom date = null,
	 @SciDeliveryTo date = null,
	 @BrandID varchar(8) = '',
	 @SubProcess varchar(20) = '',
	 @sEditDate date = null,
	 @eEditDate date = null,
	 @IsPowerBI bit = 0
AS
BEGIN
	SET NOCOUNT ON;

Declare @tmpSewingScheduleID table(
    ID bigint
)

insert into @tmpSewingScheduleID
select s.ID
from SewingSchedule s WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
where   (s.MDivisionID = @MDivisionID or @MDivisionID = '') and
        (s.FactoryID = @FactoryID or @FactoryID = '') and
        (s.SewingLineID >= @SewingLineIDFrom or @SewingLineIDFrom = '') and
        (s.SewingLineID <= @SewingLineIDTo or @SewingLineIDTo = '') and
        (@SewingDateFrom is null or convert(date,s.Inline) >= @SewingDateFrom or (@SewingDateFrom between convert(date,s.Inline) and convert(date,s.Offline))) and
	    (@SewingDateTo is null or convert(date,s.Offline) <= @SewingDateTo or (@SewingDateTo between convert(date,s.Inline) and convert(date,s.Offline))) and
        (o.BuyerDelivery >= @BuyerDeliveryFrom or @BuyerDeliveryFrom is null) and
        (o.BuyerDelivery <= @BuyerDeliveryTo or @BuyerDeliveryTo is null) and
        (o.SciDelivery >= @SciDeliveryFrom or @SciDeliveryFrom is null) and
        (o.SciDelivery <= @SciDeliveryTo or @SciDeliveryTo is null) and
        (o.BrandID = @BrandID or @BrandID = '') and
        (@SubProcess = '' or exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = @SubProcess AND (st.Qty>0 or st.TMS>0 and st.Price>0) ))
		
/*
	因為訂單有做更新，但BI資料沒有做更動，導致沒一致
	效能太差需要與BI分開處理。
*/
IF (@IsPowerBI = 1)
BEGIN
	insert into @tmpSewingScheduleID
	select s.ID
	from SewingSchedule s WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
	WHERE NOT EXISTS(SELECT 1 FROM @tmpSewingScheduleID T WHERE T.ID = S.ID) AND
	o.EditDate >= @sEditDate AND o.EditDate <= @eEditDate
END

Declare @tmp_main table(
    SewingLineID varchar(5),
	MDivisionID varchar(8),
	FactoryID varchar(8),
	OrderID varchar(13),
	CustPONo varchar(30),
	ComboType varchar(1),
	SwitchToWorkorder varchar(12),
	Article nvarchar(max),
	SeasonID varchar(10),
	SizeCode varchar(8),
	CdCodeID varchar(6),
	StyleID varchar(15),
    CriticalStyle varchar(1),
	Qty int,
	AlloQty int,
	CutQty numeric(11,2),
	SewingQty int,
	ClogQty int,
	FirstCuttingOutputDate date,
	InspDate nvarchar(100),
	StandardOutput numeric(11,6),
	Efficiency numeric(10,2),
	KPILETA date,
	SewETA date,
	MTLETA date,
	MTLExport varchar(2),
	Inline datetime,
	Offline datetime,
	SciDelivery date,
	BuyerDelivery date,
	CRDDate date,
	CPU numeric(8,3),
	SewingCPU numeric(12,5),
	VasShas varchar(1),
	ShipModeList varchar(30),
	Alias varchar(30),
	Remark nvarchar(max),
	FtyGroup varchar(8),
	CDCodeNew varchar(5),
	ProductType nvarchar(100),
	MatchFabric varchar(1),
	FabricType nvarchar(100),
	Lining varchar(20),
	Gender varchar(10),
	Construction nvarchar(100),
	Category varchar(1),
    ID bigint
)
insert into @tmp_main(
    SewingLineID,
    MDivisionID,
    FactoryID,
    OrderID,
    CustPONo,
    ComboType,
    SwitchToWorkorder,
    Article,
    SeasonID,
    SizeCode,
    CdCodeID,
    StyleID,
    CriticalStyle,
    Qty,
    AlloQty,
    CutQty,
    SewingQty,
    ClogQty,
    FirstCuttingOutputDate,
    InspDate,
    StandardOutput,
    Efficiency,
    KPILETA,
	SewETA,
    MTLETA,
    MTLExport,
    Inline,
    Offline,
    SciDelivery,
    BuyerDelivery,
    CRDDate,
    CPU,
    SewingCPU,
    VasShas,
    ShipModeList,
    Alias,
    Remark,
    FtyGroup,
    CDCodeNew,
    ProductType,
    MatchFabric,
    FabricType,
    Lining,
    Gender,
    Construction,
    Category,
    ID
)
select  s.SewingLineID
            , s.MDivisionID
            , s.FactoryID
            , s.OrderID
            , o.CustPONo
            , s.ComboType
            ,[Switch to Workorder] = Iif(cutting.WorkType='1','Combination',Iif(cutting.WorkType='2','By SP#',''))
            , ( select CONCAT(Article,',') 
                from (  select distinct Article 
                        from SewingSchedule_Detail sd WITH (NOLOCK) 
                        where sd.ID = s.ID
                ) a for xml path('')) as Article            
            , o.SeasonID
            , [SizeCode] = ''
            , o.CdCodeID
            , o.StyleID
            , CriticalStyle=iif(st.CriticalStyle='1','Y','N')
            , o.Qty
            , s.AlloQty
            , isnull((select sum(Qty) 
                        from CuttingOutput_WIP c WITH (NOLOCK) 
                        where   c.OrderID = s.OrderID 
                                and c.Article in (  select Article 
                                                    from SewingSchedule_Detail sd WITH (NOLOCK) 
                                                    where sd.ID = s.ID)
                     ) ,0) as CutQty
            , isnull((  select sum(sod.QAQty) 
                        from    SewingOutput so WITH (NOLOCK) 
                                , SewingOutput_Detail sod WITH (NOLOCK) 
                        where   so.ID = sod.ID 
                                and so.SewingLineID = s.SewingLineID 
                                and sod.OrderId = s.OrderID 
                                and sod.ComboType = s.ComboType
                    ), 0) as SewingQty
            , isnull((  select sum(pd.ShipQty) 
                        from PackingList_Detail pd WITH (NOLOCK) 
                        where   pd.OrderID = s.OrderID 
                                and pd.ReceiveDate is not null
                     ), '') as ClogQty
			, [FirstCuttingOutputDate]=FirstCuttingOutputDate.Date
            , InspDate = InspctDate.Val
            , s.StandardOutput
            , [Eff] = case when (s.sewer * s.workhour) = 0 then 0
                      ELSE ROUND(CONVERT(float ,(sa.TTlAlloQty * s.TotalSewingTime) / (s.sewer * s.workhour * 3600)) * 100,2)
                      END
            , o.KPILETA
			, o.SewETA
            , o.MTLETA
            , o.MTLExport
            , s.Inline
            , s.Offline
            , o.SciDelivery
            , o.BuyerDelivery
			, o.CRDDate
            , o.CPU * o.CPUFactor * ( isnull(isnull(ol_rate.value,sl_rate.value), 100) / 100) as CPU
            , SewingCPU = convert(numeric(12,5), iif(isnull((SELECT StdTMS * 1.0 From System),0) = 0, 0, s.TotalSewingTime / (SELECT StdTMS * 1.0 From System)))
            , IIF(o.VasShas=1, 'Y', '') as VasShas
            , o.ShipModeList,isnull(c.Alias, '') as Alias 
            , isnull((  select CONCAT(Remark, ', ') 
                        from (  select s1.SewingLineID+'('+s1.ComboType+'):'+CONVERT(varchar,s1.AlloQty) as Remark 
                                from SewingSchedule s1 WITH (NOLOCK) 
                                where   s1.OrderID = s.OrderID 
                                        and s1.ID != s.ID
                        ) a for xml path('')
                    ), '') as Remark
            ,o.FtyGroup
	        ,[CDCodeNew] = sty.CDCodeNew
	        ,[ProductType] = sty.ProductType
            ,[MatchFabric] = iif(o.IsNotRepeatOrMapping = 0 ,'Y','N')
	        ,[FabricType] = sty.FabricType
	        ,[Lining] = sty.Lining
	        ,[Gender] = sty.Gender
	        ,[Construction] = sty.Construction
            ,o.Category
            ,s.ID
    from SewingSchedule s WITH (NOLOCK) 
    inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
    inner join Style st with (nolock) on st.Ukey = o.StyleUkey
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join cutting on cutting.ID =o.CuttingSP 
    outer apply(select value = dbo.GetOrderLocation_Rate(o.id,s.ComboType) ) ol_rate
    outer apply(select value = dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType) ) sl_rate
	OUTER APPLY(	
		SELECT [Date]=MIN(co2.cDate)
		FROM  WorkOrder_Distribute wd2 WITH (NOLOCK)
		INNER JOIN CuttingOutput_Detail cod2 WITH (NOLOCK) on cod2.WorkOrderUkey = wd2.WorkOrderUkey
		INNER JOIN CuttingOutput co2 WITH (NOLOCK) on co2.id = cod2.id and co2.Status <> 'New'
		where wd2.OrderID =o.ID
	)FirstCuttingOutputDate
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		    from Order_QtyShip oq
		    WHERE ID = o.id
		    FOR XML PATH('')
		),1,1,'')
	)InspctDate
    Outer apply (
	    SELECT s.[ID]
		    , ProductType = r2.Name
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
    OUTER APPLY(
        SELECT TTlAlloQty = SUM (AlloQty)
        FROM SewingSchedule WITH (NOLOCK)
        WHERE APSNo = s.APSNo
    ) sa
    where   s.ID in (select ID from @tmpSewingScheduleID)

-----------------------------------------------------------------
/*                          TempTable                          */
-----------------------------------------------------------------
Declare @tmp_WorkHour table(
    FactoryID varchar(8),
    SewingLineID varchar(5),
    Inline Date,
    Offline Date,
    Hours numeric(6, 1),
    ctn int
)

insert into @tmp_WorkHour(
    FactoryID ,
    SewingLineID ,
    Inline ,
    Offline ,
    Hours ,
    ctn
)
Select  w.FactoryID,
        w.SewingLineID,
        t.Inline,
        t.Offline
		,isnull(sum(w.Hours),0) as Hours
        , Count(w.Date) as ctn 
from WorkHour w WITH (NOLOCK) 
inner join (select distinct FtyGroup,SewingLineID,Convert(Date,Inline) Inline,Convert(Date,Offline)Offline from @tmp_main) t 
	on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Date between Inline and Offline
where w.Hours > 0 
group by w.FactoryID, w.SewingLineID, t.Inline, t.Offline
 
Declare @tmp_PFRemark table(
    id varchar(13),
    PFRemark varchar(max)
)

insert into @tmp_PFRemark
select id, Remark
from
(
	 Select s.Id, s.Remark, s.AddDate, ROW_NUMBER() over(PARTITION BY s.Id order by s.AddDate desc) r_id
     from Order_PFHis s WITH (NOLOCK) 
	 inner join @tmp_main t on s.Id = t.OrderID
) a
where a.r_id = 1

Declare @tmp_CutInLine table(
    OrderID varchar(13),
    CutInLine date
)

insert into @tmp_CutInLine
select 
wd.OrderID,
MIN(a.EstCutDate)
from WorkOrder_Distribute wd with (nolock)
inner join  WorkOrder a with (nolock) on a.Ukey = wd.WorkOrderUkey
where exists(select 1 from @tmp_main b where wd.OrderID = b.OrderID)
group by wd.OrderID


-----------------------------------------------------------------
/*                           原CTE                             */
-----------------------------------------------------------------
Declare @tmpArtWork table(
    ID varchar(13),
    Artwork varchar(15),
	INDEX IX_ID CLUSTERED(ID)
)

insert into @tmpArtWork
select  ot.ID,
        [Artwork] = at.Abbreviation + ':' + iif(ot.Qty > 0, Convert(varchar, ot.Qty), Convert(varchar,TMS))
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK) on  ot.ArtworkTypeID = at.ID
where   (ot.Price > 0 or at.Classify in ('O','I') )
        and (at.Classify in ('S','I') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''
		and exists(select 1 from @tmp_main b where ot.ID = b.OrderID)

Declare @tmpOrderArtwork table(
    ID varchar(13),
    Artwork nvarchar(max)
)

insert into @tmpOrderArtwork
select tmpArtWorkID.ID
        , Artwork = (select   CONCAT(Artwork,', ') 
						from @tmpArtWork 
						where ID = tmpArtWorkID.ID 
						order by Artwork for xml path(''))  
from (
	select distinct ID
	from @tmpArtWork
) tmpArtWorkID

Declare @tmp2P table(
    ID varchar(13),
    ColumnN varchar(50),
    val numeric(16, 4),
    Supp nvarchar(20)
)

insert into @tmp2P
select  ot.ID
		,ColumnN = RTRIM(at.ID) + ' ('+at.ArtworkUnit+')'
        , val =  iif(at.ArtworkUnit = 'PCS',isnull(ot.Qty,0),isnull(ot.Price,0) )
		, Supp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', ls.abb, ot.LocalSuppID), '')
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID and at.ID in('PRINTING','PRINTING PPU')
left join LocalSupp ls on ls.id = ot.LocalSuppID
where ot.id in (select orderid from @tmp_main)

Declare @tmp4P table(
    SubCon nvarchar(20),
    id varchar(13),
    [PRINTING (PCS)] numeric(16, 4),
    [PRINTING PPU (PPU)] numeric(16, 4)
)

insert into @tmp4P(id, [PRINTING (PCS)], [PRINTING PPU (PPU)])
select id,[PRINTING (PCS)],[PRINTING PPU (PPU)]
from (select id,ColumnN,val from @tmp2P) a
pivot(min(val) for ColumnN in ([PRINTING (PCS)],[PRINTING PPU (PPU)]))p


update t set SubCon = isnull(s.SubCon, '')
from @tmp4P t
outer apply(select top 1 SubCon = supp from @tmp2P where id = t.id and supp <> '') s


-----------------------------------------------------------------
/*                           Final                            */
-----------------------------------------------------------------
select  SewingLineID
        , MDivisionID
        , FactoryID
        , [SPNo] = OrderID
		, CustPONo
        , Category
        , ComboType
        , [SwitchToWorkorder]
        , [Colorway] = IIF(isnull(Article, '') = '', '', SUBSTRING(Article, 1, LEN(Article) - 1))
        , SeasonID
        , SizeCode
        , CdCodeID
	    , CDCodeNew
	    , ProductType
        , MatchFabric
	    , FabricType
	    , Lining
	    , Gender
	    , Construction
        , StyleID
        , a.CriticalStyle
        , [OrderQty] = Qty
        , AlloQty
        , CutQty
        , SewingQty
        , ClogQty
		, FirstCuttingOutputDate
        , [InspectionDate] = isnull(InspDate, '')
        , [TotalStandardOutput] = isnull(StandardOutput * WorkHour, 0)
        , [WorkHour] = isnull(WorkHour, 0)
        , [StandardOutputPerHour] = isnull(StandardOutput, 0)
        , [Efficiency] = isnull(a.Efficiency, 0)
        , KPILETA
        , PFRemark
		, SewETA
        , [ActMTLETA] = MTLETA
        , MTLExport
        , CutInLine
        , Inline
        , Offline
        , SciDelivery
        , BuyerDelivery
		, CRDDate
        , CPU
        , [SewingCPU] = isnull(SewingCPU, 0)
        , VasShas
        , ShipModeList
        , [Destination] = Alias
        , ArtWork
        , [Remarks] = IIF(Remark = '','',SUBSTRING(Remark,1,LEN(Remark)-1)) 
        , [TTL_PRINTING_PCS] = isnull(t5.[PRINTING (PCS)] * Qty, 0)
        , [TTL_PRINTING_PPU_PPU] = isnull(t5.[PRINTING PPU (PPU)] * Qty, 0)
        , [SubCon] = isnull(t5.SubCon, 0)
        , a.ID
from (
	select t.* 
			,isnull(pf.PFRemark,'') PFRemark
			,IIF(w.ctn = 0, 0,w.Hours/w.ctn) WorkHour
			, isnull(SUBSTRING(ta.Artwork, 1, LEN(ta.Artwork) - 1), '') as ArtWork 
            , tc.CutInLine
		from @tmp_main t
		left join @tmp_PFRemark pf on t.OrderID = pf.Id
		left join @tmp_WorkHour w on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Inline = Convert(Date,t.Inline) and w.Offline = Convert(Date,t.Offline) 
		left join @tmpOrderArtwork ta on ta.ID = t.OrderID 
        left join @tmp_CutInLine tc on tc.OrderID = t.OrderID
) a
left join @tmp4P t5 on t5.id = a.orderid
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID


end

go

