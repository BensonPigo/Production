
CREATE TABLE [dbo].[Style_QTThreadColorCombo](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[StyleUkey] [bigint] NOT NULL,
	[Thread_Quilting_SizeUkey] [bigint] NOT NULL,
	[FabricPanelCode] [varchar](2) NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Style_QTThreadColorCombo] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Style_QTThreadColorCombo] ADD  CONSTRAINT [DF_Style_QTThreadColorCombo_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

