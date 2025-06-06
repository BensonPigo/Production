CREATE TABLE [dbo].[BundleInOut_History] (
    [BundleNo]              VARCHAR (10) CONSTRAINT [DF_BundleInOut_History_BundleNo] DEFAULT ('') NOT NULL,
    [SubProcessId]          VARCHAR (15) CONSTRAINT [DF_BundleInOut_History_SubProcessId] DEFAULT ('') NOT NULL,
    [InComing]              DATETIME     NULL,
    [OutGoing]              DATETIME     NULL,
    [AddDate]               DATETIME     NOT NULL,
    [EditDate]              DATETIME     NULL,
    [SewingLineID]          VARCHAR (5)  CONSTRAINT [DF_BundleInOut_History_SewingLineID] DEFAULT ('') NOT NULL,
    [LocationID]            VARCHAR (10) CONSTRAINT [DF_BundleInOut_History_LocationID] DEFAULT ('') NOT NULL,
    [RFIDProcessLocationID] VARCHAR (15) CONSTRAINT [DF_BundleInOut_History_RFIDProcessLocationID] DEFAULT ('') NOT NULL,
    [PanelNo]               VARCHAR (24) CONSTRAINT [DF_BundleInOut_History_PanelNo] DEFAULT ('') NOT NULL,
    [CutCellID]             VARCHAR (10) CONSTRAINT [DF_BundleInOut_History_CutCellID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BundleInOut_History] PRIMARY KEY CLUSTERED ([BundleNo] ASC, [SubProcessId] ASC, [RFIDProcessLocationID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Out', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History', @level2type = N'COLUMN', @level2name = N'OutGoing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'In', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History', @level2type = N'COLUMN', @level2name = N'InComing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub-process Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History', @level2type = N'COLUMN', @level2name = N'SubProcessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle-Subprocess In Out Date History', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut_History';

