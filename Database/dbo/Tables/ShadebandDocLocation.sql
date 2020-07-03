CREATE TABLE [dbo].[ShadebandDocLocation](
	[ID] [varchar](10) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Junk] [bit] NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[Editname] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_ShadebandDocLocation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShadebandDocLocation] ADD  CONSTRAINT [DF_ShadebandDocLocation_Junk]  DEFAULT ((0)) FOR [Junk]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色差檢驗文件存放的位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShadebandDocLocation', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'位置說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShadebandDocLocation', @level2type=N'COLUMN',@level2name=N'Description'
GO