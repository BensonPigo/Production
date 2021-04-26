CREATE TABLE [dbo].[NewCDCode] (
    [Classifty]  NVARCHAR (15)  DEFAULT ('') NOT NULL,
    [TypeName]   NVARCHAR (50)  DEFAULT ('') NULL,
    [ID]         VARCHAR (1)    DEFAULT ('') NOT NULL,
    [Placket]    NVARCHAR (30)  DEFAULT ('') NULL,
    [Definition] VARCHAR (100)  DEFAULT ('') NULL,
    [CPU]        NUMERIC (5, 3) DEFAULT ((0)) NULL,
    [ComboPcs]   NUMERIC (1)    DEFAULT ((0)) NULL,
    [Remark]     NVARCHAR (200) DEFAULT ('') NULL,
    [Junk]       BIT            DEFAULT ((0)) NULL,
    [AddName]    VARCHAR (10)   DEFAULT ('') NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   DEFAULT ('') NULL,
    [EditDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Classifty] ASC, [ID] ASC)
);

