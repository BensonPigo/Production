﻿CREATE TABLE [dbo].[SewingLine] (
    [ID]          VARCHAR (2)    CONSTRAINT [DF_SewingLine_ID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (500) CONSTRAINT [DF_SewingLine_Description] DEFAULT ('') NOT NULL,
    [FactoryID]   VARCHAR (8)    CONSTRAINT [DF_SewingLine_FactoryID] DEFAULT ('') NOT NULL,
    [SewingCell]  VARCHAR (2)    CONSTRAINT [DF_SewingLine_SewingCell] DEFAULT ('') NULL,
    [Sewer]       INT            CONSTRAINT [DF_SewingLine_Sewer] DEFAULT ((0)) NULL,
    [Junk]        BIT            CONSTRAINT [DF_SewingLine_Junk] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_SewingLine_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_SewingLine_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [LineGroup] NVARCHAR(50) NULL CONSTRAINT [DF_SewingLine_LineGroup] DEFAULT (''), 
    CONSTRAINT [PK_SewingLine] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Line Index(車線產線基本檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'SewingCell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預設車縫人數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'Sewer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'EditDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不同區顯示只顯示此Group下的Line', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'LineGroup';
