CREATE TABLE [dbo].[ClogReturn] (
    [ID]             BIGINT       NOT NULL,
    [SCICtnNo]       VARCHAR (16) NOT NULL,
    [ReturnDate]     DATE         NOT NULL,
    [OrderID]        VARCHAR (13) NOT NULL,
    [PackingListID]  VARCHAR (13) NOT NULL,
    [CustCTN]        VARCHAR (30) NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [SunriseUpdated] BIT          CONSTRAINT [DF__ClogRetur__Sunri__21B6055D] DEFAULT ((0)) NOT NULL,
    [GenSongUpdated] BIT          CONSTRAINT [DF__ClogRetur__GenSo__22AA2996] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ClogReturn] PRIMARY KEY CLUSTERED ([ID] ASC)
);

