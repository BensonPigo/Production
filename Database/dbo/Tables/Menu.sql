CREATE TABLE [dbo].[Menu] (
    [PKey]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [MenuNo]     INT           CONSTRAINT [DF_Menu_MenuNo] DEFAULT ((0)) NULL,
    [MenuName]   NVARCHAR (50) CONSTRAINT [DF_Menu_MenuName] DEFAULT ('') NULL,
    [IsSubMenu]  BIT           CONSTRAINT [DF_Menu_IsSubMenu] DEFAULT ((0)) NULL,
    [ForMISOnly] BIT           CONSTRAINT [DF_Menu_ForMISOnly] DEFAULT ((0)) NULL,
    [AddName]    VARCHAR (10)  CONSTRAINT [DF_Menu_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME      NULL,
    [EditName]   VARCHAR (10)  CONSTRAINT [DF_Menu_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME      NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([PKey] ASC)
);

