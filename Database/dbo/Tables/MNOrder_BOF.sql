CREATE TABLE [dbo].[MNOrder_BOF] (
    [ID]             VARCHAR (13)   CONSTRAINT [DF_MNOrder_BOF_ID] DEFAULT ('') NOT NULL,
    [FabricCode]     VARCHAR (3)    CONSTRAINT [DF_MNOrder_BOF_FabricCode] DEFAULT ('') NOT NULL,
    [Refno]          VARCHAR (36)   CONSTRAINT [DF_MNOrder_BOF_Refno] DEFAULT ('') NULL,
    [SCIRefno]       VARCHAR (30)   CONSTRAINT [DF_MNOrder_BOF_SCIRefno] DEFAULT ('') NULL,
    [SuppID]         VARCHAR (6)    CONSTRAINT [DF_MNOrder_BOF_SuppID] DEFAULT ('') NULL,
    [Description]    NVARCHAR (MAX) CONSTRAINT [DF_MNOrder_BOF_Description] DEFAULT ('') NULL,
    [FabricUkey_Old] VARCHAR (10)   DEFAULT ('') NULL,
    [FabricVer_OLd ] VARCHAR (2)    DEFAULT ('') NULL,
    CONSTRAINT [PK_MNOrder_BOF] PRIMARY KEY CLUSTERED ([ID] ASC, [FabricCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Bill of Fabric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_BOF', @level2type = N'COLUMN', @level2name = N'Description';

