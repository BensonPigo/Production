CREATE TABLE [dbo].[ForwarderWhse] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [BrandID]    VARCHAR (8)    CONSTRAINT [DF_ForwarderWhse_BrandID] DEFAULT ('') NULL,
    [ShipModeID] VARCHAR (10)   CONSTRAINT [DF_ForwarderWhse_ShipModeID] DEFAULT ('') NULL,
    [Forwarder]  VARCHAR (6)    CONSTRAINT [DF_ForwarderWhse_Forwarder] DEFAULT ('') NOT NULL,
    [WhseNo]     NVARCHAR (50)  CONSTRAINT [DF_ForwarderWhse_WhseNo] DEFAULT ('') NOT NULL,
    [Address]    NVARCHAR (MAX) CONSTRAINT [DF_ForwarderWhse_Address] DEFAULT ('') NULL,
    [Contact]    NVARCHAR (30)  CONSTRAINT [DF_ForwarderWhse_Contact] DEFAULT ('') NULL,
    [Tel]        VARCHAR (30)   CONSTRAINT [DF_ForwarderWhse_Tel] DEFAULT ('') NULL,
    [Mobile]     VARCHAR (30)   CONSTRAINT [DF_ForwarderWhse_Mobile] DEFAULT ('') NULL,
    [Junk]       BIT            CONSTRAINT [DF_ForwarderWhse_Junk] DEFAULT ((0)) NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_ForwarderWhse_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_ForwarderWhse_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME       NULL,
    CONSTRAINT [PK_ForwarderWhse] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨代倉庫資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'Forwarder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨代倉庫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'WhseNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'Address';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連絡人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'Contact';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'行動電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'Mobile';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ForwarderWhse', @level2type = N'COLUMN', @level2name = N'EditDate';

