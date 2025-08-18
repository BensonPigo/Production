CREATE TABLE [dbo].[P_ChangeoverCheckList_Histroy] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [FactoryID]    VARCHAR (8000) NOT NULL,
    [InlineDate]   DATETIME       NOT NULL,
    [Line]         VARCHAR (8000) NOT NULL,
    [OldSP]        VARCHAR (8000) NOT NULL,
    [OldStyle]     VARCHAR (8000) NOT NULL,
    [OldComboType] VARCHAR (8000) NOT NULL,
    [NewSP]        VARCHAR (8000) NOT NULL,
    [NewStyle]     VARCHAR (8000) NOT NULL,
    [NewComboType] VARCHAR (8000) NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_Histroy_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ChangeoverCheckList_Histroy] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'OldSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'OldStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'OldComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'NewSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'NewStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'NewComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Histroy', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ChangeoverCheckList_Histroy', @level2type = N'COLUMN', @level2name = N'BIStatus';

