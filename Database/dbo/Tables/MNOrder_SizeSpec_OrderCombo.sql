CREATE TABLE [dbo].[MNOrder_SizeSpec_OrderCombo] (
    [Id]           VARCHAR (13) NOT NULL,
    [OrderComboID] VARCHAR (13) NOT NULL,
    [SizeItem]     VARCHAR (3)  NOT NULL,
    [SizeCode]     VARCHAR (8)  NOT NULL,
    [SizeSpec]     VARCHAR (15) NOT NULL,
    [Ukey]         BIGINT       NOT NULL,
    CONSTRAINT [PK_MNOrder_SizeSpec_OrderCombo] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO


CREATE NONCLUSTERED INDEX [OrderComboID_SizeItem_SizeCode] 
ON [dbo].[MNOrder_SizeSpec_OrderCombo] 
([OrderComboID], [SizeItem], [SizeCode])
