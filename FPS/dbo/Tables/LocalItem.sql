CREATE TABLE [dbo].[LocalItem] (
    [Refno]          VARCHAR (21) NOT NULL,
    [UnPack]         BIT          DEFAULT ((0)) NOT NULL,
    [Junk]           BIT          DEFAULT ((0)) NOT NULL,
    [CmdTime]        DATETIME     NULL,
    [SunriseUpdated] BIT          DEFAULT ((0)) NOT NULL,
    [GenSongUpdated] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LocalItem] PRIMARY KEY CLUSTERED ([Refno] ASC)
);

