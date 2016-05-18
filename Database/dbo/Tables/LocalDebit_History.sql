CREATE TABLE [dbo].[LocalDebit_History] (
    [HisType]  VARCHAR (10)   CONSTRAINT [DF_LocalDebit_History_Type] DEFAULT ('') NULL,
    [Id]       VARCHAR (13)   CONSTRAINT [DF_LocalDebit_History_Id] DEFAULT ('') NULL,
    [OldValue] VARCHAR (20)   CONSTRAINT [DF_LocalDebit_History_OldValue] DEFAULT ('') NULL,
    [NewValue] VARCHAR (20)   CONSTRAINT [DF_LocalDebit_History_NewValue] DEFAULT ('') NULL,
    [ReasonId] VARCHAR (5)    CONSTRAINT [DF_LocalDebit_History_ReasonId] DEFAULT ('') NULL,
    [Remark]   NVARCHAR (MAX) CONSTRAINT [DF_LocalDebit_History_Remark] DEFAULT ('') NULL,
    [Ukey]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_LocalDebit_History_EditName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    CONSTRAINT [PK_LocalDebit_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LocalDebit HISTORY', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'舊值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'OldValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'NewValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'ReasonId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'Ukey';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'HisType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalDebit_History', @level2type = N'COLUMN', @level2name = N'AddDate';

