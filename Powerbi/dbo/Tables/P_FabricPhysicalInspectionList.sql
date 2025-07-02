CREATE TABLE [dbo].[P_FabricPhysicalInspectionList](
	[FactoryID] [varchar](8) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[SEQ] [varchar](6) NOT NULL,
	[WKNo] [varchar](13) NOT NULL,
	[Invoice] [varchar](25) NOT NULL,
	[ReceivingID] [varchar](13) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[SupplierName] [varchar](6) NOT NULL,
	[Refno] [varchar](36) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[CuttingDate] [date] NULL,
	[ArriveWHDate] [date] NULL,
	[ArriveQty] [numeric](10, 2) NOT NULL,
	[WeaveType] [varchar](20) NOT NULL,
	[TotalRoll] [int] NOT NULL,
	[TotalDyeLot] [int] NOT NULL,
	[AlreadyInspectedDyelot] [int] NOT NULL,
	[NotInspectedDyelot] [int] NOT NULL,
	[NonInspection] [varchar](1) NOT NULL,
	[PhysicalInspection] [varchar](10) NOT NULL,
	[PhysicalInspector] [varchar](100) NOT NULL,
	[Approver] [varchar](10) NOT NULL,
	[ApproveDate] [datetime] NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[TicketYds] [numeric](8, 2) NOT NULL,
	[ActYdsInsdpected] [numeric](8, 2) NOT NULL,
	[LthOfDiff] [numeric](8, 2) NOT NULL,
	[TransactionID] [varchar](30) NOT NULL,
	[CutWidth] [numeric](5, 2) NOT NULL,
	[FullWidth] [numeric](5, 2) NOT NULL,
	[ActualWidth] [numeric](5, 2) NOT NULL,
	[TotalPoints] [numeric](6, 0) NOT NULL,
	[PointRate] [numeric](6, 2) NOT NULL,
	[Result] [varchar](5) NOT NULL,
	[Grade] [varchar](10) NOT NULL,
	[Moisture] [varchar](1) NOT NULL,
	[Remark] [nvarchar](60) NOT NULL,
	[InspDate] [date] NULL,
	[Inspector] [nvarchar](50) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_FabricPhysicalInspectionList] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[SP] ASC,
	[SEQ] ASC,
	[ReceivingID] ASC,
	[Roll] ASC,
	[Dyelot] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_SEQ]  DEFAULT ('') FOR [SEQ]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_WKNo]  DEFAULT ('') FOR [WKNo]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Invoice]  DEFAULT ('') FOR [Invoice]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_ReceivingID]  DEFAULT ('') FOR [ReceivingID]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_SupplierName]  DEFAULT ('') FOR [SupplierName]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_ArriveQty]  DEFAULT ((0)) FOR [ArriveQty]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_WeaveType]  DEFAULT ('') FOR [WeaveType]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_TotalRoll]  DEFAULT ((0)) FOR [TotalRoll]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_TotalDyeLot]  DEFAULT ((0)) FOR [TotalDyeLot]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_AlreadyInspectedDyelot]  DEFAULT ((0)) FOR [AlreadyInspectedDyelot]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_NotInspectedDyelot]  DEFAULT ((0)) FOR [NotInspectedDyelot]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_NonInspection]  DEFAULT ('') FOR [NonInspection]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_PhysicalInspection]  DEFAULT ('') FOR [PhysicalInspection]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_PhysicalInspector]  DEFAULT ('') FOR [PhysicalInspector]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Approver]  DEFAULT ('') FOR [Approver]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Roll]  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Dyelot]  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_TicketYds]  DEFAULT ((0)) FOR [TicketYds]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_ActYdsInsdpected]  DEFAULT ((0)) FOR [ActYdsInsdpected]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_LthOfDiff]  DEFAULT ((0)) FOR [LthOfDiff]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_TransactionID]  DEFAULT ('') FOR [TransactionID]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_CutWidth]  DEFAULT ((0)) FOR [CutWidth]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_FullWidth]  DEFAULT ((0)) FOR [FullWidth]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_ActualWidth]  DEFAULT ((0)) FOR [ActualWidth]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_TotalPoints]  DEFAULT ((0)) FOR [TotalPoints]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_PointRate]  DEFAULT ((0)) FOR [PointRate]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Result]  DEFAULT ('') FOR [Result]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Grade]  DEFAULT ('') FOR [Grade]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Moisture]  DEFAULT ('') FOR [Moisture]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_Inspector]  DEFAULT ('') FOR [Inspector]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_OrderType]  DEFAULT ('') FOR [OrderType]
GO

ALTER TABLE [dbo].[P_FabricPhysicalInspectionList] ADD  CONSTRAINT [DF_P_FabricPhysicalInspectionList_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'WKNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發票號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Invoice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收貨單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'SupplierName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁切日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'CuttingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達倉庫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'WeaveType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料捲數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TotalRoll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TotalDyeLot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已驗缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'AlreadyInspectedDyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未驗缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'NotInspectedDyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需實體驗布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'NonInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Over All 實體驗布結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'PhysicalInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Over All 實體驗布人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'PhysicalInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Over All 實體驗布日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ApproveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TicketYds'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際驗布長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ActYdsInsdpected'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長度差異' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'LthOfDiff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易系統ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TransactionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'撐布寬度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'CutWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實寬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'FullWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際撐布寬度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'ActualWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'疵點總分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'TotalPoints'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'疵點分率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'PointRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布種等級' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Grade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布種含水度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Moisture'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'InspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布員工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'Inspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricPhysicalInspectionList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO