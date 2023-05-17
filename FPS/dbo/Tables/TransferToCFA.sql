CREATE TABLE [dbo].[TransferToCFA] (
    [ID]             BIGINT       NOT NULL,
    [SCICtnNo]       VARCHAR (16) NOT NULL,
    [TransferDate]   DATE         NOT NULL,
    [OrderID]        VARCHAR (13) NOT NULL,
    [PackingListID]  VARCHAR (13) NOT NULL,
    [CustCTN]        VARCHAR (30) NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [SunriseUpdated] INT          DEFAULT ((0)) NOT NULL,
    [GenSongUpdated] INT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TransferToCFA] PRIMARY KEY CLUSTERED ([ID] ASC)
);

