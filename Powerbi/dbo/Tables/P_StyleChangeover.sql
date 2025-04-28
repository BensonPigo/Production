CREATE TABLE [dbo].[P_StyleChangeover](
	[ID] [bigint] NOT NULL,
	[Factory] [varchar](8) NOT NULL,
	[SewingLine] [varchar](5) NOT NULL,
	[Inline] [datetime] NULL,
	[OldSP] [varchar](13) NOT NULL,
	[OldStyle] [varchar](15) NOT NULL,
	[OldComboType] [varchar](1) NOT NULL,
	[NewSP] [varchar](13) NOT NULL,
	[NewStyle] [varchar](15) NOT NULL,
	[NewComboType] [varchar](1) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[COPT(min)] [numeric](8, 2) NOT NULL,
	[COT(min)] [numeric](8, 2) NOT NULL,
	[BIFactoryID] VARCHAR(8) CONSTRAINT [DF_P_StyleChangeover_BIFactoryID] NOT NULL DEFAULT (''), 
	[BIInsertDate] DATETIME NULL, 
 CONSTRAINT [PK_P_StyleChangeover] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_Factory]  DEFAULT ('') FOR [Factory]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_SewingLine]  DEFAULT ('') FOR [SewingLine]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_OldSP]  DEFAULT ('') FOR [OldSP]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_OldStyle]  DEFAULT ('') FOR [OldStyle]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_OldComboType]  DEFAULT ('') FOR [OldComboType]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_NewSP]  DEFAULT ('') FOR [NewSP]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_NewStyle]  DEFAULT ('') FOR [NewStyle]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_NewComboType]  DEFAULT ('') FOR [NewComboType]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_COPT(min)]  DEFAULT ((0)) FOR [COPT(min)]
GO

ALTER TABLE [dbo].[P_StyleChangeover] ADD  CONSTRAINT [DF_P_StyleChangeover_COT(min)]  DEFAULT ((0)) FOR [COT(min)]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ChgOver.ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inline date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'Inline'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一張的SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'OldSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一個款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'OldStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一張SP的Combo Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'OldComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'NewSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'NewStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Combo Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'NewComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款難度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Changeover Process Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'COPT(min)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Changeover Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'COT(min)'
GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StyleChangeover',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_StyleChangeover',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'