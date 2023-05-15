CREATE TABLE [dbo].[ClogGarmentDispose] (
    [SCICtnNo]       VARCHAR (16) NOT NULL,
    [CmdTime]        DATETIME     NULL,
    [Dispose]        BIT          CONSTRAINT [DF_ClogGarmentDispose_Dispose] DEFAULT ((1)) NOT NULL,
    [GenSongUpdated] BIT          CONSTRAINT [DF_ClogGarmentDispose_GenSongUpdated] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ClogGarmentDispose] PRIMARY KEY CLUSTERED ([SCICtnNo] ASC)
);

