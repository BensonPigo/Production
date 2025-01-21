CREATE TABLE [dbo].[P_ChangeoverCheckList_Detail](
	[FactoryID] [varchar](8) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Category] [varchar](1) NOT NULL,
	[ProductType] [nvarchar](100) NOT NULL,
	[Cell] [varchar](2) NOT NULL,
	[DaysLeft] [varchar](10) NULL,
	[InlineDate] [datetime] NOT NULL,
	[OverDays] [int] NOT NULL,
	[ChgOverCheck] [bit] NOT NULL,
	[CompletionDate] [datetime] NULL,
	[ResponseDep] [nvarchar](200) NOT NULL,
	[CheckListNo] [int] NOT NULL,
	[CheckListItem] [varchar](200) NOT NULL,
	[LateReason] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_P_ChangeoverCheckList_Detail] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[SP] ASC,
	[Style] ASC,
	[Category] ASC,
	[ProductType] ASC,
	[Cell] ASC,
	[InlineDate] ASC,
	[CheckListNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Cell]  DEFAULT ('') FOR [Cell]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_DaysLeft]  DEFAULT ('') FOR [DaysLeft]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_OverDays]  DEFAULT ((0)) FOR [OverDays]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_Table_1_Check]  DEFAULT ((0)) FOR [ChgOverCheck]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_ResponseDep]  DEFAULT ('') FOR [ResponseDep]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_CheckListNo]  DEFAULT ((0)) FOR [CheckListNo]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_CheckListItem]  DEFAULT ('') FOR [CheckListItem]
GO

ALTER TABLE [dbo].[P_ChangeoverCheckList_Detail] ADD  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_LateReason]  DEFAULT ('') FOR [LateReason]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason table 的 ReasonTypeID = Style_Apparel_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'Cell'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'剩餘天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'DaysLeft'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逾期天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'OverDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'完成該項CheckList後勾選' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ChgOverCheck'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按下Check的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'CompletionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'負責部門' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ResponseDep'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CheckList代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'CheckListNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check List 項目名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'CheckListItem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逾期原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'LateReason'
GO