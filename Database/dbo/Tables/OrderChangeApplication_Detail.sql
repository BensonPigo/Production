CREATE TABLE [dbo].[OrderChangeApplication_Detail] (
    [Ukey]     BIGINT       NOT NULL,
    [ID]       VARCHAR (13) CONSTRAINT [DF_OrderChangeApplication_Detail_ID] DEFAULT ('') NOT NULL,
    [Seq]      VARCHAR (2)  CONSTRAINT [DF_OrderChangeApplication_Detail_Seq] DEFAULT ('') NOT NULL,
    [Article]  VARCHAR (8)  CONSTRAINT [DF_OrderChangeApplication_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode] VARCHAR (8)  CONSTRAINT [DF_OrderChangeApplication_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]      DECIMAL (6)  CONSTRAINT [DF_OrderChangeApplication_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [OriQty]   DECIMAL (6)  CONSTRAINT [DF_OrderChangeApplication_Detail_OriQty] DEFAULT ((0)) NOT NULL,
    [NowQty]   DECIMAL (6)  CONSTRAINT [DF_OrderChangeApplication_Detail_NowQty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OrderChangeApplication_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO


GO

GO
