CREATE TABLE [dbo].[SewingOutput_DailyUnlock] (
    [Ukey]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [SewingOutputID] VARCHAR (13)   NULL,
    [ReasonID]       VARCHAR (5)    NULL,
    [Remark]         NVARCHAR (MAX) NULL,
    [RequestDate]    DATE           NULL,
    [RequestName]    VARCHAR (10)   NULL,
    [UnLockDate]     DATETIME       NULL,
    [UnLockName]     VARCHAR (10)   NULL,
    [FactoryID]      VARCHAR (8)    CONSTRAINT [DF_SewingOutput_DailyUnlock_FactoryID] DEFAULT ('') NOT NULL,
    [OutputDate]     DATE           CONSTRAINT [DF_SewingOutput_DailyUnlock_OutputDate] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SewingOutput_DailyUnlock] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'解鎖人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'UnLockName';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'解鎖日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'UnLockDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提出解鎖需求人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'RequestName';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提出解鎖需求日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'RequestDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'Remark';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'ReasonID';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SewingOutput ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'SewingOutputID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_DailyUnlock', @level2type = N'COLUMN', @level2name = N'Ukey';

