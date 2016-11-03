﻿CREATE TABLE [dbo].[BundleInOut] (
    [BundleNo]     VARCHAR (10) CONSTRAINT [DF_BundleInOut_BundleNo] DEFAULT ('') NULL,
    [SubProcessId] VARCHAR (10) CONSTRAINT [DF_BundleInOut_SubProcessId] DEFAULT ('') NULL,
    [InComing]     DATETIME     NULL,
    [OutGoing]     DATETIME     NULL,
    [AddDate]      DATETIME     NULL,
    [EditDate]     DATETIME     NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Out', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'OutGoing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'In', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'InComing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub-process Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'SubProcessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle-Subprocess In Out Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut';

