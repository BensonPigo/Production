CREATE TABLE [dbo].[Style_BOA] (
    [StyleUkey]               BIGINT          CONSTRAINT [DF_Style_BOA_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]                    BIGINT          CONSTRAINT [DF_Style_BOA_Ukey] DEFAULT ((0)) NOT NULL,
    [Refno]                   VARCHAR (36)    CONSTRAINT [DF_Style_BOA_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]                VARCHAR (30)    CONSTRAINT [DF_Style_BOA_SCIRefno] DEFAULT ('') NOT NULL,
    [SEQ1]                    VARCHAR (3)     CONSTRAINT [DF_Style_BOA_SEQ1] DEFAULT ('') NOT NULL,
    [ConsPC]                  DECIMAL (10, 4) CONSTRAINT [DF_Style_BOA_ConsPC] DEFAULT ((0)) NOT NULL,
    [PatternPanel]            VARCHAR (2)     CONSTRAINT [DF_Style_BOA_PatternPanel] DEFAULT ('') NOT NULL,
    [SizeItem]                VARCHAR (3)     CONSTRAINT [DF_Style_BOA_SizeItem] DEFAULT ('') NOT NULL,
    [ProvidedPatternRoom]     BIT             CONSTRAINT [DF_Style_BOA_ProvidedPatternRoom] DEFAULT ((0)) NOT NULL,
    [Remark]                  NVARCHAR (MAX)  CONSTRAINT [DF_Style_BOA_Remark] DEFAULT ('') NOT NULL,
    [ColorDetail]             NVARCHAR (100)  CONSTRAINT [DF_Style_BOA_ColorDetail] DEFAULT ('') NOT NULL,
    [IsCustCD]                NUMERIC (2, 0)  CONSTRAINT [DF_Style_BOA_IsCustCD] DEFAULT ((0)) NOT NULL,
    [BomTypeZipper]           BIT             CONSTRAINT [DF_Style_BOA_BomTypeZipper] DEFAULT ((0)) NOT NULL,
    [BomTypeSize]             BIT             CONSTRAINT [DF_Style_BOA_BomTypeSize] DEFAULT ((0)) NOT NULL,
    [BomTypeColor]            BIT             CONSTRAINT [DF_Style_BOA_BomTypeColor] DEFAULT ((0)) NOT NULL,
    [BomTypeStyle]            BIT             CONSTRAINT [DF_Style_BOA_BomTypeStyle] DEFAULT ((0)) NOT NULL,
    [BomTypeArticle]          BIT             CONSTRAINT [DF_Style_BOA_BomTypeArticle] DEFAULT ((0)) NOT NULL,
    [BomTypePo]               BIT             CONSTRAINT [DF_Style_BOA_BomTypePo] DEFAULT ((0)) NOT NULL,
    [BomTypeCustCD]           BIT             CONSTRAINT [DF_Style_BOA_BomTypeCustCD] DEFAULT ((0)) NOT NULL,
    [BomTypeFactory]          BIT             CONSTRAINT [DF_Style_BOA_BomTypeFactory] DEFAULT ((0)) NOT NULL,
    [BomTypeBuyMonth]         BIT             CONSTRAINT [DF_Style_BOA_BomTypeBuyMonth] DEFAULT ((0)) NOT NULL,
    [BomTypeCountry]          BIT             CONSTRAINT [DF_Style_BOA_BomTypeCountry] DEFAULT ((0)) NOT NULL,
    [SuppIDBulk]              VARCHAR (6)     CONSTRAINT [DF_Style_BOA_SuppIDBulk] DEFAULT ('') NOT NULL,
    [SuppIDSample]            VARCHAR (6)     CONSTRAINT [DF_Style_BOA_SuppIDSample] DEFAULT ('') NOT NULL,
    [AddName]                 VARCHAR (10)    CONSTRAINT [DF_Style_BOA_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                 DATETIME        NULL,
    [EditName]                VARCHAR (10)    CONSTRAINT [DF_Style_BOA_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                DATETIME        NULL,
    [FabricPanelCode]         VARCHAR (2)     CONSTRAINT [DF_Style_BOA_FabricPanelCode] DEFAULT ('') NOT NULL,
    [BomTypeCOO]              BIT             CONSTRAINT [DF_Style_BOA_BomTypeCOO] DEFAULT ((0)) NOT NULL,
    [BomTypeGender]           BIT             CONSTRAINT [DF_Style_BOA_BomTypeGender] DEFAULT ((0)) NOT NULL,
    [BomTypeCustomerSize]     BIT             CONSTRAINT [DF_Style_BOA_BomTypeCustomerSize] DEFAULT ((0)) NOT NULL,
    [CustomerSizeRelation]    VARCHAR (3)     CONSTRAINT [DF_Style_BOA_CustomerSizeRelation] DEFAULT ('') NOT NULL,
    [BomTypeDecLabelSize]     BIT             CONSTRAINT [DF_Style_BOA_BomTypeDecLabelSize] DEFAULT ((0)) NOT NULL,
    [DecLabelSizeRelation]    VARCHAR (3)     CONSTRAINT [DF_Style_BOA_DecLabelSizeRelation] DEFAULT ('') NOT NULL,
    [BomTypeBrandFactoryCode] BIT             CONSTRAINT [DF_Style_BOA_BomTypeBrandFactoryCode] DEFAULT ((0)) NOT NULL,
    [BomTypeStyleLocation]    BIT             CONSTRAINT [DF_Style_BOA_BomTypeStyleLocation] DEFAULT ((0)) NOT NULL,
    [BomTypeSeason]           BIT             CONSTRAINT [DF_Style_BOA_BomTypeSeason] DEFAULT ((0)) NOT NULL,
    [BomTypeCareCode]         BIT             CONSTRAINT [DF_Style_BOA_BomTypeCareCode] DEFAULT ((0)) NOT NULL,
    [BomTypeBuyerDlvMonth]    BIT             CONSTRAINT [DF_Style_BOA_BomTypeBuyerDlvMonth] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Style_BOA] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style - Bill of Accessory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Referce No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'量法的項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用數量由樣品室提供', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'ProvidedPatternRoom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'ColorDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客人物料展開規則', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'IsCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'計算採購時是否判斷左右插', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeZipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依尺寸展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依顏色展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依款式展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依顏色組展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeArticle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依客人訂單單號展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypePo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依客戶資料展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依工廠展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依月份展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeBuyMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依工廠的國家別展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨指定的廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'SuppIDBulk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷樣指定的廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'SuppIDSample';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA', @level2type = N'COLUMN', @level2name = N'EditDate';

