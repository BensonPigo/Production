CREATE TABLE [dbo].[ShippingMarkPic](
	[PackingListID] [varchar](13) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_ShippingMarkPic] PRIMARY KEY CLUSTERED 
(
	[PackingListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShippingMarkPic] ADD  CONSTRAINT [DF_ShippingMarkPic_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[ShippingMarkPic] ADD  CONSTRAINT [DF_ShippingMarkPic_EditName]  DEFAULT ('') FOR [EditName]
GO