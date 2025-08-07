CREATE TABLE [dbo].[P_LineMapping_History] (
    [HistoryUkey]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [FactoryID]    VARCHAR (8000) NOT NULL,
    [StyleUKey]    BIGINT         NOT NULL,
    [ComboType]    VARCHAR (8000) NOT NULL,
    [Version]      TINYINT        NOT NULL,
    [Phase]        VARCHAR (8000) NOT NULL,
    [SewingLine]   VARCHAR (8000) NOT NULL,
    [IsFrom]       VARCHAR (8000) NOT NULL,
    [Team]         VARCHAR (8000) NOT NULL,
    [BIFactoryID]  VARCHAR (8000) NOT NULL,
    [BIInsertDate] DATETIME       NOT NULL,
    [BIStatus]     VARCHAR (8000) CONSTRAINT [DF_P_LineMapping_History_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_LineMapping_History] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串Style.Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'StyleUKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM版號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'Phase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源為IE P03或IE P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'IsFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_LineMapping_History', @level2type = N'COLUMN', @level2name = N'BIStatus';

