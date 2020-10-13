CREATE TABLE [dbo].[KHImportDeclaration] (
    [ID]         VARCHAR (13)  NOT NULL,
    [BLNo]       VARCHAR (20)  CONSTRAINT [DF_KHImportDeclaration_BLNo] DEFAULT ('') NULL,
    [Cdate]      DATE          NULL,
    [DeclareNo]  VARCHAR (25)  CONSTRAINT [DF_KHImportDeclaration_DeclareNo] DEFAULT ('') NULL,
    [Status]     VARCHAR (15)  CONSTRAINT [DF_KHImportDeclaration_Status] DEFAULT ('') NULL,
    [ImportPort] VARCHAR (20)  CONSTRAINT [DF_Table_1_ImportPort] DEFAULT ('') NULL,
    [Remark]     VARCHAR (200) CONSTRAINT [DF_KHImportDeclaration_Remark] DEFAULT ('') NULL,
    [AddName]    VARCHAR (10)  CONSTRAINT [DF_KHImportDeclaration_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME      NULL,
    [EditName]   VARCHAR (10)  CONSTRAINT [DF_KHImportDeclaration_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME      NULL,
    CONSTRAINT [PK_KHImportDeclaration] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'ImportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口報關編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'DeclareNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口報關日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'Cdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'BLNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration', @level2type = N'COLUMN', @level2name = N'ID';

