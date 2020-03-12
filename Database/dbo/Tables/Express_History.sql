CREATE TABLE [dbo].[Express_History](
	[ID] [varchar](13) NOT NULL,
	[OldValue] [varchar](20) NOT NULL,
	[NewValue] [varchar](20) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Express_History] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Express_History] ADD  CONSTRAINT [DF_Express_History_OldValue]  DEFAULT ('') FOR [OldValue]
GO

ALTER TABLE [dbo].[Express_History] ADD  CONSTRAINT [DF_Express_History_NewValue]  DEFAULT ('') FOR [NewValue]
GO

ALTER TABLE [dbo].[Express_History] ADD  CONSTRAINT [DF_Express_History_AddName]  DEFAULT ('') FOR [AddName]
GO
