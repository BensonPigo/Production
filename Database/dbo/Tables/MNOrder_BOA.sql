CREATE TABLE [dbo].[MNOrder_BOA] (
    [Id]             VARCHAR (13)   CONSTRAINT [DF_MNOrder_BOA_Id] DEFAULT ('') NULL,
    [UKey]           BIGINT         CONSTRAINT [DF_MNOrder_BOA_UKey] DEFAULT ((0)) NOT NULL,
    [Refno]          VARCHAR (36)   CONSTRAINT [DF_MNOrder_BOA_Refno] DEFAULT ('') NULL,
    [SCIRefno]       VARCHAR (30)   CONSTRAINT [DF_MNOrder_BOA_SCIRefno] DEFAULT ('') NULL,
    [SuppID]         VARCHAR (6)    CONSTRAINT [DF_MNOrder_BOA_SuppID] DEFAULT ('') NULL,
    [Seq]            VARCHAR (3)    CONSTRAINT [DF_MNOrder_BOA_Seq] DEFAULT ('') NULL,
    [UsedQty]        NUMERIC (8, 4) CONSTRAINT [DF_MNOrder_BOA_UsedQty] DEFAULT ((0)) NULL,
    [BomTypeSize]    BIT            CONSTRAINT [DF_MNOrder_BOA_BomTypeSize] DEFAULT ((0)) NULL,
    [BomTypeColor]   BIT            CONSTRAINT [DF_MNOrder_BOA_BomTypeColor] DEFAULT ((0)) NULL,
    [BomTypePono]    BIT            CONSTRAINT [DF_MNOrder_BOA_BomTypePono] DEFAULT ((0)) NULL,
    [PatternPanel]   VARCHAR (2)    CONSTRAINT [DF_MNOrder_BOA_PatternPanel] DEFAULT ('') NULL,
    [SizeItem]       VARCHAR (3)    CONSTRAINT [DF_MNOrder_BOA_SizeItem] DEFAULT ('') NULL,
    [BomTypeZipper]  BIT            CONSTRAINT [DF_MNOrder_BOA_BomTypeZipper] DEFAULT ((0)) NULL,
    [Remark]         NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_BOA_Remark] DEFAULT ('') NULL,
    [Description]    NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_BOA_Description] DEFAULT ('') NULL,
    [FabricVer_Old]  VARCHAR (2)    DEFAULT ('') NULL,
    [FabricUkey_Old] VARCHAR (10)   DEFAULT ('') NULL,
    CONSTRAINT [PK_MNOrder_BOA] PRIMARY KEY CLUSTERED ([UKey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Bill of Accessory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'UKey';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'UsedQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BomTypeSize', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BomTypeColor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeColor';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BomTypePono', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'BomTypePono';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第一參考部位的Colorway-A~Z', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'參考那個量法的Code(S01~S99)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢查拉鏈否', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'BomTypeZipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'Remark';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOA', @level2type = N'COLUMN', @level2name = N'Description';

