CREATE TABLE [dbo].[MiscIn] (
    [ID]        VARCHAR (13)   CONSTRAINT [DF_MiscIn_ID] DEFAULT ('') NOT NULL,
    [cDate]     DATE           NULL,
    [Factoryid] VARCHAR (8)    CONSTRAINT [DF_MiscIn_Factoryid] DEFAULT ('') NULL,
    [Exportid]  VARCHAR (13)   CONSTRAINT [DF_MiscIn_Exportid] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (MAX) CONSTRAINT [DF_MiscIn_Remark] DEFAULT ('') NULL,
    [Status]    VARCHAR (15)   CONSTRAINT [DF_MiscIn_Status] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_MiscIn_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_MiscIn_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_MiscIn] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous In-coming', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'cDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'Factoryid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船表號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'Exportid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn', @level2type = N'COLUMN', @level2name = N'EditDate';

