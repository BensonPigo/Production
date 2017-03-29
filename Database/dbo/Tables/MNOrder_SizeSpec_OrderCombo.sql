CREATE TABLE [dbo].[MNOrder_SizeSpec_OrderCombo] (
    [Id]           VARCHAR (13) NOT NULL,
    [OrderComboID] VARCHAR (13) NOT NULL,
    [SizeItem]     VARCHAR (3)  NOT NULL,
    [SizeCode]     VARCHAR (8)  NOT NULL,
    [SizeSpec]     VARCHAR (15) NOT NULL,
    [Ukey]         BIGINT       NOT NULL
);

