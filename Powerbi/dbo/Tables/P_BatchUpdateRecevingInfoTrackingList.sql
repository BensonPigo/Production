CREATE TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList] (
    [ReceivingID]          VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ReceivingID_New] DEFAULT ('') NOT NULL,
    [ExportID]             VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ExportID_New] DEFAULT ('') NOT NULL,
    [FtyGroup]             VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_FtyGroup_New] DEFAULT ('') NOT NULL,
    [Packages]             DECIMAL (18)    CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Packages_New] DEFAULT ((0)) NOT NULL,
    [ArriveDate]           DATE            NULL,
    [Poid]                 VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Poid_New] DEFAULT ('') NOT NULL,
    [Seq]                  VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Seq_New] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_BrandID_New] DEFAULT ('') NOT NULL,
    [refno]                VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_refno_New] DEFAULT ('') NOT NULL,
    [WeaveTypeID]          VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_WeaveTypeID_New] DEFAULT ('') NOT NULL,
    [Color]                VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Color_New] DEFAULT ('') NOT NULL,
    [Roll]                 VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Roll_New] DEFAULT ('') NOT NULL,
    [Dyelot]               VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Dyelot_New] DEFAULT ('') NOT NULL,
    [StockQty]             NUMERIC (38, 2) CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StockQty_New] DEFAULT ((0)) NOT NULL,
    [StockType]            VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StockType_New] DEFAULT ('') NOT NULL,
    [Location]             VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Location_New] DEFAULT ('') NOT NULL,
    [Weight]               NUMERIC (38, 2) CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Weight_New] DEFAULT ((0)) NOT NULL,
    [ActualWeight]         NUMERIC (38, 2) CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ActualWeight_New] DEFAULT ((0)) NOT NULL,
    [CutShadebandTime]     DATETIME        NULL,
    [CutBy]                VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_CutBy_New] DEFAULT ('') NOT NULL,
    [Fabric2LabTime]       DATETIME        NULL,
    [Fabric2LabBy]         VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Fabric2LabBy_New] DEFAULT ('') NOT NULL,
    [Checker]              NVARCHAR (1000) CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Checker_New] DEFAULT ('') NOT NULL,
    [IsQRCodeCreatedByPMS] VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_IsQRCodeCreatedByPMS_New] DEFAULT ('') NOT NULL,
    [LastP26RemarkData]    NVARCHAR (1000) CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_LastP26RemarkData_New] DEFAULT ('') NOT NULL,
    [MINDChecker]          VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_MINDChecker_New] DEFAULT ('') NOT NULL,
    [QRCode_PrintDate]     DATETIME        NULL,
    [MINDCheckAddDate]     DATETIME        NULL,
    [MINDCheckEditDate]    DATETIME        NULL,
    [SuppAbbEN]            VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_SuppAbbEN_New] DEFAULT ('') NOT NULL,
    [ForInspection]        VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_ForInspection_New] DEFAULT ('') NOT NULL,
    [ForInspectionTime]    DATETIME        NULL,
    [OneYardForWashing]    VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_OneYardForWashing_New] DEFAULT ('') NOT NULL,
    [Hold]                 VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Hold_New] DEFAULT ('') NOT NULL,
    [Remark]               NVARCHAR (1000) CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_Remark_New] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME        NULL,
    [EditDate]             DATETIME        NULL,
    [StyleID]              VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_StyleID_New] DEFAULT ('') NOT NULL,
    [ColorName]            VARCHAR (8000)  CONSTRAINT [DF__P_BatchUp__Color__750F12C6_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_BatchUpdateRecevingInfoTrackingList] PRIMARY KEY CLUSTERED ([ReceivingID] ASC, [Poid] ASC, [Seq] ASC, [Roll] ASC, [Dyelot] ASC, [StockType] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_BatchUpdateRecevingInfoTrackingList', @level2type = N'COLUMN', @level2name = N'BIStatus';

