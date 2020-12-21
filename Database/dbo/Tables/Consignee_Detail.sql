CREATE TABLE [dbo].[Consignee_Detail](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[ID] [varchar](8) NOT NULL,
	[Email] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Consignee_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Consignee_Detail] ADD  CONSTRAINT [DF_Consignee_Detail_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[Consignee_Detail] ADD  CONSTRAINT [DF_Consignee_Detail_Email]  DEFAULT ('') FOR [Email]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consignee.ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Consignee_Detail', @level2type=N'COLUMN',@level2name=N'Email'
GO