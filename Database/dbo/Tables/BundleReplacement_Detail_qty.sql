CREATE TABLE [dbo].[BundleReplacement_Detail_qty] (
    [SizeCode] VARCHAR (8)  NOT NULL,
    [Qty]      NUMERIC (5)  NOT NULL,
    [Ukey]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [id]       VARCHAR (13) NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail_qty] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

