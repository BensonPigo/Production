CREATE TABLE [dbo].[P_ChangeoverCheckList](
	[FactoryID] [varchar](8) NOT NULL,
	[InlineDate] [datetime] NOT NULL,
	[Ready] [varchar](1) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[OldSP] [varchar](13) NOT NULL,
	[OldStyle] [varchar](15) NOT NULL,
	[OldComboType] [varchar](1) NOT NULL,
	[NewSP] [varchar](13) NOT NULL,
	[NewStyle] [varchar](15) NOT NULL,
	[NewComboType] [varchar](1) NOT NULL,
	[StyleType] [varchar](6) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[FirstSewingOutputDate] [datetime] NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ChangeoverCheckList] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[InlineDate] ASC,
	[Line] ASC,
	[OldSP] ASC,
	[OldStyle] ASC,
	[OldComboType] ASC,
	[NewSP] ASC,
	[NewStyle] ASC,
	[NewComboType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Ready]  DEFAULT ('') FOR [Ready]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_OldSP]  DEFAULT ('') FOR [OldSP]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_OldStyle]  DEFAULT ('') FOR [OldStyle]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_OldComboType]  DEFAULT ('') FOR [OldComboType]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_NewSP]  DEFAULT ('') FOR [NewSP]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_NewStyle]  DEFAULT ('') FOR [NewStyle]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_NewComboType]  DEFAULT ('') FOR [NewComboType]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_StyleType]  DEFAULT ('') FOR [StyleType]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否全部CheckList都已Checked' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'Ready'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'OldSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'OldStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'OldComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'NewSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'NewStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'NewComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'N顯示New，R顯示Repeat' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'StyleType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後第一次產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'FirstSewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO