CREATE TABLE [dbo].[CuttingOutput_Detail_Detail] (
    [ID]                       VARCHAR (13) CONSTRAINT [DF_CuttingOutput_Detail_Detail_ID] DEFAULT ('') NOT NULL,
    [CuttingOutput_detailUkey] BIGINT       CONSTRAINT [DF_CuttingOutput_Detail_Detail_CuttingOutput_detailUkey] DEFAULT ((0)) NOT NULL,
    [CuttingID]                VARCHAR (13) CONSTRAINT [DF_CuttingOutput_Detail_Detail_CuttingID] DEFAULT ('') NOT NULL,
    [SizeCode]                 VARCHAR (8)  CONSTRAINT [DF_CuttingOutput_Detail_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                      NUMERIC (6)  CONSTRAINT [DF_CuttingOutput_Detail_Detail_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CuttingOutput_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [CuttingOutput_detailUkey] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cuttng Dailiy output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表身Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'CuttingOutput_detailUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'CuttingID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單層裁剪比率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'Qty';

