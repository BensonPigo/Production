CREATE TABLE [dbo].[FirstDyelot](
	[SuppID] [varchar](6) NOT NULL,
	[TestDocFactoryGroup] [varchar](8) NOT NULL,
	[BrandRefno] [varchar](50) NOT NULL,
	[ColorID] [varchar](6) NOT NULL,
	[SeasonID] [varchar](8) NOT NULL,
	[Period] [int] NOT NULL,
	[FirstDyelot] [date] NULL,
	[AWBno] [varchar](30) NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[FTYReceivedReport] [date] NULL,
	[ReceivedDate] [date] NULL,
	[ReceivedRemark] [varchar](max) NULL,
	[DocumentName] [varchar](100) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[deleteColumn] [bit] NOT NULL,
	[LOT] [varchar](500) NOT NULL,
 CONSTRAINT [PK_FirstDyelot] PRIMARY KEY CLUSTERED 
(
	[SuppID] ASC,
	[TestDocFactoryGroup] ASC,
	[BrandRefno] ASC,
	[ColorID] ASC,
	[SeasonID] ASC,
	[DocumentName] ASC,
	[BrandID] ASC,
	[LOT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_SuppID]  DEFAULT ('') FOR [SuppID]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_TestDocFactoryGroup]  DEFAULT ('') FOR [TestDocFactoryGroup]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_BrandRefno]  DEFAULT ('') FOR [BrandRefno]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_ColorID]  DEFAULT ('') FOR [ColorID]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_Period]  DEFAULT ((0)) FOR [Period]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_AWBno]  DEFAULT ('') FOR [AWBno]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_ReceivedRemark]  DEFAULT ('') FOR [ReceivedRemark]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_DocumentName]  DEFAULT ('') FOR [DocumentName]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  DEFAULT ((0)) FOR [deleteColumn]
GO

ALTER TABLE [dbo].[FirstDyelot] ADD  CONSTRAINT [DF_FirstDyelot_LOT]  DEFAULT ('') FOR [LOT]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'頭缸卡工廠群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'TestDocFactoryGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌料號
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'BrandRefno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色ID
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'ColorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節ID
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'頭缸卡日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'FirstDyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AWBno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'AWBno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人名
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠接收頭缸卡的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'FTYReceivedReport'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'台北接收頭缸卡的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'ReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠接收頭缸卡的備註
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'ReceivedRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'測試報告文件名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'DocumentName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'刪除資料用備份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'deleteColumn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LOT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstDyelot', @level2type=N'COLUMN',@level2name=N'LOT'
GO