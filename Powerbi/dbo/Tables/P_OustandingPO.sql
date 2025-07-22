CREATE TABLE [dbo].[P_OustandingPO] (
    [FactoryID]                       VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_FactoryID_New] DEFAULT ('') NOT NULL,
    [OrderID]                         VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_OrderID_New] DEFAULT ('') NOT NULL,
    [CustPONo]                        VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_CustPONo_New] DEFAULT ('') NOT NULL,
    [StyleID]                         VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_StyleID_New] DEFAULT ('') NOT NULL,
    [BrandID]                         VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_BrandID_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]                   DATE           NULL,
    [Seq]                             VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_Seq_New] DEFAULT ('') NOT NULL,
    [ShipModeID]                      VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_ShipModeID_New] DEFAULT ('') NOT NULL,
    [Category]                        VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_Category_New] DEFAULT ('') NOT NULL,
    [PartialShipment]                 VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_PartialShipment_New] DEFAULT ('') NOT NULL,
    [Junk]                            VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_Junk_New] DEFAULT ('') NOT NULL,
    [OrderQty]                        INT            CONSTRAINT [DF_P_OustandingPO_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [PackingCtn]                      INT            CONSTRAINT [DF_P_OustandingPO_PackingCtn_New] DEFAULT ((0)) NOT NULL,
    [PackingQty]                      INT            CONSTRAINT [DF_P_OustandingPO_PackingQty_New] DEFAULT ((0)) NOT NULL,
    [ClogRcvCtn]                      INT            CONSTRAINT [DF_P_OustandingPO_ClogRcvCtn_New] DEFAULT ((0)) NOT NULL,
    [ClogRcvQty]                      VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_ClogRcvQty_New] DEFAULT ((0)) NOT NULL,
    [LastCMPOutputDate]               DATE           NULL,
    [CMPQty]                          VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_CMPQty_New] DEFAULT ((0)) NOT NULL,
    [LastDQSOutputDate]               DATE           NULL,
    [DQSQty]                          VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_DQSQty_New] DEFAULT ('') NOT NULL,
    [OSTPackingQty]                   VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_OSTPackingQty_New] DEFAULT ('') NOT NULL,
    [OSTCMPQty]                       VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_OSTCMPQty_New] DEFAULT ('') NOT NULL,
    [OSTDQSQty]                       VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_OSTDQSQty_New] DEFAULT ('') NOT NULL,
    [OSTClogQty]                      VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_OSTClogQty_New] DEFAULT ('') NOT NULL,
    [OSTClogCtn]                      INT            CONSTRAINT [DF_P_OustandingPO_OSTClogCtn_New] DEFAULT ((0)) NOT NULL,
    [PulloutComplete]                 VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_PulloutComplete_New] DEFAULT ('') NOT NULL,
    [Dest]                            VARCHAR (8000) NULL,
    [KPIGroup]                        VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_KPIGroup_New] DEFAULT ('') NULL,
    [CancelledButStillNeedProduction] VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_CancelledButStillNeedProduction_New] DEFAULT ('') NULL,
    [CFAInspectionResult]             VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_CFAInspectionResult_New] DEFAULT ('') NULL,
    [3rdPartyInspection]              VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_3rdPartyInspection_New] DEFAULT ('') NULL,
    [3rdPartyInspectionResult]        VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_3rdPartyInspectionResult_New] DEFAULT ('') NULL,
    [BookingSP]                       VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_BookingSP_New] DEFAULT ('') NOT NULL,
    [LastCartonReceivedDate]          DATETIME       NULL,
    [BIFactoryID]                     VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                    DATETIME       NULL,
    [BIStatus]                        VARCHAR (8000) CONSTRAINT [DF_P_OustandingPO_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_OustandingPO] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [OrderID] ASC, [Seq] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Booking SP / Is GMT Master' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'BookingSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'LastCartonReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_OustandingPO', @level2type = N'COLUMN', @level2name = N'BIStatus';

