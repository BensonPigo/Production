-- =============================================
-- Description:	轉出FPS資料
-- =============================================
Create PROCEDURE [dbo].[exp_finishingprocess]
	@inputDate date = null
AS
Begin try

IF OBJECT_ID(N'Orders') IS NULL
BEGIN
	CREATE TABLE [dbo].[Orders](
	[id]				[varchar](13) NOT NULL,
	[BrandID]			[varchar](8) NULL,
	[ProgramID]			[varchar](12) NULL,
	[StyleID]			[varchar](15) NULL,
	[SeasonID]			[varchar](10) NULL,
	[ProjectID]			[varchar](5) NULL,
	[Category]			[varchar](1) NULL,
	[OrderTypeID]		[varchar](20) NULL,
	[Dest]				[varchar](2) NULL,
	[CustCDID]			[varchar](16) NULL,
	[StyleUnit]			[varchar](8) NULL,
	[SetQty]			[int] NOT NULL,
	[Location]			[varchar](7) NULL,
	[PulloutComplete]	[bit] NULL,
	[Junk]				[bit] NULL DEFAULT ((0)),
	[CmdTime]			[datetime] NOT NULL,
	[SunriseUpdated]	[bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated]	[bit] NOT NULL DEFAULT ((0)),
	[CustPONo]			[varchar](30) NULL,
	[POID]				[varchar](13) NULL  DEFAULT ('')
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF OBJECT_ID(N'Order_QtyShip') IS NULL
BEGIN
	CREATE TABLE [dbo].[Order_QtyShip](
	[id]				[varchar](13) NOT NULL,
	[Seq]				[varchar](2) NOT NULL,
	[ShipmodeID]		[varchar](10) NULL,
	[BuyerDelivery]		[date] NULL,
	[Qty]				[int] NULL,
	[EstPulloutDate]	[date] NULL,
	[ReadyDate]			[date] NULL,
	[CmdTime]			[datetime] NOT NULL,
	[SunriseUpdated]	[bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated]	[bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Order_QtyShip] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF OBJECT_ID(N'PackingList_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[PackingList_Detail](
	[ID]				[varchar](13) NOT NULL,
	[SCICtnNo]			[varchar](15) NOT NULL,
	[CustCTN]			[varchar](30) NOT NULL,
	[PulloutDate]		[date] NULL,
	[OrderID]			[varchar](13) NOT NULL,
	[OrderShipmodeSeq]	[varchar](2) NOT NULL,
	[Article]			[varchar](8) NOT NULL,
	[SizeCode]			[varchar](8) NOT NULL,
	[ShipQty]			[int] NULL,
	[Barcode]			[varchar](30) NULL,
	[GW]				[numeric](7, 3) NULL,
	[CtnRefno]			[varchar](21) NULL,
	[CtnLength]			[numeric](8, 4) NULL,
	[CtnWidth]			[numeric](8, 4) NULL,
	[CtnHeight]			[numeric](8, 4) NULL,
	[CtnUnit]			[varchar](8) NULL,
	[Junk]				[bit] NULL DEFAULT ((0)),
	[CmdTime]			[datetime] NOT NULL,
	[SunriseUpdated]	[bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated]	[bit] NOT NULL DEFAULT ((0)),
	[PackingCTN]        [varchar](19) NOT NULL,
	[HTMLSetting]       bit NOT NULL DEFAULT ((0)),
	[PicSetting]        bit NOT NULL DEFAULT ((0))
 CONSTRAINT [PK_PackingList_Detail] PRIMARY KEY CLUSTERED 
(
	[SCICtnNo] ASC,
	[Article] ASC,
	[SizeCode] ASC,
	[OrderID] ASC,
	[OrderShipmodeSeq]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END	

IF OBJECT_ID(N'PackingList') IS NULL
BEGIN
	CREATE TABLE [dbo].[PackingList](
	[id]		[varchar](13) NOT NULL,
	[AddDate]	[datetime] NOT NULL,
	[EditDate]	[datetime] NULL,
	[CmdTime]	[datetime] NOT NULL,
	[junk]		[bit] NULL DEFAULT ((0)),
 CONSTRAINT [PK_PackingList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF OBJECT_ID(N'ClogReturn') IS NULL
BEGIN
	CREATE TABLE [dbo].[ClogReturn](
	[ID]			 [bigint] NOT NULL,
	[SCICtnNo]		 [varchar](15) NOT NULL,
	[ReturnDate]	 [date] NOT NULL,
	[OrderID]		 [varchar](13) NOT NULL,
	[PackingListID]  [varchar](13) NOT NULL,
	[CustCTN]		 [varchar](30) NULL,
	[CmdTime]		 [datetime] NOT NULL,
	[SunriseUpdated] [bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_ClogReturn] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF OBJECT_ID(N'TransferToCFA') IS NULL
BEGIN
	CREATE TABLE [dbo].[TransferToCFA](
	[ID]			 [bigint] NOT NULL,
	[SCICtnNo]		 [varchar](15) NOT NULL,
	[TransferDate]	 [date] NOT NULL,
	[OrderID]		 [varchar](13) NOT NULL,
	[PackingListID]  [varchar](13) NOT NULL,
	[CustCTN]		 [varchar](30) NULL,
	[CmdTime]		 [datetime] NOT NULL,
	[SunriseUpdated] [int] NOT NULL DEFAULT ((0)),
	[GenSongUpdated] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_TransferToCFA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF OBJECT_ID(N'ShippingMark') IS NULL
BEGIN
	CREATE TABLE [dbo].[ShippingMark](
	[ID]			 [bigint] IDENTITY(1,1) NOT NULL,
	[BrandID]		 [varchar](8) NOT NULL DEFAULT(('')),
	[CTNRefno]		 [varchar](21) NOT NULL DEFAULT(('')),
	[Side]			 [varchar](5) NOT NULL DEFAULT(('')),
	[Seq]			 [int] NOT NULL DEFAULT ((0)),
	[Category]		 [varchar](4) NOT NULL DEFAULT(('')),
	[FromRight]		 [int] NOT NULL DEFAULT ((0)),
	[FromBottom]	 [int] NOT NULL DEFAULT ((0)),
	[StickerSizeID]	 [bigint] NOT NULL DEFAULT ((0)),
	[Is2Side]		 [bit] NOT NULL  DEFAULT (0),
	[FileName]		 [varchar](25) NOT NULL DEFAULT (('')),
	[CmdTime]		 [dateTime] NOT NULL,
	[SunriseUpdated] [bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
	IsHorizontal	 [bit] NOT NULL DEFAULT ((0)),
	FilePath		 [varchar](80) NOT NULL DEFAULT (('')),
	IsSSCC			 [bit] NOT NULL DEFAULT ((0))
 CONSTRAINT [PK_ShippingMark] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,	
	[BrandID] ASC,
	[CTNRefno] ASC,	
	[Side] ASC,
	[Seq] ASC,
	[Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

	EXECUTE sp_addextendedproperty N'MS_Description', N'ID', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'ID'
	EXECUTE sp_addextendedproperty N'MS_Description', N'客戶名稱', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'BrandID'
	EXECUTE sp_addextendedproperty N'MS_Description', N'紙箱料號', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'CTNRefno'
	EXECUTE sp_addextendedproperty N'MS_Description', N'貼碼面, 上下左右前後', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'Side'
	EXECUTE sp_addextendedproperty N'MS_Description', N'序號', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'Seq'
	EXECUTE sp_addextendedproperty N'MS_Description', N'類別, 噴碼/貼碼(HTML/PIC)', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'Category'
	EXECUTE sp_addextendedproperty N'MS_Description', N'離右邊的位置(mm)', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'FromRight'
	EXECUTE sp_addextendedproperty N'MS_Description', N'離下面的位置(mm)', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'FromBottom'
	EXECUTE sp_addextendedproperty N'MS_Description', N'尺寸貼紙ID', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'StickerSizeID'
	EXECUTE sp_addextendedproperty N'MS_Description', N'是否轉角貼, (0,1)', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'Is2Side'
	EXECUTE sp_addextendedproperty N'MS_Description', N'HTML檔名', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'FileName'
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI寫入/更新此筆資料時間', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'CmdTime'
	EXECUTE sp_addextendedproperty N'MS_Description', N'Sunrise是否已轉製', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'SunriseUpdated'
	EXECUTE sp_addextendedproperty N'MS_Description', N'GenSong是否已轉製', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMark', N'COLUMN', N'GenSongUpdated'
END

IF OBJECT_ID(N'ShippingMarkPic_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[ShippingMarkPic_Detail](
	[SCICtnNo]  	 [varchar](15) NOT NULL,
	[Side]			 [varchar](5) NOT NULL,
	[Seq]			 [int] NOT NULL,
	[FilePath]		 [varchar](80) NULL,
	[FileName]		 [varchar](30) NULL,
	[CmdTime]		 [dateTime] NOT NULL,
	[SunriseUpdated] [bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
	[Image] [varbinary](max) NULL
 CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED 
(
	[SCICtnNo] ASC,	
	[Side] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI箱號', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'SCICtnNo'
	EXECUTE sp_addextendedproperty N'MS_Description', N'貼碼面, 上下左右前後', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'Side'
	EXECUTE sp_addextendedproperty N'MS_Description', N'序號', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'Seq'
	EXECUTE sp_addextendedproperty N'MS_Description', N'圖檔位置', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'FilePath'
	EXECUTE sp_addextendedproperty N'MS_Description', N'圖檔名稱', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'FileName'
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI寫入/更新此筆資料時間', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'CmdTime'
	EXECUTE sp_addextendedproperty N'MS_Description', N'Sunrise是否已轉製', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'SunriseUpdated'
	EXECUTE sp_addextendedproperty N'MS_Description', N'GenSong是否已轉製', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'GenSongUpdated'
	EXECUTE sp_addextendedproperty N'MS_Description', N'圖片二進位制資料', N'SCHEMA', N'dbo', N'TABLE', N'ShippingMarkPic_Detail', N'COLUMN', N'Image'
END

IF OBJECT_ID(N'StickerSize') IS NULL
BEGIN
	CREATE TABLE [dbo].[StickerSize](
		[ID] [bigint] NOT NULL DEFAULT(0),
		[Size] [varchar](20) NOT NULL DEFAULT(''),
		[Width] [int] NOT NULL DEFAULT(0),
		[Length] [int] NOT NULL DEFAULT(0),
		[AddName] [varchar](10) NOT NULL DEFAULT(''),
		[AddDate] [datetime] NULL,
		[EditName] [varchar](10) NOT NULL DEFAULT(''),
		[EditDate] [datetime] NULL,
	 CONSTRAINT [PK_StickerSize] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF OBJECT_ID(N'ClogReturn') IS NULL
BEGIN
	CREATE TABLE [dbo].[ClogReturn](
	[ID]			 [bigint] NOT NULL,
	[SCICtnNo]		 [varchar](15) NOT NULL,
	[ReturnDate]	 [date] NOT NULL,
	[OrderID]		 [varchar](13) NOT NULL,
	[PackingListID]  [varchar](13) NOT NULL,
	[CustCTN]		 [varchar](30) NULL,
	[CmdTime]		 [datetime] NOT NULL,
	[SunriseUpdated] [bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated] [bit] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_ClogReturn] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF OBJECT_ID(N'FinishingProcess') IS NULL
BEGIN 
CREATE TABLE [dbo].[FinishingProcess] (
    [DM300]			 TINYINT      DEFAULT ((0)) NOT NULL,
    [DM200]			 INT          DEFAULT ((0)) NULL,
    [DM201]			 INT          DEFAULT ((0)) NULL,
    [DM202]			 INT          DEFAULT ((0)) NULL,
    [DM205]			 INT          DEFAULT ((0)) NULL,
    [DM203]			 INT          DEFAULT ((0)) NULL,
    [DM204]			 INT          DEFAULT ((0)) NULL,
    [DM206]			 INT          DEFAULT ((0)) NULL,
    [DM207]			 INT          DEFAULT ((0)) NULL,
    [DM208]			 INT          DEFAULT ((0)) NULL,
    [DM209]			 INT          DEFAULT ((0)) NULL,
    [DM210]			 INT          DEFAULT ((0)) NULL,
    [DM212]			 INT          DEFAULT ((0)) NULL,
    [DM214]			 INT          DEFAULT ((0)) NULL,
    [DM215]			 INT          DEFAULT ((0)) NULL,
    [DM216]			 INT          DEFAULT ((0)) NULL,
    [DM219]			 INT          DEFAULT ((0)) NULL,
    [CmdTime]		 datetime NULL,
    [SunriseUpdated] bit   DEFAULT ((0)) NULL
	)
END

IF OBJECT_ID(N'StyleFPSetting') IS NULL
BEGIN 
CREATE TABLE [dbo].[StyleFPSetting] (
    [StyleID]			varchar(15),
	[SeasonID]			varchar(10),
	[BrandID]			varchar(8),
    [CmdTime]			datetime NULL,
    [SunriseUpdated]	bit   DEFAULT ((0)) NULL,
	[Pressing1]			INT   DEFAULT ((1)) NULL,
	[Pressing2]			INT   DEFAULT ((0)) NULL,
	[Folding1]			INT   DEFAULT ((0)) NULL,
	[Folding2]			INT   DEFAULT ((0)) NULL
	)
END

IF OBJECT_ID(N'Order_SizeCode') IS NULL
BEGIN 
CREATE TABLE [dbo].[Order_SizeCode] (
	ID				[varchar](13) NOT NULL DEFAULT (''),
	Seq				[varchar](2)  NULL DEFAULT (''),
	SizeGroup		[varchar](1)  NULL DEFAULT (''),
	SizeCode		[varchar](8)  NOT NULL DEFAULT (''),
	Ukey			[bigint]	  NOT NULL DEFAULT (0),
	Junk			[bit]		  NOT NULL DEFAULT (0),
	CmdTime			[datetime]	  NOT NULL,
	SunriseUpdated	[bit]		  NOT NULL DEFAULT (0),
 CONSTRAINT [PK_Order_SizeCode] PRIMARY KEY CLUSTERED 
(
	ID ASC,
	SizeCode ASC,
	Ukey ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF OBJECT_ID(N'LocalItem') IS NULL
BEGIN 
CREATE TABLE [dbo].[LocalItem] (
	Refno				[varchar](21) NOT NULL ,
	UnPack				[bit]		  NOT NULL DEFAULT (0),
	Junk				[bit]		  NOT NULL DEFAULT (0),
	CmdTime				[datetime]	  NULL,
	SunriseUpdated		[bit]		  NOT NULL DEFAULT (0),
	GenSongUpdated		[bit]		  NOT NULL DEFAULT (0),
 CONSTRAINT [PK_LocalItem] PRIMARY KEY CLUSTERED 
(
	Refno ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

declare @cDate date = @inputDate;
declare @yestarDay date =CONVERT(Date, dateAdd(day,-1,GetDate()));
--declare @cDate date = CONVERT(date, DATEADD(DAY,-10, GETDATE()));-- for test
--declare @yestarDay date =CONVERT(Date, dateAdd(day,-11,GetDate()));-- for test

if(@inputDate is null)
begin
	set @cDate = CONVERT(date,GETDATE());
end

--01. 轉出區間 [Production].[dbo].[Orders].AddDate or EditDate= 今天
MERGE Orders AS T
USING(
	SELECT id,BrandID,ProgramID,StyleID,SeasonID,ProjectID,Category,OrderTypeID,Dest,CustCDID,StyleUnit
	,[SetQty] = (select count(1) cnt from Production.dbo.Style_Location where o.StyleUkey=StyleUkey)
	,[Location] = sl.Location , o.PulloutComplete, o.Junk,[CmdTime] = GETDATE()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0, [CustPONo], o.POID
	FROM Production.dbo.Orders o
	outer apply(	
	select Location = STUFF((
		select distinct CONCAT(',',Location) 
		from Production.dbo.Style_Location oa WITH (NOLOCK)
		where StyleUkey=o.StyleUkey
		for xml path('')
	),1,1,'')) SL
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on T.ID = S.ID
WHEN MATCHED THEN
UPDATE SET
	t.ID = s.id,
	t.BrandID = s.BrandID,
	t.ProgramID = s.ProgramID,
	t.StyleID = s.StyleID,
	t.SeasonID = s.SeasonID,
	t.ProjectID = s.ProjectID,
	t.Category = s.Category,
	t.OrderTypeID = s.OrderTypeID,
	t.Dest = s.Dest,
	t.CustCDID = s.CustCDID,
	t.StyleUnit = s.StyleUnit,
	t.SetQty = s.SetQty,
	t.Location = s.Location,
	t.PulloutComplete = s.PulloutComplete,
	t.Junk = s.Junk,
	t.CmdTime = GetDate(),
	t.SunriseUpdated = s.SunriseUpdated,
	t.GenSongUpdated = s.GenSongUpdated,
	t.CustPONo = s.CustPONo,
	t.POID = s.POID
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   BrandID,   ProgramID,   StyleID,   SeasonID,   ProjectID,   Category,   OrderTypeID
	,  Dest,   CustCDID,   StyleUnit,   SetQty,   Location,   PulloutComplete,   Junk
	,  CmdTime,   SunriseUpdated,   GenSongUpdated, CustPONo, POID) 
VALUES(s.id, s.BrandID, s.ProgramID, s.StyleID, s.SeasonID, s.ProjectID, s.Category, s.OrderTypeID
	,s.Dest, s.CustCDID, s.StyleUnit, s.SetQty, s.Location, s.PulloutComplete, s.Junk
	,s.CmdTime, s.SunriseUpdated, s.GenSongUpdated, s.CustPONo, s.POID)	;

--02. 轉出區間 [Production].[dbo].[Order_QtyShip].AddDate or EditDate= 今天
MERGE Order_QtyShip AS T
USING(
	SELECT id, Seq, ShipmodeID, BuyerDelivery, Qty, EstPulloutDate
	,ReadyDate,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	FROM Production.dbo.Order_QtyShip o
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on T.ID = S.ID and T.SEQ = S.SEQ
WHEN MATCHED THEN
UPDATE SET
	t.ID = s.id,
	t.Seq = s.Seq,
	t.ShipmodeID = s.ShipmodeID,
	t.BuyerDelivery = s.BuyerDelivery,
	t.Qty = s.Qty,
	t.EstPulloutDate = s.EstPulloutDate,
	t.ReadyDate = s.ReadyDate,
	t.CmdTime = s.CmdTime,
	t.SunriseUpdated = s.SunriseUpdated,
	t.GenSongUpdated = s.GenSongUpdated
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   Seq,   ShipmodeID,   BuyerDelivery,   Qty,   EstPulloutDate,   ReadyDate,
	     SunriseUpdated,   GenSongUpdated,   CmdTime ) 
VALUES(s.id, s.Seq, s.ShipmodeID, s.BuyerDelivery, s.Qty, s.EstPulloutDate, s.ReadyDate,
	   s.SunriseUpdated, s.GenSongUpdated, s.CmdTime)	;

--03. 轉出區間 當AddDate or EditDate =今天、Category = 'HTML'
MERGE ShippingMark AS T
USING(
	SELECT 
	BrandID
	,CTNRefno
	,Side
	,FromRight
	,FromBottom
	,StickerSizeID
	,[Seq]=1
	,[Category] = 'HTML'
	,[Is2Side] = 0 ,FileName
	,[CmdTime] = GetDate()
	,[FilePath] = (select TOP 1 ShippingMarkPath from Production.dbo.System)
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	FROM Production.dbo.ShippingMarkStamp 
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on t.BrandID = s.BrandID and t.CTNRefno=s.CTNRefno and t.Side=s.Side and t.Seq=s.Seq and t.Category=s.Category
WHEN MATCHED THEN
UPDATE SET
	t.FromRight = s.FromRight,
	t.FromBottom = s.FromBottom,
	t.StickerSizeID = s.StickerSizeID,
	t.Is2Side = s.Is2Side,
	t.FileName = s.FileName,
	t.CmdTime = s.CmdTime,
	t.SunriseUpdated = 0,
	t.GenSongUpdated = 0,
	t.[FilePath] = s.[FilePath]
WHEN NOT MATCHED BY TARGET THEN
INSERT([BrandID] ,[CTNRefno]	,[Side]	,[Seq] ,[Category] ,[FromRight] ,[FromBottom] ,[StickerSizeID]
		,[Is2Side],[FileName],[CmdTime],[SunriseUpdated],	[GenSongUpdated] ,[FilePath])
Values(s.[BrandID] ,s.[CTNRefno],s.[Side],s.[Seq],s.[Category] ,s.[FromRight] ,s.[FromBottom] ,s.[StickerSizeID]
		,s.[Is2Side],s.[FileName],s.[CmdTime],s.[SunriseUpdated],s.[GenSongUpdated] ,s.[FilePath]);

--04. 轉出區間 當AddDate or EditDate =今天、Category = 'PIC'
MERGE ShippingMark AS T
USING(
	SELECT 
	BrandID
	,CTNRefno
	,Side
	,FromRight
	,FromBottom
	,StickerSizeID
	,Seq,[Category] = 'PIC'
	,[Is2Side] = Is2Side ,[FileName]=''
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	,[IsHorizontal]
	,[FilePath] = (select TOP 1 ShippingMarkPath from Production.dbo.System)
	,IsSSCC
	FROM Production.dbo.ShippingMarkpicture 
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on t.BrandID = s.BrandID and t.CTNRefno=s.CTNRefno and t.Side=s.Side and t.Seq=s.Seq and t.Category=s.Category
WHEN MATCHED THEN
UPDATE SET
	t.FromRight = s.FromRight,
	t.FromBottom = s.FromBottom,
	t.StickerSizeID = s.StickerSizeID,
	t.Is2Side = s.Is2Side,
	t.FileName = s.FileName,
	t.CmdTime = s.CmdTime,
	t.SunriseUpdated = 0,
	t.GenSongUpdated = 0,
	t.IsHorizontal = s.IsHorizontal,
	t.FilePath = s.FilePath,
	t.IsSSCC = s.IsSSCC
WHEN NOT MATCHED BY TARGET THEN
INSERT([BrandID]	,[CTNRefno]	,[Side]	,[Seq] ,[Category] 	,[FromRight] ,[FromBottom] ,[StickerSizeID]
		,[Is2Side],[FileName],[CmdTime],[SunriseUpdated],	[GenSongUpdated] ,[IsHorizontal] ,[FilePath],[IsSSCC])

Values(s.[BrandID],s.[CTNRefno],s.[Side],s.[Seq],s.[Category] ,s.[FromRight] ,s.[FromBottom] ,s.[StickerSizeID]
		,s.[Is2Side],s.[FileName],s.[CmdTime],s.[SunriseUpdated],s.[GenSongUpdated] ,s.[IsHorizontal] ,s.[FilePath],s.[IsSSCC]);

--05. 轉出區間 當AddDate or EditDate =今天
MERGE ShippingMarkPic_Detail AS T
USING(
	SELECT 
	s1.SCICtnNo,s2.Side,s2.Seq
	,[FilePath] = (select ShippingMarkPath from Production.dbo.System)
	,s1.FileName	
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	,s1.Image
	FROM Production.dbo.ShippingMarkPic_Detail s1
	inner join Production.dbo.ShippingMarkPic s2 on s2.ukey = s1.ShippingMarkPicUkey
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on t.SCICtnNo = s.SCICtnNo and t.Side=s.Side and t.Seq=s.Seq
WHEN MATCHED THEN
UPDATE SET
	t.FilePath = s.FilePath,
	t.FileName = s.FileName,
	t.CmdTime = s.CmdTime,
	t.SunriseUpdated = 0,
	t.GenSongUpdated = 0,
	t.Image = s.Image
WHEN NOT MATCHED BY TARGET THEN
INSERT([SCICtnNo],[Side],[Seq],[FilePath],[FileName],[CmdTime],[SunriseUpdated],[GenSongUpdated],[Image])
VALUES(s.[SCICtnNo],s.[Side],s.[Seq],s.[FilePath],s.[FileName],s.[CmdTime],s.[SunriseUpdated],s.[GenSongUpdated],s.[Image]);

--06. 轉出區間 [Production].[dbo].[PackingList].AddDate or EditDate=今天
select * 
into #tmpPackingList
from Production.dbo.PackingList p
where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)

MERGE PackingList AS T
USING(
	SELECT p.ID, p.AddDate, p.EditDate
	,[Junk] = 0
	,[CmdTime] = GetDate()
	FROM Production.dbo.PackingList p
	where exists(
		select 1 from #tmpPackingList where id = p.id	)
) as S
on T.ID = S.ID
WHEN MATCHED THEN
UPDATE SET
	t.ID = s.id,
	t.AddDate = s.AddDate,
	t.EditDate = s.EditDate,
	t.junk = s.junk,
	t.CmdTime = s.CmdTime
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   AddDate,   EditDate,   CmdTime,   junk) 
VALUES(s.id, s.AddDate, s.EditDate, s.CmdTime, s.junk)	;

-- [FPS].[dbo].[PackingList] 近期3個月的ID存在於Production.dbo.PackingList
--則Junk=0, 反之為1
update t
set t.junk = iif(s.ID is null, 1,0)
from PackingList t
left join Production.dbo.PackingList s
on t.id=s.ID
where ( CONVERT(date, t.AddDate) > CONVERT(date, DATEADD(MONTH,-3, GETDATE()))
	or (CONVERT(date, t.EditDate) > CONVERT(date, DATEADD(MONTH,-3, GETDATE()))))

--07. 轉出區間 [Production].[dbo].[PackingList].AddDate or EditDate=今天
MERGE PackingList_Detail AS T
USING(
	SELECT pd.ID, pd.SCICtnNo, pd.CustCTN, p.PulloutDate, pd.OrderID, pd.OrderShipmodeSeq
	,pd.Article, pd.SizeCode, pd.ShipQty, pd.Barcode, pd.GW, [CtnRefno] = pd.RefNo
	,LocalItem.CtnLength, LocalItem.CtnWidth, LocalItem.CtnHeight
	,LocalItem.CtnUnit
	,[Junk] = iif(fpsPacking.ID is not null,1,0)
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	,[PackingCTN] = pd.id + pd.CTNStartNo
	,[IsMixPacking]= IIF( isnull(MixCount.val, 0) > 1 , 1 ,0)
	FROM Production.dbo.PackingList p
	inner join Production.dbo.PackingList_Detail pd on p.ID=pd.ID
	left join  Production.dbo.ShipPlan sp on sp.id=p.ShipPlanID
	OUTER APPLY(
		SELECT fp2.id
		FROM FPS.dbo.PackingList fp1
		inner join FPS.dbo.PackingList_Detail fp2 on fp1.id=fp2.id
		where fp1.id = p.id
		and fp1.junk=1 and fp2.SunriseUpdated = 0 and fp2.GenSongUpdated = 0
	) fpsPacking
	outer apply(
		select * 
		from  Production.dbo.LocalItem l
		where l.RefNo=pd.RefNo
	) LocalItem	
	OUTER APPLY(
		select val = count(1)
		from (
			SELECT DISTINCT p2.Article ,p2.SizeCode
			FROm Production.dbo.PackingList_Detail p2
			WHERE p2.ID = pd.ID  AND p2.CTNStartNo= pd.CTNStartNo 
		) CheckMix
	)MixCount
	where (convert(date,p.AddDate) = @cDate or convert(date,p.EditDate) = @cDate
	or convert(date,sp.AddDate) = @cDate or convert(date,sp.EditDate) = @cDate)
) as S
on T.SCICtnNo = S.SCICtnNo and T.Article = s.Article and T.SizeCode = s.Sizecode
AND T.OrderID = S.OrderID AND T.OrderShipmodeSeq = S.OrderShipmodeSeq
WHEN MATCHED THEN
UPDATE SET
	t.ID = s.id,
	t.CustCTN = iif(s.CustCTN ='' or s.CustCTN is null,s.SCICtnNo,s.CustCTN),
	t.PulloutDate = s.PulloutDate,
	t.ShipQty = s.ShipQty,
	t.Barcode = s.Barcode,
	t.GW = s.GW,
	t.CtnRefno = s.CtnRefno,
	t.CtnLength = s.CtnLength,
	t.CtnWidth = s.CtnWidth,
	t.CtnHeight = s.CtnHeight,
	t.CtnUnit = s.CtnUnit,
	t.junk = s.junk,
	t.CmdTime = s.CmdTime,
	t.SunriseUpdated = s.SunriseUpdated,
	t.GenSongUpdated = s.GenSongUpdated,
	t.PackingCTN = s.PackingCTN,
	t.IsMixPacking = s.IsMixPacking
WHEN NOT MATCHED BY TARGET THEN
INSERT(  ID, SCICtnNo
, CustCTN
,  PulloutDate,  OrderID, OrderShipmodeSeq, Article, SizeCode
		, ShipQty, Barcode, GW, CtnRefno, CtnLength, CtnWidth, CtnHeight, CtnUnit
	,Junk	,CmdTime, SunriseUpdated, GenSongUpdated,PackingCTN ,IsMixPacking) 
VALUES(s.ID, s.SCICtnNo, 
iif(s.CustCTN ='' or s.CustCTN is null,s.SCICtnNo,s.CustCTN)
, s.PulloutDate, s.OrderID, s.OrderShipmodeSeq, s.Article, s.SizeCode
		, s.ShipQty, s.Barcode, s.GW, s.CtnRefno, s.CtnLength, s.CtnWidth, s.CtnHeight, s.CtnUnit
	, s.Junk, s.CmdTime, s.SunriseUpdated, s.GenSongUpdated,s.PackingCTN ,s.IsMixPacking)
WHEN NOT MATCHED BY SOURCE 
	AND exists(	select 1 from #tmpPackingList where id = t.id) THEN
	UPDATE SET
	t.Junk = 1	;
		
----寫入HTMLSetting和PicSetting
-- 如果FPS.dbo.PackingList.Junk=1， 則update FPS.dbo.PackingList_Detail
update t
set t.Junk= 1
,t.SunriseUpdated = 0
,t.GenSongUpdated = 0
from PackingList_Detail t 
inner join PackingList s on t.ID=s.id
where s.junk=1

		-------------------------------------------------------------HTMLSetting Update-------------------------------------------------------------
		----搜尋出Packing B02有設定的
		SELECT DISTINCT 
				pd.SCICtnNo
				,pd.OrderID
				,pd.OrderShipmodeSeq
				,pd.Article
				,pd.SizeCode
				,s.FileName
		INTO #HasSetting_HTMLSetting
		FROM [Production].[dbo].PackingList p 
		INNER JOIN [FPS].[dbo].PackingList_Detail pd ON p.id=pd.ID
		LEFT  JOIN [FPS].[dbo].ShippingMark s ON p.BrandID=s.BrandID AND pd.CtnRefno=s.CTNRefno
		WHERE s.Category='HTML'

		----有設定的：FileName有值=1，空白=0
		UPDATE pd
		SET pd.HTMLSetting = IIF(t.FileName = '' ,0 , 1)
		FROM [FPS].[dbo].PackingList_Detail pd
		INNER JOIN #HasSetting_HTMLSetting t ON    pd.SCICtnNo =t.SCICtnNo
							AND pd.OrderID=t.OrderID
							AND pd.OrderShipmodeSeq=t.OrderShipmodeSeq
							AND pd.Article=t.Article
							AND pd.SizeCode=t.SizeCode
							
		----沒設定的：=1		
		UPDATE pd
		SET pd.HTMLSetting = 1
		FROM [FPS].[dbo].PackingList_Detail pd
		WHERE NOT EXISTS (SELECT 1 FROM #HasSetting_HTMLSetting t
							WHERE pd.SCICtnNo =t.SCICtnNo
												AND pd.OrderID=t.OrderID
												AND pd.OrderShipmodeSeq=t.OrderShipmodeSeq
												AND pd.Article=t.Article
												AND pd.SizeCode=t.SizeCode
						)

		DROP TABLE #HasSetting_HTMLSetting,#tmpPackingList
		-------------------------------------------------------------HTMLSetting Update-------------------------------------------------------------

		-------------------------------------------------------------PicSetting Update-------------------------------------------------------------
		
		----以PackingList_Detail為出發點

		----PicSetting = 1 有設定對應的Packing B03 且有上傳圖片至Packing P24
		SELECT DISTINCT 
				pd.SCICtnNo
				,pd.OrderID
				,pd.OrderShipmodeSeq
				,pd.Article
				,pd.SizeCode
				,s.FileName
		INTO #tmp_HasSetting_File
		FROM [Production].[dbo].PackingList p 
		INNER JOIN [FPS].[dbo].PackingList_Detail pd ON p.id=pd.ID
		INNER JOIN [FPS].[dbo].ShippingMark s ON p.BrandID=s.BrandID AND pd.CtnRefno=s.CTNRefno
		WHERE s.Category='PIC' 
		AND NOT EXISTS( -- P24 沒有圖片FileName為空的
			SELECT 1
			FROM [FPS].[dbo].ShippingMark sm
			INNER JOIN [FPS].[dbo].ShippingMarkPic_Detail spd ON sm.Side=spd.Side AND sm.Seq = spd.Seq
			INNER JOIN [FPS].[dbo].[PackingList_Detail] pld ON pld.SCICtnNo=spd.SCICtnNo
			WHERE pld.SCICtnNo=pd.SCICtnNo AND pld.OrderID=pd.OrderID AND pd.OrderShipmodeSeq=pld.OrderShipmodeSeq AND pd.Article=pld.Article AND pd.SizeCode=pld.SizeCode
			AND spd.Image IS NULL
		)

		----找不到 PicSetting = 1 表示沒有設定對應的Packing B03, 不需考慮貼標
		SELECT DISTINCT 
				pd.SCICtnNo
				,pd.OrderID
				,pd.OrderShipmodeSeq
				,pd.Article
				,pd.SizeCode
		INTO #tmp_NoSetting
		FROM [Production].[dbo].PackingList p 
		INNER JOIN [FPS].[dbo].PackingList_Detail pd ON p.id=pd.ID
		INNER JOIN [FPS].[dbo].ShippingMark s ON p.BrandID=s.BrandID AND pd.CtnRefno=s.CTNRefno
		WHERE s.Category='PIC'

		----PicSetting = 0 表示需貼標 但沒有上傳圖片至Packing P24
		SELECT DISTINCT 
				pd.SCICtnNo
				,pd.OrderID
				,pd.OrderShipmodeSeq
				,pd.Article
				,pd.SizeCode
		INTO #tmp_HasSetting_NoFile
		FROM [Production].[dbo].PackingList p 
		INNER JOIN [FPS].[dbo].PackingList_Detail pd ON p.id=pd.ID
		INNER JOIN [FPS].[dbo].ShippingMark s ON p.BrandID=s.BrandID AND pd.CtnRefno=s.CTNRefno
		WHERE s.Category='PIC' 
		AND EXISTS( -- P24 有圖片FileName為空的
			SELECT 1
			FROM [FPS].[dbo].ShippingMark sm
			INNER JOIN [FPS].[dbo].ShippingMarkPic_Detail spd ON sm.Side=spd.Side AND sm.Seq = spd.Seq
			INNER JOIN [FPS].[dbo].[PackingList_Detail] pld ON pld.SCICtnNo=spd.SCICtnNo
			WHERE pld.SCICtnNo=pd.SCICtnNo AND pld.OrderID=pd.OrderID AND pd.OrderShipmodeSeq=pld.OrderShipmodeSeq AND pd.Article=pld.Article AND pd.SizeCode=pld.SizeCode
			AND spd.Image IS NULL
		)

		UPDATE pd
		SET pd.PicSetting = 1
		FROM [FPS].dbo.PackingList_Detail pd
		INNER JOIN #tmp_HasSetting_File t ON    pd.SCICtnNo =t.SCICtnNo
							AND pd.OrderID=t.OrderID
							AND pd.OrderShipmodeSeq=t.OrderShipmodeSeq
							AND pd.Article=t.Article
							AND pd.SizeCode=t.SizeCode
					
		UPDATE pd
		SET pd.PicSetting = 1
		FROM [FPS].dbo.PackingList_Detail pd
		WHERE NOT EXISTS ( SELECT * FROM #tmp_NoSetting t WHERE pd.SCICtnNo =t.SCICtnNo
													AND pd.OrderID=t.OrderID
													AND pd.OrderShipmodeSeq=t.OrderShipmodeSeq
													AND pd.Article=t.Article
													AND pd.SizeCode=t.SizeCode) 

		UPDATE pd
		SET pd.PicSetting = 0
		FROM [FPS].dbo.PackingList_Detail pd
		INNER JOIN #tmp_HasSetting_NoFile t ON    pd.SCICtnNo =t.SCICtnNo
							AND pd.OrderID=t.OrderID
							AND pd.OrderShipmodeSeq=t.OrderShipmodeSeq
							AND pd.Article=t.Article
							AND pd.SizeCode=t.SizeCode

		DROP TABLE #tmp_HasSetting_File,#tmp_NoSetting ,#tmp_HasSetting_NoFile
		-------------------------------------------------------------PicSetting Update-------------------------------------------------------------

--08. 轉出區間 [Production].[dbo]. [ClogReturn].AddDate=今天
MERGE ClogReturn AS T
USING(
	SELECT distinct c.ID,c.SCICtnNo,c.ReturnDate,c.OrderID,c.PackingListID
	,[CustCTN] = iif(pd.CustCTN ='' or pd.CustCTN is null,pd.SCICtnNo,pd.CustCTN)
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	FROM Production.dbo.ClogReturn c
	left join Production.dbo.PackingList_Detail pd on c.PackingListID=pd.ID
	and c.CTNStartNo=pd.CTNStartNo
	where convert(date,AddDate) = @cDate
) as S
on T.ID = S.ID
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   SCICtnNo,   ReturnDate,   OrderID,   PackingListID,   CustCTN,
		  CmdTime,   SunriseUpdated,   GenSongUpdated) 
VALUES(s.id, s.SCICtnNo, s.ReturnDate, s.OrderID, s.PackingListID, s.CustCTN,
		s.CmdTime, s.SunriseUpdated, s.GenSongUpdated)	;

--09. 轉出區間 [Production].[dbo].[TransferToCFA].AddDate =今天
MERGE TransferToCFA AS T
USING(
	SELECT distinct t.ID, t.SCICtnNo, t.TransferDate, t.OrderID, t.PackingListID
	,[CustCTN] = iif(pd.CustCTN ='' or pd.CustCTN is null,pd.SCICtnNo,pd.CustCTN)
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	FROM Production.dbo.TransferToCFA t
	left join Production.dbo.PackingList_Detail pd on t.PackingListID=pd.ID
	and t.CTNStartNo=pd.CTNStartNo
	where convert(date,AddDate) = @cDate
) as S
on T.ID = S.ID
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   SCICtnNo,   TransferDate,   OrderID,   PackingListID,   CustCTN,
		  CmdTime,   SunriseUpdated,   GenSongUpdated) 
VALUES(s.id, s.SCICtnNo, s.TransferDate, s.OrderID, s.PackingListID, s.CustCTN,
		s.CmdTime, s.SunriseUpdated, s.GenSongUpdated);


--10. 轉出區間 當AddDate or EditDate =今天
MERGE FinishingProcess AS T
USING(
	SELECT *
	FROM Production.dbo.FinishingProcess
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as s
on t.DM300=s.DM300
WHEN MATCHED THEN
UPDATE SET
    t.[DM300]   =s.[DM300],			
	t.[DM200]	=s.[DM200],			
	t.[DM201]	=s.[DM201],			
	t.[DM202]	=s.[DM202],			
	t.[DM205]	=s.[DM205],			
	t.[DM203]	=s.[DM203],			
	t.[DM204]	=s.[DM204],			
	t.[DM206]	=s.[DM206],			
	t.[DM207]	=s.[DM207],			
	t.[DM208]	=s.[DM208],			
	t.[DM209]	=s.[DM209],			
	t.[DM210]	=s.[DM210],			
	t.[DM212]	=s.[DM212],			
	t.[DM214]	=s.[DM214],			
	t.[DM215]	=s.[DM215],			
	t.[DM216]	=s.[DM216],			
	t.[DM219]	=s.[DM219],			
	t.[CmdTime]	= GetDate(),	
	t.[SunriseUpdated] = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT([DM300],[DM200],[DM201],[DM202],[DM205],[DM203],[DM204],[DM206],[DM207],
	   [DM208],[DM209],[DM210],[DM212],[DM214],[DM215],[DM216],[DM219],[CmdTime],[SunriseUpdated])
VALUES(s.[DM300],s.[DM200],s.[DM201],s.[DM202],s.[DM205],s.[DM203],s.[DM204],s.[DM206],s.[DM207],
	   s.[DM208],s.[DM209],s.[DM210],s.[DM212],s.[DM214],s.[DM215],s.[DM216],s.[DM219],GetDate(),0);


--11. 轉出區間 當AddDate or TPEEditDate or EditDate =今天
MERGE StyleFPSetting AS T
USING(
	SELECT  [StyleID] = id
		,[SeasonID]
		,[BrandID]
		,Pressing1
		,Pressing2
		,Folding1
		,Folding2
	FROM Production.dbo.Style 
	where (convert(date,AddDate) = @cDate or convert(date,TPEEditDate) = @cDate or convert(date,EditDate) = @cDate)
) as s
on t.StyleID=s.StyleID and t.SeasonID=s.SeasonID and t.BrandID=s.BrandID
WHEN MATCHED THEN
UPDATE SET
   t.[StyleID]		=s.[StyleID],               
   t.[SeasonID]		=s.[SeasonID],	
   t.[BrandID]		=s.[BrandID],	
   t.[Pressing1]	=s.[Pressing1],
   t.[Pressing2]	=s.[Pressing2],
   t.[Folding1]		=s.[Folding1],
   t.[Folding2]		=s.[Folding2],
   t.[CmdTime]	= GetDate(),	
   t.[SunriseUpdated] = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT([StyleID], [SeasonID], [BrandID], [Pressing1], [Pressing2], [Folding1], [Folding2], [CmdTime], [SunriseUpdated])
VALUES(s.[StyleID] ,s.[SeasonID], s.[BrandID], s.[Pressing1], s.[Pressing2], s.[Folding1], s.[Folding2], GetDate(), 0);


--12. 轉出區間 當AddDate or EditDate =今天
MERGE StickerSize AS T
USING(
	SELECT *
	FROM Production.dbo.StickerSize
	where (convert(date,AddDate) = @cDate OR convert(date,EditDate) = @cDate)
) as s
on t.ID = s.ID
WHEN MATCHED THEN
UPDATE SET
    t.ID		=s.ID,			
	t.Size		=s.Size,			
	t.Width		=s.Width,			
	t.Length	=s.Length,			
	t.AddName	=s.AddName,			
	t.AddDate	=s.AddDate,			
	t.EditName	=s.EditName,			
	t.EditDate	=s.EditDate		
WHEN NOT MATCHED BY TARGET THEN
INSERT( [ID], [Size] ,[Width] ,[Length] ,[AddName] ,[AddDate] ,[EditName] ,[EditDate] )
VALUES( s.[ID], s.[Size] ,s.[Width] ,s.[Length] ,s.[AddName] ,s.[AddDate] ,s.[EditName] ,s.[EditDate] )
;

--13. Order_SizeCode  轉出區間 當AddDate or EditDate =今天

MERGE Order_SizeCode AS T
USING(
	SELECT *
	FROM Production.dbo.Order_SizeCode
	WHERE (convert(date,AddDate) = @cDate OR convert(date,EditDate) = @cDate)
) as s
on t.ID = s.ID AND t.SizeCode = s.SizeCode AND t.Ukey = s.Ukey
WHEN MATCHED THEN
UPDATE SET
    t.Seq				= s.Seq,			
	t.SizeGroup			= s.SizeGroup,			
	t.Junk				= 0,		
	t.CmdTime			= GETDATE(),			
	t.SunriseUpdated	= 0
WHEN NOT MATCHED BY TARGET THEN
	INSERT( ID		,Seq		,SizeGroup		,SizeCode		,Ukey		,Junk		,CmdTime		,SunriseUpdated)
	VALUES( s.ID	,s.Seq		,s.SizeGroup	,s.SizeCode		,s.Ukey		,0			,GETDATE()		,0			   )
;
----不刪除，只Junk
UPDATE t
SET Junk = 1, CmdTime = GetDate(), SunriseUpdated = 0
FROM Order_SizeCode t
WHERE NOT EXISTS(
	SELECT 1 FROM Production.dbo.Order_SizeCode s
	WHERE t.ID = s.ID AND t.SizeCode = s.SizeCode AND t.Ukey = s.Ukey
)

--14. LocalItem  轉出區間 當AddDate or EditDate =今天

MERGE LocalItem AS T
USING(
	SELECT *
	FROM Production.dbo.LocalItem
	WHERE (convert(date,AddDate) = @cDate OR convert(date,EditDate) = @cDate) AND Category = 'CARTON' 
) as s
on t.Refno = s.Refno
WHEN MATCHED THEN
UPDATE SET
    t.UnPack			= s.UnPack,			
	t.Junk				= s.Junk,			
	t.CmdTime			= GETDATE(),		
	t.SunriseUpdated	= 0,			
	t.GenSongUpdated	= 0
WHEN NOT MATCHED BY TARGET THEN
	INSERT( Refno		,UnPack		,Junk		,CmdTime		,SunriseUpdated		,GenSongUpdated		)
	VALUES( s.Refno		,s.UnPack	,s.Junk		,GETDATE()		,0					,0					)
;

END try
Begin Catch
	DECLARE  @ErrorMessage  NVARCHAR(4000),  
			 @ErrorSeverity INT,    
			 @ErrorState    INT;
	SELECT     
		@ErrorMessage  = ERROR_MESSAGE(),    
		@ErrorSeverity = ERROR_SEVERITY(),   
		@ErrorState    = ERROR_STATE();

	RAISERROR (@ErrorMessage, -- Message text.    
				 @ErrorSeverity, -- Severity.    
				 @ErrorState -- State.    
			   ); 
End Catch