CREATE TABLE [dbo].[Export_Container] (
    [ID]        VARCHAR (13)   DEFAULT ('') NOT NULL,
    [Seq]       VARCHAR (2)    DEFAULT ('') NOT NULL,
    [Type]      VARCHAR (2)    DEFAULT ('') NOT NULL,
    [Container] VARCHAR (20)   DEFAULT ('') NOT NULL,
    [CartonQty] NUMERIC (5)    DEFAULT ((0)) NOT NULL,
    [WeightKg]  NUMERIC (9, 2) DEFAULT ((0)) NOT NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Export_Container_AddName] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Export_Container_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_Export_Container] PRIMARY KEY CLUSTERED ([ID] ASC, [Container] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'櫃內的件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'CartonQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'櫃號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'Container';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'櫃型 (20,40,HQ)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Container', @level2type = N'COLUMN', @level2name = N'ID';

