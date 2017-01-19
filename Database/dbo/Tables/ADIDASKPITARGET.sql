CREATE TABLE [dbo].[ADIDASKPITARGET] (
    [KPIItem]     NUMERIC (3)    CONSTRAINT [DF_ADIDASKPITARGET_KPIItem] DEFAULT ((0)) NOT NULL,
    [XlsColumn]   NUMERIC (3)    CONSTRAINT [DF_ADIDASKPITARGET_XlsColumn] DEFAULT ((0)) NULL,
    [Description] VARCHAR (50)   CONSTRAINT [DF_ADIDASKPITARGET_Description] DEFAULT ('') NULL,
    [Target]      NUMERIC (5, 2) CONSTRAINT [DF_ADIDASKPITARGET_Target] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_ADIDASKPITARGET_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_ADIDASKPITARGET_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_ADIDASKPITARGET] PRIMARY KEY CLUSTERED ([KPIItem] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'Target';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Desc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'XLS Column', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'XlsColumn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Kpi Item', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET', @level2type = N'COLUMN', @level2name = N'KPIItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ADIDAS KPI TARGET', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASKPITARGET';

