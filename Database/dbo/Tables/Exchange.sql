CREATE TABLE [dbo].[Exchange] (
    [ExchangeTypeID] VARCHAR (2)    CONSTRAINT [DF_Exchange_ExchangeTypeID] DEFAULT ('') NOT NULL,
    [CurrencyFrom]   VARCHAR (3)    CONSTRAINT [DF_Exchange_CurrencyFrom] DEFAULT ('') NOT NULL,
    [CurrencyTo]     VARCHAR (3)    CONSTRAINT [DF_Exchange_CurrencyTo] DEFAULT ('') NOT NULL,
    [DateStart]      DATE           NOT NULL,
    [DateEnd]        DATE           NULL,
    [Rate]           NUMERIC (9, 4) CONSTRAINT [DF_Exchange_Rate] DEFAULT ((0)) NULL,
    [Remark]         NVARCHAR (300) CONSTRAINT [DF_Exchange_Remark] DEFAULT ('') NULL,
    [Junk]           BIT            CONSTRAINT [DF_Exchange_Junk] DEFAULT ((0)) NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_Exchange_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    [EditName]       VARCHAR (10)   CONSTRAINT [DF_Exchange_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME       NULL,
    CONSTRAINT [PK_Exchange] PRIMARY KEY CLUSTERED ([ExchangeTypeID] ASC, [CurrencyFrom] ASC, [CurrencyTo] ASC, [DateStart] ASC)
);

