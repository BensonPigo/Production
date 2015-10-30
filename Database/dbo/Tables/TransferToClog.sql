CREATE TABLE [dbo].[TransferToClog] (
    [Id]           VARCHAR (13) CONSTRAINT [DF_TransferToClog_Id] DEFAULT ('') NOT NULL,
    [TransferDate] DATE         NOT NULL,
    [MDivisionID]  VARCHAR (8)  CONSTRAINT [DF_TransferToClog_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]    VARCHAR (8)  CONSTRAINT [DF_TransferToClog_FactoryID] DEFAULT ('') NOT NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_TransferToClog_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_TransferToClog_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME     NULL,
    CONSTRAINT [PK_TransferToClog] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton transfer to clog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'TransferDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'MDivisionID';

