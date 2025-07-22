CREATE TABLE [dbo].[P_CFAInspectionRecord_Detail] (
    [Action]                 NVARCHAR (1000) NULL,
    [AreaCodeDesc]           VARCHAR (8000)  NULL,
    [AuditDate]              DATE            NULL,
    [BrandID]                VARCHAR (8000)  NULL,
    [BuyerDelivery]          DATE            NULL,
    [CfaName]                VARCHAR (8000)  NULL,
    [ClogReceivedPercentage] NUMERIC (38)    NULL,
    [DefectDesc]             NVARCHAR (1000) NULL,
    [DefectQty]              INT             NULL,
    [Destination]            VARCHAR (8000)  NULL,
    [FactoryID]              VARCHAR (8000)  NOT NULL,
    [Carton]                 VARCHAR (8000)  NULL,
    [InspectedCtn]           INT             NULL,
    [InspectedPoQty]         INT             NULL,
    [InspectionStage]        VARCHAR (8000)  NULL,
    [SewingLineID]           VARCHAR (8000)  NULL,
    [Mdivisionid]            VARCHAR (8000)  NULL,
    [NoOfDefect]             INT             NULL,
    [OrderQty]               INT             NULL,
    [PONO]                   VARCHAR (8000)  NULL,
    [Remark]                 NVARCHAR (MAX)  NULL,
    [Result]                 VARCHAR (8000)  NULL,
    [SampleLot]              INT             NULL,
    [Seq]                    VARCHAR (8000)  NULL,
    [Shift]                  VARCHAR (8000)  NULL,
    [SPNO]                   VARCHAR (8000)  NULL,
    [SQR]                    NUMERIC (38, 3) NULL,
    [Status]                 VARCHAR (8000)  NULL,
    [StyleID]                VARCHAR (8000)  NULL,
    [Team]                   VARCHAR (8000)  NULL,
    [TtlCTN]                 INT             NULL,
    [VasShas]                VARCHAR (8000)  NULL,
    [1st_Inspection]         VARCHAR (8000)  NULL,
    [Ukey]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [InspectedSP]            VARCHAR (8000)  NULL,
    [InspectedSeq]           VARCHAR (8000)  NULL,
    [ReInspection]           BIT             CONSTRAINT [DF_P_CFAInspectionRecord_Detail_ReInspection_New] DEFAULT ('0') NULL,
    [BIFactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_CFAInspectionRecord_Detail_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]           DATETIME        NULL,
    [BIStatus]               VARCHAR (8000)  CONSTRAINT [DF_P_CFAInspectionRecord_Detail_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CFAInspectionRecord_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC, [FactoryID] ASC)
);



GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TOP 1 OrderID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInspectionRecord_Detail', @level2type=N'COLUMN',@level2name=N'InspectedSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TOP 1 Seq' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInspectionRecord_Detail', @level2type=N'COLUMN',@level2name=N'InspectedSeq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: 一般驗貨/ 1: 重新驗貨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInspectionRecord_Detail', @level2type=N'COLUMN',@level2name=N'ReInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInspectionRecord_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CFAInspectionRecord_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CFAInspectionRecord_Detail', @level2type = N'COLUMN', @level2name = N'BIStatus';

