CREATE TABLE [dbo].[Local_Issue_Detail] (
    [ID]            VARCHAR (13)    NOT NULL,
    [OrderID]       VARCHAR (13)    NOT NULL,
    [Refno]         VARCHAR (21)    NOT NULL,
    [ThreadColorID] VARCHAR (15)    NOT NULL,
    [Qty]           NUMERIC (10, 2) NOT NULL,
    CONSTRAINT [PK_Local_Issue_Detail] PRIMARY KEY CLUSTERED ([ID] ASC)
);



