Create PROCEDURE [dbo].[P_Import_WIP]
As
Begin
	Set NoCount On;

declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
declare @current_PMS_ServerName nvarchar(50) = 'MainServer' 

Declare @ExecSQL1 NVarChar(MAX);
Declare @ExecSQL2 NVarChar(MAX);
Declare @ExecSQL3 NVarChar(MAX);
Declare @ExecSQL4 NVarChar(MAX);
Declare @ExecSQL5 NVarChar(MAX);
Declare @ExecSQL6 NVarChar(MAX);
Declare @ExecSQL7 NVarChar(MAX);
Declare @ExecSQL8 NVarChar(MAX);
Declare @ExecSQL9 NVarChar(MAX);
Declare @ExecSQL10 NVarChar(MAX);

Set @ExecSQL1 = N'
select o.MDivisionID       , o.FactoryID  , o.SciDelivery     , O.CRDDate           , O.CFMDate       , OrderID = O.ID    
	    , O.Dest            , O.StyleID    , O.SeasonID        , O.ProjectID         , O.Customize1    , O.BuyMonth
	    , O.CustPONo        , O.BrandID    , O.CustCDID        , O.ProgramID         , O.CdCodeID      , O.CPU
	    , O.Qty             , O.FOCQty     , O.PoPrice         , O.CMPPrice          , O.KPILETA       , O.LETA
	    , O.MTLETA          , O.SewETA     , O.PackETA         , O.MTLComplete       , O.SewInLine     , O.SewOffLine
        , O.CutInLine       , O.CutOffLine , O.Category        , O.IsForecast        , O.PulloutDate   , O.ActPulloutDate
	    , O.SMR             , O.MRHandle   , O.MCHandle        , O.OrigBuyerDelivery , O.DoxType       , O.TotalCTN
	    , O.FtyCTN          , O.ClogCTN    , O.VasShas         , O.TissuePaper       , O.MTLExport     , O.SewLine
	    , O.ShipModeList    , O.PlanDate   , O.FirstProduction , O.Finished          , O.FtyGroup      , O.OrderTypeID
	    , O.SpecialMark     , O.GFR        , O.SampleReason    , InspDate = QtyShip_InspectDate.Val     
		, O.MnorderApv      , O.FtyKPI	  , O.KPIChangeReason , O.StyleUkey		    , O.POID          , OrdersBuyerDelivery = o.BuyerDelivery
        , InspResult = QtyShip_Result.Val
        , InspHandle = QtyShip_Handle.Val
        , O.Junk,CFACTN=isnull(o.CFACTN,0)
        , InStartDate = Null, InEndDate = Null, OutStartDate = Null, OutEndDate = Null
        , s.CDCodeNew
        , sty.ProductType
        , sty.FabricType
        , sty.Lining
        , sty.Gender
        , sty.Construction
        , [StyleSpecialMark] = s.SpecialMark
        , o.IsBuyBack
		, [Cancelled but Sill] =	case when o.Junk = 1 then 
							case when o.NeedProduction = 1 then ''Y''
							when o.KeepPanels = 1 then ''K''
							else ''N'' end
						else '''' end
into #cte 
from ['+@current_PMS_ServerName+'].Production.dbo.Orders o WITH (NOLOCK) 
inner join ['+@current_PMS_ServerName+'].Production.dbo.factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join ['+@current_PMS_ServerName+'].Production.dbo.Style s on s.Ukey = o.StyleUkey
left join ['+@current_PMS_ServerName+'].Production.dbo.Pass1 WITH (NOLOCK) on Pass1.ID = O.InspHandle
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT  DISTINCT '',''+ Cast(CFAFinalInspectDate as varchar)
		from ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip oqs
		WHERE ID = o.id
		FOR XML PATH('''')
	),1,1,'''')
)QtyShip_InspectDate
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT  DISTINCT '',''+ CFAFinalInspectResult 
		from ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip oqs
		WHERE ID = o.id AND CFAFinalInspectResult <> '''' AND CFAFinalInspectResult IS NOT NULL
		FOR XML PATH('''')
	),1,1,'''')
)QtyShip_Result
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT  DISTINCT '',''+ CFAFinalInspectHandle +''-''+ p.Name
		from ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip oqs
		LEFT JOIN ['+@current_PMS_ServerName+'].Production.dbo.Pass1 p WITH (NOLOCK) ON oqs.CFAFinalInspectHandle = p.ID
		WHERE oqs.ID = o.id AND CFAFinalInspectHandle <> '''' AND CFAFinalInspectHandle IS NOT NULL
		FOR XML PATH('''')
	),1,1,'''')
)QtyShip_Handle
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
	FROM ['+@current_PMS_ServerName+'].Production.dbo.Style s WITH(NOLOCK)
	left join ['+@current_PMS_ServerName+'].Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
	left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
	left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
	where s.Ukey = o.StyleUkey
)sty
WHERE 1=1   and o.SciDelivery between DATEADD(DAY, -30, GETDATE()) and DATEADD(DAY, +30, GETDATE())
and o.Category in (''B'') and exists (select 1 from ['+@current_PMS_ServerName+'].Production.dbo.Factory where o.FactoryId = id and IsProduceFty = 1)
'
Set @ExecSQL2 = N'
-- 依撈出來的order資料(cte)去找各製程的WIP
SELECT X.OrderId
	   , firstSewingDate = min(X.OutputDate) 
       , lastestSewingDate = max(X.OutputDate) 
       , QAQTY = sum(X.QAQty) 
       , AVG_QAQTY = AVG(X.QAQTY)
into #tmp_SEWOUTPUT
from (
    SELECT b.OrderId, a.OutputDate
           , QAQty = sum(a.QAQty) 
    FROM ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput a WITH (NOLOCK) 
    inner join ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	inner join (select distinct OrderID from #cte) t on b.OrderId = t.OrderID 
    group by b.OrderId,a.OutputDate 
) X
group by X.OrderId

SELECT c.OrderID, MIN(a.cDate) first_cut_date
into #tmp_first_cut_date
from ['+@current_PMS_ServerName+'].Production.dbo.CuttingOutput a WITH (NOLOCK) 
inner join ['+@current_PMS_ServerName+'].Production.dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
inner join ['+@current_PMS_ServerName+'].Production.dbo.WorkOrder_Distribute c WITH (NOLOCK) on c.WorkOrderUkey = b.WorkOrderUkey
inner join (select distinct OrderID from #cte) t on c.OrderID = t.OrderID
group by c.OrderID

select pd.OrderId, pd.ScanQty
into #tmp_PackingList_Detail
from ['+@current_PMS_ServerName+'].Production.dbo.PackingList_Detail pd
inner join #cte t on pd.OrderID = t.OrderID

select t.OrderID
       , cut_qty = (SELECT SUM(CWIP.Qty) 
                    FROM ['+@current_PMS_ServerName+'].Production.dbo.CuttingOutput_WIP CWIP WITH (NOLOCK) 
                    WHERE CWIP.OrderID = T.OrderID)
	   ,f.first_cut_date
       , sewing_output = (select MIN(isnull(tt.qaqty,0)) 
                          from ['+@current_PMS_ServerName+'].Production.dbo.style_location sl WITH (NOLOCK) 
                          left join (
                                SELECT b.ComboType
                                       , qaqty = sum(b.QAQty)  
                                FROM ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput a WITH (NOLOCK) 
                                inner join ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                                where b.OrderId = t.OrderID
                                group by ComboType 
                          ) tt on tt.ComboType = sl.Location
                          where sl.StyleUkey = t.StyleUkey) 
       , t.StyleUkey
       , EMBROIDERY_qty = (select qty = min(qty)  
                           from (
                                select qty = sum(b.Qty) 
                                       , c.PatternCode
                                       , c.ArtworkID 
                                from ['+@current_PMS_ServerName+'].Production.dbo.farmin a WITH (NOLOCK) 
                                inner join ['+@current_PMS_ServerName+'].Production.dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                                right join (
                                    select distinct v.ArtworkTypeID
                                           , v.Article
                                           , v.ArtworkID
                                           , v.PatternCode 
                                   from ['+@current_PMS_ServerName+'].Production.dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                   where v.ID=t.OrderID
                                ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                       and c.PatternCode = b.PatternCode 
                                       and c.ArtworkID = b.ArtworkID
                                where a.ArtworkTypeId=''EMBROIDERY'' 
                                      and b.Orderid = t.OrderID
                                group by c.PatternCode,c.ArtworkID
                          ) x) 
						  '
Set @ExecSQL3 = N'
       , BONDING_qty = (select qty = min(qty)  
                        from (
                           select qty = sum(b.Qty)  
                                  , c.PatternCode
                                  , c.ArtworkID 
                           from ['+@current_PMS_ServerName+'].Production.dbo.farmin a WITH (NOLOCK) 
                           inner join ['+@current_PMS_ServerName+'].Production.dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from ['+@current_PMS_ServerName+'].Production.dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                where v.ID = t.OrderID
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId=''BONDING'' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode, c.ArtworkID
                       ) x) 
       , PRINTING_qty = (select qty = min(qty) 
                         from (
                           select qty = sum(b.Qty) 
                                  , c.PatternCode
                                  , c.ArtworkID 
                           from ['+@current_PMS_ServerName+'].Production.dbo.farmin a WITH (NOLOCK) 
                           inner join ['+@current_PMS_ServerName+'].Production.dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                           right join (
                                select distinct v.ArtworkTypeID
                                       , v.ArtworkID
                                       , v.PatternCode 
                                from ['+@current_PMS_ServerName+'].Production.dbo.View_Order_Artworks v  WITH (NOLOCK) 
                                where v.ID=t.OrderID
                           ) c on c.ArtworkTypeID = a.ArtworkTypeId 
                                  and c.PatternCode = b.PatternCode 
                                  and c.ArtworkID = b.ArtworkID
                           where a.ArtworkTypeId = ''PRINTING'' 
                                 and b.Orderid = t.OrderID
                           group by c.PatternCode, c.ArtworkID
                         ) x) 
       , s.firstSewingDate
	   , s.lastestSewingDate
	   , s.QAQTY
	   , s.AVG_QAQTY
into #cte2 
from #cte t
left join #tmp_first_cut_date f on t.OrderID = f.OrderID
left join #tmp_SEWOUTPUT s on t.OrderID = s.OrderID 

drop table #tmp_first_cut_date,#tmp_SEWOUTPUT

select sod.OrderID ,Max(so.OutputDate) LastSewnDate
into #imp_LastSewnDate
from ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput so WITH (NOLOCK) 
inner join ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
inner join #cte t on sod.OrderID = t.OrderID 
group by sod.OrderID 


select s.OrderID
    , s.SubProcessID
    , TransferTime = MAX(s.TransferTime)
into #tmp_SetQtyBySubprocess_Last
from ['+@current_PMS_ServerName+'].Production.dbo.SetQtyBySubprocess s WITH (NOLOCK)
where exists (select 1 from #cte t where s.OrderID = t.OrderID)
group by s.OrderID, s.SubProcessID


select s.OrderID, s.SubprocessID 
    , InQtyBySet = SUM(s.InQtyBySet)
	, OutQtyBySet = SUM(s.OutQtyBySet)
	, FinishedQtyBySet = SUM(s.FinishedQtyBySet)
into #tmp_SetQtyBySubprocess
'
Set @ExecSQL4 = N'
from 
(
    select s.OrderID
        , s.Article
        , s.SizeCode 
        , s.SubprocessID
	    , InQtyBySet = MIN(s.InQtyBySet)
	    , OutQtyBySet = MIN(s.OutQtyBySet)
	    , FinishedQtyBySet = MIN(s.FinishedQtyBySet)
    from ['+@current_PMS_ServerName+'].Production.dbo.SetQtyBySubprocess s WITH (NOLOCK)
    where exists (select 1 from #tmp_SetQtyBySubprocess_Last t where t.OrderID = s.OrderID and t.SubProcessID = s.SubProcessID and t.TransferTime = s.TransferTime)
    and SubProcessID in (''Sorting'',''Loading'',''Emb'',''BO'',''PRT'',''AT'',''PAD-PRT'',''SubCONEMB'',''HT'',''SewingLine'')
    group by s.OrderID, s.Article, s.SizeCode, s.SubprocessID 
)s
group by s.OrderID, s.SubprocessID 
select * into #Sorting from #tmp_SetQtyBySubprocess where SubprocessID = ''Sorting''
select * into #Loading from #tmp_SetQtyBySubprocess where SubprocessID = ''Loading''
select * into #Emb from #tmp_SetQtyBySubprocess where SubprocessID = ''Emb''
select * into #BO from #tmp_SetQtyBySubprocess where SubprocessID = ''BO''
select * into #PRT from #tmp_SetQtyBySubprocess where SubprocessID = ''PRT''
select * into #AT from #tmp_SetQtyBySubprocess where SubprocessID = ''AT''
select * into #PADPRT from #tmp_SetQtyBySubprocess where SubprocessID = ''PAD-PRT''
select * into #SubCONEMB from #tmp_SetQtyBySubprocess where SubprocessID = ''SubCONEMB''
select * into #HT from #tmp_SetQtyBySubprocess where SubprocessID = ''HT''
select * into #SewingLine from #tmp_SetQtyBySubprocess where SubprocessID = ''SewingLine''

select  MDivisionID = isnull(t.MDivisionID,'''')
       , FactoryID = isnull(t.FactoryID,'''')
       , SewLine = isnull(t.SewLine,'''')
       , [BuyerDelivery] = t.OrdersBuyerDelivery
       , t.SciDelivery
       , t.SewInLine
       , t.SewOffLine
       , [IDD] = isnull(CONVERT(nvarchar(100), IDD.val),'''')
       , BrandID = isnull(t.BrandID,'''')
       , [SPNO] = isnull(t.OrderID,'''')
       , [MasterSP] = isnull(t.POID,'''')
	   , [IsBuyBack] = IIF(T.IsBuyBack = 1 , ''Y'' , '''')
	   , [Cancelled] = IIF(t.Junk=1 ,''Y'' ,'''')
	   , [CancelledStillNeedProd] = isnull(T.[Cancelled but Sill],'''')
       , [Dest] = isnull(Country.Alias,'''')
       , StyleID = isnull(t.StyleID,'''')
       , OrderTypeID = isnull(t.OrderTypeID,'''')
       , [ShipMode] = isnull(t.ShipModeList,'''')
	   , [PartialShipping] = IIF( (SELECT COUNT(ID) FROM ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip WHERE ID = t.OrderID) >=2 ,''Y'' ,'''')
       , [OrderNo] = isnull(t.Customize1,'''')
       , [PONO] = isnull(t.CustPONo,'''')
	   , [ProgramID] = isnull(t.ProgramID,'''')
       --, t.CustCDID       
       , [CdCodeID] = isnull(t.CdCodeID,'''')
	   , [CDCodeNew] = isnull(t.CDCodeNew,'''')
	   , ProductType = isnull(t.ProductType,'''')
	   , FabricType = isnull(t.FabricType,'''')
	   , Lining = isnull(t.Lining,'''')
	   , Gender = isnull(t.Gender,'''')
	   , Construction = isnull(t.Construction,'''')
       , t.KPILETA
       , [SCHDLETA] = t.LETA
       , [ActMTLETA_Master SP] = t.MTLETA
       , [SewMTLETA_SP] = t.SewETA
       , [PkgMTLETA_SP] = t.PackETA
       , CPU = isnull(t.CPU,0)
	   , [TTLCPU] = isnull(t.CPU,0) * isnull(t.Qty,0)
	   , [CPUClosed]= isnull(t.CPU,0) * isnull(#cte2.sewing_output,0)
	   , [CPUbal]= isnull(t.CPU,0) * (isnull(t.qty,0) + isnull(t.FOCQty,0) - isnull(#cte2.sewing_output,0) )
       , [Article] = isnull((select article + '','' 
        from (
            select distinct q.Article  
            from ['+@current_PMS_ServerName+'].Production.dbo.Order_Qty q WITH (NOLOCK) 
            where q.ID = t.OrderID
        ) t 
        for xml path('''')) ,'''')
       , Qty = isnull(t.Qty,0)
       , [StandardOutput] = isnull(StandardOutput.StandardOutput,'''')
	   , OrigArtwork = isnull(ann.Artwork,'''')
	   , AddedArtwork = isnull(EXa.Artwork,'''')
       , [BundleArtwork] = isnull(Artwork.Artwork,'''')
       , [SubProcessDest] = isnull(spdX.SubProcessDest,'''')
	   	   '
Set @ExecSQL5 = N'
       , [EstCutDate] = EstCutDate.EstimatedCutDate
       , [1stCutDate] = #cte2.first_cut_date
       , [CutQty] = isnull(#cte2.cut_qty,0)
       , [RFIDCutQty] = isnull(#SORTING.OutQtyBySet,0)
       , [RFIDSewingLineInQty] = isnull(#SewingLine.InQtyBySet,0)
       , [RFIDLoadingQty] = isnull(#loading.InQtyBySet,0)
       , [RFIDEmbFarmInQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''EMBROIDERY'')),ISNULL( #Emb.InQtyBySet ,0) ,isnull(#Emb.InQtyBySet,0))
       , [RFIDEmbFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''EMBROIDERY'')),ISNULL( #Emb.OutQtyBySet ,0) ,isnull(#Emb.OutQtyBySet,0))
       , [RFIDBondFarmInQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''BO'')),ISNULL( #BO.InQtyBySet ,0) ,isnull(#BO.InQtyBySet,0))	
       , [RFIDBondFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''BO'')),ISNULL( #BO.OutQtyBySet ,0) ,isnull(#BO.OutQtyBySet,0))
       , [RFIDPrintFarmInQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''PRINTING'')),ISNULL( #prt.InQtyBySet,0) ,isnull(#prt.InQtyBySet,0))
       , [RFIDPrintFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''PRINTING'')),ISNULL( #prt.OutQtyBySet,0) ,isnull(#prt.OutQtyBySet,0))
       , [RFIDATFarmInQty] =IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''AT'')),ISNULL(#AT.InQtyBySet,0) ,isnull(#AT.InQtyBySet,0))
       , [RFIDATFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''AT'')),ISNULL(#AT.OutQtyBySet,0) ,isnull(#AT.OutQtyBySet,0))
       , [RFIDPadPrintFarmInQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''PAD PRINTING'')),ISNULL(#PADPRT.InQtyBySet ,0) ,isnull(#PADPRT.InQtyBySet,0))
       , [RFIDPadPrintFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''PAD PRINTING'')),ISNULL( #PADPRT.OutQtyBySet,0) ,isnull(#PADPRT.OutQtyBySet,0))
       , [RFIDEmbossDebossFarmInQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''EMBOSS/DEBOSS'')),ISNULL( #SUBCONEMB.InQtyBySet ,0) ,isnull(#SUBCONEMB.InQtyBySet,0))
       , [RFIDEmbossDebossFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''EMBOSS/DEBOSS'')),ISNULL( #SUBCONEMB.OutQtyBySet ,0) ,isnull(#SUBCONEMB.OutQtyBySet,0))
       , [RFIDHTFarmInQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''HEAT TRANSFER'')),ISNULL( #HT.InQtyBySet ,0) ,isnull(#HT.InQtyBySet,0))
       , [RFIDHTFarmOutQty] = IIF(EXISTS (SELECT 1 FROM [SplitString]( Artwork.Artwork , ''+'')  WHERE Data IN (''HEAT TRANSFER'')),ISNULL( #HT.OutQtyBySet ,0) ,isnull(#HT.OutQtyBySet,0))
       , SubProcessStatus=
			case when t.Junk = 1 then ''''
                 when #SORTING.OutQtyBySet is null and #loading.InQtyBySet is null 
                    and #SewingLine.InQtyBySet is null
                    and #Emb.InQtyBySet is null and #Emb.OutQtyBySet is null
                    and #BO.InQtyBySet is null and #BO.OutQtyBySet  is null 
                    and #prt.InQtyBySet  is null and #prt.OutQtyBySet  is null 
                    and #AT.InQtyBySet  is null and #AT.OutQtyBySet  is null 
                    and #PADPRT.InQtyBySet is null and #PADPRT.OutQtyBySet is null
                    and #SUBCONEMB.InQtyBySet is null and #SUBCONEMB.OutQtyBySet is null
                    and #HT.InQtyBySet is null and #HT.OutQtyBySet is null                
                then ''''
'
Set @ExecSQL6 = N'
				 when SORTINGStatus.v = 1 and loadingStatus.v = 1 --判斷有做加工段的數量=訂單qty,則為1,全部為1才為Y
					and SewingLineStatus.v = 1
					and Emb_i.v = 1 and Emb_o.v = 1
					and BO_i.v = 1 and BO_o.v = 1
					and prt_i.v = 1 and prt_o.v = 1
					and AT_i.v = 1 and AT_o.v = 1
					and PADPRT_i.v = 1 and PADPRT_o.v = 1
					and SUBCONEMB_i.v = 1 and SUBCONEMB_o.v = 1
					and HT_i.v = 1 and HT_o.v = 1
				 then ''Y''
				 else ''''
			end			
       , [EmbQty] = isnull(#cte2.EMBROIDERY_qty,0)
       , [BondQty] = isnull(#cte2.BONDING_qty,0)
       , [PrintQty] = isnull(#cte2.PRINTING_qty,0)
       , [SewQty] = isnull(#cte2.sewing_output,0)
       , [SewBal] = isnull(t.qty,0) + isnull(t.FOCQty,0) - isnull(#cte2.sewing_output,0) 
       , [1stSewDate] = #cte2.firstSewingDate
	   , [LastSewDate] = l.LastSewnDate
       , [AverageDailyOutput] = isnull(#cte2.AVG_QAQTY,0)
       , [EstOfflinedate] = DATEADD(DAY
                                 , iif(isnull(#cte2.AVG_QAQTY, 0) = 0, 0
                                                                     , ceiling((t.qty+t.FOCQty - #cte2.sewing_output) / (#cte2.AVG_QAQTY*1.0)))
                                 , #cte2.firstSewingDate) 
       , [ScannedQty] = isnull(PackDetail.ScanQty,0)
       , [PackedRate] = IIF(isnull(t.TotalCTN, 0) = 0, 0
                                                    , isnull(round(t.ClogCTN / (t.TotalCTN * 1.0), 4),0) * 100 ) 
       , [TTLCTN] = isnull(t.TotalCTN,0)
       , FtyCtn = isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0)
       , ClogCTN = isnull(t.ClogCTN,0)
       , CFACTN = isnull(t.CFACTN,0)
       , InspDate = isnull(t.InspDate,'''')
       , InspResult = isnull(InspResult,'''')
       , [CFAName] = isnull(InspHandle,'''')
       , [ActPulloutDate] = t.ActPulloutDate
       , [KPIDeliveryDate] = t.FtyKPI                
       , UpdateDeliveryReason = isnull(KPIChangeReason.KPIChangeReason ,'''') 
       , t.PlanDate
       , [SMR] = isnull(dbo.getTPEPass1(t.SMR),'''') 
       , [Handle] = isnull(dbo.getTPEPass1(T.MRHandle),'''') 
       , [POSMR] = isnull((select dbo.getTPEPass1(p.POSMR) 
                     from ['+@current_PMS_ServerName+'].Production.dbo.PO p WITH (NOLOCK) 
                     where p.ID = t.POID) ,'''')
       , [POHandle] = isnull((select dbo.getTPEPass1(p.POHandle) 
                        from ['+@current_PMS_ServerName+'].Production.dbo.PO p WITH (NOLOCK) 
                        where p.ID = t.POID),'''')
       , [MCHandle] = isnull(dbo.getTPEPass1(t.McHandle),'''') 
       , DoxType = isnull(t.DoxType,'''')
       , [SpecialMark] = isnull((select Name 
                        from ['+@current_PMS_ServerName+'].Production.dbo.Style_SpecialMark sp WITH(NOLOCK) 
                        where sp.ID = t.[StyleSpecialMark]
                        and sp.BrandID = t.BrandID
                        and sp.Junk = 0),'''') 
       , [GlobalFoundationRange] = isnull(t.GFR,0)
       , [SampleReason] = isnull(t.SampleReason,'''')
       , [TMS] = (select isnull(s.StdTms,0) * isnull(t.CPU ,0)
                  from ['+@current_PMS_ServerName+'].Production.dbo.System s WITH (NOLOCK)) 
into #tmpFinal
from #cte t 
inner join #cte2 on #cte2.OrderID = t.OrderID 
left join ['+@current_PMS_ServerName+'].Production.dbo.Country with (Nolock) on Country.id= t.Dest
left join #imp_LastSewnDate l on t.OrderID = l.OrderID
outer apply ( 
    select KPIChangeReason = ID + ''-'' + Name   
    from ['+@current_PMS_ServerName+'].Production.dbo.Reason  WITH (NOLOCK) 
    where ReasonTypeID = ''Order_BuyerDelivery'' 
          and ID = t.KPIChangeReason 
          and t.KPIChangeReason != '''' 
          and t.KPIChangeReason is not null 
) KPIChangeReason 
outer apply (SELECT val =  Stuff((select concat( '','',Format(oqs.IDD, ''yyyy/MM/dd''))   from ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip oqs with (nolock) where oqs.ID = t.OrderID and oqs.IDD is not null FOR XML PATH('''')),1,1,'''') 
  ) IDD

left join #Sorting on #Sorting.OrderID = t.OrderID
'
Set @ExecSQL7 = N'
left join #SewingLine on #SewingLine.OrderID = t.OrderID
left join #Loading on #Loading.OrderID = t.OrderID
left join #Emb on #Emb.OrderID = t.OrderID
left join #BO on #BO.OrderID = t.OrderID
left join #PRT on #PRT.OrderID = t.OrderID
left join #AT on #AT.OrderID = t.OrderID
left join #PADPRT on #PADPRT.OrderID = t.OrderID
left join #SUBCONEMB on #SUBCONEMB.OrderID = t.OrderID
left join #HT on #HT.OrderID = t.OrderID
outer apply(select v = case when #SORTING.OutQtyBySet is null or #SORTING.OutQtyBySet >= t.Qty then 1 else 0 end)SORTINGStatus--null即不用判斷此加工段 標記1, 數量=訂單數 標記1
outer apply(select v = case when #SewingLine.InQtyBySet is null or #SewingLine.InQtyBySet >= t.Qty then 1 else 0 end)SewingLineStatus
outer apply(select v = case when #loading.InQtyBySet is null or #loading.InQtyBySet >= t.Qty then 1 else 0 end)loadingStatus
outer apply(select v = case when #Emb.InQtyBySet is null or #Emb.InQtyBySet >= t.Qty then 1 else 0 end)Emb_i
outer apply(select v = case when #Emb.OutQtyBySet is null or #Emb.OutQtyBySet >= t.Qty then 1 else 0 end)Emb_o
outer apply(select v = case when #BO.InQtyBySet is null or #BO.InQtyBySet >= t.Qty then 1 else 0 end)BO_i
outer apply(select v = case when #BO.OutQtyBySet is null or #BO.OutQtyBySet >= t.Qty then 1 else 0 end)BO_o
outer apply(select v = case when #prt.InQtyBySet is null or #prt.InQtyBySet >= t.Qty then 1 else 0 end)prt_i
outer apply(select v = case when #prt.OutQtyBySet is null or #prt.OutQtyBySet >= t.Qty then 1 else 0 end)prt_o
outer apply(select v = case when #AT.InQtyBySet is null or #AT.InQtyBySet >= t.Qty then 1 else 0 end)AT_i
outer apply(select v = case when #AT.OutQtyBySet is null or #AT.OutQtyBySet >= t.Qty then 1 else 0 end)AT_o
outer apply(select v = case when #PADPRT.InQtyBySet is null or #PADPRT.InQtyBySet >= t.Qty then 1 else 0 end)PADPRT_i
outer apply(select v = case when #PADPRT.OutQtyBySet is null or #PADPRT.OutQtyBySet >= t.Qty then 1 else 0 end)PADPRT_o
outer apply(select v = case when #SUBCONEMB.InQtyBySet is null or #SUBCONEMB.InQtyBySet >= t.Qty then 1 else 0 end)SUBCONEMB_i
outer apply(select v = case when #SUBCONEMB.OutQtyBySet is null or #SUBCONEMB.OutQtyBySet >= t.Qty then 1 else 0 end)SUBCONEMB_o
outer apply(select v = case when #HT.InQtyBySet is null or #HT.InQtyBySet >= t.Qty then 1 else 0 end)HT_i
outer apply(select v = case when #HT.OutQtyBySet is null or #HT.OutQtyBySet >= t.Qty then 1 else 0 end)HT_o
outer apply(
	select StandardOutput =stuff((
		  select distinct concat('','',ComboType,'':'',StandardOutput)
		  from ['+@current_PMS_ServerName+'].Production.dbo.[SewingSchedule] WITH (NOLOCK) 
		  where orderid = t.OrderID 
		  for xml path('''')
	  ),1,1,'''')
)StandardOutput
outer apply(select PatternUkey from dbo.GetPatternUkey(t.POID,'''','''',t.StyleUkey,''''))gp
outer apply(
	select Artwork = STUFF((
		select CONCAT(''+'', ArtworkTypeId)
		from(
			select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='''',s1.ID,s1.ArtworkTypeId)
			from(
				SELECT bda1.SubprocessId
				FROM ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Order bd1 WITH (NOLOCK)
				INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
				WHERE bd1.Orderid=t.OrderID
	
				EXCEPT
				select s.Data
				from(
					Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
					from ['+@current_PMS_ServerName+'].Production.dbo.Pattern_GL a WITH (NOLOCK) 
					Where a.PatternUkey = gp.PatternUkey
					and a.Annotation <> ''''
				)x
				outer apply(select * from SplitString(x.Annotation ,''+''))s
			)x
			INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Subprocess s1 WITH (NOLOCK) ON s1.ID = x.SubprocessId
		)x
		order by ArtworkTypeID
		for xml path('''')
	),1,1,'''')
)EXa
outer apply('
Set @ExecSQL8 = N'
	select Artwork = stuff((
		select CONCAT(''+'',ArtworkTypeID)
		from(
			select distinct [ArtworkTypeId]=IIF(s1.ArtworkTypeId='''',s1.ID,s1.ArtworkTypeId)
			from(
				Select distinct Annotation = dbo.[RemoveNumericCharacters](a.Annotation)
				from ['+@current_PMS_ServerName+'].Production.dbo.Pattern_GL a WITH (NOLOCK) 
				Where a.PatternUkey = gp.PatternUkey
				and a.Annotation <> ''''
			)x
			outer apply(select * from SplitString(x.Annotation ,''+''))s
			INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Subprocess s1 WITH (NOLOCK) ON s1.ID = s.Data
		)x
		order by ArtworkTypeID
		for xml path('''')
	),1,1,'''')
)ann
outer apply(
	select Artwork =stuff((	
		select concat(''+'',ArtworkTypeID)
		from(
			SELECT DISTINCT [ArtworkTypeId]=IIF(s1.ArtworkTypeId='''',s1.ID,s1.ArtworkTypeId)
			FROM ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Order bd1 WITH (NOLOCK)
			INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
			INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Subprocess s1 WITH (NOLOCK) ON s1.ID=bda1.SubprocessId
			WHERE bd1.Orderid=t.OrderID
		)tmpartwork
		for xml path('''')
	),1,1,'''')
)Artwork
outer apply(
	select SubProcessDest = concat(''Inhouse:''+stuff((
		select concat('','',ot.ArtworkTypeID)
		from ['+@current_PMS_ServerName+'].Production.dbo.order_tmscost ot WITH (NOLOCK)
		inner join ['+@current_PMS_ServerName+'].Production.dbo.artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
		where ot.id = t.OrderID and ot.InhouseOSP = ''I''
		and artworktype.isSubprocess = 1
		for xml path('''')
	),1,1,'''')
	,''; ''+(
	select opsc=stuff((
		select concat(''; '',ospA.abb+'':''+ospB.spdO)
		from
		(
			select distinct abb = isnull(l.abb,'''')
			from ['+@current_PMS_ServerName+'].Production.dbo.order_tmscost ot WITH (NOLOCK)
			inner join ['+@current_PMS_ServerName+'].Production.dbo.artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
			left join ['+@current_PMS_ServerName+'].Production.dbo.localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
			where ot.id = t.OrderID and ot.InhouseOSP = ''o''
			and artworktype.isSubprocess = 1
		)ospA
		outer apply(
			select spdO = stuff((
				select concat('','',ot.ArtworkTypeID) 
				from ['+@current_PMS_ServerName+'].Production.dbo.order_tmscost ot WITH (NOLOCK)
				inner join ['+@current_PMS_ServerName+'].Production.dbo.artworktype WITH (NOLOCK) on ot.artworktypeid = artworktype.id 
				left join ['+@current_PMS_ServerName+'].Production.dbo.localsupp l WITH (NOLOCK) on l.id = ot.LocalSuppID
				where ot.id = t.OrderID and ot.InhouseOSP = ''o'' and isnull(l.Abb,'''') = ospA.abb
			    and artworktype.isSubprocess = 1
				for xml path('''')
			),1,1,'''')
		)ospB
		for xml path('''')
	),1,1,'''')))
)spdX
outer apply(select EstimatedCutDate = min(EstCutDate) from ['+@current_PMS_ServerName+'].Production.dbo.WorkOrder wo WITH (NOLOCK) where t.POID = wo.id)EstCutDate
outer apply(
    select ScanQty = sum(pd.ScanQty)
    from #tmp_PackingList_Detail pd
    where pd.OrderId = t.OrderID
)PackDetail
 order by t.orderid
 Merge P_WIP t
	using( select * from #tmpFinal) as s
	on t.SPNO = s.SPNO
	WHEN MATCHED then UPDATE SET
	   t.[MDivisionID]=s .[MDivisionID]
      ,t.[FactoryID] = s.[FactoryID]
      ,t.[SewLine] = s.[SewLine]
      ,t.[BuyerDelivery]=s.[BuyerDelivery]		
      ,t.[SciDelivery]=s.[SciDelivery]
      ,t.[SewInLine]=s.[SewInLine]
      ,t.[SewOffLine]=s.[SewOffLine]
      ,t.[IDD]=s.[IDD]
      ,t.[BrandID]=s.[BrandID]
      ,t.[MasterSP]=s.[MasterSP]
      ,t.[IsBuyBack]=s.[IsBuyBack]
      ,t.[Cancelled]=s.[Cancelled]
      ,t.[CancelledStillNeedProd]=s.[CancelledStillNeedProd]
      ,t.[Dest]=s.[Dest]
      ,t.[StyleID]=s.[StyleID]
      ,t.[OrderTypeID]=s.[OrderTypeID]
      ,t.[ShipMode]=s.[ShipMode]
      ,t.[PartialShipping]=s.[PartialShipping]
      ,t.[OrderNo]=s.[OrderNo]
      ,t.[PONO]=s.[PONO]
      ,t.[ProgramID]=s.[ProgramID]
      ,t.[CdCodeID]=s.[CdCodeID]
      ,t.[CDCodeNew]=s.[CDCodeNew]
      ,t.[ProductType]=s.[ProductType]	  '
Set @ExecSQL9 = N'
      ,t.[FabricType]=s.[FabricType]
      ,t.[Lining]=s.[Lining]
      ,t.[Gender]=s.[Gender]
      ,t.[Construction]=s.[Construction]
      ,t.[KPILETA]=s.[KPILETA]
      ,t.[SCHDLETA]=s.[SCHDLETA]
      ,t.[ActMTLETA_Master SP]=s.[ActMTLETA_Master SP]
      ,t.[SewMTLETA_SP]=s.[SewMTLETA_SP]
      ,t.[PkgMTLETA_SP]=s.[PkgMTLETA_SP]
      ,t.[Cpu]=s.[Cpu]
      ,t.[TTLCPU]=s.[TTLCPU]
      ,t.[CPUClosed]=s.[CPUClosed]
      ,t.[CPUBal]=s.[CPUBal]
      ,t.[Article]=s.[Article]
      ,t.[Qty]=s.[Qty]
      ,t.[StandardOutput]=s.[StandardOutput]
      ,t.[OrigArtwork]=s.[OrigArtwork]
      ,t.[AddedArtwork]=s.[AddedArtwork]
      ,t.[BundleArtwork]=s.[BundleArtwork]
      ,t.[SubProcessDest]=s.[SubProcessDest]
      ,t.[EstCutDate]=s.[EstCutDate]
      ,t.[1stCutDate]=s.[1stCutDate]
      ,t.[CutQty]=s.[CutQty]
      ,t.[RFIDCutQty]=s.[RFIDCutQty]
      ,t.[RFIDSewingLineInQty]=s.[RFIDSewingLineInQty]
      ,t.[RFIDLoadingQty]=s.[RFIDLoadingQty]
      ,t.[RFIDEmbFarmInQty]=s.[RFIDEmbFarmInQty]
      ,t.[RFIDEmbFarmOutQty]=s.[RFIDEmbFarmOutQty]
      ,t.[RFIDBondFarmInQty]=s.[RFIDBondFarmInQty]
      ,t.[RFIDBondFarmOutQty]=s.[RFIDBondFarmOutQty]
      ,t.[RFIDPrintFarmInQty]=s.[RFIDPrintFarmInQty]
      ,t.[RFIDPrintFarmOutQty]=s.[RFIDPrintFarmOutQty]
      ,t.[RFIDATFarmInQty]=s.[RFIDATFarmInQty]
      ,t.[RFIDATFarmOutQty]=s.[RFIDATFarmOutQty]
      ,t.[RFIDPadPrintFarmInQty]=s.[RFIDPadPrintFarmInQty]
      ,t.[RFIDPadPrintFarmOutQty]=s.[RFIDPadPrintFarmOutQty]
      ,t.[RFIDEmbossDebossFarmInQty]=s.[RFIDEmbossDebossFarmInQty]
      ,t.[RFIDEmbossDebossFarmOutQty]=s.[RFIDEmbossDebossFarmOutQty]
      ,t.[RFIDHTFarmInQty]=s.[RFIDHTFarmInQty]
      ,t.[RFIDHTFarmOutQty]=s.[RFIDHTFarmOutQty]
      ,t.[SubProcessStatus]=s.[SubProcessStatus]
      ,t.[EmbQty]=s.[EmbQty]
      ,t.[BondQty]=s.[BondQty]
      ,t.[PrintQty]=s.[PrintQty]
      ,t.[SewQty]=s.[SewQty]
      ,t.[SewBal]=s.[SewBal]
      ,t.[1stSewDate]=s.[1stSewDate]
      ,t.[LastSewDate]=s.[LastSewDate]
      ,t.[AverageDailyOutput]=s.[AverageDailyOutput]
      ,t.[EstOfflinedate]=s.[EstOfflinedate]
      ,t.[ScannedQty]=s.[ScannedQty]
      ,t.[PackedRate]=s.[PackedRate]
      ,t.[TTLCTN]=s.[TTLCTN]
      ,t.[FtyCTN]=s.[FtyCTN]
      ,t.[cLogCTN]=s.[cLogCTN]
      ,t.[CFACTN]=s.[CFACTN]
      ,t.[InspDate]=s.[InspDate]
      ,t.[InspResult]=s.[InspResult]
      ,t.[CFAName]=s.[CFAName]
      ,t.[ActPulloutDate]=s.[ActPulloutDate]
      ,t.[KPIDeliveryDate]=s.[KPIDeliveryDate]
      ,t.[UpdateDeliveryReason]=s.[UpdateDeliveryReason]
      ,t.[PlanDate]=s.[PlanDate]
      ,t.[SMR]=s.[SMR]
      ,t.[Handle]=s.[Handle]
      ,t.[Posmr]=s.[Posmr]
      ,t.[PoHandle]=s.[PoHandle]
      ,t.[MCHandle]=s.[MCHandle]
      ,t.[doxtype]=s.[doxtype]
      ,t.[SpecialMark]=s.[SpecialMark]
      ,t.[GlobalFoundationRange]=s.[GlobalFoundationRange]
      ,t.[SampleReason]=s.[SampleReason]
      ,t.[TMS]=s.[TMS]
	WHEN NOT MATCHED BY TARGET THEN
insert (
		[MDivisionID]
      ,[FactoryID],[SewLine] ,[BuyerDelivery],[SciDelivery],[SewInLine],[SewOffLine]
      ,[IDD],[BrandID],[SPNO],[MasterSP],[IsBuyBack],[Cancelled],[CancelledStillNeedProd]
      ,[Dest],[StyleID],[OrderTypeID],[ShipMode],[PartialShipping],[OrderNo],[PONO],[ProgramID],[CdCodeID]
      ,[CDCodeNew],[ProductType],[FabricType],[Lining],[Gender],[Construction],[KPILETA],[SCHDLETA]
      ,[ActMTLETA_Master SP],[SewMTLETA_SP],[PkgMTLETA_SP],[Cpu],[TTLCPU],[CPUClosed],[CPUBal],[Article]
      ,[Qty],[StandardOutput],[OrigArtwork],[AddedArtwork],[BundleArtwork],[SubProcessDest],[EstCutDate],[1stCutDate]
      ,[CutQty],[RFIDCutQty],[RFIDSewingLineInQty],[RFIDLoadingQty],[RFIDEmbFarmInQty],[RFIDEmbFarmOutQty],[RFIDBondFarmInQty],[RFIDBondFarmOutQty] '
Set @ExecSQL10 = N'
      ,[RFIDPrintFarmInQty],[RFIDPrintFarmOutQty],[RFIDATFarmInQty],[RFIDATFarmOutQty],[RFIDPadPrintFarmInQty]
      ,[RFIDPadPrintFarmOutQty],[RFIDEmbossDebossFarmInQty],[RFIDEmbossDebossFarmOutQty],[RFIDHTFarmInQty],[RFIDHTFarmOutQty]
      ,[SubProcessStatus],[EmbQty],[BondQty],[PrintQty],[SewQty],[SewBal],[1stSewDate],[LastSewDate]
      ,[AverageDailyOutput],[EstOfflinedate],[ScannedQty],[PackedRate],[TTLCTN],[FtyCTN],[cLogCTN]
      ,[CFACTN],[InspDate],[InspResult],[CFAName],[ActPulloutDate],[KPIDeliveryDate],[UpdateDeliveryReason]
      ,[PlanDate],[SMR],[Handle],[Posmr],[PoHandle],[MCHandle],[doxtype],[SpecialMark]
      ,[GlobalFoundationRange],[SampleReason],[TMS])
VALUES (
		  s.[MDivisionID]
      ,s.[FactoryID],s.[SewLine] ,s.[BuyerDelivery],s.[SciDelivery],s.[SewInLine],s.[SewOffLine]
      ,s.[IDD],s.[BrandID],s.[SPNO],s.[MasterSP],s.[IsBuyBack],s.[Cancelled],s.[CancelledStillNeedProd]
      ,s.[Dest],s.[StyleID],s.[OrderTypeID],s.[ShipMode],s.[PartialShipping],s.[OrderNo],s.[PONO],s.[ProgramID],s.[CdCodeID]
      ,s.[CDCodeNew],s.[ProductType],s.[FabricType],s.[Lining],s.[Gender],s.[Construction],s.[KPILETA],s.[SCHDLETA]
      ,s.[ActMTLETA_Master SP],s.[SewMTLETA_SP],s.[PkgMTLETA_SP],s.[Cpu],s.[TTLCPU],s.[CPUClosed],s.[CPUBal],s.[Article]
      ,s.[Qty],s.[StandardOutput],s.[OrigArtwork],s.[AddedArtwork],s.[BundleArtwork],s.[SubProcessDest],s.[EstCutDate],s.[1stCutDate]
      ,s.[CutQty],s.[RFIDCutQty],s.[RFIDSewingLineInQty],s.[RFIDLoadingQty],s.[RFIDEmbFarmInQty],s.[RFIDEmbFarmOutQty],s.[RFIDBondFarmInQty],s.[RFIDBondFarmOutQty]
      ,s.[RFIDPrintFarmInQty],s.[RFIDPrintFarmOutQty],s.[RFIDATFarmInQty],s.[RFIDATFarmOutQty],s.[RFIDPadPrintFarmInQty]
      ,s.[RFIDPadPrintFarmOutQty],s.[RFIDEmbossDebossFarmInQty],s.[RFIDEmbossDebossFarmOutQty],s.[RFIDHTFarmInQty],s.[RFIDHTFarmOutQty]
      ,s.[SubProcessStatus],s.[EmbQty],s.[BondQty],s.[PrintQty],s.[SewQty],s.[SewBal],s.[1stSewDate],s.[LastSewDate]
      ,s.[AverageDailyOutput],s.[EstOfflinedate],s.[ScannedQty],s.[PackedRate],s.[TTLCTN],s.[FtyCTN],s.[cLogCTN]
      ,s.[CFACTN],s.[InspDate],s.[InspResult],s.[CFAName],s.[ActPulloutDate],s.[KPIDeliveryDate],s.[UpdateDeliveryReason]
      ,s.[PlanDate],s.[SMR],s.[Handle],s.[Posmr],s.[PoHandle],s.[MCHandle],s.[doxtype],s.[SpecialMark]
      ,s.[GlobalFoundationRange],s.[SampleReason],s.[TMS])
;
 '
 	print @ExecSQL1
	PRINT @ExecSQL2
	PRINT @ExecSQL3
	PRINT @ExecSQL4
	PRINT @ExecSQL5
	PRINT @ExecSQL6
	PRINT @ExecSQL7
	PRINT @ExecSQL8
	print @ExecSQL9
	print @ExecSQL10
Exec (@ExecSQL1+@ExecSQL2+@ExecSQL3+@ExecSQL4+@ExecSQL5+@ExecSQL6+@ExecSQL7+@ExecSQL8+@ExecSQL9+@ExecSQL10);

if exists (select 1 from BITableInfo b where b.id = 'P_WIP')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_WIP'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_WIP', getdate())
end

end
GO


