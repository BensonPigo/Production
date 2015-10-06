CREATE TABLE [dbo].[Pullout] (
    [ID]          VARCHAR (13) CONSTRAINT [DF_Pullout_ID] DEFAULT ('') NOT NULL,
    [PulloutDate] DATE         NOT NULL,
    [FactoryID]   VARCHAR (8)  CONSTRAINT [DF_Pullout_FactoryID] DEFAULT ('') NOT NULL,
    [Status]      VARCHAR (15) CONSTRAINT [DF_Pullout_Status] DEFAULT ('') NULL,
    [SendToTPE]   DATE         NULL,
    [LockDate]    DATE         NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_Pullout_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_Pullout_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME     NULL,
    CONSTRAINT [PK_Pullout] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Report', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'PulloutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳回台北日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'SendToTPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'月結', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'LockDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout', @level2type = N'COLUMN', @level2name = N'EditDate';

