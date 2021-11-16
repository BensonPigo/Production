CREATE TABLE [dbo].[PO] (
    [ID]                            VARCHAR (13)   CONSTRAINT [DF_PO_ID] DEFAULT ('') NOT NULL,
    [StyleID]                       VARCHAR (15)   CONSTRAINT [DF_PO_StyleID] DEFAULT ('') NULL,
    [SeasonId]                      VARCHAR (10)   CONSTRAINT [DF_PO_SeasonId] DEFAULT ('') NULL,
    [StyleUkey]                     BIGINT         CONSTRAINT [DF_PO_StyleUkey] DEFAULT ((0)) NULL,
    [BrandID]                       VARCHAR (8)    CONSTRAINT [DF_PO_BrandID] DEFAULT ('') NULL,
    [POSMR]                         VARCHAR (10)   CONSTRAINT [DF_PO_POSMR] DEFAULT ('') NOT NULL,
    [POHandle]                      VARCHAR (10)   CONSTRAINT [DF_PO_POHandle] DEFAULT ('') NOT NULL,
    [PCHandle]                      VARCHAR (10)   CONSTRAINT [DF_PO_PCHandle] DEFAULT ('') NOT NULL,
    [PCSMR]                         VARCHAR (10)   CONSTRAINT [DF_PO_PCSMR] DEFAULT ('') NOT NULL,
    [McHandle]                      VARCHAR (10)   CONSTRAINT [DF_PO_McHandle] DEFAULT ('') NOT NULL,
    [ShipMark]                      NVARCHAR (MAX) CONSTRAINT [DF_PO_ShipMark] DEFAULT ('') NULL,
    [FTYMark]                       VARCHAR (10)    CONSTRAINT [DF_PO_FTYMark] DEFAULT ('') NULL,
    [Complete]                      BIT            CONSTRAINT [DF_PO_Complete] DEFAULT ((0)) NULL,
    [PoRemark]                      NVARCHAR (MAX) CONSTRAINT [DF_PO_PoRemark] DEFAULT ('') NULL,
    [CostRemark]                    NVARCHAR (MAX) CONSTRAINT [DF_PO_CostRemark] DEFAULT ('') NULL,
    [IrregularRemark]               NVARCHAR (MAX) CONSTRAINT [DF_PO_IrregularRemark] DEFAULT ('') NULL,
    [FirstPoError]                  VARCHAR (3)    CONSTRAINT [DF_PO_FirstPoError] DEFAULT ('') NULL,
    [FirstEditName]                 VARCHAR (10)   CONSTRAINT [DF_PO_FirstEditName] DEFAULT ('') NULL,
    [FirstEditDate]                 DATETIME       NULL,
    [FirstAddDate]                  DATETIME       NULL,
    [FirstCostDate]                 DATETIME       NULL,
    [LastPoError]                   VARCHAR (3)    CONSTRAINT [DF_PO_LastPoError] DEFAULT ('') NULL,
    [LastEditName]                  VARCHAR (10)   CONSTRAINT [DF_PO_LastEditName] DEFAULT ('') NULL,
    [LastEditDate]                  DATETIME       NULL,
    [LastAddDate]                   DATETIME       NULL,
    [LastCostDate]                  DATETIME       NULL,
    [AddName]                       VARCHAR (10)   CONSTRAINT [DF_PO_AddName] DEFAULT ('') NULL,
    [AddDate]                       DATETIME       NULL,
    [EditName]                      VARCHAR (10)   CONSTRAINT [DF_PO_EditName] DEFAULT ('') NULL,
    [EditDate]                      DATETIME       NULL,
    [FIRRemark]                     VARCHAR (60)   CONSTRAINT [DF_PO_FIRRemark] DEFAULT ('') NULL,
    [AIRRemark]                     VARCHAR (60)   CONSTRAINT [DF_PO_AIRemark] DEFAULT ('') NULL,
    [FIRLaboratoryRemark]           VARCHAR (60)   CONSTRAINT [DF_PO_FIRLaboratoryRemark] DEFAULT ('') NULL,
    [AIRLaboratoryRemark]           VARCHAR (60)   CONSTRAINT [DF_PO_AIRLaboratoryRemark] DEFAULT ('') NULL,
    [OvenLaboratoryRemark]          VARCHAR (60)   CONSTRAINT [DF_PO_OvenLaboratoryRemark] DEFAULT ('') NULL,
    [ColorFastnessLaboratoryRemark] VARCHAR (60)   CONSTRAINT [DF_PO_ColorFastnessLaboratoryRemark] DEFAULT ('') NULL,
    [MTLDelay]                      DATE           NULL,
    [MinSciDelivery]                DATE           NULL,
    [FIRInspPercent]                NUMERIC (5, 2) DEFAULT ((0.00)) NOT NULL,
    [AIRInspPercent]                NUMERIC (5, 2) DEFAULT ((0.00)) NOT NULL,
    [FIRLabInspPercent]             NUMERIC (5, 2) CONSTRAINT [DF_PO_FIRLabInspPercent] DEFAULT ((0)) NOT NULL,
    [LabColorFastnessPercent]       NUMERIC (5, 2) CONSTRAINT [DF_PO_LabColorFastnessPercent] DEFAULT ((0)) NOT NULL,
    [LabOvenPercent]                NUMERIC (5, 2) CONSTRAINT [DF_PO_LabOvenPercent] DEFAULT ((0)) NULL,
    [AIRLabInspPercent]             NUMERIC (5, 2) CONSTRAINT [DF_PO_AIRLabInspPercent] DEFAULT ((0)) NULL,
    [ThreadVersion] VARCHAR(5) NULL, 
    CONSTRAINT [PK_PO] PRIMARY KEY CLUSTERED ([ID] ASC)
);














GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'SeasonId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'POSMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'po Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'POHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排船表的PC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'PCHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PC的主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'PCSMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠MC handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'McHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'由 <工廠> 代入', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'ShipMark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠嘜頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FTYMark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結清', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'Complete';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'PoRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'<COST>備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'CostRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'<IRRE L/ETA>的備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'IrregularRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購成本異常代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FirstPoError';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購成本異常修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FirstEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購成本異常修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FirstEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'異常代碼第一次填入日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FirstAddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第一次 COST異常發生日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FirstCostDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後採購成本異常代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'LastPoError';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後採購成本異常修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'LastEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後採購成本異常修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'LastEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後異常代碼第一次填入日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'LastAddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後COST異常發生日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'LastCostDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料檢驗備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FIRRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'輔料檢驗備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = 'AIRRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料水洗房備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'FIRLaboratoryRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'輔料水洗房備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'AIRLaboratoryRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'OvenLaboratoryRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掉色備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'ColorFastnessLaboratoryRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MTL contiguous delay', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO', @level2type = N'COLUMN', @level2name = N'MTLDelay';

