CREATE TABLE [dbo].[Order_Qty_Garment] (
    [ID]          VARCHAR (13) NOT NULL,
    [OrderIDFrom] VARCHAR (13) NOT NULL,
    [Article]     VARCHAR (8)  NOT NULL,
    [SizeCode]    VARCHAR (8)  NOT NULL,
    [Qty]         NUMERIC (6)  NOT NULL,
    [AddName]     VARCHAR (13) CONSTRAINT [DF_Order_Qty_Garment_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (13) CONSTRAINT [DF_Order_Qty_Garment_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME     NULL,
    [Junk]        BIT          CONSTRAINT [DF_Order_Qty_Garment_Junk] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_Qty_Garment] PRIMARY KEY CLUSTERED ([Article] ASC, [ID] ASC, [OrderIDFrom] ASC, [SizeCode] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IDX_Order_Qty_Garment_OrderIDFrom]
    ON [dbo].[Order_Qty_Garment]([OrderIDFrom] ASC, [Junk] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_Order_Qty_Garment_ID]
    ON [dbo].[Order_Qty_Garment]([ID] ASC, [Junk] ASC);

