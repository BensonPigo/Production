CREATE TABLE [dbo].[Bundle] (
    [ID]              VARCHAR (13)  CONSTRAINT [DF_Bundle_ID] DEFAULT ('') NOT NULL,
    [POID]            VARCHAR (13)  CONSTRAINT [DF_Bundle_CuttingID] DEFAULT ('') NOT NULL,
    [MDivisionid]     VARCHAR (8)   CONSTRAINT [DF_Bundle_Factoryid] DEFAULT ('') NOT NULL,
    [Sizecode]        VARCHAR (100) CONSTRAINT [DF_Bundle_Sizecode] DEFAULT ('') NOT NULL,
    [Colorid]         VARCHAR (6)   CONSTRAINT [DF_Bundle_Colorid] DEFAULT ('') NOT NULL,
    [Article]         VARCHAR (8)   CONSTRAINT [DF_Bundle_Article] DEFAULT ('') NOT NULL,
    [PatternPanel]    VARCHAR (2)   CONSTRAINT [DF_Bundle_FabricCombo] DEFAULT ('') NOT NULL,
    [Cutno]           NUMERIC (6)   CONSTRAINT [DF_Bundle_Cutno] DEFAULT ((0)) NULL,
    [Cdate]           DATE          NULL,
    [Orderid]         VARCHAR (13)  CONSTRAINT [DF_Bundle_Orderid] DEFAULT ('') NOT NULL,
    [Sewinglineid]    VARCHAR (2)   CONSTRAINT [DF_Bundle_Sewinglineid] DEFAULT ('') NOT NULL,
    [Item]            VARCHAR (20)  CONSTRAINT [DF_Bundle_Item] DEFAULT ('') NULL,
    [SewingCell]      VARCHAR (2)   CONSTRAINT [DF_Bundle_SewingCell] DEFAULT ('') NOT NULL,
    [Ratio]           VARCHAR (100) CONSTRAINT [DF_Bundle_Ratio] DEFAULT ('') NULL,
    [Startno]         NUMERIC (5)   CONSTRAINT [DF_Bundle_Startno] DEFAULT ((0)) NULL,
    [Qty]             NUMERIC (2)   CONSTRAINT [DF_Bundle_Qty] DEFAULT ((0)) NULL,
    [PrintDate]       DATETIME      NULL,
    [AllPart]         NUMERIC (5)   CONSTRAINT [DF_Bundle_AllPart] DEFAULT ((0)) NULL,
    [CutRef]          VARCHAR (8)   CONSTRAINT [DF_Bundle_CutRef] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_Bundle_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_Bundle_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME      NULL,
    [oldid]           VARCHAR (13)  NULL,
    [FabricPanelCode] VARCHAR (2)   NULL,
    [IsEXCESS]        BIT           DEFAULT ((0)) NOT NULL,
    [ByToneGenerate]  BIT           DEFAULT ((0)) NOT NULL,
    [SubCutNo]        INT           CONSTRAINT [DF_Bundle_SubCutNo] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bundle] PRIMARY KEY CLUSTERED ([ID] ASC)
);




















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Sizecode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Article';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Cutno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Cdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Orderid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Sewinglineid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Item', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Item';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing 組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'SewingCell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸配比', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Ratio';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Begin bundle group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Startno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'列印日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'PrintDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'All Part 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'AllPart';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'MDivisionid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
CREATE NONCLUSTERED INDEX [POID]
    ON [dbo].[Bundle]([POID] ASC)
    INCLUDE([ID], [Article], [CutRef]);


GO
CREATE NONCLUSTERED INDEX [Orderid]
    ON [dbo].[Bundle]([Orderid] ASC);


GO
CREATE NONCLUSTERED INDEX [ForQtyBySetPerSubprocess]
    ON [dbo].[Bundle]([Orderid] ASC, [PatternPanel] ASC, [FabricPanelCode] ASC, [Article] ASC, [Sizecode] ASC);


GO
CREATE NONCLUSTERED INDEX [cuttingMB]
    ON [dbo].[Bundle]([MDivisionid] ASC)
    INCLUDE([ID], [Orderid], [CutRef]);


GO
CREATE NONCLUSTERED INDEX [Testindex]
    ON [dbo].[Bundle]([MDivisionid] ASC, [CutRef] ASC)
    INCLUDE([ID], [POID], [Colorid], [Article], [PatternPanel], [Cutno], [Orderid], [Sewinglineid], [Item], [SewingCell], [AddDate], [FabricPanelCode]);


GO
CREATE NONCLUSTERED INDEX [CutRef]
    ON [dbo].[Bundle]([CutRef] ASC, [MDivisionid] ASC);


GO
CREATE NONCLUSTERED INDEX [BundleCdate]
    ON [dbo].[Bundle]([Cdate] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'子裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle', @level2type = N'COLUMN', @level2name = N'SubCutNo';

