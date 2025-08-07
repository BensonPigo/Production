CREATE TABLE [dbo].[P_InlineDefectSummary] (
    [Ukey]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [FirstInspectedDate] DATE            NULL,
    [FactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_FactoryID_New] DEFAULT ('') NOT NULL,
    [BrandID]            VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]            VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_StyleID_New] DEFAULT ('') NOT NULL,
    [CustPoNo]           VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_CustPoNo_New] DEFAULT ('') NOT NULL,
    [OrderID]            VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_OrderID_New] DEFAULT ('') NOT NULL,
    [Article]            VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Article_New] DEFAULT ('') NOT NULL,
    [Alias]              VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Alias_New] DEFAULT ('') NOT NULL,
    [CDCodeID]           VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_CDCodeID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]          VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [ProductType]        NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectSummary_ProductType_New] DEFAULT ('') NOT NULL,
    [FabricType]         NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectSummary_FabricType_New] DEFAULT ('') NOT NULL,
    [Lining]             VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Lining_New] DEFAULT ('') NOT NULL,
    [Gender]             VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Gender_New] DEFAULT ('') NOT NULL,
    [Construction]       NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectSummary_Construction_New] DEFAULT ('') NOT NULL,
    [ProductionFamilyID] VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_ProductionFamilyID_New] DEFAULT ('') NOT NULL,
    [Team]               VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Team_New] DEFAULT ('') NOT NULL,
    [QCName]             VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_QCName_New] DEFAULT ('') NOT NULL,
    [Shift]              VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Shift_New] DEFAULT ('') NOT NULL,
    [Line]               VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_Line_New] DEFAULT ('') NOT NULL,
    [SewingCell]         VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_SewingCell_New] DEFAULT ('') NOT NULL,
    [InspectedQty]       INT             CONSTRAINT [DF_P_InlineDefectSummary_InspectedQty_New] DEFAULT ((0)) NOT NULL,
    [RejectWIP]          INT             CONSTRAINT [DF_P_InlineDefectSummary_RejectWIP_New] DEFAULT ((0)) NOT NULL,
    [InlineWFT]          INT             CONSTRAINT [DF_P_InlineDefectSummary_InlineWFT_New] DEFAULT ((0)) NOT NULL,
    [InlineRFT]          INT             CONSTRAINT [DF_P_InlineDefectSummary_InlineRFT_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]       DATETIME        NULL,
    [BIStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectSummary_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_InlineDefectSummary] PRIMARY KEY CLUSTERED ([Ukey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次檢驗日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'FirstInspectedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'CustPoNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地區' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Alias'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'CDCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'New CD Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'CDCodeNew'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平織/針織' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Construction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品種類群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'ProductionFamilyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QC Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'QCName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線線別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'SewingCell'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectedQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'InspectedQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RejectWIP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'RejectWIP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineWFT ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'InlineWFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineRFT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'InlineRFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectSummary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_InlineDefectSummary', @level2type = N'COLUMN', @level2name = N'BIStatus';

