CREATE TABLE [dbo].[Style_BOA_KeyWord] (
    [StyleUkey]     BIGINT        CONSTRAINT [DF_Style_BOA_KeyWord_StyleUkey] DEFAULT ((0)) NULL,
    [Style_BOAUkey] BIGINT        CONSTRAINT [DF_Style_BOA_KeyWord_Style_BOAUkey] DEFAULT ((0)) NOT NULL,
    [SEQ]           VARCHAR (2)   CONSTRAINT [DF_Style_BOA_KeyWord_SEQ] DEFAULT ('') NOT NULL,
    [Prefix]        NVARCHAR (60) CONSTRAINT [DF_Style_BOA_KeyWord_Prefix] DEFAULT ('') NULL,
    [KeyWordID]     VARCHAR (30)  CONSTRAINT [DF_Style_BOA_KeyWord_KeyWordID] DEFAULT ('') NULL,
    [Postfix]       NVARCHAR (60) CONSTRAINT [DF_Style_BOA_KeyWord_Postfix] DEFAULT ('') NULL,
    [Code]          VARCHAR (3)   CONSTRAINT [DF_Style_BOA_KeyWord_Code] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Style_BOA_KeyWord_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_Style_BOA_KeyWord_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    CONSTRAINT [PK_Style_BOA_KeyWord] PRIMARY KEY CLUSTERED ([Style_BOAUkey] ASC, [SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill of Accessories- Key word', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOA的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Style_BOAUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'前置固定的顯示字樣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Prefix';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'對應欄位名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'KeyWordID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結束固定的顯示字樣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Postfix';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸的項目或部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'EditDate';

