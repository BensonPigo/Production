CREATE TABLE [dbo].[SewingMachineAttachment](
	[ID] [nvarchar](200) NOT NULL,
	[Picture1] [nvarchar](60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Picture1] DEFAULT '',
	[Picture2] [nvarchar](60) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_Picture2] DEFAULT '',
	[AddName] [varchar](10) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_AddName] DEFAULT '',
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL CONSTRAINT [DF_SewingMachineAttachment_EditName] DEFAULT '',
	[EditDate] [datetime] NULL,
	CONSTRAINT [PK_SewingMachineAttachment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO