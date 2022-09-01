
CREATE TABLE [dbo].[JobCrashLog](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[JobName] [varchar](30) NOT NULL,
	[TransTime] [datetime] NULL,
	[OrderID] [varchar](13) NULL,
	[ErrorMsg] [nvarchar](max) NULL,
	[Success] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
 CONSTRAINT [PK_JobCrashLog] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[JobCrashLog] ADD  CONSTRAINT [DF_JobCrashLog_Success]  DEFAULT ((0)) FOR [Success]
GO

ALTER TABLE [dbo].[JobCrashLog] ADD  CONSTRAINT [DF_JobCrashLog_AddDate]  DEFAULT (getdate()) FOR [AddDate]
GO