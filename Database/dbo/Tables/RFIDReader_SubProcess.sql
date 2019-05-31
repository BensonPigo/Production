CREATE TABLE [dbo].[RFIDReader_SubProcess] (
    [RFIDReaderID] VARCHAR (24) NOT NULL,
    [ProcessID]    VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_RFIDReader_SubProcess] PRIMARY KEY CLUSTERED ([RFIDReaderID] ASC, [ProcessID] ASC)
);

