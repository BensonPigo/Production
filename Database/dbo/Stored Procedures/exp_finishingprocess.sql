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
	[DestCountry]		[varchar](30) NULL,
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
	[Junk]				[bit] NOT NULL DEFAULT ((0)),
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
	[PicSetting]        bit NOT NULL DEFAULT ((0)),
	[IsMixPacking]				[bit] NULL DEFAULT ((0)),
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
	IsSSCC			 [bit] NOT NULL DEFAULT ((0)),
	ShippingMarkCombinationUkey			 [bit] NOT NULL DEFAULT ((0))
 CONSTRAINT [PK_ShippingMark] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC,
	[ShippingMarkCombinationUkey] ASC,	
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
	[PackingListID]			[varchar](15) NOT NULL,
	[SCICtnNo]  			[varchar](15) NOT NULL,
	[Side]					[varchar](5) NOT NULL,
	[GensongUpdateTime]		[varchar](50) NULL,
	[Seq]					[int] NOT NULL,
	[FilePath]				[varchar](150) NULL,
	[FileName]				[varchar](30) NULL,
	[CmdTime]				[dateTime] NOT NULL,
	[SunriseUpdated]		[bit] NOT NULL DEFAULT ((0)),
	[GenSongUpdated]		[bit] NOT NULL DEFAULT ((0)),
	[Image]					[varbinary](max) NULL,
	ShippingMarkTypeUkey	[bigint] NOT NULL DEFAULT ((0)),
	Is2Side					[bit] NOT NULL DEFAULT ((0)),
	IsHorizontal			[bit] NOT NULL DEFAULT ((0)),
	IsSSCC					[bit] NOT NULL DEFAULT ((0)),
	FromRight				[numeric](8, 2) NOT NULL DEFAULT ((0)),
	FromBottom				[numeric](8, 2) NOT NULL DEFAULT ((0)),
	Width					[int] NOT NULL DEFAULT ((0)),
	Length					[int] NOT NULL DEFAULT ((0)),
	Junk					[bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED 
(
	[SCICtnNo] ASC,	
	[PackingListID] ASC,	
	[ShippingMarkTypeUkey] ASC
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

IF OBJECT_ID(N'ShippingMarkStamp_Detail') IS NULL
BEGIN
	CREATE TABLE [dbo].[ShippingMarkStamp_Detail](
		[PackingListID] [varchar](15) NOT NULL,
		[SCICtnNo] [varchar](15) NOT NULL,
		[ShippingMarkTypeUkey] [bigint] NOT NULL,
		[FilePath] [varchar](150) NOT NULL,
		[FileName] [varchar](30) NOT NULL,
		[Image] [varbinary](max) NULL,
		[Side] [varchar](5) NOT NULL,
		[Seq] [int] NOT NULL,
		[FromRight] [numeric](8, 2) NOT NULL,
		[FromBottom] [numeric](8, 2) NOT NULL,
		[Width] [int] NOT NULL,
		[Length] [int] NOT NULL,
		[CmdTime] [dateTime] NOT NULL,
		GenSongUpdated bit NOT NULL,
		Junk bit NOT NULL,
 CONSTRAINT [PK_ShippingMarkStamp_Detail] PRIMARY KEY CLUSTERED 
(
	[SCICtnNo] ASC,	
	[PackingListID] ASC,	
	[ShippingMarkTypeUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_ShippingMarkTypeUkey]  DEFAULT ((0)) FOR [ShippingMarkTypeUkey]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_FilePath]  DEFAULT ('') FOR [FilePath]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_FileName]  DEFAULT ('') FOR [FileName]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_Side]  DEFAULT ('') FOR [Side]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_Seq]  DEFAULT ((0)) FOR [Seq]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_FromRight]  DEFAULT ((0)) FOR [FromRight]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_FromBottom]  DEFAULT ((0)) FOR [FromBottom]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_Width]  DEFAULT ((0)) FOR [Width]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_Length]  DEFAULT ((0)) FOR [Length]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_GenSongUpdated]  DEFAULT ((0)) FOR [GenSongUpdated]
	ALTER TABLE [dbo].[ShippingMarkStamp_Detail] ADD  CONSTRAINT [DF_ShippingMarkStamp_Detail_Junk]  DEFAULT ((0)) FOR [Junk]
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裝箱清單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'PackingListID'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'SCICtnNo'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipping Mark 種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'ShippingMarkTypeUkey'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HTML 位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'FilePath'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HTML 名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'FileName'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BMP圖片二進位制資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'Image'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貼碼面, 上下左右前後' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'Side'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'Seq'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'離右邊的位置(mm)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'FromRight'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'離下面的位置(mm)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'FromBottom'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標籤寬度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'Width'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標籤長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'Length'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI寫入/更新此筆資料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'CmdTime'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GenSong是否已轉製' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'GenSongUpdated'
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'判斷資料是否需要移除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkStamp_Detail', @level2type=N'COLUMN',@level2name=N'Junk'
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

IF OBJECT_ID(N'CFANeedInsp') IS NULL
BEGIN 
CREATE TABLE [dbo].[CFANeedInsp] (
    [SCICtnNo]			varchar(15) NOT NULL,
	[CmdTime]			Datetime NOT NULL,
    [GenSongUpdated]	bit   NOT NULL DEFAULT ((0)) ,
	CONSTRAINT [PK_CFANeedInsp] PRIMARY KEY CLUSTERED 
	(
		[SCICtnNo] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	;	
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI箱號', N'SCHEMA', N'dbo', N'TABLE', N'CFANeedInsp', N'COLUMN', N'SCICtnNo';
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI寫入/更新此筆資料時間', N'SCHEMA', N'dbo', N'TABLE', N'CFANeedInsp', N'COLUMN', N'CmdTime';
	EXECUTE sp_addextendedproperty N'MS_Description', N'GenSong是否已轉製', N'SCHEMA', N'dbo', N'TABLE', N'CFANeedInsp', N'COLUMN', N'GenSongUpdated';
END

IF OBJECT_ID(N'ClogGarmentDispose') IS NULL
BEGIN 
CREATE TABLE [dbo].[ClogGarmentDispose] (
    [SCICtnNo]			varchar(15) NOT NULL,
	[CmdTime]			Datetime NOT NULL,
    [Dispose]			bit   NOT NULL DEFAULT ((1)) ,
    [GenSongUpdated]	bit   NOT NULL DEFAULT ((0)) ,
	CONSTRAINT [PK_ClogGarmentDispose] PRIMARY KEY CLUSTERED 
	(
		[SCICtnNo] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	;	
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI箱號', N'SCHEMA', N'dbo', N'TABLE', N'ClogGarmentDispose', N'COLUMN', N'SCICtnNo';
	EXECUTE sp_addextendedproperty N'MS_Description', N'SCI寫入/更新此筆資料時間', N'SCHEMA', N'dbo', N'TABLE', N'ClogGarmentDispose', N'COLUMN', N'CmdTime';
	EXECUTE sp_addextendedproperty N'MS_Description', N'0:轉回倉庫; 1:轉出倉庫報廢', N'SCHEMA', N'dbo', N'TABLE', N'ClogGarmentDispose', N'COLUMN', N'Dispose';
	EXECUTE sp_addextendedproperty N'MS_Description', N'GenSong是否已轉製', N'SCHEMA', N'dbo', N'TABLE', N'ClogGarmentDispose', N'COLUMN', N'GenSongUpdated';
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
--並記錄下這次有哪些OrderID異動
SELECT ID ,POID
INTO #tmpOrders
FROM Production.dbo.Orders o
where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate or convert(date,PulloutCmplDate) = @cDate)

MERGE Orders AS T
USING(
	SELECT o.id,BrandID,ProgramID,StyleID,SeasonID,ProjectID,Category,OrderTypeID,Dest,CustCDID,StyleUnit
	,[SetQty] = (select count(1) cnt from Production.dbo.Style_Location where o.StyleUkey=StyleUkey)
	,[Location] = sl.Location , o.PulloutComplete, o.Junk,[CmdTime] = GETDATE()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0, [CustPONo], o.POID ,[DestCountry] = c.Alias
	FROM Production.dbo.Orders o
	LEFT JOIN Production.dbo.Country c ON o.Dest = c.ID
	outer apply(	
	select Location = STUFF((
		select distinct CONCAT(',',Location) 
		from Production.dbo.Style_Location oa WITH (NOLOCK)
		where StyleUkey=o.StyleUkey
		for xml path('')
	),1,1,'')) SL
	where o.id in (select ID from #tmpOrders)
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
	t.POID = s.POID,
	t.DestCountry = s.DestCountry
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   BrandID,   ProgramID,   StyleID,   SeasonID,   ProjectID,   Category,   OrderTypeID
	,  Dest,   CustCDID,   StyleUnit,   SetQty,   Location,   PulloutComplete,   Junk
	,  CmdTime,   SunriseUpdated,   GenSongUpdated, CustPONo, POID	,DestCountry) 
VALUES(s.id, s.BrandID, s.ProgramID, s.StyleID, s.SeasonID, s.ProjectID, s.Category, s.OrderTypeID
	,s.Dest, s.CustCDID, s.StyleUnit, s.SetQty, s.Location, s.PulloutComplete, s.Junk
	,s.CmdTime, s.SunriseUpdated, s.GenSongUpdated, s.CustPONo, s.POID	,s.DestCountry)	;

--02. 轉出區間 [Production].[dbo].[Order_QtyShip].ID 在本次有更新的 Orders 之中
MERGE Order_QtyShip AS T
USING(
	SELECT id, Seq, ShipmodeID, BuyerDelivery, Qty, EstPulloutDate
	,ReadyDate,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0, [GenSongUpdated] = 0
	FROM Production.dbo.Order_QtyShip o
	WHERE o.ID IN (SELECT ID FROM #tmpOrders)
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
	t.GenSongUpdated = s.GenSongUpdated,
	t.Junk = 0
WHEN NOT MATCHED BY TARGET THEN
INSERT(  id,   Seq,   ShipmodeID,   BuyerDelivery,   Qty,   EstPulloutDate,   ReadyDate,
	     SunriseUpdated,   GenSongUpdated,   CmdTime) 
VALUES(s.id, s.Seq, s.ShipmodeID, s.BuyerDelivery, s.Qty, s.EstPulloutDate, s.ReadyDate,
	   s.SunriseUpdated, s.GenSongUpdated, s.CmdTime)	;

	   
----不刪除，只Junk
UPDATE t
SET t.Junk = 1, t.CmdTime = GetDate() ,t.SunriseUpdated = 0 ,t.GenSongUpdated = 0
FROM Order_QtyShip t
WHERE NOT EXISTS(
	SELECT 1 FROM Production.dbo.Order_QtyShip s
	WHERE t.ID = s.ID and t.SEQ = s.SEQ
)
AND t.Junk = 0

--05-1. ShippingMarkPic_Detail轉出區間 當AddDate or EditDate =今天
MERGE ShippingMarkPic_Detail AS T
USING(
	SELECT 
	s1.SCICtnNo--*PK
	,s1.Side
	,s1.Seq
	,[FilePath] = ''-- (select ShippingMarkPath from Production.dbo.System)
	,[FileName]=''--s1.FileName	
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0
	,[GenSongUpdated] = 0
	,s1.Image
	,s1.ShippingMarkTypeUkey--*PK
	,s2.PackingListID--*PK
	,s1.Is2Side
	,s1.IsHorizontal
	,s1.IsSSCC
	,s1.FromRight
	,s1.FromBottom
	,s1.Width
	,s1.Length
	FROM Production.dbo.ShippingMarkPic_Detail s1
	inner join Production.dbo.ShippingMarkPic s2 on s2.ukey = s1.ShippingMarkPicUkey
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on t.SCICtnNo = s.SCICtnNo and t.PackingListID =s.PackingListID  and t.ShippingMarkTypeUkey=s.ShippingMarkTypeUkey
WHEN MATCHED THEN
UPDATE SET
	 t.Side=s.Side
	,t.Seq = s.Seq
	,t.FilePath = s.FilePath
	,t.FileName = s.FileName
	,t.CmdTime = s.CmdTime
	,t.SunriseUpdated = 0
	,t.GenSongUpdated = 0
	,t.Image = s.Image	
	,t.Is2Side=s.Is2Side
	,t.IsHorizontal=s.IsHorizontal
	,t.IsSSCC=s.IsSSCC
	,t.FromRight=s.FromRight
	,t.FromBottom=s.FromBottom
	,t.Width=s.Width
	,t.Length=s.Length
WHEN NOT MATCHED BY TARGET THEN
INSERT
(SCICtnNo	,Side	,Seq	,FilePath	,FileName	,CmdTime	,SunriseUpdated	,GenSongUpdated	,Image	,ShippingMarkTypeUkey
	,PackingListID	,Is2Side	,IsHorizontal	,IsSSCC	,FromRight	,FromBottom	,Width	,Length)
VALUES(s.[SCICtnNo],s.[Side],s.[Seq],s.[FilePath],s.[FileName],s.[CmdTime],s.[SunriseUpdated],s.[GenSongUpdated],s.[Image],s.ShippingMarkTypeUkey
	,s.PackingListID	,s.Is2Side	,s.IsHorizontal	,s.IsSSCC	,s.FromRight	,s.FromBottom	,s.Width	,s.Length
);

UPDATE fps
SET Junk = 1 , SunriseUpdated = 0 , GenSongUpdated = 0 ,CmdTime = GetDate()
FROM ShippingMarkPic_Detail fps
WHERE NOT EXISTS(
	SELECT	 1
	FROM Production.dbo.ShippingMarkPic_Detail s1
	inner join Production.dbo.ShippingMarkPic s2 on s2.ukey = s1.ShippingMarkPicUkey
	WHERE s1.SCICtnNo = fps.SCICtnNo AND s1.ShippingMarkTypeUkey = fps.ShippingMarkTypeUkey AND s2.PackingListID = fps.PackingListID
)
AND Junk = 0


--05-2. ShippingMarkStamp_Detail轉出區間 當AddDate or EditDate =今天
MERGE ShippingMarkStamp_Detail AS T
USING(
	SELECT 
	 s1.SCICtnNo--*PK
	,s1.Side
	,s1.Seq
	,s1.[FilePath]
	,s1.[FileName]
	,[CmdTime] = GetDate()
	,[GenSongUpdated] = 0
	,s1.Image
	,s1.ShippingMarkTypeUkey--*PK
	,s2.PackingListID--*PK
	,s1.FromRight
	,s1.FromBottom
	,s1.Width
	,s1.Length
	FROM Production.dbo.ShippingMarkStamp_Detail s1
	inner join Production.dbo.ShippingMarkStamp s2 on s2.PackingListID = s1.PackingListID
	where (convert(date,AddDate) = @cDate or convert(date,EditDate) = @cDate)
) as S
on t.SCICtnNo = s.SCICtnNo and t.PackingListID =s.PackingListID  and t.ShippingMarkTypeUkey=s.ShippingMarkTypeUkey
WHEN MATCHED THEN
UPDATE SET
	 t.Side=s.Side
	,t.Seq = s.Seq
	,t.FilePath = s.FilePath
	,t.FileName = s.FileName
	,t.CmdTime = s.CmdTime
	,t.GenSongUpdated = 0
	,t.Image = s.Image	
	,t.FromRight=s.FromRight
	,t.FromBottom=s.FromBottom
	,t.Width=s.Width
	,t.Length=s.Length
WHEN NOT MATCHED BY TARGET THEN
INSERT
(PackingListID	,SCICtnNo	,ShippingMarkTypeUkey	,FilePath	,FileName	,Image	,Side	,Seq
           ,FromRight	,FromBottom	,Width	,Length	,CmdTime	,GenSongUpdated)
VALUES
(s.PackingListID	,s.SCICtnNo	,s.ShippingMarkTypeUkey	,s.FilePath	,s.FileName	,s.Image	,s.Side		,s.Seq
           ,s.FromRight	,s.FromBottom	,s.Width	,s.Length	,s.CmdTime	,s.GenSongUpdated);

UPDATE fps
SET Junk = 1 , GenSongUpdated = 0 ,CmdTime = GetDate()
FROM ShippingMarkStamp_Detail fps
WHERE NOT EXISTS(
	SELECT	 1
	FROM Production.dbo.ShippingMarkStamp_Detail s1
	inner join Production.dbo.ShippingMarkStamp s2 on s2.PackingListID = s1.PackingListID
	WHERE s1.SCICtnNo = fps.SCICtnNo AND s1.ShippingMarkTypeUkey = fps.ShippingMarkTypeUkey AND s2.PackingListID = fps.PackingListID
)
AND Junk = 0

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

--07-1. PackingList_Detail (PIC) 轉出區間 [Production].[dbo].[PackingList].AddDate or EditDate=今天，更新 PicSetting
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
	,[PicSetting]= [FPS].[dbo].CheckShippingMarkPicSetting(p.ID,pd.SCICtnNo,pd.RefNO, IIF( isnull(MixCount.val, 0) > 1 , 1 ,0),p.CustCDID,p.BrandID)
	FROM Production.dbo.PackingList p
	inner join Production.dbo.PackingList_Detail pd on p.ID=pd.ID
	left join  Production.dbo.ShipPlan sp on sp.id=p.ShipPlanID
	LEFT JOIN Production.dbo.ShippingMarkPic pic ON pic.PackingListID = p.ID
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
		or convert(date,sp.AddDate) = @cDate or convert(date,sp.EditDate) = @cDate
		or convert(date,pic.AddDate) = @cDate or convert(date,pic.EditDate) = @cDate
	)
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
	t.IsMixPacking = s.IsMixPacking,
	t.PicSetting = s.PicSetting
WHEN NOT MATCHED BY TARGET THEN
INSERT(  ID, SCICtnNo
, CustCTN
,  PulloutDate,  OrderID, OrderShipmodeSeq, Article, SizeCode
		, ShipQty, Barcode, GW, CtnRefno, CtnLength, CtnWidth, CtnHeight, CtnUnit
	,Junk	,CmdTime, SunriseUpdated, GenSongUpdated,PackingCTN ,IsMixPacking ,PicSetting) 
VALUES(s.ID, s.SCICtnNo, 
iif(s.CustCTN ='' or s.CustCTN is null,s.SCICtnNo,s.CustCTN)
, s.PulloutDate, s.OrderID, s.OrderShipmodeSeq, s.Article, s.SizeCode
		, s.ShipQty, s.Barcode, s.GW, s.CtnRefno, s.CtnLength, s.CtnWidth, s.CtnHeight, s.CtnUnit
	, s.Junk, s.CmdTime, s.SunriseUpdated, s.GenSongUpdated,s.PackingCTN ,s.IsMixPacking ,s.PicSetting)
WHEN NOT MATCHED BY SOURCE 
	AND exists(	select 1 from #tmpPackingList where id = t.id) THEN
	UPDATE SET
	t.Junk = 1	;
		
--07-2. PackingList_Detail (HTML) 轉出區間 [Production].[dbo].[PackingList].AddDate or EditDate=今天，更新 HTMLSetting
MERGE PackingList_Detail AS T
USING(
	SELECT pd.ID, pd.SCICtnNo, pd.CustCTN, p.PulloutDate, pd.OrderID, pd.OrderShipmodeSeq
	,pd.Article, pd.SizeCode, pd.ShipQty, pd.Barcode, pd.GW, [CtnRefno] = pd.RefNo
	,LocalItem.CtnLength, LocalItem.CtnWidth, LocalItem.CtnHeight
	,LocalItem.CtnUnit
	,[Junk] = iif(fpsPacking.ID is not null,1,0)
	,[CmdTime] = GetDate()
	,[SunriseUpdated] = 0
	,[GenSongUpdated] = 0
	,[PackingCTN] = pd.id + pd.CTNStartNo
	,[IsMixPacking]= 0
	,[HTMLSetting] = [FPS].[dbo].CheckShippingMarkHTMLSetting(p.ID,pd.SCICtnNo,pd.RefNO, p.CustCDID,p.BrandID)
	FROM Production.dbo.PackingList p
	inner join Production.dbo.PackingList_Detail pd on p.ID=pd.ID
	left join  Production.dbo.ShipPlan sp on sp.id=p.ShipPlanID
	LEFT JOIN Production.dbo.ShippingMarkStamp stamp ON stamp.PackingListID = p.ID
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
	where (convert(date,p.AddDate) = @cDate or convert(date,p.EditDate) = @cDate
		or convert(date,sp.AddDate) = @cDate or convert(date,sp.EditDate) = @cDate
		or convert(date,stamp.AddDate) = @cDate or convert(date,stamp.EditDate) = @cDate
	)
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
	t.IsMixPacking = s.IsMixPacking,
	t.HTMLSetting = s.HTMLSetting
WHEN NOT MATCHED BY TARGET THEN
INSERT(  ID, SCICtnNo, CustCTN
, PulloutDate, OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty, Barcode, GW,
	CtnRefno, CtnLength, CtnWidth, CtnHeight, CtnUnit, Junk, CmdTime, SunriseUpdated, GenSongUpdated, PackingCTN, IsMixPacking, HTMLSetting) 
VALUES(
s.ID, s.SCICtnNo, 
iif(s.CustCTN ='' or s.CustCTN is null,s.SCICtnNo,s.CustCTN)
, s.PulloutDate, s.OrderID, s.OrderShipmodeSeq, s.Article, s.SizeCode
		, s.ShipQty, s.Barcode, s.GW, s.CtnRefno, s.CtnLength, s.CtnWidth, s.CtnHeight, s.CtnUnit
	, s.Junk, s.CmdTime, s.SunriseUpdated, s.GenSongUpdated,s.PackingCTN ,s.IsMixPacking ,s.HTMLSetting)
WHEN NOT MATCHED BY SOURCE 
	AND exists(	select 1 from #tmpPackingList where id = t.id) THEN
	UPDATE SET
	t.Junk = 1	;

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

--13. Order_SizeCode  轉出區間 當AddDate or EditDate =今天

MERGE Order_SizeCode AS T
USING(
	SELECT *
	FROM Production.dbo.Order_SizeCode
	WHERE (convert(date,AddDate) = @cDate OR convert(date,EditDate) = @cDate) OR ID IN (SELECT POID FROM #tmpOrders)
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

--15. CFANeedInsp
MERGE CFANeedInsp AS T
USING(
	SELECT DISTINCT pd.SCICtnNo, [CmdTime]=GETDATE(), [GenSongUpdated]=0
	FROM Production.dbo.PackingList p
	INNER JOIN Production.dbo.PackingList_Detail pd ON p.ID= pd.ID
	where pd.CFASelectInspDate = @cDate AND pd.CFANeedInsp = 1
) as S
on t.SCICtnNo = s.SCICtnNo
WHEN MATCHED THEN
UPDATE SET
	t.CmdTime = s.CmdTime,
	t.GenSongUpdated = s.GenSongUpdated
WHEN NOT MATCHED BY TARGET THEN
INSERT(SCICtnNo, CmdTime, GenSongUpdated )
Values(s.SCICtnNo, s.CmdTime, s.GenSongUpdated )
;

--16. ClogGarmentDispose
MERGE ClogGarmentDispose AS T
USING(
	SELECT DISTINCT pd.SCICtnNo, [CmdTime]=GETDATE(), [Dispose] = IIF(a.Status='Confirmed',1,0), [GenSongUpdated]=0
	FROM Production.dbo.ClogGarmentDispose a 
	INNER JOIN Production.dbo.ClogGarmentDispose_Detail b ON a.ID= b.ID
	INNER JOIN Production.dbo.PackingList p ON p.ID = b.PackingListID
	INNER JOIN Production.dbo.PackingList_Detail pd On p.ID = pd.ID AND pd.CTNStartNO = b.CTNStartNO
	WHERE (Cast(a.EditDate as Date) = @cDate OR Cast(a.AddDate as Date) = @cDate)
) as S
on t.SCICtnNo = s.SCICtnNo
WHEN MATCHED THEN
UPDATE SET
	t.CmdTime = s.CmdTime,
	t.Dispose = s.Dispose,
	t.GenSongUpdated = s.GenSongUpdated
WHEN NOT MATCHED BY TARGET THEN
INSERT(SCICtnNo, CmdTime, Dispose, GenSongUpdated )
Values(s.SCICtnNo, s.CmdTime, s.Dispose,  s.GenSongUpdated )
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