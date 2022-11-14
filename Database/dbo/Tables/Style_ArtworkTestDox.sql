CREATE TABLE [dbo].[Style_ArtworkTestDox] (
    [Ukey]                        BIGINT         NOT NULL,
    [StyleUkey]                   BIGINT         CONSTRAINT [DF_Style_ArtworkTestDox_StyleUkey] DEFAULT ((0)) NOT NULL,
    [ArtworkTypeID]               VARCHAR (20)   CONSTRAINT [DF_Style_ArtworkTestDox_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [ArtworkID]                   VARCHAR (20)   CONSTRAINT [DF_Style_ArtworkTestDox_ArtworkID] DEFAULT ('') NOT NULL,
    [Article]                     VARCHAR (8)    CONSTRAINT [DF_Style_ArtworkTestDox_Article] DEFAULT ('') NOT NULL,
    [F_FabricPanelCode]           VARCHAR (2)    CONSTRAINT [DF_Style_ArtworkTestDox_F_FabricPanelCode] DEFAULT ('') NOT NULL,
    [F_Refno]                     VARCHAR (30)   CONSTRAINT [DF_Style_ArtworkTestDox_F_Refno] DEFAULT ('') NOT NULL,
    [A_FabricPanelCode]           VARCHAR (2)    CONSTRAINT [DF_Style_ArtworkTestDox_A_FabricPanelCode] DEFAULT ('') NOT NULL,
    [A_Refno]                     VARCHAR (30)   CONSTRAINT [DF_Style_ArtworkTestDox_A_Refno] DEFAULT ('') NOT NULL,
    [FabricFaceSide]              VARCHAR (1)    CONSTRAINT [DF_Style_ArtworkTestDox_FabricFaceSide] DEFAULT ('') NOT NULL,
    [PrintType]                   VARCHAR (10)   CONSTRAINT [DF_Style_ArtworkTestDox_PrintType] DEFAULT ('') NOT NULL,
    [TestNo]                      VARCHAR (20)   CONSTRAINT [DF_Style_ArtworkTestDox_TestNo] DEFAULT ('(''0') NOT NULL,
    [TestResult]                  VARCHAR (1)    CONSTRAINT [DF_Style_ArtworkTestDox_TestResult] DEFAULT ('') NOT NULL,
    [Remark]                      NVARCHAR (MAX) CONSTRAINT [DF_Style_ArtworkTestDox_Remark] DEFAULT ('') NOT NULL,
    [AddName]                     VARCHAR (10)   CONSTRAINT [DF_Style_ArtworkTestDox_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                     DATETIME       NULL,
    [EditName]                    VARCHAR (10)   CONSTRAINT [DF_Style_ArtworkTestDox_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                    DATETIME       NULL,
    [SubstrateFormSendDate]       DATE           NULL,
    [FactoryID]                   VARCHAR (8)    CONSTRAINT [DF_Style_ArtworkTestDox_FactoryID] DEFAULT ('') NOT NULL,
    [OrderID]                     VARCHAR (13)   CONSTRAINT [DF_Style_ArtworkTestDox_OrderID] DEFAULT ('') NULL,
    [IsA_FabricPanelCodeCanEmpty] BIT            CONSTRAINT [DF_Style_ArtworkTestDox_IsA_FabricPanelCodeCanEmpty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Style_ArtworkTestDox] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Edit Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Add Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Add Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Test Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'TestResult';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Print Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'PrintType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A Ref No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'A_Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A Fabric Panel Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'A_FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'F Ref No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'F_Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'F Fabric Panel Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'F_FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'ArtworkID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ArtworkTestDox', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';

