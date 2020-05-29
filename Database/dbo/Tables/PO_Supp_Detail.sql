CREATE TABLE [dbo].[PO_Supp_Detail] (
    [ID]              VARCHAR (13)    CONSTRAINT [DF_PO_Supp_Detail_ID] DEFAULT ('') NOT NULL,
    [SEQ1]            VARCHAR (3)     CONSTRAINT [DF_PO_Supp_Detail_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]            VARCHAR (2)     CONSTRAINT [DF_PO_Supp_Detail_SEQ2] DEFAULT ('') NOT NULL,
    [FactoryID]       VARCHAR (8)     DEFAULT ('') NULL,
    [Refno]           VARCHAR (20)    CONSTRAINT [DF_PO_Supp_Detail_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]        VARCHAR (30)    CONSTRAINT [DF_PO_Supp_Detail_SCIRefno] DEFAULT ('') NULL,
    [FabricType]      VARCHAR (1)     CONSTRAINT [DF_PO_Supp_Detail_FabricType] DEFAULT ('') NOT NULL,
    [Price]           NUMERIC (16, 4) CONSTRAINT [DF_PO_Supp_Detail_Price] DEFAULT ((0)) NOT NULL,
    [UsedQty]         NUMERIC (10, 4) CONSTRAINT [DF_PO_Supp_Detail_UsedQty] DEFAULT ((0)) NULL,
    [Qty]             NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_Qty] DEFAULT ((0)) NULL,
    [POUnit]          VARCHAR (8)     CONSTRAINT [DF_PO_Supp_Detail_POUnit] DEFAULT ('') NOT NULL,
    [Complete]        BIT             CONSTRAINT [DF_PO_Supp_Detail_Complete] DEFAULT ((0)) NULL,
    [SystemETD]       DATE            NULL,
    [CFMETD]          DATE            NULL,
    [RevisedETA]      DATE            NULL,
    [FinalETD]        DATE            NULL,
    [ShipETA]         DATE            NULL,
    [ETA]             DATE            NULL,
    [FinalETA]        DATE            NULL,
    [ShipModeID]      VARCHAR (10)    CONSTRAINT [DF_PO_Supp_Detail_ShipModeID] DEFAULT ('') NOT NULL,
    [SMRLock]         VARCHAR (10)    CONSTRAINT [DF_PO_Supp_Detail_SMRLock] DEFAULT ('') NULL,
    [SystemLock]      DATE            NULL,
    [PrintDate]       DATETIME        NULL,
    [PINO]            VARCHAR (25)    CONSTRAINT [DF_PO_Supp_Detail_PINO] DEFAULT ('') NULL,
    [PIDate]          DATE            NULL,
    [BrandId]         VARCHAR (8)     DEFAULT ('') NULL,
    [ColorID]         VARCHAR (6)     CONSTRAINT [DF_PO_Supp_Detail_ColorID] DEFAULT ('') NULL,
    [ColorID_Old]     VARCHAR (MAX)   DEFAULT ('') NULL,
    [SuppColor]       NVARCHAR (MAX)  CONSTRAINT [DF_PO_Supp_Detail_SuppColor] DEFAULT ('') NULL,
    [SizeSpec]        VARCHAR (15)    CONSTRAINT [DF_PO_Supp_Detail_SizeSpec] DEFAULT ('') NULL,
    [SizeUnit]        VARCHAR (8)     CONSTRAINT [DF_PO_Supp_Detail_SizeUnit] DEFAULT ('') NULL,
    [Remark]          NVARCHAR (MAX)  CONSTRAINT [DF_PO_Supp_Detail_Remark] DEFAULT ('') NULL,
    [Special]         NVARCHAR (MAX)  CONSTRAINT [DF_PO_Supp_Detail_Special] DEFAULT ('') NULL,
    [Width]           NUMERIC (5, 2)  CONSTRAINT [DF_PO_Supp_Detail_Width] DEFAULT ((0)) NULL,
    [StockQty]        NUMERIC (12, 1) CONSTRAINT [DF_PO_Supp_Detail_StockQty] DEFAULT ((0)) NULL,
    [NETQty]          NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_NETQty] DEFAULT ((0)) NULL,
    [LossQty]         NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_lossQty] DEFAULT ((0)) NULL,
    [SystemNetQty]    NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_SystemNetQty] DEFAULT ((0)) NULL,
    [StockPOID]       VARCHAR (13)    CONSTRAINT [DF_PO_Supp_Detail_StockPOID] DEFAULT ('') NULL,
    [StockSeq1]       VARCHAR (3)     NULL,
    [StockSeq2]       VARCHAR (2)     NULL,
    [InventoryUkey]   BIGINT          CONSTRAINT [DF_PO_Supp_Detail_InventoryUkey] DEFAULT ('') NULL,
    [OutputSeq1]      VARCHAR (3)     CONSTRAINT [DF_PO_Supp_Detail_OutputSeq1] DEFAULT ('') NULL,
    [OutputSeq2]      VARCHAR (2)     CONSTRAINT [DF_PO_Supp_Detail_OutputSeq2] DEFAULT ('') NULL,
    [SystemCreate]    BIT             CONSTRAINT [DF_PO_Supp_Detail_SystemCreate] DEFAULT ((0)) NULL,
    [ShipQty]         NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_ShipQty] DEFAULT ((0)) NULL,
    [ShipFOC]         NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_ShipFOC] DEFAULT ((0)) NULL,
    [ApQty]           NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_ApQty] DEFAULT ((0)) NULL,
    [Shortage]        NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_Shortage] DEFAULT ((0)) NULL,
    [FOC]             NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_FOC] DEFAULT ((0)) NULL,
    [Junk]            BIT             CONSTRAINT [DF_PO_Supp_Detail_Junk] DEFAULT ((0)) NULL,
    [ColorDetail]     NVARCHAR (MAX)  CONSTRAINT [DF_PO_Supp_Detail_ColorDetail] DEFAULT ('') NULL,
    [BomFactory]      VARCHAR (10)    CONSTRAINT [DF_PO_Supp_Detail_BomFactory] DEFAULT ('') NULL,
    [BomCountry]      VARCHAR (2)     CONSTRAINT [DF_PO_Supp_Detail_BomCountry] DEFAULT ('') NULL,
    [BomStyle]        VARCHAR (15)    CONSTRAINT [DF_PO_Supp_Detail_BomStyle] DEFAULT ('') NULL,
    [BomCustCD]       VARCHAR (20)    CONSTRAINT [DF_PO_Supp_Detail_BomCustCD] DEFAULT ('') NULL,
    [BomArticle]      VARCHAR (8)     CONSTRAINT [DF_PO_Supp_Detail_BomArticle] DEFAULT ('') NULL,
    [BomZipperInsert] VARCHAR (5)     CONSTRAINT [DF_PO_Supp_Detail_BomZipperInsert] DEFAULT ('') NULL,
    [BomBuymonth]     VARCHAR (10)    CONSTRAINT [DF_PO_Supp_Detail_BomBuymonth] DEFAULT ('') NULL,
    [BomCustPONo]     VARCHAR (30)    CONSTRAINT [DF_PO_Supp_Detail_BomCustPONo] DEFAULT ('') NULL,
    [Spec]            NVARCHAR (MAX)  NULL,
    [InputQty]        NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_InputQty] DEFAULT ((0)) NULL,
    [OutputQty]       NUMERIC (10, 2) CONSTRAINT [DF_PO_Supp_Detail_OutputQty] DEFAULT ((0)) NULL,
    [AddName]         VARCHAR (10)    CONSTRAINT [DF_PO_Supp_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME        NULL,
    [EditName]        VARCHAR (10)    CONSTRAINT [DF_PO_Supp_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME        NULL,
    [StockUnit]       VARCHAR (8)     CONSTRAINT [DF_PO_Supp_Detail_StockUnit] DEFAULT ('') NULL,
    [CfmETA]          DATE            NULL,
    [RevisedETD]      DATE            NULL,
    [POAmt] NUMERIC(16, 4) NOT NULL DEFAULT ((0)), 
    [ShipAmt] NUMERIC(16, 4) NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_PO_Supp_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統算出的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SystemNetQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用庫存SP#+SEQ#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'StockPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存primary key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'InventoryUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領用大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'OutputSeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領用小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'OutputSeq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統新增', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SystemCreate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'免費數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'FOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ColorDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomArticle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉鋉左右拉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomZipperInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂購月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomBuymonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'BomCustPONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ShipETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'短裝數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Shortage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'免費出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ShipFOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ApQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存累計入庫數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'InputQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存累計領料數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'OutputQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計到貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ETA';


GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'StockUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單-明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'UsedQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'POUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結清', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Complete';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原始交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SystemETD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商回復交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'CFMETD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商修正交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = 'RevisedETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'FinalETD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際到達日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = 'FinalETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組長鎖碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SMRLock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統鎖碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = 'SystemLock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'列印日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'PrintDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商PI#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'PINO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收到PI的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'PIDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SuppColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'SizeUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特殊規格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Special';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'Width';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存數量-', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'StockQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求量(淨需求)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'NETQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'耗損數量-自動轉單存入', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail', @level2type = N'COLUMN', @level2name = N'lossQty';

