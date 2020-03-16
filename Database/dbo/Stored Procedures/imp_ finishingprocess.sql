IF NOT EXISTS(SELECT * FROM SYS.DATABASES WHERE NAME='FPS')
BEGIN 
 CREATE DATABASE FPS
END
GO

USE FPS
GO

-- =============================================
-- Description:	轉入/更新資料
-- =============================================
CREATE PROCEDURE imp_finishingprocess

AS

-- Create Table
Begin
	IF OBJECT_ID(N'TransferLocation') IS NULL
	BEGIN
		CREATE TABLE [dbo].[TransferLocation](
		[ID] [bigint] NOT NULL,
		[SCICtnNo] [varchar](15) NOT NULL,
		[CustCTN] [varchar](30) NOT NULL,
		[GW] [numeric](7, 3) NULL,
		[ClogLocationId] [varchar](10) NULL,
		[Pallet] [varchar](10) NULL,
		[Time] [datetime] NOT NULL,
		[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_TransferLocation] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	END

	IF OBJECT_ID(N'MiniToPallet') IS NULL
	BEGIN
		CREATE TABLE [dbo].[MiniToPallet](
		[ID] [bigint] NOT NULL,
		[SCICtnNo] [varchar](15) NOT NULL,
		[CustCTN] [varchar](30) NOT NULL,
		[ClogLocationId] [varchar](10) NOT NULL,
		[Pallet] [varchar](10) NULL,
		[Time] [datetime] NOT NULL,
		[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_MiniToPallet] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END

	IF OBJECT_ID(N'CompleteClogReturn') IS NULL
	BEGIN
		CREATE TABLE [dbo].[CompleteClogReturn](
		[ID] [bigint] NOT NULL,
		[Time] [datetime] NOT NULL,
		[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_CompleteClogReturn] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END	

	IF OBJECT_ID(N'CompleteTransferToCFA') IS NULL
	BEGIN
		CREATE TABLE [dbo].[CompleteTransferToCFA](
		[ID] [bigint] NOT NULL,
		[Time] [datetime] NOT NULL,
		[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_CompleteTransferToCFA] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END

	IF OBJECT_ID(N'CompletePullout') IS NULL
	BEGIN
		CREATE TABLE [dbo].[CompletePullout](
		[ID] [bigint] NOT NULL,
		[SCICtnNo] [varchar](15) NOT NULL,
		[CustCTN] [varchar](30) NOT NULL,
		[Pulloutscanname] [varchar](10) NOT NULL,
		[Pulloutscandate] [datetime] NOT NULL,
		[TruckNo] [varchar](15) NULL,
		[Time] [datetime] NOT NULL,
		[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_CompletePullout] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END

	IF OBJECT_ID(N'CompleteSacnPack') IS NULL
	BEGIN
		CREATE TABLE [dbo].[CompleteSacnPack](
		[ID] [bigint] NOT NULL,
		[SCICtnNo] [varchar](15) NOT NULL,
		[ScanQty] [smallint] NOT NULL,
		[ScanName] [varchar](10) NOT NULL,
		[ScanEditDate] [datetime] NOT NULL,
		[Lacking] [bit] NOT NULL,
		[Time] [datetime] NOT NULL,
		[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_CompleteSacnPack] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END
End

-- Create Tmp Table
Begin
	--建立tmpe table存放要全部的資料
	DECLARE @tmpPackingList TABLE (
	   ID VARCHAR(13),   
	   ukey bigint IDENTITY(1,1)
	)

	DECLARE @tmpOrder TABLE (
	   ID VARCHAR(13),
	   ukey bigint IDENTITY(1,1)
	)
End

-- Do Something
Begin
	--01. PackingList_Detail / TransferLocation
	--只撈取 Clog P02、P08 資料
	Begin
		select s.*
		into #tmpTransferLocation
		from TransferLocation s
		outer apply(
			select SCICtnNo, Time = max(Time)
			from TransferLocation t
			where SCICtnNo = s.SCICtnNo
			and SCIUpdate = s.SCIUpdate
			group by SCICtnNo 
		)t
		where s.SCIUpdate = 0
		and s.Time = t.Time
		and (
			exists (select 1 from  Production.dbo.PackingList_Detail where SCICtnNo= s.SCICtnNo and ReceiveDate is null)
			or
			exists (select 1 from  Production.dbo.PackingList_Detail where SCICtnNo= s.SCICtnNo and CFAReturnClogDate is not null)
		)

		-- 加入@tmpPacking
		insert into @tmpPackingList
		select distinct t.ID
		from Production.dbo.PackingList_Detail t
		inner join #tmpTransferLocation s on t.SCICtnNo=s.SCICtnNo

		-- 加入@tmpOrder
		insert into @tmpOrder
		select distinct orderid
		from #tmpTransferLocation t
		inner join Production.dbo.PackingList_Detail pd on t.SCICtnNo=pd.SCICtnNo
	End

	--02、03 Pcaking P10, Clog P02 P08
	Begin
		--Packing P10 log
			-- Insert TransferToClog 
		Begin
			insert into Production.dbo.TransferToClog(TransferDate, MDivisionID, PackingListID, OrderID, CTNStartNo, AddDate, OldID, TransferSlipNo, AddName, SCICtnNo)
			select [TransferDate] = CONVERT(date, t.Time)
				,MDvisionID = (select top 1 ID from Production.dbo.MDivision)
				,[PackingListID] = pd.ID
				,[OrderID] = pd.OrderID
				,[CtnStartNo] = pd.CTNStartNo
				,t.Time,null,null,'SCIMIS'
				,pd.SCICtnNo
			from #tmpTransferLocation t
			inner join Production.dbo.PackingList_Detail pd on t.SCICtnNo=pd.SCICtnNo
			where pd.ReceiveDate is null
		END

		-- Clog P02 log 
			-- Insert ClogReceive
		Begin
			insert into Production.dbo.ClogReceive(ReceiveDate, MDivisionID, PackingListID, OrderID, CTNStartNo, ClogLocationID, AddDate, OldID, AddName, SCICtnNo)
			select [ReceiveDate] = CONVERT(date, t.Time)
				,[MDvisionID] = (select top 1 ID from Production.dbo.MDivision)
				,[PackingListID] = pd.ID
				,[OrderID] = pd.OrderID
				,[CtnStartNo] = pd.CTNStartNo
				,t.ClogLocationId
				,t.Time
				,null
				,'SCIMIS'
				,pd.SCICtnNo
			from #tmpTransferLocation t
			inner join Production.dbo.PackingList_Detail pd on t.SCICtnNo=pd.SCICtnNo
			where pd.ReceiveDate is null
		End

		-- Clog P02 
			-- Update PackingList_Detail
		Begin
			update pd
			set pd.ReceiveDate = CONVERT(date, s.Time)
				,pd.TransferDate = CONVERT(date,s.Time)
				,pd.NewGW = s.GW
				,pd.ReturnDate = null
			from Production.dbo.PackingList_Detail pd
			inner join #tmpTransferLocation s on pd.SCICtnNo=s.SCICtnNo
			where pd.ReceiveDate is null
		End

		-- Clog P08 log
			-- Update ClogReceiveCFA
		Begin
			insert into Production.dbo.ClogReceiveCFA(ReceiveDate, MDivisionID, OrderID, PackingListID, CTNStartNo, AddName, AddDate, SCICtnNo)
			select [ReceiveDate] = CONVERT(date, t.Time)
				,[MDvisionID] = (select top 1 ID from Production.dbo.MDivision)
				,[OrderID] = pd.OrderID
				,[PackingListID] = pd.ID
				,[CtnStartNo] = pd.CTNStartNo
				,[AddName] = 'SCIMIS'
				,[AddDate] = t.Time
				,pd.SCICtnNo
			from #tmpTransferLocation t
			inner join Production.dbo.PackingList_Detail pd on t.SCICtnNo=pd.SCICtnNo
			where pd.CFAReturnClogDate is not null
		End

		-- Clog P08 
			-- Update PackingList_Detail
		Begin
			update pd
			set pd.ClogReceiveCFADate = CONVERT(date, s.Time)
				,pd.TransferDate = CONVERT(date,s.Time)
				,pd.NewGW = s.GW
				,pd.CFAReturnClogDate = null
				,pd.CFALocationId = null
			from Production.dbo.PackingList_Detail pd
			inner join #tmpTransferLocation s on pd.SCICtnNo=s.SCICtnNo
			where pd.CFAReturnClogDate is not null
		End
	End

	--04. PackingList_Detail.ClogLocationId
	Begin
		select * 
		 ,[row] = ROW_NUMBER() over(partition by SCICtnNo order by time desc)
		 into #tmp_LastLocation
		 from 
		(
			select ClogLocationId,Pallet,Time,SCICtnNo
			from TransferLocation
			where SCIUpdate=0 
				union all
			select ClogLocationId,Pallet,Time,SCICtnNo
			from MiniToPallet
			where SCIUpdate=0 
		) a 

		update pd
		set pd.ClogLocationId = s.ClogLocationId
			,pd.Pallet = s.Pallet
			,pd.EditLocationDate = s.Time
			,pd.EditLocationName = 'GenSong'
		from Production.dbo.PackingList_Detail as pd
		inner join #tmp_LastLocation s on pd.SCICtnNo = s.SCICtnNo
		where row=1

		-- 加入@tmpPacking
		insert into @tmpPackingList
		select distinct pd.ID
		from Production.dbo.PackingList_Detail as pd
		inner join #tmp_LastLocation s on pd.SCICtnNo = s.SCICtnNo
		where row=1

		-- TransferLocation
		update t
		set t.SCIUpdate=1
		from TransferLocation t
		where exists(
			select * 
			from #tmp_LastLocation tmp
			inner join Production.dbo.PackingList_Detail pd 
				on tmp.SCICtnNo = pd.SCICtnNo
			where tmp.SCICtnNo= t.SCICtnNo and row=1
		)

		-- MiniToPallet
		update t
		set t.SCIUpdate=1
		from MiniToPallet t
		where exists(
			select * 
			from #tmp_LastLocation tmp
			inner join Production.dbo.PackingList_Detail pd 
				on tmp.SCICtnNo = pd.SCICtnNo
			where tmp.SCICtnNo= t.SCICtnNo and row=1
		)
	End

	--05 ClogReturn / CompleteClogReturn
	Begin
		select * 
		into #tmpCompleteClogReturn
		from CompleteClogReturn where SCIUpdate=0

		update t
		set t.CompleteTime = s.Time
		from Production.dbo.ClogReturn t
		inner join #tmpCompleteClogReturn s on t.ID=s.ID

		-- 加入@tmpOrder
		insert into @tmpOrder
		select distinct t.orderid
		from Production.dbo.ClogReturn t
		inner join #tmpCompleteClogReturn s on t.ID=s.ID

		-- CompleteClogReturn
		update t
		set t.SCIUpdate=1
		from CompleteClogReturn t
		where exists(
			select * 
			from #tmpCompleteClogReturn tmp
			inner join Production.dbo.ClogReturn cr 
				on tmp.ID = cr.ID
			where t.ID= tmp.ID
		)
	End

	--06 TransferToCFA / CompleteTransferToCFA
	Begin
		select * 
		into #tmpCompleteTransferToCFA
		from CompleteTransferToCFA where SCIUpdate=0

		update t
		set t.CompleteTime = s.Time
		from Production.dbo.TransferToCFA t
		inner join #tmpCompleteTransferToCFA s on t.ID=s.ID

		-- CompleteTransferToCFA
		update t
		set t.SCIUpdate=1
		from CompleteTransferToCFA t
		where exists(
			select * 
			from #tmpCompleteTransferToCFA tmp
			inner join Production.dbo.TransferToCFA cfa on tmp.ID = cfa.ID
			where t.ID=tmp.id
		)
	End

	-- 07 TransferToTruck / CompletePullout
	Begin
		select * 
		into #tmpCompletePullout
		from CompletePullout 
		where SCIUpdate=0

		insert into Production.dbo.TransferToTruck(TransferDate, MDivisionID, OrderID, PackingListID, CTNStartNo, TruckNo, AddName, AddDate, SCICtnNo)
		select distinct [TransferDate] = CONVERT(date, s.Time)
			,MDvisionID = (select top 1 ID from Production.dbo.MDivision)
			,[OrderID] = pd.OrderID
			,[PackingListID] = pd.ID
			,[CtnStartNo] = pd.CTNStartNo
			,[TruckNo] = s.TruckNo
			,'SCIMIS'
			,s.Time
			,pd.SCICtnNo
		from #tmpCompletePullout s
		inner join Production.dbo.PackingList_Detail pd on s.SCICtnNo=pd.SCICtnNo

		-- Clog P12
		update pd
			set pd.ClogPulloutDate = t.Time
				,pd.ClogPulloutName = 'SCIMIS'
				,pd.PulloutTransport = 'T'
				,pd.PulloutTransportNo = t.TruckNo
		from #tmpCompletePullout t
		inner join Production.dbo.PackingList_Detail pd on t.SCICtnNo = pd.SCICtnNo 

		update t
			set t.SCIUpdate=1
		from CompletePullout t
		inner join #tmpCompletePullout s on t.ID=s.ID
		inner join Production.dbo.PackingList_Detail pd on s.SCICtnNo=pd.SCICtnNo
		inner join Production.dbo.TransferToTruck tt on tt.OrderID = pd.OrderID 
		and tt.PackingListID = pd.ID and tt.CTNStartNo = pd.CTNStartNo
	End

	-- 08 PackingList_Detail/CompleteSacnPack
	Begin
		select * 
		into #tmpCompleteSacnPack
		from CompleteSacnPack where SCIUpdate=0

		update t
		set t.ScanQty = s.ScanQty
		,t.ScanName = s.ScanName
		,t.ScanEditDate = s.ScanEditDate
		,t.Lacking=s.Lacking
		from Production.dbo.PackingList_Detail t
		inner join #tmpCompleteSacnPack s on t.SCICtnNo=s.SCICtnNo

		-- 新增@tmpPacking
		insert into @tmpPackingList
		select distinct t.ID
		from Production.dbo.PackingList_Detail t
		inner join #tmpCompleteSacnPack s on t.SCICtnNo=s.SCICtnNo

		update t
		set t.SCIUpdate=1
		from CompleteSacnPack t
		where exists(
			select * 
			from #tmpCompleteSacnPack tmp
			inner join Production.dbo.PackingList_Detail pd 
				on tmp.SCICtnNo = pd.SCICtnNo
			where t.ID = tmp.id)
	End

	drop table #tmp_LastLocation,#tmpCompleteClogReturn,#tmpCompletePullout,#tmpCompleteSacnPack,#tmpCompleteTransferToCFA,#tmpTransferLocation
End

-- 跑迴圈執行procedure(UpdateOrdersCTN, CreateOrderCTNData)
Begin
	declare @TotalCount int , @Count int , @id varchar(13)
	set @TotalCount = (select count(*) from @tmpOrder)
	set @Count=1;

	if @TotalCount >0
	begin 
		while (@Count) <= @TotalCount
		begin
			set @id = (select id from @tmpOrder where ukey=@Count)
			exec Production.dbo.UpdateOrdersCTN @id
			set @Count += 1;	
		end
	end

	set @TotalCount = (select count(*) from @tmpPackingList)
	set @Count=1;

	if @TotalCount >0
	begin 
		while (@Count) <= @TotalCount
		begin
			set @id = (select id from @tmpOrder where ukey=@Count)
			exec Production.dbo.CreateOrderCTNData @id ,'AutoUpdate'
			set @Count += 1;
		end

	end
End