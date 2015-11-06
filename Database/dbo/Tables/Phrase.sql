CREATE TABLE [dbo].[Phrase] (
    [PhraseTypeName] VARCHAR (50)   CONSTRAINT [DF_Phrase_PhraseTypeName] DEFAULT ('') NOT NULL,
    [Junk]           BIT            CONSTRAINT [DF_Phrase_Junk] DEFAULT ((0)) NULL,
    [Name]           VARCHAR (50)   CONSTRAINT [DF_Phrase_Name] DEFAULT ('') NOT NULL,
    [Seq#]           SMALLINT       CONSTRAINT [DF_Phrase_Seq#] DEFAULT ((0)) NULL,
    [Description]    NVARCHAR (MAX) CONSTRAINT [DF_Phrase_Description] DEFAULT ('') NULL,
    [Module]         VARCHAR (20)   CONSTRAINT [DF_Phrase_Module] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_Phrase_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    [EditName]       VARCHAR (10)   CONSTRAINT [DF_Phrase_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME       NULL,
    CONSTRAINT [PK_Phrase] PRIMARY KEY CLUSTERED ([PhraseTypeName] ASC, [Name] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'片語資料檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'片語分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'PhraseTypeName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'片語簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'Seq#';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'片語', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示模組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'Module';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Phrase', @level2type = N'COLUMN', @level2name = N'EditDate';

