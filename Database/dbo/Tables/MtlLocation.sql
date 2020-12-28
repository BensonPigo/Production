CREATE TABLE [dbo].[MtlLocation] (
    [ID]          VARCHAR (20)  CONSTRAINT [DF_MtlLocation_ID] DEFAULT ('') NOT NULL,
    [StockType]   VARCHAR (10)  CONSTRAINT [DF_MtlLocation_StockType] DEFAULT ('') NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_MtlLocation_Junk] DEFAULT ((0)) NOT NULL,
    [Description] NVARCHAR (40) CONSTRAINT [DF_MtlLocation_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_MtlLocation_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_MtlLocation_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    [IsWMS]       BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MtlLocation] PRIMARY KEY CLUSTERED ([ID] ASC, [StockType] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Warehouse location Basic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉位類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlLocation', @level2type = N'COLUMN', @level2name = N'EditDate';

