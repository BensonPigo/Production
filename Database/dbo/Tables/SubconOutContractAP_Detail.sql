CREATE TABLE [dbo].[SubconOutContractAP_Detail]
(
	[ID]                VARCHAR(13)     CONSTRAINT [DF_SubconOutContractAP_Detail_ID]                   DEFAULT ((''))  NOT NULL,
    [ContractNumber]    VARCHAR(50)     CONSTRAINT [DF_SubconOutContractAP_Detail_ContractNumber]       DEFAULT ((''))  NOT NULL, 
    [OrderId]           VARCHAR(13)     CONSTRAINT [DF_SubconOutContractAP_Detail_OrderId]              DEFAULT ((''))  NOT NULL, 
    [ComboType]         VARCHAR(1)      CONSTRAINT [DF_SubconOutContractAP_Detail_ComboType]            DEFAULT ((''))  NOT NULL, 
    [Article]           VARCHAR(8)      CONSTRAINT [DF_SubconOutContractAP_Detail_Article]              DEFAULT ((''))  NOT NULL, 
    [Price]             NUMERIC(16, 4)  CONSTRAINT [DF_SubconOutContractAP_Detail_Price]                DEFAULT ((0))   NOT NULL,
    [Qty]               INT             CONSTRAINT [DF_SubconOutContractAP_Detail_Qty]                  DEFAULT ((0))   NOT NULL,    
    [Amount]            NUMERIC(16, 4)  CONSTRAINT [DF_SubconOutContractAP_Detail_Amount]               DEFAULT ((0))   NOT NULL, 
    [Ukey]              BIGINT NOT NULL IDENTITY(1,1), 
    CONSTRAINT [PK_SubconOutContractAP_Detail] PRIMARY KEY ([Ukey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AP單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'合約號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ContractNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'OrderId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'部位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ComboType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Article'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'價格',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Price'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AP數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總額',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Amount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubconOutContractAP_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'