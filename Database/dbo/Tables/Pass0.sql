CREATE TABLE [dbo].[Pass0] (
    [PKey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ID]          VARCHAR (20)   CONSTRAINT [DF_Pass0_ID] DEFAULT ('') NULL,
    [Description] NVARCHAR (100) CONSTRAINT [DF_Pass0_Description] DEFAULT ('') NULL,
    [IsAdmin]     BIT            CONSTRAINT [DF_Pass0_IsAdmin] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_Pass0_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_Pass0_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_Pass0] PRIMARY KEY CLUSTERED ([PKey] ASC)
);

