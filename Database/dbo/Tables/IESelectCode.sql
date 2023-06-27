﻿CREATE TABLE [dbo].[IESelectCode] (
    [Type]     VARCHAR (5)   CONSTRAINT [DF_IESelectCode_Type] DEFAULT ('') NOT NULL,
    [ID]       VARCHAR (20)  CONSTRAINT [DF_IESelectCode_ID] DEFAULT ('') NOT NULL,
    [Name]     VARCHAR (100) CONSTRAINT [DF_IESelectCode_Name] DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_IESelectCode_AddName] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_IESelectCode_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME      NULL,
    CONSTRAINT [PK_IESelectCode] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);



