CREATE TABLE [dbo].[ClogReturn] (
    [ID]         VARCHAR (13) CONSTRAINT [DF_ClogReturn_ID] DEFAULT ('') NOT NULL,
    [ReturnDate] DATE         NOT NULL,
    [FactoryID]  VARCHAR (8)  CONSTRAINT [DF_ClogReturn_FactoryID] DEFAULT ('') NOT NULL,
    [Status]     VARCHAR (15) CONSTRAINT [DF_ClogReturn_Status] DEFAULT ('') NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_ClogReturn_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_ClogReturn_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_ClogReturn] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton Return', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Return Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'ReturnDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'EditDate';

