CREATE TABLE [dbo].[FIR] (
    [ID]                           BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [POID]                         VARCHAR (13)    CONSTRAINT [DF_FIR_POID] DEFAULT ('') NOT NULL,
    [SEQ1]                         VARCHAR (3)     CONSTRAINT [DF_FIR_SEQ] DEFAULT ('') NOT NULL,
    [SEQ2]                         VARCHAR (2)     CONSTRAINT [DF_FIR_SEQ2] DEFAULT ('') NOT NULL,
    [Suppid]                       VARCHAR (6)     CONSTRAINT [DF_FIR_Suppid] DEFAULT ('') NOT NULL,
    [SCIRefno]                     VARCHAR (30)    CONSTRAINT [DF_FIR_SCIRefno] DEFAULT ('') NOT NULL,
    [Refno]                        VARCHAR (20)    CONSTRAINT [DF_FIR_BrandRefno] DEFAULT ('') NOT NULL,
    [ReceivingID]                  VARCHAR (13)    CONSTRAINT [DF_FIR_ReceivingID] DEFAULT ('') NULL,
    [ReplacementReportID]          VARCHAR (13)    CONSTRAINT [DF_FIR_ReplacementReportID] DEFAULT ('') NULL,
    [ArriveQty]                    NUMERIC (10, 2) CONSTRAINT [DF_FIR_ArriveQty] DEFAULT ((0)) NULL,
    [TotalInspYds]                 NUMERIC (10, 2) CONSTRAINT [DF_FIR_TotalInspYds] DEFAULT ((0)) NULL,
    [TotalDefectPoint]             NUMERIC (5)     CONSTRAINT [DF_FIR_TotalDefectPoint] DEFAULT ((0)) NULL,
    [Result]                       VARCHAR (5)     CONSTRAINT [DF_FIR_Result] DEFAULT ('') NULL,
    [Remark]                       NVARCHAR (100)  CONSTRAINT [DF_FIR_Remark] DEFAULT ('') NULL,
    [Nonphysical]                  BIT             CONSTRAINT [DF_FIR_Nonphysical] DEFAULT ((0)) NULL,
    [Physical]                     VARCHAR (5)     CONSTRAINT [DF_FIR_Physical] DEFAULT ('') NULL,
    [PhysicalEncode]               BIT             CONSTRAINT [DF_FIR_PhysicalEncode] DEFAULT ((0)) NULL,
    [PhysicalDate]                 DATETIME        NULL,
    [nonWeight]                    BIT             CONSTRAINT [DF_FIR_nonWeight] DEFAULT ((0)) NULL,
    [Weight]                       VARCHAR (5)     CONSTRAINT [DF_FIR_Weight] DEFAULT ('') NULL,
    [WeightEncode]                 BIT             CONSTRAINT [DF_FIR_WeightEncode] DEFAULT ((0)) NULL,
    [WeightDate]                   DATETIME        NULL,
    [nonShadebond]                 BIT             CONSTRAINT [DF_FIR_nonShadebond] DEFAULT ((0)) NULL,
    [ShadeBond]                    VARCHAR (5)     CONSTRAINT [DF_FIR_ShadeBond] DEFAULT ('') NULL,
    [ShadebondEncode]              BIT             CONSTRAINT [DF_FIR_ShadebondEncode] DEFAULT ((0)) NULL,
    [ShadeBondDate]                DATETIME        NULL,
    [nonContinuity]                BIT             CONSTRAINT [DF_FIR_nonContinuity] DEFAULT ((0)) NULL,
    [Continuity]                   VARCHAR (5)     CONSTRAINT [DF_FIR_Continuity] DEFAULT ('') NULL,
    [ContinuityEncode]             BIT             CONSTRAINT [DF_FIR_ContinuityEncode] DEFAULT ((0)) NULL,
    [ContinuityDate]               DATETIME        NULL,
    [InspDeadline]                 DATE            NULL,
    [Approve]                      VARCHAR (10)    CONSTRAINT [DF_FIR_Approve] DEFAULT ('') NULL,
    [ApproveDate]                  DATETIME        NULL,
    [AddName]                      VARCHAR (10)    CONSTRAINT [DF_FIR_AddName] DEFAULT ('') NULL,
    [AddDate]                      DATETIME        NULL,
    [EditName]                     VARCHAR (10)    CONSTRAINT [DF_FIR_EditName] DEFAULT ('') NULL,
    [EditDate]                     DATETIME        NULL,
    [Status]                       VARCHAR (15)    CONSTRAINT [DF_FIR_Status] DEFAULT ('') NULL,
    [OldFabricUkey]                VARCHAR (10)    DEFAULT ('') NULL,
    [OldFabricVer]                 VARCHAR (2)     DEFAULT ('') NULL,
    [nonOdor]                      BIT             CONSTRAINT [DF_FIR_nonOdor] DEFAULT ((0)) NULL,
    [Odor]                         VARCHAR (5)     CONSTRAINT [DF_FIR_Odor] DEFAULT ('') NULL,
    [OdorEncode]                   BIT             CONSTRAINT [DF_FIR_OdorEncode] DEFAULT ((0)) NULL,
    [OdorDate]                     DATETIME        NULL,
    [PhysicalInspector]            VARCHAR (10)    CONSTRAINT [DF_FIR_PhysicalInspector] DEFAULT ('') NOT NULL,
    [WeightInspector]              VARCHAR (10)    CONSTRAINT [DF_FIR_WeightInspector] DEFAULT ('') NOT NULL,
    [ShadeboneInspector]           VARCHAR (10)    CONSTRAINT [DF_FIR_ShadeboneInspector] DEFAULT ('') NOT NULL,
    [ContinuityInspector]          VARCHAR (10)    CONSTRAINT [DF_FIR_ContinuityInspector] DEFAULT ('') NOT NULL,
    [OdorInspector]                VARCHAR (10)    CONSTRAINT [DF_FIR_OdorInspector] DEFAULT ('') NOT NULL,
    [Moisture]                     VARCHAR (5)     CONSTRAINT [DF_FIR_Moisture] DEFAULT ('') NOT NULL,
    [NonMoisture]                  BIT             CONSTRAINT [DF_FIR_NonMoisture] DEFAULT ((0)) NOT NULL,
    [MoistureDate]                 DATE            NULL,
    [MaterialCompositionGrp]       VARCHAR (50)    CONSTRAINT [DF_FIR_MaterialCompositionGrp] DEFAULT ('') NOT NULL,
    [MaterialCompositionItem]      VARCHAR (100)   CONSTRAINT [DF_FIR_MaterialCompositionItem] DEFAULT ('') NOT NULL,
    [MoistureStandardDesc]         NVARCHAR (10)   CONSTRAINT [DF_FIR_MoistureStandardDesc] DEFAULT ('') NOT NULL,
    [MoistureStandard1]            NUMERIC (4, 1)  CONSTRAINT [DF_FIR_MoistureStandard1] DEFAULT ((0)) NOT NULL,
    [MoistureStandard1_Comparison] TINYINT         CONSTRAINT [DF_FIR_MoistureStandard1_Comparison] DEFAULT ((0)) NOT NULL,
    [MoistureStandard2]            NUMERIC (4, 1)  CONSTRAINT [DF_FIR_MoistureStandard2] DEFAULT ((0)) NOT NULL,
    [MoistureStandard2_Comparison] TINYINT         CONSTRAINT [DF_FIR_MoistureStandard2_Comparison] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FIR] PRIMARY KEY CLUSTERED ([ID] ASC)
);
























GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Inspection Report', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'POID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Suppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ReceivingID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ReplacementReportID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ArriveQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總檢驗碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'TotalInspYds';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總檢驗點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'TotalDefectPoint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否檢驗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Nonphysical';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布瑕疵點檢驗Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Physical';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布瑕疵點檢驗Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'PhysicalEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布瑕疵點檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'PhysicalDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'nonWeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量檢驗Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量檢驗Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'WeightEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'WeightDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗 Shade Bond', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'nonShadebond';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ShadeBond  Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ShadeBond';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ShadeBond Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ShadebondEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ShadeBond Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ShadeBondDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需檢驗漸進色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'nonContinuity';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'漸進色Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Continuity';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'漸進色 Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ContinuityEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'漸進色日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ContinuityDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗截止日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'InspDeadline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve 人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Approve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve 時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'ApproveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Refno';


GO
CREATE NONCLUSTERED INDEX [PO_Seq]
    ON [dbo].[FIR]([POID] ASC, [SEQ1] ASC, [SEQ2] ASC);


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'����������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'nonOdor';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'OdorEncode';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'OdorDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Odor';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不須檢測濕度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'NonMoisture';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pass 判斷標準 - 此欄位主要是給人看的', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MoistureStandardDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的終值，判斷符號
（0 = 等於 EqualTo、3 = 小於 LessThan、4 = 小於等於 LessThanOrEqualTo）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MoistureStandard2_Comparison';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的終值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MoistureStandard2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的起始值，判斷符號
（0 = 等於 EqualTo、1 = 大於 GreaterThan、2 = 大於等於 GreaterThanOrEqualTo）', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MoistureStandard1_Comparison';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測 Pass 標準範圍的起始值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MoistureStandard1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MoistureDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'Moisture';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該布料的成分說明 - 細項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MaterialCompositionItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該布料的成分說明 - 群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR', @level2type = N'COLUMN', @level2name = N'MaterialCompositionGrp';

