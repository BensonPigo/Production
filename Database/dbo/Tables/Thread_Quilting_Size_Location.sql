
CREATE TABLE [dbo].[Thread_Quilting_Size_Location](
	[Shape] [varchar](25) NOT NULL,
	[Thread_Quilting_SizeUkey] [bigint] NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[Location] [varchar](20) NOT NULL,
	[Ratio] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_Thread_Quilting_Size_Location] PRIMARY KEY CLUSTERED 
(
	[Thread_Quilting_SizeUkey] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size_Location] ADD  CONSTRAINT [DF_Thread_Quilting_Size_Location_Shape]  DEFAULT ('') FOR [Shape]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size_Location] ADD  CONSTRAINT [DF_Thread_Quilting_Size_Location_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size_Location] ADD  CONSTRAINT [DF_Thread_Quilting_Size_Location_Location]  DEFAULT ('') FOR [Location]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size_Location] ADD  CONSTRAINT [DF_Thread_Quilting_Size_Location_Ratio]  DEFAULT ((0)) FOR [Ratio]
GO


