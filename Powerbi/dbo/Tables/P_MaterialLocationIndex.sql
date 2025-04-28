CREATE TABLE [dbo].[P_MaterialLocationIndex](
	[ID] [varchar](20) NOT NULL,
	[StockType] [varchar](10) NOT NULL,
	[Junk] [bit] NOT NULL,
	[Description] [nvarchar](40) NOT NULL,
	[IsWMS] [bit] NOT NULL,
	[Capacity] [int] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_MaterialLocationIndex] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[StockType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_StockType]  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_IsWMS]  DEFAULT ((0)) FOR [IsWMS]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_Capacity]  DEFAULT ((0)) FOR [Capacity]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[P_MaterialLocationIndex] ADD  CONSTRAINT [DF_P_MaterialLocationIndex_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否Junk' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為自動倉' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'IsWMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位容量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'Capacity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MaterialLocationIndex',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MaterialLocationIndex',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'