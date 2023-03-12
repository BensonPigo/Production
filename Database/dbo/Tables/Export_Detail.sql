CREATE TABLE [dbo].[Export_Detail] (
    [ID]             VARCHAR (13)    CONSTRAINT [DF_Export_Detail_ID] DEFAULT ('') NOT NULL,
    [PoID]           VARCHAR (13)    CONSTRAINT [DF_Export_Detail_PoID] DEFAULT ('') NULL,
    [Seq1]           VARCHAR (3)     CONSTRAINT [DF_Export_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]           VARCHAR (2)     CONSTRAINT [DF_Export_Detail_Seq2] DEFAULT ('') NOT NULL,
    [ExportIDOld]    VARCHAR (13)    CONSTRAINT [DF_Export_Detail_ExportIDOld] DEFAULT ('') NULL,
    [Ukey]           BIGINT          CONSTRAINT [DF_Export_Detail_Ukey] DEFAULT ((0)) NOT NULL,
    [Qty]            NUMERIC (12, 2)  CONSTRAINT [DF_Export_Detail_Qty] DEFAULT ((0)) NULL,
    [Foc]            NUMERIC (12, 2)  CONSTRAINT [DF_Export_Detail_Foc] DEFAULT ((0)) NULL,
    [Carton]         NVARCHAR (1000)  CONSTRAINT [DF_Export_Detail_Carton] DEFAULT ('') NULL,
    [Confirm]        BIT             CONSTRAINT [DF_Export_Detail_Confirm] DEFAULT ((0)) NULL,
    [UnitId]         VARCHAR (8)     CONSTRAINT [DF_Export_Detail_UnitId] DEFAULT ('') NOT NULL,
    [Price]          NUMERIC (16, 4) CONSTRAINT [DF_Export_Detail_Price] DEFAULT ((0)) NOT NULL,
    [NetKg]          NUMERIC (10, 2)  CONSTRAINT [DF_Export_Detail_NetKg] DEFAULT ((0)) NULL,
    [WeightKg]       NUMERIC (10, 2)  CONSTRAINT [DF_Export_Detail_WeightKg] DEFAULT ((0)) NULL,
    [Remark]         NVARCHAR (600)  CONSTRAINT [DF_Export_Detail_Remark] DEFAULT ('') NULL,
    [PayDesc]        NVARCHAR (600)  CONSTRAINT [DF_Export_Detail_PayDesc] DEFAULT ('') NULL,
    [LastEta]        DATE            NULL,
    [Refno]          VARCHAR (36)    CONSTRAINT [DF_Export_Detail_Refno] DEFAULT ('') NULL,
    [SuppID]         VARCHAR (6)     CONSTRAINT [DF_Export_Detail_SuppID] DEFAULT ('') NOT NULL,
    [Pino]           VARCHAR (25)    CONSTRAINT [DF_Export_Detail_Pino] DEFAULT ('') NULL,
    [Description]    NVARCHAR (MAX)  CONSTRAINT [DF_Export_Detail_Description] DEFAULT ('') NULL,
    [UnitOld]        VARCHAR (8)     CONSTRAINT [DF_Export_Detail_UnitOld] DEFAULT ('') NULL,
    [PinoOld]        VARCHAR (25)    CONSTRAINT [DF_Export_Detail_PinoOld] DEFAULT ('') NULL,
    [SuppIDOld]      VARCHAR (6)     CONSTRAINT [DF_Export_Detail_SuppIDOld] DEFAULT ('') NULL,
    [PriceOld]       NUMERIC (16, 4) CONSTRAINT [DF_Export_Detail_PriceOld] DEFAULT ((0)) NOT NULL,
    [PoType]         VARCHAR (1)     CONSTRAINT [DF_Export_Detail_PoType] DEFAULT ('') NULL,
    [FabricType]     VARCHAR (1)     CONSTRAINT [DF_Export_Detail_FabricType] DEFAULT ('') NULL,
    [ShipPlanID]     VARCHAR (13)    CONSTRAINT [DF_Export_Detail_ShipPlanID] DEFAULT ('') NULL,
    [ShipPlanHandle] VARCHAR (10)    CONSTRAINT [DF_Export_Detail_ShipPlanHandle] DEFAULT ('') NOT NULL,
    [PoHandle]       VARCHAR (10)    CONSTRAINT [DF_Export_Detail_PoHandle] DEFAULT ('') NULL,
    [PcHandle]       VARCHAR (10)    CONSTRAINT [DF_Export_Detail_PcHandle] DEFAULT ('') NULL,
    [IsFormA]        BIT             CONSTRAINT [DF_Export_Detail_IsFormA] DEFAULT ((0)) NULL,
    [FormXDraftCFM]  DATE            NULL,
    [FormXINV]       NVARCHAR (600)  CONSTRAINT [DF_Export_Detail_FormXINV] DEFAULT ('') NULL,
    [FormXReceived]  DATE            NULL,
    [FormXFTYEdit]   DATETIME        NULL,
    [FormXEdit]      DATETIME        NULL,
    [FormXPayINV]    NVARCHAR (600)  CONSTRAINT [DF_Export_Detail_FormXPayINV] DEFAULT ('') NULL,
    [FormXType]      VARCHAR (8)     CONSTRAINT [DF_Export_Detail_FormXType] DEFAULT ('') NULL,
    [FormXAwb]       VARCHAR (20)    CONSTRAINT [DF_Export_Detail_FormXAwb] DEFAULT ('') NULL,
    [FormXCarrier]   VARCHAR (30)    CONSTRAINT [DF_Export_Detail_FormXCarrier] DEFAULT ('') NULL,
    [FormXRemark]    NVARCHAR (600)  CONSTRAINT [DF_Export_Detail_FormXRemark] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10)    CONSTRAINT [DF_Export_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME        NULL,
    [EditDate]       DATETIME        NULL,
    [EditName]       VARCHAR (10)    CONSTRAINT [DF_Export_Detail_EditName] DEFAULT ('') NULL,
    [BalanceQty]     NUMERIC (12, 2)  CONSTRAINT [DF_Export_Detail_BalanceQty] DEFAULT ((0)) NULL,
    [BalanceFOC]     NUMERIC (12, 2)  CONSTRAINT [DF_Export_Detail_BalanceFOC] DEFAULT ((0)) NULL,
    [CurrencyId]     VARCHAR (3)     CONSTRAINT [DF_Export_Detail_CurrencyId] DEFAULT ('') NULL,
    [InvoiceNo] VARCHAR(max) CONSTRAINT [DF_Export_Detail_InvoiceNo] DEFAULT ('') NULL,
    CONSTRAINT [PK_Export_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC, [ShipPlanHandle] ASC)
);










GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20150824-133821]
    ON [dbo].[Export_Detail]([PoID] ASC, [Seq1] ASC, [Seq2] ASC, [ID] ASC, [SuppID] ASC);




GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20150901-170439]
    ON [dbo].[Export_Detail]([FabricType] ASC, [PoType] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務出貨檔明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Working No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用WK Separation時的來源WKNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'ExportIDOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID+OrderID+Seq1+Seq2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口FOC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Foc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Carton';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量過入採購單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Confirm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'UnitId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'已過帳未請款原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PayDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Last ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'LastEta';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商PI#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Pino';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位(舊)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'UnitOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商PI#(舊)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PinoOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商(舊)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'SuppIDOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價(舊)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PriceOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Gmt/ Mms Po', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PoType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric, Acc,Parts,Machine,Misc.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄來源編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'ShipPlanID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源單據負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'ShipPlanHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單poHandle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PoHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單pcHandle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'PcHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mtl FormA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'IsFormA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'草稿收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXDraftCFM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FormA的Invoice NO.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXINV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXReceived';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠更新收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXFTYEdit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的船務編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXEdit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的payment invoice#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXPayINV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的doc type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的AWB #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXAwb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的Carrier Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXCarrier';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'FormXRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發票號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InvoiceNo'