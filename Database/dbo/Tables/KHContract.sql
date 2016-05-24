CREATE TABLE [dbo].[KHContract] (
    [ID]        VARCHAR (15) NOT NULL,
    [StartDate] DATE         NULL,
    [EndDate]   DATE         NULL,
    [FactoryID] VARCHAR (8)  CONSTRAINT [DF_KHContract_FactoryID] DEFAULT ('') NULL,
    [Status]    VARCHAR (15) CONSTRAINT [DF_KHContract_Status] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_KHContract_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_KHContract_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    CONSTRAINT [PK_KHContract] PRIMARY KEY CLUSTERED ([ID] ASC)
);

