CREATE TABLE [dbo].[PadPrint] (
    [Ukey]       BIGINT          NOT NULL,
    [Refno]      VARCHAR (36)    CONSTRAINT [DF_PadPrint_Refno] DEFAULT ('') NULL,
    [BrandID]    VARCHAR (8)     CONSTRAINT [DF_PadPrint_BrandID] DEFAULT ('') NOT NULL,
    [Category]   VARCHAR (1)     CONSTRAINT [DF_PadPrint_Category] DEFAULT ('') NOT NULL,
    [SuppID]     VARCHAR (6)     CONSTRAINT [DF_PadPrint_SuppID] DEFAULT ('') NOT NULL,
    [CurrencyID] VARCHAR (3)     CONSTRAINT [DF_PadPrint_CurrencyID] DEFAULT ('') NOT NULL,
    [Junk]       BIT             CONSTRAINT [DF_PadPrint_Junk] DEFAULT ((0)) NOT NULL,
    [Remark]     NVARCHAR (1000) CONSTRAINT [DF_PadPrint_Remark] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10)    CONSTRAINT [DF_PadPrint_AddName] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME        NULL,
    [EditName]   VARCHAR (10)    CONSTRAINT [DF_PadPrint_EditName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME        NULL,
    CONSTRAINT [PK_PadPrint_Ukey] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

