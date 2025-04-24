CREATE TABLE [dbo].[P_InlineDefectSummary]
(
	[Ukey]                  bigint          CONSTRAINT [DF_P_InlineDefectSummary_Ukey]              NOT NULL  IDENTITY(1,1), 
    [FirstInspectedDate]    DATE            CONSTRAINT [DF_P_InlineDefectSummary_FirstInspectedDate]                                NULL, 
    [FactoryID]             VARCHAR(8)      CONSTRAINT [DF_P_InlineDefectSummary_FactoryID]   NOT NULL DEFAULT (('')), 
    [BrandID]               VARCHAR(8)      CONSTRAINT [DF_P_InlineDefectSummary_BrandID]   NOT NULL DEFAULT (('')), 
    [StyleID]               VARCHAR(15)     CONSTRAINT [DF_P_InlineDefectSummary_StyleID]   NOT NULL DEFAULT (('')), 
    [CustPoNo]              VARCHAR(30)     CONSTRAINT [DF_P_InlineDefectSummary_CustPoNo]   NOT NULL DEFAULT (('')), 
    [OrderID]               VARCHAR(13)     CONSTRAINT [DF_P_InlineDefectSummary_OrderID]   NOT NULL DEFAULT (('')), 
    [Article]               VARCHAR(8)      CONSTRAINT [DF_P_InlineDefectSummary_Article]   NOT NULL DEFAULT (('')), 
    [Alias]                 VARCHAR(30)     CONSTRAINT [DF_P_InlineDefectSummary_Alias]   NOT NULL DEFAULT (('')), 
    [CDCodeID]              VARCHAR(6)      CONSTRAINT [DF_P_InlineDefectSummary_CDCodeID]   NOT NULL DEFAULT (('')), 
    [CDCodeNew]             VARCHAR(5)      CONSTRAINT [DF_P_InlineDefectSummary_CDCodeNew]   NOT NULL DEFAULT (('')), 
    [ProductType]           NVARCHAR(30)    CONSTRAINT [DF_P_InlineDefectSummary_ProductType]   NOT NULL DEFAULT (('')), 
    [FabricType]            NVARCHAR(30)    CONSTRAINT [DF_P_InlineDefectSummary_FabricType]   NOT NULL DEFAULT (('')), 
    [Lining]                VARCHAR(20)     CONSTRAINT [DF_P_InlineDefectSummary_Lining]   NOT NULL DEFAULT (('')), 
    [Gender]                VARCHAR(10)     CONSTRAINT [DF_P_InlineDefectSummary_Gender]   NOT NULL DEFAULT (('')), 
    [Construction]          NVARCHAR(30)    CONSTRAINT [DF_P_InlineDefectSummary_Construction]   NOT NULL DEFAULT (('')), 
    [ProductionFamilyID]    VARCHAR(20)     CONSTRAINT [DF_P_InlineDefectSummary_ProductionFamilyID]   NOT NULL DEFAULT (('')), 
    [Team]                 VARCHAR(10)     CONSTRAINT [DF_P_InlineDefectSummary_Team]   NOT NULL DEFAULT (('')), 
    [QCName]                VARCHAR(100)     CONSTRAINT [DF_P_InlineDefectSummary_QCName]   NOT NULL DEFAULT (('')), 
    [Shift]                 VARCHAR(5)      CONSTRAINT [DF_P_InlineDefectSummary_Shift]   NOT NULL DEFAULT (('')), 
    [Line]                  VARCHAR(5)      CONSTRAINT [DF_P_InlineDefectSummary_Line]   NOT NULL DEFAULT (('')), 
    [SewingCell]            VARCHAR(2)      CONSTRAINT [DF_P_InlineDefectSummary_SewingCell]   NOT NULL DEFAULT (('')), 
    [InspectedQty]          INT             CONSTRAINT [DF_P_InlineDefectSummary_InspectedQty]   NOT NULL DEFAULT ((0)), 
    [RejectWIP]             INT             CONSTRAINT [DF_P_InlineDefectSummary_RejectWIP]   NOT NULL DEFAULT ((0)), 
    [InlineWFT ]            INT             CONSTRAINT [DF_P_InlineDefectSummary_InlineWFT ]   NOT NULL DEFAULT ((0)), 
    [InlineRFT]             INT             CONSTRAINT [DF_P_InlineDefectSummary_]   NOT NULL DEFAULT ((0))
    CONSTRAINT [PK_P_InlineDefectSummary] PRIMARY KEY ([Ukey],[FactoryID]), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (('')), 
    [BIInsertDate] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'首次檢驗日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'FirstInspectedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'款式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'StyleID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客戶訂單單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'CustPoNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Article'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'地區',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Alias'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CD Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'CDCodeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'New CD Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'CDCodeNew'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產品種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'ProductType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'平織/針織',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'FabricType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'襯',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Lining'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'性別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Gender'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Construction',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Construction'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產品種類群組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'ProductionFamilyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'QC Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'QCName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產線班別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Shift'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產線線別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Line'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產線組別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'SewingCell'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InspectedQty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'InspectedQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'RejectWIP',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'RejectWIP'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InlineRFT',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'InlineRFT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'InlineWFT ',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'InlineWFT '
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'組別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'Team'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectSummary',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'