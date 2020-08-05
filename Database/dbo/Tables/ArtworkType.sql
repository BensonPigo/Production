CREATE TABLE [dbo].[ArtworkType] (
    [ID]                     VARCHAR (20)   CONSTRAINT [DF_ArtworkType_ID] DEFAULT ('') NOT NULL,
    [Abbreviation]           VARCHAR (2)    CONSTRAINT [DF_ArtworkType_Abbreviation] DEFAULT ('') NULL,
    [Classify]               VARCHAR (1)    CONSTRAINT [DF_ArtworkType_Classify] DEFAULT ('') NULL,
    [Seq]                    VARCHAR (4)    CONSTRAINT [DF_ArtworkType_Seq] DEFAULT ('') NULL,
    [Junk]                   BIT            CONSTRAINT [DF_ArtworkType_Junk] DEFAULT ((0)) NULL,
    [ArtworkUnit]            VARCHAR (10)   CONSTRAINT [DF_ArtworkType_ArtworkUnit] DEFAULT ('') NULL,
    [ProductionUnit]         VARCHAR (10)   CONSTRAINT [DF_ArtworkType_ProductionUnit] DEFAULT ('') NULL,
    [IsTMS]                  BIT            CONSTRAINT [DF_ArtworkType_IsTMS] DEFAULT ((0)) NULL,
    [IsPrice]                BIT            CONSTRAINT [DF_ArtworkType_IsPrice] DEFAULT ((0)) NULL,
    [IsArtwork]              BIT            CONSTRAINT [DF_ArtworkType_IsArtwork] DEFAULT ((0)) NULL,
    [IsTtlTMS]               BIT            CONSTRAINT [DF_ArtworkType_IsTtlTMS] DEFAULT ((0)) NULL,
    [IsSubprocess]           BIT            CONSTRAINT [DF_ArtworkType_IsSubprocess] DEFAULT ((0)) NULL,
    [Remark]                 NVARCHAR (60)  CONSTRAINT [DF_ArtworkType_Remark] DEFAULT ('') NULL,
    [ReportDropdown]         BIT            CONSTRAINT [DF_ArtworkType_ReportDropdown] DEFAULT ((0)) NULL,
    [UseArtwork]             BIT            CONSTRAINT [DF_ArtworkType_UseArtwork] DEFAULT ((0)) NULL,
    [SystemType]             VARCHAR (1)    CONSTRAINT [DF_ArtworkType_SystemType] DEFAULT ('') NULL,
    [InhouseOSP]             VARCHAR (1)    CONSTRAINT [DF_ArtworkType_InhouseOSP] DEFAULT ('') NULL,
    [AccountNo]              VARCHAR (8)    CONSTRAINT [DF_ArtworkType_AccountNo] DEFAULT ('') NULL,
    [BcsLt]                  NUMERIC (2, 1) CONSTRAINT [DF_ArtworkType_BcsLt] DEFAULT ((0)) NULL,
    [CutLt]                  TINYINT        CONSTRAINT [DF_ArtworkType_CutLt] DEFAULT ((0)) NULL,
    [AddName]                VARCHAR (10)   CONSTRAINT [DF_ArtworkType_AddName] DEFAULT ('') NULL,
    [AddDate]                DATETIME       NULL,
    [EditName]               VARCHAR (10)   CONSTRAINT [DF_ArtworkType_EditName] DEFAULT ('') NULL,
    [EditDate]               DATETIME       NULL,
    [PostSewingDays]         INT            DEFAULT ((0)) NULL,
    [IsPrintToCMP]           BIT            CONSTRAINT [DF_ArtworkType_IsPrintToCMP] DEFAULT ((1)) NULL,
    [IsLocalPurchase]        BIT            CONSTRAINT [DF_ArtworkType_IsLocalPurchase] DEFAULT ((0)) NULL,
    [IsSubprocessInspection] BIT            CONSTRAINT [DF_ArtworkType_IsSubprocessInspection] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ArtworkType] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簡碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'Abbreviation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'Classify';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'ArtworkUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能單位設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'ProductionUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'秒數換算成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'IsTMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直接輸入成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'IsPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否加入Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'IsArtwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否加入ttl TMS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'IsTtlTMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為SubProcess', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'IsSubprocess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報表下拉顯示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'ReportDropdown';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'允許建立在Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'UseArtwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統區分', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'SystemType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'InHouse/OSP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'InhouseOSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會科- 銷貨成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'AccountNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Subprocess BCS Lead Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'BcsLt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標準裁剪Leadtime', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'CutLt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用來判斷此item是否可以在工廠端當地採購', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkType', @level2type = N'COLUMN', @level2name = N'IsLocalPurchase';

