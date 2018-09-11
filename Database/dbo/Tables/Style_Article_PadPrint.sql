CREATE TABLE [dbo].[Style_Article_PadPrint](
    [StyleUkey]          VARCHAR (13) CONSTRAINT [DF_Style_Article_PadPrint_StyleUkey]  NOT NULL,
    [Article]         VARCHAR (8)     CONSTRAINT [DF_Style_Article_PadPrint_Article]  NOT NULL,
    [ColorID]     VARCHAR (6)  CONSTRAINT [DF_Style_Article_PadPrint_ColorID]  NOT NULL,
    [Qty]  INT          CONSTRAINT [DF_Style_Article_PadPrint_Qty]  NULL,
    CONSTRAINT [PK_Style_Article_PadPrint] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Article] ASC,[ColorID]  ASC)
);