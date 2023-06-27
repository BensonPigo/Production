CREATE TABLE [dbo].[Order_Article_PadPrint] (
    [ID]      VARCHAR (13) NOT NULL,
    [Article] VARCHAR (8)  NOT NULL,
    [ColorID] VARCHAR (6)  NOT NULL,
    [Qty]     INT          CONSTRAINT [DF_Order_Article_PadPrint_Qty] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC, [ColorID] ASC)
);


