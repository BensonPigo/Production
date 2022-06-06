CREATE TABLE [dbo].[RFID_Combine] (
    [RFUID]    VARCHAR (10) CONSTRAINT [DF_RFID_Combine_RFUID] DEFAULT ('') NOT NULL,
    [BundleNo] VARCHAR (10) CONSTRAINT [DF_RFID_Combine_BundleNo] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME     NULL,
    [EditDate] DATETIME     NULL, 
    CONSTRAINT [PK_RFID_Combine] PRIMARY KEY ([RFUID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'低頻卡目前對應上哪一個Bundle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFID_Combine', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SNP低頻卡RFID編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFID_Combine', @level2type = N'COLUMN', @level2name = N'RFUID';

