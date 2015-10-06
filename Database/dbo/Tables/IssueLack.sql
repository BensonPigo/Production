﻿CREATE TABLE [dbo].[IssueLack] (
    [Id]         VARCHAR (13)  CONSTRAINT [DF_IssueLack_Id] DEFAULT ('') NOT NULL,
    [Type]       VARCHAR (1)   CONSTRAINT [DF_IssueLack_Type] DEFAULT ('') NULL,
    [IssueDate]  DATE          NULL,
    [FactoryId]  VARCHAR (8)   CONSTRAINT [DF_IssueLack_FactoryId] DEFAULT ('') NULL,
    [Status]     VARCHAR (15)  CONSTRAINT [DF_IssueLack_Status] DEFAULT ('') NULL,
    [RequestID]  VARCHAR (13)  CONSTRAINT [DF_IssueLack_RequestID] DEFAULT ('') NULL,
    [Remark]     NVARCHAR (60) CONSTRAINT [DF_IssueLack_Remark] DEFAULT ('') NULL,
    [ApvName]    VARCHAR (10)  CONSTRAINT [DF_IssueLack_ApvName] DEFAULT ('') NULL,
    [ApvDate]    DATE          NULL,
    [FabricType] VARCHAR (1)   CONSTRAINT [DF_IssueLack_FabricType] DEFAULT ('') NULL,
    [AddName]    VARCHAR (10)  CONSTRAINT [DF_IssueLack_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME      NULL,
    [EditName]   VARCHAR (10)  CONSTRAINT [DF_IssueLack_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME      NULL,
    CONSTRAINT [PK_IssueLack] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缺補料發料主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'RequestID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack', @level2type = N'COLUMN', @level2name = N'EditDate';

