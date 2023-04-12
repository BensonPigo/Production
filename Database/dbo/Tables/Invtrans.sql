CREATE TABLE [dbo].[Invtrans] (
    [ID]                  VARCHAR (13)    CONSTRAINT [DF_Invtrans_ID] DEFAULT ('') NOT NULL,
    [ConfirmDate]         DATETIME        NULL,
    [ConfirmHandle]       VARCHAR (10)    CONSTRAINT [DF_Invtrans_ConfirmHandle] DEFAULT ('') NULL,
    [Confirmed]           BIT             CONSTRAINT [DF_Invtrans_Confirmed] DEFAULT ((0)) NOT NULL,
    [Qty]                 NUMERIC (10, 2) CONSTRAINT [DF_Invtrans_Qty] DEFAULT ((0)) NOT NULL,
    [Type]                VARCHAR (1)     CONSTRAINT [DF_Invtrans_Type] DEFAULT ('') NOT NULL,
    [TransferFactory]     VARCHAR (8)     CONSTRAINT [DF_Invtrans_TransferFactory] DEFAULT ('') NULL,
    [InventoryUkey]       BIGINT          CONSTRAINT [DF_Invtrans_InventoryUkey] DEFAULT ((0)) NOT NULL,
    [InventoryRefnoId]    BIGINT          CONSTRAINT [DF_Invtrans_InventoryRefnoId] DEFAULT ((0)) NOT NULL,
    [PoID]                VARCHAR (13)    CONSTRAINT [DF_Invtrans_PoID] DEFAULT ('') NULL,
    [Seq1]                VARCHAR (3)     CONSTRAINT [DF_Invtrans_Seq1] DEFAULT ('') NULL,
    [Seq2]                VARCHAR (2)     CONSTRAINT [DF_Invtrans_Seq2] DEFAULT ('') NULL,
    [InventoryPOID]       VARCHAR (13)    CONSTRAINT [DF_Invtrans_InventoryPOID] DEFAULT ('') NULL,
    [InventorySeq1]       VARCHAR (3)     CONSTRAINT [DF_Invtrans_InventorySeq1] DEFAULT ('') NULL,
    [InventorySeq2]       VARCHAR (2)     CONSTRAINT [DF_Invtrans_InventorySeq2] DEFAULT ('') NULL,
    [Remark]              NVARCHAR (MAX)  CONSTRAINT [DF_Invtrans_Remark] DEFAULT ('') NULL,
    [CurrencyID]          VARCHAR (3)     CONSTRAINT [DF_Invtrans_CurrencyID] DEFAULT ('') NOT NULL,
    [JunkPo3]             VARCHAR (1)     CONSTRAINT [DF_Invtrans_JunkPo3] DEFAULT ('') NULL,
    [Deadline]            DATE            NULL,
    [ReasonID]            VARCHAR (5)     CONSTRAINT [DF_Invtrans_ReasonID] DEFAULT ('') NULL,
    [Payable]             VARCHAR (1)     CONSTRAINT [DF_Invtrans_Payable] DEFAULT ('') NULL,
    [PoHandle]            VARCHAR (10)    CONSTRAINT [DF_Invtrans_PoHandle] DEFAULT ('') NULL,
    [PoSmr]               VARCHAR (10)    CONSTRAINT [DF_Invtrans_PoSmr] DEFAULT ('') NULL,
    [OrderHandle]         VARCHAR (10)    CONSTRAINT [DF_Invtrans_OrderHandle] DEFAULT ('') NULL,
    [OrderSmr]            VARCHAR (10)    CONSTRAINT [DF_Invtrans_OrderSmr] DEFAULT ('') NULL,
    [PoFactory]           VARCHAR (8)     CONSTRAINT [DF_Invtrans_PoFactory] DEFAULT ('') NULL,
    [LimitHandle]         VARCHAR (10)    CONSTRAINT [DF_Invtrans_LimitHandle] DEFAULT ('') NULL,
    [LimitSmr]            VARCHAR (10)    CONSTRAINT [DF_Invtrans_LimitSmr] DEFAULT ('') NULL,
    [AuthMr]              VARCHAR (10)    CONSTRAINT [DF_Invtrans_AuthMr] DEFAULT ('') NULL,
    [VoucherID]           VARCHAR (16)    CONSTRAINT [DF_Invtrans_TransId] DEFAULT ('') NULL,
    [TransferUkey]        BIGINT          CONSTRAINT [DF_Invtrans_TransferUkey] DEFAULT ((0)) NULL,
    [Po3QtyOld]           NUMERIC (10, 2) CONSTRAINT [DF_Invtrans_Po3QtyOld] DEFAULT ((0)) NULL,
    [InventoryQtyOld]     NUMERIC (10, 2) CONSTRAINT [DF_Invtrans_InventoryQtyOld] DEFAULT ((0)) NULL,
    [ProjectOld]          VARCHAR (5)     CONSTRAINT [DF_Invtrans_ProjectOld] DEFAULT ('') NULL,
    [BrandID]             VARCHAR (8)     CONSTRAINT [DF_Invtrans_BrandID] DEFAULT ('') NULL,
    [BrandGroup]          VARCHAR (8)     CONSTRAINT [DF_Invtrans_BrandGroup] DEFAULT ('') NULL,
    [Refno]               VARCHAR (36)    CONSTRAINT [DF_Invtrans_Refno] DEFAULT ('') NOT NULL,
    [FabricType]          VARCHAR (1)     CONSTRAINT [DF_Invtrans_FabricType] DEFAULT ('') NOT NULL,
    [FactoryID]           VARCHAR (8)     CONSTRAINT [DF_Invtrans_FactoryID] DEFAULT ('') NOT NULL,
    [MtlTypeID]           VARCHAR (20)    CONSTRAINT [DF_Invtrans_MtlTypeID] DEFAULT ('') NULL,
    [ProjectID]           VARCHAR (5)     CONSTRAINT [DF_Invtrans_ProjectID] DEFAULT ('') NULL,
    [SeasonID]            VARCHAR (10)    CONSTRAINT [DF_Invtrans_SeasonID] DEFAULT ('') NULL,
    [StyleID]             VARCHAR (15)    CONSTRAINT [DF_Invtrans_StyleID] DEFAULT ('') NULL,
    [UnitID]              VARCHAR (8)     CONSTRAINT [DF_Invtrans_UnitID] DEFAULT ('') NOT NULL,
    [BomArticle]          VARCHAR (8)     CONSTRAINT [DF_Invtrans_BomArticle] DEFAULT ('') NULL,
    [BomFactory]          VARCHAR (10)    CONSTRAINT [DF_Invtrans_BomFactory] DEFAULT ('') NULL,
    [BomBuymonth]         VARCHAR (10)    CONSTRAINT [DF_Invtrans_BomBuymonth] DEFAULT ('') NULL,
    [BomStyle]            VARCHAR (15)    CONSTRAINT [DF_Invtrans_BomStyle] DEFAULT ('') NULL,
    [BomCountry]          VARCHAR (2)     CONSTRAINT [DF_Invtrans_BomCountry] DEFAULT ('') NULL,
    [BomCustCD]           VARCHAR (16)    CONSTRAINT [DF_Invtrans_BomCustCD] DEFAULT ('') NULL,
    [BomCustPONo]         VARCHAR (30)    CONSTRAINT [DF_Invtrans_BomCustPONo] DEFAULT ('') NULL,
    [BomZipperInsert]     VARCHAR (5)     CONSTRAINT [DF_Invtrans_BomZipperInsert] DEFAULT ('') NULL,
    [AddName]             VARCHAR (10)    CONSTRAINT [DF_Invtrans_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME        NULL,
    [EditDate]            DATETIME        NULL,
    [EditName]            VARCHAR (10)    CONSTRAINT [DF_Invtrans_EditName] DEFAULT ('') NULL,
    [Ukey]                BIGINT          CONSTRAINT [DF_Invtrans_Ukey] DEFAULT ((0)) NOT NULL,
    [CreateDate]          DATE            NULL,
    [seq70poid]           VARCHAR (13)    NULL,
    [seq70seq1]           VARCHAR (3)     NULL,
    [seq70seq2]           VARCHAR (2)     NULL,
    [TransferMDivisionID] VARCHAR (8)     NULL,
    CONSTRAINT [PK_Invtrans] PRIMARY KEY CLUSTERED ([ID] ASC, [Ukey] ASC)
);














GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存交易紀錄明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號(AAYYYYMMxxxxx)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認日期時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'ConfirmDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'ConfirmHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'已確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Confirmed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存轉廠後的工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'TransferFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存項目的Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'InventoryUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'InventoryRefnoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'PoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存SP#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'InventoryPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存Seq1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'InventorySeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存Seq2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'InventorySeq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Remark';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領料單在使用庫存後之數量是否為零', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'JunkPo3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後使用日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Deadline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否可跟客戶收款', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Payable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'po handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'PoHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'po smr', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'PoSmr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'OrderHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order smr', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'OrderSmr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單的Factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'PoFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'限定使用人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'LimitHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'限定使用人員的主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'LimitSmr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'允許使用的人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'AuthMr';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉廠庫存項目的Ukey(轉入庫)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'TransferUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購項目在領料/退回之前, 該po3的qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Po3QtyOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整之前庫存的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'InventoryQtyOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整之後庫存的ProjectID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'ProjectOld';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單的Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BRAND', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BrandGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'REFNO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric,Accessior,Other', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫別(轉出庫)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'MtlTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Project ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'ProjectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SEASON', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'STYLE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomArticle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的Factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單的顯示生產月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomBuymonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的Style', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的PO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomCustPONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的Zipper', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'BomZipperInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Invtrans]([InventoryPOID] ASC, [InventorySeq1] ASC, [InventorySeq2] ASC)
    INCLUDE([Qty], [Type], [TransferFactory], [FactoryID], [UnitID]);


GO
CREATE NONCLUSTERED INDEX [Type_ConfirmDate]
    ON [dbo].[Invtrans]([Type] ASC, [ConfirmDate] ASC)
    INCLUDE([ID], [Qty], [TransferFactory], [InventoryUkey], [InventoryRefnoId], [InventoryPOID], [InventorySeq1], [InventorySeq2], [ReasonID], [Refno], [FactoryID], [UnitID], [seq70poid], [seq70seq1], [seq70seq2]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Invtrans', @level2type = N'COLUMN', @level2name = N'VoucherID';


GO
CREATE NONCLUSTERED INDEX [seq70]
    ON [dbo].[Invtrans]([seq70poid] ASC, [seq70seq1] ASC, [seq70seq2] ASC);

