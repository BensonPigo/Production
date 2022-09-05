
CREATE TABLE [dbo].[Bundle_Detail_History](
	[BundleNo] [varchar](10) NOT NULL,
	[Id] [varchar](13) NOT NULL,
	[BundleGroup] [numeric](5, 0) NULL,
	[Patterncode] [varchar](20) NOT NULL,
	[PatternDesc] [nvarchar](100) NOT NULL,
	[SizeCode] [varchar](8) NULL,
	[Qty] [numeric](5, 0) NULL,
	[Parts] [numeric](5, 0) NULL,
	[Farmin] [numeric](5, 0) NULL,
	[FarmOut] [numeric](5, 0) NULL,
	[PrintDate] [datetime] NULL,
	[IsPair] [bit] NULL,
	[Location] [varchar](1) NOT NULL,
	[RFUID] [varchar](10) NULL,
	[Tone] [varchar](1) NOT NULL,
	[RFPrintDate] [datetime] NULL,
	[PrintGroup] [tinyint] NULL,
	[RFIDScan] [bit] NOT NULL,
	[Dyelot] VARCHAR(50) CONSTRAINT [DF_Bundle_Detail_History_Dyelot] DEFAULT ('') NOT NULL,
 CONSTRAINT [PK_Bundle_Detail_History] PRIMARY KEY CLUSTERED 
(
	[BundleNo] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_BundleNo_Bundle_Detail_History] UNIQUE NONCLUSTERED 
(
	[BundleNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_BundleNo]  DEFAULT ('') FOR [BundleNo]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_Id]  DEFAULT ((0)) FOR [Id]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_BundleGroup]  DEFAULT ((0)) FOR [BundleGroup]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_Patterncode]  DEFAULT ('') FOR [Patterncode]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_PatternDesc]  DEFAULT ('') FOR [PatternDesc]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_Parts]  DEFAULT ((0)) FOR [Parts]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_Farmin]  DEFAULT ((0)) FOR [Farmin]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_FarmOut]  DEFAULT ((0)) FOR [FarmOut]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_RFUID]  DEFAULT ('') FOR [RFUID]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_Tone]  DEFAULT ('') FOR [Tone]
GO

ALTER TABLE [dbo].[Bundle_Detail_History] ADD  CONSTRAINT [DF_Bundle_Detail_History_RFIDScan]  DEFAULT ((0)) FOR [RFIDScan]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捆包號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'BundleNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Group' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'BundleGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版片編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'Patterncode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版片敘述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'PatternDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'SizeCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Part 數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'Parts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發收入數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'Farmin'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發發出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'FarmOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'for SNP RF Card UID, printer(CHP_1800)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'RFUID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RF Print Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'RFPrintDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列印Bundle Card時的群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'PrintGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle 資訊印在 RFID card 上的時候, 是否要加上 RFID 的mark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History', @level2type=N'COLUMN',@level2name=N'RFIDScan'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle Detail History' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_Detail_History'
GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Bundle_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'