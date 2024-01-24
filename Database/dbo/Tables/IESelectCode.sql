CREATE TABLE [dbo].[IESelectCode] (
    [Type]     VARCHAR (5)   CONSTRAINT [DF_IESelectCode_Type] DEFAULT ('') NOT NULL,
    [ID]       VARCHAR (20)  CONSTRAINT [DF_IESelectCode_ID] DEFAULT ('') NOT NULL,
    [Name]     VARCHAR (100) CONSTRAINT [DF_IESelectCode_Name] DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_IESelectCode_AddName] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_IESelectCode_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME      NULL,
    [Value] NUMERIC(6, 3)   CONSTRAINT [DF_IESelectCode_Value] DEFAULT ((0)) not null,
    CONSTRAINT [PK_IESelectCode] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Value',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IESelectCode',
    @level2type = N'COLUMN',
    @level2name = N'Value'
