CREATE TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] (
    [Year-Month]    DATE            NOT NULL,
    [FtyZone]       VARCHAR (8000)  NOT NULL,
    [Factory]       VARCHAR (8000)  NOT NULL,
    [Line]          VARCHAR (8000)  NOT NULL,
    [TotalQty]      INT             CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_TotalQty_New] DEFAULT ((0)) NOT NULL,
    [TotalCPU]      NUMERIC (38, 3) CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_TotalCPU_New] DEFAULT ((0)) NOT NULL,
    [TotalManhours] NUMERIC (38, 3) CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_TotalManhours_New] DEFAULT ((0)) NOT NULL,
    [PPH]           NUMERIC (38, 2) CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_PPH_New] DEFAULT ((0)) NOT NULL,
    [EFF]           NUMERIC (38, 2) CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_EFF_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]   VARCHAR (8000)  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]  DATETIME        NULL,
    [BIStatus]      VARCHAR (8000)  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ProdEfficiencyByFactorySewingLine] PRIMARY KEY CLUSTERED ([Year-Month] ASC, [FtyZone] ASC, [Factory] ASC, [Line] ASC)
);



GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput當月月底日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'Year-Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty Zone' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'FtyZone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產出數量的CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'TotalCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'TotalManhours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PPH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EFF' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'EFF'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ProdEfficiencyByFactorySewingLine', @level2type = N'COLUMN', @level2name = N'BIStatus';

