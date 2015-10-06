CREATE TABLE [dbo].[Fabric_Supp] (
    [SCIRefno]  VARCHAR (26)   CONSTRAINT [DF_Fabric_Supp_SCIRefno] DEFAULT ('') NOT NULL,
    [SuppID]    VARCHAR (6)    CONSTRAINT [DF_Fabric_Supp_SuppID] DEFAULT ('') NOT NULL,
    [Remark]    NVARCHAR (MAX) CONSTRAINT [DF_Fabric_Supp_Remark] DEFAULT ('') NULL,
    [POUnit]    VARCHAR (8)    CONSTRAINT [DF_Fabric_Supp_POUnit] DEFAULT ('') NOT NULL,
    [Delay]     DATE           NULL,
    [DelayMemo] NVARCHAR (MAX) CONSTRAINT [DF_Fabric_Supp_DelayMemo] DEFAULT ('') NULL,
    [WeightYDS] NUMERIC (9, 4) CONSTRAINT [DF_Fabric_Supp_WeightYDS] DEFAULT ((0)) NULL,
    [WeightM2]  NUMERIC (4, 1) CONSTRAINT [DF_Fabric_Supp_WeightM2] DEFAULT ((0)) NULL,
    [CBM]       NUMERIC (6, 2) CONSTRAINT [DF_Fabric_Supp_CBM] DEFAULT ((0)) NULL,
    [CBMWeight] NUMERIC (9, 4) CONSTRAINT [DF_Fabric_Supp_CBMWeight] DEFAULT ((0)) NULL,
    [AbbCH]     NVARCHAR (70)  CONSTRAINT [DF_Fabric_Supp_AbbCH] DEFAULT ('') NULL,
    [AbbEN]     NVARCHAR (70)  CONSTRAINT [DF_Fabric_Supp_AbbEN] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Fabric_Supp_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Fabric_Supp_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    [SuppRefno] VARCHAR (30)   CONSTRAINT [DF_Fabric_Supp_SuppRefno] DEFAULT ('') NULL,
    CONSTRAINT [PK_Fabric_Supp] PRIMARY KEY CLUSTERED ([SCIRefno] ASC, [SuppID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主副料基本檔 - By Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'POUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料延誤', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'Delay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料延誤說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'DelayMemo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料：碼重(g)、副料：重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'WeightYDS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平方米重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'WeightM2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'CBMWeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文名稱簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'AbbCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文名稱簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'AbbEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_Supp', @level2type = N'COLUMN', @level2name = N'SuppRefno';

