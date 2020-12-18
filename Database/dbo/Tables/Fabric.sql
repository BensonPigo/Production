CREATE TABLE [dbo].[Fabric] (
    [SCIRefno]         VARCHAR (30)    CONSTRAINT [DF_Fabric_SCIRefno] DEFAULT ('') NOT NULL,
    [BrandID]          VARCHAR (8)     CONSTRAINT [DF_Fabric_BrandID] DEFAULT ('') NULL,
    [Refno]            VARCHAR (20)    CONSTRAINT [DF_Fabric_Refno] DEFAULT ('') NULL,
    [Junk]             BIT             CONSTRAINT [DF_Fabric_Junk] DEFAULT ((0)) NULL,
    [Type]             VARCHAR (1)     CONSTRAINT [DF_Fabric_Type] DEFAULT ('') NULL,
    [MtlTypeID]        VARCHAR (20)    CONSTRAINT [DF_Fabric_MtlTypeID] DEFAULT ('') NULL,
    [BomTypeCalculate] BIT             CONSTRAINT [DF_Fabric_BomTypeCalculate] DEFAULT ((0)) NULL,
    [Description]      NVARCHAR (150)  CONSTRAINT [DF_Fabric_Description] DEFAULT ('') NULL,
    [DescDetail]       NVARCHAR (MAX)  CONSTRAINT [DF_Fabric_DescDetail] DEFAULT ('') NULL,
    [LossType]         TINYINT         CONSTRAINT [DF_Fabric_LossType] DEFAULT ((0)) NULL,
    [LossPercent]      NUMERIC (4, 1)  CONSTRAINT [DF_Fabric_LossPercent] DEFAULT ((0)) NULL,
    [LossQty]          SMALLINT        CONSTRAINT [DF_Fabric_LossQty] DEFAULT ((0)) NULL,
    [LossStep]         INT             CONSTRAINT [DF_Fabric_LossStep] DEFAULT ((0)) NULL,
    [UsageUnit]        VARCHAR (8)     CONSTRAINT [DF_Fabric_UsageUnit] DEFAULT ('') NULL,
    [Width]            NUMERIC (5, 2)  CONSTRAINT [DF_Fabric_Width] DEFAULT ((0)) NULL,
    [Weight]           NUMERIC (9, 4)  CONSTRAINT [DF_Fabric_Supp_WeightYDS] DEFAULT ((0)) NULL,
    [CBM]              NUMERIC (7, 2)  CONSTRAINT [DF_Fabric_Supp_CBM] DEFAULT ((0)) NULL,
    [CBMWeight]        NUMERIC (10, 4) CONSTRAINT [DF_Fabric_Supp_CBMWeight] DEFAULT ((0)) NULL,
    [NoSizeUnit]       BIT             CONSTRAINT [DF_Fabric_NoSizeUnit] DEFAULT ((0)) NULL,
    [BomTypeSize]      BIT             CONSTRAINT [DF_Fabric_BomTypeSize] DEFAULT ((0)) NULL,
    [BomTypeColor]     BIT             CONSTRAINT [DF_Fabric_BomTypeColor] DEFAULT ((0)) NULL,
    [ConstructionID]   VARCHAR (20)    CONSTRAINT [DF_Fabric_ConstructionID] DEFAULT ('') NULL,
    [MatchFabric]      VARCHAR (1)     CONSTRAINT [DF_Fabric_MatchFabric] DEFAULT ('') NULL,
    [WeaveTypeID]      VARCHAR (20)    CONSTRAINT [DF_Fabric_WeaveTypeID] DEFAULT ('') NULL,
    [AddName]          VARCHAR (10)    CONSTRAINT [DF_Fabric_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME        NULL,
    [EditName]         VARCHAR (10)    CONSTRAINT [DF_Fabric_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME        NULL,
    [NLCode]           VARCHAR (9)     CONSTRAINT [DF_Fabric_NLCode] DEFAULT ('') NULL,
    [HSCode]           VARCHAR (11)    CONSTRAINT [DF_Fabric_HSCode] DEFAULT ('') NULL,
    [CustomsUnit]      VARCHAR (8)     CONSTRAINT [DF_Fabric_CustomsUnit] DEFAULT ('') NULL,
    [PcsWidth]         NUMERIC (7, 4)  CONSTRAINT [DF_Fabric_PcsWidth] DEFAULT ((0)) NULL,
    [PcsLength]        NUMERIC (7, 4)  CONSTRAINT [DF_Fabric_PcsLength] DEFAULT ((0)) NULL,
    [PcsKg]            NUMERIC (5, 4)  CONSTRAINT [DF_Fabric_PcsKg] DEFAULT ((0)) NULL,
    [NoDeclare]        BIT             CONSTRAINT [DF_Fabric_NoDeclare] DEFAULT ((0)) NULL,
    [NLCodeEditName]   VARCHAR (10)    CONSTRAINT [DF_Fabric_NLCodeEditName] DEFAULT ('') NULL,
    [NLCodeEditDate]   DATETIME        NULL,
    [WeightM2]         NUMERIC (5, 1)  CONSTRAINT [DF_Fabric_WeightM2] DEFAULT ((0)) NULL,
    [preshrink]        BIT             DEFAULT ((0)) NOT NULL,
    [RibItem]          BIT             CONSTRAINT [DF_Fabric_RibItem] DEFAULT ((0)) NOT NULL,
    [DWR]              BIT             CONSTRAINT [DF_Fabric_DWR] DEFAULT ((0)) NOT NULL,
    [Clima]            BIT             CONSTRAINT [DF_Fabric_Clima] DEFAULT ((0)) NOT NULL,
    [NLCode2] VARCHAR(9) CONSTRAINT [DF_Fabric_NLCode2] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_Fabric] PRIMARY KEY CLUSTERED ([SCIRefno] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'MtlTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用長度計算(即Size SPEC中keyin計算值)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'BomTypeCalculate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'詳細描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'DescDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'計算損耗方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'LossType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗百分比', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'LossPercent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'LossQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗單位量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'LossStep';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單的使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'UsageUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料：碼重(g)、副料：重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'CBMWeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依尺寸展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'BomTypeSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依顏色展開', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'BomTypeColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組成代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'ConstructionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'MatchFabric';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'織法', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'WeaveTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NL Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'NLCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HS Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'HSCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關使用單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'CustomsUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Width/Pcs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'PcsWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Length/Pcs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'PcsLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KG/Pcs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'PcsKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需做出口報關的物料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'NoDeclare';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改NL Code人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'NLCodeEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改NL Code時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'NLCodeEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平方米重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric', @level2type = N'COLUMN', @level2name = N'WeightM2';

