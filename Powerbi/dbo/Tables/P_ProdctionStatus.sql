CREATE TABLE [dbo].[P_ProdctionStatus] (
    [SewingLineID]            VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_SewingLineID_New] DEFAULT ('') NOT NULL,
    [FactoryID]               VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_FactoryID_New] DEFAULT ('') NOT NULL,
    [SPNO]                    VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_SPNO_New] DEFAULT ('') NOT NULL,
    [StyleID]                 VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_StyleID_New] DEFAULT ('') NOT NULL,
    [StyleName]               NVARCHAR (1000) CONSTRAINT [DF_P_ProdctionStatus_StyleName_New] DEFAULT ('') NOT NULL,
    [ComboType]               VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_ComboType_New] DEFAULT ('') NOT NULL,
    [SPCategory]              VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_SPCategory_New] DEFAULT ('') NOT NULL,
    [SCIDelivery]             DATE            NULL,
    [BuyerDelivery]           DATE            NULL,
    [InlineDate]              DATETIME        NOT NULL,
    [OfflineDate]             DATETIME        NOT NULL,
    [OrderQty]                INT             CONSTRAINT [DF_P_ProdctionStatus_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [AlloQty]                 INT             CONSTRAINT [DF_P_ProdctionStatus_AlloQty_New] DEFAULT ((0)) NOT NULL,
    [SewingQty]               INT             CONSTRAINT [DF_P_ProdctionStatus_SewingQty_New] DEFAULT ((0)) NOT NULL,
    [SewingBalance]           INT             CONSTRAINT [DF_P_ProdctionStatus_SewingBalance_New] DEFAULT ((0)) NOT NULL,
    [TtlSewingQtyByComboType] INT             CONSTRAINT [DF_P_ProdctionStatus_TtlSewingQtyByComboType_New] DEFAULT ((0)) NOT NULL,
    [TtlSewingQtyBySP]        INT             CONSTRAINT [DF_P_ProdctionStatus_TtlSewingQtyBySP_New] DEFAULT ((0)) NOT NULL,
    [ClogQty]                 INT             CONSTRAINT [DF_P_ProdctionStatus_ClogQty_New] DEFAULT ((0)) NOT NULL,
    [TtlClogBalance]          INT             CONSTRAINT [DF_P_ProdctionStatus_TtlClogBalance_New] DEFAULT ((0)) NOT NULL,
    [DaysOffToDDSched]        VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_DaysOffToDDSched_New] DEFAULT ('') NOT NULL,
    [DaysTodayToDD]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_DaysTodayToDD_New] DEFAULT ('') NOT NULL,
    [NeedQtyByStdOut]         VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_NeedQtyByStdOut_New] DEFAULT ('') NOT NULL,
    [Pending]                 VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_Pending_New] DEFAULT ('') NOT NULL,
    [TotalStandardOutput]     NUMERIC (38, 6) CONSTRAINT [DF_P_ProdctionStatus_TotalStandardOutput_New] DEFAULT ((0)) NOT NULL,
    [DaysToDrainByStdOut]     VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_DaysToDrainByStdOut_New] DEFAULT ('') NOT NULL,
    [OfflineDateByStdOut]     DATETIME        NULL,
    [DaysOffToDDByStdOut]     VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_DaysOffToDDByStdOut_New] DEFAULT ('') NOT NULL,
    [MaxOutput]               VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_MaxOutput_New] DEFAULT ('') NOT NULL,
    [DaysToDrainByMaxOut]     VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_DaysToDrainByMaxOut_New] DEFAULT ('') NOT NULL,
    [OfflineDateByMaxOut]     DATETIME        NULL,
    [DaysOffToDDByMaxOut]     VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_DaysOffToDDByMaxOut_New] DEFAULT ('') NOT NULL,
    [TightByMaxOut]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_TightByMaxOut_New] DEFAULT ('') NOT NULL,
    [TightByStdOut]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_TightByStdOut_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]            DATETIME        NULL,
    [BIStatus]                VARCHAR (8000)  CONSTRAINT [DF_P_ProdctionStatus_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ProdctionStatus] PRIMARY KEY CLUSTERED ([SewingLineID] ASC, [FactoryID] ASC, [SPNO] ASC, [ComboType] ASC, [InlineDate] ASC, [OfflineDate] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdctionStatus', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdctionStatus', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ProdctionStatus', @level2type = N'COLUMN', @level2name = N'BIStatus';

