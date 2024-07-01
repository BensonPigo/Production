CREATE TABLE [dbo].[CuttingOutput_Detail] (
    [ID]                     VARCHAR (13)   CONSTRAINT [DF_CuttingOutput_Detail_ID] DEFAULT ('') NOT NULL,
    [CutRef]                 VARCHAR (10)   CONSTRAINT [DF_CuttingOutput_Detail_CutRef] DEFAULT ('') NOT NULL,
    [CuttingID]              VARCHAR (13)   CONSTRAINT [DF_CuttingOutput_Detail_CuttingID] DEFAULT ('') NOT NULL,
    [Cutno]                  NUMERIC (6)    CONSTRAINT [DF_CuttingOutput_Detail_Cutno] DEFAULT ('') NOT NULL,
    [MarkerName]             VARCHAR (20)   CONSTRAINT [DF_CuttingOutput_Detail_MarkName] DEFAULT ('') NOT NULL,
    [MarkerLength]           VARCHAR (13)   CONSTRAINT [DF_CuttingOutput_Detail_Marker] DEFAULT ('') NOT NULL,
    [Layer]                  NUMERIC (5)    CONSTRAINT [DF_CuttingOutput_Detail_Layers] DEFAULT ((0)) NOT NULL,
    [Cons]                   NUMERIC (9, 4) CONSTRAINT [DF_CuttingOutput_Detail_Cons] DEFAULT ((0)) NOT NULL,
    [WorkOrderForOutputUkey] BIGINT         CONSTRAINT [DF_CuttingOutput_Detail_WorkOrderForOutputUkey] DEFAULT ((0)) NOT NULL,
    [Colorid]                VARCHAR (6)    CONSTRAINT [DF_CuttingOutput_Detail_Colorid] DEFAULT ('') NOT NULL,
    [Ukey]                   BIGINT         IDENTITY (1, 1) NOT NULL,
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



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Cutno';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Output裁剪工單主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'WorkOrderForOutputUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'MarkerLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Layer';


GO
CREATE NONCLUSTERED INDEX [WorkOrderForOutputUkey]
    ON [dbo].[CuttingOutput_Detail]([WorkOrderForOutputUkey] ASC);


GO
CREATE NONCLUSTERED INDEX [ID]
    ON [dbo].[CuttingOutput_Detail]([ID] ASC)
    INCLUDE([WorkOrderForOutputUkey]);

