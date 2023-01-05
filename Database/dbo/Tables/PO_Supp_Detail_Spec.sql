CREATE TABLE [dbo].[PO_Supp_Detail_Spec] (
    [ID]           VARCHAR (13) CONSTRAINT [DF_PO_Supp_Detail_Spec_ID] DEFAULT ('') NOT NULL,
    [Seq1]         VARCHAR (3)  CONSTRAINT [DF_PO_Supp_Detail_Spec_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]         VARCHAR (2)  CONSTRAINT [DF_PO_Supp_Detail_Spec_Seq2] DEFAULT ('') NOT NULL,
    [SpecColumnID] VARCHAR (50) CONSTRAINT [DF_PO_Supp_Detail_Spec_SpecColumnID] DEFAULT ('') NOT NULL,
    [SpecValue]    VARCHAR (50) CONSTRAINT [DF_PO_Supp_Detail_Spec_SpecValue] DEFAULT ('') NOT NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_PO_Supp_Detail_Spec_AddName] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_PO_Supp_Detail_Spec_EditName] DEFAULT ('') NOT NULL,
    [EditDate]     DATETIME     NULL,
    CONSTRAINT [PK_PO_Supp_Detail_Spec] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq1] ASC, [Seq2] ASC, [SpecColumnID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Supp_Detail_Spec', @level2type = N'COLUMN', @level2name = N'ID';

