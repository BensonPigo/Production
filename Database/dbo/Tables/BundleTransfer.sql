CREATE TABLE [dbo].[BundleTransfer] (
    [Sid]                   BIGINT        CONSTRAINT [DF_BundleTransfer_Sid] DEFAULT ((0)) NULL,
    [RFIDReaderId]          VARCHAR (24)  CONSTRAINT [DF_BundleTransfer_RFIDReaderId] DEFAULT ('') NULL,
    [Type]                  VARCHAR (1)   CONSTRAINT [DF_BundleTransfer_Type] DEFAULT ('') NULL,
    [SubProcessId]          VARCHAR (15)  CONSTRAINT [DF_BundleTransfer_SubProcessId] DEFAULT ('') NULL,
    [TagId]                 VARCHAR (24)  CONSTRAINT [DF_BundleTransfer_TagId] DEFAULT ('') NULL,
    [BundleNo]              NVARCHAR (43) CONSTRAINT [DF_BundleTransfer_BundleNo] DEFAULT ('') NULL,
    [TransferDate]          DATETIME      NULL,
    [AddDate]               DATETIME      NULL,
    [LocationID]            VARCHAR (10)  DEFAULT ('') NOT NULL,
    [RFIDProcessLocationID] VARCHAR (15)  CONSTRAINT [DF_BundleTransfer_RFIDProcessLocationID] DEFAULT ('') NOT NULL,
    [PanelNo]               VARCHAR (24)  CONSTRAINT [DF_BundleTransfer_PanelNo] DEFAULT ('') NOT NULL,
    [CutCellID]             VARCHAR (10)  CONSTRAINT [DF_BundleTransfer_CutCellID] DEFAULT ('') NOT NULL, 
    [SewingLineID] VARCHAR(2) NOT NULL DEFAULT ('')
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'TransferDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RFID tag Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'TagId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub-process Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'SubProcessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RFID Reader Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'RFIDReaderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sid', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer', @level2type = N'COLUMN', @level2name = N'Sid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle transfer record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTransfer';

