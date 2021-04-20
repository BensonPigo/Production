
CREATE TABLE [dbo].[Thread_Quilting_Size](
	[Shape] [varchar](25) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[HSize] [numeric](5, 2) NOT NULL,
	[VSize] [numeric](5, 2) NOT NULL,
	[ASize] [numeric](5, 2) NOT NULL,
	[NeedleDistance] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_Thread_Quilting_Size] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size] ADD  CONSTRAINT [DF_Thread_Quilting_Size_Shape]  DEFAULT ('') FOR [Shape]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size] ADD  CONSTRAINT [DF_Thread_Quilting_Size_HSize]  DEFAULT ((0)) FOR [HSize]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size] ADD  CONSTRAINT [DF_Thread_Quilting_Size_VSize]  DEFAULT ((0)) FOR [VSize]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size] ADD  CONSTRAINT [DF_Thread_Quilting_Size_ASize]  DEFAULT ((0)) FOR [ASize]
GO

ALTER TABLE [dbo].[Thread_Quilting_Size] ADD  CONSTRAINT [DF_Thread_Quilting_Size_NeedleDistance]  DEFAULT ((0)) FOR [NeedleDistance]
GO
