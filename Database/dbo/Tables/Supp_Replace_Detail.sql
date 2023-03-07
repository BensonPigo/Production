CREATE TABLE [dbo].[Supp_Replace_Detail] (
    [SuppGroupFabric] VARCHAR (8)  CONSTRAINT [DF_Supp_Replace_Detail_SuppGroupFabric] DEFAULT ('') NOT NULL,
    [Seq]             VARCHAR (3)  CONSTRAINT [DF_Supp_Replace_Detail_Seq] DEFAULT ('') NOT NULL,
    [Type]            VARCHAR (1)  CONSTRAINT [DF_Supp_Replace_Detail_Type] DEFAULT ('') NULL,
    [FromCountry]     VARCHAR (2)  CONSTRAINT [DF_Supp_Replace_Detail_FromCountry] DEFAULT ('') NULL,
    [ToCountry]       VARCHAR (2)  CONSTRAINT [DF_Supp_Replace_Detail_ToCountry] DEFAULT ('') NULL,
    [SuppID]          VARCHAR (6)  CONSTRAINT [DF_Supp_Replace_Detail_SuppID] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10) CONSTRAINT [DF_Supp_Replace_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME     NULL,
    [EditName]        VARCHAR (10) CONSTRAINT [DF_Supp_Replace_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME     NULL,
    [FactoryKpiCode]  VARCHAR (8)  NULL,
    CONSTRAINT [PK_Supp_Replace_Detail] PRIMARY KEY CLUSTERED ([SuppGroupFabric] ASC, [Seq] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supplier Replace', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'ToCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'From Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'FromCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ECFA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Priority', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supp Group Fabric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail', @level2type = N'COLUMN', @level2name = N'SuppGroupFabric';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supplier Replacement Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp_Replace_Detail';

