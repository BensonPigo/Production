CREATE TABLE [dbo].[TrimApproval](
	[SuppID] [varchar](6) NOT NULL,
	[TestDocFactoryGroup] [varchar](8) NOT NULL,
	[Refno] [varchar](50) NOT NULL,
	[ColorID] [varchar](6) NOT NULL,
	[SeasonID] [varchar](8) NOT NULL,
	[Period] [int] NOT NULL,
	[FirstDyelot] [date] NOT NULL,
	[AWBno] [varchar](30) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[FTYReceivedReport] [date] NULL,
	[ReceivedDate] [date] NULL,
	[ReceivedRemark] [varchar](max) NOT NULL,
	[DocumentName] [varchar](100) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Junk] [bit] NOT NULL,
	[LOT] [varchar](500) NOT NULL,
 CONSTRAINT [PK_TrimApproval] PRIMARY KEY CLUSTERED 
(
	[SuppID] ASC,
	[TestDocFactoryGroup] ASC,
	[Refno] ASC,
	[ColorID] ASC,
	[SeasonID] ASC,
	[DocumentName] ASC,
	[BrandID] ASC,
	[LOT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TrimApproval] ADD  DEFAULT ((0)) FOR [Period]
GO

ALTER TABLE [dbo].[TrimApproval] ADD  DEFAULT ((0)) FOR [Junk]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件適用廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'TestDocFactoryGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'ColorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Period' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'Period'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'頭缸卡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'FirstDyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AWBno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'AWBno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠接收日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'FTYReceivedReport'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'台北接收日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'ReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接收Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'ReceivedRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'DocumentName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否Junk' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LOT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval', @level2type=N'COLUMN',@level2name=N'LOT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trim審核資料主檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TrimApproval'
GO


