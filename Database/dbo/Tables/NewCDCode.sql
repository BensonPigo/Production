CREATE TABLE [dbo].[NewCDCode] (
    [Classifty]  NVARCHAR (15)  DEFAULT ('') NOT NULL,
    [TypeName]   NVARCHAR (50)  CONSTRAINT [DF_NewCDCode_TypeName] DEFAULT ('') NOT NULL,
    [ID]         VARCHAR (1)    DEFAULT ('') NOT NULL,
    [Placket]    NVARCHAR (30)  CONSTRAINT [DF_NewCDCode_Placket] DEFAULT ('') NOT NULL,
    [Definition] VARCHAR (100)  CONSTRAINT [DF_NewCDCode_Definition] DEFAULT ('') NOT NULL,
    [CPU]        DECIMAL (5, 3) CONSTRAINT [DF_NewCDCode_CPU] DEFAULT ((0)) NOT NULL,
    [ComboPcs]   DECIMAL (1)    CONSTRAINT [DF_NewCDCode_ComboPcs] DEFAULT ((0)) NOT NULL,
    [Remark]     NVARCHAR (200) CONSTRAINT [DF_NewCDCode_Remark] DEFAULT ('') NOT NULL,
    [Junk]       BIT            CONSTRAINT [DF_NewCDCode_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_NewCDCode_AddName] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_NewCDCode_EditName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Classifty] ASC, [ID] ASC)
);



