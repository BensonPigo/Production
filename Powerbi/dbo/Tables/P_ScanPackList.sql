CREATE TABLE [dbo].[P_ScanPackList](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[PackingID] [varchar](13) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[CTNStartNo] [varchar](6) NOT NULL,
	[ShipModeID] [varchar](10) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[SewLine] [varchar](60) NOT NULL,
	[Customize1] [varchar](30) NOT NULL,
	[CustPONo] [varchar](30) NOT NULL,
	[BuyerID] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[Destination] [varchar](50) NOT NULL,
	[Colorway] [varchar](500) NOT NULL,
	[Color] [varchar](500) NOT NULL,
	[Size] [varchar](500) NOT NULL,
	[CTNBarcode] [varchar](20) NOT NULL,
	[QtyPerCTN] [varchar](500) NOT NULL,
	[ShipQty] [int] NOT NULL,
	[QtyPerCTNScan] [varchar](500) NOT NULL,
	[PackingError] [nvarchar](71) NOT NULL,
	[ErrQty] [smallint] NOT NULL,
	[AuditQCName] [varchar](30) NOT NULL,
	[ActCTNWeight] [numeric](7, 3) NOT NULL,
	[HangtagBarcode] [varchar](500) NOT NULL,
	[ScanDate] [datetime] NULL,
	[ScanName] [varchar](100) NOT NULL,
	[CartonStatus] [varchar](12) NOT NULL,
	[Lacking] [varchar](1) NOT NULL,
	[LackingQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ScanPackList] PRIMARY KEY CLUSTERED 
(
	[Ukey] DESC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_PackingID]  DEFAULT ('') FOR [PackingID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_CTNStartNo]  DEFAULT ('') FOR [CTNStartNo]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_ShipModeID]  DEFAULT ('') FOR [ShipModeID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_SewLine]  DEFAULT ('') FOR [SewLine]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_Customize1]  DEFAULT ('') FOR [Customize1]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_CustPONo]  DEFAULT ('') FOR [CustPONo]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_BuyerID]  DEFAULT ('') FOR [BuyerID]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_Destination]  DEFAULT ('') FOR [Destination]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_Colorway]  DEFAULT ('') FOR [Colorway]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_Size]  DEFAULT ('') FOR [Size]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_CTNBarcode]  DEFAULT ('') FOR [CTNBarcode]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_QtyPerCTN]  DEFAULT ('') FOR [QtyPerCTN]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_ShipQty]  DEFAULT ((0)) FOR [ShipQty]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_QtyPerCTNScan]  DEFAULT ('') FOR [QtyPerCTNScan]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_PackingError]  DEFAULT ('') FOR [PackingError]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_ErrQty]  DEFAULT ((0)) FOR [ErrQty]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_AuditQCName]  DEFAULT ('') FOR [AuditQCName]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_HangtagBarcode]  DEFAULT ('') FOR [HangtagBarcode]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_ScanName]  DEFAULT ('') FOR [ScanName]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_CartonStatus]  DEFAULT ('') FOR [CartonStatus]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_Lacking]  DEFAULT ('') FOR [Lacking]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_LackingQty]  DEFAULT ((0)) FOR [LackingQty]
GO

ALTER TABLE [dbo].[P_ScanPackList] ADD  CONSTRAINT [DF_P_ScanPackList_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PackingListID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'PackingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起始箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'CTNStartNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'ShipModeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'SewLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'Customize1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'CustPONo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'BuyerID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目的地' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'Destination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'Colorway'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'Size'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'箱子條碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'CTNBarcode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每箱數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'QtyPerCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'掃瞄件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'QtyPerCTNScan'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PackingErrorReason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'PackingError'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing Error數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'ErrQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢查人員姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'AuditQCName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際箱子總重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'ActCTNWeight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'條碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'HangtagBarcode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'掃描最後修改日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'ScanDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後掃描人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'ScanName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'箱子狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'CartonStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否缺件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'Lacking'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缺件數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'LackingQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ScanPackList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO