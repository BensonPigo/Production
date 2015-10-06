CREATE TABLE [dbo].[MNOrder_FabricCode] (
    [ID]           VARCHAR (13)   CONSTRAINT [DF_MNOrder_FabricCode_ID] DEFAULT ('') NOT NULL,
    [PatternPanel] VARCHAR (2)    CONSTRAINT [DF_MNOrder_FabricCode_PatternPanel] DEFAULT ('') NULL,
    [FabricCode]   VARCHAR (3)    CONSTRAINT [DF_MNOrder_FabricCode_FabricCode] DEFAULT ('') NULL,
    [LectraCode]   VARCHAR (2)    CONSTRAINT [DF_MNOrder_FabricCode_LectraCode] DEFAULT ('') NOT NULL,
    [ForArticle]   NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_FabricCode_ForArticle] DEFAULT ('') NULL,
    CONSTRAINT [PK_MNOrder_FabricCode] PRIMARY KEY CLUSTERED ([ID] ASC, [LectraCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Fabric Code (Bill Of Farbic)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_FabricCode', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_FabricCode', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_FabricCode', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_FabricCode', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'指定特定顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_FabricCode', @level2type = N'COLUMN', @level2name = N'ForArticle';

