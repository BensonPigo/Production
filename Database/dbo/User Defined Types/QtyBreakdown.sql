CREATE TYPE [dbo].[QtyBreakdown] AS TABLE (
    [ID]       VARCHAR (13) NOT NULL,
    [Article]  VARCHAR (8)  NOT NULL,
    [SizeCode] VARCHAR (8)  NOT NULL,
    [Qty]      NUMERIC (6)  NULL,
    [OriQty]   NUMERIC (6)  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC, [SizeCode] ASC));

