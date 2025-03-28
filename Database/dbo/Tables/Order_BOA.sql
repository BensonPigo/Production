CREATE TABLE [dbo].[Order_BOA] (
    [Id]                      VARCHAR (13)    CONSTRAINT [DF_Order_BOA_Id] DEFAULT ('') NOT NULL,
    [Ukey]                    BIGINT          CONSTRAINT [DF_Order_BOA_Ukey] DEFAULT ((0)) NOT NULL,
    [Refno]                   VARCHAR (36)    CONSTRAINT [DF_Order_BOA_Refno] DEFAULT ('') NULL,
    [SCIRefno]                VARCHAR (30)    CONSTRAINT [DF_Order_BOA_SCIRefno] DEFAULT ('') NOT NULL,
    [SuppID]                  VARCHAR (6)     CONSTRAINT [DF_Order_BOA_SuppID] DEFAULT ('') NOT NULL,
    [Seq]                     VARCHAR (3)     CONSTRAINT [DF_Order_BOA_Seq] DEFAULT ('') NOT NULL,
    [ConsPC]                  DECIMAL (12, 4) CONSTRAINT [DF_Order_BOA_ConsPC] DEFAULT ((0)) NOT NULL,
    [BomTypeSize]             BIT             CONSTRAINT [DF_Order_BOA_BomTypeSize] DEFAULT ((0)) NOT NULL,
    [BomTypeColor]            BIT             CONSTRAINT [DF_Order_BOA_BomTypeColor] DEFAULT ((0)) NOT NULL,
    [BomTypePono]             BIT             CONSTRAINT [DF_Order_BOA_BomTypePono] DEFAULT ((0)) NOT NULL,
    [FabricPanelCode]         VARCHAR (2)     CONSTRAINT [DF_Order_BOA_FabricPanelCode] DEFAULT ('') NOT NULL,
    [PatternPanel]            VARCHAR (2)     CONSTRAINT [DF_Order_BOA_PatternPanel] DEFAULT ('') NOT NULL,
    [SizeItem]                VARCHAR (3)     CONSTRAINT [DF_Order_BOA_SizeItem] DEFAULT ('') NOT NULL,
    [BomTypeZipper]           BIT             CONSTRAINT [DF_Order_BOA_BomTypeZipper] DEFAULT ((0)) NOT NULL,
    [Remark]                  NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOA_Remark] DEFAULT ('') NOT NULL,
    [ProvidedPatternRoom]     BIT             CONSTRAINT [DF_Order_BOA_ProvidedPatternRoom] DEFAULT ((0)) NOT NULL,
    [ColorDetail]             NVARCHAR (100)  CONSTRAINT [DF_Order_BOA_ColorDetail] DEFAULT ('') NOT NULL,
    [isCustCD]                NUMERIC (2, 0)  CONSTRAINT [DF_Order_BOA_isCustCD] DEFAULT ((0)) NOT NULL,
    [lossType]                DECIMAL (1)     CONSTRAINT [DF_Order_BOA_lossType] DEFAULT ((0)) NOT NULL,
    [LossPercent]             DECIMAL (3, 1)  CONSTRAINT [DF_Order_BOA_LossPercent] DEFAULT ((0)) NOT NULL,
    [LossQty]                 DECIMAL (3)     CONSTRAINT [DF_Order_BOA_LossQty] DEFAULT ((0)) NOT NULL,
    [LossStep]                DECIMAL (6)     CONSTRAINT [DF_Order_BOA_LossStep] DEFAULT ((0)) NOT NULL,
    [AddName]                 VARCHAR (10)    CONSTRAINT [DF_Order_BOA_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                 DATETIME        NULL,
    [EditName]                VARCHAR (10)    CONSTRAINT [DF_Order_BOA_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                DATETIME        NULL,
    [SizeItem_Elastic]        VARCHAR (3)     CONSTRAINT [DF_Order_BOA_SizeItem_Elastic] DEFAULT ('') NOT NULL,
    [BomTypeFactory]          BIT             CONSTRAINT [DF_Order_BOA_BomTypeFactory] DEFAULT ((0)) NOT NULL,
    [BomTypePo]               BIT             CONSTRAINT [DF_Order_BOA_BomTypePo] DEFAULT ((0)) NOT NULL,
    [Keyword]                 VARCHAR (MAX)   CONSTRAINT [DF_Order_BOA_Keyword] DEFAULT ('') NOT NULL,
    [Seq1]                    VARCHAR (3)     CONSTRAINT [DF_Order_BOA_Seq1] DEFAULT ('') NOT NULL,
    [BomTypeMatching]         BIT             CONSTRAINT [DF_Order_BOA_BomTypeMatching] DEFAULT ((0)) NOT NULL,
    [BomTypeCalculatePCS]     BIT             CONSTRAINT [DF_Order_BOA_BomTypeCalculatePCS] DEFAULT ((0)) NOT NULL,
    [SizeItem_PCS]            VARCHAR (3)     CONSTRAINT [DF_Order_BOA_SizeItem_PCS] DEFAULT ('') NOT NULL,
    [LimitUp]                 DECIMAL (7, 2)  CONSTRAINT [DF_Order_BOA_LimitUp] DEFAULT ((0)) NOT NULL,
    [LimitDown]               DECIMAL (7, 2)  CONSTRAINT [DF_Order_BOA_LimitDown] DEFAULT ((0)) NOT NULL,
    [BomTypeArticle]          BIT             CONSTRAINT [DF_Order_BOA_BomTypeArticle] DEFAULT ((0)) NOT NULL,
    [BomTypeCOO]              BIT             CONSTRAINT [DF_Order_BOA_BomTypeCOO] DEFAULT ((0)) NOT NULL,
    [BomTypeGender]           BIT             CONSTRAINT [DF_Order_BOA_BomTypeGender] DEFAULT ((0)) NOT NULL,
    [BomTypeCustomerSize]     BIT             CONSTRAINT [DF_Order_BOA_BomTypeCustomerSize] DEFAULT ((0)) NOT NULL,
    [CustomerSizeRelation]    VARCHAR (3)     CONSTRAINT [DF_Order_BOA_CustomerSizeRelation] DEFAULT ('') NOT NULL,
    [BomTypeDecLabelSize]     BIT             CONSTRAINT [DF_Order_BOA_BomTypeDecLabelSize] DEFAULT ((0)) NOT NULL,
    [DecLabelSizeRelation]    VARCHAR (3)     CONSTRAINT [DF_Order_BOA_DecLabelSizeRelation] DEFAULT ('') NOT NULL,
    [BomTypeBrandFactoryCode] BIT             CONSTRAINT [DF_Order_BOA_BomTypeBrandFactoryCode] DEFAULT ((0)) NOT NULL,
    [BomTypeStyle]            BIT             CONSTRAINT [DF_Order_BOA_BomTypeStyle] DEFAULT ((0)) NOT NULL,
    [BomTypeStyleLocation]    BIT             CONSTRAINT [DF_Order_BOA_BomTypeStyleLocation] DEFAULT ((0)) NOT NULL,
    [BomTypeSeason]           BIT             CONSTRAINT [DF_Order_BOA_BomTypeSeason] DEFAULT ((0)) NOT NULL,
    [BomTypeCareCode]         BIT             CONSTRAINT [DF_Order_BOA_BomTypeCareCode] DEFAULT ((0)) NOT NULL,
    [BomTypeBuyMonth]         BIT             CONSTRAINT [DF_Order_BOA_BomTypeBuyMonth] DEFAULT ((0)) NOT NULL,
    [BomTypeBuyerDlvMonth]    BIT             CONSTRAINT [DF_Order_BOA_BomTypeBuyerDlvMonth] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_BOA] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);
















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Bill of Accessory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Referce No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依尺寸展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依顏色展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeColor';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依客人訂單單號展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'BomTypePono';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'量法的項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依左右插展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeZipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用數量由樣品室提供', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'ProvidedPatternRoom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Color Detial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'ColorDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依戶客戶資料展開規則', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'isCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗計算規則', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'lossType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗%', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'LossPercent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'LossQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗單位量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'LossStep';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA', @level2type = N'COLUMN', @level2name = N'EditDate';


GO

CREATE INDEX [Boa_Expand] ON [dbo].[Order_BOA] ([Id],[Ukey],[Refno],[Seq])
go

CREATE NONCLUSTERED INDEX [IDX_Order_BOA_WH_P03] ON [dbo].[Order_BOA] ([Id],[SCIRefno],[Seq1])