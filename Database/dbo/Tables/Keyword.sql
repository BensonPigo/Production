CREATE TABLE [dbo].[Keyword] (
    [ID]                 VARCHAR (30)   NOT NULL,
    [Description]        NVARCHAR (100) NOT NULL,
    [Junk]               BIT            NULL,
    [Prefix]             NVARCHAR (150) NULL,
    [Fieldname]          NVARCHAR (100) NULL,
    [Postfix]            NVARCHAR (150) NULL,
    [IsSize]             BIT            NULL,
    [IsPatternPanel]     BIT            NULL,
    [AddName]            VARCHAR (10)   NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   NULL,
    [EditDate]           DATETIME       NULL,
    [SubKeyword]         BIT            CONSTRAINT [DF_Keyword_SubKeyword] DEFAULT ((0)) NOT NULL,
    [CannotOperateStock] BIT            CONSTRAINT [DF_Keyword_CannotOperateStock] DEFAULT ((0)) NOT NULL
);



