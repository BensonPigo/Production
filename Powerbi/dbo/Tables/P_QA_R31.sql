CREATE TABLE [dbo].[P_QA_R31] (
    [Stage]                  VARCHAR (8000)  NULL,
    [InspResult]             VARCHAR (8000)  NULL,
    [NotYetInspCtn#]         VARCHAR (8000)  NULL,
    [NotYetInspCtn]          INT             NULL,
    [NotYetInspQty]          INT             NULL,
    [FailCtn#]               VARCHAR (8000)  NULL,
    [FailCtn]                INT             NULL,
    [FailQty]                INT             NULL,
    [MDivisionID]            VARCHAR (8000)  NULL,
    [FactoryID]              VARCHAR (8000)  NOT NULL,
    [BuyerDelivery]          DATE            NULL,
    [BrandID]                VARCHAR (8000)  NULL,
    [OrderID]                VARCHAR (8000)  NULL,
    [Category]               VARCHAR (8000)  NULL,
    [OrderTypeID]            VARCHAR (8000)  NULL,
    [CustPoNo]               VARCHAR (8000)  NULL,
    [StyleID]                VARCHAR (8000)  NULL,
    [StyleName]              NVARCHAR (1000) NULL,
    [SeasonID]               VARCHAR (8000)  NULL,
    [Dest]                   VARCHAR (8000)  NULL,
    [Customize1]             VARCHAR (8000)  NULL,
    [CustCDID]               VARCHAR (8000)  NULL,
    [Seq]                    VARCHAR (8000)  NULL,
    [ShipModeID]             VARCHAR (8000)  NULL,
    [ColorWay]               VARCHAR (8000)  NULL,
    [SewLine]                VARCHAR (8000)  NULL,
    [TtlCtn]                 VARCHAR (8000)  NULL,
    [StaggeredCtn]           VARCHAR (8000)  NULL,
    [ClogCtn]                VARCHAR (8000)  NULL,
    [ClogCtn%]               VARCHAR (8000)  NULL,
    [LastCartonReceivedDate] DATE            NULL,
    [CFAFinalInspectDate]    DATE            NULL,
    [CFA3rdInspectDate]      DATE            NULL,
    [CFARemark]              NVARCHAR (1000) NULL,
    [Ukey]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [BIFactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_QA_R31_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]           DATETIME        NULL,
    [BIStatus]               VARCHAR (8000)  CONSTRAINT [DF_P_QA_R31_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_QA_R31] PRIMARY KEY CLUSTERED ([Ukey] ASC, [FactoryID] ASC)
);



GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R31', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_R31', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_QA_R31', @level2type = N'COLUMN', @level2name = N'BIStatus';

