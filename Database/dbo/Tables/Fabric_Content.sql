CREATE TABLE [dbo].[Fabric_Content] (
    [SCIRefno]        VARCHAR (26)   CONSTRAINT [DF_Fabric_Content_SCIRefno] DEFAULT ('') NOT NULL,
    [Ukey]            BIGINT         CONSTRAINT [DF_Fabric_Content_Ukey] DEFAULT ((0)) NOT NULL,
    [Layerno]         NUMERIC (2)    CONSTRAINT [DF_Fabric_Content_Layerno] DEFAULT ((0)) NOT NULL,
    [percentage]      NUMERIC (5, 2) CONSTRAINT [DF_Fabric_Content_percentage] DEFAULT ((0)) NOT NULL,
    [MtltypeId]       VARCHAR (20)   CONSTRAINT [DF_Fabric_Content_MtltypeId] DEFAULT ('') NOT NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_Fabric_Content_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_Fabric_Content_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    [OldSys_GroupKey] VARCHAR (10)   CONSTRAINT [DF_Fabric_Content_OldSys_GroupKey] DEFAULT ('') NULL,
    CONSTRAINT [PK_Fabric_Content] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Content', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'Layerno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'百分比', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'percentage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'MtltypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Content', @level2type = N'COLUMN', @level2name = N'OldSys_GroupKey';

