CREATE TABLE [dbo].[Order_ECMNFailed] (
    [ID]            VARCHAR (13)  NOT NULL,
    [Type]          VARCHAR (2)   NOT NULL,
    [KPIFailed]     VARCHAR (16)  NOT NULL,
    [KPIDate]       DATE          NULL,
    [FailedComment] VARCHAR (100) CONSTRAINT [DF_Order_ECMNFailed_FailedComment] DEFAULT ('') NOT NULL,
    [ExpectApvDate] DATE          NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Order_ECMNFailed_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_Order_ECMNFailed_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME      NULL,
    CONSTRAINT [PK_Order_ECMNFailed] PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC)
);



