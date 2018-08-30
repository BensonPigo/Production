CREATE TABLE [dbo].[Order_Article_PadPrint](
    [ID]          VARCHAR (13) CONSTRAINT [DF_Order_Article_PadPrint_ID] NOT NULL,
    [Article]         VARCHAR (8)   CONSTRAINT [DF_Order_Article_PadPrint_Article]   NOT NULL,
    [ColorID]     VARCHAR (6) CONSTRAINT [DF_Order_Article_PadPrint_ColorID]  NOT NULL,
    [Qty]  INT           NULL,
    CONSTRAINT [PK_Order_Article_PadPrint] PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC,[ColorID]  ASC)
);
