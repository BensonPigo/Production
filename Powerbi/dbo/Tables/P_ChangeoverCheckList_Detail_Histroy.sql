CREATE TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[ComboType] [varchar](1) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[ProductType] [nvarchar](100) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[InlineDate] [datetime] NOT NULL,
	[CheckListNo] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ChangeoverCheckList_Detail_Histroy] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_CheckListNo]  DEFAULT ((0)) FOR [CheckListNo]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason table 的 ReasonTypeID = Style_Apparel_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CheckList代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'CheckListNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO