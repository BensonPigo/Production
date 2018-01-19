CREATE TABLE [dbo].[AdjustGMT_Detail] (
    [ID]       VARCHAR (13) NOT NULL,
    [OrderId]  VARCHAR (13) NOT NULL,
    [Article]  VARCHAR (8)  NOT NULL,
    [SizeCode] VARCHAR (8)  NOT NULL,
    [Qty]      INT          NOT NULL,
    [ReasonId] VARCHAR (5)  NULL,
    CONSTRAINT [PK_AdjustGMT_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderId] ASC, [Article] ASC, [SizeCode] ASC)
);

