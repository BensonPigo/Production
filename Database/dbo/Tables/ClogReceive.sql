CREATE TABLE [dbo].[ClogReceive] (
    [ID]          VARCHAR (13) CONSTRAINT [DF_ClogReceive_ID] DEFAULT ('') NOT NULL,
    [ReceiveDate] DATE         NOT NULL,
    [MDivisionID] VARCHAR (8)  CONSTRAINT [DF_ClogReceive_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]   VARCHAR (8)  CONSTRAINT [DF_ClogReceive_FactoryID] DEFAULT ('') NOT NULL,
    [Status]      VARCHAR (15) CONSTRAINT [DF_ClogReceive_Status] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_ClogReceive_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_ClogReceive_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME     NULL,
    CONSTRAINT [PK_ClogReceive] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton Receiving', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Receive Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'MDivisionID';

