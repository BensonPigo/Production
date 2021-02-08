CREATE TABLE [dbo].[HealthLabelSupp_FtyExpiration] (
    [ID]             VARCHAR (10)   CONSTRAINT [DF_HealthLabelSupp_FtyExpiration_ID] DEFAULT ('') NOT NULL,
    [FactoryID]      VARCHAR (8)    CONSTRAINT [DF_HealthLabelSupp_FtyExpiration_FactoryID] DEFAULT ('') NOT NULL,
    [Registry]       VARCHAR (30)   CONSTRAINT [DF_HealthLabelSupp_FtyExpiration_Registry] DEFAULT ('') NOT NULL,
    [Expiration]     DATE           NOT NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_HealthLabelSupp_FtyExpiration_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    [EditName]       VARCHAR (10)   CONSTRAINT [DF_HealthLabelSupp_FtyExpiration_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME       NULL,
    [ApplyExtension] DATE           NULL,
    [Remark]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_HealthLabelSupp_FtyExpiration] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用期限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'Expiration';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registry Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'Registry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'健康標設定(明細檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HealthLabelSupp_FtyExpiration';

