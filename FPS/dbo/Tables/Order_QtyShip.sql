CREATE TABLE [dbo].[Order_QtyShip] (
    [id]             VARCHAR (13) NOT NULL,
    [Seq]            VARCHAR (2)  NOT NULL,
    [ShipmodeID]     VARCHAR (10) NULL,
    [BuyerDelivery]  DATE         NULL,
    [Qty]            INT          NULL,
    [EstPulloutDate] DATE         NULL,
    [ReadyDate]      DATE         NULL,
    [CmdTime]        DATETIME     NOT NULL,
    [SunriseUpdated] BIT          DEFAULT ((0)) NOT NULL,
    [GenSongUpdated] BIT          DEFAULT ((0)) NOT NULL,
    [Junk]           BIT          CONSTRAINT [DF_Order_QtyShip_Junk] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_QtyShip] PRIMARY KEY CLUSTERED ([id] ASC, [Seq] ASC)
);

