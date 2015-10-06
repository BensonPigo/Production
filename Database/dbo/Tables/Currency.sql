CREATE TABLE [dbo].[Currency] (
    [ID]       VARCHAR (3)    CONSTRAINT [DF_Currency_ID] DEFAULT ('') NOT NULL,
    [StdRate]  NUMERIC (9, 4) CONSTRAINT [DF_Currency_StdRate] DEFAULT ((0)) NOT NULL,
    [NameCH]   NVARCHAR (8)   CONSTRAINT [DF_Currency_NameCH] DEFAULT ('') NULL,
    [NameEN]   NVARCHAR (30)  CONSTRAINT [DF_Currency_NameEN] DEFAULT ('') NULL,
    [Junk]     BIT            CONSTRAINT [DF_Currency_Junk] DEFAULT ((0)) NULL,
    [Exact]    TINYINT        CONSTRAINT [DF_Currency_Exact] DEFAULT ((0)) NULL,
    [Symbol]   VARCHAR (3)    CONSTRAINT [DF_Currency_Symbol] DEFAULT ('') NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_Currency_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_Currency_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標準匯率基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標準匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'StdRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文名稱(Chinese Name)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作廢碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別精確度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'Exact';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'符號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'Symbol';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Currency', @level2type = N'COLUMN', @level2name = N'EditDate';

