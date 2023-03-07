CREATE TABLE [dbo].[PO_Factory] (
    [ID]              VARCHAR (13)   CONSTRAINT [DF_PO_Factory_ID] DEFAULT ('') NOT NULL,
    [SEQ1]            VARCHAR (3)    CONSTRAINT [DF_PO_Factory_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]            VARCHAR (2)    CONSTRAINT [DF_PO_Factory_SEQ2] DEFAULT ('') NOT NULL,
    [Refno]           VARCHAR (36)   CONSTRAINT [DF_PO_Factory_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]        VARCHAR (26)   CONSTRAINT [DF_PO_Factory_SCIRefno] DEFAULT ('') NULL,
    [FabricType]      VARCHAR (1)    CONSTRAINT [DF_PO_Factory_FabricType] DEFAULT ('') NOT NULL,
    [Qty]             NUMERIC (8, 2) CONSTRAINT [DF_PO_Factory_Qty] DEFAULT ((0)) NULL,
    [POUnit]          VARCHAR (8)    CONSTRAINT [DF_PO_Factory_POUnit] DEFAULT ('') NOT NULL,
    [ColorID]         VARCHAR (6)    CONSTRAINT [DF_PO_Factory_ColorID] DEFAULT ('') NULL,
    [SizeSpec]        VARCHAR (15)   CONSTRAINT [DF_PO_Factory_SizeSpec] DEFAULT ('') NULL,
    [SizeUnit]        VARCHAR (8)    CONSTRAINT [DF_PO_Factory_SizeUnit] DEFAULT ('') NULL,
    [Remark]          NVARCHAR (MAX) CONSTRAINT [DF_PO_Factory_Remark] DEFAULT ('') NULL,
    [Width]           NUMERIC (3, 1) CONSTRAINT [DF_PO_Factory_Width] DEFAULT ((0)) NULL,
    [Special]         VARCHAR (60)   CONSTRAINT [DF_PO_Factory_Special] DEFAULT ('') NULL,
    [Junk]            BIT            CONSTRAINT [DF_PO_Factory_Junk] DEFAULT ((0)) NULL,
    [BomFactory]      VARCHAR (10)   CONSTRAINT [DF_PO_Factory_BomFactory] DEFAULT ('') NULL,
    [BomCountry]      VARCHAR (2)    CONSTRAINT [DF_PO_Factory_BomCountry] DEFAULT ('') NULL,
    [BomStyle]        VARCHAR (15)   CONSTRAINT [DF_PO_Factory_BomStyle] DEFAULT ('') NULL,
    [BomCustCD]       VARCHAR (20)   CONSTRAINT [DF_PO_Factory_BomCustCD] DEFAULT ('') NULL,
    [BomArticle]      VARCHAR (8)    CONSTRAINT [DF_PO_Factory_BomArticle] DEFAULT ('') NULL,
    [BomZipperInsert] VARCHAR (5)    CONSTRAINT [DF_PO_Factory_BomZipperInsert] DEFAULT ('') NULL,
    [BomBuymonth]     VARCHAR (10)   CONSTRAINT [DF_PO_Factory_BomBuymonth] DEFAULT ('') NULL,
    [BomCustPONo]     VARCHAR (30)   CONSTRAINT [DF_PO_Factory_BomCustPONo] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_PO_Factory_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_PO_Factory_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    CONSTRAINT [PK_PO_Factory] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'POUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'SizeUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特殊規格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'Special';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomArticle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉鋉左右拉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomZipperInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂購月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomBuymonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'BomCustPONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Factory', @level2type = N'COLUMN', @level2name = N'EditDate';

