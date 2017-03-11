CREATE TABLE [dbo].[Local_Issue_Detail] (
    [ID]       VARCHAR (13)    NOT NULL,
    [Poid]     VARCHAR (13)    NOT NULL,
    [SCIRefno] VARCHAR (26)    NOT NULL,
    [Qty]      NUMERIC (10, 2) NULL,
    CONSTRAINT [PK_Local_Issue_Detail] PRIMARY KEY CLUSTERED ([ID] ASC)
);

