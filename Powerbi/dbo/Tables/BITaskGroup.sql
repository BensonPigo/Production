CREATE TABLE [dbo].[BITaskGroup](
	[Name] [varchar](50) NOT NULL,
	[GroupID] [int] NOT NULL,
	[SEQ] [int] NOT NULL,
 CONSTRAINT [PK_BITaskGroup] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BITaskGroup] ADD  CONSTRAINT [DF_BITaskGroup_GroupID]  DEFAULT ((0)) FOR [GroupID]
GO

ALTER TABLE [dbo].[BITaskGroup] ADD  CONSTRAINT [DF_BITaskGroup_SEQ]  DEFAULT ((1)) FOR [SEQ]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BI Table Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskGroup', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskGroup', @level2type=N'COLUMN',@level2name=N'GroupID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskGroup', @level2type=N'COLUMN',@level2name=N'SEQ'
GO