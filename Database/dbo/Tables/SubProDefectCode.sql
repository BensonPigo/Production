﻿CREATE TABLE [dbo].[SubProDefectCode] (
    [SubProcessID] VARCHAR (15)   NOT NULL,
    [DefectCode]   VARCHAR (50)   NOT NULL,
    [Junk]         BIT            CONSTRAINT [DF_SubProDefectCode_Junk] DEFAULT ((0)) NOT NULL,
    [Description]  NVARCHAR (500) CONSTRAINT [DF_SubProDefectCode_Description] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME       NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_SubProDefectCode_AddName] DEFAULT ('') NOT NULL,
    [EditDate]     DATETIME       NULL,
    [Editname]     VARCHAR (10)   CONSTRAINT [DF_SubProDefectCode_Editname] DEFAULT ('') NOT NULL,
    [LocalDesc]    NVARCHAR (100) CONSTRAINT [DF_SubProDefectCode_LocalDesc] DEFAULT ('') NOT NULL,
    [IsHidden]     BIT            CONSTRAINT [DF_SubProDefectCode_IsHidden] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SubProDefectCode] PRIMARY KEY CLUSTERED ([SubProcessID] ASC, [DefectCode] ASC)
);






GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠自行決定要隱藏',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProDefectCode',
    @level2type = N'COLUMN',
    @level2name = N'IsHidden'