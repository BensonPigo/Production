CREATE TABLE [dbo].[CFAInspectionRecord](
	[ID] [varchar](13) NOT NULL,
	[AuditDate] [date] NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[MDivisionid] [varchar](8) NOT NULL,
	[SewingLineID] [varchar](100) NOT NULL,
	[Team] [varchar](5) NOT NULL,
	[Shift] [varchar](1) NOT NULL,
	[Stage] [varchar](15) NOT NULL,
	[InspectQty] [numeric](7, 0) NOT NULL,
	[DefectQty] [numeric](7, 0) NOT NULL,
	[ClogReceivedPercentage] [numeric](3, 0) NOT NULL,
	[Result] [varchar](16) NOT NULL,
	[CFA] [varchar](10) NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[Remark] [varchar](max) NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[IsCombinePO] [bit] NOT NULL,
	[FirstInspection] [bit] NOT NULL,
	[IsImportFromMES] [bit] NOT NULL,
 [InspectionFailCount] INT NOT NULL DEFAULT ((0)), 
    [ReInspection] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_CFAInspectionRecord] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [MDivisionid]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [Stage]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ((0)) FOR [InspectQty]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ((0)) FOR [DefectQty]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ((0)) FOR [ClogReceivedPercentage]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [Result]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [CFA]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  CONSTRAINT [DF_CFAInspectionRecord_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  CONSTRAINT [DF_CFAInspectionRecord_IsCombinePO]  DEFAULT ((0)) FOR [IsCombinePO]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  CONSTRAINT [DF_CFAInspectionRecord_FirstInspection]  DEFAULT ((0)) FOR [FirstInspection]
GO

ALTER TABLE [dbo].[CFAInspectionRecord] ADD  DEFAULT ((0)) FOR [IsImportFromMES]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該單據所有的紙箱是否是第一次檢驗 Stagger' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CFAInspectionRecord', @level2type=N'COLUMN',@level2name=N'FirstInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'IsImportFromMES', @value=N'是否為從MES轉入的資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CFAInspectionRecord', @level2type=N'COLUMN',@level2name=N'IsImportFromMES'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0: 一般驗貨/ 1: 重新驗貨',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CFAInspectionRecord',
    @level2type = N'COLUMN',
    @level2name = N'ReInspection'