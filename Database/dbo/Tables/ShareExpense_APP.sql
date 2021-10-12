CREATE TABLE [dbo].[ShareExpense_APP] (
    [ShippingAPID]  VARCHAR (13)    NOT NULL,
    [InvNo]         VARCHAR (25)    NOT NULL,
    [PackingListID] VARCHAR (13)    NOT NULL,
    [AirPPID]       VARCHAR (13)    NOT NULL,
    [AccountID]     VARCHAR (8)     NOT NULL,
    [CurrencyID]    VARCHAR (3)     CONSTRAINT [DF_ShareExpense_APP_CurrencyID] DEFAULT ('') NOT NULL,
    [NW]            NUMERIC (10, 3) CONSTRAINT [DF_ShareExpense_APP_NW] DEFAULT ((0)) NOT NULL,
    [RatioFty]      NUMERIC (5, 2)  CONSTRAINT [DF_ShareExpense_APP_RatioFty] DEFAULT ((0)) NOT NULL,
    [AmtFty]        NUMERIC (12, 2) CONSTRAINT [DF_ShareExpense_APP_AmtFty] DEFAULT ((0)) NOT NULL,
    [RatioOther]    NUMERIC (5, 2)  CONSTRAINT [DF_ShareExpense_APP_RatioOther] DEFAULT ((0)) NOT NULL,
    [AmtOther]      NUMERIC (12, 2) CONSTRAINT [DF_ShareExpense_APP_AmtOther] DEFAULT ((0)) NOT NULL,
    [Junk]          BIT             CONSTRAINT [DF_ShareExpense_APP_Junk] DEFAULT ((0)) NOT NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_ShareExpense_APP_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME        NULL,
    [GW] NUMERIC(10, 3) NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_ShareExpense_APP] PRIMARY KEY CLUSTERED ([ShippingAPID] ASC, [InvNo] ASC, [PackingListID] ASC, [AirPPID] ASC, [AccountID] ASC)
);



