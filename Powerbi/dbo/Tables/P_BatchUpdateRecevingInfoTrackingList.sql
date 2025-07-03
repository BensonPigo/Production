CREATE TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList](
	[ReceivingID] [varchar](13) NOT NULL,
	[ExportID] [varchar](13) NOT NULL,
	[FtyGroup] [varchar](3) NOT NULL,
	[Packages] [decimal](5, 0) NOT NULL,
	[ArriveDate] [date] NULL,
	[Poid] [varchar](13) NOT NULL,
	[Seq] [varchar](6) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[refno] [varchar](36) NOT NULL,
	[WeaveTypeID] [varchar](20) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[StockQty] [numeric](11, 2) NOT NULL,
	[StockType] [varchar](1) NOT NULL,
	[Location] [varchar](500) NOT NULL,
	[Weight] [numeric](7, 2) NOT NULL,
	[ActualWeight] [numeric](7, 2) NOT NULL,
	[CutShadebandTime] [datetime] NULL,
	[CutBy] [varchar](10) NOT NULL,
	[Fabric2LabTime] [datetime] NULL,
	[Fabric2LabBy] [varchar](10) NOT NULL,
	[Checker] [nvarchar](30) NOT NULL,
	[IsQRCodeCreatedByPMS] [varchar](1) NOT NULL,
	[LastP26RemarkData] [nvarchar](60) NOT NULL,
	[MINDChecker] [varchar](10) NOT NULL,
	[QRCode_PrintDate] [datetime] NULL,
	[MINDCheckAddDate] [datetime] NULL,
	[MINDCheckEditDate] [datetime] NULL,
	[SuppAbbEN] [varchar](12) NOT NULL,
	[ForInspection] [varchar](1) NOT NULL,
	[ForInspectionTime] [datetime] NULL,
	[OneYardForWashing] [varchar](1) NOT NULL,
	[Hold] [varchar](1) NOT NULL,
	[Remark] [nvarchar](100) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[StyleID] [varchar](15) NOT NULL,
	[ColorName] [varchar](150) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_BatchUpdateRecevingInfoTrackingList] PRIMARY KEY CLUSTERED 
(
	[ReceivingID] ASC,
	[Poid] ASC,
	[Seq] ASC,
	[Roll] ASC,
	[Dyelot] ASC,
	[StockType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ReceivingID]  DEFAULT ('') FOR [ReceivingID]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ExportID]  DEFAULT ('') FOR [ExportID]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_FtyGroup]  DEFAULT ('') FOR [FtyGroup]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Packages]  DEFAULT ((0)) FOR [Packages]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Poid]  DEFAULT ('') FOR [Poid]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_refno]  DEFAULT ('') FOR [refno]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_WeaveTypeID]  DEFAULT ('') FOR [WeaveTypeID]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Roll]  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Dyelot]  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StockQty]  DEFAULT ((0)) FOR [StockQty]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StockType]  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Location]  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Weight]  DEFAULT ((0)) FOR [Weight]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ActualWeight]  DEFAULT ((0)) FOR [ActualWeight]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_CutBy]  DEFAULT ('') FOR [CutBy]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Fabric2LabBy]  DEFAULT ('') FOR [Fabric2LabBy]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Checker]  DEFAULT ('') FOR [Checker]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_IsQRCodeCreatedByPMS]  DEFAULT ('') FOR [IsQRCodeCreatedByPMS]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_LastP26RemarkData]  DEFAULT ('') FOR [LastP26RemarkData]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_MINDChecker]  DEFAULT ('') FOR [MINDChecker]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_SuppAbbEN]  DEFAULT ('') FOR [SuppAbbEN]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ForInspection]  DEFAULT ('') FOR [ForInspection]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_OneYardForWashing]  DEFAULT ('') FOR [OneYardForWashing]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Hold]  DEFAULT ('') FOR [Hold]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] ADD  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'ExportID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'FtyGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總件/箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'ArriveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Poid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'StockQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Location'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Weight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'ActualWeight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪檢驗色差布料的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'CutShadebandTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪檢驗色差布料的人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'CutBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫剪布給實驗室的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Fabric2LabTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新倉庫剪布給實驗室時間的人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Fabric2LabBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PH 收料時負責秤重 + 剪一小塊布 (ShadeBand) + 搬該物料入庫' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Checker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QRCode是否建立於PMS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'IsQRCodeCreatedByPMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LocationTrans備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'LastP26RemarkData'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND收料人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'MINDChecker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QRCode首次列印的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'QRCode_PrintDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND第一次收料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'MINDCheckAddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND收料修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'MINDCheckEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商英文全名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'SuppAbbEN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已經檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'ForInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送檢驗的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'ForInspectionTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MIND [1 Yard for Washing] 系統自動建立的發料單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'OneYardForWashing'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當倉庫收料時，若發現缺少重量或數量則會先將布捲放在特定的位置 Hold Rack Location' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Hold'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO