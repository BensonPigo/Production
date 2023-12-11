CREATE TABLE [dbo].[Order_BOA_Expend_Keyword](
	[Id] [varchar](13) NOT NULL,
	[Order_BOA_ExpendUkey] [bigint] NOT NULL,
	[KeywordField] [varchar](30) NOT NULL,
	[KeywordValue] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Order_BOA_Expend_Keyword] PRIMARY KEY CLUSTERED 
(
	[Order_BOA_ExpendUkey] ASC,
	[KeywordField] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order_BOA_Expend_Keyword] ADD  CONSTRAINT [DF_Order_BOA_Expend_Keyword_KeywordValue]  DEFAULT ('') FOR [KeywordValue]
GO