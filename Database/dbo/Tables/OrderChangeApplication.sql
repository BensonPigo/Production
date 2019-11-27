CREATE TABLE [dbo].[OrderChangeApplication] (
    [ID]       VARCHAR (13) NOT NULL,
    [ReasonID] VARCHAR (5)  NULL,
    [OrderID]  VARCHAR (13) NULL,
    [Status]   VARCHAR (15) NULL,
    [AddName]  VARCHAR (10) NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_OrderChangeApplication] PRIMARY KEY CLUSTERED ([ID] ASC)
);

