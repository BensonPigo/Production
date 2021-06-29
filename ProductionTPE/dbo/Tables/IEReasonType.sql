CREATE TABLE [dbo].[IEReasonType](
	[Type] [varchar](50) NOT NULL,
	[Junk] [bit] NOT NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
 CONSTRAINT [PK_IEReasonType] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[IEReasonType] ADD  CONSTRAINT [DF_IEReasonType_Junk]  DEFAULT ((0)) FOR [Junk]
GO