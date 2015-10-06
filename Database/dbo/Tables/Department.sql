CREATE TABLE [dbo].[Department] (
    [ID]       VARCHAR (8)   CONSTRAINT [DF_Department_ID] DEFAULT ('') NOT NULL,
    [Name]     NVARCHAR (30) CONSTRAINT [DF_Department_Name] DEFAULT ('') NOT NULL,
    [Junk]     BIT           CONSTRAINT [DF_Department_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_Department_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_Department_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME      NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門別資本資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'EditDate';

