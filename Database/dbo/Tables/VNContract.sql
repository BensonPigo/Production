﻿CREATE TABLE [dbo].[VNContract] (
    [ID]            CHAR (15)     CONSTRAINT [DF_VNContract_ID] DEFAULT ('') NOT NULL,
    [StartDate]     DATE          NOT NULL,
    [EndDate]       DATE          NOT NULL,
    [TotalQty]      NUMERIC (10)  CONSTRAINT [DF_VNContract_TotalQty] DEFAULT ((0)) NOT NULL,
    [SubConName]    VARCHAR (40)  CONSTRAINT [DF_VNContract_SubConName] DEFAULT ('') NULL,
    [SubConAddress] VARCHAR (500) CONSTRAINT [DF_VNContract_SubConAddress] DEFAULT ('') NULL,
    [Encode]        BIT           CONSTRAINT [DF_VNContract_Encode] DEFAULT ((0)) NULL,
    [AddName]       CHAR (10)     CONSTRAINT [DF_VNContract_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      CHAR (10)     CONSTRAINT [DF_VNContract_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    CONSTRAINT [PK_VNContract] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'Encode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Subcon Address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'SubConAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Subcon Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'SubConName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年出口數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'TotalQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'合約截止日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'合約生效日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'StartDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關簽約紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract';

