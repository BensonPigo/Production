CREATE TABLE [dbo].[Order_ShipPerformance] (
    [ID]                   VARCHAR (13) NOT NULL,
    [Seq]                  VARCHAR (2)  NOT NULL,
    [BookDate]             DATE         NULL,
    [PKManifestCreateDate] DATE         NULL,
    [AddName]              VARCHAR (10) NOT NULL,
    [AddDate]              DATETIME     NOT NULL,
    [EditName]             VARCHAR (10) CONSTRAINT [DF_Order_ShipPerformance_EditName] DEFAULT ('') NOT NULL,
    [EditDate]             DATETIME     NULL,
    CONSTRAINT [PK_Order_ShipPerformance] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq] ASC)
);


GO

