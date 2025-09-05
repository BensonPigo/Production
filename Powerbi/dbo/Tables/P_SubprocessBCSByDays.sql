CREATE TABLE [dbo].[P_SubprocessBCSByDays] (
    [SewingInline]    DATE            NOT NULL,
    [Factory]         VARCHAR (8000)  NOT NULL,
    [SubprocessBCS]   DECIMAL (18, 2) CONSTRAINT [DF_P_SubprocessBCSByDays_SubprocessBCS_New] DEFAULT ((0)) NOT NULL,
    [TTLLoadedBundle] INT             CONSTRAINT [DF_P_SubprocessBCSByDays_TTLLoadedBundle_New] DEFAULT ((0)) NOT NULL,
    [TTLBundle]       INT             CONSTRAINT [DF_P_SubprocessBCSByDays_TTLBundle_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessBCSByDays_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]    DATETIME        NULL,
    [BIStatus]        VARCHAR (8000)  CONSTRAINT [DF_P_SubprocessBCSByDays_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SubprocessBCSByDays] PRIMARY KEY CLUSTERED ([SewingInline] ASC, [Factory] ASC)
);



GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TTLLoadedBundle/TTLBundle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByDays', @level2type=N'COLUMN',@level2name=N'SubprocessBCS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GroupBy後P_SubprocessWIP.Inline不為null的筆數統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByDays', @level2type=N'COLUMN',@level2name=N'TTLLoadedBundle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P_SubprocessWIP GroupBy後筆數統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByDays', @level2type=N'COLUMN',@level2name=N'TTLBundle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByDays', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByDays', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubprocessBCSByDays', @level2type = N'COLUMN', @level2name = N'BIStatus';

