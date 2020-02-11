CREATE TABLE [dbo].[OrderChangeApplication_Detail] (
    [Ukey]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [ID]       VARCHAR (13) NULL,
    [Seq]      VARCHAR (2)  NULL,
    [Article]  VARCHAR (8)  NULL,
    [SizeCode] VARCHAR (8)  NULL,
    [Qty]      NUMERIC (6)  NULL,
    [OriQty]   NUMERIC (6)  NULL,
    CONSTRAINT [PK_OrderChangeApplication_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

