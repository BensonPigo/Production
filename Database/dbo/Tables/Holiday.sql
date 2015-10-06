CREATE TABLE [dbo].[Holiday] (
    [FactoryID]   VARCHAR (8)   CONSTRAINT [DF_Holiday_FactoryID] DEFAULT ('') NOT NULL,
    [HolidayDate] DATE          NOT NULL,
    [Name]        NVARCHAR (20) CONSTRAINT [DF_Holiday_Name] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_Holiday_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_Holiday_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [HolidayDate] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠休假日基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'休假日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'HolidayDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修假名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holiday', @level2type = N'COLUMN', @level2name = N'EditDate';

