﻿CREATE TABLE [dbo].[SewingReason] (
    [Type]        VARCHAR (2)   DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (5)   DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60) DEFAULT ('') NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_SewingReason_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_SewingReason_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_SewingReason_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME      NULL,
    [ForDQSCheck] BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SewingReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'取消',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'Junk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後編輯時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingReason',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'