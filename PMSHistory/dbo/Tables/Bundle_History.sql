CREATE TABLE [dbo].[Bundle_History] (
    [ID]              VARCHAR (13)  CONSTRAINT [DF_Bundle_History_ID] DEFAULT ('') NOT NULL,
    [POID]            VARCHAR (13)  CONSTRAINT [DF_Bundle_History_CuttingID] DEFAULT ('') NOT NULL,
    [MDivisionid]     VARCHAR (8)   CONSTRAINT [DF_Bundle_History_Factoryid] DEFAULT ('') NOT NULL,
    [Sizecode]        VARCHAR (100) CONSTRAINT [DF_Bundle_History_Sizecode] DEFAULT ('') NOT NULL,
    [Colorid]         VARCHAR (6)   CONSTRAINT [DF_Bundle_History_Colorid] DEFAULT ('') NOT NULL,
    [Article]         VARCHAR (8)   CONSTRAINT [DF_Bundle_History_Article] DEFAULT ('') NOT NULL,
    [PatternPanel]    VARCHAR (2)   CONSTRAINT [DF_Bundle_History_FabricCombo] DEFAULT ('') NOT NULL,
    [Cutno]           NUMERIC (6)   CONSTRAINT [DF_Bundle_History_Cutno] DEFAULT ((0)) NULL,
    [Cdate]           DATE          NULL,
    [Orderid]         VARCHAR (13)  CONSTRAINT [DF_Bundle_History_Orderid] DEFAULT ('') NOT NULL,
    [Sewinglineid]    VARCHAR (5)   CONSTRAINT [DF_Bundle_History_Sewinglineid] DEFAULT ('') NOT NULL,
    [Item]            VARCHAR (20)  CONSTRAINT [DF_Bundle_History_Item] DEFAULT ('') NULL,
    [SewingCell]      VARCHAR (2)   CONSTRAINT [DF_Bundle_History_SewingCell] DEFAULT ('') NOT NULL,
    [Ratio]           VARCHAR (100) CONSTRAINT [DF_Bundle_History_Ratio] DEFAULT ('') NULL,
    [Startno]         NUMERIC (5)   CONSTRAINT [DF_Bundle_History_Startno] DEFAULT ((0)) NULL,
    [Qty]             NUMERIC (2)   CONSTRAINT [DF_Bundle_History_Qty] DEFAULT ((0)) NULL,
    [PrintDate]       DATETIME      NULL,
    [AllPart]         NUMERIC (5)   CONSTRAINT [DF_Bundle_History_AllPart] DEFAULT ((0)) NULL,
    [CutRef]          VARCHAR (8)   CONSTRAINT [DF_Bundle_History_CutRef] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_Bundle_History_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_Bundle_History_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME      NULL,
    [oldid]           VARCHAR (13)  NULL,
    [FabricPanelCode] VARCHAR (2)   NULL,
    [IsEXCESS]        BIT           DEFAULT ((0)) NOT NULL,
    [ByToneGenerate]  BIT           DEFAULT ((0)) NOT NULL,
    [SubCutNo]        VARCHAR (2)   CONSTRAINT [DF_Bundle_History_SubCutNo] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Bundle_History] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'子裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'SubCutNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'All Part 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'AllPart';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'All Part 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'PrintDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Begin bundle group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Startno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸配比', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Ratio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing 組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'SewingCell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Item', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Item';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Sewinglineid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Orderid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Cdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Cutno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'Sizecode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'MDivisionid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle History', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_History';

