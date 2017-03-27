CREATE TABLE [dbo].[LocalIssue_Detail] (
    [ID]            VARCHAR (13)    NOT NULL,
    [OrderID]       VARCHAR (13)    NOT NULL,
    [Refno]         VARCHAR (21)    NOT NULL,
    [ThreadColorID] VARCHAR (15)    NOT NULL,
    [Qty]           NUMERIC (10, 2) NOT NULL,
    [ukey]          BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_LocalIssue_Detail] PRIMARY KEY CLUSTERED ([ukey] ASC)
);



