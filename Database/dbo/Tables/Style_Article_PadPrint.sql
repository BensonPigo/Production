CREATE TABLE [dbo].[Style_Article_PadPrint] (
    [StyleUkey] BIGINT      NOT NULL,
    [Article]   VARCHAR (8) NOT NULL,
    [ColorID]   VARCHAR (6) NOT NULL,
    [Qty]       INT         CONSTRAINT [DF_Style_Article_PadPrint_Qty] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Article] ASC, [ColorID] ASC)
);

