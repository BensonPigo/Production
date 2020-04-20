CREATE TABLE [dbo].[OrderChangeApplication](
	[ID] [varchar](13) NOT NULL,
	[ReasonID] [varchar](5) NULL,
	[OrderID] [varchar](13) NULL,
	[Status] [varchar](15) NULL,
	[SentName] [varchar](10) NULL,
	[SentDate] [datetime] NULL,
	[ApprovedName] [varchar](10) NULL,
	[ApprovedDate] [datetime] NULL,
	[ConfirmedName] [varchar](10) NULL,
	[ConfirmedDate] [datetime] NULL,
	[RejectName] [varchar](10) NULL,
	[RejectDate] [datetime] NULL,
	[ClosedName] [varchar](10) NULL,
	[ClosedDate] [datetime] NULL,
	[JunkName] [varchar](10) NULL,
	[JunkDate] [datetime] NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[ToOrderID] [varchar](13) NULL,
	[NeedProduction] [bit] NOT NULL,
	[OldQty] [numeric](6, 0) NOT NULL,
	[RatioFty] [numeric](5, 2) NOT NULL,
	[RatioSubcon] [numeric](5, 2) NOT NULL,
	[RatioSCI] [numeric](5, 2) NOT NULL,
	[RatioSupp] [numeric](5, 2) NOT NULL,
	[RatioBuyer] [numeric](5, 2) NOT NULL,
	[ResponsibleFty] [bit] NOT NULL,
	[ResponsibleSubcon] [bit] NOT NULL,
	[ResponsibleSCI] [bit] NOT NULL,
	[ResponsibleSupp] [bit] NOT NULL,
	[ResponsibleBuyer] [bit] NOT NULL,
	[FactoryICRDepartment] [nvarchar](40) NULL,
	[FactoryICRNo] [nvarchar](13) NULL,
	[FactoryICRRemark] [nvarchar](max) NULL,
	[SubconDBCNo] [nvarchar](13) NULL,
	[SubconDBCRemark] [nvarchar](max) NULL,
	[SubConName] [nvarchar](20) NULL,
	[SCIICRDepartment] [nvarchar](40) NULL,
	[SCIICRNo] [nvarchar](13) NULL,
	[SCIICRRemark] [nvarchar](max) NULL,
	[SuppDBCNo] [nvarchar](13) NULL,
	[SuppDBCRemark] [nvarchar](max) NULL,
	[BuyerDBCDepartment] [nvarchar](40) NULL,
	[BuyerDBCNo] [nvarchar](13) NULL,
	[BuyerDBCRemark] [nvarchar](max) NULL,
	[BuyerICRNo] [nvarchar](13) NULL,
	[BuyerICRRemark] [nvarchar](max) NULL,
	[MRComment] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
	[BuyerRemark] [nvarchar](max) NULL,
	[FTYComments] [nvarchar](max) NULL,
 [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [TPEEditName] VARCHAR(10) NULL DEFAULT (''), 
    [TPEEditDate] DATETIME NULL, 
    CONSTRAINT [PK_OrderChangeApplication] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_NeedProduction]  DEFAULT ((0)) FOR [NeedProduction]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_OldQty]  DEFAULT ((0)) FOR [OldQty]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_RatioFty]  DEFAULT ((0)) FOR [RatioFty]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_RatioSubcon]  DEFAULT ((0)) FOR [RatioSubcon]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_RatioSCI]  DEFAULT ((0)) FOR [RatioSCI]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_RatioSupp]  DEFAULT ((0)) FOR [RatioSupp]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_RatioBuyer]  DEFAULT ((0)) FOR [RatioBuyer]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_ResponsibleFty]  DEFAULT ((0)) FOR [ResponsibleFty]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_ResponsibleSubcon]  DEFAULT ((0)) FOR [ResponsibleSubcon]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_ResponsibleSCI]  DEFAULT ((0)) FOR [ResponsibleSCI]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_ResponsibleSupp]  DEFAULT ((0)) FOR [ResponsibleSupp]
GO

ALTER TABLE [dbo].[OrderChangeApplication] ADD  CONSTRAINT [DF_OrderChangeApplication_ResponsibleBuyer]  DEFAULT ((0)) FOR [ResponsibleBuyer]
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北最後編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北最後編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditDate'