CREATE TABLE [dbo].[P_ScanPackList] (
    [Ukey]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [FactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_FactoryID_New] DEFAULT ('') NOT NULL,
    [PackingID]      VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_PackingID_New] DEFAULT ('') NOT NULL,
    [OrderID]        VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_OrderID_New] DEFAULT ('') NOT NULL,
    [CTNStartNo]     VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_CTNStartNo_New] DEFAULT ('') NOT NULL,
    [ShipModeID]     VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_ShipModeID_New] DEFAULT ('') NOT NULL,
    [StyleID]        VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_StyleID_New] DEFAULT ('') NOT NULL,
    [BrandID]        VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_BrandID_New] DEFAULT ('') NOT NULL,
    [SeasonID]       VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_SeasonID_New] DEFAULT ('') NOT NULL,
    [SewLine]        VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_SewLine_New] DEFAULT ('') NOT NULL,
    [Customize1]     VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_Customize1_New] DEFAULT ('') NOT NULL,
    [CustPONo]       VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_CustPONo_New] DEFAULT ('') NOT NULL,
    [BuyerID]        VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_BuyerID_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]  DATE            NULL,
    [Destination]    VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_Destination_New] DEFAULT ('') NOT NULL,
    [Colorway]       VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_Colorway_New] DEFAULT ('') NOT NULL,
    [Color]          VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_Color_New] DEFAULT ('') NOT NULL,
    [Size]           VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_Size_New] DEFAULT ('') NOT NULL,
    [CTNBarcode]     VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_CTNBarcode_New] DEFAULT ('') NOT NULL,
    [QtyPerCTN]      VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_QtyPerCTN_New] DEFAULT ('') NOT NULL,
    [ShipQty]        INT             CONSTRAINT [DF_P_ScanPackList_ShipQty_New] DEFAULT ((0)) NOT NULL,
    [QtyPerCTNScan]  VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_QtyPerCTNScan_New] DEFAULT ('') NOT NULL,
    [PackingError]   NVARCHAR (1000) CONSTRAINT [DF_P_ScanPackList_PackingError_New] DEFAULT ('') NOT NULL,
    [ErrQty]         SMALLINT        CONSTRAINT [DF_P_ScanPackList_ErrQty_New] DEFAULT ((0)) NOT NULL,
    [AuditQCName]    VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_AuditQCName_New] DEFAULT ('') NOT NULL,
    [ActCTNWeight]   NUMERIC (38, 3) NOT NULL,
    [HangtagBarcode] VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_HangtagBarcode_New] DEFAULT ('') NOT NULL,
    [ScanDate]       DATETIME        NULL,
    [ScanName]       VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_ScanName_New] DEFAULT ('') NOT NULL,
    [CartonStatus]   VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_CartonStatus_New] DEFAULT ('') NOT NULL,
    [Lacking]        VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_Lacking_New] DEFAULT ('') NOT NULL,
    [LackingQty]     INT             CONSTRAINT [DF_P_ScanPackList_LackingQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]   DATETIME        NULL,
    [BIStatus]       VARCHAR (8000)  CONSTRAINT [DF_P_ScanPackList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ScanPackList] PRIMARY KEY CLUSTERED ([Ukey] ASC, [FactoryID] ASC)
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ScanPackList', @level2type = N'COLUMN', @level2name = N'BIStatus';

