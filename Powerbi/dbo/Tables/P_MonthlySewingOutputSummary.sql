CREATE TABLE [dbo].[P_MonthlySewingOutputSummary] (
    [Fty]                  VARCHAR (8000)  CONSTRAINT [DF_P_MonthlySewingOutputSummary_Fty_New] DEFAULT ('') NOT NULL,
    [Period]               VARCHAR (8000)  CONSTRAINT [DF_P_MonthlySewingOutputSummary_Period_New] DEFAULT ('') NOT NULL,
    [LastDatePerMonth]     DATE            NULL,
    [TtlQtyExclSubconOut]  INT             CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlQtyExclSubconOut_New] DEFAULT ((0)) NOT NULL,
    [TtlCPUInclSubconIn]   NUMERIC (38, 3) CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlCPUInclSubconIn_New] DEFAULT ((0)) NOT NULL,
    [SubconInTtlCPU]       NUMERIC (38, 3) CONSTRAINT [DF_P_MonthlySewingOutputSummary_SubconInTtlCPU_New] DEFAULT ((0)) NOT NULL,
    [SubconOutTtlCPU]      NUMERIC (38, 3) CONSTRAINT [DF_P_MonthlySewingOutputSummary_SubconOutTtlCPU_New] DEFAULT ((0)) NOT NULL,
    [PPH]                  NUMERIC (38, 2) CONSTRAINT [DF_P_MonthlySewingOutputSummary_PPH_New] DEFAULT ((0)) NOT NULL,
    [AvgWorkHr]            NUMERIC (38, 2) CONSTRAINT [DF_P_MonthlySewingOutputSummary_AvgWorkHr_New] DEFAULT ((0)) NOT NULL,
    [TtlManpower]          INT             CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManpower_New] DEFAULT ((0)) NOT NULL,
    [TtlManhours]          NUMERIC (38, 1) CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManhours_New] DEFAULT ((0)) NOT NULL,
    [Eff]                  NUMERIC (38, 2) CONSTRAINT [DF_P_MonthlySewingOutputSummary_Eff_New] DEFAULT ((0)) NOT NULL,
    [AvgWorkHrPAMS]        NUMERIC (38, 2) CONSTRAINT [DF_P_MonthlySewingOutputSummary_AvgWorkHrPAMS_New] DEFAULT ((0)) NOT NULL,
    [TtlManpowerPAMS]      INT             CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManpowerPAMS_New] DEFAULT ((0)) NOT NULL,
    [TtlManhoursPAMS]      NUMERIC (38, 4) CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlManhoursPAMS_New] DEFAULT ((0)) NOT NULL,
    [EffPAMS]              NUMERIC (38, 2) CONSTRAINT [DF_P_MonthlySewingOutputSummary_EffPAMS_New] DEFAULT ((0)) NOT NULL,
    [TransferManpowerPAMS] INT             CONSTRAINT [DF_P_MonthlySewingOutputSummary_TransferManpowerPAMS_New] DEFAULT ((0)) NOT NULL,
    [TransferManhoursPAMS] NUMERIC (38, 4) CONSTRAINT [DF_P_MonthlySewingOutputSummary_TransferManhoursPAMS_New] DEFAULT ((0)) NOT NULL,
    [TtlRevenue]           NUMERIC (38, 3) CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlRevenue_New] DEFAULT ((0)) NOT NULL,
    [TtlWorkDay]           TINYINT         CONSTRAINT [DF_P_MonthlySewingOutputSummary_TtlWorkDay_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_MonthlySewingOutputSummary_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_MonthlySewingOutputSummary_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MonthlySewingOutputSummary] PRIMARY KEY CLUSTERED ([Fty] ASC, [Period] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Output Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'Fty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LastDatePerMonth的年月(YYYYMM)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'Period'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每月月底日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'LastDatePerMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Output(Qty) Exclude subcon-out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlQtyExclSubconOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total CPU Included Subcon-In' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlCPUInclSubconIn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon-In Total CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'SubconInTtlCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon-Out Total CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'SubconOutTtlCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU/Sewer/HR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Average Working Hour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'AvgWorkHr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manpower' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManpower'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manhours' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManhours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Eff%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'Eff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Average Working Hour(PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'AvgWorkHrPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manpower (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManpowerPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Manhours (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlManhoursPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Eff% (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'EffPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Transfer Manpower (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TransferManpowerPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Transfer Manhours (PAMS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TransferManhoursPAMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Revenue (US$)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlRevenue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total work day' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'TtlWorkDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MonthlySewingOutputSummary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MonthlySewingOutputSummary', @level2type = N'COLUMN', @level2name = N'BIStatus';

