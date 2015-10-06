CREATE TABLE [dbo].[ThreadType] (
    [ID]       VARCHAR (15) CONSTRAINT [DF_ThreadType_ID] DEFAULT ('') NOT NULL,
    [Junk]     BIT          CONSTRAINT [DF_ThreadType_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_ThreadType_AddName] DEFAULT ('') NULL,
    [AddDaate] DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_ThreadType_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_ThreadType] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType', @level2type = N'COLUMN', @level2name = N'AddDaate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadType', @level2type = N'COLUMN', @level2name = N'EditDate';

