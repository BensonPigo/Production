CREATE TABLE [dbo].[KHGoodsHSCode] (
    [ID]               BIGINT       IDENTITY (1, 1) NOT NULL,
    [GoodsDescription] VARCHAR (50) NOT NULL,
    [HSCode]           VARCHAR (8)  CONSTRAINT [DF_KHGoodsHSCode_HSCode] DEFAULT ('') NULL,
    [Category]         VARCHAR (10) CONSTRAINT [DF_KHGoodsHSCode_Category] DEFAULT ('') NULL,
    [NLCode]           VARCHAR (5)  CONSTRAINT [DF_KHGoodsHSCode_NLCode] DEFAULT ('') NULL,
    [Junk]             BIT          CONSTRAINT [DF_KHGoodsHSCode_Junk] DEFAULT ((0)) NULL,
    [AddName]          VARCHAR (10) CONSTRAINT [DF_KHGoodsHSCode_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME     NULL,
    [EditName]         VARCHAR (10) CONSTRAINT [DF_KHGoodsHSCode_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME     NULL,
    CONSTRAINT [PK_KHGoodsHSCode] PRIMARY KEY CLUSTERED ([GoodsDescription] ASC)
);

