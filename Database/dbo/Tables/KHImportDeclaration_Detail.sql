CREATE TABLE [dbo].[KHImportDeclaration_Detail] (
    [ID]                VARCHAR (13)   NOT NULL,
    [Ukey]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [BLNo]              VARCHAR (20)   CONSTRAINT [DF_KHImportDeclaration_Detail_BLNo] DEFAULT ('') NULL,
    [ExportID]          VARCHAR (13)   CONSTRAINT [DF_KHImportDeclaration_Detail_ExportID] DEFAULT ('') NULL,
    [Consignee]         VARCHAR (8)    CONSTRAINT [DF_KHImportDeclaration_Detail_Consignee] DEFAULT ('') NULL,
    [FactoryID]         VARCHAR (8)    CONSTRAINT [DF_KHImportDeclaration_Detail_FactoryID] DEFAULT ('') NULL,
    [ETA]               DATE           NULL,
    [PortArrival]       DATE           NULL,
    [WhseArrival]       DATE           NULL,
    [ShipModeID]        VARCHAR (10)   CONSTRAINT [DF_KHImportDeclaration_Detail_ShipModeID] DEFAULT ('') NULL,
    [Vessel]            VARCHAR (60)   CONSTRAINT [DF_KHImportDeclaration_Detail_Vessel] DEFAULT ('') NULL,
    [ExportPort]        VARCHAR (20)   CONSTRAINT [DF_KHImportDeclaration_Detail_ExportPort] DEFAULT ('') NULL,
    [RefNo]             VARCHAR (23)   CONSTRAINT [DF_KHImportDeclaration_Detail_RefNo] DEFAULT ('') NULL,
    [Description]       NVARCHAR (MAX) CONSTRAINT [DF_KHImportDeclaration_Detail_Description] DEFAULT ('') NULL,
    [Qty]               NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_Detail_Qty] DEFAULT ((0)) NULL,
    [Price]             NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_Detail_Price] DEFAULT ((0)) NULL,
    [UnitId]            VARCHAR (8)    CONSTRAINT [DF_KHImportDeclaration_Detail_UnitId] DEFAULT ('') NULL,
    [NetKg]             NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_Detail_NetKg] DEFAULT ((0)) NULL,
    [WeightKg]          NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_Detail_WeightKg] DEFAULT ((0)) NULL,
    [KHCustomsItemUkey] BIGINT         CONSTRAINT [DF_KHImportDeclaration_Detail_KHCustomsItemUkey] DEFAULT ((0)) NULL,
    [CDCQty]            NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_Detail_CDCQty] DEFAULT ((0)) NULL,
    [CDCUnit]           NVARCHAR (50)  CONSTRAINT [DF_KHImportDeclaration_Detail_CDCUnit] DEFAULT ('') NULL,
    [CDCUnitPrice]      NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_Detail_CDCUnitPrice] DEFAULT ((0)) NULL,
    [ActNetKg]          NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_Detail_ActNetKg] DEFAULT ((0)) NULL,
    [ActWeightKg]       NUMERIC (9, 2) CONSTRAINT [DF_KHImportDeclaration_Detail_ActWeightKg] DEFAULT ((0)) NULL,
    [ActAmount]         NUMERIC (9, 4) CONSTRAINT [DF_KHImportDeclaration_Detail_ActAmount] DEFAULT ((0)) NULL,
    [ActHSCode]         VARCHAR (14)   CONSTRAINT [DF_KHImportDeclaration_Detail_ActHSCode] DEFAULT ('') NULL,
    CONSTRAINT [PK_KHImportDeclaration_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ActHSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口報關總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ActAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口報關毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ActWeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口報關實際淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ActNetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關單位單價(USD) (當下)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'CDCUnitPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口報關數量(當下)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'CDCQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KHCustomsItem Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'KHCustomsItemUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'UnitId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船名/航次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ExportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船名/航次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Vessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達W/H日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'WhseArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到港日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'PortArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'('''')', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Consignee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ExportID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'BLNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KHCustomsItem Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHImportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ID';

