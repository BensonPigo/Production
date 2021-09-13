CREATE TABLE [dbo].[IEReasonTypeGroup](
	[Type] [varchar](50) NOT NULL,
	[TypeGroup] [varchar](50) NOT NULL,
	[IEReasonApplyFunction] [int] NULL,
	[Junk] [bit] NOT NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
 CONSTRAINT [PK_IEReasonTypeGroup] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[TypeGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[IEReasonTypeGroup] ADD  CONSTRAINT [DF_IEReasonTypeGroup_Junk]  DEFAULT ((0)) FOR [Junk]
GO