CREATE TABLE [dbo].[SampleRFTNotification] (
    [FactoryID]       VARCHAR (8)    NOT NULL,
    [RFTStandard]     NUMERIC (5, 2) CONSTRAINT [DF_SampleRFTNotification_RFTStandard] DEFAULT ((0)) NOT NULL,
    [ToAddress]       VARCHAR (500)  CONSTRAINT [DF_SampleRFTNotification_ToAddress] DEFAULT ('') NOT NULL,
    [CcAddress]       VARCHAR (500)  CONSTRAINT [DF_SampleRFTNotification_CcAddress] DEFAULT ('') NOT NULL,
    [StartTime]       TIME (7)       NULL,
    [EndTime]         TIME (7)       NULL,
    [Frequency]       VARCHAR (20)   CONSTRAINT [DF_SampleRFTNotification_Frequency] DEFAULT ('') NOT NULL,
    [Description]     NVARCHAR (500) CONSTRAINT [DF_SampleRFTNotification_Description] DEFAULT ('') NOT NULL,
    [AddDate]         DATETIME       NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_SampleRFTNotification_AddName] DEFAULT ('') NOT NULL,
    [EditDate]        DATETIME       NULL,
    [Editname]        VARCHAR (10)   CONSTRAINT [DF_SampleRFTNotification_Editname] DEFAULT ('') NOT NULL,
    [LastExecuteTime] DATETIME       NULL,
    CONSTRAINT [PK_SampleRFTNotification] PRIMARY KEY CLUSTERED ([FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後一次執行的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'LastExecuteTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'Editname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'頻率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'Frequency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結束時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'EndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'StartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail CC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'CcAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail To', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'ToAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SampleRFT標準', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'RFTStandard';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SampleRFTNotification', @level2type = N'COLUMN', @level2name = N'FactoryID';

