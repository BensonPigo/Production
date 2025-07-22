CREATE TABLE [dbo].[P_ICRAnalysis] (
    [ICRNo]                         VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_ICRNo_New] DEFAULT ('') NOT NULL,
    [Status]                        VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_Status_New] DEFAULT ('') NOT NULL,
    [Mdivision]                     VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_Mdivision_New] DEFAULT ('') NOT NULL,
    [ResponsibilityFTY]             NVARCHAR (1000) CONSTRAINT [DF_P_ICRAnalysis_ResponsibilityFTY_New] DEFAULT ('') NOT NULL,
    [FTY]                           VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_FTY_New] DEFAULT ('') NOT NULL,
    [SDPKPICode]                    VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_SDPKPICode_New] DEFAULT ('') NOT NULL,
    [SPNo]                          VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_SPNo_New] DEFAULT ('') NOT NULL,
    [StyleID]                       VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_StyleID_New] DEFAULT ('') NOT NULL,
    [SeasonID]                      VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_SeasonID_New] DEFAULT ('') NOT NULL,
    [BrandID]                       VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_BrandID_New] DEFAULT ('') NOT NULL,
    [TotalQty]                      INT             CONSTRAINT [DF_P_ICRAnalysis_TotalQty_New] DEFAULT ((0)) NOT NULL,
    [POHandle]                      VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_POHandle_New] DEFAULT ('') NOT NULL,
    [POSMR]                         VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_POSMR_New] DEFAULT ('') NOT NULL,
    [MR]                            VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_MR_New] DEFAULT ('') NOT NULL,
    [SMR]                           VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_SMR_New] DEFAULT ('') NOT NULL,
    [IssueSubject]                  NVARCHAR (1000) CONSTRAINT [DF_P_ICRAnalysis_IssueSubject_New] DEFAULT ('') NOT NULL,
    [ResponsibilityAndExplaination] NVARCHAR (MAX)  CONSTRAINT [DF_P_ICRAnalysis_ResponsibilityAndExplaination_New] DEFAULT ('') NOT NULL,
    [RMtlAmtUSD]                    NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_RMtlAmtUSD_New] DEFAULT ((0)) NOT NULL,
    [OtherAmtUSD]                   NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_OtherAmtUSD_New] DEFAULT ((0)) NOT NULL,
    [ActFreightAmtUSD]              NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_ActFreightAmtUSD_New] DEFAULT ((0)) NOT NULL,
    [TotalUSD]                      NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_TotalUSD_New] DEFAULT ((0)) NOT NULL,
    [Createdate]                    DATE            NULL,
    [Confirmeddate]                 DATE            NULL,
    [VoucherNo]                     VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_VoucherNo_New] DEFAULT ('') NOT NULL,
    [VoucherDate]                   DATE            NULL,
    [Seq]                           VARCHAR (8000)  CONSTRAINT [DF_P_ICRReportList_Seq_New] DEFAULT ('') NOT NULL,
    [SourceType]                    NVARCHAR (1000) CONSTRAINT [DF_P_ICRAnalysis_SourceType_New] DEFAULT ('') NOT NULL,
    [WeaveType]                     VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_WeaveType_New] DEFAULT ('') NOT NULL,
    [IrregularMtlType]              VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_IrregularMtlType_New] DEFAULT ('') NOT NULL,
    [IrregularQty]                  NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_IrregularQty_New] DEFAULT ((0)) NOT NULL,
    [IrregularFOC]                  NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_IrregularFOC_New] DEFAULT ((0)) NOT NULL,
    [IrregularPriceUSD]             NUMERIC (38, 2) CONSTRAINT [DF_P_ICRAnalysis_IrregularPriceUSD_New] DEFAULT ((0)) NOT NULL,
    [IrregularAmtUSD]               NUMERIC (38, 5) CONSTRAINT [DF_P_ICRAnalysis_IrregularAmtUSD_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]                   VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                  DATETIME        NULL,
    [BIStatus]                      VARCHAR (8000)  CONSTRAINT [DF_P_ICRAnalysis_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ICRAnalysis] PRIMARY KEY CLUSTERED ([ICRNo] ASC, [Seq] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ICRAnalysis', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ICRAnalysis', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ICRAnalysis', @level2type = N'COLUMN', @level2name = N'BIStatus';

