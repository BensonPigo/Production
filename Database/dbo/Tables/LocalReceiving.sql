CREATE TABLE [dbo].[LocalReceiving] (
    [Id]          VARCHAR (13)  CONSTRAINT [DF_LocalReceiving_Id] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)   CONSTRAINT [DF_LocalReceiving_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryId]   VARCHAR (8)   CONSTRAINT [DF_LocalReceiving_FactoryId] DEFAULT ('') NOT NULL,
    [LocalSuppID] VARCHAR (8)   CONSTRAINT [DF_LocalReceiving_LocalSuppID] DEFAULT ('') NOT NULL,
    [IssueDate]   DATE          NOT NULL,
    [Remark]      NVARCHAR (60) CONSTRAINT [DF_LocalReceiving_Remark] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_LocalReceiving_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_LocalReceiving_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    [InvNo]       VARCHAR (25)  CONSTRAINT [DF_LocalReceiving_InvNo] DEFAULT ('') NULL,
    [Status]      VARCHAR (15)  CONSTRAINT [DF_LocalReceiving_Status] DEFAULT ('') NULL,
    CONSTRAINT [PK_LocalReceiving] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Recceiving', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving', @level2type = N'COLUMN', @level2name = N'MDivisionID';

