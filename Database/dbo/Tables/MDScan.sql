CREATE TABLE [dbo].[MDScan](
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
	[SCICtnNo] [varchar](15) NOT NULL,
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