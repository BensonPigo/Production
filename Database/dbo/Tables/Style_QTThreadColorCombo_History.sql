
CREATE TABLE [dbo].[Style_QTThreadColorCombo_History](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[StyleUkey] [bigint] NOT NULL,
	[Thread_Quilting_SizeUkey] [bigint] NOT NULL,
	[FabricPanelCode] [varchar](2) NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[LockDate] [datetime] NOT NULL,
	[HSize] [numeric](5, 2) NOT NULL,
	[VSize] [numeric](5, 2) NOT NULL,
	[ASize] [numeric](5, 2) NOT NULL,
	[NeedleDistance] [numeric](5, 2) NOT NULL,
	[FabricCode] [varchar](3) NOT NULL,
	[SCIRefno] [varchar](30) NOT NULL,
	[Width] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_Style_QTThreadColorCombo_History] PRIMARY KEY CLUSTERED 
(
	[StyleUkey] ASC,
	[Thread_Quilting_SizeUkey] ASC,
	[FabricPanelCode] ASC,
	[LockDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo_History] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_History_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo_History] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_History_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo_History] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_History_HSize]  DEFAULT ((0)) FOR [HSize]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo_History] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_History_VSize]  DEFAULT ((0)) FOR [VSize]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo_History] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_History_ASize]  DEFAULT ((0)) FOR [ASize]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo_History] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_History_NeedleDistance]  DEFAULT ((0)) FOR [NeedleDistance]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

