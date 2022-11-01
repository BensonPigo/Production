﻿CREATE TABLE [dbo].[SubProMachine] (
    [ID]           VARCHAR (50)   NOT NULL,
    [FactoryID]    VARCHAR (8)    NOT NULL,
    [SubProcessID] VARCHAR (15)   NOT NULL,
    [Junk]         BIT            CONSTRAINT [DF_SubProMachine_Junk] DEFAULT ((0)) NOT NULL,
    [Description]  NVARCHAR (500) NULL,
    [AddDate]      DATETIME       NULL,
    [AddName]      VARCHAR (10)   NULL,
    [EditDate]     DATETIME       NULL,
    [Editname]     VARCHAR (10)   NULL,
    [Serial#]      VARCHAR(50)    CONSTRAINT [DF_SubProMachinel_Serial#] DEFAULT ('') NOT NULL , 
    CONSTRAINT [PK_SubProMachine] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC, [SubProcessID] ASC)
);

