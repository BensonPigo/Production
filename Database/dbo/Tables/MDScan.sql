Create TABLE [dbo].[MDScan](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[ScanDate] [date] NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[PackingListID] [varchar](13) NOT NULL,
	[CTNStartNo] [varchar](6) NOT NULL,
	[CartonQty] [int] NOT NULL,
	[MDFailQty] [int] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[SCICtnNo] [varchar](16) NOT NULL,
	[DataRemark] [nvarchar](200) NOT NULL  CONSTRAINT [DF_MDScan_DataRemark] DEFAULT (''),
	[Status] VARCHAR(6) NOT NULL  CONSTRAINT [DF_MDScan_Status] DEFAULT (''),
    [Remark] NVARCHAR(MAX) NOT NULL  CONSTRAINT [DF_MDScan_Remark] DEFAULT (''),
    [IsFromM360] INT NOT NULL  CONSTRAINT [DF_MDScan_IsFromM360] DEFAULT ((0)),
    [HoldRemark] nvarchar(max) NOT NULL CONSTRAINT [DF_MDScan_HoldRemark] DEFAULT (''),
    CONSTRAINT [PK_MDScan] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_PackingListID]  DEFAULT ('') FOR [PackingListID]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_CTNStartNo]  DEFAULT ('') FOR [CTNStartNo]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_CartonQty]  DEFAULT ((0)) FOR [CartonQty]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_MDFailQty]  DEFAULT ((0)) FOR [MDFailQty]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[MDScan] ADD  CONSTRAINT [DF_MDScan_SCICtnNo]  DEFAULT ('') FOR [SCICtnNo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'掃描日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MDScan', @level2type=N'COLUMN',@level2name=N'ScanDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MDScan', @level2type=N'COLUMN',@level2name=N'CTNStartNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'箱內的成衣件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MDScan', @level2type=N'COLUMN',@level2name=N'CartonQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金屬檢測失敗數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MDScan', @level2type=N'COLUMN',@level2name=N'MDFailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI內部箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MDScan', @level2type=N'COLUMN',@level2name=N'SCICtnNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按下Hold寫入的Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MDScan', @level2type=N'COLUMN',@level2name=N'HoldRemark'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MD狀態
通過 : Pass
不通過 : Hold
退回 : Return',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDScan',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'退回原因備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDScan',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'資料從哪寫入
0 = 來自Sewing P08
1 = 來自M360 MD
2 = 來自M360 Sewing P08',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDScan',
    @level2type = N'COLUMN',
    @level2name = N'IsFromM360'