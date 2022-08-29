CREATE TABLE [dbo].[Factory_BrandDefinition] (
    [ID]              VARCHAR (8)  CONSTRAINT [DF_Factory_BrandDefinition_ID] DEFAULT ('') NOT NULL,
    [BrandID]         VARCHAR (8)  CONSTRAINT [DF_Factory_BrandDefinition_BrandID] DEFAULT ('') NOT NULL,
    [CDCodeID]        VARCHAR (6)  CONSTRAINT [DF_Factory_BrandDefinition_CDCodeID] DEFAULT ('') NOT NULL,
    [BrandAreaCode]   VARCHAR (30) CONSTRAINT [DF_Factory_BrandDefinition_BrandAreaCode] DEFAULT ('') NULL,
    [BrandFTYCode]    VARCHAR (30) CONSTRAINT [DF_Factory_BrandDefinition_BrandFTYCode] DEFAULT ('') NULL,
    [BrandVendorCode] VARCHAR (10) CONSTRAINT [DF_Factory_BrandDefinition_BrandVendorCode] DEFAULT ('') NULL,
    [BrandReportCode] VARCHAR (30) CONSTRAINT [DF_Factory_BrandDefinition_BrandReportCode] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10) CONSTRAINT [DF_Factory_BrandDefinition_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME     NULL,
    [EditName]        VARCHAR (10) CONSTRAINT [DF_Factory_BrandDefinition_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME     NULL,
    [V_Code] VARCHAR(30) NULL, 
    CONSTRAINT [PK_Factory_BrandDefinition] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC, [CDCodeID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠的相關客人代碼設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'CDCodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'區域代碼 AGC002', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'BrandAreaCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼 WIP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'BrandFTYCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Vendor Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'BrandVendorCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報表群組代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'BrandReportCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_BrandDefinition', @level2type = N'COLUMN', @level2name = N'EditDate';

