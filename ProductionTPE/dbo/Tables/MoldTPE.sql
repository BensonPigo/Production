CREATE TABLE [dbo].[MoldTPE](
	[ID] [varchar](20) NOT NULL,
	[IsAttachment] [bit] NOT NULL,
	[IsTemplate] [bit] NOT NULL,
 CONSTRAINT [PK_MoldTPE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MoldTPE] ADD  CONSTRAINT [DF_MoldTPE_IsAttachment]  DEFAULT ((0)) FOR [IsAttachment]
GO

ALTER TABLE [dbo].[MoldTPE] ADD  CONSTRAINT [DF_MoldTPE_IsTemplate]  DEFAULT ((0)) FOR [IsTemplate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MoldTPE', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為Attachment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MoldTPE', @level2type=N'COLUMN',@level2name=N'IsAttachment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為Template' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MoldTPE', @level2type=N'COLUMN',@level2name=N'IsTemplate'
GO

