CREATE TABLE [dbo].[PO_Supp] (
    [ID]          VARCHAR (13)   CONSTRAINT [DF_PO_Supp_ID] DEFAULT ('') NOT NULL,
    [SEQ1]        VARCHAR (3)    CONSTRAINT [DF_PO_Supp_SEQ1] DEFAULT ('') NOT NULL,
    [SuppID]      VARCHAR (6)    CONSTRAINT [DF_PO_Supp_SuppID] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (MAX) CONSTRAINT [DF_PO_Supp_Remark] DEFAULT ('') NULL,
    [Description] NVARCHAR (MAX) CONSTRAINT [DF_PO_Supp_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_PO_Supp_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_PO_Supp_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_PO_Supp] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單-廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'SuppID';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'Description';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp', @level2type = N'COLUMN', @level2name = N'EditDate';

