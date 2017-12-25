﻿CREATE TABLE [dbo].[SewingMachineTemplate] (
    [ID]          VARCHAR (20)   NOT NULL,
    [Description] NVARCHAR (100) NULL,
    [Junk]        BIT            NULL,
    [AddName]     VARCHAR (10)   NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_SewingMachineTemplate] PRIMARY KEY CLUSTERED ([ID] ASC)
);

