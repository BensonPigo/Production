Create PROCEDURE [dbo].[P_Import_Subcon_R41]
As
Begin

Set NoCount On;
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	declare @current_PMS_ServerName nvarchar(50) 
	= (
		select [value] = 
			CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
				 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
				 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
				 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
				 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
				 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
				 WHEN @current_ServerName= 'SQL' THEN 'MainServer' -- HXG
				 WHEN (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
				 WHEN @current_ServerName= 'TESTING\PH1' THEN 'TESTING\PH1' -- testing\ph1
			ELSE '' END
	)

Declare @ExecSQL1 NVarChar(MAX);
Declare @ExecSQL2 NVarChar(MAX);
Declare @ExecSQL3 NVarChar(MAX);
Declare @ExecSQL4 NVarChar(MAX);
Declare @ExecSQL5 NVarChar(MAX);
Set @ExecSQL1 = N'
select distinct bd.BundleNo,
                bd.Location,
                b.Article,
                b.ColorID,
                bd.SizeCode,
                bd.Patterncode,
                b.OrderID
into #tmp_Workorder
from ['+@current_PMS_ServerName+'].Production.dbo.Bundle b WITH (NOLOCK)

inner join ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id 
INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Order BDO with(nolock) on BDO.BundleNo = BD.BundleNo
inner join ['+@current_PMS_ServerName+'].Production.dbo.orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join ['+@current_PMS_ServerName+'].Production.dbo.factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join ['+@current_PMS_ServerName+'].Production.dbo.BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno --and bio.SubProcessId = s.Id
where 1=1
 and (
	b.Adddate >= DATEADD(Day, -7, GETDATE()) or
	b.EditDate >= DATEADD(Day, -7, GETDATE()) or
	bio.InComing >= DATEADD(Day, -7, GETDATE()) or
	bio.OutGoing >= DATEADD(Day, -7, GETDATE()) 
 )


--取得Last Sew. Date相關資料
select  vsl.OrderId, vsl.Article, vsl.SizeCode, vsl.ComboType, vsl.LastSewDate, vsl.SewQty
into #tmpSewingInfo
from ['+@current_PMS_ServerName+'].Production.dbo.View_SewingInfoLocation vsl with (nolock)
where   exists(select 1 from #tmp_Workorder tw 
                        where   tw.OrderID = vsl.OrderId and
                                tw.Article = vsl.Article and
                                tw.SizeCode = vsl.SizeCode and
                                tw.Location = vsl.ComboType)

alter table #tmpSewingInfo alter column ComboType varchar(8)

insert into #tmpSewingInfo(OrderId, Article, SizeCode, ComboType, LastSewDate, SewQty)
select  OrderId, 
        Article, 
        SizeCode,
        ''ALLPARTS'',
        [LastSewDate] = max(LastSewDate), 
        [SewQty] = sum(SewQty)
from #tmpSewingInfo
group by OrderId, Article, SizeCode



 
Select 
    [Bundleno] = bd.BundleNo,
    [RFIDProcessLocationID] = isnull(bio.RFIDProcessLocationID,''''),
    [EXCESS] = iif(b.IsEXCESS = 0, '''',''Y''),
    [Cut Ref#] = isnull(b.CutRef,''''),
    [SP#] = b.Orderid,
	sps =iif((select count(1) from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo) = 1
		, (select OrderID from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo)
		, dbo.GetSinglelineSP((select OrderID from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo order by OrderID for XML RAW))),
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
    [Post Sewing SubProcess]= iif(ps.sub = 1,N''?'',''''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N''?'',''''),
    bio.LocationID,
    b.Cdate,
    o.BuyerDelivery,
    o.SewInLine,'
Set @ExecSQL2 = N'
    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing,
    [POSupplier] = iif(PoSuppFromOrderID.Value = '''',PoSuppFromPOID.Value,PoSuppFromOrderID.Value),
    [AllocatedSubcon]=Stuff((					
					select concat('','',ls.abb)
					from ['+@current_PMS_ServerName+'].Production.dbo.order_tmscost ot
					inner join ['+@current_PMS_ServerName+'].Production.dbo.LocalSupp ls on ls.id = ot.LocalSuppID
					 where ot.id = o.id and ot.ArtworkTypeID=s.ArtworkTypeId 
					for xml path('''')
					),1,1,''''),
	AvgTime = case  when s.InOutRule = 1 then iif(bio.InComing is null, null, round(Datediff(Hour,isnull(b.Cdate,''''),isnull(bio.InComing,''''))/24.0,2))
					when s.InOutRule = 2 then iif(bio.OutGoing is null, null, round(Datediff(Hour,isnull(b.Cdate,''''),isnull(bio.OutGoing,''''))/24.0,2))
					when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then null
					when s.InOutRule = 3 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.InComing,''''),isnull(bio.OutGoing,''''))/24.0,2))
					when s.InOutRule = 4 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.OutGoing,''''),isnull(bio.InComing,''''))/24.0,2))
					end,
	TimeRangeFail = case	when s.InOutRule = 1 and bio.InComing is null then ''No Scan''
						when s.InOutRule = 2 and bio.OutGoing is null then ''No Scan''
						when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then ''No Scan''
						when s.InOutRule = 3 and (bio.OutGoing is null or bio.InComing is null) then ''Not Valid''
						when s.InOutRule = 4 and (bio.OutGoing is null or bio.InComing is null) then ''Not Valid''
						else '''' end,
	s.InOutRule
	,b.Item
	,bio.PanelNo
	,bio.CutCellID
	,[SpreadingNo] = wk.SpreadingNo
	,[FabricKind] = FabricKind.val
    ,[BundleLocation] = w.Location
    ,[InspectionDate] = sp.InspectionDate
into #result
from #tmp_Workorder w 
inner join ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail bd WITH (NOLOCK, Index(PK_Bundle_Detail)) on bd.BundleNo = w.BundleNo 
inner join ['+@current_PMS_ServerName+'].Production.dbo.Bundle b WITH (NOLOCK, index(PK_Bundle)) on b.ID = bd.ID
inner join ['+@current_PMS_ServerName+'].Production.dbo.orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join ['+@current_PMS_ServerName+'].Production.dbo.factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
outer apply(
    select s.ID,s.InOutRule,s.ArtworkTypeId
    from ['+@current_PMS_ServerName+'].Production.dbo.SubProcess s
        where exists (
                        select 1 from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or  (s.IsSelection = 0 AND s.IsRFIDDefault = 1)
) s
left join ['+@current_PMS_ServerName+'].Production.dbo.BundleInOut bio WITH (NOLOCK, index(PK_BundleInOut)) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
outer apply(
	    select sub= stuff((
		    Select distinct concat(''+'', bda.SubprocessId)
		    from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('''')
	    ),1,1,'''')
) as sub 
outer apply(
    select sub = 1
    from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = s.ID
) as ps
outer apply(
    select top 1 sub = 1
    from ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    --and bda.SubprocessId = s.ID
) as nbs '
Set @ExecSQL3 = N'
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'''') = '''' then Stuff((select distinct concat( '','',ls.Abb)
	                                                            from ['+@current_PMS_ServerName+'].Production.dbo.ArtworkPO ap with (nolock)
	                                                            inner join ['+@current_PMS_ServerName+'].Production.dbo.ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join ['+@current_PMS_ServerName+'].Production.dbo.LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = ''O'' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = b.OrderId 
	                                                            AND (ap.Status =''Approved'' OR (ap.Status =''Closed'' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('''')),1,1,'''')  
                    else '''' end
) PoSuppFromOrderID
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'''') = '''' and isnull(PoSuppFromOrderID.Value,'''') = '''' then Stuff((select distinct concat( '','',ls.Abb)
	                                                            from ['+@current_PMS_ServerName+'].Production.dbo.ArtworkPO ap with (nolock)
	                                                            inner join ['+@current_PMS_ServerName+'].Production.dbo.ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join ['+@current_PMS_ServerName+'].Production.dbo.LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = ''O'' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = o.POID 
	                                                            AND (ap.Status =''Approved'' OR (ap.Status =''Closed'' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('''')),1,1,'''')  
                    else '''' end
) PoSuppFromPOID
outer apply(
	 select SpreadingNo = stuff((
		    Select distinct concat('','', wo.SpreadingNoID)
		    from ['+@current_PMS_ServerName+'].Production.dbo.WorkOrder wo WITH (NOLOCK, Index(CutRefNo)) 
		    where   wo.CutRef = b.CutRef 
                    and wo.ID = b.POID
            and wo.SpreadingNoID is not null
            and wo.SpreadingNoID != ''''
		    for xml path('''')
	    ),1,1,'''')
)wk
outer apply(
	SELECT top 1 [val] = DD.id + ''-'' + DD.NAME 
	FROM ['+@current_PMS_ServerName+'].Production.dbo.dropdownlist DD 
	OUTER apply(
			SELECT OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM ['+@current_PMS_ServerName+'].Production.dbo.order_colorcombo OCC WITH (NOLOCK)
			INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = b.poid 
		AND LIST.patternpanel = b.patternpanel 
		AND DD.[type] = ''FabricKind'' 
		AND DD.id = LIST.kind 
)FabricKind
outer apply(
    select [InspectionDate] = MAX(InspectionDate)
    from ['+@current_PMS_ServerName+'].Production.dbo.SubProInsRecord sp with(nolock)
    where sp.Bundleno = bd.Bundleno
    and sp.SubProcessID = s.ID
)sp
 where 1=1  '
Set @ExecSQL4 = N'
;with GetCutDateTmp as
(
	select	r.[Cut Ref#],
			r.M,
			[EstCutDate] = MAX(w.EstCutDate),
			[CuttingOutputDate] = MAX(co.cDate)
	from #result r
	inner join ['+@current_PMS_ServerName+'].Production.dbo.WorkOrder w with (nolock) on w.CutRef = r.[Cut Ref#] and w.id = r.[Master SP#]
	left join ['+@current_PMS_ServerName+'].Production.dbo.CuttingOutput_Detail cod with (nolock) on cod.WorkOrderUkey = w.Ukey
	left join ['+@current_PMS_ServerName+'].Production.dbo.CuttingOutput co  with (nolock) on co.ID = cod.ID
    where r.[Cut Ref#] <> ''''
	group by r.[Cut Ref#],r.M
)
select
    [Bundleno] = isnull(r.[Bundleno],'''') ,
    [RFIDProcessLocationID] = isnull(r.[RFIDProcessLocationID],''''),
	[EXCESS] = isnull(r.[EXCESS],''''),
	[FabricKind] = isnull(r.[FabricKind],''''),
    [Cut Ref#] = isnull(r.[Cut Ref#],'''') ,
    [SP#] = isnull(r.sps,''''),
    [Master SP#] = isnull(r.[Master SP#],''''),
    [M] = isnull(r.[M],''''),
    [Factory] = isnull(r.[Factory],''''),
	[Category] = isnull(r.[Category],''''),
	[Program] = isnull(r.[Program],''''),
    [Style] = isnull(r.[Style],''''),
    [Season] = isnull(r.[Season],''''),
    [Brand] = isnull(r.[Brand],''''),
    [Comb] = isnull(r.[Comb],''''),
    [Cutno] = isnull(r.Cutno,0),
	[Fab_Panel Code] = isnull(r.[Fab_Panel Code],''''),
    [Article] = r.[Article],
    [Color] = isnull(r.[Color],''''),
    [Line] = isnull(r.[Line],''''),
    SewingLineID = isnull(r.SewingLineID,''''),
    [Cell] = isnull(r.[Cell],''''),
    [Pattern] = isnull(r.[Pattern],''''),
    [PtnDesc] = isnull(r.[PtnDesc],''''),
    [Group] = isnull(r.[Group],0),
    [Size] = isnull(r.[Size],''''),
    [Artwork] = isnull(r.[Artwork],''''),
    [Qty] = isnull(r.[Qty],0),
    [Sub-process] = isnull(r.[Sub-process],''''),
    [Post Sewing SubProcess] = isnull(r.[Post Sewing SubProcess],''''),
    [No Bundle Card After Subprocess] = isnull(r.[No Bundle Card After Subprocess],''''),
    [LocationID] = isnull(r.LocationID,''''),
    r.Cdate,
    r.[BuyerDelivery],
    r.[SewInLine],
    r.[InspectionDate],
    r.[InComing],
    r.[Out (Time)],
    [POSupplier] = isnull(r.[POSupplier],''''),
    [AllocatedSubcon] = isnull(r.[AllocatedSubcon],''''),
	AvgTime = isnull(r.AvgTime,0),
    [TimeRange] = case	when TimeRangeFail <> '''' then TimeRangeFail
                        when AvgTime < 0 then ''Not Valid''
						when AvgTime >= 0 and AvgTime < 1 then ''<1''
						when AvgTime >= 1 and AvgTime < 2 then ''1-2''
						when AvgTime >= 2 and AvgTime < 3 then ''2-3''
						when AvgTime >= 3 and AvgTime < 4 then ''3-4''
						when AvgTime >= 4 and AvgTime < 5 then ''4-5''
						when AvgTime >= 5 and AvgTime < 10 then ''5-10''
						when AvgTime >= 10 and AvgTime < 20 then ''10-20''
						when AvgTime >= 20 and AvgTime < 30 then ''20-30''
						when AvgTime >= 30 and AvgTime < 40 then ''30-40''
						when AvgTime >= 40 and AvgTime < 50 then ''40-50''
						when AvgTime >= 50 and AvgTime < 60 then ''50-60''
						else ''>60'' end,
    gcd.EstCutDate,
    gcd.CuttingOutputDate,
	Item = isnull(r.Item,'''')
	,PanelNo = isnull(r.PanelNo,'''')
	,CutCellID = isnull(r.CutCellID,'''')
    ,SpreadingNo = isnull(r.SpreadingNo,'''')
    ,[LastSewDate] = tsi.LastSewDate
    ,[SewQty] = isnull(tsi.SewQty,0)
into #tmpFinal
from #result r
left join GetCutDateTmp gcd on r.[Cut Ref#] = gcd.[Cut Ref#] and r.M = gcd.M 
left join #tmpSewingInfo tsi on tsi.OrderId =   r.[SP#] and 
                                tsi.Article = r.[Article]     and
                                tsi.SizeCode  = r.[Size]   and
                                (tsi.ComboType = r.BundleLocation or tsi.ComboType = r.Pattern)
where 1 = 1 
order by [Bundleno],[Sub-process],[RFIDProcessLocationID] '
Set @ExecSQL5 = N'

Merge P_SubprocessWIP t
using( select * from #tmpFinal) as s
	on t.Bundleno = s.Bundleno 
	and t.RFIDProcessLocationID = s.RFIDProcessLocationID 
	and t.Sp = s.[SP#] 
	and t.Pattern = s.Pattern
	and t.SubprocessID = s.[Sub-process]
WHEN MATCHED then UPDATE SET
      t.[EXCESS] = s.[EXCESS]
      ,t.[FabricKind] = s.[FabricKind]
      ,t.[CutRef] = s.[Cut Ref#]
      ,t.[Sp] = s.[SP#]
      ,t.[MasterSP] = s.[Master SP#]
      ,t.[M] = s.[M]
      ,t.[Factory] = s.[Factory]
      ,t.[Category] = s.[Category]
      ,t.[Program] = s.[Program]
      ,t.[Style] = s.[Style]
      ,t.[Season] = s.[Season]
      ,t.[Brand] = s.[Brand]
      ,t.[Comb] = s.[Comb]
      ,t.[CutNo] = s.[CutNo]
      ,t.[FabPanelCode] = s.[Fab_Panel Code]
      ,t.[Article] = s.[Article]
      ,t.[Color] = s.[Color]
      ,t.[ScheduledLineID] = s.[Line]
      ,t.[ScannedLineID] = s.[SewingLineID]
      ,t.[Cell] = s.[Cell]
      ,t.[Pattern] = s.[Pattern]
      ,t.[PtnDesc] = s.[PtnDesc]
      ,t.[Group] = s.[Group]
      ,t.[Size] = s.[Size]
      ,t.[Artwork] = s.[Artwork]
      ,t.[Qty] = s.[Qty]
      ,t.[SubprocessID] = s.[Sub-process]
      ,t.[PostSewingSubProcess] = s.[Post Sewing SubProcess]
      ,t.[NoBundleCardAfterSubprocess] = s.[No Bundle Card After Subprocess]
      ,t.[Location] = s.[LocationID]
      ,t.[BundleCreateDate] = s.Cdate
      ,t.[BuyerDeliveryDate] = s.[BuyerDelivery]
      ,t.[SewingInline] = s.[SewInLine]
      ,t.[SubprocessQAInspectionDate] = s.[InspectionDate]
      ,t.[InTime] = s.[InComing]
      ,t.[OutTime] = s.[Out (Time)]
      ,t.[POSupplier] = s.[POSupplier]
      ,t.[AllocatedSubcon] = s.[AllocatedSubcon] 
      ,t.[AvgTime] = s.AvgTime
      ,t.[TimeRange] = s.[TimeRange]
      ,t.[EstimatedCutDate] = s.EstCutDate
      ,t.[CuttingOutputDate] = s.CuttingOutputDate
      ,t.[Item] = s.Item
      ,t.[PanelNo] = s.PanelNo
      ,t.[CutCellID] = s.CutCellID
      ,t.[SpreadingNo] = s.SpreadingNo
      ,t.[LastSewDate] = s.LastSewDate
      ,t.[SewQty] = s.SewQty
	  WHEN NOT MATCHED BY TARGET THEN
	  insert([Bundleno]   ,[RFIDProcessLocationID]   ,[EXCESS]  ,[FabricKind]  ,[CutRef]  ,[Sp]  ,[MasterSP]
      ,[M] ,[Factory],[Category],[Program],[Style],[Season],[Brand],[Comb],[CutNo],[FabPanelCode],[Article]
      ,[Color],[ScheduledLineID],[ScannedLineID],[Cell],[Pattern],[PtnDesc],[Group],[Size],[Artwork]
      ,[Qty],[SubprocessID],[PostSewingSubProcess],[NoBundleCardAfterSubprocess],[Location],[BundleCreateDate]
      ,[BuyerDeliveryDate],[SewingInline],[SubprocessQAInspectionDate],[InTime],[OutTime],[POSupplier]
      ,[AllocatedSubcon],[AvgTime],[TimeRange],[EstimatedCutDate],[CuttingOutputDate]
      ,[Item],[PanelNo],[CutCellID],[SpreadingNo],[LastSewDate],[SewQty])
	  values(    s.[Bundleno] ,s.[RFIDProcessLocationID] ,s.[EXCESS] ,s.[FabricKind] ,s.[Cut Ref#] ,
    s.[SP#] ,s.[Master SP#] ,s.[M] ,s.[Factory] ,s.[Category],s.[Program],s.[Style] ,s.[Season],
    s.[Brand],s.[Comb] ,s.[Cutno],s.[Fab_Panel Code] ,s.[Article] ,s.[Color] ,s.[Line] ,s.[SewingLineID] ,
    s.[Cell] ,s.[Pattern] ,s.[PtnDesc] ,s.[Group] ,s.[Size] ,s.[Artwork] ,s.[Qty] ,s.[Sub-process] ,
    s.[Post Sewing SubProcess] ,s.[No Bundle Card After Subprocess] ,s.[LocationID] ,s.Cdate,
    s.[BuyerDelivery],s.[SewInLine],s.[InspectionDate],s.[InComing],s.[Out (Time)],s.[POSupplier] ,
    s.[AllocatedSubcon] ,s.AvgTime ,s.[TimeRange],s.EstCutDate,s.CuttingOutputDate,	s.Item 
	,s.PanelNo	,s.CutCellID     ,s.SpreadingNo     ,s.[LastSewDate]     ,s.[SewQty] );


drop table #result,#tmp_Workorder,#tmpFinal,#tmpSewingInfo
';

	--print @ExecSQL1
	--PRINT @ExecSQL2
	--PRINT @ExecSQL3
	--PRINT @ExecSQL4
	--PRINT @ExecSQL5
Exec (@ExecSQL1+@ExecSQL2+@ExecSQL3+@ExecSQL4+@ExecSQL5);

if exists (select 1 from BITableInfo b where b.id = 'P_SubprocessWIP')
begin
	update b
		set b.TransferDate = getdate()
			, b.IS_Trans = 1
	from BITableInfo b
	where b.id = 'P_SubprocessWIP'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_SubprocessWIP', getdate())
end


END