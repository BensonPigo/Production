CREATE PROCEDURE [dbo].[P_ImportLoadingvsCapacity]
AS
begin
	SET NOCOUNT ON
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'
	declare @SqlCmd nvarchar(max) ='';;
	set @SqlCmd = '
	/************* 撈取 Production 資料 *************/
	Create table #tmp(
		[MDivisionID] [varchar](8) NOT NULL,
		[KpiCode] [varchar](8) NOT NULL,
		[Key] [varchar](6) NOT NULL,
		[Halfkey] [varchar](8) NOT NULL,
		[ArtworkTypeID] [varchar](20) NOT NULL,
		[CapacityCPU] [numeric](38, 6) NOT NULL,
		[LoadingCPU] [numeric](38, 6) NOT NULL,
		[TransferBIDate] [datetime] NULL,
	)
	insert into #tmp
	exec ['+ @current_PMS_ServerName +'].production.[dbo].GetLoadingvsCapacity

	/************* 刪除P_LoadingvsCapacity全資料*************/
	delete P_LoadingvsCapacity

	/************* update 至 P_LoadingvsCapacity *************/
	--目前全山全轉邏輯，不用做update
	--update p
	--set 
	--p.[MDivisionID]		= t.[MDivisionID],
	--p.[FactoryID]		= t.[KpiCode],
	--p.[Key]				= t.[Key],
	--p.[Halfkey]			= t.[Halfkey],
	--p.[ArtworkTypeID]	= t.[ArtworkTypeID],
	--p.[Capacity(CPU)]	= t.[CapacityCPU],
	--p.[Loading(CPU)]	= t.[LoadingCPU],
	--p.[TransferBIDate]	= t.[TransferBIDate]
	--from P_LoadingvsCapacity p with(nolock)
	--inner join #tmp t with(nolock) on p.[MDivisionID]	= t.[MDivisionID]	and 
	--								p.[FactoryID]		= t.[KpiCode]		and
	--								p.[Key]				= t.[Key]			and
	--								p.[Halfkey]			= t.[Halfkey]		and
	--								p.[ArtworkTypeID]	= t.[ArtworkTypeID] 
	

	/************* insert 至 P_LoadingvsCapacity *************/
	insert into P_LoadingvsCapacity
	select 
	t.[MDivisionID],
	t.[KpiCode],
	t.[Key],
	t.[Halfkey],
	t.[ArtworkTypeID],
	t.[CapacityCPU],
	t.[LoadingCPU],
	t.[TransferBIDate]
	from #tmp t
	where not exists
	(
		select 1 from P_LoadingvsCapacity p 
		where 
		p.[MDivisionID]		= t.[MDivisionID]	and 
		p.[FactoryID]		= t.[KpiCode]		and
		p.[Key]				= t.[Key]			and
		p.[Halfkey]			= t.[Halfkey]		and
		p.[ArtworkTypeID]	= t.[ArtworkTypeID] 
	)
	order by [MDivisionID],[KpiCode],[Halfkey] asc,[ArtworkTypeID]

	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_LoadingvsCapacity'')
	BEGIN
		update b
			set b.TransferDate = getdate(), IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_LoadingvsCapacity''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate, IS_Trans)
		values(''P_LoadingvsCapacity'', getdate(), 1)
	END
	';

	EXEC sp_executesql @SqlCmd
end
