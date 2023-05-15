CREATE TABLE [dbo].[CompletePullout] (
    [ID]              BIGINT       NOT NULL,
    [SCICtnNo]        VARCHAR (16) NOT NULL,
    [CustCTN]         VARCHAR (30) NOT NULL,
    [Pulloutscanname] VARCHAR (10) NOT NULL,
    [Pulloutscandate] DATETIME     NOT NULL,
    [TruckNo]         VARCHAR (15) NULL,
    [Time]            DATETIME     NOT NULL,
    [SCIUpdate]       BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompletePullout] PRIMARY KEY CLUSTERED ([ID] ASC)
);

