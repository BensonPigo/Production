CREATE TABLE [dbo].[P_LineMapping] (
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Factory_New] DEFAULT ('') NOT NULL,
    [StyleUKey]            BIGINT          CONSTRAINT [DF_P_LineMapping_StyleUKey_New] DEFAULT ((0)) NOT NULL,
    [ComboType]            VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_ComboType_New] DEFAULT ('') NOT NULL,
    [Version]              TINYINT         CONSTRAINT [DF_P_LineMapping_Version_New] DEFAULT ('') NOT NULL,
    [Phase]                VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Phase_New] DEFAULT ('') NOT NULL,
    [SewingLine]           VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_SewingLine_New] DEFAULT ('') NOT NULL,
    [IsFrom]               VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_IsFrom_New] DEFAULT ('') NOT NULL,
    [Team]                 VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Team_New] DEFAULT ('') NOT NULL,
    [ID]                   BIGINT          NOT NULL,
    [Style]                VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Style_New] DEFAULT ('') NOT NULL,
    [Season]               VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Season_New] DEFAULT ('') NOT NULL,
    [Brand]                VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Brand_New] DEFAULT ('') NOT NULL,
    [Desc.]                VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Desc._New] DEFAULT ('') NOT NULL,
    [CPU/PC]               DECIMAL (18, 3) CONSTRAINT [DF_P_LineMapping_CPU/PC_New] DEFAULT ((0)) NOT NULL,
    [No. of Sewer]         TINYINT         CONSTRAINT [DF_P_LineMapping_No. of Sewer_New] DEFAULT ((0)) NOT NULL,
    [LBR By GSD Time(%)]   NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_LBR By GSD Time(%)_New] DEFAULT ((0)) NOT NULL,
    [Total GSD Time]       NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Total GSD Time_New] DEFAULT ((0)) NOT NULL,
    [Avg. GSD Time]        NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Avg. GSD Time_New] DEFAULT ((0)) NOT NULL,
    [Highest GSD Time]     NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Highest GSD Time_New] DEFAULT ((0)) NULL,
    [LBR By Cycle Time(%)] NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_LBR By Cycle Time(%)_New] DEFAULT ((0)) NOT NULL,
    [Total Cycle Time]     NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Total Cycle Time_New] DEFAULT ((0)) NOT NULL,
    [Avg. Cycle Time]      NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Avg. Cycle Time_New] DEFAULT ((0)) NOT NULL,
    [Highest Cycle Time]   NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Highest Cycle Time_New] DEFAULT ((0)) NOT NULL,
    [Total % Time Diff(%)] INT             CONSTRAINT [DF_P_LineMapping_Total % Time Diff(%)_New] DEFAULT ((0)) NOT NULL,
    [No. of Hours]         NUMERIC (38, 1) CONSTRAINT [DF_P_LineMapping_No. of Hours_New] DEFAULT ((0)) NOT NULL,
    [Oprts of Presser]     TINYINT         CONSTRAINT [DF_P_LineMapping_Oprts of Presser_New] DEFAULT ((0)) NOT NULL,
    [Oprts of Packer]      TINYINT         CONSTRAINT [DF_P_LineMapping_Oprts of Packer_New] DEFAULT ((0)) NOT NULL,
    [Ttl Sew Line Oprts]   TINYINT         CONSTRAINT [DF_P_LineMapping_Ttl Sew Line Oprts_New] DEFAULT ((0)) NOT NULL,
    [Target / Hr.(100%)]   INT             CONSTRAINT [DF_P_LineMapping_Target / Hr.(100%)_New] DEFAULT ((0)) NOT NULL,
    [Daily Demand / Shift] NUMERIC (38, 1) CONSTRAINT [DF_P_LineMapping_Daily Demand / Shift_New] DEFAULT ((0)) NOT NULL,
    [Takt Time]            NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Takt Time_New] DEFAULT ((0)) NOT NULL,
    [EOLR]                 NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_EOLR_New] DEFAULT ((0)) NOT NULL,
    [PPH]                  NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_PPH_New] DEFAULT ((0)) NOT NULL,
    [GSD Status]           VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_GSD Status_New] DEFAULT ('') NOT NULL,
    [GSD Version]          VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_GSD Version_New] DEFAULT ('') NOT NULL,
    [Status]               VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Status_New] DEFAULT ('') NOT NULL,
    [Add Name]             VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Add Name_New] DEFAULT ('') NOT NULL,
    [Add Date]             DATETIME        NULL,
    [Edit Name]            VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Edit Name_New] DEFAULT ('') NOT NULL,
    [Edit Date]            DATETIME        NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_LineMapping] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [StyleUKey] ASC, [ComboType] ASC, [Version] ASC, [Phase] ASC, [SewingLine] ASC, [IsFrom] ASC, [Team] ASC)
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


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串Style.Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'StyleUKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM版號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Phase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源為IE P03或IE P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'IsFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串表身ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Desc.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'CPU/PC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車工人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'No. of Sewer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total GSD Time] / [Highest GSD Time] / [No. of Sewer] × 100%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'LBR By GSD Time(%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總GSD時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Total GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total GSD Time] / [No. of Sewer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Avg. GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最高的GSD時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Highest GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total Cycle Time] / [Highest Cycle Time] / [No. of Sewer] × 100%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'LBR By Cycle Time(%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總CycleTime時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Total Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total Cycle Time] / [No. of Sewer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Avg. Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最高的CycleTime時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Highest Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'([Total GSD Time]-[Total Cycle Time]) / [Total Cycle Time]×100%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Total % Time Diff(%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每站工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'No. of Hours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'整燙人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Oprts of Presser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Oprts of Packer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[No. Of Sewer] + [Oprts Of Presser] + [Oprts Of Packer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Ttl Sew Line Oprts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3600 × [No. of Sewer] / [Total Cycle Time]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Target / Hr.(100%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Target / Hr. (100%)] × [No. of Hours]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Daily Demand / Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3600 × [No. Of Hours] / [Daily Demand / Shift]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Takt Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3600 / [Highest Cycle Time]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'EOLR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ELOR] × [CPU /PC] / [No. Of Sewer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'GSD Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD版號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'GSD Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line Mapping狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Add Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Add Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Edit Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Edit Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_LineMapping', @level2type = N'COLUMN', @level2name = N'BIStatus';

