CREATE TABLE [dbo].[KHExportDeclaration] (
    [ID]         VARCHAR (13)  NOT NULL,
    [Cdate]      DATE          NULL,
    [DeclareNo]  VARCHAR (25)  CONSTRAINT [DF_KHExportDeclaration_DeclareNo] DEFAULT ('') NULL,
    [Status]     VARCHAR (15)  CONSTRAINT [DF_KHExportDeclaration_Status] DEFAULT ('') NULL,
    [Shipper]    VARCHAR (8)   CONSTRAINT [DF_KHExportDeclaration_Shipper] DEFAULT ('') NULL,
    [Buyer]      VARCHAR (8)   CONSTRAINT [DF_KHExportDeclaration_Buyer] DEFAULT ('') NULL,
    [ShipModeID] VARCHAR (10)  CONSTRAINT [DF_KHExportDeclaration_ShipModeID] DEFAULT ('') NULL,
    [CustCDID]   VARCHAR (16)  CONSTRAINT [DF_KHExportDeclaration_CustCDID] DEFAULT ('') NULL,
    [Dest]       VARCHAR (2)   CONSTRAINT [DF_KHExportDeclaration_Dest] DEFAULT ('') NULL,
    [Forwarder]  VARCHAR (8)   CONSTRAINT [DF_KHExportDeclaration_Forwarder] DEFAULT ('') NULL,
    [ExportPort] VARCHAR (20)  CONSTRAINT [DF_KHExportDeclaration_ExportPort] DEFAULT ('') NULL,
    [Remark]     VARCHAR (200) CONSTRAINT [DF_KHExportDeclaration_Remark] DEFAULT ('') NULL,
    [AddName]    VARCHAR (10)  CONSTRAINT [DF_KHExportDeclaration_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME      NULL,
    [EditName]   VARCHAR (10)  NULL,
    [EditDate]   DATETIME      NULL,
    CONSTRAINT [PK_KHExportDeclaration] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'ExportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Forwarder';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Dest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Buyer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨人(工廠)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Shipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口報關編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'DeclareNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口報關日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'Cdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration', @level2type = N'COLUMN', @level2name = N'ID';

