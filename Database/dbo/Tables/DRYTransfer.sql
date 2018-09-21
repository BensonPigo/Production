CREATE TABLE [dbo].[DRYTransfer] (
    [ID]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [TransferDate]  DATETIME         NOT NULL,
    [MDivisionID]   VARCHAR (8)  DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) DEFAULT ('') NOT NULL,
    [PackingListID] VARCHAR (13) DEFAULT ('') NOT NULL,
    [CTNStartNo]    VARCHAR (6)  DEFAULT ('') NOT NULL,
    [TransferTo]    VARCHAR (11) DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NOT NULL,
    CONSTRAINT [PK_DRYTransfer] PRIMARY KEY CLUSTERED ([ID] ASC)
);

