CREATE TABLE [dbo].[Orders] (
    [id]              VARCHAR (13) NOT NULL,
    [BrandID]         VARCHAR (8)  NULL,
    [ProgramID]       VARCHAR (12) NULL,
    [StyleID]         VARCHAR (15) NULL,
    [SeasonID]        VARCHAR (10) NULL,
    [ProjectID]       VARCHAR (5)  NULL,
    [Category]        VARCHAR (1)  NULL,
    [OrderTypeID]     VARCHAR (20) NULL,
    [Dest]            VARCHAR (2)  NULL,
    [CustCDID]        VARCHAR (16) NULL,
    [StyleUnit]       VARCHAR (8)  NULL,
    [SetQty]          INT          NOT NULL,
    [Location]        VARCHAR (7)  NULL,
    [PulloutComplete] BIT          NULL,
    [Junk]            BIT          CONSTRAINT [DF__Orders__Junk__117F9D94] DEFAULT ((0)) NULL,
    [CmdTime]         DATETIME     NOT NULL,
    [SunriseUpdated]  INT          CONSTRAINT [DF__Orders__SunriseU__1273C1CD] DEFAULT ((0)) NOT NULL,
    [GenSongUpdated]  BIT          CONSTRAINT [DF__Orders__GenSongU__1367E606] DEFAULT ((0)) NOT NULL,
    [CustPONo]        VARCHAR (30) NULL,
    [POID]            VARCHAR (13) CONSTRAINT [DF_Orders_POID] DEFAULT ('') NULL,
    [DestCountry]     VARCHAR (30) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨地國家全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Orders', @level2type = N'COLUMN', @level2name = N'DestCountry';

