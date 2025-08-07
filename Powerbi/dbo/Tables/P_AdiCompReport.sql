CREATE TABLE [dbo].[P_AdiCompReport] (
    [Year]           VARCHAR (8000)  NULL,
    [Month]          VARCHAR (8000)  NULL,
    [ID]             VARCHAR (8000)  NULL,
    [SalesID]        VARCHAR (8000)  NULL,
    [SalesName]      NVARCHAR (1000) NULL,
    [Article]        VARCHAR (8000)  NULL,
    [ArticleName]    NVARCHAR (1000) NULL,
    [ProductionDate] DATE            NULL,
    [DefectMainID]   VARCHAR (8000)  NULL,
    [DefectSubID]    VARCHAR (8000)  NULL,
    [FOB]            NUMERIC (38, 2) NULL,
    [Qty]            NUMERIC (38)    NULL,
    [ValueinUSD]     NUMERIC (38, 4) NULL,
    [ValueINExRate]  NUMERIC (38, 2) NULL,
    [OrderID]        VARCHAR (8000)  NULL,
    [RuleNo]         NUMERIC (38)    NULL,
    [UKEY]           BIGINT          NULL,
    [BrandID]        VARCHAR (8000)  NULL,
    [FactoryID]      VARCHAR (8000)  NULL,
    [SuppID]         VARCHAR (8000)  NULL,
    [Refno]          VARCHAR (8000)  NULL,
    [IsEM]           BIT             NULL,
    [StyleID]        VARCHAR (8000)  NULL,
    [ProgramID]      VARCHAR (8000)  NULL,
    [Supplier]       VARCHAR (8000)  NULL,
    [SupplierName]   NVARCHAR (1000) NULL,
    [DefectMain]     NVARCHAR (1000) NULL,
    [DefectSub]      NVARCHAR (1000) NULL,
    [Responsibility] VARCHAR (8000)  NULL,
    [MDivisionID]    VARCHAR (8000)  NULL,
    [BIFactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_AdiCompReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]   DATETIME        NULL
);



GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AdiCompReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AdiCompReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO