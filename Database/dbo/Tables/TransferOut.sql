﻿CREATE TABLE [dbo].[TransferOut] (
    [Id]            VARCHAR (13)   CONSTRAINT [DF_TransferOut_Id] DEFAULT ('') NOT NULL,
    [MDivisionID]   VARCHAR (8)    CONSTRAINT [DF_TransferOut_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]     VARCHAR (8)    NULL,
    [IssueDate]     DATE           NOT NULL,
    [Status]        VARCHAR (15)   CONSTRAINT [DF_TransferOut_Status] DEFAULT ('') NOT NULL,
    [Remark]        NVARCHAR (100) CONSTRAINT [DF_TransferOut_Remark] DEFAULT ('') NULL,
    [RequestID]     VARCHAR (13)   CONSTRAINT [DF_TransferOut_RequestID] DEFAULT ('') NULL,
    [ToMDivisionId] VARCHAR (8)    CONSTRAINT [DF_TransferOut_ToFtyId] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_TransferOut_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_TransferOut_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_TransferOut] PRIMARY KEY CLUSTERED ([Id] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠出主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠出單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'Remark';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferOut', @level2type = N'COLUMN', @level2name = N'ToMDivisionId';

