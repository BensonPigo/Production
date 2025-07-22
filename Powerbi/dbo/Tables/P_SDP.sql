CREATE TABLE [dbo].[P_SDP] (
    [Country]                  VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Country_New] DEFAULT ('') NOT NULL,
    [KPIGroup]                 VARCHAR (8000)  CONSTRAINT [DF_P_SDP_KPIGroup_New] DEFAULT ('') NOT NULL,
    [FactoryID]                VARCHAR (8000)  CONSTRAINT [DF_P_SDP_FactoryID_New] DEFAULT ('') NOT NULL,
    [SPNo]                     VARCHAR (8000)  CONSTRAINT [DF_P_SDP_SPNo_New] DEFAULT ('') NOT NULL,
    [Style]                    VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Style_New] DEFAULT ('') NOT NULL,
    [Seq]                      VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Seq_New] DEFAULT ('') NOT NULL,
    [Brand]                    VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Brand_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]            DATE            NULL,
    [FactoryKPI]               DATE            NULL,
    [Extension]                DATE            NULL,
    [DeliveryByShipmode]       VARCHAR (8000)  CONSTRAINT [DF_P_SDP_DeliveryByShipmode_New] DEFAULT ('') NOT NULL,
    [OrderQty]                 INT             CONSTRAINT [DF_P_SDP_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [OnTimeQty]                INT             CONSTRAINT [DF_P_SDP_OnTimeQty_New] DEFAULT ((0)) NOT NULL,
    [FailQty]                  INT             CONSTRAINT [DF_P_SDP_FailQty_New] DEFAULT ((0)) NOT NULL,
    [ClogRec_OnTimeQty]        INT             CONSTRAINT [DF_P_SDP_ClogRec_OnTimeQty_New] DEFAULT ((0)) NOT NULL,
    [ClogRec_FailQty]          INT             CONSTRAINT [DF_P_SDP_ClogRec_FailQty_New] DEFAULT ((0)) NOT NULL,
    [PullOutDate]              DATE            CONSTRAINT [DF_P_SDP_PullOutDate_New] DEFAULT ('') NULL,
    [ShipMode]                 VARCHAR (8000)  CONSTRAINT [DF_P_SDP_ShipMode_New] DEFAULT ('') NOT NULL,
    [Pullouttimes]             INT             CONSTRAINT [DF_P_SDP_Pullouttimes_New] DEFAULT ((0)) NOT NULL,
    [GarmentComplete]          VARCHAR (8000)  CONSTRAINT [DF_P_SDP_GarmentComplete_New] DEFAULT ('') NOT NULL,
    [ReasonID]                 VARCHAR (8000)  CONSTRAINT [DF_P_SDP_ReasonID_New] DEFAULT ('') NOT NULL,
    [OrderReason]              NVARCHAR (1000) CONSTRAINT [DF_P_SDP_OrderReason_New] DEFAULT ('') NOT NULL,
    [Handle]                   VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Handle_New] DEFAULT ('') NOT NULL,
    [SMR]                      VARCHAR (8000)  CONSTRAINT [DF_P_SDP_SMR_New] DEFAULT ('') NOT NULL,
    [POHandle]                 VARCHAR (8000)  CONSTRAINT [DF_P_SDP_POHandle_New] DEFAULT ('') NOT NULL,
    [POSMR]                    VARCHAR (8000)  CONSTRAINT [DF_P_SDP_POSMR_New] DEFAULT ('') NOT NULL,
    [OrderType]                VARCHAR (8000)  CONSTRAINT [DF_P_SDP_OrderType_New] DEFAULT ('') NOT NULL,
    [DevSample]                VARCHAR (8000)  CONSTRAINT [DF_P_SDP_DevSample_New] DEFAULT ('') NOT NULL,
    [SewingQty]                INT             CONSTRAINT [DF_P_SDP_SewingQty_New] DEFAULT ((0)) NOT NULL,
    [FOCQty]                   INT             CONSTRAINT [DF_P_SDP_FOCQty_New] DEFAULT ((0)) NOT NULL,
    [LastSewingOutputDate]     DATE            CONSTRAINT [DF_P_SDP_LastSewingOutputDate_New] DEFAULT ('') NULL,
    [LastCartonReceivedDate]   DATETIME        NULL,
    [IDDReason]                NVARCHAR (1000) CONSTRAINT [DF_P_SDP_IDDReason_New] DEFAULT ('') NOT NULL,
    [PartialShipment]          VARCHAR (8000)  CONSTRAINT [DF_P_SDP_PartialShipment_New] DEFAULT ('') NOT NULL,
    [Alias]                    VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Alias_New] DEFAULT ('') NOT NULL,
    [CFAInspectionDate]        DATETIME        NULL,
    [CFAFinalInspectionResult] VARCHAR (8000)  CONSTRAINT [DF_P_SDP_CFAFinalInspectionResult_New] DEFAULT ('') NOT NULL,
    [CFA3rdInspectDate]        DATETIME        NULL,
    [CFA3rdInspectResult]      VARCHAR (8000)  CONSTRAINT [DF_P_SDP_CFA3rdInspectResult_New] DEFAULT ('') NOT NULL,
    [Destination]              VARCHAR (8000)  CONSTRAINT [DF_P_SDP_Destination_New] DEFAULT ('') NOT NULL,
    [PONO]                     VARCHAR (8000)  CONSTRAINT [DF_P_SDP_PONO_New] DEFAULT ('') NOT NULL,
    [OutstandingReason]        NVARCHAR (1000) CONSTRAINT [DF_P_SDP_OutstandingReason_New] DEFAULT ('') NOT NULL,
    [ReasonRemark]             NVARCHAR (MAX)  CONSTRAINT [DF_P_SDP_ReasonRemark_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]              VARCHAR (8000)  CONSTRAINT [DF_P_SDP_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]             DATETIME        NULL,
    [BIStatus]                 VARCHAR (8000)  CONSTRAINT [DF_P_SDP_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SDP] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [SPNo] ASC, [Style] ASC, [Seq] ASC, [Pullouttimes] ASC)
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


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Country'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠kpi統計群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'KPIGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠KPI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FactoryKPI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'展延日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Extension'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運輸方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'DeliveryByShipmode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'On Time Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OnTimeQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fail Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'On Time Qty(Clog rec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ClogRec_OnTimeQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fail Qty(Clog rec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ClogRec_FailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'PullOutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運輸方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ShipMode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Pullouttimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment Complete ( From Trade)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'GarmentComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ReasonID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ReasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason敘述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OrderReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'POHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po 組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'POSMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'is Dev Sample' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'DevSample'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'SewingQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FOCQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'LastSewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後收料日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'LastCartonReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IDD(Intended Delivery Date) Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'IDDReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否分批運送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'PartialShipment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'別名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Alias'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFAInspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFAFinalInspectionResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFA3rdInspectDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFA3rdInspectResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國別+別名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Destination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'PONO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Outstanding Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OutstandingReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ReasonRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SDP', @level2type = N'COLUMN', @level2name = N'BIStatus';

