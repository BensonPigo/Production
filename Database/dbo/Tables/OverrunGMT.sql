CREATE TABLE [dbo].[OverrunGMT] (
    [ID]        VARCHAR (13)  CONSTRAINT [DF_OverrunGMT_ID] DEFAULT ('') NOT NULL,
    [FactoryID] VARCHAR (8)   CONSTRAINT [DF_OverrunGMT_FactoryID] DEFAULT ('') NOT NULL,
    [CloseDate] DATE          NOT NULL,
    [Remark]    NVARCHAR (40) CONSTRAINT [DF_OverrunGMT_Remark] DEFAULT ('') NULL,
    [Status]    VARCHAR (15)  CONSTRAINT [DF_OverrunGMT_Status] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)  CONSTRAINT [DF_OverrunGMT_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME      NULL,
    [EditName]  VARCHAR (10)  CONSTRAINT [DF_OverrunGMT_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME      NULL,
    CONSTRAINT [PK_OverrunGMT] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Overrun Garment Record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結算日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'CloseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT', @level2type = N'COLUMN', @level2name = N'EditDate';

