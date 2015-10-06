CREATE TABLE [dbo].[CDCode] (
    [ID]          VARCHAR (6)    CONSTRAINT [DF_CDCode_ID] DEFAULT ('') NOT NULL,
    [Junk]        BIT            CONSTRAINT [DF_CDCode_Junk] DEFAULT ((0)) NULL,
    [Description] NVARCHAR (45)  CONSTRAINT [DF_CDCode_Description] DEFAULT ('') NULL,
    [Cpu]         NUMERIC (5, 3) CONSTRAINT [DF_CDCode_Cpu] DEFAULT ((0)) NULL,
    [ComboPcs]    TINYINT        CONSTRAINT [DF_CDCode_ComboPcs] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_CDCode_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_CDCode_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_CDCode] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能代碼定義(CD Code)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能/單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'Cpu';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'ComboPcs';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode', @level2type = N'COLUMN', @level2name = N'EditDate';

