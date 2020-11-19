CREATE TABLE [dbo].[Style] (
    [ID]                  VARCHAR (15)   CONSTRAINT [DF_Style_ID] DEFAULT ('') NOT NULL,
    [Ukey]                BIGINT         CONSTRAINT [DF_Style_Ukey] DEFAULT ((0)) NOT NULL,
    [BrandID]             VARCHAR (8)    CONSTRAINT [DF_Style_BrandID] DEFAULT ('') NOT NULL,
    [ProgramID]           VARCHAR (12)   CONSTRAINT [DF_Style_ProgramID] DEFAULT ('') NOT NULL,
    [SeasonID]            VARCHAR (10)   CONSTRAINT [DF_Style_SeasonID] DEFAULT ('') NOT NULL,
    [Model]               VARCHAR (5)    CONSTRAINT [DF_Style_Model] DEFAULT ('') NOT NULL,
    [Description]         NVARCHAR (50)  CONSTRAINT [DF_Style_Description] DEFAULT ('') NOT NULL,
    [StyleName]           NVARCHAR (50)  CONSTRAINT [DF_Style_StyleName] DEFAULT ('') NOT NULL,
    [ComboType]           VARCHAR (4)    CONSTRAINT [DF_Style_ComboType] DEFAULT ('') NULL,
    [CdCodeID]            VARCHAR (6)    CONSTRAINT [DF_Style_CdCodeID] DEFAULT ('') NOT NULL,
    [ApparelType]         VARCHAR (5)    CONSTRAINT [DF_Style_ApparelType] DEFAULT ('') NOT NULL,
    [FabricType]          VARCHAR (5)    CONSTRAINT [DF_Style_FabricType] DEFAULT ('') NOT NULL,
    [Contents]            NVARCHAR (MAX) CONSTRAINT [DF_Style_Content] DEFAULT ('') NOT NULL,
    [GMTLT]               SMALLINT       CONSTRAINT [DF_Style_GMTLT] DEFAULT ((0)) NOT NULL,
    [CPU]                 NUMERIC (5, 3) CONSTRAINT [DF_Style_CPU] DEFAULT ((0)) NULL,
    [Factories]           NVARCHAR (180) CONSTRAINT [DF_Style_Factories] DEFAULT ('') NULL,
    [FTYRemark]           NVARCHAR (MAX) CONSTRAINT [DF_Style_FTYRemark] DEFAULT ('') NULL,
    [SampleSMR]           VARCHAR (10)   CONSTRAINT [DF_Style_SampleSMR] DEFAULT ('') NULL,
    [SampleMRHandle]      VARCHAR (10)   CONSTRAINT [DF_Style_SampleMRHandle] DEFAULT ('') NULL,
    [BulkSMR]             VARCHAR (10)   CONSTRAINT [DF_Style_BulkSMR] DEFAULT ('') NULL,
    [BulkMRHandle]        VARCHAR (10)   CONSTRAINT [DF_Style_BulkMRHandle] DEFAULT ('') NULL,
    [Junk]                BIT            CONSTRAINT [DF_Style_Junk] DEFAULT ((0)) NULL,
    [RainwearTestPassed]  BIT            CONSTRAINT [DF_Style_PassedRainwearTest] DEFAULT ((0)) NULL,
    [SizePage]            VARCHAR (2)    CONSTRAINT [DF_Style_SizePage] DEFAULT ('') NULL,
    [SizeRange]           NVARCHAR (MAX) CONSTRAINT [DF_Style_SizeRange] DEFAULT ('') NULL,
    [CTNQty]              SMALLINT       CONSTRAINT [DF_Style_CTNQty] DEFAULT ((0)) NULL,
    [StdCost]             NUMERIC (7, 2) CONSTRAINT [DF_Style_StdCost] DEFAULT ((0)) NULL,
    [Processes]           NVARCHAR (60)  CONSTRAINT [DF_Style_Processes] DEFAULT ('') NULL,
    [ArtworkCost]         VARCHAR (1)    CONSTRAINT [DF_Style_ArtworkCost] DEFAULT ('') NOT NULL,
    [Picture1]            NVARCHAR (60)  CONSTRAINT [DF_Style_Picture1] DEFAULT ('') NULL,
    [Picture2]            NVARCHAR (60)  CONSTRAINT [DF_Style_Picture2] DEFAULT ('') NULL,
    [Label]               NVARCHAR (MAX) CONSTRAINT [DF_Style_Label] DEFAULT ('') NULL,
    [Packing]             NVARCHAR (MAX) CONSTRAINT [DF_Style_Packing] DEFAULT ('') NULL,
    [IETMSID]             VARCHAR (10)   CONSTRAINT [DF_Style_IETMSID] DEFAULT ('') NULL,
    [IETMSVersion]        VARCHAR (3)    CONSTRAINT [DF_Style_IETMSVersion] DEFAULT ('') NULL,
    [IEImportName]        VARCHAR (10)   CONSTRAINT [DF_Style_IEImportName] DEFAULT ('') NULL,
    [IEImportDate]        DATETIME       NULL,
    [ApvDate]             DATETIME       NULL,
    [ApvName]             VARCHAR (10)   CONSTRAINT [DF_Style_ApvName] DEFAULT ('') NULL,
    [CareCode]            VARCHAR (8)    CONSTRAINT [DF_Style_CareCode] DEFAULT ('') NULL,
    [SpecialMark]         VARCHAR (5)    CONSTRAINT [DF_Style_SpecialMark] DEFAULT ('') NULL,
    [Lining]              VARCHAR (15)   CONSTRAINT [DF_Style_Lining] DEFAULT ('') NULL,
    [StyleUnit]           VARCHAR (8)    CONSTRAINT [DF_Style_StyleUnit] DEFAULT ('') NULL,
    [ExpectionForm]       BIT            CONSTRAINT [DF_Style_ExpectionForm] DEFAULT ((0)) NULL,
    [ExpectionFormRemark] NVARCHAR (MAX) CONSTRAINT [DF_Style_ExpectionFormRemark] DEFAULT ('') NULL,
    [LocalMR]             VARCHAR (10)   CONSTRAINT [DF_Style_LocalMR] DEFAULT ('') NULL,
    [LocalStyle]          BIT            CONSTRAINT [DF_Style_LocalStyle] DEFAULT ((0)) NULL,
    [PPMeeting]           DATE           NULL,
    [NoNeedPPMeeting]     BIT            CONSTRAINT [DF_Style_NoNeedPPMeeting] DEFAULT ((0)) NULL,
    [SampleApv]           DATETIME       NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Style_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Style_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME       NULL,
    [SizeUnit]            VARCHAR (8)    CONSTRAINT [DF_Style_SizeUnit] DEFAULT ('') NULL,
    [ModularParent]       VARCHAR (20)   CONSTRAINT [DF_Style_ModularParent] DEFAULT ('') NULL,
    [CPUAdjusted]         NUMERIC (6, 4) CONSTRAINT [DF_Style_CPUAdjusted] DEFAULT ((0)) NULL,
    [Phase]               VARCHAR (10)   CONSTRAINT [DF_Style_Phase1] DEFAULT ('') NULL,
    [Gender]              VARCHAR (10)   CONSTRAINT [DF_Style_Gender1] DEFAULT ('') NULL,
    [ThreadEditname]      VARCHAR (10)   NULL,
    [ThreadEditdate]      DATETIME       NULL,
    [ThickFabric] BIT CONSTRAINT [DF_Style_ThickFabric] DEFAULT (0) NOT NULL, 
    [DyeingID] VARCHAR(5) NULL, 
    [Pressing1] INT NULL DEFAULT (1), 
    [Pressing2] INT NULL DEFAULT (0), 
    [Folding1] INT NULL DEFAULT (0), 
    [Folding2] INT NULL DEFAULT (0), 
	ExpectionFormStatus Varchar(1) NOT NULL CONSTRAINT [DF_Style_ExpectionFormStatus] DEFAULT(''),
	ExpectionFormDate Date NULL,
    [ThickFabricBulk] BIT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Style] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC, [SeasonID] ASC)
);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式資料基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系列代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ProgramID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Model';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'StyleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣套裝組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'CdCodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ApparelType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣成份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = 'Contents';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣LEADTIME', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'GMTLT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'CPU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Factories';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Planning 放置工廠備註用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'FTYRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷樣階段的訂單主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SampleSMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample階段的訂單Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SampleMRHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨階段訂單Handle主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'BulkSMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨階段的訂單Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'BulkMRHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作廢', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗測式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'RainwearTestPassed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SizePage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺碼範圍', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SizeRange';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購標準成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'StdCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Processes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工成本建立方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ArtworkCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Picture1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Picture2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標籤 & 保養說明標籤', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Label';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Packing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'IETMSID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time Study 的版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'IETMSVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE 更新者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'IEImportName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE 更新日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'IEImportDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式核准日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式核准人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'洗水標籤/洗水嘜', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'CareCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特別註記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SpecialMark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'襯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'Lining';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'StyleUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Expection Form', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ExpectionForm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Exception Form Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ExpectionFormRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠當地業務', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'LocalMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠自行建立之款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'LocalStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory PP Meeting', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'PPMeeting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不需要PP Meeting', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'NoNeedPPMeeting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample approval', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'SampleApv';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'EditDate';


GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'性別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'Gender'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'目前階段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'Phase'
GO
CREATE NONCLUSTERED INDEX [StyleUkey]
    ON [dbo].[Style]([Ukey] ASC);

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ThreadP01use', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ThreadEditname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ThreadP01use', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style', @level2type = N'COLUMN', @level2name = N'ThreadEditdate';


GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'整燙設定1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'Pressing1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'整燙設定2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'Pressing2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'折衣設定1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'Folding1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'折衣設定2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'Folding2'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approce/Reject', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Style'
, @level2type = N'COLUMN', @level2name = N'ExpectionFormStatus'
GO
	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紀錄日期', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Style'
, @level2type = N'COLUMN', @level2name = N'ExpectionFormDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大貨與報價拆分判斷邏輯',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style',
    @level2type = N'COLUMN',
    @level2name = N'ThickFabricBulk'