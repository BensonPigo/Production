CREATE TABLE [dbo].[P_Factory](
	[FtyCode] [varchar](8) NOT NULL,
	[SdpKpiCode] [varchar](8) NOT NULL,
	[Junk] [bit] NOT NULL,
 CONSTRAINT [PK_P_Factory] PRIMARY KEY CLUSTERED 
(
	[FtyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_Factory] ADD  CONSTRAINT [DF_P_Factory_FtyCode]  DEFAULT ('') FOR [FtyCode]
GO

ALTER TABLE [dbo].[P_Factory] ADD  CONSTRAINT [DF_P_Factory_SdpKpiCode]  DEFAULT ('') FOR [SdpKpiCode]
GO

ALTER TABLE [dbo].[P_Factory] ADD  CONSTRAINT [DF_P_Factory_Junk]  DEFAULT (0) FOR [Junk]
GO