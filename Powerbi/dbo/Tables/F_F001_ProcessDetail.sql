CREATE TABLE [dbo].[F_F001_ProcessDetail] (
    [ProcessDetailSeq]  NUMERIC (18, 2) NOT NULL,
    [ProcessSeq]        INT             NOT NULL,
    [ProcessDetailSort] INT             NOT NULL,
    [ProcessDetail]     NVARCHAR (100)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ProcessDetailSeq] ASC)
);

