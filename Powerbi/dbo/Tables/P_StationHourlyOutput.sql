CREATE TABLE [dbo].[P_StationHourlyOutput] (
    [FactoryID]         VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_FactoryID_New] DEFAULT ('') NOT NULL,
    [Date]              DATE            NULL,
    [Shift]             VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_Shift_New] DEFAULT ('') NOT NULL,
    [Team]              VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_Team_New] DEFAULT ('') NOT NULL,
    [Line]              VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_Line_New] DEFAULT ('') NOT NULL,
    [Station]           VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_Station_New] DEFAULT ('') NOT NULL,
    [Capacity]          INT             CONSTRAINT [DF_P_StationHourlyOutput_Capacity_New] DEFAULT ((0)) NOT NULL,
    [Target]            INT             CONSTRAINT [DF_P_StationHourlyOutput_Target_New] DEFAULT ((0)) NOT NULL,
    [TotalOutput]       INT             CONSTRAINT [DF_P_StationHourlyOutput_TotalOutput_New] DEFAULT ((0)) NOT NULL,
    [ProblemsEncounter] NVARCHAR (1000) CONSTRAINT [DF_P_StationHourlyOutput_ProblemsEncounter_New] DEFAULT ('') NOT NULL,
    [ActionsTaken]      NVARCHAR (1000) CONSTRAINT [DF_P_StationHourlyOutput_ActionsTaken_New] DEFAULT ('') NOT NULL,
    [Problems4MS]       VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_Problems4MS_New] DEFAULT ('') NOT NULL,
    [Ukey]              INT             CONSTRAINT [DF_P_StationHourlyOutput_Ukey_New] DEFAULT ('') NOT NULL,
    [StyleID]           VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_StyleID_New] DEFAULT ('') NOT NULL,
    [OrderID]           VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_OrderID_New] DEFAULT ('') NOT NULL,
    [Problems4MSDesc]   VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_Problems4MSDesc_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]      DATETIME        NULL,
    [BIStatus]          VARCHAR (8000)  CONSTRAINT [DF_P_StationHourlyOutput_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_StationHourlyOutput] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [Ukey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Team' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Station' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Station'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時最大能產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Capacity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時目標數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'TotalOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'問題說明紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'ProblemsEncounter'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採取何種改善紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'ActionsTaken'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Problems 4MS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Problems4MS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StyleID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_StationHourlyOutput', @level2type = N'COLUMN', @level2name = N'BIStatus';

