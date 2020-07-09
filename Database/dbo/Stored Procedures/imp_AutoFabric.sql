
-- =============================================
-- Description:	Auto Warehouse資料轉入
-- =============================================
CREATE PROCEDURE [dbo].[imp_AutoFabric]
	
AS
DECLARE @v sql_variant 

IF OBJECT_ID(N'CompleteReceiving_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[CompleteReceiving_Detail](
	[ID] [VARCHAR](13) NOT NULL,
	[ActualQty] [NUMERIC](11,2) NULL DEFAULT ((0)),
	[ActualWeight] [NUMERIC](7,2) NULL DEFAULT ((0)),
	[Location] [VARCHAR](60) NOT NULL,
	[Barcode] [VARCHAR](13) NOT NULL,
	[Ukey] [BIGINT] NOT NULL DEFAULT ((0)),
	[CompleteTime] [DATETIME] NOT NULL,
	[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_CompleteReceiving_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Ukey]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'收料單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'ID'

	SET @v = N'實際數量'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'ActualQty'

	SET @v = N'實際重量'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'ActualWeight'

	SET @v = N'儲位'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'Location'

	SET @v = N'布捲條碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'Barcode'

	SET @v = N'完成時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'CompleteTime'

	SET @v = N'SCI是否已轉入'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteReceiving_Detail'
	, N'COLUMN', N'SCIUpdate'
END

IF OBJECT_ID(N'CompleteIssue_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[CompleteIssue_Detail](
	[ID] [VARCHAR](13) NOT NULL,
	[Ukey] [BIGINT] NOT NULL DEFAULT ((0)),
	[Barcode] [VARCHAR](13) NOT NULL,
	[CompleteTime] [DATETIME] NOT NULL,
	[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_CompleteIssue_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Ukey]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


	SET @v = N'發料單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteIssue_Detail'
	, N'COLUMN', N'ID'

	SET @v = N'布捲條碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteIssue_Detail'
	, N'COLUMN', N'Barcode'

	SET @v = N'完成時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteIssue_Detail'
	, N'COLUMN', N'CompleteTime'

	SET @v = N'SCI是否已轉入'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteIssue_Detail'
	, N'COLUMN', N'SCIUpdate'
END

IF OBJECT_ID(N'CompleteSubTransfer_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[CompleteSubTransfer_Detail](
	[ID] [VARCHAR](13) NOT NULL,	
	[Barcode] [VARCHAR](13) NOT NULL,
	[Ukey] [BIGINT] NOT NULL DEFAULT ((0)),
	[CompleteTime] [DATETIME] NOT NULL,
	[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_CompleteSubTransfer_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Ukey]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


	SET @v = N'轉倉單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteSubTransfer_Detail'
	, N'COLUMN', N'ID'

	SET @v = N'布捲條碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteSubTransfer_Detail'
	, N'COLUMN', N'Barcode'

	SET @v = N'完成時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteSubTransfer_Detail'
	, N'COLUMN', N'CompleteTime'

	SET @v = N'SCI是否已轉入'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CompleteSubTransfer_Detail'
	, N'COLUMN', N'SCIUpdate'
END

/* 處理資料邏輯層 */
-- 01 CompleteReceiving_Detail
BEGIN TRY
BEGIN TRANSACTION 

-- W/H P07,P18
select * 
into #tmp_P07_P18
from CompleteReceiving_Detail where SCIUpdate=0
and SUBSTRING(id,4,2) in ('PR' ,'TI')

update t
set t.ActualQty = s.ActualQty
,t.ActualWeight = s.ActualWeight
,t.CompleteTime = s.CompleteTime
from Production.dbo.Receiving_Detail t
inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey

update t
set t.Qty = s.ActualQty
,t.Weight = s.ActualWeight
,t.CompleteTime = s.CompleteTime
from Production.dbo.TransferIn_Detail t
inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey

select distinct Barcode,Location,Ukey
into #tmpFtyinventory
from (
	select distinct r.Barcode,r.Location,fi.Ukey
	from Production.dbo.FtyInventory fi
	inner join (
		select s.Barcode,s.Location,t.PoId,t.Seq1,t.Seq2,t.Roll,t.Dyelot,t.StockType
		from Production.dbo.Receiving_Detail t
		inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey
	) r
	on r.PoId=fi.POID and r.Seq1 = fi.Seq1 and r.Seq2 = fi.Seq2
	and r.Roll = fi.Roll and r.Dyelot = fi.Dyelot and fi.StockType = r.StockType

	union 

	select distinct r.Barcode,r.Location,fi.Ukey
	from Production.dbo.FtyInventory fi
	inner join (
		select s.Barcode,s.Location,t.PoId,t.Seq1,t.Seq2,t.Roll,t.Dyelot,t.StockType
		from Production.dbo.TransferIn_Detail t
		inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey
	) r
	on r.PoId=fi.POID and r.Seq1 = fi.Seq1 and r.Seq2 = fi.Seq2
	and r.Roll = fi.Roll and r.Dyelot = fi.Dyelot and fi.StockType = r.StockType
) s

update t
set t.Barcode = s.Barcode
from Production.dbo.FtyInventory t
inner join #tmpFtyinventory s on t.Ukey=s.Ukey

update t
set t.MtlLocationID = s.Location
from Production.dbo.FtyInventory_Detail t
inner join #tmpFtyinventory s on t.Ukey=s.Ukey

update t
set t.SCIUpdate=1
from CompleteReceiving_Detail t
where exists(select * from #tmp_P07_P18 where id= t.id and Ukey = t.Ukey)

/* 同WH.P07,P18 更新Ftyinventory & MDivisionPoDetail */

select PoId,Seq1,Seq2,StockType,Location,sum(Qty) as Qty
into #TmpSource_MDPO
from (
	select PoId,Seq1,Seq2,StockType,t.Location,sum(t.StockQty) as Qty	
	from Production.dbo.Receiving_Detail t
	inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey
	group by PoId,Seq1,Seq2,StockType,t.Location

	union

	select PoId,Seq1,Seq2,StockType,t.Location,sum(t.Qty) as Qty	
	from Production.dbo.TransferIn_Detail t
	inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey
	group by PoId,Seq1,Seq2,StockType,t.Location
) s
group by PoId,Seq1,Seq2,StockType,Location


-- 更新 In Qty
ALTER TABLE #TmpSource_MDPO ALTER COLUMN POID VARCHAR(20)
ALTER TABLE #TmpSource_MDPO ALTER COLUMN SEQ1 VARCHAR(3)
ALTER TABLE #TmpSource_MDPO ALTER COLUMN SEQ2 VARCHAR( 3)
ALTER TABLE #TmpSource_MDPO ALTER COLUMN STOCKTYPE VARCHAR(1)

merge Production.dbo.mdivisionpodetail as target
using  #TmpSource_MDPO as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'I' then
	update set
	 target.inqty = isnull(target.inqty,0.00) + src.qty 
	,target.blocation = src.location
when not matched by target and src.stocktype = 'I' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

merge Production.dbo.mdivisionpodetail as target
using  #TmpSource_MDPO as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'B' then
	update set 
	 target.inqty = isnull(target.inqty,0.00) + src.qty 
	,target.alocation = src.location
when not matched by target and src.stocktype = 'B' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[alocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

merge Production.dbo.mdivisionpodetail as target
using  #TmpSource_MDPO as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'O' then
	update set 
	 target.inqty = isnull(target.inqty,0.00) + src.qty 
	,target.Clocation = src.location
when not matched by target and src.stocktype = 'O' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[Clocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

--  Update LInvQty

ALTER TABLE #TMPSOURCE_MDPO ALTER COLUMN POID VARCHAR(20)
ALTER TABLE #TMPSOURCE_MDPO ALTER COLUMN SEQ1 VARCHAR(3)
ALTER TABLE #TMPSOURCE_MDPO ALTER COLUMN SEQ2 VARCHAR(3)

merge Production.dbo.mdivisionpodetail as target
using (select * from #TmpSource_MDPO where stocktype='I') as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched then
	update set 
	 target.LInvQty = isnull(target.LInvQty,0.00) + src.qty 
	,target.blocation = src.location
when not matched then
    insert ([Poid],[Seq1],[Seq2],[LInvQty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

drop table #TmpSource_MDPO

-- Update Ftyinventory

select PoId,Seq1,Seq2,StockType,Location,Roll,Dyelot,sum(Qty) as Qty
into #TmpSource_FTY
from (
	select PoId,Seq1,Seq2,StockType,t.Location,t.Roll,t.Dyelot,sum(t.StockQty) as Qty
	from Production.dbo.Receiving_Detail t
	inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey
	group by PoId,Seq1,Seq2,StockType,t.Location,t.Roll,t.Dyelot

	union

	select PoId,Seq1,Seq2,StockType,t.Location,t.Roll,t.Dyelot,sum(t.Qty) as Qty
	from Production.dbo.TransferIn_Detail t
	inner join #tmp_P07_P18 s on t.Id=s.ID and t.Ukey=s.Ukey
	group by PoId,Seq1,Seq2,StockType,t.Location,t.Roll,t.Dyelot
) s
group by PoId,Seq1,Seq2,StockType,Location,Roll,Dyelot




declare @MtlAutoLock bit = (select MtlAutoLock from Production.dbo.system);

alter table #TmpSource_FTY alter column poid varchar(20)
alter table #TmpSource_FTY alter column seq1 varchar(3)
alter table #TmpSource_FTY alter column seq2 varchar(3)
alter table #TmpSource_FTY alter column stocktype varchar(1)
alter table #TmpSource_FTY alter column roll varchar(15)

select distinct poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) , qty, dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource_FTY


select s.*,psdseq1=psd.seq1
into #tmpS11
from #tmpS1 s
left join Production.dbo.PO_Supp_Detail psd on psd.id = s.poid and psd.seq1 = s.seq1 and psd.seq2 = s.seq2

merge Production.dbo.FtyInventory as target
using #tmpS11 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set inqty = isnull(inqty,0.00) + s.qty,
         Lock = iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,@MtlAutoLock,0),
         LockName = iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and @MtlAutoLock=1 ,'FPSAutoImp',''),
         LockDate = iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and @MtlAutoLock=1 ,getdate(),null)
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty], [Lock],[LockName],[LockDate])
    values ((select ukey from Production.dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty,
              iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,@MtlAutoLock,0),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and @MtlAutoLock=1 ,'FPSAutoImp',''),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and @MtlAutoLock=1 ,getdate(),null)
            );

select location,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource_FTY s
left join Production.dbo.ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
merge Production.dbo.ftyinventory_detail as t 
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.location,'')
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

drop table #tmp_L_K ,#tmp_P07_P18,#tmpFtyinventory
drop table #tmpS1, #tmpS11; 
drop table #TmpSource_FTY;

commit transaction
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'UPDATE W/H P07,P18 sql command fail'
END CATCH


BEGIN TRY
BEGIN TRANSACTION 
-- W/H P17
select * 
into #tmp_P17
from CompleteReceiving_Detail where SCIUpdate=0
and SUBSTRING(id,4,2) in ('RR')

update t
set t.Qty = s.ActualQty
,t.CompleteTime = s.CompleteTime
from Production.dbo.IssueReturn_Detail t
inner join #tmp_P17 s on t.Id=s.ID and t.Ukey=s.Ukey

select distinct r.Barcode,r.Location,fi.Ukey
into #tmpFtyinventory_P17
from Production.dbo.FtyInventory fi
inner join (
	select s.Barcode,s.Location,t.PoId,t.Seq1,t.Seq2,t.Roll,t.Dyelot,t.StockType
	from Production.dbo.IssueReturn_Detail t
	inner join #tmp_P17 s on t.Id=s.ID and t.Ukey=s.Ukey
) r
on r.PoId=fi.POID and r.Seq1 = fi.Seq1 and r.Seq2 = fi.Seq2
and r.Roll = fi.Roll and r.Dyelot = fi.Dyelot and fi.StockType = r.StockType


update t
set t.Barcode = s.Barcode
from Production.dbo.FtyInventory t
inner join #tmpFtyinventory_P17 s on t.Ukey=s.Ukey

update t
set t.MtlLocationID = s.Location
from Production.dbo.FtyInventory_Detail t
inner join #tmpFtyinventory_P17 s on t.Ukey=s.Ukey

update t
set t.SCIUpdate=1
from CompleteReceiving_Detail t
where exists(select * from #tmp_P17 where id= t.id and Ukey = t.Ukey)

/* 同WH.P17 更新Ftyinventory & MDivisionPoDetail */

select PoId,Seq1,Seq2,StockType,- sum(Qty) as Qty
into #TmpSource_MDPO_P17
from Production.dbo.IssueReturn_Detail t
inner join #tmp_P17 s on t.Id=s.ID and t.Ukey=s.Ukey
group by PoId,Seq1,Seq2,StockType

ALTER TABLE #TmpSource_MDPO_P17 ALTER COLUMN POID VARCHAR(20)
ALTER TABLE #TmpSource_MDPO_P17 ALTER COLUMN SEQ1 VARCHAR(3)
ALTER TABLE #TmpSource_MDPO_P17 ALTER COLUMN SEQ2 VARCHAR(3)

update t
set t.OutQty = isnull(t.OutQty,0.00) + s.qty
from Production.dbo.MDivisionPoDetail t WITH (NOLOCK) 
inner join #TmpSource_MDPO_P17 s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

-- FtyInventory 
select t.PoId,t.Seq1,t.Seq2,t.StockType
,[Location] = Production.dbo.Getlocation(fi.Ukey)
,t.Roll,t.Dyelot
,[Qty] = - sum(Qty) 
into #TmpSource_FTY_P17
from Production.dbo.IssueReturn_Detail t
inner join #tmp_P17 s on t.Id=s.ID and t.Ukey=s.Ukey
left join Production.dbo.FtyInventory FI on t.Poid = FI.Poid and t.Seq1 = FI.Seq1 and t.Seq2 = FI.Seq2 and t.Dyelot = FI.Dyelot
    and t.Roll = FI.Roll and t.StockType = FI.StockType
group by t.PoId,t.Seq1,t.Seq2,t.StockType,Production.dbo.Getlocation(fi.Ukey),t.Roll,t.Dyelot

ALTER TABLE #TmpSource_FTY_P17 ALTER COLUMN POID VARCHAR(20)
ALTER TABLE #TmpSource_FTY_P17 ALTER COLUMN SEQ1 VARCHAR(3)
ALTER TABLE #TmpSource_FTY_P17 ALTER COLUMN SEQ2 VARCHAR(3)
ALTER TABLE #TmpSource_FTY_P17 ALTER COLUMN STOCKTYPE VARCHAR(1)
ALTER TABLE #TmpSource_FTY_P17 ALTER COLUMN ROLL VARCHAR(15)

select distinct poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))), qty, dyelot = isnull(dyelot, '')
into #tmpS1_P17
from #TmpSource_FTY_P17

merge Production.dbo.FtyInventory as target
using #tmpS1_P17 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey from Production.dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);

drop table #TmpSource_FTY_P17,#tmp_P17,#TmpSource_MDPO_P17,#tmpS1_P17

commit transaction
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'UPDATE W/H P17 sql command fail'
END CATCH

-- 02 CompleteIssue_Detail

select * 
into #tmpCompleteIssue_Detail
from CompleteIssue_Detail where SCIUpdate=0

--W/H_P10 Issue_detail
UPDATE t
set t.CompleteTime = s.CompleteTime
from Production.dbo.Issue_Detail t
inner join #tmpCompleteIssue_Detail s on t.id=s.ID and t.ukey=s.Ukey
where SUBSTRING(s.ID,4,2) in ('IC','II')

--W/H_P16 IssueLack_Detail
UPDATE t
set t.CompleteTime = s.CompleteTime
from Production.dbo.IssueLack_Detail t
inner join #tmpCompleteIssue_Detail s on t.id=s.ID and t.ukey=s.Ukey
where SUBSTRING(s.ID,4,2) in ('IF')

--W/H_P19 IssueLack_Detail
UPDATE t
set t.CompleteTime = s.CompleteTime
from Production.dbo.TransferOut_Detail t
inner join #tmpCompleteIssue_Detail s on t.id=s.ID and t.ukey=s.Ukey
where SUBSTRING(s.ID,4,2) in ('TO')

update t
set t.SCIUpdate=1
from CompleteIssue_Detail t
where exists(select * from #tmpCompleteIssue_Detail where id= t.id and Ukey = t.Ukey)


-- 03 CompleteSubTransfer_Detail

select * 
into #tmpCompleteSubTransfer_Detail
from CompleteSubTransfer_Detail
where SCIUpdate=0

UPDATE t
set t.CompleteTime = s.CompleteTime
from Production.dbo.SubTransfer_Detail t
inner join #tmpCompleteSubTransfer_Detail s on t.id=s.ID and t.ukey=s.Ukey

update t
set t.SCIUpdate=1
from CompleteSubTransfer_Detail t
where exists(select * from #tmpCompleteSubTransfer_Detail where id= t.id and Ukey = t.Ukey)