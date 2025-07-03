CREATE TABLE [dbo].[P_LineMapping_Detail](
	[ID] [bigint] NOT NULL,
	[Ukey] [bigint] NOT NULL,
	[IsFrom] [varchar](6) NOT NULL,
	[No] [varchar](4) NOT NULL,
	[Seq] [smallint] NOT NULL,
	[Location] [varchar](20) NOT NULL,
	[ST/MC Type] [varchar](10) NOT NULL,
	[MC Group] [varchar](4) NOT NULL,
	[OperationID] [varchar](20) NOT NULL,
	[Operation] [nvarchar](500) NOT NULL,
	[Annotation] [nvarchar](200) NOT NULL,
	[Attachment] [varchar](100) NOT NULL,
	[PartID] [varchar](200) NOT NULL,
	[Template] [varchar](100) NOT NULL,
	[GSD Time] [numeric](6, 2) NOT NULL,
	[Cycle Time] [numeric](6, 2) NOT NULL,
	[%] [numeric](3, 2) NOT NULL,
	[Div. Sewer] [numeric](5, 4) NOT NULL,
	[Ori. Sewer] [numeric](5, 4) NOT NULL,
	[Thread Combination] [varchar](10) NOT NULL,
	[Notice] [nvarchar](200) NOT NULL,
	[OperatorID] [varchar](10) NOT NULL,
	[OperatorName] [nvarchar](50) NOT NULL,
	[Skill] [nvarchar](200) NOT NULL,
 [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_LineMapping_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[IsFrom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [IsFrom]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [No]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ((0)) FOR [Seq]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [ST/MC Type]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [MC Group]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [OperationID]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Operation]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Annotation]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Attachment]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [PartID]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Template]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ((0)) FOR [GSD Time]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ((0)) FOR [Cycle Time]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ((0)) FOR [%]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ((0)) FOR [Div. Sewer]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ((0)) FOR [Ori. Sewer]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Thread Combination]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Notice]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [OperatorID]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [OperatorName]
GO

ALTER TABLE [dbo].[P_LineMapping_Detail] ADD  DEFAULT ('') FOR [Skill]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串表頭ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源為IE P03或IE P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'IsFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站位編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'No'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Location'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫機器代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'ST/MC Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫機器分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'MC Group'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'OperationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Operation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段註解' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Annotation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模具附屬物代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Attachment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'輔具規格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'PartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模具模版代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Template'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'General Sewing Data時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cycle秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車工差異百分比' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'%'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配分車工數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Div. Sewer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始車工數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Ori. Sewer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'線組合代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Thread Combination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注意事項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Notice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工人代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'OperatorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'	工人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'OperatorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熟悉的做工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Skill'
GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'