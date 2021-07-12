CREATE TABLE [dbo].[Bundle_History](
	[ID] [varchar](13) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[MDivisionid] [varchar](8) NOT NULL,
	[Sizecode] [varchar](100) NOT NULL,
	[Colorid] [varchar](6) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[PatternPanel] [varchar](2) NOT NULL,
	[Cutno] [numeric](6, 0) NULL,
	[Cdate] [date] NULL,
	[Orderid] [varchar](13) NOT NULL,
	[Sewinglineid] [varchar](2) NOT NULL,
	[Item] [varchar](20) NULL,
	[SewingCell] [varchar](2) NOT NULL,
	[Ratio] [varchar](100) NULL,
	[Startno] [numeric](5, 0) NULL,
	[Qty] [numeric](2, 0) NULL,
	[PrintDate] [datetime] NULL,
	[AllPart] [numeric](5, 0) NULL,
	[CutRef] [varchar](8) NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[oldid] [varchar](13) NULL,
	[FabricPanelCode] [varchar](2) NULL,
	[IsEXCESS] [bit] NOT NULL,
	[ByToneGenerate] [bit] NOT NULL,
	[SubCutNo] [varchar](2) NOT NULL,
 CONSTRAINT [PK_Bundle_History] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_CuttingID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Factoryid]  DEFAULT ('') FOR [MDivisionid]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Sizecode]  DEFAULT ('') FOR [Sizecode]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Colorid]  DEFAULT ('') FOR [Colorid]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_FabricCombo]  DEFAULT ('') FOR [PatternPanel]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Cutno]  DEFAULT ((0)) FOR [Cutno]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Orderid]  DEFAULT ('') FOR [Orderid]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Sewinglineid]  DEFAULT ('') FOR [Sewinglineid]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Item]  DEFAULT ('') FOR [Item]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_SewingCell]  DEFAULT ('') FOR [SewingCell]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Ratio]  DEFAULT ('') FOR [Ratio]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Startno]  DEFAULT ((0)) FOR [Startno]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_AllPart]  DEFAULT ((0)) FOR [AllPart]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_CutRef]  DEFAULT ('') FOR [CutRef]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  DEFAULT ((0)) FOR [IsEXCESS]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  DEFAULT ((0)) FOR [ByToneGenerate]
GO

ALTER TABLE [dbo].[Bundle_History] ADD  CONSTRAINT [DF_Bundle_History_SubCutNo]  DEFAULT ('') FOR [SubCutNo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'MDivisionid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Sizecode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Colorid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'PatternPanel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Cutno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Cdate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Orderid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Sewinglineid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Item' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Item'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing 組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'SewingCell'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸配比' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Ratio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Begin bundle group' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Startno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle 數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列印日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'PrintDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'All Part 數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'AllPart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪Refno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'CutRef'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History', @level2type=N'COLUMN',@level2name=N'SubCutNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle History' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Bundle_History'
GO