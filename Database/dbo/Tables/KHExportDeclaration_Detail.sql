CREATE TABLE [dbo].[KHExportDeclaration_Detail] (
    [ID]             VARCHAR (13)   CONSTRAINT [DF_KHExportDeclaration_Detail_ID] DEFAULT ('') NOT NULL,
    [INVNo]          VARCHAR (25)   CONSTRAINT [DF_KHExportDeclaration_Detail_INVNo] DEFAULT ('') NOT NULL,
    [OrderID]        VARCHAR (13)   CONSTRAINT [DF_KHExportDeclaration_Detail_OrderID] DEFAULT ('') NOT NULL,
    [StyleUkey]      BIGINT         CONSTRAINT [DF_KHExportDeclaration_Detail_StyleUkey] DEFAULT ((0)) NULL,
    [Description]    NVARCHAR (MAX) CONSTRAINT [DF_KHExportDeclaration_Detail_Description] DEFAULT ('') NULL,
    [ShipModeSeqQty] INT            CONSTRAINT [DF_KHExportDeclaration_Detail_ShipModeSeqQty] DEFAULT ((0)) NULL,
    [CTNQty]         INT            CONSTRAINT [DF_KHExportDeclaration_Detail_CTNQty] DEFAULT ((0)) NULL,
    [POPrice]        NUMERIC (9, 4) CONSTRAINT [DF_KHExportDeclaration_Detail_POPrice] DEFAULT ((0)) NULL,
    [LocalINVNo]     VARCHAR (25)   CONSTRAINT [DF_KHExportDeclaration_Detail_LocalINVNo] DEFAULT ('') NULL,
    [NetKg]          NUMERIC (9, 2) CONSTRAINT [DF_KHExportDeclaration_Detail_NetKg] DEFAULT ((0)) NULL,
    [WeightKg]       NUMERIC (9, 2) NULL,
    [HSCode]         VARCHAR (14)   CONSTRAINT [DF_KHExportDeclaration_Detail_HSCode] DEFAULT ('') NULL,
    [COFormType]     VARCHAR (20)   CONSTRAINT [DF_KHExportDeclaration_Detail_COFormType] DEFAULT ('') NULL,
    [COID]           VARCHAR (25)   CONSTRAINT [DF_KHExportDeclaration_Detail_COID] DEFAULT ('') NULL,
    [CODate]         DATE           NULL,
    CONSTRAINT [PK_KHExportDeclaration_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [INVNo] ASC, [OrderID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產地認證日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'CODate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產地認證編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'COID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產地認證格式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'COFormType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'HSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Invoice No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'LocalINVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FOB單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'POPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的總箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'INVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHExportDeclaration_Detail', @level2type = N'COLUMN', @level2name = N'ID';

