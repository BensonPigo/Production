CREATE TABLE [dbo].[MyGridUDF1] (
    [PKey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [FormName]    VARCHAR (80)   CONSTRAINT [DF_MyGridUDF1_FormName] DEFAULT ('') NULL,
    [Description] NVARCHAR (100) CONSTRAINT [DF_MyGridUDF1_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_MyGridUDF1_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_MyGridUDF1_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_MyGridUDF1] PRIMARY KEY CLUSTERED ([PKey] ASC)
);

