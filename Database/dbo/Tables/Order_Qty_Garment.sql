CREATE TABLE [dbo].[Order_Qty_Garment] (
    [ID]          VARCHAR (13) NOT NULL,
    [OrderIDFrom] VARCHAR (13) NOT NULL,
    [Article]     VARCHAR (8)  NOT NULL,
    [SizeCode]    VARCHAR (8)  NOT NULL,
    [Qty]         NUMERIC (6)  NOT NULL,
    [AddName]     VARCHAR (13) NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (13) NULL,
    [EditDate]    DATETIME     NULL,
    [Junk]        BIT          NULL,
    CONSTRAINT [PK_Order_Qty_Garment] PRIMARY KEY CLUSTERED ([Article] ASC, [ID] ASC, [OrderIDFrom] ASC, [SizeCode] ASC)
);



