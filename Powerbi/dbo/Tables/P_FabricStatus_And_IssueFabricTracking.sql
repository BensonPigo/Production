CREATE TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking](
	[SewingCell] [varchar](2) NOT NULL,
	[LineID] [varchar](5) NOT NULL,
	[Department] [varchar](15) not null DEFAULT (''),
	[ReplacementID] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[Seq] [varchar](6) NOT NULL,
	[FabricType] [varchar](1) NOT NULL,
	[Color] [nvarchar](150) NOT NULL,
	[RefNo] [varchar](36) NOT NULL,
	[ApvDate] [datetime] NULL,
	[NoOfPcsRejected] [int] NOT NULL,
	[RequestQtyYrds] [numeric](10, 2) NOT NULL,
	[IssueQtyYrds] [numeric](10, 2) NOT NULL,
	[ReplacementFinishedDate] [datetime] NULL,
	[Type] [varchar](11) NOT NULL,
	[Process] [varchar](30) NOT NULL,
	[Description] [nvarchar](60) NOT NULL,
	[OnTime] [varchar](1) NOT NULL,
	[Remark] [nvarchar](60) NOT NULL,
    [DetailRemark] NVARCHAR(60) NOT NULL DEFAULT (''), 
    [StyleName] NVARCHAR(50)      CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_StyleName]  NOT NULL DEFAULT (''), 
    [MaterialType] NVARCHAR(30)   CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_MaterialType]  NOT NULL DEFAULT (''), 
    [SewingQty] INT               CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SewingQty]  NOT NULL DEFAULT ((0)), 
    [FactotyID] VARCHAR(10) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_P_FabricStatus_And_IssueFabricTracking] PRIMARY KEY CLUSTERED 
(
	[ReplacementID] ASC,
	[SP] ASC,
	[Seq] ASC,
	[RefNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SewingCell]  DEFAULT ('') FOR [SewingCell]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_LineID]  DEFAULT ('') FOR [LineID]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_ReplacementID]  DEFAULT ('') FOR [ReplacementID]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_RefNo]  DEFAULT ('') FOR [RefNo]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_NoOfPcsRejected]  DEFAULT ((0)) FOR [NoOfPcsRejected]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_RequestQtyYrds]  DEFAULT ((0)) FOR [RequestQtyYrds]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_IssueQtyYrds]  DEFAULT ((0)) FOR [IssueQtyYrds]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Process]  DEFAULT ('') FOR [Process]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_OnTime]  DEFAULT ('') FOR [OnTime]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Remark]  DEFAULT ('') FOR [Remark]
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'補料報告表身備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricStatus_And_IssueFabricTracking',
    @level2type = N'COLUMN',
    @level2name = N'DetailRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'款式名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricStatus_And_IssueFabricTracking',
    @level2type = N'COLUMN',
    @level2name = N'StyleName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料類別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricStatus_And_IssueFabricTracking',
    @level2type = N'COLUMN',
    @level2name = N'MaterialType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sewing Output Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricStatus_And_IssueFabricTracking',
    @level2type = N'COLUMN',
    @level2name = N'SewingQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricStatus_And_IssueFabricTracking',
    @level2type = N'COLUMN',
    @level2name = N'FactotyID'