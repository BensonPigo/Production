CREATE TABLE [dbo].[Po_Supp_Detail] (
    [ID]          VARCHAR (13)  NOT NULL,
    [SEQ1]        VARCHAR (3)   NOT NULL,
    [SEQ2]        VARCHAR (2)   NOT NULL,
    [Refno]       VARCHAR (36)  NOT NULL,
    [SCIRefno]    VARCHAR (30)  NOT NULL,
    [NewRefno]    VARCHAR (36)  DEFAULT ('') NOT NULL,
    [NewSCIRefno] VARCHAR (30)  DEFAULT ('') NOT NULL,
    [Color]       VARCHAR (300) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Po_Supp_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC)
);

