CREATE TABLE [dbo].[CuttingOutput_Detail] (
    [ID]            VARCHAR (13)   CONSTRAINT [DF_CuttingOutput_Detail_ID] DEFAULT ('') NOT NULL,
    [CutRef]        VARCHAR (6)    CONSTRAINT [DF_CuttingOutput_Detail_CutRef] DEFAULT ('') NOT NULL,
    [CuttingID]     VARCHAR (13)   CONSTRAINT [DF_CuttingOutput_Detail_CuttingID] DEFAULT ('') NOT NULL,
    [FabricCombo]   VARCHAR (1)    CONSTRAINT [DF_CuttingOutput_Detail_FabricCombo] DEFAULT ('') NOT NULL,
    [Cutno]         VARCHAR (3)    CONSTRAINT [DF_CuttingOutput_Detail_Cutno] DEFAULT ('') NOT NULL,
    [MarkName]      VARCHAR (5)    CONSTRAINT [DF_CuttingOutput_Detail_MarkName] DEFAULT ('') NOT NULL,
    [Marker]        VARCHAR (12)   CONSTRAINT [DF_CuttingOutput_Detail_Marker] DEFAULT ('') NOT NULL,
    [Layers]        NUMERIC (5)    CONSTRAINT [DF_CuttingOutput_Detail_Layers] DEFAULT ((0)) NOT NULL,
    [Cons]          NUMERIC (7, 2) CONSTRAINT [DF_CuttingOutput_Detail_Cons] DEFAULT ((0)) NOT NULL,
    [WorkOrderUkey] BIGINT         CONSTRAINT [DF_CuttingOutput_Detail_WorkOrderUkey] DEFAULT ((0)) NOT NULL,
    [Colorid]       VARCHAR (6)    CONSTRAINT [DF_CuttingOutput_Detail_Colorid] DEFAULT ('') NOT NULL,
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_CuttingOutput_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily output(Detail)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Work Order Refno#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'CuttingID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Cutno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'MarkName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Marker';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Layers';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'WorkOrderUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';

