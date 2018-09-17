CREATE TABLE [dbo].[SubconOutContract_Detail] (
    [SubConOutFty]    VARCHAR (8)     NOT NULL,
    [ContractNumber]  VARCHAR (50)    NOT NULL,
    [OrderId]         VARCHAR (13)    NOT NULL,
    [ComboType]       VARCHAR (1)     NOT NULL,
    [Article]         VARCHAR (8)     NOT NULL,
    [OutputQty]       INT             CONSTRAINT [DF_SubconOutContract_Detail_OutputQty] DEFAULT ((0)) NULL,
    [UnitPrice]       NUMERIC (16, 4) CONSTRAINT [DF_SubconOutContract_Detail_UnitPrice] DEFAULT ((0)) NULL,
    [LocalCurrencyID] VARCHAR (3)     NULL,
    [LocalUnitPrice]  NUMERIC (16, 4) NULL,
    [Vat]             NUMERIC (11, 2) NULL,
    [KpiRate]         NUMERIC (3, 2)  NULL,
    CONSTRAINT [PK_SubconOutContract_Detail] PRIMARY KEY CLUSTERED ([SubConOutFty] ASC, [ContractNumber] ASC, [OrderId] ASC, [ComboType] ASC, [Article] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'增值稅', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubconOutContract_Detail', @level2type = N'COLUMN', @level2name = N'Vat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價(不含增值稅)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubconOutContract_Detail', @level2type = N'COLUMN', @level2name = N'LocalUnitPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本地幣值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubconOutContract_Detail', @level2type = N'COLUMN', @level2name = N'LocalCurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KPI率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubconOutContract_Detail', @level2type = N'COLUMN', @level2name = N'KpiRate';

