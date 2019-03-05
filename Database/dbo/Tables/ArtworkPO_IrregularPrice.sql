CREATE TABLE [dbo].[ArtworkPO_IrregularPrice] (
    [POID]           VARCHAR (13)    NOT NULL,
    [ArtworkTypeID]  VARCHAR (20)    NOT NULL,
    [POPrice]        NUMERIC (16, 4) NULL,
    [StandardPrice]  NUMERIC (16, 4) NULL,
    [SubconReasonID] VARCHAR (5)     NOT NULL,
    [AddDate]        DATETIME        NULL,
    [AddName]        VARCHAR (10)    NULL,
    [EditDate]       DATETIME        NULL,
    [EditName]       VARCHAR (10)    NULL,
    CONSTRAINT [PK_ArtworkPO_IrregularPrice] PRIMARY KEY CLUSTERED ([POID] ASC, [ArtworkTypeID] ASC)
);

