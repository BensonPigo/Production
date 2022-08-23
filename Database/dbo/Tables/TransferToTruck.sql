CREATE TABLE [dbo].[TransferToTruck] (
    [ID]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [TransferDate]  DATE         NOT NULL,
    [MDivisionID]   VARCHAR (8)  NULL,
    [OrderID]       VARCHAR (13) NULL,
    [PackingListID] VARCHAR (13) NULL,
    [CTNStartNo]    VARCHAR (6)  NULL,
    [TruckNo]       VARCHAR (15) NULL,
    [AddName]       VARCHAR (10) NULL,
    [AddDate]       DATETIME     NULL,
    [SCICtnNo]      VARCHAR (16) DEFAULT ('') NOT NULL
);



