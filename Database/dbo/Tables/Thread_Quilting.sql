
CREATE TABLE [dbo].[Thread_Quilting](
	[Shape] [varchar](25) NOT NULL,
	[Picture1] [nvarchar](60) NOT NULL,
	[Picture2] [nvarchar](60) NOT NULL,
	[Junk] [bit] NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Thread_Quilting] PRIMARY KEY CLUSTERED 
(
	[Shape] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Thread_Quilting] ADD  CONSTRAINT [DF_Thread_Quilting_Picture1]  DEFAULT ('') FOR [Picture1]
GO

ALTER TABLE [dbo].[Thread_Quilting] ADD  CONSTRAINT [DF_Thread_Quilting_Picture2]  DEFAULT ('') FOR [Picture2]
GO

ALTER TABLE [dbo].[Thread_Quilting] ADD  CONSTRAINT [DF_Thread_Quilting_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[Thread_Quilting] ADD  CONSTRAINT [DF_Thread_Quilting_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Thread_Quilting] ADD  CONSTRAINT [DF_Thread_Quilting_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

