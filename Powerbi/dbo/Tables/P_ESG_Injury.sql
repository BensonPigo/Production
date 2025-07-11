CREATE TABLE [dbo].[P_ESG_Injury] (
    [ID]                VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injurye_ID_New] DEFAULT ('') NOT NULL,
    [FactoryID]         VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injurye_FactoryID_New] DEFAULT ('') NOT NULL,
    [InjuryType]        VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injurye_InjuryType_New] DEFAULT ('') NOT NULL,
    [CDate]             DATE            NULL,
    [LossHours]         NUMERIC (38, 1) CONSTRAINT [DF_P_ESG_Injurye_LossHours_New] DEFAULT ((0)) NOT NULL,
    [IncidentType]      VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injurye_IncidentType_New] DEFAULT ('') NOT NULL,
    [IncidentRemark]    NVARCHAR (2000) CONSTRAINT [DF_P_ESG_Injurye_IncidentRemark_New] DEFAULT ('') NOT NULL,
    [SevereLevel]       VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injurye_SevereLevel_New] DEFAULT ('') NOT NULL,
    [SevereRemark]      NVARCHAR (2000) CONSTRAINT [DF_P_ESG_Injurye_SevereRemark_New] DEFAULT ('') NOT NULL,
    [CAP]               NVARCHAR (2000) CONSTRAINT [DF_P_ESG_Injurye_CAP_New] DEFAULT ('') NOT NULL,
    [Incharge]          NVARCHAR (1000) CONSTRAINT [DF_P_ESG_Injurye_Incharge_New] DEFAULT ('') NOT NULL,
    [InchargeCheckDate] DATE            NULL,
    [Approver]          NVARCHAR (1000) CONSTRAINT [DF_P_ESG_Injurye_Approver_New] DEFAULT ('') NOT NULL,
    [ApproveDate]       DATE            NULL,
    [ProcessDate]       DATE            NULL,
    [ProcessTime]       TIME (7)        NULL,
    [ProcessUpdate]     NVARCHAR (2000) CONSTRAINT [DF_P_ESG_Injurye_ProcessUpdate_New] DEFAULT ('') NOT NULL,
    [Status]            VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injurye_Status_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injury_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]      DATETIME        NULL,
    [BIStatus]          VARCHAR (8000)  CONSTRAINT [DF_P_ESG_Injury_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ESG_Injury] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工傷;勞安' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'InjuryType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'損失時數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'LossHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'IncidentType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'IncidentRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'嚴重程度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'SevereLevel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'嚴重程度描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'SevereRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Action plan執行方案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'CAP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'負責人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'Incharge'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'Approver'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ApproveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ProcessDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ProcessTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ProcessUpdate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單據狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ESG_Injury', @level2type = N'COLUMN', @level2name = N'BIStatus';

