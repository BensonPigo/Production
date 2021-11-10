CREATE TABLE [dbo].[RFIDReader] (
    [Id]                    VARCHAR (24) CONSTRAINT [DF_RFIDReader_Id] DEFAULT ('') NOT NULL,
    [Type]                  VARCHAR (1)  CONSTRAINT [DF_RFIDReader_Type] DEFAULT ('') NULL,
    [AddName]               VARCHAR (10) CONSTRAINT [DF_RFIDReader_AddName] DEFAULT ('') NULL,
    [AddDate]               DATETIME     NULL,
    [EditName]              VARCHAR (10) CONSTRAINT [DF_RFIDReader_EditName] DEFAULT ('') NULL,
    [EditDate]              DATETIME     NULL,
    [SewingLineID]          VARCHAR (5)  NULL,
    [MDivisionID]           VARCHAR (8)  NOT NULL,
    [FactoryID]             VARCHAR (8)  CONSTRAINT [DF_RFIDReader_FactoryID] DEFAULT ('') NOT NULL,
    [Location]              VARCHAR (50) CONSTRAINT [DF_RFIDReader_Location] DEFAULT ('') NOT NULL,
    [RFIDProcessLocationID] VARCHAR (15) CONSTRAINT [DF_RFIDReader_RFIDProcessLocationID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_RFIDReader] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader', @level2type = N'COLUMN', @level2name = N'Type';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reader Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RFID Reader設定檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDReader';

