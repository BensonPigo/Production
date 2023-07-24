CREATE TABLE [dbo].[Keyword] (
    [ID]                 VARCHAR (30)   NOT NULL,
    [Description]        NVARCHAR (100) NOT NULL,
    [Junk]               BIT            CONSTRAINT [DF_Keyword_Junk] DEFAULT ((0)) NOT NULL,
    [Prefix]             NVARCHAR (150) CONSTRAINT [DF_Keyword_Prefix] DEFAULT ('') NOT NULL,
    [Fieldname]          NVARCHAR (100) CONSTRAINT [DF_Keyword_Fieldname] DEFAULT ('') NOT NULL,
    [Postfix]            NVARCHAR (150) CONSTRAINT [DF_Keyword_Postfix] DEFAULT ('') NOT NULL,
    [IsSize]             BIT            CONSTRAINT [DF_Keyword_IsSize] DEFAULT ((0)) NOT NULL,
    [IsPatternPanel]     BIT            CONSTRAINT [DF_Keyword_IsPatternPanel] DEFAULT ((0)) NOT NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_Keyword_AddName] DEFAULT ('') NOT NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   CONSTRAINT [DF_Keyword_EditName] DEFAULT ('') NOT NULL,
    [EditDate]           DATETIME       NULL,
    [SubKeyword]         BIT            CONSTRAINT [DF_Keyword_SubKeyword] DEFAULT ((0)) NOT NULL,
    [CannotOperateStock] BIT            CONSTRAINT [DF_Keyword_CannotOperateStock] DEFAULT ((0)) NOT NULL
);





