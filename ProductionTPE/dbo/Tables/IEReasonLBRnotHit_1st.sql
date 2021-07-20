CREATE TABLE [dbo].[IEReasonLBRnotHit_1st](
	[Type] [varchar](50) NOT NULL,
	[TypeGroup] [varchar](50) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Remark] [nvarchar](200) NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
 CONSTRAINT [PK_IEReasonLBRnotHit_1st] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[TypeGroup] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO