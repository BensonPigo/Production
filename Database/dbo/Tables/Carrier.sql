CREATE TABLE [dbo].[Carrier] (
    [ID]       VARCHAR (4)  CONSTRAINT [DF_Carrier_ID] DEFAULT ('') NOT NULL,
    [SuppID]   VARCHAR (6)  CONSTRAINT [DF_Carrier_SuppID] DEFAULT ('') NULL,
    [Account]  VARCHAR (20) CONSTRAINT [DF_Carrier_Account] DEFAULT ('') NULL,
    [Junk]     BIT          CONSTRAINT [DF_Carrier_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_Carrier_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_Carrier_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_Carrier] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際快遞帳號檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'Account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier', @level2type = N'COLUMN', @level2name = N'EditDate';

