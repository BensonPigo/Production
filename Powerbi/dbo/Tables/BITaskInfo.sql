CREATE TABLE [dbo].[BITaskInfo](
	[Name] [varchar](50) NOT NULL,
	[ProcedureName] [varchar](200) NOT NULL,
	[DBName] [varchar](10) NOT NULL,
	[HasStartDate] [bit] NOT NULL,
	[HasEndDate] [bit] NOT NULL,
	[HasStartDate2] [bit] NOT NULL,
	[HasEndDate2] [bit] NOT NULL,
	[StartDateDefault] [nvarchar](500) NOT NULL,
	[EndDateDefault] [nvarchar](500) NOT NULL,
	[StartDateDefault2] [nvarchar](500) NOT NULL,
	[EndDateDefault2] [nvarchar](500) NOT NULL,
	[RunOnSunday] [bit] NOT NULL,
	[Source] [varchar](50) NOT NULL,
	[Junk] [bit] NOT NULL,
 CONSTRAINT [PK_BITaskInfo] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_ProcedureName]  DEFAULT ('') FOR [ProcedureName]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_DBName]  DEFAULT ('') FOR [DBName]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_HasStartDate]  DEFAULT ((0)) FOR [HasStartDate]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_HasEndDate]  DEFAULT ((0)) FOR [HasEndDate]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_HasStartDate2]  DEFAULT ((0)) FOR [HasStartDate2]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_HasEndDate2]  DEFAULT ((0)) FOR [HasEndDate2]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_StartDateDefault]  DEFAULT ('') FOR [StartDateDefault]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_EndDateDefault]  DEFAULT ('') FOR [EndDateDefault]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_StartDateDefault2]  DEFAULT ('') FOR [StartDateDefault2]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_EndDateDefault2]  DEFAULT ('') FOR [EndDateDefault2]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_RunOnSunday]  DEFAULT ((0)) FOR [RunOnSunday]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_Source]  DEFAULT ('') FOR [Source]
GO

ALTER TABLE [dbo].[BITaskInfo] ADD  CONSTRAINT [DF_BITaskInfo_Junk]  DEFAULT ((0)) FOR [Junk]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BI Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ProcedureName Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'ProcedureName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DB Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'DBName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 StartDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasStartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 EndDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasEndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 第二 StartDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasStartDate2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 第二 EndDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasEndDate2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StartDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'StartDateDefault'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EndDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'EndDateDefault'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第二 StartDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'StartDateDefault2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第二 EndDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'EndDateDefault2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'只在周日執行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'RunOnSunday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'Source'
GO