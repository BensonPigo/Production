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




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'請款單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'ShippingAPID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發票號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'InvNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裝箱單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'空運預付單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'AirPPID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'會計科目',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'AccountID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'幣別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'CurrencyID'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'毛重',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'GW'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠責任歸屬的比例',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'RatioFty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠責任歸屬的總額',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'AmtFty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'其他責任歸屬的比例',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'RatioOther'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'其他責任歸屬的總額',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShareExpense_APP',
    @level2type = N'COLUMN',
    @level2name = N'AmtOther'