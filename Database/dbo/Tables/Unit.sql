CREATE TABLE [dbo].[Unit] (
    [ID]            VARCHAR (8)     CONSTRAINT [DF_Unit_ID] DEFAULT ('') NOT NULL,
    [PriceRate]     NUMERIC (12, 4) CONSTRAINT [DF_Unit_PriceRate] DEFAULT ((0)) NOT NULL,
    [Round]         TINYINT         CONSTRAINT [DF_Unit_Round] DEFAULT ((0)) NOT NULL,
    [Description]   NVARCHAR (120)  CONSTRAINT [DF_Unit_Description] DEFAULT ('') NOT NULL,
    [ExtensionUnit] VARCHAR (8)     CONSTRAINT [DF_Unit_ExtensionUnit] DEFAULT ('') NOT NULL,
    [Junk]          BIT             CONSTRAINT [DF_Unit_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_Unit_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_Unit_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME        NULL,
    [MiAdidasRound] DECIMAL (2)     CONSTRAINT [DF_Unit_MiAdidasRound] DEFAULT ((0)) NOT NULL,
    [RoundStep]     DECIMAL (4, 2)  CONSTRAINT [DF_Unit_RoundStep] DEFAULT ((0)) NOT NULL,
    [StockRound]    NUMERIC (2)     CONSTRAINT [DF_Unit_StockRound] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位設定檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價倍率( Default=1 )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'PriceRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進位數(採買計算使用)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'Round';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉換的發料單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'ExtensionUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit', @level2type = N'COLUMN', @level2name = N'EditDate';

