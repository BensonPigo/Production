CREATE TABLE [dbo].[P_SubProInsReport](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SubProLocationID] [varchar](20) NOT NULL,
	[InspectionDate] [date] NULL,
	[SewInLine] [date] NULL,
	[SewinglineID] [varchar](5) NOT NULL,
	[Shift] [varchar](5) NOT NULL,
	[RFT] [numeric](5, 2) NOT NULL,
	[SubProcessID] [varchar](15) NOT NULL,
	[BundleNo] [varchar](10) NOT NULL,
	[Artwork] [varchar](30) NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Alias] [varchar](30) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[BundleGroup] [numeric](5, 0) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[ColorID] [varchar](6) NOT NULL,
	[SizeCode] [varchar](8) NOT NULL,
	[PatternDesc] [nvarchar](100) NOT NULL,
	[Item] [varchar](20) NOT NULL,
	[Qty] [numeric](5, 0) NOT NULL,
	[RejectQty] [int] NOT NULL,
	[Machine] [varchar](50) NOT NULL,
	[Serial] [varchar](50) NOT NULL,
	[Junk] [bit] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[DefectCode] [varchar](50) NOT NULL,
	[DefectQty] [int] NOT NULL,
	[Inspector] [varchar](50) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[AddDate] [datetime] NULL,
	[RepairedDatetime] [datetime] NULL,
	[RepairedTime] [int] NOT NULL,
	[ResolveTime] [int] NOT NULL,
	[SubProResponseTeamID] [varchar](1000) NOT NULL,
	[CustomColumn1] [varchar](300) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[OperatorID] [nvarchar](500) NOT NULL,
	[OperatorName] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_P_SubProInsReport] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_SubProLocationID]  DEFAULT ('') FOR [SubProLocationID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_SewinglineID]  DEFAULT ('') FOR [SewinglineID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_RFT]  DEFAULT ((0)) FOR [RFT]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_SubProcessID]  DEFAULT ('') FOR [SubProcessID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_BundleNo]  DEFAULT ('') FOR [BundleNo]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Artwork]  DEFAULT ('') FOR [Artwork]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Alias]  DEFAULT ('') FOR [Alias]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_BundleGroup]  DEFAULT ((0)) FOR [BundleGroup]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_ColorID]  DEFAULT ('') FOR [ColorID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_PatternDesc]  DEFAULT ('') FOR [PatternDesc]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Item]  DEFAULT ('') FOR [Item]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_RejectQty]  DEFAULT ((0)) FOR [RejectQty]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Machine]  DEFAULT ('') FOR [Machine]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Serial]  DEFAULT ('') FOR [Serial]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_DefectCode]  DEFAULT ('') FOR [DefectCode]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_DefectQty]  DEFAULT ((0)) FOR [DefectQty]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_Inspector]  DEFAULT ('') FOR [Inspector]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [DF_P_SubProInsReport_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_RepairedTime]  DEFAULT ((0)) FOR [RepairedTime]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_ResolveTime]  DEFAULT ((0)) FOR [ResolveTime]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_SubProResponseTeamID]  DEFAULT ('') FOR [SubProResponseTeamID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [PK_P_SubProInsReport_CustomColumn1]  DEFAULT ('') FOR [CustomColumn1]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [DF_P_SubProInsReport_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [DF_P_SubProInsReport_OperatorID]  DEFAULT ('') FOR [OperatorID]
GO

ALTER TABLE [dbo].[P_SubProInsReport] ADD  CONSTRAINT [DF_P_SubProInsReport_OperatorName]  DEFAULT ('') FOR [OperatorName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段廠房位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'SubProLocationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'SewInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第一次檢驗就通過的成功率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'RFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'SubProcessID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工藝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Artwork'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Alias'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'PatternDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'衣服項目，T-shirt、Shorts…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Item'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'綁包數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不通過數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'RejectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段機器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Machine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段機器狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段機器說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'DefectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'Inspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修復完成日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'RepairedDatetime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗到綁包完成修復總時長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'RepairedTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總修復時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'ResolveTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段負責單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'SubProResponseTeamID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'CustomColumn1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operator Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'OperatorName'
GO