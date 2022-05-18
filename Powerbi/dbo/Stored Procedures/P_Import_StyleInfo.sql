CREATE PROCEDURE [dbo].[P_Import_StyleInfo] 
	@SDate as date = null, 
	@EDate as date = null
AS

BEGIN

	if @SDate is null
	begin
		set @SDate = dateadd(day, -30, getdate())
	end

	if @EDate is null
	begin
		set @EDate = getdate()
	end
	

	select Ukey, id, BrandID , SeasonID
	into #tmp_Style
	from [TRADEDB].[Trade].dbo.[Style] s with(nolock)
	where (s.AddDate >= format(@SDate, 'yyyy/MM/dd 00:00:00')
		and s.AddDate <= format(@EDate, 'yyyy/MM/dd 23:59:59'))
	or (s.EditDate >= format(@SDate, 'yyyy/MM/dd 00:00:00')
		and s.EditDate <= format(@EDate, 'yyyy/MM/dd 23:59:59'))

	select i.StyleUKey
		, i.StyleID
		, i.BrandID
		, i.SeasonID
		, i.GSDType
		, IETMSUkey = MAX(i.Ukey)
	into #tmp_Last_IETMS
	from [TRADEDB].[Trade].dbo.IETMS i
	where exists (select 1 from #tmp_Style t where i.StyleUKey = t.Ukey and i.BrandID = t.BrandID and i.SeasonID = t.SeasonID)
	and i.Status in ('Completed', 'History') 
	group by i.StyleUKey, i.StyleID, i.BrandID, i.SeasonID, i.GSDType
	
	select *
	into #tmp_final
	from 
	(
		select [StyleID] = t.ID
			, t.BrandID
			, t.SeasonID
			, [GSD_CostingSMV] = Cast(sum(sq.TMS * 1.0 / 60) as numeric(16,3))
		from #tmp_Style t
		inner join [TRADEDB].[Trade].dbo.Style_Quotation sq with(nolock) on sq.StyleUkey = t.Ukey
		where t.BrandID = 'ADIDAS'
		and exists (select 1 from [TRADEDB].[Trade].dbo.ArtworkType art with(nolock) where art.id = sq.ArtworkTypeID and art.isTtlTMS=1)
		group by t.ID, t.BrandID, t.SeasonID

		union all

		select i.StyleID
			, i.BrandID
			, i.SeasonID
			, [GSD_CostingSMV] = Cast(sum(iif(i.GSDType = 'C', iSummary.ProSMV, 0)) as numeric(16,3))
		from [TRADEDB].[Trade].dbo.IETMS_Summary iSummary with(nolock)
		inner join #tmp_Last_IETMS i with(nolock) on iSummary.IETMSUkey = i.IETMSUkey
		inner Join [TRADEDB].[Trade].dbo.ArtworkType art with(nolock) on art.ID = iSummary.ArtworkTypeID
		where exists (select 1 from #tmp_Style t where i.StyleUKey = t.Ukey and i.BrandID = t.BrandID and i.SeasonID = t.SeasonID)
		and i.BrandID <> 'ADIDAS'
		and art.isTtlTMS = 1
		group by i.StyleID, i.BrandID, i.SeasonID 
	)a
	where a.GSD_CostingSMV <> 0

	insert into P_StyleInfo(StyleID, BrandID, SeasonID, GSD_CostingSMV)
	select t.StyleID, t.BrandID, t.SeasonID, t.GSD_CostingSMV
	from #tmp_final t
	where not exists (select 1 from P_StyleInfo p where t.StyleID = p.StyleID and t.BrandID = p.BrandID and t.SeasonID = p.SeasonID)

	update p
		set p.GSD_CostingSMV = t.GSD_CostingSMV
	from P_StyleInfo p
	inner join #tmp_final t on t.StyleID = p.StyleID and t.BrandID = p.BrandID and t.SeasonID = p.SeasonID


	drop table #tmp_Style, #tmp_Last_IETMS, #tmp_final
End