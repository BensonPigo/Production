CREATE TABLE [dbo].[LocalSupp_Bank_Detail](
	[ID] [varchar](8) NOT NULL,
	[Pkey] [bigint] NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountNo] [varchar](30) NOT NULL,
	[AccountName] [nvarchar](60) NOT NULL,
	[BankName] [nvarchar](70) NOT NULL,
	[BranchCode] [varchar](30) NOT NULL,
	[BranchName] [nvarchar](60) NOT NULL,
	[CountryID] [varchar](2) NOT NULL,
	[City] [nvarchar](20) NULL,
	[SWIFTCode] [varchar](11) NULL,
	[MidSWIFTCode] [varchar](11) NULL,
	[MidBankName] [nvarchar](70) NULL,
	[Remark] [nvarchar](max) NULL,
	[IsDefault] [bit] NULL,
 CONSTRAINT [PK_LocalSupp_Bank_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Pkey] ASC,
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [AccountNo]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [AccountName]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [BankName]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [BranchCode]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [BranchName]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [CountryID]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [SWIFTCode]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [MidSWIFTCode]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [MidBankName]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[LocalSupp_Bank_Detail] ADD  DEFAULT ((0)) FOR [IsDefault]
GO


