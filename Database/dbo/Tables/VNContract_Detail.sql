CREATE TABLE [dbo].[VNContract_Detail] (
    [ID]            VARCHAR (15)    CONSTRAINT [DF_VNContract_Detail_ID] DEFAULT ('') NOT NULL,
    [HSCode]        VARCHAR (11)    CONSTRAINT [DF_VNContract_Detail_HSCode] DEFAULT ('') NOT NULL,
    [NLCode]        VARCHAR (5)     CONSTRAINT [DF_VNContract_Detail_NLCode] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (14, 3) CONSTRAINT [DF_VNContract_Detail_Qty] DEFAULT ((0)) NULL,
    [UnitID]        VARCHAR (8)     CONSTRAINT [DF_VNContract_Detail_UnitID] DEFAULT ('') NOT NULL,
    [WasteLower]    NUMERIC (5, 3)  CONSTRAINT [DF_VNContract_Detail_WasteLower] DEFAULT ((0)) NOT NULL,
    [Price]         NUMERIC (6, 3)  CONSTRAINT [DF_VNContract_Detail_Price] DEFAULT ((0)) NOT NULL,
    [LocalPurchase] BIT             CONSTRAINT [DF_VNContract_Detail_LocalPurchase] DEFAULT ((0)) NULL,
    [NecessaryItem] BIT             CONSTRAINT [DF_VNContract_Detail_NecessaryItem] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_VNContract_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [WasteUpper]	NUMERIC(5, 3)	CONSTRAINT [DF_VNContract_Detail_WasteUpper] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_VNContract_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [NLCode] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用量計算必要項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'NecessaryItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'越南採購', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'LocalPurchase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'合約簽訂的損耗率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = 'WasteLower';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簽約單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簽約數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NL Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'NLCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際商品統一分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'HSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關簽約紀錄明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Detail';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'合約簽訂的損耗率',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'VNContract_Detail',
    @level2type = N'COLUMN',
    @level2name = N'WasteUpper'