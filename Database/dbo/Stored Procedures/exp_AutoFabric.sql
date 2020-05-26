

-- =============================================
-- Description:	Auto Warehouse資料轉出
-- =============================================
CREATE PROCEDURE [dbo].[exp_AutoFabric]
	
AS
DECLARE @v sql_variant 
begin
IF OBJECT_ID(N'Receiving_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[Receiving_Detail](
	[ID] [VARCHAR](13) NOT NULL,
	[InvNo] [VARCHAR](25) NOT NULL,
	[PoId] [VARCHAR](13) NOT NULL,
	[Seq1] [VARCHAR](3) NOT NULL,
	[Seq2] [VARCHAR](2) NOT NULL,
	[Roll] [VARCHAR](8) NOT NULL,
	[Refno] [VARCHAR](20) NULL,
	[ColorID] [VARCHAR](6) NULL,
	[Dyelot] [VARCHAR](8) NOT NULL,
	[PoUnit] [VARCHAR](8) NULL DEFAULT (''),
	[ShipQty] [NUMERIC](11,2) NULL DEFAULT ((0)),
	[Weight] [NUMERIC](7,2) NULL DEFAULT ((0)),
	[StockType] [VARCHAR](1) NULL DEFAULT (''),
	[Ukey] [BIGINT] NOT NULL,
	[IsInspection] [BIT] NOT NULL DEFAULT((0)),
	[Junk] [BIT] NOT NULL DEFAULT ((0)),	
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Receiving_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Ukey]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'收料單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'ID'

	SET @v = N'船務出貨單號/提單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'InvNo'

	SET @v = N'訂單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'PoId'

	SET @v = N'序號一'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Seq1'

	SET @v = N'序號二'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Seq2'

	SET @v = N'物料料號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Refno'

	SET @v = N'顏色'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'ColorID'

	SET @v = N'捲號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Roll'

	SET @v = N'缸號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Dyelot'

	SET @v = N'採購單位'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'PoUnit'

	SET @v = N'採購數量'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'ShipQty'

	SET @v = N'重量'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Weight'

	SET @v = N'倉別'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'StockType'

	SET @v = N'是否檢驗'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'IsInspection'

	SET @v = N'被移除的資料'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'Junk'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receiving_Detail'
	, N'COLUMN', N'GenSongUpdated'
END

IF OBJECT_ID(N'Issue_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[Issue_Detail](
	[ID] [VARCHAR](13) NOT NULL,
	[Type] [VARCHAR](3) NOT NULL,
	[CutplanID] [VARCHAR](13) NOT NULL,
	[EstCutdate] [date] NULL,
	[SpreadingNoID] [VARCHAR](3) NOT NULL,
	[PoId] [VARCHAR](13) NOT NULL,
	[Seq1] [VARCHAR](3) NOT NULL,
	[Seq2] [VARCHAR](2) NOT NULL,
	[Roll] [VARCHAR](8) NOT NULL,
	[Dyelot] [VARCHAR](8) NOT NULL,
	[Barcode] [VARCHAR](13) NOT NULL DEFAULT (''),
	[Qty] [NUMERIC](11,2) NULL DEFAULT ((0)),
	[Ukey] [BIGINT] NOT NULL,
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Issue_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Ukey]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'發料單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'ID'

	SET @v = N'發料來源P10 / P13 / P16'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Type'

	SET @v = N'裁剪計畫'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'CutplanID'

	SET @v = N'預計裁剪日'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'EstCutdate'

	SET @v = N'拉布機桌號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'SpreadingNoID'
	
	SET @v = N'訂單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'PoId'

	SET @v = N'序號一'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Seq1'

	SET @v = N'序號二'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Seq2'

	SET @v = N'捲號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Roll'

	SET @v = N'缸號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Dyelot'

	SET @v = N'布捲條碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Barcode'

	SET @v = N'發料數量'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'Qty'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Issue_Detail'
	, N'COLUMN', N'GenSongUpdated'
END

IF OBJECT_ID(N'WHClose') IS NULL
BEGIN
	CREATE TABLE [dbo].[WHClose](
	[POID] [VARCHAR](13) NOT NULL,	
	[WhseClose] [Date] NULL,	
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_WHClose] PRIMARY KEY CLUSTERED 
(
	[POID] ASC	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


	SET @v = N'訂單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WHClose'
	, N'COLUMN', N'POID'

	SET @v = N'關倉日'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WHClose'
	, N'COLUMN', N'WhseClose'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WHClose'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'WHClose'
	, N'COLUMN', N'GenSongUpdated'
END

IF OBJECT_ID(N'SubTransfer_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[SubTransfer_Detail](
	[ID] [VARCHAR](13) NOT NULL,
	[Type] [VARCHAR](1) NOT NULL,
	[FromPoId] [VARCHAR](13) NOT NULL,
	[FromSeq1] [VARCHAR](3) NOT NULL,
	[FromSeq2] [VARCHAR](2) NOT NULL,
	[FromRoll] [VARCHAR](8) NOT NULL,
	[FromDyelot] [VARCHAR](8) NOT NULL,
	[FromStockType] [VARCHAR](1) NULL DEFAULT (''),
	[ToPoId] [VARCHAR](13) NOT NULL,
	[ToSeq1] [VARCHAR](3) NOT NULL,
	[ToSeq2] [VARCHAR](2) NOT NULL,
	[ToRoll] [VARCHAR](8) NOT NULL,
	[ToDyelot] [VARCHAR](8) NOT NULL,
	[ToStockType] [VARCHAR](1) NULL DEFAULT (''),
	[Barcode] [VARCHAR](13) NOT NULL,	
	[Ukey] [BIGINT] NOT NULL,
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0))
 CONSTRAINT [PK_SubTransfer_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Ukey]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'轉倉單號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ID'

	SET @v = N'轉倉類型'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'Type'

	SET @v = N'原訂單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'FromPOID'

	SET @v = N'原序號一'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'FromSeq1'

	SET @v = N'原序號二'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'FromSeq2'

	SET @v = N'原捲號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'FromRoll'

	SET @v = N'原缸號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'FromDyelot'

	SET @v = N'原倉別'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'FromStockType'

	SET @v = N'新訂單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ToPOID'

	SET @v = N'新序號一'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ToSeq1'

	SET @v = N'新序號二'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ToSeq2'

	SET @v = N'新捲號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ToRoll'

	SET @v = N'新缸號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ToDyelot'

	SET @v = N'新倉別'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'ToStockType'

	SET @v = N'布捲條碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'Barcode'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'SubTransfer_Detail'
	, N'COLUMN', N'GenSongUpdated'
END

IF OBJECT_ID(N'MtlLocation') IS NULL
BEGIN
	CREATE TABLE [dbo].[MtlLocation](
	[ID] [VARCHAR](10) NOT NULL,	
	[StockType] [VARCHAR](10) NOT NULL,	
	[Junk] [bit] NOT NULL,	
	[Description] [NVARCHAR](40) NOT NULL,	
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_MtlLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[StockType]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'儲位'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'MtlLocation'
	, N'COLUMN', N'ID'

	SET @v = N'倉別'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'MtlLocation'
	, N'COLUMN', N'StockType'

	SET @v = N'刪除'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'MtlLocation'
	, N'COLUMN', N'Junk'

	SET @v = N'描述'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'MtlLocation'
	, N'COLUMN', N'Description'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'MtlLocation'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'MtlLocation'
	, N'COLUMN', N'GenSongUpdated'
END

IF OBJECT_ID(N'RefnoRelaxtime') IS NULL
BEGIN
	CREATE TABLE [dbo].[RefnoRelaxtime](
	[Refno] [VARCHAR](20) NOT NULL,	
	[FabricRelaxationID] [VARCHAR](30) NULL,	
	[Relaxtime] [NUMERIC](5,2) NULL,	
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_RefnoRelaxtime] PRIMARY KEY CLUSTERED 
(	
	[Refno]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'物料料號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'RefnoRelaxtime'
	, N'COLUMN', N'Refno'

	SET @v = N'鬆布說明'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'RefnoRelaxtime'
	, N'COLUMN', N'FabricRelaxationID'

	SET @v = N'鬆布時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'RefnoRelaxtime'
	, N'COLUMN', N'Relaxtime'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'RefnoRelaxtime'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'RefnoRelaxtime'
	, N'COLUMN', N'GenSongUpdated'
END


IF OBJECT_ID(N'Cutplan_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[Cutplan_Detail]
(
	[CutplanID] [VARCHAR](13) NOT NULL,	
	[CutRef] [VARCHAR](6) NOT NULL,	
	[CutSeq] [VARCHAR](13) NULL,	
	[POID] [VARCHAR](13) NULL,	
	[Seq1] [VARCHAR](3) NULL,	
	[Seq2] [VARCHAR](2) NULL,	
	[Refno] [VARCHAR](20) NULL,	
	[Article] [VARCHAR](50) NULL,	
	[ColorID] [VARCHAR](6) NULL,
	[SizeCode] [VARCHAR](50) NULL,
	[CmdTime] [DATETIME] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Cutplan_Detail] PRIMARY KEY CLUSTERED 
(	
	[CutplanID],
	[CutRef]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	SET @v = N'裁剪計畫'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'CutplanID'

	SET @v = N'裁次'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'CutRef'

	SET @v = N'裁剪順序'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'CutSeq'

	SET @v = N'訂單號碼'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'POID'

	SET @v = N'序號一'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'Seq1'

	SET @v = N'序號二'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'Seq2'

	SET @v = N'物料料號'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'Refno'

	SET @v = N'色組'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'Article'

	SET @v = N'顏色'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'ColorID'

	SET @v = N'尺寸'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'SizeCode'

	SET @v = N'SCI寫入/更新此筆資料時間'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'CmdTime'

	SET @v = N'GenSong是否已轉製'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Cutplan_Detail'
	, N'COLUMN', N'GenSongUpdated'
END

declare @Today date = CONVERT(date,GETDATE());
--declare @Today date = '20191029'-- CONVERT(date, DATEADD(DAY,-5, GETDATE()));-- for test

--01. 轉出區間 [Production].[dbo].[Receiving_Detail].EditDate= 今天 Status=Confirmed and FabricType=F
MERGE Receiving_Detail AS T
USING(
	SELECT rd.id,r.InvNo,rd.Poid,rd.Seq1,rd.Seq2,rd.Roll,rd.Dyelot,rd.PoUnit,rd.ShipQty,rd.Weight,rd.StockType,rd.Ukey
,po3.Refno , po3.ColorID,IsInspection = 0
	FROM Production.dbo.Receiving_Detail rd
	inner join Production.dbo.Receiving r on rd.id = r.id
	inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
		and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
	where convert(date,r.EditDate) =  @Today
	and exists(
		select 1 from Production.dbo.PO_Supp_Detail 
		where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
		and FabricType='F'
	)
	and r.Status='Confirmed'
) as S
on T.ID = S.ID and T.Ukey=S.Ukey
WHEN MATCHED THEN
UPDATE SET
	t.InvNo = s.InvNo,
	t.Poid = s.Poid,
	t.Seq1 = s.Seq1,
	t.Seq2 = s.Seq2,
	t.Refno = s.Refno,
	t.ColorID = s.ColorID,
	t.IsInspection = 0,
	t.Roll = s.Roll,
	t.Dyelot = s.Dyelot,
	t.PoUnit = s.PoUnit,
	t.ShipQty = s.ShipQty,
	t.Weight = s.Weight,
	t.StockType = s.StockType,
	t.junk = 0,
	t.CmdTime = Getdate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,  InvNo,  Poid,  Seq1,  Seq2,  Refno,  ColorID,  IsInspection,   Roll,  Dyelot,  PoUnit,  ShipQty,  Weight,  StockType,  Ukey,CmdTime  ,GenSongUpdated) 
VALUES(s.id,s.InvNo,s.Poid,s.Seq1,s.Seq2,s.Refno,s.ColorID,s.IsInspection, s.Roll,s.Dyelot,s.PoUnit,s.ShipQty,s.Weight,s.StockType,s.Ukey,GetDate(),0)	
WHEN NOT MATCHED BY SOURCE and convert(date,t.cmdTime) = convert(date, @Today) THEN 
UPDATE SET
	t.junk=1
;

--02. 轉出區間 [Production].[dbo].[Issue_Detail].EditDate or AddDate= 今天 Status=Confirmed and FabricType=F
MERGE Issue_Detail AS T
USING(
	-- WH.P10,P13
	select distinct i2.Id ,i.Type
	,[CutPlanID] = isnull(i.CutplanID,'')
	,c.EstCutdate
	,[SpreadingNoID] = isnull(c.SpreadingNoID,'')
	,i2.POID
	,i2.Seq1,i2.Seq2,i2.Roll,i2.Dyelot,fty.Barcode,i2.Qty,i2.ukey
	from Production.dbo.Issue_Detail i2
	inner join Production.dbo.Issue i on i2.Id=i.Id
	left join Production.dbo.Cutplan c on c.ID = i.CutplanID
	outer apply(
		select Barcode
		from Production.dbo.FtyInventory
		where POID = i2.POID and Seq1=i2.Seq1
		and Seq2=i2.Seq2 and Roll=i2.Roll and Dyelot=i2.Dyelot
	)fty
	where i.Type in ('A','D')
	and exists(
			select 1 from Production.dbo.PO_Supp_Detail 
			where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
			and FabricType='F'
		)
	and i.Status='Confirmed'
	and (convert(date,i.EditDate) =  @Today	or convert(date,i.AddDate) =  @Today)

	union all
	-- WH.P16
	select distinct ik2.Id,ik.Type ,'' as CutPlanID
	,[EstCutdate] = null
	,[SpreadingNoID] = ''
	,ik2.POID,ik2.Seq1,ik2.Seq2,ik2.Roll,ik2.Dyelot,fty.Barcode
	,ik2.Qty,ik2.Ukey
	from Production.dbo.IssueLack_Detail ik2
	inner join Production.dbo.IssueLack ik on ik2.Id=ik.Id
	outer apply(
		select Barcode
		from Production.dbo.FtyInventory
		where POID = ik2.POID and Seq1=ik2.Seq1
		and Seq2=ik2.Seq2 and Roll=ik2.Roll and Dyelot=ik2.Dyelot
	)fty
	where ik.Status='Confirmed'
	and exists(
			select 1 from Production.dbo.PO_Supp_Detail 
			where id = ik2.Poid and seq1=ik2.seq1 and seq2=ik2.seq2 
			and FabricType='F'
		)
	and (convert(date,ik.EditDate) =  @Today	or convert(date,ik.AddDate) =  @Today)
) as S
on T.ID = S.ID and T.Ukey=S.Ukey
WHEN MATCHED THEN
UPDATE SET
	t.Type = s.Type,
	t.CutPlanID = s.CutPlanID,
	t.EstCutdate = s.EstCutdate,
	t.SpreadingNoID = s.SpreadingNoID,
	t.Poid = s.Poid,
	t.Seq1 = s.Seq1,
	t.Seq2 = s.Seq2,
	t.Roll = s.Roll,
	t.Dyelot = s.Dyelot,
	t.Qty = s.Qty,	
	t.Barcode = s.Barcode,
	t.CmdTime = GetDate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  Id,  Type,  CutPlanID,  EstCutdate,  SpreadingNoID,  POID,  Seq1,  Seq2,  Roll,  Dyelot
        ,  Barcode,  Qty, Ukey,    CmdTime,GenSongUpdated) 
VALUES(s.Id,s.Type,s.CutPlanID,s.EstCutdate,s.SpreadingNoID,s.POID,s.Seq1,s.Seq2,s.Roll,s.Dyelot
		,s.Barcode,s.Qty,s.Ukey,GetDate(),0)	
;

--03. 轉出區間 [Production].[dbo].[Orders].WHClose= 今天 
MERGE WHClose AS T
USING(
	SELECT o.id as Poid,o.WhseClose
	FROM Production.dbo.Orders o	
	where convert(date,o.WhseClose) =  @Today	
) as S
on T.PoID = S.PoID
WHEN MATCHED THEN
UPDATE SET
	t.WhseClose = s.WhseClose,
	t.CmdTime = GetDate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  Poid,  WhseClose, CmdTime  ,GenSongUpdated) 
VALUES(s.Poid,s.WhseClose, GetDate(),0)	
;

--04. 轉出區間 [Production].[dbo].[SubTransfer_Detail].EditDate= or Adddate 今天 FabricType=F
MERGE SubTransfer_Detail AS T
USING(
	select distinct sd.ID,s.Type
	,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromRoll,sd.FromDyelot,sd.FromStockType
	,sd.ToPOID,sd.ToSeq1,sd.ToSeq2,sd.ToRoll,sd.ToDyelot,sd.ToStockType
	,fty.Barcode,sd.ukey
	from Production.dbo.SubTransfer_Detail sd
	inner join Production.dbo.SubTransfer s on sd.ID=s.Id
	outer apply(
			select Barcode
			from Production.dbo.FtyInventory
			where POID = sd.ToPOID and Seq1=sd.ToSeq1
			and Seq2=sd.ToSeq2 and Roll=sd.ToRoll and Dyelot=sd.ToDyelot
		)fty
	where 1=1
	and exists(
			select 1 from Production.dbo.PO_Supp_Detail 
			where id = sd.ToPOID and seq1=sd.ToSeq1 and seq2=sd.ToSeq2 
			and FabricType='F'
		)
	and (convert(date,s.EditDate) =  @Today	or convert(date,s.AddDate) =  @Today)
) as S
on T.ID = S.ID and T.ukey=S.ukey
WHEN MATCHED THEN
UPDATE SET
	t.Type = s.Type,
	t.FromPoid =   s.FromPoid,
	t.FromSeq1 =   s.FromSeq1,
	t.FromSeq2 =   s.FromSeq2,
	t.FromRoll =   s.FromRoll,
	t.FromDyelot = s.FromDyelot,
	t.FromStockType = s.FromStockType,
	t.ToPoid =     s.ToPoid,
	t.ToSeq1 =     s.ToSeq1,
	t.ToSeq2 =     s.ToSeq2,
	t.ToRoll =     s.ToRoll,
	t.ToDyelot =   s.ToDyelot,
	t.ToStockType = s.ToStockType,	
	t.CmdTime = GetDate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  ID,Type,FromPOID,FromSeq1,FromSeq2,FromRoll,FromDyelot,FromStockType
	,ToPOID,ToSeq1,ToSeq2,ToRoll,ToDyelot,ToStockType,Barcode,CmdTime  ,GenSongUpdated,ukey) 
VALUES(s.ID,s.Type,s.FromPOID,s.FromSeq1,s.FromSeq2,s.FromRoll,s.FromDyelot,s.FromStockType
	,s.ToPOID,s.ToSeq1,s.ToSeq2,s.ToRoll,s.ToDyelot,s.ToStockType,s.Barcode,GetDate(),0,s.ukey)	
;

--05. 轉出區間 [Production].[dbo].[MtlLocation].EditDate= or Adddate 今天 
MERGE MtlLocation AS T
USING(
	select ID,StockType,Junk,Description
	from Production.dbo.MtlLocation
	where StockType='B'
	and (convert(date,EditDate) =  @Today or convert(date,AddDate) =  @Today)
) as S
on T.ID = S.ID and T.StockType=S.StockType
WHEN MATCHED THEN
UPDATE SET
	t.junk = s.junk,
	t.Description = s.Description,
	t.CmdTime = GetDate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  ID,  StockType,  Junk,  Description,CmdTime  ,GenSongUpdated) 
VALUES(s.ID,s.StockType,s.Junk,s.Description,GetDate(),0)	
;

--06. 轉出區間 ManufacturingExecution.dbo.RefnoRelaxtime AddDate or EditDate = 今天
select distinct
	 Refno
	,FabricRelaxationID
	,f.Relaxtime
	,[CmdTime] = GETDATE()
	,[GenSongUpdated] = 0
	into #RefnoRelaxtime
	from [ExtendServer].ManufacturingExecution.dbo.RefnoRelaxtime r
	inner join [ExtendServer].ManufacturingExecution.dbo.FabricRelaxation f on r.FabricRelaxationID=f.ID
	where (convert(date,r.EditDate) = @Today or convert(date,r.AddDate) = @Today)

MERGE RefnoRelaxtime AS T
USING #RefnoRelaxtime as S
on T.Refno = S.Refno
WHEN MATCHED THEN
UPDATE SET
	t.FabricRelaxationID = s.FabricRelaxationID,
	t.Relaxtime = s.Relaxtime,
	t.CmdTime = GetDate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  Refno,  FabricRelaxationID,  Relaxtime,CmdTime  ,GenSongUpdated) 
VALUES(s.Refno,s.FabricRelaxationID,s.Relaxtime,GetDate(),0)	
;

drop table #RefnoRelaxtime

--07. 轉出區間 [Production][CutPlan] AddDate or EditDate = 今天的資料
--且 CutPlan_detail.id = 轉出的 issue_Detail.CutplanID
MERGE Cutplan_Detail AS T
USING(
	select distinct
	[CutplanID] = cp2.ID
	,cp2.CutRef
	,[CutSeq] = ''
	,[PoID] = wo.ID
	,[Seq1] = wo.SEQ1
	,[Seq2] = wo.SEQ2
	,[Refno] = wo.Refno
	,[Article] = Article.value
	,cp2.Colorid
	,[SizeCode] = SizeCode.value
	,[CmdTime] = GETDATE()
	,[GenSongUpdated] = 0
	from  Production.dbo.Cutplan_Detail cp2
	inner join Production.dbo.Cutplan cp1 on cp2.id = cp1.id
	inner join Production.dbo.WorkOrder wo on cp2.POID = wo.ID and cp2.CutRef = wo.CutRef
	outer apply(
		select value = STUFF((
			select CONCAT(',',SizeCode)
			from(
				select distinct SizeCode
				from Production.dbo.WorkOrder_Distribute
				where WorkOrderUkey = wo.Ukey
				)s
				for xml path('')
			),1,1,'')
	) SizeCode
	outer apply(
		select value = STUFF((
			select CONCAT(',',Article)
			from(
				select distinct Article
				from Production.dbo.WorkOrder_Distribute
				where WorkOrderUkey = wo.Ukey
				and Article !=''
				)s
				for xml path('')
			),1,1,'')
	) Article
	where (convert(date,cp1.EditDate) = @Today or convert(date,cp1.AddDate) = @Today)
	and exists(select 1 from Issue_Detail where cp2.ID = CutplanID)
) as S
on T.CutRef = S.CutRef and T.CutplanID = S.CutplanID
WHEN MATCHED THEN
UPDATE SET
	[CutSeq] = '',
	t.PoID = s.PoID,
	t.Seq1 = s.Seq1,
	t.Seq2 = s.Seq2,
	t.Refno = s.Refno,
	t.Article = s.Article,
	t.Colorid = s.Colorid,
	t.SizeCode = s.SizeCode,
	t.CmdTime = GetDate(),
	t.GenSongUpdated = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  CutplanID,  CutRef,  CutSeq,  POID,  Seq1,  Seq2,  Refno,  Article,  ColorID,  SizeCode,CmdTime  ,GenSongUpdated) 
VALUES(s.CutplanID,s.CutRef,s.CutSeq,s.POID,s.Seq1,s.Seq2,s.Refno,s.Article,s.ColorID,s.SizeCode,GetDate(),0)	
;
end