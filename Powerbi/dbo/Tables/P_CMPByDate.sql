CREATE TABLE [dbo].[P_CMPByDate] (
    [FactoryID]            VARCHAR (8000)  NOT NULL,
    [OutputDate]           DATE            NOT NULL,
    [GPHCPU]               DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_GPHCPU_New] DEFAULT ((0)) NOT NULL,
    [SPHCPU]               DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_SPHCPU_New] DEFAULT ((0)) NOT NULL,
    [VPHCPU]               DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_VPHCPU_New] DEFAULT ((0)) NOT NULL,
    [GPHManhours]          DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_GPHManhours_New] DEFAULT ((0)) NOT NULL,
    [SPHManhours]          DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_SPHManhours_New] DEFAULT ((0)) NOT NULL,
    [VPHManhours]          DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_VPHManhours_New] DEFAULT ((0)) NOT NULL,
    [GPH]                  DECIMAL (18, 2) CONSTRAINT [DF_P_CMPByDate_GPH_New] DEFAULT ((0)) NOT NULL,
    [SPH]                  DECIMAL (18, 2) CONSTRAINT [DF_P_CMPByDate_SPH_New] DEFAULT ((0)) NOT NULL,
    [VPH]                  DECIMAL (18, 2) CONSTRAINT [DF_P_CMPByDate_VPH_New] DEFAULT ((0)) NOT NULL,
    [ManhoursRatio]        DECIMAL (18, 2) CONSTRAINT [DF_P_CMPByDate_ManhoursRatio_New] DEFAULT ((0)) NOT NULL,
    [TotalActiveHeadcount] DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_TotalActiveHeadcount_New] DEFAULT ((0)) NOT NULL,
    [RevenumDeptHeadcount] DECIMAL (18, 3) CONSTRAINT [DF_P_CMPByDate_RevenumDeptHeadcount_New] DEFAULT ((0)) NOT NULL,
    [ManpowerRatio]        DECIMAL (18, 2) CONSTRAINT [DF_P_CMPByDate_ManpowerRatio_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_CMPByDate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_CMPByDate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CMPByDate] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [OutputDate] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment per hour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'GPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subprocess per hour ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'SPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value per hour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'VPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工時比率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'ManhoursRatio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收入部門人頭數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'RevenumDeptHeadcount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人力比率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'ManpowerRatio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CMPByDate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CMPByDate', @level2type = N'COLUMN', @level2name = N'BIStatus';

