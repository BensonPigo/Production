CREATE TABLE [dbo].[FIR_PointRateFormula](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[SuppID] [varchar](8) NOT NULL,
	[Formula] [nvarchar](100) NOT NULL,
	[WeaveTypeID] [varchar](20) NOT NULL,
	[Junk] [bit] NOT NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_FIR_PointRateFormula] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_SuppID]  DEFAULT ('') FOR [SuppID]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_Formula]  DEFAULT ('') FOR [Formula]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_WeaveTypeID]  DEFAULT ('') FOR [WeaveTypeID]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[FIR_PointRateFormula] ADD  CONSTRAINT [DF_FIR_PointRateFormula_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布公式
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'Formula'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織物類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為Junk' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FIR_PointRateFormula', @level2type=N'COLUMN',@level2name=N'EditName'
GO


