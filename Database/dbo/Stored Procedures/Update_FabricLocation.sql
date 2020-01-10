-- =============================================
-- Description:	更新SCI Location 
-- =============================================
CREATE PROCEDURE [dbo].[Update_FabricLocation]

AS
DECLARE @v sql_variant 

IF OBJECT_ID(N'LocationTrans') IS NULL
BEGIN
	CREATE TABLE [dbo].[LocationTrans](
	[ID] [VARCHAR](13) NOT NULL,
	[Barcode] [VARCHAR](13) NOT NULL,
	[ToLocation] [VARCHAR](60) NOT NULL,	
	[CmdTime] [DATETIME] NOT NULL,
	[SCIUpdate] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_LocationTrans] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Barcode]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocationTrans'
	, N'COLUMN', N'ID'

	SET @v = N'新儲位'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocationTrans'
	, N'COLUMN', N'ToLocation'

	SET @v = N'布捲條碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocationTrans'
	, N'COLUMN', N'Barcode'

	SET @v = N'完成時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocationTrans'
	, N'COLUMN', N'CmdTime'

	SET @v = N'SCI是否已轉入'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'LocationTrans'
	, N'COLUMN', N'SCIUpdate'
END


-- 更新Production.dbo.LocationTrans (Insert)

declare @ID_Header varchar(10)= 'VM3LH' + (select format(GETDATE(),'yyMM'))
--select Top 1 id from Production.dbo.LocationTrans
--where id like @ID_Header+'%' 
--order by id desc

declare @FPS_Barcode varchar(13);

declare LocationTrans_cursor cursor
for
select distinct FPS_LT.Barcode
from Production.dbo.FtyInventory fty
inner join LocationTrans FPS_LT on FPS_LT.Barcode = fty.Barcode
where FPS_LT.SCIUpdate=0

open LocationTrans_cursor
fetch next from LocationTrans_cursor into @FPS_Barcode
while(@@FETCH_STATUS = 0)
begin 

BEGIN TRY

BEGIN TRANSACTION

	-- GetID
	declare @num_NewID varchar(6) =  (select Top 1 SUBSTRING(id,10,4)+1 from Production.dbo.LocationTrans where id like @ID_Header+'%' order by id desc)
	declare @GetNewID varchar(13)

	if (LEN(@num_NewID)=3)
	begin
		set @num_NewID = '0'+@num_NewID
	end

	else if (LEN(@num_NewID)=2)
	begin
		set @num_NewID = '00'+@num_NewID
	end
	else if (LEN(@num_NewID)=1)
	begin
		set @num_NewID = '000'+@num_NewID
	end

	set @GetNewID = @ID_Header + @num_NewID

	declare @FPS_CmdTime date,@FPS_ToLocation varchar(60);

	select Top 1 @FPS_CmdTime = CONVERT(date, CmdTime)
	,@FPS_ToLocation = ToLocation
	from LocationTrans where Barcode=@FPS_Barcode 
	order by CmdTime desc

	insert into Production.dbo.LocationTrans(ID,MDivisionID,FactoryID,IssueDate,Status,Remark,AddName,AddDate,EditName,EditDate)
	values(@GetNewID,'VM3','SNP',CONVERT(date, @FPS_CmdTime),'Confirmed','GenSong Updated','SCIMIS',GETDATE(),'SCIMIS',GETDATE())


	insert into Production.dbo.LocationTrans_detail(ID,FtyInventoryUkey,MDivisionID,Poid,Seq1,Seq2,Roll,Dyelot,FromLocation,ToLocation,Qty,StockType)
	select [NewID] = @GetNewID
	,fty.Ukey,null,fty.POID,fty.Seq1,fty.Seq2,fty.Roll,fty.Dyelot
	,[FromLocation] = Fty2.Location
	,@FPS_ToLocation
	,Qty = fty.InQty-fty.OutQty+fty.AdjustQty
	,StockType='B'
	from Production.dbo.FtyInventory fty	
	outer apply(
		select [Location] = STUFF((
		select distinct concat(',',MtlLocationID)
		from Production.dbo.FtyInventory_Detail
		where Ukey=fty.Ukey
		for xml path('')),1,1,'')
	) Fty2
	where fty.Barcode=@FPS_Barcode
	and fty.StockType='B'


	-- 更新SCIUpdate
	update LocationTrans
	set SCIUpdate=1
	where Barcode=@FPS_Barcode

COMMIT TRANSACTION	
END TRY

BEGIN CATCH
ROLLBACK TRANSACTION
END CATCH 
	fetch Next from LocationTrans_cursor into @FPS_Barcode
end

close LocationTrans_cursor
deallocate LocationTrans_cursor

-- 更新Production.dbo.LocationTrans (update)
-- update PMS Ftyinventory_Detail

--update LT1
--set IssueDate = convert(date,FPS_LT.CmdTime)
select distinct
fty.POID,fty.Seq1,fty.Seq2,fty.Roll,fty.Dyelot
,FPS_LT.ToLocation
,[CmdDate] = CONVERT(date, FPS_LT.CmdTime)
,[PMS_LocationID] = LT1.ID
,[FTY_ukey] = fty.Ukey
into #tmpTransLocation
from Production.dbo.FtyInventory fty
inner join Production.dbo.LocationTrans_detail LT2
on LT2.Poid= fty.POID and LT2.Seq1=fty.Seq1
	and LT2.Seq2=fty.Seq2 and lt2.Roll = fty.Roll
	and LT2.Dyelot = fty.Dyelot and LT2.StockType = fty.StockType
inner join Production.dbo.LocationTrans LT1 on LT1.ID=LT2.ID
outer apply(
	select top 1 *
	from LocationTrans
	where Barcode = fty.Barcode
	and SCIUpdate=1
	order by CmdTime desc
)FPS_LT
where FPS_LT.CmdTime is not null
and LT1.Remark='GenSong Updated'

update T
set t.IssueDate = s.CmdDate
from Production.dbo.LocationTrans as T
inner join #tmpTransLocation as s on t.ID=s.PMS_LocationID

update T
set ToLocation = s.ToLocation
from Production.dbo.LocationTrans_detail as T
inner join #tmpTransLocation as s on t.ID = s.PMS_LocationID

-- update FtyInventory_Detail.MtlLocationID

delete t 
from Production.dbo.FtyInventory_Detail t
where  t.ukey = (select distinct ukey from #tmpTransLocation where t.Ukey = FTY_ukey)                                          

merge Production.dbo.ftyinventory_detail as t
using #tmpTransLocation as s 
on t.ukey = s.FTY_ukey 
and isnull(t.mtllocationid,'') = isnull(s.tolocation,'')
when not matched AND s.FTY_ukey IS NOT NULL then
    insert ([ukey],[mtllocationid]) 
       values (s.FTY_ukey,isnull(s.tolocation,''));

-- update MDivisionPoDetail.ALocation
update T
set t.ALocation =  concat(t.alocation, iif(s.ToLocation != '', ','+ s.ToLocation,''))
from Production.dbo.MDivisionPoDetail as T
outer apply(
		select ToLocation = Stuff((
		select concat(',',ToLocation)
		from (
				select 	distinct
					ToLocation
				from dbo.#tmpTransLocation d
				where d.POID = t.POID
				and d.Seq1 = t.Seq1 and d.Seq2 = t.Seq2
			) s
		for xml path ('')
	) , 1, 1, '')
)s
where s.ToLocation is not null


drop table #tmpTransLocation