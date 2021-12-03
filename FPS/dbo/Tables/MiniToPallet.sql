CREATE TABLE [dbo].[MiniToPallet] (
    [ID]             BIGINT       NOT NULL,
    [SCICtnNo]       VARCHAR (15) NOT NULL,
    [CustCTN]        VARCHAR (30) NOT NULL,
    [ClogLocationId] NVARCHAR(50) NOT NULL,
    [Pallet]         NVARCHAR (50) NULL,
    [Time]           DATETIME     NOT NULL,
    [SCIUpdate]      BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MiniToPallet] PRIMARY KEY CLUSTERED ([ID] ASC)
);

