CREATE TABLE [dbo].[CFANeedInsp] (
    [SCICtnNo]       VARCHAR (16) NOT NULL,
    [CmdTime]        DATETIME     NULL,
    [GenSongUpdated] BIT          CONSTRAINT [DF_CFANeedInsp_GenSongUpdated] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CFANeedInsp] PRIMARY KEY CLUSTERED ([SCICtnNo] ASC)
);

