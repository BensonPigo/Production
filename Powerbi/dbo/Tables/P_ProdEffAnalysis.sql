CREATE TABLE [dbo].[P_ProdEffAnalysis] (
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Month]            DATE            NOT NULL,
    [ArtworkType]      VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_ArtworkType_New] DEFAULT ('') NOT NULL,
    [Program]          NVARCHAR (1000) CONSTRAINT [DF_P_ProdEffAnalysis_Program_New] DEFAULT ('') NOT NULL,
    [Style]            VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Style_New] DEFAULT ('') NOT NULL,
    [FtyZone]          VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_FtyZone_New] DEFAULT ('') NOT NULL,
    [FactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Factory_New] DEFAULT ('') NOT NULL,
    [Brand]            VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Brand_New] DEFAULT ('') NOT NULL,
    [NewCDCode]        VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_NewCDCode_New] DEFAULT ('') NOT NULL,
    [ProductType]      VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_ProductType_New] DEFAULT ('') NOT NULL,
    [FabricType]       VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_FabricType_New] DEFAULT ('') NOT NULL,
    [Lining]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Lining_New] DEFAULT ('') NOT NULL,
    [Gender]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Gender_New] DEFAULT ('') NOT NULL,
    [Construction]     VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Construction_New] DEFAULT ('') NOT NULL,
    [StyleDescription] VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_StyleDescription_New] DEFAULT ('') NOT NULL,
    [Season]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Season_New] DEFAULT ('') NOT NULL,
    [TotalQty]         NUMERIC (38, 2) CONSTRAINT [DF_P_ProdEffAnalysis_TotalQty_New] DEFAULT ((0)) NOT NULL,
    [TotalCPU]         NUMERIC (38, 4) CONSTRAINT [DF_P_ProdEffAnalysis_TotalCPU_New] DEFAULT ((0)) NOT NULL,
    [TotalManHours]    VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_TotalManHours_New] DEFAULT ('') NOT NULL,
    [PPH]              VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_PPH_New] DEFAULT ('') NOT NULL,
    [EFF]              VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_EFF_New] DEFAULT ('') NOT NULL,
    [Remark]           VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_Remark_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]      VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]     DATETIME        NULL,
    [BIStatus]         VARCHAR (8000)  CONSTRAINT [DF_P_ProdEffAnalysis_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ProdEffAnalysis] PRIMARY KEY CLUSTERED ([Month] ASC, [ArtworkType] ASC, [Program] ASC, [Style] ASC, [FactoryID] ASC, [Brand] ASC, [Season] ASC)
);

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計算月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Program'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠區域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'FtyZone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新CDCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'NewCDCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料總累' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'構造' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'StyleDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'TotalCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總人工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'TotalManHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PPH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EFF' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'EFF'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ProdEffAnalysis', @level2type = N'COLUMN', @level2name = N'BIStatus';
GO
