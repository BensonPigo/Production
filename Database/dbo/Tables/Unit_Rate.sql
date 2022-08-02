CREATE TABLE [dbo].[Unit_Rate] (
    [UnitFrom]    VARCHAR (8)      CONSTRAINT [DF_Unit_Rate_UnitFrom] DEFAULT ('') NOT NULL,
    [UnitTo]      VARCHAR (8)      CONSTRAINT [DF_Unit_Rate_UnitTo] DEFAULT ('') NOT NULL,
    [Rate]        VARCHAR (22)     CONSTRAINT [DF_Unit_Rate_Rate] DEFAULT ('') NULL,
    [RateValue]   NUMERIC (28, 18) CONSTRAINT [DF_Unit_Rate_RateValue] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)     CONSTRAINT [DF_Unit_Rate_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME         NULL,
    [EditName]    VARCHAR (10)     CONSTRAINT [DF_Unit_Rate_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME         NULL,
    [Numerator]   NUMERIC (28, 18) DEFAULT ((0)) NOT NULL,
    [Denominator] NUMERIC (28, 18) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Unit_Rate] PRIMARY KEY CLUSTERED ([UnitFrom] ASC, [UnitTo] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位換算檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'UnitFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換算單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'UnitTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換算比例(公式)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unit_Rate', @level2type = N'COLUMN', @level2name = N'EditDate';

