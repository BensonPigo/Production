CREATE TABLE [dbo].[LocalItem_ThreadColorPrice] (
    [Refno]         VARCHAR (21)    NOT NULL,
    [ThreadColorID] VARCHAR (15)    NOT NULL,
    [Price]         NUMERIC (12, 4) CONSTRAINT [DF_LocalItem_ThreadColorPrice_Price] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_LocalItem_ThreadColorPrice_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_LocalItem_ThreadColorPrice_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME        NULL,
    CONSTRAINT [PK_LocalItem_ThreadColorPrice] PRIMARY KEY CLUSTERED ([Refno] ASC, [ThreadColorID] ASC)
);

