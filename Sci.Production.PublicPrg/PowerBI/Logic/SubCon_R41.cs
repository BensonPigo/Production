using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class SubCon_R41
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public SubCon_R41()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSubprocessWIPData(SubCon_R41_ViewModel model)
        {
            Base_ViewModel base_ViewModel = new Base_ViewModel();
            List<SqlParameter> listPar = new List<SqlParameter>();
            if (model.IsPowerBI)
            {
                listPar = new List<SqlParameter>
                {
                    new SqlParameter("@BIEditDate",  SqlDbType.VarChar, 20) { Value = Convert.ToDateTime(model.BIEditDate).ToString("yyyy/MM/dd") },
                };
            }
            else
            {
                listPar = new List<SqlParameter>
                {
                    new SqlParameter("@CutRefNo1", SqlDbType.VarChar, 50) { Value = model.CutRefNo1 },
                    new SqlParameter("@CutRefNo2", SqlDbType.VarChar, 50) { Value = model.CutRefNo2 },
                    new SqlParameter("@sp1", SqlDbType.VarChar, 20) { Value = model.SP1 },
                    new SqlParameter("@MDivisionID", SqlDbType.VarChar, 10) { Value = model.MDivisionID },
                    new SqlParameter("@FactoryID", SqlDbType.VarChar, 10) { Value = model.FactoryID },
                    new SqlParameter("@ProcessLocation", SqlDbType.VarChar, 20) { Value = model.ProcessLocation },
                    new SqlParameter("@BIEditDate",  SqlDbType.VarChar, 20) { Value = Convert.ToDateTime(model.BIEditDate).ToString("yyyy/MM/dd") },
                    new SqlParameter("@LastSewDateFrom", SqlDbType.VarChar, 20) { Value = Convert.ToDateTime(model.LastSewDate1).ToString("yyyy/MM/dd") },
                    new SqlParameter("@LastSewDateTo", SqlDbType.VarChar, 20) { Value = Convert.ToDateTime(model.LastSewDate2).ToString("yyyy/MM/dd") },
                };
            }

            #region Append畫面上的條件
            StringBuilder sqlWhere = new StringBuilder();
            StringBuilder sqlWhereFirstQuery = new StringBuilder();
            string joinWorkOrder = string.Empty;
            string whereExistsLastSewDate = string.Empty;
            string whereSewDate = string.Empty;
            string whereBI = string.Empty;
            if (model.IsPowerBI)
            {
                whereBI = $@"
 and 
(
    b.Adddate >= @BIEditDate
    or b.EditDate >= @BIEditDate
    or bio.InComing >= @BIEditDate
    or bio.OutGoing >= @BIEditDate
)";
            }
            else
            {
                if (!MyUtility.Check.Empty(model.SubProcessList))
                {
                    sqlWhere.Append($@" and s.id in ('{model.SubProcessList.ToString().Replace(",", "','")}') ");
                }

                if (!MyUtility.Check.Empty(model.CutRefNo1))
                {
                    joinWorkOrder = "inner join WorkorderForOutput w WITH (NOLOCK, index(IDX_WorkOrderForOutput_CutRef)) on b.CutRef = w.CutRef ";
                    sqlWhereFirstQuery.Append($@" and w.CutRef >= @CutRefNo1 ");
                }

                if (!MyUtility.Check.Empty(model.CutRefNo2))
                {
                    joinWorkOrder = "inner join WorkorderForOutput w WITH (NOLOCK, index(IDX_WorkOrderForOutput_CutRef)) on b.CutRef = w.CutRef ";
                    sqlWhereFirstQuery.Append($@" and w.CutRef <= @CutRefNo2 ");
                }

                if (!MyUtility.Check.Empty(model.SP1))
                {
                    sqlWhereFirstQuery.Append($@" and BDO.BundleNo in (Select BundleNo from Bundle_Detail_Order Where OrderID = @sp1)");
                }

                if (!MyUtility.Check.Empty(model.BundleCDate1))
                {
                    sqlWhereFirstQuery.Append($@" and b.Cdate >= '{Convert.ToDateTime(model.BundleCDate1).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.BundleCDate2))
                {
                    sqlWhereFirstQuery.Append($@" and b.Cdate <= '{Convert.ToDateTime(model.BundleCDate2).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.BundleScanDate1) && !MyUtility.Check.Empty(model.BundleScanDate2))
                {
                    sqlWhere.Append($@" and ((convert (date, bio.InComing) >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}' and convert (date, bio.InComing) <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}' ) or (convert (date, bio.OutGoing) >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}' and convert (date, bio.OutGoing) <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}'))");
                    sqlWhereFirstQuery.Append($@" and ((convert (date, bio.InComing) >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}' and convert (date, bio.InComing) <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}' ) or (convert (date, bio.OutGoing) >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}' and convert (date, bio.OutGoing) <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}'))");
                }
                else
                {
                    if (!MyUtility.Check.Empty(model.BundleScanDate1))
                    {
                        sqlWhere.Append($@" and (convert (date, bio.InComing)  >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}' or convert (date, bio.OutGoing) >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}')");
                        sqlWhereFirstQuery.Append($@" and (convert (date, bio.InComing)  >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}' or convert (date, bio.OutGoing) >= '{Convert.ToDateTime(model.BundleScanDate1).ToString("yyyy/MM/dd")}')");
                    }

                    if (!MyUtility.Check.Empty(model.BundleScanDate2))
                    {
                        sqlWhere.Append($@" and (convert (date, bio.InComing)  <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}' or convert (date, bio.OutGoing) <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}')");
                        sqlWhereFirstQuery.Append($@" and (convert (date, bio.InComing)  <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}' or convert (date, bio.OutGoing) <= '{Convert.ToDateTime(model.BundleScanDate2).ToString("yyyy/MM/dd")}')");
                    }
                }

                if (!MyUtility.Check.Empty(model.MDivisionID))
                {
                    sqlWhereFirstQuery.Append(@" and b.MDivisionid = @MDivisionID");
                }

                if (!MyUtility.Check.Empty(model.FactoryID))
                {
                    sqlWhereFirstQuery.Append(@" and o.FtyGroup = @FactoryID");
                }

                if (model.ProcessLocation != "ALL")
                {
                    sqlWhere.Append(@" and isnull(bio.RFIDProcessLocationID,'') = @ProcessLocation");
                    sqlWhereFirstQuery.Append(@" and isnull(bio.RFIDProcessLocationID,'') = @ProcessLocation");
                }

                if (!MyUtility.Check.Empty(model.BuyerDelDate1))
                {
                    sqlWhereFirstQuery.Append($@" and o.BuyerDelivery >= convert(date,'{Convert.ToDateTime(model.BuyerDelDate1).ToString("yyyy/MM/dd")}')");
                }

                if (!MyUtility.Check.Empty(model.BuyerDelDate2))
                {
                    sqlWhereFirstQuery.Append($@" and o.BuyerDelivery <= convert(date,'{Convert.ToDateTime(model.BuyerDelDate2).ToString("yyyy/MM/dd")}')");
                }

                if (!MyUtility.Check.Empty(model.SewingInlineDate1))
                {
                    sqlWhereFirstQuery.Append($@" and o.SewInLine >= convert(date,'{Convert.ToDateTime(model.SewingInlineDate1).ToString("yyyy/MM/dd")}')");
                }

                if (!MyUtility.Check.Empty(model.SewingInlineDate2))
                {
                    sqlWhereFirstQuery.Append($@" and o.SewInLine <= convert(date,'{Convert.ToDateTime(model.SewingInlineDate2).ToString("yyyy/MM/dd")}')");
                }

                if (!MyUtility.Check.Empty(model.EstCuttingDate1))
                {
                    joinWorkOrder = "inner join WorkorderForOutput w WITH (NOLOCK, index(IDX_WorkOrderForOutput_CutRef)) on b.CutRef = w.CutRef ";
                    sqlWhereFirstQuery.Append($@" and w.EstCutDate >= convert(date,'{Convert.ToDateTime(model.EstCuttingDate1).ToString("yyyy/MM/dd")}')");
                }

                if (!MyUtility.Check.Empty(model.EstCuttingDate2))
                {
                    joinWorkOrder = "inner join WorkorderForOutput w WITH (NOLOCK, index(IDX_WorkOrderForOutput_CutRef)) on b.CutRef = w.CutRef ";
                    sqlWhereFirstQuery.Append($@" and w.EstCutDate <= convert(date,'{Convert.ToDateTime(model.EstCuttingDate2).ToString("yyyy/MM/dd")}')");
                }

                if (!MyUtility.Check.Empty(model.EstCuttingDate1) || !MyUtility.Check.Empty(model.EstCuttingDate2))
                {
                    sqlWhereFirstQuery.Append(@" and w.CutRef <> '' ");
                }

                if (!MyUtility.Check.Empty(model.LastSewDate1))
                {
                    whereExistsLastSewDate = $@" and exists(select 1 from View_SewingInfoLocation vsis with (nolock) where vsis.OrderID = o.ID and vsis.LastSewDate between @LastSewDateFrom and @LastSewDateTo)";
                    whereSewDate = " and (tsi.LastSewDate between @LastSewDateFrom and @LastSewDateTo)";
                }
            }
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
select distinct bd.BundleNo,
                bd.Location,
                b.Article,
                b.ColorID,
                bd.SizeCode,
                bd.Patterncode,
                BDO.OrderID
into #tmp_Workorder
from Bundle b WITH (NOLOCK)
{joinWorkOrder}
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id 
INNER JOIN Bundle_Detail_Order BDO with(nolock) on BDO.BundleNo = BD.BundleNo
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno --and bio.SubProcessId = s.Id
where 1=1
{sqlWhereFirstQuery} 
{whereExistsLastSewDate}
{whereBI}

--取得Last Sew. Date相關資料
select  vsl.OrderId, vsl.Article, vsl.SizeCode, vsl.ComboType, vsl.LastSewDate, vsl.SewQty
into #tmpSewingInfo
from dbo.View_SewingInfoLocation vsl with (nolock)
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
        'ALLPARTS',
        [LastSewDate] = max(LastSewDate), 
        [SewQty] = sum(SewQty)
from #tmpSewingInfo
group by OrderId, Article, SizeCode
");

            sqlCmd.Append($@"
Select 
    [Bundleno] = bd.BundleNo,
    [RFIDProcessLocationID] = isnull(bio.RFIDProcessLocationID,''),
    [EXCESS] = iif(b.IsEXCESS = 0, '','Y'),
    [Cut Ref#] = isnull(b.CutRef,''),
    [SP#] = w.Orderid,
	sps =iif((select count(1) from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo) = 1
		, (select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo)
		, dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = bd.BundleNo order by OrderID for XML RAW))),
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
    [Artwork] = isnull(sub.sub,''),
    [Qty] = bd.Qty,
    [Sub-process] = isnull(s.Id,''),
    [Post Sewing SubProcess]= iif(ps.sub = 1,N'✔',''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N'✔',''),
    bio.LocationID,
    b.Cdate,
    o.BuyerDelivery,
    o.SewInLine,
    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing,
    [POSupplier] = iif(PoSuppFromOrderID.Value = '',PoSuppFromPOID.Value,PoSuppFromOrderID.Value),
    [AllocatedSubcon]=isnull(Stuff((					
					select concat(',',ls.abb)
					from order_tmscost ot
					inner join LocalSupp ls on ls.id = ot.LocalSuppID
					 where ot.id = o.id and ot.ArtworkTypeID=s.ArtworkTypeId 
					for xml path('')
					),1,1,''),''),
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
into #result
from #tmp_Workorder w 
inner join Bundle_Detail bd WITH (NOLOCK, Index(PK_Bundle_Detail)) on bd.BundleNo = w.BundleNo 
inner join Bundle b WITH (NOLOCK, index(PK_Bundle)) on b.ID = bd.ID
inner join orders o WITH (NOLOCK) on o.Id = w.OrderId and o.MDivisionID  = b.MDivisionID 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
outer apply(
    select s.ID,s.InOutRule,s.ArtworkTypeId
    from SubProcess s
        where exists (
                        select 1 from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or  (s.IsSelection = 0 AND s.IsRFIDDefault = 1)
) s
left join BundleInOut bio WITH (NOLOCK, index(PK_BundleInOut)) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub 
outer apply(
    select sub = 1
    from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = s.ID
) as ps
outer apply(
    select top 1 sub = 1
    from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    --and bda.SubprocessId = s.ID
) as nbs 
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from ArtworkPO ap with (nolock)
	                                                            inner join ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = w.OrderId 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromOrderID
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' and isnull(PoSuppFromOrderID.Value,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from ArtworkPO ap with (nolock)
	                                                            inner join ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = o.POID 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromPOID
outer apply(
	 select SpreadingNo = stuff((
		    Select distinct concat(',', wo.SpreadingNoID)
		    from WorkOrderForOutput wo WITH (NOLOCK, Index(IDX_WorkOrderForOutput_CutRef)) 
		    where   wo.CutRef = b.CutRef 
                    and wo.ID = b.POID
            and wo.SpreadingNoID is not null
            and wo.SpreadingNoID != ''
		    for xml path('')
	    ),1,1,'')
)wk
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME 
	FROM dropdownlist DD 
	OUTER apply(
			SELECT OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM order_colorcombo OCC WITH (NOLOCK)
			INNER JOIN order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = b.poid 
		AND LIST.patternpanel = b.patternpanel 
		AND DD.[type] = 'FabricKind' 
		AND DD.id = LIST.kind 
)FabricKind
outer apply(
    select [InspectionDate] = MAX(InspectionDate)
    from SubProInsRecord sp with(nolock)
    where sp.Bundleno = bd.Bundleno
    and sp.SubProcessID = s.ID
)sp
where 1=1
{sqlWhere}
");

            sqlCmd.Append($@"
select	r.[Cut Ref#],
		r.M,
		[EstCutDate] = MAX(w.EstCutDate),
		[CuttingOutputDate] = MAX(co.cDate)
into #tmpGetCutDateTmp
from #result r
inner join WorkOrderForOutput w with (nolock) on w.CutRef = r.[Cut Ref#] and w.id = r.[Master SP#]
left join CuttingOutput_Detail cod with (nolock) on cod.WorkOrderForOutputUkey = w.Ukey
left join CuttingOutput co  with (nolock) on co.ID = cod.ID
where r.[Cut Ref#] <> ''
group by r.[Cut Ref#],r.M

select
    [Bundleno] = isnull(r.[Bundleno],'') ,
    [RFIDProcessLocationID] = isnull(r.[RFIDProcessLocationID],''),
	[EXCESS] = isnull(r.[EXCESS],''),
	[FabricKind] = isnull(r.[FabricKind],''),
    [CutRef] = isnull(r.[Cut Ref#],'') ,
    [SP] = ISNULL(SP.val,''),
    [MasterSP] = isnull(r.[Master SP#],''),
    [M] = isnull(r.[M],''),
    [Factory] = isnull(Factory.val,''),
	[Category] = isnull(category.val,''),
	[Program] = isnull([Program].val,''),
    [Style] = isnull(r.[Style],''),
    [Season] = isnull(r.[Season],''),
    [Brand] = isnull(r.[Brand],''),
    [Comb] = isnull(r.[Comb],''),
    [Cutno] = isnull(r.Cutno,0),
	[FabPanelCode] = isnull(r.[Fab_Panel Code],''),
    [Article] = isnull(r.[Article],''),
    [Color] = isnull(r.[Color],''),
    [ScheduledLineID] = isnull(r.[Line],''),
    [ScannedLineID] = isnull(r.SewingLineID,''),
    [Cell] = isnull(r.[Cell],''),
    [Pattern] = isnull(r.[Pattern],''),
    [PtnDesc] = isnull(r.[PtnDesc],''),
    [Group] = isnull(r.[Group],0),
    [Size] = isnull(r.[Size],''),
    [Artwork] = isnull(r.[Artwork],''),
    [Qty] = isnull(r.[Qty],0),
    [SubprocessID] = isnull(r.[Sub-process],''),
    [PostSewingSubProcess] = isnull(r.[Post Sewing SubProcess],''),
    [NoBundleCardAfterSubprocess] = isnull(r.[No Bundle Card After Subprocess],''),
    [Location] = isnull(r.LocationID,''),
    [BundleCreateDate] = r.Cdate,
    [BuyerDeliveryDate] = BuyerDelivery.val,
    [SewingInline] = [SewInLine].val,
    [SubprocessQAInspectionDate] = r.[InspectionDate],
    [InTime] = r.[InComing],
    [OutTime] = r.[Out (Time)],
    [POSupplier] = isnull([POSupplier].val,''),
    [AllocatedSubcon] = [AllocatedSubcon].val,
	[AvgTime] = isnull(r.AvgTime,0),
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
    [EstimatedCutDate] = gcd.EstCutDate,
    [CuttingOutputDate] = gcd.CuttingOutputDate
	,[Item] = isnull(r.Item,'')
	,[PanelNo] = isnull(r.PanelNo,'')
	,[CutCellID] = isnull(r.CutCellID,'')
    ,[SpreadingNo] = isnull(r.SpreadingNo,'')
    ,[LastSewDate] = MAX(tsi.LastSewDate)
    ,[SewQty] = isnull(Sum(tsi.SewQty),0)
from #result r
left join #tmpGetCutDateTmp gcd on r.[Cut Ref#] = gcd.[Cut Ref#] and r.M = gcd.M 
left join #tmpSewingInfo tsi on tsi.OrderId =   r.[SP#] and 
                                tsi.Article = r.[Article]     and
                                tsi.SizeCode  = r.[Size]   and
                                (tsi.ComboType = r.BundleLocation or tsi.ComboType = r.Pattern)
outer apply(
	select val = Stuff((
		select concat(',',Category)
		from (
				select 	distinct Category
				from dbo.#result s
				where s.Bundleno = r.Bundleno
			) s
		for xml path ('')
	) , 1, 1, '')
) category
outer apply(
	select val = Stuff((
		select concat(',',[SP#])
		from (
				select 	distinct [SP#]
				from dbo.#result s
				where s.Bundleno = r.Bundleno
				and s.[Sub-process] = r.[Sub-process]
				and s.Artwork = r.Artwork 
				and s.Size = r.Size
			) s
		for xml path ('')
	) , 1, 1, '')
) SP
outer apply(
	select val = Stuff((
		select concat(',',val)
		from (
				select 	distinct val= case when isnull([POSupplier],'') != '' then CONCAT('(',[SP#],')','(',[POSupplier],')')
				else '' end
				from dbo.#result s
				where s.Bundleno = r.Bundleno
			) s
		for xml path ('')
	) , 1, 1, '')
) POSupplier
outer apply(
	select val = Stuff((
		select concat(',',val)
		from (
				select 	distinct val= case when isnull(s.BuyerDelivery,'') != '' then CONCAT('(',[SP#],')','(',BuyerDelivery,')')
				else '' end 
				from dbo.#result s
				where s.Bundleno = r.Bundleno
			) s
		for xml path ('')
	) , 1, 1, '')
) BuyerDelivery
outer apply(
	select val = Stuff((
		select concat(',',[AllocatedSubcon])
		from (
				select 	distinct [AllocatedSubcon]
				from dbo.#result s
				WHERE s.Bundleno = r.Bundleno and s.[Sub-process] = r.[Sub-process] and s.Artwork = r.Artwork and s.Size = r.Size
			) s
		for xml path ('')
	) , 1, 1, '')
) [AllocatedSubcon]
outer apply(
	select val = Stuff((
		select concat(',',factory)
		from (
				select 	distinct factory
				from dbo.#result s
				where s.Bundleno = r.Bundleno
			) s
		for xml path ('')
	) , 1, 1, '')
) Factory
outer apply(
	select val = Stuff((
		select concat(',',SewInLine)
		from (
				select 	distinct SewInLine
				from dbo.#result s
				where s.Bundleno = r.Bundleno
			) s
		for xml path ('')
	) , 1, 1, '')
) SewInLine
outer apply(
	select val = Stuff((
		select concat(',',Program)
		from (
				select 	distinct Program
				from dbo.#result s
				where s.Bundleno = r.Bundleno
			) s
		for xml path ('')
	) , 1, 1, '')
) Program
where 1 = 1 {whereSewDate}
group by r.Bundleno,
r.[RFIDProcessLocationID],r.[EXCESS],r.[FabricKind],r.[Cut Ref#],r.[Master SP#],r.[M],
r.[Style],r.[Season],r.[Brand],r.[Comb],r.Cutno,r.[Fab_Panel Code],r.[Article],r.[Color],r.[Line],
r.[Cell],r.[Pattern],r.[PtnDesc],r.[Group],r.[Size],r.[Artwork],r.[Qty],r.SewingLineID,
r.[Post Sewing SubProcess],r.[No Bundle Card After Subprocess],r.LocationID,r.Cdate,
r.[InspectionDate],r.[InComing],r.[Out (Time)],r.[POSupplier],r.AvgTime,
gcd.EstCutDate,gcd.CuttingOutputDate,r.Item,r.PanelNo,r.CutCellID,r.SpreadingNo,r.TimeRangeFail,
category.val,SP.val,POSupplier.val,BuyerDelivery.val,r.[Sub-process],[AllocatedSubcon].val,
Factory.val,Program.val,SewInLine.val
order by [Bundleno],[Sub-process],[RFIDProcessLocationID] 

drop table #result
drop table #tmp_Workorder
drop table #tmpGetCutDateTmp
drop table #tmpSewingInfo
");
            base_ViewModel.Result = this.DBProxy.Select("Production", sqlCmd.ToString(), listPar, out DataTable dtResult);
            if (!base_ViewModel.Result)
            {
                return base_ViewModel;
            }

            base_ViewModel.Dt = dtResult;
            return base_ViewModel;
        }
    }
}
