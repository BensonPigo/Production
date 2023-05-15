CREATE TABLE [dbo].[Order_SizeCode] (
    [Id]             VARCHAR (13) CONSTRAINT [DF_Order_SizeCode_Id] DEFAULT ('') NOT NULL,
    [Seq]            VARCHAR (2)  CONSTRAINT [DF_Order_SizeCode_Seq] DEFAULT ('') NULL,
    [SizeGroup]      VARCHAR (1)  CONSTRAINT [DF_Order_SizeCode_SizeGroup] DEFAULT ('') NULL,
    [SizeCode]       VARCHAR (8)  CONSTRAINT [DF_Order_SizeCode_SizeCode] DEFAULT ('') NOT NULL,
    [Ukey]           BIGINT       DEFAULT ((0)) NOT NULL,
    [Junk]           BIT          DEFAULT ((0)) NOT NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [SunriseUpdated] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_SizeCode] PRIMARY KEY CLUSTERED ([Id] ASC, [Ukey] ASC, [SizeCode] ASC)
);

