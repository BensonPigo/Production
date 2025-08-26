CREATE TABLE [dbo].[P_QA_P09] (
    [WK#]                                 VARCHAR (8000)  NULL,
    [Invoice#]                            VARCHAR (8000)  NULL,
    [ATA]                                 DATE            NULL,
    [ETA]                                 DATE            NULL,
    [Season]                              VARCHAR (8000)  NULL,
    [SP#]                                 VARCHAR (8000)  NULL,
    [Seq#]                                VARCHAR (8000)  NULL,
    [Brand]                               VARCHAR (8000)  NULL,
    [Supp]                                VARCHAR (8000)  NULL,
    [Supp Name]                           VARCHAR (8000)  NULL,
    [Ref#]                                VARCHAR (8000)  NULL,
    [Color]                               NVARCHAR (1000) NULL,
    [Qty]                                 NUMERIC (38, 2) NULL,
    [Inspection Report_Fty Received Date] DATE            NULL,
    [Inspection Report_Supp Sent Date]    DATE            NULL,
    [Test Report_Fty Received Date]       DATE            NULL,
    [Test Report_ Check Clima]            BIT             NULL,
    [Test Report_Supp Sent Date]          DATE            NULL,
    [Continuity Card_Fty Received Date]   DATE            NULL,
    [Continuity Card_Supp Sent Date]      DATE            NULL,
    [Continuity Card_AWB#]                VARCHAR (8000)  NULL,
    [1st Bulk Dyelot_Fty Received Date]   DATE            NULL,
    [1st Bulk Dyelot_Supp Sent Date]      VARCHAR (8000)  NULL,
    [T2 Inspected Yards]                  NUMERIC (38, 2) NULL,
    [T2 Defect Points]                    NUMERIC (38)    NULL,
    [Grade]                               VARCHAR (8000)  NULL,
    [T1 Inspected Yards]                  NUMERIC (38, 2) NULL,
    [T1 Defect Points]                    NUMERIC (38)    NULL,
    [Fabric with clima]                   BIT             NULL,
    [FactoryID]                           VARCHAR (8000)  NOT NULL,
    [Ukey]                                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Consignee]                           VARCHAR (8000)  CONSTRAINT [DF_P_QA_P09_Consignee_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]                         VARCHAR (8000)  CONSTRAINT [DF_P_QA_P09_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                        DATETIME        NULL,
    [BIStatus]                            VARCHAR (8000)  CONSTRAINT [DF_P_QA_P09_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_QA_P09] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_P09', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_P09', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_QA_P09', @level2type = N'COLUMN', @level2name = N'BIStatus';

