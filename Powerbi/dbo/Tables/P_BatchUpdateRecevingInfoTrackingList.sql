CREATE TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList](
		[ReceivingID] [varchar] (13) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ReceivingID] DEFAULT '',
		[ExportID] [varchar] (13) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ExportID]  DEFAULT '',
		[FtyGroup] [varchar] (3) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_FtyGroup]  DEFAULT '',
		[Packages] [decimal] (5, 0) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Packages]  DEFAULT 0,
		[ArriveDate] [date] NULL,
		[Poid] [varchar] (13) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Poid]  DEFAULT '',
		[Seq] [varchar] (6) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Seq]  DEFAULT '',
		[BrandID] [varchar] (8) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_BrandID]  DEFAULT '',
		[refno] [varchar] (36) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_refno]  DEFAULT '',
		[WeaveTypeID] [varchar] (20) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_WeaveTypeID]  DEFAULT '',
		[Color] [varchar] (50) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Color]  DEFAULT '',
		[Roll] [varchar] (8) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Roll]  DEFAULT '',
		[Dyelot] [varchar] (8) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Dyelot]  DEFAULT '',
		[StockQty] [numeric] (11, 2) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StockQty]  DEFAULT 0,
		[StockType] [varchar] (1) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StockType]  DEFAULT '',
		[Location] [varchar] (500) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Location]  DEFAULT '',
		[Weight] [numeric] (7, 2) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Weight]  DEFAULT 0,
		[ActualWeight] [numeric] (7, 2) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ActualWeight]  DEFAULT 0,
		[CutShadebandTime] [datetime] NULL,
		[CutBy] [varchar] (10) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_CutBy]  DEFAULT '',
		[Fabric2LabTime] [datetime] NULL,
		[Fabric2LabBy] [varchar] (10) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Fabric2LabBy]  DEFAULT '',
		[Checker] [nvarchar] (30) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Checker]  DEFAULT '',
		[IsQRCodeCreatedByPMS] [varchar] (1) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_IsQRCodeCreatedByPMS]  DEFAULT '',
		[LastP26RemarkData] [nvarchar] (60) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_LastP26RemarkData]  DEFAULT '',
		[MINDChecker] [varchar] (10) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_MINDChecker]  DEFAULT '',
		[QRCode_PrintDate] [datetime] NULL,
		[MINDCheckAddDate] [datetime] NULL,
		[MINDCheckEditDate] [datetime] NULL,
		[SuppAbbEN] [varchar] (12) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_SuppAbbEN]  DEFAULT '',
		[ForInspection] [varchar] (1) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ForInspection]  DEFAULT '',
		[ForInspectionTime] [datetime] NULL,
		[OneYardForWashing] [varchar] (1) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_OneYardForWashing]  DEFAULT '',
		[Hold] [varchar] (1) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Hold]  DEFAULT '',
		[Remark] [nvarchar] (100) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Remark]  DEFAULT '',
		[AddDate] [datetime] NULL,
		[EditDate] [datetime] NULL,
	 [StyleID] VARCHAR(15) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StyleID]  DEFAULT '', 
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
	Go
